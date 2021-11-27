using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class Catalogo : System.Web.UI.Page
    {
        GestionArticulos gestart = new GestionArticulos();
        Articulo art = new Articulo();
        GestionCategorias gescat = new GestionCategorias();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            cargarGvArticulos();
            CargarDlCategorias();
        }

       protected void CargarDlCategorias()
        {
            dlCategorias.DataSource = gescat.getCategorias();
            dlCategorias.DataBind();
        }

        protected void gvArticulos_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            art.Descripcion1 = ((Label)gvArticulos.Rows[e.NewSelectedIndex].FindControl("lblDescripcion")).Text;
            art.Precio_Unitario1 = (float)Convert.ToDouble(((Label)gvArticulos.Rows[e.NewSelectedIndex].FindControl("lblPrecioUnitario")).Text);
            
            if (Session["ArtSeleccionados"] == null)
            {
                DataTable TablaSeleccionados = gestart.CrearTablaArticulos();
                gestart.AgregarFila(ref TablaSeleccionados, art);
                Session["ArtSeleccionados"] = TablaSeleccionados;
                lblMensajeSeleccionado.Text = "Producto Agregado: " + art.Descripcion1;
            }
            else
            {
                ValidacionArtExiste();
            }

            
        }

        private void ValidacionArtExiste()
        {
            DataTable TablaSeleccionados = (DataTable)Session["ArtSeleccionados"];
            bool Existe = false;
            foreach (DataRow row in TablaSeleccionados.Rows)
            {
                
                if (art.Descripcion1 == row["DESCRIPCION"].ToString())
                {
                    Existe = true;
                    int Cantidad = Convert.ToInt32(row["CANTIDAD"]);
                    row.SetField("CANTIDAD", Cantidad + 1);


                }
            }

            if (!Existe)
            {
                gestart.AgregarFila(ref TablaSeleccionados, art);
                Session["ArtSeleccionados"] = TablaSeleccionados;
                lblMensajeSeleccionado.Text = "Producto Agregado: " + art.Descripcion1;
            }
            else
            {
               // System.Windows.Forms.MessageBox.Show("Este articulo ya fue añadido al carrito. Podrás modificar su cantidad mas tarde", "Atencion!");
            }

            return;
        }

        protected void ddlMenuNavegacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int op = Convert.ToInt32(ddlMenuNavegacion.SelectedValue);
            switch (op)
            {
                case 1:
                    Server.Transfer("~/UsuarioForms/EdicionDatosUsuario.aspx");
                    break;
                case 2:
                    Server.Transfer("~/UsuarioForms/HistorialCompras.aspx");
                    break;
                case 3:
                    Session["NombreUsuario"] = null;
                    Server.Transfer("~/Inicio.aspx");
                    break;
            }
            
        }

        protected void btnIralCarrito_Click(object sender, EventArgs e)
        {
            Server.Transfer("Carrito.aspx");
        }

        protected void gvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvArticulos.PageIndex = e.NewPageIndex;
            cargarGvArticulos();
        }

        protected void btnbuscar_Click(object sender, EventArgs e)
        {
            if (txtArticuloBuscar.Text == "")
            {
                cargarGvArticulos();
            }
            else
            {
                art.Descripcion1 = txtArticuloBuscar.Text;
                gvArticulos.DataSource = gestart.getArticuloBuscar(art);
                gvArticulos.DataBind();
                txtArticuloBuscar.Text = "";
            }            
        }

        protected void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            cargarGvArticulos();
        }

        public void cargarGvArticulos()
        {
            lblNombreUsuario.Text = Session["NombreUsuario"].ToString();
            gvArticulos.DataSource = gestart.getArticulos();
            gvArticulos.DataBind();
        }

        protected void btnCategoria_Command(object sender, CommandEventArgs e)
        {
            if(e.CommandName == "SeleccionarCategoria")
            {
                Categoria aux = new Categoria
                {
                    IdCategoria1 = Convert.ToInt32(e.CommandArgument.ToString())
                };

                art.Categoria1 = aux;
                gvArticulos.DataSource = gestart.getarticulosXCategoria(art);
                gvArticulos.DataBind();
            }
        }
    }
}