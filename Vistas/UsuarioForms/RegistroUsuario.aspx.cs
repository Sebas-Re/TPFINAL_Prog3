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
                Nombre = txtUsuario.Text.Trim(),
                Contrasena = txtContra1.Text,
                Email = txtEmail.Text.Trim(),
                Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? "Sin telefono" : txtTelefono.Text.Trim(),
                Tipo = false
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
            if (!us.AgregarUsuario(usuario))
            {
                MostrarMensaje(ErrorContraseña(usuario.Contrasena) ?? "No se pudo crear la cuenta. Intentá nuevamente.", TipoMensaje.Error);
                return;
            }

            Sesion.Iniciar(us.Login(usuario.Nombre, usuario.Contrasena));
            RedirigirConMensaje("~/UsuarioForms/Catalogo.aspx", "¡Bienvenido/a, " + usuario.Nombre + "! Tu cuenta ya está lista.", TipoMensaje.Exito);
        }
    }
}
