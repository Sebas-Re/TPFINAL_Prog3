using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Negocio
{
    public class GestionVentas
    {
        DaoVentas daov = new DaoVentas();
       

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
    }
}
