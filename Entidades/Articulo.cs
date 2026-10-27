namespace Entidades
{
    public class Articulo
    {
        public string CodigoArticulo { get; set; }
        public Categoria Categoria { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public float PrecioUnitario { get; set; }
        public string ImagenURL { get; set; }
        public bool Estado { get; set; }
    }
}
