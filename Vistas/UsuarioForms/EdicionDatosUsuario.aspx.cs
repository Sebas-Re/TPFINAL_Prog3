using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using TextBox = System.Web.UI.WebControls.TextBox;
using Label = System.Web.UI.WebControls.Label;

namespace Vistas
{
    public partial class EdicionDatosUsuario : System.Web.UI.Page
    {
        GestionTarjetas gestarj = new GestionTarjetas();
        GestionUsuarios gestus = new GestionUsuarios();
        Usuario usuario = new Usuario();
        Tarjeta tarj = new Tarjeta();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblNombreUsuario.Text = Session["NombreUsuario"].ToString();

            if (!IsPostBack)
            {
                cargarGvModificarUsuario();
                CargarGvTarjetas();
            }
        }

        public void cargarGvModificarUsuario ()
        {
            usuario.Nombre1 = Convert.ToString(Session["NombreUsuario"]);
            gvDatoUsuario.DataSource = gestus.getUsuariosActualizar(usuario);
            gvDatoUsuario.DataBind();
        }

        public void CargarGvTarjetas()
        {
            usuario.Nombre1 = Convert.ToString(Session["NombreUsuario"]);
            usuario.IdUsuario1 = gestus.DevolverIDusuario(usuario);
            gvTarjetas.DataSource = gestarj.getTablaTarjetasUsuario(usuario);
            gvTarjetas.DataBind();
        }

        protected void ddlMenuNavegacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int op = Convert.ToInt32(ddlMenuNavegacion.SelectedValue);
            switch (op)
            {
                case 1:
                    Server.Transfer("~/UsuarioForms/Catalogo.aspx");
                    break;
                case 2:
                    Server.Transfer("~/UsuarioForms/HistorialCompras.aspx");
                    break;
                case 3:
                    Server.Transfer("~/UsuarioForms/Carrito.aspx");
                    break;
                case 4:
                    Session["NombreUsuario"] = null;
                    Server.Transfer("~/Inicio.aspx");
                    break;
            }
        }

        protected void gvDatoUsuario_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvDatoUsuario.EditIndex = e.NewEditIndex;
            cargarGvModificarUsuario();
        }

        protected void gvDatoUsuario_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvDatoUsuario.EditIndex = -1;
            cargarGvModificarUsuario();
        }

        protected void gvDatoUsuario_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            usuario.IdUsuario1 = Convert.ToInt32(((Label)gvDatoUsuario.Rows[e.RowIndex].FindControl("lblEditId")).Text);
            usuario.Nombre1 = ((TextBox)gvDatoUsuario.Rows[e.RowIndex].FindControl("txtEditNombre")).Text;
            usuario.Contraseña1 = ((TextBox)gvDatoUsuario.Rows[e.RowIndex].FindControl("txtEditContraseña")).Text;
            usuario.Email1 = ((TextBox)gvDatoUsuario.Rows[e.RowIndex].FindControl("txtEditEmail")).Text;
            usuario.Telefono1 = ((TextBox)gvDatoUsuario.Rows[e.RowIndex].FindControl("txtEditTelefono")).Text;
            usuario.Estado1 = true;

            if(gestus.VerificarNombreModificar(usuario)==true && gestus.VerificarEmailModificar(usuario) == true)
            {
                gestus.ActualizarUsuario(usuario);

                gvDatoUsuario.EditIndex = -1;

                Session["NombreUsuario"] = usuario.Nombre1;

                cargarGvModificarUsuario();

                MessageBox.Show("SE GUARDARON LOS CAMBIOS, AHORA ESTOS SON TUS NUEVOS DATOS", "Atencion");


                Server.Transfer("~/Inicio.aspx");
            }
            else
            {
                gvDatoUsuario.EditIndex = -1;
                cargarGvModificarUsuario();
                lblMensajeUsuario.Text = "EL NOMBRE O EMAIL INGRESADOS YA ESTAN CARGADOS";
            }
            
        }

        protected void gvTarjetas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
           tarj.Num_Tarjeta1 = Convert.ToInt32(((Label)gvTarjetas.Rows[e.RowIndex].FindControl("lblNumTarjeta")).Text);
           tarj.Codigo_Seguridad1 = Convert.ToInt32(((Label)gvTarjetas.Rows[e.RowIndex].FindControl("lblCodSeguridad")).Text);
            tarj.Titular_Tarjeta1 = ((Label)gvTarjetas.Rows[e.RowIndex].FindControl("lblTitularTarjeta")).Text;
                
            if (gestarj.eliminarTarjeta(tarj))
            {
                MessageBox.Show("Tarjeta Eliminada con exito", "Atencion");
            }
            else
            {
                MessageBox.Show("No se ha podido eliminar la tarjeta", "Atencion");
            }
            gvTarjetas.EditIndex = -1;
            
            CargarGvTarjetas();



        }
    }
}