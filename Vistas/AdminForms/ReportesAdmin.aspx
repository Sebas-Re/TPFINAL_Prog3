<%@ Page Title="Reportes · Administración" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ReportesAdmin.aspx.cs" Inherits="Vistas.AdminForms.ReportesAdmin" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <div class="d-flex flex-wrap justify-content-between align-items-center gap-2 mb-4">
        <h1 class="h3 mb-0">Reportes</h1>
        <ul class="nav nav-pills">
            <asp:Repeater ID="rptPeriodos" runat="server">
                <ItemTemplate>
                    <li class="nav-item"><a class='<%# (int)Container.DataItem == Dias ? "nav-link active" : "nav-link" %>'
                        href='<%# ResolveUrl("~/AdminForms/ReportesAdmin.aspx?dias=" + Container.DataItem) %>'><%# Container.DataItem %> días</a></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>

    <div class="row row-cols-1 row-cols-sm-2 row-cols-xl-4 g-3 mb-4">
        <asp:Literal ID="litIndicadores" runat="server" />
    </div>

    <div class="card mb-4">
        <div class="card-body">
            <h2 class="h6 mb-3">Ingresos y pedidos por día <span class="text-body-secondary fw-normal">(últimos <%: Dias %> días)</span></h2>
            <div class="grafico"><canvas aria-label="Ingresos por día" role="img" data-grafico="<%: GraficoDias %>"></canvas></div>
        </div>
    </div>

    <div class="row g-4 mb-4">
        <div class="col-lg-7">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h6 mb-3">Productos más vendidos <span class="text-body-secondary fw-normal">(unidades)</span></h2>
                    <div class="grafico"><canvas aria-label="Productos más vendidos" role="img" data-grafico="<%: GraficoProductos %>"></canvas></div>
                </div>
            </div>
        </div>
        <div class="col-lg-5">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h6 mb-3">Medios de pago <span class="text-body-secondary fw-normal">(pedidos)</span></h2>
                    <div class="grafico grafico-chico"><canvas aria-label="Medios de pago" role="img" data-grafico="<%: GraficoPagos %>"></canvas></div>
                    <p class="small text-body-secondary mt-3 mb-0">
                        Tasa de recompra: <strong><asp:Label ID="lblRecompra" runat="server" /></strong>
                        de los clientes que compraron en el período lo hicieron más de una vez.
                    </p>
                </div>
            </div>
        </div>
    </div>

    <div class="row g-4">
        <div class="col-xl-5">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5 mb-3"><i class="bi bi-trophy"></i> Mejores clientes</h2>
                    <asp:GridView ID="gvTopClientes" runat="server" AutoGenerateColumns="false" CssClass="table table-hover mb-0" GridLines="None"
                        EmptyDataText="No hubo compras en el período.">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Cliente" />
                            <asp:BoundField DataField="Pedidos" HeaderText="Pedidos" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                            <asp:BoundField DataField="Gasto" HeaderText="Gasto" DataFormatString="{0:C0}" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <div class="col-xl-7">
            <div class="card h-100">
                <div class="card-body">
                    <div class="d-flex flex-wrap gap-2 justify-content-between align-items-center mb-3">
                        <h2 class="h5 mb-0"><i class="bi bi-receipt"></i> Ventas</h2>
                        <div class="d-flex gap-2">
                            <asp:Panel runat="server" DefaultButton="btnBuscarUsuario" CssClass="input-group" style="width: 220px">
                                <asp:TextBox ID="txtIdUsuarioBuscar" runat="server" CssClass="form-control" placeholder="ID de usuario" inputmode="numeric" />
                                <asp:LinkButton ID="btnBuscarUsuario" runat="server" CssClass="btn btn-outline-primary" OnClick="btnBuscarUsuario_Click" ValidationGroup="Ventas" ToolTip="Filtrar por usuario"><i class="bi bi-funnel"></i></asp:LinkButton>
                            </asp:Panel>
                            <asp:Button ID="btnTodasLasVentas" runat="server" Text="Todas" CssClass="btn btn-outline-secondary" OnClick="btnTodasLasVentas_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtIdUsuarioBuscar" ValidationGroup="Ventas" ValidationExpression="^\d+$" Display="Dynamic" CssClass="validador text-end mb-2" ErrorMessage="El ID es un número." />

                    <div class="table-responsive">
                        <asp:GridView ID="gvVentas" runat="server" AutoGenerateColumns="false" DataKeyNames="IdVentas_ve"
                            CssClass="table table-hover mb-0" GridLines="None" AllowPaging="true" PageSize="8" PagerStyle-CssClass="pager-gv"
                            SelectedRowStyle-CssClass="table-primary" EmptyDataText="No hay ventas para mostrar."
                            OnSelectedIndexChanging="gvVentas_SelectedIndexChanging" OnPageIndexChanging="gvVentas_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="IdVentas_ve" HeaderText="N°" />
                                <asp:BoundField DataField="IdUsuario_ve" HeaderText="Usuario" />
                                <asp:BoundField DataField="FechaVenta_ve" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="TotalVenta_ve" HeaderText="Total" DataFormatString="{0:C0}" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                                <asp:TemplateField ItemStyle-CssClass="text-end">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="Select" CssClass="btn btn-sm btn-outline-primary" CausesValidation="false">Detalle</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <asp:Panel ID="pnlDetalle" runat="server" Visible="false" CssClass="mt-4">
                        <asp:Label ID="lblTituloDetalle" runat="server" CssClass="h6 d-block" />
                        <asp:GridView ID="gvDetallesVenta" runat="server" AutoGenerateColumns="false" CssClass="table table-sm mb-0" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="CodArticulo_dv" HeaderText="Código" />
                                <asp:BoundField DataField="Descripcion_ar" HeaderText="Producto" />
                                <asp:BoundField DataField="Cantidad_dv" HeaderText="Cant." ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                <asp:BoundField DataField="Precio_Unitario_dv" HeaderText="Precio" DataFormatString="{0:C0}" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                                <asp:BoundField DataField="Direccion_de_Entrega" HeaderText="Entrega" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="col-12">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex flex-wrap gap-2 justify-content-between align-items-center mb-3">
                        <h2 class="h5 mb-0"><i class="bi bi-box-seam"></i> Artículos vendidos <span class="text-body-secondary fw-normal fs-6">(histórico)</span></h2>
                        <div class="d-flex flex-wrap gap-2">
                            <asp:Panel runat="server" DefaultButton="btnEncontrarArt" CssClass="input-group" style="width: 240px">
                                <asp:TextBox ID="txtCodArticulo" runat="server" CssClass="form-control" placeholder="Código de artículo" MaxLength="4" />
                                <asp:LinkButton ID="btnEncontrarArt" runat="server" CssClass="btn btn-outline-primary" OnClick="btnEncontrarArt_Click" CausesValidation="false" ToolTip="Filtrar"><i class="bi bi-search"></i></asp:LinkButton>
                            </asp:Panel>
                            <asp:DropDownList ID="ddlOrdenador" runat="server" CssClass="form-select" style="width: auto" AutoPostBack="true" OnSelectedIndexChanged="ddlOrdenador_SelectedIndexChanged">
                                <asp:ListItem Value="Codigo ASC">Ordenar por código</asp:ListItem>
                                <asp:ListItem Value="CantidadTotal DESC">Ordenar por unidades</asp:ListItem>
                                <asp:ListItem Value="totalventa DESC">Ordenar por recaudación</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvReporteArticulos" runat="server" AutoGenerateColumns="false" CssClass="table table-hover mb-0" GridLines="None"
                            AllowPaging="true" PageSize="10" PagerStyle-CssClass="pager-gv" OnPageIndexChanging="gvReporteArticulos_PageIndexChanging"
                            EmptyDataText="No hay ventas para ese criterio.">
                            <Columns>
                                <asp:BoundField DataField="Codigo" HeaderText="Código" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Producto" />
                                <asp:BoundField DataField="CantidadTotal" HeaderText="Unidades" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                                <asp:BoundField DataField="totalventa" HeaderText="Recaudación" DataFormatString="{0:C0}" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
