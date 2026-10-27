using System.Data;
using Dao;
using Entidades;

namespace Negocio
{
    public class GestionDetalleVenta
    {
        private readonly DaoDetalleVentas _daoDetalleVentas = new DaoDetalleVentas();



        public DataTable GetTablaDetalleComprasUsuario(Venta venta)
        {
            DataTable detalles = _daoDetalleVentas.GetTablaIdVentaUsuario(venta);
            return detalles;
        }

    }
}
