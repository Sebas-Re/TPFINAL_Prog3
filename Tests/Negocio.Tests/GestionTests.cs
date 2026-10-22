using System;
using System.Data.SQLite;
using System.IO;
using Dao;
using Entidades;
using Xunit;

// AccesoDatos.RutaBaseDatos es estática: los tests no pueden correr en paralelo.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Negocio.Tests
{
    /* Reglas de negocio contra una base SQLite temporal con los datos de prueba. */
    public class GestionTests : IDisposable
    {
        readonly string carpeta = Path.Combine(Path.GetTempPath(), "pcbooster-negocio-" + Guid.NewGuid().ToString("N"));

        public GestionTests()
        {
            AccesoDatos.RutaBaseDatos = Path.Combine(carpeta, "PCbooster.db");
            AccesoDatos.CargarDatosDemo = false;
        }

        public void Dispose()
        {
            SQLiteConnection.ClearAllPools();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            try { Directory.Delete(carpeta, true); } catch (IOException) { }
        }

        static Usuario Cliente => new Usuario { IdUsuario1 = 5, Nombre1 = "Nom1" };

        static Carrito CarritoConDosArticulos()
        {
            var carrito = new Carrito();
            carrito.Agregar("gb1", "Gabinete Gamemax", 3500m, 10);
            carrito.Agregar("gb1", "Gabinete Gamemax", 3500m, 10);
            carrito.Agregar("fnt1", "Fuente Deepcool 700W", 7300m, 100);
            return carrito;
        }

        [Fact]
        public void LoginDevuelveIdYTipo()
        {
            var gestion = new GestionUsuarios();

            var admin = gestion.Login("admAmador", "admpsw2");
            var cliente = gestion.Login("Nom1", "ctr1");

            Assert.Equal(1, admin.IdUsuario1);
            Assert.True(admin.Tipo1);
            Assert.Equal(5, cliente.IdUsuario1);
            Assert.False(cliente.Tipo1);
        }

        [Fact]
        public void LoginConCredencialesInvalidasDevuelveNull()
        {
            var gestion = new GestionUsuarios();

            Assert.Null(gestion.Login("Nom1", "otra"));
            Assert.Null(gestion.Login("noexiste", "ctr1"));
            Assert.Null(gestion.Login("Nom1", "' or '1'='1"));
        }

        [Fact]
        public void UnUsuarioDesactivadoNoPuedeIniciarSesion()
        {
            var gestion = new GestionUsuarios();
            var cliente = Cliente;

            Assert.True(gestion.eliminarUsuario(ref cliente));

            Assert.Null(gestion.Login("Nom1", "ctr1"));
        }

        [Fact]
        public void NoPermiteRegistrarUnUsuarioRepetido()
        {
            var gestion = new GestionUsuarios();
            var nuevo = new Usuario { Nombre1 = "nuevo", Contraseña1 = "clave", Email1 = "n@mail.com", Telefono1 = "1" };

            Assert.True(gestion.agregarUsuario(nuevo));
            Assert.False(gestion.agregarUsuario(nuevo));
            Assert.False(gestion.VerificarNombre(new Usuario { Nombre1 = "NUEVO" }));
            Assert.False(gestion.VerificarEmail(new Usuario { Email1 = "n@mail.com" }));
        }

        [Fact]
        public void NoPermiteAgregarUnArticuloConCodigoExistente()
        {
            var gestion = new GestionArticulos();
            var repetido = new Articulo
            {
                CodArticulo1 = "gb1", Categoria1 = new Categoria { IdCategoria1 = 2 }, Descripcion1 = "Otro gabinete",
                Stock1 = 1, Precio_Unitario1 = 100, ImagenURL1 = ""
            };

            Assert.False(gestion.agregarArticulo(repetido));
        }

        [Fact]
        public void CategoriasAltaYBajaLogica()
        {
            var gestion = new GestionCategorias();

            Assert.True(gestion.agregarCategoria(new Categoria { Decripcion1 = "Monitores" }));
            Assert.True(gestion.VerificarCategoria(new Categoria { Decripcion1 = "Monitores" }));
            Assert.True(gestion.eliminarCategoria(new Categoria { IdCategoria1 = 9 }));
            Assert.False(gestion.VerificarEstadoCat(new Categoria { IdCategoria1 = 9 }));
        }

        [Fact]
        public void AgregarTarjetaRechazaVencidasYRepetidas()
        {
            var gestion = new GestionTarjetas();
            Tarjeta Tarjeta(string vencimiento) => new Tarjeta
            {
                Us = Cliente, Titular_Tarjeta1 = "Nom Uno", Nombre_Tarjeta1 = "Visa", Tipo_Tarjeta1 = true,
                Num_Tarjeta1 = 4567123412340012, Fecha_Vencimiento1 = vencimiento, Codigo_Seguridad1 = 321
            };

            Assert.False(gestion.agregarTarjeta(Tarjeta("01/01/2020")));
            Assert.True(gestion.agregarTarjeta(Tarjeta("01/12/2099")));
            Assert.False(gestion.agregarTarjeta(Tarjeta("01/12/2099")));
            Assert.Single(gestion.getTablaTarjetasUsuario(Cliente).Rows);
        }

        [Fact]
        public void ConfirmarCompraRegistraVentaDetallesYDescuentaStock()
        {
            var gestion = new GestionVentas();

            int idVenta = gestion.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "1122334455", true, 321, out string error);

            Assert.Null(error);
            Assert.True(idVenta > 0);
            Assert.Single(gestion.filtrarIdUsuario(Cliente).Rows);
            Assert.Equal(2, new GestionDetalleVenta().GetTablaDetalleComprasUsuario(new Ventas { IdVenta1 = idVenta }).Rows.Count);
            Assert.Equal(8L, new GestionArticulos().getArticuloCOD(new Articulo { CodArticulo1 = "gb1" }).Rows[0]["Stock_ar"]);
            Assert.Equal(14300m, gestion.getVenta(new Ventas { IdVenta1 = idVenta }).Rows[0]["TotalVenta_ve"]);
        }

        [Fact]
        public void ConfirmarCompraValidaCarritoYDatosDeEntrega()
        {
            var gestion = new GestionVentas();

            Assert.Equal(0, gestion.ConfirmarCompra(Cliente, new Carrito(), "Calle 123", "11", false, 1, out string vacio));
            Assert.Equal(0, gestion.ConfirmarCompra(Cliente, CarritoConDosArticulos(), " ", "11", false, 1, out string sinDireccion));

            Assert.Contains("vacío", vacio);
            Assert.Contains("dirección", sinDireccion);
            Assert.Empty(gestion.filtrarIdUsuario(Cliente).Rows);
        }
    }
}
