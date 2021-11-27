using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Entidades
{
    public class DetalleVentas
    {
        private int IdDetVenta;
        private int IdUsuario;
        private string CodArticulo;
        private int cantidad;
        private float Precio;
        private string DireccionEntrega;
        private string Contacto;

        public DetalleVentas()
        {

        }
        public int IdDetVenta1 { get => IdDetVenta; set => IdDetVenta = value; }
        public int IdUsuario1 { get => IdUsuario; set => IdUsuario = value; }
        public string CodArticulo1 { get => CodArticulo; set => CodArticulo = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public float Precio1 { get => Precio; set => Precio = value; }
        public string DireccionEntrega1 { get => DireccionEntrega; set => DireccionEntrega = value; }
        public string Contacto1 { get => Contacto; set => Contacto = value; }
    }
}
