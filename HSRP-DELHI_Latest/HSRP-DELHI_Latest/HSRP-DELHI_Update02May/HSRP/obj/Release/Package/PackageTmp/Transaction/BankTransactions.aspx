<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="BankTransactions.aspx.cs" Inherits="HSRP.Master.BankTransactions" %>

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
                              <span class="headingmain">View Submit Request </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> 
                                    </td>
                                    <td valign="middle" class="form_text">
                                       <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate> 
                                             <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID" 
                                                    onselectedindexchanged="dropDownListClient_SelectedIndexChanged1"   >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient" /> 
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    
                                    


                                   <td valign="middle" class="form_text" nowrap="nowrap"> <asp:Label Text="Order Status:" Visible="true" runat="server" ID="labelOrderStatus" />&nbsp;&nbsp; </td>
                                    <td> <asp:DropDownList AutoPostBack="false" Visible="true" 
                                            ID="dropDownListorderStatus" CausesValidation="false" runat="server"  >
                                        <asp:ListItem>-- Select Status --</asp:ListItem>
                                        <asp:ListItem  Value="New Order"  >New</asp:ListItem> 
                                        <asp:ListItem  Value="Pending"  >Pending</asp:ListItem>
                                        <asp:ListItem Value="Closed" >Closed</asp:ListItem> 
                                                </asp:DropDownList>&nbsp; </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:LinkButton ID="LinkbuttonSearch" runat="server" Text="GO" class="button"  OnClientClick=" return validate()"
                                            onclick="ButtonGo_Click" /> &nbsp;  
                                         
                                    </td>

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
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
                                        <ComponentArt:Grid ID="Grid1" RTOLocationIDMode="AutoID" runat="server" 
                                            LoadingPanelPosition="MiddleCenter" LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                            GroupBySortImageHeight="10" GroupBySortImageWidth="10" GroupBySortDescendingImageUrl="group_desc.gif"
                                            GroupBySortAscendingImageUrl="group_asc.gif"
                                            IndentCellWidth="22" TreeLineImageHeight="19" TreeLineImageWidth="22" TreeLineImagesFolderUrl="~/images/lines/"
                                            PagerImagesFolderUrl="~/images/pager/" PreExpandOnGroup="true" GroupingPageSize="5"
                                            SliderPopupClientTemplateId="SliderTemplate" SliderPopupOffsetX="20" SliderGripWidth="9"
                                            SliderWidth="150" SliderHeight="20" PagerButtonHeight="22" PagerButtonWidth="41"
                                            PagerTextCssClass="GridFooterText" PageSize="25" GroupByTextCssClass="GroupByText"
                                            GroupByCssClass="GroupByCell" FooterCssClass="GridFooter" HeaderCssClass="GridHeader"
                                           
                                             CssClass="Grid" RunningMode="Callback" 
                                            FillContainer="true" 
                                            Height="300px" onitemcommand="Grid1_ItemCommand1">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="UserID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                       <ComponentArt:GridColumn DataField="UserID" Visible="False" />
                                                       <%-- <ComponentArt:GridColumn DataField="SubmitID" HeadingText="SubmitID" SortedDataCellCssClass="SortedDataCell"
                                                            Width="120" />
                                                            
                                                        <ComponentArt:GridColumn DataField="UserID" HeadingText="UserID" SortedDataCellCssClass="SortedDataCell"
                                                            Width="120" />
                                                            
                                                        <ComponentArt:GridColumn DataField="UserHSRPStateID" HeadingText="UserHSRPStateID" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                            
                                                        <ComponentArt:GridColumn DataField="UserRTOLocationID" HeadingText="UserRTOLocationID" SortedDataCellCssClass="SortedDataCell"
                                                            Width="120" />--%>

                                                            <ComponentArt:GridColumn DataField="S.No" FormatString="" HeadingText="S.No"
                                                            SortedDataCellCssClass="SortedDataCell" Width="150" />

                                                        <ComponentArt:GridColumn DataField="RequestedBy" FormatString="" HeadingText="RequestedBy"
                                                            SortedDataCellCssClass="SortedDataCell" Width="150" />
                                                        <ComponentArt:GridColumn DataField="RequestType" Visible="true" HeadingText="RequestType"
                                                            Width="100" />
                                                        <%--<ComponentArt:GridColumn DataField="UploadedFileName" HeadingText="UploadedFileName" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" />--%>
                                                        <%--<ComponentArt:GridColumn DataField="RequestPriority" HeadingText="RequestPriority" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" /> --%>
                                                         <ComponentArt:GridColumn DataField="Remarks" HeadingText="Remarks" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" /> 
                                                       <%-- <ComponentArt:GridColumn DataField="RequestDateTime" HeadingText="RequestDateTime" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> 
                                                        <ComponentArt:GridColumn DataField="ResolveDateTime" HeadingText=" ResolveDateTime" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />  
                                                        <ComponentArt:GridColumn DataField="ResolveBy" HeadingText="ResolveBy" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />--%>
                                                       <%-- <ComponentArt:GridColumn DataField="OrderStatus" HeadingText="Order Status" SortedDataCellCssClass="SortedDataCell"
                                                            Width="70" />--%>
                                                         
                                                       <%-- <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="Assign"  HeadingText="Assign" SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                        <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="LaserFree" HeadingText="Laser Free" SortedDataCellCssClass="SortedDataCell" Width="70" />
                                                       <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="Sticker" HeadingText="White Sticker" SortedDataCellCssClass="SortedDataCell" Width="50" />--%>
                                                       <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="TVSSticker" HeadingText="Action" SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                       <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="ViewUpdateRquest" HeadingText="View" SortedDataCellCssClass="SortedDataCell" Width="50" />

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

                                                <ComponentArt:GridServerTemplate ID="ViewUpdateRquest">
                                                    <Template>
                                                       


                                                         <asp:LinkButton ID="ViewRequest" runat="server" Text="View"  CommandName="ViewUpdateRquest"></asp:LinkButton>
                                                        
                                                        
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
