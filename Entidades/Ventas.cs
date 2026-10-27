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
