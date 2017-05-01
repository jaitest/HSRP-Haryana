<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Year_Month_Wise_AllStateSummaryReport.aspx.cs" Inherits="HSRP.Report.Year_Month_Wise_AllStateSummaryReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

        if (document.getElementById("<%=ddlStateName.ClientID%>").value == "--Select State--") {
            alert("Select State");
            document.getElementById("<%=ddlStateName.ClientID%>").focus();
            return false;
        }


    }
    </script>
    <table style="width:70%;">
        <tr>          
            <td colspan="5">
               <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Year Month Wise Summary Report :</span>
                                    </td>                                   
                                </tr>
                            </table></td>
        </tr>
        <tr>
            <td style="height: 31px" width="100">
                &nbsp;</td>
            <td style="height: 31px">
                <asp:Label ID="lblState" runat="server" Text="State Name : " ForeColor="Black" Font-Bold="True"></asp:Label>
            </td>
            <td style="height: 31px">
                <asp:DropDownList ID="ddlStateName" runat="server" 
                    DataTextField="HSRPStateName" DataValueField="Hsrp_StateId" Width="130px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="ddlStateName" Display="Dynamic" 
                    ErrorMessage="Please Select State" InitialValue="--Select State--"></asp:RequiredFieldValidator>
            </td>
            <td style="height: 31px">
                &nbsp;<asp:Button ID="btnExcel" runat="server" CssClass="button" 
                    onclick="btnExcel_Click" Text="Export to Excel" />
                &nbsp;</td>
            <td style="height: 31px">
                &nbsp;&nbsp;<asp:Button ID="btnSubmit" runat="server" CssClass="button_go" 
                    onclick="btnSubmit_Click" Text="GO" Visible="False" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td colspan="4">
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" Width="100%" Visible="False">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Left" VerticalAlign="Middle" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" VerticalAlign="Middle" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
