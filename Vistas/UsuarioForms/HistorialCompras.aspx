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
    </style>
</head>
<body style =" background-color:cornflowerblue">
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td class="auto-style2" colspan="2">PC - BOOSTER - STORE</td>
            </tr>
            <tr>
                <td class="auto-style4">USUARIO :&nbsp;
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
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvUsuarioCompras" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True" OnSelectedIndexChanging="GridView1_SelectedIndexChanging">
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
                    </asp:GridView>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style5"></td>
                <td class="auto-style5"></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvDetalleCompras" runat="server" AutoGenerateColumns="False">
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
                    </asp:GridView>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
