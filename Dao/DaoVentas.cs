using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Entidades;

namespace Dao
{
    public class DaoVentas
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();

        private const string TodoVentas = "select * from Ventas";

        public DataTable GetTablaVentas()
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", TodoVentas);
            return tabla;
        }

        public DataTable GetTablaIdUsuario(Usuario usuario)
        {
            string consulta = TodoVentas + " where IdUsuario_ve = @IdUsuario";
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", consulta, new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
            return tabla;
        }

        private const string InsertarVenta =
            "insert into Ventas(IdUsuario_ve,Metodo_de_Pago_ve,Codigo_Seguridad_ve,FechaVenta_ve) values (@IDUSUARIO_VE,@METODODEPAGO_VE,@CODIGODESEGURIDAD_VE,datetime('now','localtime'))";

        /* Registra la venta y sus detalles en una sola transacción y devuelve el id
           de la venta. Antes de cada detalle verifica que el artículo siga activo y
           con stock; si no, lanza StockInsuficienteException y no queda nada guardado.
           El id sale de last_insert_rowid() de esta conexión: no se confunde con una
           venta que otro usuario registre al mismo tiempo (como pasaba con MAX). */
        public int RegistrarCompra(Venta venta, IList<DetalleVenta> detalles)
        {
            return _accesoDatos.EnTransaccion(conexion =>
            {
                SQLiteCommand comando = new SQLiteCommand(InsertarVenta, conexion);
                ArmarParametroVentaAgregar(comando, venta);
                using (comando)
                {
                    comando.ExecuteNonQuery();
                }
                int idVenta = (int)conexion.LastInsertRowId;

                DaoDetalleVentas daoDetalle = new DaoDetalleVentas();
                foreach (DetalleVenta detalle in detalles)
                {
                    VerificarStock(conexion, detalle);
                    detalle.IdDetalleVenta = idVenta;
                    daoDetalle.AgregarDetalleVenta(detalle, conexion);
                }
                return idVenta;
            });
        }

        private static void VerificarStock(SQLiteConnection conexion, DetalleVenta detalle)
        {
            using (SQLiteCommand comandoStock = new SQLiteCommand("select Descripcion_ar, Stock_ar, Estado_ar from Articulos where CodArticulo_ar = @Codigo", conexion))
            {
                comandoStock.Parameters.AddWithValue("@Codigo", detalle.CodigoArticulo);
                using (SQLiteDataReader fila = comandoStock.ExecuteReader())
                {
                    if (!fila.Read() || !fila.GetBoolean(2))
                    {
                        throw new StockInsuficienteException(detalle.CodigoArticulo, fila.HasRows ? fila.GetString(0) : detalle.CodigoArticulo, 0);
                    }
                    long stock = fila.GetInt64(1);
                    if (stock < detalle.Cantidad)
                    {
                        throw new StockInsuficienteException(detalle.CodigoArticulo, fila.GetString(0), stock);
                    }
                }
            }
        }

        private void ArmarParametroVentaAgregar(SQLiteCommand comando, Venta venta)
        {
            comando.Parameters.Add("@IDUSUARIO_VE", DbType.Int64).Value = venta.Usuario.IdUsuario;
            comando.Parameters.Add("@METODODEPAGO_VE", DbType.Boolean).Value = venta.MetodoPago;
            comando.Parameters.Add("@CODIGODESEGURIDAD_VE", DbType.Int32).Value = venta.CodigoSeguridad;
        }
    }
}
