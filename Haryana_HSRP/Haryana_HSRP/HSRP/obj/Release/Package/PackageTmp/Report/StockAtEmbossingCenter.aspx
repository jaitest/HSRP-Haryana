<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StockAtEmbossingCenter.aspx.cs" Inherits="HSRP.Report.StockAtEmbossingCenter" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <%--   <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
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
                                
                                
                                <tr id="TR1" runat="server">
                                    <td > 
                                          <asp:Label ID="Label4" class="headingmain" runat="server"  > Report:HSRP Stock Report For HP  </asp:Label> 
                                          
                                    </td> 
                                </tr>

                               
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                  <%--  <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID">
                                        </asp:DropDownList>
                                    </td>--%>
                                   
                              
                                    
                                    <td valign="middle" Visible="false" class="Label_user_batch" nowrap="nowrap" style="width: 110px"><asp:Label Text="Date:"  runat="server" ID="labelDate" /></td>
                                    <td valign="top"   onmouseup="OrderDate_OnMouseUp()" align="justify" style="width: 123px">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server"  PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"  
                                                                    ControlType="Picker" PickerCssClass="picker" Width="80px">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                           
                                                            <td valign="top" align="left" style="width: 124px">
                                                                <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                                     onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>
                                  <%-- <td valign="middle" class="form_text" nowrap="nowrap"> &nbsp;&nbsp;<asp:Label Text="To:"   runat="server" ID="labelTO" /> &nbsp;&nbsp;</td>--%>
                                 <%--  <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()"   align="left">
                                                                <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy" 
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>--%>
                                                      
                                                            <%--<td valign="top" align="left"  Visible="false">
                                                                <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                                    class="calendar_button" src="../images/btn_calendar.gif" />
                                                            </td>--%>


                               

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                       
                                        <asp:Button ID="btnGO"  Width="58px"  runat="server" 
                                            Text="GO" ToolTip="Please Click for Report"
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnGO_Click"  /> &nbsp;&nbsp;&nbsp;  

                                             &nbsp;&nbsp;&nbsp;  
                                         
                                        <asp:Button ID="btnExport"  Width="159px"  runat="server" 
                                            Text="Export To EXCEL" ToolTip="Please Click for Report"
                                            class="button" Height="23px" OnClick="btnExport_Click"  />   
                                         
                                    </td>
                                     
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
                                    <td align="center" style=" padding:10px">
                              <asp:GridView ID="GridView1" runat="server" BackColor="White" AutoGenerateColumns="false"
                                PageSize="25" AllowPaging="false" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" >
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
                                   
                                    <asp:TemplateField HeaderText="S.No">
                                      <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                      </ItemTemplate>
                                      </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                           Embossing center</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblrtolocation" runat="server" Text='<%#Eval("EmbCenterName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                          Dstrict</HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:Label ID="lbldistrict" runat="server" Text='<%#Eval("district") %>'></asp:Label>
                                           
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                          RTo Name</HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:Label ID="lbldistrict" runat="server" Text='<%#Eval("rtolocationname") %>'></asp:Label>
                                           
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                          RTo Code</HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:Label ID="lbldistrict" runat="server" Text='<%#Eval("RTOLocationCode") %>'></asp:Label>
                                           
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                        Opening Stock </HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:Label ID="lblopeningstock" runat="server" Text='<%#Eval("stockinhand") %>'></asp:Label>
                                           
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                        HSRP Applied </HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:Label ID="lblopeningstock" runat="server" Text='<%#Eval("Hsrpapplied") %>'></asp:Label>
                                           
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                        HSRP Embossed </HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:Label ID="lblhsrpembossed" runat="server" Text='<%#Eval("HSRPEmbossed") %>'></asp:Label>
                                           
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                      Minimum Stock </HeaderTemplate>
                                        <ItemTemplate>
                                        
                                            <asp:Label ID="lblMinimumStock" runat="server" Text='<%#Eval("MinimumStock") %>'></asp:Label>
                                           
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>
                                  
                                  
                                   
                                    
                                </Columns>
                            </asp:GridView>
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
