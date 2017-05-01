<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="RTOHRAssignment.aspx.cs" Inherits="HSRP.Master.RTOHRAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
    <div style="background-color:White">
   
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="7" height="27"  width="80%" style="background-image: url(../images/midtablebg.jpg)">
                    <span class="headingmain">Assign User To RTO Location</span>
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
                    <asp:Label ID="labelOrganization" Font-Size="Medium" ForeColor="Black" runat="server" Height="20px"
                        Text="Select State:"></asp:Label>
                    <span class="alert">* </span>
                </td>
                <td valign="top" width="20%">
                    <asp:DropDownList Font-Size="Small" Width="200px" Enabled="false" TabIndex="6" AutoPostBack="true" Height="35px"
                        ID="dropDownListState" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                    </asp:DropDownList>
                  
                    <br />
                </td>

                <td valign="middle" class="form_text" width="10%">
                    <asp:Label Text="Select RTO:" runat="server" Font-Size="Medium" ForeColor="Black" Height="20px"/><span
                        class="alert">* </span>
                </td>
                <td valign="top" width="20%">
                    <asp:UpdatePanel ID="UpdatePanelUser" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" Font-Size="Small" Width="200px" Enabled="false" Height="35px"
                                TabIndex="1" AutoPostBack="true" ID="dropDownListUser" DataTextField="RTOLocationName"
                                DataValueField="RTOLocationID" OnSelectedIndexChanged="dropDownListUser_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="dropDownListState" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>

                <td valign="middle" class="form_text" width="10%">
                        <asp:Label ID="lblcounter" runat="server" Text="AssignmentDay:" Font-Size="Medium" ForeColor="Black"  Height="20px"></asp:Label>
                        <span class="alert">* </span>
                    </td>
                    <td valign="top" width="20%">
                        <asp:TextBox class="form_textbox" ID="txtassignday" runat="server" Height="35px"></asp:TextBox>                                                     
                    </td>
                 <td valign="middle" class="form_text" width="10%">
                    <asp:Button ID="buttonSave" runat="server" Text="Save"  class="button" AutoPostBack="true"  OnClick="buttonSave_Click" Height="35px" Width="70px"/>
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
