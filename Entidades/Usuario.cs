using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario
    {
		private int IdUsuario;
		private string Nombre;
		private string Contraseña;
		private string Email;
		private string Telefono;
		private bool Tipo;
		private bool Estado;

        public Usuario()
        { 
        
        }
        public int IdUsuario1 { get => IdUsuario; set => IdUsuario = value; }
        public string Nombre1 { get => Nombre; set => Nombre = value; }
        public string Contraseña1 { get => Contraseña; set => Contraseña = value; }
        public string Email1 { get => Email; set => Email = value; }
        public string Telefono1 { get => Telefono; set => Telefono = value; }
        public bool Tipo1 { get => Tipo; set => Tipo = value; }
        public bool Estado1 { get => Estado; set => Estado = value; }
    }
}
