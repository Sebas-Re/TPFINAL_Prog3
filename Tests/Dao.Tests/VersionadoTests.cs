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
            using (var cn = new SQLiteConnection("Data Source=" + AccesoDatos.RutaBaseDatos))
            {
                cn.Open();
                return new SQLiteCommand(sql, cn).ExecuteScalar();
            }
        }

        void Ejecutar(string sql)
        {
            using (var cn = new SQLiteConnection("Data Source=" + AccesoDatos.RutaBaseDatos))
            {
                cn.Open();
                new SQLiteCommand(sql, cn).ExecuteNonQuery();
            }
            SQLiteConnection.ClearAllPools();
        }

        /* Obliga a AccesoDatos a volver a revisar el archivo, como al reiniciar la app. */
        void ReiniciarApp()
        {
            string ruta = AccesoDatos.RutaBaseDatos;
            AccesoDatos.RutaBaseDatos = Path.Combine(carpeta, "otra.db");
            new DaoCategorias().getTablaCategorias();
            AccesoDatos.RutaBaseDatos = ruta;
        }

        [Fact]
        public void UnaBaseNuevaIncluyeElHistorialDeDemo()
        {
            AccesoDatos.CargarDatosDemo = true;

            Assert.Equal(29, new DaoUsuarios().getTablaUsuarios().Rows.Count);
            Assert.True(Convert.ToInt64(Escalar("select count(*) from Ventas")) > 100);
            Assert.Equal(AccesoDatos.VersionSchema, Convert.ToInt32(Escalar("pragma user_version")));
            // Ninguna venta en el futuro ni fuera de los últimos 6 meses.
            Assert.Equal(0L, Escalar("select count(*) from Ventas where FechaVenta_ve > datetime('now','localtime') or FechaVenta_ve < datetime('now','localtime','-181 days')"));
            // El total de cada venta coincide con sus detalles (tr_SumaTotal acumula de más).
            Assert.Equal(0L, Escalar("select count(*) from Ventas v where TotalVenta_ve <> (select sum(Cantidad_dv * Precio_Unitario_dv) from Detalle_Ventas where IdDetVenta_dv = v.IdVentas_ve)"));
            // Ningún cliente compra antes de darse de alta.
            Assert.Equal(0L, Escalar("select count(*) from Ventas v join Usuarios u on u.IdUsuario_us = v.IdUsuario_ve where v.FechaVenta_ve < u.FechaAlta_us"));
        }

        [Fact]
        public void UnaBaseSinVersionarSeMigraSinPerderDatos()
        {
            // Simula una base creada antes del versionado: sin FechaAlta_us y con la imagen rota.
            new DaoCategorias().agregarCategoria(new Categoria { Decripcion1 = "Monitores" });
            Ejecutar("alter table Usuarios drop column FechaAlta_us; " +
                     "update Articulos set Imagen = '~/Imagenes_ar/Fuente/fnt8png' where CodArticulo_ar = 'fnt8'; " +
                     "pragma user_version = 0;");

            ReiniciarApp();
            Assert.Equal(1, new DaoUsuarios().agregarUsuario(new Usuario { Nombre1 = "nuevo", Contraseña1 = "x", Email1 = "n@x.com", Telefono1 = "1" }));

            Assert.Equal(AccesoDatos.VersionSchema, Convert.ToInt32(Escalar("pragma user_version")));
            Assert.Equal(0L, Escalar("select count(*) from Usuarios where FechaAlta_us is null"));
            Assert.Equal("~/Imagenes_ar/Fuente/fnt8.png", Escalar("select Imagen from Articulos where CodArticulo_ar = 'fnt8'"));
            Assert.Equal(1L, Escalar("select count(*) from Categorias where Descripcion_ca = 'Monitores'"));
        }

        [Fact]
        public void UnaBaseAlDiaNoSeVuelveAMigrar()
        {
            new DaoCategorias().getTablaCategorias();
            ReiniciarApp();

            Assert.Equal(9, new DaoUsuarios().getTablaUsuarios().Rows.Count);
            Assert.Equal(AccesoDatos.VersionSchema, Convert.ToInt32(Escalar("pragma user_version")));
        }
    }
}
