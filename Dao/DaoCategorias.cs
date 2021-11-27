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
    public class DaoCategorias
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoCategorias = "select * from Categorias";

        public Categoria ObtenerCategoria(Categoria cat)
        {
            DataTable tabla = ds.ObtenerTabla("Categorias", "select IdCategoria_ca,Descripcion_ca,Estado_ca from Categorias where Id_Categoria_ca  = " + cat.IdCategoria1);
            cat.IdCategoria1 = Convert.ToInt32(tabla.Rows[0][0].ToString());
            cat.Decripcion1 = tabla.Rows[0][1].ToString();
            cat.Estado1 = Convert.ToBoolean(tabla.Rows[0][2].ToString());
            return cat;
        }

        public DataTable GetTablaCategoria(Categoria cat)
        {
            String NuevaConsul = TodoCategorias + " where IdCategoria_ca =" + cat.IdCategoria1;
            DataTable tabla = ds.ObtenerTabla("Categoria", NuevaConsul);
            return tabla;
        }

        public DataTable getTablaCategorias()
        {
            DataTable tabla = ds.ObtenerTabla("Categorias", TodoCategorias);
            return tabla;
        }

        public bool ActualizarCategoria(Categoria cat)
        {
            SqlCommand Comando = new SqlCommand();
            ArmarParametroCategoriaModificar(ref Comando, cat);
            AccesoDatos ad = new AccesoDatos();
            int FilasInsertadas = ad.EjecutarProcedimientoAlmacenado(Comando, "sp_ModificarCategoria");
            if (FilasInsertadas == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean ExisteCategoria(Categoria cat)
        {
            string consulta = TodoCategorias + " where IdCategoria_ca = " + cat.IdCategoria1;
            return ds.existe(consulta);
        }

        public Boolean ExisteCategoriaDescripcion(Categoria cat)
        {
            string consulta = TodoCategorias + " where Descripcion_ca = '" + cat.Decripcion1+"'";
            return ds.existe(consulta);
        }

        public Boolean ExisteCategoriaModificar(Categoria cat)
        {
            string consulta = TodoCategorias + " where Descripcion_ca = '" + cat.Decripcion1 + "' and IdCategoria_ca <> "+cat.IdCategoria1;
            return ds.existe(consulta);
        }

        public Boolean DevolverEstadoCat(Categoria cat)
        {
            String NuevaConsul = "select Estado_ca from Categorias where IdCategoria_ca = " + cat.IdCategoria1 ;
            DataTable tabla = ds.ObtenerTabla("Categorias", NuevaConsul);
            bool Id = Convert.ToBoolean(tabla.Rows[0][0].ToString());

            return Id;

        }

        public int eliminarCategoria(Categoria cat)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametroCategoriaEliminar(ref comando, cat);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_EliminarCategoria");
        }

        public int agregarCategoria(Categoria cat)
        {
            SqlCommand comando = new SqlCommand();
            ArmarParametroCategoriaAgregar(ref comando, cat);
            return ds.EjecutarProcedimientoAlmacenado(comando, "sp_AgregarCategoria");
        }

        private void ArmarParametroCategoriaEliminar(ref SqlCommand comando, Categoria cat)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@IDCATEGORIA_CA", SqlDbType.BigInt);
            SqlParametros.Value = cat.IdCategoria1;
        }

        private void ArmarParametroCategoriaAgregar(ref SqlCommand comando, Categoria cat)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@DESCRIPCION_CA", SqlDbType.VarChar);
            SqlParametros.Value = cat.Decripcion1;
        }
        private void ArmarParametroCategoriaModificar(ref SqlCommand comando, Categoria cat)
        {
            SqlParameter SqlParametros = new SqlParameter();
            SqlParametros = comando.Parameters.Add("@IDCATEGORIA_CA", SqlDbType.BigInt);
            SqlParametros.Value = cat.IdCategoria1;
            SqlParametros = comando.Parameters.Add("@DESCRIPCION_CA", SqlDbType.VarChar);
            SqlParametros.Value = cat.Decripcion1;
            SqlParametros = comando.Parameters.Add("@ESTADO_CA", SqlDbType.Bit);
            SqlParametros.Value = cat.Estado1;
        }
    }
}
