<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VehicleModel.aspx.cs" Inherits="HSRP.Master.VhicleModel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListVehicleMaker.ClientID%>").value == "0") {
                alert("Please Provide Vehicle Maker Desc");
                document.getElementById("<%=DropDownListVehicleMaker.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=textboxVehicleModel.ClientID%>").value == "") {
                alert("Please Provide Vehicle Model desc");
                document.getElementById("<%=textboxVehicleModel.ClientID%>").focus();
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Vehicle Model</div>
            </legend>
            <br />
            <table width="400px;">
                <tr>
                    <td class="lable_style">
                        Vehicle Maker Desc:<span style="color:Red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListVehicleMaker" runat="server" Width="220px" class="form_textbox">
                        <asp:ListItem>.....Select Model Name.....</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td class="lable_style">
                        Vehicle Model Desc:<span style="color:Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="textboxVehicleModel" runat="server" class="form_textbox"></asp:TextBox>
                    </td>
                </tr>
                <tr class="lable_style">
                    <td>
                        Active Status:
                    </td>
                    <td>
                        <asp:CheckBox ID="checkBoxActiveStatus" TextAlign="Left" runat="server"  />
                    </td>
                </tr>
                <tr align="right" style="margin-right: 10px">
                    <td>
                        <%--<asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>--%>
                    </td>
                    <td colspan="2">
                        <br />
                        <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8" OnClientClick=" return validate()"
                            class="button" OnClick="buttonUpdate_Click" />&nbsp;&nbsp;
                        <asp:Button ID="ButtonSave" runat="server" Text="Save" TabIndex="2" OnClientClick=" return validate()"
                            class="button" OnClick="ButtonSave_Click1" />&nbsp;&nbsp;
                        <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                            id="buttonClose" value="Close" class="button" />
                        &nbsp;&nbsp;
                        <input type="reset" class="button" value="Reset" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <asp:HiddenField ID="H_VehicleModelID" runat="server" />
    </div>
    </form>
</body>
</html>
