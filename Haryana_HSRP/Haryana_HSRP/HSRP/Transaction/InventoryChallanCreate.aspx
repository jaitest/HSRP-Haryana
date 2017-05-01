<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryChallanCreate.aspx.cs"
    Inherits="HSRP.Master.InventoryChallanCreate" %>

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
        .style4
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 159px;
        }
        .style7
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 159px;
            height: 45px;
        }
        .style9
        {
            height: 45px;
        }
        .style10
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            height: 45px;
        }
        .style11
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 159px;
            height: 33px;
        }
        .style13
        {
            height: 33px;
        }
        .style14
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            height: 33px;
        }
        .style15
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 259px;
        }
        .style16
        {
            height: 33px;
            width: 259px;
        }
        .style17
        {
            height: 45px;
            width: 259px;
        }
        .style18
        {
            height: 33px;
            width: 88px;
        }
        .style19
        {
            height: 45px;
            width: 88px;
        }
        .style20
        {
            width: 88px;
        }
        .style21
        {
            width: 159px;
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
                    Inventory Challan Create
                    <asp:HiddenField ID="printflag" runat="server"></asp:HiddenField>
                </div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td class="style4" align="left">
                        Invoice No: <span class="alert">* </span>
                    </td>
                    <td align="left" class="style20">
                        <%--<asp:TextBox ID="textboxDispatchCode" runat="server" ReadOnly="true" class="form_textbox"></asp:TextBox>--%><%--<asp:DropDownList ID="ddlDispatchcode" runat="server" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlDispatchcode_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <asp:TextBox ID="txtinvoiceno" runat="server"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td class="form_text" align="left">
                        Client: <span class="alert">* </span>
                    </td>

                    <td align="left" class="style15">
                      
                        <asp:DropDownList ID="ddlclient" runat="server" Height="36px" Width="169px" 
                            DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                        </asp:DropDownList>
                        <%-- <asp:TextBox ID="txtReceiveCode" runat="server" class="form_textbox"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                <td class="style4">Product Name: <span class="alert">* </span></td>
                <td class="style20">

                    <asp:DropDownList ID="ddlproductname" runat="server" Height="24px" 
                        style="margin-left: 0px" Width="130px">
                        <asp:ListItem>--Select Product Name--</asp:ListItem>
                        <asp:ListItem>Plate</asp:ListItem>
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style11" align="left">
                        Product Size: <span class="alert">* </span>
                    </td>
                    <td align="left" class="style18">
                        <asp:DropDownList ID="ddlproductsize" runat="server" Height="37px" 
                            Width="174px">
                            <asp:ListItem>--Select Product Size--</asp:ListItem>
                            <asp:ListItem>285X45</asp:ListItem>
                            <asp:ListItem>200X100</asp:ListItem>
                            <asp:ListItem>500X120</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txtStateName" runat="server" class="form_textbox" Enabled="false"></asp:TextBox>--%>
                    </td>
                    <td class="style13">
                        &nbsp;
                    </td>
                    <td class="style14" align="left">
                        Product Color:<span class="alert">* </span>
                    </td>
                    <td align="left" class="style16">
                        <asp:DropDownList ID="ddlproductcolor" runat="server" Height="22px" 
                            Width="167px">
                            <asp:ListItem>--Select Product Color--</asp:ListItem>
                            <asp:ListItem>WHITE</asp:ListItem>
                            <asp:ListItem>YELLOW</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txtRtoName" runat="server" class="form_textbox"></asp:TextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td class="style7" align="left">
                        Prefix Name:<span class="alert">* </span>
                    </td>
                    <td align="left" class="style19">
                        <%--<asp:TextBox ID="txtAddress" runat="server" TabIndex="6" TextMode="MultiLine" Columns="30"
                            Rows="4"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlprefixname" runat="server" Height="34px" Width="170px">
                            <asp:ListItem>--Select Prefix Name--</asp:ListItem>
                            <asp:ListItem>AA</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="style9">
                    </td>
                    <td class="style10" align="left">
                        Quantity:<span class="alert">* </span>
                    </td>
                    <td align="left" class="style17">
                        <%--<asp:TextBox ID="textboxRemark" runat="server" TabIndex="6" TextMode="MultiLine"
                            Columns="30" Rows="4"></asp:TextBox>--%>
                        <asp:TextBox ID="txtquantity" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style7" align="left">
                        Laser Code From:<span class="alert">* </span></td>
                    <td align="left" class="style19">
                        <asp:TextBox ID="txtlasercodefrom" runat="server"></asp:TextBox>
                    </td>
                    <td class="style9">
                        &nbsp;</td>
                    <td class="style10" align="left">
                        Laser Code To: <span class="alert">* </span></td>
                    <td align="left" class="style17">
                        <asp:TextBox ID="txtlasercodeto" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style7" align="left">
                        Lasered:</td>
                    <td align="left" class="style19">
                        <asp:Panel ID="Panel1" runat="server" Height="33px" Width="173px">
                            <asp:RadioButton ID="OptYes" runat="server" 
                                oncheckedchanged="OptYes_CheckedChanged" Text="Yes" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            </asp:Panel>
                    </td>
                    <td class="style9">
                        &nbsp;</td>
                    <td class="style10" align="left">
                        Rate(Unit):</td>
                    <td align="left" class="style17">
                        <asp:TextBox ID="txtrate" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style7" align="left">
                        &nbsp;</td>
                    <td align="left" class="style19">
                        &nbsp;</td>
                    <td class="style9">
                        &nbsp;</td>
                    <td class="style10" align="left">
                        &nbsp;</td>
                    <td align="left" class="style17">
                        <asp:Button ID="btnadd" runat="server" Height="25px" onclick="btnadd_Click" 
                            Text="Add" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td class="style21">
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
                                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
                                        GridLines="None" Width="100%">
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
                    <td class="style21">
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
