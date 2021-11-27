using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
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
            gvUsuarios.DataSource = us.getUsuarios();
            gvUsuarios.DataBind();

        }

        public void CargarGvCategorias()
        {
            GestionCategorias cat = new GestionCategorias();
            gvCategorias.DataSource = cat.getCategorias();
            gvCategorias.DataBind();
        }

        public void CargarGvArticulos()
        {
            GestionArticulos ar = new GestionArticulos();
            gvArticulos.DataSource = ar.getArticulos();
            gvArticulos.DataBind();
        }

    }
}