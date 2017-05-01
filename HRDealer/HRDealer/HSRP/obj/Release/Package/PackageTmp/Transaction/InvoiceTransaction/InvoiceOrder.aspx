<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceOrder.aspx.cs" Inherits="HSRP.Master.InvoiceMaster.InvoiceOrder" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script language="javascript" type="text/javascript">
     function validate() {
        if (document.getElementById("<%=ddlCustomerName.ClientID%>").value == "0") {
             alert("Please Provide Customer Name ");
             document.getElementById("<%=ddlCustomerName.ClientID%>").focus();
             return false;
         }
         

          if (document.getElementById("<%=txtTransportName.ClientID%>").value == "") {
             alert("Please Provide Transport Name ");
             document.getElementById("<%=txtTransportName.ClientID%>").focus();
             return false;
         }


         if (document.getElementById("<%=txtGoodsReceiptNote.ClientID%>").value == "") {
                     alert("Please Provide Goods Receipt Note ");
                     document.getElementById("<%=txtGoodsReceiptNote.ClientID%>").focus();
                     return false;
                 }




             if (document.getElementById("<%=ddlTransportVia.ClientID%>").value == "0") {
             alert("Please Provide Transport Via ");
             document.getElementById("<%=ddlTransportVia.ClientID%>").focus();
             return false;
         }
        

         if (document.getElementById("<%=txtBillingAddress.ClientID%>").value == "") {
             alert("Please Provide Billing Address ");
             document.getElementById("<%=txtBillingAddress.ClientID%>").focus();
             return false;
         }



         if (document.getElementById("<%=txtShippingAddress.ClientID%>").value == "") {
             alert("Please Provide Shipping Address ");
             document.getElementById("<%=txtShippingAddress.ClientID%>").focus();
             return false;
         }
        
        
       
      
   
        

       
        
        

         if (invalidChar(document.getElementById("ddlCustomerName"))) {
             alert("Your can't enter special characters in Plant Address. \nThese are not allowed.\n Please remove them.");
             document.getElementById("ddlCustomerName").focus();
             return false;
         }


         if (invalidChar(document.getElementById("txtPaymentTerm"))) {
             alert("Your can't enter special characters in contact Person Name. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtPaymentTerm").focus();
             return false;
         }

        
         var emailID = document.getElementById("txtEmailID").value;
         if (emailID != "") {
             if (emailcheck(emailID) == false) {
                 document.getElementById("txtEmailID").value = "";
                 document.getElementById("txtEmailID").focus();
                 return false;
             }
         }


     }

     function isNumberKey(evt) {
         //debugger;
         var charCode = (evt.which) ? evt.which : event.keyCode
         if (charCode > 31 && (charCode < 48 || charCode > 57))
             return false;

         return true;
     }
   </script>

