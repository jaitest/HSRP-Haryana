<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InvoiceSummaryWithAmount.aspx.cs" Inherits="HSRP.Report.InvoiceSummaryWithAmount" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
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

        function Dateto_OnDateChange(sender, eventArgs)
        { 
            var toDate = Dateto.getSelectedDate();
            CalendarDateto.setSelectedDate(toDate);
        }

        function Dateto_OnChange(sender, eventArgs)
        {
            var toDate = CalendarDateto.getSelectedDate();
            Dateto.setSelectedDate(toDate);
        }

        function Dateto_OnClick()
        {
            if (CalendarDateto.get_popUpShowing()) {
                CalendarDateto.hide();
            }
            else {
                CalendarDateto.setSelectedDate(Dateto.getSelectedDate());
                CalendarDateto.show();
            }
        }

        function Dateto_OnMouseUp()
        {
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
    <div style="width: 1107px; height: 500px;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="../images/midtablebg.jpg">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <span class="headingmain">Summary report on Type Wise</span>
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

        <table width="100%">
            <tr>
                <td colspan="8">
                    <table style="width: 90%">
                        <tr>
                            <td colspan="10" align="center">
                                <table border="1" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1"
                                                ForeColor="Black" Font-Bold="True" Width="83px" />
                                        </td>
                                        <td align="left">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>

                                                
                                            <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="True" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                                CausesValidation="false" Height="22px" Width="120px" OnTextChanged="DropDownListStateName_TextChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="DropDownListStateName" ErrorMessage="Select State"
                                                InitialValue="--Select State--" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
<%--                                                                                <td align="left">
                                            <asp:Label Text="Location:" runat="server" ID="label4"
                                                ForeColor="Black" Font-Bold="True" Width="83px" />
                                        </td>
                                        <td align="left">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>                                                
                                            <asp:DropDownList ID="ddlRtoLocation" runat="server" AutoPostBack="True" DataTextField="RTOLocationName" DataValueField="RTOLocationID"
                                                CausesValidation="false" Height="22px" Width="120px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                ControlToValidate="ddlRtoLocation" ErrorMessage="Select Location"
                                                InitialValue="--Select Location--" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>--%>
                                        <td>
                                            <asp:Label ID="label2" runat="server" Font-Bold="True" ForeColor="Black"
                                                Text="Report Type:" Width="100px" />
                                        </td>
                                        <td>
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>                                                
                                            <asp:DropDownList ID="DropDownListReportType" runat="server"
                                                CausesValidation="false" Height="22px" Width="100px" AutoPostBack="true">
                                                <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                                                <asp:ListItem Value="all">All</asp:ListItem>
                                                <asp:ListItem Value="co">Commercial</asp:ListItem>
                                                <asp:ListItem Value="de">Dealer</asp:ListItem>
                                                <asp:ListItem Value="ce">Centre</asp:ListItem>
                                                <asp:ListItem Value="on">Online</asp:ListItem>
                                                <asp:ListItem Value="cd">Commercial C vs D</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DropDownListReportType" InitialValue="--Select--" ForeColor="Red" ErrorMessage="Select Report Type"></asp:RequiredFieldValidator>
                                                    </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label Text="From:" runat="server"
                                                ID="labelDate" Font-Bold="True"
                                                ForeColor="Black" Width="48px" />
                                        </td>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Datefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Datefrom_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_from_button" alt="" onclick="Datefrom_OnClick()"
                                                        onmouseup="Datefrom_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label Text="To:" runat="server"
                                                ID="label3" Font-Bold="True"
                                                ForeColor="Black" />
                                        </td>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Dateto" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Dateto_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_to_button" alt="" onclick="Dateto_OnClick()"
                                                        onmouseup="Dateto_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>                                        
                                        <td>
                                            <asp:Button ID="btn" runat="server" Text="Preview"
                                                Font-Bold="True" ForeColor="#3333FF" OnClick="btn_Click" />
                                            
                                            <asp:Button ID="btn_download" runat="server" Text="Export Excel"
                                                Font-Bold="True" ForeColor="#3333FF" OnClick="btn_download_Click"/>
                                            
                                            <%--<asp:Button ID="Button1" runat="server" Text="Detail"
                                                Font-Bold="True" ForeColor="#3333FF"/>--%>
                                        </td>
                                    </tr>
                                    <tr>
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
                    <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="457px" Width="1095px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
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
