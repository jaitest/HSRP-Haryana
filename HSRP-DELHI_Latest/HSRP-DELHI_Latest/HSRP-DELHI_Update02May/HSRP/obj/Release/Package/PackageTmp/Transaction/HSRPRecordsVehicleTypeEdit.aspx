<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HSRPRecordsVehicleTypeEdit.aspx.cs" Inherits="HSRP.Transaction.HSRPRecordsVehicleTypeEdit" %>
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
                                <asp:Label ID="Label4" class="headingmain" runat="server">Vehicle Type update</asp:Label>
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
                                            <asp:Label Text="Vehicle Class:" Visible="true" runat="server" ID="Label6" />
                                        </td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            <asp:TextBox ID="txtVehicleClass" runat="server" Enabled="false"></asp:TextBox>
                                        </td>                                                                            
                                    </tr>
                                    <tr>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            <asp:Label Text="Vehicle Type:" Visible="true" runat="server" ID="lblVehi" />
                                        </td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            <asp:DropDownList AutoPostBack="true" CausesValidation="false" ID="ddlVehicleType"  runat="server" Width="170px" OnSelectedIndexChanged="ddlVehicleType_SelectedIndexChanged">
                                                <asp:ListItem Value="--Select Vehicle Type--" Text="--Select Vehicle Type--"></asp:ListItem>
                                                <asp:ListItem Value="SCOOTER" Text="SCOOTER"></asp:ListItem>
                                                <asp:ListItem Value="MOTOR CYCLE" Text="MOTOR CYCLE"></asp:ListItem>
                                                <asp:ListItem Value="TRACTOR" Text="TRACTOR"></asp:ListItem>
                                                <asp:ListItem Value="THREE WHEELER" Text="THREE WHEELER"></asp:ListItem>
                                                <asp:ListItem Value="LMV" Text="LMV"></asp:ListItem> 
                                                <asp:ListItem Value="LMV(CLASS)" Text="LMV(CLASS)"></asp:ListItem> 
                                                <asp:ListItem Value="MCV/HCV/TRAILERS" Text="MCV/HCV/TRAILERS"></asp:ListItem>
                                                <asp:ListItem Value="E-RICKSHAW" Text="E-RICKSHAW"></asp:ListItem>
                                             </asp:DropDownList>
                                        </td>
                                        <td align="left"  class="form_text" nowrap="nowrap">
                                            
                                        </td>
                                        <td align="left">
                                            
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
                                            <asp:Label ID="labelFrontPlateSize" runat="server" Visible="false"></asp:Label>
                                            <asp:Label ID="labelRearPlateSize" runat="server" Visible="false"></asp:Label>
                                            <asp:CheckBox class="form_text" ID="checkBoxThirdSticker" runat="server" Visible="false" />
                                            <asp:CheckBox class="form_text" ID="checkBoxSnapLock" runat="server" Visible="false" />
                                            <asp:TextBox ID="HiddenFieldFrontPlate" Width="10px" Visible="false" Text="0" runat="server" />
                                            <asp:TextBox ID="HiddenFieldRearPlate" Width="10px" runat="server" Text="0" Visible="false" />                                                                                                                                                                  
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form_text" nowrap="nowrap" align="center" colspan="4">
                                            <asp:TextBox ID="HiddenFieldSticker" runat="server" Width="10px" Text="0" Visible="false" />
                                            <asp:TextBox ID="HiddenFieldScrew" runat="server" Width="10px" Text="0" Visible="false" />
                                            <asp:TextBox ID="HiddenFieldFixing" runat="server" Width="10px" Text="0" Visible="false" />    
                                            <asp:TextBox ID="HiddenFieldFrontPlateCode" runat="server" Width="10px" Text="0"  Visible="false" />
                                            <asp:TextBox ID="HiddenFieldRearPlateCode" runat="server" Width="10px" Text="0" Visible="false" />
                                            <asp:TextBox ID="HiddenFieldCashReceipt" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="HiddenFieldHSRPRecords" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="textBoxVat" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="textBoxVatAmount" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="textBoxNetTotal" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox> 
                                            <asp:TextBox ID="textBoxServiceTax" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="textboxServiceTaxAmount" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>                                            
                                            <asp:TextBox ID="textBoxTotalAmount" runat="server" Width="10px" Text="0" Visible="false"></asp:TextBox>        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="form_text" nowrap="nowrap" align="center" colspan="4">
                                            <asp:HiddenField ID="HiddenRecordid" runat="server" Visible="false" />
                                            <asp:Button ID="btnEdit" Width="58px" runat="server"
                                                Text="Edit" ToolTip="Please Click for Edit"
                                                class="button" OnClick="btnEdit_Click"/>
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
