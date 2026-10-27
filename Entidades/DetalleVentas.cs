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
