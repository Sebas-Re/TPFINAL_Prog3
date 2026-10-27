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

        private readonly string _tipo;
        private readonly IList<string> _etiquetas;
        private readonly List<object> _series = new List<object>();
        private bool _horizontal;

        public Grafico(string tipo, IList<string> etiquetas)
        {
            _tipo = tipo;
            _etiquetas = etiquetas;
        }

        public Grafico Serie(string nombre, IList<decimal> valores, string formato, string tipoSerie = null, bool ejeDerecho = false)
        {
            _series.Add(new { nombre, valores, formato, tipo = tipoSerie, ejeDerecho });
            return this;
        }

        public Grafico Horizontal()
        {
            _horizontal = true;
            return this;
        }

        public string Json()
        {
            return new JavaScriptSerializer().Serialize(new { tipo = _tipo, etiquetas = _etiquetas, series = _series, horizontal = _horizontal });
        }
    }
}
