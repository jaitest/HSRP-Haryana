<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerDataEntry.aspx.cs"
    Inherits="HSRP.Dealer.Transaction.DealerDataEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
 <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../javascript/common.js"></script>
  <script type="text/javascript" src="../../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/toword.js" type="text/javascript"></script>
     <style type="text/css">
    .alert
    {
        color:Red;
    }
        .style4
        {
            width: 203px;
        }
    </style>
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #333333;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        
         .btnhide
        {
            display:none;
        }
        
        .HellowWorldPopup
        {
            width:1000px;
           -webkit-border-radius: 38px 39px 39px 38px;-moz-border-radius: 38px 39px 39px 38px;border-radius: 38px 39px 39px 38px;border:4px solid #FF3617;background-color:#FAFFF0;-webkit-box-shadow: #B3B3B3 9px 9px 9px;-moz-box-shadow: #B3B3B3 9px 9px 9px; box-shadow: #B3B3B3 9px 9px 9px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function test_skill() {
            var junkVal = document.getElementById('txtRoomPrice').value;
            junkVal = Math.floor(junkVal);
            var obStr = new String(junkVal);
            numReversed = obStr.split("");
            actnumber = numReversed.reverse();

            if (Number(junkVal) >= 0) {
                //do nothing
            }
            else {
                alert('wrong Number cannot be converted');
                return false;
            }
            if (Number(junkVal) == 0) {
                document.getElementById('container').innerHTML = obStr + '' + 'Rupees Zero Only';
                return false;
            }
            if (actnumber.length > 9) {
                alert('Oops!!!! the Number is too big to covertes');
                return false;
            }

            var iWords = ["Zero", " One", " Two", " Three", " Four", " Five", " Six", " Seven", " Eight", " Nine"];
            var ePlace = ['Ten', ' Eleven', ' Twelve', ' Thirteen', ' Fourteen', ' Fifteen', ' Sixteen', ' Seventeen', ' Eighteen', ' Nineteen'];
            var tensPlace = ['dummy', ' Ten', ' Twenty', ' Thirty', ' Forty', ' Fifty', ' Sixty', ' Seventy', ' Eighty', ' Ninety'];

            var iWordsLength = numReversed.length;
            var totalWords = "";
            var inWords = new Array();
            var finalWord = "";
            j = 0;
            for (i = 0; i < iWordsLength; i++) {
                switch (i) {
                    case 0:
                        if (actnumber[i] == 0 || actnumber[i + 1] == 1) {
                            inWords[j] = '';
                        }
                        else {
                            inWords[j] = iWords[actnumber[i]];
                        }
                        inWords[j] = inWords[j] + ' Only';
                        break;
                    case 1:
                        tens_complication();
                        break;
                    case 2:
                        if (actnumber[i] == 0) {
                            inWords[j] = '';
                        }
                        else if (actnumber[i - 1] != 0 && actnumber[i - 2] != 0) {
                            inWords[j] = iWords[actnumber[i]] + ' Hundred and';
                        }
                        else {
                            inWords[j] = iWords[actnumber[i]] + ' Hundred';
                        }
                        break;
                    case 3:
                        if (actnumber[i] == 0 || actnumber[i + 1] == 1) {
                            inWords[j] = '';
                        }
                        else {
                            inWords[j] = iWords[actnumber[i]];
                        }
                        if (actnumber[i + 1] != 0 || actnumber[i] > 0) {
                            inWords[j] = inWords[j] + " Thousand";
                        }
                        break;
                    case 4:
                        tens_complication();
                        break;
                    case 5:
                        if (actnumber[i] == 0 || actnumber[i + 1] == 1) {
                            inWords[j] = '';
                        }
                        else {
                            inWords[j] = iWords[actnumber[i]];
                        }
                        if (actnumber[i + 1] != 0 || actnumber[i] > 0) {
                            inWords[j] = inWords[j] + " Lakh";
                        }
                        break;
                    case 6:
                        tens_complication();
                        break;
                    case 7:
                        if (actnumber[i] == 0 || actnumber[i + 1] == 1) {
                            inWords[j] = '';
                        }
                        else {
                            inWords[j] = iWords[actnumber[i]];
                        }
                        inWords[j] = inWords[j] + " Crore";
                        break;
                    case 8:
                        tens_complication();
                        break;
                    default:
                        break;
                }
                j++;
            }

            function tens_complication() {
                if (actnumber[i] == 0) {
                    inWords[j] = '';
                }
                else if (actnumber[i] == 1) {
                    inWords[j] = ePlace[actnumber[i - 1]];
                }
                else {
                    inWords[j] = tensPlace[actnumber[i]];
                }
            }
            inWords.reverse();
            for (i = 0; i < inWords.length; i++) {
                finalWord += inWords[i];
            }
            //document.getElementById('container').innerHTML = obStr + '  ' + finalWord;
            document.getElementById('container').innerHTML =  finalWord;
        }
    
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

    <title></title>

     <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function validate() {


        <%-- if (document.getElementById("<%=ddlLocation.ClientID%>").value == "--Select Location--") {
                alert("Select Location");
                document.getElementById("<%=ddlLocation.ClientID%>").focus();
               return false;
           }--%>

            if (document.getElementById("<%=txtVehicleRegNo.ClientID%>").value == "") {
                alert("Please Provide Vehicle Reg No");
                document.getElementById("<%=txtVehicleRegNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--") {
                alert("Select Vehicle Class");
                document.getElementById("<%=DropDownListVehicleClass.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListVehicleModel.ClientID%>").value == "--Select Vehicle Model--") {
                alert("Select Vehicle Model");
                document.getElementById("<%=DropDownListVehicleModel.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "--Select Vehicle Maker--") {
                alert("Select Vehicle Maker");
                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListModel.ClientID%>").value == "--Select Vehicle Model--") {
                alert("Select Vehicle Model");
                document.getElementById("<%=DropDownListModel.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtEngineNo.ClientID%>").value == "") {
                alert("Please Provide Engine No");
                document.getElementById("<%=txtEngineNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtchassisNo.ClientID%>").value == "") {
                alert("Please Provide Chassis No");
                document.getElementById("<%=txtchassisNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtAuthNo.ClientID%>").value == "") {
                alert("Please Provide Auth No");
                document.getElementById("<%=txtAuthNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtCustomerName.ClientID%>").value == "") {
                alert("Please Provide Customer Name");
                document.getElementById("<%=txtCustomerName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtAddress.ClientID%>").value == "") {
                alert("Please Provide Address");
                document.getElementById("<%=txtAddress.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtMobileNo.ClientID%>").value == "") {
                alert("Please Provide Mobile No");
                document.getElementById("<%=txtMobileNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtRoomPrice.ClientID%>").value == "") {
                alert("Please Provide Room Price");
                document.getElementById("<%=txtRoomPrice.ClientID%>").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxPrefiex"))) {
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("textboxPrefiex").focus();
                return false;
            }
        }
        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script> 
    <style type="text/css">
    .alert
    {
        color:Red;
    }
        .style4
        {
            width: 215px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <fieldset>
            <legend>
                <div style="margin-left: 5px; font-size: medium; color: Black; width: 198px;">
                   Data Entry 
                    For New Vehicle</div>
            </legend>
          
        
        <table width="98%">
        <%--<tr >
        <td colspan="2" class="form_text">
        Affixaction Location Required <span class="alert">* </span>
        </td>
        <td>
            <asp:DropDownList ID="ddlLocation" runat="server"  Width="150px" Style="margin-left: 8px"  DataTextField="RTOlocationName"
                                DataValueField="RTOlocationID">
            </asp:DropDownList>
        </td>
        </tr>--%>
            <tr>
                <td class="form_text">
                    Vehicle Reg No
                <span class="alert">* </span>
                </td>
                <td align="left" class="style4">
                    <asp:TextBox ID="txtVehicleRegNo" onblur="this.value=removeSpaces(this.value);" Style="text-transform: uppercase" runat="server" class="form_textbox11"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonVehicleGo" runat="server" class="button" 
                        onclick="ButtonVehicleGo_Click" Text="Go" />
                </td>
                 <td class="form_text" align="left" nowrap="nowrap" width="120px">&nbsp;</td>
                <td class="form_text">
                    Authorization No
                <span class="alert">* </span>
                </td>
                <td>
                    <asp:TextBox ID="txtAuthNo" Style="text-transform: uppercase"  runat="server" class="form_textbox11"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonAuthorizationNo" runat="server" CssClass="button" 
                        onclick="ButtonAuthorizationNo_Click" Text="Go" />
                </td>
            </tr>
            <tr>
                <td class="form_text">
                    Authorization Date
                    <span class="alert">* </span>
                </td>
                <td align="left" class="style4">
                    <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                        cellpadding="0" border="0">
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
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../../images/btn_calendar.gif" />
                                                            </td>
                        </tr>
                    </table>
                </td>
                 <td class="form_text" align="left" nowrap="nowrap" width="120px"></td>
                <td class="form_text">                    
                    Vehicle Class <span class="alert">* </span></td>
                <td>
                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleClass" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList  Font-Size="Small" AutoPostBack="true"  Style="margin-left: 8px"
                                Width="150px" ID="DropDownListVehicleClass" runat="server" TabIndex="12">
                                <asp:ListItem Value="--Select Vehicle Class--" Text="--Select Vehicle Class--"></asp:ListItem>
                                <asp:ListItem Value="Transport" Text="Commerical"></asp:ListItem>
                                <asp:ListItem Value="Non-Transport" Text="Private"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <%-- <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />--%>
                        </Triggers>
                    </asp:UpdatePanel></td>
            </tr>
            <tr>
                <td class="form_text">
                    Vehicle Type <span class="alert">* </span>
                </td>
                <td class="style4">
                    
                            <asp:DropDownList ID="DropDownListVehicleModel" runat="server" 
                                AutoPostBack="true" CausesValidation="false" Font-Size="Small" 
                                Style="margin-left: 8px" TabIndex="13" Width="150px" 
                                onselectedindexchanged="DropDownListVehicleModel_SelectedIndexChanged">
                                <asp:ListItem Text="--Select Vehicle Model--" Value="--Select Vehicle Model--"></asp:ListItem>
                                <asp:ListItem Text="SCOOTER" Value="SCOOTER"></asp:ListItem>
                                <asp:ListItem Text="MOTOR CYCLE" Value="MOTOR CYCLE"></asp:ListItem>
                                <asp:ListItem Text="TRACTOR" Value="TRACTOR"></asp:ListItem>
                                <asp:ListItem Text="THREE WHEELER" Value="THREE WHEELER"></asp:ListItem>
                                <asp:ListItem Text="LMV" Value="LMV"></asp:ListItem>
                                <asp:ListItem Text="LMV(CLASS)" Value="LMV(CLASS)"></asp:ListItem>
                                <asp:ListItem Text="MCV/HCV/TRAILERS" Value="MCV/HCV/TRAILERS"></asp:ListItem>
                            </asp:DropDownList>
                        
                            <%-- <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />--%>
                        
                    
                </td>
                 <td class="form_text" align="left" nowrap="nowrap" width="120px"></td>
                <td class="form_text" align="left" nowrap="nowrap" width="120px">
                    Manufacture Name <%-- <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />--%>
                    <span class="alert">*</span></td>
                <td align="left">
                    
      <%-- <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />--%><%-- <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />--%>
                    <asp:UpdatePanel ID="UpdatePanelVehicleMake" runat="server" 
                        ChildrenAsTriggers="true" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="DropDownListVehicleMaker1" runat="server" 
                                AutoPostBack="true" DataTextField="VehicleMakerDescription" 
                                DataValueField="VehicleMakerID" EnableViewState="true" Font-Size="Small" 
                                OnSelectedIndexChanged="DropDownList44_SelectedIndexChanged" 
                                Style="margin-left: 8px" TabIndex="14" Width="150px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <%--    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                    
                </td>
            </tr>
            <tr>
                <td class="form_text" align="left" nowrap="nowrap" width="141px">
                    Vehicle Model <%-- <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />--%>
                    <span class="alert">* </span>
            </td>
                <td align="left" class="style4">
              
                    <asp:UpdatePanel ID="UpdatePanelVehicleModel1" runat="server" 
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <%-- <asp:TextBox ID="textBoxVehicleMake" class="form_textbox11" runat="server" TabIndex="12"></asp:TextBox>--%>
                            <asp:DropDownList ID="DropDownListModel" runat="server" 
                                CausesValidation="false" DataTextField="VehicleModelDescription" 
                                DataValueField="VehicleModelID" Font-Size="Small" Style="margin-left: 8px" 
                                TabIndex="15" Width="150px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <%--  <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
              
                </td>
                 <td class="form_text" align="left" nowrap="nowrap" width="120px"></td>
                <td class="form_text" align="left" nowrap="nowrap" width="141px">
                    Customer Name
                <span class="alert">* </span>
                </td>
                <td colspan="2" align="left" style="width: 180px">
                    
                    <asp:TextBox ID="txtCustomerName" Style="text-transform: uppercase"  runat="server" class="form_textbox11"></asp:TextBox>
                    
                </td>
            </tr>
            <tr>
                <td class="form_text">
                    Engine No
                <span class="alert">* </span>
                </td>
                <td class="style4">
                    <asp:TextBox ID="txtEngineNo" Style="text-transform: uppercase"  runat="server" class="form_textbox11"></asp:TextBox>
                </td>
                 <td class="form_text" align="left" nowrap="nowrap" width="120px"></td>
                <td class="form_text">
                    Chassis No
                <span class="alert">* </span>
                </td>
                <td>
                    <asp:TextBox ID="txtchassisNo" Style="text-transform: uppercase"  runat="server" class="form_textbox11"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="form_text">
                    Third Sticker Mandatory&nbsp; <span class="alert">* </span></td>
                <td class="style4">                    
                    <asp:CheckBox ID="checkBoxThirdSticker" runat="server" />
                </td>
                
                 <td class="form_text" align="left" nowrap="nowrap" width="120px"></td>
                 <td class="form_text">
                     Oder Type:-</td>
                <td>
                    <asp:Label ID="LblOrderType" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
               
                <td class="form_text">
                    Address
                <span class="alert">* </span>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtAddress" runat="server" class="form_textbox11"  
                        TextMode="MultiLine" Width="480px" Height="40px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="form_text">
                    Mobile No
                <span class="alert">* </span>
                </td>
                <td class="style4">
                    <asp:TextBox ID="txtMobileNo" runat="server" class="form_textbox11" MaxLength="10"  onkeypress="return isNumberKey(event)"></asp:TextBox>
                </td>
                <td class="form_text">
                 Amount

                <span class="alert">* </span>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtRoomPrice" runat="server" class="form_textbox11" 
                        MaxLength="9" onkeyup="test_skill()"  onkeypress="return isNumberKey(event)" 
                        Enabled="False"></asp:TextBox> <br /> 
                     
                    <b><div style="width:389px; color:Red"  id="container"></div></b>
                </td>
                 
                <td>
                </td>
            </tr>
              <tr>
                        <td colspan="2">
                            <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                            <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                      
                        <td colspan="2" align="right">
                            <%--    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />--%>
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" TabIndex="4" class="button" OnClientClick=" return validate()"
                                OnClick="btnSave_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnReset" runat="server" Text="RESET" TabIndex="4" class="button" onclick="btnReset_Click"/>&nbsp;&nbsp;
                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                id="buttonClose" value="CLOSE" class="button" />
                           
                          <%-- <asp:DropDownList runat="server"  DataTextField="VehicleMakerDescription"
                                                        DataValueField="VehicleMakerID" TabIndex="15" EnableViewState="true" Font-Size="Small"
                                                        AutoPostBack="true" ID="DropDownList44" 
                                                        onselectedindexchanged="DropDownList44_SelectedIndexChanged"></asp:DropDownList>--%>
                        </td>
                    </tr>
        </table>

        </fieldset>
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
    </div>
      <asp:Button ID="Button2" runat="server" Text="Button"  Enabled="false" 
        CssClass="btnhide" onclick="Button2_Click" />
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CancelControlID="Button1"
        OkControlID="Button1" TargetControlID="Button2" PopupControlID="Panel1" PopupDragHandleControlID="PopupHeader"
        Drag="true" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    
    <asp:Panel ID="Panel1" Style="display: none" runat="server">
        <div class="HellowWorldPopup">
            <div class="PopupHeader" id="PopupHeader">
             </div>
            <div class="PopupBody">
                <p>
                    <div style="margin-left: 10px; font-size: 18px; color:Green">
                                    Dealer Data New Vehicle Entry
                                </div>  
                                &nbsp;<asp:Panel ID="Panel3" runat="server">
                    <div align="center" style="width: 100%; ">                     
                         
                            <asp:Label ID="lblMesageSave" runat="server" Text="" ForeColor="Blue" Font-Size="20px" Font-Bold="true"></asp:Label><br /><br />
                          
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" Width="100%" BorderColor="#3366CC"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                                AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Owner Name" >
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("OwnerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle No">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("VehicleRegNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Authorization No">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("HSRPRecord_AuthorizationNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle Type">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("VehicleType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle Class">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("VehicleClass") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Engine No" >
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("EngineNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Chassis No">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("ChassisNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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
                                        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="button" />
                                    </td>
                                    <td>
                                            <input id="Button1" type="button" value="Cancel" class="button" />
                                    </td>
                                </tr>
                            </table>
                       
                    </div>
                </asp:Panel>
                </asp:Panel>
    </form>
</body>
</html>
