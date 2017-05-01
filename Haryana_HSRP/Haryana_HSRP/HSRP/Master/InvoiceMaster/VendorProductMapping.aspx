<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="VendorProductMapping.aspx.cs" Inherits="HSRP.Master.InvoiceMaster.VendorProductMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-color: White">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" height="27" style="background-image: url(../images/midtablebg.jpg)">
                    <span class="headingmain">Assign Products To Vendor</span>
                </td>
            </tr>
            <tr>
                <td colspan="2" height="19" valign="top" nowrap="nowrap" bgcolor="#d2e0f1" class="heading1">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td width="18%" class="form_text">
                    <asp:Label ID="Label1" Text="Select Vendor:" runat="server" Font-Size="Medium" ForeColor="Black" /><span
                        class="alert">* </span>
                </td>
                <td width="82%">
                    <asp:DropDownList runat="server" Font-Size="Small" Width="200px" TabIndex="1" AutoPostBack="true"
                        ID="dropDownListVendor" DataTextField="Name" DataValueField="VendorID" OnSelectedIndexChanged="dropDownListVendor_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" style="padding-top: 30PX" class="form_text">
                    <asp:Label ID="label2" Font-Size="Medium" ForeColor="Black" runat="server" Text="Select Product:"></asp:Label>
                </td>
                <td nowrap="nowrap">
                    <asp:UpdatePanel ID="UpdatePanelCheckBoxList" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBoxList runat="server" CellPadding="10" CellSpacing="5" DataTextField="ProductName"
                                DataValueField="ProductID" Font-Size="Medium" ForeColor="Black" RepeatColumns="3"
                                RepeatDirection="Horizontal" ID="checkBoxListGroup">
                            </asp:CheckBoxList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dropDownListVendor" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:UpdatePanel ID="UpdatePanelSave" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="buttonSave" runat="server" Text="Save" TabIndex="2" class="button"
                                OnClick="buttonSave_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dropDownListVendor" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="alert">
                    * Fields are Mandatory
                </td>
            </tr>
            <tr>
                <td colspan="2" class="FieldText">
                    <asp:UpdatePanel ID="UpdatePanelError" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                            <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dropDownListVendor" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="buttonSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
