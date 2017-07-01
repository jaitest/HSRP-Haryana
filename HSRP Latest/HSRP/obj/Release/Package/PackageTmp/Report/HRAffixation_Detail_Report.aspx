<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HRAffixation_Detail_Report.aspx.cs" Inherits="HSRP.Report.HRAffixation_Detail_Report" %>

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

   <%-- <script type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
        }
    </script>--%>

    <script type="text/javascript">

        function Datefrom_OnDateChange(sender, eventArgs) {
            var fromDate = Datefrom.getSelectedDate();
            CalendarDatefrom.setSelectedDate(fromDate);

        }

        function Datefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarDatefrom.getSelectedDate();
            Datefrom.setSelectedDate(fromDate);

        }

        function Datefrom_OnClick() {
            if (CalendarDatefrom.get_popUpShowing()) {
                CalendarDatefrom.hide();
            }
            else {
                CalendarDatefrom.setSelectedDate(Datefrom.getSelectedDate());
                CalendarDatefrom.show();
            }
        }

        function Datefrom_OnMouseUp() {
            if (CalendarDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function Dateto_OnDateChange(sender, eventArgs)
        { 
            var toDate = Dateto.getSelectedDate();
            CalendarDateto.setSelectedDate(toDate);
        }

        function Dateto_OnChange(sender, eventArgs)
        {
            var toDate = CalendarDateto.getSelectedDate();
            Dateto.setSelectedDate(toDate);
        }

        function Dateto_OnClick()
        {
            if (CalendarDateto.get_popUpShowing()) {
                CalendarDateto.hide();
            }
            else {
                CalendarDateto.setSelectedDate(Dateto.getSelectedDate());
                CalendarDateto.show();
            }
        }

        function Dateto_OnMouseUp()
        {
            if (CalendarDateto.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <div style="width: 1107px; height: 500px;">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="../images/midtablebg.jpg">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <span class="headingmain">HRCollection Report</span>
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
               <td style="float:left;">
                   <asp:Button ID="btnPdfDownload" runat="server" Text="DownloadPdf"/>
               </td>
           </tr>
            <tr>
                <td colspan="8" align="center" id="gridTD" runat="server">
                    <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="750px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
                        <asp:GridView ID="grd" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                            ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px" OnRowCommand="grd_RowCommand">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>  
                                

                                                    <asp:BoundField HeaderText="S.NO" DataField="S.NO" />
                                                    <asp:BoundField HeaderText="rtolocationcode" DataField="rtolocationcode" />
                                                    <asp:BoundField HeaderText="VehicleRegNo" DataField="VehicleRegNo" />
                                                     <asp:BoundField HeaderText="OwnerName" DataField="OwnerName" />
                                                    <asp:BoundField HeaderText="EngineNo" DataField="EngineNo" />
                                                    <asp:BoundField HeaderText="ChassisNo" DataField="ChassisNo" />  
                                                     <asp:BoundField HeaderText="VehicleType" DataField="VehicleType" />
                                                     <asp:BoundField HeaderText="VehicleClass" DataField="VehicleClass" />
                                                    <asp:BoundField HeaderText="TotalAmount" DataField="TotalAmount" />

                        <%-- <asp:TemplateField HeaderText="Day1" HeaderStyle-ForeColor="Blue">
                                            <ItemTemplate>
                                            
                                                 <asp:LinkButton ID="lblDay1" CommandName="Day1" runat="server" Text=<%#Eval("RTOLocationCode") %>>LinkButton</asp:LinkButton>
                                        </ItemTemplate>
                                         </asp:TemplateField>      --%> 
                        <%--        <asp:TemplateField ItemStyle-Width="30px" HeaderText="Location">
                                <ItemTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("Location")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>     
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Users">
                            <ItemTemplate>
                                <asp:Label ID="lblUsers" runat="server"
                                  Text='<%#Eval("Users")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Scooter">
                            <ItemTemplate>
                                <asp:Label ID="lblScooter" runat="server"
                                  Text='<%#Eval("Scooter")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="MotorCycle">
                            <ItemTemplate>
                                <asp:Label ID="lblMotorCycle" runat="server"
                                  Text='<%#Eval("MotorCycle")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="ThreeWheeler">
                            <ItemTemplate>
                                <asp:Label ID="lblThreeWheeler" runat="server"
                                  Text='<%#Eval("ThreeWheeler")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Tractor">
                            <ItemTemplate>
                                <asp:Label ID="lblTractor" runat="server"
                                  Text='<%#Eval("Tractor")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="LMV">
                            <ItemTemplate>
                                <asp:Label ID="lblLMV" runat="server"
                                  Text='<%#Eval("LMV")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="LMVClass">
                            <ItemTemplate>
                                <asp:Label ID="lblLMVClass" runat="server"
                                  Text='<%#Eval("LMVClass")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="MCV/HCV/Trailer">
                            <ItemTemplate>
                                <asp:Label ID="lblMCV_HCV_Trailer" runat="server" Text='<%#Eval("MCV/HCV/Trailer")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblTotal" runat="server"
                                  Text='<%#Eval("Total")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="Total Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblTotal_Amount" runat="server"
                                  Text='<%#Eval("Total Amount")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>                                
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
