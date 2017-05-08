<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ABYearWiseCollection.aspx.cs" Inherits="HSRP.Report.ABYearWiseCollection" %>
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
                                        <span class="headingmain">Total Vehicle Count</span>
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

   
    <table style="width: 95%; height: 51px;">
        <tr>
            <td style="height: 59px;" width="100">
                &nbsp;</td>
            <td style="height: 59px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="labelOrganization1" runat="server" Font-Bold="True" 
                            ForeColor="Black" Text="HSRP State:" Width="100px" />
                        <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="True" 
                            CausesValidation="false" DataTextField="HSRPStateName" 
                            DataValueField="HSRP_StateID" Height="22px" 
                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" 
                            Width="148px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="ddlState" ErrorMessage="Select State" 
                            InitialValue="--Select State--" Width="100px"></asp:RequiredFieldValidator>
                        <asp:Label ID="labelDate" runat="server" Font-Bold="True" ForeColor="Black" 
                            Text="Year" Width="100px" />
                        <asp:DropDownList ID="ddlYear" runat="server" CausesValidation="false" 
                            DataTextField="Year" DataValueField="Year" Height="22px" 
                           
                            Width="148px">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
                                                <td style="height: 59px">
                                                    
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export To Excel" 
                                                        Font-Bold="True" ForeColor="#3333FF"/>
                                                   
                                                </td>
        </tr>
        <td>
            &nbsp;</td>
        <td>
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
        </td>
        <tr>
             <td>
                                                    &nbsp;</td>
             <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
        </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
    
                                            
</asp:Content>

