using System;
using Entidades;

namespace Negocio.Tests
{
    static class Datos
    {
        /* Id de una tarjeta vigente del usuario; la guarda si todavía no tiene. */
        public static long TarjetaDe(int idUsuario)
        {
            var gestion = new GestionTarjetas();
            var usuario = new Usuario { IdUsuario1 = idUsuario };
            var tarjetas = gestion.getTablaTarjetasUsuario(usuario);
            if (tarjetas.Rows.Count == 0)
            {
                gestion.agregarTarjeta(new Tarjeta
                {
                    Us = usuario, Titular_Tarjeta1 = "Titular", Nombre_Tarjeta1 = "Visa", Tipo_Tarjeta1 = true,
                    Num_Tarjeta1 = 4567123412340000 + idUsuario, Fecha_Vencimiento1 = "01/12/2099", Codigo_Seguridad1 = 321
                });
                tarjetas = gestion.getTablaTarjetasUsuario(usuario);
            }
            return Convert.ToInt64(tarjetas.Rows[0]["IdTarjeta_t"]);
        }
    }
}
