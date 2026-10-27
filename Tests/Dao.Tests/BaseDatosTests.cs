using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dao;
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

        static Usuario Usuario(string nombre, string contraseña) => new Usuario { Nombre1 = nombre, Contraseña1 = contraseña };

        [Fact]
        public void CreaLaBaseConLosDatosDePruebaLaPrimeraVez()
        {
            Assert.False(File.Exists(AccesoDatos.RutaBaseDatos));

            Assert.Equal(7, new DaoUsuarios().getTablaUsuarios().Rows.Count);
            Assert.Equal(8, new DaoCategorias().getTablaCategorias().Rows.Count);
            Assert.Equal(68, new DaoArticulos().getTablaArticulos().Rows.Count);
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
            var fuente = new DaoCategorias().GetTablaCategoria(new Categoria { IdCategoria1 = 1 });
            var ssd = new DaoCategorias().GetTablaCategoria(new Categoria { IdCategoria1 = 8 });

            Assert.Equal("Fuente", fuente.Rows[0]["Descripcion_ca"]);
            Assert.Equal("Ssd", ssd.Rows[0]["Descripcion_ca"]);
        }

        [Fact]
        public void NoVuelveACargarDatosSiLaBaseYaExiste()
        {
            new GestionCategorias().agregarCategoria(new Categoria { Decripcion1 = "Monitores" });

            // Cambiar de ruta y volver obliga a repetir el chequeo de inicialización.
            string original = AccesoDatos.RutaBaseDatos;
            AccesoDatos.RutaBaseDatos = Path.Combine(carpeta, "otra.db");
            Assert.Equal(8, new DaoCategorias().getTablaCategorias().Rows.Count);
            AccesoDatos.RutaBaseDatos = original;
            Assert.Equal(9, new DaoCategorias().getTablaCategorias().Rows.Count);
        }

        [Fact]
        public void LosTiposSeMapeanComoEnSqlServer()
        {
            var tabla = new DaoUsuarios().getTablaUsuarios();
            var articulos = new DaoArticulos().getTablaArticulos();

            Assert.Equal(typeof(bool), tabla.Columns["Tipo_us"].DataType);
            Assert.Equal(typeof(decimal), articulos.Columns["PrecioUnitario_ar"].DataType);
            // La UI hace Convert.ToBoolean(x.ToString()), que falla con "1".
            Assert.True(Convert.ToBoolean(tabla.Rows[0]["Estado_us"].ToString()));
        }

        [Fact]
        public void LoginValidaUsuarioContraseñaYTipo()
        {
            var dao = new DaoUsuarios();

            Assert.True(dao.ExisteUsuarioLogin(Usuario("admAmador", "admpsw2")));
            Assert.True(dao.TipoUsuarioLogin(Usuario("admAmador", "admpsw2")));
            Assert.True(dao.ExisteUsuarioLogin(Usuario("Nom1", "ctr1")));
            Assert.False(dao.TipoUsuarioLogin(Usuario("Nom1", "ctr1")));
            Assert.False(dao.ExisteUsuarioLogin(Usuario("Nom1", "incorrecta")));
        }

        [Fact]
        public void LoginNoEsVulnerableASqlInjection()
        {
            var dao = new DaoUsuarios();

            Assert.False(dao.ExisteUsuarioLogin(Usuario("admAmador", "' or '1'='1")));
            Assert.False(dao.ExisteUsuarioLogin(Usuario("' or 1=1 --", "x")));
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
            var dao = new DaoUsuarios();
            var nom1 = dao.ObtenerUsuario(new Usuario { IdUsuario1 = 5 });

            nom1.Contraseña1 = null;
            Assert.Equal(1, dao.actualizarUsuario(nom1));
            Assert.True(dao.ExisteUsuarioLogin(Usuario("Nom1", "ctr1")));

            nom1.Contraseña1 = "nueva123";
            Assert.Equal(1, dao.actualizarUsuario(nom1));
            Assert.False(dao.ExisteUsuarioLogin(Usuario("Nom1", "ctr1")));
            Assert.True(dao.ExisteUsuarioLogin(Usuario("Nom1", "nueva123")));
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
            var nuevo = new Usuario { Nombre1 = "nuevo", Contraseña1 = "clave1", Email1 = "nuevo@mail.com", Telefono1 = "123", Tipo1 = false };

            Assert.True(gestion.agregarUsuario(nuevo));
            Assert.False(gestion.agregarUsuario(nuevo));
            nuevo.IdUsuario1 = gestion.DevolverIDusuario(nuevo);
            Assert.Equal(10, nuevo.IdUsuario1);

            nuevo.Email1 = "otro@mail.com";
            nuevo.Estado1 = true;
            Assert.True(gestion.ActualizarUsuario(nuevo));
            var guardado = new DaoUsuarios().ObtenerUsuario(new Usuario { IdUsuario1 = nuevo.IdUsuario1 });
            Assert.Equal("otro@mail.com", guardado.Email1);
            Assert.True(guardado.Estado1);
        }

        [Fact]
        public void EliminarUsuarioEsBajaLogica()
        {
            var us = new Usuario { IdUsuario1 = 5 };

            Assert.Equal(1, new DaoUsuarios().eliminarUsuario(ref us));
            Assert.False(new DaoUsuarios().ObtenerUsuario(new Usuario { IdUsuario1 = 5 }).Estado1);
        }

        [Fact]
        public void CategoriasAltaModificacionYBaja()
        {
            var dao = new DaoCategorias();

            Assert.Equal(1, dao.agregarCategoria(new Categoria { Decripcion1 = "Monitores" }));
            Assert.True(dao.ExisteCategoriaDescripcion(new Categoria { Decripcion1 = "monitores" }));
            Assert.True(dao.ExisteCategoriaModificar(new Categoria { IdCategoria1 = 1, Decripcion1 = "Monitores" }));
            Assert.False(dao.ExisteCategoriaModificar(new Categoria { IdCategoria1 = 9, Decripcion1 = "Monitores" }));

            Assert.True(dao.ActualizarCategoria(new Categoria { IdCategoria1 = 9, Decripcion1 = "Monitores 4K", Estado1 = true }));
            Assert.Equal(1, dao.eliminarCategoria(new Categoria { IdCategoria1 = 9 }));
            Assert.False(dao.DevolverEstadoCat(new Categoria { IdCategoria1 = 9 }));
        }

        [Fact]
        public void ArticulosAltaBusquedaYBaja()
        {
            var dao = new DaoArticulos();
            var art = new Articulo
            {
                CodArticulo1 = "mn1", Categoria1 = new Categoria { IdCategoria1 = 1 }, Descripcion1 = "Monitor 24'' LG",
                Stock1 = 10, Precio_Unitario1 = 1500.5f, ImagenURL1 = "~/Imagenes_ar/mn1.png"
            };

            Assert.Equal(1, dao.agregarArticulo(art));
            Assert.True(dao.ExisteArticulo(new Articulo { CodArticulo1 = "mn1" }));
            Assert.True(dao.ExisteArticuloDescripcion(new Articulo { Descripcion1 = "Monitor 24'' LG" }));
            Assert.False(dao.ExisteArticuloDescripcionModificar(new Articulo { CodArticulo1 = "mn1", Descripcion1 = "Monitor 24'' LG" }));
            Assert.True(dao.ExisteArticuloDescripcionModificar(new Articulo { CodArticulo1 = "gb1", Descripcion1 = "Monitor 24'' LG" }));

            Assert.Single(dao.GetTablaArticuloBuscar(new Articulo { Descripcion1 = "24'' lg" }).Rows);
            Assert.Equal(9, dao.GetTablaArticuloxCat(new Articulo { Categoria1 = new Categoria { IdCategoria1 = 2 } }).Rows.Count);
            Assert.Equal("gb1", dao.getIdxdescripcion(new Articulo { Descripcion1 = "Gabinete Gamemax" }).CodArticulo1);

            Assert.Equal(1, dao.eliminarArticulo(new Articulo { CodArticulo1 = "mn1" }));
            Assert.False((bool)dao.GetTablaArticuloCOD(new Articulo { CodArticulo1 = "mn1" }).Rows[0]["Estado_ar"]);
        }

        [Fact]
        public void ActualizarArticuloGuardaLaCategoria()
        {
            var dao = new DaoArticulos();
            var art = new Articulo
            {
                CodArticulo1 = "gb1", Categoria1 = new Categoria { IdCategoria1 = 3 }, Descripcion1 = "Gabinete Gamemax",
                Stock1 = 10, Precio_Unitario1 = 3500, ImagenURL1 = "~/Imagenes_ar/Gabinete/gb1.png", Estado1 = true
            };

            Assert.True(dao.ActualizarArticulo(art));
            Assert.Equal(3L, dao.GetTablaArticuloCOD(new Articulo { CodArticulo1 = "gb1" }).Rows[0]["Categoria_ar"]);
        }

        [Fact]
        public void ObtenerArticuloCargaTodosLosCampos()
        {
            var art = new DaoArticulos().ObtenerArticulo(new Articulo { CodArticulo1 = "gb1" });

            Assert.Equal(2, art.Categoria1.IdCategoria1);
            Assert.Equal("Gabinete Gamemax", art.Descripcion1);
            Assert.Equal(10, art.Stock1);
            Assert.Equal(3500f, art.Precio_Unitario1);
            Assert.Equal("~/Imagenes_ar/Gabinete/gb1.png", art.ImagenURL1);
            Assert.True(art.Estado1);
        }

        [Fact]
        public void ObtenerCategoriaCargaTodosLosCampos()
        {
            var cat = new DaoCategorias().ObtenerCategoria(new Categoria { IdCategoria1 = 8 });

            Assert.Equal("Ssd", cat.Decripcion1);
            Assert.True(cat.Estado1);
        }

        [Fact]
        public void ObtenerVentaCargaTodosLosCampos()
        {
            var dao = new DaoVentas();
            dao.agregarVenta(new Ventas { Usuario1 = new Usuario { IdUsuario1 = 5 }, Metodo_Pago1 = true, Cod_Seguridad1 = 456 });
            int id = dao.IDultimaventa(new Ventas()).IdVenta1;

            var venta = dao.ObtenerVenta(new Ventas { IdVenta1 = id });

            Assert.Equal(5, venta.Usuario1.IdUsuario1);
            Assert.Equal(0f, venta.TotalVenta1);
            Assert.True(venta.Metodo_Pago1);
            Assert.Equal(456, venta.Cod_Seguridad1);
        }

        [Fact]
        public void ActualizarArticuloPermiteReactivarlo()
        {
            var dao = new DaoArticulos();
            dao.eliminarArticulo(new Articulo { CodArticulo1 = "gb1" });
            var art = new Articulo
            {
                CodArticulo1 = "gb1", Categoria1 = new Categoria { IdCategoria1 = 2 }, Descripcion1 = "Gabinete Gamemax",
                Stock1 = 10, Precio_Unitario1 = 3500, ImagenURL1 = "~/Imagenes_ar/Gabinete/gb1.png", Estado1 = true
            };

            Assert.True(dao.ActualizarArticulo(art));
            Assert.True((bool)dao.GetTablaArticuloCOD(new Articulo { CodArticulo1 = "gb1" }).Rows[0]["Estado_ar"]);
        }

        [Fact]
        public void ElCatalogoSoloMuestraArticulosActivosDeCategoriasActivas()
        {
            var dao = new DaoArticulos();
            dao.eliminarArticulo(new Articulo { CodArticulo1 = "gb1" });
            new DaoCategorias().eliminarCategoria(new Categoria { IdCategoria1 = 1 }); // Fuente: 9 artículos

            var catalogo = dao.getTablaArticulosCatalogo(null, null);

            Assert.Equal(68 - 1 - 9, catalogo.Rows.Count);
            Assert.DoesNotContain(catalogo.Rows.Cast<DataRow>(), f => (string)f["CodArticulo_ar"] == "gb1" || (long)f["Categoria_ar"] == 1);
            Assert.Equal(8, dao.getTablaArticulosCatalogo(null, 2).Rows.Count);
            Assert.Empty(dao.getTablaArticulosCatalogo(null, 1).Rows);
            Assert.Equal(2, dao.getTablaArticulosCatalogo("ryzen", null).Rows.Count);
            Assert.Empty(dao.getTablaArticulosCatalogo("Deepcool 700W", null).Rows);
        }

        [Fact]
        public void UnaCompraDescuentaStockYSumaElTotal()
        {
            var ventas = new DaoVentas();
            var detalle = new daoDetalleVentas();

            Assert.Equal(1, ventas.agregarVenta(new Ventas { Usuario1 = new Usuario { IdUsuario1 = 5 }, Metodo_Pago1 = true, Cod_Seguridad1 = 123 }));
            int idVenta = ventas.IDultimaventa(new Ventas()).IdVenta1;
            Assert.Equal(1, detalle.agregarDetalleVenta(new DetalleVentas
            {
                IdDetVenta1 = idVenta, IdUsuario1 = 5, CodArticulo1 = "gb1", Cantidad = 2, Precio1 = 3500,
                DireccionEntrega1 = "Calle 123", Contacto1 = "1122334455"
            }));

            Assert.Equal(8L, new DaoArticulos().GetTablaArticuloCOD(new Articulo { CodArticulo1 = "gb1" }).Rows[0]["Stock_ar"]);
            Assert.Equal(7000m, ventas.GetTablaVentas(new Ventas { IdVenta1 = idVenta }).Rows[0]["TotalVenta_ve"]);
            Assert.IsType<DateTime>(ventas.GetTablaVentas(new Ventas { IdVenta1 = idVenta }).Rows[0]["FechaVenta_ve"]);
            Assert.Single(detalle.GetTablaIdVentaUsuario(new Ventas { IdVenta1 = idVenta }).Rows);

            // Reportes: verifican que el SQL traducido de T-SQL corre en SQLite.
            var dao = new DaoArticulos();
            Assert.Single(dao.getTopArticulos().Rows);
            Assert.Single(dao.getTablaReporteArtiuculos().Rows);
            Assert.Single(dao.getTablaReportePorArtiuculos(new Articulo { CodArticulo1 = "gb1" }).Rows);
            Assert.Equal(2L, dao.ObtenerTablaMaximo(new Articulo { CodArticulo1 = "gb1" }).Rows[0]["UNIDADES VENDIDAS"]);
            Assert.Single(ventas.getMaximoVenta().Rows);
            Assert.Single(detalle.getMaximaCantidad().Rows);
        }

        [Fact]
        public void TarjetasAltaConsultaYBaja()
        {
            var dao = new DaoTarjetas();
            var tarjeta = new Tarjeta
            {
                Us = new Usuario { IdUsuario1 = 5 }, Titular_Tarjeta1 = "Nom Uno", Nombre_Tarjeta1 = "Visa",
                Tipo_Tarjeta1 = true, Num_Tarjeta1 = 4567123412340012, Fecha_Vencimiento1 = "01/12/2099", Codigo_Seguridad1 = 321
            };

            Assert.False(dao.ExisteTarjeta(tarjeta));
            Assert.Equal(1, dao.AgregarTarjeta(tarjeta));
            Assert.True(dao.ExisteTarjeta(tarjeta));

            // Un número de 16 dígitos se guarda completo (antes se truncaba a int).
            Assert.Equal(4567123412340012L, dao.getTablaTarjeta(tarjeta).Rows[0]["Num_Tarjeta_t"]);
            var tabla = dao.ObtenerTarjetasUsuario(new Usuario { IdUsuario1 = 5 });
            Assert.Equal("Crédito", tabla.Rows[0]["Tipo_Tarjeta_t"]);
            Assert.Equal("0012", tabla.Rows[0]["Num_Tarjeta_t"]);
            Assert.Equal(new DateTime(2099, 12, 1), tabla.Rows[0]["Fecha_Vencimiento_t"]);
            // Lo que llega a las páginas no incluye el código de seguridad.
            Assert.False(tabla.Columns.Contains("Codigo_Seguridad_t"));

            long id = Convert.ToInt64(tabla.Rows[0]["IdTarjeta_t"]);
            Assert.Equal(321, dao.ObtenerTarjetaVigente(new Usuario { IdUsuario1 = 5 }, id).Codigo_Seguridad1);
            Assert.True(dao.ObtenerTarjetaVigente(new Usuario { IdUsuario1 = 5 }, id).Tipo_Tarjeta1);
            Assert.Null(dao.ObtenerTarjetaVigente(new Usuario { IdUsuario1 = 6 }, id));

            // Se elimina por id, y solo dentro de las tarjetas del usuario.
            Assert.Equal(0, dao.EliminarTarjeta(new Tarjeta { Us = new Usuario { IdUsuario1 = 6 }, IdTarjeta1 = id }));
            Assert.Equal(1, dao.EliminarTarjeta(new Tarjeta { Us = new Usuario { IdUsuario1 = 5 }, IdTarjeta1 = id }));
            Assert.False(dao.ExisteTarjeta(tarjeta));
        }

        [Fact]
        public void NoPermiteAgregarUnaTarjetaVencida()
        {
            var vencida = new Tarjeta
            {
                Us = new Usuario { IdUsuario1 = 5 }, Titular_Tarjeta1 = "Nom Uno", Nombre_Tarjeta1 = "Visa",
                Tipo_Tarjeta1 = false, Num_Tarjeta1 = 1234, Fecha_Vencimiento1 = "01/01/2020", Codigo_Seguridad1 = 111
            };

            var error = Assert.Throws<SQLiteException>(() => new DaoTarjetas().AgregarTarjeta(vencida));
            Assert.Contains("vencida", error.Message);
        }
    }
}
