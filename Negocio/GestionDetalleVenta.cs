using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao;
using Entidades;
using System.Data;

namespace Negocio
{
    public class GestionDetalleVenta
    {
        DaoVentas daoven = new DaoVentas();
        daoDetalleVentas dao = new daoDetalleVentas();
        DetalleVentas dv = new DetalleVentas();

        public DataTable getTodoDetalleVenta() 
        {            
            return dao.getTablaDetalleVenta();
        }
        public DataTable getTabla(DetalleVentas dv) 
        {
            return dao.GetTablaDetalleVenta(dv);
        }

        public DataTable GetTablaDetalleComprasUsuario(Ventas ven)
        {
           DataTable dv = dao.GetTablaIdVentaUsuario(ven);
            return dv;
        }

        public DetalleVentas get(DetalleVentas dv)
        {  
           return dao.ObtenerDetalleVenta(dv);
        }

        public DataTable filtrarIdVenta(DetalleVentas detven)
        {

            return dao.GetTablaIdVenta(detven);

        }

        public DataTable filtrarIdUsuario(Usuario usuario)
        {
            return dao.GetTablaIdUsuario(usuario);
        }

        public DataTable ObtenerMaximaCantidadVendida()
        {
            return dao.getMaximaCantidad();
        }

        public DataTable ObtenerMaximoPrecioUnitario()
        {
            return dao.getMaximoPrecio();
        }

        public bool agregarDetalleVenta(DetalleVentas detven)
        {
            int cantFilas = 0;          

            if (dao.ExisteDetalleVenta(detven) == false)
            {
                cantFilas = dao.agregarDetalleVenta(detven);
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

        public Ventas Idultimaventa(Ventas ven)
        {
            daoven.IDultimaventa(ven);
            return ven;
        }
    }
}
