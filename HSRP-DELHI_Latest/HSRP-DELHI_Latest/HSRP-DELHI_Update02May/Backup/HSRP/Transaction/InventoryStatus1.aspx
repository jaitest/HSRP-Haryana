<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="InventoryStatus1.aspx.cs" Inherits="HSRP.Transaction.InventoryStatus1" %>

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
        function editpage(i) { //Define arbitrary function to run desired DHTML Window widget codes
            //  alert(i);
            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPRecords.aspx?Mode=Edit&HSRPRecordID=" + i, "Update HSRP Record Details", "width=1050px,height=585px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }


        function LaserAssignViewpage(i) { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCodeView.aspx?Mode=Edit&HSRPRecordID=" + i, "View HSRP Record Details", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }

        function PrintChalan(i, S) {
            //Define arbitrary function to run desired DHTML Window widget codes
            //            alert("Hello");
            googlewin = dhtmlwindow.open("googlebox", "iframe", "PrintChalan.aspx?Mode=PrintChalan&HSRPRecordID=" + i + "&Status=" + S + "", "Print Invoice", "width=400px,height=75px,resize=1,scrolling=1,center=2", "recal")
            //            googlewin.onclose = function () {
            //                window.location.href = "ViewHSRPRecords.aspx";
            //                return true;
            //            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPRecords.aspx?Mode=New", "Add HSRP Record Datails", "width=1050px,height=585px,resize=1,scrolling=1,center=1", "recal")
            //            googlewin.onclose = function () {
            //                window.location = 'ViewHSRPRecords.aspx';
            //                return true;
            //            }
        }
    </script>
    <%--<td valign="middle">
                                                <asp:LinkButton ID="ButtonPDF" runat="server" Text="PDF" class="button" OnClientClick=" return validate()" OnClick="ButtonPDF_Click" />
                                            </td>--%>
    <script type="text/javascript" language="javascript">


        function validate() {

            if (document.getElementById("<%=dropDownListHSRPState.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=dropDownListHSRPState.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropDownListRTOLocation.ClientID%>").value == "--Select RTO Location--") {
                alert("Please Select RTOLocation");
                document.getElementById("<%=dropDownListRTOLocation.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlprefix.ClientID%>").value == "--Select Prefix--") {
                alert("Please Select Prefix");
                document.getElementById("<%=ddlprefix.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtlaserNo.ClientID%>").value == "") {
                alert("Please Provide Laser No");
                document.getElementById("<%=txtlaserNo.ClientID%>").focus();
                return false;
            }
         


        }

        function vali() {

            if (document.getElementById("<%=ddltype.ClientID%>").value == "--Select  Type--") {
                alert("Please Select Type");
                document.getElementById("<%=ddltype.ClientID%>").focus();
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
                                        <span class="headingmain">Laser Code Wise Stock</span>
                                        <%--  <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="dropDownListHSRPState" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                            <ContentTemplate>--%>
                                    </td>
                                    <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%><%--   <asp:UpdatePanel runat="server" ID="UpdateUser" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="dropDownListRTOLocation" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                            <ContentTemplate>--%>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="left" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelHSRPState" />
                                    </td>
                                    <td valign="middle" style="width: 94px">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="dropDownListHSRPState"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="dropDownListHSRPState_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" colspan="3">
                                        <%--<td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;&nbsp;
                                        <asp:Label Text="To:" runat="server" ID="labelDate" />
                                        &nbsp;&nbsp;
                                    </td>--%>
                                                <asp:Label Text="Location:" Visible="true" runat="server" ID="labelRTOLocation" AssociatedControlID="dropDownListHSRPState" />&nbsp;&nbsp;
                                                <asp:DropDownList ID="dropDownListRTOLocation" CausesValidation="false" AutoPostBack="true"
                                                    runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID"
                                                    OnSelectedIndexChanged="dropDownListRTOLocation_SelectedIndexChanged" EnableTheming="False">
                                                </asp:DropDownList>
                                        <%--<td valign="middle" class="form_text" nowrap="nowrap">
                                    
                                    </td>--%>
                                    </td>
                                    <td valign="middle" class="form_text" style="width: 185px" colspan="2">
                                        <%-- <td valign="middle">
                                                <asp:LinkButton ID="ButtonExcel" runat="server" Text="Excel" class="button" OnClientClick=" return validate()"
                                                    OnClick="ButtonExcel_Click" />
                                                
                                            </td>--%>
                                                <asp:Label Text="Prefix:" Visible="True" runat="server" ID="lblPrefix" AssociatedControlID="dropDownListRTOLocation" />&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlprefix" runat="server" DataTextField="prefix" DataValueField="prefix"
                                                    Visible="False">
                                                </asp:DropDownList>
                                        <%-- <asp:BoundField DataField="AutoId" HeaderText="AutoId" InsertVisible="False" ReadOnly="True"
                                                SortExpression="AutoId" />--%>
                                    </td>
                                    <%--<asp:BoundField DataField="PlantId" HeaderText="PlantId" SortExpression="PlantId" />
                                            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />--%><%-- <asp:BoundField DataField="AutoId" HeaderText="AutoId" InsertVisible="False" ReadOnly="True"
                                                SortExpression="AutoId" />--%>
                                    <td class="form_text" colspan="2">
                                        <asp:Label Text="Laser No" runat="server" ID="lblLaserNo" />
                                        <asp:TextBox ID="txtlaserNo" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="form_text">
                                        <asp:Label Text="Records" runat="server" ID="lblRecord" />&nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlrecord" runat="server" 
                                            onselectedindexchanged="ddlrecord_SelectedIndexChanged">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>30</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <%--<asp:BoundField DataField="PlantId" HeaderText="PlantId" SortExpression="PlantId" />
                                            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />--%>
                                    <td class="form_text">
                                        &nbsp;</td>
                                    <td class="form_text">
                              
                                            <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button" OnClientClick=" return validate()"
                                                OnClick="LinkbuttonSearch_Click" />
                              
                                    </td>
                                    <%-- <asp:BoundField DataField="AutoId" HeaderText="AutoId" InsertVisible="False" ReadOnly="True"
                                                SortExpression="AutoId" />--%><%--<asp:BoundField DataField="PlantId" HeaderText="PlantId" SortExpression="PlantId" />
                                            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />--%>
                                </tr>
                                <tr>
                                    <td colspan="12" align="center" valign="middle">
                                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                                            AutoGenerateColumns="False" Width="100%" 
                                            onpageindexchanging="GridView1_PageIndexChanging">
                                            <AlternatingRowStyle BackColor="White" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Right" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                            <Columns>
                                                <%-- <asp:BoundField DataField="AutoId" HeaderText="AutoId" InsertVisible="False" ReadOnly="True"
                                                SortExpression="AutoId" />--%>
                                                <%--<asp:BoundField DataField="PlantId" HeaderText="PlantId" SortExpression="PlantId" />
                                            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />--%>
                                                <%--   <asp:BoundField DataField="Lasered" HeaderText="Lasered" 
                                                SortExpression="Lasered" />--%>
                                                <%--  <asp:BoundField DataField="Remarks" HeaderText="Remarks" 
                                                SortExpression="Remarks" />
                                            <asp:BoundField DataField="Serialno" HeaderText="Serialno" 
                                                SortExpression="Serialno" />--%>
                                                <%--   <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" 
                                                SortExpression="CreationDate" />--%>
                                                <%--   <asp:BoundField DataField="InvoiceNo" HeaderText="InvoiceNo" 
                                                SortExpression="InvoiceNo" />--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Select
                                                        <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" /></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CHKSelect" runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Serial No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="id" runat="server" Text='<%#Eval("AutoId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Laser No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LaserNo" runat="server" Text='<%#Eval("laserno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Inventory Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="InventoryStatus" runat="server" Text='<%#Eval("inventorystatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RegNo">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <tr>
                                            <td align="left">
                                                
                                                <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300"
                                                    Font-Bold="True" />
                                            </td>
                                            <td style="width: 94px">
                                            
                                                
                                            <asp:DropDownList ID="ddltype" runat="server" Visible="False"  Font-Size="Small">
                                                    <asp:ListItem>--Select  Type--</asp:ListItem>
                                                    <asp:ListItem>Blank</asp:ListItem>
                                                    <asp:ListItem>Embossed</asp:ListItem>
                                                    <asp:ListItem>Rejected</asp:ListItem>
                                                </asp:DropDownList>
                                            
                                                
                                            </td>
                                            <td align="left">
                                                </td>
                                            <td style="width: 185px" colspan="2">
                                                &nbsp;
                                            </td>
                                            <td style="width: 185px">
                                                &nbsp;</td>
                                            <td style="width: 185px">
                                                &nbsp;</td>
                                            <td colspan="4" align="right">
                                              <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
                                                    OnClick="btnSave_Click" Visible="False" OnClientClick=" return vali()" />

                                            </td>
                                        </tr>
                                        
                                    </td>
                                </tr>
                                        <tr>
                                            <td align="left">
                                                &nbsp;</td>
                                            <td style="width: 94px">
                                            
                                                
                                                &nbsp;</td>
                                            <td align="center">
                                                &nbsp;</td>
                                            <td style="width: 185px">
                                                &nbsp;</td>
                                            <td style="width: 185px">
                                                &nbsp;</td>
                                            <td style="width: 185px">
                                                <asp:Label ID="LblMessage" runat="server" Font-Bold="true" ForeColor="Blue" 
                                                    Text=""></asp:Label>
                                            </td>
                                            <td style="width: 185px">
                                                &nbsp;</td>
                                            <td style="width: 185px">
                                                &nbsp;</td>
                                            <td style="width: 185px">
                                                &nbsp;</td>
                                            <td style="width: 185px">
                                                &nbsp;</td>
                                        </tr>
                                        
                               <tr>
                              <%-- <td align="right">
                               &nbsp;&nbsp;&nbsp;
                               <asp:Button ID="Button1" runat="server" Text="Save" CssClass="button" 
                                                    OnClick="btnSave_Click" Visible="False" OnClientClick=" return vali()" />
                               </td>--%>
                               </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="Label2" runat="server" Text="0" Visible="False"></asp:Label>
                                        <asp:Label ID="lblLaserNoUsed" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" runat="server" Text="Laser No Used:" 
                                            Visible="False"></asp:Label>
&nbsp;<asp:Label ID="lblDuplicateLaserNo" runat="server" Text="Label" Visible="False" ForeColor="Blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
