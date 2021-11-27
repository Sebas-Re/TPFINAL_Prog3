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
        GestionVentas gestven = new GestionVentas();
        Ventas venta = new Ventas();
        GestionArticulos gestart = new GestionArticulos();
        Articulo art = new Articulo();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblNombreAdmin.Text = Session["NombreUsuario"].ToString();
        }

        protected void btnusuariobuscar_Click(object sender, EventArgs e)
        {            
            venta.Usuario1.IdUsuario1 = Convert.ToInt32(txtIdUsuarioBuscar.Text);           
            gvFacturas.DataSource = gestven.getVenta(venta);
            gvFacturas.DataBind();
        }

        protected void btnencontrarArt_Click(object sender, EventArgs e)
        {            
            art.CodArticulo1 = txtCodArticulo.Text;
            gvArts.DataSource = gestart.ObtenerMaximo(art);
            gvArts.DataBind();
            
        }

        protected void gvFacturas_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
           // DetalleVentas detven = new DetalleVentas();
          //  GestionDetalleVenta gesven = new GestionDetalleVenta();

           // string IDventa = ((Label)gvFacturas.Rows[e.RowIndex].FindControl("lblidVenta")).Text;

            //gvFacturas.se


            // detven.IdDetVenta1 =  
            //  detven.CodArticulo1 =
           // gesven.getTabla(detven);
        }

        protected void btnusuariobuscar_Click1(object sender, EventArgs e)
        {

        }
    }
}