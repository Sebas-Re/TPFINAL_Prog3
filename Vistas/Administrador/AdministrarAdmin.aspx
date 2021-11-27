<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdministrarAdmin.aspx.cs" Inherits="Vistas.AdministrarAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style2 {
            font-size: xx-large;
            text-align: center;
        }
        .auto-style3 {
            width: 100%;
        }
        .auto-style4 {
            height: 25px;
            width: 240px;
        }
        .auto-style7 {
            width: 465px;
            float: right;
        }
        .auto-style8 {
            height: 25px;
            width: 465px;
            text-align: right;
        }
        .auto-style9 {
            width: 465px;
            text-align: right;
            height: 31px;
        }
        .auto-style12 {
            width: 149px;
        }
        .auto-style13 {
            height: 25px;
            width: 149px;
        }
        .auto-style14 {
            width: 465px;
            text-align: right;
            height: 34px;
        }
        .auto-style15 {
            width: 149px;
            height: 34px;
        }
        .auto-style17 {
            height: 34px;
            width: 240px;
        }
        .auto-style18 {
            width: 465px;
            text-align: right;
            height: 32px;
        }
        .auto-style19 {
            width: 149px;
            height: 32px;
        }
        .auto-style21 {
            height: 32px;
            width: 240px;
        }
        .auto-style22 {
            width: 465px;
            text-align: right;
            height: 36px;
        }
        .auto-style23 {
            width: 149px;
            height: 36px;
        }
        .auto-style25 {
            height: 36px;
            width: 240px;
        }
        .auto-style26 {
            width: 149px;
            height: 31px;
        }
        .auto-style28 {
            height: 31px;
            width: 240px;
        }
        .auto-style29 {
            margin-top: 6px;
        }
        .auto-style30 {
            width: 240px;
        }
        .auto-style32 {
            width: 149px;
            height: 23px;
        }
        .auto-style34 {
            width: 240px;
            height: 23px;
        }
        .auto-style35 {
            width: 465px;
            float: right;
            height: 23px;
        }
        .auto-style36 {
            width: 465px;
            float: right;
            height: 23px;
            text-align: right;
        }
        .auto-style37 {
            width: 465px;
            float: right;
            height: 137px;
        }
        .auto-style38 {
            width: 149px;
            height: 137px;
        }
        .auto-style40 {
            width: 240px;
            height: 137px;
        }
        .auto-style41 {
            width: 465px;
            float: right;
            text-align: right;
        }
        .auto-style42 {
            height: 31px;
            width: 194px;
        }
        .auto-style43 {
            height: 36px;
            width: 194px;
        }
        .auto-style44 {
            height: 25px;
            width: 194px;
        }
        .auto-style45 {
            height: 32px;
            width: 194px;
        }
        .auto-style46 {
            height: 34px;
            width: 194px;
        }
        .auto-style47 {
            width: 194px;
        }
        .auto-style48 {
            width: 194px;
            height: 23px;
        }
        .auto-style49 {
            width: 194px;
            height: 137px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
         <p class="auto-style2">
             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Administrar Admin/Usuarios</p>
        <table class="auto-style3">
            <tr>
                <td class="auto-style9">ID admin</td>
                <td class="auto-style26">
                    <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style42">
                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ErrorMessage="Debe ingresar un Id" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style28">
                    <asp:Label ID="lblIdAdmin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style22">Contraseña</td>
                <td class="auto-style23">
                    <asp:TextBox ID="txtContra1" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td class="auto-style43">
                    <asp:RequiredFieldValidator ID="rfvContra1" runat="server" ControlToValidate="txtContra1" ErrorMessage="Debe ingresar una contraseña" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style25">
                    ir a: <asp:DropDownList ID="ddlNavegacion" runat="server">
                        <asp:ListItem>--Seleccionar--</asp:ListItem>
                        <asp:ListItem>Articulos</asp:ListItem>
                        <asp:ListItem>Inicio</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="auto-style8">Repetir Contraseña</td>
                <td class="auto-style13">
                    <asp:TextBox ID="txtContra2" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td class="auto-style44">
                    <asp:RequiredFieldValidator ID="rfvContra2" runat="server" ControlToValidate="txtContra2" ErrorMessage="Debe repetir la contraseña" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:CompareValidator ID="cvContraseñas" runat="server" ControlToCompare="txtContra2" ControlToValidate="txtContra1" ErrorMessage="Las contraseñas deben ser iguales" ValidationGroup="1">*</asp:CompareValidator>
                </td>
                <td class="auto-style4">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style18">E-mail</td>
                <td class="auto-style19">
                    <asp:TextBox ID="txtmail" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style45"></td>
                <td class="auto-style21"></td>
            </tr>
            <tr>
                <td class="auto-style14">Telefono</td>
                <td class="auto-style15">
                    <asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style46"></td>
                <td class="auto-style17"></td>
            </tr>
            <tr>
                <td class="auto-style7">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar Admin" CssClass="auto-style29" Height="26px" ValidationGroup="1" />
                </td>
                <td class="auto-style12">
                    <asp:Button ID="btnRegistrarse" runat="server" Text="Registrar Admin" ValidationGroup="1" />
                </td>
                <td class="auto-style47">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="1" />
                </td>
                <td class="auto-style30">&nbsp;</td>
            </tr>
    
            <tr>
                <td class="auto-style35">
                </td>
                <td class="auto-style32">
                </td>
                <td class="auto-style48">
                </td>
                <td class="auto-style34"></td>
            </tr>
            <tr>
                <td class="auto-style36">
                    Control usuarios</td>
                <td class="auto-style32">
                </td>
                <td class="auto-style48">
                </td>
                <td class="auto-style34">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style37">
                    </td>
                <td class="auto-style38">
                    &nbsp;</td>
                <td class="auto-style49">
                    </td>
                <td class="auto-style40"></td>
            </tr>
            <tr>
                <td class="auto-style7">
                    &nbsp;</td>
                <td class="auto-style12">
                    &nbsp;</td>
                <td class="auto-style47">
                    &nbsp;</td>
                <td class="auto-style30">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7">
                    &nbsp;</td>
                <td class="auto-style12">
                    &nbsp;</td>
                <td class="auto-style47">
                    &nbsp;</td>
                <td class="auto-style30">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style41">
                    &nbsp;Categorias</td>
                <td class="auto-style12">
                    <asp:GridView ID="gvCategorias" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
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
                <td class="auto-style47">
                    &nbsp;</td>
                <td class="auto-style30">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style41">
                    Agregar Categoria&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Id Categoria:</td>
                <td class="auto-style12">
                    <asp:TextBox ID="txtIdCategoria" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style47">
                    &nbsp;</td>
                <td class="auto-style30">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style41">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Nombre Categoria&nbsp;</td>
                <td class="auto-style12">
                    <asp:TextBox ID="txtNombreCategoria" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style47">
                    &nbsp;</td>
                <td class="auto-style30">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style7">
                    Control articulos</td>
                <td class="auto-style12">
                    <asp:GridView ID="gvArticulos" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
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
                <td class="auto-style47">
                    &nbsp;</td>
                <td class="auto-style30">&nbsp;</td>
            </tr>
            </form>
    </body>
</html>
