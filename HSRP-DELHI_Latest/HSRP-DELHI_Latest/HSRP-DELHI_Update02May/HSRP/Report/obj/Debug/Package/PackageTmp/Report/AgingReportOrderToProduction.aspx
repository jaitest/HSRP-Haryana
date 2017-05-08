<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AgingReportOrderToProduction.aspx.cs" Inherits="HSRP.Report.AgingReportOrderToProduction" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script src="../javascript/bootstrap.js" type="text/javascript"></script>   


   
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

        function editpage(i,j,k,l) { //Define arbitrary function to run desired DHTML Window widget codes
            // alert(i);
            googlewin = dhtmlwindow.open("googlebox", "iframe", "AgingReportOrderToProductionDetail.aspx?CommandName=" + i+"&StateName="+j+"&Location=" + k+"&Date="+l, "Aging Report Order To Production Details", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //window.location.href = "AgingReportOrderToProduction.aspx";
                return true;
            }
        } 

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
        }
        function PrintGridData() {
            var prtGrid = document.getElementById("Grid");
            prtGrid.border = 0;
            var prtwin = window.open('', 'PrintGridViewData', 'left=100,top=100,width=1000,height=1000,tollbar=0,scrollbars=1,status=0,resizable=1');
            prtwin.document.write(prtGrid.outerHTML);
            prtwin.document.close();
            prtwin.focus();
            prtwin.print();
            prtwin.close();
        }
        
    </script>
 
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
  
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                         <tr id="TR1" runat="server">
                                    <td > 
                                          <asp:Label ID="Label4" class="headingmain" runat="server"  >Aging Report Order To Production</asp:Label> 
                                    </td> 
                                </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap" style="width: 97px">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> 
                                    </td>
                                    <td valign="bottom"  style="width: 112px">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" >
                                       
                                        </asp:DropDownList>
                                    </td>
                               
                                    <td valign="middle"  Visible="false" class="Label_user_batch" nowrap="nowrap" 
                                        style="width: 69px">&nbsp;&nbsp; <asp:Label Text="Date" runat="server" 
                                            ID="labelDate" /> &nbsp;&nbsp;</td>
                                    <td valign="bottom"   onmouseup="OrderDate_OnMouseUp()" align="left" 
                                        style="width: 135px">
                                                                <ComponentArt:Calendar ID="OrderDate" runat="server"  PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker" Height="26px">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>
                                                            </td>
                                                           
                                                            <td align="left" valign="middle">
                                                                <img id="calendar_from_button"  alt="" onclick="OrderDate_OnClick()"
                                                                     onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                        <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button" 
                                                                    OnClientClick=" return validate()" onclick="LinkbuttonSearch_Click"    /> 
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
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" 
                                            ForeColor="#FF3300" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width:100%;">
                                            <tr>
                                                <td colspan="3" align="center" class="form_text" nowrap="nowrap">
                                                    <asp:Label ID="Label7" runat="server" Text="Aging Report Order To Production" 
                                                        Visible="False"></asp:Label>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center" class="form_text" nowrap="nowrap">
                                                    <asp:Label ID="lblReportDate" runat="server" Text="" ></asp:Label>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center" class="form_text" nowrap="nowrap">
                                                    <asp:Label ID="lblGeneratedDate" runat="server" Text="" ></asp:Label>
                                                </td>
                                                
                                            </tr>
                                   

                                            <tr>
                                                <td colspan="3">
                                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
                                            GridLines="None" Width="100%" AutoGenerateColumns="False" 
                                            onrowcommand="GridView1_RowCommand" 
                                            onselectedindexchanged="GridView1_SelectedIndexChanged">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                             <asp:TemplateField HeaderText="Sno">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("Sno") %>'></asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RtoLocationName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("RtoLocationName") %>'></asp:Label>
                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="7 days">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton2" CommandName="7 days" runat="server" Text='<%# Eval("7 days") %>'>LinkButton</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="15 days">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton3" CommandName="15 days" runat="server"  Text='<%# Eval("15 days") %>'>LinkButton</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="30 days">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton4" CommandName="30 days" runat="server"  Text='<%# Eval("30 days") %>'>LinkButton</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="60 days">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton5" CommandName="60 days" runat="server"   Text='<%# Eval("60 days") %>'>LinkButton</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="More Than 60 days">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton6" CommandName=" More Than 60 days" runat="server"  Text='<%# Eval("Morethan 60 days") %>'>LinkButton</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" HorizontalAlign="Center" 
                                                VerticalAlign="Middle" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                                HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                                HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" VerticalAlign="Middle" />
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
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
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

