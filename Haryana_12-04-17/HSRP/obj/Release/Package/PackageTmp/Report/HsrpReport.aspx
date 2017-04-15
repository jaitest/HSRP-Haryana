<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HsrpReport.aspx.cs" Inherits="HSRP.Report.HsrpReport" %>
<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />





    <script type="text/javascript">

        function OrderDatefrom_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDatefrom.getSelectedDate();
            CalendarOrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDatefrom.getSelectedDate();
            OrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnClick() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                CalendarOrderDatefrom.hide();
            }
            else {
                CalendarOrderDatefrom.setSelectedDate(OrderDatefrom.getSelectedDate());
                CalendarOrderDatefrom.show();
            }
        }

        function OrderDatefrom_OnMouseUp() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function OrderDateto_OnDateChange(sender, eventArgs) {   
            var fromDate = OrderDateto.getSelectedDate();
            CalendarOrderDateto.setSelectedDate(fromDate);

        } 

        function OrderDateto_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDateto.getSelectedDate();
            OrderDateto.setSelectedDate(fromDate);

        } 


        function OrderDateto_OnClick() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                CalendarOrderDateto.hide();
            }
            else {
                CalendarOrderDateto.setSelectedDate(OrderDateto.getSelectedDate());
                CalendarOrderDateto.show();
            }
        }

        

        function OrderDateto_OnMouseUp() {
            if (CalendarOrderDateto.get_popUpShowing()) {
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
                                        <span class="headingmain">HR Weekwise Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            </table>

   
    <table style="width: 78%; height: 51px;">
        <tr>
         <td style="height: 25px;" width="20">
                &nbsp;</td>
            <%--<td align="left" style="width: 142px">
                <asp:Label Text="HR Weekwise Report" runat="server" ID="labelOrganization" 
                                            ForeColor="Black" Font-Bold="True" />
            </td>--%>
          
                                                <td style="width: 34px"> <asp:Label Text="From:" runat="server" ID="labelDate" Font-Bold="True" 
                                                        ForeColor="Black" /> </td>
                                                <td style="width: 116px">
                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                    <ContentTemplate>
                                                                        <ComponentArt:Calendar ID="OrderDatefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    
    ControlType="Picker" PickerCssClass="picker">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="OrderDatefrom_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                <td>
                                                                <img id="calendar_from_button"  alt="" onclick="OrderDatefrom_OnClick()"
                                                                    onmouseup="OrderDatefrom_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" /></td>
                                                <td style="width: 24px"><asp:Label Text="To:" runat="server" ID="labelTO" Font-Bold="True" 
                                                        ForeColor="Black" /> </td>
                                                <td style="width: 108px">
                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                    <ContentTemplate>
                                                                        <ComponentArt:Calendar ID="OrderDateto" runat="server" ControlType="Picker" PickerCssClass="picker" 
                                                                            PickerCustomFormat="dd/MM/yyyy" PickerFormat="Custom">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="OrderDateto_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                <td>
                                                                <img id="ImgPollution" tabindex="4" alt="" onclick="OrderDateto_OnClick()" onmouseup="OrderDateto_OnMouseUp()"
                                                                    class="calendar_button" src="../images/btn_calendar.gif" /></td>
                                                <td>
                                                    <%--<asp:Button ID="btngo" runat="server" onclick="btngo_Click" Text="Get Data" 
                                                        Font-Bold="True" ForeColor="#3333FF" onclientclick="validate()" />--%>
                                                </td>
                                                <td style="height: 59px">
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export To Excel" 
                                                        Font-Bold="True" ForeColor="#3333FF" />
                                                </td>
        </tr>
        <%--<td colspan="15">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
        </td>--%>
        <tr>
             <td colspan="15">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDatefrom" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                                        NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDatefrom_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="15">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDateto" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="ImgPollution" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                                        NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDateto_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
        </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
    
          <table>
          <asp:GridView ID="GridView1" runat="server"></asp:GridView>                           
                                               
      <%-- <ComponentArt:Grid ID="DataGrid1" runat="server" Width="100%" 
                       BorderColor="#99FF99" SearchOnKeyPress="true" ShowSearchBox="true"
                                            SearchBoxPosition="TopLeft">
                                            <Levels>
                                                <ComponentArt:GridLevel  DataKeyField="MONTHNAME" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading" 
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                          
                                             <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="YEAR" HeadingText="YEAR"
                                                SortedDataCellCssClass="SortedDataCell" IsSearchable="True" />
                                             <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="MONTHNAME" HeadingText="MONTH"
                                                SortedDataCellCssClass="SortedDataCell" IsSearchable="True" />
                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="2W Single Plate" HeadingText="2W Single Plate"
                                                SortedDataCellCssClass="SortedDataCell" IsSearchable="True" />
                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="3W Single Plate" HeadingText="3W Single Plate"
                                                SortedDataCellCssClass="SortedDataCell" IsSearchable="True" />
                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="4W Single Plate" HeadingText="4W Single Plate"
                                                SortedDataCellCssClass="SortedDataCell" IsSearchable="True" />
                                            <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="2W" HeadingText="2W"
                                                SortedDataCellCssClass="SortedDataCell" IsSearchable="True" />
                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="3W" HeadingText=" 3W"
                                                SortedDataCellCssClass="SortedDataCell"  Align="Right" />
                                                 <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="4W" HeadingText="4W"
                                                SortedDataCellCssClass="SortedDataCell"  Align="Right" />
                                                 <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="MCV/HCV/TRAILERS" HeadingText="MCV/HCV/TRAILERS"
                                                SortedDataCellCssClass="SortedDataCell"  Align="Right" />
                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="TRACTOR" HeadingText="TRACTOR"
                                                SortedDataCellCssClass="SortedDataCell"   Align="Right"/>
                                                  
                                            
                                        </Columns>
                                                </ComponentArt:GridLevel>

                                            </Levels>
                                           
                                                                                     
  
                    </ComponentArt:Grid>--%>
                    </table>
                   
                                  
</asp:Content>

