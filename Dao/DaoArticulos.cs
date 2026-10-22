using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using Entidades;


namespace Dao
{
    public class DaoArticulos
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoArticulos ="select * from Articulos";

        public Articulo ObtenerArticulo(Articulo art) //Devuelve un objeto articulo
        {
            DataTable tabla = ds.ObtenerTabla("Articulos","select CodArticulo_ar,Categoria_r,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen_ar from Articulos where CodArticulo = @CodArticulo",
                new SQLiteParameter("@CodArticulo", art.CodArticulo1));
            art.CodArticulo1 = tabla.Rows[0][0].ToString();
            art.Categoria1.IdCategoria1 = Convert.ToInt32(tabla.Rows[0][1].ToString());
            art.Descripcion1 = tabla.Rows[0][2].ToString();
            art.Stock1 = Convert.ToInt32(tabla.Rows[0][3].ToString());
            art.Precio_Unitario1 = Convert.ToInt32(tabla.Rows[0][4].ToString());
            art.ImagenURL1 = tabla.Rows[0][5].ToString();
            art.Estado1 = Convert.ToBoolean(tabla.Rows[0][6].ToString());
            return art;
        }

        public DataTable GetTablaArticuloCOD(Articulo art) //Devuelve un articulo
        {
            String NuevaConsul = TodoArticulos + " where CodArticulo_ar = @CodArticulo";
            DataTable tabla = ds.ObtenerTabla("Articulos", NuevaConsul, new SQLiteParameter("@CodArticulo", art.CodArticulo1));
            return tabla;
        }

        /* Buscar articulo*/
        public DataTable GetTablaArticuloBuscar(Articulo art) //Devuelve un articulo
        {
            String NuevaConsul = TodoArticulos + " where Descripcion_ar LIKE '%' || @Descripcion || '%'";
            DataTable tabla = ds.ObtenerTabla("Articulos", NuevaConsul, new SQLiteParameter("@Descripcion", art.Descripcion1));
            return tabla;
        }

        public DataTable GetTablaArticuloxCat(Articulo art)
        {
            String nuevaConsul = TodoArticulos + " where Categoria_ar = @Categoria";
            DataTable tabla = ds.ObtenerTabla("Articulos", nuevaConsul, new SQLiteParameter("@Categoria", art.Categoria1.IdCategoria1));
            return tabla;
        }


        public Articulo getIdxdescripcion(Articulo art)
        {

            String NuevaConsul = "Select CodArticulo_ar from Articulos where Descripcion_ar = @Descripcion";
            DataTable tabla = ds.ObtenerTabla("Articulos", NuevaConsul, new SQLiteParameter("@Descripcion", art.Descripcion1));
            art.CodArticulo1 = (tabla.Rows[0][0]).ToString();
            return art;
        }
        /*---------------------------*/

        public DataTable getTablaArticulos() // Devuelve todos los articulos
        {
            DataTable tabla = ds.ObtenerTabla("Articulos",TodoArticulos);
            return tabla;
        }

        public DataTable getTablaReporteArtiuculos()
        {
            String NuevaConsul = "select CodArticulo_dv AS Codigo,Descripcion_ar AS Descripcion, SUM(Cantidad_dv) AS CantidadTotal,  sum(Cantidad_dv *Precio_Unitario_dv ) as totalventa from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv group by CodArticulo_dv,Descripcion_ar order by CodArticulo_dv,Descripcion_ar asc";
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas",NuevaConsul);
            return tabla;
        }

        public DataTable getTablaReportePorArtiuculos(Articulo art)
        {
            String NuevaConsul = "select CodArticulo_dv,Descripcion_ar, SUM(Cantidad_dv) AS CantidadTotal,  sum(Cantidad_dv *Precio_Unitario_dv ) as totalventa from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv group by CodArticulo_dv,Descripcion_ar HAVING CodArticulo_dv = @CodArticulo";
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", NuevaConsul, new SQLiteParameter("@CodArticulo", art.CodArticulo1));
            return tabla;
        }

        public DataTable getTopArticulos()
        {
            String NuevaConsul = "select CodArticulo_dv AS Codigo, Descripcion_ar AS Descripcion, SUM(Cantidad_dv) AS CantidadTotal, sum(Cantidad_dv * Precio_Unitario_dv ) as totalventa from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv group by CodArticulo_dv,Descripcion_ar limit 10";
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", NuevaConsul);
            return tabla;
        }

