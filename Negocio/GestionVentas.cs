using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao;
using System.Data;
using Entidades;

namespace Negocio
{
    public class GestionVentas
    {
        DaoVentas daov = new DaoVentas();
        daoDetalleVentas daoDetalle = new daoDetalleVentas();
       

        public DataTable getVentas()
        {            
            return daov.getTablaVentas();
        }

        public DataTable ObtenerMaximaVenta()
        {
            return daov.getMaximoVenta();
        }
        public DataTable getVenta(Ventas ve) 
        {
            return daov.GetTablaVentas(ve);           
        }        

        public DataTable filtrarIdUsuario(Usuario us)
        {
            //  return daou.GetTablaUsuario(us);
            return daov.GetTablaIdUsuario(us);
        }

        public Ventas getVentaobj(Ventas ve)
        {
            return daov.ObtenerVenta(ve);
        }

        public bool agregarVenta(Ventas ve)
        {
            int cantFilas = 0;
            if (daov.ExisteVentas(ve) == false)
            {
                cantFilas = daov.agregarVenta(ve);
            }
            if (cantFilas == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /* Registra la venta y un detalle por cada artículo del carrito. Los
           triggers de la base descuentan el stock y acumulan el total.
           Devuelve el id de la venta, o 0 si no se pudo registrar (en ese
           caso "error" explica el motivo). */
        public int ConfirmarCompra(Usuario us, Carrito carrito, string direccion, string contacto, bool pagoConCredito, int codigoSeguridad, out string error)
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

            Ventas venta = new Ventas { Usuario1 = us, Metodo_Pago1 = pagoConCredito, Cod_Seguridad1 = codigoSeguridad };
            if (!agregarVenta(venta))
            {
                error = "No se pudo registrar la compra. Intentá nuevamente.";
                return 0;
            }
            int idVenta = daov.IDultimaventa(venta).IdVenta1;

            foreach (ItemCarrito item in carrito.Items)
            {
                DetalleVentas detalle = new DetalleVentas
                {
                    IdDetVenta1 = idVenta,
                    IdUsuario1 = us.IdUsuario1,
                    CodArticulo1 = item.CodArticulo,
                    Cantidad = item.Cantidad,
                    Precio1 = (float)item.PrecioUnitario,
                    DireccionEntrega1 = direccion,
                    Contacto1 = contacto
                };
                daoDetalle.agregarDetalleVenta(detalle);
            }
            return idVenta;
        }
    }
}
