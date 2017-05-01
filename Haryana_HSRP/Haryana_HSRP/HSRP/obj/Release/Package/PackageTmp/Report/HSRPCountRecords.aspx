<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HSRPCountRecords.aspx.cs" Inherits="HSRP.Report.HSRPCountRecords" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <table>
    <tr>
    <td style=" color:Black">State Name</td>
    <td>
      <asp:DropDownList Visible="true" CausesValidation="false" ID="DropDownListStateName" AutoPostBack="true"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                                        onselectedindexchanged="DropDownListStateName_SelectedIndexChanged1" >
                                                    </asp:DropDownList> 
  
                                 
    </td>
    </tr>
    </table>
    
                                       
                                               
       <ComponentArt:Grid ID="Grid10" runat="server" Width="100%" 
                        BorderColor="#99FF99" SearchOnKeyPress="true" ShowSearchBox="true"
                                            SearchBoxPosition="TopLeft">
                                            <Levels>
                                                <ComponentArt:GridLevel  DataKeyField="RTOLocation" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading" 
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                            <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="RTOLocation" Visible="False" />
                                            <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="RTOLocation" HeadingText="Location"
                                                SortedDataCellCssClass="SortedDataCell" IsSearchable="True" />
                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="ORDER_BOOKED" HeadingText=" Order Booked"
                                                SortedDataCellCssClass="SortedDataCell"  Align="Right" />
                                                 <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="EMBOSSED" HeadingText="Embossed Vehicle"
                                                SortedDataCellCssClass="SortedDataCell"  Align="Right" />
                                                 <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="CLOSED" HeadingText="Closed Record"
                                                SortedDataCellCssClass="SortedDataCell"  Align="Right" />
                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="TOTAL" HeadingText="Total"
                                                SortedDataCellCssClass="SortedDataCell"   Align="Right"/>
                                                  
                                            
                                        </Columns>
                                                </ComponentArt:GridLevel>

                                            </Levels>
                                           
                                            <ClientTemplates>
                                                
                                                <ComponentArt:ClientTemplate ID="LoadingFeedbackTemplate" runat="server"><br /><br />
                                                    
                                                    <table cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                        </tr>
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
                                                                            Page <b>## DataItem.PageIndex + 1 ##</b> of <b>## Grid10.PageCount ##</b>
                                                                        </td>
                                                                        <td align="right">
                                                                            Record <b>## DataItem.Index + 1 ##</b> of <b>## Grid10.RecordCount ##</b>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
  
                    </ComponentArt:Grid>
                                  
</asp:Content>