        public DataTable OrdenarArticulosx(int op)
        {
            string NuevaConsulta = "asdasdasd";
              DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", NuevaConsulta);
            return tabla;
        }

        public bool ActualizarArticulo(Articulo art)
        {
            SQLiteCommand Comando = new SQLiteCommand();
            ArmarParametrosArticuloAgregar(ref Comando,art);
            AccesoDatos ad = new AccesoDatos();
            // ex sp_ModificarArticulo
            int FilasInsertadas = ad.EjecutarComando(Comando,
                "update Articulos set Categoria_ar = @Categoria, Descripcion_ar = @Descripcion, Stock_ar = @Stock, PrecioUnitario_ar = @PrecioUnitario, Imagen = @ImagenURL where CodArticulo_ar = @CodArticulo");
            if (FilasInsertadas == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable ObtenerTablaMaximo(Articulo art)
          {
            String ConsultaMaximo = "SELECT sum(Precio_Unitario_dv) AS [VENTAS TOTALES], sum(Cantidad_dv) AS [UNIDADES VENDIDAS], Stock_ar from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv GROUP BY Precio_Unitario_dv, Cantidad_dv, Stock_ar, CodArticulo_dv HAVING CodArticulo_dv = @CodArticulo";
            DataTable Maximo = ds.ObtenerTabla("Articulos", ConsultaMaximo, new SQLiteParameter("@CodArticulo", art.CodArticulo1));
             return Maximo;
          }


        public Boolean ExisteArticulo(Articulo art)
        {
            string consulta = TodoArticulos + " where CodArticulo_ar = @CodArticulo";
            return ds.existe(consulta, new SQLiteParameter("@CodArticulo", art.CodArticulo1));
        }

        public Boolean ExisteArticuloDescripcion(Articulo art)
        {
            string consulta = TodoArticulos + " where Descripcion_ar = @Descripcion";
            return ds.existe(consulta, new SQLiteParameter("@Descripcion", art.Descripcion1));
        }

        public Boolean ExisteArticuloDescripcionModificar(Articulo art)
        {
            string consulta = TodoArticulos + " where Descripcion_ar = @Descripcion and CodArticulo_ar <> @CodArticulo";
            return ds.existe(consulta, new SQLiteParameter("@Descripcion", art.Descripcion1), new SQLiteParameter("@CodArticulo", art.CodArticulo1));
        }

        public int eliminarArticulo(Articulo art)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroArticulosEliminar(ref comando, art);
            // ex sp_EliminarArticulo
            return ds.EjecutarComando(comando, "update Articulos set Estado_ar = 0 where CodArticulo_ar = @CODARTICULO_AR");
        }

        public int agregarArticulo(Articulo art)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametrosArticuloAgregar(ref comando, art);
            // ex sp_AgregarArticulo
            return ds.EjecutarComando(comando,
                "insert into Articulos(CodArticulo_ar,Categoria_ar,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen) values (@CodArticulo,@Categoria,@Descripcion,@Stock,@PrecioUnitario,@ImagenURL)");
        }

        private void ArmarParametroArticulosEliminar(ref SQLiteCommand comando, Articulo art)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@CODARTICULO_AR", DbType.String);
            Parametros.Value = art.CodArticulo1;
        }



        private void ArmarParametrosArticuloAgregar(ref SQLiteCommand comando, Articulo art)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@CodArticulo", DbType.String);
            Parametros.Value = art.CodArticulo1;
            Parametros = comando.Parameters.Add("@Categoria", DbType.Int64);
            Parametros.Value = art.Categoria1.IdCategoria1;
            Parametros = comando.Parameters.Add("@Descripcion", DbType.String);
            Parametros.Value = art.Descripcion1;
            Parametros = comando.Parameters.Add("@Stock", DbType.Int64);
            Parametros.Value = art.Stock1;
            Parametros = comando.Parameters.Add("@PrecioUnitario", DbType.Decimal);
            Parametros.Value = art.Precio_Unitario1;
            Parametros = comando.Parameters.Add("@ImagenURL", DbType.String);
            Parametros.Value = art.ImagenURL1;
        }
    }
}
