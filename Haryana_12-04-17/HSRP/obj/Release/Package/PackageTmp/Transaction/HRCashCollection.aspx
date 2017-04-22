<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="HRCashCollection.aspx.cs" Inherits="HSRP.Transaction.HRCashCollection" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    
    
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../javascript/bootstrap.min.js" type="text/javascript"></script>
    <script src="../javascript/bootbox.min.js" type="text/javascript"></script>  
    
    <script type="text/javascript">
       
        
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnSave").hide();


        });

        $(document).on("click", ".alert1", function (e)
        {
            var regno = $('#<%=txtRegNo.ClientID %>').val()
            var txt_Ac = $("#" + '<%= HiddenField1.ClientID %>').val();

            if (regno == "")
            {

                bootbox.alert("Please Enter Registration No. and Click On Go Button !", function () {

                });
            }
            else if (txt_Ac != "")
            {
                if ($('#<%=txtAC.ClientID %>').val() == "")
                {
                    bootbox.alert("Please Enter AC No. and Click On Go Button !", function () {

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
            }
            else
            {         

                bootbox.confirm("<B>Please Confirm !</b> <BR><i>Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</i>", function (result)
                {

                        if (result) {
                            $("#save1").hide();
                            $("#ctl00_ContentPlaceHolder1_btnSave").show();
                        }
                    });
                }
            
        });
       
    </script>
    <%--<script type="text/javascript">
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
    </script>--%>
    
<marquee class="mar1" direction="left" >Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</marquee>
    
      <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                  Cash Collection  </div>
            </legend>
<table width="95%" border="0" cellspacing="0" cellpadding="0" align="center">
  <tr>
    <td width="900"><table id="Table1" width="100%" align="center" style="font-size: 16px; color:Black; font-family:Batang; background-color:#fff" >
      <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>
            <asp:HiddenField ID="HiddenField1" ClientIDMode="Static" runat="server" />
          </td>
      </tr>
      <tr>
        <td> Registration No. </td>
        <td valign="top"><asp:TextBox ID="txtRegNo" runat="server" CssClass="form_textbox2"></asp:TextBox>   </td>
        <td> <asp:Button ID="btnGo" runat="server" Text="  Go  " OnClick="btnGo_Click"         CssClass="button" Width="132px"/>   </td>
        <td><asp:Button ID="BtnReset" runat="server" Text=" Reset " OnClick="btnReset_Click"         CssClass="button" Width="132px"/>  </td>
      </tr>
        <tr>
              <td>  <asp:Label ID="lblAC" runat="server" Text="AC" Visible="false"></asp:Label></td>
            <td>  <asp:TextBox ID="txtAC" runat="server" Visible="false" CssClass="text_box" MaxLength="10"></asp:TextBox><br />     </td>
             <td> <asp:Label ID="lblACError" runat="server"  Text="" ForeColor="Red" Visible="false" ></asp:Label></td>
            <td></td>
        </tr>
      <tr>
        <td class="form_text"> Authorization Number </td>
        <td><asp:Label ID="lblAuthNo" runat="server" Text=""></asp:Label></td>
        <td class="form_text"> Authorization Date </td>
        <td><asp:Label ID="lblAuthDate" runat="server" Text=""></asp:Label></td>
      </tr>
      <tr>
        <td class="form_text"> Registration No </td>
        <td><asp:Label ID="lblRegNo" runat="server" Text=""></asp:Label></td>
        <td class="form_text"> RTO Location Name </td>
        <td><asp:Label
                    ID="lblRTOName" runat="server" Text=""></asp:Label></td>
      </tr>
      <tr>
        <td class="form_text"> Owner Name </td>
        <td><asp:Label ID="lblOwnerName" runat="server" Text=""></asp:Label></td>
        <td class="form_text"> Owner Email </td>
        <td><asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></td>
      </tr>
      <tr>
        <td class="form_text"> Address </td>
        <td colspan="3"><asp:Label ID="lblAddress" runat="server" Text=""></asp:Label></td>
      </tr>
      <tr>
        <td class="form_text"> Vehicle Type </td>
        <td><asp:Label ID="lblVehicleType" runat="server" Text=""></asp:Label></td>
        <td class="form_text"> Transaction Type </td>
        <td><asp:Label ID="lblTransactionType" runat="server" Text=""></asp:Label></td>
      </tr>
      <tr>
        <td class="form_text"> Vehicle Class Type </td>
        <td><asp:Label ID="lblVehicleClassType" runat="server" Text=""></asp:Label></td>
        <td class="form_text"> Mobile No
          *</td>
         <%-- ^[0-9]{10}$--%>
        <td><asp:TextBox ID="txtMobileno"  runat="server" CssClass="text_box" MaxLength="10" 
                    ValidationGroup="Mobile" ></asp:TextBox>
          <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="txtMobileno"  ValidationExpression="^[0-9]*$" 
                    ErrorMessage="Invalid Value"></asp:RegularExpressionValidator>
          <br />

          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ControlToValidate="txtMobileno" ErrorMessage="Please Enter Mobile No"   ValidationGroup="Mobile"></asp:RequiredFieldValidator></td>
      </tr>
      <tr>
        <td class="form_text"> Manufacturer Name </td>
        <td><asp:Label ID="lblMfgName" runat="server" Text=""></asp:Label></td>
        <td class="form_text"> Model Name </td>
        <td><asp:Label ID="lblModelName" runat="server" Text=""></asp:Label></td>
      </tr>
      <tr>
        <td class="form_text"> Engine No </td>
        <td><asp:Label ID="lblEngineNo" runat="server" Text=""></asp:Label></td>
        <td class="form_text"> Chassis No </td>
        <td><asp:Label ID="lblChasisNo" runat="server" Text=""></asp:Label></td>
      </tr>
      <tr>
        <td class="form_text"> Amount </td>
        <td><asp:Label ID="lblAmount" runat="server" Text=""></asp:Label></td>
          <td></td>
        <%--<td class="form_text"> Registration Date </td>
        <td class="form_text" onmouseup="OrderDate_OnMouseUp()" >
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>           
            <img id="calendar_from_button" alt="" onclick="OrderDate_OnClick()" onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../../images/btn_calendar.gif" />
                                                            </td>--%>
          
          
      </tr>
      <tr>
        <td class="form_text"> Third Sticker </td>
        <td><asp:CheckBox ID="bln3rdSticker" runat="server" Enabled="False" /></td>
        <td class="form_text"> VIP </td>
        <td><asp:CheckBox ID="blnVIP" runat="server" /></td>
      </tr>
      <tr>
        <td class="form_text"> Remarks </td>
        <td><asp:TextBox ID="remarks" runat="server" Height="66px" TextMode="MultiLine" Width="241px"></asp:TextBox></td>

          <td colspan="2"> &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>   <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label></td>
        <%-- <td>
            Affixation Center Name :<span style="color:Red">*</span>
            </td>--%>
        <%-- <asp:DropDownList ID="ddlaffixation" runat="server" Height="25px" 
                                     Width="187px" DataTextField="AffixCenterDesc" 
                                    DataValueField="Rto_Id" Visible="False">
                                </asp:DropDownList>--%>
      </tr>
      <tr>
        <td><a class="alert1" id="save1">Confirm</a>   <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"  BackColor="Orange" Width="57px" ValidationGroup="Mobile"    /></td>
        <td><asp:Button ID="Button3" runat="server" CssClass="button" Height="23px"   onclick="Button3_Click" TabIndex="12" Text="Epson Print" Width="102px" Visible="False" />  </td>
        <td ><asp:Button ID="btnDownload" runat="server" Text="Download Receipt"  OnClick="btnDownloadReceipt_Click" CssClass="button" Visible="false" Width="136px" /></td>
         <td></td>
      </tr>

       <%-- <ComponentArt:Calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                                                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                                        ImagesBaseUrl="../../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>--%>
      <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>      
       
      </tr>
      <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
      </tr>
      <tr>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
      </tr>
    </table></td>
    <td valign="top"><table width="80%" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr>
        <td colspan="2" nowrap="nowrap"><strong>Today Summary:</strong></td>
      </tr>
      <tr>
        <td class="input-medium" style="width: 184px">Today Booking Count:</td>
        <td><asp:Label ID="lblCount" runat="server">0</asp:Label></td>
      </tr>
      <tr>
        <td class="input-medium" style="width: 184px">Today Collection :</td>
        <td><asp:Label ID="lblCollection" runat="server">0</asp:Label></td>
      </tr>
      <tr>
        <td class="input-medium" style="width: 184px"nowrap="nowrap">Last Bank Deposit Date:</td>
        <td><asp:Label ID="lblLastDepositdate" runat="server">0</asp:Label></td>
      </tr>
      <tr>
        <td class="input-medium" style="width: 184px" nowrap="nowrap">Last Bank Deposit Amount :</td>
        <td><asp:Label ID="lblLastAmont" runat="server">0</asp:Label></td>
      </tr>
      <tr>
        <td class="input-medium" style="width: 184px">&nbsp;</td>
        <td>&nbsp;</td>
      </tr>
    </table></td>
  </tr>
</table>
<br />
            <br />
    </fieldset>
</asp:Content>
