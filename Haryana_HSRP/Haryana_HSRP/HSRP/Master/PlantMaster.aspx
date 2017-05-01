<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlantMaster.aspx.cs" Inherits="HSRP.Master.PlantMaster1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script src="../windowfiles/dhtmlwindow.js" type="text/javascript"></script>
    <link href="../windowfiles/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListPlantState.ClientID%>").value == "0") {
                alert("Please Provide State Name ");
                document.getElementById("<%=DropDownListPlantState.ClientID%>").focus();
                return false;
            }

          

            if (document.getElementById("<%=TextBoxPlantAddress.ClientID%>").value == "") {
                alert("Please Provide Plant Address ");
                document.getElementById("<%=TextBoxPlantAddress.ClientID%>").focus();
                return false;
            }



            if (document.getElementById("<%=TextBoxPlantCity.ClientID%>").value == "") {
                alert("Please Provide Plant City ");
                document.getElementById("<%=TextBoxPlantCity.ClientID%>").focus();
                return false;
            }

            

            if (document.getElementById("<%=TextBoxPlantZip.ClientID%>").value == "") {
                alert("Please Provide Plant Zip ");
                document.getElementById("<%=TextBoxPlantZip.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxContactPersonName.ClientID%>").value == "") {
                alert("Please Provide Contact Person Name ");
                document.getElementById("<%=TextBoxContactPersonName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxMobileNo.ClientID%>").value == "") {
                alert("Please Provide Mobile No ");
                document.getElementById("<%=TextBoxMobileNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxEmailId.ClientID%>").value == "") {
                alert("Please Provide Email Id ");
                document.getElementById("<%=TextBoxEmailId.ClientID%>").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxPlantAddress"))) {
                alert("Your can't enter special characters in Plant Address. \nThese are not allowed.\n Please remove them.");
                document.getElementById("TextBoxPlantAddress").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxPlantZip"))) {
                alert("Your can't enter special characters in Plant Zip. \nThese are not allowed.\n Please remove them.");
                document.getElementById("TextBoxPlantZip").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxContactPersonName"))) {
                alert("Your can't enter special characters in contact Person Name. \nThese are not allowed.\n Please remove them.");
                document.getElementById("TextBoxContactPersonName").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxMobileNo"))) {
                alert("Your can't enter special characters in Mobile No. \nThese are not allowed.\n Please remove them.");
                document.getElementById("TextBoxMobileNo").focus();
                return false;
            }

            var emailID = document.getElementById("TextBoxEmailId").value;
            if (emailID != "") {
                if (emailcheck(emailID) == false) {
                    document.getElementById("TextBoxEmailId").value = "";
                    document.getElementById("TextBoxEmailId").focus();
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
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Plant Master</div>
            </legend>
            <table style=" margin-left:150px;">
                <tr>
                    <td class="Label_user">
                        Plant Address <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxPlantAddress" runat="server" class="form_textbox12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Plant State <span class="alert">* </span>
                    </td>
                    <td>
                        <%--<asp:TextBox ID="TextBoxPlantState" runat="server" class="form_textbox12"></asp:TextBox>--%>
                        <asp:DropDownList ID="DropDownListPlantState" runat="server" Style="margin-left: 8px" TabIndex="4" Width="165px">
                            <asp:ListItem>Select</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td class="Label_user">
                        Plant City <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxPlantCity" runat="server" class="form_textbox12"></asp:TextBox>
                        <%--<asp:DropDownList ID="DropDownListPlantCity" runat="server" Style="margin-left: 8px" TabIndex="4" Width="165px">
                            <asp:ListItem>Select</asp:ListItem>
                        </asp:DropDownList>--%>
                    </td>
                </tr>
                
                <tr>
                    <td class="Label_user">
                        Plant Zip <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxPlantZip" runat="server" class="form_textbox12" onkeydown="return isNumberKey(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Contact Person Name <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxContactPersonName" runat="server" class="form_textbox12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Mobile No <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxMobileNo" runat="server" class="form_textbox12" onkeydown="return isNumberKey(event);" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Landline No
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxLandlineNo" runat="server" class="form_textbox12" onkeydown="return isNumberKey(event);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Email Id <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxEmailId" runat="server" class="form_textbox12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Active Status
                    </td>
                    <td>
                        <asp:CheckBox ID="checkBoxActiveStatus" runat="server" TabIndex="1" 
                            TextAlign="Left" />
                    </td>
                </tr>
                <tr>
                <td nowrap="nowrap" align="right" colspan="5" style="margin-right: 200px">
                                                            <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                                            <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                                                            <asp:Button ID="buttonUpdate" runat="server" TabIndex="19" class="button" Visible="false"
                                                                 Text="Update"   OnClientClick="return validate()" onclick="buttonUpdate_Click" 
                                                                 />
                                                            &nbsp;&nbsp;
                                                            <%--OnClientClick="return validate();"--%>
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="18" class="button" Text="Save"
                                                                OnClientClick=" return validate()" Visible="false" OnClick="ButtonSubmit_Click" />&nbsp;&nbsp;
                                                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                                id="buttonClose" value="Close" class="button" />
                                                            &nbsp;&nbsp;
                                                            <input type="reset" class="button" value="Reset" />
                                                        </td>
                
                </tr>
            </table>
        </fieldset>
        <asp:HiddenField ID="H_PlantID1" runat="server" />
    </div>
    </form>
</body>
</html>
