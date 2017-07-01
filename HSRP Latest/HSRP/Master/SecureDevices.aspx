<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecureDevices.aspx.cs" Inherits="MultiTrack.Master.SecureDevices" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
     
</head>
<body>
    <form id="form1" runat="server">

    <div>
        <div style="margin: 20px;" align="center">
            <fieldset>
                    <legend>
                <div style="margin-left: 10px; font-size:medium;color:Black">
                    Add Secure Devices 
                   </div>
            </legend>

                <br />
                <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
            <tr>
                <td class="form_text" style="padding-bottom: 10px"> MAC Address </td>
                <td > <asp:TextBox ID="textboxMacAddress"  class="form_textbox"  runat="server"></asp:TextBox> </td> 
                <td></td>
                <td class="form_text" style="padding-bottom: 10px"> Creation Date Time </td> 
                 <td >
                            <asp:TextBox ID="textboxCurrentDateTime" class="form_textbox" TabIndex="1" runat="server"></asp:TextBox> &nbsp;
                            <asp:Image ID="Image1" runat="server" Height="19px"  ImageUrl="~/images/btn_calendar.gif" />
                    <%-- <asp:CalendarExtender ID="CalendarExtender1" PopupButtonID="Image1" TargetControlID="textboxCurrentDateTime" Format="dd/MM/yyyy" runat="server"></asp:CalendarExtender>--%>
                </td>
            </tr>
            <tr>
                <td class="form_text" style="padding-bottom: 10px"> HSRP Status  </td>
                <td > <asp:DropDownList ID="ddlHSRPStatus" class="dropdown_css"  runat="server" TabIndex="2">
                        <asp:ListItem>-- Select --</asp:ListItem>
                    </asp:DropDownList>
                </td>
            <td></td>
                <td class="form_text" style="padding-bottom: 10px">  RTO Location </td>
                <td > <asp:DropDownList ID="ddlRTOLocation" class="dropdown_css"  runat="server" TabIndex="3">
                        <asp:ListItem>-- Select --</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="form_text" style="padding-bottom: 10px"> Status </td>
                <td > <asp:CheckBox ID="chkActiveStatus" Text="Active" runat="server" TabIndex="4" /> </td>
            </tr>
            <tr>
                 
                <td colspan="6" align="right"  > <asp:Button ID="Button1" runat="server" Text="Save" TabIndex="5" class="button" /> &nbsp;&nbsp; <asp:Button ID="Button2" runat="server" Text="Close" TabIndex="3" class="button" /></td>
            </tr>
            
        </table>
            </fieldset>
        </div>
    </div>


    <%--.........................................--%>
     
 </fieldset>
 </div>
    </form>
</body>
</html>
