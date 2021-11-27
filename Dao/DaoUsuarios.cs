using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidades;


namespace Dao
{
    public class DaoUsuarios
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoUsuarios = "select * from Usuarios";

        public Usuario ObtenerUsuario(Usuario us)
        {
            DataTable tabla = ds.ObtenerTabla("Usuarios", TodoUsuarios + " where IdUsuario_us = '" + us.IdUsuario1 + "'");
            us.IdUsuario1 = Convert.ToInt32(tabla.Rows[0][0].ToString());
            us.Nombre1 = tabla.Rows[0][1].ToString();
            us.Contraseña1 = tabla.Rows[0][2].ToString();
            us.Email1 = tabla.Rows[0][3].ToString();
            us.Telefono1 = tabla.Rows[0][4].ToString();
            us.Tipo1 = Convert.ToBoolean(tabla.Rows[0][5].ToString());
            us.Estado1 = Convert.ToBoolean(tabla.Rows[0][6].ToString());
            return us;
        }

        public DataTable GetTablaUsuario (Usuario us)
        {
            String NuevaConsul = TodoUsuarios + " where IdUsuario_us = '" + us.IdUsuario1 + "'";
            DataTable tabla = ds.ObtenerTabla("Usuarios", NuevaConsul);
            return tabla;
        }

        public int DevolverIDusuario(Usuario us)
        {            
            String NuevaConsul = "select IdUsuario_us from Usuarios where Nombre_us = '" + us.Nombre1 + "'";
            DataTable tabla = ds.ObtenerTabla("Usuarios", NuevaConsul);
            int ID = (int)Convert.ToInt64(tabla.Rows[0][0]);           
            return ID;
        }

        public DataTable getTablaUsuarios()
        {
            DataTable tabla = ds.ObtenerTabla("Usuarios", TodoUsuarios);
            return tabla;
        }

        public DataTable getTablaUsuariosActualizar(Usuario us) //lo estaba usando para el griw para que los usuarios actualizen sus datos
        {
            DataTable tabla = ds.ObtenerTabla("Usuarios", "select IdUsuario_us,Nombre_us,Contraseña_us,Email_us,Telefono_us from Usuarios where Nombre_us = '"+us.Nombre1+"'");
            return tabla;
        }

        public Boolean ExisteUsuario(Usuario us)
        {
            string consulta = TodoUsuarios + " where Nombre_us = '" + us.Nombre1 +"'";
            return ds.existe(consulta);
        }

        public Boolean ExisteUsuarioModificar(Usuario us)
        {
            string consulta = TodoUsuarios + " where Nombre_us = '" + us.Nombre1 + "' and IdUsuario_us <> " +us.IdUsuario1;
            return ds.existe(consulta);
        }

        public Boolean ExisteEmailUsuario(Usuario us)
        {
            string consulta = TodoUsuarios + " where Email_us = '" + us.Email1 + "'";
            return ds.existe(consulta);
        }

        public Boolean ExisteEmailUsuarioModificar(Usuario us)
        {
            string consulta = TodoUsuarios + " where Email_us = '" + us.Email1 + "' and IdUsuario_us <> "+us.IdUsuario1;
            return ds.existe(consulta);
        }

        public Boolean ExisteUsuarioLogin(Usuario us)
        {
            string consulta = "select IdUsuario_us from Usuarios where Nombre_us = '" + us.Nombre1 +"' and Contraseña_us = '"+ us.Contraseña1 +"'";
            return ds.existe(consulta);
        }

        public bool TipoUsuarioLogin(Usuario us)
        {
            string consulta = "select Tipo_us from Usuarios where Nombre_us = '" + us.Nombre1 + "' and Contraseña_us = '" + us.Contraseña1 + "'"; ;
            return ds.existeLogin(consulta);
        }
        public int eliminarUsuario(ref Usuario us)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametroUsuariosEliminar(ref comando,ref us);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_EliminarUsuariosAdmin");
        }

        public int agregarUsuario(Usuario us)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametroUsuariosAgregar(ref comando,us);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_AgregarUsuario");
        }

        private void ArmarParametroUsuariosEliminar(ref SqlCommand comando, ref Usuario us)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@IdUsuario_us", SqlDbType.BigInt);
            SqlParametros.Value = us.IdUsuario1;
        }

        private void ArmarParametroUsuariosAgregar(ref SqlCommand comando, Usuario us)
        {
            SqlParameter SqlParametros = new SqlParameter();
            /*SqlParametros = comando.Parameters.Add("@Id_Usuario_us", SqlDbType.BigInt);
            SqlParametros.Value = us.IdUsuario1;*/
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
           /* SqlParametros = comando.Parameters.Add("@Estado", SqlDbType.Bit);
            SqlParametros.Value = us.Contraseña1;*/
        }        
    }
}

