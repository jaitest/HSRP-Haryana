<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DealerMapingMaster.aspx.cs" Inherits="HSRP.Master.DealerMapingMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



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
                        Text="Select User Name:"></asp:Label>
                    <span class="alert">* </span>
                </td>
                <td valign="top">
                    <asp:DropDownList Font-Size="Small" Width="200px" Enabled="false" TabIndex="6" AutoPostBack="true"
                        ID="dropDownLisuusername" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="dropDownLisuusername_SelectedIndexChanged"
                       >
                    </asp:DropDownList>
                   
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
                    <asp:Label Text="Select Dealer Name:" runat="server" Font-Size="Medium" ForeColor="Black" /><span
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
          
                </tr>
                <tr>
      
                <td nowrap="nowrap">
                
                 
                </td>
            </tr>
            <tr>
                
                <td colspan="2" align="center">
                    <asp:Button ID="buttonSave" runat="server" Text="Map" TabIndex="2" class="button"
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

