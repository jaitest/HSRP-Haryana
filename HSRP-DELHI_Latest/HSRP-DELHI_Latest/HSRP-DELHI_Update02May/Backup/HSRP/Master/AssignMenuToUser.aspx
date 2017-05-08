<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AssignMenuToUser.aspx.cs" Inherits="HSRP.Master.AssignMenuToUser" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" background="../images/midtablebg.jpg">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="26">
                                            <span class="headingmain">Assign Menu To User</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top" bgcolor="#c0c0c0">
                                            <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                                <tr>
                                                    <td width="3%" height="10" valign="top" nowrap="nowrap" bgcolor="#d2e0f1" class="heading1">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="26" bgcolor="#FFFFFF" class="maintext">
                                                        <table width="98%" border="0" align="center" cellpadding="3" cellspacing="3">
                                                            <tr>
                                                                <td width="18%" class="form_text">
                                                                    Organization:
                                                                </td>
                                                                <td width="82%">
                                                                    <asp:DropDownList TabIndex="6" Enabled="false" AutoPostBack="true" CausesValidation="false" Width="125px"
                                                                        ID="dropDownListHSRPState" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                                                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="18%" class="form_text">
                                                                    Client:
                                                                </td>
                                                                <td width="82%">
                                                                    <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanelClient" ChildrenAsTriggers="true"
                                                                        runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList AutoPostBack="true" Enabled="false" TabIndex="6" CausesValidation="false" Width="125px"
                                                                                ID="dropDownListRTOLocation" runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID"
                                                                                OnSelectedIndexChanged="dropDownListClient_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="dropDownListHSRPState" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="form_text">
                                                                    Select User:
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel UpdateMode="Conditional" ID="UpdatePanelUser" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList AutoPostBack="true" CausesValidation="true" ID="dropDownListUser" Width="125px"
                                                                                Enabled="false" runat="server" TabIndex="8" DataTextField="Names" DataValueField="UserID"
                                                                                OnSelectedIndexChanged="dropDownListUser_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="dropDownListRTOLocation" EventName="SelectedIndexChanged" />
                                                                            <asp:AsyncPostBackTrigger ControlID="dropDownListHSRPState" EventName="SelectedIndexChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="form_text">
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="15px"></asp:Label>
                                                                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="15px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" class="maintext">
                                                                    <div style="width: 100%; height: 500px; overflow: auto;">
                                                                    <%--<ComponentArt:TreeView ID="TreeView1" HoverPopupEnabled="true" hoverpopupcssclass="NodePopup"
                                                                                    FillContainer="true" Height="100%" Width="100%" DragAndDropEnabled="true" NodeEditingEnabled="true"
                                                                                    ShowLines="True" CssClass="TreeView" NodeCssClass="TreeNode" HoverNodeCssClass="HoverTreeNode"
                                                                                    SelectedNodeCssClass="SelectedTreeNode" NodeEditCssClass="NodeEdit" ImagesBaseUrl="../Images/"
                                                                                    ParentNodeImageUrl="folder.gif" ExpandedParentNodeImageUrl="folder_open.gif"
                                                                                    LeafNodeImageUrl="feed.gif" LineImagesFolderUrl="../Images/lines/" LineImageWidth="19"
                                                                                    LineImageHeight="20" NodeLabelPadding="3" DefaultImageHeight="16" DefaultImageWidth="16"
                                                                                    runat="server">
                                                                                </ComponentArt:TreeView>--%>
                                                                        <asp:UpdatePanel runat="server" ID="UpdatePanelTree" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <ComponentArt:TreeView ID="TreeView1" HoverPopupEnabled="true" hoverpopupcssclass="NodePopup"
                                                                                    FillContainer="true" Height="100%" Width="100%" DragAndDropEnabled="true" NodeEditingEnabled="true"
                                                                                    ShowLines="True" CssClass="TreeView" NodeCssClass="TreeNode" HoverNodeCssClass="HoverTreeNode"
                                                                                    SelectedNodeCssClass="SelectedTreeNode" NodeEditCssClass="NodeEdit" ImagesBaseUrl="../Images/"
                                                                                    ParentNodeImageUrl="folder.gif" ExpandedParentNodeImageUrl="folder_open.gif"
                                                                                    LeafNodeImageUrl="feed.gif" LineImagesFolderUrl="../Images/lines/" LineImageWidth="19"
                                                                                    LineImageHeight="20" NodeLabelPadding="3" DefaultImageHeight="16" DefaultImageWidth="16"
                                                                                    runat="server">
                                                                                </ComponentArt:TreeView>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="dropDownListUser" EventName="SelectedIndexChanged" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="alert" colspan="2" align="center">
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" OnClick="btnSave_Click"
                                                                        Width="63px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" class="alert">
                                                                    &nbsp;
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
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
