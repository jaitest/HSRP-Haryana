<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="APTGChalan.aspx.cs" Inherits="HSRP.EmbossingData.APTGChalan" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>

    <style type="text/css">
        .button-success {
            background: rgb(28, 184, 65); /* this is a green */
        }

        .auto-style1 {
            width: 2%;
        }
        .auto-style2 {
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            color: Black;
            text-decoration: none;
            nowrap: nowrap;
            width: 10%;
        }
    </style>
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
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) {

                return true;
            }
            else {
                return false;
            }
        }

        function validatevalue() {
            var temp = true;
            if ($('#txtTransporter').val() == '') {
                alert('Please Enter Transporter value');
                temp = false;
                return temp;
            }
            if ($('#txtLorryNo').val() == '') {
                alert('Please enter Lorry No');
                temp = false;
                return temp;

            }
            var tempvalues = checkcheckbox();
            if (tempvalues == true) {
                alert('please check at least one');
                temp = false;
            }
            return temp;
        }

        function checkcheckbox() {
            var ischeck = true;
            $('.testing input:checkbox').each(function () {

                if ($(this).prop("checked") == true) {
                    ischeck = false;
                    return false;
                }
                else {
                    ischeck = true;
                }

            })
            return ischeck;
        }

        $(document).ready(function () {

            //$('.testing input:checkbox').each(function () {
            //    $(this).prop("checked", false);
            //})
        })
    </script>
    <script language="javascript" type="text/javascript">
        function validateStatus() {
        }
    </script>
    <script language="javascript" type="text/javascript">
        function validate()
        {
            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select RTO Name--") {
                alert("Please Select RTO Name");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }
        }
       
            
    </script>

    <div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr align="left">
                            <td height="27" background="../images/midtablebg.jpg" align="left">
                                <asp:Label ID="Label4" class="headingmain" runat="server">Challan/Invoice for Embossed Plates</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table align="center" cellpadding="0" cellspacing="0" border="1">
                                    <tr>
                                        <td>
                                            <table align="center" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td valign="middle" class="form_text" nowrap="nowrap" style="width: 20%" align="center">
                                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                                    </td>
                                                    <td valign="middle" style="width: 20%">
                                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                                            OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged" Width="170px" ClientIDMode="Static">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="middle" class="form_text" nowrap="nowrap" align="center" width="20%">
                                                        <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />
                                                        &nbsp;</td>
                                                    <td valign="middle" align="center" style="width: 10%">
                                                        <asp:DropDownList Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                            runat="server" DataTextField="RTOLocationName" AutoPostBack="True" DataValueField="RTOLocationID"
                                                             OnSelectedIndexChanged="dropDownListClient_SelectedIndexChanged" ClientIDMode="Static">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="middle" visible="false" class="form_text" nowrap="nowrap" style="width: 10%" align="center">
                                                        <asp:Label Text="From:" runat="server" ID="labelDate" />

                                                    </td>
                                                    <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left" style="width: 1%">
                                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                            ControlType="Picker" PickerCssClass="picker" Width="107px">
                                                            <ClientEvents>
                                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                            </ClientEvents>
                                                        </ComponentArt:Calendar>
                                                    </td>
                                                    <td valign="top" align="left" class="auto-style1" style="width: 1%">
                                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                            onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                    </td>
                                                    <td valign="middle" class="form_text" nowrap="nowrap" align="right" style="width: 10%">
                                                        <asp:Label Text="To:" runat="server" ID="labelTO" />
                                                    </td>
                                                    <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" style="width: 1%">
                                                        <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                            ControlType="Picker" PickerCssClass="picker">
                                                            <ClientEvents>
                                                                <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                            </ClientEvents>
                                                        </ComponentArt:Calendar>
                                                    </td>
                                                    <td valign="top" align="left" visible="false" style="width: 1%">
                                                        <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                            class="calendar_button" src="../images/btn_calendar.gif" />
                                                    </td>
                                                    <td valign="middle" class="auto-style2" nowrap="nowrap">
                                                        <asp:Button ID="btnGO" Width="58px" runat="server"
                                                            Text="GO" ToolTip="Please Click for Report" BackColor="Orange" ForeColor="#000000"
                                                            class="button" OnClientClick=" return validate()"
                                                            OnClick="btnGO_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" class="form_text" nowrap="nowrap" colspan="11">&nbsp; &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>
                                                    <td valign="middle" align="right">
                                                        <asp:Label Text="Transporter:" runat="server" ID="label5" Font-Size="11pt"
                                                            ForeColor="Black" /><span style="color: Red">*&nbsp;&nbsp; </span></td>
                                                    <td valign="middle" class="form_text" nowrap="nowrap" align="left" colspan="2">
                                                        <asp:TextBox  ID="txtTransporter" runat="server"></asp:TextBox></td>
                                                    <td valign="middle" visible="false" class="form_text" nowrap="nowrap"
                                                        align="right" colspan="2">Lorry No:<span style="color: Red">*&nbsp; </span></td>
                                                    <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left" colspan="2">
                                                        <asp:TextBox ID="txtLorryNo" runat="server"></asp:TextBox>
                                                    </td>

                                                    <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" colspan="3"></td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" class="form_text" nowrap="nowrap" colspan="11">&nbsp;&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" class="form_text" nowrap="nowrap" colspan="11" align="center">&nbsp;
                                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="red" />
                                                        <asp:Label ID="LblMessage" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="blue" />
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
                            <td align="center">
                                <br />
                                <asp:Panel ID="PanelGrid1"  runat="server" ScrollBars="Vertical" Width="80%" Height="457px"  HorizontalAlign="Center">
                                    <asp:Button ID="btnChalan"  runat="server"  Text="Generate Challan/Invoice" CssClass="button-success" BackColor="#ff9966" autoposback="true" OnClick="btnChalan_Click" OnClientClick="HideShow();" Width="217px" />
                                    <center>
                                        <asp:GridView ID="Grid1"  runat="server" BackColor="LightGoldenrodYellow" AutoGenerateColumns="False" Width="100%"
                                            PageSize="20" BorderColor="Tan" BorderWidth="1px"
                                            CellPadding="2"
                                            DataKeyNames="hsrprecordid" ForeColor="Black" GridLines="None">
                                            <FooterStyle BackColor="Tan" />
                                            <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NextPreviousFirstLast" NextPageText="Next" PreviousPageText="Previous" />
                                            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                            <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                            <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                            <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                            <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Select
                                                   <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox CssClass="testing" ID="CHKSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        S.No
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="SrNo" runat="server" Text='<%#Eval("SNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Authorization No
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="AuthorizationNo" runat="server" Text='<%#Eval("HSRPRecord_AuthorizationNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Vehicle Reg No
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVehicleRegNo" runat="server" Text='<%#Eval("VehicleRegNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        F Laser Code
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFLaserCode" runat="server" Text='<%#Eval("hsrp_front_lasercode") %>' Enabled="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        R Laser Code
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRlaserCode" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>' Enabled="false"></asp:TextBox>
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
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Order Status
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>                                        
                                    </center>                                    
                                </asp:Panel>                                
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">                               
                               <asp:Panel ID="PanelGrid2"  runat="server" ScrollBars="Vertical" Width="80%" Height="457px" HorizontalAlign="Center" BorderWidth="1">
                                 <asp:Button ID="btnpdf" runat="server" Text="Download Challan" CssClass="button-success" BackColor="#ff6666" autoposback="true" Width="262px" OnClick="btnpdf_Click" />
                                 <asp:Button ID="btnrecordinpdf" runat="server" Text="Download Challan Summary" CssClass="button-success" BackColor="#ff6666" autoposback="true"  OnClick="btnrecordinpdf_Click" Width="262px" /> 
                                   <center>                                        
                                        <asp:GridView ID="Grid2"  runat="server" BackColor="LightGoldenrodYellow" AutoGenerateColumns="False" Width="100%"
                                            PageSize="20" BorderColor="Tan" BorderWidth="1px"
                                            CellPadding="2"
                                            DataKeyNames="hsrprecordid" ForeColor="Black" GridLines="None">
                                            <FooterStyle BackColor="Tan" />
                                            <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NextPreviousFirstLast" NextPageText="Next" PreviousPageText="Previous" />
                                            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                            <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                            <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                            <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                            <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                            <Columns>                                                
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        S.No
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="SrNo" runat="server" Text='<%#Eval("SNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Authorization No
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="AuthorizationNo" runat="server" Text='<%#Eval("HSRPRecord_AuthorizationNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Vehicle Reg No
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVehicleRegNo" runat="server" Text='<%#Eval("VehicleRegNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        F Laser Code
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFLaserCode" runat="server" Text='<%#Eval("hsrp_front_lasercode") %>' Enabled="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        R Laser Code
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRlaserCode" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>' Enabled="false"></asp:TextBox>
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
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Order Status
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView> 
                                    </center>                                    
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">                               
                                 
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
