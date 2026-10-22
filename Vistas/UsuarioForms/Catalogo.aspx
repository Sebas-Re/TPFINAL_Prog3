<%@ Page Title="Catálogo · PC Booster Store" Language="C#" MasterPageFile="~/Tienda.Master" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="Vistas.Catalogo" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <div class="d-flex flex-wrap align-items-center justify-content-between gap-3 mb-3">
        <div>
            <h1 class="h3 mb-0">Catálogo</h1>
            <p class="text-body-secondary mb-0">Elegí las piezas para potenciar tu PC</p>
        </div>
        <asp:Panel runat="server" DefaultButton="btnBuscar" CssClass="input-group buscador" style="max-width: 420px">
            <asp:TextBox ID="txtArticuloBuscar" runat="server" CssClass="form-control" placeholder="Buscar productos..." aria-label="Buscar productos" />
            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click" ToolTip="Buscar"><i class="bi bi-search"></i></asp:LinkButton>
        </asp:Panel>
    </div>

    <ul class="nav nav-pills flex-wrap gap-1 mb-4">
        <li class="nav-item"><a class="<%: ClaseCategoria(null) %>" href="<%: ResolveUrl("~/UsuarioForms/Catalogo.aspx") %>">Todas</a></li>
        <asp:Repeater ID="rptCategorias" runat="server">
            <ItemTemplate>
                <li class="nav-item">
                    <a class='<%# ClaseCategoria(Eval("IdCategoria_ca")) %>' href='<%# UrlCategoria(Eval("IdCategoria_ca")) %>'><%#: Eval("Descripcion_ca") %></a>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>

    <div class="row g-4">
        <div class="col-lg-9">
            <asp:Label ID="lblResultado" runat="server" CssClass="d-block text-body-secondary small mb-2" />

            <asp:Panel ID="pnlSinResultados" runat="server" Visible="false" CssClass="card text-center py-5">
                <div class="card-body">
                    <i class="bi bi-search display-6 text-body-tertiary"></i>
                    <p class="mt-3 mb-3">No encontramos productos con ese criterio.</p>
                    <a class="btn btn-outline-primary" href="<%: ResolveUrl("~/UsuarioForms/Catalogo.aspx") %>">Ver todo el catálogo</a>
                </div>
            </asp:Panel>

            <div class="row row-cols-1 row-cols-sm-2 row-cols-xl-3 g-3">
                <asp:Repeater ID="rptArticulos" runat="server" OnItemCommand="rptArticulos_ItemCommand">
                    <ItemTemplate>
                        <div class="col">
                            <div class="card h-100 producto">
                                <a href="#" class="text-reset text-decoration-none" data-bs-toggle="modal" data-bs-target='#<%# IdHtml("detalle", Eval("CodArticulo_ar")) %>'>
                                    <img class="card-img-top imagen" src='<%#: ResolveUrl(Eval("Imagen").ToString()) %>' alt='<%#: Eval("Descripcion_ar") %>' loading="lazy" onerror="this.onerror=null;this.src='<%# UrlSinImagen %>'" />
                                </a>
                                <div class="card-body d-flex flex-column">
                                    <div class="small text-body-secondary mb-1"><%#: Eval("Marca_ar") %></div>
                                    <h2 class="h6 card-title descripcion">
                                        <a href="#" class="text-reset text-decoration-none" data-bs-toggle="modal" data-bs-target='#<%# IdHtml("detalle", Eval("CodArticulo_ar")) %>'><%#: Eval("Descripcion_ar") %></a>
                                    </h2>
                                    <div class="d-flex align-items-baseline justify-content-between mt-auto">
                                        <span class="fs-5 fw-semibold"><%#: Eval("PrecioUnitario_ar", "{0:C0}") %></span>
                                        <span class='<%# Convert.ToInt64(Eval("Stock_ar")) > 0 ? "badge text-bg-light" : "badge text-bg-danger" %>'>
                                            <%# Convert.ToInt64(Eval("Stock_ar")) > 0 ? "Stock: " + Eval("Stock_ar") : "Sin stock" %>
                                        </span>
                                    </div>
                                </div>
                                <div class="card-footer bg-white border-0 pt-0 pb-3 d-flex gap-2">
                                    <button type="button" class="btn btn-outline-secondary" data-bs-toggle="modal" data-bs-target='#<%# IdHtml("detalle", Eval("CodArticulo_ar")) %>' title="Ver detalle">
                                        <i class="bi bi-info-circle"></i>
                                    </button>
                                    <asp:LinkButton runat="server" CommandName="Agregar" CommandArgument='<%# Eval("CodArticulo_ar") %>' ToolTip="Agregar al carrito"
                                        CssClass="btn btn-primary flex-grow-1" Enabled='<%# Convert.ToInt64(Eval("Stock_ar")) > 0 %>'>
                                        <i class="bi bi-cart-plus"></i> Agregar al carrito
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <%-- Detalle del producto: un modal por tarjeta, sin JavaScript propio. --%>
                        <div class="modal fade" id='<%# IdHtml("detalle", Eval("CodArticulo_ar")) %>' tabindex="-1" aria-labelledby='<%# IdHtml("titulo", Eval("CodArticulo_ar")) %>' aria-hidden="true">
                            <div class="modal-dialog modal-lg modal-dialog-centered modal-dialog-scrollable">
                                <div class="modal-content">
                                    <div class="modal-header border-0 pb-0">
                                        <span class="badge text-bg-light"><%#: Eval("Categoria") %></span>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                    </div>
                                    <div class="modal-body pt-2">
                                        <div class="row g-4">
                                            <div class="col-md-5">
                                                <div class="border rounded-3 p-3 bg-white text-center">
                                                    <img class="img-fluid detalle-imagen" src='<%#: ResolveUrl(Eval("Imagen").ToString()) %>' alt='<%#: Eval("Descripcion_ar") %>' loading="lazy" onerror="this.onerror=null;this.src='<%# UrlSinImagen %>'" />
                                                </div>
                                                <ul class="list-unstyled small text-body-secondary mt-3 mb-0">
                                                    <li class="mb-1"><i class="bi bi-shield-check text-success"></i> Garantía oficial: <%#: TextoGarantia(Eval("Garantia_ar")) %></li>
                                                    <li class="mb-1"><i class="bi bi-truck text-primary"></i> Envío a todo el país</li>
                                                    <li><i class="bi bi-arrow-repeat text-primary"></i> 30 días para cambios</li>
                                                </ul>
                                            </div>
                                            <div class="col-md-7">
                                                <div class="text-body-secondary small"><%#: Eval("Marca_ar") %> &middot; Código <%#: Eval("CodArticulo_ar") %></div>
                                                <h3 class="h4 mb-2" id='<%# IdHtml("titulo", Eval("CodArticulo_ar")) %>'><%#: Eval("Descripcion_ar") %></h3>
                                                <div class="d-flex align-items-center gap-3 mb-3">
                                                    <span class="fs-3 fw-semibold"><%#: Eval("PrecioUnitario_ar", "{0:C0}") %></span>
                                                    <span class='<%# Convert.ToInt64(Eval("Stock_ar")) > 0 ? "badge text-bg-success" : "badge text-bg-danger" %>'>
                                                        <%# Convert.ToInt64(Eval("Stock_ar")) > 0 ? Eval("Stock_ar") + " disponibles" : "Sin stock" %>
                                                    </span>
                                                </div>
                                                <p class="mb-3"><%#: Eval("DescripcionLarga_ar") %></p>

                                                <h4 class="h6">Especificaciones</h4>
                                                <table class="table table-sm small mb-3">
                                                    <tbody>
                                                        <asp:Repeater runat="server" DataSource='<%# Especificaciones(Eval("Especificaciones_ar")) %>'>
                                                            <ItemTemplate>
                                                                <tr><th scope="row" class="text-body-secondary fw-normal w-50"><%#: Eval("Key") %></th><td><%#: Eval("Value") %></td></tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>

                                                <asp:Panel runat="server" CssClass="d-flex gap-2" Visible='<%# Convert.ToInt64(Eval("Stock_ar")) > 0 %>'>
                                                    <asp:TextBox ID="txtCantidad" runat="server" TextMode="Number" Text="1" CssClass="form-control" style="width: 6rem"
                                                        min="1" max='<%# Eval("Stock_ar") %>' aria-label="Cantidad" />
                                                    <asp:LinkButton runat="server" CommandName="AgregarCantidad" CommandArgument='<%# Eval("CodArticulo_ar") %>'
                                                        CssClass="btn btn-primary flex-grow-1" ToolTip="Agregar la cantidad elegida">
                                                        <i class="bi bi-cart-plus"></i> Agregar al carrito
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <nav class="mt-4" aria-label="Páginas del catálogo">
                <asp:Literal ID="litPaginacion" runat="server" />
            </nav>
        </div>

        <div class="col-lg-3">
            <div class="card sticky-lg-top resumen-carrito">
                <div class="card-body">
                    <h2 class="h6 mb-3"><i class="bi bi-cart3"></i> Tu carrito</h2>
                    <asp:Panel ID="pnlCarritoVacio" runat="server" CssClass="text-body-secondary small">
                        Todavía no agregaste productos.
                    </asp:Panel>
                    <asp:Panel ID="pnlCarrito" runat="server">
                        <ul class="list-unstyled small mb-3">
                            <asp:Repeater ID="rptCarrito" runat="server">
                                <ItemTemplate>
                                    <li class="d-flex justify-content-between gap-2 py-1 border-bottom">
                                        <span><%#: Eval("Cantidad") %> &times; <%#: Eval("Descripcion") %></span>
                                        <span class="text-nowrap"><%#: Eval("Subtotal", "{0:C0}") %></span>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="d-flex justify-content-between fw-semibold mb-3">
                            <span>Total</span>
                            <asp:Label ID="lblTotalCompra" runat="server" />
                        </div>
                        <a class="btn btn-success w-100" href="<%: ResolveUrl("~/UsuarioForms/Carrito.aspx") %>">Ir a pagar</a>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
