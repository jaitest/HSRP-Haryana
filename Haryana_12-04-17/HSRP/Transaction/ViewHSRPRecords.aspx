<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="ViewHSRPRecords.aspx.cs" Inherits="HSRP.Transaction.ViewHSRPRecords" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <script type="text/javascript">
        function editpage(i) { //Define arbitrary function to run desired DHTML Window widget codes
            //  alert(i);
            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPRecords.aspx?Mode=Edit&HSRPRecordID=" + i, "Update HSRP Record Details", "width=1050px,height=585px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }


        function LaserAssignViewpage(i) { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCodeView.aspx?Mode=Edit&HSRPRecordID=" + i, "View HSRP Record Details", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }

        function PrintChalan(i, S) {
            //Define arbitrary function to run desired DHTML Window widget codes
            //            alert("Hello");
            googlewin = dhtmlwindow.open("googlebox", "iframe", "PrintChalan.aspx?Mode=PrintChalan&HSRPRecordID=" + i + "&Status=" + S + "", "Print Invoice", "width=400px,height=75px,resize=1,scrolling=1,center=2", "recal")
            //            googlewin.onclose = function () {
            //                window.location.href = "ViewHSRPRecords.aspx";
            //                return true;
            //            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPRecords.aspx?Mode=New", "Add HSRP Record Datails", "width=1050px,height=585px,resize=1,scrolling=1,center=1", "recal")
            //            googlewin.onclose = function () {
            //                window.location = 'ViewHSRPRecords.aspx';
            //                return true;
            //            }
        }
    </script>
    <%--<script type="text/javascript" language="javascript">
        function ConfirmPDF() {
           {
            document.getElementById("ctl00_ContentPlaceHolder1_printflag").value = "1";
            return true;
            
        } 
        function ConfirmCashReceipt() {
            //if (confirm("Are you really want to Print Cash Receipt?")) {
            document.getElementById("ctl00_ContentPlaceHolder1_printflag").value = "3";
            return true;
            //            }
            //            else {
            //                return false;
            //            }

        }

        function ConfirmInvoice() {
            //if (confirm("Are you really want to Print Invoice?")) {
            document.getElementById("ctl00_ContentPlaceHolder1_printflag").value = "2";
            return true;
            //            }
            //            else {
            //                return false;
            //            }

        }

    </script>--%>
    <script type="text/javascript" language="javascript">

        
        function validate() {

            if (document.getElementById("<%=dropDownListHSRPState.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=dropDownListHSRPState.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropDownListRTOLocation.ClientID%>").value == "--Select RTO Location--") {
                alert("Please Select RTOLocation");
                document.getElementById("<%=dropDownListRTOLocation.ClientID%>").focus();
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
                                        <span class="headingmain">HSRP Records </span>
                                        <%--<asp:HiddenField ID="printflag" runat="server"></asp:HiddenField>--%>
                                    </td>
                                    <%--  <td align="right"></td>--%>
                                    <td width="300px" height="26" align="right" nowrap="nowrap" style="padding-right: 10px; color: Black">
                                        Search By
                                        <asp:RadioButton ID="RadioButtonOrderDate" runat="server" Text="Order Date" Checked="true"
                                            GroupName="date" />
                                        <asp:RadioButton ID="RadioButtonAuthorizationDate" runat="server" Text="Authorization Date"
                                            GroupName="date" />
                                    </td>
                                    <td  align="right">
                                        <asp:Label ID="Label3" ForeColor="Red" CssClass="RTOLabel" runat="server"> Search By Vehicle Reg.No.  </asp:Label>
                                        <asp:TextBox runat="server" Width="120px" ID="textboxSearch"></asp:TextBox>
                                        <asp:LinkButton ID="Linkbutton1" runat="server" Text="GO" class="button" 
                                            OnClick="Linkbutton1_Click" />
                                    </td>
                                    <%--    <td align="right">
                                    
                                    </td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="left" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Select HSRP State:" Visible="false" runat="server" ID="labelHSRPState" />
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="false" CausesValidation="false" ID="dropDownListHSRPState"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="dropDownListHSRPState_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Location:" Visible="false" runat="server" ID="labelRTOLocation" />&nbsp;&nbsp;
                                                <asp:DropDownList Visible="false" ID="dropDownListRTOLocation" CausesValidation="false"
                                                    runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="dropDownListHSRPState" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListRTOLocation" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <%--  <td style="margin-left: -50px">
                                        <div align="right" style="width: 40px;">
                                            <asp:LinkButton ID="ButtonGo" runat="server" Text="GO" class="button" OnClick="ButtonGo_Click" />
                                        </div>
                                    </td>--%>
                                    <td>
                                        <div id="pdfexl" runat="server">
                                            <td class="form_text" valign="middle">
                                                <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />
                                            </td>
                                            <td valign="middle" onmouseup="OrderDate_OnMouseUp()" align="left">
                                                <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                    ControlType="Picker" PickerCssClass="picker">
                                                    <ClientEvents>
                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                    </ClientEvents>
                                                </ComponentArt:Calendar>
                                            </td>
                                            <td valign="middle">
                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                            </td>
                                            <td valign="middle" align="left">
                                            </td>
                                            <td class="form_text" valign="middle">
                                                <asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" />
                                            </td>
                                            <td valign="middle" onmouseup="HSRPAuthDate_OnMouseUp()" align="left">
                                                <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                    ControlType="Picker" PickerCssClass="picker">
                                                    <ClientEvents>
                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                    </ClientEvents>
                                                </ComponentArt:Calendar>
                                            </td>
                                            <td valign="middle">
                                                <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                    class="calendar_button" src="../images/btn_calendar.gif" />
                                            </td>
                                            <td valign="middle" align="left">
                                            </td>
                                            <td style="margin-left: -50px" valign="middle">
                                                <div align="right" style="width: 40px; height: 16px;">
                                                    <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button" OnClientClick=" return validate()"
                                                        OnClick="LinkbuttonSearch_Click" />
                                                </div>
                                            </td>
                                            <td valign="middle">
                                                <asp:LinkButton ID="ButtonPDF" runat="server" Text="PDF" class="button" OnClientClick=" return validate()" OnClick="ButtonPDF_Click" />
                                            </td>
                                            <td valign="middle">
                                                <asp:LinkButton ID="ButtonExcel" runat="server" Text="Excel" class="button" OnClientClick=" return validate()"
                                                    OnClick="ButtonExcel_Click" />
                                                
                                            </td>
                                        </div>
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
                                    </td>
                                    <%--<td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;"  title="Add New HSRP Record" class="button">Add
                                            HSRP Record</a>
                                    </td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <ComponentArt:Grid ID="Grid1" ClientIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
                                            Width="100%" GroupingNotificationText="Drag a column to this area to group by it."
                                            LoadingPanelPosition="MiddleCenter" LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                            GroupBySortImageHeight="10" GroupBySortImageWidth="10" GroupBySortDescendingImageUrl="group_desc.gif"
                                            GroupBySortAscendingImageUrl="group_asc.gif" GroupingNotificationTextCssClass="GridHeaderText"
                                            IndentCellWidth="22" TreeLineImageHeight="19" TreeLineImageWidth="22" TreeLineImagesFolderUrl="~/images/lines/"
                                            PagerImagesFolderUrl="~/images/pager/" PreExpandOnGroup="true" GroupingPageSize="5"
                                            SliderPopupClientTemplateId="SliderTemplate" SliderPopupOffsetX="20" SliderGripWidth="9"
                                            SliderWidth="150" SliderHeight="20" PagerButtonHeight="22" PagerButtonWidth="41"
                                            PagerTextCssClass="GridFooterText" PageSize="25" GroupByTextCssClass="GroupByText"
                                            GroupByCssClass="GroupByCell" FooterCssClass="GridFooter" HeaderCssClass="GridHeader"
                                            SearchOnKeyPress="true" SearchTextCssClass="GridHeaderText" ShowSearchBox="true"
                                            ShowHeader="true" CssClass="Grid" RunningMode="Client" SearchBoxPosition="TopLeft"
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px" OnItemCommand="Grid1_ItemCommand">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="HSRPRecordID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="OrderStatus" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="HSRPRecordID" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="OrderDate" HeadingText="Data Entry Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationNo" HeadingText="Authorization No."
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationDate" HeadingText="Authorization Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                      <%--  <ComponentArt:GridColumn DataField="OrderEmbossingDate" HeadingText="Embossing Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />--%>
                                                        <ComponentArt:GridColumn DataField="InvoiceDateTime" HeadingText="Invoice Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="OwnerName" HeadingText="Owner Name" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="VehicleRegNo" Visible="true" HeadingText="Reg No."
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="ChassisNo" HeadingText="Chassis No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="EngineNo" HeadingText="Engine No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="VehicleType" HeadingText="Vehicle Type" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                       <%-- <ComponentArt:GridColumn DataField="MobileNo" HeadingText="Contact No" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />--%>
                                                        <ComponentArt:GridColumn DataField="HSRP_Front_LaserCode" HeadingText="Front Laser Code"
                                                            AllowGrouping="False" SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                        <ComponentArt:GridColumn DataField="HSRP_Rear_LaserCode" HeadingText="Rear Laser Code"
                                                            AllowGrouping="False" SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                        <ComponentArt:GridColumn DataField="NetAmount" HeadingText="Net Amount" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" FormatString="#.#,0" />
                                                        <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="ViewDetail"
                                                            HeadingText="ViewDetail" SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                       <%-- <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="CashReceipt"
                                                            HeadingText="CashReceipt" SortedDataCellCssClass="SortedDataCell" Width="100" />--%>
                                                        <ComponentArt:GridColumn DataCellClientTemplateId="DeliveryChallan" HeadingText="Invoice"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="CashReceipt">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonCashReceipt" runat="server" Text="Cash Receipt" CommandName="CashReceipt"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>
                                            <%--<ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="Invoice">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonInvoice" 
                                                            runat="server" Text="Invoice" CommandName="Invoice"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>--%>
                                            <%--<ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="DeliveryChallan">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonStatus" OnClientClick="javascript:return ConfirmPDF();"
                                                            runat="server" Text="Delivery Challan" CommandName="DeliveryChallan"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>--%>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="ViewDetail">
                                                    <a style="color: Blue" onclick="javascript:LaserAssignViewpage(## DataItem.GetMember('HSRPRecordID').Value ##);">
                                                        ViewDetail</a>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="DeliveryChallan">
                                                    <a style="color: Blue" onclick="javascript:PrintChalan(## DataItem.GetMember('HSRPRecordID').Value ##,'## DataItem.GetMember('OrderStatus').Value ##');">
                                                        Invoice</a></ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate" runat="server">
                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td style="font-size: 10px;">
                                                                Loading...&nbsp;
                                                            </td>
                                                            <td>
                                                                <img alt="loading" src="/Images/spinner.gif" width="16" height="16" border="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="SliderTemplate" runat="server">
                                                    <table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" style="padding: 5px;">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="25" align="center" valign="top" style="padding-top: 3px;">
                                                                        </td>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="2" border="0" style="width: 255px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 115px;">
                                                                                            <b>Code :</b><nobr>## DataItem.GetMember(&#39;UserID&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 135px;">
                                                                                            <b>Name :</b><nobr>## DataItem.GetMember(&#39;UserLoginName&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            Page <b>## DataItem.PageIndex + 1 ##</b> of <b>## Grid1.PageCount ##</b>
                                                                        </td>
                                                                        <td align="right">
                                                                            Record <b>## DataItem.Index + 1 ##</b> of <b>## Grid1.RecordCount ##</b>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
