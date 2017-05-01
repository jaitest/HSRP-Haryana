<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HRDealerProcesStatus.aspx.cs" Inherits="HSRP.Report.HRDealerProcesStatus" %>


<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript">
        var timeintervel = setInterval("reloadpage()", 36000)
        function reloadpage() {
            location.reload();
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
                              &nbsp;<span class="headingmain"> HR Dealer Process Status </span>&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    
                                   
                                    <td valign="middle" class="form_text" nowrap="nowrap" style="width: 119px">
                                                  &nbsp &nbsp  <%--<asp:Label Text="From:" Visible="true" runat="server" ID="labelDate" />--%>
                                    </td>
                                    <td valign="middle" class="Label_user_batch" style="width: 119px">
                                                    <%--<ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>--%>
                                    </td>
                                      <td valign="middle" class="Label_user_batch" style="width: 119px">
                                                    &nbsp;&nbsp;<%--<img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />--%>
                                                    &nbsp;&nbsp;
                                                </td>
                                                
                                                <td valign="middle" class="form_text" nowrap="nowrap" 
                                        style="width: 59px">
                                                    &nbsp;&nbsp;<%--<asp:Label Text="To:" Visible="true" runat="server" ID="labelTO" />--%>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="middle" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" style="width: 120px">
                                                    <%--<ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>--%>
                                                </td>
                                                <td valign="middle" class="Label_user_batch" style="width: 120px">
                                                    <%--<img id="ImgPollution" tabindex="4" alt="" onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()"
                                                        class="calendar_button" src="../images/btn_calendar.gif" />--%>
                                                </td>

                                    <td>
                                        
                                        &nbsp;</td>
                                    <td></td>
                                    <td>
                                    <%--<asp:Button ID="btnGo" runat="server" Text="Go" class="button"  Width="100px" onclick="btnGo_Click"></asp:Button>--%>
                                    </td>

                                   
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td style="width: 191px" >
                                        <%--<asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />--%>
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td style="width: 191px">
                                        
                                        <%--<asp:Label ID="lblSucessMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />--%>
                                        
                                    </td>
                                </tr>
                                <tr>
                                <td style="width: 191px">
                                    &nbsp;</td>
                                     <tr>
                                     <td style="width: 25%" align="center" style=" padding:10px" valign="top" >
                              <asp:GridView ID="GrdAP" runat="server" BackColor="White" AutoGenerateColumns="false"
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
                                            District</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDistrict" runat="server" Text='<%#Eval("District") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                  


                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            HR Locations</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbllocation" runat="server" Text='<%#Eval("Location") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField>
                                        <HeaderTemplate>
                                            Dealer Name</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerName" runat="server" Text='<%#Eval("DealerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            HR Orders</HeaderTemplate>
                                        <ItemTemplate>
                                          
                                           <asp:Label ID="lblcollection" runat="server" Text='<%#Eval("VehicleNo") %>'></asp:Label>
 
                                           
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Amount</HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:Label ID="lblamount" runat="server" Text='<%#Eval("Amount") %>'></asp:Label>
                                            
                                          
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                                    </td>

                                    &nbsp;,&nbsp;


                                    <td style="width: 25%" align="center" style=" padding:10px" valign="top">
                              <asp:GridView ID="GridShowtotal" runat="server" BackColor="White" AutoGenerateColumns="false"
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

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Total Orders</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalOrders" runat="server" Text='<%#Eval("TotalOrders") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Total Amount</HeaderTemplate>
                                        <ItemTemplate>
                                          
                                           <asp:Label ID="lblTotalAmount" runat="server" Text='<%#Eval("TotalAmount") %>'></asp:Label>
 
                                         
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                   
                                </Columns>
                            </asp:GridView>




                                    </td>
                                    &nbsp,&nbsp
                                     


                                    <td style="width: 25%" align="center" style=" padding:10px" valign="top">

                            <br />
                             

                            <br />

                            

                                    </td>

                                   <td style="width: 25%" align="center" style=" padding:10px" valign="top">




                            <br />

                            <br />

                                    </td>
                                



                                    <tr>

                                    <td style="width: 25%" align="center" style=" padding:10px" valign="top">
                             
                                    </td>


                                   <td style="width: 25%" align="center" style=" padding:10px" valign="top">
                              
                                    </td>

                               
                                    </tr>


                               
                                </tr>
                                    
                                
                                <td style="width: 150px">
                                    &nbsp;</td>
                                
                                <td>
                                    &nbsp;</td>
                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 114px">
                                    &nbsp;</td>
                                 <td style="width: 121px">
                                     &nbsp;</td>
                                
                                <td>
                                    <br />
                                </td>
                                </tr>
                                <br />
                                <tr>
                               
                                   
                                 

                                     


                                    <td valign="middle" class="form_text" nowrap="nowrap" style="width: 114px">
                                    </td>
                                   
                                       

                                </tr>
                               

                                
                                <tr>
                                                 <td style="width: 191px">
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
                        <td style="width: 191px">
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
                                        <asp:Label ID="lblerror" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                    </td>
</asp:Content>
