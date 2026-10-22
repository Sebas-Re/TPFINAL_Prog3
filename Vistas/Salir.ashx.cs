using System.Web;
using System.Web.SessionState;

namespace Vistas
{
    /* Cierra la sesión y vuelve al inicio. */
    public class Salir : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            Sesion.Cerrar();
            context.Response.Redirect("~/Inicio.aspx", false);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
