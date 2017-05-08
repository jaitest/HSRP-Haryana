<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FirstLoginChangePassword.aspx.cs"
    Inherits="HSRP.ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
    <script language="javascript" type="text/jscript">
        function checkPass() {

            if (document.getElementById('txtpassword').value == "") {
                alert('Old Password cannot be Blank..');
                document.getElementById('txtpassword').focus();
                return false;
            }
            else if (document.getElementById('txtNewPassword').value == "") {
                alert('New Password cannot be Blank..');
                document.getElementById('txtNewPassword').focus();
                return false;
            }
            else if (document.getElementById('txtCpassword').value != document.getElementById('txtNewPassword').value) {
                alert('Confirm password and New password are not same..');
                return false;
            }
            else if (document.getElementById('txtNewPassword').value == document.getElementById('txtpassword').value) {
                alert('Old password and Confirm password are same..');
                return false;
            }
            else if (document.getElementById('txtNewPassword').value.length < 8) {
                alert('Password is less than eight character');
                document.getElementById('txtNewPassword').value = "";
                document.getElementById('txtCpassword').value = "";
                document.getElementById('txtNewPassword').focus();
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body alink="buttons2">
    <form id="form1" runat="server">
 <div>
        <br />
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" style=" background-image:url(../images/midtablebg.jpg)">
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
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblMsg" class="header"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="heading1" style="color:Blue">
                                                        You are forced to change your Password.Since your password expired or You are logging
                                                        in for the first time..
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="18%" class="form_text">
                                                        User Name <span class="alert">* </span>
                                                    </td>
                                                    <td width="82%">
                                                        <asp:TextBox ID="txtusername" runat="server" ReadOnly="true" CssClass="form_textbox"></asp:TextBox>
                                                        <span controltovalidate="txtName" errormessage="Please Enter Name" display="Dynamic"
                                                            id="reqName" class="header" evaluationfunction="RequiredFieldValidatorEvaluateIsValid"
                                                            initialvalue="" style="color: Red; display: none;">Please Enter Name</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="form_text">
                                                        Security Questions <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="form_textbox" DataTextField="QuestionText"
                                                            DataValueField="QuestionID" >
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="form_text">
                                                        Security Answer <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="form_textbox" TextMode="Password"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="form_text">
                                                        Old Password <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtpassword" runat="server" MaxLength="15" TextMode="Password" CssClass="form_textbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="form_text">
                                                        New Password <span class="alert">* </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="15" TextMode="Password"
                                                            CssClass="form_textbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span class="form_text">Confirm Password <span class="alert">* </span></span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="form_textbox" ID="txtCpassword" runat="server" MaxLength="15"
                                                            TextMode="Password"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnProceed" runat="server" class="button" OnClick="btnProceed_Click"
                                                            OnClientClick="return checkPass();" Text="Proceed" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="btnBack" runat="server" class="button" Text="Back" OnClick="btnBack_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="alert">
                                                        * Fields are Mandatory
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblSuccMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                                        <asp:Label ID="lblErrMess" ForeColor="Red" Font-Size="18px" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="alert">
                                                        &nbsp;
                                                    </td>
                                                </tr>
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
        </br>
    </div>
    </form>
</body>
</html>
