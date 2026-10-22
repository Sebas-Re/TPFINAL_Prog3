using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
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

        /* Devuelve el usuario con su id y tipo si las credenciales son válidas,
           o null si no lo son. */
        public Usuario Login(string nombre, string contraseña)
        {
            Usuario credenciales = new Usuario { Nombre1 = nombre, Contraseña1 = contraseña };
            if (!VerificarLogin(credenciales))
            {
                return null;
            }
            credenciales.IdUsuario1 = DevolverIDusuario(credenciales);
            credenciales.Tipo1 = VerificarTipoUsuario(credenciales);
            return credenciales;
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

       
        public bool ActualizarUsuario(Usuario Usuario)
        {
            int FilasInsertadas = dao.actualizarUsuario(Usuario);
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
