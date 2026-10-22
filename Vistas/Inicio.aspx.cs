using System;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class Inicio : PaginaBase
    {
        GestionUsuarios us = new GestionUsuarios();

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
            Usuario usuario = us.Login(txtUsuario.Text.Trim(), txtContraseña.Text);
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
