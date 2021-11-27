using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Articulo
    {
        private string CodArticulo;
        private Categoria categoria;
        private string Descripcion;
        private int Stock;
        private float Precio_Unitario;
        private string ImagenURL;
        private bool Estado;

        
        public Articulo()
        {

        }

        public string CodArticulo1 { get => CodArticulo; set => CodArticulo = value; }
        public Categoria Categoria1 { get => categoria; set => categoria = value; }
        public string Descripcion1 { get => Descripcion; set => Descripcion = value; }
        public int Stock1 { get => Stock; set => Stock = value; }
        public float Precio_Unitario1 { get => Precio_Unitario; set => Precio_Unitario = value; }
        public string ImagenURL1 { get => ImagenURL; set => ImagenURL = value; }
        public bool Estado1 { get => Estado; set => Estado = value; }
    }
}
