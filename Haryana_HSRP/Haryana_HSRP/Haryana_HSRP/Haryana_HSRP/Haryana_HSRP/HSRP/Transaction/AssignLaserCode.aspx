<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AssignLaserCode.aspx.cs" Inherits="HSRP.Master.AssignLaserCode" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">


        function editAssignLaser(i) {

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
               //window.location.href = "AssignLaserCode.aspx";
                return true;
            }
        }

        function editEmbossing(i) { // Define This function of Send Assign Laser ID 
          //  alert("Embossing" + i);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserCodeMakeFree.aspx?Mode=Embossing&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
              window.location.href = "AssignLaserCode.aspx";
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
        function validateStatus() {

            if (document.getElementById("<%=dropDownListorderStatus.ClientID%>").value != "Closed") {
                alert("Please Select Close Status");
                document.getElementById("<%=dropDownListorderStatus.ClientID%>").focus();
                return false;
            }
        }
    
    </script>
    
    <script language="javascript" type="text/javascript">
        function validateSearch() {

            if (document.getElementById("<%=textboxSearch.ClientID%>").value == "") {
                alert("Please Provide Search Content");
                document.getElementById("<%=textboxSearch.ClientID%>").focus();
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
            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select RTO Name--") {
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
            if (document.getElementById("<%=dropDownListorderStatus.ClientID%>").value == "-- Select Status --") {
                alert("Select Status");
                document.getElementById("<%=dropDownListorderStatus.ClientID%>").focus();
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
                                    <td >
                                    <asp:Label ID="lblmes" CssClass="headingmain" runat="server">View Laser Code </asp:Label> 
                                     <asp:Label ID="Label1" ForeColor="Black" CssClass="RTOLabel" runat="server"> Search By :</asp:Label> 
                                        <asp:RadioButton ForeColor="Black" ID="RadioButtonCreateion" CssClass="RTOLabel"  Text="Creation Date" runat="server" Checked="true"    GroupName="Check" /> 
                                        <asp:RadioButton ForeColor="Black" ID="radiobuttonAuthorization" CssClass="RTOLabel"  Text="Authorization Date"   runat="server" GroupName="Check" /> 
                                        <asp:RadioButton ForeColor="Black" ID="RadioButtonEmbossingDate" 
                                            CssClass="RTOLabel"  Text="Embossing Date" runat="server" Checked="true"    
                                            GroupName="Check" /> 
                                        <asp:RadioButton ForeColor="Black" ID="radiobuttonOrderClose" CssClass="RTOLabel"  Text="Order Closed Date" runat="server" GroupName="Check" />  
                                         <asp:Label ID="Label2" ForeColor="Black" CssClass="RTOLabel" runat="server"> Select For All Allowed RTO's :  </asp:Label> 
                                        <asp:CheckBox ForeColor="Black" ID="checkboxAll" runat="server" Checked="false" />  
                                        <asp:Label ID="Label3" ForeColor="Red" CssClass="RTOLabel" runat="server"> Search By Vehicle Reg.No.  </asp:Label> 
                                        <asp:TextBox runat="server" Width="120px" ID="textboxSearch"></asp:TextBox> 
                                        <asp:LinkButton ID="Linkbutton1" runat="server" Text="GO" class="button"  
                                             onclick="Linkbutton1_Click"    />
                                    </td>  
                                </tr>
                                <tr id="TRRTOHide" runat="server">
                                    <td > 
                                          <asp:Label ID="dataLabellbl" class="headingmain" runat="server"  >Allowed RTO's :</asp:Label> 
                                          <asp:Label ID="lblRTOCode" class="form_Label_Repeter"  runat="server">Allowed RTO's : </asp:Label> 
                                    </td> 
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> 
                                    </td>
                                    <td valign="middle" class="form_text">
                                       <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate> 
                                             <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID" 
                                                    onselectedindexchanged="dropDownListClient_SelectedIndexChanged1"   >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient" /> 
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp; <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" /> &nbsp;&nbsp;</td>
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
                                   <td valign="middle" class="form_text" nowrap="nowrap"> &nbsp;&nbsp;<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" /> &nbsp;&nbsp;</td>
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


                                   <td valign="middle" class="form_text" nowrap="nowrap"> <asp:Label Text="Order Status" Visible="true" runat="server" ID="labelOrderStatus" />&nbsp;&nbsp; </td>
                                    <td> <asp:DropDownList AutoPostBack="false" Visible="true" 
                                            ID="dropDownListorderStatus" CausesValidation="false" runat="server"  >
                                        <asp:ListItem>-- Select Status --</asp:ListItem>
                                        <asp:ListItem  Value="New Order"  >New Order</asp:ListItem> 
                                        <asp:ListItem  Value="Embossing Done"  >Embossing Done</asp:ListItem>
                                        <asp:ListItem Value="Closed" >Closed</asp:ListItem> 
                                                </asp:DropDownList>&nbsp; </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button"  OnClientClick=" return validate()"
                                            onclick="ButtonGo_Click" /> &nbsp;  
                                            <asp:Button ID="btnExportToExcel"  Width="100px"  runat="server" Text="Report In Excel" ToolTip="Please Click for Report"
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnExportToExcel_Click"    /> &nbsp;  

                                            <asp:Button ID="btnExportToPDF"  Width="100px"  runat="server" Text="Report In PDF" ToolTip="Please Click for Report"
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnExportToPDF_Click"/> &nbsp;
                                        <asp:Button Width="150px" ID="ButImpData" runat="server" class="button" Text="Generate Data For NIC" onclick="ButImpData_Click" OnClientClick=" return validateStatus()" />
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
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td >
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
                                            GroupingNotificationPosition="TopRight" FillContainer="true" 
                                            Height="300px" onitemcommand="Grid1_ItemCommand1">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="HSRPRecordID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                       <ComponentArt:GridColumn DataField="HSRPRecordID" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationNo" HeadingText="Authorization No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="120" />
                                                            
                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationDate" HeadingText="Authorization Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="120" />
                                                            
                                                        <ComponentArt:GridColumn DataField="OrderEmbossingDate" HeadingText="Embossing Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                            
                                                        <ComponentArt:GridColumn DataField="InvoiceDateTime" HeadingText="Invoice Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="120" />

                                                        <ComponentArt:GridColumn DataField="OwnerName" FormatString="" HeadingText="Owner Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="150" />
                                                        <ComponentArt:GridColumn DataField="VehicleRegNo" Visible="true" HeadingText="Vehicle Reg.No."
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationDate" HeadingText="Auth. Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" />
                                                        <ComponentArt:GridColumn DataField="Duedate" HeadingText="DueDate" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" /> 
                                                         <ComponentArt:GridColumn DataField="LaserPlateBoxNo" HeadingText="Plate Box No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" /> 
                                                        <ComponentArt:GridColumn DataField="HSRP_Front_LaserCode" HeadingText="Front Laser Code" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> 
                                                        <ComponentArt:GridColumn DataField="HSRP_Rear_LaserCode" HeadingText=" Rear Laser Code" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />  
                                                        <ComponentArt:GridColumn DataField="MobileNo" HeadingText="Contact No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="OrderStatus" HeadingText="Order Status" SortedDataCellCssClass="SortedDataCell"
                                                            Width="70" />
                                                         
                                                       <%-- <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="Assign"  HeadingText="Assign" SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                        <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="LaserFree" HeadingText="Laser Free" SortedDataCellCssClass="SortedDataCell" Width="70" />--%>
                                                       <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="Sticker" HeadingText="White Sticker" SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                       <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="TVSSticker" HeadingText="Yellow Sticker" SortedDataCellCssClass="SortedDataCell" Width="50" />

                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientTemplates>

                                                   <ComponentArt:ClientTemplate ID="Assign">
                                                    <a style="color: Blue" onclick="javascript:editAssignLaser(## DataItem.GetMember('HSRPRecordID').Value ##);" >
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
                                             <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="Sticker">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonSticker" runat="server" Text="Sticker" CommandName="Sticker"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>
                                            <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="TVSSticker">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonStickerTVS" runat="server" Text="Sticker" CommandName="TVSSticker"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>
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
