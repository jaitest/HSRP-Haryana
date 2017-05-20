<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ViewUpdateSubmitRequest.aspx.cs" Inherits="HSRP.Master.ViewUpdateSubmitRequest" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />



     <script type="text/javascript">
         function AddNewPop(k,j,l) { //Define arbitrary function to run desired DHTML Window widget codes

            //googlewin = dhtmlwindow.open("googlebox", "iframe", "UpdateSubmitRequest.aspx?StateId=" + i + "&RTOID=" + j + "&SubmitID=" + k, "Update Request", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
             googlewin = dhtmlwindow.open("googlebox", "iframe", "UpdateSubmitRequest.aspx?QrySubmitID=" + k + "&HSRPStateIDEdit="+ j + "&RTOLocationIDEdit="+l, "Update Request", "width=900px,height=400px,resize=1,scrolling=1,center=1", "recal")
             googlewin.onclose = function () {
                 window.location = 'EditSubmitRequest.aspx';
                 return true;
             }
         }
        </script>
    
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                              <span class="headingmain">View Update Submit Request </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                         
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td >
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td>
                                        <ComponentArt:Grid ID="Grid1" runat="server" LoadingPanelPosition="MiddleCenter" LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                            GroupBySortImageHeight="10" GroupBySortImageWidth="10" GroupBySortDescendingImageUrl="group_desc.gif"
                                            GroupBySortAscendingImageUrl="group_asc.gif"
                                            IndentCellWidth="22" TreeLineImageHeight="19" TreeLineImageWidth="15" TreeLineImagesFolderUrl="~/images/lines/"
                                            PagerImagesFolderUrl="~/images/pager/" PreExpandOnGroup="true" GroupingPageSize="5"
                                            SliderPopupClientTemplateId="SliderTemplate" SliderPopupOffsetX="20" SliderGripWidth="9"
                                            SliderWidth="150" SliderHeight="20" PagerButtonHeight="22" PagerButtonWidth="41"
                                            PagerTextCssClass="GridFooterText" PageSize="25" GroupByTextCssClass="GroupByText"
                                            GroupByCssClass="GroupByCell" FooterCssClass="GridFooter" HeaderCssClass="GridHeader"
                                           
                                             CssClass="Grid" RunningMode="Callback" 
                                            FillContainer="true" 
                                            Height="300px">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="RequestID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                       <ComponentArt:GridColumn DataField="RequestID" Visible="False" />
                                                      

                                                            <ComponentArt:GridColumn DataField="S.No" FormatString="" HeadingText="S.No"
                                                            SortedDataCellCssClass="SortedDataCell" Width="150" />

                                                        <ComponentArt:GridColumn DataField="RecordDateTime" FormatString="" HeadingText="Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="150" />
                                                        <ComponentArt:GridColumn DataField="Remarks" Visible="true" HeadingText="Remarks"
                                                            Width="100" />
                                                      
                                                         <ComponentArt:GridColumn DataField="REQUESTSTATUS" HeadingText="STATUS" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" /> 
                                                     
                                                      <%-- <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="TVSSticker" HeadingText="Action" SortedDataCellCssClass="SortedDataCell" Width="50" />--%>

                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientTemplates>

                                                  <%-- <ComponentArt:ClientTemplate ID="Assign">
                                                    <a style="color: Blue" onclick="javascript:editpage(i)(## DataItem.GetMember('UserID').Value ##);" >
                                                        Action</a>
                                                   </ComponentArt:ClientTemplate> --%>
                                                   <%--<ComponentArt:ClientTemplate ID="LaserFree">
                                                    <a style="color: Red" onclick="javascript:editMakeLaserFree(## DataItem.GetMember('UserID').Value ##);">
                                                        MakeLaserFree</a>
                                                   </ComponentArt:ClientTemplate>

                                                   <ComponentArt:ClientTemplate ID="Embossing">
                                                    <a style="color: Blue" onclick="javascript:editEmbossing(## DataItem.GetMember('UserID').Value ##);">
                                                        Embossing</a>
                                                   </ComponentArt:ClientTemplate>--%>

                                                <ComponentArt:ClientTemplate ID="ClientTemplate2" runat="server">
                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td style="font-size: 10px;">
                                                                Loading...&nbsp;
                                                            </td>
                                                            <td>
                                                                <img alt="loading" src="/Images/spinner.gif" width="16" height="16" border="0" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="ClientTemplate3" runat="server">
                                                    <table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" style="padding: 5px;">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="25" align="center" valign="top" style="padding-top: 3px;">
                                                                        </td>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="2" border="0" style="width: 255px;">
                                                                               
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            Page <b>## DataItem.PageIndex + 1 ##</b> of <b>## Grid1.PageCount ##</b>
                                                                        </td>
                                                                        <td align="right">
                                                                            Record <b>## DataItem.Index + 1 ##</b> of <b>## Grid1.RecordCount ##</b>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                             <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="Sticker">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonSticker" runat="server" Text="Sticker" CommandName="Sticker"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>
                                            <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="TVSSticker">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonStickerTVS" runat="server" Text="Action" OnClientClick="AddNewPop(##QrySubmitID##); return false;" CommandName="TVSSticker"></asp:LinkButton>
                                                        <%--<asp:Button ID="Button1" runat="server" Text="Button" OnClick="AddNewPop(); return false;"></asp:Button>--%>
                                                        <%--<a onclick="AddNewPop(); return false;" title="Add New HSRP State" class="button">Action</a>--%>
                                                        
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>
                                    </ComponentArt:Grid>
                                    </td>
                                </tr>
                                <tr>
                                                <td colspan="6">
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    
                                                </td>
                                          
                                            </tr>
                            </table>
                        </td>
                    </tr> 
                    
                </table>
            </td>
        </tr>
    </table>
    <br /><asp:HiddenField ID="hiddenUserType" runat="server" />
           <td >
                                        <asp:Label ID="lblSubmit" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                        <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" Visible="false" />
                                    </td>
</asp:Content>
