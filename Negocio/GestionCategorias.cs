using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Entidades;
using Dao;




namespace Negocio
{
    public class GestionCategorias
    {
        DaoCategorias dao = new DaoCategorias();
        
        public DataTable getCategorias() //devuelve todas las Categorias
        {
            return dao.getTablaCategorias();
        }

        public DataTable getCategoriaId(Categoria cat) //Devuelve una categoria
        {
            return dao.GetTablaCategoria(cat);
        }

        public bool eliminarCategoria(Categoria cat)
        {
            int eliminado = dao.eliminarCategoria(cat);
            if (eliminado == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool agregarCategoria(Categoria cat)
        {
            int cantFilas = 0;
            cantFilas = dao.agregarCategoria(cat);
            
            if (cantFilas == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ActualizarCategoria(Categoria cat)
        {
            bool eliminado = dao.ActualizarCategoria(cat);
            if (eliminado)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteCategoria(Categoria cat)
        {

            if (dao.ExisteCategoria(cat))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExisteCategoriaModificar(Categoria cat)
        {

            if (dao.ExisteCategoriaModificar(cat))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerificarEstadoCat(Categoria cat)
        {
            
                bool IdCat = dao.DevolverEstadoCat(cat);
                return IdCat;
            
        }

        public bool VerificarCategoria(Categoria cat)
        {

            if (dao.ExisteCategoriaDescripcion(cat))
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

