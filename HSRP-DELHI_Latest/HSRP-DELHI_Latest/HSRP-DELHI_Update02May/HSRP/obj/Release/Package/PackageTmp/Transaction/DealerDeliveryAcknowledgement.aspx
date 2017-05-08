<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DealerDeliveryAcknowledgement.aspx.cs" Inherits="HSRP.Transaction.DealerDeliveryAcknowledgement" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    
 <script type="text/javascript">


     function editAssignLaser(i) {

         googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCode.aspx?Mode=Edit&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
         googlewin.onclose = function () {
             //window.location.href = "AssignLaserCode.aspx";
             return true;
         }
     }

     function editMakeLaserFree(i) { // Define This function of Send Assign Laser ID 
         // alert("MakeLaserFree" + i);

         googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserCodeEmbossing.aspx?Mode=Embossing&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
         googlewin.onclose = function () {
             //window.location.href = "AssignLaserCode.aspx";
             return true;
         }
     }

     function editEmbossing(i) { // Define This function of Send Assign Laser ID 
         //  alert("Embossing" + i);

         googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserCodeMakeFree.aspx?Mode=Embossing&HSRPRecordID=" + i, "Update Laser Details", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
         googlewin.onclose = function () {
             window.location.href = "AssignLaserCode.aspx";
             return true;
         }
     }

     function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

         googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCode.aspx?Mode=New", "Add Secure Devices User", "width=950px,height=550px,resize=1,scrolling=1,center=1", "recal")
         googlewin.onclose = function () {
             // window.location = 'AssignLaserCode.aspx';
             return true;
         }
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
                                        <span class="headingmain">Confirmation</span>
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
                                    <td valign="left" class="form_text" >
                                       
                                    </td>
                                    <td align="left" valign="middle">
                                        
                                    </td>
                                    <td valign="left" class="form_text">
                                        
                                    </td>
                                    <%--<td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Bank Transaction" class="button">Add New Bank Transaction</a>
                                    </td>--%>

                                    

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                        <asp:Label ID="lblSucMess" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         
                                        

                                         <asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnRowCommand="grdid_RowCommand1">
                <AlternatingRowStyle BackColor="White" />
                 <Columns>
                                            
                      <asp:TemplateField HeaderText="S No" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
                <ItemStyle Width="10%" />
            </asp:TemplateField>
                                                       <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="id" runat="server" Text='<%#Eval("hsrprecordid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                     <asp:BoundField HeaderText="RegNo" ItemStyle-HorizontalAlign="Center" DataField="VehicleRegNo" />
                                                   <%-- <asp:BoundField HeaderText="AuthorizationNo" ItemStyle-HorizontalAlign="Center" DataField="HSRPRecord_AuthorizationNo" />--%>
                                                    <asp:BoundField HeaderText="OwnerName" ItemStyle-HorizontalAlign="Center" DataField="OwnerName" />
                                                    <%--<asp:BoundField HeaderText="VehicleClass" ItemStyle-HorizontalAlign="Center" DataField="VehicleClass" />--%>
                                                    <asp:BoundField HeaderText="VehicleType" ItemStyle-HorizontalAlign="Center" DataField="VehicleType" />
                                                    <%--<asp:BoundField HeaderText="FrontLaserCode" ItemStyle-HorizontalAlign="Center" DataField="HSRP_Front_LaserCode" />
                                                     <asp:BoundField HeaderText="RearLaserCode" ItemStyle-HorizontalAlign="Center" DataField="HSRP_Rear_LaserCode" />--%>
                   
                               <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Select
                                                        <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" /></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CHKSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>                       

                     </Columns>

                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr> 
                </table>
            </td>
            
        </tr>
     <tr>
                        <td>
                            <table  align="center">
                                <tr>
                                    <td valign="middle" visible="true" class="form_text">
                                        
                                        <asp:Label Text="Date:" runat="server" ID="labelDate" Visible="true" />
                                    </td>
                                    <td valign="middle" onmouseup="OrderDate_OnMouseUp()" align="left" style="width: 84px">
                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Visible="true">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                    <td valign="middle" align="left">
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                            onmouseup="OrderDate_OnMouseUp()" class="calendar_button"  src="../images/btn_calendar.gif" />
                                    </td>
                                    
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnGO" Width="58px" runat="server" Visible="true" Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClientClick=" return validate()" OnClick="btnGO_Click" />
                                        
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
                            <td colspan="2">
                                <asp:Label ID="lblMsgBlue" CssClass="bluelink" runat="server" Visible="false" Text="Label"></asp:Label>
                                <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server"  Visible="false" ForeColor="Red" Text="Label"></asp:Label>
                            </td>
                        </tr>
                            </table>
                        </td>
                    </tr>
    </table>


</asp:Content>
