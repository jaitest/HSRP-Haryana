<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DealerChalan.aspx.cs" Inherits="HSRP.Dealer.Transaction.DealerChalan" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">
        function editpage(i) {
            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPState.aspx?Mode=Edit&StateID=" + i, "Update State Details", "width=900px,height=350px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewHSRPState.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "DealerDataEntry.aspx?Mode=New", "Add New Data Entry", "width=900px,height=390px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'ViewDealerDataEntry.aspx';
                return true;
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function validate() {
            debugger
            if (document.getElementById("<%=ddlDealerName.ClientID%>").value == "0") {
                alert("Please Select Dealer Name");
                document.getElementById("<%=ddlDealerName.ClientID%>").focus();
                return false;
            }

        }
    </script>
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change HSRPState?")) {

                return true;
            }
            else {
                return false;
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
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" style="background-image: url(../images/midtablebg.jpg)">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Dealer Challan </span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap="nowrap">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%-- <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                 <td height="35" align="left" valign="middle" class="footer">
                                  <%--   <asp:Button ID="btnOldData" runat="server" Text="View Old Data" class="button" Visible="false"
                                         onclick="btnOldData_Click" />--%>
                        </td>
                        <td height="35" align="right" valign="middle" class="footer">
                            <%-- <a onclick="AddNewPop(); return false;" title="Add New HSRP State" class="button">Add New Data Entry</a>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"
                    class="topheader">
                    <tr>
                        <td valign="middle" class="form_text" nowrap="nowrap">
                            <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                        </td>
                        <td valign="middle">
                            <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td valign="middle" class="form_text">
                            <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                    <asp:DropDownList Visible="true" ID="dropDownListClient" CausesValidation="false"
                                        AutoPostBack="true" runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID"
                                        OnSelectedIndexChanged="DropDownListLocation_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                    <asp:PostBackTrigger ControlID="dropDownListClient" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"
                    class="topheader">
                    <tr>
                        <td style="color: Black">
                            Select Dealer Name
                        </td>
                        <td valign="middle">
                            <asp:DropDownList ID="ddlDealerName" runat="server" Width="130px">
                            </asp:DropDownList>
                        </td>
                        <td valign="middle" class="form_text" nowrap="nowrap">
                            &nbsp;&nbsp;
                            <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />
                            &nbsp;&nbsp;
                        </td>
                        <td valign="middle" onmouseup="OrderDate_OnMouseUp()" align="left">
                            <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                ControlType="Picker" PickerCssClass="picker">
                                <ClientEvents>
                                    <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                </ClientEvents>
                            </ComponentArt:Calendar>
                        </td>
                        <td valign="top" align="left">
                            <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../../images/btn_calendar.gif" />
                        </td>
                        <td valign="middle" class="form_text" nowrap="nowrap">
                            &nbsp;&nbsp;<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" />
                            &nbsp;&nbsp;
                        </td>
                        <td valign="middle" onmouseup="HSRPAuthDate_OnMouseUp()" align="left">
                            <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                ControlType="Picker" PickerCssClass="picker">
                                <ClientEvents>
                                    <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                </ClientEvents>
                            </ComponentArt:Calendar>
                        </td>
                        <td valign="middle" align="left">
                            <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                class="calendar_button" src="../../images/btn_calendar.gif" />
                        </td>
                        <td valign="middle" class="form_text" nowrap="nowrap">
                            <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button" OnClientClick="return validate()"
                                OnClick="ButtonGo_Click" />
                            &nbsp; &nbsp;
                        </td>
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
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="padding-top: 20px">
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" AutoGenerateColumns="false" 
                                PageSize="25" AllowPaging="true" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" OnPageIndexChanging="GridView1_PageIndexChanging">
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Select
                                            <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CHKSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Vehicle Reg No</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehicleRegNo" runat="server" Text='<%#Eval("VehicleRegNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            F Laser Code</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="txtFLaserCode" runat="server" Text='<%#Eval("hsrp_front_lasercode") %>'></asp:Label>
                                            <%-- <asp:TextBox ID="txtFLaserCode" runat="server"></asp:TextBox>--%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            R Laser Code</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="txtRlaserCode" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>'></asp:Label>
                                            <%-- <asp:TextBox ID="txtRlaserCode" runat="server"></asp:TextBox>--%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="id" runat="server" Text='<%#Eval("hsrprecordid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding-top: 10px">
                            <asp:Label ID="LblMessage" runat="server" Text=""></asp:Label>
                            <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="button" Visible="false"
                                OnClick="btnSave_Click" />
                        </td>
                        <td align="center" style="padding-top: 10px">
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                            <asp:Button ID="Button1" runat="server" Text="Records In PDF" CssClass="button" OnClick="Button1_Click" Visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
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
    </table>
    </td> </tr> </table>
    <br />
</asp:Content>
