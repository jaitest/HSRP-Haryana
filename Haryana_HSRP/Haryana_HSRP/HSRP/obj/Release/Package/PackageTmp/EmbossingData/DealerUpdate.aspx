<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"  CodeBehind="DealerUpdate.aspx.cs" Inherits="HSRP.EmbossingData.DealerUpdate" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   


    
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                              <span class="headingmain">Update Dealer Status </span>
                        &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName" Width="160px"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"  
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                     <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> 
                                    </td>
                                   <td valign="middle" class="form_text">
                                       
                                             <asp:DropDownList AutoPostBack="true" Visible="true" ID="dropDownListClient" Width="160px"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                            
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnupdate" runat="server" Text="Go" OnClick="btnupdate_Click" />                                  
                                         
                                    </td>

                                  
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>

                        <td valign="middle">
                            <asp:Label ID="lblSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="true" />
                        </td>
                         

                    </tr>
                    <tr>
                        <td valign="middle">
                            <asp:Label ID="lblactive" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Blue" Visible="true" />
                        </td>
                    </tr>
                    <tr>
                       <td align="center" style=" padding:10px">                         
                            <asp:GridView ID="GridView1" runat="server" BackColor="White" AutoGenerateColumns="true"
                                PageSize="25" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" DataKeyNames="dealerid" >
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                  <PagerSettings Visible="False" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Select
                                           <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox CssClass="testing" ClientIDMode="Static" ID="CHKSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                                
                            </asp:GridView>

                        </td>

                    </tr>
                    <tr>
                        
                        <td>
                            <center>
                            <asp:Button ID="btnupdates" runat="server" Text="UpdateStatus"  BackColor="Orange" ForeColor="#000000" OnClick="btnupdates_Click"  />
                                </center>
                        </td>
                            
                    </tr>

                
                    
                </table>
            </td>
        </tr>
    </table>
    <br />
   
           <td>
                                        
                                        <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                    </td>
</asp:Content>
