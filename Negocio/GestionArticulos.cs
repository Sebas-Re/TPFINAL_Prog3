using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidades;
using Dao;
using System.Windows.Forms;

namespace Negocio
{
    public class GestionArticulos
    {
        DaoArticulos dao = new DaoArticulos();
        public DataTable getArticulos() //devuelve todos los Articulos
        {
            return dao.getTablaArticulos();
        }

        public DataTable getReporteArticulos()
        {
            return dao.getTablaReporteArtiuculos();
        }

        public DataTable getReporteArticulo(Articulo art)
        {
            return dao.getTablaReportePorArtiuculos(art);
        }


        public DataTable CrearTablaArticulos()
        {
            DataTable dt = new DataTable();
            DataColumn Columna = new DataColumn("DESCRIPCION", Type.GetType("System.String"));
            dt.Columns.Add(Columna);

            Columna = new DataColumn("PRECIO UNITARIO", Type.GetType("System.Decimal"));
            dt.Columns.Add(Columna);

            Columna = new DataColumn("CANTIDAD", Type.GetType("System.String"));
            dt.Columns.Add(Columna);

            return dt;
        }

        public void AgregarFila(ref DataTable tabla, Articulo art)
        {
            DataRow dr = tabla.NewRow();
            dr["DESCRIPCION"] = art.Descripcion1;
            dr["PRECIO UNITARIO"] = art.Precio_Unitario1;
            dr["CANTIDAD"] = "1";

            tabla.Rows.Add(dr);
        }

        public DataTable getArticuloCOD(Articulo art) //Devuelve un Articulo
        {
            return dao.GetTablaArticuloCOD(art);
        }         

        public Articulo getarticuloXdescripcion(Articulo art)
        {
            return dao.getIdxdescripcion(art);
        }

        public DataTable getarticulosXCategoria(Articulo art)
        {
            return dao.GetTablaArticuloxCat(art);
        }

        public DataTable getArticuloBuscar(Articulo art) //Devuelve un Articulo
        {
            return dao.GetTablaArticuloBuscar(art);
        }

        public bool eliminarArticulo(Articulo art)
        {
            int eliminado = dao.eliminarArticulo(art);
            if (eliminado == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ActualizarArticulo(Articulo art)
        {
            bool eliminado = dao.ActualizarArticulo(art);
            if (eliminado)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public DataTable ObtenerMaximo(Articulo art)
        {
            DataTable Maximo = dao.ObtenerTablaMaximo(art);

            return Maximo;
        }

        public bool agregarArticulo(Articulo art)
        {
            int cantFilas = 0;
            if (dao.ExisteArticulo(art) == false)
            {
                cantFilas = dao.agregarArticulo(art);
            }
            else
            {
                MessageBox.Show("El articulo ya existe en la base de datos", "Error");
            }




            if (cantFilas == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteArticulo(Articulo art)
        {
            
            if (dao.ExisteArticulo(art))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteDescripcionArticulo(Articulo art)
        {

            if (dao.ExisteArticuloDescripcion(art))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteDescripcionArticuloModificar(Articulo art)
        {

            if (dao.ExisteArticuloDescripcionModificar(art))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}


