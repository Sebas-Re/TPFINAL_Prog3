using System.Web;
using Negocio;

namespace Vistas
{
    /* Tarjeta de indicador (KPI) con la variación contra el período anterior. */
    public static class Indicador
    {
        public static string Html(string titulo, string valor, decimal? variacion, string icono, string color, string comparacion)
        {
            string badge;
            if (variacion == null)
            {
                badge = "<span class=\"text-body-secondary small\">Sin datos del período anterior</span>";
            }
            else
            {
                bool sube = variacion.Value >= 0;
                badge = string.Format(
                    "<span class=\"badge rounded-pill {0}\"><i class=\"bi {1}\"></i> {2}%</span> <span class=\"text-body-secondary small\">{3}</span>",
                    sube ? "text-bg-success-subtle text-success-emphasis" : "text-bg-danger-subtle text-danger-emphasis",
                    sube ? "bi-arrow-up-right" : "bi-arrow-down-right",
                    variacion.Value.ToString("+0.#;-0.#;0"),
                    HttpUtility.HtmlEncode(comparacion));
            }
            return string.Format(
                "<div class=\"col\"><div class=\"card h-100 indicador\"><div class=\"card-body\">" +
                "<div class=\"d-flex align-items-center justify-content-between mb-2\">" +
                "<span class=\"text-body-secondary small\">{0}</span>" +
                "<span class=\"icono rounded-3 d-flex align-items-center justify-content-center bg-{2}-subtle text-{2}\"><i class=\"bi {1}\"></i></span></div>" +
                "<div class=\"fs-3 fw-semibold lh-1 mb-2\">{3}</div>{4}</div></div></div>",
                HttpUtility.HtmlEncode(titulo), icono, color, HttpUtility.HtmlEncode(valor), badge);
        }

        /* Los cuatro indicadores principales de un período. */
        public static string Resumen(Comparativa c, string comparacion)
        {
            return Html("Ingresos", c.Actual.Ingresos.ToString("C0"), Comparativa.Variacion(c.Actual.Ingresos, c.Anterior.Ingresos), "bi-cash-stack", "success", comparacion)
                + Html("Pedidos", c.Actual.Pedidos.ToString("N0"), Comparativa.Variacion(c.Actual.Pedidos, c.Anterior.Pedidos), "bi-bag-check", "primary", comparacion)
                + Html("Ticket promedio", c.Actual.TicketPromedio.ToString("C0"), Comparativa.Variacion(c.Actual.TicketPromedio, c.Anterior.TicketPromedio), "bi-receipt", "warning", comparacion);
        }
    }
}
