<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ZonewiseReport.aspx.cs" Inherits="HSRP.Report.ZonewiseReport" %>
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

      function Datefrom_OnDateChange(sender, eventArgs) {
          var fromDate = Datefrom.getSelectedDate();
          CalendarDatefrom.setSelectedDate(fromDate);

      }

      function Datefrom_OnChange(sender, eventArgs) {
          var fromDate = CalendarDatefrom.getSelectedDate();
          Datefrom.setSelectedDate(fromDate);

      }

      function Datefrom_OnClick() {
          if (CalendarDatefrom.get_popUpShowing()) {
              CalendarDatefrom.hide();
          }
          else {
              CalendarDatefrom.setSelectedDate(Datefrom.getSelectedDate());
              CalendarDatefrom.show();
          }
      }

      function Datefrom_OnMouseUp() {
          if (CalendarDatefrom.get_popUpShowing()) {
              event.cancelBubble = true;
              event.returnValue = false;
              return false;
          }
          else {
              return true;
          }
      }

      ////>>>>>> Pollution Due Date

      function Dateto_OnDateChange(sender, eventArgs) {
          var toDate = Dateto.getSelectedDate();
          CalendarDateto.setSelectedDate(toDate);
      }

      function Dateto_OnChange(sender, eventArgs) {
          var toDate = CalendarDateto.getSelectedDate();
          Dateto.setSelectedDate(toDate);
      }

      function Dateto_OnClick() {
          if (CalendarDateto.get_popUpShowing()) {
              CalendarDateto.hide();
          }
          else {
              CalendarDateto.setSelectedDate(Dateto.getSelectedDate());
              CalendarDateto.show();
          }
      }

      function Dateto_OnMouseUp() {
          if (CalendarDateto.get_popUpShowing()) {
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
                                        <span class="headingmain">PIP VS Received</span>
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

   
    <table style="height: 51px;" width="80%">
        <tr>
            <td style="height: 40px;" width="20">
                </td>
            <td style="height: 40px;" valign="middle" width="80">
                <asp:Label Text="Zone:" runat="server" ID="labelOrganization1" 
                                            ForeColor="Black" Font-Bold="True" Width="100px" />
            </td>
            <td style="width: 150px; height: 40px;" valign="middle">              
                        <asp:DropDownList ID="DropDownListzone" runat="server" AutoPostBack="True"                                                                               
                        CausesValidation="false" DataTextField="zone"  DataValueField="zone"  Height="22px" Width="120px" OnSelectedIndexChanged="DropDownListzone_SelectedIndexChanged">
                        </asp:DropDownList>                  
                
                     
                
            </td>
              <td style="height: 40px;" valign="middle" width="80">
                <asp:Label Text="Rtolocation :" runat="server" ID="label3" ForeColor="Black" Font-Bold="True" Width="100px" />
            </td>
            <td style="width: 150px; height: 40px;" valign="middle">              
                        <asp:DropDownList ID="DropDownListRtolocation" runat="server" AutoPostBack="True"                                                                               
                        CausesValidation="false" DataTextField="RTOLocationName"  DataValueField="RTOLocationID"  Height="22px" Width="120px" OnSelectedIndexChanged="DropDownListRtolocation_SelectedIndexChanged">
                        </asp:DropDownList>                  
                
                   
                
            </td>


             <td style="height: 40px;" valign="middle" width="80">
                <asp:Label Text="Dealer Name :" runat="server" ID="label4" ForeColor="Black" Font-Bold="True" Width="100px" />
            </td>
            <td style="width: 150px; height: 40px;" valign="middle">              
                        <asp:DropDownList ID="DropDownListDealer" runat="server" AutoPostBack="True"                                                                               
                        CausesValidation="false" DataTextField="DealerName"  DataValueField="Dealerid"  Height="22px" Width="120px" OnSelectedIndexChanged="DropDownListDealer_SelectedIndexChanged">
                        </asp:DropDownList>                  
                
                     ,
                
            </td>
          
            <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label ID="label5" runat="server" Font-Bold="True" ForeColor="Black"  Text="Report Type" Width="100px" />
                                        </td>
                                         <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">                                                                                         
                                            <asp:DropDownList ID="DdlReportType" runat="server">
                                            </asp:DropDownList>                                                  
                                        </td>  
                                                <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label Text="From:" runat="server" ID="labelDate" Font-Bold="True" ForeColor="Black" Width="48px" />
                                        </td>
                                         <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Datefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Datefrom_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_from_button" alt="" onclick="Datefrom_OnClick()"
                                                        onmouseup="Datefrom_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:Label Text="To:" runat="server" ID="label1" Font-Bold="True"  ForeColor="Black" />
                                        </td>
                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Dateto" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Dateto_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_to_button" alt="" onclick="Dateto_OnClick()"
                                                        onmouseup="Dateto_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>  
                                                <td style="height: 40px" width="30">
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export" 
                                                        Font-Bold="True" ForeColor="#3333FF" Height="21px" Width="224px"/>
                                                </td>
                                               
                                             
                                                

            
                                                
        </tr>
      
        
        <tr  align="center">
                                            
            <td colspan="6">
            <asp:Label ID="Lblerror" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
        </td>
        </tr>

        <%-- <tr>
                <td colspan="8" align="center" id="gridTD" runat="server">
                    <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="457px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
                        <asp:GridView ID="grd" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                            ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>         
                                             
                            </Columns>

                            <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>--%>
        

        <tr>
            <td colspan="3" align="left">
                                <ComponentArt:Calendar runat="server" ID="CalendarDatefrom" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                    PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif"
                                    NextImageUrl="cal_nextMonth.gif" Height="172px" Width="200px">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="Datefrom_OnChange" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </td>
            <td colspan="3" align="left">
                                <ComponentArt:Calendar runat="server" ID="CalendarDateto" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                    PopUp="Custom" PopUpExpandControlId="calendar_to_button" CalendarTitleCssClass="title"
                                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif"
                                    NextImageUrl="cal_nextMonth.gif" Height="172px" Width="200px">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="Dateto_OnChange" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </td>
         
                                            </tr>
                                            
                                                                                           
                                            
     </table>
   
                                            
</asp:Content>


