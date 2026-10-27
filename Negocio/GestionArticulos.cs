using System;
using System.Data;
using System.Text.RegularExpressions;
using Dao;
using Entidades;

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
            return _daoArticulos.GetTablaReporteArticulos();
        }

        public DataTable GetArticuloPorCodigo(Articulo articulo) //Devuelve un Articulo
        {
            return _daoArticulos.GetTablaArticuloPorCodigo(articulo);
        }


        public bool EliminarArticulo(Articulo articulo)
        {
            int eliminado = _daoArticulos.EliminarArticulo(articulo);
            return eliminado == 1;
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
            if (!ImagenValida(articulo.ImagenUrl))
            {
                return false;
            }
            bool eliminado = _daoArticulos.ActualizarArticulo(articulo);
            return eliminado;


        }

        public bool AgregarArticulo(Articulo articulo)
        {
            int cantFilas = 0;
            if (!CodigoValido(articulo.CodigoArticulo) || !ImagenValida(articulo.ImagenUrl))
            {
                return false;
            }
            if (_daoArticulos.ExisteArticulo(articulo) == false)
            {
                cantFilas = _daoArticulos.AgregarArticulo(articulo);
            }
            return cantFilas == 1;
        }

        public bool ExisteArticulo(Articulo articulo)
        {

            return _daoArticulos.ExisteArticulo(articulo);
        }

        public bool ExisteDescripcionArticulo(Articulo articulo)
        {

            return _daoArticulos.ExisteArticuloDescripcion(articulo);
        }

        public bool ExisteDescripcionArticuloModificar(Articulo articulo)
        {

            return _daoArticulos.ExisteArticuloDescripcionModificar(articulo);
        }
    }
}


