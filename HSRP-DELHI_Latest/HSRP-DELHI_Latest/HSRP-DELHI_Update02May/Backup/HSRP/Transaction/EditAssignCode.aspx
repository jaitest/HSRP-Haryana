<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAssignCode.aspx.cs" Inherits="HSRP.Transaction.EditAssignCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
   <div>
        <div style="margin: 20px;" align="center">
            <fieldset>
                <legend>
                    <div style="margin-left: 10px; font-size: medium; color: Black">
                        Assign Laser Code</div>
                </legend>
                <br />
                <table>
                    <tr>
                        <td class="form_text">
                            HSRP Authorization No.:&nbsp;&nbsp;
                        </td>
                        <td class="form_text" align="left">
                            <asp:TextBox ID="txtHSRPAuthNo" Width="200px" ReadOnly="true" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                        </td>
                        <td class="form_text" style="padding-bottom: 10px;padding-left:10px">
                            Vehicle Reg. No. : <span class="alert">&nbsp;</span></td>
                        <td width="200px">
                            <asp:TextBox ID="txtVehRegNo" ReadOnly="true" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                        <td class="form_text">
                            Owner Name:&nbsp;&nbsp;
                        </td>
                        <td class="form_text" align="left">
                            <asp:TextBox ID="txtOwnerName" Width="200px" ReadOnly="true" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                        </td>
                        <td class="form_text" style="padding-bottom: 10px;padding-left:10px">
                           Mobile No. : 
                        </td>
                        <td>
                            <asp:TextBox ID="txtMobNo" ReadOnly="true" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                        </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Chassis No.
                            </td>
                            <td class="form_text" align="left" >
                                <asp:TextBox ID="txtChassisNo" Width="200px" ReadOnly="true" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                            </td>
                            <td class="form_text" style="padding-bottom: 10px;padding-left:10px">
                                Engine No. : 
                            </td>
                            <td>
                                <asp:TextBox ID="txtEngineNo" ReadOnly="true" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Vehicle Class :
                            </td>
                            <td class="form_text" align="left" >
                                <asp:TextBox ID="txtVehClass" ReadOnly="true" Width="200px" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                            </td>
                            <td class="form_text" style="padding-bottom: 10px;padding-left:10px">
                                Vehicle Type : 
                            </td>
                            <td>
                                <asp:TextBox ID="txtVehType" ReadOnly="true" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Registration Date :
                            </td>
                            <td class="form_text" align="left" >
                                <asp:TextBox ID="txtRegDate" ReadOnly="true" Width="200px" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                            </td>
                            <td class="form_text" style="padding-bottom: 10px;padding-left:10px" width="200px">
                                Sticker Laser Code : <span class="alert">&nbsp;</span></td>
                            <td>
                                <asp:TextBox ID="txtStickerCode" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Front Laser Code :
                            </td>
                            <td class="form_text" align="left" >
                                <asp:TextBox ID="txtFrontCode" runat="server" Width="200px" TabIndex="1" class="form_textbox"></asp:TextBox>
                            </td>
                            <td class="form_text" style="padding-bottom: 10px;padding-left:10px">
                                Rear Laser Code : <span class="alert">&nbsp;</span></td>
                            <td>
                                <asp:TextBox ID="txtRearCode" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                            </td>
                        </tr>
                         <tr align="left" style="margin-right:10px">
                            <td colspan="4">
                             <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                              <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                            </td>
                        </tr>

                       
                    
                    <tr>
                        <td colspan="4" align="right" style="padding: 0px; vertical-align: top;">
                            <br />
                            <asp:Button ID="Button1" runat="server" Text="Update" TabIndex="3" 
                                class="button" onclick="Button1_Click" />
                                &nbsp;&nbsp;
                             <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    </form>
</body>
</html>
