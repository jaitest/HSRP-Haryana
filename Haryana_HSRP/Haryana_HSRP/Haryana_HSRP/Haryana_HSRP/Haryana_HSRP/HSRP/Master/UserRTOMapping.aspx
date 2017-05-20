<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="UserRTOMapping.aspx.cs" Inherits="HSRP.Master.UserRTOMapping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type = "text/javascript">
    function GetSelectedItem() {
        var CHK = document.getElementById("<%=checkBoxListGroup.ClientID%>");
        var checkbox = CHK.getElementsByTagName("input");
        var label = CHK.getElementsByTagName("label");
        for (var i = 0; i < checkbox.length; i++) {
            if (checkbox[i].checked) {
                alert("Selected = " + label[i].innerHTML);
                document.getElementById("<%=checkBoxListGroup.ClientID%>").checked = true;
            }
            else {
                document.getElementById("<%=checkBoxListGroup.ClientID%>").checked = false;
            }
        }
        return false;
    }
</script>

    <div style="background-color:White">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2" height="27" style="background-image: url(../images/midtablebg.jpg)">
                    <span class="headingmain">Assign User To RTO Location</span>
                </td>
            </tr>
            <tr>
                <td colspan="2" height="19" valign="top" nowrap="nowrap" bgcolor="#d2e0f1" class="heading1">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td valign="top" class="form_text">
                    <asp:Label ID="labelOrganization" Font-Size="Medium" ForeColor="Black" runat="server"
                        Text="Select State:"></asp:Label>
                    <span class="alert">* </span>
                </td>
                <td valign="top">
                    <asp:DropDownList Font-Size="Small" Width="200px" Enabled="false" TabIndex="6" AutoPostBack="true"
                        ID="dropDownListState" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                    </asp:DropDownList>
                    <%-- <br />
                                                        <asp:RequiredFieldValidator ID="requiredFieldValidatordropDownListOrg" Text="Select one organization."
                                                            Display="Dynamic" ForeColor="Red" SetFocusOnError="true" InitialValue="--Select Organization--"
                                                            ControlToValidate="dropDownListOrganization" runat="server" />--%>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td width="18%" class="form_text">
                    <asp:Label Text="Select User:" runat="server" Font-Size="Medium" ForeColor="Black" /><span
                        class="alert">* </span>
                </td>
                <td width="82%">
                    <asp:UpdatePanel ID="UpdatePanelUser" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" Font-Size="Small" Width="200px" Enabled="false"
                                TabIndex="1" AutoPostBack="true" ID="dropDownListUser" DataTextField="UserName"
                                DataValueField="UserID" OnSelectedIndexChanged="dropDownListUser_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dropDownListState" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
            <td></td>
            <td>
               
               <%-- <asp:CheckBox ID="che1dd" runat="server" onclick="GetSelectedItem()" />--%>
                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" 
                    ForeColor="Black" oncheckedchanged="CheckBox1_CheckedChanged" Text="Select All" 
                    TextAlign="Left" />
                </td>
                </tr>
                <tr>
            <td valign="top" style="padding-top:30PX" class="form_text"> 
             <asp:Label ID="label1" Font-Size="Medium" ForeColor="Black" runat="server"
                        Text="Select RTO Locations:"></asp:Label> </td>
                <td nowrap="nowrap">
                
                    <asp:UpdatePanel ID="UpdatePanelCheckBoxList" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBoxList runat="server" CellPadding="10" CellSpacing="5" DataTextField="VehicleGroupName"
                                DataValueField="VehicleGroupID" Font-Size="Medium" ForeColor="Black" RepeatColumns="6"  
                                RepeatDirection="Horizontal" ID="checkBoxListGroup" >
                            </asp:CheckBoxList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dropDownListUser" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                
                <td colspan="2" align="center">
                    <asp:Button ID="buttonSave" runat="server" Text="Save" TabIndex="2" class="button"
                        OnClick="buttonSave_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="alert">
                    * Fields are Mandatory
                </td>
            </tr>
            <tr>
                <td colspan="2" class="FieldText">
                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
