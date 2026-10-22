using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Vistas
{
    /* Arma la definición de un gráfico como JSON para Content/graficos.js:
       new Grafico("bar", etiquetas).Serie("Ingresos", valores, Formato.Moneda).Json() */
    public class Grafico
    {
        public static class Formato
        {
            public const string Moneda = "moneda";
            public const string Numero = "numero";
        }

        readonly string tipo;
        readonly IList<string> etiquetas;
        readonly List<object> series = new List<object>();
        bool horizontal;

        public Grafico(string tipo, IList<string> etiquetas)
        {
            this.tipo = tipo;
            this.etiquetas = etiquetas;
        }

        public Grafico Serie(string nombre, IList<decimal> valores, string formato, string tipoSerie = null, bool ejeDerecho = false)
        {
            series.Add(new { nombre, valores, formato, tipo = tipoSerie, ejeDerecho });
            return this;
        }

        public Grafico Horizontal()
        {
            horizontal = true;
            return this;
        }

        public string Json()
        {
            return new JavaScriptSerializer().Serialize(new { tipo, etiquetas, series, horizontal });
        }
    }
}
