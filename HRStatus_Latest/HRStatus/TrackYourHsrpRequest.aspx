<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrackYourHsrpRequest.aspx.cs"
    Inherits="TrackYourHsrpRequest" %>

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
    <style type="text/css">
        .style4
        {
            width: 231px;
        }
    </style>
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
                    <div id="prin" align="center" >
                      <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                     
                  
                    <tr align="center">
                   
                        <td align="center">
                                                <asp:Panel ID="PnlModal" runat="server" 
            Width="650px" 
                                                   Font-Names="Arial" 
            Font-Size="Small" ForeColor="Black" Height="400px" 
            HorizontalAlign="Center" CssClass="tableborder">
                                                   <br />
                                                    <div align="left">
                                                        <table style="width: 100%; height: 108px;">
                                                            <tr>
                                                                <td align="center" colspan="3">
                                                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="#000099"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="3" style="background-color: #9F9F35">
                                                                    <span class="headingmain">Track Your Hsrp Request</span>&nbsp; </td>
                                                            </tr>
                                                            <tr>
                                                               <td>
                                                                    <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Request Number</span> </td>
                                                                <td class="style4">
                                                                <asp:TextBox ID="txtRequestNo" runat="server" Width="180px" ></asp:TextBox>
                                                                    </td>
                                                                <td>
                                                                <asp:Button ID="btnGo" runat="server" onclick="btnGo_Click" Text="Go"  Width="150px"
                                                                        ValidationGroup="v" BackColor="#9F9F35" />
                                                                    </td>
                                                            </tr>
                                                             <td>
                                                           <asp:Label ID="Label1" runat="server"  Font-Bold="True" ForeColor="Blue"></asp:Label>
                                                            </td>
                                                        </table>
                                                    </div>
                                                    
                                                      
                                                </asp:Panel> 
                            <br />
                           
                                        </td>
                                    </tr>
                                </table>
                            </div>
                       
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
                            </td>
                        </tr>
                    </table>
                                </div>
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
