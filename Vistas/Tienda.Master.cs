using System;
using System.IO;
using System.Web.UI;

namespace Vistas
{
    public partial class TiendaMaster : MasterPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            // En PreRender, para reflejar lo que la página agregó o quitó del carrito.
            int unidades = Sesion.Carrito.CantidadUnidades;
            lblCantidadCarrito.Text = unidades.ToString();
            lblCantidadCarrito.Visible = unidades > 0;
            base.OnPreRender(e);
        }

        protected string NombreUsuario
        {
            get { return Sesion.Usuario.Nombre; }
        }

        protected bool EsAdmin
        {
            get { return Sesion.EsAdmin; }
        }

        protected string ClaseLink(string pagina)
        {
            bool activa = string.Equals(Path.GetFileNameWithoutExtension(Request.Path), pagina, StringComparison.OrdinalIgnoreCase);
            return activa ? "nav-link active" : "nav-link";
        }
    }
}
