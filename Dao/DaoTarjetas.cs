using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Dao
{
   public class DaoTarjetas
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoTarjetas = "select * from Tarjetas ";
        /* Ultimos 4 digitos como numero, igual que right(Num_Tarjeta_t,4) comparado
           contra un int en SQL Server (que convertia '0012' a 12). */
        const string UltimosDigitos = "cast(substr(Num_Tarjeta_t,-4) as integer)";

        public Tarjeta ObtenerTarjeta(Tarjeta tarj) //Devuelve un objeto Tarjeta
        {
            DataTable tabla = ds.ObtenerTabla("Tarjetas", TodoTarjetas + "where Num_Tarjeta_t = @NumTarjeta", new SQLiteParameter("@NumTarjeta", tarj.Num_Tarjeta1));
            tarj.Us.IdUsuario1 = Convert.ToInt32(tabla.Rows[0][0]);
            tarj.Titular_Tarjeta1 = tabla.Rows[0][1].ToString();
            tarj.Nombre_Tarjeta1 = tabla.Rows[0][2].ToString();
            tarj.Tipo_Tarjeta1 = (bool)tabla.Rows[0][3];
            tarj.Num_Tarjeta1 = Convert.ToInt64(tabla.Rows[0][4]);
            tarj.Fecha_Vencimiento1 = tabla.Rows[0][5].ToString();
            tarj.Codigo_Seguridad1 = Convert.ToInt32(tabla.Rows[0][6]);
            return tarj;
        }

        public DataTable getTablaTarjeta(Tarjeta tarj)//Devuelve una tarjeta
        {
            String NuevaConsul = TodoTarjetas + "where Num_Tarjeta_t = @NumTarjeta";
            DataTable tabla = ds.ObtenerTabla("Tarjetas", NuevaConsul, new SQLiteParameter("@NumTarjeta", tarj.Num_Tarjeta1));
            return tabla;
        }

        public DataTable ObtenerTarjetasUsuario (Usuario us)
        {
            String NuevaConsul = "select Id_TarjetaUsuario_t,Titular_Tarjeta_t,Nombre_Tarjeta_t, CASE Tipo_Tarjeta_t WHEN 0 THEN 'Débito' WHEN 1 THEN 'Crédito' END AS [Tipo_Tarjeta_t],substr(Num_Tarjeta_t,-4) as [Num_Tarjeta_t],Fecha_Vencimiento_t,Codigo_Seguridad_t from Tarjetas where Id_TarjetaUsuario_t = @IdUsuario";
            DataTable tabla = ds.ObtenerTabla("Tarjetas", NuevaConsul, new SQLiteParameter("@IdUsuario", us.IdUsuario1));
            return tabla;
        }

        /* ¿El usuario ya tiene guardada esta tarjeta? Compara el número completo. */
        public bool ExisteTarjeta (Tarjeta tarj)
        {
            string consulta = TodoTarjetas + "where Id_TarjetaUsuario_t = @IdUsuario and Num_Tarjeta_t = @NumTarjeta";
            return ds.existe(consulta,
                new SQLiteParameter("@IdUsuario", tarj.Us.IdUsuario1),
                new SQLiteParameter("@NumTarjeta", tarj.Num_Tarjeta1));
        }

        /* La página solo conoce los últimos 4 dígitos (es lo que muestra), así que
           se identifica la tarjeta por esos dígitos, el código y el titular,
           siempre dentro de las tarjetas del usuario. */
        public int EliminarTarjeta (Tarjeta tarj)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroTarjetaEliminar(ref comando, tarj);
            // ex sp_EliminarTarjeta
            return ds.EjecutarComando(comando,
                "delete from Tarjetas where Id_TarjetaUsuario_t = @IDUSUARIO_T and " + UltimosDigitos + " = @NUMEROTARJETA_T and Codigo_Seguridad_t = @CODIGODESEGURIDAD_T and Titular_Tarjeta_t = @TITULARTARJETA_T");
        }

        public int AgregarTarjeta (Tarjeta tarj)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametrosTarjetaAgregar(ref comando, tarj);
            // ex sp_AgregarTarjeta
            return ds.EjecutarComando(comando,
                "insert into Tarjetas(Id_TarjetaUsuario_t,Titular_Tarjeta_t,Nombre_Tarjeta_t,Tipo_Tarjeta_t,Num_Tarjeta_t,Fecha_Vencimiento_t,Codigo_Seguridad_t) values (@IDTARJETA_T,@TITULARTARJETA_T,@NOMBRETARJETA_T,@TIPODETARJETA_T,@NUMEROTARJETA_T,@FECHAVENCIMIENTO_T,@CODIGODESEGURIDAD_T)");
        }

        private void ArmarParametroTarjetaEliminar(ref SQLiteCommand comando, Tarjeta tarj)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDUSUARIO_T", DbType.Int64);
            Parametros.Value = tarj.Us.IdUsuario1;
            Parametros = comando.Parameters.Add("@NUMEROTARJETA_T", DbType.Int64);
            Parametros.Value = tarj.Num_Tarjeta1;
            Parametros = comando.Parameters.Add("@CODIGODESEGURIDAD_T", DbType.Int32);
            Parametros.Value = tarj.Codigo_Seguridad1;
            Parametros = comando.Parameters.Add("@TITULARTARJETA_T", DbType.String);
            Parametros.Value = tarj.Titular_Tarjeta1;
        }

        private void ArmarParametrosTarjetaAgregar(ref SQLiteCommand comando, Tarjeta tarj)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDTARJETA_T", DbType.Int64);
            Parametros.Value = tarj.Us.IdUsuario1;
            Parametros = comando.Parameters.Add("@TITULARTARJETA_T", DbType.String);
            Parametros.Value = tarj.Titular_Tarjeta1;
            Parametros = comando.Parameters.Add("@NOMBRETARJETA_T", DbType.String);
            Parametros.Value = tarj.Nombre_Tarjeta1;
            Parametros = comando.Parameters.Add("@TIPODETARJETA_T", DbType.Boolean);
            Parametros.Value = tarj.Tipo_Tarjeta1;
            Parametros = comando.Parameters.Add("@NUMEROTARJETA_T", DbType.Int64);
            Parametros.Value = tarj.Num_Tarjeta1;
            /* La vista arma la fecha como "dd/MM/yyyy"; se guarda en ISO para que
               SQLite pueda compararla con date('now'). */
            Parametros = comando.Parameters.Add("@FECHAVENCIMIENTO_T", DbType.String);
            Parametros.Value = DateTime.Parse(tarj.Fecha_Vencimiento1, new CultureInfo("es-AR")).ToString("yyyy-MM-dd");
            Parametros = comando.Parameters.Add("@CODIGODESEGURIDAD_T", DbType.Int32);
            Parametros.Value = tarj.Codigo_Seguridad1;
        }
    }
}
