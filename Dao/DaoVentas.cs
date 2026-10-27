using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Entidades;

namespace Dao
{
    public class DaoVentas
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();

        const string TodoVentas = "select * from Ventas";

        public Ventas ObtenerVenta(Ventas venta) //devuelve un objeto cargado con la data q coincida con el id
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", TodoVentas + " where IdVentas_ve = @IdVenta", new SQLiteParameter("@IdVenta", venta.IdVenta));
            DataRow fila = tabla.Rows[0];
            venta.IdVenta = Convert.ToInt32(fila["IdVentas_ve"]);
            venta.Usuario = new Usuario { IdUsuario = Convert.ToInt32(fila["IdUsuario_ve"]) };
            venta.TotalVenta = Convert.ToSingle(fila["TotalVenta_ve"]);
            venta.MetodoPago = (bool)fila["Metodo_de_Pago_ve"];
            venta.CodigoSeguridad = Convert.ToInt32(fila["Codigo_Seguridad_ve"]);
            return venta;
        }

        public DataTable GetTablaVentas(Ventas venta) //devuelve un datatable con los datos de la tabla
        {
            String NuevaConsul = TodoVentas + " where IdVentas_ve = @IdVenta";
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdVenta", venta.IdVenta));
            return tabla;
        }

        public DataTable GetTablaVentas()
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", TodoVentas);
            return tabla;
        }

        public DataTable GetMaximoVenta()
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", TodoVentas + " where TotalVenta_ve = (select max(TotalVenta_ve) from Ventas)");
            return tabla;
        }

        public DataTable GetTablaIdUsuario(Usuario usuario)
        {
            String NuevaConsul = TodoVentas + " where IdUsuario_ve = @IdUsuario";
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
            return tabla;
        }

        /* Ojo: devuelve la última venta de cualquier usuario. Para registrar una
           compra usar RegistrarCompra, que obtiene el id de su propio insert. */
        public Ventas IDultimaventa(Ventas venta)
        {
            string NuevaConsulta = "select MAX(IdVentas_ve) from Ventas";
            DataTable tabla = _accesoDatos.ObtenerTabla("Ventas", NuevaConsulta);
            venta.IdVenta = (int)Convert.ToInt64(tabla.Rows[0][0]);
            return venta;
        }

        public Boolean ExisteVentas(Ventas venta)
        {
            string consulta = TodoVentas + " where IdUsuario_ve = @IdVenta";
            return _accesoDatos.existe(consulta, new SQLiteParameter("@IdVenta", venta.IdVenta));
        }

        const string InsertarVenta =
            "insert into Ventas(IdUsuario_ve,Metodo_de_Pago_ve,Codigo_Seguridad_ve,FechaVenta_ve) values (@IDUSUARIO_VE,@METODODEPAGO_VE,@CODIGODESEGURIDAD_VE,datetime('now','localtime'))";

        public int agregarVenta(Ventas venta)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroVentaAgregar(ref comando, venta);
            // ex sp_AgregarVenta
            return _accesoDatos.EjecutarComando(comando, InsertarVenta);
        }

        /* Registra la venta y sus detalles en una sola transacción y devuelve el id
           de la venta. Antes de cada detalle verifica que el artículo siga activo y
           con stock; si no, lanza StockInsuficienteException y no queda nada guardado.
           El id sale de last_insert_rowid() de esta conexión: no se confunde con una
           venta que otro usuario registre al mismo tiempo (como pasaba con MAX). */
        public int RegistrarCompra(Ventas venta, IList<DetalleVentas> detalles)
        {
            return _accesoDatos.EnTransaccion(conexion =>
            {
                SQLiteCommand comando = new SQLiteCommand(InsertarVenta, conexion);
                ArmarParametroVentaAgregar(ref comando, venta);
                using (comando)
                {
                    comando.ExecuteNonQuery();
                }
                int idVenta = (int)conexion.LastInsertRowId;

                DaoDetalleVentas daoDetalle = new DaoDetalleVentas();
                foreach (DetalleVentas detalle in detalles)
                {
                    VerificarStock(conexion, detalle);
                    detalle.IdDetalleVenta = idVenta;
                    daoDetalle.AgregarDetalleVenta(detalle, conexion);
                }
                return idVenta;
            });
        }

        private static void VerificarStock(SQLiteConnection conexion, DetalleVentas detalle)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("select Descripcion_ar, Stock_ar, Estado_ar from Articulos where CodArticulo_ar = @Codigo", conexion))
            {
                cmd.Parameters.AddWithValue("@Codigo", detalle.CodigoArticulo);
                using (SQLiteDataReader fila = cmd.ExecuteReader())
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

        private void ArmarParametroVentaAgregar(ref SQLiteCommand comando, Ventas venta)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDUSUARIO_VE", DbType.Int64);
            Parametros.Value = venta.Usuario.IdUsuario;
            Parametros = comando.Parameters.Add("@METODODEPAGO_VE", DbType.Boolean);
            Parametros.Value = venta.MetodoPago;
            Parametros = comando.Parameters.Add("@CODIGODESEGURIDAD_VE", DbType.Int32);
            Parametros.Value = venta.CodigoSeguridad;
        }
    }
}
