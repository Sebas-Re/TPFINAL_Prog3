namespace Entidades
{
    public class Tarjeta
    {
        public long IdTarjeta { get; set; }
        public Usuario Usuario { get; set; }
        public string TitularTarjeta { get; set; }
        public string NombreTarjeta { get; set; }
        public bool TipoTarjeta { get; set; }
        public long NumTarjeta { get; set; }
        public string FechaVencimiento { get; set; }
        public int CodigoSeguridad { get; set; }
    }
}
