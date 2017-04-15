﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DeliveryChalan.aspx.cs" Inherits="HSRP.Master.DeliveryChalan" %>

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
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) {

                return true;
            }
            else {
                return false;
            }

        }
    </script>
    <script language="javascript" type="text/javascript">
        function validateStatus() {

            
        }
    
    </script>
    
    <%--<script language="javascript" type="text/javascript">
        function validateSearch() {

            if (document.getElementById("<%=textboxSearch.ClientID%>").value == "") {
                alert("Please Provide Search Content");
                document.getElementById("<%=textboxSearch.ClientID%>").focus();
                return false;
            }
        }
    
    </script>--%>
    
    <script language="javascript" type="text/javascript">
        function validate() {

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
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                            <td>
                            <div style="margin-left: 10px; font-size: medium; color: Black"> Delevery Chalan </div>
                            </td>
                            </tr>
                                <tr >
                                    <td >
                                    <asp:Label ID="lblmes" CssClass="headingmain" runat="server" Visible="false">View Laser Code </asp:Label> 
                                     <asp:Label ID="Label1" ForeColor="Black" CssClass="RTOLabel"  Visible="false" runat="server"> Search By :</asp:Label> 
                                        <asp:RadioButton ForeColor="Black" ID="RadioButtonCreateion"  Visible="false" CssClass="RTOLabel"  Text="Creation Date" runat="server" Checked="true"    GroupName="Check" /> 
                                        <asp:RadioButton ForeColor="Black" ID="radiobuttonAuthorization"  Visible="false" CssClass="RTOLabel"  Text="Authorization Date"   runat="server" GroupName="Check" /> 
                                        <asp:RadioButton ForeColor="Black" ID="radiobuttonOrderClose"  Visible="false" CssClass="RTOLabel"  Text="Order Closed Date" runat="server" GroupName="Check" />  
                                         <asp:Label ID="Label2" ForeColor="Black" CssClass="RTOLabel"  Visible="false" runat="server"> Select For All Allowed RTO's :  </asp:Label> 
                                        <asp:CheckBox ForeColor="Black" ID="checkboxAll" runat="server"  Visible="false" Checked="false" />  
                                        <asp:Label ID="Label3" ForeColor="Red" CssClass="RTOLabel" runat="server"  Visible="false"> Search By Vehicle Reg.No.  </asp:Label> 
                                        <asp:TextBox runat="server" Width="120px" ID="textboxSearch"  Visible="false"></asp:TextBox> 
                                        <asp:LinkButton ID="Linkbutton1" runat="server" Text="GO" class="button"  Visible="false"
                                             onclick="Linkbutton1_Click"    />
                                    </td>  
                                </tr>
                                <tr id="TRRTOHide" runat="server">
                                    <td > 
                                          <asp:Label ID="dataLabellbl" class="headingmain" runat="server"  >Allowed RTO's :</asp:Label> 
                                          <asp:Label ID="lblRTOCode" class="form_Label_Repeter"  runat="server">Allowed RTO's : </asp:Label> 
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
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> 
                                    </td>
                                    <td valign="middle" class="form_text">
                                       <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate> 
                                             <asp:DropDownList Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName" AutoPostBack="true"
                                                    DataValueField="RTOLocationID" 
                                                    onselectedindexchanged="dropDownListClient_SelectedIndexChanged1"   >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient" /> 
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    
                                    <td valign="middle"  Visible="false" class="form_text" nowrap="nowrap">&nbsp;&nbsp; <asp:Label Text="From:"  runat="server" ID="labelDate" /> &nbsp;&nbsp;</td>
                                    <td valign="top"   onmouseup="OrderDate_OnMouseUp()" align="left">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server"  PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"  
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
                                   <td valign="middle" class="form_text" nowrap="nowrap"> &nbsp;&nbsp;<asp:Label Text="To:"  runat="server" ID="labelTO" /> &nbsp;&nbsp;</td>
                                   <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()"   align="left">
                                                                <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"  
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                      
                                                            <td valign="top" align="left"  Visible="false">
                                                                <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                                    class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>


                                   <td valign="middle" class="form_text" nowrap="nowrap"> <asp:Label Text="File Name"  runat="server" ID="labelOrderStatus" />&nbsp;&nbsp; </td>
                                    <td> <asp:DropDownList AutoPostBack="true"  DataTextField="PdfName" 
                                            DataValueField="pdffilename" Width="125px"
                                            ID="dropDownListorderStatus" CausesValidation="false" runat="server"
                                            onselectedindexchanged="dropDownListorderStatus_SelectedIndexChanged"  > 
                                                </asp:DropDownList>&nbsp; </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button"  Visible="false" OnClientClick=" return validate()"
                                            onclick="ButtonGo_Click" /> &nbsp;  
                                            <asp:Button ID="btnExportToExcel"  Width="100px"  runat="server" Text="Report In Excel"   ToolTip="Please Click for Report" Visible="false"
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnExportToExcel_Click"    /> &nbsp;  

                                            <asp:Button ID="btnExportToPDF"  Width="100px" Visible="false" runat="server" Text="Report In PDF" ToolTip="Please Click for Report"
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnExportToPDF_Click"/> &nbsp;&nbsp;  

                                            <asp:Button ID="btnDownloadRecords"  Width="114px"  runat="server"   Visible="false"
                                            Text="Download In PDF" ToolTip="Please Click for Report"
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnDownloadRecords_Click" /> &nbsp;&nbsp;&nbsp;  
                                             
                                        <asp:Button Width="150px" ID="ButImpData" runat="server" Visible="false" class="button" Text="Generate Data For NIC" onclick="ButImpData_Click" OnClientClick=" return validateStatus()" />
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
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td >
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    <center>
                                        <asp:GridView ID="Grid1" runat="server" AutoGenerateColumns="False" 
                                             
                                            BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" Width="800px"
                                            CellPadding="2" ForeColor="Black" GridLines="None" 
                                            onpageindexchanging="Grid1_PageIndexChanging"  >
                                            <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                            <FooterStyle BackColor="Tan" />
                                            <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" 
                                                HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                            <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                            <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                            <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                            <SortedDescendingHeaderStyle BackColor="#C2A47B" />

                                            <Columns>
                                             <asp:BoundField HeaderText="ID" DataField="hsrpRecordID" Visible="false" ></asp:BoundField>
                                            <asp:TemplateField HeaderText="Select" >
                                            <ItemTemplate>
                                            <%--<asp:CheckBox  ID="gridcheckbox"  Enabled ='<%# Eval("checkb") %>' runat="server" />--%>
                                            <asp:CheckBox  ID="gridcheckbox"   runat="server" />
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField HeaderText="Sno." DataField="SrNo" ></asp:BoundField> 
                                            <asp:BoundField HeaderText="Vehicle RegNo." DataField="vehicleRegNo" />
                                            <asp:BoundField HeaderText="Front Laser Code" DataField="HSRP_Front_LaserCode" />
                                            <asp:BoundField HeaderText="Rear Laser Code" DataField="HSRP_Rear_LaserCode" /> 
                                            </Columns>


                                        </asp:GridView>

                                        <asp:Button ID="btnSaveGrid" Visible="true" class="button"  runat="server" 
                                            Text="SAVE" onclick="btnSaveGrid_Click"  />

                                        </center> 
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
                        </td>
                    </tr> 
                    
                </table>
            </td>
        </tr>
    </table>
    <br /><asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>
