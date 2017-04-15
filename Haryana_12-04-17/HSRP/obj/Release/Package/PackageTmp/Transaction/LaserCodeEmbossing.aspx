<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LaserCodeEmbossing.aspx.cs" Inherits="HSRP.Master.LaserCodeEmbossing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Laser Plate Make Free
                </div>
            </legend>
            <div style="width: 880px; margin: 0px auto 0px auto">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                <tr>
                <td colspan="7">&nbsp;</td> 
                </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            HSRP Authorization No :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelAuthorizationNo" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            State Name :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelStateID" runat="server" class="form_text" Text="State Name"></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px"> RTO Location Name : </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelRTOLocationID" runat="server" class="form_text" Text="RTO Location "></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            Vehicle Owner Name :</td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelVehicleOwnerName" runat="server" class="form_text" Text="Vehicle Owner Name"></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            Mobile No :</td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelMobileNo" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            Vehicle Class :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelVehicleClass" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            Vehicle Type:
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelVehicleType" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            Vehicle Registration No :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelVehicleRegNo" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            Engine Number :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelEngineNumber" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                   <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            Chasis No :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelChasisNo" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            Cash Receipt No : 
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelCashReceiptNo" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            Total Amount :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelTotalAmount" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            Front Plate Size And Color :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelFrontPlateSizeAndColor" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            Real Plate Size And Color :
                        </td>
                        <td></td>
                        <td>

                            <asp:Label ID="LabelRealPlateSizeColor" runat="server" class="form_text" Text="Authorization No"></asp:Label>
                        </td>   
                    </tr>
                    
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                        <asp:Label ID="LabelFrontPlate" runat="server" class="form_text" 
                                 Text=" Front Plate Laster No :"></asp:Label>
                           
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelFrontPlateLaserNo" runat="server" class="form_text" 
                                 Text="Authorization No"></asp:Label>
                             
                        </td>  
                   <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            <asp:LinkButton ID="LinkButtonFrontLaserNoFree" runat="server" OnClientClick="return confirm('Do you really want to Free this Laser Code?');"
                                onclick="LinkButtonFrontLaserNoFree_Click" style="font-weight: 700">Make Free</asp:LinkButton>
                        </td>
                        <td></td>
                        <td>
                            <asp:LinkButton ID="LinkButtonFrontLaserNoReject" runat="server" OnClientClick="return confirm('Do you really want to Reject this Plate?');"
                                style="font-weight: 700" onclick="LinkButtonFrontLaserNoReject_Click">Plate Reject</asp:LinkButton>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                         <asp:Label ID="LabelRearPlate" runat="server" class="form_text" 
                                 Text="Rear Plate Laster No :"></asp:Label>
                           
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelRearPlateLaserNo" runat="server" class="form_text" 
                                Text="Authorization No"></asp:Label>
                             
                        </td>  
                   <td></td>
                        <td class="form_text" style="padding-bottom: 10px">
                            <asp:LinkButton ID="LinkButtonRearLaserNoFree" runat="server"  OnClientClick="return confirm('Do you really want to Free this Laser Code?');"
                                style="font-weight: 700" onclick="LinkButtonRearLaserNoFree_Click">Make Free</asp:LinkButton>
                        </td>
                        <td></td>
                        <td>
                            <asp:LinkButton ID="LinkButtonRearLaserNoReject" runat="server"  OnClientClick="return confirm('Do you really want to Reject this Plate?');"
                                style="font-weight: 700" onclick="LinkButtonRearLaserNoReject_Click">Plate Reject</asp:LinkButton>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            3D Sticker :
                        </td>
                    <td>&nbsp;</td>
                    <td colspan="5" class="form_text" style="padding-bottom: 10px">
                        <asp:CheckBox ID="CheckBoxSticker" runat="server" />
                        </td>
                    

                    </tr>
                     <tr>
                       <td> <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                             <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                    </td>
                        <td colspan="3" align="center">

                            
                        </td>  
                        <td colspan="5" align="right" >
                        <%--<asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8" 
                            class="button" onclick="buttonUpdate_Click" />--%><%--
                            <asp:Button ID="buttonSave" runat="server" Text="SAVE LASER NO" 
                                onclick="buttonSave_Click" />--%>
                            &nbsp;&nbsp;
                  
                     <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> 
                    
                    <input type="reset" class="button" value="Reset" />
                </td> 
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
    </form>
</body>
</html>
