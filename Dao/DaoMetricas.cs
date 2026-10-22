using System;
using System.Data;
using System.Data.SQLite;

namespace Dao
{
    /* Consultas para el panel y los reportes. Los ingresos se calculan desde los
       detalles (cantidad x precio), que es el dato de origen: así se pueden
       desglosar por categoría o producto y siempre suman lo mismo que los totales.

       Los períodos son [inicio, fin) en días enteros relativos a hoy a las 00:00:
       "últimos 30 días" es inicio = -29, fin = 1 (mañana a las 00:00). */
    public class DaoMetricas
    {
        AccesoDatos ds = new AccesoDatos();

        const string Lineas =
            "with lineas as (" +
            " select v.IdVentas_ve, v.IdUsuario_ve, v.FechaVenta_ve, v.Metodo_de_Pago_ve, d.CodArticulo_dv, d.Cantidad_dv," +
            " d.Cantidad_dv * d.Precio_Unitario_dv as Importe" +
            " from Ventas v inner join Detalle_Ventas d on d.IdDetVenta_dv = v.IdVentas_ve" +
            " where v.FechaVenta_ve >= datetime('now','localtime','start of day',@Inicio) and v.FechaVenta_ve < datetime('now','localtime','start of day',@Fin)) ";

        static SQLiteParameter[] Periodo(int inicio, int fin, params SQLiteParameter[] otros)
        {
            SQLiteParameter[] parametros = new SQLiteParameter[otros.Length + 2];
            parametros[0] = new SQLiteParameter("@Inicio", inicio.ToString("+0;-0") + " days");
            parametros[1] = new SQLiteParameter("@Fin", fin.ToString("+0;-0") + " days");
            otros.CopyTo(parametros, 2);
            return parametros;
        }

        /* Una fila: Ingresos, Pedidos, Unidades, Compradores, CompradoresRecurrentes. */
        public DataTable getResumenVentas(int inicio, int fin)
        {
            string consulta = Lineas +
                "select coalesce(sum(Importe),0) as Ingresos, count(distinct IdVentas_ve) as Pedidos," +
                " coalesce(sum(Cantidad_dv),0) as Unidades, count(distinct IdUsuario_ve) as Compradores," +
                " (select count(*) from (select IdUsuario_ve from lineas group by IdUsuario_ve having count(distinct IdVentas_ve) >= 2)) as CompradoresRecurrentes" +
                " from lineas";
            return ds.ObtenerTabla("Resumen", consulta, Periodo(inicio, fin));
        }

        public int getClientesNuevos(int inicio, int fin)
        {
            string consulta = "select count(*) from Usuarios where Tipo_us = 0" +
                " and FechaAlta_us >= datetime('now','localtime','start of day',@Inicio) and FechaAlta_us < datetime('now','localtime','start of day',@Fin)";
            return ds.ObtenerMaximo(consulta, Periodo(inicio, fin));
        }

        /* Mes (yyyy-MM), Ingresos, Pedidos. */
        public DataTable getIngresosPorMes(int inicio, int fin)
        {
            string consulta = Lineas +
                "select strftime('%Y-%m', FechaVenta_ve) as Mes, sum(Importe) as Ingresos, count(distinct IdVentas_ve) as Pedidos" +
                " from lineas group by Mes order by Mes";
            return ds.ObtenerTabla("IngresosPorMes", consulta, Periodo(inicio, fin));
        }

        /* Mes (yyyy-MM), Clientes: altas de clientes por mes. */
        public DataTable getClientesNuevosPorMes(int inicio, int fin)
        {
            string consulta = "select strftime('%Y-%m', FechaAlta_us) as Mes, count(*) as Clientes from Usuarios" +
                " where Tipo_us = 0 and FechaAlta_us >= datetime('now','localtime','start of day',@Inicio) and FechaAlta_us < datetime('now','localtime','start of day',@Fin)" +
                " group by Mes order by Mes";
            return ds.ObtenerTabla("ClientesPorMes", consulta, Periodo(inicio, fin));
        }

        /* Dia (yyyy-MM-dd), Ingresos, Pedidos. */
        public DataTable getIngresosPorDia(int inicio, int fin)
        {
            string consulta = Lineas +
                "select date(FechaVenta_ve) as Dia, sum(Importe) as Ingresos, count(distinct IdVentas_ve) as Pedidos" +
                " from lineas group by Dia order by Dia";
            return ds.ObtenerTabla("IngresosPorDia", consulta, Periodo(inicio, fin));
        }

        /* Categoria, Ingresos, Unidades. */
        public DataTable getIngresosPorCategoria(int inicio, int fin)
        {
            string consulta = Lineas +
                "select c.Descripcion_ca as Categoria, sum(l.Importe) as Ingresos, sum(l.Cantidad_dv) as Unidades" +
                " from lineas l inner join Articulos a on a.CodArticulo_ar = l.CodArticulo_dv" +
                " inner join Categorias c on c.IdCategoria_ca = a.Categoria_ar" +
                " group by c.Descripcion_ca order by Ingresos desc";
            return ds.ObtenerTabla("IngresosPorCategoria", consulta, Periodo(inicio, fin));
        }

        /* Codigo, Descripcion, Unidades, Ingresos. */
        public DataTable getTopProductos(int inicio, int fin, int cantidad)
        {
            string consulta = Lineas +
                "select a.CodArticulo_ar as Codigo, a.Descripcion_ar as Descripcion, sum(l.Cantidad_dv) as Unidades, sum(l.Importe) as Ingresos" +
                " from lineas l inner join Articulos a on a.CodArticulo_ar = l.CodArticulo_dv" +
                " group by a.CodArticulo_ar, a.Descripcion_ar order by Unidades desc, Ingresos desc limit @Cantidad";
            return ds.ObtenerTabla("TopProductos", consulta, Periodo(inicio, fin, new SQLiteParameter("@Cantidad", cantidad)));
        }

        /* Credito (bool), Pedidos, Ingresos. */
        public DataTable getMetodosDePago(int inicio, int fin)
        {
            string consulta = Lineas +
                "select Metodo_de_Pago_ve as Credito, count(distinct IdVentas_ve) as Pedidos, sum(Importe) as Ingresos" +
                " from lineas group by Metodo_de_Pago_ve order by Metodo_de_Pago_ve";
            return ds.ObtenerTabla("MetodosDePago", consulta, Periodo(inicio, fin));
        }

        /* IdUsuario, Nombre, Pedidos, Gasto. */
        public DataTable getTopClientes(int inicio, int fin, int cantidad)
        {
            string consulta = Lineas +
                "select u.IdUsuario_us as IdUsuario, u.Nombre_us as Nombre, count(distinct l.IdVentas_ve) as Pedidos, sum(l.Importe) as Gasto" +
                " from lineas l inner join Usuarios u on u.IdUsuario_us = l.IdUsuario_ve" +
                " group by u.IdUsuario_us, u.Nombre_us order by Gasto desc limit @Cantidad";
            return ds.ObtenerTabla("TopClientes", consulta, Periodo(inicio, fin, new SQLiteParameter("@Cantidad", cantidad)));
        }

        /* Artículos activos con stock por debajo del umbral, de menor a mayor. */
        public DataTable getStockBajo(int umbral)
        {
            string consulta = "select CodArticulo_ar as Codigo, Descripcion_ar as Descripcion, Stock_ar as Stock from Articulos" +
                " where Estado_ar = 1 and Stock_ar < @Umbral order by Stock_ar, Descripcion_ar";
            return ds.ObtenerTabla("StockBajo", consulta, new SQLiteParameter("@Umbral", umbral));
        }
    }
}
