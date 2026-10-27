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
        public int IdDetalleVenta { get; set; }
        public int IdUsuario { get; set; }
        public string CodigoArticulo { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }
        public string DireccionEntrega { get; set; }
        public string Contacto { get; set; }
    }
}
