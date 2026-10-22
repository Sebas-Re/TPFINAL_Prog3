using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using Entidades;

namespace Dao
{
    public class DaoUsuarios
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoUsuarios = "select * from Usuarios";

        public Usuario ObtenerUsuario(Usuario us)
        {
            DataTable tabla = ds.ObtenerTabla("Usuarios", TodoUsuarios + " where IdUsuario_us = @IdUsuario", new SQLiteParameter("@IdUsuario", us.IdUsuario1));
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
            String NuevaConsul = TodoUsuarios + " where IdUsuario_us = @IdUsuario";
            DataTable tabla = ds.ObtenerTabla("Usuarios", NuevaConsul, new SQLiteParameter("@IdUsuario", us.IdUsuario1));
            return tabla;
        }

        public int DevolverIDusuario(Usuario us)
        {
            String NuevaConsul = "select IdUsuario_us from Usuarios where Nombre_us = @Nombre";
            DataTable tabla = ds.ObtenerTabla("Usuarios", NuevaConsul, new SQLiteParameter("@Nombre", us.Nombre1));
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
            DataTable tabla = ds.ObtenerTabla("Usuarios", "select IdUsuario_us,Nombre_us,Contraseña_us,Email_us,Telefono_us from Usuarios where Nombre_us = @Nombre",
                new SQLiteParameter("@Nombre", us.Nombre1));
            return tabla;
        }

        public Boolean ExisteUsuario(Usuario us)
        {
            string consulta = TodoUsuarios + " where Nombre_us = @Nombre";
            return ds.existe(consulta, new SQLiteParameter("@Nombre", us.Nombre1));
        }

        public Boolean ExisteUsuarioModificar(Usuario us)
        {
            string consulta = TodoUsuarios + " where Nombre_us = @Nombre and IdUsuario_us <> @IdUsuario";
            return ds.existe(consulta, new SQLiteParameter("@Nombre", us.Nombre1), new SQLiteParameter("@IdUsuario", us.IdUsuario1));
        }

        public Boolean ExisteEmailUsuario(Usuario us)
        {
            string consulta = TodoUsuarios + " where Email_us = @Email";
            return ds.existe(consulta, new SQLiteParameter("@Email", us.Email1));
        }

        public Boolean ExisteEmailUsuarioModificar(Usuario us)
        {
            string consulta = TodoUsuarios + " where Email_us = @Email and IdUsuario_us <> @IdUsuario";
            return ds.existe(consulta, new SQLiteParameter("@Email", us.Email1), new SQLiteParameter("@IdUsuario", us.IdUsuario1));
        }

        /* Credenciales de un usuario activo: id, hash de la contraseña y tipo. */
        private DataRow ObtenerCredenciales(Usuario us)
        {
            DataTable tabla = ds.ObtenerTabla("Usuarios", "select IdUsuario_us, Contraseña_us, Tipo_us from Usuarios where Nombre_us = @Nombre and Estado_us = 1",
                new SQLiteParameter("@Nombre", us.Nombre1));
            return tabla.Rows.Count == 0 ? null : tabla.Rows[0];
        }

        /* Las contraseñas se guardan hasheadas: se compara en código, no en el where. */
        public Boolean ExisteUsuarioLogin(Usuario us)
        {
            DataRow fila = ObtenerCredenciales(us);
            if (fila == null)
            {
                Contrasenas.SimularVerificacion(us.Contraseña1);
                return false;
            }
            return Contrasenas.Verificar(us.Contraseña1, fila["Contraseña_us"].ToString());
        }

        public bool TipoUsuarioLogin(Usuario us)
        {
            DataRow fila = ObtenerCredenciales(us);
            return fila != null && Contrasenas.Verificar(us.Contraseña1, fila["Contraseña_us"].ToString()) && (bool)fila["Tipo_us"];
        }

        public int eliminarUsuario(ref Usuario us)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroUsuariosEliminar(ref comando,ref us);
            // ex sp_EliminarUsuariosAdmin
            return ds.EjecutarComando(comando, "update Usuarios set Estado_us = 0 where IdUsuario_us = @IdUsuario_us");
        }

        public int agregarUsuario(Usuario us)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroUsuariosAgregar(ref comando,us);
            // ex sp_AgregarUsuario
            return ds.EjecutarComando(comando,
                "insert into Usuarios(Nombre_us,Contraseña_us,Email_us,Telefono_us,Tipo_us,Estado_us,FechaAlta_us) values (@NOMBRE_US,@CONTRASENA_US,@EMAIL_US,@TELEFONO_US,@TIPO_US,1,datetime('now','localtime'))");
        }

        public int actualizarUsuario(Usuario us)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroUsuariosAgregar(ref comando, us);
            ArmarParametroUsuariosModificar(ref comando, us);
            // ex sp_ModificarUsuario
            return ds.EjecutarComando(comando,
                "update Usuarios set Nombre_us = @NOMBRE_US, Contraseña_us = coalesce(@CONTRASENA_US, Contraseña_us), Email_us = @EMAIL_US, Telefono_us = @TELEFONO_US, Tipo_us = @TIPO_US, Estado_us = @Estado_us where IdUsuario_us = @Id_Usuario_us");
        }

        private void ArmarParametroUsuariosEliminar(ref SQLiteCommand comando, ref Usuario us)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IdUsuario_us", DbType.Int64);
            Parametros.Value = us.IdUsuario1;
        }

        private void ArmarParametroUsuariosAgregar(ref SQLiteCommand comando, Usuario us)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@NOMBRE_US", DbType.String);
            Parametros.Value = us.Nombre1;
            // Vacía = no cambiarla (al modificar); se guarda solo el hash.
            Parametros = comando.Parameters.Add("@CONTRASENA_US", DbType.String);
            Parametros.Value = string.IsNullOrEmpty(us.Contraseña1) ? (object)DBNull.Value : Contrasenas.Hashear(us.Contraseña1);
            Parametros = comando.Parameters.Add("@EMAIL_US", DbType.String);
            Parametros.Value = us.Email1;
            Parametros = comando.Parameters.Add("@TELEFONO_US", DbType.String);
            Parametros.Value = us.Telefono1;
            Parametros = comando.Parameters.Add("@TIPO_US", DbType.Boolean);
            Parametros.Value = us.Tipo1;
        }

        private void ArmarParametroUsuariosModificar(ref SQLiteCommand comando, Usuario us)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@Id_Usuario_us", DbType.Int64);
            Parametros.Value = us.IdUsuario1;
            Parametros = comando.Parameters.Add("@Estado_us", DbType.Boolean);
            Parametros.Value = us.Estado1;
        }
    }
}
