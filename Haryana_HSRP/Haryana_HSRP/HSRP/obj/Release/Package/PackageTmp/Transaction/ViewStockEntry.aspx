<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewStockEntry.aspx.cs" Inherits="HSRP.Transaction.ViewStockEntry" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
<script type="text/javascript">


    function edit(i) { // Define This function of Send Assign Laser ID 

        googlewin = dhtmlwindow.open("googlebox", "iframe", "StockEntryNew.aspx?Mode=Edit&ID=" + i, "Edit Imperest Details", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
           // window.location.href = "ViewImperest.aspx";
            return true;
        }
    }

    function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

        googlewin = dhtmlwindow.open("googlebox", "iframe", "StockEntryNew.aspx?Mode=New", "Stock Entry New", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
           // window.location = 'ViewImperest.aspx';
            return true;
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
<table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View Stock Entry </span>
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
                                    <%--<td valign="left" class="form_text" >
                                        <asp:Label Text="HSRP State:" Visible="false" runat="server" ID="labelOrganization" /> &nbsp; &nbsp;
                                        <asp:DropDownList AutoPostBack="true" Visible="false" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged"  >
                                        </asp:DropDownList>
                                    </td>--%>
                                    <td align="left" valign="middle">
                                        
                                    </td>
                                    <%--<td valign="left" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateClient" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="RTO Location:" Visible="false" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                             <asp:DropDownList AutoPostBack="true" Visible="false" ID="dropDownListClient" CausesValidation="false"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID" 
                                                    onselectedindexchanged="dropDownListClient_SelectedIndexChanged1">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers> 
                                                <asp:PostBackTrigger ControlID="dropDownListClient" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                    <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New HSRP State" class="button">Add Stock Entry</a>
                                    </td>

                                    

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                                    <td >
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
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
                                             <asp:DropDownList AutoPostBack="true" Visible="true" ID="dropDownListClient" 
                                                    CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID" onselectedindexchanged="dropDownListClient_SelectedIndexChanged1" 
                                                    >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient" /> 
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap"> <asp:Label Text="User" Visible="false" runat="server" ID="labelOrderStatus" />&nbsp;&nbsp; </td>
                                    <td> <asp:DropDownList AutoPostBack="false" Visible="false" 
                                            ID="dropDownListUser" DataTextField="Names" DataValueField="UserID" CausesValidation="false" runat="server"  >
                                         
                                                </asp:DropDownList></td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">&nbsp;&nbsp; <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" /> &nbsp;&nbsp;</td>
                                    <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left">
                                                                <componentart:calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </componentart:calendar>
                                                            </td>
                                                           
                                                            <td valign="top" align="left">
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>
                                   <td valign="middle" class="form_text" nowrap="nowrap"> &nbsp;&nbsp;<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" /> &nbsp;&nbsp;</td>
                                   <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left">
                                                                <componentart:calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </componentart:calendar>
                                                            </td>
                                                      
                                                            <td valign="top" align="left">
                                                                <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                                    class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>


                                   

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button" OnClientClick=" return validate()" onclick="ButtonGo_Click" /> &nbsp;  
                                            &nbsp;  

                                            &nbsp;
                                        </td>
                                         <td style="position: relative; right: 0px; top: 0px;">
                                                    <asp:Button ID="btnExportToExcel" runat="server" Text="Report In Excel" ToolTip="Please Click for Report"
                                                        class="button" OnClientClick=" return validate()" OnClick="btnExportToExcel_Click" />
                                                </td>

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                </tr>
                                <tr>
                                <td colspan="9">
                                                        <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                                    </td>
                                </tr>
                            </table>
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
                                            Height="300px">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="stockPlateID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                       <ComponentArt:GridColumn DataField="stockPlateID" Visible="False" />

                                                        <ComponentArt:GridColumn DataField="UserName" HeadingText="User Name" SortedDataCellCssClass="SortedDataCell" Width="170" />
                                                                                                                    
                                                        <ComponentArt:GridColumn DataField="RecordDate" HeadingText="Date" SortedDataCellCssClass="SortedDataCell" Width="120" />
                                                            
                                                        <ComponentArt:GridColumn DataField="EmbosedPlate285_45Yellow" FormatString="" HeadingText="285X45 Yellow" SortedDataCellCssClass="SortedDataCell" Width="80" />

                                                        <ComponentArt:GridColumn DataField="BlanckPlate_340_200Yellow" HeadingText="340X200 Yellow" SortedDataCellCssClass="SortedDataCell"
                                                            Width="400" />
                                                        <ComponentArt:GridColumn DataField="BlanckPlate_200_100White" HeadingText="200X100 White" SortedDataCellCssClass="SortedDataCell"
                                                            Width="400" />
                                                         
                                                        <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="Assign"  HeadingText="View" SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                       
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientTemplates>

                                                   <ComponentArt:ClientTemplate ID="Assign">
                                                    <a style="color: Blue" onclick="javascript:edit(## DataItem.GetMember('stockPlateID').Value ##);" >
                                                        View </a>
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
                                                        <asp:LinkButton ID="LinkButtonSticker" runat="server" Text="View" CommandName="Sticker"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>
                                            <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="TVSSticker">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonStickerTVS" runat="server" Text="View" CommandName="TVSSticker"></asp:LinkButton>
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

</asp:Content>
