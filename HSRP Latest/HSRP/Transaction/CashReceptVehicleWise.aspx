<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="CashReceptVehicleWise.aspx.cs" Inherits="HSRP.Transaction.CashReceptVehicleWise" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    
    
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../javascript/bootstrap.min.js" type="text/javascript"></script>
    <script src="../javascript/bootbox.min.js" type="text/javascript"></script>  
    
  
    
<%--<marquee class="mar1" direction="left">Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</marquee>--%>
    
      <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                  Download Cash Collection Receipt Vehicle Wise</div>
            </legend>
          <table>
              <%--<tr>
                   <td style="padding-left: 314px;"><asp:Label ID="lblState" runat="server" Text="Select State:">
                      </asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td valign="middle" style="padding-left: 10px;">
                         <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">                        
                         </asp:DropDownList>                      
                  </td>
                  <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlState" InitialValue="0" ErrorMessage="Please Select State"></asp:RequiredFieldValidator></td>
                
              </tr>--%>
              <tr>
                  <td style="padding-left: 314px;">
                      <asp:Label runat="server" ID="lblUserName" Text="Enter Vehicle No.">
                      </asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td style="padding-left: 10px;">
                      <asp:TextBox ID="txtVehicleregno" runat="server"></asp:TextBox>                       

                  </td>
                  <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtVehicleregno" ErrorMessage="Please Enter Vehicle Reg. No." ValidationGroup="save"></asp:RequiredFieldValidator></td>
              </tr>
              <tr><td></td><td style="padding-left: 10px;">
                  <asp:Button ID="Button3" runat="server" Visible="false" CssClass="button" Height="23px" 
                            onclick="Button3_Click" TabIndex="12" ValidationGroup="save" Text="DownLoad Receipt"/></td><td>

                                                                                                <asp:LinkButton ID="LinkButton1" runat="server" ValidationGroup="save" OnClick="LinkButton1_Click">View Detail</asp:LinkButton>

                                                                                                </td></tr>
              <tr><td></td><td></td><td>
                  <asp:Label ID="lblErrMess" runat="server" Text="" Visible="false"></asp:Label>
                                    </td></tr>
          </table>
          <div id="gridTD" style="text-align:center; padding-left:10px;">
              <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="198px" Width="1323px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
                        <asp:GridView ID="grd" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                            ShowHeader="true" Width="100%" BackColor="White" AutoGenerateColumns="false" BorderColor="#FFCC99" BorderStyle="Solid" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns> 
                                  <asp:TemplateField ItemStyle-Width="30px" HeaderText="CASH RECEIPT No.">
                                <ItemTemplate>
                                <asp:Label ID="lbCashReceiptNo" runat="server" Text='<%#Eval("CashReceiptNo")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="DATE">
                                <ItemTemplate>
                                <asp:Label ID="lblHSRPRecord_CreationDate" runat="server" Text='<%#Eval("HSRPRecord_CreationDate")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField ItemStyle-Width="30px" HeaderText="TIN NO">
                                <ItemTemplate>
                                <asp:Label ID="lblTinNo" runat="server" Text='<%#Eval("TinNo")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>--%>
                              <%--  <asp:TemplateField ItemStyle-Width="30px" HeaderText="AUTH NO">
                                <ItemTemplate>
                                <asp:Label ID="lblHSRPRecord_AuthorizationNo" runat="server" Text='<%#Eval("HSRPRecord_AuthorizationNo")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>--%>
                               <%-- <asp:TemplateField ItemStyle-Width="30px" HeaderText="AUTH. DATE">
                                <ItemTemplate>
                                <asp:Label ID="lblHSRPRecord_AuthorizationDate" runat="server" Text='<%#Eval("HSRPRecord_AuthorizationDate")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="OWNER NAME">
                                <ItemTemplate>
                                <asp:Label ID="lblOwnerName" runat="server" Text='<%#Eval("OwnerName")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="CONTACT NO">
                                <ItemTemplate>
                                <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="VEHICLE REG.">
                                <ItemTemplate>
                                <asp:Label ID="lblVehicleRegNo" runat="server" Text='<%#Eval("VehicleRegNo")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="VEHICLE MODEL">
                                <ItemTemplate>
                                <asp:Label ID="lblVehicleType" runat="server" Text='<%#Eval("VehicleType")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="VEHICLE CLASS">
                                <ItemTemplate>
                                <asp:Label ID="lblVehicleClass" runat="server" Text='<%#Eval("VehicleClass")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField ItemStyle-Width="30px" HeaderText="NET AMOUNT (Rs.)">
                                <ItemTemplate>
                                <asp:Label ID="lblNetAmount" runat="server" Text='<%#Eval("NetAmount")%>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="ROUND OF AMOUNT">
                                <ItemTemplate>
                                <asp:Label ID="lblroundAmt" runat="server" Text='<%#Eval("roundAmt")%>'></asp:Label>
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
                    </asp:Panel></div> 
    </fieldset>
</asp:Content>
