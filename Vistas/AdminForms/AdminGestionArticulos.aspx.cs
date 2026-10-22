using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas.AdminForms
{
    public partial class AdminGestionArticulos : PaginaBase
    {
        GestionArticulos gestart = new GestionArticulos();
        GestionCategorias gcat = new GestionCategorias();
        DataTable categorias;

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Admin; }
        }

        /* Código buscado; vacío = todos los artículos. Se conserva entre postbacks. */
        string FiltroCodigo
        {
            get { return (string)ViewState["FiltroCodigo"] ?? ""; }
            set { ViewState["FiltroCodigo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            categorias = gcat.getCategorias();
            if (!IsPostBack)
            {
                ddlCategoria.DataSource = CategoriasActivas();
                ddlCategoria.DataBind();
                CargarGvArticulos();
                CargarGvCategorias();
            }
        }

        private DataView CategoriasActivas()
        {
            DataView activas = new DataView(categorias) { RowFilter = "Estado_ca = true", Sort = "Descripcion_ca" };
            return activas;
        }

        protected string NombreCategoria(object idCategoria)
        {
            DataRow[] fila = categorias.Select("IdCategoria_ca = " + Convert.ToInt64(idCategoria));
            return fila.Length > 0 ? fila[0]["Descripcion_ca"].ToString() : "—";
        }

        //-------------------------------------------------------------------------
        // ARTÍCULOS
        //-------------------------------------------------------------------------
        private void CargarGvArticulos()
        {
            gvArticulos.DataSource = FiltroCodigo == ""
                ? gestart.getArticulos()
                : gestart.getArticuloCOD(new Articulo { CodArticulo1 = FiltroCodigo });
            gvArticulos.DataBind();
        }

        protected void btnBuscarporCodigo_Click(object sender, EventArgs e)
        {
            FiltroCodigo = txtIdArticulo.Text.Trim();
            gvArticulos.EditIndex = -1;
            gvArticulos.PageIndex = 0;
            CargarGvArticulos();
        }

        protected void btnCargarArticulos_Click(object sender, EventArgs e)
        {
            FiltroCodigo = "";
            txtIdArticulo.Text = "";
            gvArticulos.EditIndex = -1;
            CargarGvArticulos();
        }

        protected void gvArticulos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) != 0)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_et_Categoria");
                ddl.DataSource = CategoriasActivas();
                ddl.DataBind();
                string actual = DataBinder.Eval(e.Row.DataItem, "Categoria_ar").ToString();
                if (ddl.Items.FindByValue(actual) == null)
                {
                    // Categoría desactivada: se ofrece igual para no cambiarla sin querer.
                    ddl.Items.Insert(0, new ListItem(NombreCategoria(actual) + " (inactiva)", actual));
                }
                ddl.SelectedValue = actual;
            }
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
            GridViewRow fila = gvArticulos.Rows[e.RowIndex];
            Articulo art = new Articulo
            {
                CodArticulo1 = gvArticulos.DataKeys[e.RowIndex].Value.ToString(),
                Categoria1 = new Categoria { IdCategoria1 = Convert.ToInt32(((DropDownList)fila.FindControl("ddl_et_Categoria")).SelectedValue) },
                Descripcion1 = ((TextBox)fila.FindControl("txt_et_Descripcion")).Text.Trim(),
                Stock1 = Convert.ToInt32(((TextBox)fila.FindControl("txt_et_Stock")).Text),
                Precio_Unitario1 = (float)Convert.ToDouble(((TextBox)fila.FindControl("txt_et_PrecioUnitario")).Text),
                ImagenURL1 = ((TextBox)fila.FindControl("txt_et_Imagen")).Text.Trim()
            };

            if (gestart.ExisteDescripcionArticuloModificar(art))
            {
                MostrarMensaje("Ya existe otro artículo con esa descripción.", TipoMensaje.Error);
                return;
            }
            if (gestart.ActualizarArticulo(art))
            {
                MostrarMensaje("Artículo " + art.CodArticulo1 + " modificado.", TipoMensaje.Exito);
            }
            else
            {
                MostrarMensaje("No se pudo modificar el artículo.", TipoMensaje.Error);
            }
            gvArticulos.EditIndex = -1;
            CargarGvArticulos();
        }

        protected void gvArticulos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Articulo art = new Articulo { CodArticulo1 = gvArticulos.DataKeys[e.RowIndex].Value.ToString() };
            if (gestart.eliminarArticulo(art))
            {
                MostrarMensaje("Artículo " + art.CodArticulo1 + " dado de baja.", TipoMensaje.Exito);
            }
            gvArticulos.EditIndex = -1;
            CargarGvArticulos();
        }

        protected void gvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvArticulos.PageIndex = e.NewPageIndex;
            gvArticulos.EditIndex = -1;
            CargarGvArticulos();
        }

        protected void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            Articulo art = new Articulo
            {
                CodArticulo1 = txtCodArticulo.Text.Trim(),
                Categoria1 = new Categoria { IdCategoria1 = Convert.ToInt32(ddlCategoria.SelectedValue) },
                Descripcion1 = txtDescripcion.Text.Trim(),
                Stock1 = Convert.ToInt32(txtStock.Text),
                Precio_Unitario1 = (float)Convert.ToDouble(txtPrecio_Unitario.Text),
                ImagenURL1 = txtImagen.Text.Trim(),
                Estado1 = true
            };

            if (gestart.ExisteArticulo(art))
            {
                MostrarMensaje("Ya existe un artículo con el código " + art.CodArticulo1 + ".", TipoMensaje.Error);
                return;
            }
            if (gestart.ExisteDescripcionArticulo(art))
            {
                MostrarMensaje("Ya existe un artículo con esa descripción.", TipoMensaje.Error);
                return;
            }
            if (!gestart.agregarArticulo(art))
            {
                MostrarMensaje("No se pudo agregar el artículo.", TipoMensaje.Error);
                return;
            }
            MostrarMensaje("Artículo " + art.CodArticulo1 + " agregado.", TipoMensaje.Exito);
            LimpiarCampoArticulo();
            CargarGvArticulos();
        }

        private void LimpiarCampoArticulo()
        {
            txtCodArticulo.Text = "";
            txtDescripcion.Text = "";
            txtStock.Text = "";
            txtPrecio_Unitario.Text = "";
            txtImagen.Text = "";
        }

        //-------------------------------------------------------------------------
        // CATEGORÍAS
        //-------------------------------------------------------------------------
        private void CargarGvCategorias()
        {
            gvCategorias.DataSource = categorias;
            gvCategorias.DataBind();
        }

        /* Después de modificar categorías se recargan también los desplegables de artículos. */
        private void RecargarCategorias()
        {
            categorias = gcat.getCategorias();
            CargarGvCategorias();
            ddlCategoria.DataSource = CategoriasActivas();
            ddlCategoria.DataBind();
            CargarGvArticulos();
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            Categoria cat = new Categoria { Decripcion1 = txtDescripcioncat.Text.Trim() };
            if (gcat.VerificarCategoria(cat))
            {
                MostrarMensaje("Ya existe una categoría con ese nombre.", TipoMensaje.Error);
                return;
            }
            if (gcat.agregarCategoria(cat))
            {
                MostrarMensaje("Categoría \"" + cat.Decripcion1 + "\" agregada.", TipoMensaje.Exito);
                txtDescripcioncat.Text = "";
                RecargarCategorias();
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
            GridViewRow fila = gvCategorias.Rows[e.RowIndex];
            Categoria cat = new Categoria
            {
                IdCategoria1 = Convert.ToInt32(gvCategorias.DataKeys[e.RowIndex].Value),
                Decripcion1 = ((TextBox)fila.FindControl("txt_eit_Decripcion")).Text.Trim(),
                Estado1 = Convert.ToBoolean(((DropDownList)fila.FindControl("ddlEditEstadoCat")).SelectedValue)
            };

            if (gcat.ExisteCategoriaModificar(cat))
            {
                MostrarMensaje("Ya existe otra categoría con ese nombre.", TipoMensaje.Error);
                return;
            }
            gcat.ActualizarCategoria(cat);
            MostrarMensaje("Categoría modificada.", TipoMensaje.Exito);
            gvCategorias.EditIndex = -1;
            RecargarCategorias();
        }

        protected void gvCategorias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Categoria cat = new Categoria { IdCategoria1 = Convert.ToInt32(gvCategorias.DataKeys[e.RowIndex].Value) };
            gcat.eliminarCategoria(cat);
            MostrarMensaje("Categoría desactivada.", TipoMensaje.Exito);
            gvCategorias.EditIndex = -1;
            RecargarCategorias();
        }
    }
}
