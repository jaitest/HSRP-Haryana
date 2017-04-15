<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BankTransactionVoid.aspx.cs"
    Inherits="HSRP.Master.BankTransactionVoid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../javascript/common.js"></script>
    <script type="text/javascript">
        function DepositDate_OnDateChange(sender, eventArgs) {
            var fromDate = DepositDate.getSelectedDate();
            CalendarDepositDate.setSelectedDate(fromDate);

        }

        function DepositDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarDepositDate.getSelectedDate();
            DepositDate.setSelectedDate(fromDate);

        }

        function DepositDate_OnClick() {
            if (CalendarDepositDate.get_popUpShowing()) {
                CalendarDepositDate.hide();
            }
            else {
                CalendarDepositDate.setSelectedDate(DepositDate.getSelectedDate());
                CalendarDepositDate.show();
            }
        }

        function DepositDate_OnMouseUp() {
            if (CalendarDepositDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function boxbatchRelesedDate_OnDateChange(sender, eventArgs) {
            var fromDate = boxbatchRelesedDate.getSelectedDate();
            CalendarboxbatchRelesedDate.setSelectedDate(fromDate);

        }

        function CalendarboxbatchRelesedDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarboxbatchRelesedDate.getSelectedDate();
            boxbatchRelesedDate.setSelectedDate(fromDate);

        }

        function boxbatchRelesedDate_OnClick() {
            if (CalendarboxbatchRelesedDate.get_popUpShowing()) {
                CalendarboxbatchRelesedDate.hide();
            }
            else {
                CalendarboxbatchRelesedDate.setSelectedDate(boxbatchRelesedDate.getSelectedDate());
                CalendarboxbatchRelesedDate.show();
            }
        }

        function boxbatchRelesedDate_OnMouseUp() {
            if (CalendarboxbatchRelesedDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }
    </script>
  
    <style type="text/css">
        .style4
        {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 210px;
            padding-left: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" width:100%;">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Update Status</div>
            </legend>
            <br />
            <br />
              <div style="width: 100%; margin: 0px auto 0px auto">
              
          <%--  <table  border="0" align="center"  style="height: 200px; width:85%; border:solid 1px black;">
                    <h2>
                        Please Check UPdate Status
                    </h2>
            </table>--%>
            <div style=" clear:both"></div>
            <table border="0"  cellpadding="0" cellspacing="3" width="490px" align="center" >
                <tr>
                    <td>
                           <h2>
                               <span style="color:black; font-family:Arial; font-size:16px; vertical-align:central;">
                                   Please Check Update Status
                               </span>
                        
                    </h2>

                    </td>

                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
             <tr>
                  <td nowrap="nowrap" align="center"  >
                                                            <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                                            <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                                                            <asp:Button ID="buttonUpdate" runat="server" TabIndex="10" class="button" Visible="false"
                                                                 Text="Update" onclick="buttonUpdate_Click"  OnClientClick="return validate()" />    
                                                            &nbsp;&nbsp;
                                                         <%--  <asp:Button ID="buttonSave" runat="server" TabIndex="9" class="button" Text="Save"  OnClientClick=" return validate()" Visible="false" 
                                                                OnClick="ButtonSubmit_Click" />--%>&nbsp;&nbsp;
                                                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                                id="buttonClose" value="Close" class="button"/>
                                                            &nbsp;&nbsp;
                                                      
                                                         <%--   <asp:Button ID="btnReset" runat="server"  class="button" Text="Cancel" 
                                                                onclick="btnReset_Click" TabIndex="12" />--%>
                                                        </td>
                    
                
                    
                </tr>
              
           </table>
            </div>
           
          <%-- <table>
             <tr>
                  <td>  <ComponentArt:Calendar runat="server" ID="CalendarDepositDate" AllowMultipleSelection="false"
                            AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                            PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                            DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                            OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                            SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                            MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                            ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                            <ClientEvents>
                                <SelectionChanged EventHandler="DepositDate_OnChange" />
                            </ClientEvents>
                        </ComponentArt:Calendar></td>
                        <td>
                        <ComponentArt:Calendar runat="server" ID="Calendar1" AllowMultipleSelection="false"
                            AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                            PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                            DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                            OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                            SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                            MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                            ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                            <ClientEvents>
                                <SelectionChanged EventHandler="DepositDate_OnChange" />
                            </ClientEvents>
                        </ComponentArt:Calendar>
                        </td>
                </tr>
           </table>--%>
        </fieldset>
    </div>
    </form>
</body>
</html>
