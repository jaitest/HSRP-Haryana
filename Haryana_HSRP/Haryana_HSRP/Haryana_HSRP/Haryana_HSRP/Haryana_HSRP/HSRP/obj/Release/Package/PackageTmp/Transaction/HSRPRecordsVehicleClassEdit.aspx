<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HSRPRecordsVehicleClassEdit.aspx.cs" Inherits="HSRP.Transaction.HSRPRecordsVehicleClassEdit" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <div style="width: 100%">
        <table width="100%" cellpadding="0" cellspacing="0" border="1">
            <tr>
                <td align="center">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr align="left">
                            <td background="../images/midtablebg.jpg" align="left">
                                <asp:Label ID="Label4" class="headingmain" runat="server">Vehicle Class Update</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                               <table cellpadding="0" cellspacing="0" style="width: 30%">                                   
                                    <tr>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            <asp:Label Text="Vehicle No:" Visible="true" runat="server" ID="Label1" />
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtVehicleNo" runat="server"></asp:TextBox>
                                        </td>                                       
                                        <td class="auto-style2" nowrap="nowrap">
                                            <asp:Button ID="btnGo" Width="58px" runat="server"
                                                Text="GO" ToolTip="Please Click for get Details"
                                                class="button" OnClick="btnGO_Click" AutoPostBack="true" />
                                        </td>
                                        <td class="auto-style2" nowrap="nowrap">
                                            <asp:LinkButton ID="lnkModifyVehicle" runat="server" AutoPostBack="true" OnClick="lnkModifyVehicle_Click" Text="Modify Vehicle No."></asp:LinkButton>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left"  class="form_text" nowrap="nowrap" colspan="3">
                                            &nbsp;&nbsp;</td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td align="left"  class="form_text" nowrap="nowrap" colspan="3">
                                            <asp:Label ID="lblErrMsg" runat="server" ForeColor="red"></asp:Label>
                                            <asp:Label ID="lblmsg" runat="server" ForeColor="#006600"></asp:Label>
                                        </td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            &nbsp;</td>
                                    </tr>                                   
                                   
                                    <tr>
                                        <td align="left"  class="form_text" nowrap="nowrap" colspan="3">
                                            &nbsp;&nbsp;</td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            &nbsp;</td>
                                    </tr>                                   
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="td_Vehicle" runat="server" >
                                <br />
                                <table width="50%" cellpadding="0" cellspacing="0" class="form_text">
                                    <tr>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            <asp:Label Text="Order Type:" Visible="true" runat="server" ID="Label2" />
                                        </td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            <asp:TextBox ID="txtOrderType" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            <asp:Label Text="Vehicle Class:" Visible="true" runat="server" ID="lblVehi" />
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList AutoPostBack="true" CausesValidation="false" ID="ddlVehicleClass"  runat="server" Width="170px">
                                                <asp:ListItem Value="--Select Vehicle Class--" Text="--Select Vehicle Class--"></asp:ListItem>
                                                <asp:ListItem Value="TRANSPORT" Text="Transport"></asp:ListItem>
                                                <asp:ListItem Value="NON-TRANSPORT" Text="Non-Transport"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td class="form_text" nowrap="nowrap" align="left">
                                            <asp:Label Text="Approved By:" runat="server" ID="Label3" />
                                        </td>
                                        <td  align="left">
                                            <asp:TextBox ID="txtapprovedBy" runat="server"></asp:TextBox>
                                        </td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            <asp:Label Text="Remarks:" runat="server" ID="Label5" />
                                        </td>
                                        <td align="left">
                                             <asp:TextBox ID="txtRemarks" runat="server" Height="90px" TextMode="MultiLine" Width="280px"></asp:TextBox>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td class="form_text" nowrap="nowrap" align="center" colspan="4">
                                            &nbsp;&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="form_text" nowrap="nowrap" align="center" colspan="4">                                            
                                            <asp:Button ID="btnEdit" Width="58px" runat="server"
                                                Text="Edit" ToolTip="Please Click for Edit"
                                                class="button" OnClick="btnEdit_Click"/>
                                            <asp:HiddenField ID="HiddenRecordid" runat="server" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form_text" nowrap="nowrap" align="center" colspan="4">
                                            &nbsp; &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
