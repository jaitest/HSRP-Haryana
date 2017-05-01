<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryIssue.aspx.cs" Inherits="MultiTrack.Master.InventoryIssue" %>

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
                <div style="margin-left: 10px; font-size:medium;color:Black">
                    Inventory Issue </div>
            </legend>
   <div style="width: 856px; margin: 0px auto 0px auto">
        <table >
            <tr>
                <td class="lable_style" > Product </td>
                <td class="lable_style">
                    <asp:DropDownList ID="dropDownListProduct"  runat="server" Width="160px">
                        
                    </asp:DropDownList>
                </td>
             
                <td class="lable_style"> State  </td>
                <td style="height: 33px">
                    <asp:DropDownList ID="dropDownListState" runat="server" Width="160px" 
                        TabIndex="1">
                       
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="lable_style"> RTO Location  </td>
                <td class="lable_style">
                    <asp:DropDownList ID="dropDownListRTOLocation" runat="server" Width="160px" 
                        TabIndex="2"> 
                    </asp:DropDownList>
                </td>
            
                <td class="lable_style">
                    User Name
                </td>
                <td>
                    <asp:DropDownList ID="dropDownListUsername" runat="server" Width="160px" 
                        TabIndex="3">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="lable_style">
                    Laser Number
                </td>
                <td>
                    <asp:DropDownList ID="dropDownListLasernumber" runat="server" Width="160px" 
                        TabIndex="4"> 
                    </asp:DropDownList>
                </td>
            
                <td class="lable_style">
                    Inventory Issue Date
                </td>
                <td>
                    <asp:TextBox ID="textBoxInventoryIssueDate" class="form_textbox" runat="server" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="lable_style">
                    Inventory Status
                </td>
                <td>
                    <asp:DropDownList ID="dropDownListInventoryStatus" runat="server" Width="160px" 
                        TabIndex="6"> 
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="buttonSubmit" runat="server" Text="Submit" TabIndex="7" class="button" />
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </fieldset>
    </div>
    </form>
</body>
</html>
