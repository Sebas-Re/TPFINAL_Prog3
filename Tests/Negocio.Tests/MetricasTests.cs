using System;
using System.Data;
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
        private readonly string _carpeta = Path.Combine(Path.GetTempPath(), "pcbooster-metricas-" + Guid.NewGuid().ToString("N"));
        private readonly GestionMetricas _gestionMetricas = new GestionMetricas();

        public MetricasTests()
        {
            AccesoDatos.RutaBaseDatos = Path.Combine(_carpeta, "PCbooster.db");
            AccesoDatos.CargarDatosDemo = false;
            Contrasenas.Iteraciones = 1000; // el hash real tarda ~250 ms por usuario
        }

        public void Dispose()
        {
            AccesoDatos.CargarDatosDemo = false;
            SQLiteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            try { Directory.Delete(_carpeta, true); } catch (IOException) { }
        }

        /* Nom1 compra 2 Gabinete Gamemax ($3.500) y 1 Fuente Deepcool ($7.300) con crédito. */
        private static void Comprar(int idUsuario = 5)
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

            Comparativa comparativa = _gestionMetricas.Comparar(30);

            Assert.Equal(14300m, comparativa.Actual.Ingresos);
            Assert.Equal(1, comparativa.Actual.Pedidos);
            Assert.Equal(3, comparativa.Actual.Unidades);
            Assert.Equal(14300m, comparativa.Actual.TicketPromedio);
            Assert.Equal(1, comparativa.Actual.ClientesNuevos); // los usuarios del seed tienen 210 días
            Assert.Equal(0m, comparativa.Anterior.Ingresos);
            Assert.Null(Comparativa.Variacion(comparativa.Actual.Ingresos, comparativa.Anterior.Ingresos));
        }

        [Fact]
        public void TasaDeRecompra()
        {
            Comprar(5);
            Comprar(5);
            Comprar(6);

            ResumenPeriodo resumen = _gestionMetricas.Comparar(7).Actual;

            Assert.Equal(2, resumen.Compradores);
            Assert.Equal(1, resumen.CompradoresRecurrentes);
            Assert.Equal(50m, resumen.TasaRecompra);
        }

        [Fact]
        public void LasSeriesRellenanLosPeriodosSinVentas()
        {
            Comprar();

            Serie meses = _gestionMetricas.IngresosPorMes(6);
            Serie dias = _gestionMetricas.IngresosPorDia(30);

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

            Serie categorias = _gestionMetricas.IngresosPorCategoria(30);
            Serie productos = _gestionMetricas.TopProductos(30, 5);
            Serie pagos = _gestionMetricas.MetodosDePago(30);

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

            DataTable tabla = _gestionMetricas.StockBajo(10);
            var codigos = tabla.Rows.Cast<System.Data.DataRow>().Select(f => f["Codigo"].ToString()).ToList();

            Assert.Contains("pdv4", codigos);
            Assert.DoesNotContain("gb8", codigos);
            Assert.DoesNotContain("gb1", codigos); // stock 10: no es menor al umbral
        }

        [Fact]
        public void ConElHistorialDeDemoLasMetricasSonConsistentes()
        {
            AccesoDatos.CargarDatosDemo = true;

            decimal ingresos180 = _gestionMetricas.Comparar(180).Actual.Ingresos;
            decimal ingresos30 = _gestionMetricas.Comparar(30).Actual.Ingresos;

            Assert.True(ingresos180 > 0);
            Assert.Equal(ingresos180, _gestionMetricas.IngresosPorCategoria(180).Valores.Sum());
            Assert.Equal(ingresos30, _gestionMetricas.IngresosPorDia(30).Valores.Sum());
            Assert.All(_gestionMetricas.IngresosPorMes(6).Valores.Take(5), v => Assert.True(v > 0));
            Assert.True(_gestionMetricas.ClientesNuevosPorMes(6).Valores.Sum() > 0);
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
