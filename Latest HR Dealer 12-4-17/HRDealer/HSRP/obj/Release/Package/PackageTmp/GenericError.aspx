<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenericError.aspx.cs" Inherits="MultiTrack.GenericError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script type="text/javascript">

        var email = ('tanuj.tomer@rosmertaengg.com');

        var subject = ('Error Report');

        var cc = ('amit.Bhargava@rosmertaengg.com');

        var bcc = ('sanjay.balodi@rosmertaengg.com');

        var body = ('');

        document.write('<a href="mailto:' + email +

'?subject=' + subject +

'&cc=' + cc +

'&bcc=' + bcc +

'&body=' + body +

'">' + 'Send Error Mail' + '<' + '/a>');

    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="images/midtablebg.jpg">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="26">
                                            <span class="headingmain">Exception Occured</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top" bgcolor="#c0c0c0">
                                            <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                                <tr>
                                                    <td width="51%" height="19" valign="top" bgcolor="#d2e0f1" class="heading1">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="26" align="center" bgcolor="#FFFFFF" class="maintext">
                                                        <p>
                                                            <img alt="errror" src="images/error.jpg" width="300" height="279" /></p>
                                                        <h1 class="marquee">
                                                            Error On Page</h1>
                                                        <p class="headingmain">
                                                            Sorry For Inconvience. Please Contact System Administrator.
                                                            <script type="text/javascript">

                                                                var email = ('tanuj.tomer@rosmertaengg.com');

                                                                var subject = ('Error Report');

                                                                var cc = ('amit.Bhargava@rosmertaengg.com');

                                                                var bcc = ('sanjay.balodi@rosmertaengg.com');

                                                                var body = ('');

                                                                document.write('<a href="mailto:' + email +

'?subject=' + subject +

'&cc=' + cc +

'&bcc=' + bcc +

'&body=' + body +

'">' + 'Send Error Mail' + '<' + '/a>');

                                                            </script>
                                                            &nbsp;</p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
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
