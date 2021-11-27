<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="Vistas.AdminForms.AdminHome1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style4 {
            height: 23px;
        }
        .auto-style5 {
            width: 382px;
        }
        .auto-style7 {
            width: 382px;
            text-align: center;
        }
        .auto-style8 {
            height: 23px;
            width: 382px;
            text-align: center;
        }
        .auto-style9 {
            font-size: x-large;
            text-align: center;
            height: 30px;
        }
        .auto-style10 {
            text-align: center;
            height: 23px;
        }
        .auto-style11 {
            font-size: x-large;
        }
        .auto-style13 {
            height: 22px;
        }
        .auto-style14 {
            text-align: center;
        }
    </style>
</head>
<body style = "background-color: lightcyan">
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td class="auto-style9" colspan="5"><strong>PC - BOOSTER -STORE</strong></td>
            </tr>
            <tr>
                <td class="auto-style10" colspan="5">VISTA ADMINISTRADOR</td>
            </tr>
            <tr>
                <td class="auto-style4" colspan="2">BIENVENIDO :
                    <strong>
                    <asp:Label ID="lblNombreAdm" runat="server" CssClass="auto-style11"></asp:Label>
                    </strong>
                </td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4" colspan="5" style="background-color: #0000FF"></td>
            </tr>
            <tr>
                <td class="auto-style10" colspan="5">
                    <asp:HyperLink ID="hlGestionArticulos" runat="server" NavigateUrl="~/AdminForms/AdminGestionArticulos.aspx">GESTION ARTICULOS</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="auto-style13" colspan="5"></td>
            </tr>
            <tr>
                <td class="auto-style4" colspan="5">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style10" colspan="5">
                    <asp:HyperLink ID="hlGestionUsuarios" runat="server" NavigateUrl="~/AdminForms/AdminGestionUsuarios.aspx">GESTION USUARIOS</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="auto-style14" colspan="5">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style14" colspan="5">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style14" colspan="5">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="5" style="background-color: #0000FF">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5" colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5" colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style8">
                    &nbsp;</td>
                <td class="auto-style8" colspan="4">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5" colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style8">
                    &nbsp;</td>
                <td class="auto-style8" colspan="4">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5" colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7">
                    &nbsp;</td>
                <td class="auto-style7" colspan="4">
                    &nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
