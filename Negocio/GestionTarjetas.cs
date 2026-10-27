using System.Data;
using System.Data.Common;
using Dao;
using Entidades;

namespace Negocio
{
    public class GestionTarjetas
    {
        private readonly DaoTarjetas _daoTarjetas = new DaoTarjetas();

        public DataTable GetTablaTarjetasUsuario(Usuario us)
        {
            DataTable tarjetas = _daoTarjetas.ObtenerTarjetasUsuario(us);
            return tarjetas;
        }

        public bool EliminarTarjeta(Tarjeta tarjeta)
        {

            return _daoTarjetas.EliminarTarjeta(tarjeta) == 1;
        }

        public bool AgregarTarjeta(Tarjeta tarjeta)
        {
            if (_daoTarjetas.ExisteTarjeta(tarjeta))
            {
                return false;
            }
            try
            {
                return _daoTarjetas.AgregarTarjeta(tarjeta) == 1;
            }
            catch (DbException)
            {
                // Tarjeta vencida o repetida para el usuario (restricciones de la base).
                return false;
            }
        }
    }
}
