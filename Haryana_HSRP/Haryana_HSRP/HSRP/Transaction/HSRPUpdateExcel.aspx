<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HSRPUpdateExcel.aspx.cs"
    Inherits="HSRP.Transaction.HSRPUpdateExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding:15px 15px 15px 15px">
        <fieldset>
            <legend title="Update File Status" >
              Update File Status
            </legend>
            <table width="100%" border="0" align="left" style="padding-left:10px" cellpadding="3" cellspacing="1" style="background-color: white">
                 <tr>
                    <td class="form_text">
                        User Name :
                    </td>
                    <td class="form_text">
                        <asp:Label ID="LabelUserName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        State Name :
                    </td>
                    <td class="form_text">
                        <asp:Label ID="txtStateName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        RTO Location Name :
                    </td>
                    <td class="form_text">
                        <asp:Label ID="labelRtolOcation" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        File Name :
                    </td>
                    <td class="form_text">
                        <asp:Label ID="labelfileName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        Status :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                            <asp:ListItem Text="--Select Status--" Value="--Select Status--" />
                            <asp:ListItem Text="New" Value="New" />
                            <asp:ListItem Text="Review" Value="Review" />
                            <asp:ListItem Text="Database Upload" Value="Database Upload" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        Remarks :
                    </td>
                    <td>
                        <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" Height="68px" Width="291px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <%--  <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8" OnClientClick=" javascript:return vali();"
                                    class="button" OnClick="buttonUpdate_Click" />--%>&nbsp;&nbsp;
                        <%--OnClientClick=" javascript:return vali();"--%>
                        <asp:Button ID="btnSave" runat="server" Text="Update Excel Status" TabIndex="4" class="button"
                            OnClick="btnSave_Click" />&nbsp;&nbsp;
                        <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                            id="buttonClose" value="Close" class="button" />
                        &nbsp;&nbsp;
                        <%--<input type="reset" class="button" value="Reset" />--%>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
