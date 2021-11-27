<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="Vistas.Inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PC BOOSTER STORE - Bienvenido</title>
    <style type="text/css">
        .auto-style3 {
            font-size: medium;
        }
        .auto-style5 {
            width: 100%;
        }
        .auto-style6 {
            font-size: small;
            text-align: center;
            }
        .auto-style7 {
            text-align: right;
            width: 222px;
        }
        .auto-style8 {
            text-align: center;
            width: 297px;
        }
        .auto-style10 {
            width: 222px;
        }
        .auto-style11 {
            width: 297px;
        }
        .auto-style12 {
            text-align: right;
            height: 55px;
        }
        .auto-style13 {
            text-align: center;
        }
        .auto-style14 {
            text-align: right;
            height: 30px;
        }
        .auto-style15 {
            text-align: center;
            color: #CCFF99;
        }
        .auto-style16 {
            color: #003300;
        }
        .auto-style17 {
            text-align: center;
            height: 30px;
        }
    </style>
</head>
<body style = "background-color: lightcyan">
    <form id="form1" runat="server" >
        <table class="auto-style5" >
            <tr>
                <td class="auto-style12" colspan="3">
                    <h1 class="auto-style15"><em><span class="auto-style16">PC - BOOSTER -STORE</span></em></h1>
                </td>
            </tr>
            <tr>
                <td class="auto-style14" colspan="3">
                    <h4 class="auto-style13">TU SITIO DE COMPRAS</h4>
                </td>
            </tr>
            <tr>
                <td class="auto-style7">&nbsp;</td>
                <td class="auto-style8">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7">&nbsp;</td>
                <td class="auto-style8">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7">&nbsp;</td>
                <td class="auto-style8">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style17" colspan="3">Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtUsuario" runat="server" ValidationGroup="1" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ValidationGroup="1" ErrorMessage="Debe ingresar un usuario">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style13" colspan="3">Contraseña&nbsp;
                    <asp:TextBox ID="txtContraseña" runat="server" TextMode="Password" ValidationGroup="1" MaxLength="7"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvContra" runat="server" ControlToValidate="txtContraseña" ValidationGroup="1" ErrorMessage="Debe ingresar una contraseña">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style13" colspan="3">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style13" colspan="3">
                    <asp:Label ID="lblUsuarioInexistente" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style13" colspan="3">Ya tengo una cuenta<br />
                    <asp:Button ID="btnIniciarSesion" runat="server" Text="Iniciar Sesion" ValidationGroup="1" OnClick="btnIniciarSesion_Click" style="height: 26px" />
                </td>
            </tr>
            <tr>
                <td class="auto-style6" colspan="3">No estás registrado?<asp:HyperLink ID="hlRegistro" runat="server" NavigateUrl="~/UsuarioForms/RegistroUsuario.aspx">Registrarme!</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="auto-style10">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="1" />
                </td>
                <td class="auto-style11">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <p class="auto-style3">
            &nbsp;</p>
    </form>
</body>
</html>
