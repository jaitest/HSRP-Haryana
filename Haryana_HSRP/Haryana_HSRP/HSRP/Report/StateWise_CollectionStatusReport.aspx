<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StateWise_CollectionStatusReport.aspx.cs" Inherits="HSRP.Report.StateWise_CollectionStatusReport" %>
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
    <table style="width:100%;">
        <tr>          
            <td colspan="8">
               <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">State And Month Wise Collection Report :</span>
                                    </td>                                   
                                </tr>
                            </table></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblState" runat="server" Text="State Name : " ForeColor="Black" Font-Bold="True"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlStateName" runat="server" 
                    DataTextField="HSRPStateName" DataValueField="Hsrp_StateId">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblYear" runat="server" Text="Year :" ForeColor="Black" 
                    Font-Bold="True"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" 
                    DataTextField="HSRPStateName" DataValueField="Hsrp_StateId">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblMonth" runat="server" Text="Month : " ForeColor="Black" 
                    Font-Bold="True"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;&nbsp;<asp:Button ID="btnSubmit" runat="server" CssClass="button_go" 
                    onclick="btnSubmit_Click" Text="GO" />
            </td>
            <td>
                &nbsp;&nbsp;<asp:Button ID="btnExcel" runat="server" CssClass="button" 
                    onclick="btnExcel_Click" Text="Excel" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="8">
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
                    Width="100%">
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
