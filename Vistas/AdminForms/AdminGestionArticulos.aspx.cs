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
        private readonly GestionArticulos _gestionArticulos = new GestionArticulos();
        private readonly GestionCategorias _gestionCategorias = new GestionCategorias();
        private DataTable _categorias;

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Admin; }
        }

        /* Código buscado; vacío = todos los artículos. Se conserva entre postbacks. */
        private string FiltroCodigo
        {
            get { return (string)ViewState["FiltroCodigo"] ?? ""; }
            set { ViewState["FiltroCodigo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _categorias = _gestionCategorias.GetCategorias();
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
            DataView activas = new DataView(_categorias) { RowFilter = "Estado_ca = true", Sort = "Descripcion_ca" };
            return activas;
        }

        protected string NombreCategoria(object idCategoria)
        {
            DataRow[] fila = _categorias.Select("IdCategoria_ca = " + Convert.ToInt64(idCategoria));
            return fila.Length > 0 ? fila[0]["Descripcion_ca"].ToString() : "—";
        }

        //-------------------------------------------------------------------------
        // ARTÍCULOS
        //-------------------------------------------------------------------------
        private void CargarGvArticulos()
        {
            gvArticulos.DataSource = FiltroCodigo == ""
                ? _gestionArticulos.GetArticulos()
                : _gestionArticulos.GetArticuloPorCodigo(new Articulo { CodigoArticulo = FiltroCodigo });
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

        private const string ErrorImagen = "La imagen tiene que ser una ruta del sitio (~/Imagenes_ar/...) o una dirección http(s).";

        protected void gvArticulos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow fila = gvArticulos.Rows[e.RowIndex];
            Articulo articulo = new Articulo
            {
                CodigoArticulo = gvArticulos.DataKeys[e.RowIndex].Value.ToString(),
                Categoria = new Categoria { IdCategoria = Convert.ToInt32(((DropDownList)fila.FindControl("ddl_et_Categoria")).SelectedValue) },
                Descripcion = ((TextBox)fila.FindControl("txt_et_Descripcion")).Text.Trim(),
                Stock = Convert.ToInt32(((TextBox)fila.FindControl("txt_et_Stock")).Text),
                PrecioUnitario = Convert.ToDecimal(((TextBox)fila.FindControl("txt_et_PrecioUnitario")).Text),
                ImagenUrl = ((TextBox)fila.FindControl("txt_et_Imagen")).Text.Trim(),
                Estado = Convert.ToBoolean(((DropDownList)fila.FindControl("ddl_et_Estado")).SelectedValue)
            };

            if (!GestionArticulos.ImagenValida(articulo.ImagenUrl))
            {
                MostrarMensaje(ErrorImagen, TipoMensaje.Error);
                return;
            }
            if (_gestionArticulos.ExisteDescripcionArticuloModificar(articulo))
            {
                MostrarMensaje("Ya existe otro artículo con esa descripción.", TipoMensaje.Error);
                return;
            }
            if (_gestionArticulos.ActualizarArticulo(articulo))
            {
                MostrarMensaje("Artículo " + articulo.CodigoArticulo + " modificado.", TipoMensaje.Exito);
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
            Articulo articulo = new Articulo { CodigoArticulo = gvArticulos.DataKeys[e.RowIndex].Value.ToString() };
            if (_gestionArticulos.EliminarArticulo(articulo))
            {
                MostrarMensaje("Artículo " + articulo.CodigoArticulo + " dado de baja.", TipoMensaje.Exito);
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
            Articulo articulo = new Articulo
            {
                CodigoArticulo = txtCodArticulo.Text.Trim(),
                Categoria = new Categoria { IdCategoria = Convert.ToInt32(ddlCategoria.SelectedValue) },
                Descripcion = txtDescripcion.Text.Trim(),
                Stock = Convert.ToInt32(txtStock.Text),
                PrecioUnitario = Convert.ToDecimal(txtPrecio_Unitario.Text),
                ImagenUrl = txtImagen.Text.Trim(),
                Estado = true
            };

            if (!GestionArticulos.CodigoValido(articulo.CodigoArticulo))
            {
                MostrarMensaje("El código tiene que tener de 1 a 4 letras o números.", TipoMensaje.Error);
                return;
            }
            if (!GestionArticulos.ImagenValida(articulo.ImagenUrl))
            {
                MostrarMensaje(ErrorImagen, TipoMensaje.Error);
                return;
            }
            if (_gestionArticulos.ExisteArticulo(articulo))
            {
                MostrarMensaje("Ya existe un artículo con el código " + articulo.CodigoArticulo + ".", TipoMensaje.Error);
                return;
            }
            if (_gestionArticulos.ExisteDescripcionArticulo(articulo))
            {
                MostrarMensaje("Ya existe un artículo con esa descripción.", TipoMensaje.Error);
                return;
            }
            if (!_gestionArticulos.AgregarArticulo(articulo))
            {
                MostrarMensaje("No se pudo agregar el artículo.", TipoMensaje.Error);
                return;
            }
            MostrarMensaje("Artículo " + articulo.CodigoArticulo + " agregado.", TipoMensaje.Exito);
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
            gvCategorias.DataSource = _categorias;
            gvCategorias.DataBind();
        }

        /* Después de modificar categorías se recargan también los desplegables de artículos. */
        private void RecargarCategorias()
        {
            _categorias = _gestionCategorias.GetCategorias();
            CargarGvCategorias();
            ddlCategoria.DataSource = CategoriasActivas();
            ddlCategoria.DataBind();
            CargarGvArticulos();
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            Categoria categoria = new Categoria { Descripcion = txtDescripcioncat.Text.Trim() };
            if (_gestionCategorias.VerificarCategoria(categoria))
            {
                MostrarMensaje("Ya existe una categoría con ese nombre.", TipoMensaje.Error);
                return;
            }
            if (_gestionCategorias.AgregarCategoria(categoria))
            {
                MostrarMensaje("Categoría \"" + categoria.Descripcion + "\" agregada.", TipoMensaje.Exito);
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
            Categoria categoria = new Categoria
            {
                IdCategoria = Convert.ToInt32(gvCategorias.DataKeys[e.RowIndex].Value),
                Descripcion = ((TextBox)fila.FindControl("txt_eit_Decripcion")).Text.Trim(),
                Estado = Convert.ToBoolean(((DropDownList)fila.FindControl("ddlEditEstadoCat")).SelectedValue)
            };

            if (_gestionCategorias.ExisteCategoriaModificar(categoria))
            {
                MostrarMensaje("Ya existe otra categoría con ese nombre.", TipoMensaje.Error);
                return;
            }
            _gestionCategorias.ActualizarCategoria(categoria);
            MostrarMensaje("Categoría modificada.", TipoMensaje.Exito);
            gvCategorias.EditIndex = -1;
            RecargarCategorias();
        }

        protected void gvCategorias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Categoria categoria = new Categoria { IdCategoria = Convert.ToInt32(gvCategorias.DataKeys[e.RowIndex].Value) };
            _gestionCategorias.EliminarCategoria(categoria);
            MostrarMensaje("Categoría desactivada.", TipoMensaje.Exito);
            gvCategorias.EditIndex = -1;
            RecargarCategorias();
        }
    }
}
