using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Ventas
    {

        public int IdVenta { get; set; }
        public Usuario Usuario { get; set; }
        public float TotalVenta { get; set; }
        public bool MetodoPago { get; set; }
        public int CodigoSeguridad { get; set; }
    }
}
