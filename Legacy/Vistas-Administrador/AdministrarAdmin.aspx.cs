using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Negocio;
using Entidades;


namespace Vistas
{
    public partial class AdministrarAdmin : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGvUsuarios();
                CargarGvCategorias();
                CargarGvArticulos();
            }

        }

        public void CargarGvUsuarios()
        {
            GestionUsuarios us = new GestionUsuarios();
            gvUsuarios.DataSource = us.GetUsuarios();
            gvUsuarios.DataBind();

        }

        public void CargarGvCategorias()
        {
            GestionCategorias cat = new GestionCategorias();
            gvCategorias.DataSource = cat.GetCategorias();
            gvCategorias.DataBind();
        }

        public void CargarGvArticulos()
        {
            GestionArticulos ar = new GestionArticulos();
            gvArticulos.DataSource = ar.GetArticulos();
            gvArticulos.DataBind();
        }

    }
}