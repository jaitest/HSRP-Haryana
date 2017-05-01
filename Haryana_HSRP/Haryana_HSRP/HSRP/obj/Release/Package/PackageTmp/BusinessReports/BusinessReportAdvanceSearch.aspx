<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BusinessReportAdvanceSearch.aspx.cs" Inherits="HSRP.BusinessReports.BusinessReportAdvanceSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link href="../css/main.css" rel="stylesheet" type="text/css" />  
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />    
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
                                <span class="headingmain">Business Report search</span>
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

    
    <table style="width: 50%; margin-top:30px; height: 51px; left: 50px;">
        <tr>
            <td width="80">    &nbsp;</td>

          <%--  <td valign="middle"class="form_text" colspan="2" nowrap="nowrap">--%>
            <td width="60">
                <asp:Label Text="Search Type:" runat="server"   Visible="true"   ForeColor="Black" Font-Bold="True" />
                                           
            </td>
            <td width="40">                
                   <%-- <ContentTemplate> --%>                       
                <asp:DropDownList ID="Ddl1" runat="server" Height="25px" Width="148px" AutoPostBack="true" OnSelectedIndexChanged="Ddl1_SelectedIndexChanged" >  </asp:DropDownList>                        
                  <%--  </ContentTemplate>--%>
                
            </td>          

          
            <td width="80">
                <asp:Label  runat="server"   Text="Enter Search Type"  Visible="false" ID="lblsearch"   ForeColor="Black" Font-Bold="True" />
                                           
            </td>

              <td width="60">
               
                   <asp:TextBox ID="TxtSearchtype" Visible="false"  runat="server" ForeColor="Black" Font-Bold="True" ></asp:TextBox> 
            </td>
            <td width="40">
              <asp:Button ID="BtnSearch" Visible="false"  runat="server" Text="Search" OnClick="BtnSearch_Click" />
            </td>
            </tr>
        </table>




    

    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Label  runat="server"  Visible="false" ID="Lblerror"   ForeColor="Red" Font-Bold="True" />
                              
            </td>
            </tr>
        <tr>
        <td>
     <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical" Height="457px" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px">
                        <asp:GridView ID="grd" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical"
                            ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99"  BorderStyle="Solid" BorderWidth="1px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns> 
                                        
                                               
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
                    </asp:Panel>
                
            </td></tr></table>


</asp:Content>
