using System;
using System.Data.SQLite;
using System.IO;
using Dao;
using Entidades;
using Xunit;

namespace Dao.Tests
{
    /* Creación de la base con el historial de demo y migración de bases viejas. */
    public class VersionadoTests : IDisposable
    {
        readonly string carpeta = Path.Combine(Path.GetTempPath(), "pcbooster-version-" + Guid.NewGuid().ToString("N"));

        public VersionadoTests()
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

        object Escalar(string sql)
        {
            using var conexion = new SQLiteConnection("Data Source=" + AccesoDatos.RutaBaseDatos);
            conexion.Open();
            return new SQLiteCommand(sql, conexion).ExecuteScalar();
        }

        void Ejecutar(string sql)
        {
            using (var conexion = new SQLiteConnection("Data Source=" + AccesoDatos.RutaBaseDatos))
            {
                conexion.Open();
                new SQLiteCommand(sql, conexion).ExecuteNonQuery();
            }
            SQLiteConnection.ClearAllPools();
        }

        /* Obliga a AccesoDatos a volver a revisar el archivo, como al reiniciar la app. */
        void ReiniciarApp()
        {
            string ruta = AccesoDatos.RutaBaseDatos;
            AccesoDatos.RutaBaseDatos = Path.Combine(carpeta, "otra.db");
            new DaoCategorias().GetTablaCategorias();
            AccesoDatos.RutaBaseDatos = ruta;
        }

        [Fact]
        public void UnaBaseNuevaIncluyeElHistorialDeDemo()
        {
            AccesoDatos.CargarDatosDemo = true;

            Assert.Equal(27, new DaoUsuarios().GetTablaUsuarios().Rows.Count);
            Assert.True(Convert.ToInt64(Escalar("select count(*) from Ventas")) > 100);
            Assert.Equal(AccesoDatos.VersionSchema, Convert.ToInt32(Escalar("pragma user_version")));
            // Ninguna venta en el futuro ni fuera de los últimos 6 meses.
            Assert.Equal(0L, Escalar("select count(*) from Ventas where FechaVenta_ve > datetime('now','localtime') or FechaVenta_ve < datetime('now','localtime','-181 days')"));
            // El total de cada venta (calculado por tr_SumaTotal) coincide con sus detalles.
            Assert.Equal(0L, Escalar("select count(*) from Ventas v where TotalVenta_ve <> (select sum(Cantidad_dv * Precio_Unitario_dv) from Detalle_Ventas where IdDetVenta_dv = v.IdVentas_ve)"));
            // Ningún cliente compra antes de darse de alta.
            Assert.Equal(0L, Escalar("select count(*) from Ventas v join Usuarios u on u.IdUsuario_us = v.IdUsuario_ve where v.FechaVenta_ve < u.FechaAlta_us"));
        }

        [Fact]
        public void UnaBaseSinVersionarSeMigraSinPerderDatos()
        {
            // Simula una base creada antes del versionado: sin FechaAlta_us y con la imagen rota.
            new DaoCategorias().AgregarCategoria(new Categoria { Descripcion = "Monitores" });
            Ejecutar("alter table Usuarios drop column FechaAlta_us; " +
                     "alter table Articulos drop column Marca_ar; alter table Articulos drop column DescripcionLarga_ar; " +
                     "alter table Articulos drop column Especificaciones_ar; alter table Articulos drop column Garantia_ar; " +
                     "update Articulos set Imagen = '~/Imagenes_ar/Fuente/fnt8png' where CodArticulo_ar = 'fnt8'; " +
                     "pragma user_version = 0;");

            ReiniciarApp();
            Assert.Equal(1, new DaoUsuarios().AgregarUsuario(new Usuario { Nombre = "nuevo", Contrasena = "x", Email = "n@x.com", Telefono = "1" }));

            Assert.Equal(AccesoDatos.VersionSchema, Convert.ToInt32(Escalar("pragma user_version")));
            Assert.Equal(0L, Escalar("select count(*) from Usuarios where FechaAlta_us is null"));
            Assert.Equal("~/Imagenes_ar/Fuente/fnt8.png", Escalar("select Imagen from Articulos where CodArticulo_ar = 'fnt8'"));
            Assert.Equal(1L, Escalar("select count(*) from Categorias where Descripcion_ca = 'Monitores'"));
            Assert.Equal(68L, Escalar("select count(*) from Articulos where DescripcionLarga_ar is not null"));
        }

