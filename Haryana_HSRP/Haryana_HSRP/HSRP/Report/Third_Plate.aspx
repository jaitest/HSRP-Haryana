<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Third_Plate.aspx.cs" Inherits="HSRP.Report.Third_Plate" %>
<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
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
                                <span class="headingmain">Report</span>
                                </td>
                                <td width="300px" height="26" align="center" nowrap>
                                </td>
                            </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

   
    <table style="width: 85%; height: 51px; left: 50px;">
        <tr>
            <td width="80">
                &nbsp;</td>
            <td valign="middle" class="form_text" colspan="2" nowrap="nowrap">
                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1"  Visible="true"  ForeColor="Black" Font-Bold="True" />
                                           
            </td>
            <td style="width: 80px; ">
                
                    <ContentTemplate>
                        <asp:DropDownList ID="DropDownListStateName" runat="server"  Visible="true" CausesValidation="false" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"  Height="25px" Width="148px">
                        </asp:DropDownList>
                    </ContentTemplate>
                
            </td>

              <td width="80">
                <asp:Label Text="Dealer:" runat="server" ID="labelSelectType" Visible="false"
                                            ForeColor="Black" Font-Bold="True" />
            </td>
            <td style="width: 80px; ">
                
                    <ContentTemplate>
                        <asp:DropDownList Visible="false" ID="ddlBothDealerHHT" CausesValidation="false" Width="140px"   runat="server">
                                           
                                            <asp:ListItem Text="Both">Both</asp:ListItem>
                                            <asp:ListItem Text="Dealer Data">Dealer</asp:ListItem>
                                            <asp:ListItem Text="Other">Other</asp:ListItem>
                                        </asp:DropDownList>
                    </ContentTemplate>
                
            </td>
            <td style="width: 40px; ">
                
                    <asp:Label Text="From:" runat="server" ID="labelDate" Font-Bold="True" Visible="false"
                                                        ForeColor="Black" /> 
                
            </td>

           <%-- <td style="width: 105px;">
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
                            </td>--%>
            <td style="width: 80px; ">
                
                                                    <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy" Visible="false"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                
            </td>
            <td style="width: 20px; ">
                
                                                   <%-- <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()" 
                                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />--%></td>
            <td style="width: 20px; ">
                
                    <asp:Label Text="Date:" runat="server" ID="labelTO" Font-Bold="True" 
                                                        ForeColor="Black" /> 
                
            </td>
            <td style="width: 80px; ">
                
                                                    <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                
            </td>
                                                     <td style="width: 20px; ">
                
                                                    <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                        class="calendar_button" src="../images/btn_calendar.gif" />

                                                     </td>
           
                                            <td width="80">
                                                    <asp:Button ID="btndetails" runat="server" Text="Detail" Font-Bold="True" BackColor="Orange" ForeColor="#3333FF" onclientclick="validate()"  OnClick="btndetails_Click" />
                                                </td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                                                <td width="80">
                                                    <asp:Button ID="btnexport" runat="server" BackColor="Orange" onclick="btnexport_Click" Text="Export To Excel" 
                                                        Font-Bold="True" ForeColor="#3333FF" onclientclick="validate()" />
                                                </td>
        </tr>
        <td>
            &nbsp;</td>
        <td colspan="2">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
        </td>
        <td>
            &nbsp;

        </td>
        <td>
            &nbsp;

        </td>
        <td>
            &nbsp;

        </td>
        <td>
            &nbsp;

        </td>
        <td style="width: 106px">
            &nbsp;

        </td>
        <td>
            &nbsp;

        </td>
        <tr>
             <td>
                                                    &nbsp;</td>
             <td colspan="2">
                                                    &nbsp;</td>
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
             <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="height: 18px">
                                                    </td>
                                                <td colspan="2" style="height: 18px">
                                                    </td>
                                                <td style="height: 18px">
                                                    </td>
                                                <td style="height: 18px">
                                                    </td>
                                                <td style="height: 18px">
                                                    </td>
                                                <td style="height: 18px">
                                                    </td>
                                                <td style="height: 18px; width: 106px;">
                                                    </td>
                                                <td style="height: 18px">
                                                    </td>
        </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>


        <tr>
                <td colspan="8" align="center" id="gridTD" runat="server">
                    <asp:Panel ID="PanelContainer" runat="server" ScrollBars="Horizontal" Width="80%"  HorizontalAlign="Center">                                                 
                    <%--<asp:Table ID="tbl" runat="server" BorderWidth="1px" BorderColor="white" CellPadding="0" CellSpacing="0" 
                    CssClass="textColor" Width="100%" HorizontalAlign="Center" BackColor="#0083C1" ForeColor="White">
                    <asp:TableRow ID="TableRow1" runat="server">
                         <asp:TableCell ID="TableCell4" runat="server" Width="6%">                   
                            <asp:Label ID="Label7" Text="Zonal Manager"  runat="server"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell ID="TableCell1" runat="server" Width="6%">                   
                            <asp:Label ID="lblaffixationcenter" Text="Affixation Center"  runat="server"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell ID="TableCell5" runat="server" Width="6%">
                            <asp:Label ID="lblalc" Text="Assign Laser Code" runat="server"></asp:Label>
                        </asp:TableCell>   

                        <asp:TableCell ID="TableCell7" runat="server" Width="6%">
                            <asp:Label ID="lbltodayproduction" Text="TodayProduction" runat="server"></asp:Label>
                        </asp:TableCell> 

                        <asp:TableCell ID="TableCell8" runat="server" Width="5%">
                            <asp:Label ID="lblday1" Text="Day1" runat="server"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell ID="TableCell9" runat="server" Width="5%">
                            <asp:Label ID="lblDay2" Text="Day2" runat="server"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell ID="TableCell10" runat="server" Width="5%">
                            <asp:Label ID="lblDay3" Text="Day3" runat="server"></asp:Label>
                        </asp:TableCell>

                         <asp:TableCell ID="TableCell2" runat="server" Width="5%">
                            <asp:Label ID="lblDay4" Text="Day4" runat="server"></asp:Label>
                        </asp:TableCell>

                         <asp:TableCell ID="TableCell3" runat="server" Width="5%">
                            <asp:Label ID="lblDay5" Text="Day5orMore" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    </asp:Table>--%>
                           
                     <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="400px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
                    <asp:GridView ID="grdthirdplate" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"                         
                        ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px" OnRowCommand="grdthirdplate_RowCommand" OnSelectedIndexChanging="grdthirdplate_SelectedIndexChanging" >
                        <AlternatingRowStyle BackColor="White" />
                    
                        <Columns>      
                             <asp:TemplateField HeaderText="Affixation Center" HeaderStyle-ForeColor="White"  ItemStyle-HorizontalAlign="Left" HeaderStyle-Height="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblzonalmanager" runat="server" Text='<%#Eval("ZonalManager")%>' ></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Left" Width="6%"/>
                            </asp:TemplateField>  
                                                 
                            <asp:TemplateField HeaderText="Affixation Center" HeaderStyle-ForeColor="White" ItemStyle-HorizontalAlign="Left" HeaderStyle-Height="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblembname" runat="server" Text='<%#Eval("EmbossingCenter")%>' ></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Left" Width="6%"/>
                            </asp:TemplateField>                           

                            <asp:TemplateField HeaderText="Assign Laser Code" HeaderStyle-ForeColor="White" HeaderStyle-Height="60px">
                                <ItemTemplate> 
                                    <asp:Label ID="lblassignlasercode" runat="server" Text='<%#Eval("AssignedLasercode")%>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Left" Width="6%"/>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Today Production" HeaderStyle-ForeColor="White" HeaderStyle-Height="60px">
                                <ItemTemplate>                          
                                     <asp:Label ID="lbltodayprod" runat="server" Text='<%#Eval("TodayProd")%>'></asp:Label>         
                                    <%--<asp:LinkButton ID="lbltodayprod" CommandName="TodayProd" runat="server"  Text='<%#Eval("TodayProd") %>'>LinkButton</asp:LinkButton> --%>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Center" Width="6%" />
                            </asp:TemplateField>                           

                            <asp:TemplateField HeaderText="Day1" HeaderStyle-ForeColor="White" HeaderStyle-Height="60px">
                                <ItemTemplate>                                    
                                    <asp:LinkButton ID="lblday1" CommandName="Day1" runat="server" Text='<%#Eval("Day1") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                            </asp:TemplateField>
                          
                            <asp:TemplateField HeaderText="Day2" HeaderStyle-ForeColor="White" HeaderStyle-Height="60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblday2" CommandName="Day2" runat="server" Text='<%#Eval("Day2") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Center" Width="5%"/>
                            </asp:TemplateField>

                              <asp:TemplateField HeaderText="Day3" HeaderStyle-ForeColor="Blue" HeaderStyle-Height="60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblday3" CommandName="Day3" runat="server" Text='<%#Eval("Day3") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Center" Width="5%"/>
                            </asp:TemplateField>

                              <asp:TemplateField HeaderText="Day4" HeaderStyle-ForeColor="White" HeaderStyle-Height="60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblday4" CommandName="Day4" runat="server" Text='<%#Eval("Day4") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Center" Width="5%"/>
                            </asp:TemplateField>
                           
                              <asp:TemplateField HeaderText="Day5orMore" HeaderStyle-ForeColor="White" HeaderStyle-Height="60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblday5" CommandName="Day5orMore" runat="server" Text='<%#Eval("Day5orMore") %>'>LinkButton</asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="White" />
                                <ItemStyle HorizontalAlign="Center" Width="5%"/>
                            </asp:TemplateField>

                        </Columns>

                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#0083c1" Font-Bold="True" ForeColor="White" />
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
    
                                            
</asp:Content>

