using System;
using System.Data;
using System.Data.SQLite;
using Entidades;

namespace Dao
{
    public class DaoDetalleVentas
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();
        const string TodoDetalleVenta = "select * from Detalle_Ventas ";


        const string InsertarDetalle =
            "insert into Detalle_Ventas(IdDetVenta_dv,IdUsuario_dv,CodArticulo_dv,Cantidad_dv,Precio_Unitario_dv,Direccion_de_Entrega,Contacto) values (@IDDETVENTA_DV,@IDUSUARIO_DV,@CODARTICULO_DV,@CANTIDAD_DV,@PRECIOUNITARIO_DV,@DIRECCIONENTREGA_DV,@CONTACTO)";

        /* Dentro de una transacción abierta en "conexion" (ver DaoVentas.RegistrarCompra). */
        internal int AgregarDetalleVenta(DetalleVenta detalleVenta, SQLiteConnection conexion)
        {
            SQLiteCommand comando = new SQLiteCommand(InsertarDetalle, conexion);
            ArmarParametroDetalleVentaAgregar(ref comando, detalleVenta);
            using (comando)
            {
                return comando.ExecuteNonQuery();
            }
        }

        public DataTable GetTablaIdVentaUsuario(Venta venta)
        {
            String NuevaConsul = "select Descripcion_ar, CodArticulo_dv, Cantidad_dv, Precio_Unitario_dv, Direccion_de_Entrega, Contacto from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv where idDetVenta_dv = @IdVenta";
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdVenta", venta.IdVenta));
            return tabla;
        }

        private void ArmarParametroDetalleVentaAgregar(ref SQLiteCommand comando, DetalleVenta detalleVenta)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDDETVENTA_DV", DbType.Int64);
            Parametros.Value = detalleVenta.IdDetalleVenta;
            Parametros = comando.Parameters.Add("@IDUSUARIO_DV", DbType.Int64);
            Parametros.Value = detalleVenta.IdUsuario;
            Parametros = comando.Parameters.Add("@CODARTICULO_DV", DbType.String);
            Parametros.Value = detalleVenta.CodigoArticulo;
            Parametros = comando.Parameters.Add("@CANTIDAD_DV", DbType.Int64);
            Parametros.Value = detalleVenta.Cantidad;
            Parametros = comando.Parameters.Add("@PRECIOUNITARIO_DV", DbType.Decimal);
            Parametros.Value = detalleVenta.Precio;
            Parametros = comando.Parameters.Add("@DIRECCIONENTREGA_DV", DbType.String);
            Parametros.Value = detalleVenta.DireccionEntrega;
            Parametros = comando.Parameters.Add("@CONTACTO", DbType.String);
            Parametros.Value = detalleVenta.Contacto;
        }
    }
}
