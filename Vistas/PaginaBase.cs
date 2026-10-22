using System;
using System.Web.UI;

namespace Vistas
{
    public enum NivelAcceso
    {
        Publico,
        Cliente,
        Admin
    }

    /* Base de todas las páginas: controla el acceso según la sesión y permite
       mostrar mensajes dentro de la página (en lugar de carteles). */
    public abstract class PaginaBase : Page
    {
        protected abstract NivelAcceso Acceso { get; }

        protected override void OnInit(EventArgs e)
        {
            if (Acceso != NivelAcceso.Publico && !Sesion.Logueado)
            {
                RedirigirConMensaje("~/Inicio.aspx", "Iniciá sesión para continuar.", TipoMensaje.Info);
            }
            if (Acceso == NivelAcceso.Admin && !Sesion.EsAdmin)
            {
                Redirigir("~/UsuarioForms/Catalogo.aspx");
            }
            base.OnInit(e);
        }

        protected void MostrarMensaje(string texto, TipoMensaje tipo)
        {
            SiteMaster master = BuscarSiteMaster();
            if (master != null)
            {
                master.AgregarMensaje(new Mensaje(texto, tipo));
            }
        }

        /* Guarda el mensaje para mostrarlo en la página de destino. */
        protected void RedirigirConMensaje(string url, string texto, TipoMensaje tipo)
        {
            Sesion.GuardarMensaje(texto, tipo);
            Redirigir(url);
        }

        /* Corta la ejecución de la página (no llamar dentro de un try/catch). */
        protected void Redirigir(string url)
        {
            Response.Redirect(url, true);
        }

        private SiteMaster BuscarSiteMaster()
        {
            MasterPage master = Master;
            while (master != null && !(master is SiteMaster))
            {
                master = master.Master;
            }
            return master as SiteMaster;
        }
    }
}
