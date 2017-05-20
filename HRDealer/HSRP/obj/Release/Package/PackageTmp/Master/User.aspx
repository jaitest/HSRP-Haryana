<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="HSRP.Master.User" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
    <%--<script src="http://code.jquery.com/jquery-1.4.3.min.js" type="text/javascript"></script>--%>
    <%--    <style type="text/css">
        .focus
        {
            border: 2px solid #D9D9D9;
            background-color: #fbfbfb;
        }
    </style>
       <script language="javascript" type="text/javascript">
           $(document).ready(function () {
               $('input[type="text"]').focus(function () {
                   $(this).addClass("add-focus");
               });
               $('input[type="text"]').blur(function () {
                   $(this).removeClass("add-focus");
               });

           });
  
   </script>--%>

    <script language="javascript" type="text/javascript">

        function checkEmail() {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(textBoxEmail.value)) {
               // document.write("You have entered valid email.");
                return true;
            }
            return false;
        }



        function validate() {

            if (document.getElementById("<%=textBoxFirstName.ClientID%>").value == "") {
                alert("Please Provide First Name");
                document.getElementById("<%=textBoxFirstName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=textBoxCity.ClientID%>").value == "") {
                alert("Please Provide City Name");
                document.getElementById("<%=textBoxCity.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxLastName.ClientID%>").value == "") {
                alert("Please Provide Last Name");
                document.getElementById("<%=textBoxLastName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxState.ClientID%>").value == "") {
                alert("Please Provide State Name");
                document.getElementById("<%=textBoxState.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxLoginName.ClientID%>").value == "") {
                alert("Please Provide Login Name");
                document.getElementById("<%=textBoxLoginName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxZip.ClientID%>").value == "") {
                alert("Please Provide Zip Code");
                document.getElementById("<%=textBoxZip.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownListState.ClientID%>").value == "--Select State--") {
                alert("Select State Name");
                document.getElementById("<%=dropDownListState.ClientID%>").focus();
                return false;
            }




            var emailID = document.getElementById("textBoxEmail").value;
            if (emailID != "") {
                if (emailcheck(emailID) == false) {
                    document.getElementById("textBoxEmail").value = "";
                    document.getElementById("textBoxEmail").focus();
                    return false;
                }
            }
            if (checkEmail(textBoxEmail.value) == false) {
                textBoxEmail.value = ""
                alert("Invalid Email Adderess");
                textBoxEmail.focus()
                return false
            }
            if (document.getElementById("<%=dropDownListLocationType.ClientID%>").value == "--Select Location Type--") {
                alert("Select Location Type");
                document.getElementById("<%=dropDownListLocationType.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownListRTOLocation.ClientID%>").value == "--Select RTO Location--") {
                alert("Please Location");
                document.getElementById("<%=dropDownListRTOLocation.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxMobileNo.ClientID%>").value == "") {
                alert("Please Provide Mobile No");
                document.getElementById("<%=textBoxMobileNo.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownListUserType.ClientID%>").value == "--Select User Type--") {
                alert("Select User Type");
                document.getElementById("<%=dropDownListUserType.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxLandlineNo.ClientID%>").value == "--Select RTO Location--") {
                alert("Please Provide Landline No");
                document.getElementById("<%=textBoxLandlineNo.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=textBoxPassword.ClientID%>").value == "") {
                alert("Please Provide Password");
                document.getElementById("<%=textBoxPassword.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxAddress1.ClientID%>").value == "") {
                alert("Please Provide Address");
                document.getElementById("<%=textBoxAddress1.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxConfirmPassword.ClientID%>").value == "") {
                alert("Please Provide Confirm Password");
                document.getElementById("<%=textBoxConfirmPassword.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxAddress2.ClientID%>").value == "") {
                alert("Please Provide Address");
                document.getElementById("<%=textBoxAddress2.ClientID%>").focus();
                return false;
            }



            if (invalidChar(document.getElementById("textBoxFirstName"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxFirstName").focus();
                return false;
            }
            if(invalidChar)

            if (invalidChar(document.getElementById("textBoxCity"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxCity").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textBoxLastName"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxLastName").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxState"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxState").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textBoxLoginName"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxLoginName").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textBoxZip"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxZip").focus();
                return false;
            }

//            if (invalidChar(document.getElementById("textBoxEmail"))) {
//                alert("Special Characters Not Allowed.");
//                document.getElementById("textBoxEmail").focus();
//                return false;
//            }




            if (invalidChar(document.getElementById("textBoxMobileNo"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxMobileNo").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textBoxLandlineNo"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxLandlineNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxAddress1"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxAddress1").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textBoxAddress2"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textBoxAddress2").focus();
                return false;
            }

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

        }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                    User Table</div>
            </legend>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <table style="background-color: #FFFFFF" width="100%" border="0" align="left" cellpadding="3"
                cellspacing="1">
                <tr>
                    <td>
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="1" style="height: 385px;
                                        width: 100%">
                                        <tr>
                                            <td style="margin-left: 10px; font-size: medium; color: Black">
                                                <table border="0" align="center" cellpadding="3" cellspacing="3" style="height: 348px;
                                                    width: 100%">
                                                    <tr>
                                                        <td class="Label_user" style="padding-bottom: 10px">
                                                            First Name: <span class="alert">* </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" ID="textBoxFirstName" MaxLength="50" TabIndex="1"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="Label_user">
                                                            City <span class="alert">* </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="textBoxCity" TabIndex="10"
                                                                runat="server"></asp:TextBox><br />
                                                    </tr>
                                                    <tr>
                                                        <td class="Label_user">
                                                            Last Name: <span class="alert">* </span>
                                                            <br />
                                                        </td>
                                                        <td class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="50" ID="textBoxLastName" TabIndex="2"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        <td class="style8">
                                                            &nbsp;
                                                        </td>
                                                        <td class="Label_user">
                                                            State:<span class="alert">* </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="textBoxState" TabIndex="11"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="td1" runat="server" class="Label_user">
                                                            Login Name: <span class="alert">* </span>
                                                        </td>
                                                        <td id="td11" runat="server" class="style6">
                                                            <asp:TextBox class="form_textbox12" MaxLength="50" ID="textBoxLoginName" TabIndex="3"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        <td class="style8">
                                                            &nbsp;
                                                        </td>
                                                        <td class="Label_user">
                                                            Zip:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" MaxLength="6" ID="textBoxZip" onkeypress="return isNumberKey(event)"
                                                                runat="server" TabIndex="12"></asp:TextBox><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label_user" id="td2" nowrap="nowrap" runat="server">
                                                            <asp:Label ID="labelOrganization" runat="server" Text="Select State:"></asp:Label>
                                                        </td>
                                                        <td class="style6">
                                                            <asp:DropDownList Style="margin-left: 8px" TabIndex="4" Width="165px" AutoPostBack="true"
                                                                ID="dropDownListState" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                                                >
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="style8">
                                                            &nbsp;
                                                        </td>
                                                        <td class="Label_user">
                                                            Email ID: <span class="alert">* </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" TabIndex="13" MaxLength="30" ID="textBoxEmail"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" id="td4" runat="server" class="Label_user">
                                                            <asp:Label ID="label2" runat="server" Text="Select Location Type:"></asp:Label>
                                                            <span class="alert">* </span>
                                                        </td>
                                                        <td id="Td5" runat="server" style="margin-left: 40px" class="style6">
                                                            <asp:UpdatePanel ID="UpdatePanelLocationType" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" TabIndex="5" Width="165px" ID="dropDownListLocationType"
                                                                        runat="server" 
                                                                        onselectedindexchanged="dropDownListLocationType_SelectedIndexChanged" >
                                                                        <asp:ListItem Text="--Select Location Type--" Value="--Select Location Type--"></asp:ListItem>
                                                                        <asp:ListItem Text="Central" Value="Central"></asp:ListItem>
                                                                        <asp:ListItem Text="District" Value="District"></asp:ListItem>
                                                                        <asp:ListItem Text="Sub-Urban" Value="Sub-Urban"></asp:ListItem>

                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="dropDownListState" 
                                                                        EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            <%--  <br />
                                                        <asp:RequiredFieldValidator ID="requiredFieldValidator2" Text="Select one client."
                                                            Display="Dynamic" ForeColor="Red" SetFocusOnError="true" InitialValue="--Select Client--"
                                                            ControlToValidate="dropDownListClient" runat="server" />--%>
                                                        </td>
                                                        <td class="style8">
                                                            &nbsp;
                                                        </td>
                                                        <td class="Label_user" nowrap="nowrap">
                                                            Mobile Number:<span class="alert">* </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" TabIndex="14" ID="textBoxMobileNo" MaxLength="10"
                                                                onkeypress="return isNumberKey(event)" runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" id="td3" runat="server" class="Label_user">
                                                            <asp:Label ID="label1" runat="server" Text="Select Location :"></asp:Label>
                                                            <span class="alert">* </span>
                                                        </td>
                                                        <td runat="server" style="margin-left: 40px" class="style6">
                                                            <asp:UpdatePanel ID="UpdatePanelLocation" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList Style="margin-left: 8px" TabIndex="5" Width="165px" ID="dropDownListRTOLocation"
                                                                        runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                                
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="dropDownListLocationType" 
                                                                        EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                                
                                                            </asp:UpdatePanel>
                                                            <%--  <br />
                                                        <asp:RequiredFieldValidator ID="requiredFieldValidator2" Text="Select one client."
                                                            Display="Dynamic" ForeColor="Red" SetFocusOnError="true" InitialValue="--Select Client--"
                                                            ControlToValidate="dropDownListClient" runat="server" />--%>
                                                        </td>
                                                        <td class="style8">
                                                        </td>
                                                        <td nowrap="nowrap" class="Label_user">
                                                            Phone Landline:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" ID="textBoxLandlineNo" TabIndex="14" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" class="Label_user">
                                                            User Type:<span class="alert">* </span>
                                                        </td>
                                                        <td class="style6">
                                                            <asp:DropDownList Style="margin-left: 8px" ID="dropDownListUserType" Width="165px"
                                                                TabIndex="6" runat="server">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </td>
                                                        <td class="style9">
                                                        </td>
                                                        <td nowrap="nowrap" class="Label_user">
                                                            Password: <span class="alert">* </span>
                                                        </td>
                                                        <td width="30%" id="td22" runat="server" class="style5">
                                                            <asp:TextBox class="form_textbox12" MaxLength="50" ID="textBoxPassword" TextMode="Password"
                                                                TabIndex="16" runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" class="Label_user">
                                                            Active Status:
                                                        </td>
                                                        <td class="style7">
                                                            <asp:CheckBox Style="margin-left: 8px" ID="checkBoxStatus" runat="server" TabIndex="7" />
                                                        </td>
                                                        <td class="style8">
                                                            &nbsp;
                                                        </td>
                                                        <td nowrap="nowrap" class="Label_user">
                                                            Confirm Password: <span class="alert">* </span>
                                                        </td>
                                                        <td width="30%" id="td33" runat="server" style="margin-left: 40px">
                                                            <asp:TextBox class="form_textbox12" TextMode="Password" ID="textBoxConfirmPassword"
                                                                MaxLength="17" TabIndex="5" Text="--Confirm Password--" runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label_user">
                                                            Address 1: <span class="alert">* </span>
                                                        </td>
                                                        <td class="style6" colspan="4">
                                                            <asp:TextBox class="form_textbox12" Width="500px" ID="textBoxAddress1" TabIndex="8"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Label_user">
                                                            Address 2:
                                                        </td>
                                                        <td class="style6" colspan="4">
                                                            <asp:TextBox class="form_textbox12" Width="500px" TabIndex="9" ID="textBoxAddress2"
                                                                runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                        </td>
                                                        <td nowrap="nowrap" align="right" colspan="5" style="margin-right: 200px">
                                                            <asp:Button ID="buttonUpdate" runat="server" TabIndex="19" class="button" Visible="false"
                                                                OnClientClick=" return validate()" Text="Update" OnClick="buttonUpdate_Click" />
                                                            &nbsp;
                                                            <%--OnClientClick="return validate();"--%>
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="18" class="button" Text="Save"
                                                                OnClientClick=" return validate()" Visible="false" OnClick="buttonSave_Click" />&nbsp;
                                                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                                id="buttonClose" value="Close" class="button" />
                                                            &nbsp;
                                                            <input type="reset" class="button" value="Reset" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="alert">
                                    * Fields are mandatory.
                                </td>
                            </tr>
                            <tr>
                                <td class="FieldText">
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="alert">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </fieldset>
    </div>
    </form>
</body>
</html>
