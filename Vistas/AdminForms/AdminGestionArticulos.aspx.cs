using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas.AdminForms
{
    public partial class AdminHome : System.Web.UI.Page
    {
        GestionArticulos gestart = new GestionArticulos();
        Articulo art = new Articulo();
        GestionCategorias gcat = new GestionCategorias();
        Categoria cat = new Categoria();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblNombreadm.Text = Session["NombreUsuario"].ToString();
                CargarGvArticulos();
                CargarGvCategorias();
            }
        }

        //-------------------------------------------------------------------------
        //GESTION ARTICULOS
        //-------------------------------------------------------------------------
        public void CargarGvArticulos()
        {
            gvArticulos.DataSource = gestart.getArticulos();
            gvArticulos.DataBind();
        }

        protected void btnCargarArticulos_Click(object sender, EventArgs e)
        {
            CargarGvArticulos();
            txtIdArticulo.Text = "";
        }

        protected void gvArticulos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvArticulos.EditIndex = e.NewEditIndex;
            CargarGvArticulos();
        }

        protected void gvArticulos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvArticulos.EditIndex = -1;
            CargarGvArticulos();
        }

        protected void gvArticulos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            art.CodArticulo1 = ((Label)gvArticulos.Rows[e.RowIndex].FindControl("lbl_et_Codigo")).Text;
            art.Categoria1.IdCategoria1 = Convert.ToInt32(((Label)gvArticulos.Rows[e.RowIndex].FindControl("lbl_et_Categoria")).Text);
            art.Descripcion1 = ((TextBox)gvArticulos.Rows[e.RowIndex].FindControl("txt_et_Descripcion")).Text;
            art.Stock1 = Convert.ToInt32(((TextBox)gvArticulos.Rows[e.RowIndex].FindControl("txt_et_Stock")).Text);
            art.Precio_Unitario1 = (float)Convert.ToDouble(((TextBox)gvArticulos.Rows[e.RowIndex].FindControl("txt_et_PrecioUnitario")).Text);
            art.ImagenURL1 = ((TextBox)gvArticulos.Rows[e.RowIndex].FindControl("txt_et_Imagen")).Text;
            art.Estado1 = Convert.ToBoolean(((DropDownList)gvArticulos.Rows[e.RowIndex].FindControl("ddlEditEstado")).Text);

            if(gestart.ExisteDescripcionArticuloModificar(art) == true)
            {
                gestart.ActualizarArticulo(art);
                gvArticulos.EditIndex = -1;
                CargarGvArticulos();
                lblMensajeArticulo.Text = "ARTICULO MODIFICADO";
            }
            else
            {
                gvArticulos.EditIndex = -1;
                CargarGvArticulos();
                lblMensajeArticulo.Text = "LA DESCRIPCION DEL ARTICULO CONINCIDE CON OTRO";
            }

            
        }

        protected void gvArticulos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            art.CodArticulo1 = ((Label)gvArticulos.Rows[e.RowIndex].FindControl("lbl_it_Codart")).Text;

            gestart.eliminarArticulo(art);
            gvArticulos.EditIndex = -1;
            CargarGvArticulos();
        }

        protected void gvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvArticulos.PageIndex = e.NewPageIndex;
            CargarGvArticulos();
        }

        protected void btnBuscarporCodigo_Click(object sender, EventArgs e)
        {

            if (txtIdArticulo.Text != "")
            {
                //lo puse aca porque por fuera guardaba la el txt vacio el objeto
                art.CodArticulo1 = txtIdArticulo.Text;
                /*----------------------------------------*/
                gvArticulos.DataSource = gestart.getArticuloCOD(art);
                gvArticulos.DataBind();
                txtIdArticulo.Text = "";
            }
            else
            {
                CargarGvArticulos();
                txtIdArticulo.Text = "";
            }
        }

        protected void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            art.CodArticulo1 = txtCodArticulo.Text;
            art.Descripcion1 = txtDescripcion.Text;
            cat.IdCategoria1 = Convert.ToInt32(txtCategoria.Text);
            /*Esta decisiones la hice porque si ingresabas un nombre ya existente, una categoria inexistente y una descripcion igual
             *  a otra, tiraba error. Y la ultima es para no ingregar una categoria inactiva */
            if (gestart.ExisteArticulo(art) == false && gcat.ExisteCategoria (cat) == true && gestart.ExisteDescripcionArticulo(art)==false && gcat.VerificarEstadoCat(cat)==true)
            {
                lblMensajeA.Text = "ARTICULO AGREGADO";
                art.CodArticulo1 = txtCodArticulo.Text;
                art.Categoria1 = cat;
                art.Descripcion1 = txtDescripcion.Text;
                art.Stock1 = Convert.ToInt32(txtStock.Text);
                art.Precio_Unitario1 = (float)Convert.ToDouble(txtPrecio_Unitario.Text);
                art.ImagenURL1 = txtImagen.Text;
                art.Estado1 = true;

               
                LimpiarCampoArticulo();
                gestart.agregarArticulo(art);
                CargarGvArticulos();
                
            }
            else
            {
                LimpiarCampoArticulo();
                lblMensajeA.Text = "DATOS INVALIDOS";
            }
            

            
        }

        public void LimpiarCampoArticulo()
        {
            txtCodArticulo.Text = "";
            txtCategoria.Text = "";
            txtDescripcion.Text = "";
            txtStock.Text = "";
            txtPrecio_Unitario.Text = "";
            txtImagen.Text = "";
        }

        //-------------------------------------------------------------------------
        //GESTION CATEGORIAS
        //-------------------------------------------------------------------------
        public void CargarGvCategorias()
        {
            gvCategorias.DataSource = gcat.getCategorias();
            gvCategorias.DataBind();
        }
        
        protected void btnBuscarCategoria_Click(object sender, EventArgs e)//Funciona OK
        {            
            
            if(txtBuscarCatid.Text != "")
            {
                /*Por el motivo anterior*/
                cat.IdCategoria1 = Convert.ToInt32(txtBuscarCatid.Text);
                gvCategorias.DataSource = gcat.getCategoriaId(cat);
                gvCategorias.DataBind();
                txtBuscarCatid.Text = "";
            }
            else
            {
                CargarGvCategorias();
                txtBuscarCatid.Text = "";
            }

        }

        protected void gvCategorias_RowDeleting(object sender, GridViewDeleteEventArgs e)//Funciona OK
        {
            cat.IdCategoria1 = Convert.ToInt32(((Label)gvCategorias.Rows[e.RowIndex].FindControl("lbl_it_IdCategoria")).Text);
            gcat.eliminarCategoria(cat);
            CargarGvCategorias();
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)//Funciona OK
        {
            cat.Decripcion1 = txtDescripcioncat.Text;

            /*Esta decision es para no ingresar una Descripcion existente, antes me tiraba error*/
            if (gcat.VerificarCategoria(cat)==false)
            {
                if (gcat.agregarCategoria(cat))
                {
                    lblAgregarCategoria.Text = "AGREGADA CON EXITO!";
                    CargarGvCategorias();
                    txtDescripcioncat.Text = "";
                }
                
            }
            else
            {
                lblAgregarCategoria.Text = "Nombre de categoría en uso, ingrese otro nombre";
                CargarGvCategorias();
                txtDescripcioncat.Text = "";
            }            
        }

        protected void gvCategorias_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategorias.EditIndex = e.NewEditIndex;
            CargarGvCategorias();
        }

        protected void gvCategorias_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCategorias.EditIndex = -1;
            CargarGvCategorias();
        }

        protected void gvCategorias_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            cat.Decripcion1 = ((TextBox)gvCategorias.Rows[e.RowIndex].FindControl("txt_eit_Decripcion")).Text;
            cat.IdCategoria1 = Convert.ToInt32(((Label)gvCategorias.Rows[e.RowIndex].FindControl("lbl_eit_IdCategoria")).Text);
            cat.Estado1 = Convert.ToBoolean(((DropDownList)gvCategorias.Rows[e.RowIndex].FindControl("ddlEditEstadoCat")).SelectedValue);

            if (gcat.ExisteCategoriaModificar(cat) == false)
            {
                gcat.ActualizarCategoria(cat);
                gvCategorias.EditIndex = -1;
                CargarGvCategorias();
                lblMensajeCat.Text = "CATEGORIA MODIFICADA";
            }
            else
            {
                gvCategorias.EditIndex = -1;
                CargarGvCategorias();
                lblMensajeCat.Text = "LA DESCRIPCION DE LA CATEGORIA YA FUE CARGADA";
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CargarGvCategorias();
            txtBuscarCatid.Text = "";
        }

       
    }
}