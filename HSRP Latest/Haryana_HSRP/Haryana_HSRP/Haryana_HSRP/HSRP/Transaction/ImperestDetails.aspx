<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImperestDetails.aspx.cs" Inherits="HSRP.Transaction.ImperestDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script src="../javascript/AutoNumeric.js" type="text/javascript"></script>
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
                    <asp:Label ID="LabelFormName" runat="server" Text="Label"></asp:Label>
                </div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td colspan="5">
                        <table style="background-color: #FFFFFF" width="85%" border="0" align="center" cellpadding="3"
                            cellspacing="1">
                            <tr>
                                <td align="left" class="form_text">
                                    Select State: <span class="alert">* </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" Width="170px" AutoPostBack="true"
                                        ID="dropDownListOrg" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                               
                                <td align="left" class="form_text">
                                    Select Location:<span class="alert">* </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList Style="margin-left: 8px" AutoPostBack="true" 
                                        Font-Size="Small" Width="170px" ID="dropDownListClient"
                                        runat="server" DataTextField="RTOLocationName" 
                                        DataValueField="RTOLocationID" 
                                        onselectedindexchanged="dropDownListClient_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                              <td align="left" class="form_text">
                                    Select User: <span style="color:Red">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" Width="170px" ID="dropDownListUser"
                                        runat="server" DataTextField="Names" DataValueField="UserID">
                                    </asp:DropDownList>
                                </td>
                                <td align="left" class="form_text">
                                    Imperest Amount: <span style="color:Red">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" MaxLength="7"  class="form_textbox" ></asp:TextBox>
                                </td>
                               
                            </tr>

                            <tr>
                             
                                <td align="left" class="form_text">
                                    Date Of Issue: 
                                </td>
                                <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left">
                                                                &nbsp;&nbsp;
                                                                <componentart:calendar ID="OrderDate" Width="150px" runat="server" 
                                                                    PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker" Height="22px">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </componentart:calendar>
                                                                &nbsp;&nbsp;
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                </td>
                                
                               
                            </tr>

                           
                            <tr>
                            <td align="left" valign="top" class="form_text">
                                    Remarks:</td>
                                <td align="left" class="form_text" colspan="3"> 
                                    <asp:TextBox ID="txtRemarks" runat="server" 
                                        style="border-top: 1px solid #E5E5E5; border-bottom: 1px solid #999999; background-color: #FBFBFB; border-left: 1px solid #E5E5E5; border-right: 1px solid #E5E5E5; color: #5C5B5B; font-size: medium; font-weight: normal; left: 8px; padding-left: 2px;    position: relative;    text-decoration: none;    top: 0; width: 498px;" 
                                        TextMode="MultiLine" Rows="4" Columns="40"  ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                
                                    <asp:Button ID="btnUpdate" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Update" onclick="btnUpdate_Click" />
                                
                                    <asp:Button ID="btnSave" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Save" onclick="btnSave_Click"  />
                                        &nbsp;&nbsp;
                             <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" />
                                        
                                        <componentart:calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" 
                                        DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" 
                                        DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" 
                                        CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" 
                                        SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                        NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnChange" />
                                                        </ClientEvents>
                                                    </componentart:calendar>
                                </td>
                            </tr>
                                
                            
                            <tr>
                                <td colspan="4" align="left">
                                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                &nbsp;&nbsp;
                                    
                                    </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
