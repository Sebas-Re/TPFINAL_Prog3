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

        static HttpSessionState Actual
        {
            get { return HttpContext.Current.Session; }
        }

        public static Usuario Usuario
        {
            get { return Actual[ClaveUsuario] as Usuario; }
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
            Actual[ClaveUsuario] = usuario;
        }

        public static void Cerrar()
        {
            Actual.Clear();
            Actual.Abandon();
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
