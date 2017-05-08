<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InvoiceMasterNo.aspx.cs" Inherits="HSRP.Transaction.InvoiceMasterNo" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size:medium;color:Black">
                    HSRP New Invoice No</div>
            </legend>
            <br />
            <center>
            <table>
                <tr>
                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                     <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                     <td valign="middle" align="center">
                                    
                                             <asp:DropDownList Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName" AutoPostBack="false"
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    
                         </td>                                  
              
                    <td>
                        <asp:Button ID="buttonUpdateinvoice" runat="server" Text="Generate New Transfer" TabIndex="1"  class="button" OnClick="buttonUpdateinvoice_Click"  />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;    
                    </td>  
                         <td>
                             <asp:Button ID="btnexcel" runat="server" Text="ExportToExcel" TabIndex="1"  class="button" Visible="false" OnClick="btnexcel_Click"/>
                         </td>
                </tr>
           
                <tr align="right" style="margin-right:10px">
                <td>
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                    <td colspan="2">
                        <br />
                    
                    </td>
                    
                </tr>                
             
            </table>
                </center>
            <center>
                    <div>
                        <asp:Label ID="lblinvoicemsg" runat="server"></asp:Label>
                    </div>
                 </center>
        </fieldset>
    </div>
</asp:Content>
