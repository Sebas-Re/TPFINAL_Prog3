using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Ventas
    {
        private int IdVenta;
        private Usuario Usuario;
        private float TotalVenta;
        private bool Metodo_Pago;
        private int Cod_Seguridad;
        
        public Ventas()
        {

        }

        public int IdVenta1 { get => IdVenta; set => IdVenta = value; }
        public Usuario Usuario1 { get => Usuario; set => Usuario = value; }
        public float TotalVenta1 { get => TotalVenta; set => TotalVenta = value; }
        public bool Metodo_Pago1 { get => Metodo_Pago; set => Metodo_Pago = value; }
        public int Cod_Seguridad1 { get => Cod_Seguridad; set => Cod_Seguridad = value; }
    }
}
