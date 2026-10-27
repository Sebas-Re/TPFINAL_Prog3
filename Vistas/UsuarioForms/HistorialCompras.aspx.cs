using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class HistorialCompras : PaginaBase
    {
        GestionVentas gestven = new GestionVentas();
        GestionDetalleVenta gestDetVen = new GestionDetalleVenta();

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Cliente; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }
            gvUsuarioCompras.DataSource = gestven.FiltrarIdUsuario(Sesion.Usuario);
            gvUsuarioCompras.DataBind();

            bool tieneCompras = gvUsuarioCompras.Rows.Count > 0;
            pnlSinCompras.Visible = !tieneCompras;
            pnlCompras.Visible = tieneCompras;
            gvDetalleCompras.Visible = false;

            // Después de confirmar una compra se llega con ?compra=N: se muestra su detalle.
            int idCompra;
            if (tieneCompras && int.TryParse(Request.QueryString["compra"], out idCompra))
            {
                for (int i = 0; i < gvUsuarioCompras.DataKeys.Count; i++)
                {
                    if (Convert.ToInt32(gvUsuarioCompras.DataKeys[i].Value) == idCompra)
                    {
                        gvUsuarioCompras.SelectedIndex = i;
                        MostrarDetalle(idCompra);
                    }
                }
            }
        }

        protected void gvUsuarioCompras_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            gvUsuarioCompras.SelectedIndex = e.NewSelectedIndex;
            MostrarDetalle(Convert.ToInt32(gvUsuarioCompras.DataKeys[e.NewSelectedIndex].Value));
        }

        private void MostrarDetalle(int idVenta)
        {
            DataTable detalle = gestDetVen.GetTablaDetalleComprasUsuario(new Ventas { IdVenta = idVenta });
            gvDetalleCompras.DataSource = detalle;
            gvDetalleCompras.DataBind();
            gvDetalleCompras.Visible = true;
            lblElegirCompra.Visible = false;
            lblTituloDetalle.Text = "Detalle de la compra N° " + idVenta;
            if (detalle.Rows.Count > 0)
            {
                lblEntrega.Text = HttpUtility.HtmlEncode(string.Format("Entrega en {0} · Recibe: {1}",
                    detalle.Rows[0]["Direccion_de_Entrega"], detalle.Rows[0]["Contacto"]));
            }
        }
    }
}
