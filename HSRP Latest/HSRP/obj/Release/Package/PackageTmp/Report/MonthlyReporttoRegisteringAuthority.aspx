<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MonthlyReporttoRegisteringAuthority.aspx.cs" Inherits="HSRP.Report.MonthlyReporttoRegisteringAuthority" %>
<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />



    
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Monthly Report To Registering Authority</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            </table>

   
    <table style="width: 100%; height: 51px;">
        <tr>
            <td style="height: 25px;" width="20">
                &nbsp;</td>
            <td style="height: 25px; width: 80px;" valign="bottom">
                
                        <asp:Label ID="labelOrganization1" runat="server" Font-Bold="True" 
                            ForeColor="Black" Text="HSRP State:" Width="80px" />
                            </td>
            <td style="height: 25px; width: 266px;" valign="bottom">
                
                        <asp:DropDownList ID="ddlState" runat="server" 
                            CausesValidation="false" DataTextField="HSRPStateName" 
                            DataValueField="HSRP_StateID" Height="22px" 
                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" 
                            Width="125px" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="ddlState" ErrorMessage="Select State" 
                            InitialValue="--Select State--" Width="70px"></asp:RequiredFieldValidator>
                            </td>
                            <td style="height: 25px; color: #000000;" valign="middle" 
                width="90">
                
                                Year :</td>
            <td style="height: 25px;" valign="middle" width="195">
                
                        <asp:DropDownList ID="ddlYear" runat="server">
                        </asp:DropDownList>
            </td>
                        <td width="30" style="color: #000000;">
                            Month :</td>
                        <td width="100">
                            <asp:DropDownList ID="ddlMonth" runat="server">
                                <asp:ListItem Value="1">January</asp:ListItem>
                                <asp:ListItem Value="2">February</asp:ListItem>
                                <asp:ListItem Value="3">March</asp:ListItem>
                                <asp:ListItem Value="4">April</asp:ListItem>
                                <asp:ListItem Value="5">May</asp:ListItem>
                                <asp:ListItem Value="6">June</asp:ListItem>
                                <asp:ListItem Value="7">July</asp:ListItem>
                                <asp:ListItem Value="8">August</asp:ListItem>
                                <asp:ListItem Value="9">September</asp:ListItem>
                                <asp:ListItem Value="10">October</asp:ListItem>
                                <asp:ListItem Value="11">November</asp:ListItem>
                                <asp:ListItem Value="12">December</asp:ListItem>
                            </asp:DropDownList>
            </td>
           
                        
                                                <td style="height: 25px" width="40">
                                                    
                        <asp:Button ID="btnexport" runat="server" Font-Bold="True" ForeColor="#3333FF" 
                            onclick="btnexport_Click" Text="Export" Width="70px" />
                   
            </td>
        </tr>
        <td width="20">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td colspan="2">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
        </td>
        <tr>
             <td width="20">
                                                    &nbsp;</td>
             <td>
                                                    &nbsp;</td>
             <td colspan="5">
                                                    &nbsp;</td>
             <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td width="20">
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td colspan="2">
                                                    &nbsp;</td>
        </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
    
                                            
</asp:Content>

