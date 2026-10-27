using System.Collections.Generic;
using System.Data;
using Dao;
using Entidades;

namespace Negocio
{
    public class GestionVentas
    {
        private readonly DaoVentas daoVentas = new DaoVentas();
        private readonly DaoTarjetas daoTarjetas = new DaoTarjetas();


        public DataTable GetVentas()
        {
            return daoVentas.GetTablaVentas();
        }

        public DataTable GetVenta(Ventas venta)
        {
            return daoVentas.GetTablaVentas(venta);
        }

        public DataTable FiltrarIdUsuario(Usuario usuario)
        {
            //  return daou.GetTablaUsuario(usuario);
            return daoVentas.GetTablaIdUsuario(usuario);
        }



        /* Registra la venta y un detalle por cada artículo del carrito. Los
           triggers de la base descuentan el stock y acumulan el total.
           Devuelve el id de la venta, o 0 si no se pudo registrar (en ese
           caso "error" explica el motivo). */
        public int ConfirmarCompra(Usuario usuario, Carrito carrito, string direccion, string contacto, long idTarjeta, out string error)
        {
            error = null;
            if (carrito == null || carrito.Vacio)
            {
                error = "El carrito está vacío.";
                return 0;
            }
            if (string.IsNullOrWhiteSpace(direccion) || string.IsNullOrWhiteSpace(contacto))
            {
                error = "Completá la dirección de entrega y el contacto.";
                return 0;
            }

            // El medio de pago se toma de la base, no de lo que mande la página.
            Tarjeta tarjeta = daoTarjetas.ObtenerTarjetaVigente(usuario, idTarjeta);
            if (tarjeta == null)
            {
                error = "Elegí una de tus tarjetas guardadas que no esté vencida.";
                return 0;
            }

            Ventas venta = new Ventas { Usuario = usuario, MetodoPago = tarjeta.TipoTarjeta, CodigoSeguridad = tarjeta.CodigoSeguridad };
            List<DetalleVentas> detalles = new List<DetalleVentas>();
            foreach (ItemCarrito item in carrito.Items)
            {
                detalles.Add(new DetalleVentas
                {
                    IdUsuario = usuario.IdUsuario,
                    CodigoArticulo = item.CodArticulo,
                    Cantidad = item.Cantidad,
                    Precio = (float)item.PrecioUnitario,
                    DireccionEntrega = direccion,
                    Contacto = contacto
                });
            }

            // Venta y detalles van juntos: si algo falla no queda una compra a medias.
            try
            {
                return daoVentas.RegistrarCompra(venta, detalles);
            }
            catch (StockInsuficienteException ex)
            {
                error = ex.Disponible == 0
                    ? "\"" + ex.Descripcion + "\" ya no está disponible. Quitalo del carrito para continuar."
                    : "Solo quedan " + ex.Disponible + " unidades de \"" + ex.Descripcion + "\". Ajustá el carrito para continuar.";
                return 0;
            }
        }
    }
}
