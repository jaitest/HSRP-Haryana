<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="RemoveLaser.aspx.cs" Inherits="HSRP.Report.RemoveLaser" %>

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
                              &nbsp;<span class="headingmain">Make Laser Free</span>&nbsp;</td>
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




                                            <td valign="middle" class="form_text" nowrap="nowrap" style="width: 150px">
                                                Select Laser Type:
                                            </td>
                                           
                                            <td style="width: 168px">
                                            <asp:DropDownList ID="ddllasercode" runat="server" Visible="true" 
                                                    AutoPostBack="true" 
                                                    Height="20px" 
                                                    Width="100px" onselectedindexchanged="ddllasercode_SelectedIndexChanged" >
                                           
                                               <asp:ListItem  Text = "--Select--" Value = "0"></asp:ListItem>
                                                    <asp:ListItem Text = "Both" Value = "1"></asp:ListItem>
                                                    <asp:ListItem Text = "Front" Value = "2"></asp:ListItem>
                                                    <asp:ListItem Text = "Rear" Value = "3"></asp:ListItem>
                                                    
                                                    </asp:DropDownList>
                                            </td>
                                                <td>
                                              </td>

                                               <td valign="middle" class="form_text" nowrap="nowrap" style="width: 119px">
                                                  &nbsp; &nbsp; Remarks:  
                                    </td>
                                    <td valign="middle" class="Label_user_batch" style="width: 119px">
                                                    &nbsp;
                                     <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="186px"></asp:TextBox> 
                                     </td>


                                    <td valign="middle" class="form_text" nowrap="nowrap" style="width: 119px">
                                                  &nbsp; &nbsp; HSRPRecordID:  
                                    </td>
                                    <td valign="middle" class="Label_user_batch" style="width: 119px">
                                                    &nbsp;
                                     <asp:TextBox ID="txtHSRPRecordID" runat="server" TextMode="MultiLine" Width="186px"></asp:TextBox> 
                                     </td>
                                      <td valign="middle" class="Label_user_batch" style="width: 119px">
                                                    &nbsp;&nbsp;
                                                    &nbsp;&nbsp;
                                         <asp:Button ID="Button1" runat="server" class=button Text="Update" Width="100px" 
                                                        onclick="Update_Click" ></asp:Button>

                                                
                                                </td>
                                                
                                                <td valign="middle" class="form_text" nowrap="nowrap" 
                                        style="width: 59px">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td valign="middle" onmouseup="HSRPAuthDate_OnMouseUp()" align="left" style="width: 120px">
                                                    &nbsp;</td>
                                                <td valign="middle" class="Label_user_batch" style="width: 120px">
                                                    &nbsp;</td>

                                    <td>
                                        
                                        &nbsp;</td>
                                    <td></td>
                                    <td>
                                        &nbsp;</td>

                                   
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td style="width: 191px" >
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td style="width: 191px">
                                        
                                        <asp:Label ID="lblSucessMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                        
                                    </td>
                                </tr>
                                <tr>
                                <td style="width: 191px">
                                    <span class="headingmain">
                                    &nbsp;</span></td>
                                
                                <td style="width: 150px">
                                    &nbsp;</td>
                                
                                <td>
                                <%--<asp:Button ID="btnInsert" runat="server" class=button Text="Push" Width="100px"  
                                        onclick="btnPush_Click"></asp:Button>--%>
                                </td>
                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 114px">
                                    &nbsp;</td>
                                 <td style="width: 121px">
                                     &nbsp;</td>
                                
                                <td>
                                    &nbsp;</td>
                                </tr>
                                <br />
                                <tr>
                               
                                   
                                     <td align="left" style="width: 191px"><br />
 
                                    </td>
                                   
                                          <td valign="top" colspan="1">

                                         
                                <%--<asp:GridView ID="grdpending" runat="server" CellPadding="5" ForeColor="#333333"  AutoGenerateColumns="false"
                                        GridLines="None" Width="350px">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                                    <asp:BoundField HeaderText="STATENAME" DataField="HSRPStateName"  />
                                                    <asp:BoundField HeaderText="HSRP" DataField="Column1" />
                                                    <asp:BoundField HeaderText="ERP" DataField="Column2" />
                                                    <asp:BoundField HeaderText="PENDING" DataField="Column3" />
                                                    <asp:BoundField HeaderText="PFA" DataField="Column4" />
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
                                    
                                    </asp:GridView>--%>
                                        
                                    </td>

                                </tr>
                               

                                
                                <tr>
                                                 <td style="width: 191px">
                                                     &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 191px">
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
           <td >
                                        <asp:Label ID="lblSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                    </td>
</asp:Content>
