using System.Data;
using System.Data.SQLite;
using Entidades;

namespace Dao
{
    public class DaoCategorias
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();
        private const string TodoCategorias = "select * from Categorias";

        public DataTable GetTablaCategorias()
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Categorias", TodoCategorias);
            return tabla;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroCategoriaModificar(comando, categoria);
            // ex sp_ModificarCategoria
            int filasActualizadas = _accesoDatos.EjecutarComando(comando,
                "update Categorias set Descripcion_ca = @DESCRIPCION_CA, Estado_ca = @ESTADO_CA where IdCategoria_ca = @IDCATEGORIA_CA");
            return filasActualizadas == 1;
        }


        public bool ExisteCategoriaDescripcion(Categoria categoria)
        {
            string consulta = TodoCategorias + " where Descripcion_ca = @Descripcion";
            return _accesoDatos.Existe(consulta, new SQLiteParameter("@Descripcion", categoria.Descripcion));
        }

        public bool ExisteCategoriaModificar(Categoria categoria)
        {
            string consulta = TodoCategorias + " where Descripcion_ca = @Descripcion and IdCategoria_ca <> @IdCategoria";
            return _accesoDatos.Existe(consulta, new SQLiteParameter("@Descripcion", categoria.Descripcion), new SQLiteParameter("@IdCategoria", categoria.IdCategoria));
        }

        public int EliminarCategoria(Categoria categoria)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroCategoriaEliminar(comando, categoria);
            // ex sp_EliminarCategoria
            return _accesoDatos.EjecutarComando(comando, "update Categorias set Estado_ca = 0 where IdCategoria_ca = @IDCATEGORIA_CA");
        }

        public int AgregarCategoria(Categoria categoria)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroCategoriaAgregar(comando, categoria);
            // ex sp_AgregarCategoria
            return _accesoDatos.EjecutarComando(comando, "insert into Categorias(Descripcion_ca,Estado_ca) values (@DESCRIPCION_CA,1)");
        }

        private void ArmarParametroCategoriaEliminar(SQLiteCommand comando, Categoria categoria)
        {
            comando.Parameters.Add("@IDCATEGORIA_CA", DbType.Int64).Value = categoria.IdCategoria;
        }

        private void ArmarParametroCategoriaAgregar(SQLiteCommand comando, Categoria categoria)
        {
            comando.Parameters.Add("@DESCRIPCION_CA", DbType.String).Value = categoria.Descripcion;
        }

        private void ArmarParametroCategoriaModificar(SQLiteCommand comando, Categoria categoria)
        {
            comando.Parameters.Add("@IDCATEGORIA_CA", DbType.Int64).Value = categoria.IdCategoria;
            comando.Parameters.Add("@DESCRIPCION_CA", DbType.String).Value = categoria.Descripcion;
            comando.Parameters.Add("@ESTADO_CA", DbType.Boolean).Value = categoria.Estado;
        }
    }
}
