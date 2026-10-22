<%@ Page Title="Reportes · Administración" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="ReportesAdmin.aspx.cs" Inherits="Vistas.AdminForms.ReportesAdmin" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <h1 class="h3 mb-3">Reportes</h1>

    <div class="row g-4">
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

        <div class="col-xl-5">
            <div class="card h-100">
                <div class="card-body">
                    <h2 class="h5 mb-3"><i class="bi bi-trophy"></i> Más vendidos</h2>
                    <asp:Label ID="lblSinVentasTop" runat="server" CssClass="text-body-secondary" Text="Todavía no hay ventas." />
                    <asp:Repeater ID="rptTop" runat="server">
                        <ItemTemplate>
                            <div class="mb-3">
                                <div class="d-flex justify-content-between small mb-1">
                                    <span><%#: Eval("Descripcion") %></span>
                                    <span class="text-body-secondary text-nowrap"><%#: Eval("CantidadTotal") %> u.</span>
                                </div>
                                <div class="progress" role="progressbar" style="height: .5rem">
                                    <div class="progress-bar" style='width: <%# PorcentajeTop(Eval("CantidadTotal")) %>%'></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

        <div class="col-12">
            <div class="card">
                <div class="card-body">
                    <div class="d-flex flex-wrap gap-2 justify-content-between align-items-center mb-3">
                        <h2 class="h5 mb-0"><i class="bi bi-box-seam"></i> Artículos vendidos</h2>
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
