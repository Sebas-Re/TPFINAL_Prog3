using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Vistas.E2E
{
    [Collection("Sitio")]
    public class FlujosTests : IDisposable
    {
        static readonly Random azar = new();

        readonly Navegador navegador;
        readonly SitioFixture sitio;

        public FlujosTests(SitioFixture sitio)
        {
            this.sitio = sitio;
            navegador = new Navegador(sitio.Direccion);
        }

        public void Dispose()
        {
            navegador.Dispose();
        }

        static Dictionary<string, string> Campos(params string[] idYValor)
        {
            var campos = new Dictionary<string, string>();
            for (int i = 0; i < idYValor.Length; i += 2)
            {
                campos[idYValor[i]] = idYValor[i + 1];
            }
            return campos;
        }

        private async Task<Pagina> IniciarSesion(string usuario, string contrasena)
        {
            Pagina inicio = await navegador.Abrir("Inicio.aspx");
            return await navegador.Enviar(inicio, "btnIniciarSesion", Campos("txtUsuario", usuario, "txtContraseña", contrasena));
        }

        /* Cada test que cambia datos usa un cliente nuevo, para no depender del orden. */
        private async Task<Pagina> RegistrarClienteNuevo()
        {
            string nombre = "e2e" + azar.Next(100000, 999999);
            Pagina registro = await navegador.Abrir("UsuarioForms/RegistroUsuario.aspx");
            return await navegador.Enviar(registro, "btnRegistrarse", Campos(
                "txtUsuario", nombre, "txtEmail", nombre + "@test.com",
                "txtContra1", "clave1", "txtContra2", "clave1", "txtTelefono", "1122334455"));
        }

        private async Task<Pagina> AgregarAlCarrito(string rutaCatalogo, int cantidadProductos)
        {
            Pagina catalogo = await navegador.Abrir(rutaCatalogo);
            for (int i = 0; i < cantidadProductos; i++)
            {
                var agregar = catalogo.Links("a[title='Agregar al carrito']");
                catalogo = await navegador.Click(catalogo, agregar[i]);
                Assert.Contains(catalogo.Alertas, a => a.StartsWith("Agregaste"));
            }
            return catalogo;
        }

        [Fact]
        public async Task LoginInvalidoMuestraElMensajeEnLaPagina()
        {
            Pagina pagina = await IniciarSesion("Nom1", "incorrecta");

            Assert.EndsWith("/Inicio.aspx", pagina.Ruta);
            Assert.Contains(pagina.Alertas, a => a.Contains("Usuario o contraseña incorrectos"));
        }

        [Fact]
        public async Task DemasiadosIntentosFallidosBloqueanElLogin()
        {
            Pagina pagina = null;
            for (int i = 0; i < 6; i++)
            {
                pagina = await IniciarSesion("Nom5", "incorrecta");
            }
            Assert.Contains(pagina.Alertas, a => a.Contains("Demasiados intentos fallidos"));

            pagina = await IniciarSesion("Nom5", "ctr5");
            Assert.EndsWith("/Inicio.aspx", pagina.Ruta);
        }

        [Fact]
        public async Task ElIdDeSesionSinLaCookieDeAutenticacionNoAlcanza()
        {
            await IniciarSesion("Nom1", "ctr1");
            using (var atacante = new Navegador(sitio.Direccion))
            {
                // Quien conoce (o fijó antes del login) el id de sesión de la víctima...
                atacante.PonerCookie("ASP.NET_SessionId", navegador.Cookie("ASP.NET_SessionId").Value);
                Pagina pagina = await atacante.Abrir("UsuarioForms/HistorialCompras.aspx");
                Assert.EndsWith("/Inicio.aspx", pagina.Ruta);

                // ...necesita también el token que se generó en el login.
                atacante.PonerCookie("PCBooster.Auth", navegador.Cookie("PCBooster.Auth").Value);
                pagina = await atacante.Abrir("UsuarioForms/HistorialCompras.aspx");
                Assert.EndsWith("/HistorialCompras.aspx", pagina.Ruta);
            }
        }

        [Fact]
        public async Task LasRespuestasTraenLosHeadersDeSeguridad()
        {
            await IniciarSesion("Nom1", "ctr1");
            using (var respuesta = await navegador.Pedir("UsuarioForms/Catalogo.aspx"))
            {
                string Header(string nombre) => respuesta.Headers.TryGetValues(nombre, out var valores) ? string.Join(",", valores) : null;

                Assert.Equal("nosniff", Header("X-Content-Type-Options"));
                Assert.Equal("DENY", Header("X-Frame-Options"));
                Assert.Contains("frame-ancestors 'none'", Header("Content-Security-Policy"));
                Assert.Null(Header("X-AspNet-Version"));
                Assert.Null(Header("X-Powered-By"));
                Assert.Null(Header("Server"));
            }
            Assert.True(navegador.Cookie("ASP.NET_SessionId").HttpOnly);
            Assert.True(navegador.Cookie("PCBooster.Auth").HttpOnly);
        }

        [Fact]
        public async Task UnFormularioArmadoEnOtraSesionEsRechazado()
        {
            Pagina formularioAjeno;
            using (var atacante = new Navegador(sitio.Direccion))
            {
                await atacante.Enviar(await atacante.Abrir("Inicio.aspx"), "btnIniciarSesion", Campos("txtUsuario", "Nom2", "txtContraseña", "ctr2"));
                formularioAjeno = await atacante.Abrir("UsuarioForms/EdicionDatosUsuario.aspx");
            }
            await IniciarSesion("Nom1", "ctr1");

            // Como en un CSRF: la víctima postea un formulario que no generó su sesión.
            var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                navegador.Enviar(formularioAjeno, "btnGuardar", Campos("txtEmail", "robado@test.com")));
            Assert.Contains("500", error.Message);
        }

        [Fact]
        public async Task CambiarElEmailPideLaContraseñaActual()
        {
            await RegistrarClienteNuevo();
            Pagina cuenta = await navegador.Abrir("UsuarioForms/EdicionDatosUsuario.aspx");
            string email = "cambiado" + azar.Next(100000, 999999) + "@test.com";

            Pagina sinClave = await navegador.Enviar(cuenta, "btnGuardar", Campos("txtEmail", email));
            Assert.Contains(sinClave.Alertas, a => a.Contains("ingresá tu contraseña actual"));

            Pagina conClave = await navegador.Enviar(cuenta, "btnGuardar", Campos("txtEmail", email, "txtContraseñaActual", "clave1"));
            Assert.Contains(conClave.Alertas, a => a.Contains("Guardamos tus cambios"));
            Assert.Equal(email, conClave.Control("txtEmail").GetAttribute("value"));
        }

        [Fact]
        public async Task SinSesionLasPaginasRedirigenAlInicio()
        {
            Pagina pagina = await navegador.Abrir("UsuarioForms/Carrito.aspx");

            Assert.EndsWith("/Inicio.aspx", pagina.Ruta);
            Assert.Contains(pagina.Alertas, a => a.Contains("Iniciá sesión"));
        }

        [Fact]
        public async Task UnClienteNoPuedeEntrarAlPanelDeAdministracion()
        {
            await IniciarSesion("Nom2", "ctr2");

            Pagina pagina = await navegador.Abrir("AdminForms/AdminGestionUsuarios.aspx");

            Assert.EndsWith("/UsuarioForms/Catalogo.aspx", pagina.Ruta);
        }

        [Fact]
        public async Task MisComprasSinComprasMuestraUnEstadoVacioSinColgarse()
        {
            await RegistrarClienteNuevo();

            // Antes, un MessageBox de WinForms colgaba el request en el servidor.
            Stopwatch reloj = Stopwatch.StartNew();
            Pagina pagina = await navegador.Abrir("UsuarioForms/HistorialCompras.aspx");

            Assert.True(reloj.Elapsed < TimeSpan.FromSeconds(10), "Tardó " + reloj.Elapsed);
            Assert.Contains("Todavía no realizaste ninguna compra", pagina.Texto);
        }

        [Fact]
        public async Task RegistroIniciaSesionYLlevaAlCatalogo()
        {
            Pagina pagina = await RegistrarClienteNuevo();

            Assert.EndsWith("/UsuarioForms/Catalogo.aspx", pagina.Ruta);
            Assert.Contains(pagina.Alertas, a => a.Contains("Tu cuenta ya está lista"));
        }

        [Fact]
        public async Task ElCatalogoFiltraPorCategoriaYBusca()
        {
            await IniciarSesion("Nom3", "ctr3");

            Pagina gabinetes = await navegador.Abrir("UsuarioForms/Catalogo.aspx?cat=2");
            Pagina busqueda = await navegador.Abrir("UsuarioForms/Catalogo.aspx?q=ryzen");

            Assert.Contains("Gabinete Gamemax", gabinetes.Texto);
            Assert.DoesNotContain("Fuente Deepcool", gabinetes.Texto);
            Assert.Contains("Procesador gamer AMD Ryzen 7", busqueda.Texto);
            Assert.Contains("Procesador AMD Ryzen 7 3800XT", busqueda.Texto);
        }

        [Fact]
        public async Task ElModalMuestraElDetalleYAgregaLaCantidadElegida()
        {
            await RegistrarClienteNuevo();
            Pagina catalogo = await navegador.Abrir("UsuarioForms/Catalogo.aspx?q=Ryzen%207%203800XT");

            var modal = catalogo.Documento.QuerySelector("#detalle-ps7");
            Assert.NotNull(modal);
            Assert.Contains("Especificaciones", modal.TextContent);
            Assert.Contains("8 / 16", modal.TextContent);
            Assert.Contains("Procesador", modal.TextContent);

            var agregar = modal.QuerySelector("a[title='Agregar la cantidad elegida']");
            Pagina resultado = await navegador.Click(catalogo, agregar, Campos("txtCantidad", "3"));

            Assert.Contains(resultado.Alertas, a => a.Contains("Agregaste 3 unidades"));
            Pagina carrito = await navegador.Abrir("UsuarioForms/Carrito.aspx");
            var fila = carrito.Documento.QuerySelectorAll("[id$=gvMiscompras] tr").Single(tr => tr.TextContent.Contains("3800XT"));
            Assert.Contains("3", fila.Children[2].TextContent);
        }

        [Fact]
        public async Task FlujoCompletoDeCompra()
        {
            await RegistrarClienteNuevo();
            await AgregarAlCarrito("UsuarioForms/Catalogo.aspx?cat=2", 2);

            Pagina carrito = await navegador.Abrir("UsuarioForms/Carrito.aspx");
            Assert.Contains("Gabinete Gamemax", carrito.Texto);
            Assert.Contains("Gabinete Aerocool", carrito.Texto);

            carrito = await navegador.Enviar(carrito, "btnGuardarTarjeta", Campos(
                "txtNumeroTarjeta", "4567123412340012", "txtTarjetaTitular", "Cliente E2E", "txtNombreTarjeta", "Visa",
                "rbDebitoCredito", "1", "txtTarjetames", "12", "txtTarjetaAño", "2035", "txtCodSeguridad", "321"));
            Assert.Contains(carrito.Alertas, a => a.StartsWith("Tarjeta guardada"));
            Assert.Contains("Visa terminada en 0012 (Crédito)", carrito.Texto);
            Assert.DoesNotContain("321", carrito.Control("ddlTarjetas").OuterHtml); // el código de seguridad no llega al navegador

            Pagina compras = await navegador.Enviar(carrito, "btnConfirmarCompra", Campos(
                "txtDireccionDeEntrega", "Av. Siempreviva 742", "txtContacto", "Homero"));

            Assert.EndsWith("/UsuarioForms/HistorialCompras.aspx", compras.Ruta);
            Assert.Contains(compras.Alertas, a => a.Contains("Registramos tu compra"));
            Assert.Contains("Gabinete Gamemax", compras.Texto);
            Assert.Contains("Av. Siempreviva 742", compras.Texto);

            Pagina carritoVacio = await navegador.Abrir("UsuarioForms/Carrito.aspx");
            Assert.Contains("Tu carrito está vacío", carritoVacio.Texto);

            Pagina cuenta = await navegador.Abrir("UsuarioForms/EdicionDatosUsuario.aspx");
            cuenta = await navegador.Click(cuenta, cuenta.Links("a[title='Eliminar tarjeta']")[0]);
            Assert.Contains(cuenta.Alertas, a => a.Contains("Eliminaste la tarjeta"));
            Assert.Empty(cuenta.Links("a[title='Eliminar tarjeta']"));
        }

        [Fact]
        public async Task ConfirmarSinTarjetaNoRegistraLaCompra()
        {
            await RegistrarClienteNuevo();
            await AgregarAlCarrito("UsuarioForms/Catalogo.aspx?cat=8", 1);
            Pagina carrito = await navegador.Abrir("UsuarioForms/Carrito.aspx");

            // Sin JavaScript, la validación la tiene que hacer el servidor.
            Pagina pagina = await navegador.Enviar(carrito, "btnConfirmarCompra", Campos(
                "txtDireccionDeEntrega", "Calle 1", "txtContacto", "Yo"));

            Assert.EndsWith("/UsuarioForms/Carrito.aspx", pagina.Ruta);
            Assert.Contains(pagina.Alertas, a => a.Contains("Revisá los datos"));
            Pagina compras = await navegador.Abrir("UsuarioForms/HistorialCompras.aspx");
            Assert.Contains("Todavía no realizaste ninguna compra", compras.Texto);
        }

        [Fact]
        public async Task AdminEditaLaCategoriaDeUnArticulo()
        {
            Pagina panel = await IniciarSesion("admAmador", "admpsw2");
            Assert.EndsWith("/AdminForms/AdminHome.aspx", panel.Ruta);

            Pagina articulos = await navegador.Abrir("AdminForms/AdminGestionArticulos.aspx");
            articulos = await navegador.Click(articulos, articulos.PorId("btnBuscarporCodigo"), Campos("txtIdArticulo", "hdd9"));
            Assert.Single(articulos.Links("[id$=gvArticulos] a[title=Editar]"));

            Pagina edicion = await navegador.Click(articulos, articulos.Links("[id$=gvArticulos] a[title=Editar]")[0]);
            var guardar = edicion.Links("[id$=gvArticulos] a").FirstOrDefault(a => a.TextContent.Trim() == "Guardar");
            Assert.NotNull(guardar);
            Pagina guardado = await navegador.Click(edicion, guardar, Campos("ddl_et_Categoria", "8"));

            Assert.Contains(guardado.Alertas, a => a.Contains("Artículo hdd9 modificado"));
            var fila = guardado.Documento.QuerySelectorAll("[id$=gvArticulos] tr").Single(tr => tr.TextContent.Contains("hdd9"));
            Assert.Contains("Ssd", fila.TextContent);
        }

        [Fact]
        public async Task NoSeGuardaUnaImagenQueNoSeaUnaUrlDelSitioOHttp()
        {
            await IniciarSesion("admAmador", "admpsw2");
            Pagina articulos = await navegador.Abrir("AdminForms/AdminGestionArticulos.aspx");
            articulos = await navegador.Click(articulos, articulos.PorId("btnBuscarporCodigo"), Campos("txtIdArticulo", "hdd8"));
            Pagina edicion = await navegador.Click(articulos, articulos.Links("[id$=gvArticulos] a[title=Editar]").Single());
            var guardar = edicion.Links("[id$=gvArticulos] a").First(a => a.TextContent.Trim() == "Guardar");

            Pagina pagina = await navegador.Click(edicion, guardar, Campos("txt_et_Imagen", "x' onerror='alert(1)"));

            Assert.Contains(pagina.Alertas, a => a.Contains("La imagen tiene que ser"));
            Assert.DoesNotContain("onerror='alert", pagina.Html);
        }

        [Fact]
        public async Task UnArticuloDadoDeBajaNoSeVendeHastaReactivarlo()
        {
            await IniciarSesion("admSebas", "admpsw2");
            Pagina articulos = await navegador.Abrir("AdminForms/AdminGestionArticulos.aspx");
            articulos = await navegador.Click(articulos, articulos.PorId("btnBuscarporCodigo"), Campos("txtIdArticulo", "pm9"));

            articulos = await navegador.Click(articulos, articulos.Links("[id$=gvArticulos] a[title='Dar de baja']").Single());
            Assert.Contains(articulos.Alertas, a => a.Contains("pm9 dado de baja"));
            Assert.DoesNotContain("Placa Madre Gigabyte", (await navegador.Abrir("UsuarioForms/Catalogo.aspx?q=Gigabyte")).Texto);

            Pagina edicion = await navegador.Click(articulos, articulos.Links("[id$=gvArticulos] a[title=Editar]").Single());
            var guardar = edicion.Links("[id$=gvArticulos] a").First(a => a.TextContent.Trim() == "Guardar");
            Pagina guardado = await navegador.Click(edicion, guardar, Campos("ddl_et_Estado", "true"));

            Assert.Contains(guardado.Alertas, a => a.Contains("pm9 modificado"));
            Assert.Contains("Placa Madre Gigabyte", (await navegador.Abrir("UsuarioForms/Catalogo.aspx?q=Gigabyte")).Texto);
        }

        [Fact]
        public async Task LasPaginasDeAdministracionCargan()
        {
            await IniciarSesion("admSebas", "admpsw2");

            Pagina usuarios = await navegador.Abrir("AdminForms/AdminGestionUsuarios.aspx");
            Pagina reportes = await navegador.Abrir("AdminForms/ReportesAdmin.aspx");
            Pagina panel = await navegador.Abrir("AdminForms/AdminHome.aspx");

            Assert.Contains("admAmador", usuarios.Texto);
            Assert.Contains("Artículos vendidos", reportes.Texto);
            Assert.Contains("Ingresos y pedidos por mes", panel.Texto);
        }

        [Fact]
        public async Task LosGraficosDelPanelYLosReportesTienenDatos()
        {
            await IniciarSesion("admAmador", "admpsw2");

            Pagina panel = await navegador.Abrir("AdminForms/AdminHome.aspx");
            Pagina reportes = await navegador.Abrir("AdminForms/ReportesAdmin.aspx?dias=7");

            // El sitio de los E2E arranca con el historial de demo.
            var graficosPanel = panel.Documento.QuerySelectorAll("canvas[data-grafico]").Select(c => c.GetAttribute("data-grafico")).ToList();
            Assert.Equal(3, graficosPanel.Count);
            Assert.All(graficosPanel, g => Assert.Contains("\"etiquetas\":[\"", g));
            Assert.Contains(panel.Documento.QuerySelectorAll("script[src]"), s => s.GetAttribute("src").EndsWith("chart.umd.js"));

            var porDia = reportes.Documento.QuerySelector("canvas[aria-label='Ingresos por día']").GetAttribute("data-grafico");
            Assert.Equal(7, System.Text.RegularExpressions.Regex.Matches(porDia.Substring(0, porDia.IndexOf("]")), @"""\d\d/\d\d""").Count);
            Assert.Equal("7 días", reportes.Documento.QuerySelector(".nav-pills .active").TextContent.Trim());
            Assert.Contains("Mejores clientes", reportes.Texto);
        }

        [Fact]
        public async Task SalirCierraLaSesion()
        {
            await IniciarSesion("Nom4", "ctr4");

            await navegador.Abrir("Salir.ashx");
            Pagina pagina = await navegador.Abrir("UsuarioForms/Catalogo.aspx");

            Assert.EndsWith("/Inicio.aspx", pagina.Ruta);
        }
    }
}
