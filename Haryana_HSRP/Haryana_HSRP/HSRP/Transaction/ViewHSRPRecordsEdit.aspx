<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewHSRPRecordsEdit.aspx.cs" Inherits="HSRP.Transaction.ViewHSRPRecordsEdit" %>



<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">
        function editpage(i) { //Define arbitrary function to run desired DHTML Window widget codes
            //  alert(i);
            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPRecords.aspx?Mode=Edit&HSRPRecordID=" + i, "Update HSRP Record Details", "width=1050px,height=585px,resize=1,scrolling=1,center=1", "recal")
            
            googlewin.onclose = function () {
                window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }

        function PrintChalan(i, S) {
            //Define arbitrary function to run desired DHTML Window widget codes
            //            alert("Hello");
            googlewin = dhtmlwindow.open("googlebox", "iframe", "PrintChalan.aspx?Mode=PrintChalan&HSRPRecordID=" + i + "&Status=" + S + "", "Print Delivery Challan", "width=400px,height=75px,resize=1,scrolling=1,center=2", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPRecordsEdit.aspx?Mode=Edit", "Add HSRP Record Datails", "width=1050px,height=585px,resize=1,scrolling=1,center=1", "recal")
            //            googlewin.onclose = function () {
            //                window.location = 'ViewHSRPRecords.aspx';
            //                return true;
            //            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function ConfirmPDF() {
            //if (confirm("Are you really want to Download Delivery Challan?")) {
            document.getElementById("ctl00_ContentPlaceHolder1_printflag").value = "1";
            return true;
            //            }
            //            else {
            //                return false;
            //            }

        }
        //ConfirmCashReceipt()

        function ConfirmCashReceipt() {
            //if (confirm("Are you really want to Print Cash Receipt?")) {
            document.getElementById("ctl00_ContentPlaceHolder1_printflag").value = "3";
            return true;
            //            }
            //            else {
            //                return false;
            //            }

        }

        function ConfirmInvoice() {
            //if (confirm("Are you really want to Print Invoice?")) {
            document.getElementById("ctl00_ContentPlaceHolder1_printflag").value = "2";
            return true;
            //            }
            //            else {
            //                return false;
            //            }

        }

    </script>
    <table id="Table1" runat="server"  width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">HSRP Records </span>
                                        <%--<asp:HiddenField ID="printflag" runat="server"></asp:HiddenField>--%>
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
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Select HSRP State:" runat="server" ID="labelHSRPState" />
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" CausesValidation="false" ID="dropDownListHSRPState"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="dropDownListHSRPState_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Location:"  runat="server" ID="labelRTOLocation" />&nbsp;&nbsp;
                                                <asp:DropDownList  ID="dropDownListRTOLocation"
                                                    CausesValidation="false" runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                               
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="dropDownListHSRPState" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListRTOLocation" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        
                                    </td>
                                    <td style=" margin-left:-50px">
                                    <div align="right" style=" width:40px;"><asp:Button ID="ButtonGo" runat="server" Text="GO" class="button" 
                                                onclick="ButtonGo_Click" /> </div>
                                    </td>
                                    <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;"  title="Add New HSRP Record" class="button">Edit
                                            HSRP Record</a>
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
                                        <ComponentArt:Grid ID="Grid1" ClientIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
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
                                                <ComponentArt:GridLevel DataKeyField="HSRPRecordID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                        <ComponentArt:GridColumn DataField="OrderStatus" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="HSRPRecordID" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationNo" HeadingText="Authorization No."
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                                <ComponentArt:GridColumn DataField="InvoiceDateTime" HeadingText="InvoiceDate Time" FormatString="yyyy-MM-dd HH:mm:ss"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                        <ComponentArt:GridColumn DataField="OwnerName" HeadingText="Owner Name" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="VehicleRegNo" Visible="true" HeadingText="Reg No."
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="ChassisNo" HeadingText="Chassis No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                            <ComponentArt:GridColumn DataField="EngineNo" HeadingText="Engine No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                            <ComponentArt:GridColumn DataField="VehicleType" HeadingText="Vehicle Type" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" />
                                                        <ComponentArt:GridColumn DataField="MobileNo" HeadingText="Contact No" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                            <ComponentArt:GridColumn DataField="NetAmount" HeadingText="Net Amount" SortedDataCellCssClass="SortedDataCell"
                                                            Width="90" FormatString="#.#,0" />
                                                        <%--<ComponentArt:GridColumn DataCellClientTemplateId="GenerateChallan" HeadingText="Generate Chalnan"
                                                            SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                        <ComponentArt:GridColumn DataCellClientTemplateId="GenerateInvoice" HeadingText="Generate Invoice"
                                                            SortedDataCellCssClass="SortedDataCell" Width="50" />
                                                        <ComponentArt:GridColumn DataCellClientTemplateId="GenerateInvoiceGenerateChallan"
                                                            HeadingText="Generate Invoice & Generate Challan" SortedDataCellCssClass="SortedDataCell"
                                                            Width="50" />--%>
                                                        <%--<ComponentArt:GridColumn DataCellClientTemplateId="edit" HeadingText="Edit" SortedDataCellCssClass="SortedDataCell"
                                                            Width="50" />--%>
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            
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
