<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="WebForm1.aspx.cs" Inherits="HSRP.Transaction.WebForm1" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
   
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
    <script type="text/javascript">

        function removeSpaces(string) {
            return string.split(' ').join('');
        }

        function remove(string) {
            return trims(string);
        }

        function trims(s) {
            return rtrim(ltrim(s));
        }

        function ltrim(s) {
            var l = 0;
            while (l < s.length && s[l] == ' ')
            { l++; }
            return s.substring(l, s.length);
        }

        function rtrim(s) {
            var r = s.length - 1;
            while (r > 0 && s[r] == ' ')
            { r -= 1; }
            return s.substring(0, r + 1);
        } 
         
    </script>
    <script language="javascript" type="text/javascript">


        function validateVehicleRegNo() {


            if (document.getElementById("<%=txtVehicleRegNo.ClientID%>").value == "") {
                alert("Provide Vehicle Registration No.");
                document.getElementById("<%=txtVehicleRegNo.ClientID%>").focus();
                return false;
            }
        }
        function validate() {
            if (document.getElementById("<%=txtVehicleRegNo.ClientID%>").value == "") {
                alert("Provide Vehicle Registration No.");
                document.getElementById("<%=txtVehicleRegNo.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=txtVehicleRegNo.ClientID%>"))) {
                //        
                alert("Special Character Not Allowed!!");
                document.getElementById("<%=txtVehicleRegNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtAuthorizationNo.ClientID%>").value == "") {
                alert("Provide Authorization No.");
                document.getElementById("<%=txtAuthorizationNo.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=txtAuthorizationNo.ClientID%>"))) {
                alert("Special Character Not Allowed!!");
                document.getElementById("<%=txtAuthorizationNo.ClientID%>").focus();
                return false;
            }

            var fromDate = DepositDate.getSelectedDate();
            if (fromDate == null) {
                alert("Provide Authorization Date");
                document.getElementById("<%=DepositDate.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtOwnerName.ClientID%>").value == "") {
                alert("Provide Owner Name");
                document.getElementById("<%=txtOwnerName.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=txtOwnerName.ClientID%>"))) {
                alert("Special Character Not Allowed!!");
                document.getElementById("<%=txtOwnerName.ClientID%>").focus();
                return false;
            }



            if (document.getElementById("<%=txtMobileNo.ClientID%>").value == "0") {
                alert("Provide Mobile No.");
                document.getElementById("<%=txtMobileNo.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=txtMobileNo.ClientID%>"))) {
                alert("Special Character Not Allowed!!");
                document.getElementById("<%=txtMobileNo.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "0") {
                alert("Please Select Vehicle Class");
                document.getElementById("<%=DropDownListVehicleClass.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListVehicleModel.ClientID%>").value == "0") {
                alert("Please Select Vehicle Type");
                document.getElementById("<%=DropDownListVehicleModel.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=DropDownListOrderType.ClientID%>").value == "--Select Order Type--") {
                alert("Please Select Order Type");
                document.getElementById("<%=DropDownListOrderType.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtAddress.ClientID%>").value == "") {
                alert("Provide Address");
                document.getElementById("<%=txtAddress.ClientID%>").focus();
                return false;
            }
        }
          
    </script>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
            width: 140px !important;
        }
        #divwidth div
        {
            width: 140px !important;
        }
    </style>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth2
        {
            width: 120px !important;
        }
        #divwidth2 div
        {
            width: 120px !important;
        }
    </style>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth3
        {
            width: 150px !important;
        }
        #divwidth3 div
        {
            width: 150px !important;
        }
    </style>
    <style type="text/css">
        .modalPopup
        {
            background-color: #FFFFFF;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }
    </style>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);

        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="../images/ajax-loader.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    <asp:Panel ID="Panel2" runat="server">
        <div style="width: 100%;">
            <fieldset>
                <legend>
                    <div style="margin-left: 10px; font-size: medium; color: Black">
                        Cash Receipt Data Entry
                    </div>
                </legend>
                <br />
                <br />
                <div style="width: 100%;" id="Popupdiv" runat="server">
                </div>
                <%-- <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />--%>
                <div style="width: 100%; height: 383px; margin: 0px auto 0px auto">
                    <table border="0" align="right" style="height: 348px; width: 85%;">
                        <tr>
                            <td style="width: 128px">
                                <asp:Label ID="lblMessageCashReceipt" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td style="width: 347px">
                                <asp:Label ID="lblCashReceiptNo" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="width: 128px">
                                Vehicle Reg No : <span class="alert">* </span>
                            </td>
                            <td class="form_text" style="width: 128px">
                                <%--<asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />--%>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="UpdatePanelAuthNo" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox Width="160" class="form_textbox11" onblur="this.value=removeSpaces(this.value);"
                                                        Style="text-transform: uppercase" TabIndex="1" ID="txtVehicleRegNo" MaxLength="10"
                                                        runat="server"></asp:TextBox>
                                                    <div id="divwidth">
                                                    </div>
                                                    <asp:AutoCompleteExtender UseContextKey="true" ServiceMethod="GetVehicleCashinHandRegsNo"
                                                        MinimumPrefixLength="1" ServicePath="~/WCFService/ServiceForSuggestion.svc" CompletionInterval="10"
                                                        EnableCaching="false" TargetControlID="txtVehicleRegNo" ID="AutoCompleteExtender1"
                                                        runat="server" FirstRowSelected="false" CompletionSetCount="12" CompletionListCssClass="AutoExtender"
                                                        CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                        CompletionListElementID="divwidth">
                                                    </asp:AutoCompleteExtender>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <%-- <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />--%>
                                                    <%--<asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="buttonGo" runat="server" CausesValidation="false" OnClientClick="javascript:return validateVehicleRegNo();"
                                                Text="Go" class="button" TabIndex="2" OnClick="buttonGo_Click"></asp:LinkButton>
                                            &nbsp; &nbsp;
                                            <%-- <asp:LinkButton ID="buttonGo" runat="server" CausesValidation="false" OnClientClick="javascript:return CheckAuthNo();"
                                                                            Text="Go" class="button" TabIndex="2" OnClick="buttonGo_Click"></asp:LinkButton>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="form_text">
                                Authorization No : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAuthorizationNo" runat="server" Width="160" class="form_textbox11"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="lblRecordType" runat="server">
                            <td class="form_text" style="width: 138px">
                                Record Type :
                            </td>
                            <td class="form_text" style="width: 138px">
                                <asp:Label ID="lblOld" runat="server"></asp:Label>
                            </td>
                            <td class="form_text" style="width: 138px">
                                Registration Date :
                            </td>
                            <td class="form_text" style="width: 138px">
                                <asp:Label ID="lblRegDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="width: 138px">
                                Authorization Date <span class="alert">* </span>
                            </td>
                            <td style="width: 347px">
                                <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                                    cellpadding="0" border="0">
                                    <tr>
                                        <td valign="top" onmouseup="DepositDate_OnMouseUp()">
                                            <ComponentArt:Calendar ID="DepositDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                TabIndex="1" ControlType="Picker" PickerCssClass="picker">
                                                <ClientEvents>
                                                    <SelectionChanged EventHandler="DepositDate_OnDateChange" />
                                                </ClientEvents>
                                            </ComponentArt:Calendar>
                                        </td>
                                        <td style="font-size: 10px;">
                                            &nbsp;
                                        </td>
                                        <td valign="top">
                                            <img id="calendar_from_button" tabindex="2" alt="" onclick="DepositDate_OnClick()"
                                                onmouseup="DepositDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="form_text">
                                Owner Name : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOwnerName" runat="server" Width="160" class="form_textbox11"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="div1" runat="server">
                            <td class="form_text" style="width: 128px">
                                Father Name : <span class="alert">* </span>
                            </td>
                            <td style="width: 347px">
                                <asp:TextBox ID="txtFatherName" runat="server" Width="160" class="form_textbox11"
                                    MaxLength="10"></asp:TextBox>
                            </td>
                            <td class="form_text">
                                EmailID <span class="alert"></span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailID" runat="server" Width="160" class="form_textbox11" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="divAddress" runat="server">
                            <td class="form_text" style="width: 128px">
                                Old Address : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldAddress" runat="server" Width="320" Height="36px" class="form_textbox11"
                                    Enabled="false" MaxLength="10" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td class="form_text" style="width: 128px">
                                New Address : <span class="alert">* </span>
                            </td>
                            <td style="width: 347px">
                                <asp:TextBox ID="txtAddress" runat="server" Width="320" Height="36px" class="form_textbox11"
                                    MaxLength="10" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="div2" runat="server">
                            <td class="form_text" style="width: 128px">
                                Engine No : <span class="alert">* </span>
                            </td>
                            <td style="width: 347px">
                                <asp:TextBox ID="txtEngineNo" runat="server" Width="160" class="form_textbox11" MaxLength="10"></asp:TextBox>
                            </td>
                            <td class="form_text">
                                Chassies No. : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChassisNo" runat="server" Width="160" class="form_textbox11"
                                    MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="width: 128px">
                                Mobile No :
                            </td>
                            <td style="width: 347px">
                                <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event)" runat="server"
                                    Width="160" class="form_textbox11" MaxLength="10"></asp:TextBox>
                            </td>
                            <td class="form_text">
                                Vehicle Class : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" AutoPostBack="true"
                                    Width="170px" ID="DropDownListVehicleClass" runat="server" TabIndex="12" OnSelectedIndexChanged="DropDownListVehicleClass_SelectedIndexChanged">
                                    <%-- <asp:ListItem Value="0" Text="--Select Vehicle Class--"></asp:ListItem>
                                <asp:ListItem Value="Transport" Text="Transport"></asp:ListItem>
                                <asp:ListItem Value="Non-Transport" Text="Non-Transport"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="width: 128px">
                                Vehicle Type : <span class="alert">* </span>
                            </td>
                            <td style="width: 347px">
                                <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" Font-Size="Small"
                                    CausesValidation="false" ID="DropDownListVehicleModel" Width="170px" runat="server"
                                    TabIndex="13" OnSelectedIndexChanged="DropDownListVehicleModel_SelectedIndexChanged1">
                                    <%--<asp:ListItem Value="0" Text="--Select Vehicle Model--"></asp:ListItem>
                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                            <td class="form_text">
                                Order Type : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:DropDownList Style="margin-left: 3px; margin-bottom: 0px;" AutoPostBack="true"
                                    CausesValidation="false" ID="DropDownListOrderType" runat="server" TabIndex="20"
                                    Width="170px" OnSelectedIndexChanged="DropDownListOrderType_SelectedIndexChanged">
                                    <%--<asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="div3" runat="server">
                            <td class="form_text" style="width: 128px">
                                Front Plate : <span class="alert">* </span>
                            </td>
                            <td style="width: 347px">
                                <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" Font-Size="Small"
                                    DataTextField="ProductCode" DataValueField="ProductID" CausesValidation="false"
                                    ID="DropDownListFrontPlate" Width="170px" runat="server" TabIndex="13">
                                </asp:DropDownList>
                            </td>
                            <td class="form_text">
                                Rear Plate : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" Font-Size="Small"
                                    DataTextField="ProductCode" DataValueField="ProductID" CausesValidation="false"
                                    ID="DropDownListRearPlate" Width="170px" runat="server" TabIndex="13">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="width: 128px">
                                Third Sticker Mandatory :
                            </td>
                            <td style="width: 347px">
                                <asp:CheckBox ID="checkBoxThirdSticker" runat="server" />
                            </td>
                            <td class="form_text">
                                Vip :
                            </td>
                            <td>
                                <asp:CheckBox ID="chkVip" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text" style="width: 128px">
                                Reference: &nbsp;
                            </td>
                            <td style="width: 347px">
                                <asp:TextBox ID="txtReference" runat="server" Height="26px" Width="154px"></asp:TextBox>
                                <asp:TextBox ID="txtTax" runat="server" Visible="false" class="form_textbox11"></asp:TextBox>
                                <br />
                            </td>
                            <td class="form_text">
                                Amount : <span class="alert">* </span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAmount" runat="server" Enabled="false" Width="160" class="form_textbox11"></asp:TextBox>
                            </td>
                        </tr>
                     
                    </table>
                </div>
                <div style="border: 0px solid; width: 430px; height: 40px; float: right">
                    <div>
                        <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="buttonSave" runat="server" TabIndex="9" class="button" Text="Save"
                            OnClientClick=" return validate()" OnClick="ButtonSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="CashReceiptDataEntry.aspx" class="button">Reset</a>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnDownloadReceipt" runat="server" class="button" Text="Download Receipt"
                            TabIndex="12" OnClick="btnDownloadReceipt_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btnDuplicate" runat="server" TabIndex="9" class="button" Text="Duplicate"
                            Visible="false" OnClientClick=" return validate()" OnClick="btnDuplicate_Click" />&nbsp;&nbsp;
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </fieldset>
            <table>
                <tr>
                    <td>
                        <ComponentArt:Calendar runat="server" ID="CalendarDepositDate" AllowMultipleSelection="false"
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
                <asp:HiddenField ID="hiddenfieldReferenceValue" runat="server" />
                <asp:HiddenField ID="hiddenfieldhsrprecordStaggingID" runat="server" />
                <asp:HiddenField ID="hiddenfieldAuthorizationNo" runat="server" />
            </table>
            <%-- <asp:ListItem Value="0" Text="--Select Vehicle Class--"></asp:ListItem>
                                <asp:ListItem Value="Transport" Text="Transport"></asp:ListItem>
                                <asp:ListItem Value="Non-Transport" Text="Non-Transport"></asp:ListItem>--%>
        </div>
     <%--   <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" DropShadow="true"
            OkControlID="OkButton" PopupControlID="Panel1" TargetControlID="buttonSave">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopupSS" Style="display: none"
            BorderStyle="Solid" BorderWidth="3px">
        </asp:Panel>
        <asp:Panel ID="PanelInner" runat="server" ScrollBars="Vertical" Height="300px">
            <div id="vehshow" runat="server" style="background-color: White;">
            </div>
        </asp:Panel>--%>
    </asp:Panel>


    
  <%-- <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" />--%>

   <%--  <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CancelControlID="btnCancel"
        OkControlID="btnOkay" TargetControlID="Button2" PopupControlID="Panel1" PopupDragHandleControlID="PopupHeader"
        Drag="true" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel1" Style="display: none" runat="server">
        <div class="HellowWorldPopup">
            <div class="PopupHeader" id="PopupHeader">
                Header</div>
            <div class="PopupBody">
                <p>
                    This is a simple modal dialog</p>
            </div>
            <div class="Controls">
                <input id="btnOkay" type="button" value="Done" />
                <input id="Button1" type="button" value="Cancel" />
            </div>
        </div>
    </asp:Panel>--%>


   
  

     <%-- <asp:Panel ID="Panel3" runat="server">
                    <div align="center" style="width: 100%; padding-top: 30px">
                        <fieldset>
                            <legend>
                                <div style="margin-left: 10px; font-size: medium; color: Black">
                                    Cash Receipt Data Entry   
                                </div>
                            </legend>
                           
                            <br />
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" Width="100%" BorderColor="#3366CC"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4">
                                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                <RowStyle BackColor="White" ForeColor="#003399" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                <SortedDescendingHeaderStyle BackColor="#002876" />
                            </asp:GridView>
                          
                            <table style="padding-top: 30px">
                                <tr>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr style="padding-top: 20px">
                                    <td>
                                        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="button" OnClick="btnOk_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </asp:Panel>--%>


  <style type="text/css">
        .ModalPopupBG
        {
            background-color: #333333;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        
        .HellowWorldPopup
        {
            background-color: #ffffdd;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 5px;
            width: 800px;
            height: 300px;
        }
        .btnhide
        {
            display:none;
        }
    </style>
    <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" Enabled="false" CssClass="btnhide" />

    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CancelControlID="Button4"
        OkControlID="Button3" TargetControlID="Button2" PopupControlID="Panel1" PopupDragHandleControlID="PopupHeader"
        Drag="true" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel1" Style="display: none" runat="server">
        <div class="HellowWorldPopup">
            <div class="PopupHeader" id="PopupHeader">
                Header</div>
            <div class="PopupBody">
                <p>
                 
                  
                        <fieldset>
                            <legend>
                                <div style="margin-left: 10px; font-size: medium; color: Black">
                                    Cash Receipt Data Entry
                                </div>
                            </legend>
                            <br />
                            <br />
                           <%-- <asp:Label ID="Label1" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>--%>
                            <asp:Label ID="lblMesageSave" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                            <br />
                            <br />
                            <asp:GridView ID="GridView2" runat="server" BackColor="White" Width="100%" BorderColor="#3366CC"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4">
                                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                                <RowStyle BackColor="White" ForeColor="#003399" />
                                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                                <SortedDescendingHeaderStyle BackColor="#002876" />
                            </asp:GridView>
                            <table style="padding-top: 30px">
                                <tr>
                                    <td colspan="2">
                                    </td>
                                </tr>
                                <tr style="padding-top: 20px">
                                    <td>
                                        <asp:Button ID="Button3" runat="server" Text="Ok" CssClass="button" OnClick="btnOk_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button4" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                  
               
                </p>
            </div>
            <div class="Controls">
              <%--  <input id="btnOkay" type="button" value="Done" />
                <input id="btnCancel" type="button" value="Cancel" />--%>
            </div>
        </div>
    </asp:Panel>

    
</asp:Content>
