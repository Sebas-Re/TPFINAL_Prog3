using System;
using System.Data;
using System.Data.SQLite;
using Entidades;

namespace Dao
{
    public class DaoUsuarios
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();
        const string TodoUsuarios = "select * from Usuarios";

        public Usuario ObtenerUsuario(Usuario usuario)
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Usuarios", TodoUsuarios + " where IdUsuario_us = @IdUsuario", new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
            usuario.IdUsuario = Convert.ToInt32(tabla.Rows[0][0].ToString());
            usuario.Nombre = tabla.Rows[0][1].ToString();
            usuario.Contrasena = tabla.Rows[0][2].ToString();
            usuario.Email = tabla.Rows[0][3].ToString();
            usuario.Telefono = tabla.Rows[0][4].ToString();
            usuario.Tipo = Convert.ToBoolean(tabla.Rows[0][5].ToString());
            usuario.Estado = Convert.ToBoolean(tabla.Rows[0][6].ToString());
            return usuario;
        }

        public DataTable GetTablaUsuario(Usuario usuario)
        {
            String NuevaConsul = TodoUsuarios + " where IdUsuario_us = @IdUsuario";
            DataTable tabla = _accesoDatos.ObtenerTabla("Usuarios", NuevaConsul, new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
            return tabla;
        }

        public int DevolverIDusuario(Usuario usuario)
        {
            String NuevaConsul = "select IdUsuario_us from Usuarios where Nombre_us = @Nombre";
            DataTable tabla = _accesoDatos.ObtenerTabla("Usuarios", NuevaConsul, new SQLiteParameter("@Nombre", usuario.Nombre));
            int ID = (int)Convert.ToInt64(tabla.Rows[0][0]);
            return ID;
        }

        public DataTable GetTablaUsuarios()
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Usuarios", TodoUsuarios);
            return tabla;
        }

        public DataTable GetTablaUsuariosActualizar(Usuario usuario) //lo estaba usando para el griw para que los usuarios actualizen sus datos
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Usuarios", "select IdUsuario_us,Nombre_us,Contraseña_us,Email_us,Telefono_us from Usuarios where Nombre_us = @Nombre",
                new SQLiteParameter("@Nombre", usuario.Nombre));
            return tabla;
        }

        public Boolean ExisteUsuario(Usuario usuario)
        {
            string consulta = TodoUsuarios + " where Nombre_us = @Nombre";
            return _accesoDatos.existe(consulta, new SQLiteParameter("@Nombre", usuario.Nombre));
        }

        public Boolean ExisteUsuarioModificar(Usuario usuario)
        {
            string consulta = TodoUsuarios + " where Nombre_us = @Nombre and IdUsuario_us <> @IdUsuario";
            return _accesoDatos.existe(consulta, new SQLiteParameter("@Nombre", usuario.Nombre), new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
        }

        public Boolean ExisteEmailUsuario(Usuario usuario)
        {
            string consulta = TodoUsuarios + " where Email_us = @Email";
            return _accesoDatos.existe(consulta, new SQLiteParameter("@Email", usuario.Email));
        }

        public Boolean ExisteEmailUsuarioModificar(Usuario usuario)
        {
            string consulta = TodoUsuarios + " where Email_us = @Email and IdUsuario_us <> @IdUsuario";
            return _accesoDatos.existe(consulta, new SQLiteParameter("@Email", usuario.Email), new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
        }

        /* Credenciales de un usuario activo: id, hash de la contraseña y tipo. */
        private DataRow ObtenerCredenciales(Usuario usuario)
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Usuarios", "select IdUsuario_us, Contraseña_us, Tipo_us from Usuarios where Nombre_us = @Nombre and Estado_us = 1",
                new SQLiteParameter("@Nombre", usuario.Nombre));
            return tabla.Rows.Count == 0 ? null : tabla.Rows[0];
        }

        /* Las contraseñas se guardan hasheadas: se compara en código, no en el where. */
        public Boolean ExisteUsuarioLogin(Usuario usuario)
        {
            DataRow fila = ObtenerCredenciales(usuario);
            if (fila == null)
            {
                Contrasenas.SimularVerificacion(usuario.Contrasena);
                return false;
            }
            return Contrasenas.Verificar(usuario.Contrasena, fila["Contraseña_us"].ToString());
        }

        /* ¿Es la contraseña actual del usuario (activo) con ese id? */
        public bool VerificarContraseña(int idUsuario, string contraseña)
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Usuarios", "select Contraseña_us from Usuarios where IdUsuario_us = @IdUsuario and Estado_us = 1",
                new SQLiteParameter("@IdUsuario", idUsuario));
            return tabla.Rows.Count == 1 && Contrasenas.Verificar(contraseña, tabla.Rows[0]["Contraseña_us"].ToString());
        }

        public bool TipoUsuarioLogin(Usuario usuario)
        {
            DataRow fila = ObtenerCredenciales(usuario);
            return fila != null && Contrasenas.Verificar(usuario.Contrasena, fila["Contraseña_us"].ToString()) && (bool)fila["Tipo_us"];
        }

        public int EliminarUsuario(ref Usuario usuario)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroUsuariosEliminar(ref comando, ref usuario);
            // ex sp_EliminarUsuariosAdmin
            return _accesoDatos.EjecutarComando(comando, "update Usuarios set Estado_us = 0 where IdUsuario_us = @IdUsuario_us");
        }

        public int AgregarUsuario(Usuario usuario)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroUsuariosAgregar(ref comando, usuario);
            // ex sp_AgregarUsuario
            return _accesoDatos.EjecutarComando(comando,
                "insert into Usuarios(Nombre_us,Contraseña_us,Email_us,Telefono_us,Tipo_us,Estado_us,FechaAlta_us) values (@NOMBRE_US,@CONTRASENA_US,@EMAIL_US,@TELEFONO_US,@TIPO_US,1,datetime('now','localtime'))");
        }

        public int ActualizarUsuario(Usuario usuario)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroUsuariosAgregar(ref comando, usuario);
            ArmarParametroUsuariosModificar(ref comando, usuario);
            // ex sp_ModificarUsuario
            return _accesoDatos.EjecutarComando(comando,
                "update Usuarios set Nombre_us = @NOMBRE_US, Contraseña_us = coalesce(@CONTRASENA_US, Contraseña_us), Email_us = @EMAIL_US, Telefono_us = @TELEFONO_US, Tipo_us = @TIPO_US, Estado_us = @Estado_us where IdUsuario_us = @Id_Usuario_us");
        }

        private void ArmarParametroUsuariosEliminar(ref SQLiteCommand comando, ref Usuario usuario)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IdUsuario_us", DbType.Int64);
            Parametros.Value = usuario.IdUsuario;
        }

        private void ArmarParametroUsuariosAgregar(ref SQLiteCommand comando, Usuario usuario)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@NOMBRE_US", DbType.String);
            Parametros.Value = usuario.Nombre;
            // Vacía = no cambiarla (al modificar); se guarda solo el hash.
            Parametros = comando.Parameters.Add("@CONTRASENA_US", DbType.String);
            Parametros.Value = string.IsNullOrEmpty(usuario.Contrasena) ? (object)DBNull.Value : Contrasenas.Hashear(usuario.Contrasena);
            Parametros = comando.Parameters.Add("@EMAIL_US", DbType.String);
            Parametros.Value = usuario.Email;
            Parametros = comando.Parameters.Add("@TELEFONO_US", DbType.String);
            Parametros.Value = usuario.Telefono;
            Parametros = comando.Parameters.Add("@TIPO_US", DbType.Boolean);
            Parametros.Value = usuario.Tipo;
        }

        private void ArmarParametroUsuariosModificar(ref SQLiteCommand comando, Usuario usuario)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@Id_Usuario_us", DbType.Int64);
            Parametros.Value = usuario.IdUsuario;
            Parametros = comando.Parameters.Add("@Estado_us", DbType.Boolean);
            Parametros.Value = usuario.Estado;
        }
    }
}
