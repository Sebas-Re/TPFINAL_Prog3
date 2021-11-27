using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Entidades;
using Negocio;
using Label = System.Web.UI.WebControls.Label;

namespace Vistas
{
    public partial class Carrito : System.Web.UI.Page
    {
        GestionVentas gestven = new GestionVentas();
        Ventas venta = new Ventas();
        GestionDetalleVenta gestdetven = new GestionDetalleVenta();
        DetalleVentas detventa = new DetalleVentas();
        GestionArticulos gestart = new GestionArticulos();
        Articulo articulo = new Articulo();
        GestionUsuarios gestus = new GestionUsuarios();
        Usuario usuario = new Usuario();
        GestionTarjetas gestcard = new GestionTarjetas();
        Tarjeta Card = new Tarjeta();


        protected void Page_Load(object sender, EventArgs e)
        {
            lblNombreUsuario.Text = Session["NombreUsuario"].ToString();
            VerificacionSession();
            CargargvTarjetas();
            
        }

        private bool VerificacionSession()
        {
            if (Session["ArtSeleccionados"] != null)
            {
                DataTable Seleccionados = (DataTable)Session["ArtSeleccionados"];
                gvMiscompras.DataSource = Seleccionados;
                gvMiscompras.DataBind();

                ActivarBotones();
                CargarTotal(Seleccionados);
                return true;
            }
            else
            {
                DesactivarBotones();
                return false;
            }
        }

        private void CargarTotal(DataTable Seleccionados)
        {
            float total = 0;
            foreach (DataRow row in Seleccionados.Rows)
            {
                total += (float)Convert.ToDouble(row["PRECIO UNITARIO"]) * (float)Convert.ToDouble(row["CANTIDAD"]);

            }
            lblTotalCompra.Text = "$ " + total.ToString();
            return;
        }

        protected void btnCancelarCompra_Click(object sender, EventArgs e)
        {
            Session["ArtSeleccionados"] = null;
            Server.Transfer("~/UsuarioForms/Catalogo.aspx");
        }

