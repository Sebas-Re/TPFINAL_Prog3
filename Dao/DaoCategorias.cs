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
    public class DaoCategorias
    {
        AccesoDatos ds = new AccesoDatos();
        const string TodoCategorias = "select * from Categorias";

        public Categoria ObtenerCategoria(Categoria cat)
        {
            DataTable tabla = ds.ObtenerTabla("Categorias", "select IdCategoria_ca,Descripcion_ca,Estado_ca from Categorias where IdCategoria_ca = @IdCategoria",
                new SQLiteParameter("@IdCategoria", cat.IdCategoria1));
            cat.IdCategoria1 = Convert.ToInt32(tabla.Rows[0][0].ToString());
            cat.Decripcion1 = tabla.Rows[0][1].ToString();
            cat.Estado1 = Convert.ToBoolean(tabla.Rows[0][2].ToString());
            return cat;
        }

        public DataTable GetTablaCategoria(Categoria cat)
        {
            String NuevaConsul = TodoCategorias + " where IdCategoria_ca = @IdCategoria";
            DataTable tabla = ds.ObtenerTabla("Categoria", NuevaConsul, new SQLiteParameter("@IdCategoria", cat.IdCategoria1));
            return tabla;
        }

        public DataTable getTablaCategorias()
        {
            DataTable tabla = ds.ObtenerTabla("Categorias", TodoCategorias);
            return tabla;
        }

        public bool ActualizarCategoria(Categoria cat)
        {
            SQLiteCommand Comando = new SQLiteCommand();
            ArmarParametroCategoriaModificar(ref Comando, cat);
            AccesoDatos ad = new AccesoDatos();
            // ex sp_ModificarCategoria
            int FilasInsertadas = ad.EjecutarComando(Comando,
                "update Categorias set Descripcion_ca = @DESCRIPCION_CA, Estado_ca = @ESTADO_CA where IdCategoria_ca = @IDCATEGORIA_CA");
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
            string consulta = TodoCategorias + " where IdCategoria_ca = @IdCategoria";
            return ds.existe(consulta, new SQLiteParameter("@IdCategoria", cat.IdCategoria1));
        }

        public Boolean ExisteCategoriaDescripcion(Categoria cat)
        {
            string consulta = TodoCategorias + " where Descripcion_ca = @Descripcion";
            return ds.existe(consulta, new SQLiteParameter("@Descripcion", cat.Decripcion1));
        }

        public Boolean ExisteCategoriaModificar(Categoria cat)
        {
            string consulta = TodoCategorias + " where Descripcion_ca = @Descripcion and IdCategoria_ca <> @IdCategoria";
            return ds.existe(consulta, new SQLiteParameter("@Descripcion", cat.Decripcion1), new SQLiteParameter("@IdCategoria", cat.IdCategoria1));
        }

        public Boolean DevolverEstadoCat(Categoria cat)
        {
            String NuevaConsul = "select Estado_ca from Categorias where IdCategoria_ca = @IdCategoria";
            DataTable tabla = ds.ObtenerTabla("Categorias", NuevaConsul, new SQLiteParameter("@IdCategoria", cat.IdCategoria1));
            bool Id = Convert.ToBoolean(tabla.Rows[0][0].ToString());
            return Id;
        }

        public int eliminarCategoria(Categoria cat)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroCategoriaEliminar(ref comando, cat);
            // ex sp_EliminarCategoria
            return ds.EjecutarComando(comando, "update Categorias set Estado_ca = 0 where IdCategoria_ca = @IDCATEGORIA_CA");
        }

        public int agregarCategoria(Categoria cat)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroCategoriaAgregar(ref comando, cat);
            // ex sp_AgregarCategoria
            return ds.EjecutarComando(comando, "insert into Categorias(Descripcion_ca,Estado_ca) values (@DESCRIPCION_CA,1)");
        }

        private void ArmarParametroCategoriaEliminar(ref SQLiteCommand comando, Categoria cat)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDCATEGORIA_CA", DbType.Int64);
            Parametros.Value = cat.IdCategoria1;
        }

        private void ArmarParametroCategoriaAgregar(ref SQLiteCommand comando, Categoria cat)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@DESCRIPCION_CA", DbType.String);
            Parametros.Value = cat.Decripcion1;
        }

        private void ArmarParametroCategoriaModificar(ref SQLiteCommand comando, Categoria cat)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDCATEGORIA_CA", DbType.Int64);
            Parametros.Value = cat.IdCategoria1;
            Parametros = comando.Parameters.Add("@DESCRIPCION_CA", DbType.String);
            Parametros.Value = cat.Decripcion1;
            Parametros = comando.Parameters.Add("@ESTADO_CA", DbType.Boolean);
            Parametros.Value = cat.Estado1;
        }
    }
}
