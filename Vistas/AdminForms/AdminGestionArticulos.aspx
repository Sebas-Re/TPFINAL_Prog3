<%@ Page Title="Artículos y categorías · Administración" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminGestionArticulos.aspx.cs" Inherits="Vistas.AdminForms.AdminGestionArticulos" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <h1 class="h3 mb-3">Artículos y categorías</h1>

    <div class="card mb-4">
        <div class="card-body">
            <div class="d-flex flex-wrap gap-2 justify-content-between align-items-center mb-3">
                <h2 class="h5 mb-0"><i class="bi bi-box-seam"></i> Artículos</h2>
                <div class="d-flex flex-wrap gap-2">
                    <asp:Panel runat="server" DefaultButton="btnBuscarporCodigo" CssClass="input-group" style="width: 260px">
                        <asp:TextBox ID="txtIdArticulo" runat="server" CssClass="form-control" placeholder="Código (ej: gb1)" MaxLength="4" />
                        <asp:LinkButton ID="btnBuscarporCodigo" runat="server" CssClass="btn btn-outline-primary" OnClick="btnBuscarporCodigo_Click" ToolTip="Buscar por código"><i class="bi bi-search"></i></asp:LinkButton>
                    </asp:Panel>
                    <asp:Button ID="btnCargarArticulos" runat="server" Text="Ver todos" CssClass="btn btn-outline-secondary" OnClick="btnCargarArticulos_Click" CausesValidation="false" />
                    <button class="btn btn-primary" type="button" data-bs-toggle="collapse" data-bs-target="#nuevoArticulo" aria-expanded="false" aria-controls="nuevoArticulo">
                        <i class="bi bi-plus-lg"></i> Nuevo artículo
                    </button>
                </div>
            </div>

            <div class="collapse" id="nuevoArticulo">
                <asp:Panel runat="server" DefaultButton="btnAgregarArticulo" CssClass="border rounded-3 p-3 mb-3 bg-body-tertiary">
                    <div class="row g-3">
                        <div class="col-sm-3 col-lg-2">
                            <asp:Label runat="server" AssociatedControlID="txtCodArticulo" CssClass="form-label" Text="Código" />
                            <asp:TextBox ID="txtCodArticulo" runat="server" CssClass="form-control" MaxLength="4" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCodArticulo" ValidationGroup="NuevoArticulo" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-sm-9 col-lg-4">
                            <asp:Label runat="server" AssociatedControlID="txtDescripcion" CssClass="form-label" Text="Descripción" />
                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" MaxLength="300" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescripcion" ValidationGroup="NuevoArticulo" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-sm-6 col-lg-3">
                            <asp:Label runat="server" AssociatedControlID="ddlCategoria" CssClass="form-label" Text="Categoría" />
                            <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" DataTextField="Descripcion_ca" DataValueField="IdCategoria_ca" />
                        </div>
                        <div class="col-6 col-sm-3 col-lg-1">
                            <asp:Label runat="server" AssociatedControlID="txtStock" CssClass="form-label" Text="Stock" />
                            <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" inputmode="numeric" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStock" ValidationGroup="NuevoArticulo" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtStock" ValidationGroup="NuevoArticulo" ValidationExpression="^\d+$" Display="Dynamic" CssClass="validador" ErrorMessage="Solo números." />
                        </div>
                        <div class="col-6 col-sm-3 col-lg-2">
                            <asp:Label runat="server" AssociatedControlID="txtPrecio_Unitario" CssClass="form-label" Text="Precio ($)" />
                            <asp:TextBox ID="txtPrecio_Unitario" runat="server" CssClass="form-control" inputmode="numeric" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrecio_Unitario" ValidationGroup="NuevoArticulo" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPrecio_Unitario" ValidationGroup="NuevoArticulo" ValidationExpression="^[1-9]\d*$" Display="Dynamic" CssClass="validador" ErrorMessage="Número mayor a 0." />
                        </div>
                        <div class="col-lg-9">
                            <asp:Label runat="server" AssociatedControlID="txtImagen" CssClass="form-label" Text="URL de la imagen" />
                            <asp:TextBox ID="txtImagen" runat="server" CssClass="form-control" placeholder="~/Imagenes_ar/Gabinete/gb1.png" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtImagen" ValidationGroup="NuevoArticulo" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-lg-3 d-flex align-items-end">
                            <asp:Button ID="btnAgregarArticulo" runat="server" Text="Agregar artículo" CssClass="btn btn-primary w-100" ValidationGroup="NuevoArticulo" OnClick="btnAgregarArticulo_Click" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gvArticulos" runat="server" AutoGenerateColumns="false" DataKeyNames="CodArticulo_ar"
                    CssClass="table table-hover mb-0" GridLines="None" AllowPaging="true" PageSize="10" PagerStyle-CssClass="pager-gv"
                    OnRowEditing="gvArticulos_RowEditing" OnRowCancelingEdit="gvArticulos_RowCancelingEdit" OnRowUpdating="gvArticulos_RowUpdating"
                    OnRowDeleting="gvArticulos_RowDeleting" OnPageIndexChanging="gvArticulos_PageIndexChanging" OnRowDataBound="gvArticulos_RowDataBound"
                    EmptyDataText="No hay artículos para mostrar.">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <img src='<%# ResolveUrl(Eval("Imagen").ToString()) %>' alt="" width="48" height="48" style="object-fit: contain" loading="lazy" onerror="this.onerror=null;this.src='<%# ResolveUrl("~/Content/sin-imagen.svg") %>'" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CodArticulo_ar" HeaderText="Código" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Descripción">
                            <ItemTemplate><%#: Eval("Descripcion_ar") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_et_Descripcion" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Descripcion_ar") %>' MaxLength="300" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_et_Descripcion" ValidationGroup="EditarArticulo" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                                <asp:TextBox ID="txt_et_Imagen" runat="server" CssClass="form-control form-control-sm mt-1" Text='<%# Bind("Imagen") %>' placeholder="URL de la imagen" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Categoría">
                            <ItemTemplate><%#: NombreCategoria(Eval("Categoria_ar")) %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddl_et_Categoria" runat="server" CssClass="form-select form-select-sm" DataTextField="Descripcion_ca" DataValueField="IdCategoria_ca" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Stock">
                            <ItemTemplate><%#: Eval("Stock_ar") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_et_Stock" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Stock_ar") %>' style="width: 6rem" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_et_Stock" ValidationGroup="EditarArticulo" ValidationExpression="^\d+$" Display="Dynamic" CssClass="validador" ErrorMessage="Solo números." />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Precio">
                            <ItemTemplate><span class="text-nowrap"><%#: Eval("PrecioUnitario_ar", "{0:C0}") %></span></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_et_PrecioUnitario" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("PrecioUnitario_ar", "{0:0}") %>' style="width: 7rem" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_et_PrecioUnitario" ValidationGroup="EditarArticulo" ValidationExpression="^[1-9]\d*$" Display="Dynamic" CssClass="validador" ErrorMessage="Número mayor a 0." />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <span class='<%# (bool)Eval("Estado_ar") ? "badge text-bg-success" : "badge text-bg-secondary" %>'><%# (bool)Eval("Estado_ar") ? "Activo" : "Dado de baja" %></span>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddl_et_Estado" runat="server" CssClass="form-select form-select-sm" SelectedValue='<%# Eval("Estado_ar").ToString().ToLower() %>'>
                                    <asp:ListItem Value="true">Activo</asp:ListItem>
                                    <asp:ListItem Value="false">Dado de baja</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="text-end text-nowrap">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Edit" CssClass="btn btn-sm btn-outline-primary" ToolTip="Editar" CausesValidation="false"><i class="bi bi-pencil"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="Dar de baja" CausesValidation="false"
                                    Visible='<%# (bool)Eval("Estado_ar") %>' OnClientClick="return confirm('¿Dar de baja este artículo?');"><i class="bi bi-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Update" CssClass="btn btn-sm btn-primary" ValidationGroup="EditarArticulo">Guardar</asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-outline-secondary" CausesValidation="false">Cancelar</asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <div class="card">
        <div class="card-body">
            <div class="d-flex flex-wrap gap-2 justify-content-between align-items-center mb-3">
                <h2 class="h5 mb-0"><i class="bi bi-tags"></i> Categorías</h2>
                <asp:Panel runat="server" DefaultButton="btnAgregarCategoria" CssClass="input-group" style="width: 340px">
                    <asp:TextBox ID="txtDescripcioncat" runat="server" CssClass="form-control" placeholder="Nueva categoría" MaxLength="30" />
                    <asp:Button ID="btnAgregarCategoria" runat="server" Text="Agregar" CssClass="btn btn-primary" ValidationGroup="NuevaCategoria" OnClick="btnAgregarCategoria_Click" />
                </asp:Panel>
            </div>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescripcioncat" ValidationGroup="NuevaCategoria" Display="Dynamic" CssClass="validador text-end mb-2" ErrorMessage="Ingresá el nombre de la categoría." />

            <div class="table-responsive">
                <asp:GridView ID="gvCategorias" runat="server" AutoGenerateColumns="false" DataKeyNames="IdCategoria_ca"
                    CssClass="table table-hover mb-0" GridLines="None"
                    OnRowEditing="gvCategorias_RowEditing" OnRowCancelingEdit="gvCategorias_RowCancelingEdit"
                    OnRowUpdating="gvCategorias_RowUpdating" OnRowDeleting="gvCategorias_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="IdCategoria_ca" HeaderText="ID" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Descripción">
                            <ItemTemplate><%#: Eval("Descripcion_ca") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_eit_Decripcion" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Descripcion_ca") %>' MaxLength="30" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_eit_Decripcion" ValidationGroup="EditarCategoria" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <span class='<%# (bool)Eval("Estado_ca") ? "badge text-bg-success" : "badge text-bg-secondary" %>'><%# (bool)Eval("Estado_ca") ? "Activa" : "Inactiva" %></span>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlEditEstadoCat" runat="server" CssClass="form-select form-select-sm" SelectedValue='<%# Eval("Estado_ca").ToString().ToLower() %>'>
                                    <asp:ListItem Value="true">Activa</asp:ListItem>
                                    <asp:ListItem Value="false">Inactiva</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="text-end text-nowrap">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Edit" CssClass="btn btn-sm btn-outline-primary" ToolTip="Editar" CausesValidation="false"><i class="bi bi-pencil"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="Desactivar" CausesValidation="false"
                                    Visible='<%# (bool)Eval("Estado_ca") %>' OnClientClick="return confirm('¿Desactivar esta categoría?');"><i class="bi bi-trash"></i></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Update" CssClass="btn btn-sm btn-primary" ValidationGroup="EditarCategoria">Guardar</asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-outline-secondary" CausesValidation="false">Cancelar</asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
