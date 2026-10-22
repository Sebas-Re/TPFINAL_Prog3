<%@ Page Title="Carrito · PC Booster Store" Language="C#" MasterPageFile="~/Tienda.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="Vistas.Carrito" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <h1 class="h3 mb-3">Tu carrito</h1>

    <asp:Panel ID="pnlCarritoVacio" runat="server" CssClass="card text-center py-5">
        <div class="card-body">
            <i class="bi bi-cart-x display-6 text-body-tertiary"></i>
            <p class="mt-3 mb-3">Tu carrito está vacío.</p>
            <a class="btn btn-primary" href="<%: ResolveUrl("~/UsuarioForms/Catalogo.aspx") %>">Ir al catálogo</a>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlCompra" runat="server" CssClass="row g-4">
        <div class="col-lg-7">
            <div class="card mb-4">
                <div class="card-body">
                    <asp:GridView ID="gvMiscompras" runat="server" AutoGenerateColumns="false" DataKeyNames="CodArticulo"
                        CssClass="table mb-0" GridLines="None" OnRowDeleting="gvMiscompras_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="Descripcion" HeaderText="Producto" />
                            <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio" DataFormatString="{0:C0}" ItemStyle-CssClass="text-nowrap" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cant." ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C0}" ItemStyle-CssClass="text-end text-nowrap" HeaderStyle-CssClass="text-end" />
                            <asp:TemplateField ItemStyle-CssClass="text-end">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="Quitar del carrito">
                                        <i class="bi bi-trash"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="card-footer bg-white d-flex justify-content-between align-items-center py-3">
                    <div class="d-flex gap-2">
                        <a class="btn btn-outline-secondary" href="<%: ResolveUrl("~/UsuarioForms/Catalogo.aspx") %>"><i class="bi bi-arrow-left"></i> Seguir comprando</a>
                        <asp:Button ID="btnCancelarCompra" runat="server" Text="Vaciar carrito" CssClass="btn btn-outline-danger"
                            OnClick="btnCancelarCompra_Click" OnClientClick="return confirm('¿Vaciar el carrito?');" CausesValidation="false" />
                    </div>
                    <div class="fs-5">Total: <asp:Label ID="lblTotalCompra" runat="server" CssClass="fw-bold" /></div>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
                    <h2 class="h6 mb-3"><i class="bi bi-credit-card"></i> Agregar una tarjeta</h2>
                    <asp:Panel runat="server" DefaultButton="btnGuardarTarjeta" CssClass="row g-3">
                        <div class="col-sm-6">
                            <asp:Label runat="server" AssociatedControlID="txtNumeroTarjeta" CssClass="form-label" Text="Número de tarjeta" />
                            <asp:TextBox ID="txtNumeroTarjeta" runat="server" CssClass="form-control" MaxLength="16" inputmode="numeric" autocomplete="cc-number" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNumeroTarjeta" ValidationGroup="Tarjeta" Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá el número." />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtNumeroTarjeta" ValidationGroup="Tarjeta" ValidationExpression="^\d{4,16}$" Display="Dynamic" CssClass="validador" ErrorMessage="Solo números (4 a 16 dígitos)." />
                        </div>
                        <div class="col-sm-6">
                            <asp:Label runat="server" AssociatedControlID="txtTarjetaTitular" CssClass="form-label" Text="Titular" />
                            <asp:TextBox ID="txtTarjetaTitular" runat="server" CssClass="form-control" MaxLength="50" autocomplete="cc-name" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTarjetaTitular" ValidationGroup="Tarjeta" Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá el titular." />
                        </div>
                        <div class="col-sm-6">
                            <asp:Label runat="server" AssociatedControlID="txtNombreTarjeta" CssClass="form-label" Text="Marca (Visa, Mastercard...)" />
                            <asp:TextBox ID="txtNombreTarjeta" runat="server" CssClass="form-control" MaxLength="20" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombreTarjeta" ValidationGroup="Tarjeta" Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá la marca." />
                        </div>
                        <div class="col-sm-6">
                            <span class="form-label d-block">Tipo</span>
                            <asp:RadioButtonList ID="rbDebitoCredito" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="d-flex gap-3 pt-1">
                                <asp:ListItem Value="0" Selected="True">Débito</asp:ListItem>
                                <asp:ListItem Value="1">Crédito</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-4">
                            <asp:Label runat="server" AssociatedControlID="txtTarjetames" CssClass="form-label" Text="Mes venc." />
                            <asp:TextBox ID="txtTarjetames" runat="server" CssClass="form-control" MaxLength="2" placeholder="MM" inputmode="numeric" />
                            <asp:RangeValidator runat="server" ControlToValidate="txtTarjetames" ValidationGroup="Tarjeta" Type="Integer" MinimumValue="1" MaximumValue="12" Display="Dynamic" CssClass="validador" ErrorMessage="Mes inválido." />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTarjetames" ValidationGroup="Tarjeta" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-4">
                            <asp:Label runat="server" AssociatedControlID="txtTarjetaAño" CssClass="form-label" Text="Año venc." />
                            <asp:TextBox ID="txtTarjetaAño" runat="server" CssClass="form-control" MaxLength="4" placeholder="AAAA" inputmode="numeric" />
                            <asp:RangeValidator runat="server" ControlToValidate="txtTarjetaAño" ValidationGroup="Tarjeta" Type="Integer" MinimumValue="2000" MaximumValue="2099" Display="Dynamic" CssClass="validador" ErrorMessage="Año inválido." />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTarjetaAño" ValidationGroup="Tarjeta" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-4">
                            <asp:Label runat="server" AssociatedControlID="txtCodSeguridad" CssClass="form-label" Text="Código" />
                            <asp:TextBox ID="txtCodSeguridad" runat="server" CssClass="form-control" MaxLength="4" placeholder="CVV" inputmode="numeric" autocomplete="cc-csc" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCodSeguridad" ValidationGroup="Tarjeta" ValidationExpression="^\d{3,4}$" Display="Dynamic" CssClass="validador" ErrorMessage="3 o 4 dígitos." />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCodSeguridad" ValidationGroup="Tarjeta" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-12">
                            <asp:Button ID="btnGuardarTarjeta" runat="server" Text="Guardar tarjeta" CssClass="btn btn-outline-primary" ValidationGroup="Tarjeta" OnClick="btnGuardarTarjeta_Click" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="col-lg-5">
            <div class="card">
                <div class="card-body">
                    <h2 class="h6 mb-3"><i class="bi bi-bag-check"></i> Finalizar compra</h2>
                    <asp:Panel runat="server" DefaultButton="btnConfirmarCompra">
                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="ddlTarjetas" CssClass="form-label" Text="Medio de pago" />
                            <asp:DropDownList ID="ddlTarjetas" runat="server" CssClass="form-select" />
                            <asp:Label ID="lblSinTarjetas" runat="server" CssClass="form-text d-block" Text="Todavía no tenés tarjetas: agregá una en el formulario de la izquierda." />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlTarjetas" InitialValue="" ValidationGroup="Compra" Display="Dynamic" CssClass="validador" ErrorMessage="Elegí una tarjeta." />
                        </div>
                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtDireccionDeEntrega" CssClass="form-label" Text="Dirección de entrega" />
                            <asp:TextBox ID="txtDireccionDeEntrega" runat="server" CssClass="form-control" MaxLength="50" autocomplete="street-address" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDireccionDeEntrega" ValidationGroup="Compra" Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá la dirección de entrega." />
                        </div>
                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtContacto" CssClass="form-label" Text="¿Quién recibe?" />
                            <asp:TextBox ID="txtContacto" runat="server" CssClass="form-control" MaxLength="20" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContacto" ValidationGroup="Compra" Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá quién recibe el pedido." />
                        </div>
                        <asp:Button ID="btnConfirmarCompra" runat="server" Text="Confirmar compra" CssClass="btn btn-success w-100 btn-lg" ValidationGroup="Compra" OnClick="btnConfirmarCompra_Click" />
                    </asp:Panel>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
