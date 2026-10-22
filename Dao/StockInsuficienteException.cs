using System;

namespace Dao
{
    /* Al registrar una compra, un artículo ya no tiene el stock pedido (otro
       cliente compró antes) o fue dado de baja. Disponible = 0 en ese caso. */
    public class StockInsuficienteException : Exception
    {
        public StockInsuficienteException(string codigo, string descripcion, long disponible)
            : base("Stock insuficiente de " + codigo)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            Disponible = disponible;
        }

        public string Codigo { get; private set; }
        public string Descripcion { get; private set; }
        public long Disponible { get; private set; }
    }
}
