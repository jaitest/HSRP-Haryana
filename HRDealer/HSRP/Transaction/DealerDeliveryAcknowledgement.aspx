<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DealerDeliveryAcknowledgement.aspx.cs" Inherits="HSRP.Transaction.DealerDeliveryAcknowledgement" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Confirmation</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="left" class="form_text"></td>
                                    <td align="left" valign="middle"></td>
                                    <td valign="left" class="form_text"></td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblMsgBlue" runat="server" Visible="false" Text="Label"></asp:Label>
                                        <asp:Label ID="lblMsgRed" runat="server" Visible="false" ForeColor="Red" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>



                                        <asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>

                                                <asp:TemplateField HeaderText="S No" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="id" runat="server" Text='<%#Eval("hsrprecordid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="RegNo" ItemStyle-HorizontalAlign="Center" DataField="VehicleRegNo" />
                                                <asp:BoundField HeaderText="OwnerName" ItemStyle-HorizontalAlign="Center" DataField="OwnerName" />
                                                <asp:BoundField HeaderText="VehicleType" ItemStyle-HorizontalAlign="Center" DataField="VehicleType" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Select
                                                        <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CHKSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
        <tr>
            <td>
                <table align="center">
                    <tr>

                        <td valign="middle" class="form_text" nowrap="nowrap">
                            <asp:Button ID="btnGO" Width="58px" runat="server" Visible="true" Text="Confirm Records" ToolTip="Please Click for Report"
                                class="button" OnClientClick=" return validate()" OnClick="btnGO_Click" />

                        </td>
                    </tr>


                </table>
            </td>
        </tr>
    </table>


</asp:Content>
