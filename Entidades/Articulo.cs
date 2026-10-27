namespace Entidades
{
    public class Articulo
    {
        public string CodigoArticulo { get; set; }
        public Categoria Categoria { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string ImagenUrl { get; set; }
        public bool Estado { get; set; }
    }
}
