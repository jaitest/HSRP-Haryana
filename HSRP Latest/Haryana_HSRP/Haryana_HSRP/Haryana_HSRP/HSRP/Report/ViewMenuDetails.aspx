<%@ Page Title="View Menu Details" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewMenuDetails.aspx.cs" Inherits="HSRP.Report.ViewMenuDetails" %>
<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">
       
        function editpage(i) { //Define arbitrary function to run desired DHTML Window widget codes
          
            googlewin = dhtmlwindow.open("googlebox", "iframe", "menutreeview.aspx?Mode=Edit&UserID=" + i, "menu tree view", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
               
                return true;
            }
        }      
        
    </script>
    <script type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListMenuName.ClientID%>").value == "--Select Menu Name--") {
                alert("Select Menu Name");
                document.getElementById("<%=DropDownListMenuName.ClientID%>").focus();
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
                                        <span class="headingmain">View Menu Details</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                                    <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" 
                                            ForeColor="Black" />
                                                </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="true" 
                                                    CausesValidation="false" DataTextField="HSRPStateName" 
                                                    DataValueField="HSRP_StateID" 
                                                    OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged" 
                                                    Visible="true">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                                    <asp:Label ID="lblmenuname" runat="server" ForeColor="Black" 
                                            Text="Menu Name"></asp:Label>
                                                </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="DropDownListMenuName" runat="server" AutoPostBack="True" 
                                                    DataTextField="menuname" DataValueField="menuid" 
                                                    onselectedindexchanged="DropDownListMenuName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                    <td>
                                <asp:Button ID="btngo" runat="server" 
                                     onclientclick=" javascript:return validate();" 
                                    Text="Go" onclick="btngo_Click" />
                                    </td>
                                </tr>
                            </table>
                    </td>
                    </tr>
                <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                            </table>
                        </td>
                    </tr>
                 <tr>
                        <td colspan="6"> 
                        
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate> 
        <ComponentArt:Grid ID="Grid1" ClientIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
                                            Width="100%" 
                                            LoadingPanelPosition="MiddleCenter" LoadingPanelClientTemplateId="LoadingFeedbackTemplate"
                                            GroupBySortImageHeight="10" GroupBySortImageWidth="10" GroupBySortDescendingImageUrl="group_desc.gif"
                                            GroupBySortAscendingImageUrl="group_asc.gif" GroupingNotificationTextCssClass="GridHeaderText"
                                            IndentCellWidth="22" TreeLineImageHeight="19" TreeLineImageWidth="22" TreeLineImagesFolderUrl="~/images/lines/"
                                            PagerImagesFolderUrl="~/images/pager/" PreExpandOnGroup="true" GroupingPageSize="5"
                                            SliderPopupClientTemplateId="SliderTemplate" SliderPopupOffsetX="20" SliderGripWidth="9"
                                            SliderWidth="150" SliderHeight="20" PagerButtonHeight="22" PagerButtonWidth="41"
                                            PagerTextCssClass="GridFooterText" PageSize="15" GroupByTextCssClass="GroupByText"
                                            GroupByCssClass="GroupByCell" FooterCssClass="GridFooter" HeaderCssClass="GridHeader"
                                            SearchOnKeyPress="true" SearchTextCssClass="GridHeaderText" ShowSearchBox="true"
                                            ShowHeader="true" CssClass="Grid" SearchBoxPosition="TopLeft"
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px" >
                                            <Levels>
                                                <ComponentArt:GridLevel DataKeyField="UserID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="UserID" Visible="False" />                                                        
                                                        <ComponentArt:GridColumn DataField="UserLoginName" HeadingText="User Login Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                            <ComponentArt:GridColumn DataField="rtolocationname" HeadingText="RTO Location Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        <ComponentArt:GridColumn DataField="ActiveStatus" HeadingText="Status" Width="100" />
                                                        <ComponentArt:GridColumn DataCellClientTemplateId="menu" HeadingText="View Users Menu" SortedDataCellCssClass="SortedDataCell"
                                                            Width="50" />                                                        
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="menu">
                                                    <a style="color: Blue" onclick="javascript:editpage(## DataItem.GetMember('UserID').Value ##);">
                                                        Show Menu Of This User</a></ComponentArt:ClientTemplate>
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
                                                       
                                                    </table>
                                                </ComponentArt:ClientTemplate>
                                            </ClientTemplates>
                                        </ComponentArt:Grid>
                                        </ContentTemplate>
                                                                           
                                 </asp:UpdatePanel>
              
                   
                            <br />
                        </td>
                    </tr>
                    <tr>
                                                                <td colspan="2" class="maintext">
                                                                    <div style="width: 100%; height: 500px; overflow: auto;">
                                                                    
                                                                        
                                                                    </div>
                                                                </td>
                                                            </tr>
                </table>
            </td>
        </tr>
    </table>
  

</asp:Content>
