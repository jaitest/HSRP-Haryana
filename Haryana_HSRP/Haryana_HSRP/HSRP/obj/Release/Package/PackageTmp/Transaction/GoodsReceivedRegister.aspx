<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsReceivedRegister.aspx.cs"
    Inherits="HSRP.Master.GoodsReceivedRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 200px;
            min-height: 150px;
            background: white;
        }
    </style>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/gridStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ConfirmReceived() {
            if (confirm("You Confirmed?")) {
                document.getElementById("printflag").value = "Received";
                return true;
            }
            else {
                return false;
            }

        }

        function ConfirmInvoice() {
            if (confirm("Are you really want to Print Invoice?")) {
                document.getElementById("printflag").value = "Remarks";
                //alert(document.getElementById("printflag").value);
                return true;
            }
            else {
                return false;
            }

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="margin: 20px;" align="center">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Goods Received Register
                    <asp:HiddenField ID="printflag" runat="server"></asp:HiddenField>
                </div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td class="form_text" align="left">
                        Dispatch Receive Code: <span class="alert">* </span>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDispatchcode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDispatchcode_SelectedIndexChanged">
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="textboxDispatchCode" runat="server" ReadOnly="true" class="form_textbox"></asp:TextBox>--%>
                    </td>
                    <td>
                    </td>
                    <td class="form_text" align="left">
                        Receive Code: <span class="alert">* </span>
                    </td>
                    <td align="left" class="form_text">
                        <asp:TextBox ID="txtReceiveCode" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td class="form_text">From Plant Name</td>
                <td>
                    <asp:Label ID="lblPlantName" runat="server" Text="" class="form_textbox"></asp:Label></td>
                </tr>
                <tr>
                    <td class="form_text" align="left">
                        Receive State: <span class="alert">* </span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtStateName" runat="server" class="form_textbox" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td class="form_text" align="left">
                        Receive TO RTOLocation:<span class="alert">* </span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtRtoName" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" align="left">
                        Shipping Address
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtAddress" runat="server" TabIndex="6" TextMode="MultiLine" Columns="30"
                            Rows="4"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td class="form_text" align="left">
                        Remarks
                    </td>
                    <td align="left">
                        <asp:TextBox ID="textboxRemark" runat="server" TabIndex="6" TextMode="MultiLine"
                            Columns="30" Rows="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b><span>ITEMS:</span></b>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <%-- <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CssClass="button" />--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" Width="100%"
                                        GridLines="None" AutoGenerateColumns="False">
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
                                        <Columns>
                                            <%-- <asp:BoundField DataField="AutoId" HeaderText="AutoId" InsertVisible="False" ReadOnly="True"
                                                SortExpression="AutoId" />--%>
                                            <%--<asp:BoundField DataField="PlantId" HeaderText="PlantId" SortExpression="PlantId" />
                                            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />--%>
                                            <%--   <asp:BoundField DataField="Lasered" HeaderText="Lasered" 
                                                SortExpression="Lasered" />--%>
                                            <%--  <asp:BoundField DataField="Remarks" HeaderText="Remarks" 
                                                SortExpression="Remarks" />
                                            <asp:BoundField DataField="Serialno" HeaderText="Serialno" 
                                                SortExpression="Serialno" />--%>
                                            <%--   <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" 
                                                SortExpression="CreationDate" />--%>
                                            <%--   <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" 
                                                SortExpression="InvoiceNo" />--%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Select
                                                    <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" /></HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CHKSelect" runat="server" Checked="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="id" runat="server" Text='<%#Eval("AutoId") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="plantid" runat="server" Text='<%#Eval("PlantID") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ProductSize" HeaderText="ProductSize" SortExpression="ProductSize" />
                                            <asp:BoundField DataField="ProductColor" HeaderText="ProductColor" SortExpression="ProductColor" />
                                            <asp:BoundField DataField="Prifix" HeaderText="Prifix" SortExpression="Prifix" />
                                            <asp:BoundField DataField="Laseredcodefrom" HeaderText="Laseredcodefrom" SortExpression="Laseredcodefrom" />
                                            <asp:BoundField DataField="Laseredcodeto" HeaderText="Laseredcodeto" SortExpression="Laseredcodeto" />
                                            <%--<asp:BoundField DataField="Serialno" HeaderText="Serialno" SortExpression="SerialNo" />--%>
                                            <%--  <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity" />--%>
                                            <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblquentity" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pre Quantity">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPreQuantity" runat="server" Text='<%#Eval("prequentity") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Manual Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox1" Enabled="false" runat="server" Text='<%#Eval("Quantity") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Rate" ReadOnly="false" DataField="Rate" SortExpression="Rate" />
                                            <asp:TemplateField HeaderText="SerialNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSerialno" runat="server" Text='<%#Eval("Serialno") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                    <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300"
                                        Font-Bold="True" />
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
    </div>
    </form>
</body>
</html>
