using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas.AdminForms
{
    public partial class ReportesAdmin : PaginaBase
    {
        static readonly int[] PeriodosDisponibles = { 7, 30, 90, 180 };
        const int CantidadTopProductos = 10;
        const int CantidadTopClientes = 8;

        GestionDetalleVenta gestDetVen = new GestionDetalleVenta();
        GestionVentas gestven = new GestionVentas();
        GestionArticulos gestart = new GestionArticulos();
        GestionMetricas metricas = new GestionMetricas();

        protected string GraficoDias { get; private set; }
        protected string GraficoProductos { get; private set; }
        protected string GraficoPagos { get; private set; }

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Admin; }
        }

        /* Período de análisis por querystring (?dias=30), así se puede compartir el link. */
        protected int Dias
        {
            get
            {
                int dias;
                return int.TryParse(Request.QueryString["dias"], out dias) && PeriodosDisponibles.Contains(dias) ? dias : 30;
            }
        }

        /* Usuario filtrado en ventas; 0 = todas. Se conserva entre postbacks. */
        int FiltroUsuario
        {
            get { return (int?)ViewState["FiltroUsuario"] ?? 0; }
            set { ViewState["FiltroUsuario"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Los gráficos se arman en cada request: viajan en atributos, no en el ViewState.
            CargarMetricas();
            if (!IsPostBack)
            {
                rptPeriodos.DataSource = PeriodosDisponibles;
                rptPeriodos.DataBind();
                CargarGvVentas();
                CargarReporteArticulos();
            }
        }

        //-------------------------------------------------------------------------
        // MÉTRICAS DEL PERÍODO
        //-------------------------------------------------------------------------
        private void CargarMetricas()
        {
            string comparacion = "vs. " + Dias + " días anteriores";
            Comparativa comparativa = metricas.Comparar(Dias);
            litIndicadores.Text = Indicador.Resumen(comparativa, comparacion)
                + Indicador.Html("Unidades vendidas", comparativa.Actual.Unidades.ToString("N0"),
                    Comparativa.Variacion(comparativa.Actual.Unidades, comparativa.Anterior.Unidades), "bi-box-seam", "info", comparacion);
            lblRecompra.Text = comparativa.Actual.TasaRecompra.ToString("0") + "%";

            Serie dias = metricas.IngresosPorDia(Dias);
            GraficoDias = new Grafico("line", dias.Etiquetas)
                .Serie("Ingresos", dias.Valores, Grafico.Formato.Moneda)
                .Serie("Pedidos", dias.Valores2, Grafico.Formato.Numero, "bar", ejeDerecho: true)
                .Json();

            Serie productos = metricas.TopProductos(Dias, CantidadTopProductos);
            GraficoProductos = new Grafico("bar", productos.Etiquetas)
                .Serie("Unidades", productos.Valores, Grafico.Formato.Numero)
                .Horizontal()
                .Json();

            Serie pagos = metricas.MetodosDePago(Dias);
            GraficoPagos = new Grafico("doughnut", pagos.Etiquetas)
                .Serie("Pedidos", pagos.Valores, Grafico.Formato.Numero)
                .Json();

            gvTopClientes.DataSource = metricas.TopClientes(Dias, CantidadTopClientes);
            gvTopClientes.DataBind();
        }

        //-------------------------------------------------------------------------
        // VENTAS
        //-------------------------------------------------------------------------
        private void CargarGvVentas()
        {
            DataTable ventas = FiltroUsuario == 0
                ? gestven.GetVentas()
                : gestven.FiltrarIdUsuario(new Usuario { IdUsuario = FiltroUsuario });
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
            gvDetallesVenta.DataSource = gestDetVen.GetTablaDetalleComprasUsuario(new Ventas { IdVenta = idVenta });
            gvDetallesVenta.DataBind();
            lblTituloDetalle.Text = "Detalle de la venta N° " + idVenta;
            pnlDetalle.Visible = true;
        }

        //-------------------------------------------------------------------------
        // ARTÍCULOS VENDIDOS (HISTÓRICO)
        //-------------------------------------------------------------------------
        private void CargarReporteArticulos()
        {
            DataView reporte = gestart.GetReporteArticulos().DefaultView;
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
            gvReporteArticulos.PageIndex = 0;
            CargarReporteArticulos();
        }

        protected void ddlOrdenador_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvReporteArticulos.PageIndex = 0;
            CargarReporteArticulos();
        }

        protected void gvReporteArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReporteArticulos.PageIndex = e.NewPageIndex;
            CargarReporteArticulos();
        }
    }
}
