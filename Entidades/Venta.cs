namespace Entidades
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public Usuario Usuario { get; set; }
        public decimal TotalVenta { get; set; }
        public bool MetodoPago { get; set; }
        public int CodigoSeguridad { get; set; }
    }
}
