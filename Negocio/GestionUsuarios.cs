using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidades;
using Dao;

namespace Negocio
{
    public class GestionUsuarios
    {
        DaoUsuarios dao = new DaoUsuarios();
        Usuario us = new Usuario();

        public DataTable getUsuarios() //devuelve todos los usarios
        {            
            return dao.getTablaUsuarios();
        }
       
        public DataTable getTabla(Usuario us) //Devuelve un usuario
        {
            return dao.GetTablaUsuario(us);
        }

        public DataTable getUsuariosActualizar(Usuario us) //lo estaba usando para el griw para que los usuarios actualizen sus datos
        {
            DaoUsuarios dao = new DaoUsuarios();
            return dao.getTablaUsuariosActualizar(us);
        }

        public bool VerificarLogin(Usuario us)
        {            
            if (dao.ExisteUsuarioLogin(us) == false)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        public bool VerificarTipoUsuario(Usuario us)
        {            
            bool tipo = dao.TipoUsuarioLogin(us);
            return tipo;
        }

        public int DevolverIDusuario(Usuario us)
        {
           int ID = dao.DevolverIDusuario(us);
            return ID;
        }

        public Usuario get(Usuario us)
        {
            return dao.ObtenerUsuario(us);
        }

        public bool eliminarUsuario(ref Usuario us)
        {
            int op = dao.eliminarUsuario(ref us);
            if (op == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool agregarUsuario(Usuario us)
        {
            int cantFilas = 0;       
            if (dao.ExisteUsuario(us) == false)
            {
                cantFilas = dao.agregarUsuario(us);
            }
            else
            {
                return false;
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

        public bool VerificarNombre(Usuario us)
        {

            if (dao.ExisteUsuario(us) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerificarNombreModificar(Usuario us)
        {

            if (dao.ExisteUsuarioModificar(us) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerificarEmail(Usuario us)
        {
            
            if (dao.ExisteEmailUsuario(us) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerificarEmailModificar(Usuario us)
        {

            if (dao.ExisteEmailUsuarioModificar(us) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ArmarParametroUsuarios(ref SqlCommand comando, Usuario us)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@Id_Usuario_us", SqlDbType.BigInt);
            SqlParametros.Value = us.IdUsuario1;
            SqlParametros = comando.Parameters.Add("@NOMBRE_US", SqlDbType.VarChar);
            SqlParametros.Value = us.Nombre1;
            SqlParametros = comando.Parameters.Add("@CONTRASEÑA_US", SqlDbType.VarChar);
            SqlParametros.Value = us.Contraseña1;
            SqlParametros = comando.Parameters.Add("@EMAIL_US", SqlDbType.VarChar);
            SqlParametros.Value = us.Email1;
            SqlParametros = comando.Parameters.Add("@TELEFONO_US", SqlDbType.VarChar);
            SqlParametros.Value = us.Telefono1;
            SqlParametros = comando.Parameters.Add("@TIPO_US", SqlDbType.Bit);
            SqlParametros.Value = us.Tipo1;
            SqlParametros = comando.Parameters.Add("@Estado_us", SqlDbType.Bit);
            SqlParametros.Value = us.Estado1;
        }
       
        public bool ActualizarUsuario(Usuario Usuario)
        {
            SqlCommand Comando = new SqlCommand();
            ArmarParametroUsuarios(ref Comando, Usuario);
            AccesoDatos ad = new AccesoDatos();
            int FilasInsertadas = ad.EjecutarProcedimientoAlmacenado(Comando, "sp_ModificarUsuario");
            if (FilasInsertadas == 1)
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
