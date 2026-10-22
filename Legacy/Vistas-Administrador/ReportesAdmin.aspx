<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportesAdmin.aspx.cs" Inherits="Vistas.ConsultaAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
    .Titulo {
            font-size: xx-large;
            text-align: center;
        }
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 274px;
        }
        .auto-style3 {
            width: 551px;
        }
        .auto-style4 {
            margin-bottom: 0px;
        }
        .auto-style5 {
            width: 551px;
            height: 82px;
        }
        .auto-style6 {
            height: 82px;
        }
   </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="Titulo">
            Consultas</div>
        <table class="auto-style1">
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblNombreAdmin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>Ir a:
                    <asp:DropDownList ID="ddlNavegacion" runat="server" CssClass="auto-style4">
                        <asp:ListItem>--Seleccionar--</asp:ListItem>
                        <asp:ListItem>Catalogo</asp:ListItem>
                        <asp:ListItem>Inicio</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">Ordenar por fecha:</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td>Desde</td>
                <td>Hasta</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td>
                    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="Calendar2" runat="server"></asp:Calendar>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5">
                    <asp:DataList ID="dlFiltros" runat="server">
                    </asp:DataList>
                </td>
                <td class="auto-style6" colspan="2">
                    <asp:GridView ID="gvVentas" runat="server">
                    </asp:GridView>
                </td>
                <td class="auto-style6">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
