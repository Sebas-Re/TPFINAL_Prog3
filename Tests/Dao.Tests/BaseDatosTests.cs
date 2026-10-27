using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Entidades;
using Negocio;
using Xunit;

// AccesoDatos.RutaBaseDatos es estática: los tests no pueden correr en paralelo.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Dao.Tests
{
    /* Tests de integración contra un archivo SQLite real. Cada test arranca con
       una base nueva, creada desde el schema y los datos de prueba embebidos. */
    public class BaseDatosTests : IDisposable
    {
        readonly string carpeta = Path.Combine(Path.GetTempPath(), "pcbooster-tests-" + Guid.NewGuid().ToString("N"));

        public BaseDatosTests()
        {
            AccesoDatos.RutaBaseDatos = Path.Combine(carpeta, "App_Data", "PCbooster.db");
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

        static Usuario Usuario(string nombre, string contrasena) => new() { Nombre = nombre, Contrasena = contrasena };

        [Fact]
        public void CreaLaBaseConLosDatosDePruebaLaPrimeraVez()
        {
            Assert.False(File.Exists(AccesoDatos.RutaBaseDatos));

            Assert.Equal(7, new DaoUsuarios().GetTablaUsuarios().Rows.Count);
            Assert.Equal(8, new DaoCategorias().GetTablaCategorias().Rows.Count);
            Assert.Equal(68, new DaoArticulos().GetTablaArticulos().Rows.Count);
            Assert.True(File.Exists(AccesoDatos.RutaBaseDatos));
            Assert.False(File.Exists(AccesoDatos.RutaBaseDatos + ".tmp"));
        }

        [Fact]
        public void LaRutaPorDefectoRespetaLaVariableDeEntorno()
        {
            string ruta = Path.Combine(carpeta, "desde-entorno.db");
            Environment.SetEnvironmentVariable(AccesoDatos.VariableEntorno, ruta);
            try
            {
                Assert.Equal(ruta, AccesoDatos.RutaPorDefecto());
            }
            finally
            {
                Environment.SetEnvironmentVariable(AccesoDatos.VariableEntorno, null);
            }
            Assert.EndsWith("PCbooster.db", AccesoDatos.RutaPorDefecto());
        }

        [Fact]
        public void LosIdsDeCategoriaCoincidenConLosDeSqlServer()
        {
            // En SQL Server el 'insert ... union' asignaba los identity en orden alfabético.
            var fuente = new DaoCategorias().GetTablaCategoria(new Categoria { IdCategoria = 1 });
            var ssd = new DaoCategorias().GetTablaCategoria(new Categoria { IdCategoria = 8 });

            Assert.Equal("Fuente", fuente.Rows[0]["Descripcion_ca"]);
            Assert.Equal("Ssd", ssd.Rows[0]["Descripcion_ca"]);
        }

        [Fact]
        public void NoVuelveACargarDatosSiLaBaseYaExiste()
        {
            new GestionCategorias().AgregarCategoria(new Categoria { Descripcion = "Monitores" });

            // Cambiar de ruta y volver obliga a repetir el chequeo de inicialización.
            string original = AccesoDatos.RutaBaseDatos;
            AccesoDatos.RutaBaseDatos = Path.Combine(carpeta, "otra.db");
            Assert.Equal(8, new DaoCategorias().GetTablaCategorias().Rows.Count);
            AccesoDatos.RutaBaseDatos = original;
            Assert.Equal(9, new DaoCategorias().GetTablaCategorias().Rows.Count);
        }

        [Fact]
        public void LosTiposSeMapeanComoEnSqlServer()
        {
            var tabla = new DaoUsuarios().GetTablaUsuarios();
            var articulos = new DaoArticulos().GetTablaArticulos();

            Assert.Equal(typeof(bool), tabla.Columns["Tipo_us"].DataType);
            Assert.Equal(typeof(decimal), articulos.Columns["PrecioUnitario_ar"].DataType);
            // La UI hace Convert.ToBoolean(x.ToString()), que falla con "1".
            Assert.True(Convert.ToBoolean(tabla.Rows[0]["Estado_us"].ToString()));
        }

        [Fact]
        public void LoginValidaUsuarioContraseñaYTipo()
        {
            var daoUsuarios = new DaoUsuarios();

            Assert.True(daoUsuarios.ExisteUsuarioLogin(Usuario("admAmador", "admpsw2")));
            Assert.True(daoUsuarios.TipoUsuarioLogin(Usuario("admAmador", "admpsw2")));
            Assert.True(daoUsuarios.ExisteUsuarioLogin(Usuario("Nom1", "ctr1")));
            Assert.False(daoUsuarios.TipoUsuarioLogin(Usuario("Nom1", "ctr1")));
            Assert.False(daoUsuarios.ExisteUsuarioLogin(Usuario("Nom1", "incorrecta")));
        }

        [Fact]
        public void LoginNoEsVulnerableASqlInjection()
        {
            var daoUsuarios = new DaoUsuarios();

            Assert.False(daoUsuarios.ExisteUsuarioLogin(Usuario("admAmador", "' or '1'='1")));
            Assert.False(daoUsuarios.ExisteUsuarioLogin(Usuario("' or 1=1 --", "x")));
        }

        [Fact]
        public void LasContraseñasSeGuardanHasheadasYConSalt()
        {
            var tabla = new AccesoDatos().ObtenerTabla("Usuarios", "select Nombre_us, Contraseña_us from Usuarios");
            string Guardada(string nombre) => tabla.Select("Nombre_us = '" + nombre + "'")[0]["Contraseña_us"].ToString();

            Assert.All(tabla.Rows.Cast<DataRow>(), fila => Assert.StartsWith("pbkdf2$", fila["Contraseña_us"].ToString()));
            Assert.DoesNotContain("ctr1", Guardada("Nom1"));
            // admAmador y admSebas tienen la misma contraseña, pero el salt hace distinto el hash.
            Assert.NotEqual(Guardada("admAmador"), Guardada("admSebas"));
        }

        [Fact]
        public void ModificarUnUsuarioSinContraseñaConservaLaActual()
        {
            var daoUsuarios = new DaoUsuarios();
            var nom1 = daoUsuarios.ObtenerUsuario(new Usuario { IdUsuario = 5 });

            nom1.Contrasena = null;
            Assert.Equal(1, daoUsuarios.ActualizarUsuario(nom1));
            Assert.True(daoUsuarios.ExisteUsuarioLogin(Usuario("Nom1", "ctr1")));

            nom1.Contrasena = "nueva123";
            Assert.Equal(1, daoUsuarios.ActualizarUsuario(nom1));
            Assert.False(daoUsuarios.ExisteUsuarioLogin(Usuario("Nom1", "ctr1")));
            Assert.True(daoUsuarios.ExisteUsuarioLogin(Usuario("Nom1", "nueva123")));
        }

        [Fact]
        public void NombreDeUsuarioEsCaseInsensitiveComoEnSqlServer()
        {
            Assert.True(new DaoUsuarios().ExisteUsuario(Usuario("ADMAMADOR", null)));
        }

        [Fact]
        public void AgregaYActualizaUnUsuario()
        {
            var gestion = new GestionUsuarios();
            var nuevo = new Usuario { Nombre = "nuevo", Contrasena = "clave1", Email = "nuevo@mail.com", Telefono = "123", Tipo = false };

            Assert.True(gestion.AgregarUsuario(nuevo));
            Assert.False(gestion.AgregarUsuario(nuevo));
            nuevo.IdUsuario = gestion.DevolverIDusuario(nuevo);
            Assert.Equal(10, nuevo.IdUsuario);

            nuevo.Email = "otro@mail.com";
            nuevo.Estado = true;
            Assert.True(gestion.ActualizarUsuario(nuevo));
            var guardado = new DaoUsuarios().ObtenerUsuario(new Usuario { IdUsuario = nuevo.IdUsuario });
            Assert.Equal("otro@mail.com", guardado.Email);
            Assert.True(guardado.Estado);
        }

        [Fact]
        public void EliminarUsuarioEsBajaLogica()
        {
            var usuario = new Usuario { IdUsuario = 5 };

            Assert.Equal(1, new DaoUsuarios().EliminarUsuario(ref usuario));
            Assert.False(new DaoUsuarios().ObtenerUsuario(new Usuario { IdUsuario = 5 }).Estado);
        }

        [Fact]
        public void CategoriasAltaModificacionYBaja()
        {
            var caoCategorias = new DaoCategorias();

            Assert.Equal(1, caoCategorias.AgregarCategoria(new Categoria { Descripcion = "Monitores" }));
            Assert.True(caoCategorias.ExisteCategoriaDescripcion(new Categoria { Descripcion = "monitores" }));
            Assert.True(caoCategorias.ExisteCategoriaModificar(new Categoria { IdCategoria = 1, Descripcion = "Monitores" }));
            Assert.False(caoCategorias.ExisteCategoriaModificar(new Categoria { IdCategoria = 9, Descripcion = "Monitores" }));

            Assert.True(caoCategorias.ActualizarCategoria(new Categoria { IdCategoria = 9, Descripcion = "Monitores 4K", Estado = true }));
            Assert.Equal(1, caoCategorias.EliminarCategoria(new Categoria { IdCategoria = 9 }));
            Assert.False(caoCategorias.DevolverEstadoCat(new Categoria { IdCategoria = 9 }));
        }

        [Fact]
        public void ArticulosAltaBusquedaYBaja()
        {
            var daoArticulos = new DaoArticulos();
            var articulo = new Articulo
            {
                CodigoArticulo = "mn1",
                Categoria = new Categoria { IdCategoria = 1 },
                Descripcion = "Monitor 24'' LG",
                Stock = 10,
                PrecioUnitario = 1500.5m,
                ImagenUrl = "~/Imagenes_ar/mn1.png"
            };

            Assert.Equal(1, daoArticulos.AgregarArticulo(articulo));
            Assert.True(daoArticulos.ExisteArticulo(new Articulo { CodigoArticulo = "mn1" }));
            Assert.True(daoArticulos.ExisteArticuloDescripcion(new Articulo { Descripcion = "Monitor 24'' LG" }));
            Assert.False(daoArticulos.ExisteArticuloDescripcionModificar(new Articulo { CodigoArticulo = "mn1", Descripcion = "Monitor 24'' LG" }));
            Assert.True(daoArticulos.ExisteArticuloDescripcionModificar(new Articulo { CodigoArticulo = "gb1", Descripcion = "Monitor 24'' LG" }));

            Assert.Single(daoArticulos.GetTablaArticuloBuscar(new Articulo { Descripcion = "24'' lg" }).Rows);
            Assert.Equal(9, daoArticulos.GetTablaArticuloxCat(new Articulo { Categoria = new Categoria { IdCategoria = 2 } }).Rows.Count);
            Assert.Equal("gb1", daoArticulos.GetIdxdescripcion(new Articulo { Descripcion = "Gabinete Gamemax" }).CodigoArticulo);

            Assert.Equal(1, daoArticulos.EliminarArticulo(new Articulo { CodigoArticulo = "mn1" }));
            Assert.False((bool)daoArticulos.GetTablaArticuloCOD(new Articulo { CodigoArticulo = "mn1" }).Rows[0]["Estado_ar"]);
        }

        [Fact]
        public void ActualizarArticuloGuardaLaCategoria()
        {
            var daoArticulos = new DaoArticulos();
            var articulo = new Articulo
            {
                CodigoArticulo = "gb1",
                Categoria = new Categoria { IdCategoria = 3 },
                Descripcion = "Gabinete Gamemax",
                Stock = 10,
                PrecioUnitario = 3500,
                ImagenUrl = "~/Imagenes_ar/Gabinete/gb1.png",
                Estado = true
            };

            Assert.True(daoArticulos.ActualizarArticulo(articulo));
            Assert.Equal(3L, daoArticulos.GetTablaArticuloCOD(new Articulo { CodigoArticulo = "gb1" }).Rows[0]["Categoria_ar"]);
        }

        [Fact]
        public void ObtenerArticuloCargaTodosLosCampos()
        {
            var articulo = new DaoArticulos().ObtenerArticulo(new Articulo { CodigoArticulo = "gb1" });

            Assert.Equal(2, articulo.Categoria.IdCategoria);
            Assert.Equal("Gabinete Gamemax", articulo.Descripcion);
            Assert.Equal(10, articulo.Stock);
            Assert.Equal(3500m, articulo.PrecioUnitario);
            Assert.Equal("~/Imagenes_ar/Gabinete/gb1.png", articulo.ImagenUrl);
            Assert.True(articulo.Estado);
        }

        [Fact]
        public void ObtenerCategoriaCargaTodosLosCampos()
        {
            var categoria = new DaoCategorias().ObtenerCategoria(new Categoria { IdCategoria = 8 });

            Assert.Equal("Ssd", categoria.Descripcion);
            Assert.True(categoria.Estado);
        }

        [Fact]
        public void ObtenerVentaCargaTodosLosCampos()
        {
            var daoVentas = new DaoVentas();
            daoVentas.agregarVenta(new Venta { Usuario = new Usuario { IdUsuario = 5 }, MetodoPago = true, CodigoSeguridad = 456 });
            int id = daoVentas.IDultimaventa(new Venta()).IdVenta;

            var venta = daoVentas.ObtenerVenta(new Venta { IdVenta = id });

            Assert.Equal(5, venta.Usuario.IdUsuario);
            Assert.Equal(0m, venta.TotalVenta);
            Assert.True(venta.MetodoPago);
            Assert.Equal(456, venta.CodigoSeguridad);
        }

        [Fact]
        public void ActualizarArticuloPermiteReactivarlo()
        {
            var daoArticulos = new DaoArticulos();
            daoArticulos.EliminarArticulo(new Articulo { CodigoArticulo = "gb1" });
            var articulo = new Articulo
            {
                CodigoArticulo = "gb1",
                Categoria = new Categoria { IdCategoria = 2 },
                Descripcion = "Gabinete Gamemax",
                Stock = 10,
                PrecioUnitario = 3500,
                ImagenUrl = "~/Imagenes_ar/Gabinete/gb1.png",
                Estado = true
            };

            Assert.True(daoArticulos.ActualizarArticulo(articulo));
            Assert.True((bool)daoArticulos.GetTablaArticuloCOD(new Articulo { CodigoArticulo = "gb1" }).Rows[0]["Estado_ar"]);
        }

        [Fact]
        public void ElCatalogoSoloMuestraArticulosActivosDeCategoriasActivas()
        {
            var daoArticulos = new DaoArticulos();
            daoArticulos.EliminarArticulo(new Articulo { CodigoArticulo = "gb1" });
            new DaoCategorias().EliminarCategoria(new Categoria { IdCategoria = 1 }); // Fuente: 9 artículos

            var catalogo = daoArticulos.GetTablaArticulosCatalogo(null, null);

            Assert.Equal(68 - 1 - 9, catalogo.Rows.Count);
            Assert.DoesNotContain(catalogo.Rows.Cast<DataRow>(), f => (string)f["CodArticulo_ar"] == "gb1" || (long)f["Categoria_ar"] == 1);
            Assert.Equal(8, daoArticulos.GetTablaArticulosCatalogo(null, 2).Rows.Count);
            Assert.Empty(daoArticulos.GetTablaArticulosCatalogo(null, 1).Rows);
            Assert.Equal(2, daoArticulos.GetTablaArticulosCatalogo("ryzen", null).Rows.Count);
            Assert.Empty(daoArticulos.GetTablaArticulosCatalogo("Deepcool 700W", null).Rows);
        }

        [Fact]
        public void UnaCompraDescuentaStockYSumaElTotal()
        {
            var ventas = new DaoVentas();
            var detalle = new DaoDetalleVentas();

            Assert.Equal(1, ventas.agregarVenta(new Venta { Usuario = new Usuario { IdUsuario = 5 }, MetodoPago = true, CodigoSeguridad = 123 }));
            int idVenta = ventas.IDultimaventa(new Venta()).IdVenta;
            Assert.Equal(1, detalle.AgregarDetalleVenta(new DetalleVenta
            {
                IdDetalleVenta = idVenta,
                IdUsuario = 5,
                CodigoArticulo = "gb1",
                Cantidad = 2,
                Precio = 3500,
                DireccionEntrega = "Calle 123",
                Contacto = "1122334455"
            }));

            Assert.Equal(8L, new DaoArticulos().GetTablaArticuloCOD(new Articulo { CodigoArticulo = "gb1" }).Rows[0]["Stock_ar"]);
            Assert.Equal(7000m, ventas.GetTablaVentas(new Venta { IdVenta = idVenta }).Rows[0]["TotalVenta_ve"]);
            Assert.IsType<DateTime>(ventas.GetTablaVentas(new Venta { IdVenta = idVenta }).Rows[0]["FechaVenta_ve"]);
            Assert.Single(detalle.GetTablaIdVentaUsuario(new Venta { IdVenta = idVenta }).Rows);

            // Reportes: verifican que el SQL traducido de T-SQL corre en SQLite.
            var daoArticulos = new DaoArticulos();
            Assert.Single(daoArticulos.GetTopArticulos().Rows);
            Assert.Single(daoArticulos.GetTablaReporteArtiuculos().Rows);
            Assert.Single(daoArticulos.GetTablaReportePorArtiuculos(new Articulo { CodigoArticulo = "gb1" }).Rows);
            Assert.Equal(2L, daoArticulos.ObtenerTablaMaximo(new Articulo { CodigoArticulo = "gb1" }).Rows[0]["UNIDADES VENDIDAS"]);
            Assert.Single(ventas.GetMaximoVenta().Rows);
            Assert.Single(detalle.GetMaximaCantidad().Rows);
        }

        [Fact]
        public void TarjetasAltaConsultaYBaja()
        {
            var daoTarjetas = new DaoTarjetas();
            var tarjeta = new Tarjeta
            {
                Usuario = new Usuario { IdUsuario = 5 },
                TitularTarjeta = "Nom Uno",
                NombreTarjeta = "Visa",
                TipoTarjeta = true,
                NumTarjeta = 4567123412340012,
                FechaVencimiento = "01/12/2099",
                CodigoSeguridad = 321
            };

            Assert.False(daoTarjetas.ExisteTarjeta(tarjeta));
            Assert.Equal(1, daoTarjetas.AgregarTarjeta(tarjeta));
            Assert.True(daoTarjetas.ExisteTarjeta(tarjeta));

            // Un número de 16 dígitos se guarda completo (antes se truncaba a int).
            Assert.Equal(4567123412340012L, daoTarjetas.GetTablaTarjeta(tarjeta).Rows[0]["Num_Tarjeta_t"]);
            var tabla = daoTarjetas.ObtenerTarjetasUsuario(new Usuario { IdUsuario = 5 });
            Assert.Equal("Crédito", tabla.Rows[0]["Tipo_Tarjeta_t"]);
            Assert.Equal("0012", tabla.Rows[0]["Num_Tarjeta_t"]);
            Assert.Equal(new DateTime(2099, 12, 1), tabla.Rows[0]["Fecha_Vencimiento_t"]);
            // Lo que llega a las páginas no incluye el código de seguridad.
            Assert.False(tabla.Columns.Contains("Codigo_Seguridad_t"));

            long id = Convert.ToInt64(tabla.Rows[0]["IdTarjeta_t"]);
            Assert.Equal(321, daoTarjetas.ObtenerTarjetaVigente(new Usuario { IdUsuario = 5 }, id).CodigoSeguridad);
            Assert.True(daoTarjetas.ObtenerTarjetaVigente(new Usuario { IdUsuario = 5 }, id).TipoTarjeta);
            Assert.Null(daoTarjetas.ObtenerTarjetaVigente(new Usuario { IdUsuario = 6 }, id));

            // Se elimina por id, y solo dentro de las tarjetas del usuario.
            Assert.Equal(0, daoTarjetas.EliminarTarjeta(new Tarjeta { Usuario = new Usuario { IdUsuario = 6 }, IdTarjeta = id }));
            Assert.Equal(1, daoTarjetas.EliminarTarjeta(new Tarjeta { Usuario = new Usuario { IdUsuario = 5 }, IdTarjeta = id }));
            Assert.False(daoTarjetas.ExisteTarjeta(tarjeta));
        }

        [Fact]
        public void NoPermiteAgregarUnaTarjetaVencida()
        {
            var vencida = new Tarjeta
            {
                Usuario = new Usuario { IdUsuario = 5 },
                TitularTarjeta = "Nom Uno",
                NombreTarjeta = "Visa",
                TipoTarjeta = false,
                NumTarjeta = 1234,
                FechaVencimiento = "01/01/2020",
                CodigoSeguridad = 111
            };

            var error = Assert.Throws<SQLiteException>(() => new DaoTarjetas().AgregarTarjeta(vencida));
            Assert.Contains("vencida", error.Message);
        }
    }
}