        [Fact]
        public void TodosLosArticulosDePruebaTienenDetalle()
        {
            var catalogo = new DaoArticulos().GetTablaArticulosCatalogo(null, null);

            Assert.Equal(68, catalogo.Rows.Count);
            foreach (System.Data.DataRow fila in catalogo.Rows)
            {
                Assert.False(string.IsNullOrWhiteSpace(fila["Marca_ar"].ToString()), fila["CodArticulo_ar"].ToString());
                Assert.False(string.IsNullOrWhiteSpace(fila["Categoria"].ToString()));
                Assert.Contains(": ", fila["Especificaciones_ar"].ToString());
                Assert.True(Convert.ToInt32(fila["Garantia_ar"]) > 0);
            }
        }

        [Fact]
        public void LaMigracion3CorrigeLosTotalesInfladosPorElTriggerViejo()
        {
            // Base en versión 2, con el tr_SumaTotal original (filtraba por usuario).
            new DaoCategorias().GetTablaCategorias();
            Ejecutar("drop trigger tr_SumaTotal; " +
                     "create trigger tr_SumaTotal after insert on Detalle_Ventas begin " +
                     "update Ventas set TotalVenta_ve = TotalVenta_ve + new.Cantidad_dv * new.Precio_Unitario_dv where IdUsuario_ve = new.IdUsuario_dv; end; " +
                     "pragma user_version = 2;");
            int primera = Comprar("gb1", 3500);
            int segunda = Comprar("fnt1", 7300);
            Assert.Equal(10800m, Total(primera)); // inflado

            ReiniciarApp();
            new DaoCategorias().GetTablaCategorias();

            Assert.Equal(3500m, Total(primera));
            Assert.Equal(7300m, Total(segunda));
            int tercera = Comprar("gb2", 4750);
            Assert.Equal(4750m, Total(tercera));
            Assert.Equal(3500m, Total(primera));
        }

        decimal Total(int idVenta)
        {
            return Convert.ToDecimal(Escalar("select TotalVenta_ve from Ventas where IdVentas_ve = " + idVenta));
        }

        /* Una venta de Nom1 (id 5) con una unidad del artículo; devuelve el id de la venta. */
        static int Comprar(string codigo, int precio)
        {
            var ventas = new DaoVentas();
            ventas.agregarVenta(new Ventas { Usuario = new Usuario { IdUsuario = 5 }, MetodoPago = true, CodigoSeguridad = 1 });
            int id = ventas.IDultimaventa(new Ventas()).IdVenta;
            new DaoDetalleVentas().AgregarDetalleVenta(new DetalleVentas
            {
                IdDetalleVenta = id,
                IdUsuario = 5,
                CodigoArticulo = codigo,
                Cantidad = 1,
                Precio = precio,
                DireccionEntrega = "Calle 1",
                Contacto = "Yo"
            });
            return id;
        }

        [Fact]
        public void LaMigracion4HasheaLasContraseñasEnTextoPlano()
        {
            new DaoCategorias().GetTablaCategorias();
            Ejecutar("update Usuarios set Contraseña_us = 'ctr1' where Nombre_us = 'Nom1'; pragma user_version = 3;");

            ReiniciarApp();
            new DaoCategorias().GetTablaCategorias();

            Assert.Equal(AccesoDatos.VersionSchema, Convert.ToInt32(Escalar("pragma user_version")));
            Assert.Equal(0L, Escalar("select count(*) from Usuarios where Contraseña_us not like 'pbkdf2$%'"));
            Assert.True(new DaoUsuarios().ExisteUsuarioLogin(new Usuario { Nombre = "Nom1", Contrasena = "ctr1" }));
        }

        [Fact]
        public void UnaBaseAlDiaNoSeVuelveAMigrar()
        {
            new DaoCategorias().GetTablaCategorias();
            ReiniciarApp();

            Assert.Equal(7, new DaoUsuarios().GetTablaUsuarios().Rows.Count);
            Assert.Equal(AccesoDatos.VersionSchema, Convert.ToInt32(Escalar("pragma user_version")));
        }
    }
}
