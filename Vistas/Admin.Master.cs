using System;
using System.IO;
using System.Web.UI;

namespace Vistas
{
    public partial class AdminMaster : MasterPage
    {
        protected string NombreUsuario
        {
            get { return Sesion.Usuario.Nombre; }
        }

        protected string ClaseLink(string pagina)
        {
            bool activa = string.Equals(Path.GetFileNameWithoutExtension(Request.Path), pagina, StringComparison.OrdinalIgnoreCase);
            return activa ? "nav-link active" : "nav-link";
        }
    }
}