        protected void dllMenuNavegacion_SelectedIndexChanged(object sender, EventArgs e)
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
                    Server.Transfer("~/UsuarioForms/Catalogo.aspx");
                    break;
                case 4:
                    Session["NombreUsuario"] = null;
                    Server.Transfer("~/Inicio.aspx");
                    break;
            }
        }

        private void DesactivarBotones()
        {
            btnConfirmarCompra.Enabled = false;
            btnConfirmarCompra.Visible = false;
            btnCancelarCompra.Enabled = false;
            btnCancelarCompra.Visible = false;
            lblMSJTotal.Visible = false;
            lblDirecEntrega.Visible = false;
            lblContacto.Visible = false;
            txtDireccionDeEntrega.Enabled = false;
            txtDireccionDeEntrega.Visible = false;
            txtContacto.Enabled = false;
            txtContacto.Visible = false;
            lblCarritoVacio.Visible = true;

        }

        private void ActivarBotones()
        {
            btnConfirmarCompra.Enabled = true;
            btnConfirmarCompra.Visible = true;
            btnCancelarCompra.Enabled = true;
            btnCancelarCompra.Visible = true;
            lblMSJTotal.Visible = true;
            lblDirecEntrega.Visible = true;
            lblContacto.Visible = true;
            txtDireccionDeEntrega.Enabled = true;
            txtDireccionDeEntrega.Visible = true;
            txtContacto.Enabled = true;
            txtContacto.Visible = true;
            lblCarritoVacio.Visible = false;
        }

        protected void btnVolverACatalogo_Click(object sender, EventArgs e)
        {
            Session["ArtSeleccionados"] = (DataTable)gvMiscompras.DataSource;
            Server.Transfer("~/UsuarioForms/Catalogo.aspx");
        }

        protected void btnConfirmarCompra_Click(object sender, EventArgs e)
        {
            usuario.Nombre1 = lblNombreUsuario.Text; //extraigo nombre usuario
            usuario.IdUsuario1 = gestus.DevolverIDusuario(usuario);//con el nombre obtengo el id
            venta.Usuario1 = usuario;//cargo el id en un objeto venta
            if (lbl_Tipotarjeta.Text == "Debito")
            {
                venta.Metodo_Pago1 = false;
            }
            else
            {
                venta.Metodo_Pago1 = true;
            }
            
            venta.Cod_Seguridad1 = Convert.ToInt32(lbl_Codtarjeta.Text);
            if (gestven.agregarVenta(venta))
            {
                MessageBox.Show("COMPRA REALIZADA CON EXITO!!!", "FELICITACIONES");
            }
            else
            {
                MessageBox.Show("HA OCURRIDO UN PROBLEMA, INTENTA NUEVAMENTE", "ATENCION!!!");
            }
            venta = gestdetven.Idultimaventa(venta);
            detventa.IdDetVenta1 = venta.IdVenta1;

            DataTable ComprasConfirmadas = (DataTable)Session["ArtSeleccionados"];
            foreach (DataRow row in ComprasConfirmadas.Rows)
            {
                detventa.IdUsuario1 = usuario.IdUsuario1;
                detventa.Precio1 = (float)Convert.ToDouble(row["PRECIO UNITARIO"]);
                detventa.Cantidad = Convert.ToInt32(row["CANTIDAD"]);
                detventa.DireccionEntrega1 = txtDireccionDeEntrega.Text;
                detventa.Contacto1 = txtContacto.Text;

                articulo.Descripcion1 = row["DESCRIPCION"].ToString();
                articulo = gestart.getarticuloXdescripcion(articulo);
                detventa.CodArticulo1 = articulo.CodArticulo1;
                gestdetven.agregarDetalleVenta(detventa);
            }
            Session["ArtSeleccionados"] = null;
            gvMiscompras.DataSource = null;
            gvMiscompras.DataBind();
            Server.Transfer("~/UsuarioForms/Catalogo.aspx");
        }

        protected void gvMiscompras_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            DataTable ArticuloAeliminar = (DataTable)gvMiscompras.DataSource;
            ArticuloAeliminar.Rows.RemoveAt(e.RowIndex);
            Session["ArtSeleccionados"] = ArticuloAeliminar;

            if (VerificacionSession())
            {
                gvMiscompras.DataSource = Session["ArtSeleccionados"];
                gvMiscompras.DataBind();
                CargarTotal(ArticuloAeliminar);
            }
        }

        protected void CargargvTarjetas()
        {
            usuario.Nombre1 = Session["NombreUsuario"].ToString();
            usuario.IdUsuario1 = gestus.DevolverIDusuario(usuario);
            gvTarjetas.DataSource = gestcard.getTablaTarjetasUsuario(usuario);
            gvTarjetas.DataBind();
        }

        protected void gvTarjetas_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            lbl_Tipotarjeta.Text = ((Label)gvTarjetas.Rows[e.NewSelectedIndex].FindControl("lbl_it_Tipotarjeta")).Text;
            lbl_Codtarjeta.Text = ((Label)gvTarjetas.Rows[e.NewSelectedIndex].FindControl("lbl_it_Codseguridad")).Text;
        }

        protected void btnGuardarTarjeta_Click(object sender, EventArgs e)
        {
            byte tipo_tarjeta = Convert.ToByte(rbDebitoCredito.SelectedValue);
            usuario.Nombre1 = lblNombreUsuario.Text;
            usuario.IdUsuario1 = gestus.DevolverIDusuario(usuario);
            Card.Us = usuario;
            Card.Num_Tarjeta1 = (int)Convert.ToInt64(txtNumeroTarjeta.Text);
            Card.Titular_Tarjeta1 = txtTarjetaTitular.Text;
            Card.Nombre_Tarjeta1 = txtNombreTarjeta.Text;
            Card.Fecha_Vencimiento1 = "01/" + txtTarjetames.Text + "/" + txtTarjetaAño.Text;
            Card.Tipo_Tarjeta1 =  Convert.ToBoolean(tipo_tarjeta);
            Card.Codigo_Seguridad1 = Convert.ToInt32(txtCodSeguridad.Text);
            if (gestcard.agregarTarjeta(Card))
            {
                MessageBox.Show("TARJETA AGREGADA", "ATENCION");
                gvTarjetas.DataSource = gestcard.getTablaTarjetasUsuario(usuario);
                gvTarjetas.DataBind();
                CargargvTarjetas();
                LimpiarCamposTarjeta();
            }
            else
            {
                MessageBox.Show("YA CONTAS CON ESTA TARJETA", "ATENCION");
            }          
            
        }

        private void LimpiarCamposTarjeta()
        {
            txtNumeroTarjeta.Text = "";
            txtTarjetaTitular.Text = "";
            txtTarjetames.Text = "";
            txtTarjetaAño.Text = "";
            txtCodSeguridad.Text = "";
            txtNombreTarjeta.Text = "";
        }
    }
}