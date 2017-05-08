<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsDispatchRegister.aspx.cs" Inherits="MultiTrack.Master.GoodsDispatchRegister" %>

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
        <div style="margin: 20px;" align="center">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Goods Dispatch Register</div>
            </legend>
            <br />
            <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                <tr>
                    <td class="form_text">
                        Dispatch Code: <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxDispatchCode" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td class="form_text">
                        Dispatch Type: <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDispatchType" runat="server" Width="158px" TabIndex="1">
                            <asp:ListItem>Select</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        Dispatch State: <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDispatchState" runat="server" Width="158px" TabIndex="2">
                            <asp:ListItem>Select</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td class="form_text">
                        Dispatch TO RTOLocation:<span class="alert">* </span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDispatchTORTOLocationId" runat="server" Width="158px" TabIndex="4">
                            <asp:ListItem>Select</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                <td class="form_text">Created By</td>
                <td>
                    <asp:TextBox ID="TextBox5" class="form_textbox"  runat="server"></asp:TextBox></td>
                <td></td>
                    <td class="form_text">
                        Remarks
                    </td>
                    <td>
                        <asp:TextBox ID="textboxRemark" runat="server" TabIndex="6" TextMode="MultiLine"
                            Columns="30" Rows="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                <tr>
                <td>
                 <b><span>ITEMS:</span></b>
                </td>
                </tr>
                  <tr>
                    <td colspan="5">
                        <hr />
                    </td>
                </tr>
                    <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                        <tr>
                            <td class="form_text"> Product </td>
                            <td class="form_text"> Batch </td>
                            <td class="form_text"> Quantity </td>
                            <td class="form_text"> Edit </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                    <asp:ListItem Text="text1" />
                                    <asp:ListItem Text="text2" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList2" runat="server">
                                    <asp:ListItem Text="text1" />
                                    <asp:ListItem Text="text2" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server" />
                            </td>
                            <td>
                                <a href="#" style="color: Red">Edit</a> <a href="#" style="color: Red">Update</a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DropDownList3" runat="server">
                                    <asp:ListItem Text="text1" />
                                    <asp:ListItem Text="text2" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList4" runat="server">
                                    <asp:ListItem Text="text1" />
                                    <asp:ListItem Text="text2" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox2" runat="server" />
                            </td>
                            <td>
                                <a href="#" style="color: Red">Edit</a> <a href="#" style="color: Red">Update</a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DropDownList5" runat="server">
                                    <asp:ListItem Text="text1" />
                                    <asp:ListItem Text="text2" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList6" runat="server">
                                    <asp:ListItem Text="text1" />
                                    <asp:ListItem Text="text2" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox3" runat="server" />
                            </td>
                            <td>
                                <a href="#" style="color: Red">Edit</a> <a href="#" style="color: Red">Update</a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="DropDownList7" runat="server">
                                    <asp:ListItem Text="text1" />
                                    <asp:ListItem Text="text2" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList8" runat="server">
                                    <asp:ListItem Text="text1" />
                                    <asp:ListItem Text="text2" />
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox4" runat="server" />
                            </td>
                            <td>
                                <a href="#" style="color: Red">Edit</a> <a href="#" style="color: Red">Update</a>
                            </td>
                        </tr>
                        <tr align="right">
                    <td align="right" colspan="5" style="padding-right: 50px;">
                        <asp:Button ID="Button1" runat="server" Text="Save" class="button" TabIndex="8" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="Button2" runat="server" Text="Close" class="button" TabIndex="8" />
                    </td>
                </tr>
                </tr>
                    </table>
                
            </table>
        </fieldset>
        <br />
    </div>

    </form>
</body>
</html>
