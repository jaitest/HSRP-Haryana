<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockEntryNew.aspx.cs" Inherits="HSRP.Master.StockEntryNew" %>

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

       
    

        function validateNumeric() {
            
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
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
</script>
    <script type="text/javascript">
        function validate() { 
            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownRTOLocation.ClientID%>").value == "--Select RTO Name--") {
                alert("Select RTO Name");
                document.getElementById("<%=dropDownRTOLocation.ClientID%>").focus();
                return false;
            }

            var a1 = document.getElementById("<%=txt200_100BlnkPlate.ClientID%>").value;
            var a2 = document.getElementById("<%=txt200_100EmbosedPlate.ClientID%>").value;
            var a3 = document.getElementById("<%=txt200_100ClosedPlate.ClientID%>").value;

            var b1 = document.getElementById("<%=txt200_100BlnkPlateYellow.ClientID%>").value;
            var b2 = document.getElementById("<%=txt200_100EmbosedPlateYellow.ClientID%>").value;
            var b3 = document.getElementById("<%=txt200_100ClosedPlateYellow.ClientID%>").value;

            var c1 = document.getElementById("<%=txt285_45BlnkPlateWhite.ClientID%>").value;
            var c2 = document.getElementById("<%=txt285_45EmbosedPlateWhite.ClientID%>").value;
            var c3 = document.getElementById("<%=txt285_45ClosedPlateWhite.ClientID%>").value;

            var d1 = document.getElementById("<%=txt285_45BlnkPlateYellow.ClientID%>").value;
            var d2 = document.getElementById("<%=txt285_45EmbosedPlateYellow.ClientID%>").value;
            var d3 = document.getElementById("<%=txt285_45ClosedPlateYellow.ClientID%>").value;

            var e1 = document.getElementById("<%=txt300_200BlnkPlateWhite.ClientID%>").value;
            var e2 = document.getElementById("<%=txt300_200EmbosedPlateWhite.ClientID%>").value;
            var e3 = document.getElementById("<%=txt300_200ClosedPlateWhite.ClientID%>").value;

            var f1 = document.getElementById("<%=txt300_200BlnkPlateYellow.ClientID%>").value;
            var f2 = document.getElementById("<%=txt300_200EmbosedPlateYellow.ClientID%>").value;
            var f3 = document.getElementById("<%=txt300_200ClosedPlateYellow.ClientID%>").value;

            var g1 = document.getElementById("<%=txt500_120BlnkPlateWhite.ClientID%>").value;
            var g2 = document.getElementById("<%=txt500_120EmbosedPlateWhite.ClientID%>").value;
            var g3 = document.getElementById("<%=txt500_120ClosedPlateWhite.ClientID%>").value;

            var h1 = document.getElementById("<%=txt500_120BlnkPlateYellow.ClientID%>").value;
            var h2 = document.getElementById("<%=txt500_120EmbosedPlateYellow.ClientID%>").value;
            var h3 = document.getElementById("<%=txt500_120ClosedPlateYellow.ClientID%>").value;




            if (a1 == "" && a2 == "" && a3 == "" && b1 == "" && b2 == "" && b3 == "" && c1 == "" && c2 == "" && c3 == "" && d1 == "" && d2 == "" && d3 == ""  && e1 == "" && e2 == "" && e3 == "" && f1 == "" && f2 == "" && f3 == "" && g1 == "" && g2 == "" && g3 == "" && h1 == "" && h2 == "" && h3 == "") {
                alert("Please Enter Plate Value");
                //document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }

            if (a1 == "0" && a2 == "0" && a3 == "0" && b1 == "0" && b2 == "0" && b3 == "0" && c1 == "0" && c2 == "0" && c3 == "0" && d1 == "0" && d2 == "0" && d3 == "0" && e1 == "0" && e2 == "0" && e3 == "0" && f1 == "0" && f2 == "0" && f3 == "0" && g1 == "0" && g2 == "0" && g3 == "0" && h1 == "0" && h2 == "0" && h3 == "0") {
                alert("All Zero (0) Not Allow !!");
                //document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            } 


        }
    
    </script>


    <%--<script type="text/javascript">
        function uppercharacter() {
            var char = document.getElementById("textboxPrefixFrom").value;
            alert(document.write(char.toUpperCase());)
        }
    
    </script>--%>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div style="margin: 20px; background-color: #FFFFFF; " align="left">
          <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    <asp:Label ID="LabelFormName" runat="server" Text="Label"></asp:Label>
                </div>
            </legend>
            <div style="margin: 20px;" align="left">
     
   <div>


   <table  >
   <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged"  >
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> 
                                    </td>
                                    <td valign="middle" class="form_text">
                                       <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate> 
                                             <asp:DropDownList AutoPostBack="true" Visible="true" ID="dropDownRTOLocation" 
                                                    CausesValidation="false" Width="160px"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID" 
                                                    onselectedindexchanged="dropDownRTOLocation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownRTOLocation" /> 
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td valign="middle" class="form_text"></td>
                                    <td valign="middle" class="form_text"><asp:Label ID="lblUserNamelbl" runat="server" Text="User Name :" ></asp:Label></td>
                                    
                                    <td valign="middle" class="form_text"><asp:Label ID="lblUserName" runat="server"></asp:Label></td>
                                    <td valign="middle" class="form_text"><asp:Label ID="lblStockDate" runat="server" Text="Stock Date :"></asp:Label></td>
                                     <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left">
                                                                <componentart:calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker" Enabled="False">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </componentart:calendar>
                                                            </td>
                                                            <td valign="top" align="left">
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                    onmouseup="OrderDate_OnMouseUp()" class="calendar_button" 
                                                                    src="../images/btn_calendar.gif" style="display: none" />
                                                            </td>
                                   
            </tr>
   </table>

        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
        


        <tr> 
        <td class="form_text" style="padding-bottom: 10px">Product</td>
        <td ></td>
        <td class="form_text" style="padding-bottom: 10px">Blank Plate</td>
        <td class="form_text" style="padding-bottom: 10px">Embossed Plate</td>
        <td class="form_text" style="padding-bottom: 10px">Rejected Plate</td>
        </tr>
        
            <tr>
                <td class="form_text" style="padding-bottom: 10px"> 200X100 MM-WHITE </td> &nbsp; &nbsp;
                 <td></td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt200_100BlnkPlate" class="form_textbox"  onkeypress="return isNumberKey(event)" ViewStateMode="Disabled" Text=""  Width="70px" runat="server" MaxLength="8" ></asp:TextBox>   </td> 
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt200_100EmbosedPlate" class="form_textbox"  onkeypress="return isNumberKey(event)"  Text=""  Width="70px" runat="server" MaxLength="8" ></asp:TextBox>   </td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt200_100ClosedPlate" class="form_textbox"  onkeypress="return isNumberKey(event)"   Text="" Width="70px" runat="server"  MaxLength="8"></asp:TextBox> </td>
            </tr>
            <tr>
                <td class="form_text" style="padding-bottom: 10px"> 200X100 MM-YELLOW </td> &nbsp; &nbsp;
                 <td></td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt200_100BlnkPlateYellow" class="form_textbox"  onkeypress="return isNumberKey(event)" Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td> 
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt200_100EmbosedPlateYellow" class="form_textbox"  onkeypress="return isNumberKey(event)" Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt200_100ClosedPlateYellow" class="form_textbox"  onkeypress="return isNumberKey(event)" Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td>
            </tr>
             <tr>
                <td class="form_text" style="padding-bottom: 10px"> 285X45 MM-WHITE</td> &nbsp; &nbsp;
                 <td></td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt285_45BlnkPlateWhite" class="form_textbox"  onkeypress="return isNumberKey(event)" Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td> 
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt285_45EmbosedPlateWhite" class="form_textbox"  onkeypress="return isNumberKey(event)" Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt285_45ClosedPlateWhite" class="form_textbox" onkeypress="return isNumberKey(event)"  Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td>
            </tr>
             <tr>
                <td class="form_text" style="padding-bottom: 10px"> 285X45 MM-YELLOW </td> &nbsp; &nbsp;
                 <td></td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt285_45BlnkPlateYellow" class="form_textbox"  onkeypress="return isNumberKey(event)" Width="70px"  Text="" MaxLength="8" runat="server" ></asp:TextBox> </td> 
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt285_45EmbosedPlateYellow" class="form_textbox"   onkeypress="return isNumberKey(event)" Width="70px" Text="" MaxLength="8" runat="server" ></asp:TextBox> </td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt285_45ClosedPlateYellow" class="form_textbox"   onkeypress="return isNumberKey(event)" Width="70px" Text=""  MaxLength="8" runat="server" ></asp:TextBox> </td>
            </tr>
             <tr>
                <td class="form_text" style="padding-bottom: 10px"> 340X200 MM-WHITE </td> &nbsp; &nbsp;
                 <td></td>
                 <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt300_200BlnkPlateWhite" class="form_textbox"  onkeypress="return isNumberKey(event)" Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td> 
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt300_200EmbosedPlateWhite" class="form_textbox"  onkeypress="return isNumberKey(event)" Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt300_200ClosedPlateWhite" class="form_textbox" onkeypress="return isNumberKey(event)"  Text=""  MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td>
            </tr>
             <tr>
                <td class="form_text" style="padding-bottom: 10px"> 340X200 MM-YELLOW </td> &nbsp; &nbsp;
                 <td></td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt300_200BlnkPlateYellow" class="form_textbox" onkeypress="return isNumberKey(event)"  Text="" MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td> 
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt300_200EmbosedPlateYellow" class="form_textbox"  onkeypress="return isNumberKey(event)"  Text="" MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt300_200ClosedPlateYellow" class="form_textbox"  onkeypress="return isNumberKey(event)"  Text="" MaxLength="8" Width="70px" runat="server" ></asp:TextBox> </td>
            </tr>
             <tr>
                <td class="form_text" style="padding-bottom: 10px"> 500X120 MM-WHITE</td> &nbsp; &nbsp;
                 <td></td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt500_120BlnkPlateWhite" class="form_textbox"  MaxLength="8" onkeypress="return isNumberKey(event)"  Text="" Width="70px" runat="server" ></asp:TextBox> </td> 
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt500_120EmbosedPlateWhite" class="form_textbox"  MaxLength="8"  onkeypress="return isNumberKey(event)"  Text="" Width="70px" runat="server" ></asp:TextBox> </td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt500_120ClosedPlateWhite" class="form_textbox"  MaxLength="8"  onkeypress="return isNumberKey(event)"  Text=""  Width="70px" runat="server" ></asp:TextBox> </td>
            </tr>
             <tr>
                <td class="form_text" style="padding-bottom: 10px"> 500X120 MM-YELLOW</td> &nbsp; &nbsp;
                 <td></td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt500_120BlnkPlateYellow" class="form_textbox"  MaxLength="8" Text="" onkeypress="return isNumberKey(event)" Width="70px" runat="server" ></asp:TextBox> </td> 
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt500_120EmbosedPlateYellow" class="form_textbox"  MaxLength="8" Text="" onkeypress="return isNumberKey(event)"  Width="70px" runat="server" ></asp:TextBox> </td>
                <td class="form_text" style="padding-bottom: 10px"> <asp:TextBox ID="txt500_120ClosedPlateYellow" class="form_textbox"  MaxLength="8" Text="" onkeypress="return isNumberKey(event)"  Width="70px" runat="server" ></asp:TextBox> </td>
            </tr>
            
            <tr>
            <td colspan="5"></td>
            </tr>
             <tr>
             <td> 
                         <asp:Label ID="lblTotalRecord" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                 </td>
             </tr>
                  <td align="left" colspan="3" align="right">
                         <asp:Label ID="lblSuccess" runat="server" 
                             Font-Bold="True" ForeColor="Blue"></asp:Label>
                          <asp:Label ID="lblExist" Visible="false" runat="server" ForeColor="Red" 
                    Font-Size="18px"></asp:Label> 
                 </td>
                <td colspan="2" align="right" >
                      
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" 
                        class="button"  OnClientClick=" return validate()"
                            onclick="btnSave_Click" Visible="False" />&nbsp;&nbsp;
                    <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                    
                    <input type="reset" id="btnReset" runat="server" class="button" value="Reset" />
                </td>
            </tr>
            <tr>
            <td>;</td>
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
                                                    </ComponentArt:Calendar></td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    &nbsp;</td>
                                            </tr> 
        </table>
    </div> 
    </div>
        </fieldset>
    </div>
    </form>
</body>
</html>
