using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using System.Data;
using System.Data.SQLite;

namespace Dao
{
    public class daoDetalleVentas
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoDetalleVenta = "select * from Detalle_Ventas ";

        public DetalleVentas ObtenerDetalleVenta(DetalleVentas dven)
        {
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", TodoDetalleVenta + "where IdDetVenta_dv = @IdDetVenta and CodArticulo_dv = @CodArticulo",
                new SQLiteParameter("@IdDetVenta", dven.IdDetVenta1), new SQLiteParameter("@CodArticulo", dven.CodArticulo1));
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
            String NuevaConsul = TodoDetalleVenta + "where IdDetVenta_dv = @IdDetVenta and CodArticulo_dv = @CodArticulo";
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", NuevaConsul,
                new SQLiteParameter("@IdDetVenta", dven.IdDetVenta1), new SQLiteParameter("@CodArticulo", dven.CodArticulo1));
            return tabla;
        }

        public DataTable getTablaDetalleVenta()
        {
            DataTable tabla = ds.ObtenerTabla("Usuarios", TodoDetalleVenta);
            return tabla;
        }

        public Boolean ExisteDetalleVenta(DetalleVentas dv)
        {
            string consulta = TodoDetalleVenta + "where IdDetVenta_dv = @IdDetVenta and CodArticulo_dv = @CodArticulo";
            return ds.existe(consulta, new SQLiteParameter("@IdDetVenta", dv.IdDetVenta1), new SQLiteParameter("@CodArticulo", dv.CodArticulo1));
        }

        const string InsertarDetalle =
            "insert into Detalle_Ventas(IdDetVenta_dv,IdUsuario_dv,CodArticulo_dv,Cantidad_dv,Precio_Unitario_dv,Direccion_de_Entrega,Contacto) values (@IDDETVENTA_DV,@IDUSUARIO_DV,@CODARTICULO_DV,@CANTIDAD_DV,@PRECIOUNITARIO_DV,@DIRECCIONENTREGA_DV,@CONTACTO)";

        public int agregarDetalleVenta(DetalleVentas dv)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroDetalleVentaAgregar(ref comando, dv);
            // ex sp_AgregarDetalleVenta. Los triggers tr_SumaTotal y tr_ControlStock siguen en la base.
            return ds.EjecutarComando(comando, InsertarDetalle);
        }

        /* Dentro de una transacción abierta en "cn" (ver DaoVentas.RegistrarCompra). */
        internal int agregarDetalleVenta(DetalleVentas dv, SQLiteConnection cn)
        {
            SQLiteCommand comando = new SQLiteCommand(InsertarDetalle, cn);
            ArmarParametroDetalleVentaAgregar(ref comando, dv);
            using (comando)
            {
                return comando.ExecuteNonQuery();
            }
        }

        public DataTable GetTablaIdVenta(DetalleVentas dven)
        {
            String NuevaConsul = TodoDetalleVenta + "where idDetVenta_dv = @IdDetVenta";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdDetVenta", dven.IdDetVenta1));
            return tabla;
        }

        public DataTable GetTablaIdVentaUsuario(Ventas ven)
        {
            String NuevaConsul = "select Descripcion_ar, CodArticulo_dv, Cantidad_dv, Precio_Unitario_dv, Direccion_de_Entrega, Contacto from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv where idDetVenta_dv = @IdVenta";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdVenta", ven.IdVenta1));
            return tabla;
        }

        public DataTable GetTablaIdUsuario(Usuario us)
        {
            String NuevaConsul = TodoDetalleVenta + "where IdUsuario_dv = @IdUsuario";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdUsuario", us.IdUsuario1));
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

        private void ArmarParametroDetalleVentaAgregar(ref SQLiteCommand comando, DetalleVentas dv)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDDETVENTA_DV", DbType.Int64);
            Parametros.Value = dv.IdDetVenta1;
            Parametros = comando.Parameters.Add("@IDUSUARIO_DV", DbType.Int64);
            Parametros.Value = dv.IdUsuario1;
            Parametros = comando.Parameters.Add("@CODARTICULO_DV", DbType.String);
            Parametros.Value = dv.CodArticulo1;
            Parametros = comando.Parameters.Add("@CANTIDAD_DV", DbType.Int64);
            Parametros.Value = dv.Cantidad;
            Parametros = comando.Parameters.Add("@PRECIOUNITARIO_DV", DbType.Decimal);
            Parametros.Value = dv.Precio1;
            Parametros = comando.Parameters.Add("@DIRECCIONENTREGA_DV", DbType.String);
            Parametros.Value = dv.DireccionEntrega1;
            Parametros = comando.Parameters.Add("@CONTACTO", DbType.String);
            Parametros.Value = dv.Contacto1;
        }
    }
}
