using System;
using System.Data;
using Negocio;

namespace Vistas.AdminForms
{
    public partial class AdminHome : PaginaBase
    {
        private const int Dias = 30;
        private const int Meses = 6;
        protected const int UmbralStock = 10;

        private readonly GestionMetricas _gestionMetricas = new GestionMetricas();

        protected string GraficoMeses { get; private set; }
        protected string GraficoCategorias { get; private set; }
        protected string GraficoClientes { get; private set; }

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Admin; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblNombreAdm.Text = Sesion.Usuario.Nombre;

            Comparativa comparativa = _gestionMetricas.Comparar(Dias);
            litIndicadores.Text = Indicador.Resumen(comparativa, "vs. 30 días anteriores")
                + Indicador.Html("Clientes nuevos", comparativa.Actual.ClientesNuevos.ToString(),
                    Comparativa.Variacion(comparativa.Actual.ClientesNuevos, comparativa.Anterior.ClientesNuevos), "bi-person-plus", "info", "vs. 30 días anteriores");

            Serie meses = _gestionMetricas.IngresosPorMes(Meses);
            GraficoMeses = new Grafico("bar", meses.Etiquetas)
                .Serie("Ingresos", meses.Valores, Grafico.Formato.Moneda)
                .Serie("Pedidos", meses.Valores2, Grafico.Formato.Numero, "line", ejeDerecho: true)
                .Json();

            Serie categorias = _gestionMetricas.IngresosPorCategoria(180);
            GraficoCategorias = new Grafico("doughnut", categorias.Etiquetas)
                .Serie("Ingresos", categorias.Valores, Grafico.Formato.Moneda)
                .Json();

            Serie clientes = _gestionMetricas.ClientesNuevosPorMes(Meses);
            GraficoClientes = new Grafico("bar", clientes.Etiquetas)
                .Serie("Clientes nuevos", clientes.Valores, Grafico.Formato.Numero)
                .Json();

            DataTable stockBajo = _gestionMetricas.StockBajo(UmbralStock);
            rptStockBajo.DataSource = stockBajo;
            rptStockBajo.DataBind();
            lblSinStockBajo.Visible = stockBajo.Rows.Count == 0;
        }
    }
}
