using System.Data;
using System.Data.SQLite;
using Entidades;

namespace Dao
{
    public class DaoDetalleVentas
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();
        private const string TodoDetalleVenta = "select * from Detalle_Ventas ";


        private const string InsertarDetalle =
            "insert into Detalle_Ventas(IdDetVenta_dv,IdUsuario_dv,CodArticulo_dv,Cantidad_dv,Precio_Unitario_dv,Direccion_de_Entrega,Contacto) values (@IDDETVENTA_DV,@IDUSUARIO_DV,@CODARTICULO_DV,@CANTIDAD_DV,@PRECIOUNITARIO_DV,@DIRECCIONENTREGA_DV,@CONTACTO)";

        /* Dentro de una transacción abierta en "conexion" (ver DaoVentas.RegistrarCompra). */
        internal int AgregarDetalleVenta(DetalleVenta detalleVenta, SQLiteConnection conexion)
        {
            SQLiteCommand comando = new SQLiteCommand(InsertarDetalle, conexion);
            ArmarParametroDetalleVentaAgregar(comando, detalleVenta);
            using (comando)
            {
                return comando.ExecuteNonQuery();
            }
        }

        public DataTable GetTablaIdVentaUsuario(Venta venta)
        {
            string consulta = "select Descripcion_ar, CodArticulo_dv, Cantidad_dv, Precio_Unitario_dv, Direccion_de_Entrega, Contacto from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv where idDetVenta_dv = @IdVenta";
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", consulta, new SQLiteParameter("@IdVenta", venta.IdVenta));
            return tabla;
        }

        private void ArmarParametroDetalleVentaAgregar(SQLiteCommand comando, DetalleVenta detalleVenta)
        {
            comando.Parameters.Add("@IDDETVENTA_DV", DbType.Int64).Value = detalleVenta.IdDetalleVenta;
            comando.Parameters.Add("@IDUSUARIO_DV", DbType.Int64).Value = detalleVenta.IdUsuario;
            comando.Parameters.Add("@CODARTICULO_DV", DbType.String).Value = detalleVenta.CodigoArticulo;
            comando.Parameters.Add("@CANTIDAD_DV", DbType.Int64).Value = detalleVenta.Cantidad;
            comando.Parameters.Add("@PRECIOUNITARIO_DV", DbType.Decimal).Value = detalleVenta.Precio;
            comando.Parameters.Add("@DIRECCIONENTREGA_DV", DbType.String).Value = detalleVenta.DireccionEntrega;
            comando.Parameters.Add("@CONTACTO", DbType.String).Value = detalleVenta.Contacto;
        }
    }
}
