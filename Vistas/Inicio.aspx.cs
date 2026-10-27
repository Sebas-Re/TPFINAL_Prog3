using System;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class Inicio : PaginaBase
    {
        private readonly GestionUsuarios _gestionUsuarios = new GestionUsuarios();

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Publico; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Sesion.Logueado)
            {
                IrAlInicioDelUsuario();
            }
        }

        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string nombre = txtUsuario.Text.Trim();
            Usuario usuario = _gestionUsuarios.Login(nombre, txtContraseña.Text);
            if (usuario == null && _gestionUsuarios.LoginBloqueado(nombre))
            {
                MostrarMensaje("Demasiados intentos fallidos. Esperá unos minutos y volvé a probar.", TipoMensaje.Error);
                return;
            }
            if (usuario == null)
            {
                MostrarMensaje("Usuario o contraseña incorrectos. Si todavía no tenés cuenta, registrate.", TipoMensaje.Error);
                return;
            }
            Sesion.Iniciar(usuario);
            IrAlInicioDelUsuario();
        }

        private void IrAlInicioDelUsuario()
        {
            Redirigir(Sesion.EsAdmin ? "~/AdminForms/AdminHome.aspx" : "~/UsuarioForms/Catalogo.aspx");
        }
    }
}
