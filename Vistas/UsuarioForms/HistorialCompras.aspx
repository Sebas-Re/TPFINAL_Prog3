<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistorialCompras.aspx.cs" Inherits="Vistas.HistorialCompras" %>

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
            text-align: center;
            font-size: xx-large;
            color: #66FF66;
        }
        .auto-style3 {
            text-align: right;
        }
        .auto-style4 {
            text-align: left;
        }
        .auto-style5 {
            height: 24px;
        }
        .auto-style6 {
            height: 23px;
        }
        .auto-style7 {
            width: 280px;
        }
        .auto-style8 {
            width: 55px;
        }
    </style>
</head>
<body style =" background-color:cornflowerblue">
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td class="auto-style2" colspan="4">PC - BOOSTER - STORE</td>
            </tr>
            <tr>
                <td class="auto-style4" colspan="3">USUARIO :&nbsp;
                    <asp:Label ID="lblNombreUsuario" runat="server"></asp:Label>
                </td>
                <td class="auto-style3">IR A :&nbsp;
                    <asp:DropDownList ID="ddlMenuNavegacion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMenuNavegacion_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="0">--Seleccionar--</asp:ListItem>
                        <asp:ListItem Value="1">Mi cuenta</asp:ListItem>
                        <asp:ListItem Value="2">Mis Compras</asp:ListItem>
                        <asp:ListItem Value="3">Catalogo</asp:ListItem>
                        <asp:ListItem Value="4">MI Carrito</asp:ListItem>
                        <asp:ListItem Value="5">Salir</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7">
                    <asp:GridView ID="gvUsuarioCompras" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True" OnSelectedIndexChanging="GridView1_SelectedIndexChanging" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                        <Columns>
                            <asp:TemplateField HeaderText="Compra Nº">
                                <ItemTemplate>
                                    <asp:Label ID="lblidVenta" runat="server" Text='<%# Bind("IdVentas_ve") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Compra">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalVenta" runat="server" Text='<%# Bind("TotalVenta_ve") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha de compra">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaVenta" runat="server" Text='<%# Bind("FechaVenta_ve") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" ForeColor="#003399" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <SortedAscendingCellStyle BackColor="#EDF6F6" />
                        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                        <SortedDescendingCellStyle BackColor="#D6DFDF" />
                        <SortedDescendingHeaderStyle BackColor="#002876" />
                    </asp:GridView>
                </td>
                <td class="auto-style8" style="width: 100px">
                    &nbsp;</td>
                <td>
                    <asp:GridView ID="gvDetalleCompras" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                        <Columns>
                            <asp:TemplateField HeaderText="Descripcion del articulo">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescArt" runat="server" Text='<%# Bind("Descripcion_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Codigo de articulo">
                                <ItemTemplate>
                                    <asp:Label ID="lblCodArtDV" runat="server" Text='<%# Bind("CodArticulo_dv") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad comprada">
                                <ItemTemplate>
                                    <asp:Label ID="lblCantComprada" runat="server" Text='<%# Bind("Cantidad_dv") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrecioUnitarioDV" runat="server" Text='<%# Bind("Precio_Unitario_dv") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Direccion de entrega">
                                <ItemTemplate>
                                    <asp:Label ID="lblDirecEntrega" runat="server" Text='<%# Bind("Direccion_de_Entrega") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre de contacto">
                                <ItemTemplate>
                                    <asp:Label ID="lblContacto" runat="server" Text='<%# Bind("Contacto") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#000065" />
                    </asp:GridView>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5" colspan="3"></td>
                <td class="auto-style5"></td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    &nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style6" colspan="3"></td>
                <td class="auto-style6"></td>
            </tr>
        </table>
    </form>
</body>
</html>
