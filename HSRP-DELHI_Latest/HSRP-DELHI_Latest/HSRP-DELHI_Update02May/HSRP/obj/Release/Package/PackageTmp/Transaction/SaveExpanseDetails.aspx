<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SaveExpanseDetails.aspx.cs" Inherits="HSRP.Transaction.SaveExpanseDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../javascript/AutoNumeric.js" type="text/javascript"></script>
    <script type="text/javascript">

        // example uses the selector "input" with the class "auto" & no options passed
        jQuery(function ($) {
            $('#ctl00_ContentPlaceHolder1_txtAmount').autoNumeric();
            $('#ctl00_ContentPlaceHolder1_txtVat').autoNumeric();
            $('#ctl00_ContentPlaceHolder1_txtservicetax').autoNumeric();
            $('#ctl00_ContentPlaceHolder1_txtExcduty').autoNumeric();
            $('#ctl00_ContentPlaceHolder1_txtOthers').autoNumeric();

        });

        // example uses the selector "input" with the class "auto" & with options passed
        // See details below on allowed options
        jQuery(function ($) {
            $('#ctl00_ContentPlaceHolder1_txtAmount').autoNumeric({ aSep: '.', aDec: '' });
            $('#ctl00_ContentPlaceHolder1_txtVat').autoNumeric({ aSep: '.', aDec: '' });
            $('#ctl00_ContentPlaceHolder1_txtservicetax').autoNumeric({ aSep: '.', aDec: '' });
            $('#ctl00_ContentPlaceHolder1_txtExcduty').autoNumeric({ aSep: '.', aDec: '' });
            $('#ctl00_ContentPlaceHolder1_txtOthers').autoNumeric({ aSep: '.', aDec: '' });
        });

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
            if (document.getElementById("ctl00_ContentPlaceHolder1_ddlExpense").value == "--Select Expense--") {
                alert("Select Expense");
                document.getElementById("ctl00_ContentPlaceHolder1_ddlExpense").focus();
                return false;
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_txtBillNo").value == "") {
                alert("Fill Bill No.");
                document.getElementById("ctl00_ContentPlaceHolder1_txtBillNo").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value == "") {
                alert("Fill Basic Amount.");
                document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").focus();
                return false;
            } 
            if (document.getElementById("ctl00_ContentPlaceHolder1_txtClaimedBy").value == "") {
                alert("Fill Claimed By Person.");
                document.getElementById("ctl00_ContentPlaceHolder1_txtClaimedBy").focus();
                return false;
            }
