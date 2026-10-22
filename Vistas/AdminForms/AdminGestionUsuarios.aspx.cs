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
            gvUsuarios.DataSource = FiltroId == 0 ? us.getUsuarios() : us.getTabla(new Usuario { IdUsuario1 = FiltroId });
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
            Usuario actual = us.get(new Usuario { IdUsuario1 = Convert.ToInt32(gvUsuarios.DataKeys[e.RowIndex].Value) });
            string nuevaContraseña = ((TextBox)fila.FindControl("txt_eit_passUs")).Text;
            Usuario usuario = new Usuario
            {
                IdUsuario1 = actual.IdUsuario1,
                Nombre1 = ((TextBox)fila.FindControl("txt_eit_nombreUs")).Text.Trim(),
                Contraseña1 = nuevaContraseña, // vacía = no cambiarla
                Email1 = ((TextBox)fila.FindControl("txt_eit_emailUs")).Text.Trim(),
                Telefono1 = ((TextBox)fila.FindControl("txt_eit_telefonoUs")).Text.Trim(),
                Tipo1 = Convert.ToBoolean(((DropDownList)fila.FindControl("ddlEditTipo")).SelectedValue),
                Estado1 = Convert.ToBoolean(((DropDownList)fila.FindControl("ddlEditEstado")).SelectedValue)
            };

            if (!us.VerificarNombreModificar(usuario) || !us.VerificarEmailModificar(usuario))
            {
                MostrarMensaje("El nombre o el e-mail ya los usa otra cuenta.", TipoMensaje.Error);
                return;
            }
            if (!us.ActualizarUsuario(usuario))
            {
                MostrarMensaje(ErrorContraseña(usuario.Contraseña1) ?? "No se pudo modificar el usuario.", TipoMensaje.Error);
                return;
            }
            MostrarMensaje("Usuario " + usuario.Nombre1 + " modificado.", TipoMensaje.Exito);
            gvUsuarios.EditIndex = -1;
            CargarGvUsuarios();
        }

        protected void gvUsuarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Usuario usuario = new Usuario { IdUsuario1 = Convert.ToInt32(gvUsuarios.DataKeys[e.RowIndex].Value) };
            if (usuario.IdUsuario1 == Sesion.Usuario.IdUsuario1)
            {
                MostrarMensaje("No podés desactivar tu propia cuenta.", TipoMensaje.Aviso);
                return;
            }
            us.eliminarUsuario(ref usuario);
            MostrarMensaje("Usuario desactivado.", TipoMensaje.Exito);
            CargarGvUsuarios();
        }

        protected void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario
            {
                Nombre1 = txtNombreUsuario.Text.Trim(),
                Contraseña1 = txtContraseña.Text,
                Email1 = txtEmail.Text.Trim(),
                Telefono1 = string.IsNullOrWhiteSpace(txtTelefono.Text) ? "Sin telefono" : txtTelefono.Text.Trim(),
                Tipo1 = Convert.ToBoolean(ddlTipoUs.SelectedValue)
            };

            if (!us.VerificarNombre(usuario) || !us.VerificarEmail(usuario))
            {
                MostrarMensaje("No se pudo agregar: el nombre o el e-mail ya están en uso.", TipoMensaje.Error);
                return;
            }
            if (!us.agregarUsuario(usuario))
            {
                MostrarMensaje(ErrorContraseña(usuario.Contraseña1) ?? "No se pudo agregar el usuario.", TipoMensaje.Error);
                return;
            }
            MostrarMensaje((usuario.Tipo1 ? "Administrador " : "Cliente ") + usuario.Nombre1 + " agregado.", TipoMensaje.Exito);
            txtNombreUsuario.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlTipoUs.SelectedValue = "false";
            CargarGvUsuarios();
        }
    }
}
