using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dao;
using Entidades;
using Xunit;

namespace Negocio.Tests
{
    public class MetricasTests : IDisposable
    {
        readonly string carpeta = Path.Combine(Path.GetTempPath(), "pcbooster-metricas-" + Guid.NewGuid().ToString("N"));
        readonly GestionMetricas metricas = new GestionMetricas();

        public MetricasTests()
        {
            AccesoDatos.RutaBaseDatos = Path.Combine(carpeta, "PCbooster.db");
            AccesoDatos.CargarDatosDemo = false;
            Contrasenas.Iteraciones = 1000; // el hash real tarda ~250 ms por usuario
        }

        public void Dispose()
        {
            AccesoDatos.CargarDatosDemo = false;
            SQLiteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            try { Directory.Delete(carpeta, true); } catch (IOException) { }
        }

        /* Nom1 compra 2 Gabinete Gamemax ($3.500) y 1 Fuente Deepcool ($7.300) con crédito. */
        static void Comprar(int idUsuario = 5)
        {
            var carrito = new Carrito();
            carrito.Agregar("gb1", "Gabinete Gamemax", 3500m, 10);
            carrito.Agregar("gb1", "Gabinete Gamemax", 3500m, 10);
            carrito.Agregar("fnt1", "Fuente Deepcool 700W", 7300m, 100);
            string error;
            Assert.True(new GestionVentas().ConfirmarCompra(new Usuario { IdUsuario = idUsuario }, carrito, "Calle 1", "Yo", Datos.TarjetaDe(idUsuario), out error) > 0, error);
        }

        [Fact]
        public void ResumenDelPeriodoContraElAnterior()
        {
            Comprar();
            new GestionUsuarios().AgregarUsuario(new Usuario { Nombre = "nuevo", Contrasena = "clave1", Email = "n@x.com", Telefono = "1" });

            Comparativa c = metricas.Comparar(30);

            Assert.Equal(14300m, c.Actual.Ingresos);
            Assert.Equal(1, c.Actual.Pedidos);
            Assert.Equal(3, c.Actual.Unidades);
            Assert.Equal(14300m, c.Actual.TicketPromedio);
            Assert.Equal(1, c.Actual.ClientesNuevos); // los usuarios del seed tienen 210 días
            Assert.Equal(0m, c.Anterior.Ingresos);
            Assert.Null(Comparativa.Variacion(c.Actual.Ingresos, c.Anterior.Ingresos));
        }

        [Fact]
        public void TasaDeRecompra()
        {
            Comprar(5);
            Comprar(5);
            Comprar(6);

            ResumenPeriodo r = metricas.Comparar(7).Actual;

            Assert.Equal(2, r.Compradores);
            Assert.Equal(1, r.CompradoresRecurrentes);
            Assert.Equal(50m, r.TasaRecompra);
        }

        [Fact]
        public void LasSeriesRellenanLosPeriodosSinVentas()
        {
            Comprar();

            Serie meses = metricas.IngresosPorMes(6);
            Serie dias = metricas.IngresosPorDia(30);

            Assert.Equal(6, meses.Etiquetas.Count);
            Assert.Equal(new[] { 0m, 0m, 0m, 0m, 0m, 14300m }, meses.Valores);
            Assert.Equal(1m, meses.Valores2.Last());
            Assert.Equal(30, dias.Valores.Count);
            Assert.Equal(14300m, dias.Valores.Last());
            Assert.Equal(DateTime.Today.ToString("dd/MM"), dias.Etiquetas.Last());
        }

        [Fact]
        public void IngresosPorCategoriaProductosYMetodosDePago()
        {
            Comprar();

            Serie categorias = metricas.IngresosPorCategoria(30);
            Serie productos = metricas.TopProductos(30, 5);
            Serie pagos = metricas.MetodosDePago(30);

            Assert.Equal(new[] { "Fuente", "Gabinete" }, categorias.Etiquetas);
            Assert.Equal(new[] { 7300m, 7000m }, categorias.Valores);
            Assert.Equal("Gabinete Gamemax", productos.Etiquetas[0]);
            Assert.Equal(2m, productos.Valores[0]);
            Assert.Equal(new[] { "Débito", "Crédito" }, pagos.Etiquetas);
            Assert.Equal(new[] { 0m, 1m }, pagos.Valores);
        }

        [Fact]
        public void StockBajoSoloIncluyeArticulosActivos()
        {
            new GestionArticulos().EliminarArticulo(new Articulo { CodigoArticulo = "gb8" });

            var tabla = metricas.StockBajo(10);
            var codigos = tabla.Rows.Cast<System.Data.DataRow>().Select(f => f["Codigo"].ToString()).ToList();

            Assert.Contains("pdv4", codigos);
            Assert.DoesNotContain("gb8", codigos);
            Assert.DoesNotContain("gb1", codigos); // stock 10: no es menor al umbral
        }

        [Fact]
        public void ConElHistorialDeDemoLasMetricasSonConsistentes()
        {
            AccesoDatos.CargarDatosDemo = true;

            decimal ingresos180 = metricas.Comparar(180).Actual.Ingresos;
            decimal ingresos30 = metricas.Comparar(30).Actual.Ingresos;

            Assert.True(ingresos180 > 0);
            Assert.Equal(ingresos180, metricas.IngresosPorCategoria(180).Valores.Sum());
            Assert.Equal(ingresos30, metricas.IngresosPorDia(30).Valores.Sum());
            Assert.All(metricas.IngresosPorMes(6).Valores.Take(5), v => Assert.True(v > 0));
            Assert.True(metricas.ClientesNuevosPorMes(6).Valores.Sum() > 0);
        }

        [Fact]
        public void VariacionPorcentual()
        {
            Assert.Equal(50m, Comparativa.Variacion(150, 100));
            Assert.Equal(-25m, Comparativa.Variacion(75, 100));
            Assert.Null(Comparativa.Variacion(10, 0));
        }
    }
}
