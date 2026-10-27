using System;
using System.Collections.Generic;
using System.Linq;

namespace Entidades
{
    /* Carrito de compras del usuario. Vive en la sesión mientras el usuario
       navega el catálogo y se vacía al confirmar o cancelar la compra. */
    [Serializable]
    public class Carrito
    {
        private readonly List<ItemCarrito> _items = new List<ItemCarrito>();

        public IList<ItemCarrito> Items
        {
            get { return _items.AsReadOnly(); }
        }

        public bool Vacio
        {
            get { return _items.Count == 0; }
        }

        public int CantidadUnidades
        {
            get { return _items.Sum(i => i.Cantidad); }
        }

        public decimal Total
        {
            get { return _items.Sum(i => i.Subtotal); }
        }

        /* Suma una unidad del artículo. Devuelve false si no queda stock
           para agregar otra unidad. */
        public bool Agregar(string codArticulo, string descripcion, decimal precioUnitario, int stock)
        {
            ItemCarrito item = Buscar(codArticulo);
            int cantidadNueva = (item == null ? 0 : item.Cantidad) + 1;
            if (cantidadNueva > stock)
            {
                return false;
            }
            if (item == null)
            {
                _items.Add(new ItemCarrito { CodArticulo = codArticulo, Descripcion = descripcion, PrecioUnitario = precioUnitario, Cantidad = 1 });
            }
            else
            {
                item.Cantidad = cantidadNueva;
            }
            return true;
        }

        public bool Quitar(string codArticulo)
        {
            return _items.RemoveAll(i => MismoCodigo(i.CodArticulo, codArticulo)) > 0;
        }

        public void Vaciar()
        {
            _items.Clear();
        }

        private ItemCarrito Buscar(string codArticulo)
        {
            return _items.FirstOrDefault(i => MismoCodigo(i.CodArticulo, codArticulo));
        }

        private static bool MismoCodigo(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
    }
}
