<%@ Page Title="Mi cuenta · PC Booster Store" Language="C#" MasterPageFile="~/Tienda.Master" AutoEventWireup="true" CodeBehind="EdicionDatosUsuario.aspx.cs" Inherits="Vistas.EdicionDatosUsuario" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <h1 class="h3 mb-3">Mi cuenta</h1>

    <div class="row g-4">
        <div class="col-lg-6">
            <div class="card">
                <div class="card-body">
                    <h2 class="h6 mb-3"><i class="bi bi-person"></i> Mis datos</h2>
                    <asp:Panel runat="server" DefaultButton="btnGuardar" CssClass="row g-3">
                        <div class="col-sm-6">
                            <asp:Label runat="server" AssociatedControlID="txtNombre" CssClass="form-label" Text="Nombre de usuario" />
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="10" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre" ValidationGroup="Datos" Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá un nombre." />
                        </div>
                        <div class="col-sm-6">
                            <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="E-mail" />
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="30" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="Datos" Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá un e-mail." />
                        </div>
                        <div class="col-sm-6">
                            <asp:Label runat="server" AssociatedControlID="txtTelefono" CssClass="form-label" Text="Teléfono" />
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="15" />
                        </div>
                        <div class="col-sm-6">
                            <asp:Label runat="server" AssociatedControlID="txtContraseña" CssClass="form-label" Text="Nueva contraseña" />
                            <asp:TextBox ID="txtContraseña" runat="server" CssClass="form-control" TextMode="Password" MaxLength="64" autocomplete="new-password" />
                            <div class="form-text">Dejala vacía para mantener la actual.</div>
                        </div>
                        <div class="col-12">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar cambios" CssClass="btn btn-primary" ValidationGroup="Datos" OnClick="btnGuardar_Click" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="col-lg-6">
            <div class="card">
                <div class="card-body">
                    <h2 class="h6 mb-3"><i class="bi bi-credit-card"></i> Mis tarjetas</h2>
                    <asp:Label ID="lblSinTarjetas" runat="server" CssClass="text-body-secondary" Text="No tenés tarjetas guardadas. Podés agregar una desde el carrito." />
                    <asp:GridView ID="gvTarjetas" runat="server" AutoGenerateColumns="false" CssClass="table mb-0" GridLines="None"
                        DataKeyNames="Num_Tarjeta_t,Codigo_Seguridad_t,Titular_Tarjeta_t" OnRowDeleting="gvTarjetas_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="Tarjeta">
                                <ItemTemplate>
                                    <div class="fw-semibold"><%#: Eval("Nombre_Tarjeta_t") %> &bull;&bull;&bull;&bull; <%#: Eval("Num_Tarjeta_t") %></div>
                                    <div class="small text-body-secondary"><%#: Eval("Titular_Tarjeta_t") %></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Tipo_Tarjeta_t" HeaderText="Tipo" />
                            <asp:BoundField DataField="Fecha_Vencimiento_t" HeaderText="Vence" DataFormatString="{0:MM/yyyy}" />
                            <asp:TemplateField ItemStyle-CssClass="text-end">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="Eliminar tarjeta"
                                        OnClientClick="return confirm('¿Eliminar esta tarjeta?');"><i class="bi bi-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
