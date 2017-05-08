<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="EmbossingCenterWiseQuantity.aspx.cs" Inherits="HSRP.Transaction.EmbossingCenterWiseQuantity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" style="background-image: url(../images/midtablebg.jpg)">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="26">
                                            <span class="headingmain">Embossing Center</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                    <tr>
                                        <td height="26" bgcolor="#FFFFFF" class="maintext">
                                            <table width="98%" border="0" align="center" cellpadding="3" cellspacing="3">
                                                <tr>
                                                    <td>
                                                        <span id="lblMsg" class="header"></span>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="label1" runat="server" Text="Embossing Center:"></asp:Label>
                                                    </td>
                                                    <td width="30%">
                                                    <asp:DropDownList ID="dropDownListClient" Height="20px" Width="228px" Tabindex="1"
                                                    runat="server" DataTextField="EmbCenterName" AutoPostBack="false"
                                                    DataValueField="NAVEMBID" >
                                                </asp:DropDownList>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="labelboxno" runat="server" Text="Box No:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  MaxLength="11" TabIndex="1"
                                                            ID="txtboxno" runat="server"></asp:TextBox>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lbllaserfrom" runat="server" Text="Laser from:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  MaxLength="11" TabIndex="2"
                                                            ID="txtlaserfrom" runat="server"></asp:TextBox>
                                                     
                                                   </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lbllaserto" runat="server" Text="Laser To:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="11"
                                                            ID="txtlaserto" runat="server" ></asp:TextBox>
                                                   </td>

                                                   <%--<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTimein" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                </tr>

                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblQuantity" runat="server" Text="Quantity:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="txtQuantity" runat="server" ></asp:TextBox>
                                                   </td>
                                                      <%--<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txttimeout" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 45px" class="form_text" align="center" colspan="2" width="50%">
                                                        <asp:Button ID="buttonSave" runat="server" Text="Save" class="button"  OnClick="buttonSave_Click" />&nbsp;&nbsp;
                                                        &nbsp;&nbsp;
                                                        <input type="reset" id="Reset" value="Reset" class="button" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="alert">
                                                        * Fields are mandatory.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" class="FieldText">
                                                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
