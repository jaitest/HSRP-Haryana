<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestForm.aspx.cs"
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
    <style type="text/css">
        .style5
        {
            height: 27px;
        }
    </style>
    </head>
<body >
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
<td>
    <uc1:Page_Header ID="Page_Header1" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle" >
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
                                <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" >
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
                                                                                        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td height="27" background="images/midboxtopbg.jpg">
                                                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td align="center" colspan="3">
                                                                                                                <span class="headingmain">Register Your HSRP Request </span><br>

                                                                                                                <span class="headingmain">Dear Customer Please fill below form to get the electronic authorization of your HSRP from Transport Department. </span>,<br>
                                                                                                                <%--<font color="black">
                                                                                                                &nbsp
                                                                                                                <strong>Dear Customer Please fill below form to get the electronic authorization of your HSRP from Transport Department.<br>
                                                                                                                </strong></font>--%><asp:ScriptManager 
                                                                                                                    ID="ScriptManager1" runat="server">
                                                                                                                </asp:ScriptManager>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0"
                                                                                                       style="color: #000000" >
                                                                                                        <tr>
                                                                                                            <td valign="top">
                                                                                                                <div>
<table cellspacing="0" border="0" id="GridV" style="width:700px"; >
				<tr>
                <td  align="left">
                    RTO<asp:Label ID="Label4" runat="server" Font-Bold="True" 
                        ForeColor="Red" Text="*"></asp:Label>
                                                                </td>
                    <td  align="left">
                        <%--<asp:TextBox ID="txtVehicleRegNo" runat="server" Width="175px" ></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlRto" runat="server" DataValueField="RtoLocationid" 
                            DataTextField="RtoLocationName" Width="175px">
                        </asp:DropDownList>
                    </td>
                    <td  align="left" width="250px">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                                                                        
                            ControlToValidate="txtVehicleRegNo" ErrorMessage="Please Select Your  RTO" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                   <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                                                        
                            ControlToValidate="txtVehicleRegNo" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        
                            ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                                </td></tr>
				<tr>
                <td   align="left" class="style5" >
                  Vehicle Registration Number<asp:Label ID="Label5" runat="server" Font-Bold="True" 
                        ForeColor="Red" Text="*"></asp:Label>
                                                                </td>
                    <td  align="left" class="style5">
                        <%--<asp:DropDownList ID="ddlRto" runat="server" DataValueField="RtoLocationid" 
                            DataTextField="RtoLocationName" Width="175px">
                        </asp:DropDownList>--%>
                        <asp:TextBox ID="txtVehicleRegNo" runat="server" Width="175px" ></asp:TextBox>
                    </td>
                    <td  align="left" class="style5" width="250px">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                        ControlToValidate="ddlRto" ErrorMessage="Please Provide Your Vehcile Registration Number " 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red" InitialValue="-1"></asp:RequiredFieldValidator>
                                                                    </td></tr>
				<%--<tr>
                <td  align="left">
                    Address<asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" 
                        Text="*"></asp:Label>
                                                                </td>
                    <td  align="left">
                        <asp:TextBox ID="txtAddress" runat="server" Width="175px"></asp:TextBox>
                    </td>
                    <td  align="left" width="250px">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                                                                        ControlToValidate="txtAddress" ErrorMessage="Please Enter Address" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" 
                                                                        ControlToValidate="txtAddress" 
                            Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        
                            ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v" ForeColor="Red"></asp:RegularExpressionValidator>
                                                                </td></tr>--%>
				<tr>
                <td  align="left">
                    Mobile No<asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" 
                        Text="*"></asp:Label>
                                                                </td>
                    <td  align="left">
                        <asp:TextBox ID="txtPhone" runat="server" Width="175px"></asp:TextBox>
                    </td>
                    <td  align="left" width="250px">
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                                                                        ControlToValidate="txtPhone" ErrorMessage="Please Enter Mob. No" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                                                        ControlToValidate="txtPhone" 
                            Display="Dynamic" ErrorMessage="Please Provide Mobile Number" 
                                                                        
                            ValidationExpression="^[0-9]{10}$" ValidationGroup="v" ForeColor="Red"></asp:RegularExpressionValidator>
                                                                </td></tr>
				<%--<tr>
                <td  align="left">
                    Email Id<asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" 
                        Text="*"></asp:Label>
                                                                </td>
                    <td  align="left">
                        <asp:TextBox ID="txtEmail" runat="server" Width="175px"></asp:TextBox>
                    </td>
                    <td  align="left" width="250px">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                                                                        ControlToValidate="txtEmail" ErrorMessage="Please Enter Email Id" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                                                        ErrorMessage="Invalid Value" 
                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                                        ControlToValidate="txtEmail" 
                            Display="Dynamic" ValidationGroup="v" ForeColor="Red"></asp:RegularExpressionValidator>
                                                                </td></tr>--%>
				<tr>
                <td  align="left">
                    Chassis No<%--<asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" 
                        Text="*"></asp:Label>--%>
                                                                </td>
                    <td  align="left">
                        <asp:TextBox ID="txtChasis" runat="server" Width="175px"></asp:TextBox>
                    </td>
                    <td  align="left" width="250px">
                                                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" 
                                                                        ControlToValidate="txtChasis" ErrorMessage="Please Enter Chasis No" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" 
                                                                        ControlToValidate="txtChasis" 
                            Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        
                            ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                                </td></tr>
				<tr>
                <td  align="left">
                    Engine No<%--<asp:Label ID="Label10" runat="server" Font-Bold="True" 
                        ForeColor="Red" Text="*"></asp:Label>--%>
                                                                </td>
                    <td  align="left">
                        <asp:TextBox ID="txtEngine" runat="server" Width="175px"></asp:TextBox>
                    </td>
                    <td  align="left" width="250px">
                                                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" 
                                                                        ControlToValidate="txtEngine" ErrorMessage="Please Enter Engine No" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" 
                                                                        ControlToValidate="txtEngine" 
                            Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        
                            ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                                </td></tr>
				<tr>
                <td  align="left">
                    Vehicle Type<%--<asp:Label ID="Label11" runat="server" Font-Bold="True" 
                        ForeColor="Red" Text="*"></asp:Label>--%>
                                                                </td>
                    <td  align="left">
                        <asp:DropDownList ID="ddlVehicleType" runat="server" Width="175px">
                            <asp:ListItem Value="-1">--Select Vehicle Type--</asp:ListItem>
                            <asp:ListItem>SCOOTER</asp:ListItem>
                            <asp:ListItem>MOTOR CYCLE</asp:ListItem>
                            <asp:ListItem>TRACTOR</asp:ListItem>
                            <asp:ListItem>THREE WHEELER</asp:ListItem>
                            <asp:ListItem>LMV</asp:ListItem>
                            <asp:ListItem>LMV(CLASS)</asp:ListItem>
                            <asp:ListItem>MCV/HCV/TRAILERS</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td  align="left" width="250px">
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" 
                                                                        
                            ControlToValidate="ddlVehicleType" ErrorMessage="Please Select Vehicle Type" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                                                                    </td></tr>
				<tr>
                <td  align="left">
                    Vehicle Class<%--<asp:Label ID="Label12" runat="server" Font-Bold="True" 
                        ForeColor="Red" Text="*"></asp:Label>--%>
                                                                </td>
                    <td  align="left">
                        <asp:DropDownList ID="ddlVehicleClass" runat="server" Width="175px">
                            <asp:ListItem Value="-1">--Select Vehicle Class--</asp:ListItem>
                            <asp:ListItem>Non-Transport</asp:ListItem>
                            <asp:ListItem>Transport</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td  align="left" width="250px">
                                                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" 
                                                                        
                            ControlToValidate="ddlVehicleClass" ErrorMessage="Please Select Vehicle Class" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                                                                    </td></tr>
				<%--<tr>
                <td  align="left">
                    Order Type<asp:Label ID="Label13" runat="server" Font-Bold="True" 
                        ForeColor="Red" Text="*"></asp:Label>
                                                                </td>
                    <td  align="left">
                        <asp:DropDownList ID="ddlOrderType" runat="server" Width="175px">
                            <asp:ListItem Value="-1">--Select Order Type--</asp:ListItem>
                            <asp:ListItem>NB</asp:ListItem>
                            <asp:ListItem>OB</asp:ListItem>
                            <asp:ListItem>DB</asp:ListItem>
                            <asp:ListItem>DF</asp:ListItem>
                            <asp:ListItem>DR</asp:ListItem>
                            <asp:ListItem>OS</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td  align="left" width="250px">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" 
                                                                        
                            ControlToValidate="ddlOrderType" ErrorMessage="Please Select Order Type" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red" InitialValue="-1"></asp:RequiredFieldValidator>
                                                                    </td></tr>--%>
				<%--<tr>
                <td  align="left">
                    Vehicle Maker</td>
                    <td  align="left">
                        <asp:DropDownList ID="ddlMaker" runat="server" AutoPostBack="True" 
                            DataTextField="VehicleMakerDescription" DataValueField="VehicleMakerID" 
                            onselectedindexchanged="ddlMaker_SelectedIndexChanged" Width="175px">
                        </asp:DropDownList>
                    </td>
                    <td  align="left" width="250px">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" 
                                                                        
                            ControlToValidate="ddlVehicleClass" ErrorMessage="Please Select Vehicle Maker" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red" InitialValue="-1"></asp:RequiredFieldValidator>
                                                                    </td></tr>
				<tr>
                <td  align="left" class="style5">
                    Vehicle Model</td>
                    <td  align="left" class="style5">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlModel" runat="server" Width="175px" 
                                    DataTextField="VehicleModelDescription" DataValueField="VehicleModelID">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlMaker" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td  align="left" class="style5" width="250px">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" 
                                                                        
                            ControlToValidate="ddlModel" ErrorMessage="Please Select Vehicle Model" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red" InitialValue="-1"></asp:RequiredFieldValidator>
                 --%>                                                   </td></tr>
				<tr>
                <td  align="left">
                   Photo Copy Of RC</td>
                    <td  align="left">
                        <%--<asp:TextBox ID="txtDealer" runat="server" Width="175px"></asp:TextBox>--%>
                        <asp:FileUpload ID="FileUpload1" runat="server"></asp:FileUpload>
                        
                    </td>
                    <td  align="left" width="250px">
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" 
                                                                        
                            ControlToValidate="FileUpload1" ErrorMessage="Please Select RC" 
                                                                        ValidationGroup="v" 
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                                   <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" 
                                                                        
                            ControlToValidate="txtDealer" Display="Dynamic" ErrorMessage="Invalid Value" 
                                                                        
                            ValidationExpression="^[a-zA-Z0-9 ]+$" ValidationGroup="v" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                                    </td></tr>
				<tr>
                <td  align="left">
                    &nbsp;</td>
                    <td  align="left">
                        <asp:Button ID="btnGo" runat="server" BackColor="#A4A438" BorderColor="Black" 
                            BorderWidth="1px" onclick="btnGo_Click" Text="Go" ValidationGroup="v" 
                            Width="100px" />
                    </td>
                    <td  align="left" width="250px">
                        &nbsp;</td></tr>
				<tr>
                <td  align="center" colspan="3">
                    <asp:Label ID="lblSuccess" runat="server" Text="Your record has been received already.Pay <a href='http://hsrphr.com/payment.htm'>Online</a> or Visit RTO HSRP Center to Pay HSRP Fee.For Affixation." Font-Bold="True" ForeColor="Blue"></asp:Label>
                    </td>
                    </tr>
			</table>
		</div>

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
