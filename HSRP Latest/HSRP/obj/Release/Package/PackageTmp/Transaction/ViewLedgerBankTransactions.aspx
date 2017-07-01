<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewLedgerBankTransactions.aspx.cs" Inherits="HSRP.Transaction.ViewLedgerBankTransactions" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
 <script type="text/javascript">
     function StartDate_OnDateChange(sender, eventArgs) {
         var fromDate = StartDate.getSelectedDate();
         CalendarStartDate.setSelectedDate(fromDate);

     }

     function StartDate_OnChange(sender, eventArgs) {
         var fromDate = CalendarStartDate.getSelectedDate();
         StartDate.setSelectedDate(fromDate);

     }

     function StartDate_OnClick() {
         if (CalendarStartDate.get_popUpShowing()) {
             CalendarStartDate.hide();
         }
         else {
             CalendarStartDate.setSelectedDate(StartDate.getSelectedDate());
             CalendarStartDate.show();
         }
     }

     function StartDate_OnMouseUp() {
         if (CalendarStartDate.get_popUpShowing()) {
             event.cancelBubble = true;
             event.returnValue = false;
             return false;
         }
         else {
             return true;
         }
     }
         
     function EndDate_OnDateChange(sender, eventArgs) {
         var fromDate = EndDate.getSelectedDate();
         CalendarEndDate.setSelectedDate(fromDate);

     }

     function EndDate_OnChange(sender, eventArgs) {
         var fromDate = CalendarEndDate.getSelectedDate();
         EndDate.setSelectedDate(fromDate);

     }

     function EndDate_OnClick() {
         if (CalendarEndDate.get_popUpShowing()) {
             CalendarEndDate.hide();
         }
         else {
             CalendarEndDate.setSelectedDate(EndDate.getSelectedDate());
             CalendarEndDate.show();
         }
     }

     function EndDate_OnMouseUp() {
         if (CalendarEndDate.get_popUpShowing()) {
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
                                        <span class="headingmain">Ledger Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" visible="true" class="form_text" nowrap="nowrap"> &nbsp;&nbsp;
                                        <asp:Label Text="Location:" runat="server" ID="label2" Visible="true" /></td>
                                    <td valign="middle"  align="left">
                                        <asp:DropDownList Visible="true" ID="dropDownRtoLocation" CausesValidation="false"
                                                    Width="140px" runat="server" DataTextField="RTOLocationName" AutoPostBack="false"
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                    </td>
                                    <td valign="middle" visible="true" class="form_text" nowrap="nowrap">
                                        &nbsp;&nbsp;
                                        <asp:Label Text="Start Date:" runat="server" ID="labelDate" Visible="true" />
                                    </td>
                                    <td valign="middle" onmouseup="StartDate_OnMouseUp()" align="left">
                                        <ComponentArt:Calendar ID="StartDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Visible="true">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="StartDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="StartDate_OnClick()"
                                            onmouseup="StartDate_OnMouseUp()" class="calendar_button"  src="../images/btn_calendar.gif" />
                                     </td>
                                 
                                    <td valign="middle" visible="true" class="form_text" nowrap="nowrap">
                                        &nbsp;&nbsp;
                                        <asp:Label Text="End Date:" runat="server" ID="label1" Visible="true" />
                                    </td>
                                    <td valign="middle" onmouseup="EndDate_OnMouseUp()" align="left">
                                        <ComponentArt:Calendar ID="EndDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Visible="true">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="EndDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                        <img id="calendar_from_button1" tabindex="3" alt="" onclick="EndDate_OnClick()"
                                            onmouseup="EndDate_OnMouseUp()" class="calendar_button"  src="../images/btn_calendar.gif" />
                                        <asp:Button ID="btnGO" Width="58px" runat="server" Visible="true" Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClick="btnGO_Click" />  <%--  OnClientClick=" return validate()"--%>
                                        &nbsp;&nbsp;&nbsp;<asp:Button ID="btnExportExcel" class="button" Visible="false" runat="server" ToolTip="Download for Grid Report" Text="Download" OnClick="btnExportExcel_Click" />
                                        
                                    </td>
                                    
                                    <%--<td valign="middle" align="left">
                                        &nbsp;</td>
                                     
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;</td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                        <asp:Label ID="lblSucMess" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                <td align="left">
                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="llbMSGError0" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                  
                    <br />
                </td>
            </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" AutoGenerateColumns="false">
                                           <AlternatingRowStyle BackColor="White" />
                                         <Columns>
                                            
                                         <asp:TemplateField HeaderText="S No" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                         <%# Container.DataItemIndex + 1 %>
                                             </ItemTemplate>
                                         <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                                   <asp:BoundField HeaderText="Date" ItemStyle-HorizontalAlign="Center" DataField="Date" />
                                                   <asp:BoundField HeaderText="Name" ItemStyle-HorizontalAlign="Center" DataField="Name" />
                                                    <asp:BoundField HeaderText="Cr" ItemStyle-HorizontalAlign="Center" DataField="Cr" />
                                                    <asp:BoundField HeaderText="Dr" ItemStyle-HorizontalAlign="Center" DataField="Dr" />
                                                     <asp:BoundField HeaderText="Balance" ItemStyle-HorizontalAlign="Center" DataField="Running Balance" />
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
                                         <%--<asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" AutoGenerateColumns="false">
                                           <AlternatingRowStyle BackColor="White" />
                                         <Columns>
                                            
                                         <asp:TemplateField HeaderText="S No" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                         <%# Container.DataItemIndex + 1 %>
                                             </ItemTemplate>
                                         <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                                     
                                                   <asp:BoundField HeaderText="TransactionDate" ItemStyle-HorizontalAlign="Center" DataField="TransactionDate" />
                                                    <asp:BoundField HeaderText="Particular" ItemStyle-HorizontalAlign="Center" DataField="Particular" />
                                                    <asp:BoundField HeaderText="Cr" ItemStyle-HorizontalAlign="Center" DataField="Cr" />
                                                   <asp:BoundField HeaderText="Dr" ItemStyle-HorizontalAlign="Center" DataField="Dr" />
                                                     <asp:BoundField HeaderText="Balance" ItemStyle-HorizontalAlign="Center" DataField="Balance" />
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
                                                </asp:GridView>--%>
                                    </td>
                                </tr>
                                  <tr>
                                    <td>
                                        <ComponentArt:Calendar runat="server" ID="CalendarStartDate" AllowMultipleSelection="false"
                                            AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                            PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                            DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                            OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                            SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                            MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                            ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="StartDate_OnChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                       <td>
                                        <ComponentArt:Calendar runat="server" ID="CalendarEndDate" AllowMultipleSelection="false"
                                            AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                            PopUp="Custom" PopUpExpandControlId="calendar_from_button1" CalendarTitleCssClass="title"
                                            DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                            OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                            SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                            MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                            ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="EndDate_OnChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                </tr>
                                  <tr>
                                    <td>
                                        &nbsp;</td>
                                       <td>
                                           &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr> 
                </table>
            </td>
            
        </tr>
    </table>


</asp:Content>
