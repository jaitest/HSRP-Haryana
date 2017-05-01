<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DLViewDealerOrder.aspx.cs" Inherits="HSRP.Transaction.DLViewDealerOrder" %>

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

 <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain"> View  Orders</span>
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
                                    <td valign="middle" visible="true" class="form_text" nowrap="nowrap">
                                        &nbsp;&nbsp;
                                        <asp:Label Text="Date:" runat="server" ID="labelDate" Visible="true" />
                                    </td>
                                    <td valign="middle" onmouseup="OrderDate_OnMouseUp()" align="left">
                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Visible="true">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                            onmouseup="OrderDate_OnMouseUp()" class="calendar_button"  src="../images/btn_calendar.gif" />

                                        <asp:Button ID="btnGO" Width="58px" runat="server" Visible="true" Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClick="btnGO_Click" /> <%-- OnClientClick=" return validate()"--%>
                                        &nbsp;&nbsp;&nbsp;<asp:Button ID="btnExportExcel" class="button" Visible="false" runat="server" ToolTip="Download for Grid Report" Text="Download" OnClick="btnExportExcel_Click" />
                                        
                                    </td>
                                    
                                    <td valign="middle" align="left">
                                        &nbsp;</td>
                                     
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;</td>
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
                                        <asp:Label ID="lblSucMess" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left" style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="llbMSGError0" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="lblVehicleRegNo" runat="server" Font-Bold="True"
                        ForeColor="Blue" Text="VehicleRegNo=" Visible="False"></asp:Label>
                    <br />
                </td>
            </tr>
                                <tr>
                                    <td>
                                         <asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="true">
                                         <AlternatingRowStyle BackColor="White" />
                                         <Columns>                                         
                      
                                         </Columns>
                                         <EditRowStyle BackColor="#2461BF" />
                                         <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                         <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                         <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                         <RowStyle BackColor="#EFF3FB" />
                                         <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                         <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                         <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                         <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                         <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                         </asp:GridView>
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
                            </table>
                        </td>
                    </tr> 
                </table>
            </td>
            
        </tr>
    </table>


</asp:Content>
