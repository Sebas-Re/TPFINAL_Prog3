<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportesAdmin.aspx.cs" Inherits="Vistas.AdminForms.ReportesAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            font-size: x-large;
            text-align: center;
        }
        .auto-style3 {
            text-align: right;
        }
        .auto-style4 {
            width: 175px;
        }
        .auto-style5 {
            margin-right: 0px;
        }
        .auto-style6 {
            height: 52px;
        }
    </style>
</head>
<body style = "background-color: lightcyan">
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td class="auto-style2" colspan="7"><strong>PC -BOOSTER - STORE</strong></td>
            </tr>
            <tr>
                <td colspan="7" style="background-color: #000066">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">BIENVENIDO :
                    <asp:Label ID="lblNombreAdmin" runat="server"></asp:Label>
                </td>
                <td class="auto-style3" colspan="4">
                    <asp:HyperLink ID="hlVolverhome" runat="server" NavigateUrl="~/AdminForms/AdminHome.aspx">VOLVER</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td colspan="7" style="background-color: #000066">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">REPORTES CLIENTES</td>
                <td>&nbsp;</td>
                <td colspan="2">&nbsp;</td>
                <td colspan="2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">ID CLIENTE :&nbsp;
                    <asp:TextBox ID="txtIdUsuarioBuscar" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnusuariobuscar" runat="server" Text="Encontrar Usuario" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove"/>
                </td>
                <td colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <asp:GridView ID="gvVentas" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" AutoGenerateColumns="False" CssClass="auto-style5" Height="114px" OnSelectedIndexChanging="gvVentas_SelectedIndexChanging" Width="205px" AutoGenerateSelectButton="True" GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <Columns>
                            <asp:TemplateField HeaderText="ID Venta">
                                <ItemTemplate>
                                    <asp:Label ID="lblidVenta" runat="server" Text='<%# Bind("IdVentas_ve") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID Usuario">
                                <ItemTemplate>
                                    <asp:Label ID="lblidUsuario" runat="server" Text='<%# Bind("IdUsuario_ve") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Venta">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalVenta" runat="server" Text='<%# Bind("TotalVenta_ve") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Venta">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaVenta" runat="server" Text='<%# Bind("FechaVenta_ve") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle ForeColor="Black" BackColor="#EEEEEE" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView>
                </td>
                <td colspan="6">
                    <asp:GridView ID="gvDetallesVenta" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle ForeColor="Black" BackColor="#EEEEEE" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="7" style="background-color: #000066">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">REPORTES ARTICULO</td>
                <td>&nbsp;</td>
                <td colspan="2">&nbsp;</td>
                <td colspan="2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">TOP 10 ARTICULOS</td>
                <td>COD ARTICULO :
                    &nbsp;
                    </td>
                <td>
                    <asp:TextBox ID="txtCodArticulo" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                    <asp:Button ID="btnencontrarArt" runat="server" Text="Encontrar Articulo" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove"/>
                </td>
            </tr>
            <tr>
                <td class="auto-style4" rowspan="3">
                    &nbsp;</td>
                <td colspan="6" class="auto-style6">
                    <asp:GridView ID="gvArts" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle ForeColor="Black" BackColor="#EEEEEE" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="6">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="6">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">&nbsp;</td>
                <td>&nbsp;</td>
                <td colspan="2">&nbsp;</td>
                <td colspan="2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
