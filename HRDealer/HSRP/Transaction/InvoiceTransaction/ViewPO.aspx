<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewPO.aspx.cs" Inherits="HSRP.Master.InvoiceMaster.ViewPO" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
 <script type="text/javascript" src="../../windowfiles/dhtmlwindow.js"></script>
<link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />

<script type="text/javascript">
    function edit(i) { // Define This function of Send Assign Laser ID


        googlewin = dhtmlwindow.open("googlebox", "iframe", "POorder.aspx?Mode=Edit&POheaderID1=" + i, "Update PO Order", "width=1000px,height=500px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
            window.location.href = "ViewPO.aspx";
            return true;
        }
    }

    function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

        googlewin = dhtmlwindow.open("googlebox", "iframe", "POorder.aspx?Mode=New", "Add New PO Order", "width=1000px,height=500px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
            window.location = 'ViewPO.aspx';
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
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View PO</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader" align="right">
                                <tr>
                               <td style=" width:500px;">
                               
                               </td>
                                      
                                            <td class="form_text" valign="middle" style=" width:60px;">
                                                <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />
                                            </td>
                                            <td valign="middle" onmouseup="OrderDate_OnMouseUp()" align="center" style=" margin-left:50px; width:150px;">
                                                <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy" 
                                                    ControlType="Picker" PickerCssClass="picker">
                                                    <ClientEvents>
                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                    </ClientEvents>
                                                </ComponentArt:Calendar>
                                            </td>
                                                <td align="left" style="width:100px;">
                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../../images/btn_calendar.gif" />
                                            </td>
                                        
                                         <td style="margin-left: -50px ;width:150px;" align="left" >
                                                <div  style="width: 40px;">
                                                    <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button" OnClick="LinkbuttonSearch_Click" />
                                                </div>
                                            </td>
                                            <td style="margin-left: -50px ;width:150px;" align="left" >
                                            <%--  <asp:LinkButton ID="btnExportToPDF" runat="server" Text="pdf" class="button" OnClick="btnExportToPDF_Click" />--%>
                                            </td>
                                        
                                      
                                    
                                            <td colspan="6">
                                                <ComponentArt:Calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                                                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                    PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                    ImagesBaseUrl="../../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                    <ClientEvents>
                                                        <SelectionChanged EventHandler="OrderDate_OnChange" />
                                                    </ClientEvents>
                                                </ComponentArt:Calendar>
                                            </td>
                                       
                                      
                                           
                                   
                                    <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New HSRP State" class="button">Add Purchase Order </a>
                                    </td>
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
                                       <%-- <asp:GridView ID="Grid1" runat="server">
                                        </asp:GridView>--%>
                                        <ComponentArt:Grid ID="Grid1" CustomerIDMode="POheaderID" runat="server" ImagesBaseUrl="~/images"
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
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px"  onitemcommand="Grid1_ItemCommand1">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="POheaderID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                   <Columns>
                                                       <ComponentArt:GridColumn DataField="POheaderID" Visible="False" />
                                                       
                                                        <ComponentArt:GridColumn DataField="POheaderID" HeadingText="PO No"
                                                            SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                            <ComponentArt:GridColumn DataField="Name" Visible="true" HeadingText="Vendor Name"
                                                            Width="100" />
                                                    <%--   <ComponentArt:GridColumn DataField="BillingAddress" Visible="true" HeadingText="Billing Address"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="ShippingAddress" Visible="true" HeadingText="Shipping Address"
                                                            Width="100" />--%>
                                                            
                                                             <ComponentArt:GridColumn DataField="PaymentTerm" Visible="true" HeadingText="Payment Term"
                                                            Width="100" />
                                                         <ComponentArt:GridColumn DataField="Status" Visible="true" HeadingText="Status"
                                                            Width="100" />

                                                              <ComponentArt:GridColumn DataField="counts" Visible="true" HeadingText="Inline Value"
                                                            Width="100" />

                                                        <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="edit" HeadingText="Edit" SortedDataCellCssClass="SortedDataCell" Width="50" /> 

                                                        <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="PO" HeadingText="PDF PO" SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                       
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientTemplates>
                                               
                                                   <ComponentArt:ClientTemplate ID="edit">
                                                    <a style="color: Red" onclick="javascript:edit(## DataItem.GetMember('POheaderID').Value ##);">
                                                        Edit</a>
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
                                                <ComponentArt:GridServerTemplate ID="PO">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonPO" runat="server" Text="Generate PO" CommandName="PurchaseOrder"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>
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
