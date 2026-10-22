using System;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class RegistroUsuario : PaginaBase
    {
        GestionUsuarios us = new GestionUsuarios();

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Publico; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario
            {
                Nombre1 = txtUsuario.Text.Trim(),
                Contraseña1 = txtContra1.Text,
                Email1 = txtEmail.Text.Trim(),
                Telefono1 = string.IsNullOrWhiteSpace(txtTelefono.Text) ? "Sin telefono" : txtTelefono.Text.Trim(),
                Tipo1 = false
            };

            if (!us.VerificarNombre(usuario))
            {
                MostrarMensaje("Ese nombre de usuario ya está en uso. Probá con otro.", TipoMensaje.Error);
                return;
            }
            if (!us.VerificarEmail(usuario))
            {
                MostrarMensaje("Ya existe una cuenta con ese e-mail.", TipoMensaje.Error);
                return;
            }
            if (!us.agregarUsuario(usuario))
            {
                MostrarMensaje("No se pudo crear la cuenta. Intentá nuevamente.", TipoMensaje.Error);
                return;
            }

            Sesion.Iniciar(us.Login(usuario.Nombre1, usuario.Contraseña1));
            RedirigirConMensaje("~/UsuarioForms/Catalogo.aspx", "¡Bienvenido/a, " + usuario.Nombre1 + "! Tu cuenta ya está lista.", TipoMensaje.Exito);
        }
    }
}
