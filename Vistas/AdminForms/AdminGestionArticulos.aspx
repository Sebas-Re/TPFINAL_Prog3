<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminGestionArticulos.aspx.cs" Inherits="Vistas.AdminForms.AdminHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style8 {
            background-color: midnightblue;
        }
        .auto-style11 {
            height: 23px;
            width: 484px;
        }
        .auto-style12 {
            font-size: x-large;
        }
        .auto-style13 {
            text-align: left;
        }
        .auto-style16 {
            text-align: left;
            width: 179px;
        }
        .auto-style17 {
            width: 179px;
        }
        .auto-style18 {
            height: 23px;
            width: 179px;
        }
        .auto-style19 {
            height: 26px;
            width: 179px;
        }
        .auto-style20 {
            text-align: left;
            width: 484px;
        }
        .auto-style21 {
            width: 484px;
        }
        .auto-style22 {
            height: 26px;
            width: 484px;
        }
    </style>
</head>
<body style = "background-color: lightcyan">
    <form id="form1" runat="server">
        <table class="auto-style1">
            <tr>
                <td class="auto-style20">BIENVENIDO<strong>&nbsp; <asp:Label ID="lblNombreadm" runat="server" CssClass="auto-style12"></asp:Label>
                &nbsp;</strong></td>
                <td class="auto-style16">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style8" colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    GESTIONAR ARTICULOS</td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    <asp:TextBox ID="txtIdArticulo" runat="server"></asp:TextBox>
&nbsp;&nbsp;
                    <asp:Button ID="btnBuscarporCodigo" runat="server" Text="Buscar por Codigo" OnClick="btnBuscarporCodigo_Click" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" />
                    <asp:Button ID="btnCargarArticulos" runat="server" OnClick="btnCargarArticulos_Click" Text="Cargar articulos" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove"/>
                    <asp:Label ID="lblMensajeArticulo" runat="server"></asp:Label>
                </td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    <asp:GridView ID="gvArticulos" runat="server" AutoGenerateColumns="False" CellPadding="3" OnRowCancelingEdit="gvArticulos_RowCancelingEdit" OnRowEditing="gvArticulos_RowEditing" OnRowUpdating="gvArticulos_RowUpdating" AllowPaging="True" AutoGenerateEditButton="True" OnPageIndexChanging="gvArticulos_PageIndexChanging" PageSize="5" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" OnRowDeleting="gvArticulos_RowDeleting">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button runat="server" ForeColor="DarkBlue" BackColor="LightBlue"
                                        CommandName="Delete" OnClientClick="return confirm('Esta seguro que desea eliminar este articulo?');"
                                        AlternateText="Delete" Text="Eliminar"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CODIGO">
                                <EditItemTemplate>
                                    <asp:Label ID="lbl_et_Codigo" runat="server" Text='<%# Bind("CodArticulo_ar") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_Codart" runat="server" Text='<%# Bind("CodArticulo_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CATEGORIA">
                                <EditItemTemplate>
                                    <asp:Label ID="lbl_et_Categoria" runat="server" Text='<%# Bind("Categoria_ar") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_Categoria" runat="server" Text='<%# Bind("Categoria_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DESCRIPCION">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_et_Descripcion" runat="server" Text='<%# Bind("Descripcion_ar") %>' MaxLength="300"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_et_Descripcion">Ingrese una descripcion</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_descripcion" runat="server" Text='<%# Bind("Descripcion_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="STOCK">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_et_Stock" runat="server" Text='<%# Bind("Stock_ar") %>' TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_et_Stock">Ingrese el stock</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_Stock" runat="server" Text='<%# Bind("Stock_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PRECIO UNITARIO">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_et_PrecioUnitario" runat="server" Text='<%# Bind("Preciounitario_ar") %>' TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_et_PrecioUnitario">Ingrese el precio</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_PrecioU" runat="server" Text='<%# Bind("PrecioUnitario_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="URL IMAGEN">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_et_Imagen" runat="server" Text='<%# Bind("Imagen") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_Imagen" runat="server" Text='<%# Bind("Imagen") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DISPONIBLE">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlEditEstado" runat="server">
                                        <asp:ListItem Value="true">Activo</asp:ListItem>
                                        <asp:ListItem Value="false">Inactivo</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_Estado" runat="server" Text='<%# Bind("Estado_ar") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                </td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style8" colspan="2" style="background-color: #000066">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">AGREGAR ARTICULOS</td>
                <td class="auto-style17">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    Codigo&nbsp; Articulo
                    <asp:TextBox ID="txtCodArticulo" runat="server" ValidationGroup="1" MaxLength="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCodArticulo" ErrorMessage="Por favor ingresar un codigo de articulo" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style17">
                </td>
            </tr>
            <tr>
                <td class="auto-style11">
                    &nbsp;Categoria&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtCategoria" runat="server" ValidationGroup="1" MaxLength="2"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCategoria" ErrorMessage="Por favor ingresar una categoria" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCategoria" ErrorMessage="Solo ingrese numero en Categoria" ValidationExpression="^\d+$" ValidationGroup="1">*</asp:RegularExpressionValidator>
                </td>
                <td class="auto-style18">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="1" />
                </td>
            </tr>
            <tr>
                <td class="auto-style21">
                    &nbsp;Descripcion&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtDescripcion" runat="server" ValidationGroup="1" MaxLength="300"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDescripcion" ErrorMessage="Por favor ingresar una descripcion" ValidationGroup="1">*</asp:RequiredFieldValidator>
