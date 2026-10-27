using System;
using System.Text.RegularExpressions;
using System.Data;
using Entidades;
using Dao;

namespace Negocio
{
    public class GestionArticulos
    {
        private readonly DaoArticulos _daoArticulos = new DaoArticulos();
        public DataTable GetArticulos() //devuelve todos los Articulos
        {
            return _daoArticulos.GetTablaArticulos();
        }

        public DataTable GetArticulosCatalogo(string busqueda, int? categoria) //solo los que se pueden comprar
        {
            return _daoArticulos.GetTablaArticulosCatalogo(busqueda, categoria);
        }

        public DataTable GetReporteArticulos()
        {
            return _daoArticulos.GetTablaReporteArtiuculos();
        }

        public DataTable GetArticuloCOD(Articulo articulo) //Devuelve un Articulo
        {
            return _daoArticulos.GetTablaArticuloCOD(articulo);
        }


        public bool EliminarArticulo(Articulo articulo)
        {
            int eliminado = _daoArticulos.EliminarArticulo(articulo);
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

        public bool ActualizarArticulo(Articulo articulo)
        {
            if (!ImagenValida(articulo.ImagenURL))
            {
                return false;
            }
            bool eliminado = _daoArticulos.ActualizarArticulo(articulo);
            if (eliminado)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public bool AgregarArticulo(Articulo articulo)
        {
            int cantFilas = 0;
            if (!CodigoValido(articulo.CodigoArticulo) || !ImagenValida(articulo.ImagenURL))
            {
                return false;
            }
            if (_daoArticulos.ExisteArticulo(articulo) == false)
            {
                cantFilas = _daoArticulos.AgregarArticulo(articulo);
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

        public bool ExisteArticulo(Articulo articulo)
        {

            if (_daoArticulos.ExisteArticulo(articulo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteDescripcionArticulo(Articulo articulo)
        {

            if (_daoArticulos.ExisteArticuloDescripcion(articulo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteDescripcionArticuloModificar(Articulo articulo)
        {

            if (_daoArticulos.ExisteArticuloDescripcionModificar(articulo))
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


