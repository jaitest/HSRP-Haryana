<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewMACbase.aspx.cs" Inherits="HSRP.Master.ViewMACbase" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
 
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View MACbase Active/InActive </span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                       <td></td>
                                       <td></td>
                                      <td valign="middle" class="form_text">  </td>
                                     <td valign="middle" class="form_text">  </td>
                                </tr>
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                      <asp:Label Text="State Name :-"  runat="server" ID="label1"  /> &nbsp;&nbsp; 
                                        <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="True"                                                                    
                                            CausesValidation="false" DataTextField="HSRPStateName"   DataValueField="HSRP_StateID" Height="22px" Width="120px">
                                                  </asp:DropDownList>
                                    </td>

                                    
                                    <td valign="middle" class="form_text">
                                        <asp:Label Text="Status :-"  runat="server" ID="label3"  />  &nbsp;&nbsp; 
                                                         <asp:DropDownList id="ddlstatus"    AppendDataBoundItems="True"   runat="server">
                                                             <asp:ListItem Selected="True" Value="0">----Select Status---</asp:ListItem>
                                                              <asp:ListItem Selected="False" Value="Y"> Active </asp:ListItem>
                                                                 <asp:ListItem Selected="False" Value="N"> InActive </asp:ListItem>
                                                                  </asp:DropDownList>
                                    </td>                                    
                                     <td valign="middle" class="form_text">  <asp:Button ID="btnDetail" runat="server" Text="Detail"  
                                         Font-Bold="True" ForeColor="#3333FF" Height="18px" Width="61px" OnClick="btnDetail_Click" /> </td>
                                   
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        
                                         <ComponentArt:Grid ID="Grid1" PlantIDMode="MacBaseID" runat="server" ImagesBaseUrl="~/images"
                                            Width="100%" GroupingNotificationText="Drag a column to this area to group by it."
                                            LoadingPanelPosition="MiddleCenter" LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                            GroupBySortImageHeight="10" GroupBySortImageWidth="10" GroupBySortDescendingImageUrl="group_desc.gif"
                                            GroupBySortAscendingImageUrl="group_asc.gif" GroupingNotificationTextCssClass="GridHeaderText"
                                            IndentCellWidth="22" TreeLineImageHeight="19" TreeLineImageWidth="22" TreeLineImagesFolderUrl="~/images/lines/"
                                            PagerImagesFolderUrl="~/images/pager/" PreExpandOnGroup="true" GroupingPageSize="5"
                                            SliderPopupClientTemplateId="SliderTemplate" SliderPopupOffsetX="20" SliderGripWidth="9"
                                            SliderWidth="150" SliderHeight="20" PagerButtonHeight="22" PagerButtonWidth="41"
                                            PagerTextCssClass="GridFooterText" PageSize="25" GroupByTextCssClass="GroupByText"
                                            GroupByCssClass="GroupByCell" FooterCssClass="GridFooter" HeaderCssClass="GridHeader"
                                            SearchOnKeyPress="true" SearchTextCssClass="GridHeaderText" ShowSearchBox="true"
                                            ShowHeader="true" CssClass="Grid" RunningMode="Callback" SearchBoxPosition="TopLeft"
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="MacBaseID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
  



                                                        <ComponentArt:GridColumn DataField="MacBaseID" Visible="False" />

                                                        <ComponentArt:GridColumn DataField="HSRPStateName" HeadingText="HSRP State Name" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="RTOLocationName" HeadingText="RTOLocation Name" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />

                                                        <ComponentArt:GridColumn DataField="MachineName" HeadingText="Machine Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                       
                                                        <ComponentArt:GridColumn DataField="MobileNo" Visible="true" HeadingText="MobileNo"
                                                            Width="100" />
                                                            <ComponentArt:GridColumn DataField="ActiveStatus" Visible="true" HeadingText="ActiveStatus"
                                                            Width="100" />
                                                            <ComponentArt:GridColumn DataField="Email" Visible="true" HeadingText="Email"
                                                            Width="100" />
                                                           
                                                      
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="edit">
                                                    <a style="color: Red" onclick="javascript:editpage(## DataItem.GetMember('MacBaseID').Value ##);">
                                                        Edit</a></ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="ClientTemplate4" runat="server">
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
                                                <ComponentArt:ClientTemplate ID="ClientTemplate5" runat="server">
                                                    <table class="SliderPopup" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" style="padding: 5px;">
                                                                <table width="741" cellspacing="0" cellpadding="0" border="0">
                                                                    <tr>
                                                                        <td width="25" align="center" valign="top" style="padding-top: 3px;">
                                                                        </td>
                                                                        <td>
                                                                            <table cellspacing="0" cellpadding="2" border="0" style="width: 255px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 115px;">
                                                                                            <b>Code :</b><nobr>## DataItem.GetMember(&#39;OrgID&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 135px;">
                                                                                            <b>Name :</b><nobr>## DataItem.GetMember(&#39;OrgName&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                </tr>
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
                                        </ComponentArt:Grid>
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
</asp:Content>







