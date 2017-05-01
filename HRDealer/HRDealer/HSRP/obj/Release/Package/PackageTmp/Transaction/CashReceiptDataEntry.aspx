<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="CashReceiptDataEntry.aspx.cs" Inherits="HSRP.Transaction.CashReceiptDataEntry" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../javascript/bootstrap.min.js" type="text/javascript"></script>
    <script src="../javascript/bootbox.min.js" type="text/javascript"></script>

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
                alert("Please Provide  Date ");
                document.getElementById("<%=DepositDate.ClientID%>").focus();
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
            if (charCode > 31 && (charCode < 96 || charCode > 122) && (charCode < 65 || charCode > 90) && (charCode < 31 || charCode > 33))
                return false;

            return true;
        }



    </script>
    <script type="text/javascript">


        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnSave").hide();


        });

        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }

        $(document).on("click", ".alert1", function (e) {


            var regno = $('#<%=txtEngineNo.ClientID %>').val()
            if (regno == "") {

                bootbox.alert("Please Enter Engine No. and Click On Go Button !", function () {

                });
            }
            else {


                bootbox.confirm("<B>Please Confirm !</b> <BR><i>Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</i>", function (result) {

                    if (result) {
                        $("#save1").hide();
                        $("#ctl00_ContentPlaceHolder1_btnSave").show();
                    }
                });
            }

        });

    </script>



    <fieldset>
        <legend>

            <asp:Label runat="server" Style="margin-left: 10px; height: 75px; margin-top: 50px; font-size: medium; color: Black;" Text="  HSRP Booking Form"></asp:Label>

        </legend>
        <br />

        <table id="Table1" width="80%" align="center" style="font-size: 16px; color: Black; font-family: @Batang; background-color: #fff">

            <tr>

                <td class="form_text" style="height: 22px">Registration No </td>
                <td>
                    <asp:TextBox ID="txtRegNumber" CssClass="text_box" runat="server"></asp:TextBox>
                    <asp:Button ID="btngo" runat="server" Text=" Go " OnClick="btngo_Click" BackColor="Orange" Width="57px" />

                </td>

                 <td align="left">
                    
                    <asp:Button ID="btnupload" runat="server" Text=" Upload Data " OnClick="btnupload_Click" Width="150px"  />

                      
                    </td>

               <%-- <td valign="top">
                    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2" nowrap="nowrap"><strong>Today Summary:</strong></td>
                        </tr>


                        <tr>
                            <td class="input-xlarge" style="width: 186px">Total Collection:</td>
                            <td>
                                <asp:Label ID="lblCount" runat="server">0</asp:Label></td>
                        </tr>
                        <tr>
                            <td class="input-xlarge" style="width: 186px">Utilized Amount:</td>
                            <td>
                                <asp:Label ID="lblCollection" runat="server">0</asp:Label></td>
                        </tr>

                        <tr>
                            <td class="input-xlarge" style="width: 186px">Available Amount:</td>
                            <td>
                                <asp:Label ID="lblAvailableamt" runat="server">0</asp:Label></td>
                        </tr>

                        <tr>
                            <td class="input-xlarge" style="width: 186px">Dealer Name:</td>
                            <td>
                                <asp:Label ID="lbldealername" runat="server" Width="85px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="input-xlarge" style="width: 186px">Dealer Code:</td>
                            <td>
                                <asp:Label ID="lbldealercode" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="input-xlarge" style="width: 186px">Area:</td>
                            <td>
                                <asp:Label ID="lblarea" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="input-xlarge" style="width:186px "  visible="false">Last Bank Deposit Amount:</td>
                            <td>
                                <asp:Label ID="lblaccountbal" runat="server">0</asp:Label></td>
                        </tr>
                        <tr>
                            <td class="input-medium" style="width: 184px" visible="false" nowrap="nowrap">Last Bank Deposit Date:</td>
                            <td>
                                <asp:Label ID="lblLastDepositdate" runat="server">0</asp:Label></td>
                        </tr>


                    </table>
                </td>--%>

            </tr>


            <tr>


                <td class="form_text">Engine No <span style="color: #FF3300">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtEngineNo" runat="server" CssClass="text_box"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                        ControlToValidate="txtEngineNo" ErrorMessage="Please Enter Engine No" ValidationGroup="save"></asp:RequiredFieldValidator>
                </td>
               



            </tr>
            <tr>
                <td class="form_text">Chassis No <span style="color: #FF3300">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtChassisno" runat="server" CssClass="text_box"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txtChassisno" ErrorMessage="Please Enter Chassis No" ValidationGroup="save"></asp:RequiredFieldValidator>
                </td>


            </tr>


            <tr>
                <td class="form_text">Owner Name <span style="color: #FF3300">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtOwnerName"   onkeypress="return ischarKey(event)" MaxLength="50"   runat="server" CssClass="text_box"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtOwnerName" ErrorMessage="Please Enter Owner Name" ValidationGroup="save"></asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr>
                <td class="form_text">Address <span style="color: #FF3300">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="text_box"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                        ControlToValidate="txtAddress" ErrorMessage="Please Enter Address" ValidationGroup="save"></asp:RequiredFieldValidator>
                </td>


            </tr>


            <tr>


                <td class="form_text" style="height: 22px">Email Id</td>
                <td style="height: 22px">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="text_box"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Format" ValidationGroup="save"></asp:RegularExpressionValidator>
                </td>

            </tr>
            <tr>
                <td class="form_text">Mobile No <span style="color: #FF3300">*</span>
                </td>
                <td>
                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="text_box" MaxLength="10" onkeypress="javascript:return isNumber(event)"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Enter MobileNo" ControlToValidate="txtMobileNo" ValidationGroup="save"></asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr>

                <td class="form_text" align="left">Vehicle Type (<span style="color: #FF3300">*</span>)
                </td>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlVehicletype" runat="server" AutoPostBack="True"  CssClass="form_text"
                                OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged">
                                <asp:ListItem Value="-Select Vehicle Type-"></asp:ListItem>
                                <asp:ListItem>SCOOTER</asp:ListItem>
                                <asp:ListItem>MOTOR CYCLE</asp:ListItem>
                                <asp:ListItem>TRACTOR</asp:ListItem>
                                <asp:ListItem>THREE WHEELER</asp:ListItem>
                                <asp:ListItem>LMV</asp:ListItem>
                                <asp:ListItem>LMV(CLASS)</asp:ListItem>
                                <asp:ListItem>MCV/HCV/TRAILERS</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                        ControlToValidate="ddlVehicletype" ValidationGroup="save" ErrorMessage="Please Select Vehicle Type" InitialValue="-Select Vehicle Type-"></asp:RequiredFieldValidator>
              
                  

                </td>


            </tr>

            <tr>

                <td class="form_text">Vehicle Class <span style="color: #FF3300">*</span>
                </td>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlVehicletype" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlVehicleclass" runat="server" CssClass="form_text"
                                AutoPostBack="True"
                                OnSelectedIndexChanged="ddlVehicleclass_SelectedIndexChanged">
                                <asp:ListItem Value="-Select Vehicle Class-">-Select Vehicle Class-</asp:ListItem>
                                <asp:ListItem>Transport</asp:ListItem>
                                <asp:ListItem>Non-Transport</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                        ControlToValidate="ddlVehicleclass" ValidationGroup="save" ErrorMessage="Please Select VehicleClass" InitialValue="-Select Vehicle Class-"></asp:RequiredFieldValidator>
                </td>

            </tr>

            <tr>
                <td class="form_text" style="height: 22px">Ex Showroom Price <span style="color: #FF3300">*</span>
                </td>
                <td style="height: 22px">
                    <asp:TextBox ID="txtexprice" runat="server"  CssClass="text_box"  MaxLength="10"   onkeypress="javascript:return isNumber(event)" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="txtexprice" ValidationGroup="save" ErrorMessage="Please Enter Ex Showroom Price"></asp:RequiredFieldValidator>
                </td>


            </tr>

            <tr>
                <td class="form_text" style="height: 22px">Model <span style="color: #FF3300">*</span>
                </td>
                <td style="height: 22px">
                    <asp:TextBox ID="txtmodel" runat="server" CssClass="text_box"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ControlToValidate="txtmodel" ValidationGroup="save" ErrorMessage="Please Enter Model"></asp:RequiredFieldValidator>
                </td>


            </tr>

            <tr>

                <%--<td>
                <asp:TextBox ID="txtrecno" runat="server" Visible="false" CssClass="text_box"></asp:TextBox>
               
            </td>--%>
            </tr>

            <tr>
                <td class="style4">
                    <%--  class="alert"--%>
                    Date of Insurance <span style="color: #FF3300">*</span>
                </td>
                <td>
                    <table id="Table2" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                        cellpadding="0" border="0">
                        <tr>
                            <td valign="top" onmouseup="DepositDate_OnMouseUp()">
                                <ComponentArt:Calendar ID="DepositDate" runat="server" PickerFormat="Custom"
                                    PickerCustomFormat="dd/MM/yyyy" TabIndex="1"
                                    ControlType="Picker" PickerCssClass="picker" Visible="true">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="DepositDate_OnDateChange" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </td>
                            <td style="font-size: 10px;">&nbsp;
                            </td>
                            <td valign="top">
                                <img id="calendar_from_button" tabindex="2" alt="" onclick="DepositDate_OnClick()"
                                    onmouseup="DepositDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td class="form_text">Amount (<span style="color: #FF3300">*</span>)
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlVehicleclass" EventName="SelectedIndexChanged" />
                        </Triggers>
                        <ContentTemplate>
                           <asp:Label ID="lblAmount" Text="" runat="server"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </td>



            </tr>

             <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr> 

            <tr>
                <td>
                    <a class="alert1" id="save1">Confirm</a>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                        BackColor="Orange" Width="57px" ValidationGroup="save" />
                </td>
                <td>
                    <asp:Button ID="Button3" runat="server" CssClass="button" Height="23px"
                        OnClick="Button3_Click" TabIndex="12" Text="Download Print"
                        Width="153px" Visible="false" />
                    <asp:Button ID="btnDownload" runat="server" Text="Download Receipt"
                        OnClick="btnDownloadReceipt_Click" CssClass="button" Visible="false" />
                </td>
                <td colspan="2">
                    <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                    <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>


                </td>
            </tr>
            
        </table>

    </fieldset>
 


    <table>
        <tr>
            <td>
                <ComponentArt:Calendar runat="server" ID="CalendarDepositDate" AllowMultipleSelection="false"
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
</asp:Content>
