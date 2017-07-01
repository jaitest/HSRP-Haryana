<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BlockVehicleRegNo.aspx.cs" Inherits="HSRP.Transaction.BlockVehicleRegNo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style type="text/css">
        .modalPopup
        {
            background-color: #FFFFFF;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }
    </style>

 

    <div style="margin: 20px;" align="left"> 
        <fieldset>
                     
                <div align="left" style="margin-left: 10px; font-size: medium; color: Black; " >Block Vehicle Registration No </div>
            
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td colspan="5">
                        <table style="background-color: #FFFFFF" width="50%" border="0" align="center" cellpadding="3"
                            cellspacing="1">
                            <tr>
                           
                            <td align="left" valign="top" class="form_text">
                                    Vehicle Registration No :</td>
                                <td  align="left" class="form_text"> 
                                    <asp:TextBox ID="txtvehicleRegNo" runat="server" MaxLength="10"     class="form_textbox" Width="154px" ></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red" ></asp:Label>
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Green" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                <asp:UpdatePanel ID="UpdatePanelButton" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                          
                                    <asp:Button ID="btnSave"  runat="server" class="button"  Text="Save" onclick="btnSave_Click"  />
                                   
                          
                                        </ContentTemplate>
                             </asp:UpdatePanel>
                                        
                                </td>
                            </tr>
                                
                            
                           
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>

</asp:Content>
