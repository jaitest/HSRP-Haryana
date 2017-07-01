<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TgGovReport.aspx.cs" Inherits="HSRP.Report.TgGovReport" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <style type="text/css">
        .style4 {
            width: 428px;
        }

        .style5 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 103px;
            padding-left: 20px;
        }

        .style6 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 184px;
            padding-left: 20px;
        }

        .style7 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
        }
    </style>

    <script type="text/javascript">

        function Datefrom_OnDateChange(sender, eventArgs) {
            var fromDate = Datefrom.getSelectedDate();
            CalendarDatefrom.setSelectedDate(fromDate);

        }

        function Datefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarDatefrom.getSelectedDate();
            Datefrom.setSelectedDate(fromDate);

        }

        function Datefrom_OnClick() {
            if (CalendarDatefrom.get_popUpShowing()) {
                CalendarDatefrom.hide();
            }
            else {
                CalendarDatefrom.setSelectedDate(Datefrom.getSelectedDate());
                CalendarDatefrom.show();
            }
        }

        function Datefrom_OnMouseUp() {
            if (CalendarDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function Dateto_OnDateChange(sender, eventArgs) {
            var toDate = Dateto.getSelectedDate();
            CalendarDateto.setSelectedDate(toDate);
        }

        function Dateto_OnChange(sender, eventArgs) {
            var toDate = CalendarDateto.getSelectedDate();
            Dateto.setSelectedDate(toDate);
        }

        function Dateto_OnClick() {
            if (CalendarDateto.get_popUpShowing()) {
                CalendarDateto.hide();
            }
            else {
                CalendarDateto.setSelectedDate(Dateto.getSelectedDate());
                CalendarDateto.show();
            }
        }

        function Dateto_OnMouseUp() {
            if (CalendarDateto.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

    </script>

   
    <div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="../images/midtablebg.jpg">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <span class="headingmain">TG Govement Report</span>
                                        </td>
                                        <td width="300px" height="26" align="center" nowrap></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>


        <table style="height: 51px;" width="70%">
            <tr>
                <td style="height: 40px;" width="20"></td>
                <td style="height: 30px;" valign="middle" width="80">
                    <asp:Label Text="From Date:" runat="server" ID="labelOrganization1"
                        ForeColor="Black" Font-Bold="True" Width="100px" />
                </td>
             
                <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                
                    <ComponentArt:Calendar ID="Datefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                        ControlType="Picker" PickerCssClass="picker">
                        <ClientEvents>
                            <SelectionChanged EventHandler="Datefrom_OnDateChange" />
                        </ClientEvents>
                    </ComponentArt:Calendar>
                    <img id="calendar_from_button" alt="" onclick="Datefrom_OnClick()"
                        onmouseup="Datefrom_OnMouseUp()" class="calendar_button"
                        src="../images/btn_calendar.gif" />
                  
                                   
                
                               
                </td>



                <td style="height: 30px;" valign="middle" width="80">
                    <asp:Label Text="To Date:" runat="server" ID="label1"
                        ForeColor="Black" Font-Bold="True" Width="100px" />
                </td>
             

                <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                 
                    <ComponentArt:Calendar ID="Dateto" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                        ControlType="Picker" PickerCssClass="picker">
                        <ClientEvents>
                            <SelectionChanged EventHandler="Dateto_OnDateChange" />
                        </ClientEvents>
                    </ComponentArt:Calendar>
                    <img id="calendar_to_button" alt="" onclick="Dateto_OnClick()"
                        onmouseup="Dateto_OnMouseUp()" class="calendar_button"
                        src="../images/btn_calendar.gif" />
                   
                </td>






                <%--   </td>--%>



                <td style="height: 20px" width="20">
                    <asp:Button ID="btnexport" runat="server" OnClientClick="return validate()" OnClick="btnexport_Click" Text="Export" Font-Bold="True" ForeColor="#3333FF" />
                </td>


            </tr>

            <tr>

                <td colspan="10" align="center">
                    <asp:Label ID="Lblerror" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
                </td>

            </tr>

            <tr>
                <td colspan="3" align="left">
                    <ComponentArt:Calendar runat="server" ID="CalendarDatefrom" AllowMultipleSelection="false"
                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif"
                        NextImageUrl="cal_nextMonth.gif" Height="172px" Width="200px">
                        <ClientEvents>
                            <SelectionChanged EventHandler="Datefrom_OnChange" />
                        </ClientEvents>
                    </ComponentArt:Calendar>
                </td>
                <td colspan="3" align="left">
                    <ComponentArt:Calendar runat="server" ID="CalendarDateto" AllowMultipleSelection="false"
                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                        PopUp="Custom" PopUpExpandControlId="calendar_to_button" CalendarTitleCssClass="title"
                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif"
                        NextImageUrl="cal_nextMonth.gif" Height="172px" Width="200px">
                        <ClientEvents>
                            <SelectionChanged EventHandler="Dateto_OnChange" />
                        </ClientEvents>
                    </ComponentArt:Calendar>
                </td>

            </tr>


        </table>
    </div>
   
</asp:Content>
