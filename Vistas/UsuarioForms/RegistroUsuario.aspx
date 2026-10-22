<%@ Page Title="Crear cuenta · PC Booster Store" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistroUsuario.aspx.cs" Inherits="Vistas.RegistroUsuario" %>

<asp:Content ContentPlaceHolderID="Contenido" runat="server">
    <div class="row justify-content-center align-items-center portada">
        <div class="col-md-8 col-lg-6">
            <div class="text-center mb-4">
                <div class="marca"><i class="bi bi-cpu-fill text-primary"></i> PC Booster Store</div>
                <p class="text-body-secondary mb-0">Sumate a la comunidad Booster</p>
            </div>
            <div class="card">
                <div class="card-body p-4">
                    <h1 class="h5 mb-3">Crear cuenta</h1>
                    <asp:Panel runat="server" DefaultButton="btnRegistrarse">
                        <div class="row g-3">
                            <div class="col-sm-6">
                                <asp:Label runat="server" AssociatedControlID="txtUsuario" CssClass="form-label" Text="Nombre de usuario" />
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" MaxLength="10" autocomplete="username" />
                                <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ValidationGroup="Registro"
                                    Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá un nombre de usuario." />
                            </div>
                            <div class="col-sm-6">
                                <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass="form-label" Text="E-mail" />
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="30" TextMode="Email" autocomplete="email" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="Registro"
                                    Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá un e-mail." />
                            </div>
                            <div class="col-sm-6">
                                <asp:Label runat="server" AssociatedControlID="txtContra1" CssClass="form-label" Text="Contraseña" />
                                <asp:TextBox ID="txtContra1" runat="server" CssClass="form-control" TextMode="Password" MaxLength="7" autocomplete="new-password" />
                                <div class="form-text">Hasta 7 caracteres.</div>
                                <asp:RequiredFieldValidator ID="rfvContra1" runat="server" ControlToValidate="txtContra1" ValidationGroup="Registro"
                                    Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá una contraseña." />
                            </div>
                            <div class="col-sm-6">
                                <asp:Label runat="server" AssociatedControlID="txtContra2" CssClass="form-label" Text="Repetir contraseña" />
                                <asp:TextBox ID="txtContra2" runat="server" CssClass="form-control" TextMode="Password" MaxLength="7" autocomplete="new-password" />
                                <asp:RequiredFieldValidator ID="rfvContra2" runat="server" ControlToValidate="txtContra2" ValidationGroup="Registro"
                                    Display="Dynamic" CssClass="validador" ErrorMessage="Repetí la contraseña." />
                                <asp:CompareValidator ID="cvContraseñas" runat="server" ControlToValidate="txtContra2" ControlToCompare="txtContra1" ValidationGroup="Registro"
                                    Display="Dynamic" CssClass="validador" ErrorMessage="Las contraseñas no coinciden." />
                            </div>
                            <div class="col-sm-6">
                                <asp:Label runat="server" AssociatedControlID="txtTelefono" CssClass="form-label" Text="Teléfono (opcional)" />
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="15" TextMode="Phone" />
                                <asp:RegularExpressionValidator ID="revTelefono" runat="server" ControlToValidate="txtTelefono" ValidationGroup="Registro"
                                    ValidationExpression="^\d+$" Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá solo números." />
                            </div>
                        </div>
                        <asp:Button ID="btnRegistrarse" runat="server" Text="Crear cuenta" CssClass="btn btn-primary w-100 mt-4" ValidationGroup="Registro" OnClick="btnRegistrarse_Click" />
                    </asp:Panel>
                </div>
                <div class="card-footer bg-white text-center py-3">
                    ¿Ya tenés cuenta? <a href="<%: ResolveUrl("~/Inicio.aspx") %>">Iniciá sesión</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
