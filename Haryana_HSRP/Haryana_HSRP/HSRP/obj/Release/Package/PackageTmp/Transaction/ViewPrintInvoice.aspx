<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPrintInvoice.aspx.cs" Inherits="HSRP.Transaction.ViewPrintInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<link href="../css/main.css" rel="stylesheet" type="text/css" />
<head runat="server">
    <title></title>
</head>
<body>
    <center>
        <form id="form1" runat="server">
       
        <div>
            <table width="30%" style="background-color:White">
                <tr>
                    <td align="center" nowrap="nowrap">
                      <%--  <asp:Button ID="buttonUpdate" runat="server" TabIndex="17" class="button" Visible="True"
                            Text="Print" OnClick="buttonUpdate_Click" Width="90px" />--%>
                        &nbsp;
                        <asp:Button ID="btnDC" runat="server" TabIndex="18" class="button" Text="Print delivery challan"
                            Visible="True" OnClick="buttonSave_Click" />

                        <asp:Button ID="buttonSave" runat="server" TabIndex="18" class="button" Text="Print Invoice/DC"
                            Visible="True" OnClick="buttonSave_Click" />
                        &nbsp;
                        <input type="button" onclick="javascript:parent.googlewin.close();" tabindex="19"
                            name="buttonClose" id="buttonClose" value="Window Close" class="button" />
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </center>
</body>
</html>