<script type="text/javascript">

    function SendCustomerDetail(CustomerID1,TransporterName1,GoodsReceiptNote1,TransportVia1) { // Define This function of Send Assign Laser ID
        alert(CustomerID1);
        alert(TransporterName1);
        alert(GoodsReceiptNote1);
        alert(TransportVia1);
        googlewin = dhtmlwindow.open("googlebox", "iframe", "InvoiceOrder.aspx?SendCustomerDetail=SendCustomerDetail&CustomerID=" + CustomerID1 + "&TransporterName=" + TransporterName + "&GoodsReceiptNote=" + GoodsReceiptNote1 + "&TransportVia=" + TransportVia1, "Add Product Detail", "width=950px,height=300px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
            window.location.href = "ViewInvoiceOrder.aspx";
            return true;
        }
    }

    function edit(i) { // Define This function of Send Assign Laser ID 

        googlewin = dhtmlwindow.open("googlebox", "iframe", "InvoiceOrder.aspx?Mode=Edit&ProductID=" + i, "Update Invoice Product", "width=950px,height=300px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
            window.location.href = "ViewInvoiceOrder.aspx";
            return true;
        }
    }

    function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

        googlewin = dhtmlwindow.open("googlebox", "iframe", "InvoiceOrder.aspx?Mode=New", "Add New Invoice Product", "width=950px,height=300px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
            window.location = 'ViewInvoiceOrder.aspx';
            return true;
        }
    }
    </script>
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

        ////>>>>>> Pollution Due Date

        function HSRPAuthDate_OnDateChange(sender, eventArgs) {
            var fromDate = HSRPAuthDate.getSelectedDate();
            CalendarHSRPAuthDate.setSelectedDate(fromDate);

        }

        function CalendarHSRPAuthDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarHSRPAuthDate.getSelectedDate();
            HSRPAuthDate.setSelectedDate(fromDate);

        }

        function HSRPAuthDate_OnClick() {
            if (CalendarHSRPAuthDate.get_popUpShowing()) {
                CalendarHSRPAuthDate.hide();
            }
            else {
                CalendarHSRPAuthDate.setSelectedDate(HSRPAuthDate.getSelectedDate());
                CalendarHSRPAuthDate.show();
            }
        }

        function HSRPAuthDate_OnMouseUp() {
            if (CalendarHSRPAuthDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
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
    <title></title>
    <link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />
    <script src="../../windowfiles/dhtmlwindow.js" type="text/javascript"></script>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <script src="../../windowfiles/dhtmlwindow.js" type="text/javascript"></script>
    <link href="../../windowfiles/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) {

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
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View Invoice </span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <table align="center" style=" padding-top:20px; width:100%;">
                             <tr>
                             <td colspan="4" align="center">
                                <table style=" width:100%; height:50px; background-color:#4c4d5c  " id="tblShow" runat="server" visible="false" >
                            <tr>
                            <td colspan="4" align="center">
                                <asp:Label ID="lblMessageBox" runat="server" Text="" ForeColor="#09f02c" Font-Bold="true" Font-Size="20px"></asp:Label> <asp:Label ID="lblErrorMessageBox" runat="server" Text="" ForeColor="#f0091d" Font-Bold="true" Font-Size="20px"></asp:Label></td>
                            </tr>
                            </table>
                             </td>
                             </tr>

                            <tr>
                            <td style=" width:150px" class ="Label_user">Customer Name<span style=" color:Red">*</span></td>
                            <td >
                               
                                <asp:DropDownList ID="ddlCustomerName" class="form_textbox" Width="220px" 
                                    runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="ddlCustomerName_SelectedIndexChanged1">
                                  </asp:DropDownList>
                            </td>

                            <td style=" width:150px" class ="Label_user">Transport Name<span style=" color:Red">*</span></td>
                            <td>
                           <asp:TextBox ID="txtTransportName" runat="server" class="form_textbox"></asp:TextBox>
                             
                            </td>
                            </tr>
                            <tr>
                            <td class ="Label_user">Goods Receipt Note<span style=" color:Red">*</span></td>
                            <td>
                            <asp:TextBox ID="txtGoodsReceiptNote" runat="server" class="form_textbox"></asp:TextBox>
                            </td>

                            <td class ="Label_user">Transport Via<span style=" color:Red">*</span></td>
                            <td>
                             <asp:DropDownList ID="ddlTransportVia" class="form_textbox" Width="220px" runat="server" >
                                  <asp:ListItem Value="0">--Select Transport Type--</asp:ListItem>
                                  <asp:ListItem Value="1">Logistics</asp:ListItem>
                                  <asp:ListItem Value="2">Bontreger</asp:ListItem>
                                  <asp:ListItem Value="3">Courier</asp:ListItem>
                            
                                  </asp:DropDownList>
                          <%--  <asp:TextBox ID="txtTransportVia" runat="server" class="form_textbox"></asp:TextBox>--%>
                            </td>
                            </tr>

                           <tr>
                            <td class ="Label_user">Billing Address<span style=" color:Red">*</span></td>
                            <td>
                           <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                              <ContentTemplate>--%>
                            <asp:TextBox ID="txtBillingAddress" runat="server" class="form_textbox" Height="100px" TextMode="MultiLine"></asp:TextBox>
                          <%--  </ContentTemplate>
                           <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="ddlCustomerName" EventName="Click" />
                             <asp:PostBackTrigger ControlID="ddlCustomerName" />
                             </Triggers>
                              </asp:UpdatePanel>--%>
                              
                            </td>

                            <td class ="Label_user">Shipping Address<span style=" color:Red">*</span></td>
                            <td>

                           <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                              <ContentTemplate>--%>
                           <asp:TextBox ID="txtShippingAddress" runat="server" class="form_textbox" Height="100px" TextMode="MultiLine"></asp:TextBox>
                         <%--   </ContentTemplate>
                           <Triggers>--%>
                          <%--  <asp:AsyncPostBackTrigger ControlID="ddlCustomerName" EventName="Click" />--%>
                             <%--<asp:PostBackTrigger ControlID="ddlCustomerName" />

                             </Triggers>
                              </asp:UpdatePanel>--%>
                            
                            </td>
                            </tr>
                            <tr>
                            <td class ="Label_user">Active Status</td>
                            <td>
                                  <asp:CheckBox ID="chkActive" runat="server" Text="" class="tb7" />
                            </td>
                            </tr>
                            <tr>
                            <td colspan="4" align="right">
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="35" align="right" valign="middle">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update Inline Data" CssClass="button"  OnClientClick="return validate()"
                                OnClick="btnUpdate_Click1"  />
                            <asp:Button ID="btnShowPopup" runat="server" Text="Add Inline Data" CssClass="button" OnClientClick="return validate()"
                                OnClick="btnShowPopup_Click"   />
                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                id="button1" value="Close" class="button" />
                            &nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                            </td>
                            </tr>
               </table>
                        </td>
                    </tr>
        <div id="divGrid" runat="server" visible="false">
            <asp:GridView ID="gvEG" runat="server" AutoGenerateColumns="False" CssClass="grid"
                AlternatingRowStyle-CssClass="gridAltRow" RowStyle-CssClass="gridRow" ShowFooter="True"
                EditRowStyle-CssClass="gridEditRow" FooterStyle-CssClass="gridFooterRow" OnRowCancelingEdit="gvEG_RowCancelingEdit"
                OnRowCommand="gvEG_RowCommand" OnRowDataBound="gvEG_RowDataBound" OnRowDeleting="gvEG_RowDeleting"
                OnRowEditing="gvEG_RowEditing" OnRowUpdating="gvEG_RowUpdating" DataKeyNames="ID">
                <Columns>
                    <asp:TemplateField HeaderText="Product Name" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlProductName" runat="server" Width="220px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlProductName_SelectedIndexChanged1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Update"
                                runat="server" ControlToValidate="ddlProductName" ErrorMessage="Please Product Name"
                                ToolTip="Please Enter Product Name" SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%# Eval("ProductName")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddlProductName" runat="server" Width="220px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlProductName_SelectedIndexChanged1">
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
                    <asp:TemplateField HeaderText="Rate" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRate" runat="server" onkeydown="return isNumberKey(event);" Text='<%# Bind("Rate") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmployeeCode" ValidationGroup="Update" runat="server"
                                ControlToValidate="txtRate" ErrorMessage="Please Enter Rate" ToolTip="Please Enter Employee Code"
                                SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtRate" runat="server" onkeydown="return isNumberKey(event);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmployeeCode" ValidationGroup="Insert" runat="server"
                                ControlToValidate="txtRate" ErrorMessage="Please Enter Rate" ToolTip="Please Enter Employee Code"
                                SetFocusOnError="true" ForeColor="Red">*</asp:RequiredFieldValidator>
                        </FooterTemplate>
                        <ItemTemplate>
                            <%# Eval("Rate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMesurementUnit" runat="server" Text='<%# Bind("MeasurementUnit") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                        <%# Eval("MeasurementUnit")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtMesurementUnit" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                       <asp:TemplateField HeaderText="Laser code From" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLaserCodeNoform" runat="server" Text='<%# Bind("LaserCodeNoFrom") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%# Eval("LaserCodeNoFrom")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtLaserCodeNoform" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>


                       <asp:TemplateField HeaderText="Laser Code No to" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLaserCodeNoto" runat="server" Text='<%# Bind("LaserCodeNoTo") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%# Eval("LaserCodeNoTo")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtLaserCodeNoto" runat="server"></asp:TextBox>
                        </FooterTemplate>
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
                               Laser Code From
                            </th>
                             <th align="left" scope="col">
                               Laser Code No To
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
                                <asp:DropDownList ID="ddlProductName" runat="server" Width="220px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlProductName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Insert"
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

                              <td>
                              
                                        <asp:TextBox ID="txtLaserCodeNoform" runat="server"></asp:TextBox>
                               
                            </td>

                              <td>
                                
                                        <asp:TextBox ID="txtLaserCodeNoto" runat="server"></asp:TextBox>
                               
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
            </asp:GridView>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSavePO" runat="server" Text="Generate Invoice" CssClass="button" OnClick="btnSavePO_Click" />
                </td>
            </tr>
        </div>
    </table>
    <table>
        <tr>
            <td>
                <asp:HiddenField ID="HStatus" runat="server" />
                <asp:HiddenField ID="HInvoiceID" runat="server" />
                <asp:HiddenField ID="H_HeaderID" runat="server" />
                <asp:HiddenField ID="H_headerID1" runat="server" />
                <asp:HiddenField ID="H_ID" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
