<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LookUp.aspx.cs" Inherits="HSRP.Master.LookUp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px;" align="center"  style="background-color: #FFFFFF">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size:medium;color:Black">
                    LookUP Table</div>
            </legend>
            <br />
            <table  width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
             <tr valign="top">
                    <td width="35%" class="form_text" nowrap="nowrap" align="left" colspan="2">
                        Login User:
                        <asp:Label ID="LabelCreatedID" ForeColor="Blue" runat="server" />
                    </td>
                    <td class="form_text" nowrap="nowrap" align="left" style="padding-left:5px"  colspan="2">
                        Record Date:
                        <asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="Label_user" style="padding-bottom: 10px"> Lookup Type : <span class="alert">* </span> </td>
                    <td >
                        <asp:DropDownList ID="ddlLookupType" runat="server" Width="170px">
                            <asp:ListItem>Select</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                     
                    <td class="Label_user" style="padding-bottom: 10px" >  Lookup Text : <span class="alert">* </span></td>
                    <td > <asp:TextBox ID="textboxLookupText" runat="server" TabIndex="1" class="form_textbox" ></asp:TextBox> </td>
                </tr>
                <tr>
                    <td class="Label_user" style="padding-bottom: 10px"> Lookup value : <span class="alert">* </span></td>
                    <td  > <asp:TextBox ID="textboxLookupValue" runat="server" TabIndex="2" class="form_textbox"></asp:TextBox> </td> 
                   
                    <td class="Label_user" style="padding-bottom: 10px"> Lookup Order No. : <span class="alert">* </span></td>
                    <td  > <asp:TextBox ID="textboxOrderNo" runat="server" TabIndex="3" class="form_textbox" ></asp:TextBox> </td>
                </tr>
                
                <tr>
                
                    <td colspan="5" align="right">
                        <br />
                        <asp:Button ID="ButtonSubmit" runat="server" Text="Save" TabIndex="5" class="button" />
                        &nbsp;&nbsp; <asp:Button ID="Button2" runat="server" Text="Close" TabIndex="3" class="button" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
