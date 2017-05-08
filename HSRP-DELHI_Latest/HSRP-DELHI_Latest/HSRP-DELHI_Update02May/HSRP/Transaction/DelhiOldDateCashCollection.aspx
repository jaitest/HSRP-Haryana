<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DelhiOldDateCashCollection.aspx.cs" Inherits="HSRP.Transaction.DelhiOldDateCashCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../javascript/common.js"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     
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

        function validateVehicleRegistrationNo() {


            if (document.getElementById("<%=txtVehicleRegNo.ClientID%>").value == "") {
                alert("Provide Vehicle Registration No.");
                document.getElementById("<%=txtVehicleRegNo.ClientID%>").focus();
                return false;
            }
        }

        function validateVehicleRegNo() {


            if (document.getElementById("<%=txtEngineRegNo.ClientID%>").value == "") {
                alert("Provide Vehicle Engine No.");
                document.getElementById("<%=txtEngineRegNo.ClientID%>").focus();
                return false;
            }
        }
        function validate() {

            if (document.getElementById("<%=txtEngineRegNo.ClientID%>").value == "") {
                alert("Provide Vehicle Engine No.");
                document.getElementById("<%=txtEngineRegNo.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=txtEngineRegNo.ClientID%>"))) {
                //        
                alert("Special Character Not Allowed!!");
                document.getElementById("<%=txtEngineRegNo.ClientID%>").focus();
                return false;
            }
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



            if (document.getElementById("<%=txtCounterNo.ClientID%>").value == "") {
                alert("Provide Counter No");
                document.getElementById("<%=txtCounterNo.ClientID%>").focus();
                return false;
            }

           

            if (document.getElementById("<%=txtAddress.ClientID%>").value == "") {
                alert("Provide Address");
                document.getElementById("<%=txtAddress.ClientID%>").focus();
                return false;
            }

           //var len = document.getElementById("<%=txtCounterNo.ClientID%>").value.length;
            //            if (len <= "2") {
            //                alert("Provide Currect Counter No");
            //                document.getElementById("<%=txtCounterNo.ClientID%>").focus();
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
      
    <div style="width: 100%;"> 
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Cash Receipt Delhi Data
                </div>
            </legend>
            <br />
            <br />
            <%--<asp:ListItem Value="0" Text="--Select Vehicle Model--"></asp:ListItem>
                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>--%>
            <div style="width: 100%; height:443px; margin: 0px auto 0px auto">
                <table border="0" align="right" style="height: 348px; width: 85%;">
                <tr> 
                <td colspan="4"  >
                  <center>  <asp:Label ID="lblMessageHeading"  class="alert" Font-Bold="true"   runat="server"> <font size="6"> OLD VEHICLE </font></asp:Label></td></center>
                </tr>
                <tr>
                <td style="width: 128px"><asp:Label ID="lblMessageCashReceipt" Visible="false" runat="server"></asp:Label></td>
                <td style="width: 347px">
                    <asp:Label ID="lblCashReceiptNo"  Visible="false" runat="server"></asp:Label></td>
                </tr>
                    <tr>
                        <td class="form_text" style="width: 128px">
                            Engine No : <span class="alert">* </span>
                        </td>
                        <td >
                            <%--<asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>--%><%--<asp:ListItem Value="0" Text="--Select Vehicle Model--"></asp:ListItem>
                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>--%><%--<asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>--%><%--<asp:ListItem Value="0" Text="--Select Vehicle Model--"></asp:ListItem>
                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>--%>
                                                            <asp:TextBox Width="160" class="form_textbox11" onblur="this.value=removeSpaces(this.value);"
                                                                            Style="text-transform: uppercase" TabIndex="1"  ID="txtEngineRegNo"  
                                                                            runat="server"></asp:TextBox> &nbsp; &nbsp; 
                                                                            
                                                                            <asp:LinkButton ID="buttonGo" runat="server" CausesValidation="false" OnClientClick="javascript:return validateVehicleRegNo();"
                                                                            Text="Go" class="button" TabIndex="2" onclick="buttonGo_Click" ></asp:LinkButton>
                            <%--<asp:ListItem Text="--Select Order Type--" Value="--Select Order Type--"></asp:ListItem>
                                                                <asp:ListItem Text="NEW BOTH PLATES" Value="NB"></asp:ListItem>
                                                                <asp:ListItem Text="OLD BOTH PLATES" Value="OB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED BOTH PLATES" Value="DB"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED FRONT PLATE" Value="DF"></asp:ListItem>
                                                                <asp:ListItem Text="DAMAGED REAR PLATE" Value="DR"></asp:ListItem>
                                                                <asp:ListItem Text="ONLY STICKER" Value="OS"></asp:ListItem>--%>                                                        <%--<asp:ListItem Value="0" Text="--Select Vehicle Model--"></asp:ListItem>
                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>--%><%-- <asp:ListItem Value="0" Text="--Select Vehicle Class--"></asp:ListItem>
                                <asp:ListItem Value="Transport" Text="Transport"></asp:ListItem>
                                <asp:ListItem Value="Non-Transport" Text="Non-Transport"></asp:ListItem>--%>
                        </td>
                    
                        <td class="form_text">
                            Vehicle Reg. No <span class="alert">* </span>
                        </td>
                        <td>
                            <%--<asp:ListItem Value="0" Text="--Select Vehicle Model--"></asp:ListItem>
                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem>
                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem>
                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>--%>&nbsp;
                            
                            <asp:TextBox Width="160" class="form_textbox11" onblur="this.value=removeSpaces(this.value);"
                                                                            Style="text-transform: uppercase" TabIndex="1"  ID="txtVehicleRegNo"  
                                                                            runat="server"></asp:TextBox> &nbsp;&nbsp;
                                                                            
                                                                        <asp:LinkButton ID="btnGOVehicleRegNo" 
                                runat="server" CausesValidation="false" OnClientClick="javascript:return validateVehicleRegistrationNo();"
                                                                            Text="Go" class="button" 
                                TabIndex="2" onclick="LinkButton1_Click" ></asp:LinkButton>
                                                                
                                                                         
                        </td>
                    </tr>
                    <tr id="lblOrder" runat="server">
                        <td class="form_text" style="width: 128px">
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                    
                        <td class="form_text">
                            Order Status:</td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblOrderStatus"  runat="server" Text="label1" Font-Bold="False" 
                                Font-Size="Medium" ForeColor="Black"></asp:Label>
                                                                
                                                                         
                        </td>
                    </tr>
                    <tr id="lblRecordType" runat="server">
                    <td class="form_text" style="width: 138px"> Record Type :</td>
                    <td class="form_text" style="width: 138px">  <asp:Label ID="lblOld" runat="server"></asp:Label> </td>
                    <td class="form_text" style="width: 138px"> Registration Date :</td>
                    <td class="form_text" style="width: 138px">&nbsp;&nbsp;&nbsp; <asp:Label ID="lblRegDate" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="form_text" style="width: 138px">
                            Authorization No : <span class="alert">* </span>
                        </td>
                        <td style="width: 347px">
                            <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                            cellpadding="0" border="0">
                            <tr>
                                <td  valign="top" onmouseup="DepositDate_OnMouseUp()">
                                    <ComponentArt:Calendar ID="DepositDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"  Visible="false" TabIndex="1"
                                        ControlType="Picker" PickerCssClass="picker" >
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="DepositDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                     <asp:TextBox ID="txtAuthorizationNo" runat="server" Width="160" class="form_text" ></asp:TextBox>
                                </td>
                                <td style="font-size: 10px;">
                                    &nbsp;
                                </td>
                                <td valign="top">
                                    <%-- <asp:ListItem Value="0" Text="--Select Vehicle Class--"></asp:ListItem>
                                <asp:ListItem Value="Transport" Text="Transport"></asp:ListItem>
                                <asp:ListItem Value="Non-Transport" Text="Non-Transport"></asp:ListItem>--%>
                                </td>
                            </tr>


                        </table>
                        </td>
                     
                        <td class="form_text">
                            Owner Name : <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOwnerName"  runat="server" Width="160" class="form_text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="div1" runat="server">
                        <td class="form_text" style="width: 128px">
                            Father Name : <span class="alert">* </span>
                        </td>

                        <td style="width: 347px">
                            <asp:TextBox ID="txtFatherName"  runat="server" Width="160" class="form_text"   MaxLength="10"></asp:TextBox>
                        </td>

                        <td class="form_text">
                            EmailID <span class="alert">  </span>
                        </td>
                        <td>
                           <asp:TextBox ID="txtEmailID"   runat="server" Width="160" class="form_text" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="divAddress" runat="server">
                     <td class="form_text" style="width: 128px">
                           Old Address : <span class="alert">* </span>
                        </td>

                        <td>
                            <asp:TextBox ID="txtOldAddress"  runat="server" Width="320" Height="36px" class="form_text"  Enabled="false"
                                MaxLength="10" TextMode="MultiLine"></asp:TextBox>
                        </td>
                        <td class="form_text" style="width: 128px">
                            New Address : <span class="alert">* </span>
                        </td>

                        <td style="width: 347px"   >
                            <asp:TextBox ID="txtAddress"  runat="server" Width="320" Height="36px" class="form_text" 
                                MaxLength="10" TextMode="MultiLine"></asp:TextBox>
                        </td>
                         
                    </tr>
                    <tr  id="div2" runat="server">
                        <td class="form_text" style="width: 128px">
                           Mobile No :
                        </td>
                        <td style="width: 347px">
                            <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event)" runat="server" Width="160" class="form_text"   MaxLength="10"></asp:TextBox>
                        </td>
                         
                        <td class="form_text">
                            Chassies No.  : <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChassisNo"  runat="server" Width="160" class="form_text"  MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" style="width: 128px">
                            Vehicle Type : <span class="alert">* </span>
                        </td>
                        <td style="width: 347px">
                            <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" Font-Size="Small"
                                CausesValidation="false" ID="DropDownListVehicleModel" Width="170px" runat="server"
                                TabIndex="13" 
                                onselectedindexchanged="DropDownListVehicleModel_SelectedIndexChanged1" >
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
                            <asp:Label ID="lblordertype"  runat="server" Text="label1" Font-Bold="False" 
                                Font-Size="Medium" ForeColor="Black"></asp:Label>
                                                                
                                                                         
                        </td>
                    </tr>
     
                    <tr>
                        <td class="form_text" style="width: 128px">
                           
                        </td>
                        <td style="width: 347px">
                           
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
      
                    <tr  id="div3" runat="server">
                        <td class="form_text" style="width: 128px">
                            Front Plate : <span class="alert">* </span>
                        </td>
                        <td style="width: 347px">
                            <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" 
                                Font-Size="Small"  DataTextField="ProductCode" DataValueField="ProductID"
                                CausesValidation="false" ID="DropDownListFrontPlate" Width="170px" 
                                runat="server" TabIndex="13"  > 
                            </asp:DropDownList>
                        </td>
                     
                        <td class="form_text">
                            Rear Plate : <span class="alert">* </span>
                        </td>
                        <td>
                              <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" Font-Size="Small" DataTextField="ProductCode" DataValueField="ProductID"
                                CausesValidation="false" ID="DropDownListRearPlate" Width="170px" runat="server"
                                TabIndex="13" > 
                            </asp:DropDownList>
                             
                        </td>
                    </tr>
      
                    <tr  id="div4" runat="server">
                        <td class="form_text" style="width: 128px">
                                                    Vehicle Model :</td>
                        <td style="width: 347px">
                            <asp:DropDownList ID="DropDownVehicleModel1" runat="server" Height="23px" 
                                Width="177px" DataTextField="VehicleModelDescription" 
                                DataValueField="VehicleModelID" 
                                onselectedindexchanged="DropDownVehicleModel1_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                     
                        <td class="form_text">
                            &nbsp;</td>
                        <td>
                              &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="form_text" style="width: 151px">
                            Third Sticker Mandatory :  
                        </td>
                        <td style="width: 347px">
                            <asp:CheckBox ID="checkBoxThirdSticker" runat="server" OnCheckedChanged="checkBoxThirdSticker_CheckedChanged" />
                        </td>
                     
                        <td class="form_text">
                            Vip :
                        </td>
                        <td>
                            <asp:CheckBox ID="chkVip" runat="server" />
                        </td>
                    </tr>
                    <tr>
                     
                        <td class="form_text"  visible="false" style="width: 128px">
                           <div id="divlabelCounterNo" class="form_text"  runat="server" > Counter No : <span class="alert">*</span> </div>
                        </td>
                        <td style="width: 347px">
                        <div id="DivCounterNo" class="form_text"   runat="server" > 
                        <asp:TextBox ID="txtCounterNo" runat="server"  Width="160" visible="false" class="form_textbox11"></asp:TextBox> 
                        </div>
                            <asp:TextBox ID="txtTax" runat="server" Visible="false" Width="160" class="form_textbox11"></asp:TextBox>
                        </td>
                   
                        <td class="form_text">
                            Amount : <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" Enabled="false" Width="160" class="form_text" ></asp:TextBox>
                        </td>
                    </tr>
                   
                         
                    <tr>
                     
                        <td class="form_text"  visible="false" style="width: 128px">
                            Reference</td>
                        <td style="width: 347px">
                            <asp:TextBox ID="txtReference" runat="server" Height="24px" Width="167px"></asp:TextBox>
                        </td>
                   
                        <td class="form_text">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                   
                         
                </table>
            </div>
            
      <div style="border-style: solid; border-color: inherit; border-width: 0px; width:947px; height:40px; float:right">

      <div style="margin-left: 0px">
        <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label> &nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;
      <asp:Button ID="buttonSave" runat="server" TabIndex="9" class="button" Text="Save"
                                            OnClientClick=" return validate()" OnClick="ButtonSubmit_Click" />&nbsp;&nbsp;&nbsp;&nbsp;

                                        <a href="DelhiOldDateCashCollection.aspx" class="button" >Reset</a>&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnDownloadReceipt" runat="server" class="button" Text="Download Receipt" 
                                            TabIndex="12" onclick="btnDownloadReceipt_Click" />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnSticker" runat="server"  Visible="false"
              class="button" Text="White Sticker"  TabIndex="12" onclick="btnSticker_Click"   />&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnYellowSticker" runat="server" Visible="false"
              class="button" Text="White Sticker"  TabIndex="12" 
              onclick="btnYellowSticker_Click"  />&nbsp;&nbsp;
                                        <asp:Button ID="btnDuplicate" runat="server" TabIndex="9" class="button" Text="Duplicate" Visible="false"
                                            OnClientClick=" return validate()" OnClick="btnDuplicate_Click" />&nbsp;&nbsp;
                                            
                                            </div>
      
      </div>
            <div style="clear: both">
            
                        
            </div>
        </fieldset>

          <table>
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
                <asp:HiddenField ID="hiddenfieldReferenceValue" runat="server" />
                <asp:HiddenField ID="hiddenfieldhsrprecordStaggingID" runat="server" />
                <asp:HiddenField ID="hiddenfieldAuthorizationNo" runat="server" />
                <asp:HiddenField ID="HiddenFieldFrontPlatePrice" runat="server" />
                <asp:HiddenField ID="HiddenFieldRearPlatePrice" runat="server" />
                <asp:HiddenField ID="HiddenFieldStickerID" runat="server" />
                <asp:HiddenField ID="HiddenFieldscrewrate" runat="server" />
                <asp:HiddenField ID="hiddenfieldDiscount" runat="server" />
                <asp:HiddenField ID="hiddenfieldTax" runat="server" />
                <asp:HiddenField ID="hiddenfieldTotalAmount" runat="server" />
                 <asp:HiddenField ID="hiddenfieldNetAmount" runat="server" />
                 <asp:HiddenField ID="hiddenfieldVATAMOUNT" runat="server" />
 
           </table>
           <style type="text/css">
            .ModalPopupBG
            {
                background-color: #333333;
                filter: alpha(opacity=80);
                opacity: 0.8;
            }
            
            .btnhide
            {
                display: none;
            }
            
            .HellowWorldPopup
            {
                width: 1000px;
                -webkit-border-radius: 38px 39px 39px 38px;
                -moz-border-radius: 38px 39px 39px 38px;
                border-radius: 38px 39px 39px 38px;
                border: 4px solid #FF3617;
                background-color: #FAFFF0;
                -webkit-box-shadow: #B3B3B3 9px 9px 9px;
                -moz-box-shadow: #B3B3B3 9px 9px 9px;
                box-shadow: #B3B3B3 9px 9px 9px;
            }
        </style>
        <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button2_Click" Enabled="false" 
            CssClass="btnhide" />
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" CancelControlID="Button1"
            OkControlID="Button1" TargetControlID="Button2" PopupControlID="Panel1" PopupDragHandleControlID="PopupHeader"
            Drag="true" BackgroundCssClass="ModalPopupBG">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1"  runat="server">
            <div class="HellowWorldPopup">
                <div class="PopupHeader" id="PopupHeader">
                </div>
                <div class="PopupBody"> 
                    <p>
                        <div style="margin-left: 10px; font-size: 18px; color: Green">
                            Old Vehicle Entry
                        </div>
                       
                        &nbsp;<asp:Panel ID="Panel3" runat="server">
                            <div align="center" style="width: 100%;">
                                <asp:Label ID="lblMesageSave" runat="server" Text="" ForeColor="Blue" Font-Size="20px"
                                    Font-Bold="true"></asp:Label><br />
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
                                            <input id="Button1" type="button" value="Cancel" class="button" />
                                        </td>
                                    </tr>
                                </table>
                               </div>
                                </asp:Panel>
                            </div>
      
     <%-- <asp:ListItem Value="0" Text="--Select Vehicle Class--"></asp:ListItem>
                                <asp:ListItem Value="Transport" Text="Transport"></asp:ListItem>
                                <asp:ListItem Value="Non-Transport" Text="Non-Transport"></asp:ListItem>--%>
    </div>
    </asp:Panel>
</asp:Content>
