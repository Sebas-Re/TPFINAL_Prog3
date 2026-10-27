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
        const string TodoTarjetas = "select * from Tarjetas ";

        /* Lo que las páginas muestran de las tarjetas del usuario: sin el código de
           seguridad ni el número completo. Cada tarjeta se identifica por su rowid
           (IdTarjeta_t), porque la clave primaria de la tabla incluye el código. */
        public DataTable ObtenerTarjetasUsuario(Usuario usuario)
        {
            String NuevaConsul = "select rowid as IdTarjeta_t,Id_TarjetaUsuario_t,Titular_Tarjeta_t,Nombre_Tarjeta_t, CASE Tipo_Tarjeta_t WHEN 0 THEN 'Débito' WHEN 1 THEN 'Crédito' END AS [Tipo_Tarjeta_t],substr(Num_Tarjeta_t,-4) as [Num_Tarjeta_t],Fecha_Vencimiento_t from Tarjetas where Id_TarjetaUsuario_t = @IdUsuario";
            DataTable tabla = _accesoDatos.ObtenerTabla("Tarjetas", NuevaConsul, new SQLiteParameter("@IdUsuario", usuario.IdUsuario));
            return tabla;
        }

        /* ¿El usuario ya tiene guardada esta tarjeta? Compara el número completo. */
        public bool ExisteTarjeta(Tarjeta tarjeta)
        {
            string consulta = TodoTarjetas + "where Id_TarjetaUsuario_t = @IdUsuario and Num_Tarjeta_t = @NumTarjeta";
            return _accesoDatos.existe(consulta,
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
            ArmarParametroTarjetaEliminar(ref comando, tarjeta);
            // ex sp_EliminarTarjeta
            return _accesoDatos.EjecutarComando(comando,
                "delete from Tarjetas where Id_TarjetaUsuario_t = @IDUSUARIO_T and rowid = @IDTARJETA");
        }

        public int AgregarTarjeta(Tarjeta tarjeta)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametrosTarjetaAgregar(ref comando, tarjeta);
            // ex sp_AgregarTarjeta
            return _accesoDatos.EjecutarComando(comando,
                "insert into Tarjetas(Id_TarjetaUsuario_t,Titular_Tarjeta_t,Nombre_Tarjeta_t,Tipo_Tarjeta_t,Num_Tarjeta_t,Fecha_Vencimiento_t,Codigo_Seguridad_t) values (@IDTARJETA_T,@TITULARTARJETA_T,@NOMBRETARJETA_T,@TIPODETARJETA_T,@NUMEROTARJETA_T,@FECHAVENCIMIENTO_T,@CODIGODESEGURIDAD_T)");
        }

        private void ArmarParametroTarjetaEliminar(ref SQLiteCommand comando, Tarjeta tarjeta)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDUSUARIO_T", DbType.Int64);
            Parametros.Value = tarjeta.Usuario.IdUsuario;
            Parametros = comando.Parameters.Add("@IDTARJETA", DbType.Int64);
            Parametros.Value = tarjeta.IdTarjeta;
        }

        private void ArmarParametrosTarjetaAgregar(ref SQLiteCommand comando, Tarjeta tarjeta)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDTARJETA_T", DbType.Int64);
            Parametros.Value = tarjeta.Usuario.IdUsuario;
            Parametros = comando.Parameters.Add("@TITULARTARJETA_T", DbType.String);
            Parametros.Value = tarjeta.TitularTarjeta;
            Parametros = comando.Parameters.Add("@NOMBRETARJETA_T", DbType.String);
            Parametros.Value = tarjeta.NombreTarjeta;
            Parametros = comando.Parameters.Add("@TIPODETARJETA_T", DbType.Boolean);
            Parametros.Value = tarjeta.TipoTarjeta;
            Parametros = comando.Parameters.Add("@NUMEROTARJETA_T", DbType.Int64);
            Parametros.Value = tarjeta.NumTarjeta;
            /* La vista arma la fecha como "dd/MM/yyyy"; se guarda en ISO para que
               SQLite pueda compararla con date('now'). */
            Parametros = comando.Parameters.Add("@FECHAVENCIMIENTO_T", DbType.String);
            Parametros.Value = DateTime.Parse(tarjeta.FechaVencimiento, new CultureInfo("es-AR")).ToString("yyyy-MM-dd");
            Parametros = comando.Parameters.Add("@CODIGODESEGURIDAD_T", DbType.Int32);
            Parametros.Value = tarjeta.CodigoSeguridad;
        }
    }
}
