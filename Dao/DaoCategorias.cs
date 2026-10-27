using System;
using System.Data;
using System.Data.SQLite;
using Entidades;

namespace Dao
{
    public class DaoCategorias
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();
        const string TodoCategorias = "select * from Categorias";

        public Categoria ObtenerCategoria(Categoria categoria)
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Categorias", "select IdCategoria_ca,Descripcion_ca,Estado_ca from Categorias where IdCategoria_ca = @IdCategoria",
                new SQLiteParameter("@IdCategoria", categoria.IdCategoria));
            categoria.IdCategoria = Convert.ToInt32(tabla.Rows[0][0].ToString());
            categoria.Descripcion = tabla.Rows[0][1].ToString();
            categoria.Estado = Convert.ToBoolean(tabla.Rows[0][2].ToString());
            return categoria;
        }

        public DataTable GetTablaCategoria(Categoria categoria)
        {
            String NuevaConsul = TodoCategorias + " where IdCategoria_ca = @IdCategoria";
            DataTable tabla = _accesoDatos.ObtenerTabla("Categoria", NuevaConsul, new SQLiteParameter("@IdCategoria", categoria.IdCategoria));
            return tabla;
        }

        public DataTable GetTablaCategorias()
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Categorias", TodoCategorias);
            return tabla;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            SQLiteCommand Comando = new SQLiteCommand();
            ArmarParametroCategoriaModificar(ref Comando, categoria);
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


        public Boolean ExisteCategoriaDescripcion(Categoria categoria)
        {
            string consulta = TodoCategorias + " where Descripcion_ca = @Descripcion";
            return _accesoDatos.existe(consulta, new SQLiteParameter("@Descripcion", categoria.Descripcion));
        }

        public Boolean ExisteCategoriaModificar(Categoria categoria)
        {
            string consulta = TodoCategorias + " where Descripcion_ca = @Descripcion and IdCategoria_ca <> @IdCategoria";
            return _accesoDatos.existe(consulta, new SQLiteParameter("@Descripcion", categoria.Descripcion), new SQLiteParameter("@IdCategoria", categoria.IdCategoria));
        }

        public Boolean DevolverEstadoCat(Categoria categoria)
        {
            String NuevaConsul = "select Estado_ca from Categorias where IdCategoria_ca = @IdCategoria";
            DataTable tabla = _accesoDatos.ObtenerTabla("Categorias", NuevaConsul, new SQLiteParameter("@IdCategoria", categoria.IdCategoria));
            bool Id = Convert.ToBoolean(tabla.Rows[0][0].ToString());
            return Id;
        }

        public int EliminarCategoria(Categoria categoria)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroCategoriaEliminar(ref comando, categoria);
            // ex sp_EliminarCategoria
            return _accesoDatos.EjecutarComando(comando, "update Categorias set Estado_ca = 0 where IdCategoria_ca = @IDCATEGORIA_CA");
        }

        public int AgregarCategoria(Categoria categoria)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroCategoriaAgregar(ref comando, categoria);
            // ex sp_AgregarCategoria
            return _accesoDatos.EjecutarComando(comando, "insert into Categorias(Descripcion_ca,Estado_ca) values (@DESCRIPCION_CA,1)");
        }

        private void ArmarParametroCategoriaEliminar(ref SQLiteCommand comando, Categoria categoria)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDCATEGORIA_CA", DbType.Int64);
            Parametros.Value = categoria.IdCategoria;
        }

        private void ArmarParametroCategoriaAgregar(ref SQLiteCommand comando, Categoria categoria)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@DESCRIPCION_CA", DbType.String);
            Parametros.Value = categoria.Descripcion;
        }

        private void ArmarParametroCategoriaModificar(ref SQLiteCommand comando, Categoria categoria)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDCATEGORIA_CA", DbType.Int64);
            Parametros.Value = categoria.IdCategoria;
            Parametros = comando.Parameters.Add("@DESCRIPCION_CA", DbType.String);
            Parametros.Value = categoria.Descripcion;
            Parametros = comando.Parameters.Add("@ESTADO_CA", DbType.Boolean);
            Parametros.Value = categoria.Estado;
        }
    }
}
