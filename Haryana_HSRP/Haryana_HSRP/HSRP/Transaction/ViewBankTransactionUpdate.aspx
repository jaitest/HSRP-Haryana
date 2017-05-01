<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewBankTransactionUpdate.aspx.cs" Inherits="HSRP.Transaction.ViewBankTransactionUpdate" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />


    <script type="text/javascript">



        function getQueryStrings() {
            var assoc = {};
            var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
            var queryString = location.search.substring(1);
            var keyValues = queryString.split('&');

            for (var i in keyValues) {
                var key = keyValues[i].split('=');
                if (key.length > 1) {
                    assoc[decode(key[0])] = decode(key[1]);
                }
            }

            return assoc;
        }



        function edit(i)
        {
           
           
            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransactionUpdate.aspx?Mode=Edit&TransactionID=" + i, "Update Bank Transaction", "width=650px,height=520px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //var qs = getQueryStrings();
                //var myParam = qs["PageIndex"];
                window.location.href = 'ViewBankTransactionUpdate.aspx';
                    //?PageIndex=' + myParam;

                return true;

            }
        }

      
      

        function Voids(i) {

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransactionVoid.aspx?Mode=Voids&TransactionID=" + i, "  Void Bank Transaction  ", "width=450px,height=220px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {

                //var qs = getQueryStrings();
              //  var myParam = qs["PageIndex"];
                window.location.href = 'ViewBankTransactionUpdate.aspx';
                //?PageIndex=' + myParam;

                return true;

            }
        }


        function AddNewPop() {
                       
            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransactionUpdate.aspx?Mode=New", "Add New Bank Transaction", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'ViewBankTransactionUpdate.aspx';
                return true;
            }
        }


        function PrintChalan(i, S) {

            googlewin = dhtmlwindow.open("googlebox", "iframe", "ViewPrintInvoice.aspx?Mode=PrintChalan&HSRPRecordID=" + i + "&Status=" + S + "", "Print DELIVERY CHALLAN / AP", "width=400px,height=75px,resize=1,scrolling=1,center=2", "recal")
            googlewin.onclose = function () {
                 return true;
            }
        }


    </script>

    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) {

                return true;
            }
            else {
                return false;
            }

        }
    </script>


    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>

            <td>

                <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"
                    class="topheader">
                </table>
            </td>

        </tr>
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View Bank Transaction Update</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap></td>
                                </tr>


                                <tr>
                                    <td>
                                        <span class="headingmain"></span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap></td>
                                </tr>


                                <tr>
                                    <td>
                                        <span class="headingmain"> Transaction Id </span> &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtransactionid" runat="server"></asp:TextBox>&nbsp;&nbsp; 

                                        <span class="headingmain"> Account No </span> &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtacno" runat="server"></asp:TextBox>
                                         
                                        <span class="headingmain"> Bank Name  </span> &nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>
                                        
                                        
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                        <asp:Button ID="BtnSearch" runat="server" Text="Search" OnClick="BtnSearch_Click" /></td>
                                </tr>



                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
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
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>


                                <tr>

                                    <td>
                                        <asp:GridView ID="Grid1" runat="server" OnRowDataBound="Grid1_RowDataBound" AllowPaging="true" PageSize="15" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                                            AutoGenerateColumns="false" OnPageIndexChanging="Grid1_PageIndexChanging" ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid"
                                            BorderWidth="1px">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>

                                                
                                                <asp:BoundField HeaderText="TransactionID" DataField="TransactionID" />
                                                <asp:BoundField HeaderText="Deposit Date" DataField="Deposit Date" />
                                                <asp:BoundField HeaderText="BankName" DataField="BankName" />
                                                <asp:BoundField HeaderText="BranchName" DataField="BranchName" />
                                                <asp:BoundField HeaderText="DepositAmount" DataField="DepositAmount" />
                                                <asp:BoundField HeaderText="DepositBy" DataField="DepositBy" />
                                              <%--  <asp:BoundField HeaderText="StateID" DataField="StateID" />
                                                <asp:BoundField HeaderText="RTOLocation" DataField="RTOLocation" />--%>
                                                 <asp:BoundField HeaderText="DepositLocation" DataField="DepositLocation" />
                                               <%-- <asp:BoundField HeaderText="UserID" DataField="UserID" />   --%>                                             
                                                <asp:BoundField HeaderText="EntryDate" DataField="EntryDate" />
                                            <%--    <asp:BoundField HeaderText="CurrentDate" DataField="CurrentDate" />--%>
                                                <asp:BoundField HeaderText="BankSlipNo" DataField="BankSlipNo" />
                                                <asp:BoundField HeaderText="Remarks" DataField="Remarks" />
                                                <asp:BoundField HeaderText="AccountNo" DataField="AccountNo" />

                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>

                                                        <a href="javascript:edit(<%#Eval("TransactionID")%>)"> <asp:Label runat="server" ID="lbledit" Text="Edit"></asp:Label></a>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Void">
                                                    <ItemTemplate>
                                                    <a href="javascript:Voids(<%#Eval("TransactionID")%>);"> <asp:Label runat="server" ID="lblvoid" Text='<%#Bind("voidstatus")%>'></asp:Label></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                            <FooterStyle BackColor="#CCCC99" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <RowStyle BackColor="#F7F7DE" />
                                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                            <SortedAscendingHeaderStyle BackColor="#848384" />
                                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                            <SortedDescendingHeaderStyle BackColor="#575357" />
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
