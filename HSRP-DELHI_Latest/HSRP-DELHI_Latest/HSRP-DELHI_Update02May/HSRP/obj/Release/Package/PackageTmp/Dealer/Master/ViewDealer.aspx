<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ViewDealer.aspx.cs" Inherits="HSRP.Dealer.Master.ViewDealer" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../../windowfiles/dhtmlwindow.js"></script> 
    <link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">
        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            //googlewin = dhtmlwindow.open("googlebox", "iframe", "Dealer.aspx?Mode=New", "Add New Dealer", "width=1200px,height=500px,resize=1,scrolling=1,center=1", "recal")
            //googlewin.onclose = function () {
            //    window.location = 'ViewDealer.aspx';

            window.open("NewDealer.aspx?Mode=New", 'Add New Dealer', 'width=1200px,height=500px,resize=1,scrolling=1,center=1');
            return true;   
                return true;
            }
       
    </script>
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Dealer status?")) {

                return true;
            }
            else {
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
                                        <span class="headingmain">Dealer Master </span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap="nowrap">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" align="center" cellpadding="0" cellspacing="0" 
                                class="topheader">
                                <tr>
                                    <td height="35" align="right" valign="middle" class="footer" 
                                        style="width: 1147px">
                                        <%--<asp:Button ID="btnviewdealer" runat="server" onclick="btnviewdealer_Click" 
                                            Text="View Dealer" Width="102px" Font-Bold="True" ForeColor="Black" 
                                            BackColor="#999966" />
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export To Excel" 
                                                        Font-Bold="True" ForeColor="Black" 
                                            Width="111px" BackColor="#999966" />--%>
                                    </td>
                                    <td height="70" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Dealer" class="button">Add New Dealer</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2" style="height: 16px">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" id="g1" runat="server">
                                        <asp:GridView ID="Grid1" runat="server" BackColor="LightGoldenrodYellow" AutoGenerateColumns="False" Width="100%"
                                            PageSize="20" BorderColor="Tan" BorderWidth="1px"
                                            CellPadding="2" ForeColor="Black" GridLines="None">
                                            <Columns>
                                                <asp:BoundField DataField="SNO" HeaderText="Sno" />
                                                <asp:BoundField DataField="Dealer" HeaderText="Dealer" />
                                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                                <asp:BoundField DataField="ActiveStatus" HeaderText="ActiveStatus" />                                                
                                            </Columns>
                                        </asp:GridView>
                                                                             
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2" id="g3" runat="server" visible="false">
                        
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
