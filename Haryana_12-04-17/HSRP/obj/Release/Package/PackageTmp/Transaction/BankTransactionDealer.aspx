<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BankTransactionDealer.aspx.cs" Inherits="HSRP.Transaction.BankTransactionDealer" %>


<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <html xmlns="http://www.w3.org/1999/xhtml">

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
            if (document.getElementById("<%=DepositDate.ClientID%>").value == "") {
                alert("Please Provide Deposit Date ");
                document.getElementById("<%=DepositDate.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxDepositAmount.ClientID%>").value == "") {
                alert("Please Provide Deposit Amount ");
                document.getElementById("<%=TextBoxDepositAmount.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListBankName.ClientID%>").value == "Select Bank Name") {
                alert("Please Select  Bank Name ");
                document.getElementById("<%=DropDownListBankName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DdltransactionMode.ClientID%>").value == "--Select--") {
                alert("Please Select Payment Mode");
                document.getElementById("<%=DdltransactionMode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtChecqNo.ClientID%>").value == "") {
                alert("Please Provide  Payment Details ");
                document.getElementById("<%=txtChecqNo.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtBranchName.ClientID%>").value == "") {
                alert("Please Provide  Bank Address ");
                document.getElementById("<%=txtBranchName.ClientID%>").focus();
                return false;
            }


            

            

            


            

           


            //if (invalidChar(document.getElementById("TextBoxDepositAmount"))) {
            //    alert("Special Characters Not Allowed.");
            //    document.getElementById("TextBoxDepositAmount").focus();
            //    return false;
            //}

            //if (invalidChar(document.getElementById("TextBoxDepositby"))) {
            //    alert("Special Characters Not Allowed in Deposited by");
            //    document.getElementById("TextBoxDepositby").focus();
            //    return false;
            //}



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
            if (charCode > 31 && (charCode < 96 || charCode > 122) && (charCode < 65 || charCode > 90) && (charCode < 31 || charCode > 33))
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

