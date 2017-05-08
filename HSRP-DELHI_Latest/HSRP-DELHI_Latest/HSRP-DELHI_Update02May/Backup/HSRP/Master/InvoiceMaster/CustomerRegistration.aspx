<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerRegistration.aspx.cs"
    Inherits="HSRP.Master.CustomerRegistration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
 <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../javascript/common.js" type="text/javascript"></script>
     <script src="../../windowfiles/dhtmlwindow.js" type="text/javascript"></script>
    <link href="../../windowfiles/dhtmlwindow.css" rel="stylesheet" type="text/css" />
 <script language="javascript" type="text/javascript">
     function validate() {

       



         if (document.getElementById("<%=txtCustomerName.ClientID%>").value == "") {
             alert("Please Provide Customer Name ");
             document.getElementById("<%=txtCustomerName.ClientID%>").focus();
             return false;
         }



         if (document.getElementById("<%=txtBillingaddress.ClientID%>").value == "") {
             alert("Please Provide Billing Address ");
             document.getElementById("<%=txtBillingaddress.ClientID%>").focus();
             return false;
         }



         if (document.getElementById("<%=txtShippingAddress.ClientID%>").value == "") {
             alert("Please Provide Shipping Address ");
             document.getElementById("<%=txtShippingAddress.ClientID%>").focus();
             return false;
         }

         if (document.getElementById("<%=txtCity.ClientID%>").value == "") {
             alert("Please Provide Contact City ");
             document.getElementById("<%=txtCity.ClientID%>").focus();
             return false;
         }

         if (document.getElementById("<%=txtState.ClientID%>").value == "") {
             alert("Please Provide State");
             document.getElementById("<%=txtState.ClientID%>").focus();
             return false;
         }

         if (document.getElementById("<%=txtCountry.ClientID%>").value == "") {
             alert("Please Provide Country ");
             document.getElementById("<%=txtCountry.ClientID%>").focus();
             return false;
         }

         if (document.getElementById("<%=txtBillingCity.ClientID%>").value == "") {
             alert("Please Provide Contact City ");
             document.getElementById("<%=txtBillingCity.ClientID%>").focus();
             return false;
         }

         if (document.getElementById("<%=txtBillingState.ClientID%>").value == "") {
             alert("Please Provide State");
             document.getElementById("<%=txtBillingState.ClientID%>").focus();
             return false;
         }

         if (document.getElementById("<%=txtBillingCountry.ClientID%>").value == "") {
             alert("Please Provide Country ");
             document.getElementById("<%=txtBillingCountry.ClientID%>").focus();
             return false;
         }

         if (document.getElementById("<%=txtContactPerson.ClientID%>").value == "") {
             alert("Please Provide Contact Person ");
             document.getElementById("<%=txtContactPerson.ClientID%>").focus();
             return false;
         }
         if (document.getElementById("<%=txtMobileNo.ClientID%>").value == "") {
             alert("Please Provide MobileNo ");
             document.getElementById("<%=txtMobileNo.ClientID%>").focus();
             return false;
         }
         if (document.getElementById("<%=txtEmailID.ClientID%>").value == "") {
             alert("Please Provide EmailID ");
             document.getElementById("<%=txtEmailID.ClientID%>").focus();
             return false;
         }
         if (document.getElementById("<%=txtTinNo.ClientID%>").value == "") {
             alert("Please Provide TinNo ");
             document.getElementById("<%=txtTinNo.ClientID%>").focus();
             return false;
         }
         if (document.getElementById("<%=txtVatNo.ClientID%>").value == "") {
             alert("Please Provide VatNo ");
             document.getElementById("<%=txtVatNo.ClientID%>").focus();
             return false;
         }

         if (document.getElementById("<%=txtCST.ClientID%>").value == "") {
             alert("Please Provide CST ");
             document.getElementById("<%=txtCST.ClientID%>").focus();
             return false;
         }


         if (invalidChar(document.getElementById("txtCustomerName"))) {
             alert("Your can't enter special characters in Plant Address. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtCustomerName").focus();
             return false;
         }

         if (invalidChar(document.getElementById("txtBillingaddress"))) {
             alert("Your can't enter special characters in Plant Zip. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtBillingaddress").focus();
             return false;
         }

         if (invalidChar(document.getElementById("txtShippingAddress"))) {
             alert("Your can't enter special characters in contact Person Name. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtShippingAddress").focus();
             return false;
         }

         if (invalidChar(document.getElementById("txtCity"))) {
             alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtCity").focus();
             return false;
         }

         if (invalidChar(document.getElementById("txtState"))) {
             alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtState").focus();
             return false;
         }


         if (invalidChar(document.getElementById("txtContactPerson"))) {
             alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtContactPerson").focus();
             return false;
         }

         if (invalidChar(document.getElementById("txtMobileNo"))) {
             alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtMobileNo").focus();
             return false;
         }
         if (invalidChar(document.getElementById("txtTinNo"))) {
             alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtTinNo").focus();
             return false;
         }

         if (invalidChar(document.getElementById("txtVatNo"))) {
             alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtVatNo").focus();
             return false;
         }
         if (invalidChar(document.getElementById("txtCST"))) {
             alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
             document.getElementById("txtCST").focus();
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
    <title></title>
     <script type="text/javascript">

         function copytext() {
             var chck = document.getElementById('<%=CheckBox1.ClientID %>');
             if (chck.checked) {
                 document.getElementById('<%=txtShippingAddress.ClientID %>').value = document.getElementById('<%=txtBillingaddress.ClientID %>').value;
                 document.getElementById('<%=txtCity.ClientID %>').value = document.getElementById('<%=txtBillingCity.ClientID %>').value;
                 document.getElementById('<%=txtState.ClientID %>').value = document.getElementById('<%=txtBillingState.ClientID %>').value;
                 document.getElementById('<%=txtCountry.ClientID %>').value = document.getElementById('<%=txtBillingCountry.ClientID %>').value;
                 document.getElementById('<%=txtPin.ClientID %>').value = document.getElementById('<%=txtBillingPinNo.ClientID %>').value;
             }
             else {
                 document.getElementById('<%=txtShippingAddress.ClientID %>').value = "";
                 document.getElementById('<%=txtCity.ClientID %>').value = "";
                 document.getElementById('<%=txtState.ClientID %>').value = "";
                 document.getElementById('<%=txtCountry.ClientID %>').value = "";
                 document.getElementById('<%=txtPin.ClientID %>').value = "";

             }
             return true;
         }


     </script>
    

    <%-- <link href="../css/RegSheet.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../javascript/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; margin: 0px auto 0px auto;">
       
<div style="width: 100%; margin: 0px auto 0px auto;">
        <fieldset style="background-color: #faf7f7">
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Customer Registration</div>
            </legend>
            <table style="width: 100%; height: 50px; background-color: #4c4d5c" id="tblShow"
                runat="server" visible="false">
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblMessageBox" runat="server" Text="" ForeColor="#09f02c" Font-Bold="true"
                            Font-Size="20px"></asp:Label>
                        <asp:Label ID="lblErrorMessageBox" runat="server" Text="" ForeColor="#f0091d" Font-Bold="true"
                            Font-Size="20px"></asp:Label>
                    </td>
                </tr>
            </table>
              <fieldset style="background-color: #faf7f7">
          
            <table align="center" style="padding-top: 20px; padding-left: 20px; padding-right: 20px;
                padding-bottom: 20px;" width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td class="Label_user">
                        Customer Name<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerName" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                    <td class="Label_user">
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            </fieldset>

              <fieldset style="background-color: #faf7f7; margin-top:10px;">
        

            <table align="center" style="padding-top: 20px; padding-left: 20px; padding-right: 20px; float:left ;
                padding-bottom: 20px;" width="100%" border="0"  cellpadding="3" cellspacing="1">
                <tr>
                <td colspan="4" align="center">
                                      
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Check IF Same" onclick="return copytext()" ForeColor="#660066" Font-Bold="true" Font-Size="15px" />
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Billing Address<span style="color: Red">*</span>
                    </td>
                    <td >
                        <asp:TextBox ID="txtBillingaddress" runat="server" class="form_textbox" Height="40px" 
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                      <td class="Label_user">
                        Shipping Address<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtShippingAddress" runat="server" class="form_textbox" Height="40px" 
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                    </tr>
                <tr>
                    <td class="Label_user">
                        City<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillingCity" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                      <td class="Label_user">
                        City<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                       </tr>
                <tr>
                    <td class="Label_user">
                        State<span style="color: Red">*</span>
                    </td>
                    <td >
                        <asp:TextBox ID="txtBillingState" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                     <td class="Label_user">
                        State<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtState" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                       </tr>
                <tr>
                    <td class="Label_user">
                        Country<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillingCountry" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                      <td class="Label_user">
                        Country<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCountry" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                       </tr>
                <tr>

                  <td class="Label_user">
                        Pin No
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillingPinNo" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                    <td class="Label_user">
                        Pin No
                    </td>
                     <td>
                        <asp:TextBox ID="txtPin" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                   
                </tr>
                </table>
             

            </fieldset>

              <fieldset style="background-color: #faf7f7; margin-top:10px;">
         
            <table align="center" style="padding-top: 20px; padding-left: 20px; padding-right: 20px;
                padding-bottom: 20px;" width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td class="Label_user">
                        Contact Person<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtContactPerson" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                    <td class="Label_user">
                        Landline No
                    </td>
                    <td>
                        <asp:TextBox ID="txtLandlineNo" runat="server" class="form_textbox" MaxLength="6"
                            onkeydown="return isNumberKey(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Mobile No<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMobileNo" runat="server" class="form_textbox" MaxLength="10"
                            onkeydown="return isNumberKey(event);"></asp:TextBox>
                    </td>
                    <td class="Label_user">
                        Email-ID<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailID" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td class="Label_user">
                        Remark
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemark" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                    <td class="Label_user">
                        Active
                    </td>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Text="" class="form_textbox" />
                    </td>
                </tr>
                 </table>
                 </fieldset>
                   <fieldset style="background-color: #faf7f7; margin-top:10px;">
                <table align="center" style="padding-top: 20px; padding-left: 20px; padding-right: 20px;
                padding-bottom: 20px;" width="100%" border="0" align="left" cellpadding="3" cellspacing="1">

                <tr>
                    <td class="Label_user">
                        TIN No.<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTinNo" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                    <td class="Label_user">
                        VAT No.<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVatNo" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        CST<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCST" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                    <td class="Label_user">
                        Excise No<span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtExciseNo" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                </tr>
               </table>
                </fieldset>
                <table align="center" style="padding-top: 20px; padding-left: 20px; padding-right: 20px;
                padding-bottom: 20px;" width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td colspan="4" align="right" style="padding-top: 30px;">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="button"  OnClientClick="return validate()" OnClick="btnSubmit_Click1" />&nbsp;
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" class="button"  OnClientClick="return validate()" OnClick="btnUpdate_Click1" />&nbsp;
                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" class="button" OnClick="btnRefresh_Click" />&nbsp;
                        <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                            id="buttonClose" value="Close" class="button" />
                        &nbsp;&nbsp;
                      <asp:HiddenField ID="H_CustomerID" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
