<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderStatus.aspx.cs" Inherits="OrderStatus" %>

<%@ Register src="plugins/Page_Header.ascx" tagname="Page_Header" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <script src="javascript/common.js" type="text/javascript"></script>        
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").value == "") {
                alert("Please Provide Vehicle Registration Number.");
                document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>"))) {
                alert("Please Provide Valid Vehicle Registration Number.");
                document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").value != "") {
                var VehicalRegNo = document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").value.toUpperCase();
                if (VehicalRegNo.length < 4) {
                    alert("Please Provide Valid Vehicle Registration Number.");
                    document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").focus();
                    return false;
                }


                // alert(VehicalRegNo.substring(0, 2));
                if (VehicalRegNo.substring(0, 2) != 'HR') {
                    alert("Please Provide Valid Vehicle Registration Number.");
                    document.getElementById("<%=TextBoxVehicalRegNo.ClientID%>").focus();
                    return false;
                }

            }




        }

         
        
    
    </script>
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
<uc1:Page_Header ID="Page_Header1" runat="server" />
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle" bgcolor="#3d3d3d">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="40%" align="right">
                                            &nbsp;</td>
                                        <td width="20%" align="Left">
                                            &nbsp;
                                            <asp:LinkButton ID="lbtnComplaint" runat="server" Font-Bold="False" 
                                                Font-Size="Medium" ForeColor="White" onclick="lbtnComplaint_Click">Complaints/Lodge Feedback</asp:LinkButton>
                                        </td>                                        
                                        <td width="10%" align="center">
                                        <asp:LinkButton ID="lbtnRates" runat="server" Font-Bold="False" 
                                            Font-Size="Medium" ForeColor="White" onclick="lbtnRates_Click">Plate Rates</asp:LinkButton>
                                        </td>
                                    <td width="30%" align="center">
                                        <asp:LinkButton ID="LinkButton1" runat="server" Font-Size="Medium" 
                                            ForeColor="White" onclick="LinkButton1_Click" Visible="True">Register Your HSRP Request </asp:LinkButton>
                                        </td>
                                    
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" >
                                    <tr>
                                        <td>
                                            <div>
                                                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td height="27" background="images/midboxtopbg.jpg">
                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td height="26">
                                                                                                <span class="headingmain">CHECK STATUS OF HIGH SECURITY REGISTRATION PLATES</span>
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
                                                                                            <td valign="top">
                                                                                                <table width="1024" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
                                                                                                    <tr>
                                                                                                        <td height="324" background="images/homebgl.jpg" bgcolor="#FFFFFF">
                                                                                                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                                <tr>
                                                                                                                    <td valign="top">
                                                                                                                        <table width="510" border="0" align="right" cellpadding="3" cellspacing="3">
                                                                                                                            <tr>
                                                                                                                                <td width="117" nowrap="nowrap" class="form_text">
                                                                                                                                    &nbsp;
                                                                                                                                </td>
                                                                                                                                <td width="372" nowrap="nowrap">
                                                                                                                                    <span class="headingmain" id="lblMsg">Vehicle Registration No :</span><span class="header"><span
                                                                                                                                        class="form_text"> <span class="alert">* </span></span></span>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td nowrap="nowrap" class="form_text">
                                                                                                                                    &nbsp;
                                                                                                                                </td>
                                                                                                                                <td nowrap="nowrap" class="headingmain">
                                                                                                                                    <label for="test">
                                                                                                                                    </label>
                                                                                                                                    <input id="TextBoxVehicalRegNo" runat="server" style="text-transform: uppercase"
                                                                                                                                        maxlength="12" name="test" autocomplete="off" type="text" class="form_textboxn" />
                                                                                                                                    <br />
                                                                                                                                    Ex : HR02AA6815
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td class="form_text">
                                                                                                                                    &nbsp;
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td class="form_text">
                                                                                                                                    &nbsp;
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    &nbsp;
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td class="form_text">
                                                                                                                                    &nbsp;
                                                                                                                                </td>
                                                                                                                                <td>
                                                                                                                                    &nbsp;
                                                                                                                                </td>
                                                                                                                            </tr>
                                                                                                                            <tr>
                                                                                                                                <td class="form_text">
                                                                                                                                    <asp:Button ID="ButtonGo" runat="server" Text="Search" OnClick="ButtonGo_Click" OnClientClick="return validate()"
                                                                                                                                        CssClass="button_h" />
                                                                                                                                </td>
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
                                                                            <tr>
                                                                                <td>
                                                                                    <div id="vehshow" runat="server" style="text-align: center; width: inherit">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 80px">
                                                        <td style="color: Black; font: normal 14px tahoma, arial, verdana;" valign="bottom"
                                                            align="center">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" style="height: 60px" border="0" align="center" cellpadding="0"
                        cellspacing="0" class="marqueelinebg">
                        <tr>
                            <td style="color: Black; font: normal 15px tahoma, arial, verdana;" valign="middle"
                                align="center">
                               
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="footerbottoms">
                        <tr>
                            <td align="center">
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle">
                                Copyright@2012.All right Reserved to Transport Department , Haryana
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