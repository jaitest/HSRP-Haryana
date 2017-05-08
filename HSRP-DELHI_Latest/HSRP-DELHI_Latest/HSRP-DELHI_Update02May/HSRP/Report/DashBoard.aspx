<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DashBoard.aspx.cs" Inherits="HSRP.Report.DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td style="width: 33%">
                    <asp:Panel Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
                <td style="width: 33%">
                    <asp:Panel ID="Panel1" Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
                <td style="width: 33%">
                    <asp:Panel ID="Panel2" Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 33%">
                    <asp:Panel ID="Panel3" Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
                <td style="width: 33%">
                    <asp:Panel ID="Panel4" Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
                <td style="width: 33%">
                    <asp:Panel ID="Panel5" Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 33%">
                    <asp:Panel ID="Panel6" Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
                <td style="width: 33%">
                    <asp:Panel ID="Panel7" Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
                <td style="width: 33%">
                    <asp:Panel ID="Panel8" Width="100%" runat="server" ScrollBars="Auto">
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                        <asp:View ID="Tab1" runat="server">
                            <table width="600" height="400" cellpadding="0" cellspacing="0">
                                <tr valign="top">
                                    <td class="TabArea" style="width: 600px">
                                        <br />
                                        <br />
                                        TAB VIEW 1 INSERT YOUR CONENT IN HERE CHANGE SELECTED IMAGE URL AS NECESSARY
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="Tab2" runat="server">
                            <table width="600px" height="400px" cellpadding="0" cellspacing="0">
                                <tr valign="top">
                                    <td class="TabArea" style="width: 600px">
                                        <br />
                                        <br />
                                        TAB VIEW 2 INSERT YOUR CONENT IN HERE CHANGE SELECTED IMAGE URL AS NECESSARY
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="Tab3" runat="server">
                            <table width="600px" height="400px" cellpadding="0" cellspacing="0">
                                <tr valign="top">
                                    <td class="TabArea" style="width: 600px">
                                        <br />
                                        <br />
                                        TAB VIEW 3 INSERT YOUR CONENT IN HERE CHANGE SELECTED IMAGE URL AS NECESSARY
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
