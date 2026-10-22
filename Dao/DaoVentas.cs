using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using Entidades;

namespace Dao
{
    public class DaoVentas
    {
        AccesoDatos ds = new AccesoDatos();
        Ventas venta = new Ventas();
        Usuario usuario = new Usuario();
        const string TodoVentas = "select * from Ventas";

        public Ventas ObtenerVenta(Ventas ve) //devuelve un objeto cargado con la data q coincida con el id
        {
            DataTable tabla = ds.ObtenerTabla("Ventas", TodoVentas + " where IdVentas_ve = @IdVenta", new SQLiteParameter("@IdVenta", ve.IdVenta1));
            ve.IdVenta1 = Convert.ToInt32(tabla.Rows[0][0].ToString());
            ve.Usuario1.IdUsuario1 =Convert.ToInt32(tabla.Rows[0][1].ToString());
            ve.TotalVenta1 =Convert.ToInt32(tabla.Rows[0][2].ToString());
            ve.Metodo_Pago1 = (bool)tabla.Rows[0][3];
            ve.Cod_Seguridad1 = Convert.ToInt32(tabla.Rows[0][4]);
            return ve;
        }

        public DataTable GetTablaVentas(Ventas ve) //devuelve un datatable con los datos de la tabla
        {
            String NuevaConsul = TodoVentas + " where IdVentas_ve = @IdVenta";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdVenta", ve.IdVenta1));
            return tabla;
        }

        public DataTable getTablaVentas()
        {
            DataTable tabla = ds.ObtenerTabla("Ventas", TodoVentas);
            return tabla;
        }

        public DataTable getMaximoVenta()
        {
            DataTable tabla = ds.ObtenerTabla("Ventas", TodoVentas + " where TotalVenta_ve = (select max(TotalVenta_ve) from Ventas)");
            return tabla;
        }

        public DataTable GetTablaIdUsuario(Usuario us)
        {
            String NuevaConsul = TodoVentas + " where IdUsuario_ve = @IdUsuario";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsul, new SQLiteParameter("@IdUsuario", us.IdUsuario1));
            return tabla;
        }

        public Ventas IDultimaventa(Ventas ven)
        {
            string NuevaConsulta = "select MAX(IdVentas_ve) from Ventas";
            DataTable tabla = ds.ObtenerTabla("Ventas", NuevaConsulta);
            ven.IdVenta1 = (int)Convert.ToInt64(tabla.Rows[0][0]);
            return ven;
        }

        public Boolean ExisteVentas(Ventas ve)
        {
            string consulta = TodoVentas + " where IdUsuario_ve = @IdVenta";
            return ds.existe(consulta, new SQLiteParameter("@IdVenta", ve.IdVenta1));
        }

        public int agregarVenta(Ventas ve)
        {
            SQLiteCommand comando = new SQLiteCommand();
            ArmarParametroVentaAgregar(ref comando, ve);
            // ex sp_AgregarVenta
            return ds.EjecutarComando(comando,
                "insert into Ventas(IdUsuario_ve,Metodo_de_Pago_ve,Codigo_Seguridad_ve,FechaVenta_ve) values (@IDUSUARIO_VE,@METODODEPAGO_VE,@CODIGODESEGURIDAD_VE,datetime('now','localtime'))");
        }

        private void ArmarParametroVentaAgregar(ref SQLiteCommand comando,  Ventas ve)
        {
            SQLiteParameter Parametros = new SQLiteParameter();
            Parametros = comando.Parameters.Add("@IDUSUARIO_VE", DbType.Int64);
            Parametros.Value = ve.Usuario1.IdUsuario1;
            Parametros = comando.Parameters.Add("@METODODEPAGO_VE", DbType.Boolean);
            Parametros.Value = ve.Metodo_Pago1;
            Parametros = comando.Parameters.Add("@CODIGODESEGURIDAD_VE", DbType.Int32);
            Parametros.Value = ve.Cod_Seguridad1;
        }
    }
}
