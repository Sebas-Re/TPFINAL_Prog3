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
    public partial class GestionUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGvUsuarios();
            }

            public void CargarGvUsuarios()
            {
                GestionUsuarios us = new GestionUsuarios();
                gvUsuarios.DataSource = us.getUsuarios();
                gvUsuarios.DataBind();

            }
        }
    }
}