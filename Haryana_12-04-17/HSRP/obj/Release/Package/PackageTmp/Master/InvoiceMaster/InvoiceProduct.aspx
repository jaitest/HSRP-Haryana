<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceProduct.aspx.cs" Inherits="HSRP.Master.InvoiceMaster.InvoiceProduct" %>

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

        if (document.getElementById("<%=txtProductName.ClientID%>").value == "") {
            alert("Please Provide Product Name ");
            document.getElementById("<%=txtProductName.ClientID%>").focus();
            return false;
        }



        if (document.getElementById("<%=txtProductCode.ClientID%>").value == "") {
            alert("Please Provide Product Code ");
            document.getElementById("<%=txtProductCode.ClientID%>").focus();
            return false;
        }



        if (document.getElementById("<%=txtProductColor.ClientID%>").value == "") {
            alert("Please Provide Product Color ");
            document.getElementById("<%=txtProductColor.ClientID%>").focus();
            return false;
        }

        if (document.getElementById("<%=txtProductCost.ClientID%>").value == "") {
            alert("Please Provide Product Cost ");
            document.getElementById("<%=txtProductCost.ClientID%>").focus();
            return false;
        }

        if (document.getElementById("<%=txtMeasurementUnit.ClientID%>").value == "") {
            alert("Please Provide Measurement Unit");
            document.getElementById("<%=txtMeasurementUnit.ClientID%>").focus();
            return false;
        }



        if (invalidChar(document.getElementById("txtProductName"))) {
            alert("Your can't enter special characters in Plant Address. \nThese are not allowed.\n Please remove them.");
            document.getElementById("txtProductName").focus();
            return false;
        }

        if (invalidChar(document.getElementById("txtProductCode"))) {
            alert("Your can't enter special characters in Plant Zip. \nThese are not allowed.\n Please remove them.");
            document.getElementById("txtProductCode").focus();
            return false;
        }

        if (invalidChar(document.getElementById("txtProductColor"))) {
            alert("Your can't enter special characters in contact Person Name. \nThese are not allowed.\n Please remove them.");
            document.getElementById("txtProductColor").focus();
            return false;
        }

        if (invalidChar(document.getElementById("txtProductCost"))) {
            alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
            document.getElementById("txtProductCost").focus();
            return false;
        }

        if (invalidChar(document.getElementById("txtMeasurementUnit"))) {
            alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
            document.getElementById("txtMeasurementUnit").focus();
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
   <%-- <link href="../css/RegSheet.css" rel="stylesheet" type="text/css" />--%>
      <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../javascript/common.js" type="text/javascript"></script>
</head>


<body>
    <form id="form2" runat="server">
   <div style=" width:100%; margin:0px auto 0px auto;" >
  <fieldset  style="background-color:#faf7f7">
            <legend>
                <div style="margin-left: 10px; font-size:medium;color:Black" >
                    Invoice Product</div>
            </legend>
            <table style=" width:100%; height:50px; background-color:#4c4d5c  " id="tblShow" runat="server" visible="false">
            <tr>
            <td colspan="4" align="center">
                <asp:Label ID="lblMessageBox" runat="server" Text="" ForeColor="#09f02c" Font-Bold="true" Font-Size="20px"></asp:Label> <asp:Label ID="lblErrorMessageBox" runat="server" Text="" ForeColor="#f0091d" Font-Bold="true" Font-Size="20px"></asp:Label></td>
            </tr>
            </table>
<table style="width:100%" align="center" style=" padding-top:20px;">


<tr>
<td style=" width:150px" class ="Label_user">Product Name<span style=" color:Red">*</span></td>
<td >
    <asp:TextBox ID="txtProductName" runat="server" class="form_textbox" ></asp:TextBox>
</td>

<td style=" width:150px" class ="Label_user">Product Code<span style=" color:Red">*</span></td>
<td>
<asp:TextBox ID="txtProductCode" runat="server" class="form_textbox"></asp:TextBox>
</td>
</tr>
<tr>
<td class ="Label_user">Product Color<span style=" color:Red">*</span></td>
<td>
<asp:TextBox ID="txtProductColor" runat="server" class="form_textbox"></asp:TextBox>
</td>

<td class ="Label_user">Product Cost<span style=" color:Red">*</span></td>
<td>
<asp:TextBox ID="txtProductCost" runat="server" class="form_textbox"></asp:TextBox>
</td>
</tr>
<tr>
<td class ="Label_user">Measurement Unit<span style=" color:Red">*</span></td>
<td>
<asp:TextBox ID="txtMeasurementUnit" runat="server" class="form_textbox"></asp:TextBox>
</td>

<td class ="Label_user">Product description</td>
<td>
<asp:TextBox ID="txtProductDiscription" runat="server" class="form_textbox"></asp:TextBox>
</td>
</tr>

<tr>
<td class ="Label_user">Active Status</td>
<td>
    <asp:CheckBox ID="chkActive" runat="server" Text="" class="tb7" />
</td>
</tr>

<tr>
<td colspan="4" align="right" style="Padding-top:30px;">
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="button" OnClientClick="return validate()"
        onclick="btnSubmit_Click" /> &nbsp;
        <asp:Button ID="btnUpdate" runat="server" Text="Update" class="button" OnClientClick="return validate()"
        onclick="btnUpdate_Click" />&nbsp;
    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" class="button" 
        onclick="btnRefresh_Click" />&nbsp;
        <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
    <asp:HiddenField ID="ProductID" runat="server" />
</td>
</tr>

</table>


</fieldset>
</div>
    </form>
</body>
</html>
