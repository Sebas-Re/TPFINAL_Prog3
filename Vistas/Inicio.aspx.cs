using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Entidades;
using Negocio;

namespace Vistas
{

    public partial class Inicio : System.Web.UI.Page
    {
        GestionUsuarios us = new GestionUsuarios();
        Usuario usuario = new Usuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            usuario.Nombre1 = txtUsuario.Text;
            usuario.Contraseña1 = txtContraseña.Text;
            if (us.VerificarLogin(usuario))
            {
                Session["NombreUsuario"] = txtUsuario.Text;
                Session["ContraseñaUsuario"] = txtContraseña.Text;
                if (us.VerificarTipoUsuario(usuario))
                {
                    Server.Transfer("~/AdminForms/AdminHome.aspx");
                }
                else
                {
                    Server.Transfer("~/UsuarioForms/Catalogo.aspx");
                }
            }
            else
            {
                lblUsuarioInexistente.Text = "NO ESTAS REGISTRADO, CREA UNA CUENTA PARA EMPEZAR A COMPRAR!";
                txtUsuario.Text = "";
                txtContraseña.Text = "";
            }
        }
    }
}