&nbsp;<asp:Button ID="btnAgregarArticulo" runat="server" Text="Agregar Articulo" OnClick="btnAgregarArticulo_Click" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" ValidationGroup="1" />
                    <asp:Label ID="lblMensajeA" runat="server"></asp:Label>
                </td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style11">
                    Stock&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtStock" runat="server" ValidationGroup="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtStock" ErrorMessage="Por favor ingresar stock" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStock" ErrorMessage="Solo ingrese numero en Stock" ValidationExpression="^\d+$" ValidationGroup="1">*</asp:RegularExpressionValidator>
                </td>
                <td class="auto-style18">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style22">
                    Precio Unitario&nbsp;&nbsp;
                    <asp:TextBox ID="txtPrecio_Unitario" runat="server" ValidationGroup="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPrecio_Unitario" ErrorMessage="Por favor ingresar precio unitario" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPrecio_Unitario" ErrorMessage="Solo ingrese numero en Precio unitario" ValidationExpression="^\d+$" ValidationGroup="1">*</asp:RegularExpressionValidator>
                </td>
                <td class="auto-style19">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    Imagen (Solo url)<asp:TextBox ID="txtImagen" runat="server" ValidationGroup="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtImagen" ErrorMessage="Por favor ingresar link de la imagen" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="background-color: #000066">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    GESTIONAR CATEGORIAS</td>
                <td class="auto-style13">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    <asp:TextBox ID="txtBuscarCatid" runat="server"></asp:TextBox>
&nbsp;
                    <asp:Button ID="btnBuscarCategoria" runat="server" Text="Buscar por Categoria " OnClick="btnBuscarCategoria_Click" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" />
                    <asp:Button ID="Button1" runat="server" Text="Cargar categorias" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" OnClick="Button1_Click"/>
                    <asp:Label ID="lblMensajeCat" runat="server"></asp:Label>
                </td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    <asp:GridView ID="gvCategorias" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" AutoGenerateEditButton="True" OnRowCancelingEdit="gvCategorias_RowCancelingEdit" OnRowDeleting="gvCategorias_RowDeleting" OnRowEditing="gvCategorias_RowEditing" OnRowUpdating="gvCategorias_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Button runat="server" BackColor="#0052a4" ForeColor="White"
                                        CommandName="Delete" OnClientClick="return confirm('Esta seguro que desea eliminar la categoria?');"
                                        AlternateText="Delete" Text="Eliminar"/>               
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID">
                                <EditItemTemplate>
                                    <asp:Label ID="lbl_eit_IdCategoria" runat="server" Text='<%# Bind("IdCategoria_ca") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_IdCategoria" runat="server" Text='<%# Bind("IdCategoria_ca") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DESCRIPCION">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_eit_Decripcion" runat="server" Text='<%# Bind("Descripcion_ca") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_Descripcion" runat="server" Text='<%# Bind("Descripcion_ca") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DISPONIBLE">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlEditEstadoCat" runat="server">
                                        <asp:ListItem Value="true">Activo</asp:ListItem>
                                        <asp:ListItem Value="false">Inactivo</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_it_Estado" runat="server" Text='<%# Bind("Estado_ca") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
                </td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="background-color: #000066">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    AGREGAR CATEGORIAS</td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    Descripcion&nbsp;&nbsp;
                    <asp:TextBox ID="txtDescripcioncat" runat="server"></asp:TextBox>
&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnAgregarCategoria" runat="server" Text="Agregar Categoria" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" OnClick="btnAgregarCategoria_Click" />
                &nbsp;<asp:Label ID="lblAgregarCategoria" runat="server"></asp:Label>
                </td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    &nbsp;</td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    &nbsp;</td>
                <td class="auto-style17">
                    &nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
