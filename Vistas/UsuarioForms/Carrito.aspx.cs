using System;
using System.Data;
using System.Web.UI.WebControls;
using Entidades;
using Negocio;

namespace Vistas
{
    public partial class Carrito : PaginaBase
    {
        GestionVentas gestven = new GestionVentas();
        GestionTarjetas gestcard = new GestionTarjetas();

        protected override NivelAcceso Acceso
        {
            get { return NivelAcceso.Cliente; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCarrito();
                CargarTarjetas();
            }
        }

        private void CargarCarrito()
        {
            Entidades.Carrito carrito = Sesion.Carrito;
            pnlCarritoVacio.Visible = carrito.Vacio;
            pnlCompra.Visible = !carrito.Vacio;
            gvMiscompras.DataSource = carrito.Items;
            gvMiscompras.DataBind();
            lblTotalCompra.Text = carrito.Total.ToString("C0");
        }

        /* El valor de cada opción es el id de la tarjeta: el código de seguridad no sale del servidor. */
        private void CargarTarjetas()
        {
            DataTable tarjetas = gestcard.getTablaTarjetasUsuario(Sesion.Usuario);
            ddlTarjetas.Items.Clear();
            ddlTarjetas.Items.Add(new ListItem("Elegí una tarjeta", ""));
            foreach (DataRow fila in tarjetas.Rows)
            {
                string texto = string.Format("{0} terminada en {1} ({2})", fila["Nombre_Tarjeta_t"], fila["Num_Tarjeta_t"], fila["Tipo_Tarjeta_t"]);
                ddlTarjetas.Items.Add(new ListItem(texto, fila["IdTarjeta_t"].ToString()));
            }
            lblSinTarjetas.Visible = tarjetas.Rows.Count == 0;
            if (tarjetas.Rows.Count == 1)
            {
                ddlTarjetas.SelectedIndex = 1;
            }
        }

        protected void gvMiscompras_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string codigo = gvMiscompras.DataKeys[e.RowIndex].Value.ToString();
            Sesion.Carrito.Quitar(codigo);
            CargarCarrito();
        }

        protected void btnCancelarCompra_Click(object sender, EventArgs e)
        {
            Sesion.Carrito.Vaciar();
            RedirigirConMensaje("~/UsuarioForms/Catalogo.aspx", "Vaciaste el carrito.", TipoMensaje.Info);
        }

        protected void btnGuardarTarjeta_Click(object sender, EventArgs e)
        {
            Tarjeta tarjeta = new Tarjeta
            {
                Us = Sesion.Usuario,
                Num_Tarjeta1 = Convert.ToInt64(txtNumeroTarjeta.Text),
                Titular_Tarjeta1 = txtTarjetaTitular.Text.Trim(),
                Nombre_Tarjeta1 = txtNombreTarjeta.Text.Trim(),
                Fecha_Vencimiento1 = "01/" + txtTarjetames.Text + "/" + txtTarjetaAño.Text,
                Tipo_Tarjeta1 = rbDebitoCredito.SelectedValue == "1",
                Codigo_Seguridad1 = Convert.ToInt32(txtCodSeguridad.Text)
            };

            if (gestcard.agregarTarjeta(tarjeta))
            {
                MostrarMensaje("Tarjeta guardada. Ya podés elegirla como medio de pago.", TipoMensaje.Exito);
                LimpiarCamposTarjeta();
                CargarTarjetas();
                ddlTarjetas.SelectedIndex = ddlTarjetas.Items.Count - 1;
            }
            else
            {
                MostrarMensaje("No se pudo guardar la tarjeta: ya la tenías guardada, está vencida o tiene el mismo código de seguridad que otra tuya.", TipoMensaje.Error);
            }
        }

        protected void btnConfirmarCompra_Click(object sender, EventArgs e)
        {
            long idTarjeta;
            long.TryParse(ddlTarjetas.SelectedValue, out idTarjeta);
            string error;
            int idVenta = gestven.ConfirmarCompra(Sesion.Usuario, Sesion.Carrito, txtDireccionDeEntrega.Text.Trim(), txtContacto.Text.Trim(),
                idTarjeta, out error);

            if (idVenta == 0)
            {
                MostrarMensaje(error, TipoMensaje.Error);
                return;
            }
            Sesion.Carrito.Vaciar();
            RedirigirConMensaje("~/UsuarioForms/HistorialCompras.aspx?compra=" + idVenta,
                "¡Listo! Registramos tu compra N° " + idVenta + ".", TipoMensaje.Exito);
        }

        private void LimpiarCamposTarjeta()
        {
            txtNumeroTarjeta.Text = "";
            txtTarjetaTitular.Text = "";
            txtNombreTarjeta.Text = "";
            txtTarjetames.Text = "";
            txtTarjetaAño.Text = "";
            txtCodSeguridad.Text = "";
        }
    }
}
