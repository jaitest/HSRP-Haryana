<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerInformationDetails.aspx.cs" Inherits="HSRP.Dealer.Transaction.DealerInformationDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/User.js" type="text/javascript"></script>
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <script src="../../javascript/AutoNumeric.js" type="text/javascript"></script>
    <script type="text/javascript">

        // example uses the selector "input" with the class "auto" & no options passed
        jQuery(function ($) {
            $('#txtAmount').autoNumeric();
        });

        // example uses the selector "input" with the class "auto" & with options passed
        // See details below on allowed options
        jQuery(function ($) {
            $('#txtAmount').autoNumeric({ aSep: '.', aDec: '' });
        });

        function validate() {

            if (document.getElementById("dropDownListOrg").value == "--Select State--") {
                alert("Select State");
                document.getElementById("dropDownListOrg").focus();
                return false;
            }
            if (document.getElementById("dropDownListClient").value == "--Select RTO--") {
                alert("Select RTO");
                document.getElementById("dropDownListClient").focus();
                return false;
            }
            if (document.getElementById("dropDownListUser").value == "--Select User--") {
                alert("Select User");
                document.getElementById("dropDownListUser").focus();
                return false;
            }

            
            if (document.getElementById("txtAmount").value == "") {
                alert("Fill Basic Amount.");
                document.getElementById("txtAmount").focus();
                return false;
            }
            if ((parseFloat(document.getElementById("txtAmount").value) <= 0.0) || (parseFloat(document.getElementById("txtAmount").value) <= 0)) {
                alert("Imperest Amount Should be greater than zero.");
                document.getElementById("txtAmount").focus();
                return false;
            }


            else {
                return true;
            }
        } 
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

         

         

         

         
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
      <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="../../images/ajax-loader.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
     <center>
        <div style="margin: 20px; background-color: #FFFFFF; width: 700px" align="left">
            <fieldset>
                <legend>
                    <div style="margin-left: 2px; width: 250px; font-size: 15px; color: Black">
                        <asp:Label ID="LabelFormName" runat="server" Text="Dealer Information Entry"></asp:Label>
                    </div>
                </legend>
                <div style="margin: 20px;" align="left">
                    <div>
                        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                            <%--<tr>
                                <td class="form_text">
                                    Cheque Date :<span class="alert">* </span>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left" style="padding: 5px">
                                                <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                    ControlType="Picker" PickerCssClass="picker">
                                                    <ClientEvents>
                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                    </ClientEvents>
                                                </ComponentArt:Calendar>
                                            </td>
                                            <td valign="top" align="left">
                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../../images/btn_calendar.gif" />
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
                                    <asp:DropDownList ID="ddlDealerName" runat="server" Width="180">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Dealer Name :<span class="alert">* </span>
                                </td>
                                <td style="padding-left: 10px">
                                    <asp:DropDownList ID="DropDownList1" runat="server" Width="180">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Employee ID : <span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeAmount" runat="server" class="form_textbox" MaxLength="6"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Sales Person's Name :<span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeNo" runat="server" class="form_textbox" MaxLength="6"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Phone :<span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDrawnBankName" runat="server" class="form_textbox p" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Email ID : <span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDeliveredBy" runat="server" class="form_textbox" ></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td class="form_text">
                                    Address :
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" class="form_textbox" ></asp:TextBox>
                                </td>
                            </tr> <tr>
                                <td class="form_text">
                                    Company Name : <span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server" class="form_textbox" ></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td class="form_text">
                                    Remarks :
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox3"  runat="server" Height="120px" class="form_textbox" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td class="form_text">
                                    Active Status :
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkAcitive" runat="server" Checked="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <%-- <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8" OnClientClick=" javascript:return vali();"
                                        class="button" OnClick="buttonUpdate_Click" />&nbsp;&nbsp;--%>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" class="button" OnClientClick=" javascript:return vali();"
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
        <center>
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
                            ImagesBaseUrl="../../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
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
                            ImagesBaseUrl="../../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                            <ClientEvents>
                                <SelectionChanged EventHandler="CalendarHSRPAuthDate_OnChange" />
                            </ClientEvents>
                        </ComponentArt:Calendar>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <ComponentArt:Calendar runat="server" ID="CalendarCheckDeliveredDate" AllowMultipleSelection="false"
                            AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                            PopUp="Custom" PopUpExpandControlId="Img1" CalendarTitleCssClass="title" DayHoverCssClass="dayhover"
                            DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday" OtherMonthDayCssClass="othermonthday"
                            DayHeaderCssClass="dayheader" DayCssClass="day" SelectedDayCssClass="selectedday"
                            CalendarCssClass="calendar" NextPrevCssClass="nextprev" MonthCssClass="month"
                            SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters" ImagesBaseUrl="../../images"
                            PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                            <ClientEvents>
                                <SelectionChanged EventHandler="CalendarHSRPAuthDate1_OnChange" />
                            </ClientEvents>
                        </ComponentArt:Calendar>
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
