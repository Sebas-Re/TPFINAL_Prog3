using System;

namespace Entidades
{
    [Serializable]
    public class ItemCarrito
    {
        public string CodArticulo { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        public decimal Subtotal
        {
            get { return PrecioUnitario * Cantidad; }
        }
    }
}
