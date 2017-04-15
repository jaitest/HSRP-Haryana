<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="LaserTransferInterState.aspx.cs" Inherits="HSRP.Master.LaserTransferInterState" %>

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

    <script language="javascript" type="text/javascript">

        function validateState() {
         
        }

        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select RTO Name--") {
                alert("Please Select Location");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropDownListTransferLocationName.ClientID%>").value == "--Select Transfer RTO Location--") {
                alert("Please Select Transfer Location");
                document.getElementById("<%=dropDownListTransferLocationName.ClientID%>").focus();
                return false;
            }

            var LocationRTO = document.getElementById("<%=dropDownListClient.ClientID%>").value;
            var TransferLocation = document.getElementById("<%=dropDownListTransferLocationName.ClientID%>").value;
          
            if (LocationRTO == TransferLocation) {
                alert("Same Location Can't Transfer Laser Code");
                document.getElementById("<%=dropDownListTransferLocationName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListPrefix.ClientID%>").value == "-- Select Prefix --") {
                alert("Please Select Prefix Lode");
                document.getElementById("<%=DropDownListPrefix.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").value == "") {
                alert("Please Provide Laser Code From");
                document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxLaserCodeTo.ClientID%>").value == "") {
                alert("Please Provide Laser Code To");
                document.getElementById("<%=textboxLaserCodeTo.ClientID%>").focus();
                return false;
            }


            var LaserFrom = document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").value;
            var LaserTo = document.getElementById("<%=textboxLaserCodeTo.ClientID%>").value;
            var diff = LaserTo - LaserFrom;
            alert(diff);
            if (diff < 0) {
                alert("You are Enter Small value in Laser Code to");
                document.getElementById("<%=textboxLaserCodeTo.ClientID%>").focus();
                return false;
            }



        }
    
    </script>

    <script type="text/javascript">
        function uppercharacter() {
            var char = document.getElementById("textboxPrefixFrom").value;
            alert(document.write(char.toUpperCase());)
        }
    
    </script>

    <script type="text/javascript">
        function OrderDate_OnDateChange(sender, eventArgs) {
            
            var fromDate = OrderDate.getSelectedDate();
            alert(fromDate);
            CalendarOrderDate.setSelectedDate(fromDate);

        }

        function OrderDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDate.getSelectedDate();
            //alert(fromDate);
            var toDate = new Date()
           // alert(currentTime);
            OrderDate.setSelectedDate(fromDate);
            if (fromDate > toDate) {
                alert("Check Selected Date");
                OrderDate.setSelectedDate(toDate);
                return false; 
            }

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
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px; background-color: #FFFFFF; " align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black; width: 314px;">
                    <asp:Label ID="LabelFormName" runat="server" Text="Label"></asp:Label>
                </div>
            </legend>
            <div style="margin: 20px;" align="left">
     
   <div>
        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

      <%--  <asp:ListView ID="IDLIstView" runat="server">
        <ItemTemplate>
         <div id="idDivFirst" style="border:1px solid;color: black; width: 120px;" >
        <asp:Label ID="lblRefNo" Text=<%#Eval("hsrpRecordID") %>   CssClass="linklabel"  runat="server"></asp:Label>
        </div>  
        <div id="Div1" style="border:1px solid;color: black; width: 180px;" ">
        <asp:Label ID="Label2" Text=<%#Eval("OwnerName") %>  CssClass="linklabel"  runat="server"></asp:Label>
        </div>  
        </ItemTemplate>
        </asp:ListView>

            <asp:DataPager ID="DataPager1" PageSize="10" runat="server" PagedControlID="IDLIstView">
            <Fields>
            <asp:NumericPagerField ButtonCount="5" />
            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" />

            </Fields>
            </asp:DataPager>--%>
            <tr id="TRRTOHide" runat="server">
                                    <td colspan="5" > 
                                          <asp:Label ID="dataLabellbl" class="headingmain" runat="server"  >Allowed RTO's :</asp:Label> 
                                          <asp:Label ID="lblRTOCode" class="form_Label_Repeter"  runat="server">Allowed RTO's : </asp:Label> 
                                    </td> 
            </tr>
            
            <tr>
            <td valign="middle" class="form_text" nowrap="nowrap">
              <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> <span class="alert"> * </span>&nbsp;&nbsp;
             </td>
              <td valign="middle" colspan="4" >
               <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                 runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" Width="180px"
                      onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" >
              </asp:DropDownList>
            </td>
            </tr> 
             
            <tr>
                 <td class="form_text" style="padding-bottom: 10px">  Location Name : <span class="alert">* </span></td>
                 <td> <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false" Width="180px" runat="server" DataTextField="RTOLocationName" 
                   DataValueField="RTOLocationID" ></asp:DropDownList>

                <td></td>
                 <td class="form_text" style="padding-bottom: 10px"> Transfer Location Name : <span class="alert">* </span></td>
                <td> <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListTransferLocationName" CausesValidation="false" Width="220px" runat="server" DataTextField="RTOLocationName" 
                   DataValueField="RTOLocationID" ></asp:DropDownList>
                </td>
            </tr>
            <tr>
            
                <td  class="form_text" style="padding-bottom: 10px"> Prefix Laser Code 
                    From : <span class="alert">* </span></td>
                <td > <asp:DropDownList ID="DropDownListPrefix" Width="140" runat="server" 
                        DataTextField="Prefix" DataValueField="Prefix"
                        AutoPostBack="True"  > </asp:DropDownList> </td>
                <td></td>
                <td class="form_text" style="padding-bottom: 10px"> Laser Code From : <span class="alert">* </span></td>
                <td > <asp:TextBox ID="textboxLaserCodeFrom" class="form_textbox" MaxLength="15" onkeypress="return isNumberKey(event)" runat="server" TabIndex="1"></asp:TextBox> </td>
                
            </tr>
            <tr>
             <td  class="form_text" style="padding-bottom: 10px">Product : <span class="alert">* </span> </td>
                <td > <asp:DropDownList ID="DropDownListProduct" Width="140" runat="server" 
                        DataTextField="ProductCode"  DataValueField="ProductID"
                        AutoPostBack="True"  > </asp:DropDownList>
                </td> 
                <td></td>
                <td class="form_text" style="padding-bottom: 10px"> Laser Code To : <span class="alert">* </span></td>
                <td > <asp:TextBox ID="textboxLaserCodeTo" class="form_textbox" MaxLength="15"  onkeypress="return isNumberKey(event)" runat="server" TabIndex="1"></asp:TextBox> </td>
            </tr>
             
             
            <tr>
                 
                <td class="form_text" style="padding-bottom: 10px"> Remarks : </td> 
                <td colspan="4" valign="top" onmouseup="HSRPAuthDate_OnMouseUp()">
                                                                <asp:TextBox ID="textboxRemarks" class="form_textbox" runat="server" Columns="25" Rows="15" Height="85px" Width="600px"
                                                                    TabIndex="6" TextMode="MultiLine"></asp:TextBox> 
                                                                <br />
                                                                <%--<ComponentArt:Calendar ID="HSRPAuthDate" runat="server" Visible="false" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>--%>
                                                            &nbsp;&nbsp;
                                                                <img id="ImgPollution" alt="" class="calendar_button" style="visibility:hidden" 
                                                                    onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()" 
                                                                    src="../images/btn_calendar.gif" tabindex="4" /></td>
            </tr>
            
            <tr>
            <td colspan="5"></td>
            </tr>
             <tr>
             <td> <asp:Label ID="LabelUpdated" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                         <asp:Label ID="lblTotalRecord" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label></td>
             </tr>
                  <td align="left" colspan="3" align="right">
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label> 
                        </td>
                <td colspan="2" align="right" >
                        <%--<asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validateState()()"
                            class="button" onclick="buttonUpdate_Click" />--%>&nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" class="button"  OnClientClick=" return validate()"
                            onclick="btnSave_Click" />&nbsp;&nbsp;
                    <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                    
                    <input type="reset" class="button" value="Reset" />
                </td>
            </tr>
            <tr>
            <td>
                          <asp:Label ID="lblExist" runat="server" ForeColor="Red" 
                    Font-Size="18px"></asp:Label> 
                <asp:TextBox ID="TextBoxLaserNoError" runat="server" Rows="10" Columns="17" TextMode="MultiLine"></asp:TextBox>
</td>
            </tr>

            
           <tr>
                                                <td colspan="5">
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
                                                <td colspan="5">
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
    </div> 
    </div>
        </fieldset>
    </div>
    </form>
</body>
</html>