<body>
    
    <div style=" width:100%;">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Bank Deposit</div>
            </legend>
            <br />
            <br />
              <div style="width: 100%; margin: 0px auto 0px auto">
              
            <table  border="0" align="right"  style="height: 348px; width:85%;">
                <tr>
                    <%--<td class="style4">
                        ReferenceNo<span class="alert">* </span>
                    </td>--%>

                   <td class="style4">
                        <asp:Label ID="lblref" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                    </td>

                </tr>
               <tr>
                   

                    <td class="style4">
                        Deposit Date <span class="alert">* </span>
                    </td>
                    <td>
                        <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                            cellpadding="0" border="0">
                            <tr>
                                <td valign="top" onmouseup="DepositDate_OnMouseUp()">
                                    <ComponentArt:Calendar ID="DepositDate" runat="server" PickerFormat="Custom" 
                                        PickerCustomFormat="dd/MM/yyyy"  TabIndex="1"
                                        ControlType="Picker" PickerCssClass="picker" Visible="true" >
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="DepositDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                </td>
                                <td style="font-size: 10px;">
                                    &nbsp;
                                </td>
                                <td valign="top">
                                    <img id="calendar_from_button" TabIndex="2" alt="" onclick="DepositDate_OnClick()"
                                        onmouseup="DepositDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        Deposit Amount <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxDepositAmount" class="form_textbox12" runat="server" 
                            onkeypress="return isNumberKey(event)" MaxLength="30" TabIndex="6"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        Bank Name <span class="alert">* </span>
                    </td>
                    <td>
                        <%-- <asp:TextBox ID="txtBankName" runat="server" class="form_textbox12" MaxLength="30"></asp:TextBox>--%>
                        <asp:DropDownList ID="DropDownListBankName" runat="server" 
                            Style="margin-left: 8px" TabIndex="4" Width="165px" AutoPostBack="true" 
                            onselectedindexchanged="DropDownListBankName_SelectedIndexChanged">                           
                          
                        </asp:DropDownList>
                         
                    </td>
                </tr>

                 <tr>
                    <td class="style4">
                        Payment Mode <span class="alert">* </span>
                    </td>
                    <td>
                          <asp:DropDownList ID="DdltransactionMode" runat="server"
                                                   Height="22px" Width="165px" AutoPostBack="true" style="margin-left:8px;">
                                                  <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                                                <asp:ListItem Value="Cheque">Cheque</asp:ListItem>
                                                <asp:ListItem Value="Cash">Cash</asp:ListItem>
                                                    <asp:ListItem Value="NEFT/RTGS">NEFT/RTGS</asp:ListItem>
                                               
                                            </asp:DropDownList>
                                          
                          
                        
                         
                    </td>
                </tr>
             
                <%-- edited on 10/10/2015 --%>
                <tr>
                    <td class="style4">
                       Payment Details
                        <span class="alert">* </span>
                    </td>
                    <td>
                        <%--<asp:Label ID="lblChecqNo" runat="server" class="form_textbox12" 
                            Width="160px" TabIndex="5"></asp:Label>--%>
                  <asp:TextBox ID="txtChecqNo" runat="server" class="form_textbox12" MaxLength="30" ></asp:TextBox>

                
                    </td>
                </tr>
                <%-- end --%>
                 

                <tr>
                    <td class="style4">
                       Bank Address
                        <span class="alert">* </span>
                    </td>
                    <td>
                       
                   <asp:TextBox ID="txtBranchName" runat="server" class="form_textbox12" MaxLength="30" ></asp:TextBox>
                
                    </td>
                </tr>

                   <tr>
                    <td class="style4">
                       
                        
                    </td>
                    <td>
                       
                   <asp:TextBox ID="txtacc" runat="server"  Visible="false" class="form_textbox12" ></asp:TextBox>
                
                    </td>
                </tr>
                 <tr>
                    <td class="style4">
                       TransactionID:                        
                    </td>
                    <td>
                       
                       &nbsp;<asp:Label ID="lblTransactionID" runat="server" Font-Size="18px" ForeColor="Blue" Text=""></asp:Label>
                
                    </td>
                </tr>        

                
                
              
            </table>
            <div style=" clear:both"></div>
            <table border="0"  cellpadding="2" cellspacing="3" width="490px" align="right" >
             <tr>
                  <td nowrap="nowrap" align="right"  >
                      
                       <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                                          <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                                                            <asp:Button ID="buttonUpdate" runat="server" TabIndex="10" class="button" Visible="false"
                                                                 Text="Update" onclick="buttonUpdate_Click" /> <%-- OnClientClick="return validate()"--%>
                                  <%--<asp:Button ID="btnApproval" runat="server" TabIndex="10" class="button" Visible="true"
                                                                 Text="Approved"  OnClientClick="return validate()" OnClick="btnApproval_Click" 
                                                                 />--%>

                                                            &nbsp;&nbsp;
                                                            <%--OnClientClick="return validate();"--%>
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="9" class="button" Text="Save"
                                                                OnClientClick=" return validate()" Visible="false" 
                                                                OnClick="ButtonSubmit_Click" />&nbsp;&nbsp;
                                                          <%--  <input type="button"  name="buttonClose" id="buttonClose"   onclick="javascript: parent.googlewin.close();"  value="Close" class="button" TabIndex="11"/>
                                                            &nbsp;&nbsp;--%>
                                                            <%-- <input type="reset" class="button" value="Reset" />--%>
                                                            <asp:Button ID="btnReset" runat="server"  class="button" Text="Reset" 
                                                                onclick="btnReset_Click" TabIndex="12" />
                                                        </td>
                    
                
                    
                </tr>
              
           </table>
            </div>
           
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
           </table>
        </fieldset>
    </div>
    
</body>
</html>

</asp:Content>
