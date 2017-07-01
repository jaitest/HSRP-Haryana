<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FaliorReport.aspx.cs" Inherits="HSRP.Report.FaliorReport" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />    
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

 

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
                                            <span class="headingmain">Faliure Report</span>
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
                <td colspan="8">
                    <table style="width: 90%">
                        <tr>
                            <td colspan="10" align="center">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td >
                                            <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1"
                                                ForeColor="Black" Font-Bold="True" Width="83px" />
                                        </td>
                                        <td >
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>

                                                
                                            <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="false" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                                CausesValidation="false" Height="22px" Width="120px" >
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownListStateName" ErrorMessage="Select State ..."
                                                InitialValue="--Select State--" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                       
                                       
                                                                   
                                        <td>                                           
                                            
                                            <asp:Button ID="btn_download" runat="server" Text="Export Report"
                                                Font-Bold="True" ForeColor="#3333FF" OnClick="btn_download_Click"/> &nbsp;&nbsp;&nbsp;&nbsp;
                                            
                                           
                                        </td>
                                         <td>                                           
                                            
                                            <asp:Button ID="BtnFliutre" runat="server" Text="Export Fliure Report"
                                                Font-Bold="True" ForeColor="#3333FF" OnClick="BtnFliutre_Click"/>
                                            
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                       
                                    </tr>
                                </table>
                            </td>
                            
                        </tr>
                      
                        <tr>
                            <td colspan="10" align="center">
                                <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
         
        </table>
    </div>
</asp:Content>


