<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StateAndDaysWiseAgingReport.aspx.cs" Inherits="HSRP.Report.StateAndDaysWiseAgingReport" %>
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
                                        <span class="headingmain">Age Analysis Report</span>
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

   
    <table style="width: 95%; height: 51px;">
        <tr>
            <td style="height: 59px;" width="50">
                &nbsp;</td>
            <td class="style3" style="width: 80px; height: 59px;">
                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1" 
                                            ForeColor="Black" Font-Bold="True" />
            </td>
            <td class="style3" style="width: 200px; height: 59px;">
                   
                        <asp:DropDownList ID="DropDownListStateName" runat="server" 
                                                                              
                        CausesValidation="false" DataTextField="HSRPStateName" 
                                                                              DataValueField="HSRP_StateID" 
                                                                              
                                    onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" 
                                    Height="22px" Width="120px">
                        </asp:DropDownList>                    
                
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="DropDownListStateName" ErrorMessage="Select State" 
                            InitialValue="--Select State--"></asp:RequiredFieldValidator>
                
            </td>
            <td style="height: 59px">
                &nbsp;</td>
         
                                                <td> 
                                                    <asp:Label Text="Days:" runat="server" ID="labelDate" Font-Bold="True" 
                                                        ForeColor="Black" /> </td>
                                                <td width="80">
                   
                        <asp:DropDownList ID="ddlDays" runat="server" 
                                                                              
                        CausesValidation="false" 
                                                                              
                                    onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" 
                                    Height="22px" Width="80px">
                            <asp:ListItem Value="-90">90</asp:ListItem>
                            <asp:ListItem Value="-180">180</asp:ListItem>
                            <asp:ListItem Value="-360">360</asp:ListItem>
                            <asp:ListItem Value="0">360+</asp:ListItem>
                        </asp:DropDownList>                    
                
                                                            </td>
                                                <td>
                                                                &nbsp;</td>
                                                <td><asp:Label Text="Date" runat="server" ID="labelTO" Font-Bold="True" 
                                                        ForeColor="Black" /> </td>
                                                <td>
                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                    <ContentTemplate>
                                                                        <ComponentArt:Calendar ID="OrderDateto" runat="server" ControlType="Picker" PickerCssClass="picker" 
                                                                            PickerCustomFormat="dd/MM/yyyy" PickerFormat="Custom">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="OrderDateto_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>
                                                                        <img id="ImgPollution" tabindex="4" alt="" onclick="OrderDateto_OnClick()" onmouseup="OrderDateto_OnMouseUp()"
                                                                    
    class="calendar_button" src="../images/btn_calendar.gif" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                <td style="height: 59px">
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export To Excel" 
                                                        Font-Bold="True" ForeColor="#3333FF"/>
                                                </td>
        </tr>
        <td>
            &nbsp;</td>
        <td colspan="8">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
        </td>
        <tr>
             <td>
                                                    &nbsp;</td>
             <td colspan="8">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDatefrom" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                                        NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDatefrom_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td colspan="8">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDateto" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="ImgPollution" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                                        NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDateto_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
        </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
    
                                            
</asp:Content>

