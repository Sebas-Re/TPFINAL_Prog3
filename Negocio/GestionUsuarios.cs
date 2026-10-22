using System;
using System.Collections.Concurrent;
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

        public const int LargoMinimoContraseña = 6;
        public const int LargoMaximoContraseña = 64;

        public static bool ContraseñaValida(string contraseña)
        {
            return contraseña != null && contraseña.Length >= LargoMinimoContraseña && contraseña.Length <= LargoMaximoContraseña;
        }

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

        /* Freno a la fuerza bruta: después de MaxIntentosFallidos seguidos, el
           usuario queda bloqueado por DuracionBloqueo. Vive en memoria, así que
           se reinicia con la aplicación. */
        public const int MaxIntentosFallidos = 5;
        public static TimeSpan DuracionBloqueo { get; set; } = TimeSpan.FromMinutes(5);

        class Intentos
        {
            public int Fallidos;
            public DateTime Ultimo;
        }

        static readonly ConcurrentDictionary<string, Intentos> intentosFallidos =
            new ConcurrentDictionary<string, Intentos>(StringComparer.OrdinalIgnoreCase);

        public bool LoginBloqueado(string nombre)
        {
            Intentos intentos;
            if (nombre == null || !intentosFallidos.TryGetValue(nombre, out intentos))
            {
                return false;
            }
            lock (intentos)
            {
                return intentos.Fallidos >= MaxIntentosFallidos && DateTime.UtcNow - intentos.Ultimo < DuracionBloqueo;
            }
        }

        static void RegistrarFallo(string nombre)
        {
            // Que nombres inventados no hagan crecer el diccionario sin límite.
            if (intentosFallidos.Count > 10000)
            {
                foreach (var par in intentosFallidos)
                {
                    if (DateTime.UtcNow - par.Value.Ultimo >= DuracionBloqueo)
                    {
                        Intentos descartado;
                        intentosFallidos.TryRemove(par.Key, out descartado);
                    }
                }
            }
            Intentos intentos = intentosFallidos.GetOrAdd(nombre, _ => new Intentos());
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
            Usuario credenciales = new Usuario { Nombre1 = nombre, Contraseña1 = contraseña };
            if (!VerificarLogin(credenciales))
            {
                RegistrarFallo(nombre);
                return null;
            }
            Intentos anteriores;
            intentosFallidos.TryRemove(nombre, out anteriores);
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
            if (!ContraseñaValida(us.Contraseña1))
            {
                return false;
            }
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

       
        /* El usuario edita su propia cuenta. Cambiar nombre, e-mail o contraseña
           exige la contraseña actual: si alguien usa una sesión ajena abierta, no
           puede quedarse con la cuenta. Los errores cuentan para el mismo bloqueo
           que el login. Tipo y estado se conservan (un cliente no puede cambiarlos).
           "cambios.Contraseña1" vacía = no cambiarla. Devuelve null si se guardó, o
           el motivo por el que no. */
        public string ActualizarDatosPropios(Usuario cambios, string contraseñaActual)
        {
            Usuario actual = dao.ObtenerUsuario(new Usuario { IdUsuario1 = cambios.IdUsuario1 });
            bool cambiaCredenciales = cambios.Nombre1 != actual.Nombre1
                || !string.Equals(cambios.Email1, actual.Email1, StringComparison.OrdinalIgnoreCase)
                || !string.IsNullOrEmpty(cambios.Contraseña1);

            if (cambiaCredenciales)
            {
                if (LoginBloqueado(actual.Nombre1))
                {
                    return "Demasiados intentos fallidos. Esperá unos minutos y volvé a probar.";
                }
                if (string.IsNullOrEmpty(contraseñaActual) || !dao.VerificarContraseña(actual.IdUsuario1, contraseñaActual))
                {
                    RegistrarFallo(actual.Nombre1);
                    return "Para cambiar el nombre, el e-mail o la contraseña, ingresá tu contraseña actual correcta.";
                }
                Intentos anteriores;
                intentosFallidos.TryRemove(actual.Nombre1, out anteriores);
            }

            cambios.Tipo1 = actual.Tipo1;
            cambios.Estado1 = actual.Estado1;
            if (!VerificarNombreModificar(cambios) || !VerificarEmailModificar(cambios))
            {
                return "Ese nombre de usuario o e-mail ya lo usa otra cuenta.";
            }
            if (!string.IsNullOrEmpty(cambios.Contraseña1) && !ContraseñaValida(cambios.Contraseña1))
            {
                return string.Format("La contraseña tiene que tener entre {0} y {1} caracteres.", LargoMinimoContraseña, LargoMaximoContraseña);
            }
            return ActualizarUsuario(cambios) ? null : "No se pudieron guardar los cambios.";
        }

        /* Contraseña1 vacía o null = conservar la actual. */
        public bool ActualizarUsuario(Usuario Usuario)
        {
            if (!string.IsNullOrEmpty(Usuario.Contraseña1) && !ContraseñaValida(Usuario.Contraseña1))
            {
                return false;
            }
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
