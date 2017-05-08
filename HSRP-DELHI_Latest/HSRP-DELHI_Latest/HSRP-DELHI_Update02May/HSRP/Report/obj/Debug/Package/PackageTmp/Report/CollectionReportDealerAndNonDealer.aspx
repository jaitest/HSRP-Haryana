<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CollectionReportDealerAndNonDealer.aspx.cs" Inherits="HSRP.Report.CollectionReportDealerAndNonDealer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
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

        if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
            alert("Select State");
            document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
            return false;
        }


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
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">&nbsp;Location Wise Summary Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            </table>

    <asp:Panel ID="Panel1" runat="server" Width="100%" HorizontalAlign="Left">
    
    <table style="width: 100%; height: 51px;">
        <tr>
            <td style="width: 80px; height: 59px;" align="right">
                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1" 
                                            ForeColor="Black" Font-Bold="True" />
            </td>
            <td style="height: 59px">
                &nbsp;</td>
            <td style="width: 150px; height: 70px;">
                
                  
                        <asp:DropDownList ID="DropDownListStateName" runat="server" 
                                                                              
                        CausesValidation="false" DataTextField="HSRPStateName" 
                                                                              DataValueField="HSRP_StateID" 
                                                                              
                                    onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" 
                                    Height="22px" Width="148px">
                        </asp:DropDownList>
                   
                
            </td>

           
                <td align="center" style="width: 100px; height: 59px;">
                    <asp:Label ID="label2" runat="server" Font-Bold="True" ForeColor="Black" 
                        Text="Order Type:" />
                </td>
                <td style="height: 59px">
                    &nbsp;</td>
                <td style="width: 85px; height: 70px;">
                    <asp:DropDownList ID="DropDownListOrderType" runat="server" 
                        CausesValidation="false" DataTextField="HSRPStateName" 
                        DataValueField="HSRP_StateID" Height="22px" Width="80px">
                        <asp:ListItem Value="B">Both</asp:ListItem>
                        <asp:ListItem Value="D">Dealer</asp:ListItem>
                        <asp:ListItem Value="N">Non Dealer</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="center" width="50">
                    <asp:Label ID="labelDate" runat="server" Font-Bold="True" ForeColor="Black" 
                        Text="From:" />
                </td>
                <td width="80">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <ComponentArt:Calendar ID="OrderDatefrom" runat="server" ControlType="Picker" 
                                PickerCssClass="picker" PickerCustomFormat="dd/MM/yyyy" PickerFormat="Custom">
                                <ClientEvents>
                                    <SelectionChanged EventHandler="OrderDatefrom_OnDateChange" />
                                </ClientEvents>
                            </ComponentArt:Calendar>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td width="30">
                    <img id="calendar_from_button"  alt="" onclick="OrderDatefrom_OnClick()"
                                                                    onmouseup="OrderDatefrom_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                </td>
                <td width="50">
                    <asp:Label ID="labelTO" runat="server" Font-Bold="True" ForeColor="Black" 
                        Text="To:" />
                </td>
                <td width="80">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <ComponentArt:Calendar ID="OrderDateto" runat="server" ControlType="Picker" 
                                PickerCssClass="picker" PickerCustomFormat="dd/MM/yyyy" PickerFormat="Custom">
                                <ClientEvents>
                                    <SelectionChanged EventHandler="OrderDateto_OnDateChange" />
                                </ClientEvents>
                            </ComponentArt:Calendar>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td width="30">
                    <img id="ImgPollution" tabindex="4" alt="" onclick="OrderDateto_OnClick()" onmouseup="OrderDateto_OnMouseUp()"
                                                                    class="calendar_button" src="../images/btn_calendar.gif" />
                </td>
                <td>
                    <asp:Button ID="btnexport" runat="server" Font-Bold="True" ForeColor="#3333FF" 
                        onclick="btnexport_Click" onclientclick="validate()" Text="Export To Excel" 
                        Width="50px" />
                </td>
            </caption>
        </tr>
        <tr>
             <td colspan="8">
                                                    <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
                                                </td>
                                            <tr>
                                                <td colspan="8">
                                                    <ComponentArt:Calendar ID="CalendarOrderDatefrom" runat="server" 
                                                        AllowMonthSelection="false" AllowMultipleSelection="false" 
                                                        AllowWeekSelection="false" CalendarCssClass="calendar" 
                                                        CalendarTitleCssClass="title" ControlType="Calendar" DayCssClass="day" 
                                                        DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover" 
                                                        DayNameFormat="FirstTwoLetters" DisabledDayCssClass="disabledday" 
                                                        DisabledDayHoverCssClass="disabledday" ImagesBaseUrl="../images" 
                                                        MonthCssClass="month" NextImageUrl="cal_nextMonth.gif" 
                                                        NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday" 
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" 
                                                        PrevImageUrl="cal_prevMonth.gif" SelectedDayCssClass="selectedday" 
                                                        SwapDuration="300" SwapSlide="Linear">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDatefrom_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
             </tr>
             <tr>
                 <td colspan="8">
                     <ComponentArt:Calendar ID="CalendarOrderDateto" runat="server" 
                         AllowMonthSelection="false" AllowMultipleSelection="false" 
                         AllowWeekSelection="false" CalendarCssClass="calendar" 
                         CalendarTitleCssClass="title" ControlType="Calendar" DayCssClass="day" 
                         DayHeaderCssClass="dayheader" DayHoverCssClass="dayhover" 
                         DayNameFormat="FirstTwoLetters" DisabledDayCssClass="disabledday" 
                         DisabledDayHoverCssClass="disabledday" ImagesBaseUrl="../images" 
                         MonthCssClass="month" NextImageUrl="cal_nextMonth.gif" 
                         NextPrevCssClass="nextprev" OtherMonthDayCssClass="othermonthday" 
                         PopUp="Custom" PopUpExpandControlId="ImgPollution" 
                         PrevImageUrl="cal_prevMonth.gif" SelectedDayCssClass="selectedday" 
                         SwapDuration="300" SwapSlide="Linear">
                         <ClientEvents>
                             <SelectionChanged EventHandler="OrderDateto_OnChange" />
                         </ClientEvents>
                     </ComponentArt:Calendar>
                 </td>
             </tr>
             <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            </tr>
                                            
                                            
                                        </table>
    
     </asp:Panel>                                       
</asp:Content>

