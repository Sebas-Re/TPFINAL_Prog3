<%@ Page Title="Panel · Administración" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="Vistas.AdminForms.AdminHome" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <h1 class="h3 mb-1">Hola, <asp:Label ID="lblNombreAdm" runat="server" /></h1>
    <p class="text-body-secondary mb-4">Resumen de la tienda</p>

    <div class="row row-cols-1 row-cols-sm-2 row-cols-xl-4 g-3 mb-4">
        <div class="col">
            <div class="card h-100 indicador"><div class="card-body d-flex align-items-center gap-3">
                <span class="icono rounded-3 d-flex align-items-center justify-content-center bg-primary-subtle text-primary"><i class="bi bi-box-seam"></i></span>
                <div><div class="text-body-secondary small">Artículos activos</div><asp:Label ID="lblArticulos" runat="server" CssClass="fs-4 fw-semibold" /></div>
            </div></div>
        </div>
        <div class="col">
            <div class="card h-100 indicador"><div class="card-body d-flex align-items-center gap-3">
                <span class="icono rounded-3 d-flex align-items-center justify-content-center bg-info-subtle text-info"><i class="bi bi-people"></i></span>
                <div><div class="text-body-secondary small">Clientes</div><asp:Label ID="lblClientes" runat="server" CssClass="fs-4 fw-semibold" /></div>
            </div></div>
        </div>
        <div class="col">
            <div class="card h-100 indicador"><div class="card-body d-flex align-items-center gap-3">
                <span class="icono rounded-3 d-flex align-items-center justify-content-center bg-warning-subtle text-warning"><i class="bi bi-receipt"></i></span>
                <div><div class="text-body-secondary small">Ventas</div><asp:Label ID="lblVentas" runat="server" CssClass="fs-4 fw-semibold" /></div>
            </div></div>
        </div>
        <div class="col">
            <div class="card h-100 indicador"><div class="card-body d-flex align-items-center gap-3">
                <span class="icono rounded-3 d-flex align-items-center justify-content-center bg-success-subtle text-success"><i class="bi bi-cash-stack"></i></span>
                <div><div class="text-body-secondary small">Recaudación</div><asp:Label ID="lblRecaudacion" runat="server" CssClass="fs-4 fw-semibold" /></div>
            </div></div>
        </div>
    </div>

    <div class="row row-cols-1 row-cols-md-3 g-3">
        <div class="col">
            <a class="card h-100 text-decoration-none" href="<%: ResolveUrl("~/AdminForms/AdminGestionArticulos.aspx") %>"><div class="card-body">
                <h2 class="h5"><i class="bi bi-box-seam text-primary"></i> Artículos y categorías</h2>
                <p class="text-body-secondary mb-0">Alta, edición y baja de productos y categorías.</p>
            </div></a>
        </div>
        <div class="col">
            <a class="card h-100 text-decoration-none" href="<%: ResolveUrl("~/AdminForms/AdminGestionUsuarios.aspx") %>"><div class="card-body">
                <h2 class="h5"><i class="bi bi-people text-primary"></i> Usuarios</h2>
                <p class="text-body-secondary mb-0">Clientes y administradores de la tienda.</p>
            </div></a>
        </div>
        <div class="col">
            <a class="card h-100 text-decoration-none" href="<%: ResolveUrl("~/AdminForms/ReportesAdmin.aspx") %>"><div class="card-body">
                <h2 class="h5"><i class="bi bi-graph-up text-primary"></i> Reportes</h2>
                <p class="text-body-secondary mb-0">Ventas y artículos más vendidos.</p>
            </div></a>
        </div>
    </div>
</asp:Content>
