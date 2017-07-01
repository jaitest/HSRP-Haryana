<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DailyCashCollectionRTOLocationWise_WithDate.aspx.cs" Inherits="HSRP.Report.DailyCashCollectionRTOLocationWise_WithDate" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">

        function OrderDate_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDate.getSelectedDate();
            CalendarOrderDate.setSelectedDate(fromDate);

        }

        function OrderDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDate.getSelectedDate();
            OrderDate.setSelectedDate(fromDate);

        }

        function OrderDate_OnClick() {
            if (CalendarOrderDate.get_popUpShowing()) {
                CalendarOrderDate.hide();
            }
            else {
                CalendarOrderDate.setSelectedDate(OrderDate.getSelectedDate());
                CalendarOrderDate.show();
            }
        }

        function OrderDate_OnMouseUp() {
            if (CalendarOrderDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function HSRPAuthDate_OnDateChange(sender, eventArgs) {
            var fromDate = HSRPAuthDate.getSelectedDate();
            CalendarHSRPAuthDate.setSelectedDate(fromDate);

        }

        function CalendarHSRPAuthDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarHSRPAuthDate.getSelectedDate();
            HSRPAuthDate.setSelectedDate(fromDate);

        }

        function HSRPAuthDate_OnClick() {
            if (CalendarHSRPAuthDate.get_popUpShowing()) {
                CalendarHSRPAuthDate.hide();
            }
            else {
                CalendarHSRPAuthDate.setSelectedDate(HSRPAuthDate.getSelectedDate());
                CalendarHSRPAuthDate.show();
            }
        }

        function HSRPAuthDate_OnMouseUp() {
            if (CalendarHSRPAuthDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script language="javascript" type="text/javascript">



        function validate() {

            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "Select Location--") {
                alert("Select Location");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
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
                                        <span class="headingmain">Daily Cash Collection Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td>
                                        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                            <tr>
                                                <td>
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap">
                                                    <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                                </td>
                                                <td valign="middle">
                                                    <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td valign="middle" class="form_text">
                                                    <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                                            <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false"
                                                                runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                            <asp:PostBackTrigger ControlID="dropDownListClient" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap">Vehicle Reference&nbsp;
                                                    <asp:DropDownList ID="ddlVehicleReference" runat="server">
                                                        <asp:ListItem>Both</asp:ListItem>
                                                        <asp:ListItem>New</asp:ListItem>
                                                        <asp:ListItem>Old</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap">Vehicle Type&nbsp; 
                                                    <asp:DropDownList ID="DropDownListVehicleModel" runat="server" 
                                                        AutoPostBack="true" CausesValidation="false" Font-Size="Small" 
                                                        Style="margin-left: 8px" TabIndex="13" Width="150px">
                                                        <asp:ListItem>All</asp:ListItem>
                                                        <asp:ListItem Text="SCOOTER" Value="SCOOTER"></asp:ListItem>
                                                        <asp:ListItem Text="MOTOR CYCLE" Value="MOTOR CYCLE"></asp:ListItem>
                                                        <asp:ListItem Text="TRACTOR" Value="TRACTOR"></asp:ListItem>
                                                        <asp:ListItem Text="THREE WHEELER" Value="THREE WHEELER"></asp:ListItem>
                                                        <asp:ListItem Text="LMV" Value="LMV"></asp:ListItem>
                                                        <asp:ListItem Text="LMV(CLASS)" Value="LMV(CLASS)"></asp:ListItem>
                                                        <asp:ListItem Text="MCV/HCV/TRAILERS" Value="MCV/HCV/TRAILERS"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap">
                                                    &nbsp;
                                                    <asp:Label Text="From:" runat="server" ID="labelDate" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left">
                                                    <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                                <td valign="top" align="left">
                                                    <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap">
                                                    &nbsp;&nbsp;<asp:Label Text="To:" runat="server" ID="labelTO" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left">
                                                    <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                                <td valign="top" align="left">
                                                    <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                        class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                 <asp:Button ID="Button1" runat="server" 
                                                        Text=" All Records" ToolTip="Please Click for Report"
                                                        class="button"  OnClick="Button1_Click" />
                                                    &nbsp;</td>
                                                    <td>
                                               <asp:Button ID="btnExportToExcel" runat="server" 
                                                        Text="Records With Complete Info" ToolTip="Please Click for Report"
                                                        class="button"  OnClick="btnExportToExcel_Click" />
                                            
                                                </td>
                                                <td>
                                                  <asp:Button ID="btnAllLocationPdf" runat="server" 
                                                        Text="For All Location" ToolTip="Please Click for Report"
                                                        class="button" onclick="btnAllLocationPdf_Click"/>
                                                </td>


                                                <td>
                                                  <asp:Button ID="btnpdf" runat="server" 
                                                        Text="For All Location PDF" ToolTip="Please Click for Report"
                                                        class="button" onclick="btnpdf_Click"/>
                                                </td>
                                                <tr>
                                                    <td colspan="9">
                                                        <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                                    &nbsp;
                                                    </td>
                                                </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <ComponentArt:Calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                                AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                <ClientEvents>
                                    <SelectionChanged EventHandler="OrderDate_OnChange" />
                                </ClientEvents>
                            </ComponentArt:Calendar>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <ComponentArt:Calendar runat="server" ID="CalendarHSRPAuthDate" AllowMultipleSelection="false"
                                AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                PopUp="Custom" PopUpExpandControlId="ImgPollution" CalendarTitleCssClass="title"
                                DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                <ClientEvents>
                                    <SelectionChanged EventHandler="CalendarHSRPAuthDate_OnChange" />
                                </ClientEvents>
                            </ComponentArt:Calendar>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
