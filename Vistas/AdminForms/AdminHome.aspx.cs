using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vistas.AdminForms
{
    public partial class AdminHome1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblNombreAdm.Text = Session["Nombreusuario"].ToString();
        }
    }
}