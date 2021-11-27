using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
   public class Tarjeta
    {
        private Usuario us; //Id_TarjetaUsuario_t
        private string Titular_Tarjeta;
        private string Nombre_Tarjeta;
        private bool Tipo_Tarjeta;
        private int Num_Tarjeta;
        private string Fecha_Vencimiento;
        private int Codigo_Seguridad;
        
        public Tarjeta()
        {
        }

        public Usuario Us { get => us; set => us = value; }
        public string Titular_Tarjeta1 { get => Titular_Tarjeta; set => Titular_Tarjeta = value; }
        public string Nombre_Tarjeta1 { get => Nombre_Tarjeta; set => Nombre_Tarjeta = value; }
        public bool Tipo_Tarjeta1 { get => Tipo_Tarjeta; set => Tipo_Tarjeta = value; }
        public int Num_Tarjeta1 { get => Num_Tarjeta; set => Num_Tarjeta = value; }
        public string Fecha_Vencimiento1 { get => Fecha_Vencimiento; set => Fecha_Vencimiento = value; }
        public int Codigo_Seguridad1 { get => Codigo_Seguridad; set => Codigo_Seguridad = value; }
    }
}