//            if ((parseFloat(document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value) <= 0.0) || (parseFloat(document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value) <= 0)) {
//                alert("Basic Amount Should be greater than zero.");
//                document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").focus();
//                return false;
//            }
            else 
            {
                return true;
            }
        }

       
       function isNumberKey(evt) {
           var charCode = (evt.which) ? evt.which : event.keyCode
           if (charCode > 31 && (charCode < 48 || charCode > 57 || charCode == 110) && charCode != 46) {
               alert("1");
               return false;
           }
           else {
               alert(evt.value);
              if ((evt.value) && (evt.value.indexOf('.') >= 0))
                                return false;

               alert("2");

//               var len = document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value.length;
//               var index = document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value.indexOf('.');

//               if (index > 0 && charCode == 46) {
//                   return false;
//               }
//               if (index > 0) {
//                   var CharAfterdot = (len + 1) - index;
//                   if (CharAfterdot > 6) {
//                       return false;
//                   }
//               }
//               if (len < 8 && charCode == 46) {
//                   return false;
//               }

//               var text = document.getElementById("ctl00_ContentPlaceHolder1_txtAmount").value;

//               if (charCode != 46 && len == 8) {
//                   return false;
//               }

           }
           return true;
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

    <style type="text/css">
        .modalPopup
        {
            background-color: #FFFFFF;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
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
                    Record Expense Details
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
                                    Today Saved Record :
                                </td>
                                <td align="left" class="form_text">
                                    <asp:Label ID="lblTotalRecord" runat="server"></asp:Label>

                                </td>
                               
                                <td align="left" class="form_text">
                                    
                                </td>
                                <td align="left">
                                     
                                </td>
                            </tr>
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
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" Width="170px" ID="dropDownListClient"
                                        runat="server" DataTextField="RTOLocationName" 
                                        DataValueField="RTOLocationID" 
                                        onselectedindexchanged="dropDownListClient_SelectedIndexChanged" 
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="form_text">
                                    Select Employee:<span style="color:Red">*</span>
                                </td>
                                <td align="left">
                                             <asp:DropDownList ID="dropDownListEmployee" runat="server" AutoPostBack="true" 
                                                 DataTextField="UserName" DataValueField="UserId" Style="margin-left: 8px" 
                                                 Font-Size="Small" Width="170px" >
                                                 <asp:ListItem>--Select Employee--</asp:ListItem>
                                             </asp:DropDownList>                                         
                                </td>
                               
                                <td align="left" class="form_text">
                                    &nbsp;</td>
                                <td align="left">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                              <td align="left" class="form_text">
                                    Expense: <span style="color:Red">*</span>
                                </td>
                                <td>
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" Width="170px" ID="ddlExpense"
                                        runat="server" DataTextField="ExpenseName" DataValueField="ExpenceID">
                                    </asp:DropDownList>
                                </td>
                                <td align="left" class="form_text">
                                    Bill No.: <span style="color:Red">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtBillNo" runat="server" MaxLength="20"  class="form_textbox" ></asp:TextBox>
                                </td>
                               
                            </tr>

                            <tr>
                             
                                <td align="left" class="form_text">
                                    Bill Date: <span style="color:Red">*</span>
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
                                 <td align="left" class="form_text">
                                   Basic Amount: <span style="color:Red">*</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" Text="0"  AutoPostBack="true" MaxLength="7" 
                                        class="form_textbox" ontextchanged="txtAmount_TextChanged" 
                                        ></asp:TextBox>

                                   
                                    
                                </td>
                               
                            </tr>

                            <%--<asp:UpdatePanel ID="UpdatePanelTotalAmount" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>--%>
                            <tr>
                            <td align="left" valign="top" class="form_text">
                                    VAT\CST: </td>
                                <td align="left" class="form_text" valign="top"> 
                                    <asp:TextBox ID="txtVat" AutoPostBack="true" Text="0" runat="server" class="form_textbox" 
                                        ontextchanged="txtVat_TextChanged"></asp:TextBox>
                                </td>
                            <td align="left" valign="top" class="form_text">
                                    Service Tax: </td>
                                <td align="left" class="form_text"> 
                                    <asp:TextBox ID="txtservicetax" runat="server" AutoPostBack="true" Text="0" class="form_textbox" 
                                        ontextchanged="txtservicetax_TextChanged"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                            <td align="left" valign="top" class="form_text">
                                    Excise Duty + Cess: </td>
                                <td align="left" class="form_text" valign="top"> 
                                    <asp:TextBox ID="txtExcduty" runat="server" AutoPostBack="true" Text="0" class="form_textbox" 
                                        ontextchanged="txtExcduty_TextChanged"></asp:TextBox>
                                </td>
                            <td align="left" valign="top" class="form_text">
                                    Others :</td>
                                <td align="left" class="form_text"> 
                                    <asp:TextBox ID="txtOthers" Text="0" AutoPostBack="true" runat="server" class="form_textbox" 
                                        ontextchanged="txtOthers_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <%--</ContentTemplate>
                            <Triggers>
                                     <asp:AsyncPostBackTrigger ControlID="txtAmount" EventName="TextChanged" />
                            </Triggers>
                             </asp:UpdatePanel>--%>


                            
                            <tr>
                            <td align="left" valign="top" class="form_text">
                                    Total Amount :</td>
                                <td align="left" class="form_text"> 
                                <asp:UpdatePanel ID="UpdatePanelTotalAmount" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                    <asp:TextBox ID="txttotamt" Text="0" ReadOnly="true" runat="server" class="form_textbox"></asp:TextBox>
                                    </ContentTemplate>
                            <Triggers>
                                     <asp:AsyncPostBackTrigger ControlID="txtAmount" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtVat" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtservicetax" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtExcduty" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtOthers" EventName="TextChanged" />
                            </Triggers>
                             </asp:UpdatePanel>
                                </td>

                            <td align="left" valign="top" class="form_text">
                                    Vendor/Supplier Name:</td>
                                <td align="left" class="form_text" valign="top"> 
                                    <asp:TextBox ID="txtVenName" runat="server" class="form_textbox"></asp:TextBox>
                                </td>
                            
                            </tr>
                            <tr>
                            <td align="left" valign="top" class="form_text">
                                    Claimed By Person: <span style="color:Red">*</span></td>
                            <td align="left" class="form_text" valign="top"> 
                                    <asp:TextBox ID="txtClaimedBy" runat="server" class="form_textbox"></asp:TextBox>
                                </td>
                            <td align="left" valign="top" class="form_text">
                                    Remarks:</td>
                                <td align="left" class="form_text"> 
                                    <asp:TextBox ID="txtRemarks" runat="server" style="background-color: #FBFBFB;border-color: #E5E5E5 #E5E5E5 #999999;border-left: 1px solid #E5E5E5;border-right: 1px solid #E5E5E5;border-style: solid;border-width: 1px; color: #5C5B5B;font-size: medium;font-weight: normal;left: 8px; padding-left: 2px;    position: relative;    text-decoration: none;    top: 0; width: 215px;" TextMode="MultiLine" Rows="4" Columns="40"  ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                <asp:UpdatePanel ID="UpdatePanelMsg" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                    </ContentTemplate>
                                     <Triggers>
                                     <asp:AsyncPostBackTrigger ControlID="txtAmount" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtVat" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtservicetax" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtExcduty" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtOthers" EventName="TextChanged" />
                            </Triggers>
                             </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                <asp:UpdatePanel ID="UpdatePanelButton" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                    <asp:Button ID="btnSave" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Save" onclick="btnSave_Click"  />
                                        
                                        &nbsp;&nbsp;
                             <a class="button" href="../LiveReports/LiveTracking.aspx" style="height:20px">Close</a>
                                        </ContentTemplate>
                                         <Triggers>
                                     <asp:AsyncPostBackTrigger ControlID="txtAmount" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtVat" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtservicetax" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtExcduty" EventName="TextChanged" />
                                     <asp:AsyncPostBackTrigger ControlID="txtOthers" EventName="TextChanged" />
                            </Triggers>
                             </asp:UpdatePanel>
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
</asp:Content>
