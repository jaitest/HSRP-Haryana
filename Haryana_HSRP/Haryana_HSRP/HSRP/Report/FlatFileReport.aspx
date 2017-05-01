<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FlatFileReport.aspx.cs" Inherits="HSRP.Report.FlatFileReport" Title="Flat File Report" %>

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
    function validate() {



        if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
            alert("Please Select State");
            document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
            return false;
        }
        if (document.getElementById("<%=DropDownList1.ClientID%>").value == "--Select Type--") {
            alert("Please Select Type");
            document.getElementById("<%=DropDownList1.ClientID%>").focus();
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
                                        <span class="headingmain">&nbsp; Flat File Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr> 
                                <td></td>
                                 <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                    </td>

                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged"  >                                          
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:Label Text="Select Type:" Visible="true" runat="server" ID="label1" />
                                    </td>
                                   <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownList1"
                                            runat="server" >
                                            <asp:ListItem>--Select Type--</asp:ListItem>
                                            <asp:ListItem>Both</asp:ListItem>
                                            <asp:ListItem>Data Matched By Location</asp:ListItem>
                                            <asp:ListItem>Data Matched without Location</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="color: #828282; font: 11pt tahoma,arial,verdana; text-decoration: none; width: 97px;";> 
                                        
                                    </td>
                                    <td valign="top" style="width:105px"; onmouseup="OrderDate_OnMouseUp()">
                                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                                                <asp:Button ID="btngo" runat="server" CssClass="button" Height="29px" 
                                                                    onclick="btngo_Click" Text="Go" Width="60px" OnClientClick=" return validate()"/>
                                                            </td>
                                    
                                    <td style="Position:relative; right:0px; top: 0px;"> 
                                        
                                        <asp:Button ID="btnExportToExcel" runat="server" Text="Report In Excel" ToolTip="Please Click for Report"
                                            class="button"  
                                            onclick="btnExportToExcel_Click"  OnClientClick=" return validate()"  />
                                    </td>
                                    <tr>
                                    <td colspan="8">
                                    <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                    </td>
                                    </tr>

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                               
                                 
                            </table>
                        </td>
                    </tr>
                    <tr>
                                                <td colspan="6">
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
                                                        Height="103px" onrowdatabound="GridView1_RowDataBound" 
                                                        Width="100%">
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <EditRowStyle BackColor="#2461BF" />
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                                                            HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                    </asp:GridView>
                                                    <br />
                                                </td>
                                            </tr> 
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

