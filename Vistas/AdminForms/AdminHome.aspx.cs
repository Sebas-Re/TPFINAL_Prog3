using System;
using System.Data;
using Negocio;

namespace Vistas.AdminForms
{
    public partial class AdminHome : PaginaBase
    {
        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Admin; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblNombreAdm.Text = Sesion.Usuario.Nombre1;

            DataTable articulos = new GestionArticulos().getArticulos();
            lblArticulos.Text = articulos.Select("Estado_ar = true").Length.ToString();

            DataTable usuarios = new GestionUsuarios().getUsuarios();
            lblClientes.Text = usuarios.Select("Tipo_us = false AND Estado_us = true").Length.ToString();

            lblVentas.Text = new GestionVentas().getVentas().Rows.Count.ToString();

            // Se calcula desde los detalles (cantidad x precio) de cada artículo vendido.
            decimal recaudacion = 0;
            foreach (DataRow fila in new GestionArticulos().getReporteArticulos().Rows)
            {
                recaudacion += Convert.ToDecimal(fila["totalventa"]);
            }
            lblRecaudacion.Text = recaudacion.ToString("C0");
        }
    }
}
