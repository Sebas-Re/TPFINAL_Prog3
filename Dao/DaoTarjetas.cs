using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public Tarjeta ObtenerTarjeta(Tarjeta tarj) //Devuelve un objeto Tarjeta
        {
            DataTable tabla = ds.ObtenerTabla("Tarjetas", "select * from Tarjetas where Num_Tarjeta_t = " + "'" + tarj.Num_Tarjeta1 + "'");
            tarj.Us.IdUsuario1 = Convert.ToInt32(tabla.Rows[0][0]);
            tarj.Titular_Tarjeta1 = tabla.Rows[0][1].ToString();
            tarj.Nombre_Tarjeta1 = tabla.Rows[0][2].ToString();
            tarj.Tipo_Tarjeta1 = (bool)tabla.Rows[0][3];
            tarj.Num_Tarjeta1 = Convert.ToInt32(tabla.Rows[0][4]);
            tarj.Fecha_Vencimiento1 = tabla.Rows[0][5].ToString();
            tarj.Codigo_Seguridad1 = Convert.ToInt32(tabla.Rows[0][6]);

            return tarj;
        }


        public DataTable getTablaTarjeta(Tarjeta tarj)//Devuelve una tarjeta
        {
            String NuevaConsul = TodoTarjetas + "where Num_Tarjeta_t = " + "'" + tarj.Num_Tarjeta1 + "'";
            DataTable tabla = ds.ObtenerTabla("Tarjetas", NuevaConsul);
            return tabla;
        }

        public DataTable ObtenerTarjetasUsuario (Usuario us)
        {
            String NuevaConsul = "select Id_TarjetaUsuario_t,Titular_Tarjeta_t,Nombre_Tarjeta_t, CASE Tipo_Tarjeta_t WHEN 0 THEN 'Débito' WHEN 1 THEN 'Crédito' END AS [Tipo_Tarjeta_t],right(Num_Tarjeta_t,4) as [Num_Tarjeta_t],Fecha_Vencimiento_t,Codigo_Seguridad_t from Tarjetas where Id_TarjetaUsuario_t = " + "'" + us.IdUsuario1 + "'";
            DataTable tabla = ds.ObtenerTabla("Tarjetas", NuevaConsul);
            return tabla;
        }

        public bool ExisteTarjeta (Tarjeta tarj)
        {
            string consulta = TodoTarjetas + "where right(Num_Tarjeta_t,4) = " + "'" + tarj.Num_Tarjeta1 + "' and Codigo_Seguridad_t = '"+ tarj.Codigo_Seguridad1 + "' and Titular_Tarjeta_t = '" +tarj.Titular_Tarjeta1 +"'";
            return ds.existe(consulta);
        }
        
        public int EliminarTarjeta (Tarjeta tarj)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametroTarjetaEliminar(ref comando, tarj);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_EliminarTarjeta");
        }

        public int AgregarTarjeta (Tarjeta tarj)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametrosTarjetaAgregar(ref comando, tarj);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_AgregarTarjeta");
        }

        private void ArmarParametroTarjetaEliminar(ref SqlCommand comando, Tarjeta tarj)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@NUMEROTARJETA_T", SqlDbType.BigInt);
            SqlParametros.Value = tarj.Num_Tarjeta1;
            SqlParametros = comando.Parameters.Add("@CODIGODESEGURIDAD_T", SqlDbType.Int);
            SqlParametros.Value = tarj.Codigo_Seguridad1;
            SqlParametros = comando.Parameters.Add("@TITULARTARJETA_T", SqlDbType.VarChar);
            SqlParametros.Value = tarj.Titular_Tarjeta1;

        }

        private void ArmarParametrosTarjetaAgregar(ref SqlCommand comando, Tarjeta tarj)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@IDTARJETA_T", SqlDbType.BigInt);
            SqlParametros.Value = tarj.Us.IdUsuario1;
            SqlParametros = comando.Parameters.Add("@TITULARTARJETA_T", SqlDbType.VarChar);
            SqlParametros.Value = tarj.Titular_Tarjeta1;
            SqlParametros = comando.Parameters.Add("@NOMBRETARJETA_T", SqlDbType.VarChar);
            SqlParametros.Value = tarj.Nombre_Tarjeta1;
            SqlParametros = comando.Parameters.Add("@TIPODETARJETA_T", SqlDbType.Bit);
            SqlParametros.Value = tarj.Tipo_Tarjeta1;
            SqlParametros = comando.Parameters.Add("@NUMEROTARJETA_T", SqlDbType.BigInt);
            SqlParametros.Value = tarj.Num_Tarjeta1;
            SqlParametros = comando.Parameters.Add("@FECHAVENCIMIENTO_T", SqlDbType.Date);
            SqlParametros.Value = tarj.Fecha_Vencimiento1;
            SqlParametros = comando.Parameters.Add("@CODIGODESEGURIDAD_T", SqlDbType.Int);
            SqlParametros.Value = tarj.Codigo_Seguridad1;
        }
    }
}
