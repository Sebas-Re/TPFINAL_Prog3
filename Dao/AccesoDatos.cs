using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Entidades;

namespace Dao
{
    public class AccesoDatos
    {
        const String NombreArchivo = "PCbooster.db";
        static readonly object bloqueoInicializacion = new object();
        static String rutaInicializada;

        /* Ruta del archivo SQLite. Por defecto App_Data (ASP.NET define "DataDirectory")
           o, fuera de ASP.NET, la carpeta del ejecutable. Los tests la redirigen. */
        public static String RutaBaseDatos { get; set; } = Path.Combine(
            AppDomain.CurrentDomain.GetData("DataDirectory") as String ?? AppDomain.CurrentDomain.BaseDirectory,
            NombreArchivo);

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

        /* La primera vez que se usa la base, si el archivo no existe se crea con
           el schema y los datos de prueba embebidos en el assembly. */
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
                rutaInicializada = ruta;
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
                    tx.Commit();
                }
            }
            SQLiteConnection.ClearAllPools();
            File.Move(temporal, ruta);
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

        public Boolean existe(String consulta, params SQLiteParameter[] parametros)
        {
            using (SQLiteConnection Conexion = ObtenerConexion())
            using (SQLiteCommand cmd = ArmarComando(consulta, Conexion, parametros))
            using (SQLiteDataReader datos = cmd.ExecuteReader())
            {
                return datos.Read();
            }
        }

        public Boolean existeLogin(String consulta, params SQLiteParameter[] parametros)
        {
            using (SQLiteConnection Conexion = ObtenerConexion())
            using (SQLiteCommand cmd = ArmarComando(consulta, Conexion, parametros))
            using (SQLiteDataReader datos = cmd.ExecuteReader())
            {
                return datos.Read() && datos.GetBoolean(0);
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
