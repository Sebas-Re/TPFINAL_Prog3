using System;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas.AdminForms
{
    public partial class AdminGestionUsuarios : PaginaBase
    {
        GestionUsuarios us = new GestionUsuarios();

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Admin; }
        }

        /* ID buscado; 0 = todos los usuarios. Se conserva entre postbacks. */
        int FiltroId
        {
            get { return (int?)ViewState["FiltroId"] ?? 0; }
            set { ViewState["FiltroId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGvUsuarios();
            }
        }

        private void CargarGvUsuarios()
        {
            gvUsuarios.DataSource = FiltroId == 0 ? us.GetUsuarios() : us.GetTabla(new Usuario { IdUsuario = FiltroId });
            gvUsuarios.DataBind();
        }

        protected void btnBuscarId_Click(object sender, EventArgs e)
        {
            int id;
            FiltroId = int.TryParse(txtBuscarID.Text, out id) ? id : 0;
            gvUsuarios.EditIndex = -1;
            CargarGvUsuarios();
        }

        protected void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            FiltroId = 0;
            txtBuscarID.Text = "";
            gvUsuarios.EditIndex = -1;
            CargarGvUsuarios();
        }

        protected void gvUsuarios_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsuarios.EditIndex = e.NewEditIndex;
            CargarGvUsuarios();
        }

        protected void gvUsuarios_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsuarios.EditIndex = -1;
            CargarGvUsuarios();
        }

        protected void gvUsuarios_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow fila = gvUsuarios.Rows[e.RowIndex];
            Usuario actual = us.get(new Usuario { IdUsuario = Convert.ToInt32(gvUsuarios.DataKeys[e.RowIndex].Value) });
            string nuevaContraseña = ((TextBox)fila.FindControl("txt_eit_passUs")).Text;
            Usuario usuario = new Usuario
            {
                IdUsuario = actual.IdUsuario,
                Nombre = ((TextBox)fila.FindControl("txt_eit_nombreUs")).Text.Trim(),
                Contrasena = nuevaContraseña, // vacía = no cambiarla
                Email = ((TextBox)fila.FindControl("txt_eit_emailUs")).Text.Trim(),
                Telefono = ((TextBox)fila.FindControl("txt_eit_telefonoUs")).Text.Trim(),
                Tipo = Convert.ToBoolean(((DropDownList)fila.FindControl("ddlEditTipo")).SelectedValue),
                Estado = Convert.ToBoolean(((DropDownList)fila.FindControl("ddlEditEstado")).SelectedValue)
            };

            if (!us.VerificarNombreModificar(usuario) || !us.VerificarEmailModificar(usuario))
            {
                MostrarMensaje("El nombre o el e-mail ya los usa otra cuenta.", TipoMensaje.Error);
                return;
            }
            if (!us.ActualizarUsuario(usuario))
            {
                MostrarMensaje(ErrorContraseña(usuario.Contrasena) ?? "No se pudo modificar el usuario.", TipoMensaje.Error);
                return;
            }
            MostrarMensaje("Usuario " + usuario.Nombre + " modificado.", TipoMensaje.Exito);
            gvUsuarios.EditIndex = -1;
            CargarGvUsuarios();
        }

        protected void gvUsuarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Usuario usuario = new Usuario { IdUsuario = Convert.ToInt32(gvUsuarios.DataKeys[e.RowIndex].Value) };
            if (usuario.IdUsuario == Sesion.Usuario.IdUsuario)
            {
                MostrarMensaje("No podés desactivar tu propia cuenta.", TipoMensaje.Aviso);
                return;
            }
            us.EliminarUsuario(ref usuario);
            MostrarMensaje("Usuario desactivado.", TipoMensaje.Exito);
            CargarGvUsuarios();
        }

        protected void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario
            {
                Nombre = txtNombreUsuario.Text.Trim(),
                Contrasena = txtContraseña.Text,
                Email = txtEmail.Text.Trim(),
                Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? "Sin telefono" : txtTelefono.Text.Trim(),
                Tipo = Convert.ToBoolean(ddlTipoUs.SelectedValue)
            };

            if (!us.VerificarNombre(usuario) || !us.VerificarEmail(usuario))
            {
                MostrarMensaje("No se pudo agregar: el nombre o el e-mail ya están en uso.", TipoMensaje.Error);
                return;
            }
            if (!us.AgregarUsuario(usuario))
            {
                MostrarMensaje(ErrorContraseña(usuario.Contrasena) ?? "No se pudo agregar el usuario.", TipoMensaje.Error);
                return;
            }
            MostrarMensaje((usuario.Tipo ? "Administrador " : "Cliente ") + usuario.Nombre + " agregado.", TipoMensaje.Exito);
            txtNombreUsuario.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlTipoUs.SelectedValue = "false";
            CargarGvUsuarios();
        }
    }
}
