using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidades;


namespace Dao
{
    public class DaoArticulos
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoArticulos ="select * from Articulos";  

        public Articulo ObtenerArticulo(Articulo art) //Devuelve un objeto articulo
        {
            DataTable tabla = ds.ObtenerTabla("Articulos","select CodArticulo_ar,Categoria_r,Descripcion_ar,Stock_ar,PrecioUnitario_ar,Imagen_ar from Articulos where CodArticulo = " + art.CodArticulo1);
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
            String NuevaConsul = TodoArticulos + " where CodArticulo_ar = " + "'"+ art.CodArticulo1 +"'";
            DataTable tabla = ds.ObtenerTabla("Articulos", NuevaConsul);
            return tabla;
        }

        /* Buscar articulo*/
        public DataTable GetTablaArticuloBuscar(Articulo art) //Devuelve un articulo
        {
            String NuevaConsul = TodoArticulos + " where Descripcion_ar LIKE '%" +  art.Descripcion1 + "%'";
            DataTable tabla = ds.ObtenerTabla("Articulos", NuevaConsul);
            return tabla;
        }

        public DataTable GetTablaArticuloxCat(Articulo art)
        {
            String nuevaConsul = TodoArticulos + " where Categoria_ar = '" + art.Categoria1.IdCategoria1 + "'";
            DataTable tabla = ds.ObtenerTabla("Articulos", nuevaConsul);
            return tabla;
        }


        public Articulo getIdxdescripcion(Articulo art)
        {
            
            String NuevaConsul = "Select CodArticulo_ar from Articulos where Descripcion_ar = '" + art.Descripcion1 + "'";
            DataTable tabla = ds.ObtenerTabla("Articulos", NuevaConsul);
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
            String NuevaConsul = "select CodArticulo_dv AS Codigo,Descripcion_ar AS Descripcion, SUM(Cantidad_dv) AS CantidadTotal,  sum(Cantidad_dv *Precio_Unitario_dv ) as totalventa from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv group by CodArticulo_dv,Descripcion_ar";
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas",NuevaConsul);
            return tabla;
        }
        
        public DataTable getTablaReportePorArtiuculos(Articulo art)
        {
            String NuevaConsul = "select CodArticulo_dv,Descripcion_ar, SUM(Cantidad_dv) AS CantidadTotal,  sum(Cantidad_dv *Precio_Unitario_dv ) as totalventa from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv group by CodArticulo_dv,Descripcion_ar HAVING CodArticulo_dv = '" + art.CodArticulo1 +"'";
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", NuevaConsul);
            return tabla;
        }

        public DataTable getTopArticulos()
        {
            String NuevaConsul = "select top(10) CodArticulo_dv AS Codigo, Descripcion_ar AS Descripcion, SUM(Cantidad_dv) AS CantidadTotal, sum(Cantidad_dv * Precio_Unitario_dv ) as totalventa from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv group by CodArticulo_dv,Descripcion_ar";
            DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", NuevaConsul);
            return tabla;
        }

        public DataTable OrdenarArticulosx(int op)
        {
            string NuevaConsulta =
              DataTable tabla = ds.ObtenerTabla("Detalle_Ventas", NuevaConsul);
            return tabla;
        }

        public bool ActualizarArticulo(Articulo art)
        {
            SqlCommand Comando = new SqlCommand();
            ArmarParametrosArticuloAgregar(ref Comando,art);
            AccesoDatos ad = new AccesoDatos();
            //cambie el procedimiento almacenado
            int FilasInsertadas = ad.EjecutarProcedimientoAlmacenado(Comando, "sp_ModificarArticulo");
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
            String ConsultaMaximo = "SELECT sum(Precio_Unitario_dv) AS [VENTAS TOTALES], sum(Cantidad_dv) AS [UNIDADES VENDIDAS], Stock_ar from Detalle_Ventas inner join Articulos on CodArticulo_ar = CodArticulo_dv GROUP BY Precio_Unitario_dv, Cantidad_dv, Stock_ar, CodArticulo_dv HAVING CodArticulo_dv =";
            ConsultaMaximo += "'" + art.CodArticulo1 + "'";
            DataTable Maximo = ds.ObtenerTabla("Articulos", ConsultaMaximo);
             return Maximo;
          }


        public Boolean ExisteArticulo(Articulo art) 
        {
            string consulta = TodoArticulos + " where CodArticulo_ar =" + "'"+ art.CodArticulo1 +"'";
            return ds.existe(consulta);
        }

        public Boolean ExisteArticuloDescripcion(Articulo art)
        {
            string consulta = TodoArticulos + " where Descripcion_ar =" + "'" + art.Descripcion1 + "'";
            return ds.existe(consulta);
        }

        public Boolean ExisteArticuloDescripcionModificar(Articulo art)
        {
            string consulta = TodoArticulos + " where Descripcion_ar =" + "'" + art.Descripcion1 + "' and CodArticulo_ar <> "+art.CodArticulo1;
            return ds.existe(consulta);
        }

        public int eliminarArticulo(Articulo art)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametroArticulosEliminar(ref comando, art);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_EliminarArticulo");
        }

        public int agregarArticulo(Articulo art)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametrosArticuloAgregar(ref comando, art);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_AgregarArticulo");
        }

        private void ArmarParametroArticulosEliminar(ref SqlCommand comando, Articulo art)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@CODARTICULO_AR", SqlDbType.Char);
            SqlParametros.Value = art.CodArticulo1;
        }



        private void ArmarParametrosArticuloAgregar(ref SqlCommand comando, Articulo art)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@CodArticulo", SqlDbType.Char);
            SqlParametros.Value = art.CodArticulo1;
            SqlParametros = comando.Parameters.Add("@Categoria", SqlDbType.BigInt);
            SqlParametros.Value = art.Categoria1.IdCategoria1;
            SqlParametros = comando.Parameters.Add("@Descripcion", SqlDbType.VarChar);
            SqlParametros.Value = art.Descripcion1;
            SqlParametros = comando.Parameters.Add("@Stock", SqlDbType.BigInt);
            SqlParametros.Value = art.Stock1;
            SqlParametros = comando.Parameters.Add("@PrecioUnitario", SqlDbType.Money);
            SqlParametros.Value = art.Precio_Unitario1;
            SqlParametros = comando.Parameters.Add("@ImagenURL", SqlDbType.VarChar);
            SqlParametros.Value = art.ImagenURL1;           
        }        
    }
}

