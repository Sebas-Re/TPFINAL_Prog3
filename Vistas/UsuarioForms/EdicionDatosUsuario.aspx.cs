using System;
using System.Data;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class EdicionDatosUsuario : PaginaBase
    {
        GestionTarjetas gestarj = new GestionTarjetas();
        GestionUsuarios gestus = new GestionUsuarios();

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Cliente; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosUsuario();
                CargarTarjetas();
            }
        }

        private void CargarDatosUsuario()
        {
            Usuario usuario = gestus.get(new Usuario { IdUsuario1 = Sesion.Usuario.IdUsuario1 });
            txtNombre.Text = usuario.Nombre1;
            txtEmail.Text = usuario.Email1;
            txtTelefono.Text = usuario.Telefono1;
        }

        private void CargarTarjetas()
        {
            DataTable tarjetas = gestarj.getTablaTarjetasUsuario(Sesion.Usuario);
            gvTarjetas.DataSource = tarjetas;
            gvTarjetas.DataBind();
            lblSinTarjetas.Visible = tarjetas.Rows.Count == 0;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Usuario actual = gestus.get(new Usuario { IdUsuario1 = Sesion.Usuario.IdUsuario1 });
            Usuario usuario = new Usuario
            {
                IdUsuario1 = actual.IdUsuario1,
                Nombre1 = txtNombre.Text.Trim(),
                Contraseña1 = txtContraseña.Text, // vacía = no cambiarla
                Email1 = txtEmail.Text.Trim(),
                Telefono1 = txtTelefono.Text.Trim(),
                Tipo1 = actual.Tipo1,
                Estado1 = true
            };

            if (!gestus.VerificarNombreModificar(usuario) || !gestus.VerificarEmailModificar(usuario))
            {
                MostrarMensaje("Ese nombre de usuario o e-mail ya lo usa otra cuenta.", TipoMensaje.Error);
                return;
            }
            if (!gestus.ActualizarUsuario(usuario))
            {
                MostrarMensaje(ErrorContraseña(usuario.Contraseña1) ?? "No se pudieron guardar los cambios.", TipoMensaje.Error);
                return;
            }
            Sesion.Usuario.Nombre1 = usuario.Nombre1;
            MostrarMensaje("Guardamos tus cambios.", TipoMensaje.Exito);
        }

        protected void gvTarjetas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Tarjeta tarjeta = new Tarjeta
            {
                Us = Sesion.Usuario,
                IdTarjeta1 = Convert.ToInt64(gvTarjetas.DataKeys[e.RowIndex].Value)
            };
            if (gestarj.eliminarTarjeta(tarjeta))
            {
                MostrarMensaje("Eliminaste la tarjeta.", TipoMensaje.Exito);
            }
            else
            {
                MostrarMensaje("No se pudo eliminar la tarjeta.", TipoMensaje.Error);
            }
            CargarTarjetas();
        }
    }
}
