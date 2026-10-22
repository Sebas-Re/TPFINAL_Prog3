using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using Entidades;
using Dao;

namespace Negocio
{
    public class GestionArticulos
    {
        DaoArticulos dao = new DaoArticulos();
        public DataTable getArticulos() //devuelve todos los Articulos
        {
            return dao.getTablaArticulos();
        }

        public DataTable getArticulosCatalogo(string busqueda, int? categoria) //solo los que se pueden comprar
        {
            return dao.getTablaArticulosCatalogo(busqueda, categoria);
        }

        public DataTable getReporteArticulos()
        {
            return dao.getTablaReporteArtiuculos();
        }

        public DataTable getReporteArticulo(Articulo art)
        {
            return dao.getTablaReportePorArtiuculos(art);
        }

        public DataTable TopArticulos()
        {
            return dao.getTopArticulos();
        }

        public DataTable OrdenarArticulos(int op)
        {
            return dao.OrdenarArticulosx(op);
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

        /* Código de 1 a 4 letras o números, como los de los datos de prueba. */
        public static bool CodigoValido(string codigo)
        {
            return codigo != null && Regex.IsMatch(codigo, "^[A-Za-z0-9]{1,4}$");
        }

        /* La imagen se muestra en el catálogo: solo rutas del sitio ("~/..." o
           "/...") o direcciones http(s). Nada de "javascript:", "data:", etc. */
        public static bool ImagenValida(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || url.Length > 100)
            {
                return false;
            }
            if (url.StartsWith("~/") || (url.StartsWith("/") && !url.StartsWith("//")))
            {
                return true;
            }
            Uri uri;
            return Uri.TryCreate(url, UriKind.Absolute, out uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        public bool ActualizarArticulo(Articulo art)
        {
            if (!ImagenValida(art.ImagenURL1))
            {
                return false;
            }
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
            if (!CodigoValido(art.CodArticulo1) || !ImagenValida(art.ImagenURL1))
            {
                return false;
            }
            if (dao.ExisteArticulo(art) == false)
            {
                cantFilas = dao.agregarArticulo(art);
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


