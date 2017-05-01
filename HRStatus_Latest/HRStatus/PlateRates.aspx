<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlateRates.aspx.cs"
    Inherits="PlateRates" %>

<%@ Register src="plugins/Page_Header.ascx" tagname="Page_Header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function printSelection(node) {

            var content = node.innerHTML
            var pwin = window.open('', 'print_content', 'width=1500,height=1000');

            pwin.document.open();
            pwin.document.write('<html><body onload="window.print()">' + content + '</body></html>');
            pwin.document.close();

            setTimeout(function () { pwin.close(); }, 1000);

        }
    </script>
</head>
<body style="background-color: #F0F0F0">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                <uc1:Page_Header ID="Page_Header2" runat="server" />
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle" bgcolor="#3d3d3d">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td height="30">
                                            &nbsp;                                            
                                        </td>
                                        <td width="40%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%" style="height: 400px; background-color: White" border="0" align="center"
                        cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <%-- <asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server">
                                </asp:ContentPlaceHolder>--%>
                                <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
                                    <tr>
                                        <td valign="top">
                                            <div id="prin">
                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                   
                                                    <tr>
                                                        <td>
                                                            <div align="center">
                                                                <table id="show" runat="server">
                                                                    <tr>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td height="27" background="images/midboxtopbg.jpg">
                                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td height="26">
                                                                                                                <span class="headingmain">HSRP PLATE RATES</span>
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
                                                                                                               <img src="images/rates.jpg"  />

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
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div style="height: 100px;">
                                    <table width="100%" style="height: 60px" border="0" align="center" cellpadding="0"
                                        cellspacing="0" class="marqueelinebg">
                                        <tr>
                                            <td style="color: Black; font: normal 15px tahoma, arial, verdana;" valign="middle"
                                                align="center">
                                               
                                            </td>
                                        </tr>
                                    </table>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="Go Back" Font-Size="Medium"
                                    OnClick="Button1_Click" BackColor="#B0B050" />
                                <%--<asp:HyperLink ID="HyperLinkGoBack" NavigateUrl="~/OrderStatus.aspx" runat="server"><img src="img/Goback1.png" style=" width:50px; height:50px" /></asp:HyperLink>--%>
                            </td>
                            <td align="right">
                                <%--<asp:HyperLink ID="HyperLink1" runat="server" Onclick="print()"><img src="img/print.png" /></asp:HyperLink>--%>
                            </td>
                        </tr>
                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="footerbottoms">
                        <tr>
                            <td valign="middle">
                                Copyright@2012.All right Reserved to LINK UTSAV REGISTRATION PLATES PVT. LTD
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
