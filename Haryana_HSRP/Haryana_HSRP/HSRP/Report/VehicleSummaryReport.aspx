<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VehicleSummaryReport.aspx.cs" Inherits="HSRP.Report.VehicleSummaryReport" %>
<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />



<script type="text/javascript">
    function validate() {



    }
    </script>

    <script type="text/javascript">

        function OrderDatefrom_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDatefrom.getSelectedDate();
            CalendarOrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDatefrom.getSelectedDate();
            OrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnClick() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                CalendarOrderDatefrom.hide();
            }
            else {
                CalendarOrderDatefrom.setSelectedDate(OrderDatefrom.getSelectedDate());
                CalendarOrderDatefrom.show();
            }
        }

        function OrderDatefrom_OnMouseUp() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function OrderDateto_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDateto.getSelectedDate();
            CalendarOrderDateto.setSelectedDate(fromDate);

        }

        function OrderDateto_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDateto.getSelectedDate();
            OrderDateto.setSelectedDate(fromDate);

        }


        function OrderDateto_OnClick() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                CalendarOrderDateto.hide();
            }
            else {
                CalendarOrderDateto.setSelectedDate(OrderDateto.getSelectedDate());
                CalendarOrderDateto.show();
            }
        }



        function OrderDateto_OnMouseUp() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        
    </script>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg" colspan=7>
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Vehicle wise Summary For FY 12-13</span></td>
                                    
                                </tr>
                            </table>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            </table>

   
    <table width: 100%>
        <tr>
            <td class="style3" style="width: 123px; height: 59px;">
                                                    <asp:Label Text="Month:" runat="server" ID="labelDate" Font-Bold="True" 
                                                        ForeColor="Black" /> </td>
            <td style="width: 195px; height: 59px;">
                                                                        <asp:DropDownList ID="ddlMonth" runat="server">
                                                                            <asp:ListItem>--Select Month--</asp:ListItem>
                                                                            <asp:ListItem>ALL</asp:ListItem>
                                                                            <asp:ListItem>Mar-12</asp:ListItem>
                                                                            <asp:ListItem>Apr-12</asp:ListItem>
                                                                            <asp:ListItem>May-12</asp:ListItem>
                                                                            <asp:ListItem>Jun-12</asp:ListItem>
                                                                            <asp:ListItem>Jul-12</asp:ListItem>
                                                                            <asp:ListItem>Aug-12</asp:ListItem>
                                                                            <asp:ListItem>Sep-12</asp:ListItem>
                                                                            <asp:ListItem>Oct-12</asp:ListItem>
                                                                            <asp:ListItem>Nov-12</asp:ListItem>
                                                                            <asp:ListItem>Dec-12</asp:ListItem>
                                                                            <asp:ListItem>Jan-13</asp:ListItem>
                                                                            <asp:ListItem>Feb-13</asp:ListItem>
                                                                            <asp:ListItem>Mar-13</asp:ListItem>
                                                                        </asp:DropDownList>
                </td>
            <td style="width: 124px; height: 59px;">
                                                    <asp:Label Text="Report:" runat="server" ID="labelTO" Font-Bold="True" 
                                                        ForeColor="Black" /> </td>
            <td style="height: 59px">
                
                                                                        <asp:DropDownList ID="ddlReport" runat="server">
                                                                            <asp:ListItem>--Select Report Wise--</asp:ListItem>
                                                                            <asp:ListItem Value="Count">Count Wise</asp:ListItem>
                                                                            <asp:ListItem Value="Value">Value Wise</asp:ListItem>
                                                                        </asp:DropDownList>
                
            </td>
         
                                                <td style="width: 70px"> 
                                                    &nbsp;</td>
                                                <td style="width: 101px">
                                                                
                                                            </td>
                                                <td style="height: 59px">
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export To Excel" 
                                                        Font-Bold="True" ForeColor="#3333FF" onclientclick="validate()" 
                                                        CssClass="button" />
                                                </td>
        </tr>
        <tr>
            <td class="style3" style="height: 59px;" colspan="7">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300"></asp:Label>
            </td>
        </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
    
                                            
</asp:Content>
