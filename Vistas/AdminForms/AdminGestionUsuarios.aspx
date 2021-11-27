<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminGestionUsuarios.aspx.cs" Inherits="Vistas.AdminForms.InicioAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style3 {
            width: 374px;
        }
        .auto-style5 {
            width: 374px;
            height: 23px;
        }
        .auto-style6 {
            height: 23px;
        }
        .auto-style7 {
            width: 204px;
        }
        .auto-style8 {
            width: 204px;
            height: 23px;
        }
        .auto-style9 {
            background-color: #0000CC;
        }
        .auto-style10 {
            width: 374px;
            background-color: #0000CC;
        }
        .auto-style11 {
            width: 204px;
            background-color: #0000CC;
        }
        .auto-style12 {
            width: 374px;
            text-align: left;
            height: 30px;
        }
        .auto-style13 {
            text-align: right;
        }
        .auto-style14 {
            width: 204px;
            height: 26px;
        }
        .auto-style15 {
            width: 374px;
            height: 26px;
        }
        .auto-style16 {
            height: 26px;
        }
        .auto-style17 {
            margin-right: 3px;
        }
        .auto-style18 {
            width: 204px;
            height: 30px;
        }
        .auto-style19 {
            height: 30px;
        }
    </style>
</head>
<body style = "background-color: lightcyan">
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td class="auto-style7">BIENVENIDO</td>
                <td class="auto-style3">
                    <asp:Label ID="lblNombreadm" runat="server"></asp:Label>
                </td>
                <td class="auto-style13">
                    <asp:HyperLink ID="hlVolver" runat="server" NavigateUrl="~/AdminForms/AdminHome.aspx">Volver</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="auto-style7">PC - BOOSTER - STORE</td>
                <td class="auto-style3">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style11">&nbsp;</td>
                <td class="auto-style10">&nbsp;</td>
                <td class="auto-style9">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style8">GESTION USUARIOS</td>
                <td class="auto-style5"></td>
                <td class="auto-style6"></td>
            </tr>
            <tr>
                <td class="auto-style18">lista de usuarios : </td>
                <td class="auto-style12">
                    <asp:TextBox ID="txtBuscarID" runat="server"></asp:TextBox>
                    <asp:Button ID="btnBuscarId" runat="server" Text="Buscar ID" OnClick="btnBuscarId_Click" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" />
                    <asp:Button ID="btnMostrarTodo" runat="server" Text="Mostrar Todo" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" OnClick="btnMostrarTodo_Click" />
                    <asp:Label ID="lblMensajeUsuario" runat="server"></asp:Label>
                </td>
                <td class="auto-style19">
                    </td>
            </tr>
            <tr>
                <td class="auto-style7">&nbsp;</td>
                <td class="auto-style3">
                    <asp:GridView ID="gvUsuarios" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" AutoGenerateEditButton="True" CssClass="auto-style17" OnRowCancelingEdit="gvUsuarios_RowCancelingEdit1" OnRowDeleting="gvUsuarios_RowDeleting1" OnRowEditing="gvUsuarios_RowEditing1" OnRowUpdating="gvUsuarios_RowUpdating1">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ForeColor="#000066" BackColor="#6699ff"
                                        CommandName="Delete" OnClientClick="return confirm('Esta seguro que quiere eliminar el usuario?');"
                                        AlternateText="Delete" Text="Eliminar"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID">
                                <EditItemTemplate>
                                    <asp:Label ID="lbl_eit_idUs" runat="server" Text='<%# Bind("IdUsuario_us") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_idUs" runat="server" Text='<%# Bind("IdUsuario_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NOMBRE">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_eit_nombreUs" runat="server" Text='<%# Bind("Nombre_us") %>' MaxLength="20"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_eit_nombreUs">Ingrese un nombre</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_nombreUs" runat="server" Text='<%# Bind("Nombre_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CONTRASEÑA">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_eit_passUs" runat="server" Text='<%# Bind("Contraseña_us") %>' MaxLength="7" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_eit_passUs">Ingrese una contraseña</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_passUs" runat="server" Text='<%# Bind("Contraseña_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EMAIL">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_eit_emailUs" runat="server" Text='<%# Bind("Email_us") %>' MaxLength="30" TextMode="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt_eit_emailUs">Ingrese un email</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_emailUs" runat="server" Text='<%# Bind("Email_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TELEFONO">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_eit_telefonoUs" runat="server" Text='<%# Bind("Telefono_us") %>' MaxLength="15" TextMode="Number"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_telefonoUs" runat="server" Text='<%# Bind("Telefono_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TIPO">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlEditTipo" runat="server">
                                        <asp:ListItem Value="true">Administrador</asp:ListItem>
                                        <asp:ListItem Value="false">Cliente</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_tipoUs" runat="server" Text='<%# Bind("Tipo_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ESTADO">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlEditEstado" runat="server">
                                        <asp:ListItem Value="true">Activo</asp:ListItem>
                                        <asp:ListItem Value="false">Inactivo</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_estadoUs" runat="server" Text='<%# Bind("Estado_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style11">&nbsp;</td>
                <td class="auto-style10">&nbsp;</td>
                <td class="auto-style9">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7">AGREGAR USUARIOS</td>
                <td class="auto-style3">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7">Nombre</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtNombreUsuario" runat="server" MaxLength="20"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombreUsuario" ErrorMessage="Por favor ingresar un nombre" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style14">Contraseña</td>
                <td class="auto-style15">
                    <asp:TextBox ID="txtContraseña" runat="server" MaxLength="7" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRepContraseña" ErrorMessage="Por favor ingrese una contraseña" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style16"></td>
            </tr>
            <tr>
                <td class="auto-style7">Repetir Contraseña</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtRepContraseña" runat="server" MaxLength="7" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtRepContraseña" ErrorMessage="Por favor repita la contraseña" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtContraseña" ControlToValidate="txtRepContraseña" ErrorMessage="Las contraseñas no son iguales" ValidationGroup="1">*</asp:CompareValidator>
                &nbsp;</td>
                <td>
                    <asp:Button ID="btnAgregarUsuario" runat="server" Text="Agregar Usuario" OnClick="btnAgregarUsuario_Click" ValidationGroup="1" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" />
                </td>
            </tr>
            <tr>
                <td class="auto-style7">Email</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmail" ErrorMessage="Por favor ingresar ingrese un email" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style7">Telefono</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtTelefono" runat="server" MaxLength="15"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTelefono" ErrorMessage="Solo ingrese numeros en telefono" ValidationExpression="^\d+$" ValidationGroup="1">*</asp:RegularExpressionValidator>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style8">Tipo</td>
                <td class="auto-style5">
                     <asp:DropDownList ID="ddlTipoUs" runat="server">
                        <asp:ListItem Value="seleccionar">--Selccionar tipo usuario--</asp:ListItem>
                        <asp:ListItem Value="true">Administrador</asp:ListItem>
                        <asp:ListItem Value="false">Cliente</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlTipoUs" ErrorMessage="Seleccione un tipo de usuario" InitialValue="seleccionar" ValidationGroup="1">*</asp:RequiredFieldValidator>&nbsp;</td>
                <td class="auto-style6">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="1" />
                </td>
            </tr>
            <tr>
                <td class="auto-style11">&nbsp;</td>
                <td class="auto-style10">&nbsp;</td>
                <td class="auto-style9">&nbsp;</td>
            </tr>
            </table>
        <div>
        </div>
    </form>
</body>
</html>
