using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class RegistroUsuario : System.Web.UI.Page
    {
        GestionUsuarios us = new GestionUsuarios();
        Usuario usuario = new Usuario();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            usuario.Nombre1 = txtUsuario.Text;
            usuario.Contraseña1 = txtContra1.Text;
            usuario.Email1 = txtEmail.Text;
            usuario.Telefono1 = txtTelefono.Text;
            usuario.Tipo1 = false;
            if (us.VerificarNombre(usuario)==true && us.VerificarEmail(usuario)==true)
            {
                us.agregarUsuario(usuario);
                Session["NombreUsuario"] = null;
                Session["ContraseñaUsuario"] = null;
                Session["NombreUsuario"] = txtUsuario.Text;
                Session["ContraseñaUsuario"] = txtContra1.Text;
                Server.Transfer("~/UsuarioForms/Catalogo.aspx");
            }
            else
            {
                LimpiarLosCampos();
                lblMsjRegistro.Text = "LO SIENTO, EL NOMBRE INGRESADO O EMAIL YA ESTAN CARGADOS, VUELVE A INTENTARLO";
            }
        }

        private void LimpiarLosCampos()
        {
            txtUsuario.Text = "";
            txtContra1.Text = "";
            txtContra2.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
        }
    }
}