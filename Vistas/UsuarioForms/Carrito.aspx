﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="Vistas.Carrito" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>PC BOOSTER STORE - Mi carrito</title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style7 {
            text-align: justify;
        }
        .auto-style9 {
            width: 382px;
            height: 30px;
        }
        .auto-style10 {
            text-align: center;
        }
        .auto-style11 {
            text-align: left;
        }
        .auto-style12 {
            height: 40px;
        }
        .auto-style13 {
            font-size: x-large;
        }
        .auto-style17 {
            text-align: right;
        }
        .auto-style18 {
            width: 382px;
            text-align: left;
        }
        .auto-style19 {
            width: 382px;
        }
        .auto-style20 {
            font-size: xx-large;
        }
    </style>
</head>
<body style =" background-color:cornflowerblue">
        <form id="form1" runat="server">
            <table class="auto-style1">
                <tr>
                    <td colspan="3" class="auto-style12">
                        <h1 class="auto-style10">PC - BOOSTER - STORE</h1>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style19">
                        <h4 class="auto-style11">BIENVENIDO :
                    <asp:Label ID="lblNombreUsuario" runat="server" CssClass="auto-style13"></asp:Label>
                        </h4>
                    </td>
                    <td class="auto-style17" colspan="2">
                        <strong>IR A :</strong>&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlMenuNavegacion" runat="server" OnSelectedIndexChanged="dllMenuNavegacion_SelectedIndexChanged">
                        <asp:ListItem Selected="True">--Seleccionar--</asp:ListItem>
                        <asp:ListItem Value="1">Mi cuenta</asp:ListItem>
                        <asp:ListItem Value="2">Mis Compras</asp:ListItem>
                        <asp:ListItem Value="3">Catalogo</asp:ListItem>
                        <asp:ListItem Value="4">Salir</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3" style="background-color: #000066">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3"><strong>MI CARRITO</strong></td>
                </tr>
                <tr>
                    <td class="auto-style19" rowspan="6">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:GridView ID="gvMiscompras" runat="server" AutoGenerateDeleteButton="True" OnRowDeleting="gvMiscompras_RowDeleting" style =" background-color:aliceblue" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
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
                    <td class="auto-style10" colspan="2">
                        <strong>
                        <asp:Label ID="lblMSJTotal" runat="server" Text="TOTAL" CssClass="auto-style20"></asp:Label>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10" colspan="2"><strong>
                        <asp:Label ID="lblTotalCompra" runat="server" CssClass="auto-style20"></asp:Label>
                        </strong>          
                        <br />
&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style7" colspan="2">
                        <asp:Label ID="lblDirecEntrega" runat="server" Text="DIRECCION DE ENTREGA:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style11" colspan="2">
                        <asp:TextBox ID="txtDireccionDeEntrega" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDireccionDeEntrega" ErrorMessage="Por favor ingrese la direccion de entrega" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style7" colspan="2">
                        <asp:Label ID="lblContacto" runat="server" Text="QUIEN RECIBE?"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style11" colspan="2">
                        <asp:TextBox ID="txtContacto" runat="server" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtContacto" ErrorMessage="Ingrese un nombre de contacto" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style9">
&nbsp;&nbsp;&nbsp;<asp:Button ID="btnConfirmarCompra" runat="server" Text="Confirmar Compra" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" OnClick="btnConfirmarCompra_Click" ValidationGroup="1"  />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancelarCompra" runat="server" Text="Cancelar Compra" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" OnClick="btnCancelarCompra_Click"  />
                    </td>
                    <td colspan="2">&nbsp;&nbsp;&nbsp;
                        </td>
                </tr>
                <tr>
                    <td class="auto-style9" style="background-color: #000066">
                        &nbsp;</td>
                    <td colspan="2" style="background-color: #000066">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style19" rowspan="2">
                        <asp:Label ID="lblCarritoVacio" runat="server" Text="No hay productos seleccionados." Visible="False"></asp:Label>
                        <br />
                        <br />
                        <asp:Button ID="btnVolverACatalogo" runat="server" OnClick="btnVolverACatalogo_Click" Text="Volver al catalogo"  />
                    </td>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
&nbsp;&nbsp;&nbsp;<strong> AGREGAR NUEVA TARJETA</strong></td>
                </tr>
                <tr>
                    <td class="auto-style18" rowspan="2">
                        Anteriormente Utilizaste alguna de estas tarjetas, podes volver a asignarlas como medio de pago seleccionandola</td>
                    <td>
                        Numero de Tarjeta:&nbsp; 
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumeroTarjeta" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Titular :
                        </td>
                    <td>
                        <asp:TextBox ID="txtTarjetaTitular" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style18" rowspan="4">
                        <asp:GridView ID="gvTarjetas" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True" OnSelectedIndexChanging="gvTarjetas_SelectedIndexChanging" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                            <Columns>
                                <asp:TemplateField HeaderText="USUARIO N°">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_it_numerousuario" runat="server" Text='<%# Bind("Id_TarjetaUsuario_t") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TITULAR">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTitular" runat="server" Text='<%# Bind("Titular_Tarjeta_t") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NOMBRE TARJETA">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNombreTarjeta" runat="server" Text='<%# Bind("Nombre_Tarjeta_t") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TIPO">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_it_Tipotarjeta" runat="server" Text='<%# Bind("Tipo_Tarjeta_t") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NUMERO TARJETA(4)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNumeroTarjeta" runat="server" Text='<%# Bind("Num_Tarjeta_t") %>'></asp:Label>
                                        <br />
                                        <asp:Label ID="lbl_it_Codseguridad" runat="server" Text='<%# Bind("Codigo_Seguridad_t") %>' Visible="False"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VENCIMIENTO">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVencimiento" runat="server" Text='<%# Bind("Fecha_Vencimiento_t") %>'></asp:Label>
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
                    <td class="auto-style11">
                        Vencimiento : </td>
                    <td>
                        MM<asp:TextBox ID="txtTarjetames" runat="server" Height="16px" Width="17px"></asp:TextBox>
&nbsp;/ AAAA<asp:TextBox ID="txtTarjetaAño" runat="server" Height="16px" Width="28px"></asp:TextBox>
&nbsp;&nbsp;&nbsp; </td>
                </tr>
                <tr>
                    <td colspan="2" class="auto-style17">
                        <asp:RadioButtonList ID="rbDebitoCredito" runat="server" AutoPostBack="True" Height="29px" RepeatDirection="Horizontal" Width="321px">
                            <asp:ListItem Value="0">Debito</asp:ListItem>
                            <asp:ListItem Value="1">Credito</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Codigo Seguridad:&nbsp; 
                        <br />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCodSeguridad" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                        Nombre Tarjeta</td>
                    <td class="auto-style11">
                        <strong>
                        <asp:TextBox ID="txtNombreTarjeta" runat="server"></asp:TextBox>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style19" rowspan="2">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="1" />
                        <asp:Label ID="lbl_Tipotarjeta" runat="server" Visible="False"></asp:Label>
&nbsp;<asp:Label ID="lbl_Codtarjeta" runat="server" Visible="False"></asp:Label>
                    </td>
                    <td colspan="2" class="auto-style10">
                        <asp:Button ID="btnGuardarTarjeta" runat="server" Text="Guardar Tarjeta" BackColor="#0066CC" BorderColor="#666699" ForeColor="#CCFFFF" BorderStyle="Groove" OnClick="btnGuardarTarjeta_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
            </table>
        <div>
        </div>
    </form>
</body>
</html>
