<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="UkQuickCapture.aspx.cs" Inherits="HSRP.Transaction.UkQuickCapture" %>
    <%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


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
        function validate() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_dropDownListOrg").value == "--Select State--") {
                alert("Select State");
                document.getElementById("ctl00_ContentPlaceHolder1_dropDownListOrg").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_dropDownListClient").value == "--Select RTO--") {
                alert("Select RTO");
                document.getElementById("ctl00_ContentPlaceHolder1_dropDownListClient").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_txtRequestName").value == "") {
                alert("Please Fill Request Name");
                document.getElementById("ctl00_ContentPlaceHolder1_txtRequestName").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_DDLReqType").value == "--Select Request Type--") {
                alert("Please Select Request Type");
                document.getElementById("ctl00_ContentPlaceHolder1_DDLReqType").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_DDLPriority").value == "--Select Priority--") {
                alert("Please Select Priority");
                document.getElementById("ctl00_ContentPlaceHolder1_DDLPriority").focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
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
    <style type="text/css">
        .modalPopup
        {
            background-color: #FFFFFF;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }
        .style6
        {
            width: 53%;
        }
        .style8
        {
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            color: Black;
            text-decoration: none;
            nowrap: nowrap;
            width: 341px;
        }
        .style10
        {
            width: 341px;
        }
        .style15
        {
            width: 150px;
        }
    </style>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="../images/ajax-loader.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                    UK Quick Capture
                </div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td colspan="5">
                        <table style="background-color: #FFFFFF" width="60%" border="0" align="center" cellpadding="3"
                            cellspacing="1">
                              
                           
                            <tr>
                                <td colspan="3">
                                    <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                        <tr valign="top">
                                            <td colspan="1" align="left" style="margin-left: 50px" width="150px" class="form_text">
                                                <%--<b>AUTHORZIATION INFORMATION</b>--%>
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="center" width="250px">
                                              Status:
                                                
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="250px">
                                                <%--<asp:Label ID="LabelCreatedID" ForeColor="Blue" runat="server" />--%>
                                                  
                <asp:RadioButtonList ID="RadioButtonList1" runat="server"  AutoPostBack="true"
                                                    onselectedindexchanged="RadioButtonList1_SelectedIndexChanged" 
                                                    RepeatDirection="Horizontal">
                                                     
                    <asp:ListItem>Embossing</asp:ListItem>  
                     
                    <asp:ListItem>Embossing&Closing</asp:ListItem>  
                    
                </asp:RadioButtonList> 
                                                  
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="160px">
                                               <%-- Record Date:--%>
                                               <%--<asp:RadioButton ID="RadioButton2" runat="server" Text="Embossing & Closing" Font-Bold="true" />--%> 
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="150px">
                                                <%--<asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />--%>
                                                 
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                               <%-- <td align="left" class="form_text">
                                    Select State: <span class="alert">* </span>
                                </td>
                                <td align="left" class="style3">
                                    <asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" AutoPostBack="true"
                                        ID="dropDownListOrg" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>--%>
                               
                                <td colspan="2" class="style8">
                                    Select Location:
                                </td>
                                <td align="left" class="style15">
                                    <asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="150px" ID="dropDownListClient"
                                        runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
            <td colspan="2" class="style8">Vehicle Reg.NO <span class="alert">* </span> : </td>
            <td><asp:TextBox ID="txtVehicleNo" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2" class="style8" width="250px">Front Laser Number:</td>
            <td>
                <%--<asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" ID="DDLReqType" runat="server">
                <asp:ListItem Text="--Select Request Type--" Value="--Select Request Type--"></asp:ListItem>
                    <asp:ListItem Text="Error" Value="Error"></asp:ListItem>
                    <asp:ListItem Text="New Features" Value="New Features"></asp:ListItem>
                </asp:DropDownList>--%>
                <asp:TextBox ID="txtFlaserNo" runat="server"></asp:TextBox>
            </td>
        </tr>
            
        <tr>
            <td colspan="2" class="style8">Rear Laser Number:</td>
            <td>
                <%--<asp:FileUpload ID="FileUpload1" runat="server"  />--%>
                <%--<a href=""  id="ShowFile" target="_self" visible="false" runat="server">Download</a>
                <asp:LinkButton ID="ShowFile" runat="server" visible="false" Text="Download" 
                    onclick="ShowFile_Click"> </asp:LinkButton>--%>
                    <asp:TextBox ID="txtRno" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="style8">Date Of Embossing:</td>
             <td onmouseup="OrderDate_OnMouseUp()" align="left" class="style15">
                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Width="200px">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                    <td>
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                            onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                    </td>
        </tr>
            
        <tr>
            <td colspan="2" class="style8" valign="top">Date Of Closing:</td>
             <td valign="middle" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" 
                class="style15">
                                        <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Visible="True">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                    <td valign="middle" align="left" visible="false" 
                class="style6">
                                        <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                            class="calendar_button" src="../images/btn_calendar.gif" />
                                    </td>
        </tr>
        <tr>
            <td class="style10">
                                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                                        </td>
            <td>
                <br />
            </td>
        </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Button ID="Button1" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Save" OnClick="Button1_Click" Width="150px" />
                                    <%--<asp:Button ID="ButImpData" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Generate Data For NIC" onclick="ButImpData_Click" />--%>
                                </td>
                            </tr>
                                <tr>
                                    <td colspan="2">
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
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
