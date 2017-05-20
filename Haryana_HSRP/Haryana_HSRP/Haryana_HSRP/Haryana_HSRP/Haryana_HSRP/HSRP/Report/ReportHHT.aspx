<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ReportHHT.aspx.cs" Inherits="HSRP.Report.ReportHHT" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function PickerFrom_OnDateChange(sender, eventArgs) {
            var fromDate = PickerFrom.getSelectedDate();
            CalendarFrom.setSelectedDate(fromDate);

        }

        function CalendarFrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarFrom.getSelectedDate();
            PickerFrom.setSelectedDate(fromDate);

        }

        function ButtonFrom_OnClick() {
            if (CalendarFrom.get_popUpShowing()) {
                CalendarFrom.hide();
            }
            else {
                CalendarFrom.setSelectedDate(PickerFrom.getSelectedDate());
                CalendarFrom.show();
            }
        }

        function ValidateForm() {
            alert(document.getElementById('kk').value);
        }
    </script>
    <link href="../css/Grid.css" rel="stylesheet" type="text/css" />
    <div align="center">
        <br />
          <div align="left" style="padding-left: 40px;text-decoration:underline">
            <asp:Label Font-Size="Large" ForeColor="Blue" runat="server" ID="label1"  Text="HandHeld Status Screen "></asp:Label></div>
            <br />
        <asp:GridView ID="Grid" CssClass="grid" runat="server" AlternatingRowStyle-CssClass="gridAltRow"
            RowStyle-CssClass="gridRow" AutoGenerateColumns="false" DataKeyNames="StateName"
            OnRowCommand="Grid_RowCommand" OnRowEditing="Grid_RowEditing">
            <Columns>
                <asp:TemplateField HeaderText="Serial No" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%#Eval("SNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("StateName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Machine" ShowHeader="False" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Text='<%# Bind("TotalMachine") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <div id="vehshow" align="center" runat="server" style="display: none; text-align: center">
        <table border="0" cellpadding="0" align="center" cellspacing="0">
            <tr align="center">
                <td valign="top">
                    <table align="center" cellspacing="0" cellpadding="0" border="0">
                        <tr align="center">
                            <td valign="top" onmouseup="ButtonFrom_OnMouseUp()">
                                <ComponentArt:Calendar ID="PickerFrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                    ControlType="Picker" PickerCssClass="picker">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="PickerFrom_OnDateChange" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>
                            </td>
                            <td style="font-size: 10px;">
                                &nbsp;
                            </td>
                            <td valign="top">
                                <img id="calendar_from_button" alt="" onclick="ButtonFrom_OnClick()" onmouseup="ButtonFrom_OnMouseUp()"
                                    class="calendar_button" src="../images/btn_calendar.gif" />
                            </td>
                            <td>
                                &nbsp;&nbsp;
                                <asp:LinkButton Text="Go" ID="linkButtonGenerateReport" CssClass="button" runat="server"
                                    OnClick="linkButtonGenerateReport_Click" OnClientClick="javascript:return ValidateForm();"></asp:LinkButton>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <%-- <asp:RequiredFieldValidator ID="requiredFieldValidator3" SetFocusOnError="true" runat="server"
                                                            ErrorMessage="Please enter date." ControlToValidate="PickerFrom" ForeColor="Red"
                                                            Display="Dynamic" Style="clear: both; float: left;"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <ComponentArt:Calendar runat="server" ID="CalendarFrom" AllowMultipleSelection="false"
                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                        <ClientEvents>
                            <SelectionChanged EventHandler="CalendarFrom_OnChange" />
                        </ClientEvents>
                    </ComponentArt:Calendar>
                </td>
            </tr>
        </table>
    </div>
    <div id="veh" align="center" runat="server" style="display: none;">
        <br />
           <div align="left" style="padding-left: 40px;text-decoration:underline">
            <asp:Label Font-Size="Large" ForeColor="Blue" runat="server" ID="labelGridView1"></asp:Label></div>
            <br />
        <asp:GridView ID="GridView1" CssClass="grid1" runat="server" EmptyDataText="No Record For the Selected Date." AlternatingRowStyle-CssClass="gridAltRow"
            RowStyle-CssClass="gridRow" AutoGenerateColumns="false" DataKeyNames="MachineNo"
            OnRowCommand="GridView1_RowCommand" OnRowEditing="GridView1_RowEditing">
            <Columns>
                <asp:TemplateField HeaderText="Serial No" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%#Eval("SNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Machine No" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("MachineNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Location" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%#Eval("Location") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Start Time" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("StartTime") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EndTime" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("EndTime")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Record" ShowHeader="False" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Text='<%# Bind("TotalRecord") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns> 
        </asp:GridView>
    </div>
    <div id="Div1" align="center" runat="server">
        <br />
        <div align="left" style="padding-left: 40px;text-decoration:underline">
            <asp:Label Font-Size="Large" ForeColor="Blue" runat="server" ID="labelGridView2"></asp:Label></div>
            <br />
        <asp:GridView ID="GridView2" CssClass="grid1" runat="server" AlternatingRowStyle-CssClass="gridAltRow"
            RowStyle-CssClass="gridRow" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Serial No" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%#Eval("SNo") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Authorization No" HeaderStyle-HorizontalAlign="Left"
                    ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("AuthorizationNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left"
                    ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%#Eval("CustomerName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vehicle Reg No" HeaderStyle-HorizontalAlign="Left"
                    ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("VehicleRegNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mobile Number" HeaderStyle-HorizontalAlign="Left"
                    ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("MobileNumber") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vehicle Type" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("VehicleType")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="90px">
                    <ItemTemplate>
                        <%# Eval("Amount")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <input type="hidden" id="kk" runat="server" />
        <input type="hidden" id="Hiddenfrom" runat="server" />
        <input type="hidden" id="HiddenTo" runat="server" />
    </div>
</asp:Content>
