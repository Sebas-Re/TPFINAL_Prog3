namespace Entidades
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdUsuario { get; set; }
        public string CodigoArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string DireccionEntrega { get; set; }
        public string Contacto { get; set; }
    }
}
