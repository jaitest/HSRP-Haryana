<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConfirmHoldSalarySheet.aspx.cs" Inherits="HSRP.Transaction.ConfirmHoldSalarySheet" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js">

      </script><link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
    <style type="text/css">
         
          .button-success {
            background: rgb(28, 184, 65); /* this is a green */
        }
      </style><table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">

        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                
                                
                                <tr id="TR1" runat="server">
                                    <td > 
                                          <asp:Label ID="Label4" class="headingmain" runat="server"  >Confirm/Hold Salary Status</asp:Label> 
                                          
                                    </td> 
                                </tr>

                                
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Company Name:"  runat="server" ID="lblCompanyName" ForeColor="Black" />&nbsp;&nbsp;
                                    </td>
                                   <td valign="middle" class="form_text" nowrap="nowrap">                                    
                                     
                                    <asp:DropDownList  ID="DDlCompany_Name"  runat="server" DataTextField="CompanyName" DataValueField="CompanyId" >
                                    </asp:DropDownList>&nbsp;&nbsp;                                                                   

                                    </td>

                                      <td valign="middle" class="form_text" nowrap="nowrap">
                                           <asp:Label Text="Month Name :"  runat="server" ID="lblMonth" ForeColor="Black" />&nbsp;&nbsp;

                                      </td>

                                         <td valign="middle" class="form_text" nowrap="nowrap">
                                                   
                                                     <asp:DropDownList ID="DDLMonth" runat="server"   >
                                                         <asp:ListItem Text="--Select Month--" Value="--Select Month--" />
                                                         <asp:ListItem Text="January" Value="1" />
                                                         <asp:ListItem Text="February" Value="2" />
                                                          <asp:ListItem Text="March" Value="3" />
                                                          <asp:ListItem Text="April" Value="4" />
                                                          <asp:ListItem Text="May" Value="5" />
                                                          <asp:ListItem Text="June" Value="6" />
                                                          <asp:ListItem Text="July" Value="7" />
                                                          <asp:ListItem Text="August" Value="8" />
                                                          <asp:ListItem Text="September" Value="9" />
                                                          <asp:ListItem Text="October" Value="10" />
                                                          <asp:ListItem Text="NovemBer" Value="11" />
                                                          <asp:ListItem Text="December" Value="12" />
                                                          

                                                       
                                                            </asp:DropDownList>&nbsp;&nbsp;
                                                         </td>

                                      <td> 
                        <asp:Label Text="Year:"  runat="server" ID="lblyear" 
                            ForeColor="Black" />&nbsp;&nbsp;
                    </td>

                                       <td valign="middle" class="form_text" nowrap="nowrap">
                                                   
                                                      <asp:DropDownList ID="ddlyear"                                                    
                                                                runat="server" >
                                                          <asp:ListItem>--Select Year--</asp:ListItem>
                                                          <asp:ListItem>2016</asp:ListItem>
                                                          <asp:ListItem>2017</asp:ListItem>
                                                          <asp:ListItem>2018</asp:ListItem>
                                                          <asp:ListItem>2019</asp:ListItem>
                                                          <asp:ListItem>2020</asp:ListItem>
                                                          <asp:ListItem>2021</asp:ListItem>
                                                          <asp:ListItem>2022</asp:ListItem>
                                                          <asp:ListItem>2023</asp:ListItem>
                                                          <asp:ListItem>2024</asp:ListItem>
                                                          <asp:ListItem>2025</asp:ListItem>                                                         
                                                            </asp:DropDownList>&nbsp;&nbsp;
                                                         </td>

                                    
                                   

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                       
                                        <asp:Button ID="btnGO"  Width="79px"  runat="server" 
                                            Text="GO" ToolTip="Please Click for Report" BackColor="Orange" ForeColor="#000000"
                                            class="button"  OnClientClick=" return validate()" 
                                            onclick="btnGO_Click"  /> &nbsp;&nbsp;&nbsp;  

                                             &nbsp;&nbsp;&nbsp;  
                                         
                                    </td>


                                     <td valign="middle" Visible="false" class="form_text" nowrap="nowrap" 
                                        align="left">
                                         &nbsp;</td>
                                </tr>
                                  <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                    <td align="left" valign="middle">
                                        &nbsp;</td>
                                    <td colspan="2">
                                        &nbsp;</td>
                                    <td align="left" valign="middle">
                                        &nbsp;</td>
                                </tr>
                                <tr>                                     
                                    
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Employee Code:"  runat="server" ID="Lblempcode" ForeColor="Black" />&nbsp;&nbsp;
                                    </td>
                                      
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:TextBox ID="txtepcode" runat="server"></asp:TextBox>&nbsp;&nbsp;

                                    </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                         <asp:Button ID="btnHold"  Width="104px"  runat="server" 
                                            Text="Hold Salary" ToolTip="Please Click for Report" BackColor="Orange" ForeColor="#000000"
                                            class="button" OnClick="btnHold_Click"   /> &nbsp;&nbsp;&nbsp;  

                                           

                                        </td>
                                    <td colspan="2">
                                        &nbsp;</td>
                                    <td align="left" valign="middle">
                                        &nbsp;</td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                    <td align="left" valign="middle">
                                        &nbsp;</td>
                                    <td colspan="2">
                                        &nbsp;</td>
                                    <td align="left" valign="middle">
                                        &nbsp;</td>
                                </tr>
                              
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="red" />
                                     <asp:Label ID="LblMessage" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="blue" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                
                                <tr>
                                    <td align="center" style=" padding:10px">
                                         <asp:GridView ID="GridView1" runat="server" BackColor="White" AutoGenerateColumns="false"
                                PageSize="25" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3"  DataKeyNames="EmpCode"  >
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
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            S.No</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="SrNo" runat="server" Text='<%#Eval("SNo") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                    </asp:TemplateField> 
	
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Employee Code
                                            </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpCode" runat="server" Text='<%#Eval("EmpCode") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                    </asp:TemplateField>



                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            ESIC No</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblESICNo" runat="server" Text='<%#Eval("ESICNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                           EPF No</HeaderTemplate>
                                        <ItemTemplate>                                        
                                            <asp:TextBox ID="txtEPFNo" runat="server" Text='<%#Eval("EPFNo") %>' Enabled="false"></asp:TextBox>                                          
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                           UAN No</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUANNo" runat="server" Text='<%#Eval("UANNo") %>' Enabled="false"></asp:TextBox>
                                           
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <HeaderTemplate>
                                            Bank Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="BankName" runat="server" Text='<%#Eval("BankName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField >
                                        <HeaderTemplate>
                                      A/C No
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblACountNo" runat="server" Text='<%#Eval("AccountNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                      <asp:TemplateField >
                                        <HeaderTemplate>
                                  Branch Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBranchName" runat="server" Text='<%#Eval("BranchName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField >
                                        <HeaderTemplate>
                                  IFSC Code
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblIFSCode" runat="server" Text='<%#Eval("IFSCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                  
                                    <asp:TemplateField >
                                        <HeaderTemplate>
                                          Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField >
                                        <HeaderTemplate>
                                         Department
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    
                                </Columns>
                            </asp:GridView>
                         
                                    &nbsp;</td>
                                </tr>
                                   <tr>
                            <td align="center" style="padding-top: 10px">                                                     
                            <asp:Button ID="btnConfirm"  runat="server" Text="Confirm"  BackColor="Orange" ForeColor="#000000" autoposback="true" Visible="false"  OnClick="btnConfirm_Click" />
                                                              
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
