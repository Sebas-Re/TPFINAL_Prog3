<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="Vistas.Catalogo" EnableEventValidation="false" ValidateRequest ="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PC BOOSTER STORE - Selecciona tus productos</title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style3 {
            width: 195px;
        }
        .auto-style4 {
            width: 183px;
        }
        .auto-style5 {
            width: 183px;
            text-align: right;
        }
        .auto-style7 {
            text-align: right;
            height: 20px;
        }
        .auto-style8 {
            text-align: left;
            height: 20px;
        }
        .auto-style9 {
            height: 4px;
            font-size: small;
            text-align: center;
        }
        .auto-style10 {
            height: 16px;
            text-align: center;
            font-size: xx-large;
        }
        .auto-style11 {
            width: 183px;
            text-align: left;
        }
        .auto-style12 {
            text-align: right;
        }
        .auto-style16 {
            text-align: right;
            width: 42px;
        }
        .auto-style17 {
            width: 42px;
        }
        .auto-style18 {
            text-align: center;
        }
    </style>
</head>
<body  style = "background-color: lightcyan">
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td class="auto-style10" colspan="3">PC - BOOSTER - STORE </td>
            </tr>
            <tr>
                <td class="auto-style9" colspan="3">SELECCIONA LAS PIEZAS PARA POTENCIAR TU PC</td>
            </tr>
            <tr>
                <td class="auto-style8" colspan="2">BINVENIDO:&nbsp; <asp:Label ID="lblNombreUsuario" runat="server" Font-Bold="True" Font-Size="XX-Large" Width="176px"></asp:Label>
                </td>
                <td class="auto-style7">IR A :&nbsp;
                    <asp:DropDownList ID="ddlMenuNavegacion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMenuNavegacion_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="0">--Seleccionar--</asp:ListItem>
                        <asp:ListItem Value="1">Mi cuenta</asp:ListItem>
                        <asp:ListItem Value="2">Mis Compras</asp:ListItem>
                        <asp:ListItem Value="3">Salir</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="auto-style16" style="background-color: #000066">&nbsp;</td>
                <td class="auto-style12" colspan="2" style="background-color: #000066">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style18">
                    <asp:TextBox ID="txtArticuloBuscar" runat="server" Width="179px"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnbuscar" runat="server" Text="Buscar" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" OnClick="btnbuscar_Click" />
                    &nbsp;
                    <asp:Button ID="btnMostrarTodo" runat="server" Text="Mostrar Todo" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" OnClick="btnMostrarTodo_Click" />
                </td>
                <td class="auto-style5">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    <div class="auto-style18">
&nbsp;
                    &nbsp;
                    &nbsp;
                    &nbsp;
                    &nbsp;
                    &nbsp;
                    &nbsp;</div>
                    <div class="auto-style18">
                    <asp:DataList ID="dlCategorias" runat="server" CellSpacing="12" RepeatDirection="Horizontal" Width="488px">
                        <ItemTemplate>
                            <asp:Button ID="btnCategoria" runat="server" Text='<%# Bind("Descripcion_ca") %>' CommandArgument='<%# bind("IdCategoria_ca") %>' CommandName="SeleccionarCategoria" OnCommand="btnCategoria_Command" Width="150px" />
                        </ItemTemplate>
                    </asp:DataList>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="auto-style17" rowspan="8">
                    &nbsp;</td>
                <td class="auto-style3" rowspan="8">
                    <asp:GridView ID="gvArticulos" runat="server" Height="167px" Width="228px" AutoGenerateSelectButton="True" style =" background-color:aliceblue" AutoGenerateColumns="False" OnSelectedIndexChanging="gvArticulos_SelectedIndexChanging" AllowPaging="True" OnPageIndexChanging="gvArticulos_PageIndexChanging" PageSize="5">
                        <Columns>
                            <asp:TemplateField HeaderText="DESCRIPCION">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind ("Descripcion_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STOCK">
                                <ItemTemplate>
                                    <asp:Label ID="lblStock" runat="server" Text='<%# Bind ("Stock_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PRECIO UNITARIO">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrecioUnitario" runat="server" Text='<%# Bind ("PrecioUnitario_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IMAGEN">
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" Height="100px" ImageUrl='<%# Bind ("Imagen") %>' Width="100px" />
                                    <br />
                                    <asp:Label ID="lblImagen" runat="server" Text='<%# Bind ("Imagen") %>' Visible="False"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
                <td class="auto-style11">&nbsp;Productos en tu carrito</td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <asp:GridView ID="gvCarrito" runat="server" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
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
                    <asp:Label ID="lblTotalCompra" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style17">
                    &nbsp;</td>
                <td class="auto-style3">
                    <asp:Button ID="btnIralCarrito" runat="server" OnClick="btnIralCarrito_Click" Text="Ir a Mi Carrito" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" />
                </td>
                <td class="auto-style4">
                    &nbsp;</td>
            </tr>
            </table>
    </form>
</body>
</html>
