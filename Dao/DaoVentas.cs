using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Entidades;

namespace Dao
{
    public class DaoVentas
    {
        AccesoDatos ds = new AccesoDatos();
        Ventas venta = new Ventas();
        Usuario usuario = new Usuario();
        const string TodoVentas = "select * from Ventas";

        public Ventas ObtenerVenta(Ventas ve) //devuelve un objeto cargado con la data q coincida con el id
        {
            DataTable tabla = ds.ObtenerTabla("Ventas",TodoVentas +" where IdVentas_ve = '" + ve.IdVenta1+ "'");
            ve.IdVenta1 = Convert.ToInt32(tabla.Rows[0][0].ToString());
            ve.Usuario1.IdUsuario1 =Convert.ToInt32(tabla.Rows[0][1].ToString());
            ve.TotalVenta1 =Convert.ToInt32(tabla.Rows[0][2].ToString());
            ve.Metodo_Pago1 = (bool)tabla.Rows[0][3];
            ve.Cod_Seguridad1 = Convert.ToInt32(tabla.Rows[0][4]);
            return ve;
        }

        public DataTable GetTablaVentas(Ventas ve) //devuelve un datatable con los datos de la tabla
        {
            
            String NuevaConsul = TodoVentas + "where IdVentas_ve = " + ve.IdVenta1;
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul);
            return tabla;
        }
        
        public DataTable getTablaVentas()
        {
            DataTable tabla = ds.ObtenerTabla("Ventas", TodoVentas);
            return tabla;
        }

        public DataTable getMaximoVenta()
        {            
            DataTable tabla = ds.ObtenerTabla("Ventas", TodoVentas + " where TotalVenta_ve = (select max(TotalVenta_ve) from Ventas)");
            return tabla;
        }

        public DataTable GetTablaIdUsuario(Usuario us)
        {
            String NuevaConsul = TodoVentas + " where IdUsuario_ve = '" + us.IdUsuario1 + "'";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul);
            return tabla;
        }

        public Ventas IDultimaventa(Ventas ven)
        {
            string NuevaConsulta = "select MAX(IdVentas_ve) from Ventas";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsulta);
            ven.IdVenta1 = (int)Convert.ToInt64(tabla.Rows[0][0]); 
            return ven;
        }

        public Boolean ExisteVentas(Ventas ve)
        {
            string consulta = TodoVentas + " where IdUsuario_ve = '" + ve.IdVenta1 + "'";
            return ds.existe(consulta);
        }

        public int agregarVenta(Ventas ve)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametroVentaAgregar(ref comando, ve);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_AgregarVenta");
        }

        private void ArmarParametroVentaAgregar(ref SqlCommand comando,  Ventas ve)
        {
            SqlParameter SqlParametros = new SqlParameter();            
            SqlParametros = comando.Parameters.Add("@IDUSUARIO_VE", SqlDbType.BigInt);
            SqlParametros.Value = ve.Usuario1.IdUsuario1;
            SqlParametros = comando.Parameters.Add("@METODODEPAGO_VE", SqlDbType.Bit);
            SqlParametros.Value = ve.Metodo_Pago1;
            SqlParametros = comando.Parameters.Add("@CODIGODESEGURIDAD_VE", SqlDbType.Int);
            SqlParametros.Value = ve.Cod_Seguridad1;
        }
    }
}
