using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Dao;

namespace Negocio
{
    /* Métricas de e-commerce para el panel y los reportes. Los períodos son de
       días completos e incluyen hoy: "últimos 30 días" = hoy y los 29 anteriores. */
    public class GestionMetricas
    {
        static readonly CultureInfo Cultura = new CultureInfo("es-AR");
        DaoMetricas dao = new DaoMetricas();

        /* Últimos "dias" días contra los "dias" días anteriores. */
        public Comparativa Comparar(int dias)
        {
            return new Comparativa
            {
                Actual = Resumen(1 - dias, 1),
                Anterior = Resumen(1 - 2 * dias, 1 - dias)
            };
        }

        private ResumenPeriodo Resumen(int inicio, int fin)
        {
            DataRow fila = dao.getResumenVentas(inicio, fin).Rows[0];
            return new ResumenPeriodo
            {
                Ingresos = Convert.ToDecimal(fila["Ingresos"]),
                Pedidos = Convert.ToInt32(fila["Pedidos"]),
                Unidades = Convert.ToInt32(fila["Unidades"]),
                Compradores = Convert.ToInt32(fila["Compradores"]),
                CompradoresRecurrentes = Convert.ToInt32(fila["CompradoresRecurrentes"]),
                ClientesNuevos = dao.getClientesNuevos(inicio, fin)
            };
        }

        /* Ingresos (Valores) y pedidos (Valores2) de los últimos "meses" meses
           calendario, incluido el actual; los meses sin ventas van en 0. */
        public Serie IngresosPorMes(int meses)
        {
            DataTable tabla = dao.getIngresosPorMes(DiasHastaInicioDeMes(meses), 1);
            return SeriePorMes(meses, tabla, "Ingresos", "Pedidos");
        }

        public Serie ClientesNuevosPorMes(int meses)
        {
            DataTable tabla = dao.getClientesNuevosPorMes(DiasHastaInicioDeMes(meses), 1);
            return SeriePorMes(meses, tabla, "Clientes", null);
        }

        /* Ingresos (Valores) y pedidos (Valores2) por día; los días sin ventas van en 0. */
        public Serie IngresosPorDia(int dias)
        {
            var porDia = new Dictionary<string, DataRow>();
            foreach (DataRow fila in dao.getIngresosPorDia(1 - dias, 1).Rows)
            {
                porDia[fila["Dia"].ToString()] = fila;
            }
            Serie serie = new Serie();
            for (DateTime dia = DateTime.Today.AddDays(1 - dias); dia <= DateTime.Today; dia = dia.AddDays(1))
            {
                DataRow fila;
                porDia.TryGetValue(dia.ToString("yyyy-MM-dd"), out fila);
                serie.Agregar(dia.ToString("dd/MM", Cultura),
                    fila == null ? 0 : Convert.ToDecimal(fila["Ingresos"]),
                    fila == null ? 0 : Convert.ToDecimal(fila["Pedidos"]));
            }
            return serie;
        }

        public Serie IngresosPorCategoria(int dias)
        {
            Serie serie = new Serie();
            foreach (DataRow fila in dao.getIngresosPorCategoria(1 - dias, 1).Rows)
            {
                serie.Agregar(fila["Categoria"].ToString(), Convert.ToDecimal(fila["Ingresos"]), Convert.ToDecimal(fila["Unidades"]));
            }
            return serie;
        }

        /* Unidades (Valores) e ingresos (Valores2) de los productos más vendidos. */
        public Serie TopProductos(int dias, int cantidad)
        {
            Serie serie = new Serie();
            foreach (DataRow fila in dao.getTopProductos(1 - dias, 1, cantidad).Rows)
            {
                serie.Agregar(fila["Descripcion"].ToString(), Convert.ToDecimal(fila["Unidades"]), Convert.ToDecimal(fila["Ingresos"]));
            }
            return serie;
        }

        /* Pedidos (Valores) e ingresos (Valores2) con débito y con crédito. */
        public Serie MetodosDePago(int dias)
        {
            decimal[] pedidos = new decimal[2], ingresos = new decimal[2];
            foreach (DataRow fila in dao.getMetodosDePago(1 - dias, 1).Rows)
            {
                int i = Convert.ToBoolean(fila["Credito"]) ? 1 : 0;
                pedidos[i] = Convert.ToDecimal(fila["Pedidos"]);
                ingresos[i] = Convert.ToDecimal(fila["Ingresos"]);
            }
            Serie serie = new Serie();
            serie.Agregar("Débito", pedidos[0], ingresos[0]);
            serie.Agregar("Crédito", pedidos[1], ingresos[1]);
            return serie;
        }

        public DataTable TopClientes(int dias, int cantidad)
        {
            return dao.getTopClientes(1 - dias, 1, cantidad);
        }

        public DataTable StockBajo(int umbral)
        {
            return dao.getStockBajo(umbral);
        }

        private static int DiasHastaInicioDeMes(int meses)
        {
            DateTime inicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1 - meses);
            return (int)(inicio - DateTime.Today).TotalDays;
        }

        private static Serie SeriePorMes(int meses, DataTable tabla, string columna, string columna2)
        {
            var porMes = new Dictionary<string, DataRow>();
            foreach (DataRow fila in tabla.Rows)
            {
                porMes[fila["Mes"].ToString()] = fila;
            }
            Serie serie = new Serie();
            DateTime mes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1 - meses);
            for (int i = 0; i < meses; i++, mes = mes.AddMonths(1))
            {
                DataRow fila;
                porMes.TryGetValue(mes.ToString("yyyy-MM"), out fila);
                serie.Agregar(Cultura.TextInfo.ToTitleCase(mes.ToString("MMM yy", Cultura).Replace(".", "")),
                    fila == null ? 0 : Convert.ToDecimal(fila[columna]),
                    fila == null || columna2 == null ? 0 : Convert.ToDecimal(fila[columna2]));
            }
            return serie;
        }
    }

    public class ResumenPeriodo
    {
        public decimal Ingresos { get; set; }
        public int Pedidos { get; set; }
        public int Unidades { get; set; }
        public int Compradores { get; set; }
        public int CompradoresRecurrentes { get; set; }
        public int ClientesNuevos { get; set; }

        public decimal TicketPromedio
        {
            get { return Pedidos == 0 ? 0 : Ingresos / Pedidos; }
        }

        /* Porcentaje de compradores del período que compraron más de una vez. */
        public decimal TasaRecompra
        {
            get { return Compradores == 0 ? 0 : 100m * CompradoresRecurrentes / Compradores; }
        }
    }

    public class Comparativa
    {
        public ResumenPeriodo Actual { get; set; }
        public ResumenPeriodo Anterior { get; set; }

        /* Variación porcentual; null si en el período anterior no hubo nada contra qué comparar. */
        public static decimal? Variacion(decimal actual, decimal anterior)
        {
            if (anterior == 0)
            {
                return null;
            }
            return Math.Round((actual - anterior) / anterior * 100, 1);
        }
    }

    /* Serie para un gráfico: una etiqueta por punto y hasta dos valores. */
    public class Serie
    {
        public List<string> Etiquetas { get; } = new List<string>();
        public List<decimal> Valores { get; } = new List<decimal>();
        public List<decimal> Valores2 { get; } = new List<decimal>();

        public void Agregar(string etiqueta, decimal valor, decimal valor2)
        {
            Etiquetas.Add(etiqueta);
            Valores.Add(valor);
            Valores2.Add(valor2);
        }
    }
}
