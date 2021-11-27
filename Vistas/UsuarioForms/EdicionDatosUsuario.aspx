<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdicionDatosUsuario.aspx.cs" Inherits="Vistas.EdicionDatosUsuario" %>

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
        }
        .auto-style3 {
            font-size: x-large;
        }
        .auto-style4 {
            color: #FFFFCC;
        }
        .auto-style7 {
            color: #FFFFCC;
            text-align: left;
        }
        .auto-style10 {
            text-align: right;
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
                <td colspan="2" class="auto-style10">IR A :&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlMenuNavegacion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMenuNavegacion_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="0">--Seleccionar--</asp:ListItem>
                        <asp:ListItem Value="1">Catalogo</asp:ListItem>
                        <asp:ListItem Value="2">Mis Compras</asp:ListItem>
                        <asp:ListItem Value="3">Mi Carrito</asp:ListItem>
                        <asp:ListItem Value="4">Salir</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="auto-style3" colspan="2">MI PERFIL:<asp:Label ID="lblNombreUsuario" runat="server" Font-Bold="True" Font-Size="XX-Large" Width="176px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style4" colspan="2" style="background-color: #008080">ESTOS SON LOS DATOS QUE BRINDASTE&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblMensajeUsuario" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style7" style="background-color: #008080">
                    <asp:GridView ID="gvDatoUsuario" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="True" OnRowCancelingEdit="gvDatoUsuario_RowCancelingEdit" OnRowEditing="gvDatoUsuario_RowEditing" OnRowUpdating="gvDatoUsuario_RowUpdating">
                        <Columns>
                            <asp:TemplateField HeaderText="ID usuario">
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditId" runat="server" Text='<%# Bind("IdUsuario_us") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("IdUsuario_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditNombre" runat="server" MaxLength="20" Text='<%# Bind("Nombre_us") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEditNombre">Ingrese un nombre</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Nombre_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contraseña">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditContraseña" runat="server" MaxLength="7" Text='<%# Bind("Contraseña_us") %>' TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEditContraseña">Ingrese un contraseña</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Contraseña_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditEmail" runat="server" MaxLength="30" Text='<%# Bind("Email_us") %>' TextMode="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEditEmail">Ingrese un email</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("Email_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Telefono">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditTelefono" runat="server" MaxLength="15" Text='<%# Bind("Telefono_us") %>' TextMode="Number"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("Telefono_us") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />
                    ESTAS SON LAS TARJETAS QUE TENÉS GUARDADAS</td>
                <td class="auto-style4" style="background-color: #008080">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4" colspan="2" style="background-color: #008080">
                    
                    <asp:GridView ID="gvTarjetas" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvTarjetas_RowDeleting" AutoGenerateDeleteButton="True">
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Button runat="server" ForeColor="LightGoldenrodYellow" BackColor="#007575"
                                        CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this event?');"
                                        AlternateText="Delete" Text="Eliminar"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Titular de la tarjeta">
                                <ItemTemplate>
                                    <asp:Label ID="lblTitularTarjeta" runat="server" Text='<%# Bind("Titular_Tarjeta_t") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre Tarjeta">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombreTarjeta" runat="server" Text='<%# Bind("Nombre_Tarjeta_t") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo tarjeta">
                                <ItemTemplate>
                                    <asp:Label ID="lblTipoTarjeta" runat="server" Text='<%# Bind("Tipo_Tarjeta_t") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Numero de tarjeta">
                                <ItemTemplate>
                                    <asp:Label ID="lblNumTarjeta" runat="server" Text='<%# Bind("Num_Tarjeta_t") %>'></asp:Label>
                                    <br />
                                    <asp:Label ID="lblCodSeguridad" runat="server" Text='<%# Bind("Codigo_Seguridad_t") %>' Visible="False"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha vencimiento">
                                <ItemTemplate>
                                    <asp:Label ID="lblFechaVencimiento" runat="server" Text='<%# Bind("Fecha_Vencimiento_t") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            </table>
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
