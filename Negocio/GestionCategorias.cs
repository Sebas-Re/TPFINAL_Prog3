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
            return eliminado == 1;
        }

        public bool AgregarCategoria(Categoria categoria)
        {
            int cantFilas = 0;
            cantFilas = _daoCategorias.AgregarCategoria(categoria);

            return cantFilas == 1;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            bool eliminado = _daoCategorias.ActualizarCategoria(categoria);
            return eliminado;
        }


        public bool ExisteCategoriaModificar(Categoria categoria)
        {

            return _daoCategorias.ExisteCategoriaModificar(categoria);
        }

        public bool VerificarCategoria(Categoria categoria)
        {

            return _daoCategorias.ExisteCategoriaDescripcion(categoria);
        }

    }
}

