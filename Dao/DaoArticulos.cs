using System;
using System.Data;
using System.Data.SQLite;
using Entidades;


namespace Dao
{
    public class DaoArticulos
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();
        private const string TodoArticulos = "select * from Articulos";

        public DataTable GetTablaArticuloPorCodigo(Articulo articulo) //Devuelve un articulo
        {
            string consulta = TodoArticulos + " where CodArticulo_ar = @CodArticulo";
            DataTable tabla = _accesoDatos.ObtenerTabla("Articulos", consulta, new SQLiteParameter("@CodArticulo", articulo.CodigoArticulo));
            return tabla;
        }

        /*---------------------------*/

        /* Lo que ve el cliente: solo artículos activos de categorías activas,
           opcionalmente filtrados por texto o por categoría. */
        public DataTable GetTablaArticulosCatalogo(string busqueda, int? categoria)
        {
            string consulta = "select a.*, c.Descripcion_ca as Categoria from Articulos a inner join Categorias c on c.IdCategoria_ca = a.Categoria_ar " +
                "where a.Estado_ar = 1 and c.Estado_ca = 1 " +
                "and (@Busqueda is null or a.Descripcion_ar like '%' || @Busqueda || '%') " +
                "and (@Categoria is null or a.Categoria_ar = @Categoria) " +
                "order by a.rowid";
            return _accesoDatos.ObtenerTabla("Articulos", consulta,
                new SQLiteParameter("@Busqueda", string.IsNullOrWhiteSpace(busqueda) ? (object)DBNull.Value : busqueda.Trim()),
                new SQLiteParameter("@Categoria", categoria.HasValue ? (object)categoria.Value : DBNull.Value));
        }

        public DataTable GetTablaArticulos() // Devuelve todos los articulos
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Articulos", TodoArticulos);
            return tabla;
        }

        public DataTable GetTablaReporteArticulos()
        {
            string consulta = "select CodArticulo_dv AS Codigo,Descripcion_ar AS Descripcion, SUM(Cantidad_dv) AS CantidadTotal,  sum(Cantidad_dv *Precio_Unitario_dv ) as totalventa from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv group by CodArticulo_dv,Descripcion_ar order by CodArticulo_dv,Descripcion_ar asc";
            DataTable tabla = _accesoDatos.ObtenerTabla("Detalle_Ventas", consulta);
            return tabla;
        }

        public bool ActualizarArticulo(Articulo articulo)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametrosArticuloAgregar(comando, articulo);
            comando.Parameters.Add("@Estado", DbType.Boolean).Value = articulo.Estado;
            AccesoDatos ad = new AccesoDatos();
            // ex sp_ModificarArticulo (que no actualizaba el estado: no había forma de reactivar un artículo)
            int filasActualizadas = ad.EjecutarComando(comando,
                "update Articulos set Categoria_ar = @Categoria, Descripcion_ar = @Descripcion, Stock_ar = @Stock, PrecioUnitario_ar = @PrecioUnitario, Imagen = @ImagenURL, Estado_ar = @Estado where CodArticulo_ar = @CodArticulo");
            return filasActualizadas == 1;
        }


        public bool ExisteArticulo(Articulo articulo)
        {
            string consulta = TodoArticulos + " where CodArticulo_ar = @CodArticulo";
            return _accesoDatos.Existe(consulta, new SQLiteParameter("@CodArticulo", articulo.CodigoArticulo));
        }

        public bool ExisteArticuloDescripcion(Articulo articulo)
        {
            string consulta = TodoArticulos + " where Descripcion_ar = @Descripcion";
            return _accesoDatos.Existe(consulta, new SQLiteParameter("@Descripcion", articulo.Descripcion));
        }

        public bool ExisteArticuloDescripcionModificar(Articulo articulo)
        {
            string consulta = TodoArticulos + " where Descripcion_ar = @Descripcion and CodArticulo_ar <> @CodArticulo";
            return _accesoDatos.Existe(consulta, new SQLiteParameter("@Descripcion", articulo.Descripcion), new SQLiteParameter("@CodArticulo", articulo.CodigoArticulo));
        }

        public int EliminarArticulo(Articulo articulo)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroArticulosEliminar(comando, articulo);
            // ex sp_EliminarArticulo
            return _accesoDatos.EjecutarComando(comando, "update Articulos set Estado_ar = 0 where CodArticulo_ar = @CODARTICULO_AR");
        }

        public int AgregarArticulo(Articulo articulo)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametrosArticuloAgregar(comando, articulo);
            // ex sp_AgregarArticulo
            return _accesoDatos.EjecutarComando(comando,
                "insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen) values (@CodArticulo,@Categoria,@Descripcion,@Stock,@PrecioUnitario,@ImagenURL)");
        }

        private void ArmarParametroArticulosEliminar(SQLiteCommand comando, Articulo articulo)
        {
            comando.Parameters.Add("@CODARTICULO_AR", DbType.String).Value = articulo.CodigoArticulo;
        }



        private void ArmarParametrosArticuloAgregar(SQLiteCommand comando, Articulo articulo)
        {
            comando.Parameters.Add("@CodArticulo", DbType.String).Value = articulo.CodigoArticulo;
            comando.Parameters.Add("@Categoria", DbType.Int64).Value = articulo.Categoria.IdCategoria;
            comando.Parameters.Add("@Descripcion", DbType.String).Value = articulo.Descripcion;
            comando.Parameters.Add("@Stock", DbType.Int64).Value = articulo.Stock;
            comando.Parameters.Add("@PrecioUnitario", DbType.Decimal).Value = articulo.PrecioUnitario;
            comando.Parameters.Add("@ImagenURL", DbType.String).Value = articulo.ImagenUrl;
        }
    }
}
