<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AgingReport.aspx.cs" Inherits="HSRP.DLReports.AgingReport" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
   <%-- <script language="javascript" type="text/javascript">



        function validate() {


            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
        } 
    </script>--%>
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
                                        <span class="headingmain">AgeWiseEmbossingReport</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                                    <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" />
                                                </td>
                                                <td valign="middle" width="100px">
                                                    <asp:DropDownList Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                                        runat="server" DataTextField="HSRPStateName" Width="150px" DataValueField="HSRP_StateID" 
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                 <td valign="middle" class="form_text" nowrap="nowrap" width="100px">
                                                 
                                                    <asp:Label Text="Zone:" runat="server" ID="label2" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>

                                             

                                                <td>
                                                 <asp:DropDownList Visible="true" CausesValidation="false" ID="ddl_zone" Width="150px"
                                                        runat="server" DataTextField="zone"
                                                        AutoPostBack="True" 
                                                        DataValueField="zone" 
                                                        onselectedindexchanged="ddl_zone_SelectedIndexChanged"> 
                                                        
                                                        
                                                    </asp:DropDownList>

                                                </td>
                                            <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:UpdatePanel runat="server" ID="UpdateClient" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                             <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false"
                                                    runat="server" DataTextField="RTOLocationName"  
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>                                                 
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" 
                                                    EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                                </td>
                                               <%--  <td valign="middle" class="form_text" nowrap="nowrap">
                                            <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                             <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                           
                                                &nbsp;&nbsp;
                                           
                                                </td>--%>




                                         <%--   <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                Order Status:
                                            </td>
                                           
                                            <td style="width: 168px">
                                            <asp:DropDownList ID="ddlTCReport" runat="server" Visible="true" 
                                                    AutoPostBack="true" 
                                                    onselectedindexchanged="ddlTCReport_SelectedIndexChanged" Height="20px" 
                                                    Width="100px" >
                                           
                                               <asp:ListItem  Text = "--Select Orders--" Value = "0"></asp:ListItem>
                                                    <asp:ListItem Text = "Order Booked" Value = "1"></asp:ListItem>
                                                    <asp:ListItem Text = "Embossing" Value = "2"></asp:ListItem>
                                                    <asp:ListItem Text = "Affixation" Value = "3"></asp:ListItem>
                                                    <asp:ListItem Text = "Pending for Embossing" Value = "4"></asp:ListItem>
                                                    <asp:ListItem Text = "Pending for Affixation" Value = "5"></asp:ListItem>
                                                    </asp:DropDownList>
                                            </td>--%>
                                                <td>
                                              </td>
                                               <%-- <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                    <asp:Label Text="HSRP State:" Visible="false" runat="server" ID="labelOrganization" />
                                                </td>
                                                <td valign="middle" style="width:  50px">
                                                    <asp:DropDownList Visible="false" CausesValidation="false" ID="DropDownListStateName"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID">
                                                    </asp:DropDownList>
                                                </td> --%>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 71px">
                                                    &nbsp;&nbsp;
                                                    <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left" style="width: 120px">
                                                    <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                                <td valign="top" align="left">
                                                    <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap">
                                                    &nbsp;&nbsp;<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" />
                                                    
                                                </td>
                                                <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" style="width: 120px">
                                                    <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                                <td valign="top" align="left">
                                                    <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                        class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>
                                                <td></td>
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                    <asp:Button ID="btnGo" runat="server" Text="Go" 
                                                        ToolTip="Please Click for Report" Width="100px"
                                                        class="button" onclick="btnGo_Click" BackColor="Orange" ForeColor="#000000" />
                                                </td>

                                                

                                                </tr>
                                                <tr>
                                                    <td colspan="9">
                                                        <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            </td>
                        
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                            </table>
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
                        <td colspan="6">
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
                    <tr>
                        <br />
                        <br />
                        <td align="center">

                           <%-- <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                                Width="100%">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField HeaderText="Zone" />
                                    <asp:BoundField HeaderText="RtoLocationname" />
                                    <asp:BoundField HeaderText="Authorizationdate" />
                                    <asp:BoundField HeaderText="Order date" />
                                    <asp:BoundField HeaderText="VehicleRegNo" />
                                    <asp:BoundField HeaderText="OwnerName" />
                                    <asp:BoundField HeaderText="VehicleType" />
                                    <asp:BoundField HeaderText="VehicleClass" />
                                    <asp:BoundField HeaderText="OrderType" />
                                    <asp:BoundField HeaderText="OrderStatus" />
                                    <asp:BoundField HeaderText="MobileNo" />
                                    <asp:BoundField HeaderText="EngineNo" />
                                    <asp:BoundField HeaderText="ChassisNo" />
                                    <asp:BoundField HeaderText="Front_Lasercode" />
                                    <asp:BoundField HeaderText="Rear_Lasercode" />
                                    <asp:BoundField HeaderText="Front_size" />
                                    <asp:BoundField HeaderText="Rear_size" />
                                    <asp:BoundField HeaderText="OrderEmbossingDate" />
                                    <asp:BoundField HeaderText="OrderClosedDate" />
                                    <asp:BoundField HeaderText="StickerMandatory" />
                                    <asp:BoundField HeaderText="RoundOff_NetAmount" />
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"
                                    VerticalAlign="Middle" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>--%>

                             <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333"  AutoGenerateColumns="false" 
                                         Width="300px">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns> 
                                    <asp:BoundField HeaderText="Zone"  DataField="Zone"/>
                                    <asp:BoundField HeaderText="RtoLocationname"  DataField="rtoLocationname"/>
                                    <asp:BoundField HeaderText="Year" DataField="year" />
                                    <asp:BoundField HeaderText="Month"  DataField="month"/>
                                    <asp:BoundField HeaderText="Count" DataField="total"/>
                                   
                                   

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
</asp:Content>
