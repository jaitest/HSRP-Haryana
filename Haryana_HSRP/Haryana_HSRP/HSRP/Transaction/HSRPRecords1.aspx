<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="HSRPRecords1.aspx.cs" Inherits="HSRP.Transaction.HSRPRecords1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        1.
        function clearForm(form) {
            alert(form.toString());
            $(':input', form).each(function () {
                var type = this.type;
                var tag = this.tagName.toLowerCase(); // normalize case
                if (type == 'text' || type == 'password' || tag == 'textarea')
                    this.value = "";
                else if (type == 'checkbox' || type == 'radio')
                    this.checked = false;
                else if (tag == 'select')
                    this.selectedIndex = -1;
            });
        };


    </script>
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
    <script type="text/javascript">
        function CheckAuthNo() {
            var textBoxAuthrizationNo = document.getElementById("ctl00_ContentPlaceHolder1_textBoxAuthorizationNo").value;
            if (textBoxAuthrizationNo == "") {
                alert("Please Provide Authorization No.");
                document.getElementById("ctl00_ContentPlaceHolder1_textBoxAuthorizationNo").focus();
                return false;
            }
        } 
    </script>
    <script type="text/javascript">
        function ValidateForm() {

            var textBoxAuthrizationNo = document.getElementById("ctl00_ContentPlaceHolder1_textBoxAuthorizationNo").value;
            if (textBoxAuthrizationNo == "") {
                alert("Please Provide Authorization No.");
                document.getElementById("ctl00_ContentPlaceHolder1_textBoxAuthorizationNo").focus();
                return false;
            }
            var HSRPDate = OrderDate_picker.value;
            if (HSRPDate == "") {
                alert("Please Provide HSRP Authentication Date.");
                document.getElementById("OrderDate_picker").focus();
                return false;
            }

            var OrderDate = HSRPAuthDate_picker.value;
            if (OrderDate == "") {
                alert("Please Provide Order Date.");
                document.getElementById("OrderDate").focus();
                return false;
            }
            var CustomerName = document.getElementById("ctl00_ContentPlaceHolder1_textBoxCustomerName").value;

            if (CustomerName == "") {
                alert("Please Provide Customer Name.");
                document.getElementById("ctl00_ContentPlaceHolder1_textBoxCustomerName").focus();
                return false;
            }
            if (invalidChar(document.getElementById("ctl00_ContentPlaceHolder1_textBoxCustomerName"))) {
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("ctl00_ContentPlaceHolder1_textBoxCustomerName").focus();
                return false;
            }

            var Address1 = document.getElementById("ctl00_ContentPlaceHolder1_textBoxAddress1").value;

            if (Address1 == "") {
                alert("Please Provide address.");
                document.getElementById("ctl00_ContentPlaceHolder1_textBoxAddress1").focus();
                return false;
            }
            if (invalidChar(document.getElementById("ctl00_ContentPlaceHolder1_textBoxAddress1"))) {
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("ctl00_ContentPlaceHolder1_textBoxAddress1").focus();
                return false;
            }

            var MobileNo = document.getElementById("ctl00_ContentPlaceHolder1_textBoxMobileNo").value;

            if (MobileNo != "") {
                if (MobileNo.length != 10) {
                    alert("Please Provide Correct Mobile No.");
                    document.getElementById("ctl00_ContentPlaceHolder1_textBoxMobileNo").focus();
                    return false;
                }
            }

            var Landline = document.getElementById("ctl00_ContentPlaceHolder1_textBoxLandline").value;

            if (Landline != "") {
                if (Landline.length < 6) {
                    alert("Please Provide Correct Landline No.");
                    document.getElementById("ctl00_ContentPlaceHolder1_textBoxLandline").focus();
                    return false;
                }
            }

            var emailID = document.getElementById("ctl00_ContentPlaceHolder1_textBoxEmailId").value;
            if (emailID != "") {
                if (emailcheck(emailID) == false) {
                    document.getElementById("ctl00_ContentPlaceHolder1_textBoxEmailId").value = "";
                    document.getElementById("ctl00_ContentPlaceHolder1_textBoxEmailId").focus();
                    return false;
                }
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_DropDownListVehicleClass").selectedIndex == 0) {
                alert("Please Select Correct Vehicle Type");
                document.getElementById("ctl00_ContentPlaceHolder1_DropDownListVehicleClass").focus();
                return false;
            }

            var VehicleMake = document.getElementById("ctl00_ContentPlaceHolder1_textBoxVehicleMake").value;

            if (VehicleMake == "") {
                alert("Please Provide Vehicle Maker.");
                document.getElementById("ctl00_ContentPlaceHolder1_textBoxVehicleMake").focus();
                return false;
            }
            if (invalidChar(document.getElementById("ctl00_ContentPlaceHolder1_textBoxVehicleMake"))) {
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("textBoxVehicleMake").focus();
                return false;
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_DropDownListVehicleModel").selectedIndex == 0) {
                alert("Please Select Correct Vehicle Model");
                document.getElementById("ctl00_ContentPlaceHolder1_DropDownListVehicleModel").focus();
                return false;
            }

            var VehicleRegNo = document.getElementById("ctl00_ContentPlaceHolder1_TextBoxVehicleRegNo").value;

            if (VehicleRegNo == "") {
                alert("Please Provide Vehicle Registration Number.");
                document.getElementById("ctl00_ContentPlaceHolder1_TextBoxVehicleRegNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("ctl00_ContentPlaceHolder1_TextBoxVehicleRegNo"))) {
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("ctl00_ContentPlaceHolder1_TextBoxVehicleRegNo").focus();
                return false;
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_DropDownListOrderType").selectedIndex == 0) {
                alert("Please Select Correct Order Type.");
                document.getElementById("ctl00_ContentPlaceHolder1_DropDownListOrderType").focus();
                return false;
            }

            //            if (document.getElementById("DropDownListFrontPlateSize").selectedIndex == 0) {
            //                alert("Please Select Correct Front Plate Size.");
            //                document.getElementById("DropDownListFrontPlateSize").focus();
            //                return false;
            //            }
            //            if (document.getElementById("DropDownListRearPlateSize").selectedIndex == 0) {
            //                alert("Please Select Correct Rear Plate Size.");
            //                document.getElementById("DropDownListRearPlateSize").focus();
            //                return false;
            //            }

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
            width: 150px !important;
        }
        #divwidth div
        {
            width: 150px !important;
        }
    </style>
    <table width="100%" border="0" align="left">
        <tr>
            <td>
                <div style="margin: 20px;" align="left">
                    <fieldset>
                        <legend>
                            <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                                <span>Add HSRP Records</span>
                            </div>
                        </legend>
                        <div>
                            <table style="background-color: #FFFFFF" border="0" align="left" width="100%" cellpadding="3"
                                cellspacing="1">
                                <tr>
                                    <td>
                                        <table style="background-color: #FFFFFF" width="100%" border="0" align="left" cellpadding="3"
                                            cellspacing="1">
                                            <tr>
                                                <td colspan="2" style="margin-left: 50px" align="left" class="form_text">
                                                    <b>AUTHORZIATION INFORMATION</b>
                                                </td>
                                                <td class="form_text" nowrap="nowrap" align="left" width="140px">
                                                    Record Created By:
                                                </td>
                                                <td width="150px">
                                                    <asp:Label ID="LabelCreatedID" ForeColor="Blue" runat="server" />
                                                </td>
                                                <td class="form_text" nowrap="nowrap" align="left" width="160px">
                                                    Record Creation Date:
                                                </td>
                                                <td>
                                                    <asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />
                                                </td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form_text" nowrap="nowrap" align="left" style="width: 12.8%">
                                        HSRP Auth. No :<span class="alert">* </span>
                                    </td>
                                    <td nowrap="nowrap" style="width: 175px" align="left">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelAuthNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox class="form_textbox11" TabIndex="1" Width="160" ID="textBoxAuthorizationNo"
                                                                runat="server"></asp:TextBox>
                                                            <div id="divwidth">
                                                            </div>
                                                            <asp:AutoCompleteExtender ServiceMethod="GetCustomers" MinimumPrefixLength="1" ServicePath="~/WCFService/ServiceForSuggestion.svc"
                                                                CompletionInterval="10" EnableCaching="false" TargetControlID="textBoxAuthorizationNo"
                                                                ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionSetCount="12"
                                                                CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                CompletionListHighlightedItemCssClass="AutoExtenderHighlight" CompletionListElementID="divwidth">
                                                            </asp:AutoCompleteExtender>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelGo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="buttonGo" runat="server" CausesValidation="false" OnClientClick="javascript:return CheckAuthNo();"
                                                                Text="Go" class="button" TabIndex="2" OnClick="buttonGo_Click"></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--<asp:Button ID="buttonGo" runat="server" OnClientClick="javascript:return CheckAuthNo();"
                                                        Text="Go" class="button" TabIndex="2" OnClick="buttonGo_Click" />--%>
                                    </td>
                                    <td class="form_text" nowrap="nowrap" align="left">
                                        HSRP Auth. Date :<span class="alert">* </span>
                                    </td>
                                    <td>
                                        <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                                            cellpadding="0" border="0">
                                            <tr>
                                                <td valign="top" onmouseup="OrderDate_OnMouseUp()">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelOrderDate" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                ControlType="Picker" PickerCssClass="picker">
                                                                <ClientEvents>
                                                                    <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                </ClientEvents>
                                                            </ComponentArt:Calendar>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td style="font-size: 10px;">
                                                    &nbsp;
                                                </td>
                                                <td valign="top">
                                                    <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="form_text" align="left" nowrap="nowrap" width="120px">
                                        Order No :
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanelOrderNo" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="textBoxOrderNo" class="form_textbox11" Enabled="false" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form_text" align="left">
                                        Order Date :
                                    </td>
                                    <td valign="bottom" align="left">
                                        <table style="margin-left: 8px" align="left" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelHSRPAuthDate" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                ControlType="Picker" PickerCssClass="picker">
                                                                <ClientEvents>
                                                                    <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                </ClientEvents>
                                                            </ComponentArt:Calendar>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td style="font-size: 10px;">
                                                    &nbsp;
                                                </td>
                                                <td valign="top">
                                                    <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                        class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="form_text" align="left">
                                        Customer Name :<span class="alert">* </span>
                                    </td>
                                    <td align="left" width="220px">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanelCustomerName" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="textBoxCustomerName" runat="server" class="form_textbox11" TabIndex="5"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td class="form_text" align="left" nowrap="nowrap">
                                        Order Status :
                                    </td>
                                    <td align="left" style="margin-left: 8px">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanelOrderStatus" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="textBoxOrderStatus" class="form_textbox11" Enabled="false" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form_text" align="left">
                                        Address1 :<span class="alert">* </span>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanelAddress1" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="textBoxAddress1" Width="493px" Columns="400" Rows="2" class="form_textbox11"
                                                    runat="server" TabIndex="6"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td class="form_text" align="left">
                                        Mobile No :
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanelMobile" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="textBoxMobileNo" runat="server" MaxLength="10" TabIndex="7" class="form_textbox11"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form_text" align="left">
                                        Landline :
                                    </td>
                                    <td align="left">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanelLandline" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="textBoxLandline" MaxLength="12" class="form_textbox11" runat="server"
                                                    TabIndex="8"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td class="form_text" align="left" width="141px">
                                        Email Id :
                                    </td>
                                    <td align="left" width="180px">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanelEmail" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox ID="textBoxEmailId" class="form_textbox11" runat="server" TabIndex="9"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                                <td class="form_text" align="left">
                                                    Delievery Challan :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelDelieveryChallan" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBox1" runat="server" MaxLength="10" TabIndex="7" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                </tr>
                            </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                    <tr valign="top">
                        <td colspan="6" style="margin-left: 50px" align="left" class="form_text">
                            <b>VEHICLE INFO</b>
                            <asp:TextBox Visible="false" Width="1px" ID="textBoxAddress2" runat="server" class="form_textbox11"
                                TabIndex="9"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" align="left" style="width: 12.8%">
                            Vehicle Class :<span class="alert">* </span>
                        </td>
                        <td align="left" width="200px">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanelVehicleClass">
                                <ContentTemplate>
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" Width="185px" ID="DropDownListVehicleClass"
                                        AutoPostBack="true" runat="server" TabIndex="10" OnSelectedIndexChanged="DropDownListVehicleClass_SelectedIndexChanged">
                                        <asp:ListItem Value="--Select Vehicle Class--" Text="--Select Vehicle Class--"></asp:ListItem>
                                        <asp:ListItem Value="Transport" Text="Transport"></asp:ListItem>
                                        <asp:ListItem Value="Non-Transport" Text="Non-Transport"></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left" nowrap="nowrap" width="141px">
                            Vehicle Make :<span class="alert">* </span>
                        </td>
                        <td align="left" style="width: 220px">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleMake" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="textBoxVehicleMake" class="form_textbox11" runat="server" TabIndex="11"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left" nowrap="nowrap" width="120px">
                            Vehicle Model :<span class="alert">* </span>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleType" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" CausesValidation="false"
                                        AutoPostBack="true" ID="DropDownListVehicleModel" Width="185px" runat="server"
                                        TabIndex="12" OnSelectedIndexChanged="DropDownListVehicleModel_SelectedIndexChanged">
                                        <asp:ListItem Value="--Select Vehicle Model--" Text="--Select Vehicle Model--"></asp:ListItem>
                                        <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                        <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                        <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                        <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                        <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                        <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                        <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ButtonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" nowrap="nowrap" align="left">
                            Vehicle Reg No :<span class="alert">* </span>
                        </td>
                        <td class=" textboxboxstyle" align="left">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleRegNo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="TextBoxVehicleRegNo" MaxLength="10" class="form_textbox11" runat="server"
                                        TabIndex="13"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left">
                            Engine No :
                        </td>
                        <td align="left" width="220px">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelEngineNo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="textboxEngineNo" Enabled="true" class="form_textbox11" runat="server"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left">
                            Chassis No :
                        </td>
                        <td align="left">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelChassisNo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="textBoxChassisNo" Width="160" Enabled="true" class="form_textbox11"
                                        runat="server"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                    <tr valign="top">
                        <td colspan="7" style="margin-left: 50px" align="left" class="form_text">
                            <b>NUMBER PLATE INFO</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" nowrap="nowrap" align="left" style="width: 13.4%">
                            Order Type :
                        </td>
                        <td align="left" width="193px">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelOrderType" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" AutoPostBack="true"
                                        CausesValidation="false" ID="DropDownListOrderType" runat="server" TabIndex="14"
                                        Width="185px" OnSelectedIndexChanged="DropDownListOrderType_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                        <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                        <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                        <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                        <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                        <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                        <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ButtonSave" EventName="Click" />
 <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td align="left" style="width: 145px">
                            <asp:UpdatePanel ID="UpdatePanelCheckBoxFrontPlate" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:CheckBox Text="Front Plate" Enabled="false" class="form_text" TextAlign="Right"
                                        ID="checkBoxFrontPlate" runat="server" />&nbsp;&nbsp;
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td align="left" width="14%">
                            <asp:UpdatePanel ID="UpdatePanelDropDownListFrontPlate" UpdateMode="Conditional"
                                runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" CausesValidation="false"
                                        AutoPostBack="true" ID="DropDownListFrontPlateSize" Width="185px" DataTextField="ProductCode"
                                        DataValueField="ProductID" runat="server" TabIndex="15" OnSelectedIndexChanged="DropDownListFrontPlateSize_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="checkBoxFrontPlate" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
 <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" align="left">
                            <asp:CheckBox Text="Snap Lock" Checked="true" Enabled="false" class="form_text" TextAlign="Right"
                                ID="checkBoxSnapLock" runat="server" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanelThirdSticker" ChildrenAsTriggers="true" UpdateMode="Conditional"
                                runat="server">
                                <ContentTemplate>
                                    <asp:CheckBox Text="Third sticker" Enabled="false" class="form_text" TextAlign="Right"
                                        ID="checkBoxThirdSticker" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanelCheckBoxRearPlate" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:CheckBox Text="Rear Plate" Enabled="false" TextAlign="Right" class="form_text"
                                        ID="checkBoxRearPlate" runat="server" />
                                    &nbsp;&nbsp;
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanelDropDownListRearPlate" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" AutoPostBack="true"
                                        DataTextField="ProductCode" DataValueField="ProductID" ID="DropDownListRearPlateSize"
                                        Width="185px" CausesValidation="false" runat="server" TabIndex="16" OnSelectedIndexChanged="DropDownListRearPlateSize_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="checkBoxRearPlate" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
									  <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                    <tr valign="top">
                        <td colspan="10" align="left" class="form_text" style="margin-left: 50px">
                            <b>FINANCIAL INFO</b>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" align="left" width="13%">
                            Invoice No :
                        </td>
                        <td align="left" width="196px">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelInvoiceNo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox class="form_textbox11" ID="textBoxInvoiceNo" Enabled="false" runat="server"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left" nowrap="nowrap">
                            Cash Receipt No :
                        </td>
                        <td align="left" width="220px">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelCashReceiptNo" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="textBoxCashReceiptNo" class="form_textbox11" Enabled="false" runat="server"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" align="left">
                            Total Amount :
                        </td>
                        <td align="left" width="193px">
                            <asp:UpdatePanel ID="UpdatePanelTotalAmount" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="textBoxTotalAmount" Enabled="false" class="form_textbox11" runat="server"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListFrontPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListRearPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
 <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left">
                            VAT (%) :
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanelVat" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="textBoxVat" runat="server" Enabled="false" class="form_textbox11"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListFrontPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListRearPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left" nowrap="nowrap" width="120px">
                            <span id="Span12">VAT Amount : </span>
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanelVatAmount" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="textBoxVatAmount" runat="server" Enabled="false" class="form_textbox11"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListFrontPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListRearPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" nowrap="nowrap" align="left">
                            Service Tax (%) :
                        </td>
                        <td align="left" width="193px">
                            <asp:UpdatePanel ID="UpdatePanelServiceTax" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="textBoxServiceTax" Enabled="false" class="form_textbox11" runat="server"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListFrontPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListRearPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left" nowrap="nowrap" width="144px">
                            Service Tax Amount :
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanelServiceTaxAmount" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="textboxServiceTaxAmount" Enabled="false" class="form_textbox11"
                                        runat="server"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListFrontPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListRearPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="form_text" align="left">
                            Net Total :
                        </td>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanelNetTotal" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="textBoxNetTotal" runat="server" Enabled="false" class="form_textbox11"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListFrontPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListRearPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="alert" nowrap="nowrap" align="left">
                            * Fields are mandatory.
                        </td>
                        <td colspan="6" align="left">
                            <asp:UpdatePanel ID="UpdatePanelMessage" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListFrontPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListRearPlateSize" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="buttonGo" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="nowrap" align="right" colspan="4" style="margin-right: 10px;">
                            <asp:Button ID="buttonUpdate" runat="server" TabIndex="17" class="button" Visible="false"
                                Text="Update" />
                           
                        </td>
                        <td align="right">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="buttonSave" runat="server" TabIndex="18" OnClientClick="javascript:return ValidateForm();"
                                        class="button" Text="Save" Visible="false" OnClick="buttonSave_Click" />&nbsp;
                                    <asp:Button Text="reset" ID="Button1" class="button" runat="server" OnClientClick="this.form.reset();"
                                        OnClick="Button1_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <input type="button" onclick="javascript:parent.googlewin.close();" tabindex="19"
                                                        name="buttonClose" id="buttonClose" value="Close" class="button" />
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
                    <tr>
                        <td colspan="6">
                            <ComponentArt:Calendar runat="server" ID="CalendarHSRPAuthDate" AllowMultipleSelection="false"
                                AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                PopUp="Custom" PopUpExpandControlId="ImgPollution" CalendarTitleCssClass="title"
                                DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                <ClientEvents>
                                    <SelectionChanged EventHandler="CalendarHSRPAuthDate_OnChange" />
                                </ClientEvents>
                            </ComponentArt:Calendar>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </div> </fieldset> </div> </td> </tr> </table>
</asp:Content>
