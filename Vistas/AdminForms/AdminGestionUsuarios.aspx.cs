using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas.AdminForms
{
    public partial class InicioAdmin : System.Web.UI.Page
    {
        GestionUsuarios us = new GestionUsuarios();
        Usuario usuario = new Usuario();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               lblNombreadm.Text = Session["NombreUsuario"].ToString();
                CargarGvUsuarios();
            }

        }

        public void CargarGvUsuarios()
        {            
            gvUsuarios.DataSource = us.getUsuarios();
            gvUsuarios.DataBind();
        }

        protected void gvUsuarios_RowEditing1(object sender, GridViewEditEventArgs e)
        {
            gvUsuarios.EditIndex = e.NewEditIndex;
            CargarGvUsuarios();
        }

        protected void gvUsuarios_RowCancelingEdit1(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsuarios.EditIndex = -1;
            CargarGvUsuarios();
        }
        
        protected void gvUsuarios_RowUpdating1(object sender, GridViewUpdateEventArgs e)
        {
            usuario.IdUsuario1 = Convert.ToInt32(((Label)gvUsuarios.Rows[e.RowIndex].FindControl("lbl_eit_idUs")).Text);
            usuario.Nombre1 = ((TextBox)gvUsuarios.Rows[e.RowIndex].FindControl("txt_eit_nombreUs")).Text;
            usuario.Contraseña1 = ((TextBox)gvUsuarios.Rows[e.RowIndex].FindControl("txt_eit_passUs")).Text;
            usuario.Email1 = ((TextBox)gvUsuarios.Rows[e.RowIndex].FindControl("txt_eit_emailUs")).Text;
            usuario.Telefono1 = ((TextBox)gvUsuarios.Rows[e.RowIndex].FindControl("txt_eit_telefonoUs")).Text;
            usuario.Tipo1 = Convert.ToBoolean(((DropDownList)gvUsuarios.Rows[e.RowIndex].FindControl("ddlEditTipo")).SelectedValue);
            usuario.Estado1 = Convert.ToBoolean(((DropDownList)gvUsuarios.Rows[e.RowIndex].FindControl("ddlEditEstado")).SelectedValue);
           
            
            if(us.VerificarNombreModificar(usuario)== true && us.VerificarEmailModificar(usuario) == true)
            {
                lblMensajeUsuario.Text = "DATOS MODIFICADOS CON EXITO";
                us.ActualizarUsuario(usuario);
                gvUsuarios.EditIndex = -1;
                CargarGvUsuarios();
            }
            else
            {
                lblMensajeUsuario.Text = "EL NOMBRE MODIFICADO O EL EMAIL YA ESTAN CARGADOS";
                gvUsuarios.EditIndex = -1;
                CargarGvUsuarios();
            }
            
        }

        protected void gvUsuarios_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {
            usuario.IdUsuario1 = Convert.ToInt32(((Label)gvUsuarios.Rows[e.RowIndex].FindControl("lbl_it_idUs")).Text);
            us.eliminarUsuario(ref usuario);
            CargarGvUsuarios();
        }

        protected void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            bool TipoUsuario = false;
            usuario.Nombre1 = txtNombreUsuario.Text;
            usuario.Contraseña1 = txtRepContraseña.Text;
            usuario.Email1 = txtEmail.Text;
            usuario.Telefono1 = txtTelefono.Text;
            usuario.Tipo1 = Convert.ToBoolean(ddlTipoUs.SelectedValue);
            TipoUsuario = Convert.ToBoolean(ddlTipoUs.SelectedValue);

            if (us.VerificarNombre(usuario)==true && us.VerificarEmail(usuario)==true)
            {
                us.agregarUsuario(usuario);
                LimpiarLosCampos();
                if (TipoUsuario)
                {
                    lblMensaje.Text = "Administrador agregado";
                }
                else
                {
                    lblMensaje.Text = "Cliente agregado";
                }
                
                CargarGvUsuarios();
            }
            else
            {
                LimpiarLosCampos();
                lblMensaje.Text = "No pudo ser agregado, el Nombre o Email ya se encuentra ocupado,elige otro.";
            }
        }

        private void LimpiarLosCampos()
        {
            txtNombreUsuario.Text = "";
            txtContraseña.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            ddlTipoUs.SelectedValue = "seleccionar";
        }

        protected void btnBuscarId_Click(object sender, EventArgs e)
        {
           
            if (txtBuscarID.Text == "")
            {
                CargarGvUsuarios();
            }
            else
            {
                usuario.IdUsuario1 = Convert.ToInt32(txtBuscarID.Text);
                gvUsuarios.DataSource = us.getTabla(usuario);
                gvUsuarios.DataBind();
                txtBuscarID.Text = "";
            }

        }

        protected void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            CargarGvUsuarios();
            txtBuscarID.Text = "";
        }

        //protected void gvUsuarios_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{
        //    string NombreFila = ((Label)gvUsuarios.Rows[e.NewSelectedIndex].FindControl("lbl_it_nombreUs")).Text;
        //    string NombreAdmin = Session["NombreUsuario"].ToString();
        //    lblMensaje.Text = NombreFila;
        //    if (NombreFila == NombreAdmin)
        //    {

        //        gvUsuarios.Rows[e.NewSelectedIndex].FindControl("ddlEditEstado").Visible = false;

        //    }
        //}

        //protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        //{


        //    //foreach (DataRow row in gvUsuarios.Rows)
        //    //{
        //    //    //string NombreFila = ((Label)gvUsuarios.Rows[e.Row.RowIndex].FindControl("lbl_it_nombreUs")).Text;
        //    //    string NombreFila = ((Label)gvUsuarios.Rows[row.].FindControl("lbl_it_nombreUs")).Text;
        //    //    string NombreAdmin = Session["NombreUsuario"].ToString();

        //    //    if (NombreFila == NombreAdmin)
        //    //    {

        //    //        gvUsuarios.Rows[e.Row.RowIndex].FindControl("ddlEditEstado").Visible = false;

        //    //    }

        //    //}

        //}
    }    
}