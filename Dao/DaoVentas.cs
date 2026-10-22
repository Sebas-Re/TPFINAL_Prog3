using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
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
            DataTable tabla = ds.ObtenerTabla("Ventas", TodoVentas + " where IdVentas_ve = @IdVenta", new SQLiteParameter("@IdVenta", ve.IdVenta1));
            DataRow fila = tabla.Rows[0];
            ve.IdVenta1 = Convert.ToInt32(fila["IdVentas_ve"]);
            ve.Usuario1 = new Usuario { IdUsuario1 = Convert.ToInt32(fila["IdUsuario_ve"]) };
            ve.TotalVenta1 = Convert.ToSingle(fila["TotalVenta_ve"]);
            ve.Metodo_Pago1 = (bool)fila["Metodo_de_Pago_ve"];
            ve.Cod_Seguridad1 = Convert.ToInt32(fila["Codigo_Seguridad_ve"]);
            return ve;
        }

        public DataTable GetTablaVentas(Ventas ve) //devuelve un datatable con los datos de la tabla
        {
            String NuevaConsul = TodoVentas + " where IdVentas_ve = @IdVenta";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdVenta", ve.IdVenta1));
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
            String NuevaConsul = TodoVentas + " where IdUsuario_ve = @IdUsuario";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdUsuario", us.IdUsuario1));
            return tabla;
        }

        /* Ojo: devuelve la última venta de cualquier usuario. Para registrar una
           compra usar RegistrarCompra, que obtiene el id de su propio insert. */
        public Ventas IDultimaventa(Ventas ven)
        {
            string NuevaConsulta = "select MAX(IdVentas_ve) from Ventas";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsulta);
            ven.IdVenta1 = (int)Convert.ToInt64(tabla.Rows[0][0]);
            return ven;
        }

        public Boolean ExisteVentas(Ventas ve)
        {
            string consulta = TodoVentas + " where IdUsuario_ve = @IdVenta";
            return ds.existe(consulta, new SQLiteParameter("@IdVenta", ve.IdVenta1));
        }

        const string InsertarVenta =
            "insert into Ventas(IdUsuario_ve,Metodo_de_Pago_ve,Codigo_Seguridad_ve,FechaVenta_ve) values (@IDUSUARIO_VE,@METODODEPAGO_VE,@CODIGODESEGURIDAD_VE,datetime('now','localtime'))";

        public int agregarVenta(Ventas ve)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroVentaAgregar(ref comando, ve);
            // ex sp_AgregarVenta
            return ds.EjecutarComando(comando, InsertarVenta);
        }

        /* Registra la venta y sus detalles en una sola transacción y devuelve el id
           de la venta. Antes de cada detalle verifica que el artículo siga activo y
           con stock; si no, lanza StockInsuficienteException y no queda nada guardado.
           El id sale de last_insert_rowid() de esta conexión: no se confunde con una
           venta que otro usuario registre al mismo tiempo (como pasaba con MAX). */
        public int RegistrarCompra(Ventas venta, IList<DetalleVentas> detalles)
        {
            return ds.EnTransaccion(cn =>
            {
                SQLiteCommand comando = new SQLiteCommand(InsertarVenta, cn);
                ArmarParametroVentaAgregar(ref comando, venta);
                using (comando)
                {
                    comando.ExecuteNonQuery();
                }
                int idVenta = (int)cn.LastInsertRowId;

                daoDetalleVentas daoDetalle = new daoDetalleVentas();
                foreach (DetalleVentas detalle in detalles)
                {
                    VerificarStock(cn, detalle);
                    detalle.IdDetVenta1 = idVenta;
                    daoDetalle.agregarDetalleVenta(detalle, cn);
                }
                return idVenta;
            });
        }

        private static void VerificarStock(SQLiteConnection cn, DetalleVentas detalle)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("select Descripcion_ar, Stock_ar, Estado_ar from Articulos where CodArticulo_ar = @Codigo", cn))
            {
                cmd.Parameters.AddWithValue("@Codigo", detalle.CodArticulo1);
                using (SQLiteDataReader fila = cmd.ExecuteReader())
                {
                    if (!fila.Read() || !fila.GetBoolean(2))
                    {
                        throw new StockInsuficienteException(detalle.CodArticulo1, fila.HasRows ? fila.GetString(0) : detalle.CodArticulo1, 0);
                    }
                    long stock = fila.GetInt64(1);
                    if (stock < detalle.Cantidad)
                    {
                        throw new StockInsuficienteException(detalle.CodArticulo1, fila.GetString(0), stock);
                    }
                }
            }
        }

        private void ArmarParametroVentaAgregar(ref SQLiteCommand comando,  Ventas ve)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDUSUARIO_VE", DbType.Int64);
            Parametros.Value = ve.Usuario1.IdUsuario1;
            Parametros = comando.Parameters.Add("@METODODEPAGO_VE", DbType.Boolean);
            Parametros.Value = ve.Metodo_Pago1;
            Parametros = comando.Parameters.Add("@CODIGODESEGURIDAD_VE", DbType.Int32);
            Parametros.Value = ve.Cod_Seguridad1;
        }
    }
}
