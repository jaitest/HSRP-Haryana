<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RtoWiseAffixationStatus.aspx.cs" Inherits="HSRP.Report.RtoWiseAffixationStatus" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
            return false;
        }
    }
    </script>

    <script type="text/javascript">

        function OrderDatefrom_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDatefrom.getSelectedDate();
            CalendarOrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDatefrom.getSelectedDate();
            OrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnClick() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                CalendarOrderDatefrom.hide();
            }
            else {
                CalendarOrderDatefrom.setSelectedDate(OrderDatefrom.getSelectedDate());
                CalendarOrderDatefrom.show();
            }
        }

        function OrderDatefrom_OnMouseUp() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function OrderDateto_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDateto.getSelectedDate();
            CalendarOrderDateto.setSelectedDate(fromDate);

        }

        function OrderDateto_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDateto.getSelectedDate();
            OrderDateto.setSelectedDate(fromDate);
        }

        function OrderDateto_OnClick() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                CalendarOrderDateto.hide();
            }
            else {
                CalendarOrderDateto.setSelectedDate(OrderDateto.getSelectedDate());
                CalendarOrderDateto.show();
            }
        }

        function OrderDateto_OnMouseUp() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <div style="width:1107px; height: 500px;">   
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="../images/midtablebg.jpg">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <span class="headingmain">Rto Wise Affixation Report</span>
                                        </td>
                                        <td width="300px" height="26" align="center" nowrap></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <table width="100%">
            <tr>
                <td colspan="8">
                    <table style="width: 80%">
                        <tr>
                            <td align="left">
                                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1"
                                    ForeColor="Black" Font-Bold="True" Width="83px" />
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="True"
                                    CausesValidation="false" DataTextField="HSRPStateName"
                                    DataValueField="HSRP_StateID"
                                    Height="22px" Width="120px" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="DropDownListStateName" ErrorMessage="Select State"
                                    InitialValue="--Select State--"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="label2" runat="server" Font-Bold="True" ForeColor="Black"
                                    Text="Order Type:" Width="100px" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListOrderType" runat="server"
                                    CausesValidation="false" DataTextField="HSRPStateName"
                                    DataValueField="HSRP_StateID" Height="22px" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="DropDownListOrderType_SelectedIndexChanged">
                                    <asp:ListItem Value="B">Both</asp:ListItem>
                                    <asp:ListItem Value="D">Dealer</asp:ListItem>
                                    <asp:ListItem Value="N">Non Dealer</asp:ListItem>
                                </asp:DropDownList>
                            </td>

                            <td>
                                <asp:Label Text="From:" runat="server"
                                    ID="labelDate" Font-Bold="True"
                                    ForeColor="Black" Width="60px" />
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <ComponentArt:Calendar ID="OrderDatefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="OrderDatefrom_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                        <img id="calendar_from_button" alt="" onclick="OrderDatefrom_OnClick()"
                                            onmouseup="OrderDatefrom_OnMouseUp()" class="calendar_button"
                                            src="../images/btn_calendar.gif" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btnexport" runat="server" OnClick="btnexport_Click" Text="Export"
                                    Font-Bold="True" ForeColor="#3333FF" />
                            </td>
                            <td>
                                <asp:Button ID="btnaffection" runat="server" Text="Detail"
                                    Font-Bold="True" ForeColor="#3333FF" OnClick="btnaffection_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td width="80" colspan="4">
                                &nbsp;</td>
                            <td colspan="4" align="left">
                                <ComponentArt:Calendar runat="server" ID="CalendarOrderDatefrom" AllowMultipleSelection="false"
                        AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                        PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                        DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                        OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                        SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                        MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                        ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif"
                        NextImageUrl="cal_nextMonth.gif" Height="172px" Width="200px">
                        <ClientEvents>
                            <SelectionChanged EventHandler="OrderDatefrom_OnChange" />
                        </ClientEvents>
                    </ComponentArt:Calendar>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8" align="center">
                                <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="8" align="center" id="gridTD" runat="server">
                    <asp:Panel ID="PanelContainer" runat="server" ScrollBars="Horizontal" Width="80%"  HorizontalAlign="Center"><%-- --%>                                                 
                    <asp:Table ID="tbl" runat="server" BorderWidth="1px" BorderColor="white" CellPadding="0" CellSpacing="0" 
                    CssClass="textColor" Width="100%" HorizontalAlign="Center" BackColor="#0083C1" ForeColor="White">
                    <asp:TableRow ID="TableRow1" runat="server">
                        <asp:TableCell ID="TableCell1" runat="server" Width="5%">                   
                            <asp:Label ID="lbl" Text="Affixation Center"  runat="server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell5" runat="server" Width="3%">
                            <asp:Label ID="Label30" Text="Location Name" runat="server"></asp:Label>
                        </asp:TableCell>   
                        <asp:TableCell ID="TableCell7" runat="server" Width="4%">
                            <asp:Label ID="Label3" Text="Opening" runat="server"></asp:Label>
                        </asp:TableCell> 
                        <asp:TableCell ID="TableCell8" runat="server" Width="4%">
                            <asp:Label ID="Label4" Text="Today Received" runat="server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell9" runat="server" Width="4%">
                            <asp:Label ID="Label15" Text="Today Affixation" runat="server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell ID="TableCell10" runat="server" Width="3%">
                            <asp:Label ID="Label5" Text="Closing Balance" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    </asp:Table>   
                     <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="457px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
                    <asp:GridView ID="grdAffection" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" 
                        OnRowCommand="grdAffection_RowCommand" OnSelectedIndexChanged="grdAffection_SelectedIndexChanged" 
                        ShowHeader="False" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>                           
                            <asp:TemplateField HeaderText="Affixation Center" HeaderStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblAffiCName" runat="server" Text='<%#Eval("EmbCenterName")%>' ></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="Blue" />
                                <ItemStyle HorizontalAlign="Left" Width="4%"/>
                            </asp:TemplateField>                           

                            <asp:TemplateField HeaderText="Location Name" HeaderStyle-ForeColor="Blue">
                                <ItemTemplate> 
                                    <asp:Label ID="lblLocName" runat="server" Text='<%#Eval("Locationname")%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="Blue" />
                                <ItemStyle HorizontalAlign="Left" Width="2%"/>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Opening" HeaderStyle-ForeColor="Blue">
                                <ItemTemplate>                                   
                                    <asp:LinkButton ID="lblOpening" CommandName="Opening" runat="server" CommandArgument='<%#Eval("EmbCenterName") %>' Text='<%#Eval("Opening") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="Blue" />
                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                            </asp:TemplateField>                           

                            <asp:TemplateField HeaderText="Today Received" HeaderStyle-ForeColor="Blue">
                                <ItemTemplate>                                    
                                    <asp:LinkButton ID="lblTodayRec" CommandName="Today Received" runat="server" Text='<%#Eval("Today Received") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="Blue" />
                                <ItemStyle HorizontalAlign="Center" Width="4%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Today Affixation" HeaderStyle-ForeColor="Blue">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbnTodayAffi" CommandName="Today Affixation" runat="server" Text='<%#Eval("Today Affixation") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="Blue" />
                                <ItemStyle HorizontalAlign="Center" Width="4%"/>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Closing Balance" HeaderStyle-ForeColor="Blue">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbnClosingBal" CommandName="Closing Balance" runat="server" Text='<%#Eval("Closing Balance") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                 <HeaderStyle ForeColor="Blue" />
                                 <ItemStyle HorizontalAlign="Center" Width="3%" />
                            </asp:TemplateField>
                        </Columns>

                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#F7F7DE" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                        <SortedAscendingHeaderStyle BackColor="#848384" />
                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                        <SortedDescendingHeaderStyle BackColor="#575357" />
                    </asp:GridView>
                </asp:Panel>
                </asp:Panel>

                </td>            
            </tr>              
        </table>
    </div>
</asp:Content>

