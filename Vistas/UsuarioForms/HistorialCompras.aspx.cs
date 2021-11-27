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
    public partial class HistorialCompras : System.Web.UI.Page
    {
        
        GestionUsuarios gestus = new GestionUsuarios();
        Usuario us = new Usuario();
        GestionVentas gestven = new GestionVentas();
        Ventas ven = new Ventas();
        GestionDetalleVenta gestDetVen = new GestionDetalleVenta();
        DetalleVentas detven = new DetalleVentas();

        protected void Page_Load(object sender, EventArgs e)
        {           
            us.Nombre1 = Session["NombreUsuario"].ToString();
            us.IdUsuario1 = gestus.DevolverIDusuario(us);
            lblNombreUsuario.Text = us.Nombre1;

            gvUsuarioCompras.DataSource = gestven.filtrarIdUsuario(us);
            gvUsuarioCompras.DataBind();
        }

        protected void ddlMenuNavegacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int op = Convert.ToInt32(ddlMenuNavegacion.SelectedValue);
            switch (op)
            {
                case 1:
                    Server.Transfer("~/UsuarioForms/EdicionDatosUsuario.aspx");
                    break;
                case 2:
                    Server.Transfer("~/UsuarioForms/HistorialCompras.aspx");
                    break;
                case 3:
                    Server.Transfer("~/UsuarioForms/Catalogo.aspx");
                    break;
                case 4:
                    Server.Transfer("~/UsuarioForms/Carrito.aspx");
                    break;
                case 5:
                    Session["NombreUsuario"] = null;
                    Server.Transfer("~/Inicio.aspx");
                    break;
                
            }
        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {            
            ven.IdVenta1 = Convert.ToInt32(((Label)gvUsuarioCompras.Rows[e.NewSelectedIndex].FindControl("lblidVenta")).Text);
            gvDetalleCompras.DataSource = gestDetVen.GetTablaDetalleComprasUsuario(ven);
            gvDetalleCompras.DataBind();
        }
    }
}

