using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using System.Data;
using System.Data.SqlClient;


namespace Dao
{
    public class daoDetalleVentas
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoDetalleVenta = "select * from Detalle_Ventas ";

        public DetalleVentas ObtenerDetalleVenta(DetalleVentas dven)
        {
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas",TodoDetalleVenta + "where IdDetVenta_dv = '" + dven.IdDetVenta1+ "' and CodArticulo_dv = '"+dven.CodArticulo1+"'");
            dven.IdDetVenta1 = Convert.ToInt32(tabla.Rows[0][0].ToString());
            dven.IdUsuario1 =Convert.ToInt32(tabla.Rows[0][1].ToString());
            dven.CodArticulo1 = tabla.Rows[0][2].ToString();
            dven.Cantidad =Convert.ToInt32(tabla.Rows[0][3].ToString());
            dven.Precio1 =Convert.ToInt32(tabla.Rows[0][4].ToString());
            dven.DireccionEntrega1 = tabla.Rows[0][5].ToString();
            dven.Contacto1 = tabla.Rows[0][6].ToString();
            return dven;          
        }

        public DataTable GetTablaDetalleVenta(DetalleVentas dven)
        {
            String NuevaConsul = TodoDetalleVenta + "where IdDetVenta_dv = '"+ dven.IdDetVenta1 +"' and CodArticulo_dv = '"+dven.CodArticulo1+"'";
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", NuevaConsul);
            return tabla;
        }

        public DataTable getTablaDetalleVenta()
        {
            DataTable tabla = ds.ObtenerTabla("Usuarios", TodoDetalleVenta);
            return tabla;
        }

        public Boolean ExisteDetalleVenta(DetalleVentas dv)
        {
            string consulta = TodoDetalleVenta + "where IdDetVenta_dv = '" + dv.IdDetVenta1  + "' and CodArticulo_dv = '" + dv.CodArticulo1 + "'";
            return ds.existe(consulta);
        }


        public int agregarDetalleVenta(DetalleVentas dv)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametroDetalleVentaAgregar(ref comando, dv);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_AgregarDetalleVenta");
        }

        public DataTable GetTablaIdVenta(DetalleVentas dven)
        {
            String NuevaConsul = TodoDetalleVenta + "where idDetVenta_dv = '" + dven.IdDetVenta1 + "'";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul);
            return tabla;
        }

        public DataTable GetTablaIdVentaUsuario(Ventas ven)
        {
            String NuevaConsul = "select Descripcion_ar, CodArticulo_dv, Cantidad_dv, Precio_Unitario_dv, Direccion_de_Entrega, Contacto from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv where idDetVenta_dv = '" + ven.IdVenta1 + "'";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul);
            return tabla;
        }

        public DataTable GetTablaIdUsuario(Usuario us)
        {
            String NuevaConsul = TodoDetalleVenta + "where IdUsuario_dv = '" + us.IdUsuario1 + "'";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul);
            return tabla;
        }

        public DataTable getMaximaCantidad()
        {
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas",TodoDetalleVenta + "where Cantidad_dv = (select max(Cantidad_dv) from Detalle_Ventas)");
            return tabla;
        }

        public DataTable getMaximoPrecio()
        {
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", TodoDetalleVenta +"where Precio_Unitario_dv = (select max(Precio_Unitario_dv) from Detalle_Ventas)");
            return tabla;
        }


        private void ArmarParametroDetalleVentaAgregar(ref SqlCommand comando, DetalleVentas dv)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@IDDETVENTA_DV", SqlDbType.BigInt);
            SqlParametros.Value = dv.IdDetVenta1;
            SqlParametros = comando.Parameters.Add("@IDUSUARIO_DV", SqlDbType.BigInt);
            SqlParametros.Value = dv.IdUsuario1;
            SqlParametros = comando.Parameters.Add("@CODARTICULO_DV", SqlDbType.Char);
            SqlParametros.Value = dv.CodArticulo1;
            SqlParametros = comando.Parameters.Add("@CANTIDAD_DV", SqlDbType.BigInt);
            SqlParametros.Value = dv.Cantidad;
            SqlParametros = comando.Parameters.Add("@PRECIOUNITARIO_DV", SqlDbType.Money);
            SqlParametros.Value = dv.Precio1;
            SqlParametros = comando.Parameters.Add("@DIRECCIONENTREGA_DV", SqlDbType.Text);
            SqlParametros.Value = dv.DireccionEntrega1;
            SqlParametros = comando.Parameters.Add("@CONTACTO", SqlDbType.VarChar);
             SqlParametros.Value = dv.Contacto1;
        }
    }
}
