<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"  CodeBehind="Holiday.aspx.cs" Inherits="HSRP.Transaction.Holiday" %>

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

    <style type="text/css">
        .auto-style1 {
            width: 151px;
        }
        .auto-style2 {
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            color: Black;
            text-decoration: none;
            nowrap: nowrap;
            width: 87px;
        }
    </style>

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
                                                    <td class="auto-style1">
                                                        <span id="lblMsg" class="header"></span>
                                                    </td>
                                                    <td style="width: 13%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" class="auto-style1 form_text">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> 
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select State" ValidationGroup="save" InitialValue="--Select State--" ControlToValidate="DropDownListStateName"></asp:RequiredFieldValidator>
                                    </td>                                            
                                     <td valign="middle" class="form_text">
                                       <asp:UpdatePanel ID="ddls" runat="server">
                                            
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                     <td  nowrap="nowrap" class="auto-style1 form_text">
                                                          <asp:Label Text="Date:"  runat="server" ID="labelDate" /> </td>
                                    <td  onmouseup="OrderDate_OnMouseUp()">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server" Width="125px"  PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy" ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()" onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>
                                                           <td></td>
                                                           


                                                  <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTimein" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                 
                                                 <%-- <asp:RegularExpressionValidator ID="revTime" runat="server" ControlToValidate="txtTimein" ErrorMessage="hh:mi" SetFocusOnError="True" ValidationExpression="^(23):(00)|([01][0-9]|2[0-3]):([0-5][0-9])$" ValidationGroup="MyGroup"></asp:RegularExpressionValidator>--%>


                                                </tr>
                                                <tr>
                    <td valign="middle" class="auto-style2 form_text"> 
                        <asp:Label Text="Status:"  runat="server" ID="lblEmb" 
                            ForeColor="Black"  />
                    </td>
                    <td valign="middle">
                                                   
                                                      <asp:DropDownList ID="ddlEmbossingCenter" 
                                                                CausesValidation="false"  AutoPostBack="true"
                                                                runat="server" 
                                                                 Width="164px" 
                                                                EnableTheming="False">
                                                          <asp:ListItem>--Select Status--</asp:ListItem>
                                                          <asp:ListItem>Y</asp:ListItem>
                                                          <asp:ListItem>N</asp:ListItem>
                                                            </asp:DropDownList>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select Status" ValidationGroup="save" InitialValue="--Select Status--" ControlToValidate="ddlEmbossingCenter"></asp:RequiredFieldValidator>
                                                         </td>
                                                    <td></td>
                    </tr>
                                                 <tr>
                                                    <td class="auto-style2 form_text">
                                                        <asp:Label ID="label2" runat="server" Text="Description :"></asp:Label>
                                                        
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:TextBox  ID="txtdesc" TextMode="MultiLine" runat="server" ></asp:TextBox>
                                                        <br />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="save" ErrorMessage="Please Enter Description" ControlToValidate="txtdesc"></asp:RequiredFieldValidator>                                                        
                                                   </td>
                                                     <td>
                                                         &nbsp;</td>                                                   
                                                                
                                                </tr>
                                                <tr>
                                                    <td class="auto-style1"></td>
                                                    <td class="form_text">                                                        
                                                        <asp:Button ID="buttonSave" runat="server" Text="Save" class="button" ValidationGroup="save"  OnClick="buttonSave_Click" />
                                                        &nbsp;&nbsp; &nbsp;&nbsp;
                                                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                                                        <%--<input type="reset" id="Reset" value="Reset" class="button" />--%>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="alert">
                                                         Fields are mandatory*.
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

<%--<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder2">

    
</asp:Content>--%>

