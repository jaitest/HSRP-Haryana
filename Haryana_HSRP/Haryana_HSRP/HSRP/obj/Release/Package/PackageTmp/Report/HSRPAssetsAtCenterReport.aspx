<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HSRPAssetsAtCenterReport.aspx.cs" Inherits="HSRP.Report.HSRPAssetsAtCenterReport" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    
    
    <script type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select Location--") {
                alert("Select Location");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }


        }
    </script>
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
    <script type="text/javascript">


        function edit(i) { // Define This function of Send Assign Laser ID 
            //alert("AssignLaser" + i);
            //            var usertype = document.getElementById('username').value;
            //            alert(usertype);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransaction.aspx?Mode=Edit&TransactionID=" + i, "Update BankTransaction.aspx", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewBankTransaction.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransaction.aspx?Mode=New", "Add New Bank Transaction", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'ViewBankTransaction.aspx';
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
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td colspan="15">
                                        <span class="headingmain">Hsrp Assets At Center Report</span>
                                    </td>
                                </tr>
                                        <tr>
            <td class="style3" style="width: 93px; height: 59px;">
                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" 
                                            ForeColor="Black" Font-Bold="True" />
            </td>
            <td style="height: 59px; ">
                &nbsp;</td>
            <td style="width: 225px; height: 59px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="true" 
                                                                              
                        CausesValidation="false" DataTextField="HSRPStateName" 
                                                                              DataValueField="HSRP_StateID" 
                                                                              
                                    onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" 
                                    Height="16px" Width="148px">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="height: 59px">
                </td>
            <td style="width: 124px; height: 59px;">
                <asp:Label Text="RTO Location:" 
                                runat="server" ID="labelClient" ForeColor="Black" 
                    Font-Bold="True" />
            </td>
            <td style="height: 59px">
                </td>
            <td style="height: 59px">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="dropDownListClient" runat="server" AutoPostBack="True" DataTextField="RTOLocationName" 
                            DataValueField="RTOLocationID" Height="21px" 
                            onselectedindexchanged="dropDownListClient_SelectedIndexChanged" 
                            Width="151px">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td valign="middle" class="Label_user_batch" nowrap="nowrap" 
                            style="width: 7px; height: 59px;">
                                                    &nbsp;&nbsp;
                                                    &nbsp;&nbsp;
                                                </td>
         
                                                <td> <asp:Label Text="From:" runat="server" ID="labelDate" Font-Bold="True" 
                                                        ForeColor="Black" /> </td>
                                                <td>
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
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDatefrom_OnClick()"
                                                                    onmouseup="OrderDatefrom_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" /></td>
                                                <td><asp:Label Text="To:" runat="server" ID="labelTO" Font-Bold="True" 
                                                        ForeColor="Black" /> </td>
                                                <td>
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
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export To Excel" 
                                                        Font-Bold="True" ForeColor="#3333FF" onclientclick="validate()" />
                                                </td>
                                               
                                               <tr>
                                                <td colspan="15">
                                                    <asp:Label ID="lblmsg" runat="server" ForeColor="#CC3300"></asp:Label>
                                                </td>
                                                </tr>
                                              
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
                            </table>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
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
