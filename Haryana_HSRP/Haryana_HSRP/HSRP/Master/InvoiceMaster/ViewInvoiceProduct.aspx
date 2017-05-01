<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewInvoiceProduct.aspx.cs" Inherits="HSRP.Master.InvoiceMaster.ViewInvoiceProduct" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />
    <script src="../../windowfiles/dhtmlwindow.js" type="text/javascript"></script>

<script type="text/javascript">
    function edit(i) { // Define This function of Send Assign Laser ID 

        googlewin = dhtmlwindow.open("googlebox", "iframe", "InvoiceProduct.aspx?Mode=Edit&ProductID=" + i, "Update Invoice Product", "width=950px,height=300px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
            window.location.href = "ViewInvoiceProduct.aspx";
            return true;
        }
    }

    function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

        googlewin = dhtmlwindow.open("googlebox", "iframe", "InvoiceProduct.aspx?Mode=New", "Add New Invoice Product", "width=950px,height=300px,resize=1,scrolling=1,center=1", "recal")
        googlewin.onclose = function () {
            window.location = 'ViewInvoiceProduct.aspx';
            return true;
        }
    }
    </script>
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) {

                return true;
            }
            else {
                return false;
            }

        }
    </script>
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View Invoice Product </span>
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
                                    <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New HSRP State" class="button">Add New Product</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       
                                        <ComponentArt:Grid ID="Grid1" ProductIDMode="ProductID" runat="server" ImagesBaseUrl="~/images" 
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
                                            ShowHeader="true" CssClass="Grid" RunningMode="Client" SearchBoxPosition="TopLeft"
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px">
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="ProductID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                       <ComponentArt:GridColumn DataField="ProductID" Visible="False" />
                                                       
                                                        <ComponentArt:GridColumn DataField="ProductName" HeadingText="Product Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="130" />
                                                       <ComponentArt:GridColumn DataField="ProductCode" Visible="true" HeadingText="Product Code"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="ProductColor" Visible="true" HeadingText="Product Color"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="ProductCost" HeadingText="Product Cost" SortedDataCellCssClass="SortedDataCell"
                                                            Width="60" />
                                                        <ComponentArt:GridColumn DataField="P_MeasurementUnit" HeadingText="Measurement Unit" SortedDataCellCssClass="SortedDataCell"
                                                            Width="45" /> 
                                                     <%--   <ComponentArt:GridColumn DataField="TinNo" HeadingText="TinNo" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> 
                                                        <ComponentArt:GridColumn DataField="VatNo" HeadingText="VatNo" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> 
                                                        <ComponentArt:GridColumn DataField="CST" HeadingText="CST" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> 
                                                              <ComponentArt:GridColumn DataField="ExciseNo" HeadingText="ExciseNo" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" /> --%>
                                                        <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="edit" HeadingText="Edit" SortedDataCellCssClass="SortedDataCell" Width="50" /> 

                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientTemplates>
                                                 
                                                   <ComponentArt:ClientTemplate ID="edit">
                                                    <a style="color: Red" onclick="javascript:edit(## DataItem.GetMember('ProductID').Value ##);">
                                                        Edit</a>
                                                   </ComponentArt:ClientTemplate>  

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
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
