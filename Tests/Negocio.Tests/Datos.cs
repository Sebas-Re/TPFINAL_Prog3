using System;
using System.Data;
using Entidades;

namespace Negocio.Tests
{
    internal static class Datos
    {
        /* Id de una tarjeta vigente del usuario; la guarda si todavía no tiene. */
        public static long TarjetaDe(int idUsuario)
        {
            var gestionTarjetas = new GestionTarjetas();
            var usuario = new Usuario { IdUsuario = idUsuario };
            DataTable tarjetas = gestionTarjetas.GetTablaTarjetasUsuario(usuario);
            if (tarjetas.Rows.Count == 0)
            {
                gestionTarjetas.AgregarTarjeta(new Tarjeta
                {
                    Usuario = usuario,
                    TitularTarjeta = "Titular",
                    NombreTarjeta = "Visa",
                    TipoTarjeta = true,
                    NumTarjeta = 4567123412340000 + idUsuario,
                    FechaVencimiento = "01/12/2099",
                    CodigoSeguridad = 321
                });
                tarjetas = gestionTarjetas.GetTablaTarjetasUsuario(usuario);
            }
            return Convert.ToInt64(tarjetas.Rows[0]["IdTarjeta_t"]);
        }
    }
}
