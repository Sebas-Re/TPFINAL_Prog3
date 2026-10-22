using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.SessionState;
using Entidades;

namespace Vistas
{
    /* Acceso tipado a lo que la app guarda en la sesión: el usuario logueado,
       su carrito y un mensaje "flash" que sobrevive a un Response.Redirect. */
    public static class Sesion
    {
        const string ClaveUsuario = "Usuario";
        const string ClaveCarrito = "Carrito";
        const string ClaveMensaje = "MensajeFlash";
        const string ClaveToken = "TokenAutenticacion";
        const string CookieToken = "PCBooster.Auth";

        static HttpSessionState Actual
        {
            get { return HttpContext.Current.Session; }
        }

        /* El usuario cuenta como logueado solo si el navegador presenta la cookie
           con el token que se generó al iniciar sesión. Así, conocer el id de
           sesión (por ejemplo, uno fijado por un atacante antes del login) no
           alcanza para usar la sesión ya autenticada. */
        public static Usuario Usuario
        {
            get
            {
                string token = Token;
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieToken];
                if (token == null || cookie == null || !IgualesEnTiempoConstante(cookie.Value, token))
                {
                    return null;
                }
                return Actual[ClaveUsuario] as Usuario;
            }
        }

        /* Se usa también como ViewStateUserKey, para atar los formularios al login. */
        public static string Token
        {
            get { return Actual[ClaveToken] as string; }
        }

        public static bool Logueado
        {
            get { return Usuario != null; }
        }

        public static bool EsAdmin
        {
            get { return Logueado && Usuario.Tipo1; }
        }

        public static Entidades.Carrito Carrito
        {
            get
            {
                Entidades.Carrito carrito = Actual[ClaveCarrito] as Entidades.Carrito;
                if (carrito == null)
                {
                    carrito = new Entidades.Carrito();
                    Actual[ClaveCarrito] = carrito;
                }
                return carrito;
            }
        }

        public static void Iniciar(Usuario usuario)
        {
            Actual.Clear();
            byte[] bytes = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            string token = Convert.ToBase64String(bytes);
            Actual[ClaveUsuario] = usuario;
            Actual[ClaveToken] = token;
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(CookieToken, token)
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = HttpContext.Current.Request.IsSecureConnection
            });
        }

        public static void Cerrar()
        {
            Actual.Clear();
            Actual.Abandon();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(CookieToken, "")
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(-1)
            });
        }

        static bool IgualesEnTiempoConstante(string a, string b)
        {
            int diferencia = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diferencia |= a[i] ^ b[i];
            }
            return diferencia == 0;
        }

        public static void GuardarMensaje(string texto, TipoMensaje tipo)
        {
            Actual[ClaveMensaje] = new Mensaje(texto, tipo);
        }

        public static Mensaje TomarMensaje()
        {
            Mensaje mensaje = Actual[ClaveMensaje] as Mensaje;
            Actual.Remove(ClaveMensaje);
            return mensaje;
        }
    }

    public enum TipoMensaje
    {
        Exito,
        Error,
        Aviso,
        Info
    }

    public class Mensaje
    {
        public Mensaje(string texto, TipoMensaje tipo)
        {
            Texto = texto;
            Tipo = tipo;
        }

        public string Texto { get; private set; }
        public TipoMensaje Tipo { get; private set; }

        public string ClaseBootstrap
        {
            get
            {
                switch (Tipo)
                {
                    case TipoMensaje.Exito: return "success";
                    case TipoMensaje.Error: return "danger";
                    case TipoMensaje.Aviso: return "warning";
                    default: return "info";
                }
            }
        }

        public string Icono
        {
            get
            {
                switch (Tipo)
                {
                    case TipoMensaje.Exito: return "bi-check-circle-fill";
                    case TipoMensaje.Error: return "bi-x-circle-fill";
                    case TipoMensaje.Aviso: return "bi-exclamation-triangle-fill";
                    default: return "bi-info-circle-fill";
                }
            }
        }
    }
}
