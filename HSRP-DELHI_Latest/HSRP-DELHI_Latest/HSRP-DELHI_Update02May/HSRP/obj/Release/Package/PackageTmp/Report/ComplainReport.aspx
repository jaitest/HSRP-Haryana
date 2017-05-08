<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ComplainReport.aspx.cs" Inherits="HSRP.Report.ComplainReport" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

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

     <script type="text/javascript">
//         function AddNewPop(a,b,c,d,e,f,g,h,i,j) { //Define arbitrary function to run desired DHTML Window widget codes

//             //googlewin = dhtmlwindow.open("googlebox", "iframe", "UpdateSubmitRequest.aspx?StateId=" + i + "&RTOID=" + j + "&SubmitID=" + k, "Update Request", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
//             googlewin = dhtmlwindow.open("googlebox", "iframe", "ViewComplainReport.aspx?strcomplaindatetime=" + a + "&strname=" + b + "&strmobileNo=" + c + "&stremailid=" + d + "&strRegNo=" + e + "&strEngineNo=" + f + "&strChessisNo=" + g + "&strRemarks=" + h + "&strStatus=" + i + "&strSolution=" + j, "View Complaint", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
//             googlewin.onclose = function () {
//                 window.location = 'ComplainReport.aspx';
//                 return true;
//             }
         //         }
              function AddNewPop(a) { //Define arbitrary function to run desired DHTML Window widget codes

             //googlewin = dhtmlwindow.open("googlebox", "iframe", "UpdateSubmitRequest.aspx?StateId=" + i + "&RTOID=" + j + "&SubmitID=" + k, "Update Request", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
             googlewin = dhtmlwindow.open("googlebox", "iframe", "ViewComplainReport.aspx?strComplaintID=" + a, "View Complaint", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
             googlewin.onclose = function () {
                 window.location = 'ComplainReport.aspx';
                 return true;
             }
         }


         function AddNewPopAction(z) { //Define arbitrary function to run desired DHTML Window widget codes

             //googlewin = dhtmlwindow.open("googlebox", "iframe", "UpdateSubmitRequest.aspx?StateId=" + i + "&RTOID=" + j + "&SubmitID=" + k, "Update Request", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
             googlewin = dhtmlwindow.open("googlebox", "iframe", "ComplainReportResolution.aspx?strComplaintID=" + z, "Add Resolution ", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
             googlewin.onclose = function () {
                 window.location = 'ComplainReport.aspx';
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
                              <span class="headingmain">View Complaint Report </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap" style="width: 114px">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" style="width: 123px">
                                      <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate> 
                                    
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            >
                                        </asp:DropDownList>
                                    
                                     </ContentTemplate>
                                     </asp:UpdatePanel>
                                     </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap" style="width: 54px">
                                                    <asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />
                                    </td>
                                    <td valign="middle" class="Label_user_batch" style="width: 119px">
                                                    <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                    </td>
                                      <td valign="middle" class="form_text" nowrap="nowrap" style="width: 44px">
                                                    &nbsp;&nbsp;<img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                
                                                <td valign="middle" class="form_text" nowrap="nowrap" 
                                        style="width: 59px">
                                                    &nbsp;&nbsp;<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="top" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" style="width: 120px">
                                                    <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                </td>
                                                <td valign="top" align="left" style="width: 70px">
                                                    <img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                        class="calendar_button" src="../images/btn_calendar.gif" />
                                                </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button"  OnClientClick=" return validate()"
                                            onclick="ButtonGo_Click" /> &nbsp;  
                                         
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
                                       
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                                            CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id" 
                                            onrowdatabound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" >
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                
                                                <asp:BoundField DataField="ComplaintNo" HeaderText="ComplaintNo" />
                                                 <asp:BoundField DataField="ComplaintDate" HeaderText="ComplaintDate" />
                                                <asp:BoundField DataField="OwnerName" HeaderText="Name" />
                                                <asp:BoundField DataField="MobileNo" HeaderText="MobileNo" />
                                                <asp:BoundField DataField="Email" HeaderText="EmailID" />
                                                <asp:BoundField DataField="Regno" HeaderText="RegNo" />
                                                <asp:BoundField DataField="ChasisNo" HeaderText="ChassisNo" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Complaint"  />
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:BoundField DataField="Solution" HeaderText="Resolution" />
                                                <asp:TemplateField HeaderText="View">
                                                <ItemTemplate>
                                                
                                                
                                                <asp:LinkButton ID="ViewRequest" runat="server" Text="View" Visible="false"  CommandName="ViewUpdateRquest" OnClick="Linkbutton1_Click"></asp:LinkButton>
                                        
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                            
                                                 <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>

                                                
                                                
                                                <asp:LinkButton ID="AddResolution" runat="server" Text="Action" Visible="false" CommandName="ViewUpdateRquest" OnClick="Linkbutton2_Click"></asp:LinkButton>
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
           <td >
                                        <asp:Label ID="lblSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                    </td>
</asp:Content>
