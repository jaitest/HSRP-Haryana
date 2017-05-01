<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Complaints.aspx.cs"
    Inherits="Complaints" %>

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
                                                                    <span class="headingmain">Complaint / Feedback FORM</span>&nbsp; </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="200">&nbsp;
                                                                    </td>
                                                                <td width="150">&nbsp;
                                                                    </td>
                                                                <td width="200">&nbsp;
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Reason"></asp:Label>
                                                                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                </td>
                                                                <td width="190">
                                                                    <asp:DropDownList ID="ddlComplaintRegion" runat="server" Width="185px" ValidationGroup="v">
                                                                        <%--<asp:ListItem Value="0">-Select Reason-</asp:ListItem>
                                                                        <asp:ListItem Value="1">Issue In Affixiation</asp:ListItem>
                                                                        <asp:ListItem Value="2">Complaint</asp:ListItem--%>
                                                                        <asp:ListItem Value="0">-Select Reason-</asp:ListItem>
                                                                        <asp:ListItem Value="1">Delay</asp:ListItem>
                                                                        <asp:ListItem Value="2">Excess Charges</asp:ListItem>
                                                                        <asp:ListItem Value="3">Rude Staff</asp:ListItem>
                                                                        <asp:ListItem Value="4">Staff Not Available on Time</asp:ListItem>
                                                                        <asp:ListItem Value="5">Other</asp:ListItem>
                                                                        
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                                                        ControlToValidate="ddlComplaintRegion" ErrorMessage="Please Select Region" 
                                                                        InitialValue="-Select Region-" ValidationGroup="v"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="125">
                                                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Owner Name"></asp:Label>
                                                                   <%-- <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>--%>
                                                                    <br />
                                                                </td>
                                                                <td class="style8" style="color: #FF0000" width="190">
                                                                    <asp:TextBox ID="txtName" runat="server" ValidationGroup="v" Width="180px"></asp:TextBox>
                                                                    </td>
                                                                <td>
                                                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                        ControlToValidate="txtName" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                                                        ControlToValidate="txtName" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="110">
                                                                    <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Mobile Number" Width="100px"></asp:Label>
                                                                    <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red"
                                                                        Text="*"></asp:Label>
                                                                </td>
                                                                <td class="style8" style="color: #FF0000" width="190">
                                                                    <asp:TextBox ID="txtMobile" runat="server" ValidationGroup="v" Width="180px" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                                                        ControlToValidate="txtMobile" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                                                        ControlToValidate="txtMobile" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[0-9]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100">
                                                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="E-Mail"></asp:Label>
                                                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" 
                                                                        Text="*"></asp:Label>
                                                                </td>
                                                                <td class="style8" style="color: #FF0000" width="190">
                                                                    <asp:TextBox ID="txtMail" runat="server" Width="180px" ValidationGroup="v"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                                        ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                                        ControlToValidate="txtMail" Display="Dynamic" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="175">
                                                                    <asp:Label ID="lblRegno" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Vehicle Registration No."></asp:Label>
                                                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                    <br />
                                                                </td>
                                                                <td class="style8">
                                                                    <asp:TextBox ID="txtRegno" runat="server" ValidationGroup="v" Width="180px" MaxLength="10"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                                        ControlToValidate="txtRegno" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                                                        ControlToValidate="txtRegno" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style10">
                                                                    <asp:Label ID="lblEngineNo" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Engine No"></asp:Label>
                                                                    <%--<asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>--%>
                                                                    <br />
                                                                </td>
                                                                <td class="style8">
                                                                    <asp:TextBox ID="txtEngineno" runat="server" Width="180px" ValidationGroup="v"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                                                        ControlToValidate="txtEngineno" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" 
                                                                        ControlToValidate="txtEngineno" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblChassisNo" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Chassis No"></asp:Label>
                                                                   <%-- <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>--%>
                                                                </td>
                                                                <td class="style7">
                                                                    <asp:TextBox ID="txtChasisNo" runat="server" Width="180px" ValidationGroup="v"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                                                        ControlToValidate="txtChasisNo" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" 
                                                                        ControlToValidate="txtChasisNo" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRemarks" runat="server" Font-Bold="True" ForeColor="Black" 
                                                                        Text="Remarks"></asp:Label>
                                                                    <%--<asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>--%>
                                                                </td>
                                                                <td class="style7">
                                                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="40px"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                  <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                                                        ControlToValidate="txtRemarks" ErrorMessage="This Field can not be empty" 
                                                                        ValidationGroup="v" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" 
                                                                        ControlToValidate="txtRemarks" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v"></asp:RegularExpressionValidator>
--%>                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style5">&nbsp;
                                                                    <asp:Label ID="lblComp" runat="server" Text="Label" Visible="False"></asp:Label>
                                                                    </td>
                                                                <td>
                                                                    <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" Text="Submit" 
                                                                        ValidationGroup="v" BackColor="#9F9F35" />
                                                                    &nbsp;
                                                                    <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                                                                        Text="Cancel" CausesValidation="False" BackColor="#9F9F35" />
                                                                </td>
                                                                <td>&nbsp;
                                                                    </td>
                                                            </tr>
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
