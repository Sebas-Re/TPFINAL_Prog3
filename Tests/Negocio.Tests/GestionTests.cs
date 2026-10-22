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
            Contrasenas.Iteraciones = 1000; // el hash real tarda ~250 ms por usuario
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
        public void BloqueaElLoginDespuesDeVariosIntentosFallidos()
        {
            var gestion = new GestionUsuarios();
            try
            {
                for (int i = 0; i < GestionUsuarios.MaxIntentosFallidos; i++)
                {
                    Assert.Null(gestion.Login("Nom2", "incorrecta"));
                }

                Assert.True(gestion.LoginBloqueado("NOM2"));
                Assert.Null(gestion.Login("Nom2", "ctr2")); // ni con la contraseña correcta
                Assert.False(gestion.LoginBloqueado("Nom3"));

                GestionUsuarios.DuracionBloqueo = TimeSpan.Zero; // pasó el tiempo de bloqueo
                Assert.NotNull(gestion.Login("Nom2", "ctr2"));
            }
            finally
            {
                GestionUsuarios.DuracionBloqueo = TimeSpan.FromMinutes(5);
            }
        }

        [Fact]
        public void UnLoginCorrectoReiniciaLosIntentosFallidos()
        {
            var gestion = new GestionUsuarios();

            for (int i = 0; i < GestionUsuarios.MaxIntentosFallidos - 1; i++)
            {
                gestion.Login("Nom3", "incorrecta");
            }
            Assert.NotNull(gestion.Login("Nom3", "ctr3"));
            gestion.Login("Nom3", "incorrecta");

            Assert.False(gestion.LoginBloqueado("Nom3"));
            Assert.NotNull(gestion.Login("Nom3", "ctr3"));
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
            var nuevo = new Usuario { Nombre1 = "nuevo", Contraseña1 = "clave1", Email1 = "n@mail.com", Telefono1 = "1" };

            Assert.True(gestion.agregarUsuario(nuevo));
            Assert.False(gestion.agregarUsuario(nuevo));
            Assert.False(gestion.VerificarNombre(new Usuario { Nombre1 = "NUEVO" }));
            Assert.False(gestion.VerificarEmail(new Usuario { Email1 = "n@mail.com" }));
        }

        [Fact]
        public void ExigeUnLargoMinimoDeContraseña()
        {
            var gestion = new GestionUsuarios();
            var corta = new Usuario { Nombre1 = "corta", Contraseña1 = "12345", Email1 = "c@mail.com", Telefono1 = "1" };

            Assert.False(gestion.agregarUsuario(corta));
            Assert.True(gestion.VerificarNombre(corta)); // no se creó

            var cliente = new GestionUsuarios().get(Cliente);
            cliente.Contraseña1 = "123";
            Assert.False(gestion.ActualizarUsuario(cliente));
            cliente.Contraseña1 = "";
            Assert.True(gestion.ActualizarUsuario(cliente)); // vacía = conservar la actual
            Assert.NotNull(gestion.Login("Nom1", "ctr1"));
        }

        [Fact]
        public void NoPermiteAgregarUnArticuloConCodigoExistente()
        {
            var gestion = new GestionArticulos();
            var repetido = new Articulo
            {
                CodArticulo1 = "gb1", Categoria1 = new Categoria { IdCategoria1 = 2 }, Descripcion1 = "Otro gabinete",
                Stock1 = 1, Precio_Unitario1 = 100, ImagenURL1 = "~/Imagenes_ar/Gabinete/gb1.png"
            };

            Assert.False(gestion.agregarArticulo(repetido));
            repetido.CodArticulo1 = "gb99";
            Assert.True(gestion.agregarArticulo(repetido));
        }

        [Theory]
        [InlineData("~/Imagenes_ar/Gabinete/gb1.png", true)]
        [InlineData("/Imagenes_ar/x.png", true)]
        [InlineData("https://cdn.ejemplo.com/a.png", true)]
        [InlineData("javascript:alert(1)", false)]
        [InlineData("data:image/svg+xml,<svg onload=alert(1)>", false)]
        [InlineData("//otro-sitio.com/a.png", false)]
        [InlineData("", false)]
        public void SoloAceptaImagenesDelSitioOHttp(string url, bool valida)
        {
            Assert.Equal(valida, GestionArticulos.ImagenValida(url));
        }

        [Fact]
        public void RechazaArticulosConCodigoOImagenInvalidos()
        {
            var gestion = new GestionArticulos();
            Articulo Articulo(string codigo, string imagen) => new Articulo
            {
                CodArticulo1 = codigo, Categoria1 = new Categoria { IdCategoria1 = 2 }, Descripcion1 = "Nuevo " + codigo,
                Stock1 = 1, Precio_Unitario1 = 100, ImagenURL1 = imagen, Estado1 = true
            };

            Assert.False(gestion.agregarArticulo(Articulo("x'><", "~/a.png")));
            Assert.False(gestion.agregarArticulo(Articulo("ok1", "javascript:alert(1)")));
            Assert.False(gestion.ActualizarArticulo(Articulo("gb1", "' onerror='alert(1)")));
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

            int idVenta = gestion.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "1122334455", Datos.TarjetaDe(5), out string error);

            Assert.Null(error);
            Assert.True(idVenta > 0);
            Assert.Single(gestion.filtrarIdUsuario(Cliente).Rows);
            Assert.Equal(2, new GestionDetalleVenta().GetTablaDetalleComprasUsuario(new Ventas { IdVenta1 = idVenta }).Rows.Count);
            Assert.Equal(8L, new GestionArticulos().getArticuloCOD(new Articulo { CodArticulo1 = "gb1" }).Rows[0]["Stock_ar"]);
            Assert.Equal(14300m, gestion.getVenta(new Ventas { IdVenta1 = idVenta }).Rows[0]["TotalVenta_ve"]);
        }

        [Fact]
        public void CadaCompraConservaSuPropioTotal()
        {
            var gestion = new GestionVentas();
            string error;

            int primera = gestion.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "11", Datos.TarjetaDe(5), out error);
            var carrito = new Carrito();
            carrito.Agregar("gb2", "Gabinete Aerocool", 4750m, 100);
            int segunda = gestion.ConfirmarCompra(Cliente, carrito, "Calle 123", "11", Datos.TarjetaDe(5), out error);

            Assert.Equal(14300m, gestion.getVenta(new Ventas { IdVenta1 = primera }).Rows[0]["TotalVenta_ve"]);
            Assert.Equal(4750m, gestion.getVenta(new Ventas { IdVenta1 = segunda }).Rows[0]["TotalVenta_ve"]);
        }

        static long Stock(string codigo)
        {
            return Convert.ToInt64(new GestionArticulos().getArticuloCOD(new Articulo { CodArticulo1 = codigo }).Rows[0]["Stock_ar"]);
        }

        [Fact]
        public void SiFaltaStockDeUnArticuloNoSeRegistraNadaDeLaCompra()
        {
            var gestion = new GestionVentas();
            long stockFuente = Stock("fnt1"), stockGabinete = Stock("gb1");
            // El carrito se armó cuando había más stock (otro cliente compró después).
            var carrito = new Carrito();
            carrito.Agregar("fnt1", "Fuente Deepcool 700W", 7300m, 1000);
            for (int i = 0; i <= stockGabinete; i++)
            {
                carrito.Agregar("gb1", "Gabinete Gamemax", 3500m, 1000);
            }

            int idVenta = gestion.ConfirmarCompra(Cliente, carrito, "Calle 123", "11", Datos.TarjetaDe(5), out string error);

            Assert.Equal(0, idVenta);
            Assert.Contains("Solo quedan " + stockGabinete + " unidades", error);
            Assert.Empty(gestion.filtrarIdUsuario(Cliente).Rows);
            Assert.Equal(stockFuente, Stock("fnt1")); // el detalle de la fuente también se deshizo
            Assert.Equal(stockGabinete, Stock("gb1"));
        }

        [Fact]
        public void NoSePuedeComprarUnArticuloDadoDeBaja()
        {
            var gestion = new GestionVentas();
            var carrito = CarritoConDosArticulos();
            new GestionArticulos().eliminarArticulo(new Articulo { CodArticulo1 = "fnt1" });

            Assert.Equal(0, gestion.ConfirmarCompra(Cliente, carrito, "Calle 123", "11", Datos.TarjetaDe(5), out string error));
            Assert.Contains("ya no está disponible", error);
            Assert.Empty(gestion.filtrarIdUsuario(Cliente).Rows);
        }

        [Fact]
        public void ConfirmarCompraDevuelveElIdDeSuPropiaVenta()
        {
            var gestion = new GestionVentas();
            int primera = gestion.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "11", Datos.TarjetaDe(5), out string error);
            var otroCliente = new Usuario { IdUsuario1 = 6 };
            var carrito = new Carrito();
            carrito.Agregar("gb2", "Gabinete Aerocool", 4750m, 100);
            int segunda = gestion.ConfirmarCompra(otroCliente, carrito, "Calle 9", "22", Datos.TarjetaDe(6), out error);

            Assert.Equal(primera + 1, segunda);
            Assert.Equal(6L, gestion.getVenta(new Ventas { IdVenta1 = segunda }).Rows[0]["IdUsuario_ve"]);
            Assert.Single(new GestionDetalleVenta().GetTablaDetalleComprasUsuario(new Ventas { IdVenta1 = segunda }).Rows);
        }

        [Fact]
        public void ConfirmarCompraSoloAceptaUnaTarjetaDelUsuario()
        {
            var gestion = new GestionVentas();
            long deOtroCliente = Datos.TarjetaDe(6);

            Assert.Equal(0, gestion.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "11", deOtroCliente, out string ajena));
            Assert.Equal(0, gestion.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "11", 0, out string inexistente));

            Assert.Contains("tarjetas guardadas", ajena);
            Assert.Contains("tarjetas guardadas", inexistente);
            Assert.Empty(gestion.filtrarIdUsuario(Cliente).Rows);
        }

        [Fact]
        public void ConfirmarCompraValidaCarritoYDatosDeEntrega()
        {
            var gestion = new GestionVentas();

            long tarjeta = Datos.TarjetaDe(5);
            Assert.Equal(0, gestion.ConfirmarCompra(Cliente, new Carrito(), "Calle 123", "11", tarjeta, out string vacio));
            Assert.Equal(0, gestion.ConfirmarCompra(Cliente, CarritoConDosArticulos(), " ", "11", tarjeta, out string sinDireccion));

            Assert.Contains("vacío", vacio);
            Assert.Contains("dirección", sinDireccion);
            Assert.Empty(gestion.filtrarIdUsuario(Cliente).Rows);
        }
    }
}
