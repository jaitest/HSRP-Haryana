<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogOut.aspx.cs" Inherits="HSRP.LogOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="shortcut icon" type="image/ico" href="../images/favicon.ico" />
<link rel="shortcut icon" href="../images/logo.ico" type="image/x-icon" />  
    <title>ROSMERTA HSRP VENTURES PVT. LTD</title>
    <link href="./css/style.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%--  <script type="text/javascript">

        var slowLoad = window.setTimeout(function () {

            alert("the page is taking longer time in loading please check internet connection.");

        }, 10);


        document.addEventListener('load', function () {

            window.clearTimeout(slowLoad);

        }, false);

    </script>--%>
    <script type="text/javascript">
        var totalCount = 8;

        function ChangeIt() {
            var num = Math.ceil(Math.random() * totalCount);
            document.body.background = 'bgimages/' + num + '.jpg';
            document.body.style.backgroundRepeat = "repeat";
        }
        function validateForm() {

            var username = document.getElementById("txtUserID").value;
            var pass = document.getElementById("txtUserPassword").value;

            if (username == '') {
                alert('Please Provide User Name.');
                document.getElementById("txtUserID").focus();
                return false;
            }
            if (pass == '') {
                alert('Please Provide Password.');
                document.getElementById("txtUserPassword").focus();
                return false;
            }
            return true;


        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:100%;vertical-align:">
        <script type="text/javascript">
            ChangeIt();
        </script>
        <table  >
            <%--<tr>
                <td valign="top">
                    <table width="90%" border="0" align="center" cellpadding="1" cellspacing="0">
                        <tr>
                            <td class="form_text">
                                &nbsp;
                            </td>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td height="5" class="form_text">
                                <img src="images/trans.gif" width="1" height="5" />
                            </td>
                            <td align="right">
                                <span class="form_text">
                                    <img src="images/trans.gif" width="1" height="5" /></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                User Name:
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtUserID" CssClass="text_box" TabIndex="1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Password:
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtUserPassword" CssClass="text_box" TabIndex="2" TextMode="Password"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bluelink" align="left"> 
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="btnLogin" ImageUrl="~/images/login.png" Width="75" Height="22"
                                    runat="server" OnClick="btnLogin_Click" OnClientClick="javascript: return validateForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsgBlue" CssClass="bluelink" runat="server" Text="Label"></asp:Label>
                                <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <tr>
            <td style=" color: red; left: 19px; position: absolute; width: 599px;">
            Your Sessin Expired. Please Launch Application Again.
            </td>
            </tr>
        </table>
      
    </div>
    </form>
</body>
</html>
