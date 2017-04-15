<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="PendingOrder.aspx.cs" Inherits="HSRP.Master.PendingOrder" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">

        function editAssignLaser(i) { // Define This function of Send Assign Laser ID 
            //alert("AssignLaser" + i);
//            var usertype = document.getElementById('username').value;
//            alert(usertype);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCode.aspx?Mode=Edit&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () { 
                //window.location.href = "AssignLaserCode.aspx";
                return true;
            }
        }

        function editMakeLaserFree(i) { // Define This function of Send Assign Laser ID 
        // alert("MakeLaserFree" + i);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserCodeEmbossing.aspx?Mode=Embossing&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
               // window.location.href = "AssignLaserCode.aspx";
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


        function editEmbossing(i) { // Define This function of Send Assign Laser ID 
          //  alert("Embossing" + i);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserCodeMakeFree.aspx?Mode=Embossing&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
              //  window.location.href = "AssignLaserCode.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCode.aspx?Mode=New", "Add Secure Devices User", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
               // window.location = 'AssignLaserCode.aspx';
                return true;
            }
        }
    </script>
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
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) {

                return true;
            }
            else {
                return false;
            }

        }
    </script>

    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select Location--") {
                alert("Select RTO Name");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=OrderDate.ClientID%>").value == "") {
                alert("Please Provide Date1");
                document.getElementById("<%=OrderDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=HSRPAuthDate.ClientID%>").value == "") {
                alert("Please Provide Date2");
                document.getElementById("<%=HSRPAuthDate.ClientID%>").focus();
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
                                        <span class="headingmain">Pending Orders </span>
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
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                    </td>

                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged"  >
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateClient" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                             <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID" 
                                                    onselectedindexchanged="dropDownListClient_SelectedIndexChanged1"   >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers> 
                                                <asp:PostBackTrigger ControlID="dropDownListClient" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>

                                    <td  class="form_text"> <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" /> </td>
                                    <td valign="top" onmouseup="OrderDate_OnMouseUp()">
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
                                                            <td valign="top">
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>  
                                    <td  class="form_text"> <asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" /> </td>
                                   <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()">
                                                                <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                                    class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>
                                    
                                    <td> &nbsp;</td>
                                    <td>
                                        <asp:Button ID="ButtonGo" runat="server" Text="GO" class="button"  OnClientClick=" return validate()"
                                            onclick="ButtonGo_Click" /> 
                                    </td>
                                    <td> 
                                        
                                        <asp:Button ID="btnExportToExcel" runat="server" Text="Export To Excel" 
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnExportToExcel_Click"  />
                                    </td>

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
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
                                     <td><asp:Label ID="LabelError" runat="server" ForeColor="Red" Font-Size="15px"></asp:Label></td>
                                </tr>
                                <tr>
                               
                                    <td colspan="4">
                                        <ComponentArt:Grid ID="Grid1" RTOLocationIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
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
                                            ShowHeader="true" CssClass="Grid" RunningMode="Callback" SearchBoxPosition="TopLeft"
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="HSRPRecordID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                       <ComponentArt:GridColumn DataField="HSRPRecordID" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="OrderDate" HeadingText="Order Date" SortedDataCellCssClass="SortedDataCell" Width="60" /> 
                                                        <ComponentArt:GridColumn DataField="DueDate" HeadingText="DueDate" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" /> 
                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationNo" HeadingText="Authorization No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="120" />
                                                            
                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationDate" HeadingText="Authorization Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                            
                                                        <ComponentArt:GridColumn DataField="OrderEmbossingDate" HeadingText="Embossing Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                            
                                                        <ComponentArt:GridColumn DataField="InvoiceDateTime" HeadingText="Invoice Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />

                                                        <ComponentArt:GridColumn DataField="OwnerName" HeadingText="Owner Name" SortedDataCellCssClass="SortedDataCell" Width="130" />
                                                        <ComponentArt:GridColumn DataField="VehicleType" HeadingText="Vehicle Type" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> 
                                                        <ComponentArt:GridColumn DataField="VehicleClass" HeadingText="Vehicle Class" SortedDataCellCssClass="SortedDataCell" Width="130" />
                                                        <ComponentArt:GridColumn DataField="MobileNo" HeadingText="Mobile No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> 
                                                        <ComponentArt:GridColumn DataField="VehicleRegNo" Visible="true" HeadingText="Vehicle Reg.No." Width="100" />  
                                                         
                                                        <ComponentArt:GridColumn DataField="ChassisNo" HeadingText="Chassis No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> 
                                                        <ComponentArt:GridColumn DataField="EngineNo" HeadingText="Engine No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="70" />
                                                        
                                                        <ComponentArt:GridColumn DataField="HSRP_Front_LaserCode" HeadingText="Front Laser Code" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />


                                                        <ComponentArt:GridColumn DataField="HSRP_Rear_LaserCode" HeadingText="Rear Laser Code" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                            
                                                        <ComponentArt:GridColumn DataField="orderStatus" HeadingText="Order Status" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />

                                                            <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="ViewDetail"  HeadingText="ViewDetail" SortedDataCellCssClass="SortedDataCell" Width="90" />
  
                                                        <%--<ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="Assign"  HeadingText="Assign" SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                        <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="LaserFree" HeadingText="Laser Free" SortedDataCellCssClass="SortedDataCell" Width="70" />--%>
                                                       <%-- <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="Embossing" HeadingText="Embossing" SortedDataCellCssClass="SortedDataCell" Width="50" />--%>

                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientTemplates>
                                                 <ComponentArt:ClientTemplate ID="ViewDetail">
                                                    <a style="color: Blue" onclick="javascript:LaserAssignViewpage(## DataItem.GetMember('HSRPRecordID').Value ##);" >
                                                        ViewDetail</a>
                                                   </ComponentArt:ClientTemplate>

                                                   <ComponentArt:ClientTemplate ID="Assign">
                                                    <a style="color: Blue" onclick="javascript:editAssignLaser(## DataItem.GetMember('HSRPRecordID').Value ##);">
                                                        AssignLaser</a>
                                                   </ComponentArt:ClientTemplate> 
                                                   <ComponentArt:ClientTemplate ID="LaserFree">
                                                    <a style="color: Red" onclick="javascript:editMakeLaserFree(## DataItem.GetMember('HSRPRecordID').Value ##);">
                                                        MakeLaserFree</a>
                                                   </ComponentArt:ClientTemplate>

                                                   <ComponentArt:ClientTemplate ID="Embossing">
                                                    <a style="color: Blue" onclick="javascript:editEmbossing(## DataItem.GetMember('HSRPRecordID').Value ##);">
                                                        Embossing</a>
                                                   </ComponentArt:ClientTemplate>

                                                <ComponentArt:ClientTemplate ID="ClientTemplate2" runat="server">
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
                                                <ComponentArt:ClientTemplate ID="ClientTemplate3" runat="server">
                                                    <table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" style="padding: 5px;">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="25" align="center" valign="top" style="padding-top: 3px;">
                                                                        </td>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="2" border="0" style="width: 255px;">
                                                                                <%--<tr>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 115px;">
                                                                                            <b>Code :</b><nobr>## DataItem.GetMember(&#39;QuestionID&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 135px;">
                                                                                            <b>Name :</b><nobr>## DataItem.GetMember(&#39;OrgName&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                </tr>--%>
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
            </td>
        </tr>
    </table>
    <br /><asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
