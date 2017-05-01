<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DayWiseProductionAnalysis.aspx.cs" Inherits="HSRP.Transaction.DayWiseProductionAnalysis" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">

        function OrderDatefrom_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDatefrom.getSelectedDate();
            CalendarOrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDatefrom.getSelectedDate();
            OrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnClick() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                CalendarOrderDatefrom.hide();
            }
            else {
                CalendarOrderDatefrom.setSelectedDate(OrderDatefrom.getSelectedDate());
                CalendarOrderDatefrom.show();
            }
        }

        function OrderDatefrom_OnMouseUp() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function OrderDateto_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDateto.getSelectedDate();
            CalendarOrderDateto.setSelectedDate(fromDate);

        }

        function OrderDateto_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDateto.getSelectedDate();
            OrderDateto.setSelectedDate(fromDate);

        }


        function OrderDateto_OnClick() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                CalendarOrderDateto.hide();
            }
            else {
                CalendarOrderDateto.setSelectedDate(OrderDateto.getSelectedDate());
                CalendarOrderDateto.show();
            }
        }



        function OrderDateto_OnMouseUp() {
            if (CalendarOrderDateto.get_popUpShowing()) {
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
                                            <span class="headingmain">Day Wise Production Analysis:</span>
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
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="label1" runat="server" Text="Embossing Center:"></asp:Label>
                                                    </td>
                                                    <td width="30%">
                                                    <asp:DropDownList ID="dropDownListClient" Height="20px" Width="228px" Tabindex="1"
                                                    runat="server" DataTextField="EmbCenterName" AutoPostBack="false"
                                                    DataValueField="NAVEMBID" >
                                                </asp:DropDownList>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="labeluser" runat="server" Text="Operator Name:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  MaxLength="11" TabIndex="1"
                                                            ID="Txtoperatorname" runat="server"></asp:TextBox>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblMachine" runat="server" Text="Machine Type:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  MaxLength="11" TabIndex="2"
                                                            ID="txtmachinename" runat="server"></asp:TextBox>
                                                     
                                                   </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblStart" runat="server" Text="Start Time:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="11"
                                                            ID="txtstarytime" runat="server" ></asp:TextBox>
                                                   </td>

                                                   <%--<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTimein" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                </tr>

                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblEnd" runat="server" Text="End Time:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="txtendTime" runat="server" ></asp:TextBox>
                                                   </td>
                                                      <%--<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txttimeout" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                </tr>

                                                  <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblDuration" runat="server" Text="Duration:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="txtDuration" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>
                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblhundred" runat="server" Text="200*100:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="Txthundred" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>
                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lbltwo" runat="server" Text="285*45:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="Txttwo" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>
                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblthree" runat="server" Text="340*200:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="txtthree" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>
                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblfine" runat="server" Text="500*120:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="Txtfive" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>

                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lbltotal" runat="server" Text="Total:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="Txttotal" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>

                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="Label2" runat="server" Text="Reject:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="Txtreject" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblperhour" runat="server" Text="Per Hours:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="Txtperhour" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>

                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lbldown" runat="server" Text="Down Time:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="TxtDownTime" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>

                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblReason" runat="server" Text="Reason:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="TxtReason" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>

                                                <tr>
                                                     <td style="height: 40px" width="20"> <asp:Label Text="Date:" runat="server" 
                                                        ID="labelDate" Font-Bold="True" 
                                                        ForeColor="Black" Width="60px" /> </td>
                                                   <td style="height: 40px; width: 188px;">
                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                    <ContentTemplate>
                                                                        <ComponentArt:Calendar ID="OrderDatefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    
                                                 ControlType="Picker" PickerCssClass="picker" Height="22px" Width="79px">
                                                                            <ClientEvents>
                                                                                <SelectionChanged EventHandler="OrderDatefrom_OnDateChange" />
                                                                            </ClientEvents>
                                                                        </ComponentArt:Calendar>
                                                                        <img id="calendar_from_button"  alt="" onclick="OrderDatefrom_OnClick()" onmouseup="OrderDatefrom_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                    <tr>
             <td>
                                                    &nbsp;</td>
             <td colspan="6">
                                                    <ComponentArt:Calendar runat="server" ID="CalendarOrderDatefrom" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" 
                                                        NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDatefrom_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                            </tr>
            </td>
                                                </tr>

                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblsheet" runat="server" Text="Sheet No:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="Txtsheetno" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>

                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblCenter" runat="server" Text="Center:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="txtCenter" runat="server" ></asp:TextBox>
                                                   </td>
                                                      
                                                </tr>



                                                <tr>
                                                    <td style="padding-left: 45px" class="form_text" align="center" colspan="2" width="50%">
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
