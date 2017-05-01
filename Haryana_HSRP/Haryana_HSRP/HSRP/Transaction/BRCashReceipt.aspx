<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BRCashReceipt.aspx.cs" Inherits="HSRP.Transaction.BRCashReceipt" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 
     <%-- <link href="../css/table.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="../css/legend.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" /> --%>
               
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <style type="text/css">
        .style4 {
            width: 428px;
        }

        .style5 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 103px;
            padding-left: 20px;
        }

        .style6 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 184px;
            padding-left: 20px;
        }

        .style7 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
        }
    </style>





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

        //function Dateto_OnDateChange(sender, eventArgs) {
        //    var toDate = Dateto.getSelectedDate();
        //    CalendarDateto.setSelectedDate(toDate);
        //}

        //function Dateto_OnChange(sender, eventArgs) {
        //    var toDate = CalendarDateto.getSelectedDate();
        //    Dateto.setSelectedDate(toDate);
        //}

        //function Dateto_OnClick() {
        //    if (CalendarDateto.get_popUpShowing()) {
        //        CalendarDateto.hide();
        //    }
        //    else {
        //        CalendarDateto.setSelectedDate(Dateto.getSelectedDate());
        //        CalendarDateto.show();
        //    }
        //}

        //function Dateto_OnMouseUp() {
        //    if (CalendarDateto.get_popUpShowing()) {
        //        event.cancelBubble = true;
        //        event.returnValue = false;
        //        return false;
        //    }
        //    else {
        //        return true;
        //    }
        //}

    </script>

    
  
    
<%--<marquee class="mar1" direction="left">Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</marquee>--%>
    
      <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                  Download AC Cash Collection Receipt</div>
            </legend>
          <table>
              <tr>
                   <td style="padding-left: 314px;"><asp:Label ID="lblState" runat="server" Text="Select State:">
                      </asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td valign="middle" style="padding-left: 10px;">
                         <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">                        
                         </asp:DropDownList>                      
                  </td>
                  <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlState" InitialValue="0" ErrorMessage="Please Select State"></asp:RequiredFieldValidator></td>
                
              </tr>


              <tr>
                  <td style="padding-left: 314px;">
                      <asp:Label runat="server" ID="lblUserName" Text="Select Dealer Name">
                      </asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td style="padding-left: 10px;"> <asp:DropDownList runat="server" AutoPostBack="true" Visible="true" CausesValidation="false" ID="ddlUserName" >  </asp:DropDownList>

                  </td>

                  <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlUserName" InitialValue="0" ErrorMessage="Please Select Dealer Name"></asp:RequiredFieldValidator></td>

              </tr>

               <tr>
                   <td style="padding-left: 314px;"><asp:Label ID="Label1" runat="server" Text="Select Date:">
                      </asp:Label>
                      
                  </td>

                 <td nowrap="nowrap" class="style7" style="padding-bottom: 10px; padding-left: 10px;">
                
                    <ComponentArt:Calendar ID="Datefrom" runat="server" TabIndex="13" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                        ControlType="Picker" PickerCssClass="picker">
                        <ClientEvents>
                            <SelectionChanged EventHandler="Datefrom_OnDateChange" />
                        </ClientEvents>
                    </ComponentArt:Calendar>
                    <img id="calendar_from_button" alt="" onclick="Datefrom_OnClick()"
                        onmouseup="Datefrom_OnMouseUp()" class="calendar_button"
                        src="../images/btn_calendar.gif" />                 
                                   
                
                               
                </td>
                        
              </tr>





              <tr><td></td><td style="padding-left: 10px;"><asp:Button ID="Button3" runat="server" CssClass="button" Height="23px" onclick="Button3_Click" TabIndex="13" Text="Epson Print"/></td><td></td></tr>
              <tr><td></td><td></td><td>
                  <asp:Label ID="lblErrMess" runat="server" Text=""   ForeColor="Red"  Visible="false"></asp:Label>
                                    </td></tr>


              <tr>
                  <td>
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
                  </td>
                    <td></td>
                    <td></td>

              </tr>

          </table> 
    </fieldset>

</asp:Content>
