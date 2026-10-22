<%@ Page Title="Mis compras · PC Booster Store" Language="C#" MasterPageFile="~/Tienda.Master" AutoEventWireup="true" CodeBehind="HistorialCompras.aspx.cs" Inherits="Vistas.HistorialCompras" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <h1 class="h3 mb-3">Mis compras</h1>

    <asp:Panel ID="pnlSinCompras" runat="server" CssClass="card text-center py-5">
        <div class="card-body">
            <i class="bi bi-bag display-6 text-body-tertiary"></i>
            <p class="mt-3 mb-3">Todavía no realizaste ninguna compra.</p>
            <a class="btn btn-primary" href="<%: ResolveUrl("~/UsuarioForms/Catalogo.aspx") %>">Ir al catálogo</a>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlCompras" runat="server" CssClass="row g-4">
        <div class="col-lg-5">
            <div class="card">
                <div class="card-body">
                    <asp:GridView ID="gvUsuarioCompras" runat="server" AutoGenerateColumns="false" DataKeyNames="IdVentas_ve"
                        CssClass="table table-hover mb-0" GridLines="None" OnSelectedIndexChanging="gvUsuarioCompras_SelectedIndexChanging"
                        SelectedRowStyle-CssClass="table-primary">
                        <Columns>
                            <asp:BoundField DataField="IdVentas_ve" HeaderText="N°" />
                            <asp:BoundField DataField="FechaVenta_ve" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="TotalVenta_ve" HeaderText="Total" DataFormatString="{0:C0}" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                            <asp:TemplateField ItemStyle-CssClass="text-end">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="Select" CssClass="btn btn-sm btn-outline-primary">Ver detalle</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-lg-7">
            <div class="card">
                <div class="card-body">
                    <asp:Label ID="lblTituloDetalle" runat="server" CssClass="h6 d-block mb-3" Text="Detalle de la compra" />
                    <asp:Label ID="lblElegirCompra" runat="server" CssClass="text-body-secondary" Text="Elegí una compra para ver qué productos incluía." />
                    <asp:GridView ID="gvDetalleCompras" runat="server" AutoGenerateColumns="false" CssClass="table mb-0" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="Descripcion_ar" HeaderText="Producto" />
                            <asp:BoundField DataField="Cantidad_dv" HeaderText="Cant." ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                            <asp:BoundField DataField="Precio_Unitario_dv" HeaderText="Precio" DataFormatString="{0:C0}" ItemStyle-CssClass="text-end text-nowrap" HeaderStyle-CssClass="text-end" />
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblEntrega" runat="server" CssClass="d-block small text-body-secondary mt-3" />
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
