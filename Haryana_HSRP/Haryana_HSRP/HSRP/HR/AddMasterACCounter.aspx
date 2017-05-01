<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AddMasterACCounter.aspx.cs" Inherits="HSRP.HR.AddMasterACCounter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div style="background-color:White">
   
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
           
            <tr>
                <td colspan="7" height="27"  style="background-image: url(../images/midtablebg.jpg)">
                    <span class="headingmain">Master AC Counter</span>
                </td>
            </tr>
         
            <tr>
                <td colspan="7" height="19" valign="top" nowrap="nowrap" bgcolor="#d2e0f1" class="heading1">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                
                <td valign="middle" class="form_text" width="10%">
                    <asp:Label ID="labelOrganization" Font-Size="Medium" ForeColor="Black" Height="20px" runat="server" Text="Select State:"></asp:Label>
                    <span class="alert">* </span>
                </td>
                <td valign="top" width="20%">
                    <asp:DropDownList Font-Size="Small" Enabled="false" AutoPostBack="true" Width="200px" Height="35px"
                        ID="dropDownListState" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                    </asp:DropDownList>
                  
                </td>

                <td width="10%" class="form_text" valign="middle">
                    <asp:Label Text="Select User:" runat="server" Font-Size="Medium"  Height="20px" ForeColor="Black" /><span
                        class="alert">* </span>
                </td>
                <td width="20%" valign="top">
                    <asp:UpdatePanel ID="UpdatePanelUser" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" Font-Size="Small" Width="200px" Enabled="false" Height="35px"
                                TabIndex="1" AutoPostBack="true" ID="dropDownListUser" DataTextField="UserName"
                                DataValueField="UserID" OnSelectedIndexChanged="dropDownListUser_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dropDownListState" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>

                <td width="10%" class="form_text" valign="middle">
                        <asp:Label ID="lblcounter" runat="server" Text="ACCounter:" Font-Size="Medium" ForeColor="Black"  Height="20px"></asp:Label>
                        <span class="alert">* </span>
                    </td>
                    <td width="20%" valign="top">
                        <asp:TextBox class="form_textbox" ID="txtaccounter" Height="35px" runat="server"></asp:TextBox>                                                     
                    </td>
                <td width="10%" class="form_text" valign="middle">
                <asp:Button ID="buttonSave" runat="server" Text="Save" class="button" Height="35px" Style="padding-left:-20px"  Width="70px" OnClick="buttonSave_Click" />
                </td>
            </tr>
           
           
            <tr>
                <td colspan="2" class="FieldText">
                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                </td>
            </tr>
        </table>
      
    </div>
</asp:Content>
