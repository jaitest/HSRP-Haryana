<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ViewDealerReceiptEntryA.aspx.cs" Inherits="HSRP.Dealer.Transaction.ViewDealerReceiptEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../..//legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <script src="../../javascript/RemoveSpace.js" type="text/javascript"></script>
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
    <script type="text/javascript">

        function vali() {
           
            //            if (document.getElementById("<%=ddlDealerName.ClientID%>").value == "--Select Dealer Name--") {
            //                alert("Select correct Dealer Name.");
            //                document.getElementById("<%=ddlDealerName.ClientID%>").focus();
            //                return false;
            //            }
            //            debugger;
            //            if (document.getElementById("<%=ddlDealerName.ClientID%>").options[0].selected = true) {
            //                alert("Select correct Dealer Name.");
            //                document.getElementById("<%=ddlDealerName.ClientID%>").focus();
            //                return false;
            //            }


           

            if (document.getElementById("<%=ddlDealerName.ClientID%>").value == "--Select Dealer Name--") {
                alert("Please select correct Dealer Name.");
                document.getElementById("<%=ddlDealerName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlPaymentMode.ClientID%>").value == "--Select Payment Mode--") {
                alert("Please select Payment Mode.");
                document.getElementById("<%=ddlPaymentMode.ClientID%>").focus();
                return false;
            }


          

            if (document.getElementById("<%=txtChequeAmount.ClientID%>").value == "") {

                
                    alert("Please Provide  Amount.");
                    document.getElementById("<%=txtChequeAmount.ClientID%>").focus();
                    return false;
                 
            }

                if (document.getElementById("<%=ddlPaymentMode.ClientID%>").value == "Cheque") 
            {

                if (document.getElementById("<%=txtChequeNo.ClientID%>").value == "") {

                    
                        alert("Please Provide Cheque No.");
                        document.getElementById("<%=txtChequeNo.ClientID%>").focus();
                        return false;
                    
                }

                if (document.getElementById("<%=txtDrawnBankName.ClientID%>").value == "") {

                    
                        alert("Please Provide Draw Bank Name.");
                        document.getElementById("<%=txtDrawnBankName.ClientID%>").focus();
                        return false;
                    
                }
                if (document.getElementById("<%=txtDeliveredBy.ClientID%>").value == "") {

                    
                        alert("Please Provide Deliverd by Whom ?");
                        document.getElementById("<%=txtDeliveredBy.ClientID%>").focus();
                        return false;
                    
                }
            }
        }

        function ischarKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode > 96 && charCode <= 122) || (charCode >= 65 && charCode <= 90) || (charCode == 32)) {
                return true;

            }
            else {
                return false;
            }
        }

    </script>
    <center>
        <div style="margin: 20px; background-color: #FFFFFF; width: 700px" align="left">
            <fieldset>
                <legend>
                    <div style="margin-left: 2px; width: 250px; font-size: 15px; color: Black">
                        <asp:Label ID="LabelFormName" runat="server" Text="Dealer Cheque Receipt Entry"></asp:Label>
                    </div>
                </legend>
                <div style="margin: 20px;" align="left">
                    <div>
                        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                            <tr>
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
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Dealer Name :<span class="alert">* </span>
                                </td>
                                <td style="padding-left: 10px">
                                    <asp:DropDownList ID="ddlDealerName" runat="server" Width="180">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                            <td class="form_text">Payment Mode:<span class="alert">* </span>
                            </td>
                            <td>
                            &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlPaymentMode" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlPaymentMode_SelectedIndexChanged">
                                    <asp:ListItem>--Select Payment Mode--</asp:ListItem>
                                    <asp:ListItem>Cheque</asp:ListItem>
                                    <asp:ListItem>Cash</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    <asp:Label ID="Label1" runat="server" Text="Amount"></asp:Label>
&nbsp;:<span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeAmount" runat="server" class="form_textbox" 
                                        MaxLength="6"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Cheque No :<span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChequeNo" runat="server" class="form_textbox" MaxLength="6" 
                                        Enabled="False"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Drawn On Bank Name :<span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDrawnBankName" runat="server" class="form_textbox p" 
                                        Enabled="False" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Cheque Delivered By :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDeliveredBy" runat="server" class="form_textbox" 
                                        Enabled="False" ></asp:TextBox>
                                </td>
                            </tr>
                           
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8" OnClientClick=" javascript:return vali();"
                                        class="button" OnClick="buttonUpdate_Click" />&nbsp;&nbsp;
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
            <asp:HiddenField ID="dealerID" runat="server" />
</asp:Content>
