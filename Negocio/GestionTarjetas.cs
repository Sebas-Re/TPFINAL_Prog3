using System;
using System.Data;
using Dao;
using Entidades;

namespace Negocio
{
    public class GestionTarjetas
    {
        private readonly DaoTarjetas _daoTarjetas = new DaoTarjetas();

        public DataTable GetTablaTarjetasUsuario(Usuario us)
        {
            DataTable Tarjetas = _daoTarjetas.ObtenerTarjetasUsuario(us);
            return Tarjetas;
        }

        public bool EliminarTarjeta(Tarjeta tarjeta)
        {

            if (_daoTarjetas.EliminarTarjeta(tarjeta) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            catch (Exception)
            {
                // Tarjeta vencida o repetida para el usuario (restricciones de la base).
                return false;
            }
        }
    }
}
