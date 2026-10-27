using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Dao;
using Entidades;

namespace Negocio
{
    public class GestionUsuarios
    {
        private readonly DaoUsuarios _daoUsuarios = new DaoUsuarios();


        public const int LargoMinimoContraseña = 6;
        public const int LargoMaximoContraseña = 64;

        public static bool ContraseñaValida(string contraseña)
        {
            return contraseña != null && contraseña.Length >= LargoMinimoContraseña && contraseña.Length <= LargoMaximoContraseña;
        }

        public DataTable GetUsuarios() //devuelve todos los usarios
        {
            return _daoUsuarios.GetTablaUsuarios();
        }

        public DataTable GetTabla(Usuario usuario) //Devuelve un usuario
        {
            return _daoUsuarios.GetTablaUsuario(usuario);
        }

        public bool VerificarLogin(Usuario usuario)
        {
            if (_daoUsuarios.ExisteUsuarioLogin(usuario) == false)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        /* Freno a la fuerza bruta: después de MaxIntentosFallidos seguidos, el
           usuario queda bloqueado por DuracionBloqueo. Vive en memoria, así que
           se reinicia con la aplicación. */
        public const int MaxIntentosFallidos = 5;
        public static TimeSpan DuracionBloqueo { get; set; } = TimeSpan.FromMinutes(5);

        private class Intentos
        {
            public int Fallidos;
            public DateTime Ultimo;
        }

        private static readonly ConcurrentDictionary<string, Intentos> s_intentosFallidos =
            new ConcurrentDictionary<string, Intentos>(StringComparer.OrdinalIgnoreCase);

        public bool LoginBloqueado(string nombre)
        {
            Intentos intentos;
            if (nombre == null || !s_intentosFallidos.TryGetValue(nombre, out intentos))
            {
                return false;
            }
            lock (intentos)
            {
                return intentos.Fallidos >= MaxIntentosFallidos && DateTime.UtcNow - intentos.Ultimo < DuracionBloqueo;
            }
        }

        private static void RegistrarFallo(string nombre)
        {
            // Que nombres inventados no hagan crecer el diccionario sin límite.
            if (s_intentosFallidos.Count > 10000)
            {
                foreach (KeyValuePair<string, Intentos> par in s_intentosFallidos)
                {
                    if (DateTime.UtcNow - par.Value.Ultimo >= DuracionBloqueo)
                    {
                        Intentos descartado;
                        s_intentosFallidos.TryRemove(par.Key, out descartado);
                    }
                }
            }
            Intentos intentos = s_intentosFallidos.GetOrAdd(nombre, _ => new Intentos());
            lock (intentos)
            {
                // Pasado el bloqueo se vuelve a empezar la cuenta.
                if (DateTime.UtcNow - intentos.Ultimo >= DuracionBloqueo)
                {
                    intentos.Fallidos = 0;
                }
                intentos.Fallidos++;
                intentos.Ultimo = DateTime.UtcNow;
            }
        }

        /* Devuelve el usuario con su id y tipo si las credenciales son válidas,
           o null si no lo son o si el usuario está bloqueado por intentos fallidos. */
        public Usuario Login(string nombre, string contraseña)
        {
            nombre = nombre ?? "";
            if (LoginBloqueado(nombre))
            {
                return null;
            }
            Usuario credenciales = new Usuario { Nombre = nombre, Contrasena = contraseña };
            if (!VerificarLogin(credenciales))
            {
                RegistrarFallo(nombre);
                return null;
            }
            Intentos anteriores;
            s_intentosFallidos.TryRemove(nombre, out anteriores);
            credenciales.IdUsuario = DevolverIdUsuario(credenciales);
            credenciales.Tipo = VerificarTipoUsuario(credenciales);
            return credenciales;
        }

        public bool VerificarTipoUsuario(Usuario usuario)
        {
            bool tipo = _daoUsuarios.TipoUsuarioLogin(usuario);
            return tipo;
        }

        public int DevolverIdUsuario(Usuario usuario)
        {
            int id = _daoUsuarios.DevolverIdUsuario(usuario);
            return id;
        }

        public Usuario Obtener(Usuario usuario)
        {
            return _daoUsuarios.ObtenerUsuario(usuario);
        }

        public bool EliminarUsuario(Usuario usuario)
        {
            int op = _daoUsuarios.EliminarUsuario(usuario);
            return op == 1;
        }

        public bool AgregarUsuario(Usuario usuario)
        {
            int cantFilas = 0;
            if (!ContraseñaValida(usuario.Contrasena))
            {
                return false;
            }
            if (_daoUsuarios.ExisteUsuario(usuario) == false)
            {
                cantFilas = _daoUsuarios.AgregarUsuario(usuario);
            }
            else
            {
                return false;
            }

            return cantFilas == 1;
        }

        public bool VerificarNombre(Usuario usuario)
        {

            return _daoUsuarios.ExisteUsuario(usuario) == false;
        }

        public bool VerificarNombreModificar(Usuario usuario)
        {

            return _daoUsuarios.ExisteUsuarioModificar(usuario) == false;
        }

        public bool VerificarEmail(Usuario usuario)
        {

            return _daoUsuarios.ExisteEmailUsuario(usuario) == false;
        }

        public bool VerificarEmailModificar(Usuario usuario)
        {

            return _daoUsuarios.ExisteEmailUsuarioModificar(usuario) == false;
        }


        /* El usuario edita su propia cuenta. Cambiar nombre, e-mail o contraseña
           exige la contraseña actual: si alguien usa una sesión ajena abierta, no
           puede quedarse con la cuenta. Los errores cuentan para el mismo bloqueo
           que el login. Tipo y estado se conservan (un cliente no puede cambiarlos).
           "cambios.Contrasena" vacía = no cambiarla. Devuelve null si se guardó, o
           el motivo por el que no. */
        public string ActualizarDatosPropios(Usuario cambios, string contraseñaActual)
        {
            Usuario actual = _daoUsuarios.ObtenerUsuario(new Usuario { IdUsuario = cambios.IdUsuario });
            bool cambiaCredenciales = cambios.Nombre != actual.Nombre
                || !string.Equals(cambios.Email, actual.Email, StringComparison.OrdinalIgnoreCase)
                || !string.IsNullOrEmpty(cambios.Contrasena);

            if (cambiaCredenciales)
            {
                if (LoginBloqueado(actual.Nombre))
                {
                    return "Demasiados intentos fallidos. Esperá unos minutos y volvé a probar.";
                }
                if (string.IsNullOrEmpty(contraseñaActual) || !_daoUsuarios.VerificarContraseña(actual.IdUsuario, contraseñaActual))
                {
                    RegistrarFallo(actual.Nombre);
                    return "Para cambiar el nombre, el e-mail o la contraseña, ingresá tu contraseña actual correcta.";
                }
                Intentos anteriores;
                s_intentosFallidos.TryRemove(actual.Nombre, out anteriores);
            }

            cambios.Tipo = actual.Tipo;
            cambios.Estado = actual.Estado;
            if (!VerificarNombreModificar(cambios) || !VerificarEmailModificar(cambios))
            {
                return "Ese nombre de usuario o e-mail ya lo usa otra cuenta.";
            }
            if (!string.IsNullOrEmpty(cambios.Contrasena) && !ContraseñaValida(cambios.Contrasena))
            {
                return string.Format("La contraseña tiene que tener entre {0} y {1} caracteres.", LargoMinimoContraseña, LargoMaximoContraseña);
            }
            return ActualizarUsuario(cambios) ? null : "No se pudieron guardar los cambios.";
        }

        /* Contrasena vacía o null = conservar la actual. */
        public bool ActualizarUsuario(Usuario usuario)
        {
            if (!string.IsNullOrEmpty(usuario.Contrasena) && !ContraseñaValida(usuario.Contrasena))
            {
                return false;
            }
            int filasActualizadas = _daoUsuarios.ActualizarUsuario(usuario);
            return filasActualizadas == 1;
        }


    }
}
