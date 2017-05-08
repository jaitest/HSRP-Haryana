<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DailyHREmbossingReport.aspx.cs" Inherits="HSRP.Report.DailyHREmbossingReport" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script language="javascript" type="text/javascript">



        function validate() {


            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
        } 
    </script>
    <%--<td style="color: #828282; font: 11pt tahoma,arial,verdana; text-decoration: none; width: 97px;";> 
                                    <asp:Label Text="From Date:" Visible="true" runat="server" ID="labelDate" /> </td>
                                    <td valign="top" style="width:105px"; onmouseup="OrderDate_OnMouseUp()">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top" style="width:44px";>
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>  


                                                            <td style="color: #828282; font: 11pt tahoma,arial,verdana; text-decoration: none; width: 97px;";> 
                                    <asp:Label Text="To Date:" Visible="true" runat="server" ID="label1" /> </td>
                                    <td valign="top" style="width:105px"; onmouseup="OrderDate_OnMouseUp()">
                                                                <ComponentArt:Calendar ID="Calendar1" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top" style="width:44px";>
                                                                <img id="Img1" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>  --%>
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
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">HARYANA : Daily Report from Embossing Stations to Registering Authority</span>
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
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                    <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                                </td>
                                                <td valign="middle" style="width: 105px">
                                                    <asp:DropDownList Visible="true" CausesValidation="false" 
                                                        ID="DropDownListStateName" AutoPostBack="true"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                                        onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                 <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:UpdatePanel runat="server" ID="UpdateClient" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                             <asp:DropDownList AutoPostBack="true" Visible="true" ID="dropDownListClient" CausesValidation="false"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>                                                 
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" 
                                                    EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                                </td>

                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                                <%--<td style="color: #828282; font: 11pt tahoma,arial,verdana; text-decoration: none; width: 97px;";> 
                                    <asp:Label Text="From Date:" Visible="true" runat="server" ID="labelDate" /> </td>
                                    <td valign="top" style="width:105px"; onmouseup="OrderDate_OnMouseUp()">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top" style="width:44px";>
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>  


                                                            <td style="color: #828282; font: 11pt tahoma,arial,verdana; text-decoration: none; width: 97px;";> 
                                    <asp:Label Text="To Date:" Visible="true" runat="server" ID="label1" /> </td>
                                    <td valign="top" style="width:105px"; onmouseup="OrderDate_OnMouseUp()">
                                                                <ComponentArt:Calendar ID="Calendar1" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top" style="width:44px";>
                                                                <img id="Img1" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>  --%>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 70px">
                                                    &nbsp;&nbsp;
                                                    <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="middle" onmouseup="OrderDate_OnMouseUp()" align="left" style="width: 120px">
                                                    <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                                <td valign="middle" align="left" style="width: 39px">
                                                    <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 58px">
                                                    &nbsp;&nbsp;<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="middle" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" 
                                                    style="width: 87px">
                                                    <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                                <td valign="middle" align="left">
                                                    <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                        class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>
                                                <td style="position: relative; right: 0px; top: 0px;" width="500px"nowrap="nowrap">
                                                    <asp:Button ID="btnGo" runat="server" Text="Go" ToolTip="Please Click for Report"
                                                        class="button"  OnClick="btnGo_Click" Width="100px" />
                                                    <asp:Button ID="btnExportToExcel" runat="server" Text="Report In Excel" ToolTip="Please Click for Report"
                                                        class="button"  OnClick="btnExportToExcel_Click" Width="100px" 
                                                        Visible="False" />
                                                    <asp:Button ID="btnExportToPDF" runat="server" Text="Report In PDF" ToolTip="Please Click for Report"
                                                        class="button"  OnClick="btnExportToPDF_Click" Width="100px" />

                                                         <asp:Button ID="btnAll" runat="server" Text="All Report In PDF" ToolTip="Please Click for Report"
                                                        class="button"  OnClick="btnAllReportPDF_Click" Width="100px" />
                                                </td>
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                    &nbsp;</td>
                                                <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                    &nbsp;</td>
                                                <td valign="middle" style="width: 105px">
                                                    &nbsp;</td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                    &nbsp;</td>
                                                <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left" style="width: 70px">
                                                    &nbsp;</td>
                                                <td valign="top" align="left">
                                                    &nbsp;</td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 39px">
                                                    &nbsp;</td>
                                                <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" 
                                                        style="width: 58px">
                                                    &nbsp;</td>
                                                <td valign="top" align="left" style="width: 87px">
                                                    &nbsp;</td>
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                    &nbsp;</td>
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                    &nbsp;</td>
                                                <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" colspan="9">
                                                    <asp:Panel ID="panel1" runat="server" Height="480px" ScrollBars="Vertical" 
                                                        Width="100%">
                                                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
                                                            GridLines="Vertical" AllowPaging="True" Font-Size="Small" 
                                                            onpageindexchanging="GridView1_PageIndexChanging" PageSize="15" Width="100%">
                                                            <AlternatingRowStyle BackColor="White" />
                                                            <EditRowStyle BackColor="#2461BF" />
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                                                HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" 
                                                                VerticalAlign="Bottom" />
                                                            <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                        </asp:GridView>
                                                        </asp:Panel>
                                                </td>
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                    &nbsp;</td>
                                                <tr>
                                                    <td colspan="9">
                                                        <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
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
