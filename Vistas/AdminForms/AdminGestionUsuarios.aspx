<%@ Page Title="Usuarios · Administración" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AdminGestionUsuarios.aspx.cs" Inherits="Vistas.AdminForms.AdminGestionUsuarios" %>

<asp:Content ContentPlaceHolderID="Pagina" runat="server">
    <h1 class="h3 mb-3">Usuarios</h1>

    <div class="card">
        <div class="card-body">
            <div class="d-flex flex-wrap gap-2 justify-content-between align-items-center mb-3">
                <h2 class="h5 mb-0"><i class="bi bi-people"></i> Cuentas</h2>
                <div class="d-flex flex-wrap gap-2">
                    <asp:Panel runat="server" DefaultButton="btnBuscarId" CssClass="input-group" style="width: 220px">
                        <asp:TextBox ID="txtBuscarID" runat="server" CssClass="form-control" placeholder="ID de usuario" inputmode="numeric" />
                        <asp:LinkButton ID="btnBuscarId" runat="server" CssClass="btn btn-outline-primary" OnClick="btnBuscarId_Click" ToolTip="Buscar por ID" ValidationGroup="Buscar"><i class="bi bi-search"></i></asp:LinkButton>
                    </asp:Panel>
                    <asp:Button ID="btnMostrarTodo" runat="server" Text="Ver todos" CssClass="btn btn-outline-secondary" OnClick="btnMostrarTodo_Click" CausesValidation="false" />
                    <button class="btn btn-primary" type="button" data-bs-toggle="collapse" data-bs-target="#nuevoUsuario" aria-expanded="false" aria-controls="nuevoUsuario">
                        <i class="bi bi-person-plus"></i> Nuevo usuario
                    </button>
                </div>
            </div>
            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtBuscarID" ValidationGroup="Buscar" ValidationExpression="^\d+$" Display="Dynamic" CssClass="validador text-end mb-2" ErrorMessage="El ID es un número." />

            <div class="collapse" id="nuevoUsuario">
                <asp:Panel runat="server" DefaultButton="btnAgregarUsuario" CssClass="border rounded-3 p-3 mb-3 bg-body-tertiary">
                    <div class="row g-3">
                        <div class="col-sm-6 col-lg-3">
                            <asp:Label runat="server" AssociatedControlID="txtNombreUsuario" CssClass="form-label" Text="Nombre de usuario" />
                            <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="form-control" MaxLength="10" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombreUsuario" ValidationGroup="NuevoUsuario" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-sm-6 col-lg-3">
                            <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="E-mail" />
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="30" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="NuevoUsuario" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-sm-6 col-lg-3">
                            <asp:Label runat="server" AssociatedControlID="txtContraseña" CssClass="form-label" Text="Contraseña" />
                            <asp:TextBox ID="txtContraseña" runat="server" CssClass="form-control" TextMode="Password" MaxLength="64" autocomplete="new-password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContraseña" ValidationGroup="NuevoUsuario" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                        </div>
                        <div class="col-sm-6 col-lg-3">
                            <asp:Label runat="server" AssociatedControlID="txtRepContraseña" CssClass="form-label" Text="Repetir contraseña" />
                            <asp:TextBox ID="txtRepContraseña" runat="server" CssClass="form-control" TextMode="Password" MaxLength="64" autocomplete="new-password" />
                            <asp:CompareValidator runat="server" ControlToValidate="txtRepContraseña" ControlToCompare="txtContraseña" ValidationGroup="NuevoUsuario" Display="Dynamic" CssClass="validador" ErrorMessage="No coincide." />
                        </div>
                        <div class="col-sm-6 col-lg-3">
                            <asp:Label runat="server" AssociatedControlID="txtTelefono" CssClass="form-label" Text="Teléfono" />
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="15" />
                        </div>
                        <div class="col-sm-6 col-lg-3">
                            <asp:Label runat="server" AssociatedControlID="ddlTipoUs" CssClass="form-label" Text="Tipo" />
                            <asp:DropDownList ID="ddlTipoUs" runat="server" CssClass="form-select">
                                <asp:ListItem Value="false" Selected="True">Cliente</asp:ListItem>
                                <asp:ListItem Value="true">Administrador</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 offset-lg-3 d-flex align-items-end">
                            <asp:Button ID="btnAgregarUsuario" runat="server" Text="Agregar usuario" CssClass="btn btn-primary w-100" ValidationGroup="NuevoUsuario" OnClick="btnAgregarUsuario_Click" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="table-responsive">
                <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false" DataKeyNames="IdUsuario_us"
                    CssClass="table table-hover mb-0" GridLines="None" EmptyDataText="No hay usuarios para mostrar."
                    OnRowEditing="gvUsuarios_RowEditing" OnRowCancelingEdit="gvUsuarios_RowCancelingEdit"
                    OnRowUpdating="gvUsuarios_RowUpdating" OnRowDeleting="gvUsuarios_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="IdUsuario_us" HeaderText="ID" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Usuario">
                            <ItemTemplate><%#: Eval("Nombre_us") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_eit_nombreUs" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Nombre_us") %>' MaxLength="10" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_eit_nombreUs" ValidationGroup="EditarUsuario" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                                <asp:TextBox ID="txt_eit_passUs" runat="server" CssClass="form-control form-control-sm mt-1" TextMode="Password" MaxLength="64" placeholder="Nueva contraseña (opcional)" autocomplete="new-password" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="E-mail">
                            <ItemTemplate><%#: Eval("Email_us") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_eit_emailUs" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Email_us") %>' MaxLength="30" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txt_eit_emailUs" ValidationGroup="EditarUsuario" Display="Dynamic" CssClass="validador" ErrorMessage="Requerido." />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Teléfono">
                            <ItemTemplate><%#: Eval("Telefono_us") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_eit_telefonoUs" runat="server" CssClass="form-control form-control-sm" Text='<%# Bind("Telefono_us") %>' MaxLength="15" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo">
                            <ItemTemplate>
                                <span class='<%# (bool)Eval("Tipo_us") ? "badge text-bg-primary" : "badge text-bg-light" %>'><%# (bool)Eval("Tipo_us") ? "Administrador" : "Cliente" %></span>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlEditTipo" runat="server" CssClass="form-select form-select-sm" SelectedValue='<%# Eval("Tipo_us").ToString().ToLower() %>'>
                                    <asp:ListItem Value="true">Administrador</asp:ListItem>
                                    <asp:ListItem Value="false">Cliente</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <span class='<%# (bool)Eval("Estado_us") ? "badge text-bg-success" : "badge text-bg-secondary" %>'><%# (bool)Eval("Estado_us") ? "Activo" : "Inactivo" %></span>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlEditEstado" runat="server" CssClass="form-select form-select-sm" SelectedValue='<%# Eval("Estado_us").ToString().ToLower() %>'>
                                    <asp:ListItem Value="true">Activo</asp:ListItem>
                                    <asp:ListItem Value="false">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-CssClass="text-end text-nowrap">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Edit" CssClass="btn btn-sm btn-outline-primary" ToolTip="Editar" CausesValidation="false"><i class="bi bi-pencil"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Delete" CssClass="btn btn-sm btn-outline-danger" ToolTip="Desactivar" CausesValidation="false"
                                    Visible='<%# (bool)Eval("Estado_us") %>' OnClientClick="return confirm('¿Desactivar este usuario?');"><i class="bi bi-person-x"></i></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Update" CssClass="btn btn-sm btn-primary" ValidationGroup="EditarUsuario">Guardar</asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-outline-secondary" CausesValidation="false">Cancelar</asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
