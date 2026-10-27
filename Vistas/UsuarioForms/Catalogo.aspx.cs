using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class Catalogo : PaginaBase
    {
        const int ArticulosPorPagina = 12;

        GestionArticulos gestart = new GestionArticulos();
        GestionCategorias gescat = new GestionCategorias();

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Cliente; }
        }

        /* Filtros y página vienen por querystring: las URLs se pueden compartir
           y el botón "Atrás" del navegador funciona. */
        int? CategoriaSeleccionada
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["cat"], out id) ? id : (int?)null;
            }
        }

        string Busqueda
        {
            get { return (Request.QueryString["q"] ?? "").Trim(); }
        }

        int PaginaActual
        {
            get
            {
                int pagina;
                return int.TryParse(Request.QueryString["pagina"], out pagina) && pagina > 0 ? pagina : 1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtArticuloBuscar.Text = Busqueda;
                CargarCategorias();
                CargarArticulos();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            CargarResumenCarrito();
            base.OnPreRender(e);
        }

        private void CargarCategorias()
        {
            DataView activas = gescat.GetCategorias().DefaultView;
            activas.RowFilter = "Estado_ca = true";
            rptCategorias.DataSource = activas;
            rptCategorias.DataBind();
        }

        private void CargarArticulos()
        {
            // La búsqueda tiene prioridad sobre la categoría, como en los links de la página.
            DataTable articulos = gestart.GetArticulosCatalogo(Busqueda, Busqueda != "" ? null : CategoriaSeleccionada);
            lblResultado.Text = Busqueda != ""
                ? string.Format("Resultados para \"{0}\": {1}", HttpUtility.HtmlEncode(Busqueda), articulos.Rows.Count)
                : articulos.Rows.Count + " productos";

            int totalPaginas = Math.Max(1, (int)Math.Ceiling(articulos.Rows.Count / (double)ArticulosPorPagina));
            int pagina = Math.Min(PaginaActual, totalPaginas);

            DataTable paginaArticulos = articulos.Clone();
            for (int i = (pagina - 1) * ArticulosPorPagina; i < Math.Min(pagina * ArticulosPorPagina, articulos.Rows.Count); i++)
            {
                paginaArticulos.ImportRow(articulos.Rows[i]);
            }

            rptArticulos.DataSource = paginaArticulos;
            rptArticulos.DataBind();
            pnlSinResultados.Visible = articulos.Rows.Count == 0;
            litPaginacion.Text = totalPaginas > 1 ? ArmarPaginacion(pagina, totalPaginas) : "";
        }

        protected void rptArticulos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Agregar" && e.CommandName != "AgregarCantidad")
            {
                return;
            }
            int cantidad = 1;
            if (e.CommandName == "AgregarCantidad")
            {
                // Desde el modal de detalle, con la cantidad elegida.
                int.TryParse(((TextBox)e.Item.FindControl("txtCantidad")).Text, out cantidad);
                cantidad = Math.Max(1, Math.Min(cantidad, 99));
            }
            // Precio y stock se leen de la base, no de lo que muestra la página.
            DataTable tabla = gestart.GetArticuloCOD(new Articulo { CodigoArticulo = e.CommandArgument.ToString() });
            if (tabla.Rows.Count == 0 || !(bool)tabla.Rows[0]["Estado_ar"])
            {
                MostrarMensaje("El producto ya no está disponible.", TipoMensaje.Error);
                return;
            }
            DataRow fila = tabla.Rows[0];
            string descripcion = fila["Descripcion_ar"].ToString();
            int agregadas = 0;
            while (agregadas < cantidad && Sesion.Carrito.Agregar(fila["CodArticulo_ar"].ToString(), descripcion,
                Convert.ToDecimal(fila["PrecioUnitario_ar"]), Convert.ToInt32(fila["Stock_ar"])))
            {
                agregadas++;
            }

            if (agregadas == cantidad)
            {
                MostrarMensaje(cantidad == 1
                    ? "Agregaste \"" + descripcion + "\" al carrito."
                    : "Agregaste " + cantidad + " unidades de \"" + descripcion + "\" al carrito.", TipoMensaje.Exito);
            }
            else if (agregadas > 0)
            {
                MostrarMensaje("Solo había stock para agregar " + agregadas + " unidades de \"" + descripcion + "\".", TipoMensaje.Aviso);
            }
            else
            {
                MostrarMensaje("No hay más stock de \"" + descripcion + "\".", TipoMensaje.Aviso);
            }
        }

        /* "Clave: Valor" por línea -> pares para la tabla de especificaciones del modal. */
        protected IEnumerable<KeyValuePair<string, string>> Especificaciones(object texto)
        {
            var pares = new List<KeyValuePair<string, string>>();
            foreach (string linea in (texto as string ?? "").Split('\n'))
            {
                int separador = linea.IndexOf(':');
                if (separador > 0)
                {
                    pares.Add(new KeyValuePair<string, string>(linea.Substring(0, separador).Trim(), linea.Substring(separador + 1).Trim()));
                }
            }
            if (pares.Count == 0)
            {
                pares.Add(new KeyValuePair<string, string>("Especificaciones", "Consultá con nuestro equipo de ventas"));
            }
            return pares;
        }

        /* Id HTML a partir del código del artículo, con solo caracteres seguros
           (va dentro de atributos y de los selectores de los modales). */
        protected string IdHtml(string prefijo, object codigo)
        {
            return prefijo + "-" + Regex.Replace(Convert.ToString(codigo), "[^A-Za-z0-9_-]", "_");
        }

        protected string TextoGarantia(object meses)
        {
            int valor = meses is DBNull || meses == null ? 12 : Convert.ToInt32(meses);
            return valor % 12 == 0 && valor >= 24 ? (valor / 12) + " años" : valor + " meses";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtArticuloBuscar.Text.Trim();
            Redirigir(texto == "" ? "~/UsuarioForms/Catalogo.aspx" : "~/UsuarioForms/Catalogo.aspx?q=" + HttpUtility.UrlEncode(texto));
        }

        private void CargarResumenCarrito()
        {
            Entidades.Carrito carrito = Sesion.Carrito;
            pnlCarritoVacio.Visible = carrito.Vacio;
            pnlCarrito.Visible = !carrito.Vacio;
            rptCarrito.DataSource = carrito.Items;
            rptCarrito.DataBind();
            lblTotalCompra.Text = carrito.Total.ToString("C0");
        }

        protected string ClaseCategoria(object idCategoria)
        {
            bool activa = idCategoria == null
                ? !CategoriaSeleccionada.HasValue && Busqueda == ""
                : CategoriaSeleccionada == Convert.ToInt32(idCategoria);
            return activa ? "nav-link active" : "nav-link";
        }

        protected string UrlCategoria(object idCategoria)
        {
            return ResolveUrl("~/UsuarioForms/Catalogo.aspx?cat=" + idCategoria);
        }

        private string ArmarPaginacion(int pagina, int totalPaginas)
        {
            StringBuilder html = new StringBuilder("<ul class=\"pagination justify-content-center\">");
            html.Append(ItemPaginacion("&laquo;", pagina - 1, pagina == 1, false));
            for (int i = 1; i <= totalPaginas; i++)
            {
                html.Append(ItemPaginacion(i.ToString(), i, false, i == pagina));
            }
            html.Append(ItemPaginacion("&raquo;", pagina + 1, pagina == totalPaginas, false));
            return html.Append("</ul>").ToString();
        }

        private string ItemPaginacion(string texto, int pagina, bool deshabilitado, bool activo)
        {
            List<string> parametros = new List<string>();
            if (CategoriaSeleccionada.HasValue) parametros.Add("cat=" + CategoriaSeleccionada.Value);
            if (Busqueda != "") parametros.Add("q=" + HttpUtility.UrlEncode(Busqueda));
            parametros.Add("pagina=" + pagina);
            string url = ResolveUrl("~/UsuarioForms/Catalogo.aspx?" + string.Join("&", parametros));

            string clase = "page-item" + (deshabilitado ? " disabled" : "") + (activo ? " active" : "");
            return string.Format("<li class=\"{0}\"><a class=\"page-link\" href=\"{1}\">{2}</a></li>",
                clase, HttpUtility.HtmlAttributeEncode(url), texto);
        }
    }
}
