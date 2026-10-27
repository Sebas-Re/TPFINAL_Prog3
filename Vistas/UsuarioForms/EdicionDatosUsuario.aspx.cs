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
            Usuario usuario = gestus.get(new Usuario { IdUsuario = Sesion.Usuario.IdUsuario });
            txtNombre.Text = usuario.Nombre;
            txtEmail.Text = usuario.Email;
            txtTelefono.Text = usuario.Telefono;
        }

        private void CargarTarjetas()
        {
            DataTable tarjetas = gestarj.GetTablaTarjetasUsuario(Sesion.Usuario);
            gvTarjetas.DataSource = tarjetas;
            gvTarjetas.DataBind();
            lblSinTarjetas.Visible = tarjetas.Rows.Count == 0;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario
            {
                IdUsuario = Sesion.Usuario.IdUsuario,
                Nombre = txtNombre.Text.Trim(),
                Contrasena = txtContraseña.Text, // vacía = no cambiarla
                Email = txtEmail.Text.Trim(),
                Telefono = txtTelefono.Text.Trim()
            };

            string error = gestus.ActualizarDatosPropios(usuario, txtContraseñaActual.Text);
            if (error != null)
            {
                MostrarMensaje(error, TipoMensaje.Error);
                return;
            }
            Sesion.Usuario.Nombre = usuario.Nombre;
            MostrarMensaje("Guardamos tus cambios.", TipoMensaje.Exito);
        }

        protected void gvTarjetas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Tarjeta tarjeta = new Tarjeta
            {
                Us = Sesion.Usuario,
                IdTarjeta = Convert.ToInt64(gvTarjetas.DataKeys[e.RowIndex].Value)
            };
            if (gestarj.EliminarTarjeta(tarjeta))
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
