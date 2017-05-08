<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HSRP.Dealer.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="shortcut icon" type="image/ico" href="../images/favicon.ico" />
<link rel="shortcut icon" href="../images/logo.ico" type="image/x-icon" />  
    <title>ROSMERTA HSRP VENTURES PVT. LTD</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" src="windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="windowfiles/dhtmlwindow.css" type="text/css" />
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
    <script type="text/javascript">
        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes
            //alert('a');
            googlewin = dhtmlwindow.open("googlebox", "iframe", "Master/User.aspx?Mode=New", "New User", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = '../Login.aspx';
                return false;
            }
        }
         
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:100%;vertical-align:">
        <script type="text/javascript">
            ChangeIt();
        </script>
        <table width="300" border="0" align="right" cellpadding="0" cellspacing="0" class="x-window-mc">
            <tr>
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
                                <%--                                <asp:LinkButton ID="LinkButtonForget" runat="server" CssClass="bluelink" Text="Forgot password ?"
                                    OnClick="LinkButtonForget_Click"></asp:LinkButton>--%>
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="btnLogin" ImageUrl="~/images/login.png" Width="75" Height="22"
                                    runat="server" OnClick="btnLogin_Click" OnClientClick="javascript:return validateForm();" />
                            </td>
                        </tr>
                      <%--  <tr>
                            <td colspan="2" align="right" style="text-align: left">
                            <a onclick="AddNewPop(); return false;" title="Add New Hub" style="color:Blue; cursor:pointer; text-decoration:underline;">New User Registration</a>
                               
                            </td>
                        </tr>--%>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsgBlue" CssClass="bluelink" runat="server" Text="Label"></asp:Label>
                                <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--  <div style="vertical-align:bottom;"  id="footerbg">
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" height="60px">
                <tr>
                    <td width="75%" class="topheaderboldtext">
                        &nbsp;&nbsp; Linkpoint Infrastructure Private Limited
                    </td>
                    <td width="25%" valign="middle">
                        <span class="maintext">137, Udyog Vihar, Phase-l, Gurgaon-122001 (Haryana)<br />
                            &nbsp;Support Toll Free: 1800-120-3030,<br />
                            &nbsp;Email: contact@hsrp.com</span>
                    </td>
                </tr>
            </table>
        </div>--%>
    </div>
    </form>
</body>
</html>
