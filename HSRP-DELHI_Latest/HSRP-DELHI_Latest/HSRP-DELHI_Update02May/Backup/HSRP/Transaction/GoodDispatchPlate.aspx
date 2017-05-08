<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodDispatchPlate.aspx.cs" Inherits="HSRP.Master.GoodDispatchPlate" %>

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

        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropdownRTOName.ClientID%>").value == "-- Select Embossing Location --") {
                alert("Select Embossing Location");
                document.getElementById("<%=DropdownRTOName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropdownRTOLocation.ClientID%>").value == "-- Select Dispatch Location --") {
                alert("Select Dispatch Location");
                document.getElementById("<%=DropdownRTOLocation.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxBoxNo.ClientID%>").value == "") {
                alert("Please Provide Box No.");
                document.getElementById("<%=textboxBoxNo.ClientID%>").focus();
                return false;
            }
//            if (document.getElementById("<%=textboxDespatchRefNo.ClientID%>").value == "") {
//                alert("Please Provide Dispatch Ref./Tracking No.");
//                document.getElementById("<%=textboxDespatchRefNo.ClientID%>").focus();
//                return false;
//            }
//            if (document.getElementById("<%=textboxBillingAddress.ClientID%>").value == "") {
//                alert("Please Provide Billing Address");
//                document.getElementById("<%=textboxBillingAddress.ClientID%>").focus();
//                return false;
//            }
//            if (document.getElementById("<%=textboxShippingAddress.ClientID%>").value == "") {
//                alert("Please Provide Shipping Address");
//                document.getElementById("<%=textboxShippingAddress.ClientID%>").focus();
//                return false;
//            }
            if (invalidChar(document.getElementById("textboxBoxNo"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxBoxNo").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxDespatchRefNo"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxDespatchRefNo").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxBillingAddress"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxBillingAddress").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textboxShippingAddress"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxShippingAddress").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxRemarks"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxRemarks").focus();
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

    <%--<script type="text/javascript">
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
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px; background-color: #FFFFFF; " align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    <asp:Label ID="LabelFormName" runat="server" Text="Label"></asp:Label>
                </div>
            </legend>
            <div style="margin: 20px;" align="center">
     
   <div>
        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
        <tr>
                <td  class="form_text" style="padding-bottom: 10px">
                    <asp:Label ID="Label1" runat="server" Text=" ">State Name : <span class="alert">* </span></asp:Label></td>
                <td>
                    <asp:DropDownList ID="DropDownListStateName" DataTextField="HSRPStateName"  
                        DataValueField="HSRP_StateID" runat="server" TabIndex="1" AutoPostBack="True" 
                        onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" >
                        <asp:ListItem>--Select State Name--</asp:ListItem>
                    </asp:DropDownList>
                </td>
               
                <td></td>
            
            </tr>
        <tr>
                <td  class="form_text" style="padding-bottom: 10px">
                    <asp:Label ID="LabelStateName" runat="server" Text=" ">Embossing Location : <span class="alert">* </span></asp:Label></td>
                <td>
                    <asp:DropDownList ID="DropdownRTOName" DataTextField="RTOLocationName"  DataValueField="RTOLocationID"
                        runat="server" TabIndex="1" AutoPostBack="True" 
                        onselectedindexchanged="DropdownRTOName_SelectedIndexChanged" >
                        <asp:ListItem>--Select Embossing Location--</asp:ListItem>
                    </asp:DropDownList>
                </td>
               
                <td></td>
            <td  class="form_text" style="padding-bottom: 10px"> <asp:Label ID="LabelRTOLocation" runat="server" Text=" ">Dispatch Location : <span class="alert">* </span></asp:Label> </td>
               <td > 
                   <asp:DropDownList ID="DropdownRTOLocation" DataTextField="RTOLocationName" 
                        DataValueField="RTOLocationID" class="dropdown_css" runat="server" 
                          AutoPostBack="True" style="margin-top: 0px" 
                       onselectedindexchanged="DropdownRTOLocation_SelectedIndexChanged" > 
                        <asp:ListItem>--Select Dispatch Location--</asp:ListItem>
                    </asp:DropDownList>
                </td> 
            </tr>
            <tr><td class="form_text" style="padding-bottom: 10px"> Dispatch Ref./Tracking No. : <span class="alert"> </span></td>
                 &nbsp; &nbsp;
               
                <td> <asp:TextBox ID="textboxDespatchRefNo"   class="form_textbox" runat="server" ></asp:TextBox> </td>
                <td></td>
                 <td class="form_text" style="padding-bottom: 10px"> Box No. : <span class="alert">* </span></td>
                  <td> <asp:TextBox ID="textboxBoxNo"  onkeypress="return "  class="form_textbox" runat="server" ></asp:TextBox> </td>
                
            </tr>
            <tr>
            
                <td  class="form_text" style="padding-bottom: 10px"> Billing Address
                    From : <span class="alert">&nbsp;</span></td>
                <td > <asp:TextBox ID="textboxBillingAddress" class="form_textbox" runat="server" Rows="20" Columns="26" Height="79px"   
                        TabIndex="2" TextMode="MultiLine"></asp:TextBox> </td>
                <td></td>
                <td class="form_text" style="padding-bottom: 10px"> Shipping Address : 
                    <span class="alert">&nbsp;</span></td>
                <td > <asp:TextBox ID="textboxShippingAddress" class="form_textbox" MaxLength="15"  runat="server" TabIndex="1" Rows="20" Columns="26"   Height="79px"  
  
                        TextMode="MultiLine"></asp:TextBox> </td>
                
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
            <td colspan="6"></td>
            </tr>
             <tr>
             <td> &nbsp;</td>
             </tr>
                  <td align="left" colspan="3" align="right">
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label> 
                        </td>
                <td colspan="5" align="right" >
                        <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()"
                            class="button" onclick="buttonUpdate_Click1"   />&nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" class="button"  
                            OnClientClick=" return validate()" onclick="btnSave_Click"
                              />&nbsp;&nbsp;
                    <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                    
                    <input type="reset" class="button" value="Reset" />
                </td>
            </tr>
            <tr>
            <td>
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
