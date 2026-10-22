using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vistas
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Mensaje dejado por la página anterior antes de un Response.Redirect.
            Mensaje pendiente = Sesion.TomarMensaje();
            if (pendiente != null)
            {
                AgregarMensaje(pendiente);
            }
        }

        public void AgregarMensaje(Mensaje mensaje)
        {
            string html = string.Format(
                "<div class=\"alert alert-{0} alert-dismissible fade show d-flex align-items-center gap-2\" role=\"alert\">" +
                "<i class=\"bi {1}\"></i><div>{2}</div>" +
                "<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Cerrar\"></button></div>",
                mensaje.ClaseBootstrap, mensaje.Icono, HttpUtility.HtmlEncode(mensaje.Texto));
            phMensajes.Controls.Add(new Literal { Text = html });
        }
    }
}
