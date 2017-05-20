<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="GoodsDispatchRegister1.aspx.cs" Inherits="HSRP.Transaction.GoodsDispatchRegister1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <link href="../css/gridStyle.css" rel="stylesheet" type="text/css" />


    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .HellowWorldPopup
        {
            min-width:200px;
            min-height:150px;
            background:white;
        }
   </style>

    <script type="text/javascript">


        function OrderDate_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDate.getSelectedDate();
            CalendarOrderDate.setSelectedDate(fromDate);

        }

        function OrderDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDate.getSelectedDate();
            OrderDate.setSelectedDate(fromDate);

        }

        function OrderDate_OnClick() {
            if (CalendarOrderDate.get_popUpShowing()) {
                CalendarOrderDate.hide();
            }
            else {
                CalendarOrderDate.setSelectedDate(OrderDate.getSelectedDate());
                CalendarOrderDate.show();
            }
        }

        function OrderDate_OnMouseUp() {
            if (CalendarOrderDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }




        function validation() {
            if (document.getElementById('ddlProduct').value == "--Select Product--") {
                alert("Please Select Products");
                document.getElementById('ddlProduct').focus();
                return false;
            }
            else if (document.getElementById('ddlBatchID').value == "--Select Batch--") {
                alert("Please Select Batch");
                document.getElementById('ddlBatchID').focus();
                return false;
            }
            else if (document.getElementById('ddlPrefix').value == "--Select Prefix--") {
                alert("Please Select Prefix");
                document.getElementById('ddlPrefix').focus();
                return false;
            }
            else if (document.getElementById('txtLaserFrom').value == "") {
                alert("Please Fill Laser From");
                document.getElementById('txtLaserFrom').focus();
                return false;
            }
            else if (document.getElementById('txtLaserTo').value == "") {
                alert("Please Fill Laser To");
                document.getElementById('txtLaserTo').focus();
                return false;
            }
            else {
                return true;
            }
        }



        function validate() {

            if (document.getElementById("ddlDispatchType").value == "Select Type") {
                alert("Select Dispatch Type");
                document.getElementById("ddlDispatchType").focus();
                return false;
            }

            if (document.getElementById("dropDownListOrg").value == "--Select State--") {
                alert("Select State");
                document.getElementById("dropDownListOrg").focus();
                return false;
            }
            if (document.getElementById("dropDownListClient").value == "--Select RTO--") {
                alert("Select RTO");
                document.getElementById("dropDownListClient").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    
    <style>
        .grid
        {
            font-family: tahoma;
            font-size: 12px;
            border: solid 1px #7f7f7f;
            border-collapse: collapse;
            color: #333333;
            width: 100%;
            margin-top: 0px;
        }
        .grid th
        {
            border-color: #989898 #cbcbcb #989898 #989898;
            border-style: solid solid solid none;
            border-width: 1px 1px 1px 1px;
            color: Menu;
            padding: 4px 5px 4px 10px;
            vertical-align: bottom;
            text-align: left;
            background-color: Highlight;
        }
        .grid td
        {
            color: #333333;
            padding: 4px 1px 4px 1px;
            border-bottom: solid 1px #BBD9EE;
            padding: 4px 5px 4px 10px;
        }
        .gridRow
        {
            background-color: #B1DEDC;
        }
        
        .gridAltRow
        {
            background-color: #D1EDE9;
        }
        
        .gridEditRow
        {
            background-color: #0DC6DE;
        }
        .gridFooterRow
        {
            background-color: #E8ECED;
        }
        .grid tr.gridRow:hover, .grid tr.gridAltRow:hover
        {
            font-family: tahoma;
            font-size: 12px;
            border: solid 1px #7f7f7f;
            border-collapse: collapse;
            background-color: #99BBE1;
        }
        
        .hCursor
        {
            cursor: pointer;
            cursor: hand;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Goods Dispatch Register
            </div>
            </legend>
            <br />
            <table width="100%" style="background-color: #FFFFFF" border="0" align="left" cellpadding="3" cellspacing="1">
            <tr>
                        <td colspan="5">
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                                <td colspan="6">
                                                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                                        <tr valign="top">
                                                            <td colspan="0" align="left" style="margin-left: 50px" width="258px" >
                                                                <b>AUTHORZIATION INFORMATION</b>
                                                            </td>
                                                            <td nowrap="nowrap" align="left" width="150px">
                                                                Record Created By:
                                                            </td>
                                                            <td width="170px">
                                                                <asp:Label ID="LabelCreatedID" ForeColor="Blue" runat="server" />
                                                            </td>
                                                            <td nowrap="nowrap" align="left" width="160px">
                                                                Record Creation Date:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                </table>
                       </td>
                       </tr>
                       <tr>
                    
                    <td align="left" class="form_text">
                        Dispatch Type: <span class="alert">* </span>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDispatchType" runat="server" Width="158px" TabIndex="1">
                            <asp:ListItem>Select Type</asp:ListItem>
                            <asp:ListItem Value="Blank Plate">Blank Plate</asp:ListItem>
                            <asp:ListItem Value="Lasered Plate">Lasered Plate</asp:ListItem>
                       <%--     <asp:ListItem Value="Embossed Plate">Embossed Plate</asp:ListItem>--%>
                            <%--<asp:ListItem Value="Damaged Plate">Damaged Plate</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td align="left" class="form_text">
                        Transport
                        Vehicle No.:<span class="alert">* </span>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtVehRegNo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" class="form_text">
                        Dispatch State: <span class="alert">* </span>
                    </td>
                    <td align="left">
                    
                        <asp:DropDownList AutoPostBack="true" ID="dropDownListOrg" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" Width="180px"
                         onselectedindexchanged="dropDownListOrg_SelectedIndexChanged">
                                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td align="left" class="form_text">
                        Dispatch To Location:<span class="alert">* </span>
                    </td>
                    <td align="left">
                         <asp:DropDownList ID="dropDownListClient" runat="server" AutoPostBack="true"
                             DataTextField="RTOLocationName" DataValueField="RTOLocationID" Width="180px"
                             onselectedindexchanged="dropDownListClient_SelectedIndexChanged">
                                                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                 <td align="left" class="form_text">
                        Dispatch From Location:<span class="alert">* </span>
                    </td>
                    <td align="left">
                         <asp:DropDownList ID="ddlFromlocation" runat="server" AutoPostBack="true"
                             DataTextField="RTOLocationName" DataValueField="RTOLocationID" Width="180px"
                             onselectedindexchanged="dropDownListClient_SelectedIndexChanged">
                                                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td align="left" valign="top" class="form_text">Shipping Address</td>
                <td align="left">
                        <asp:TextBox ID="txtAddress" runat="server" TabIndex="6" TextMode="MultiLine"
                            Columns="20" Rows="3"></asp:TextBox>
                    </td>
                    <td></td>
                <td align="left" valign="top" class="form_text">
                        Remarks
                    </td>
                    <td align="left">
                        <asp:TextBox ID="textboxRemark" runat="server" TabIndex="6" TextMode="MultiLine"
                            Columns="20" Rows="3"></asp:TextBox>
                    </td>
                
                    
                </tr>
                <tr>
                    <td align="left" class="form_text">Invoice No.</td>
                    <td align="left">
                        <asp:DropDownList AutoPostBack="true" ID="dropDownInvoiceNo" runat="server" Width="120px"
                            DataTextField="InvoiceNo" DataValueField="HeaderID" 
                            onselectedindexchanged="dropDownInvoiceNo_SelectedIndexChanged" >
                                        </asp:DropDownList>
                    </td>
                    <td align="left">&nbsp;</td>
                   <td class="form_text" nowrap="nowrap" align="left"> Invoice Date :<span class="alert">* </span> </td>
                                                <td>
                                                    <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                                                        cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" onmouseup="OrderDate_OnMouseUp()">
                                                                
                                                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                            ControlType="Picker" PickerCssClass="picker">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>
                                                                   
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                </tr>

                

                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                
                <tr>
                            <td colspan="5">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
    
      </table>
                            </td>
                        </tr>
                <tr>
                <td colspan="5">

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
                                            <asp:CheckBox ID="CHKSelect1"  runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CHKSelect" Checked="true" runat="server" />
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField>
                                     <ItemTemplate>
                                         <asp:Label ID="id" runat="server" Text='<%#Eval("id") %>' Visible="false"></asp:Label>
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
                                           <%-- <asp:BoundField DataField="manualquantity2" HeaderText="Quantity" SortExpression="Quantity" />--%>
                                              <asp:TemplateField HeaderText="Quantity">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblquentity" runat="server" Text='<%#Eval("manualquantity2") %>'></asp:Label>
                                                 </ItemTemplate>
                                             </asp:TemplateField>

                                               <asp:TemplateField HeaderText="Pre Quantity">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblPreQuantity" runat="server" Text='<%#Eval("manualquantity1") %>'></asp:Label>
                                                 </ItemTemplate>
                                             </asp:TemplateField>

                                             <asp:TemplateField HeaderText="Manual Quantity">
                                                 <ItemTemplate>
                                                     <asp:TextBox ID="TextBox1" Enabled="false" runat="server" Text='<%#Eval("manualquantity2") %>'></asp:TextBox>
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                            <asp:BoundField HeaderText="Rate"  ReadOnly="false" DataField="Rate" 
                                                 SortExpression="Rate" />
                                        </Columns>
                                    </asp:GridView>
                <%--<asp:GridView ID="gvEG" runat="server" AutoGenerateColumns="False" CssClass="grid"
                AlternatingRowStyle-CssClass="gridAltRow" RowStyle-CssClass="gridRow" ShowFooter="True"
                EditRowStyle-CssClass="gridEditRow" FooterStyle-CssClass="gridFooterRow" OnRowCancelingEdit="gvEG_RowCancelingEdit"
                OnRowCommand="gvEG_RowCommand" OnRowDataBound="gvEG_RowDataBound" OnRowDeleting="gvEG_RowDeleting"
                OnRowEditing="gvEG_RowEditing" OnRowUpdating="gvEG_RowUpdating" 
                        onselectedindexchanged="gvEG_SelectedIndexChanged">
                <Columns>


                    <asp:TemplateField HeaderText="Product Name" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlProductName" DataTextField="ProductCode" DataValueField="ProductID" runat="server" Width="220px" AutoPostBack="true" OnSelectedIndexChanged="ddlProductName_SelectedIndexChanged" >
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Update"
                                runat="server" ControlToValidate="ddlProductName" ErrorMessage="Please Product Name"
                                ToolTip="Please Enter Product Name" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <%# Eval("ProductName")%>
                        </ItemTemplate> 
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlProductName" DataTextField="ProductCode" DataValueField="ProductID" runat="server" Width="220px" AutoPostBack="true" OnSelectedIndexChanged="ddlProductName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Insert"
                                runat="server" ControlToValidate="ddlProductName" ErrorMessage="Please Product Name"
                                ToolTip="Please Enter Product Name" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="50px">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" runat="server" onkeydown="return isNumberKey(event);"
                                Text='<%# Bind("Quantity") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Update"
                                runat="server" ControlToValidate="txtQuantity" ErrorMessage="Please Enter Quantity"
                                ToolTip="Please Enter Quantity" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtQuantity" runat="server" onkeydown="return isNumberKey(event);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Insert"
                                runat="server" ControlToValidate="txtQuantity" ErrorMessage="Please Enter Quantity"
                                ToolTip="Please Enter Quantity" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemTemplate>
                            <%# Eval("Quantity")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     
                    
                    <asp:TemplateField HeaderText="Prefix" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlPrefix" DataTextField="Prefix" DataValueField="PrefixID" runat="server" Width="120px" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Update"
                                runat="server" ControlToValidate="ddlPrefix" ErrorMessage="Select Prefix"
                                ToolTip="Please Select Prefix" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                         <ItemTemplate>
                            <%# Eval("Prefix")%>
                        </ItemTemplate> 
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlPrefix" DataTextField="Prefix" DataValueField="PrefixID"  runat="server" Width="120px" AutoPostBack="true" >
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="Insert"
                                runat="server" ControlToValidate="ddlPrefix" ErrorMessage="Please Select Prefix"
                                ToolTip="Select Prefix" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    
                    <asp:TemplateField HeaderText="Laser Noumber From" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="50px">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLaserNoFrom" runat="server" onkeydown="return isNumberKey(event);"
                                Text='<%# Bind("LaserFrom") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Update"
                                runat="server" ControlToValidate="txtLaserNoFrom" ErrorMessage="Please Enter Laser Number"
                                ToolTip="Please Enter Laser Number" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtLaserNoFrom" runat="server" onkeydown="return isNumberKey(event);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Insert"
                                runat="server" ControlToValidate="txtLaserNoFrom" ErrorMessage="Please Enter Quantity"
                                ToolTip="Please Enter Laser Number" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemTemplate>
                            <%# Eval("LaserFrom")%>
                        </ItemTemplate> 
                    </asp:TemplateField>
                     
                     
                    <asp:TemplateField HeaderText="Laser Noumber To" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="50px">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLaserNoTo" runat="server" onkeydown="return isNumberKey(event);"
                                Text='<%# Bind("LaserTo") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="Update"
                                runat="server" ControlToValidate="txtLaserNoFrom" ErrorMessage="Please Enter Laser Number"
                                ToolTip="Please Enter Laser Number" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtLaserNoTo" runat="server" onkeydown="return isNumberKey(event);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="Insert"
                                runat="server" ControlToValidate="txtLaserNoTo" ErrorMessage="Please Enter Quantity"
                                ToolTip="Please Enter Laser Number" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemTemplate>
                            <%# Eval("LaserTo")%>
                        </ItemTemplate> 
                    </asp:TemplateField>
                     


                    <asp:TemplateField HeaderText="Edit" ShowHeader="False" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                Text="Update" OnClientClick="return confirm('Update?')" ValidationGroup="Update"></asp:LinkButton>
                            <asp:ValidationSummary ID="vsUpdate" runat="server" ShowMessageBox="true" ShowSummary="false"
                                ValidationGroup="Update" Enabled="true" HeaderText="Validation Summary..." />
                            <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="True" CommandName="Insert"
                                ValidationGroup="Insert" Text="Insert"></asp:LinkButton>
                            <asp:ValidationSummary ID="vsInsert" runat="server" ShowMessageBox="true" ShowSummary="false"
                                ValidationGroup="Insert" Enabled="true" HeaderText="Validation..." />
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true"
                                ShowSummary="false" ValidationGroup="Insert" Enabled="true" HeaderText="Validation..." />
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                Text="Edit"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Delete" ShowHeader="False" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                Text="Delete" OnClientClick="return confirm('Delete?')"></asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>


                </Columns>
                <EmptyDataTemplate>
                    <table class="grid" cellspacing="0" rules="all" border="1" id="gvEG" style="border-collapse: collapse;">
                        <tr>
                            <th align="left" scope="col">
                                Product Name
                            </th>
                            <th align="left" scope="col">
                                Quantity
                            </th>
                            <th align="left" scope="col">
                                Rate
                            </th>
                            <th align="left" scope="col">
                                Mesurement Unit
                            </th>
                            <th align="left" scope="col">
                                Edit
                            </th>
                            <th scope="col">
                                Delete
                            </th>
                        </tr>
                        <tr class="gridRow">
                            <td colspan="8">
                                No Records found...
                            </td>
                        </tr>
                        <tr class="gridFooterRow">
                            <td align="left" style="margin-left: -20px;">
                                <asp:DropDownList ID="ddlProductName" DataTextField="ProductCode" DataValueField="ProductID" runat="server" Width="220px" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="Insert"
                                    runat="server" ControlToValidate="ddlProductName" ErrorMessage="Please Product Name"
                                    ToolTip="Please Enter Product Name" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server" onkeydown="return isNumberKey(event);"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Insert"
                                    runat="server" ControlToValidate="txtQuantity" ErrorMessage="Please EnterQuantity"
                                    ToolTip="Please Enter Quantity" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanelRate" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtRate" runat="server" onkeydown="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEmployeeCode" ValidationGroup="Insert" runat="server"
                                            ControlToValidate="txtRate" ErrorMessage="Please Enter Quantity" ToolTip="Please Enter Employee Code"
                                            SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlProductName" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtMesurementUnit" runat="server"></asp:TextBox>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlProductName" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2" align="justify" valign="middle">
                                <asp:LinkButton ID="lnkAdd" runat="server" Text="Insert" CommandName="ADD" ValidationGroup="Insert"></asp:LinkButton>
                                <asp:ValidationSummary ID="vsInsert" runat="server" ShowMessageBox="true" ShowSummary="false"
                                    ValidationGroup="Insert" Enabled="true" HeaderText="Validation..." />
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true"
                                    ShowSummary="false" ValidationGroup="Insert" Enabled="true" HeaderText="Validation..." />
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>--%>

          <%--  <ComponentArt:Grid ID="Grid2" ClientIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
                                            Width="100%" GroupingNotificationText="Drag a column to this area to group by it."
                                            LoadingPanelPosition="MiddleCenter" LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                            GroupBySortImageHeight="10" GroupBySortImageWidth="10" GroupBySortDescendingImageUrl="group_desc.gif"
                                            GroupBySortAscendingImageUrl="group_asc.gif" GroupingNotificationTextCssClass="GridHeaderText"
                                            IndentCellWidth="22" TreeLineImageHeight="19" TreeLineImageWidth="22" TreeLineImagesFolderUrl="~/images/lines/"
                                            PagerImagesFolderUrl="~/images/pager/" PreExpandOnGroup="true" GroupingPageSize="5"
                                            SliderPopupClientTemplateId="SliderTemplate" SliderPopupOffsetX="20" SliderGripWidth="9"
                                            SliderWidth="150" SliderHeight="20" PagerButtonHeight="22" PagerButtonWidth="41"
                                            PagerTextCssClass="GridFooterText" PageSize="25" GroupByTextCssClass="GroupByText"
                                            GroupByCssClass="GroupByCell" FooterCssClass="GridFooter" HeaderCssClass="GridHeader"
                                            SearchOnKeyPress="true" SearchTextCssClass="GridHeaderText" ShowSearchBox="true"
                                            ShowHeader="true" CssClass="Grid" RunningMode="Callback" SearchBoxPosition="TopLeft"
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px" >
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="ID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns> 
                                                       
                                                       
                                                        <ComponentArt:GridColumn DataField="ID" Visible="false" HeadingText="Product Code"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="ProductCode" Visible="true" HeadingText="Product Code"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="Quantity" HeadingText="Quantity" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                            
                                                            <ComponentArt:GridColumn DataField="Prefix" HeadingText="Prefix" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="50" />

                                                            <ComponentArt:GridColumn DataField="LaserCodeNoFrom" HeadingText="Laser From" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />

                                                        <ComponentArt:GridColumn DataField="LaserCodeNoTo" HeadingText="Laser To" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" /> 

                                                         
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                              
                                            <ClientTemplates>
                                              
                                                <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate" runat="server">
                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td style="font-size: 10px;">
                                                                Loading...&nbsp;
                                                            </td>
                                                            <td>
                                                                <img alt="loading" src="/Images/spinner.gif" width="16" height="16" border="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="SliderTemplate" runat="server">
                                                    <table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" style="padding: 5px;">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="25" align="center" valign="top" style="padding-top: 3px;">
                                                                        </td>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="2" border="0" style="width: 255px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 115px;">
                                                                                            <b>Code :</b><nobr>## DataItem.GetMember(&#39;ID&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 135px;">
                                                                                            <b>Name :</b><nobr>## DataItem.GetMember(&#39;ID&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            Page <b>## DataItem.PageIndex + 1 ##</b> of <b>## Grid2.PageCount ##</b>
                                                                        </td>
                                                                        <td align="right">
                                                                            Record <b>## DataItem.Index + 1 ##</b> of <b>## Grid2.RecordCount ##</b>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>--%>

                </td>
               </tr>



                    <tr>
        <td width="270" valign="top" align="left">
        
          <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
        
        </td>
        <td valign="top">
            &nbsp;</td>
        <td align="right" valign="top" colspan="3">
            
            <%--<asp:Button ID="btnAddItem" runat="server" class="button" onclick="btnAddItem_Click" Text="Add Dispatch Item" />--%>
            &nbsp;
             <asp:Button ID="btnRecord" class="button" runat="server" 
                            Text="Save" onclick="btnRecord_Click" OnClientClick="return validate()" />
                            &nbsp;&nbsp;
                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" />
        </td>
      </tr>

                <tr>
                    
                    <td>
                 <div id="ModalID" visible="false" runat="server">
   <%-- <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                         TargetControlID="LinkButton11"
                         PopupControlID="Panel2"
                         BackgroundCssClass="ModalPopupBG"
                         DropShadow="true">
                      </asp:ModalPopupExtender>--%>

	<%--<div class="HellowWorldPopup">
                <asp:LinkButton ID="LinkButton11" runat="server"></asp:LinkButton> 
                <asp:Panel ID="Panel2" runat="server" CssClass="HellowWorldPopup"  Style="display: none; background-color:#242831" Font-Names="@Arial">
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate> 
                    <table>
                    <tr>
                    <td colspan="2">Add Dispatch Item Here...</td>
                    </tr>
                    <tr>
                    <td>Product Code : </td>
                    <td>
                        <asp:DropDownList AutoPostBack="true" ID="ddlProduct" runat="server" DataTextField="ProductCode" DataValueField="ProductID">
                                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                    <td>Batch : </td>
                    <td>
                        <asp:DropDownList AutoPostBack="true" ID="ddlBatchID" runat="server" DataTextField="BatchID" DataValueField="BatchID"
                        onselectedindexchanged="ddlBatchID_SelectedIndexChanged">
                                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                    <td>Prefix : </td>
                    <td>
                        <asp:DropDownList AutoPostBack="true" ID="ddlPrefix" runat="server" DataTextField="prefix" DataValueField="BatchID"
                        onselectedindexchanged="ddlPrefix_SelectedIndexChanged">
                                        </asp:DropDownList>
                    </td>
                    </tr>
                    <tr>
                    <td>Stock Quantity : </td>
                    <td>
                        <asp:Label ID="lblStockQty" runat="server"></asp:Label>
                    </td>
                    </tr>
                    <tr>
                    <td>LaserCode From : </td>
                    <td>
                        <asp:TextBox ID="txtLaserFrom" runat="server" Font-Names="ariel"></asp:TextBox>
        
                    </td>
                    </tr>
                    <tr>
                    <td>LaserCode To</td>
                    <td>
                        <asp:TextBox ID="txtLaserTo" runat="server" Font-Names="ariel"></asp:TextBox> 
                    </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblSuccessMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="YellowGreen" />
                        </td>
                    </tr>
                    <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnPutRemarks" runat="server" Text="Save Item" OnClientClick="return validation()" OnClick="btnPutRemarks_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click"/>
                    </td>
                    </tr>
                    </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
        </asp:Panel>
        </div>--%>
        </div>
                      
                    </td>
                </tr>
                <tr>
                    <td colspan="5" align="left">
                        <asp:Label ID="lblSucMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Blue" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="right">
                       
                       
                    </td>
                </tr>
                 <tr>
                                                <td colspan="6" align="left">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                                        NextImageUrl="cal_nextMonth.gif" onload="CalendarOrderDate_Load">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
            </table>
            <table>
        <tr>
            <td>
                <asp:HiddenField ID="HStatus" runat="server" />
                <asp:HiddenField ID="HInvoiceID" runat="server" />
                <asp:HiddenField ID="H_HeaderID" runat="server" />
                <asp:HiddenField ID="H_POheaderID1" runat="server" />
                <asp:HiddenField ID="H_ID" runat="server" />
            </td>
        </tr>
    </table>
                    </td>
                </tr>
                </table>
                    
        </fieldset>
        <br />
    </div>
    </form>
</body>
</html>
