<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistroUsuario.aspx.cs" Inherits="Vistas.RegistroUsuario" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PC BOOSTER STORE - Registro</title>
    <style type="text/css">
        .auto-style3 {
            width: 100%;
        }
        .auto-style10 {
            width: 266px;
            height: 64px;
        }
        .auto-style14 {
            text-align: center;
            height: 32px;
        }
        .auto-style18 {
            text-align: center;
            height: 34px;
        }
        .auto-style26 {
            text-align: center;
            height: 35px;
        }
        .auto-style34 {
            text-align: right;
            height: 33px;
        }
        .auto-style35 {
            text-align: center;
        }
        .auto-style36 {
            text-align: center;
            height: 33px;
        }
        .auto-style37 {
            text-align: center;
            color: #66FF99;
        }
        .auto-style38 {
            width: 370px;
            height: 64px;
        }
        .auto-style39 {
            width: 294px;
            height: 64px;
        }
    </style>
</head>
<body  style =" background-color:cornflowerblue">
    <form id="form1" runat="server">
        <table class="auto-style3">
            <tr>
                <td class="auto-style34" colspan="3">
                    <h1 class="auto-style37"><em>PC - BOOSTER -STORE</em></h1>
                </td>
            </tr>
            <tr>
                <td class="auto-style34" colspan="3">
                    <h2 class="auto-style35">Se parte de la comunidad booster store!</h2>
                </td>
            </tr>
            <tr>
                <td class="auto-style36" colspan="3">Nombre usuario&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtUsuario" runat="server" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ErrorMessage="Debe ingresar un usuario" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style26" colspan="3">Contraseña&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtContra1" runat="server" TextMode="Password" MaxLength="7"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvContra1" runat="server" ControlToValidate="txtContra1" ErrorMessage="Debe ingresar una contraseña" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style35" colspan="3">Repetir Contraseña
                    <asp:TextBox ID="txtContra2" runat="server" TextMode="Password" MaxLength="7"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvContra2" runat="server" ControlToValidate="txtContra2" ErrorMessage="Debe repetir la contraseña" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvContraseñas" runat="server" ControlToCompare="txtContra2" ControlToValidate="txtContra1" ErrorMessage="Las contraseñas deben ser iguales" ValidationGroup="1">*</asp:CompareValidator>
                    <br />
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style18" colspan="3">E-mail&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtEmail" runat="server" MaxLength="30" TextMode="Email"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="Por favor ingrese un correo electronico" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style14" colspan="3">Telefono&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtTelefono" runat="server" MaxLength="15"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTelefono" ErrorMessage="Ingrese solo numeros en telefono" ValidationExpression="^\d+$" ValidationGroup="1">*</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style35" colspan="3">
                    <asp:Button ID="btnRegistrarse" runat="server" Text="Registrarse" ValidationGroup="1" OnClick="btnRegistrarse_Click" />
                </td>
            </tr>
            <tr>
                <td class="auto-style38">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="1" Width="373px" />
                </td>
                <td class="auto-style39">
                    <asp:Label ID="lblMsjRegistro" runat="server"></asp:Label>
                    </td>
                <td class="auto-style10">
                    <br />
                    <asp:HyperLink ID="hlvolverinicio" runat="server" NavigateUrl="~/Inicio.aspx">Volver</asp:HyperLink>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
