﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HSRPRTOInventory.aspx.cs"
    Inherits="HSRP.Master.HSRPRTOInventory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../javascript/common.js"></script>
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
    <script language="javascript" type="text/javascript">
        
        function validate() {

            if (document.getElementById("<%=DropDownListStation.ClientID%>").value == "") {
                alert("Please Provide Status ");
                document.getElementById("<%=DropDownListStation.ClientID%>").focus();
                return false;
            }


        

            if (document.getElementById("<%=DropDownListProduct.ClientID%>").value == "0") {
                alert("Please Select  Product ");
                document.getElementById("<%=DropDownListProduct.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtBoxMake.ClientID%>").value == "0") {
                alert("Please Select  Location Name ");
                document.getElementById("<%=txtBoxMake.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=TextBoxModel.ClientID%>").value == "") {
                alert("Please Provide  Branch Name ");
                document.getElementById("<%=TextBoxModel.ClientID%>").focus();
                return false;
            }
            

         
        

            if (document.getElementById("<%=txtBoxSerial.ClientID%>").value == "") {
                alert("Please Provide Deposit Amount ");
                document.getElementById("<%=txtBoxSerial.ClientID%>").focus();
                return false;
            }




            if (document.getElementById("<%=DropDownListStation.ClientID%>").value == "") {
                alert("Please Provide Deposited by ");
                document.getElementById("<%=DropDownListStation.ClientID%>").focus();
                return false;
            }



            if (invalidChar(document.getElementById("TextBoxBankSlipNo"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("TextBoxBankSlipNo").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxDepositAmount"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("TextBoxDepositAmount").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxDepositby"))) {
                alert("Special Characters Not Allowed in Deposited by");
                document.getElementById("TextBoxDepositby").focus();
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

            function ischarKey(evt) {
                //debugger;
                 var charCode = (evt.which) ? evt.which : event.keyCode
                 if (charCode > 31 && (charCode < 96 || charCode > 122) && (charCode < 65 || charCode > 90 ) && (charCode < 31 || charCode > 33 ))
                    return false;

                 return true;
                }

//                var TCode = document.getElementById("TextBoxDepositby").value;
//                if (/[^a-zA-Z]/.test(TCode)) {
//                    alert('Input is not alphanumeric');
//                    return false;
//                }
//                return true;


//            }
              

  

 </script>
    <style type="text/css">
        .style4
        {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 210px;
            padding-left: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" width:100%;">
        Bank Transaction<fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Bank Transaction</div>
            </legend>
            <br />
            <br />
              <div style="width: 100%; margin: 0px auto 0px auto">
              
            <table  border="0" align="right"  style="height: 348px; width:85%;">
               <tr>
                    <td class="style4">
                        RTO Name <span class="alert"> </span>
                    </td>
                  
                    <td>
                        <table id="Table1" runat="server" style="margin-left: 8px; width: 292px;" align="left" cellspacing="0"
                            cellpadding="0" border="0">
                            <tr>
                                <td valign="top" >

                                                           
                   <%--<asp:TextBox ID="TextBoxRTO" runat="server" class="form_textbox12" MaxLength="30" ></asp:TextBox>--%>
                
                    
                                 <%--    <ComponentArt:Calendar ID="DepositDate" runat="server" PickerFormat="Custom" 
                                        PickerCustomFormat="dd/MM/yyyy"  TabIndex="1"
                                        ControlType="Picker" PickerCssClass="picker" Visible="true" >
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="DepositDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar> --%>
                                    <asp:Label ID="LblRTO" runat="server" ></asp:Label>
                                </td>
                                 <td class="style4">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Emb Center Name <span class="alert"> </span>
                    </td>
                                <td style="font-size: 10px;">
                                     <td valign="top" >

                 
                                    <asp:Label ID="LblEmb" runat="server" ></asp:Label>
                                </td>
                                </td>
                                <td valign="top">
                                 <%--    <img id="calendar_from_button" TabIndex="2" alt="" onclick="DepositDate_OnClick()"
                                        onmouseup="DepositDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />  --%>
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        Product
                        <%-- <span class="alert">* </span>--%>
                        *</td>
                    <td>
                        <asp:DropDownList ID="DropDownListProduct" runat="server" 
                            Style="margin-left: 8px" TabIndex="4" Width="165px" AutoPostBack="true" 
                            onselectedindexchanged="DropDownListProduct_SelectedIndexChanged">
                            <asp:ListItem>--Select Product--</asp:ListItem>
                          
                            <asp:ListItem>Data Card</asp:ListItem>
                            <asp:ListItem>Laptop</asp:ListItem>
                            <asp:ListItem>Desktop</asp:ListItem>
                            <asp:ListItem>Mini Desktop</asp:ListItem>
                            <asp:ListItem>Inverter</asp:ListItem>
                            <asp:ListItem>UPS</asp:ListItem>
                            <asp:ListItem>Printer</asp:ListItem>
                            <asp:ListItem>Laser Printer</asp:ListItem>
                            <asp:ListItem>Third Sticker Printer</asp:ListItem>
                            <asp:ListItem>Mobile</asp:ListItem>
                          
                        </asp:DropDownList>

                       <%--  <asp:TextBox ID="TextBoxBankSlipNo" class="form_textbox12" runat="server" 
                            MaxLength="30"  TabIndex="3"></asp:TextBox>  --%> 
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        Make <span class="alert"> *</span></td>
                    <td>
                        <asp:TextBox ID="txtBoxMake" runat="server" class="form_textbox12" MaxLength="30"></asp:TextBox>
                       <%-- <asp:DropDownList ID="DropDownListBankName" runat="server" 
                            Style="margin-left: 8px" TabIndex="4" Width="165px" AutoPostBack="true" 
                            onselectedindexchanged="DropDownListBankName_SelectedIndexChanged">
                            <asp:ListItem>--Select Bank Name--</asp:ListItem> --%>
                          
                        </asp:DropDownList>
                         
                    </td>
                </tr>
             

                 <tr>
                    <td class="style4">
                        Model 
                        <%--<span class="alert"> </span>--%>
                        *</td>
                    <td>

                    <asp:TextBox ID="TextBoxModel" runat="server" class="form_textbox12" MaxLength="30"></asp:TextBox>
                       
                       <%--  <asp:Label ID="lblAccountNo" runat="server" class="form_textbox12" 
                            Width="160px" TabIndex="5"></asp:Label>  --%>
                  

                
                    </td>
                </tr>

                   <tr>
                    <td class="style4">
                       Serial No./Mobile Number
                        <span class="alert"> *</span></td>
                    <td>
                       
                   <asp:TextBox ID="txtBoxSerial" runat="server" class="form_textbox12" MaxLength="30" ></asp:TextBox>
                
                    </td>
                </tr>

                <tr>
                    <td class="style4">
                        Working/Live Status <span class="alert"> *</span></td>
                    <td>
                        <asp:DropDownList ID="DropDownListStation" runat="server" 
                            Style="margin-left: 8px" TabIndex="4" Width="165px" AutoPostBack="true" 
                            onselectedindexchanged="DropDownListProduct_SelectedIndexChanged">
                            <asp:ListItem>--Select--</asp:ListItem>
                          
                            <asp:ListItem>Yes</asp:ListItem>
                            <asp:ListItem>No</asp:ListItem>
                           
                        </asp:DropDownList>
                       <%--  <asp:TextBox ID="TextBoxWorking" class="form_textbox12" runat="server" 
                            MaxLength="30" TabIndex="6"></asp:TextBox> --%>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                       User Name <span class="alert"> *</span></td>
                    <td>
                  
                        <asp:TextBox ID="TextBoxUser" class="form_textbox12" runat="server" 
                            MaxLength="30" TabIndex="7" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                       User Mobile No.<span class="alert"> *</span></td>
                    <td>
                                    
                        <asp:TextBox ID="TextBoxMobile" class="form_textbox12" runat="server" 
                            MaxLength="30" TabIndex="7" ></asp:TextBox>
                    
                     <%--    <asp:DropDownList ID="ddlLocation" runat="server" 
                            DataTextField="RTOLocationName" DataValueField="RTOLocationID" Height="20px" 
                            style="margin-left: 10px" Width="163px">
                        </asp:DropDownList> --%>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        Remarks 
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxRemarks" runat="server" Height="56px" 
                            class="form_textbox12" TextMode="MultiLine" MaxLength="30"
                            Width="223px" TabIndex="8"></asp:TextBox>
                    </td>
                </tr>
              
            </table>
            <div style=" clear:both"></div>
            <table border="0"  cellpadding="2" cellspacing="3" width="490px" align="right" >
             <tr>
                  <td nowrap="nowrap" align="right"  >
                                                            <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                                            <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                                                          <%--   <asp:Button ID="buttonUpdate" runat="server" TabIndex="10" class="button" Visible="false"
                                                                 Text="Update" onclick="buttonUpdate_Click"  OnClientClick="return validate()" 
                                                                 /> --%>
                                                            &nbsp;&nbsp;
                                                            <%--OnClientClick="return validate();"--%>
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="9" class="button" Text="Save"
                                                                OnClientClick=" return validate()" Visible="false" 
                                                                OnClick="ButtonSubmit_Click" />&nbsp;&nbsp;
                                                          <%--  <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                                id="buttonClose" value="Close" class="button" TabIndex="11"/> --%> 
                                                            &nbsp;&nbsp;
                                                            <%-- <input type="reset" class="button" value="Reset" />--%>
                                                         <%--    <asp:Button ID="btnReset" runat="server"  class="button" Text="Reset" 
                                                                onclick="btnReset_Click" TabIndex="12" /> --%>
                                                        </td>
                    
                
                    
                </tr>
              
           </table>
            </div>
           
           <table>
             <tr>
                <%--   <td>  <ComponentArt:Calendar runat="server" ID="CalendarDepositDate" AllowMultipleSelection="false"
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
                      </td> --%>
                        <td>
                     <%--    <ComponentArt:Calendar runat="server" ID="Calendar1" AllowMultipleSelection="false"
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
                        </ComponentArt:Calendar> --%>
                        </td>
                </tr>
           </table>
        </fieldset>
    </div>
    </form>
</body>
</html>