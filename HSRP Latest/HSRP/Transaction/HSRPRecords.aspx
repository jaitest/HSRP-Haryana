<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HSRPRecords.aspx.cs" Inherits="HSRP.Transaction.HSRPRecords" ValidateRequest="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.alerts.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.js" type="text/javascript"></script>
    <link href="../css/jquery.alerts.css" rel="stylesheet" type="text/css" />

    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script>
    <script type="text/javascript">
        function OnSelectedIndexChangeVehicleModel() {
            if (document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--") {
                alert("Select correct Vehicle Class.");
                //document.getElementById("<%=DropDownListVehicleClass.ClientID%>").options[0].selected = true;
                //document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--";
                document.getElementById("<%=DropDownListVehicleModel.ClientID%>").selectedIndex = 0;
                document.getElementById("<%=DropDownListVehicleClass.ClientID%>").focus();
                return false;
            }
        }

        function OnSelectedIndexChangeVehicleMaker() {
            //            if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "--Select Vehicle Maker--") {
            //                alert("Select correct Vehicle Maker.");
            //                //document.getElementById("<%=DropDownListVehicleClass.ClientID%>").options[0].selected = true;
            //                //document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--";
            //                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").selectedIndex = 0;
            //                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
            //                return false;
            //            }
        }


        function OnSelectedIndexChangeModel() {
            //            if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "--Select Vehicle Maker--") {
            //                alert("Select correct Vehicle Maker.");
            //                document.getElementById("<%=DropDownListModel.ClientID%>").selectedIndex = 0;
            //                document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
            //                return false;
            //            }
        }



        function OnSelectedIndexChangeVehicleOrder() {
            if (document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--") {
                alert("Select correct Vehicle Class.");
                document.getElementById("<%=DropDownListVehicleClass.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListVehicleModel.ClientID%>").value == "--Select Vehicle Model--") {
                alert("Select correct Vehicle Type.");
                document.getElementById("<%=DropDownListVehicleModel.ClientID%>").focus();
                return false;
            }
//                        if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "0") {
//                            alert("Select correct Vehicle Maker.");
//                            document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
//                            return false;
//                        }
//                        if (document.getElementById("<%=DropDownListModel.ClientID%>").value == "0") {
//                            alert("Select correct Vehicle Model.");
//                            document.getElementById("<%=DropDownListModel.ClientID%>").focus();
//                            return false;
//                        }
        }
    </script>
   
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

        function CashDate_OnDateChange(sender, eventArgs) {
            var fromDate1 = CashDate.getSelectedDate();
            CalendarCashDate.setSelectedDate(fromDate1);

        }

        function CashDate_OnChange(sender, eventArgs) {
            var fromDate2 = CalendarCashDate.getSelectedDate();
            CashDate.setSelectedDate(fromDate2);

        }

        function CashDate_OnClick() {
            if (CalendarCashDate.get_popUpShowing()) {
                CalendarCashDate.hide();
            }
            else {
                CalendarCashDate.setSelectedDate(CashDate.getSelectedDate());
                CalendarCashDate.show();
            }
        }

        function CashDate_OnMouseUp() {
            if (CalendarCashDate.get_popUpShowing()) {
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




        function CheckAuthNo() {
            var textBoxAuthrizationNo = document.getElementById("textBoxAuthorizationNo").value;

            if (textBoxAuthrizationNo == "") {
                //jAlert("Please Provide Authorization No.","Order Form : Requirement");
                alert("Please Provide Authorization No.");
                document.getElementById("textBoxAuthorizationNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxAuthorizationNo"))) {
                jAlert("Please Provide Valid Authorization No.", "Order Form : Requirement");
                document.getElementById("textBoxAuthorizationNo").focus();
                return false;
            }

            // ValidRegNo(document.getElementById("textBoxAuthorizationNo").value, document.getElementById("<%=LabelCreatedID.ClientID%>").innerText);

        }

        function CheckVehicleRegNo() {
            var TextBoxVehicleRegNo = document.getElementById("TextBoxVehicleRegNo").value;
            // alert(TextBoxVehicleRegNo);
            if (TextBoxVehicleRegNo == "") {
                jAlert("Please Provide Vehicle Registration No.", "Order Form : Requirement");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxVehicleRegNo"))) {
                jAlert("Please Provide Valid Registration No.", "Order Form : Requirement");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }

//            if (!ValidRegNo(document.getElementById("TextBoxVehicleRegNo").value, document.getElementById("<%=LabelCreatedID.ClientID%>").innerText)) {
//                jAlert("Please Provide Valid Registration No.", "Order Form : Requirement");
//                document.getElementById("TextBoxVehicleRegNo").focus();
//                return false;
//            }
        }

        function CheckCashReceiptNo() {
            var TextBoxCashReceiptNo = document.getElementById("TextBoxCashReceiptNo").value;
            if (TextBoxCashReceiptNo == "") {
                alert("Please Provide Cash Receipt No.");
 //               jAlert("Please Provide Cash Receipt No.", "Order Form : Requirement");
                document.getElementById("TextBoxCashReceiptNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("TextBoxCashReceiptNo"))) {
                //             jAlert("Please Provide Valid Cash Receipt No.","Order Form : Requirement");
                alert("Please Provide Valid Cash Receipt No.");
                document.getElementById("TextBoxCashReceiptNo").focus();
                return false;
            }
        }

        
    </script>
    <script type="text/javascript">
        function ClearDate() {
            OrderDate.ClearSelectedDate();
            CalendarOrderDate.ClearSelectedDate();
            CashDate.ClearSelectedDate();
            CalendarCashDate.ClearSelectedDate();
        }
    </script>
    <script type="text/javascript">
        function ValidateForm() {
           // alert(CashDate.getSelectedDate());
            if (document.getElementById("<%=dropDownListRTOLocation.ClientID%>").value == "--Select RTO Location--") {
                //jAlert("Select correct RTO Location ", "Order Form : Requirement");
                alert("Select correct RTO Location ");
                document.getElementById("<%=dropDownListRTOLocation.ClientID%>").focus();
                return false;
                        }

            var textBoxAuthrizationNo = document.getElementById("textBoxAuthorizationNo").value;
            if (textBoxAuthrizationNo == "") {
                alert("Please Provide Authorization No.");
               // jAlert("Please Provide Authorization No. ", "Order Form : Requirement");
                document.getElementById("textBoxAuthorizationNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxAuthorizationNo"))) {
                //jAlert("Please Provide Valid Authorization No.", "Order Form : Requirement");
                alert("Please Provide Valid Authorization No.");
                document.getElementById("textBoxAuthorizationNo").focus();
                return false;
            }
            if (OrderDate.getSelectedDate() == "" || OrderDate.getSelectedDate()==null) {
                alert("Please select Auth. Date.");
              //  document.getElementById("OrderDate").focus();
                return false;
            }
            var today = new Date();
            if (OrderDate.getSelectedDate() > today) {
                alert("Auth. date should not greater than current date.");
                OrderDate.ClearSelectedDate();
                CalendarOrderDate.ClearSelectedDate();
                return false;
            }
           
            var CustomerName = document.getElementById("textBoxCustomerName").value;

            if (CustomerName.trim() == "") {
                //jAlert("Please Provide Customer Name.", "Order Form : Requirement");
                alert("Please Provide Customer Name.");
                document.getElementById("textBoxCustomerName").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxCustomerName"))) {
                //jAlert("Your can't enter special characters.", "Order Form : Requirement");
                alert("Your can't enter special characters.");
                document.getElementById("textBoxCustomerName").focus();
                return false;
            }

            var Address1 = document.getElementById("textBoxAddress1").value;
            if (Address1 == "") {
                //jAlert("Please Provide address.", "Order Form : Requirement");
                alert("Please Provide address.");
                document.getElementById("textBoxAddress1").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxAddress1"))) {
                //jAlert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.", "Order Form : Requirement");
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("textBoxAddress1").focus();
                return false;
            }


            var Landline = document.getElementById("textBoxLandline").value;

            if (Landline != "") {
                if (Landline.length < 6) {
                    //jAlert("Please Provide Correct Landline No.", "Order Form : Requirement");
                    alert("Please Provide Correct Landline No.");
                    document.getElementById("textBoxLandline").focus();
                    return false;
                }
            }

            var emailID = document.getElementById("textBoxEmailId").value;
            if (emailID != "") {
                if (emailcheck(emailID) == false) {
                    document.getElementById("textBoxEmailId").value = "";
                    document.getElementById("textBoxEmailId").focus();
                    return false;
                }
            }
            //            if (document.getElementById("DropDownListVehicleClass").selectedIndex == 0) {
            //                alert("Please Select Correct Vehicle Type");
            //                document.getElementById("DropDownListVehicleClass").focus();
            //                return false;
            //            }

            //            var VehicleMake = document.getElementById("textBoxVehicleMake").value;

            //            if (VehicleMake == "") {
            //                alert("Please Provide Vehicle Maker.");
            //                document.getElementById("textBoxVehicleMake").focus();
            //                return false;
            //            }
            // alert("mm");
           //debugger
           var CashReceiptNo = document.getElementById("TextBoxCashReceiptNo").value;
//           alert(CashReceiptNo);
            if (CashReceiptNo != "") {
                if (CashDate.getSelectedDate() == "" || CashDate.getSelectedDate() == null) {
                    alert("Please select Cash Date.");
                   // document.getElementById("CashDate").focus();
                    return false;
                }
                
               // debugger
                var today = new Date();
                if (CashDate.getSelectedDate() > today) {
                    alert("Cash Receipt date should not greater than current date.");
                    CashDate.ClearSelectedDate();
                    CalendarCashDate.ClearSelectedDate();
                    return false;
                }
            }
            else {

                if (CashDate.getSelectedDate() == "" || CashDate.getSelectedDate() == null) {
                
                }
                else {
                    alert("Without Cash Receipt no need to fill Casd Reeceipt date .");
                    CashDate.ClearSelectedDate();
                    CalendarCashDate.ClearSelectedDate();
                    return false;
                }
            }
            if (document.getElementById("<%=DropDownListVehicleClass.ClientID%>").value == "--Select Vehicle Class--") {
                //jAlert("Select correct Vehicle Class.", "Order Form : Requirement");
                alert("Select Vehicle Class.");
                // document.getElementById("<%=DropDownListVehicleClass.ClientID%>").selectedIndex = 0;
                document.getElementById("<%=DropDownListVehicleClass.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListVehicleModel.ClientID%>").value == "--Select Vehicle Model--") {
                //jAlert("Select correct Vehicle Type.", "Order Form : Requirement");
                alert("Select Vehicle Type.");
                //document.getElementById("<%=DropDownListVehicleModel.ClientID%>").selectedIndex = 0;
                document.getElementById("<%=DropDownListVehicleModel.ClientID%>").focus();
                return false;
            }
//                        if (document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").value == "--Select Vehicle Maker--") {
//                            alert("Select correct Vehicle Maker.");
//                            document.getElementById("<%=DropDownListVehicleMaker1.ClientID%>").focus();
//                            return false;
//                        }
//                        if (document.getElementById("<%=DropDownListModel.ClientID%>").value == "--Select Vehicle Model--") {
//                            alert("Select correct Vehicle Model.");
//                            //document.getElementById("<%=DropDownListModel.ClientID%>").selectedIndex = 0;
//                            document.getElementById("<%=DropDownListModel.ClientID%>").focus();
//                            return false;
//                        }
            //            var VehicleMake = document.getElementById("textBoxVehicleMake").value;

            //            if (VehicleMake == "") {
            //                alert("Please Provide Vehicle Maker.");
            //                document.getElementById("textBoxVehicleMake").focus();
            //                return false;
            //            }

            //            if (invalidChar(document.getElementById("textBoxVehicleMake"))) {
            //                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
            //                document.getElementById("textBoxVehicleMake").focus();
            //                return false;
            //            }

            if (document.getElementById("DropDownListVehicleModel").selectedIndex == 0) {
                //jAlert("Please Select Correct Vehicle Model", "Order Form : Requirement");
                alert("Please Select Correct Vehicle Model");
                document.getElementById("DropDownListVehicleModel").focus();
                return false;
            }
            var VehicleRegNo = document.getElementById("TextBoxVehicleRegNo").value;

            if (VehicleRegNo == "") {
                alert("Please Provide Vehicle Registration Number.");
                //jAlert("Please Provide Vehicle Registration Number.", "Order Form : Requirement");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("TextBoxVehicleRegNo"))) {
                alert ("Please Provide Valid Registration No.");
                //jAlert("Please Provide Valid Registration No.", "Order Form : Requirement");
                document.getElementById("TextBoxVehicleRegNo").focus();
                return false;
            }

//            if (!ValidRegNo(document.getElementById("TextBoxVehicleRegNo").value, document.getElementById("<%=LabelCreatedID.ClientID%>").innerText)) {
//                alert("Please Provide Valid Registration No.");
//                document.getElementById("TextBoxVehicleRegNo").focus();
//                return false;
//            }


            var textboxEngineNo = document.getElementById("textboxEngineNo").value;

            if (textboxEngineNo == "") {
                alert("Please Provide Vehicle Engine No.");
                //jAlert("Please Provide Vehicle Engine No.", "Order Form : Requirement");
                document.getElementById("textboxEngineNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textboxEngineNo"))) {
                alert("Please Provide Valid Vehicle Engine No.");
                //jAlert("Please Provide Valid Vehicle Engine No.", "Order Form : Requirement");
                document.getElementById("textboxEngineNo").focus();
                return false;
            }

            var textBoxChassisNo = document.getElementById("textBoxChassisNo").value;
            if (textBoxChassisNo == "") {
                alert("Please Provide Vehicle Chassis No.");
                //jAlert("Please Provide Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxChassisNo").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxChassisNo"))) {
                alert("Please Provide Valid Vehicle Chassis No.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxChassisNo").focus();
                return false;
            }

            if (document.getElementById("DropDownListOrderType").selectedIndex == 0) {

                alert("Please Select Correct order Type.");
                ///jAlert("Please Select Correct order Type.", "Order Form : Requirement");
                document.getElementById("DropDownListOrderType").focus();
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
     <style type="text/css">
      
        .ModalPopupBG
        {
            background-color: #333333;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        .modalPopup
        {
           
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 250px;
        }
         .style1
         {
             width: 1284px;
         }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ScriptMode="Release">
    </asp:ScriptManager>--%>
     <script type="text/javascript" language="javascript">
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
         function EndRequestHandler(sender, args) {
             if (args.get_error() != undefined) {
                 args.set_errorHandled(true);
             }
         }
</script>
    <asp:HiddenField ID="HiddenField2" runat="server" Value="2" />
        <asp:toolkitscriptmanager ID="ToolkitScriptManager1" runat="server">
        </asp:toolkitscriptmanager>
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
    <table width="100%" border="0" align="left">
        <tr>
            <td class="style1">
                <div style="margin: 20px;" align="left">
                    <fieldset>
                        <legend>
                            <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                                <span>Add HSRP Record</span>
                            </div>
                        </legend>
                        <div>
                            <table style="background-color: #FFFFFF" border="0" align="left" width="100%" cellpadding="3"
                                cellspacing="1">
                                <tr>
                                <td nowrap="nowrap">
                                    <asp:Label  ID="LabelUSer1" Font-Size="small" Font-Italic="true" ForeColor="Blue" runat="server" />
                                    &nbsp;&nbsp;<asp:CheckBox Font-Size="Medium" ForeColor="Red" Text="Checked If Vehicle has VIP No. " TextAlign="Left" runat="server" />
                                  <asp:UpdatePanel runat="server" ID="UpdateRTOLocation11" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Select RTO Location for which you want to add Vehicle Reg. No:" ForeColor="Black" runat="server" ID="labelRTOLocation" />&nbsp;&nbsp;
                                                <asp:DropDownList  ID="dropDownListRTOLocation" Width="220px"
                                                    CausesValidation="false" runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                               
                                            </ContentTemplate>
                                            
                                        </asp:UpdatePanel>
                                </td>
                                
                                </tr>
                                <tr>
                                    <td>
                                        <table style="background-color: #FFFFFF" width="100%" border="0" align="left" cellpadding="3"
                                            cellspacing="1">
                                            <tr>
                                                <td colspan="6">
                                                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                                        <tr valign="top">
                                                            <td colspan="1">
                                                                <asp:Label Text="Allowed RTO's :- " ForeColor="OrangeRed" Visible="false" Font-Size="Medium" runat="server" />
                                                                <asp:Label Font-Size="medium" ID="LabelCreatedID" Visible="false" ForeColor="OrangeRed" runat="server" />
                                                            </td>
                                                            <td colspan="5" class="form_text">
                                                            
                                                             
                                                            
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                        <td colspan="6">
                                                        <asp:Label Font-Size="medium" ID="lblMessageError"  ForeColor="OrangeRed" runat="server" />
                                                        </td>
                                                        </tr>
                                                        <tr valign="top">
                                                            <td colspan="2" align="left" style="margin-left: 50px;width:258px " class="form_text">
                                                                <b>AUTHORIZATION INFO</b>
                                                            </td>
                                                            <td class="form_text" nowrap="nowrap" align="left" width="150px">
                                                                Record Created By:
                                                            </td>
                                                            <td width="170px">
                                                                <asp:Label ID="LabelUSER" ForeColor="Blue" runat="server" />
                                                            </td>
                                                            <td class="form_text" nowrap="nowrap" align="left" width="140px">
                                                                Record Date:
                                                            </td>
                                                            <td> 
                                                                <asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" nowrap="nowrap" align="left" style="width: 12.8%">
                                                    HSRP Auth. No :<span class="alert">* </span>
                                                </td>
                                                <td nowrap="nowrap" style="width: 170px" align="left">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelAuthNo" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox class="form_textbox11" onblur="this.value=removeSpaces(this.value);"
                                                                            Style="text-transform: uppercase" TabIndex="1" Width="160" ID="textBoxAuthorizationNo"
                                                                            runat="server"></asp:TextBox>
                                                                        <%--<div class="demo">
                                                                            <div class="ui-widget">
                                                                                <asp:TextBox ID="tbAuto" class="tb" runat="server">
                                                                                </asp:TextBox>
                                                                            </div>
                                                                        </div>--%>
                                                                        <div id="divwidth">
                                                                        </div>
                                                                        <asp:AutoCompleteExtender UseContextKey="true" ServiceMethod="GetCustomers1" MinimumPrefixLength="1"
                                                                            ServicePath="~/WCFService/ServiceForSuggestion.svc" CompletionInterval="10" EnableCaching="false"
                                                                            TargetControlID="textBoxAuthorizationNo" ID="AutoCompleteExtender1" runat="server"
                                                                            FirstRowSelected="false" CompletionSetCount="12" CompletionListCssClass="AutoExtender"
                                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                            CompletionListElementID="divwidth">
                                                                        </asp:AutoCompleteExtender>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                         
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
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

                                                                       <%-- <asp:LinkButton ID="buttonGo" runat="server" CausesValidation="false" OnClientClick="javascript:return CheckAuthNo();"
                                                                            Text="Go" class="button" TabIndex="2" OnClick="buttonGo_Click"></asp:LinkButton>--%>
                                                                   
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%--<asp:Button ID="buttonGo" runat="server" OnClientClick="javascript:return CheckAuthNo();"
                                                        Text="Go" class="button" TabIndex="2" OnClick="buttonGo_Click" />--%>
                                                </td>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    HSRP Auth. Date :<span class="alert">* </span>
                                                </td>
                                                <td align="left">
                                                    <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                                                        cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" onmouseup="OrderDate_OnMouseUp()">
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelOrderDate" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <ComponentArt:Calendar TabIndex="3" ID="OrderDate" runat="server" PickerFormat="Custom"
                                                                            PickerCustomFormat="dd/MM/yyyy" ControlType="Picker" PickerCssClass="picker">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                         
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
                                                <td class="form_text" align="left">
                                                    Mobile No :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelMobile" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxMobileNo" onblur="this.value=remove(this.value);" runat="server" Style="text-transform: uppercase"
                                                                MaxLength="10" TabIndex="4" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left" nowrap="nowrap">
                                                    Customer Name :<span class="alert">* </span>
                                                </td>
                                                <td align="left" width="170px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelCustomerName" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxCustomerName" Style="text-transform: uppercase" runat="server"
                                                                Width="160px" class="form_textbox11" TabIndex="5"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap">
                                                    Order Status :
                                                </td>
                                                <td align="left" style="margin-left: 8px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelOrderStatus" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxOrderStatus" class="form_textbox11" Width="126px" TabIndex="6"
                                                                Enabled="false" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left">
                                                    Landline :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelLandline" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxLandline" onblur="this.value=remove(this.value);" Style="text-transform: uppercase" MaxLength="12"
                                                                class="form_textbox11" runat="server" TabIndex="7"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left">
                                                    Address :<span class="alert">* </span>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelAddress1" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxAddress1" Style="text-transform: uppercase" Width="480px"
                                                                Columns="400" Rows="2" class="form_textbox11" runat="server" TabIndex="8"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                  <td class="form_text" nowrap="nowrap" align="left">
                                                    Cash Receipt Date :<span class="alert">* </span><span class="alert">* </span>
                                                </td>
                                                 <td runat="server" align="left">
                                                    <table id="Table2" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                                                        cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" onmouseup="CashDate_OnMouseUp()">
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <%--<ComponentArt:Calendar TabIndex="3" ID="CashDate" runat="server" PickerFormat="Custom"
                                                                            PickerCustomFormat="dd/MM/yyyy" ControlType="Picker" PickerCssClass="picker">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="CashDate_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>--%>

                                                                        <asp:TextBox ID="CashDate" runat="server"></asp:TextBox>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                         
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <%--<img id="Img1" tabindex="3" alt="" onclick="CashDate_OnClick()"
                                                                    onmouseup="CashDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left" width="141px">
                                                    Email Id :
                                                </td>
                                                <td align="left" width="180px" colspan="3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelEmail" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxEmailId" Width="480px" onblur="this.value=remove(this.value);" class="form_textbox11" runat="server"
                                                                TabIndex="9"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    Cash Receipt No :
                                                </td>
                                                <td nowrap="nowrap" style="width: 170px" align="left">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelCashReceiptNo" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="TextBoxCashReceiptNo" Style="text-transform: uppercase" onblur="this.value=removeSpaces(this.value);"
                                                                            MaxLength="10" class="form_textbox11" runat="server" TabIndex="10"></asp:TextBox>
                                                                        <div id="divwidth3">
                                                                        </div>
                                                                        <asp:AutoCompleteExtender ServiceMethod="GetCashReceiptNos" MinimumPrefixLength="1"
                                                                            ServicePath="~/WCFService/ServiceForSuggestion.svc" CompletionInterval="10" EnableCaching="false"
                                                                            TargetControlID="TextBoxCashReceiptNo" ID="AutoCompleteExtender3" runat="server"
                                                                            FirstRowSelected="false" CompletionSetCount="12" CompletionListCssClass="AutoExtender"
                                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                            CompletionListElementID="divwidth3">
                                                                        </asp:AutoCompleteExtender>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                          
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                         
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:LinkButton Enabled="false" ID="LinkButtonGo3" runat="server" Visible="false" CausesValidation="false"
                                                                            OnClientClick="javascript:return CheckCashReceiptNo();" Text="Go" class="button"
                                                                            TabIndex="11" OnClick="LinkButtonGo3_Click"></asp:LinkButton>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                            <tr valign="top">
                                                <td colspan="6" style="margin-left: 50px;" align="left" class="form_text">
                                                    <b>VEHICLE INFO</b>
                                                    <asp:TextBox Visible="false" Width="1px" ID="textBoxAddress2" runat="server" CssClass="form_textbox11"
                                                        TabIndex="9"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left" style="width: 12.8%">
                                                    Vehicle Class :<span class="alert">* </span>
                                                </td>
                                                <td align="left" width="200px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleClass" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" AutoPostBack="true"
                                                                Width="170px" ID="DropDownListVehicleClass" runat="server" TabIndex="12" OnSelectedIndexChanged="DropDownListVehicleClass_SelectedIndexChanged">
                                                                <asp:ListItem Value="--Select Vehicle Class--" Text="--Select Vehicle Class--"></asp:ListItem>
                                                                <asp:ListItem Value="Transport" Text="Transport"></asp:ListItem>
                                                                <asp:ListItem Value="Non-Transport" Text="Non-Transport"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap" width="120px">
                                                    Vehicle Type :<span class="alert">* </span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleModel" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" Font-Size="Small"
                                                                CausesValidation="false" ID="DropDownListVehicleModel" Width="170px" runat="server"
                                                                TabIndex="13" OnSelectedIndexChanged="DropDownListVehicleModel_SelectedIndexChanged">
                                                                <asp:ListItem Value="--Select Vehicle Model--" Text="--Select Vehicle Model--"></asp:ListItem>
                                                            <%--<asp:ListItem Value="--Select Vehicle Model--" Text="--Select Vehicle Type--"></asp:ListItem>--%>
                                                               
                                                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>
                                                                 <asp:ListItem Value="E-RICKSHAW" Text="E-RICKSHAW"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left" nowrap="nowrap" width="141px">
                                                    Vehicle Maker :</td>
                                                <td align="left" style="width: 180px">
                                                    <%-- <asp:DropDownList runat="server" Style="margin-left: 8px" DataTextField="VehicleMakerDescription"
                                                        DataValueField="VehicleMakerID" TabIndex="15" EnableViewState="true" Font-Size="Small"
                                                        AutoPostBack="true" ID="DropDownList44" 
                                                        onselectedindexchanged="DropDownList44_SelectedIndexChanged"></asp:DropDownList>--%>                                                    <%-- <asp:DropDownList runat="server" Style="margin-left: 8px" DataTextField="VehicleMakerDescription"
                                                        DataValueField="VehicleMakerID" TabIndex="15" EnableViewState="true" Font-Size="Small"
                                                        AutoPostBack="true" ID="DropDownListVehicleMaker" 
                                                        onselectedindexchanged="DropDownList44_SelectedIndexChanged"></asp:DropDownList>--%>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleMake" UpdateMode="Conditional"
                                                        ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <asp:DropDownList Style="margin-left: 8px" EnableViewState="true" Font-Size="Small"
                                                                AutoPostBack="true" ID="DropDownListVehicleMaker1" Width="170px" DataTextField="VehicleMakerDescription"
                                                                DataValueField="VehicleMakerID" runat="server" TabIndex="14" OnSelectedIndexChanged="DropDownList44_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap" width="141px">
                                                    Vehicle Model :</td>
                                                <td colspan="3" align="left" style="width: 180px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleModel1" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%-- <asp:TextBox ID="textBoxVehicleMake" class="form_textbox11" runat="server" TabIndex="12"></asp:TextBox>--%>
                                                            <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" CausesValidation="false"
                                                                ID="DropDownListModel" Width="170px" DataTextField="VehicleModelDescription"
                                                                DataValueField="VehicleModelID" runat="server" TabIndex="15">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    Vehicle Reg No :<span class="alert">* </span>
                                                </td>
                                                <td nowrap="nowrap" style="width: 170px" align="left">
                                                    <table>
                                                        <tr>
                                                            <td nowrap="nowrap">
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanelVehicleRegNo" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="TextBoxVehicleRegNo" onblur="this.value=removeSpaces(this.value);"
                                                                            Style="text-transform: uppercase" MaxLength="12" class="form_textbox11" runat="server"
                                                                            TabIndex="16"></asp:TextBox>
                                                                        <div id="divwidth2">
                                                                        </div>
                                                                        <asp:AutoCompleteExtender ServiceMethod="GetVehicleRegs" UseContextKey="true" MinimumPrefixLength="1"
                                                                            ServicePath="~/WCFService/ServiceForSuggestion.svc" CompletionInterval="10" EnableCaching="false"
                                                                            TargetControlID="TextBoxVehicleRegNo" ID="AutoCompleteExtender2" runat="server"
                                                                            FirstRowSelected="false" CompletionSetCount="12" CompletionListCssClass="AutoExtender"
                                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                            CompletionListElementID="divwidth2">
                                                                        </asp:AutoCompleteExtender>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                          
                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                                         
                                                                        <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:LinkButton ID="LinkButtonGo2" runat="server" CausesValidation="false" OnClientClick="javascript:return CheckVehicleRegNo();"
                                                                            Text="Go" class="button" TabIndex="17" OnClick="LinkButtonGo2_Click"></asp:LinkButton>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="form_text" align="left">
                                                    Engine No :<span class="alert">* </span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelEngineNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textboxEngineNo" onblur="this.value=removeSpaces(this.value);" Style="text-transform: uppercase" TabIndex="18"
                                                                Enabled="true" class="form_textbox11" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left">
                                                    Chassis No :<span class="alert">* </span>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelChassisNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxChassisNo" TabIndex="19" onblur="this.value=removeSpaces(this.value);" Style="text-transform: uppercase"
                                                                Enabled="true" class="form_textbox11" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
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
                                                    Order Type :<span class="alert">* </span>
                                                </td>
                                                <td align="left" width="193px">
                                                    <%--<asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" CausesValidation="false"
                                                                ID="DropDownList1" runat="server" TabIndex="18" 
                                                        Width="170px" onselectedindexchanged="DropDownList1_SelectedIndexChanged1" 
                                                                 >
                                                                <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelOrderType" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <%--  <asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" CausesValidation="false"
                                                                ID="DropDownListOrderType" runat="server" TabIndex="18" Width="170px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                                                <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                            <asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" CausesValidation="false"
                                                                ID="DropDownListOrderType" runat="server" TabIndex="20" Width="170px" OnSelectedIndexChanged="DropDownListOrderType_SelectedIndexChanged">
                                                                <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <%-- <asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" CausesValidation="false"
                                                                ID="DropDownList1" runat="server" TabIndex="18" 
                                                        Width="170px" onselectedindexchanged="DropDownList1_SelectedIndexChanged" >
                                                                <asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>
                                                            </asp:DropDownList>--%>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:UpdatePanel ID="UpdatePanelDropDownListFrontPlate" UpdateMode="Conditional"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <%-- <asp:DropDownList Style="margin-left: 3px" CausesValidation="false" AutoPostBack="true"
                                                                ID="DropDownListFrontPlateSize" Width="170px" DataTextField="ProductCode" DataValueField="ProductID"
                                                                runat="server" TabIndex="16" OnSelectedIndexChanged="DropDownListFrontPlateSize_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>
                                                            <asp:Label ID="labelFrontPlateSize" Font-Size="Medium" Visible="false" ForeColor="Black"
                                                                runat="server"></asp:Label>
                                                            <asp:DropDownList Style="margin-left: 3px" CausesValidation="false" ID="DropDownListFrontPlateSize"
                                                                Width="170px" DataTextField="ProductCode" DataValueField="ProductID" runat="server"
                                                                TabIndex="21">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="form_text" align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelSnap" ChildrenAsTriggers="true" UpdateMode="Conditional"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox Text="Snap Lock" Checked="true" Enabled="false" class="form_text" TextAlign="Right"
                                                                ID="checkBoxSnapLock" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanelThirdSticker" ChildrenAsTriggers="true" UpdateMode="Conditional"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:CheckBox Text="Third Sticker" Enabled="false" class="form_text" TextAlign="Right"
                                                                ID="checkBoxThirdSticker" runat="server" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:UpdatePanel ID="UpdatePanelDropDownListRearPlate" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                            <%--   <asp:DropDownList Style="margin-left: 3px" AutoPostBack="true" DataTextField="ProductCode"
                                                                DataValueField="ProductID" ID="DropDownListRearPlateSize" Width="170px" CausesValidation="false"
                                                                runat="server" TabIndex="17" OnSelectedIndexChanged="DropDownListRearPlateSize_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>
                                                            <asp:Label ID="labelRearPlateSize" Font-Size="Medium" Visible="false" ForeColor="Black"
                                                                runat="server"></asp:Label>
                                                            <asp:DropDownList Style="margin-left: 3px" DataTextField="ProductCode" DataValueField="ProductID"
                                                                ID="DropDownListRearPlateSize" Width="170px" CausesValidation="false" runat="server"
                                                                TabIndex="22">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                              
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
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
                                                <td colspan="10" align="left" class="form_text" style="margin-left: 50px">
                                                    <b>FINANCIAL INFO</b>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td class="form_text" style="margin-left: 50px">
                                                    Remarks:
                                                </td>
                                                <td colspan="9" align="left" ">
                                                <asp:UpdatePanel ID="UpdatePanelRemarks" ChildrenAsTriggers="true" UpdateMode="Conditional"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox  ID="textBoxRemarks" runat="server" onblur="this.value=remove(this.value);"
                                                            Height="22px" Width="608px"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                           
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            
                                            <%--  <tr>
                                                <td class="form_text" align="left" width="11%">
                                                    Invoice No :
                                                </td>
                                                <td align="left" width="196px">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelInvoiceNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox class="form_textbox11" ID="textBoxInvoiceNo" Enabled="false" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap">
                                                    Cash Receipt No :
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:UpdatePanel runat="server" ID="UpdatePanelCashReceiptNo" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxCashReceiptNo" class="form_textbox11" Enabled="false" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                             
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td class="form_text" align="left">
                                                    Total Amount :
                                                </td>
                                                <td align="left" width="193px">
                                                    <asp:UpdatePanel ID="UpdatePanelTotalAmount" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxTotalAmount" Width="50px" Enabled="false" class="form_textbox11"
                                                                runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                              
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" nowrap="nowrap" align="left">
                                                    VAT (%) :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelVat" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxVat" runat="server" Width="50px" Enabled="false" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                              
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
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
                                                            <asp:TextBox ID="textBoxVatAmount" runat="server" Width="50px" Enabled="false" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                              
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td class="form_text" align="left" nowrap="nowrap">
                                                    Net Total :
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelNetTotal" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxNetTotal" Width="50px" runat="server" Enabled="false" class="form_textbox11"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                              
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleClass" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListVehicleModel" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr style="border:0px solid">
                                                <td class="alert" nowrap="nowrap" align="left">
                                                    <%--Service Tax Amount :--%>* Fields are mandatory.
                                                    
                                                </td>
                                                <td colspan="4" align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelMessage" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                                            <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" /> 
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td nowrap="nowrap" align="right" colspan="" style="margin-right: 10px; margin-left: 100px">
                                                    <asp:Button ID="buttonUpdate" runat="server" TabIndex="23" class="button"
                                                        Text="Update" onclick="buttonUpdate_Click" Visible="False" />
                                                </td>
                                                <td align="right" nowrap="nowrap">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="24" OnClientClick="javascript:return ValidateForm();"
                                                                class="button" Text="Save" Visible="false" OnClick="buttonSave_Click" />&nbsp;
                                                            <%--<a id="LinkButtonCashReceipts" visible="false" runat="server" class="button" target="_self">
                                                                Download Cash Receipt</a> --%>
                                                             <%--<asp:LinkButton ID="LinkButtonCashReceipts" runat="server" Visible="false" Text="Download Cash Receipt" class="button" onclick="LinkButtonCashReceipts_Click"></asp:LinkButton>nbsp;--%>
                                                           
                                                            <asp:Button Text="Reset" ID="Button1" class="button" runat="server" TabIndex="25"
                                                                OnClientClick="this.form.reset();ClearDate();" OnClick="Button1_Click" />

                                                                
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td  >
                                                    <input type="button" onclick="javascript:parent.googlewin.close();" tabindex="26"
                                                        name="buttonClose" id="buttonClose" value="Close" class="button" />&nbsp;
                                                </td>
                                                <td >
                                                  <asp:LinkButton ID="LknBtnCashReceipts" runat="server" Visible="false"  
                                                                Text="Download Cash Receipt" class="button" Width="139px"  onclick="LknBtnCashReceipts_Click"></asp:LinkButton>
                                                                
                                                                &nbsp;
                                                                
                                                                <asp:LinkButton ID="LinkButton1Epson" runat="server"  Visible="false"
                                                                Text="Epson Cash Receipt" class="button" Width="139px" 
                                                        onclick="LinkButton1Epson_Click"  ></asp:LinkButton>
                                                                 </td>
                                                
                                            </tr>
                                            <tr>
                                              
                                                <td class="alert" nowrap="nowrap" align="left">
                                                   
                                        
                                                    <asp:UpdatePanel ID="UpdatePanelServiceTaxAmount" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                         ** Field are mandatory when<br /> Cash Receipt Number Field is Filled.
                                                            <asp:TextBox ID="textboxServiceTaxAmount" Width="1px" Visible="false" Enabled="false"
                                                                class="form_textbox11" runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UpdatePanelServiceTax" UpdateMode="Conditional" ChildrenAsTriggers="true"
                                                        runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="textBoxServiceTax" Width="1px" Visible="false" Enabled="false" class="form_textbox11"
                                                                runat="server"></asp:TextBox>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListOrderType" EventName="SelectedIndexChanged" />
                                                            
                                                             
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo2" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="LinkButtonGo3" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td colspan="6">
                                                    <asp:TextBox ID="HiddenFieldFrontPlate" Width="10px" Visible="false" Text="0" runat="server" />
                                                    <asp:TextBox ID="HiddenFieldRearPlate" Width="10px" runat="server" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldSticker" runat="server" Width="10px" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldScrew" runat="server" Width="10px" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldFixing" runat="server" Width="10px" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldFrontPlateCode" runat="server" Width="10px" Text="0"
                                                        Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldRearPlateCode" runat="server" Width="10px" Text="0" Visible="false" />
                                                    <asp:TextBox ID="HiddenFieldCashReceipt" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="HiddenFieldHSRPRecords" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
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
                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                              <td colspan="8">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarCashDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="Img1" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="CashDate_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
                                           <tr>
                                           <td>
                                            <asp:Button ID="btnHidden" runat="server" Text="Button" Enabled="false" CssClass="btnhide" />
                                            <cc1:modalpopupextender ID="mpeSave" runat="server" 
                                                    TargetControlID="btnHidden" PopupControlID="PnlModal" 
                                                    BackgroundCssClass="ModalPopupBG">
                                                </cc1:modalpopupextender>
                                                <asp:Button ID="Button2" runat="server" Text="Button" style="visibility:hidden"/>
                                                <asp:Panel ID="PnlModal" runat="server" Width="300px" CssClass="modalPopup" 
                                                   Font-Names="Arial" Font-Size="Small" ForeColor="Black" Height="300px" HorizontalAlign="Center">
                                                   <br /><br />
                                                        <asp:Label ID="Label1" runat="server" Text="Vehicle already in database" 
                                                        Font-Bold="True" Font-Underline="True"></asp:Label>  <br /><br />
                                                   <asp:Label ID="lblLocation" runat="server" Text=""></asp:Label>  <br /><br />
                                                   <asp:Label ID="lblVehId" runat="server" Text=""></asp:Label>   <br /><br />
                                                   <asp:Label ID="lblVehType" runat="server" Text=""></asp:Label>   <br /><br />
                                                   <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>  <br /><br />
                                                      <asp:Label ID="lblQues" runat="server" Text="Do you want to save?" 
                                                        Font-Bold="True" Font-Underline="True"></asp:Label>  <br />
                                                    <br />
                                                   <asp:Button ID="btnYes" runat="server" Text="Yes" onclick="btnYes_Click" /> 
                                                   <asp:Button 
                                                        ID="btnNo" runat="server" Text="No" onclick="btnNo_Click" />  
                                                      
                                                </asp:Panel>   
                                           </td>
                                           </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </div>
            </td>
            </tr>
            </table>
    </form>
</body>
</html>
