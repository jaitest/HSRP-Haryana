<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewBankTransactionApprovalDealer_HO.aspx.cs" Inherits="HSRP.Transaction.ViewBankTransactionApprovalDealer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .cal_Calendar .ajax__calendar_title,
        .cal_Calendar .ajax__calendar_next,
        .cal_Calendar .ajax__calendar_prev {
            color: #004080;
            padding-top: 3px;
        }

        .cal_Calendar .ajax_calendar_invalid .ajax_calendar_day,
        .ajax_calendar_invalid {
            color: red;
            text-decoration: strike;
            cursor: pointer;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .listMain {
            background-repeat: repeat-x;
            background-color: #FFFFFF;
            z-index: 1000;
            width: 302px !important;
            height: 250px !important;
            text-align: inherit;
            text-indent: -1;
            list-style: none;
            overflow-y: scroll;
            scrollbar-arrow-color: #B89020;
            scrollbar-base-color: #8E6E1C;
            scrollbar-face-color: #B6C5D4;
            scrollbar-3dlight-color: #8E6E1C;
            scrollbar-highlight-color: #EED47D;
            scrollbar-shadow-color: #959595;
            scrollbar-darkshadow-color: #00337E;
            margin-left: 0px;
            border-bottom: 1px solid #B5C6D4;
            border-left: 1px solid #B5C6D4;
            margin-top: 0px;
        }

        .wordWheel .itemsMain {
            background: none;
            border-collapse: collapse;
            color: #00337E;
            white-space: nowrap;
            text-align: inherit left;
        }

        .wordWheel .itemsSelected {
            background-repeat: repeat-x;
            background-color: #EED47D;
            color: #00337E;
            border-top: 1px solid #FFF8E8;
            border-left: 1px solid #FFF8E8;
            border-bottom: 1px solid #00337E;
            border-right: 1px solid #00337E;
        }
    </style>

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
                                    <td width="300px" height="26" align="center" nowrap></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Search preference" runat="server" ID="lblSearchPreference" />
                                    </td>
                              
                                    <td>
                                        <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                            <asp:ListItem Value="--Select--">--Select--</asp:ListItem>
                                            <asp:ListItem Value="Dealerid">Dealer Id</asp:ListItem>
                                            <asp:ListItem Value="Dealername">Dealer Name</asp:ListItem>
                                            <asp:ListItem Value="ManualId">Manual By Id</asp:ListItem>
                                            <asp:ListItem Value="ManualName">Manual By Name</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Dealer :" runat="server" Visible="true" ID="labelSelectType" /></td>
                                    <td>
                                        <asp:TextBox ID="txtSearchByname" Width="300px" Visible="false" runat="server"></asp:TextBox>
                                        <asp:AutoCompleteExtender ServiceMethod="SearchByDealerName" MinimumPrefixLength="1" CompletionInterval="500" TargetControlID="txtSearchByname" ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false"
                                            CompletionListCssClass="wordWheel listMain .box"
                                            CompletionListItemCssClass="wordWheel itemsMain"
                                            CompletionListHighlightedItemCssClass="wordWheel itemsSelected">
                                        </asp:AutoCompleteExtender>

                                        <asp:TextBox ID="txtSearchByID" Width="300px" Visible="false" runat="server"></asp:TextBox>
                                        <asp:AutoCompleteExtender ServiceMethod="SearchCustomers" MinimumPrefixLength="1" CompletionInterval="500" TargetControlID="txtSearchByID" ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" 
                                            CompletionListCssClass="wordWheel listMain .box"
                                            CompletionListItemCssClass="wordWheel itemsMain"
                                            CompletionListHighlightedItemCssClass="wordWheel itemsSelected">
                                        </asp:AutoCompleteExtender>
                                        <asp:DropDownList Visible="false" ID="ddlBothDealerHHT" CausesValidation="false" Width="250px" AutoPostBack="false" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" valign="middle">
                                        <asp:Button ID="btngo" Width="58px" Visible="true" runat="server"
                                            Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClientClick=" return validate()" OnClick="btngo_Click" />
                                    </td>
                                    <td valign="left" class="form_text"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                 <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                        <asp:Label ID="lblSucMess" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Blue"></asp:Label>
                                    </td>
                                      </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>

                                        <asp:GridView ID="grdid" runat="server" CellPadding="4" ForeColor="#333333"
                                            GridLines="None" AutoGenerateColumns="false" OnRowCommand="grdid_RowCommand1"
                                            OnRowDataBound="grdid_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Transaction ID" DataField="TransactionID" />
                                                <asp:BoundField HeaderText="Deposit Date" DataField="Deposit Date" />
                                                <asp:BoundField HeaderText="Bank Slip No" DataField="BankSlipNo" />
                                                <asp:BoundField HeaderText="Branch Name" DataField="BranchName" />
                                                <asp:BoundField HeaderText="Deposit Amount" DataField="DepositAmount" />
                                                <asp:BoundField HeaderText="Deposit By" DataField="DepositBy" />
                                                <asp:BoundField HeaderText="DealerName And ID" DataField="Dealer" />
                                                <asp:BoundField HeaderText="Deposit Location" DataField="DepositLocation" />
                                                <asp:BoundField HeaderText="Amount Clearance Date" DataField="AmtClearDate" />
                                                <asp:BoundField HeaderText="Approved By" DataField="ApprovedBy" />
                                                <asp:BoundField HeaderText="Approved Date" DataField="ApprovedDate" />
                                                <asp:BoundField HeaderText="Cheque Approved By" DataField="ChqrecBy" />
                                                <asp:BoundField HeaderText="Cheque Approved Date" DataField="ChqrecDateTime" />
                                                <asp:BoundField HeaderText="Rejected By" DataField="RejectedBy" />
                                                <asp:BoundField HeaderText="Reject Date" DataField="RejectDate" />

                                                <asp:TemplateField HeaderText="Approval">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkView" ClientIDMode="Static" CommandArgument='<%#Eval("TransactionID") %>'
                                                            CommandName="Approval" ValidationGroup="link">Approved</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Cheque Approval">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkChrec" ClientIDMode="Static" CommandArgument='<%#Eval("TransactionID") %>'
                                                            CommandName="CheqeApproval">cheque Approved</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rejected">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkRejected" ClientIDMode="Static" CommandArgument='<%#Eval("TransactionID") %>' CommandName="Rejected" ValidationGroup="Reject">Rejected</asp:LinkButton>
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
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlpopup" runat="server" BackColor="White" Height="269px" Width="400px" Style="display: none">
                                            <table width="100%" style="border: Solid 3px #507CD1; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                                                <tr style="background-color: #507CD1">
                                                    <td colspan="2" style="height: 10%; color: White; font-weight: bold; font-size: larger" align="center">Approvel Transaction</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 45%">TransactionId:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblID" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">Date:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtdate" runat="server" /><br />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtdate" ValidationGroup="Approved" runat="server" ErrorMessage="Please Enter Amount Clearance Date"></asp:RequiredFieldValidator>
                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdate" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnTransactionApproved" CommandName="Update" runat="server" Text="Approved" ValidationGroup="Approved" OnClick="btnApproval" />
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                                            CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                                        </asp:ModalPopupExtender>
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlpopup2" runat="server" BackColor="White" Height="269px" Width="400px" Style="display: none">
                                            <table width="100%" style="border: Solid 3px #507CD1; width: 100%; height: 100%" cellpadding="0" cellspacing="0">
                                                <tr style="background-color: #507CD1">
                                                    <td colspan="2" style="height: 10%; color: White; font-weight: bold; font-size: larger" align="center">Reject Transaction</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 45%">TransactionId:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTransactionId" runat="server"></asp:Label>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td align="right">Rejected Summary
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" />
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtRemarks" ValidationGroup="reject" runat="server" ErrorMessage="Please Enter Rejected Summary"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="Button1" CommandName="Update" runat="server" Text="Rejected" OnClick="btnReject" ValidationGroup="reject" />
                                                        <asp:Button ID="Button2" runat="server" Text="Cancel" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Button ID="Button3" runat="server" Style="display: none" />
                                        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="Button3" PopupControlID="pnlpopup2"
                                            CancelControlID="Button2" BackgroundCssClass="modalBackground">
                                        </asp:ModalPopupExtender>
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
