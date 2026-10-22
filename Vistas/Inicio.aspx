<%@ Page Title="Iniciar sesión · PC Booster Store" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="Vistas.Inicio" %>

<asp:Content ContentPlaceHolderID="Contenido" runat="server">
    <div class="row justify-content-center align-items-center portada">
        <div class="col-md-6 col-lg-5 col-xl-4">
            <div class="text-center mb-4">
                <div class="marca"><i class="bi bi-cpu-fill text-primary"></i> PC Booster Store</div>
                <p class="text-body-secondary mb-0">Las piezas para potenciar tu PC</p>
            </div>
            <div class="card">
                <div class="card-body p-4">
                    <h1 class="h5 mb-3">Iniciar sesión</h1>
                    <asp:Panel runat="server" DefaultButton="btnIniciarSesion">
                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtUsuario" CssClass="form-label" Text="Usuario" />
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" MaxLength="20" autocomplete="username" />
                            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ValidationGroup="Login"
                                Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá tu usuario." />
                        </div>
                        <div class="mb-3">
                            <asp:Label runat="server" AssociatedControlID="txtContraseña" CssClass="form-label" Text="Contraseña" />
                            <asp:TextBox ID="txtContraseña" runat="server" CssClass="form-control" TextMode="Password" MaxLength="7" autocomplete="current-password" />
                            <asp:RequiredFieldValidator ID="rfvContra" runat="server" ControlToValidate="txtContraseña" ValidationGroup="Login"
                                Display="Dynamic" CssClass="validador" ErrorMessage="Ingresá tu contraseña." />
                        </div>
                        <asp:Button ID="btnIniciarSesion" runat="server" Text="Ingresar" CssClass="btn btn-primary w-100" ValidationGroup="Login" OnClick="btnIniciarSesion_Click" />
                    </asp:Panel>
                </div>
                <div class="card-footer bg-white text-center py-3">
                    ¿No tenés cuenta? <a href="<%: ResolveUrl("~/UsuarioForms/RegistroUsuario.aspx") %>">Registrate</a>
                </div>
            </div>
            <p class="text-center text-body-secondary small mt-3 mb-0">
                Para probar: <code>Nom1</code> / <code>ctr1</code> (cliente) o <code>admAmador</code> / <code>admpsw2</code> (admin)
            </p>
        </div>
    </div>
</asp:Content>
