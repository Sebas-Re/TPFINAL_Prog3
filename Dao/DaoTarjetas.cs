using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using Entidades;

namespace Dao
{
    public class DaoTarjetas
    {
        private readonly AccesoDatos _accesoDatos = new AccesoDatos();
        private const string TodoTarjetas = "select * from Tarjetas ";

        /* Lo que las páginas muestran de las tarjetas del usuario: sin el código de
           seguridad ni el número completo. Cada tarjeta se identifica por su rowid
           (IdTarjeta_t), porque la clave primaria de la tabla incluye el código. */
        public DataTable ObtenerTarjetasUsuario(Usuario usuario)
        {
            string consulta = "select rowid as IdTarjeta_t,Id_TarjetaUsuario_t,Titular_Tarjeta_t,Nombre_Tarjeta_t, CASE Tipo_Tarjeta_t WHEN 0 THEN 'Débito' WHEN 1 THEN 'Crédito' END AS [Tipo_Tarjeta_t],substr(Num_Tarjeta_t,-4) as [Num_Tarjeta_t],Fecha_Vencimiento_t from Tarjetas where Id_TarjetaUsuario_t = @IdUsuario";
            DataTable tabla = _accesoDatos.ObtenerTabla("Tarjetas", consulta, new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
            return tabla;
        }

        /* ¿El usuario ya tiene guardada esta tarjeta? Compara el número completo. */
        public bool ExisteTarjeta(Tarjeta tarjeta)
        {
            string consulta = TodoTarjetas + "where Id_TarjetaUsuario_t = @IdUsuario and Num_Tarjeta_t = @NumTarjeta";
            return _accesoDatos.Existe(consulta,
                new SQLiteParameter("@IdUsuario", tarjeta.Usuario.IdUsuario),
                new SQLiteParameter("@NumTarjeta", tarjeta.NumTarjeta));
        }

        /* Una tarjeta no vencida del usuario, o null si no es suya, no existe o venció. */
        public Tarjeta ObtenerTarjetaVigente(Usuario usuario, long idTarjeta)
        {
            DataTable tabla = _accesoDatos.ObtenerTabla("Tarjetas",
                "select Tipo_Tarjeta_t, Codigo_Seguridad_t from Tarjetas where rowid = @IdTarjeta and Id_TarjetaUsuario_t = @IdUsuario and Fecha_Vencimiento_t > date('now', 'localtime')",
                new SQLiteParameter("@IdTarjeta", idTarjeta), new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
            if (tabla.Rows.Count == 0)
            {
                return null;
            }
            return new Tarjeta
            {
                IdTarjeta = idTarjeta,
                Usuario = usuario,
                TipoTarjeta = (bool)tabla.Rows[0]["Tipo_Tarjeta_t"],
                CodigoSeguridad = Convert.ToInt32(tabla.Rows[0]["Codigo_Seguridad_t"])
            };
        }

        /* Siempre dentro de las tarjetas del usuario: no se puede borrar una ajena. */
        public int EliminarTarjeta(Tarjeta tarjeta)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroTarjetaEliminar(comando, tarjeta);
            // ex sp_EliminarTarjeta
            return _accesoDatos.EjecutarComando(comando,
                "delete from Tarjetas where Id_TarjetaUsuario_t = @IDUSUARIO_T and rowid = @IDTARJETA");
        }

        public int AgregarTarjeta(Tarjeta tarjeta)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametrosTarjetaAgregar(comando, tarjeta);
            // ex sp_AgregarTarjeta
            return _accesoDatos.EjecutarComando(comando,
                "insert into Tarjetas(Id_TarjetaUsuario_t,Titular_Tarjeta_t,Nombre_Tarjeta_t,Tipo_Tarjeta_t,Num_Tarjeta_t,Fecha_Vencimiento_t,Codigo_Seguridad_t) values (@IDTARJETA_T,@TITULARTARJETA_T,@NOMBRETARJETA_T,@TIPODETARJETA_T,@NUMEROTARJETA_T,@FECHAVENCIMIENTO_T,@CODIGODESEGURIDAD_T)");
        }

        private void ArmarParametroTarjetaEliminar(SQLiteCommand comando, Tarjeta tarjeta)
        {
            comando.Parameters.Add("@IDUSUARIO_T", DbType.Int64).Value = tarjeta.Usuario.IdUsuario;
            comando.Parameters.Add("@IDTARJETA", DbType.Int64).Value = tarjeta.IdTarjeta;
        }

        private void ArmarParametrosTarjetaAgregar(SQLiteCommand comando, Tarjeta tarjeta)
        {
            comando.Parameters.Add("@IDTARJETA_T", DbType.Int64).Value = tarjeta.Usuario.IdUsuario;
            comando.Parameters.Add("@TITULARTARJETA_T", DbType.String).Value = tarjeta.TitularTarjeta;
            comando.Parameters.Add("@NOMBRETARJETA_T", DbType.String).Value = tarjeta.NombreTarjeta;
            comando.Parameters.Add("@TIPODETARJETA_T", DbType.Boolean).Value = tarjeta.TipoTarjeta;
            comando.Parameters.Add("@NUMEROTARJETA_T", DbType.Int64).Value = tarjeta.NumTarjeta;
            /* La vista arma la fecha como "dd/MM/yyyy"; se guarda en ISO para que
               SQLite pueda compararla con date('now'). */
            comando.Parameters.Add("@FECHAVENCIMIENTO_T", DbType.String).Value = DateTime.Parse(tarjeta.FechaVencimiento, new CultureInfo("es-AR")).ToString("yyyy-MM-dd");
            comando.Parameters.Add("@CODIGODESEGURIDAD_T", DbType.Int32).Value = tarjeta.CodigoSeguridad;
        }
    }
}
