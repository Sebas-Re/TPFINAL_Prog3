using System;
using System.Data;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas.AdminForms
{
    public partial class ReportesAdmin : PaginaBase
    {
        const int CantidadTop = 5;

        GestionDetalleVenta gestDetVen = new GestionDetalleVenta();
        GestionVentas gestven = new GestionVentas();
        GestionArticulos gestart = new GestionArticulos();
        long maximoTop;

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Admin; }
        }

        /* Usuario filtrado en ventas; 0 = todas. Se conserva entre postbacks. */
        int FiltroUsuario
        {
            get { return (int?)ViewState["FiltroUsuario"] ?? 0; }
            set { ViewState["FiltroUsuario"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGvVentas();
                CargarReporteArticulos();
                CargarTop();
            }
        }

        //-------------------------------------------------------------------------
        // VENTAS
        //-------------------------------------------------------------------------
        private void CargarGvVentas()
        {
            DataTable ventas = FiltroUsuario == 0
                ? gestven.getVentas()
                : gestven.filtrarIdUsuario(new Usuario { IdUsuario1 = FiltroUsuario });
            DataView recientes = ventas.DefaultView;
            recientes.Sort = "IdVentas_ve DESC";
            gvVentas.DataSource = recientes;
            gvVentas.DataBind();
        }

        protected void btnBuscarUsuario_Click(object sender, EventArgs e)
        {
            int id;
            FiltroUsuario = int.TryParse(txtIdUsuarioBuscar.Text, out id) ? id : 0;
            gvVentas.PageIndex = 0;
            gvVentas.SelectedIndex = -1;
            pnlDetalle.Visible = false;
            CargarGvVentas();
        }

        protected void btnTodasLasVentas_Click(object sender, EventArgs e)
        {
            FiltroUsuario = 0;
            txtIdUsuarioBuscar.Text = "";
            gvVentas.SelectedIndex = -1;
            pnlDetalle.Visible = false;
            CargarGvVentas();
        }

        protected void gvVentas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVentas.PageIndex = e.NewPageIndex;
            gvVentas.SelectedIndex = -1;
            pnlDetalle.Visible = false;
            CargarGvVentas();
        }

        protected void gvVentas_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            int idVenta = Convert.ToInt32(gvVentas.DataKeys[e.NewSelectedIndex].Value);
            gvVentas.SelectedIndex = e.NewSelectedIndex;
            gvDetallesVenta.DataSource = gestDetVen.GetTablaDetalleComprasUsuario(new Ventas { IdVenta1 = idVenta });
            gvDetallesVenta.DataBind();
            lblTituloDetalle.Text = "Detalle de la venta N° " + idVenta;
            pnlDetalle.Visible = true;
        }

        //-------------------------------------------------------------------------
        // ARTÍCULOS VENDIDOS
        //-------------------------------------------------------------------------
        private void CargarReporteArticulos()
        {
            DataView reporte = gestart.getReporteArticulos().DefaultView;
            reporte.Sort = ddlOrdenador.SelectedValue;
            string codigo = txtCodArticulo.Text.Trim();
            if (codigo != "")
            {
                reporte.RowFilter = "Codigo = '" + codigo.Replace("'", "''") + "'";
            }
            gvReporteArticulos.DataSource = reporte;
            gvReporteArticulos.DataBind();
        }

        protected void btnEncontrarArt_Click(object sender, EventArgs e)
        {
            CargarReporteArticulos();
        }

        protected void ddlOrdenador_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarReporteArticulos();
        }

        private void CargarTop()
        {
            DataView top = gestart.getReporteArticulos().DefaultView;
            top.Sort = "CantidadTotal DESC";
            DataTable primeros = top.ToTable().Clone();
            for (int i = 0; i < Math.Min(CantidadTop, top.Count); i++)
            {
                primeros.ImportRow(top[i].Row);
            }
            maximoTop = primeros.Rows.Count > 0 ? Convert.ToInt64(primeros.Rows[0]["CantidadTotal"]) : 0;
            rptTop.DataSource = primeros;
            rptTop.DataBind();
            lblSinVentasTop.Visible = primeros.Rows.Count == 0;
        }

        protected int PorcentajeTop(object cantidad)
        {
            return maximoTop == 0 ? 0 : (int)(Convert.ToInt64(cantidad) * 100 / maximoTop);
        }
    }
}
