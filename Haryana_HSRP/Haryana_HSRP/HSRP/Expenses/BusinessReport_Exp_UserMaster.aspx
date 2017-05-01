
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BusinessReport_Exp_UserMaster.aspx.cs" Inherits="HSRP.Expenses.BusinessReport_Exp_UserMaster" %>


<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
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

    <%--<script type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
        }
    </script>--%>

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
    <div style="width: 1180px; height: 500px;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="30" background="../images/midtablebg.jpg">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <span class="headingmain">User Master Report</span>
                                        </td>
                                        <td width="240px" height="26" align="center" nowrap></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <table width="100%">
            <tr>
                <td colspan="8">
                    <table style="width: 90%">
                        <tr>
                            <td colspan="10" align="center">
                                <table border="0" align="center" cellpadding="3" cellspacing="1" style="width: 100%; border:solid 1px gray;">
                                    <tr>
                                         <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label ID="label5" runat="server" Font-Bold="True" ForeColor="Black"  Text="Report Type" Width="100px" />
                                        </td>
                                         <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">                                                                                         
                                            <asp:DropDownList ID="DdlReportType" runat="server" OnSelectedIndexChanged="DdlReportType_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>                                                  
                                        </td>                                    
                                         <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label Text="From:" runat="server" ID="labelDate" Font-Bold="True" ForeColor="Black" Width="48px" />
                                        </td>
                                         <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Datefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Datefrom_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_from_button" alt="" onclick="Datefrom_OnClick()" style="margin-bottom:-6px;"
                                                        onmouseup="Datefrom_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label Text="To:" runat="server" ID="label3" Font-Bold="True"  ForeColor="Black" />
                                        </td>
                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Dateto" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Dateto_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_to_button" alt="" onclick="Dateto_OnClick()" style="margin-bottom:-6px;"
                                                        onmouseup="Dateto_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>   
                                        
                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label ID="label2" runat="server" Font-Bold="True" ForeColor="Black"  Text="Search" Visible="false"/>
                                        </td>
                                        <td nowrap="nowrap" class="style7">
                                            <asp:TextBox ID="txtSearch" runat="server" Visible="false"></asp:TextBox>                                           
                                        </td>                                    
                                     
                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <%--<asp:Button ID="btn_Go" runat="server" Visible="false" Text="Go" OnClick="btn_Go_Click1" />--%>

                                            <asp:Button ID="btn" runat="server" Text="Preview" Font-Bold="True" ForeColor="#3333FF" OnClick="btn_Click" />
                                            
                                            <asp:Button ID="btn_download" runat="server" Text="Export Excel" Visible="true" Font-Bold="True" ForeColor="#3333FF" OnClick="btn_download_Click"/>
                                            
                                         </td>
                                    </tr>
                                    <tr>
                                        <td><asp:Label ID="lblErrMess" runat="server" Text="" ForeColor="Red"></asp:Label></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>                             
                                </table>
                            </td>
                            
                        </tr>
                        <tr>
                            <td width="80" colspan="4">&nbsp;</td>
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
                        <tr>
                            <td colspan="10" align="center">
                                <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="8" align="center" id="gridTD" runat="server">
                    <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="700px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
                        <asp:GridView ID="grd" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                            ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>         
                                          
                            </Columns>

                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
