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
        private readonly DaoDetalleVentas _daoDetalleVentas = new DaoDetalleVentas();



        public DataTable GetTablaDetalleComprasUsuario(Ventas ven)
        {
            DataTable dv = _daoDetalleVentas.GetTablaIdVentaUsuario(ven);
            return dv;
        }

    }
}
