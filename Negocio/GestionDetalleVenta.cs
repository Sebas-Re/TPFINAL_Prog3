using System.Data;
using Dao;
using Entidades;

namespace Negocio
{
    public class GestionDetalleVenta
    {
        private readonly DaoDetalleVentas _daoDetalleVentas = new DaoDetalleVentas();



        public DataTable GetTablaDetalleComprasUsuario(Venta ven)
        {
            DataTable dv = _daoDetalleVentas.GetTablaIdVentaUsuario(ven);
            return dv;
        }

    }
}
