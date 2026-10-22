<%@ Page Title="Panel · Administración" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="Vistas.AdminForms.AdminHome" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <div class="d-flex flex-wrap justify-content-between align-items-end gap-2 mb-4">
        <div>
            <h1 class="h3 mb-1">Hola, <asp:Label ID="lblNombreAdm" runat="server" /></h1>
            <p class="text-body-secondary mb-0">Así viene la tienda en los últimos 30 días</p>
        </div>
        <a class="btn btn-outline-primary" href="<%: ResolveUrl("~/AdminForms/ReportesAdmin.aspx") %>"><i class="bi bi-graph-up"></i> Ver reportes</a>
    </div>

    <div class="row row-cols-1 row-cols-sm-2 row-cols-xl-4 g-3 mb-4">
        <asp:Literal ID="litIndicadores" runat="server" />
    </div>

    <div class="row g-4 mb-4">
        <div class="col-lg-8">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h6 mb-3">Ingresos y pedidos por mes</h2>
                    <div class="grafico"><canvas aria-label="Ingresos y pedidos por mes" role="img" data-grafico="<%: GraficoMeses %>"></canvas></div>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h6 mb-3">Ingresos por categoría <span class="text-body-secondary fw-normal">(6 meses)</span></h2>
                    <div class="grafico"><canvas aria-label="Ingresos por categoría" role="img" data-grafico="<%: GraficoCategorias %>"></canvas></div>
                </div>
            </div>
        </div>
    </div>

    <div class="row g-4">
        <div class="col-lg-7">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h6 mb-3">Clientes nuevos por mes</h2>
                    <div class="grafico grafico-chico"><canvas aria-label="Clientes nuevos por mes" role="img" data-grafico="<%: GraficoClientes %>"></canvas></div>
                </div>
            </div>
        </div>
        <div class="col-lg-5">
            <div class="card h-100">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h2 class="h6 mb-0">Stock bajo <span class="text-body-secondary fw-normal">(menos de <%: UmbralStock %> u.)</span></h2>
                        <a class="small" href="<%: ResolveUrl("~/AdminForms/AdminGestionArticulos.aspx") %>">Gestionar</a>
                    </div>
                    <asp:Label ID="lblSinStockBajo" runat="server" CssClass="text-body-secondary" Text="Todos los artículos activos tienen stock suficiente." />
                    <ul class="list-group list-group-flush">
                        <asp:Repeater ID="rptStockBajo" runat="server">
                            <ItemTemplate>
                                <li class="list-group-item d-flex justify-content-between align-items-center px-0">
                                    <span><span class="text-body-secondary small me-2"><%#: Eval("Codigo") %></span><%#: Eval("Descripcion") %></span>
                                    <span class='<%# Convert.ToInt64(Eval("Stock")) == 0 ? "badge text-bg-danger" : "badge text-bg-warning" %>'><%#: Eval("Stock") %> u.</span>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
