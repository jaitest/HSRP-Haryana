<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HSRPExcel.aspx.cs" Inherits="HSRP.Transaction.HSRPExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
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



        ////>>>>>> Pollution Due Date

        function HSRPAuthDate1_OnDateChange(sender, eventArgs) {
            var fromDate = CheckDeliveredDate.getSelectedDate();
            CalendarCheckDeliveredDate.setSelectedDate(fromDate);

        }

        function CalendarHSRPAuthDate1_OnChange(sender, eventArgs) {
            var fromDate = CalendarCheckDeliveredDate.getSelectedDate();
            CheckDeliveredDate.setSelectedDate(fromDate);

        }

        function HSRPAuthDate1_OnClick() {
            if (CalendarCheckDeliveredDate.get_popUpShowing()) {
                CalendarCheckDeliveredDate.hide();
            }
            else {
                CalendarCheckDeliveredDate.setSelectedDate(HSRPAuthDate.getSelectedDate());
                CalendarCheckDeliveredDate.show();
            }
        }

        function HSRPAuthDate1_OnMouseUp() {
            if (CalendarCheckDeliveredDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div style="margin: 20px; background-color: #FFFFFF;" align="left">
        <fieldset>
            <legend>
                    Upload Excel
            </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                       <%-- <tr>
                            <td class="form_text">
                                Date :<span class="alert">* </span>
                            </td>
                            <td>
                                <table>
                                    <tr>
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
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                          <tr>
                            <td class="form_text">
                                State Name :<span class="alert">* </span>
                            </td>
                            <td style="padding-left: 10px">
                                <asp:TextBox ID="txtStateName" BorderColor="Transparent" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                RTO Location Name :<span class="alert">* </span>
                            </td>
                            <td style="padding-left: 10px">
                                <asp:DropDownList ID="ddlRtoLocation" runat="server" Width="180">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="form_text">
                                Select File:<span class="alert">* </span>
                            </td>
                            <td colspan="3">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Remarks :
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" Height="68px" Width="291px"></asp:TextBox>
                            </td>
                        </tr>
                        <%--  <tr>
                            <td class="form_text">
                                ChequeDeliveredDate
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td valign="top" onmouseup="HSRPAuthDate1_OnMouseUp()" align="left">
                                            <ComponentArt:Calendar ID="CheckDeliveredDate" runat="server" PickerFormat="Custom"
                                                PickerCustomFormat="dd/MM/yyyy" ControlType="Picker" PickerCssClass="picker">
                                                <ClientEvents>
                                                    <SelectionChanged EventHandler="HSRPAuthDate1_OnDateChange" />
                                                </ClientEvents>
                                            </ComponentArt:Calendar>
                                        </td>
                                        <td valign="top" align="left">
                                            <img id="Img1" tabindex="4" alt="" onclick="HSRPAuthDate1_OnClick()" onmouseup="HSRPAuthDate1_OnMouseUp()"
                                                class="calendar_button" src="../images/btn_calendar.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <%--  <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8" OnClientClick=" javascript:return vali();"
                                    class="button" OnClick="buttonUpdate_Click" />--%>&nbsp;&nbsp;
                                    <%--OnClientClick=" javascript:return vali();"--%>
                                <asp:Button ID="btnSave" runat="server" Text="Upload Excel" TabIndex="4" class="button"
                                     OnClick="btnSave_Click" />&nbsp;&nbsp;
                                <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                    id="buttonClose" value="Close" class="button" />
                                &nbsp;&nbsp;
                                <%--<input type="reset" class="button" value="Reset" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="alert">
                                * Fields are mandatory.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </fieldset>
    </div>
    <table>
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
    </form>
</body>
</html>
