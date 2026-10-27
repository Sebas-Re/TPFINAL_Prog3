using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace Dao
{
    public class AccesoDatos
    {
        const String NombreArchivo = "PCbooster.db";
        static readonly object bloqueoInicializacion = new object();
        static String rutaInicializada;

        public const String VariableEntorno = "PCBOOSTER_DB";

        /* Ruta del archivo SQLite. Los tests la redirigen. */
        public static String RutaBaseDatos { get; set; } = RutaPorDefecto();

        /* La variable de entorno PCBOOSTER_DB, si está definida (la usan los tests
           E2E para levantar el sitio con una base temporal). Si no, App_Data
           (ASP.NET define "DataDirectory") o la carpeta del ejecutable. */
        public static String RutaPorDefecto()
        {
            String desdeEntorno = Environment.GetEnvironmentVariable(VariableEntorno);
            if (!String.IsNullOrWhiteSpace(desdeEntorno))
            {
                return desdeEntorno;
            }
            return Path.Combine(
                AppDomain.CurrentDomain.GetData("DataDirectory") as String ?? AppDomain.CurrentDomain.BaseDirectory,
                NombreArchivo);
        }

        public AccesoDatos()
        {
        }

        private SQLiteConnection ObtenerConexion()
        {
            InicializarBaseDatos();
            SQLiteConnection cn = new SQLiteConnection("Data Source=" + RutaBaseDatos + ";Foreign Keys=True");
            cn.Open();
            return cn;
        }

        /* Versión del schema, guardada en PRAGMA user_version. Una base nueva nace
           en esta versión; una existente se actualiza con los scripts de Migraciones. */
        public const int VersionSchema = 4;

        /* Scripts que llevan una base de la versión N-1 a la N (índice 0 = versión 1). */
        static readonly String[][] Migraciones =
        {
            new[] { "migracion_1.sql" },
            new[] { "migracion_2.sql", "articulos_detalle.sql" },
            new[] { "migracion_3.sql" },
            new String[0] // versión 4: hash de contraseñas (HashearContraseñas, no se puede hacer en SQL)
        };

        /* Al crear una base nueva, cargar también el historial de demo (Sql/demo.sql:
           clientes y ventas de los últimos 6 meses). Los tests de las capas lo
           apagan para trabajar con datos chicos y predecibles. */
        public static bool CargarDatosDemo { get; set; } = true;

        /* La primera vez que se usa la base, si el archivo no existe se crea con
           el schema y los datos de prueba embebidos en el assembly; si existe, se
           le aplican las migraciones pendientes. */
        private static void InicializarBaseDatos()
        {
            String ruta = RutaBaseDatos;
            if (ruta == rutaInicializada) return;
            lock (bloqueoInicializacion)
            {
                if (ruta == rutaInicializada) return;
                if (!File.Exists(ruta))
                {
                    CrearBaseDatos(ruta);
                }
                else
                {
                    Migrar(ruta);
                }
                rutaInicializada = ruta;
            }
        }

        private static void Migrar(String ruta)
        {
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + ruta + ";Foreign Keys=True"))
            {
                cn.Open();
                int version = Convert.ToInt32(EjecutarEscalar(cn, "pragma user_version"));
                for (int siguiente = version + 1; siguiente <= VersionSchema; siguiente++)
                {
                    using (SQLiteTransaction tx = cn.BeginTransaction())
                    {
                        foreach (String script in Migraciones[siguiente - 1])
                        {
                            EjecutarScript(cn, script);
                        }
                        if (siguiente == 4)
                        {
                            HashearContraseñas(cn);
                        }
                        EjecutarEscalar(cn, "pragma user_version = " + siguiente);
                        tx.Commit();
                    }
                }
            }
        }

        private static void CrearBaseDatos(String ruta)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ruta));
            /* Se arma en un archivo temporal y se mueve al final, para no dejar
               una base a medio crear si algo falla. */
            String temporal = ruta + ".tmp";
            File.Delete(temporal);
            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + temporal + ";Foreign Keys=True"))
            {
                cn.Open();
                using (SQLiteTransaction tx = cn.BeginTransaction())
                {
                    EjecutarScript(cn, "schema.sql");
                    EjecutarScript(cn, "seed.sql");
                    EjecutarScript(cn, "articulos_detalle.sql");
                    if (CargarDatosDemo)
                    {
                        EjecutarScript(cn, "demo.sql");
                    }
                    HashearContraseñas(cn);
                    EjecutarEscalar(cn, "pragma user_version = " + VersionSchema);
                    tx.Commit();
                }
            }
            SQLiteConnection.ClearAllPools();
            File.Move(temporal, ruta);
        }

        /* Los comandos se cierran siempre: uno abierto deja el archivo bloqueado. */
        private static object EjecutarEscalar(SQLiteConnection cn, String sql)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sql, cn))
            {
                return cmd.ExecuteScalar();
            }
        }

        /* Los scripts de datos traen las contraseñas en texto plano (son legibles
           para quien prueba el proyecto); acá se reemplazan por su hash. */
        private static void HashearContraseñas(SQLiteConnection cn)
        {
            DataTable usuarios = new DataTable();
            using (SQLiteDataAdapter adp = new SQLiteDataAdapter("select IdUsuario_us, Contraseña_us from Usuarios", cn))
            {
                adp.Fill(usuarios);
            }
            foreach (DataRow fila in usuarios.Rows)
            {
                String guardada = fila["Contraseña_us"].ToString();
                if (Contrasenas.EstaHasheada(guardada)) continue;
                using (SQLiteCommand cmd = new SQLiteCommand("update Usuarios set Contraseña_us = @Hash where IdUsuario_us = @Id", cn))
                {
                    cmd.Parameters.AddWithValue("@Hash", Contrasenas.Hashear(guardada));
                    cmd.Parameters.AddWithValue("@Id", fila["IdUsuario_us"]);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void EjecutarScript(SQLiteConnection cn, String nombre)
        {
            using (Stream recurso = typeof(AccesoDatos).Assembly.GetManifestResourceStream("Dao.Sql." + nombre))
            using (StreamReader lector = new StreamReader(recurso, Encoding.UTF8))
            using (SQLiteCommand cmd = new SQLiteCommand(lector.ReadToEnd(), cn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static SQLiteCommand ArmarComando(String consultaSql, SQLiteConnection cn, SQLiteParameter[] parametros)
        {
            SQLiteCommand cmd = new SQLiteCommand(consultaSql, cn);
            cmd.Parameters.AddRange(parametros);
            return cmd;
        }

        public DataTable ObtenerTabla(String NombreTabla, String Sql, params SQLiteParameter[] parametros)
        {
            DataSet ds = new DataSet();
            using (SQLiteConnection Conexion = ObtenerConexion())
            using (SQLiteDataAdapter adp = new SQLiteDataAdapter(ArmarComando(Sql, Conexion, parametros)))
            {
                adp.Fill(ds, NombreTabla);
            }
            return ds.Tables[NombreTabla];
        }

        /* Reemplaza a EjecutarProcedimientoAlmacenado: SQLite no tiene stored
           procedures, así que cada DAO manda el SQL que antes vivía en el SP. */
        public int EjecutarComando(SQLiteCommand Comando, String Sql)
        {
            using (SQLiteConnection Conexion = ObtenerConexion())
            {
                Comando.Connection = Conexion;
                Comando.CommandType = CommandType.Text;
                Comando.CommandText = Sql;
                return Comando.ExecuteNonQuery();
            }
        }

        /* Ejecuta "trabajo" en una sola conexión y transacción: se confirma todo o
           nada. System.Data.SQLite abre la transacción con BEGIN IMMEDIATE, así que
           otra escritura concurrente espera hasta que esta termine. */
        public T EnTransaccion<T>(Func<SQLiteConnection, T> trabajo)
        {
            using (SQLiteConnection Conexion = ObtenerConexion())
            using (SQLiteTransaction tx = Conexion.BeginTransaction())
            {
                T resultado = trabajo(Conexion);
                tx.Commit();
                return resultado; // si "trabajo" lanza una excepción, el Dispose hace rollback
            }
        }

        public Boolean existe(String consulta, params SQLiteParameter[] parametros)
        {
            using (SQLiteConnection Conexion = ObtenerConexion())
            using (SQLiteCommand cmd = ArmarComando(consulta, Conexion, parametros))
            using (SQLiteDataReader datos = cmd.ExecuteReader())
            {
                return datos.Read();
            }
        }

        public int ObtenerMaximo(String consulta, params SQLiteParameter[] parametros)
        {
            using (SQLiteConnection Conexion = ObtenerConexion())
            using (SQLiteCommand cmd = ArmarComando(consulta, Conexion, parametros))
            using (SQLiteDataReader datos = cmd.ExecuteReader())
            {
                return datos.Read() ? Convert.ToInt32(datos[0].ToString()) : 0;
            }
        }

    }

}
