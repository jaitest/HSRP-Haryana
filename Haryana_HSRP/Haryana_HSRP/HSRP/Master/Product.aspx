<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="HSRP.Master.Product" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" /> 
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function validate() {


            if (document.getElementById("<%=textboxProductCode.ClientID%>").value == "") {
                alert("Please Provide Product Code");
                document.getElementById("<%=textboxProductCode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxProductColor.ClientID%>").value == "") {
                alert("Please Provide Product Color");
                document.getElementById("<%=textboxProductColor.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxProductDimension.ClientID%>").value == "") {
                alert("Please Provide Product Dimension");
                document.getElementById("<%=textboxProductDimension.ClientID%>").focus();
                return false;
            } 
            
            if (invalidChar(document.getElementById("textboxProductCode"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxProductCode").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textboxProductColor"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxProductColor").focus();
                return false;
            } 
            if (invalidChar(document.getElementById("textboxProductDimension"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxPtextboxProductDimensionroductColor").focus();
                return false;
            }
        }
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="margin: 20px;  background-color: #FFFFFF;" align="center">
            <fieldset>
                <legend>
                    <div style="margin-left: 10px; font-size: medium; color: Black">
                        Product</div>
                </legend>
                <br />
                <table>
                    <tr>
                        <td class="form_text">
                            
                        </td>
                        <td class="form_text" align="left" colspan="4">
                            <asp:DropDownList ID="DropDownListProductType" Width="170px" Visible="false" runat="server">
                                <asp:ListItem Text="--Select Product Type--" />
                                <asp:ListItem Text="Number plate" />
                                <asp:ListItem Text="Snap Lock" />
                                <asp:ListItem Text="Third Sticker" />
                                <asp:ListItem Text="Black Foil" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            ProductCode : <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="textboxProductCode" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;&nbsp;
                        </td>
                        <td class="form_text" style="padding-bottom: 10px">
                            Product Color : <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="textboxProductColor" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px">
                            Status :
                        </td>
                        <td>
                            <asp:CheckBox ID="checkBoxActiveStatus" TextAlign="Left" runat="server" TabIndex="2" />
                        </td>
                        <td>
                            &nbsp; &nbsp;
                        </td>
                        <td class="form_text" style="padding-bottom: 10px">
                            Product Dimension : <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="textboxProductDimension" runat="server" class="form_textbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td>
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                    <td colspan="2">
                        <td colspan="5" align="right" style="padding: 0px; vertical-align: top;">
                            <br />
                            <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()"
                                class="button" onclick="buttonUpdate_Click" /> &nbsp; &nbsp;
                            <asp:Button ID="ButtonProductSave" runat="server" Text="Save" TabIndex="3"  OnClientClick=" return validate()"
                                class="button" onclick="ButtonProductSave_Click"  />
                            &nbsp;&nbsp;&nbsp;&nbsp; <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp; 
                                           <input type="reset" class="button" value="Reset" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    </form>
</body>
</html>
