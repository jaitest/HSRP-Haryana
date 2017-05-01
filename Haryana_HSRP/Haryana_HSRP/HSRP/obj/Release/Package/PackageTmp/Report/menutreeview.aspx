<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menutreeview.aspx.cs" Inherits="HSRP.Report.menutreeview" %>

<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    
    <table class="style1">
        <tr>
            <td>
             <asp:ScriptManager ID="ScriptManager1" runat="server">
             </asp:ScriptManager>
               <asp:UpdatePanel runat="server" ID="UpdatePanelTree" UpdateMode="Conditional">
                   <ContentTemplate>
                        <ComponentArt:TreeView ID="TreeView1" HoverPopupEnabled="true" hoverpopupcssclass="NodePopup"
                                                                                    FillContainer="true" Height="100%" Width="100%" DragAndDropEnabled="true" NodeEditingEnabled="False"
                                                                                    ShowLines="True" CssClass="TreeView" NodeCssClass="TreeNode" HoverNodeCssClass="HoverTreeNode"
                                                                                    SelectedNodeCssClass="SelectedTreeNode" NodeEditCssClass="NodeEdit" ImagesBaseUrl="../Images/"
                                                                                    ParentNodeImageUrl="folder.gif" ExpandedParentNodeImageUrl="folder_open.gif"
                                                                                    LeafNodeImageUrl="feed.gif" LineImagesFolderUrl="../Images/lines/" LineImageWidth="19"
                                                                                    LineImageHeight="20" NodeLabelPadding="3" DefaultImageHeight="16" DefaultImageWidth="16"
                                                                                    runat="server">
 
                        </ComponentArt:TreeView>
                  </ContentTemplate>

             </asp:UpdatePanel>
             
            </td>
        </tr>
    </table>
    
    
    </form>
</body>
</html>
