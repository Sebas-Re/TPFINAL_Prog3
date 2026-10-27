using System.Data;
using Dao;
using Entidades;




namespace Negocio
{
    public class GestionCategorias
    {
        private readonly DaoCategorias _daoCategorias = new DaoCategorias();

        public DataTable GetCategorias() //devuelve todas las Categorias
        {
            return _daoCategorias.GetTablaCategorias();
        }


        public bool EliminarCategoria(Categoria categoria)
        {
            int eliminado = _daoCategorias.EliminarCategoria(categoria);
            if (eliminado == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AgregarCategoria(Categoria categoria)
        {
            int cantFilas = 0;
            cantFilas = _daoCategorias.AgregarCategoria(categoria);

            if (cantFilas == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            bool eliminado = _daoCategorias.ActualizarCategoria(categoria);
            if (eliminado)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool ExisteCategoriaModificar(Categoria categoria)
        {

            if (_daoCategorias.ExisteCategoriaModificar(categoria))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerificarCategoria(Categoria categoria)
        {

            if (_daoCategorias.ExisteCategoriaDescripcion(categoria))
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

