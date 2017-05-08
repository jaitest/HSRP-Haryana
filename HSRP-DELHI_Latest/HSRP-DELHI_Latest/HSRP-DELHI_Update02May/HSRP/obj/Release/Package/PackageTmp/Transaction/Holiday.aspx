<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Holiday.aspx.cs" Inherits="HSRP.Transaction.Holiday" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
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
    <div>

        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" style="background-image: url(../images/midtablebg.jpg)">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="26">
                                            <span class="headingmain">Holiday List</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                    <tr>
                                        <td height="26" bgcolor="#FFFFFF" class="maintext">
                                            <table width="98%" border="0" align="center" cellpadding="3" cellspacing="3">
                                                <tr>
                                                    <td>
                                                        <span id="lblMsg" class="header"></span>
                                                    </td>
                                                    <td style="width: 13%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>

                                            
                                     <td valign="middle" class="form_text">
                                       <asp:UpdatePanel ID="ddls" runat="server">
                                            
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="labelembcode" runat="server" Text="Emb.Code:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  MaxLength="10" TabIndex="1"
                                                            ID="txtEmbcode" runat="server"></asp:TextBox>
                                                        <br />
                                                    </td>
                                                </tr>--%>
                                               <%-- <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblDesignation" runat="server" Text="Designation:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  MaxLength="10" TabIndex="2"
                                                            ID="txtDesignation" runat="server"></asp:TextBox>
                                                     
                                                   </td>
                                                </tr>--%>
                                                <tr>
                                                     <td  Visible="false" class="form_text" nowrap="nowrap">&nbsp;&nbsp; <asp:Label Text="From:"  runat="server" ID="labelDate" /> &nbsp;&nbsp;</td>
                                    <td valign="top"   onmouseup="OrderDate_OnMouseUp()" align="left">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server"  PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"  
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                     onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>
                                                           
                                                           


                                                  <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTimein" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                 
                                                 <%-- <asp:RegularExpressionValidator ID="revTime" runat="server" ControlToValidate="txtTimein" ErrorMessage="hh:mi" SetFocusOnError="True" ValidationExpression="^(23):(00)|([01][0-9]|2[0-3]):([0-5][0-9])$" ValidationGroup="MyGroup"></asp:RegularExpressionValidator>--%>


                                                </tr>
                                                <tr>
                    <td class="form_text"> 
                        <asp:Label Text="Status:"  runat="server" ID="lblEmb" 
                            ForeColor="Black"  />
                    </td>
                    <td valign="middle" class="Label_user_batch" style="width: 165px">
                                                   
                                                      <asp:DropDownList ID="ddlEmbossingCenter" 
                                                                CausesValidation="false"  AutoPostBack="true"
                                                                runat="server" 
                                                                 
                                                                EnableTheming="False" Height="16px">
                                                          <asp:ListItem>--Select Status--</asp:ListItem>
                                                          <asp:ListItem>Y</asp:ListItem>
                                                          <asp:ListItem>N</asp:ListItem>
                                                            </asp:DropDownList>
                                                         </td>
                    </tr>
                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="label2" runat="server" Text="Description :"></asp:Label>
                                                        
                                                    </td>
                                                    <td style="width: 13%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" TextMode="MultiLine"
                                                            ID="txtdesc" runat="server" ></asp:TextBox>
                                                        <%--<asp:RegularExpressionValidator ValidationExpression="^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$" ID="EndTimeValidator" runat="server"  ErrorMessage="You Must Supply an END Time in Correct format"  ControlToValidate="txttimeout"></asp:RegularExpressionValidator>--%>
                                                   </td>
                                                     <td>
                                                         &nbsp;</td>

                                                      <%--<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txttimeout" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                     
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 45px" class="form_text" align="center" colspan="2">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="buttonSave" runat="server" Text="Save" class="button"  OnClick="buttonSave_Click" />&nbsp;&nbsp;
                                                        &nbsp;&nbsp;
                                                        <input type="reset" id="Reset" value="Reset" class="button" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="alert">
                                                        * Fields are mandatory.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" class="FieldText">
                                                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
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


                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
