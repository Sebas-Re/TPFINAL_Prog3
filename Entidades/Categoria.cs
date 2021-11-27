using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Categoria
    {
        private int IdCategoria;
        private string Decripcion;
        private bool Estado;

        public Categoria()
        {
            
        }

        public int IdCategoria1 { get => IdCategoria; set => IdCategoria = value; }
        public string Decripcion1 { get => Decripcion; set => Decripcion = value; }
        public bool Estado1 { get => Estado; set => Estado = value; }
    }
}
