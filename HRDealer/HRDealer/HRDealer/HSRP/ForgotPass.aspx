<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPass.aspx.cs" Inherits="MultiTrack.ForgotPass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forget Password</title>
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function UserName(element, evt) {

            if (evt.type == "focusout") {
                if (element.value == '' || element.value == element.defaultValue) {
                    $('#txtusername').css('background-color', '#fbfbfb');
                    $('#txtusername').css('border', '1px solid #e5e5e5');
                    $('#txtusername').css('font-weight', 'normal');
                    $('#txtusername').css('font-size', 'medium');
                    element.value = element.defaultValue;
                }

                else {
                    $('#txtusername').css('background-color', '#fbfbfb');
                    $('#txtusername').css('border', '1px solid #e5e5e5');
                    $('#txtusername').css('font-weight', 'normal');
                    $('#txtusername').css('font-size', 'medium');
                }
            }
            if (evt.type == "focus") {
                if (element.value == element.defaultValue) {
                    $('#txtusername').css('background-color', '#ffffff');
                    $('#txtusername').css('border', '2px solid #D9D9D9');
                    $('#txtusername').css('font-weight', 'normal');
                    $('#txtusername').css('font-size', 'large');
                    element.value = '';
                }
                else {
                    $('#txtusername').css('background-color', '#ffffff');
                    $('#txtusername').css('border', '2px solid #D9D9D9');
                    $('#txtusername').css('font-weight', 'normal');
                    $('#txtusername').css('font-size', 'large');
                }
            }
        }

        function SecurityAnswer(element, evt) {

            if (evt.type == "focusout") {
                if (element.value == '' || element.value == element.defaultValue) {
                    $('#txtSecAnswer').css('background-color', '#fbfbfb');
                    $('#txtSecAnswer').css('border', '1px solid #e5e5e5');
                    $('#txtSecAnswer').css('font-weight', 'normal');
                    $('#txtSecAnswer').css('font-size', 'medium');
                    element.value = element.defaultValue;
                }

                else {
                    $('#txtSecAnswer').css('background-color', '#fbfbfb');
                    $('#txtSecAnswer').css('border', '1px solid #e5e5e5');
                    $('#txtSecAnswer').css('font-weight', 'normal');
                    $('#txtSecAnswer').css('font-size', 'medium');
                }
            }
            if (evt.type == "focus") {
                if (element.value == element.defaultValue) {
                    $('#txtSecAnswer').css('background-color', '#ffffff');
                    $('#txtSecAnswer').css('border', '2px solid #D9D9D9');
                    $('#txtSecAnswer').css('font-weight', 'normal');
                    $('#txtSecAnswer').css('font-size', 'large');
                    element.value = '';
                }
                else {
                    $('#txtSecAnswer').css('background-color', '#ffffff');
                    $('#txtSecAnswer').css('border', '2px solid #D9D9D9');
                    $('#txtSecAnswer').css('font-weight', 'normal');
                    $('#txtSecAnswer').css('font-size', 'large');
                }
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table width="99%" style="height: 99%" border="0" align="center" cellpadding="0"
            cellspacing="0" class="midtable">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" style="background-image: url(../images/midtablebg.jpg)">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="26">
                                            <span class="headingmain">Change Password</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                    <tr>
                                        <td height="26" bgcolor="#FFFFFF" class="maintext">
                                            <table width="98%" border="0" align="center" cellpadding="3" cellspacing="3">
                                                <tr id="tr0" runat="server" visible="false">
                                                    <td width="18%" class="form_text">
                                                        User Name <span class="alert">* </span>
                                                    </td>
                                                    <td width="82%">
                                                        <asp:TextBox CssClass="form_textbox" ID="txtusername" runat="server" Text="--Enter User Name--"
                                                            TabIndex="1"></asp:TextBox>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="requiredFieldValidatortxtusername" SetFocusOnError="true"
                                                            runat="server" ErrorMessage="please enter User Name" ControlToValidate="txtusername"
                                                            ForeColor="Red" Display="Dynamic" InitialValue="--Enter User Name--" Style="clear: both;
                                                            float: left;"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr id="tr1" runat="server" visible="false">
                                                    <td colspan="2" style="padding-left: 100px">
                                                        &nbsp;
                                                        <asp:Button ID="btnView" Width="250px" TabIndex="2" runat="server" CssClass="button"
                                                            Text="Show My Security Question" OnClick="btnView_Click" />
                                                        &nbsp; &nbsp; &nbsp; &nbsp; <a href="Login.aspx">back</a>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="tr2" runat="server" visible="false">
                                                    <td width="18%" class="form_text">
                                                        Your Security Question <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:Label class="form_text" ID="labelQuestion" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr id="tr3" runat="server" visible="false">
                                                    <td width="18%" class="form_text">
                                                        Your Security Answer <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSecAnswer" TabIndex="3" runat="server" TextMode="Password" Text="--Enter Your Answer--"
                                                            class="form_textbox"></asp:TextBox>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="requiredFieldValidatortxtSecAnswer" SetFocusOnError="true"
                                                            runat="server" ErrorMessage="please enter Answer." ControlToValidate="txtSecAnswer"
                                                            ForeColor="Red" Display="Dynamic" InitialValue="--Enter Your Answer--" Style="clear: both;
                                                            float: left;"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr id="tr4" runat="server" visible="false">
                                                    <td colspan="2" style="padding-left: 100px">
                                                        <asp:Button ID="btnProceed" runat="server" Text="Proceed" TabIndex="4" CssClass="button"
                                                            OnClick="btnProceed_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <a href="Login.aspx">back</a>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="tr5" runat="server" visible="false">
                                                    <td colspan="2" class="alert">
                                                        * Fields are Mandatory
                                                    </td>
                                                </tr>
                                                <%--                                                <tr>
                                                    <td class="FieldText">
                                                        
                                                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                                    
                                                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                                    </td>
                                                </tr>--%>
                                                <tr id="tr6" runat="server" visible="false">
                                                    <td width="18%" class="form_text">
                                                        User Name: <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtResetUsername" TabIndex="6" CssClass="form_textbox" ReadOnly="true"
                                                            runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="tr7" runat="server" visible="false">
                                                    <td width="18%" class="form_text">
                                                        New Password: <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtResetPassword" TabIndex="7" CssClass="form_textbox" runat="server"
                                                            TextMode="Password" MaxLength="15"></asp:TextBox>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="requiredFieldValidatortxtResetPassword" SetFocusOnError="true"
                                                            runat="server" ErrorMessage="please enter Password." ControlToValidate="txtResetPassword"
                                                            ForeColor="Red" Display="Dynamic" Style="clear: both; float: left;"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr id="tr8" runat="server" visible="false">
                                                    <td style="width: 18%" class="form_text">
                                                        Confirm Password: <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form_textbox" TabIndex="8"
                                                            TextMode="Password" MaxLength="15"></asp:TextBox>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="requiredFieldValidatortxtConfirmPassword" SetFocusOnError="true"
                                                            runat="server" ErrorMessage="please enter Password Again." ControlToValidate="txtConfirmPassword"
                                                            ForeColor="Red" Display="Dynamic" Style="clear: both; float: left;"></asp:RequiredFieldValidator>
                                                        <asp:CompareValidator runat="server" ID="compareValidatortxtConfirmPassword" SetFocusOnError="true"
                                                            ErrorMessage="Password Not matched." ControlToValidate="txtConfirmPassword" ForeColor="Red" ControlToCompare="txtResetPassword"
                                                            Display="Dynamic" Style="clear: both; float: left;"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr id="tr9" runat="server" visible="false">
                                                    <td colspan="2" style="padding-left: 100px; margin-left: 40px;">
                                                        <asp:Button ID="btn_Proceed" runat="server" TabIndex="9" Text="Proceed" CssClass="button"
                                                            OnClick="btn_Proceed_Click" />
                                                        &nbsp; &nbsp; &nbsp; &nbsp; <a href="Login.aspx">back</a>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="tr10" runat="server" visible="false">
                                                    <td colspan="2" class="alert">
                                                        * Fields are Mandatory
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="width: 18%" class="form_text">
                                                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="tr11" runat="server" visible="false">
                                                    <td colspan="2" nowrap="nowrap" class="form_text">
                                                        <h2 style="color: #78bb14">
                                                            Your Password has been changed. Now you can login with new password</h2>
                                                        <asp:LinkButton ID="LinkButton1" Text="Back to login page." runat="server" PostBackUrl="~/Login.aspx"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                    <td colspan="2" align="center" valign="bottom" style="color:Blue">
                                                        © Copyright MultiTrack System.All Rights Reserved.Site designed and Maintained By RosMerta technologies.
                                                    </td>
                                                    <td></td>
                                                </tr>--%>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
