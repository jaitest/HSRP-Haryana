<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewMOProductivity.aspx.cs" Inherits="HSRP.Transaction.ViewMOProductivity" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">


        function edit(i) { // Define This function of Send Assign Laser ID 
            //alert("AssignLaser" + i);
            //            var usertype = document.getElementById('username').value;
            //            alert(usertype);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "MOProductivity.aspx?Mode=Edit&ProductivityID=" + i, "Update Batch Productivity", "width=650px,height=450px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "viewMOProductivity.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "MOProductivity.aspx?Mode=New", "Add New Productivity", "width=650px,height=450px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'ViewMOProductivity.aspx';
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
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View Productivity </span>
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
                                    <td valign="left" class="form_text">
                                        &nbsp; &nbsp;
                                        </td>
                                    <td align="left" valign="middle">
                                    </td>
                                    <td valign="left" class="form_text">
                                      
                                    </td>
                                    <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New HSRP State" class="button">Add New Machine/Operator Productivity</a>
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
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                                <tr>
                                                                            <td align="center">
                                         <ComponentArt:Grid ID="Grid1" TransactionIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
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
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px" >
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="MOProductivityID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="MOProductivityID" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="ProductivityDate" HeadingText="Productivity Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                            
                                                            <ComponentArt:GridColumn DataField="PlantAddress" HeadingText="Plant Address"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                       
                                                             <ComponentArt:GridColumn DataField="mtype" HeadingText="Machine"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                       
                                                        <ComponentArt:GridColumn DataField="OperatorName" HeadingText="Operator Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        <ComponentArt:GridColumn DataField="ProductCode" Visible="true" HeadingText="Product"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="Quantity" HeadingText="Quantity" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="ScrapQuantity" HeadingText="Scrap Quantity" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        <ComponentArt:GridColumn DataField="ScrapWeight" HeadingText="Scrap Weight" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />


                                                            <ComponentArt:GridColumn DataField="Remarks" HeadingText="Remarks" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        
                                                              <ComponentArt:GridColumn DataCellClientTemplateId="edit" HeadingText="Edit" SortedDataCellCssClass="SortedDataCell"
                                                            Width="50" />
                                                        <%--<ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="DetachDevice"
                                                            HeadingText="Change Status" SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                        --%>
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                           <%-- <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="DetachDevice">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonStatus" OnClientClick="javascript:return ConfirmOnActivateUser();"
                                                            runat="server" Text="Change Status" CommandName="TransactionID"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>--%>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="edit">
                                                    <a style="color: Red" onclick="javascript:edit(## DataItem.GetMember('MOProductivityID').Value ##);">
                                                        Edit</a></ComponentArt:ClientTemplate>
                                                <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate" runat="server">
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
                                                <ComponentArt:ClientTemplate ID="SliderTemplate" runat="server">
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
                                                                                            <b>Code :</b><nobr>## DataItem.GetMember(&#39;UserID&#39;).Value ##</nobr></div>
                                                                                    </td>
                                                                                    <td>
                                                                                        <div style="overflow: hidden; width: 135px;">
                                                                                            <b>Name :</b><nobr>## DataItem.GetMember(&#39;UserLoginName&#39;).Value ##</nobr></div>
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
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
