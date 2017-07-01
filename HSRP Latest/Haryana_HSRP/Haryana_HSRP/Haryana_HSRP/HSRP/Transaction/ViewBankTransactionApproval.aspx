<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewBankTransactionApproval.aspx.cs" Inherits="HSRP.Transaction.ViewBankTransactionApproval" %>

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
                                        <span class="headingmain">View Bank Transaction</span>
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
                                    <td valign="left" class="form_text" >
                                       
                                    </td>
                                    <td align="left" valign="middle">
                                        
                                    </td>
                                    <td valign="left" class="form_text">
                                        
                                    </td>
                                    <%--<td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Bank Transaction" class="button">Add New Bank Transaction</a>
                                    </td>--%>

                                    

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
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
                                        <asp:Label ID="lblSucMess" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                         
                                        

                                         <asp:GridView ID="grdid" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnRowCommand="grdid_RowCommand1">
                <AlternatingRowStyle BackColor="White" />
                 <Columns>

                                    <asp:BoundField HeaderText="TransactionID" DataField="TransactionID" />
                                                    <asp:BoundField HeaderText="Deposit Date" DataField="Deposit Date" />
                                                    <asp:BoundField HeaderText="BankSlipNo" DataField="BankSlipNo" />
                                                    <asp:BoundField HeaderText="BranchName" DataField="BranchName" />
                                                    <asp:BoundField HeaderText="DepositAmount" DataField="DepositAmount" />
                                                    <asp:BoundField HeaderText="DepositBy" DataField="DepositBy" />
                                                     <asp:BoundField HeaderText="DepositLocation" DataField="DepositLocation" />
                                                    <asp:BoundField HeaderText="StateID" DataField="StateID" />
                                                     <asp:BoundField HeaderText="RTOLocation" DataField="RTOLocation" />
                                                    <asp:BoundField HeaderText="UserID" DataField="UserID" />
                                                    <asp:BoundField HeaderText="CurrentDate" DataField="CurrentDate" />
                     <asp:TemplateField HeaderText="Approval">
        <ItemTemplate>
       <asp:LinkButton runat="server" ID="lnkView" CommandArgument='<%#Eval("TransactionID") %>'
         CommandName="Approval">Approved</asp:LinkButton>
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
    </table>


</asp:Content>
