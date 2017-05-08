<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TransportTracking.aspx.cs" Inherits="HSRP.Transaction.TransportTracking" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
        <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center img
        {
            height: 128px;
            width: 128px;
        }
    </style>
    </asp:Content>
    

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

<%--    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal">
                <div class="center">
                    <img alt="" src="loader.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
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
                                            <%--<span class="headingmain"> Delhi Report</span>--%>
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
                                                        <asp:Button ID="btncollection" runat="server" Text="HSRP Daily Cash Collection Report" Font-Bold="true" class="button" OnClick="btncollection_Click" />
                                                    </td>
                                    <td valign="middle">
                                                        &nbsp;</td>

                                            
                                     <td valign="middle" class="form_text">
                                       
                                    </td>
                                                       <td valign="middle" class="form_text" nowrap="nowrap" style="text-align: right;">
                                       <asp:Label ID="lblusername" runat="server" ForeColor="black" Font-Size="15px" Text="Supervisor Name"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; :</td>
                                                       <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label ID="lbluname" runat="server" ForeColor="black" Font-Size="15px"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                 <tr>
                                                    
                                                    <td style="width: 13%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                                        <asp:Button ID="btnmonthwise" runat="server" Text="HSRP Month Wise Report" Font-Bold="true" class="button" OnClick="btnmonthwise_Click" />
                                                    </td>
                                    <td valign="middle">
                                                        &nbsp;</td>

                                            
                                     <td valign="middle" class="form_text">
                                       
                                    </td>
                                                      
                                                </tr>
                                                <tr>
                                                    <td></td>
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
                                                     <td  Visible="false" class="form_text" nowrap="nowrap"><asp:Button ID="btnaffixation" runat="server" Text="HSRP Affixation Report" class="button" Font-Bold="true" OnClick="btnaffixation_Click"  />
                                                     </td>
                                    <td valign="top"   onmouseup="OrderDate_OnMouseUp()" align="left">
                                        &nbsp;</td>
                                                           
                                                           
                                                       <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;</td>
                                                       <td valign="middle" class="form_text" nowrap="nowrap" style="text-align: right;">
                                        <asp:Label ID="lblmobno" runat="server" ForeColor="black" Font-Size="15px" Text="Supervisor Mobile No"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; :</td>
                                                       <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label ID="lblmname" runat="server" ForeColor="black" Font-Size="15px"></asp:Label></td>

                                                  <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTimein" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                 
                                                 <%-- <asp:RegularExpressionValidator ID="revTime" runat="server" ControlToValidate="txtTimein" ErrorMessage="hh:mi" SetFocusOnError="True" ValidationExpression="^(23):(00)|([01][0-9]|2[0-3]):([0-5][0-9])$" ValidationGroup="MyGroup"></asp:RegularExpressionValidator>--%>


                                                </tr>
                                                <tr>
                                                    <td></td>
                                                </tr>
                                                <tr>
                    <td class="form_text"> 
                                                   
                                                        <asp:Button ID="btnmlo" runat="server" Text="HSRP Authority Order Closed Report" class="button" Font-Bold="true" OnClick="btnmlo_Click"  />
                                                         </td>
                    <td valign="middle" class="Label_user_batch" style="width: 165px">
                                                   
                                                        &nbsp;</td>
                                                       <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;</td>
                                                       <td valign="middle" class="form_text" style="text-align: right;" nowrap="nowrap">
                                        <asp:Label ID="lblemail" runat="server" ForeColor="black" Font-Size="15px" Text="Company Email Id"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; :</td>
                                                       <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label ID="lblename" runat="server" ForeColor="black" Font-Size="15px"></asp:Label></td>
                    </tr>

                                                 <tr>
                                                    <td></td>
                                                </tr>
                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Button ID="buttonSave" runat="server" Text="HSRP Daily Cash Collection With Tax and Royalty Report" class="button" Font-Bold="true" OnClick="buttonSave_Click"  />
                                                     </td>
                                                    <td style="width: 13%">
                                                        <%--<asp:RegularExpressionValidator ValidationExpression="^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$" ID="EndTimeValidator" runat="server"  ErrorMessage="You Must Supply an END Time in Correct format"  ControlToValidate="txttimeout"></asp:RegularExpressionValidator>--%>
                                                   </td>
                                                        <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;</td>
                                                        <td valign="middle" class="form_text" style="text-align: right;" nowrap="nowrap">
                                        <asp:Label ID="lbladdress" runat="server" ForeColor="black" Font-Size="15px" Text="Affixation Center Address"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; :</td>
                                                        <td valign="middle" style="width: 0px;" class="form_text" nowrap="nowrap">
                                        <asp:Label ID="lblaname" runat="server" ForeColor="black" Font-Size="15px"></asp:Label></td>

                                                      <%--<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txttimeout" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                     
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 45px" class="form_text" align="center" colspan="2">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        &nbsp;&nbsp;
                                                        &nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                  <td width="20%" class="form_text">
                                                        <asp:Button ID="btndealer" runat="server" Text="Dealer Report" class="button" Font-Bold="true" OnClick="btndealer_Click" />
                                                     </td>
                                                                                                        <td style="width: 13%">
                                                        <%--<asp:RegularExpressionValidator ValidationExpression="^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$" ID="EndTimeValidator" runat="server"  ErrorMessage="You Must Supply an END Time in Correct format"  ControlToValidate="txttimeout"></asp:RegularExpressionValidator>--%>
                                                   </td>
                                                        <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;</td>
                                                    <td valign="middle" class="form_text" style="text-align: right;" nowrap="nowrap">
                                        <asp:Label ID="lblPCA" runat="server" ForeColor="black" Font-Size="15px" Text="Production Center Address"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; :</td>
                                                        <td valign="middle" style="width: 0px;" class="form_text" nowrap="nowrap">
                                        <asp:Label ID="lblProductionCenterAddress" runat="server" ForeColor="black" Font-Size="15px"></asp:Label></td>
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
        <div style="text-align: right;margin-top: 30px; margin-right: 7px;">
         <asp:Panel ID="Panel1" Visible="false" runat="server" style="text-align: right; margin-right: 388px;">
         <span style="color: black;">Your Message : </span><asp:TextBox runat="server" ID="TextBox1" Text="" TextMode="MultiLine" style="margin: 0px; height: 192px; width: 585px; margin-top: 12px;"></asp:TextBox><br /><br />
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ValidationGroup="send" ErrorMessage="Please Enter Your Message"></asp:RequiredFieldValidator><br /><br />
             <span style="color: black;">File Attachment : </span><asp:FileUpload ID="FileUpload1" runat="server"  /><br />
             <asp:Button ID="Button1" runat="server" ValidationGroup="send" Text="Send" OnClick="Button1_Click" />
         </asp:Panel>  
            <asp:Button ID="btntextbox" runat="server" Text="Suggestions/Observation/Complaint by MLO" OnClick="btntextbox_Click"/>
            &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            <asp:Button ID="btncomplaint" runat="server" Text="Complaint Register" OnClick="btncomplaint_Click" />  
<%--                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <div align="Right" style="padding-right: 51px;">
                
              <%--  <h1>
                    Click the button to see the UpdateProgress!</h1>
                <asp:Button ID="Button2" Text="Submit" runat="server" OnClick="Button1_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
            
            
            <%--<asp:Button ID="btnObservation" runat="server" Text="Observation/Suggestion" OnClick="btnObservation_Click" />--%>
            <%--<asp:Button runat="server" ID="btnObservation" class="button"  Text="Observation/Suggestion" OnClick="btnObservation_Click"/>--%>
            
            
            
        </div>
    </div>
    <div>
        <asp:HiddenField ID="hdnRtoLocationName" runat="server" />
    </div>
</asp:Content>
