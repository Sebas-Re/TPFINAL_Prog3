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
    public partial class ReportesAdmin : System.Web.UI.Page
    {
        GestionDetalleVenta gestDetVen = new GestionDetalleVenta();
        DetalleVentas detven = new DetalleVentas();
        GestionVentas gestven = new GestionVentas();
        Ventas venta = new Ventas();
        GestionArticulos gestart = new GestionArticulos();
        Articulo art = new Articulo();
        protected void Page_Load(object sender, EventArgs e)
        {
           // lblNombreAdmin.Text = Session["NombreUsuario"].ToString();
            CargarGvVentas();
            CargargvReporteArticulos();
        }

        private void CargarGvVentas()
        {
            gvVentas.DataSource = gestven.getVentas();
            gvVentas.DataBind();
        }

        protected void btnusuariobuscar_Click(object sender, EventArgs e)
        {            
            venta.Usuario1.IdUsuario1 = Convert.ToInt32(txtIdUsuarioBuscar.Text);           
            gvVentas.DataSource = gestven.getVenta(venta);
            gvVentas.DataBind();
        }

        protected void btnencontrarArt_Click(object sender, EventArgs e)
        {            
            art.CodArticulo1 = txtCodArticulo.Text;
            gvReporteArticulos.DataSource = gestart.getArticuloCOD(art);
            gvReporteArticulos.DataBind();            
        }

        protected void gvVentas_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            venta.IdVenta1 = Convert.ToInt32(((Label)gvVentas.Rows[e.NewSelectedIndex].FindControl("lblidVenta")).Text);
            gvDetallesVenta.DataSource = gestDetVen.GetTablaDetalleComprasUsuario(venta);
            gvDetallesVenta.DataBind();
        }

        protected void btnusuariobuscar_Click1(object sender, EventArgs e)
        {

        }
        protected void CargargvReporteArticulos()
        {
            gvReporteArticulos.DataSource = gestart.getReporteArticulos();
            gvReporteArticulos.DataBind();
        }

        protected void btnencontrarArt_Click1(object sender, EventArgs e)
        {
            if (txtCodArticulo.Text != "") 
            {
                art.CodArticulo1 = txtCodArticulo.Text;
                gvReporteArticulos.DataSource = gestart.getReporteArticulo(art);
                gvReporteArticulos.DataBind();
            }
            else
            {
                art.CodArticulo1 = txtCodArticulo.Text;
                gvReporteArticulos.DataSource = gestart.getArticulos();
                gvReporteArticulos.DataBind();
            }
        }
    }
}