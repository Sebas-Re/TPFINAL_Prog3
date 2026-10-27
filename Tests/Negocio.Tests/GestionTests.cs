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

        static Usuario Cliente => new Usuario { IdUsuario = 5, Nombre = "Nom1" };

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
            var gestionUsuarios = new GestionUsuarios();

            var admin = gestionUsuarios.Login("admAmador", "admpsw2");
            var cliente = gestionUsuarios.Login("Nom1", "ctr1");

            Assert.Equal(1, admin.IdUsuario);
            Assert.True(admin.Tipo);
            Assert.Equal(5, cliente.IdUsuario);
            Assert.False(cliente.Tipo);
        }

        [Fact]
        public void LoginConCredencialesInvalidasDevuelveNull()
        {
            var gestionUsuarios = new GestionUsuarios();

            Assert.Null(gestionUsuarios.Login("Nom1", "otra"));
            Assert.Null(gestionUsuarios.Login("noexiste", "ctr1"));
            Assert.Null(gestionUsuarios.Login("Nom1", "' or '1'='1"));
        }

        [Fact]
        public void BloqueaElLoginDespuesDeVariosIntentosFallidos()
        {
            var gestionUsuarios = new GestionUsuarios();
            try
            {
                for (int i = 0; i < GestionUsuarios.MaxIntentosFallidos; i++)
                {
                    Assert.Null(gestionUsuarios.Login("Nom2", "incorrecta"));
                }

                Assert.True(gestionUsuarios.LoginBloqueado("NOM2"));
                Assert.Null(gestionUsuarios.Login("Nom2", "ctr2")); // ni con la contraseña correcta
                Assert.False(gestionUsuarios.LoginBloqueado("Nom3"));

                GestionUsuarios.DuracionBloqueo = TimeSpan.Zero; // pasó el tiempo de bloqueo
                Assert.NotNull(gestionUsuarios.Login("Nom2", "ctr2"));
            }
            finally
            {
                GestionUsuarios.DuracionBloqueo = TimeSpan.FromMinutes(5);
            }
        }

        [Fact]
        public void UnLoginCorrectoReiniciaLosIntentosFallidos()
        {
            var gestionUsuarios = new GestionUsuarios();

            for (int i = 0; i < GestionUsuarios.MaxIntentosFallidos - 1; i++)
            {
                gestionUsuarios.Login("Nom3", "incorrecta");
            }
            Assert.NotNull(gestionUsuarios.Login("Nom3", "ctr3"));
            gestionUsuarios.Login("Nom3", "incorrecta");

            Assert.False(gestionUsuarios.LoginBloqueado("Nom3"));
            Assert.NotNull(gestionUsuarios.Login("Nom3", "ctr3"));
        }

        [Fact]
        public void UnUsuarioDesactivadoNoPuedeIniciarSesion()
        {
            var gestionUsuarios = new GestionUsuarios();
            var cliente = Cliente;

            Assert.True(gestionUsuarios.EliminarUsuario(ref cliente));

            Assert.Null(gestionUsuarios.Login("Nom1", "ctr1"));
        }

        [Fact]
        public void NoPermiteRegistrarUnUsuarioRepetido()
        {
            var gestionUsuarios = new GestionUsuarios();
            var nuevo = new Usuario { Nombre = "nuevo", Contrasena = "clave1", Email = "n@mail.com", Telefono = "1" };

            Assert.True(gestionUsuarios.AgregarUsuario(nuevo));
            Assert.False(gestionUsuarios.AgregarUsuario(nuevo));
            Assert.False(gestionUsuarios.VerificarNombre(new Usuario { Nombre = "NUEVO" }));
            Assert.False(gestionUsuarios.VerificarEmail(new Usuario { Email = "n@mail.com" }));
        }

        [Fact]
        public void ExigeUnLargoMinimoDeContraseña()
        {
            var gestionUsuarios = new GestionUsuarios();
            var corta = new Usuario { Nombre = "corta", Contrasena = "12345", Email = "c@mail.com", Telefono = "1" };

            Assert.False(gestionUsuarios.AgregarUsuario(corta));
            Assert.True(gestionUsuarios.VerificarNombre(corta)); // no se creó

            var cliente = new GestionUsuarios().get(Cliente);
            cliente.Contrasena = "123";
            Assert.False(gestionUsuarios.ActualizarUsuario(cliente));
            cliente.Contrasena = "";
            Assert.True(gestionUsuarios.ActualizarUsuario(cliente)); // vacía = conservar la actual
            Assert.NotNull(gestionUsuarios.Login("Nom1", "ctr1"));
        }

        static Usuario CambiosDeNom4(string email = "ema4", string nombre = "Nom4", string nueva = "", string telefono = "23456")
        {
            return new Usuario { IdUsuario = 8, Nombre = nombre, Email = email, Contrasena = nueva, Telefono = telefono };
        }

        [Fact]
        public void CambiarNombreOEmailExigeLaContraseñaActual()
        {
            var gestionUsuarios = new GestionUsuarios();

            Assert.Contains("contraseña actual", gestionUsuarios.ActualizarDatosPropios(CambiosDeNom4(email: "nuevo@mail.com"), ""));
            Assert.Contains("contraseña actual", gestionUsuarios.ActualizarDatosPropios(CambiosDeNom4(nombre: "Intruso"), "incorrecta"));
            Assert.Equal("ema4", gestionUsuarios.get(new Usuario { IdUsuario = 8 }).Email);
            Assert.Equal("Nom4", gestionUsuarios.get(new Usuario { IdUsuario = 8 }).Nombre);

            Assert.Null(gestionUsuarios.ActualizarDatosPropios(CambiosDeNom4(email: "nuevo@mail.com", nombre: "Nom4b"), "ctr4"));
            var guardado = gestionUsuarios.get(new Usuario { IdUsuario = 8 });
            Assert.Equal("nuevo@mail.com", guardado.Email);
            Assert.Equal("Nom4b", guardado.Nombre);
            Assert.False(guardado.Tipo);
            Assert.True(guardado.Estado);
        }

        [Fact]
        public void CambiarSoloElTelefonoNoPideContraseña()
        {
            var gestionUsuarios = new GestionUsuarios();

            Assert.Null(gestionUsuarios.ActualizarDatosPropios(CambiosDeNom4(telefono: "1199887766"), ""));
            Assert.Equal("1199887766", gestionUsuarios.get(new Usuario { IdUsuario = 8 }).Telefono);
        }

        [Fact]
        public void CambiarLaContraseñaExigeLaActual()
        {
            var gestionUsuarios = new GestionUsuarios();

            Assert.NotNull(gestionUsuarios.ActualizarDatosPropios(CambiosDeNom4(nueva: "otra123"), ""));
            Assert.NotNull(gestionUsuarios.Login("Nom4", "ctr4"));

            Assert.Null(gestionUsuarios.ActualizarDatosPropios(CambiosDeNom4(nueva: "otra123"), "ctr4"));
            Assert.Null(gestionUsuarios.Login("Nom4", "ctr4"));
            Assert.NotNull(gestionUsuarios.Login("Nom4", "otra123"));
        }

        [Fact]
        public void LosErroresDeContraseñaActualCuentanParaElBloqueo()
        {
            var gestionUsuarios = new GestionUsuarios();
            for (int i = 0; i < GestionUsuarios.MaxIntentosFallidos; i++)
            {
                gestionUsuarios.ActualizarDatosPropios(CambiosDeNom4(email: "robado@mail.com"), "incorrecta");
            }

            Assert.Contains("Demasiados intentos", gestionUsuarios.ActualizarDatosPropios(CambiosDeNom4(email: "robado@mail.com"), "ctr4"));
            Assert.True(gestionUsuarios.LoginBloqueado("Nom4"));
            Assert.Equal("ema4", gestionUsuarios.get(new Usuario { IdUsuario = 8 }).Email);

            GestionUsuarios.DuracionBloqueo = TimeSpan.Zero;
            try
            {
                Assert.NotNull(gestionUsuarios.Login("Nom4", "ctr4")); // reinicia la cuenta de intentos
            }
            finally
            {
                GestionUsuarios.DuracionBloqueo = TimeSpan.FromMinutes(5);
            }
        }

        [Fact]
        public void NoPermiteAgregarUnArticuloConCodigoExistente()
        {
            var gestionArticulos = new GestionArticulos();
            var repetido = new Articulo
            {
                CodigoArticulo = "gb1",
                Categoria = new Categoria { IdCategoria = 2 },
                Descripcion = "Otro gabinete",
                Stock = 1,
                PrecioUnitario = 100,
                ImagenUrl = "~/Imagenes_ar/Gabinete/gb1.png"
            };

            Assert.False(gestionArticulos.AgregarArticulo(repetido));
            repetido.CodigoArticulo = "gb99";
            Assert.True(gestionArticulos.AgregarArticulo(repetido));
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
            var gestionArticulos = new GestionArticulos();
            Articulo Articulo(string codigo, string imagen) => new Articulo
            {
                CodigoArticulo = codigo,
                Categoria = new Categoria { IdCategoria = 2 },
                Descripcion = "Nuevo " + codigo,
                Stock = 1,
                PrecioUnitario = 100,
                ImagenUrl = imagen,
                Estado = true
            };

            Assert.False(gestionArticulos.AgregarArticulo(Articulo("x'><", "~/a.png")));
            Assert.False(gestionArticulos.AgregarArticulo(Articulo("ok1", "javascript:alert(1)")));
            Assert.False(gestionArticulos.ActualizarArticulo(Articulo("gb1", "' onerror='alert(1)")));
        }

        [Fact]
        public void CategoriasAltaYBajaLogica()
        {
            var gestionCategorias = new GestionCategorias();

            Assert.True(gestionCategorias.AgregarCategoria(new Categoria { Descripcion = "Monitores" }));
            Assert.True(gestionCategorias.VerificarCategoria(new Categoria { Descripcion = "Monitores" }));
            Assert.True(gestionCategorias.EliminarCategoria(new Categoria { IdCategoria = 9 }));
            Assert.False(gestionCategorias.VerificarEstadoCat(new Categoria { IdCategoria = 9 }));
        }

        [Fact]
        public void AgregarTarjetaRechazaVencidasYRepetidas()
        {
            var gestionTarjetas = new GestionTarjetas();
            static Tarjeta Tarjeta(string vencimiento) => new()
            {
                Usuario = Cliente,
                TitularTarjeta = "Nom Uno",
                NombreTarjeta = "Visa",
                TipoTarjeta = true,
                NumTarjeta = 4567123412340012,
                FechaVencimiento = vencimiento,
                CodigoSeguridad = 321
            };

            Assert.False(gestionTarjetas.AgregarTarjeta(Tarjeta("01/01/2020")));
            Assert.True(gestionTarjetas.AgregarTarjeta(Tarjeta("01/12/2099")));
            Assert.False(gestionTarjetas.AgregarTarjeta(Tarjeta("01/12/2099")));
            Assert.Single(gestionTarjetas.GetTablaTarjetasUsuario(Cliente).Rows);
        }

        [Fact]
        public void ConfirmarCompraRegistraVentaDetallesYDescuentaStock()
        {
            var gestionVentas = new GestionVentas();

            int idVenta = gestionVentas.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "1122334455", Datos.TarjetaDe(5), out string error);

            Assert.Null(error);
            Assert.True(idVenta > 0);
            Assert.Single(gestionVentas.FiltrarIdUsuario(Cliente).Rows);
            Assert.Equal(2, new GestionDetalleVenta().GetTablaDetalleComprasUsuario(new Venta { IdVenta = idVenta }).Rows.Count);
            Assert.Equal(8L, new GestionArticulos().GetArticuloCOD(new Articulo { CodigoArticulo = "gb1" }).Rows[0]["Stock_ar"]);
            Assert.Equal(14300m, gestionVentas.GetVenta(new Venta { IdVenta = idVenta }).Rows[0]["TotalVenta_ve"]);
        }

        [Fact]
        public void CadaCompraConservaSuPropioTotal()
        {
            var gestionVentas = new GestionVentas();
            string error;

            int primera = gestionVentas.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "11", Datos.TarjetaDe(5), out error);
            var carrito = new Carrito();
            carrito.Agregar("gb2", "Gabinete Aerocool", 4750m, 100);
            int segunda = gestionVentas.ConfirmarCompra(Cliente, carrito, "Calle 123", "11", Datos.TarjetaDe(5), out error);

            Assert.Equal(14300m, gestionVentas.GetVenta(new Venta { IdVenta = primera }).Rows[0]["TotalVenta_ve"]);
            Assert.Equal(4750m, gestionVentas.GetVenta(new Venta { IdVenta = segunda }).Rows[0]["TotalVenta_ve"]);
        }

        static long Stock(string codigo)
        {
            return Convert.ToInt64(new GestionArticulos().GetArticuloCOD(new Articulo { CodigoArticulo = codigo }).Rows[0]["Stock_ar"]);
        }

        [Fact]
        public void SiFaltaStockDeUnArticuloNoSeRegistraNadaDeLaCompra()
        {
            var gestionVentas = new GestionVentas();
            long stockFuente = Stock("fnt1"), stockGabinete = Stock("gb1");
            // El carrito se armó cuando había más stock (otro cliente compró después).
            var carrito = new Carrito();
            carrito.Agregar("fnt1", "Fuente Deepcool 700W", 7300m, 1000);
            for (int i = 0; i <= stockGabinete; i++)
            {
                carrito.Agregar("gb1", "Gabinete Gamemax", 3500m, 1000);
            }

            int idVenta = gestionVentas.ConfirmarCompra(Cliente, carrito, "Calle 123", "11", Datos.TarjetaDe(5), out string error);

            Assert.Equal(0, idVenta);
            Assert.Contains("Solo quedan " + stockGabinete + " unidades", error);
            Assert.Empty(gestionVentas.FiltrarIdUsuario(Cliente).Rows);
            Assert.Equal(stockFuente, Stock("fnt1")); // el detalle de la fuente también se deshizo
            Assert.Equal(stockGabinete, Stock("gb1"));
        }

        [Fact]
        public void NoSePuedeComprarUnArticuloDadoDeBaja()
        {
            var gestionVentas = new GestionVentas();
            var carrito = CarritoConDosArticulos();
            new GestionArticulos().EliminarArticulo(new Articulo { CodigoArticulo = "fnt1" });

            Assert.Equal(0, gestionVentas.ConfirmarCompra(Cliente, carrito, "Calle 123", "11", Datos.TarjetaDe(5), out string error));
            Assert.Contains("ya no está disponible", error);
            Assert.Empty(gestionVentas.FiltrarIdUsuario(Cliente).Rows);
        }

        [Fact]
        public void ConfirmarCompraDevuelveElIdDeSuPropiaVenta()
        {
            var gestionVentas = new GestionVentas();
            int primera = gestionVentas.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "11", Datos.TarjetaDe(5), out string error);
            var otroCliente = new Usuario { IdUsuario = 6 };
            var carrito = new Carrito();
            carrito.Agregar("gb2", "Gabinete Aerocool", 4750m, 100);
            int segunda = gestionVentas.ConfirmarCompra(otroCliente, carrito, "Calle 9", "22", Datos.TarjetaDe(6), out error);

            Assert.Equal(primera + 1, segunda);
            Assert.Equal(6L, gestionVentas.GetVenta(new Venta { IdVenta = segunda }).Rows[0]["IdUsuario_ve"]);
            Assert.Single(new GestionDetalleVenta().GetTablaDetalleComprasUsuario(new Venta { IdVenta = segunda }).Rows);
        }

        [Fact]
        public void ConfirmarCompraSoloAceptaUnaTarjetaDelUsuario()
        {
            var gestionVentas = new GestionVentas();
            long deOtroCliente = Datos.TarjetaDe(6);

            Assert.Equal(0, gestionVentas.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "11", deOtroCliente, out string ajena));
            Assert.Equal(0, gestionVentas.ConfirmarCompra(Cliente, CarritoConDosArticulos(), "Calle 123", "11", 0, out string inexistente));

            Assert.Contains("tarjetas guardadas", ajena);
            Assert.Contains("tarjetas guardadas", inexistente);
            Assert.Empty(gestionVentas.FiltrarIdUsuario(Cliente).Rows);
        }

        [Fact]
        public void ConfirmarCompraValidaCarritoYDatosDeEntrega()
        {
            var gestionVentas = new GestionVentas();

            long tarjeta = Datos.TarjetaDe(5);
            Assert.Equal(0, gestionVentas.ConfirmarCompra(Cliente, new Carrito(), "Calle 123", "11", tarjeta, out string vacio));
            Assert.Equal(0, gestionVentas.ConfirmarCompra(Cliente, CarritoConDosArticulos(), " ", "11", tarjeta, out string sinDireccion));

            Assert.Contains("vacío", vacio);
            Assert.Contains("dirección", sinDireccion);
            Assert.Empty(gestionVentas.FiltrarIdUsuario(Cliente).Rows);
        }
    }
}
