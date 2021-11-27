using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Dao;
using System.Windows.Forms;
using System.Data;

namespace Negocio
{
   public class GestionTarjetas
    {
        DaoTarjetas dao = new DaoTarjetas();

        public Tarjeta getTarjeta(Tarjeta tarj)
        {
            dao.ObtenerTarjeta(tarj);
            return tarj;
        }

        public DataTable getTablaTarjeta(Tarjeta tarj)
        {
            DataTable tarjeta = dao.getTablaTarjeta(tarj);
            return tarjeta;
        }

        public DataTable getTablaTarjetasUsuario(Usuario us)
        {
            DataTable Tarjetas = dao.ObtenerTarjetasUsuario(us);
            return Tarjetas;
        }

        public bool ExisteTarjeta(Tarjeta tarj)
        {
            if (dao.ExisteTarjeta(tarj))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool eliminarTarjeta(Tarjeta tarj)
        {
            
            if (dao.EliminarTarjeta(tarj) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool agregarTarjeta(Tarjeta tarj)
        {            
            if (dao.ExisteTarjeta(tarj))
            {
                MessageBox.Show("Esta tarjeta ya existe en la base de datos", "Error");
                return false;
            }
            else
            {
                dao.AgregarTarjeta(tarj);
                return true;
            }
        }
    }
}
