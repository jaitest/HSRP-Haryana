<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="WeekendReport.aspx.cs" Inherits="HSRP.Report.WeekendReport" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script language="javascript" type="text/javascript">

        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
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
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Weekend Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td>
                                          <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                            <tr>
                                                <td>
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style=" width:100px">
                                                    <asp:Label Text="HSRP State:" Visible="false" runat="server" ID="labelOrganization" />
                                                </td>
                                                <td valign="middle" style=" width:200px">
                                                    <asp:DropDownList ID="DropDownListStateName"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID">
                                                    </asp:DropDownList>
                                                </td>
                                             
                                                <td valign="middle" class="form_text" nowrap="nowrap" style=" width:100px">
                                                    &nbsp;&nbsp;
                                                    <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left" style=" width:120px;">
                                                    <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>

                                                </td>
                                                <td valign="top" align="left">
                                                    <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif"/>
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style=" width:100px" >
                                                    &nbsp;&nbsp;<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" style=" width:120px;">
                                                    <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                                <td valign="top" align="left">
                                                    <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()" style="padding-right:20px;"
                                                        class="calendar_button" src="../images/btn_calendar.gif"/>
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="GO" style=" width:80px" OnClick="Button1_Click" OnClientClick=" return validate()"/>
                                                </td>
                                                
                                                <tr>
                                                    <td colspan="9">
                                                        <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                               
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table  width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
               
                
                     <tr>                         
                         <td style="padding-left:22px; color:green;">Status</td> &nbsp;<td style="padding-left:105px; color:green;">NO</td>&nbsp;&nbsp;<td style="padding-left:105px; color:green;">Amt</td>
                           
                     </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            <asp:Label ID="lblcollection" runat="server" Text="Collection :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtcollectionNo" runat="server" class="form_textbox11" ReadOnly="true"  Enabled="false"></asp:TextBox> </td> 
                    
                        <td> <asp:TextBox ID="txtcollectionamt" runat="server" class="form_textbox11" ReadOnly="true"  Enabled="false" ></asp:TextBox> </td> 
                      
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px"> 
                            <asp:Label ID="lblemb" runat="server" Text="Embossed :"></asp:Label>
                            <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtembno" runat="server" TabIndex="2"  ReadOnly="true"  Enabled="false"
                                class="form_textbox11"></asp:TextBox> </td> 
                  
                         <td> <asp:TextBox ID="txtembamt" runat="server" TabIndex="2" class="form_textbox11" ReadOnly="true"  Enabled="false"></asp:TextBox> </td> 
                       
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            <asp:Label ID="lbldispatch" runat="server" Text="Dispatched :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtdispatchno" runat="server" TabIndex="4" class="form_textbox11" ReadOnly="true"  Enabled="false" ></asp:TextBox> </td> 
                      
                         <td> <asp:TextBox ID="txtdispatchamt" runat="server" TabIndex="4" class="form_textbox11" ReadOnly="true"  Enabled="false" ></asp:TextBox> </td> 
                     
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px"> <asp:Label ID="lblrec" runat="server" Text="Received at AFC :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtrecno" runat="server" TabIndex="6" class="form_textbox11" ReadOnly="true"  Enabled="false"></asp:TextBox> </td> 

                     
                        <td> <asp:TextBox ID="txtrecamt" runat="server" TabIndex="6" class="form_textbox11" ReadOnly="true"  Enabled="false"></asp:TextBox> </td> 
                       
                    </tr>
                     <tr>
                        <td class="form_text" style="padding-bottom: 10px"> <asp:Label ID="lblaffix" runat="server" Text="Affixed :" ReadOnly="true"  Enabled="false"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtaffino" runat="server" TabIndex="4" class="form_textbox11" ReadOnly="true"  Enabled="false" MaxLength="10"></asp:TextBox> </td> 
                      
                         <td> <asp:TextBox ID="txtaffiamt" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10"></asp:TextBox> </td> 
                     
                    </tr>
                     <tr>
                        <td class="form_text" style="padding-bottom: 10px"><asp:Label ID="lblbal" runat="server" Text="Balance at AFC :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtbalanceno" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox> </td> 
                      
                         <td> <asp:TextBox ID="txtbalanceamt" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox> </td> 
                     
                    </tr>

                     <tr>
                        <td class="form_text" style="padding-bottom: 10px"> <asp:Label ID="lblmanpower" runat="server" Text="Manpower Count Upto (04.09.2016) :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtmanpowerno" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox> </td>                        
                         <td> <asp:TextBox ID="txtmanpoweramt" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox> </td> 
                     
                    </tr>

                    <tr>
                        <td class="form_text" style="padding-bottom: 10px"><asp:Label ID="lblnetadd" runat="server" Text="Net Addition(+) / Reduction(-)  On CTC :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtnetadditionno" runat="server" TabIndex="4" class="form_textbox11"  MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox> </td>                        
                         <td> <asp:TextBox ID="txtnetadditionamt" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox> </td> 
                     
                    </tr>
                  
                        <tr>
                        <td class="form_text" style="padding-bottom: 10px"><asp:Label ID="lblmanpowerdate" runat="server" Text="Manpower As on Date :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtmanpowerdateno" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox> </td>                        
                         <td> <asp:TextBox ID="txtmanpowerdateamt" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox> h</td> 
                     
                    </tr>
                        <tr>
                        <td class="form_text" style="padding-bottom: 10px"><asp:Label ID="lblfailure" runat="server" Text="Failure - Embossing :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtfailureno" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)">0</asp:TextBox> </td>                        
                         <td> <asp:TextBox ID="txtfailuremodule" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)">0</asp:TextBox> </td> 
                     
                    </tr>
                      <tr>
                        <td class="form_text" style="padding-bottom: 10px"><asp:Label ID="Label2" runat="server" Text="Failure - Received Invoicing :"></asp:Label> <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="txtfailurereceivedno" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)">0</asp:TextBox> </td>                        
                         <td> <asp:TextBox ID="txtfailurereceivedmodule" runat="server" TabIndex="4" class="form_textbox11" MaxLength="10" onkeypress="return isNumberKey(event)">0</asp:TextBox> </td> 
                     
                    </tr>
                  
                        
                 
                </table>
                                    </td>
                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblmsg" runat="server" Font-Names="Tahoma" ForeColor="blue"></asp:Label>
                                                    </td>
                                                </tr>
                                            <tr>
                                                <td align="right" valign="middle">
                                                   
                                                     <asp:Button ID="btnsave" runat="server" Text="Save" class="button" OnClientClick=" return validate()" OnClick="btnsave_Click"/>
                                                     &nbsp;&nbsp;&nbsp;&nbsp;
                                                     <asp:Button ID="btnExportToExcel" runat="server" Text="Report In Excel" ToolTip="Please Click for Report"
                                                        class="button" OnClientClick=" return validate()" OnClick="btnExportToExcel_Click" />
                   
                                                </td>
                                              
                                            </tr>
                            </table>
                            



                        </td>
                    </tr>
                 
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                            </table>
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
</asp:Content>
