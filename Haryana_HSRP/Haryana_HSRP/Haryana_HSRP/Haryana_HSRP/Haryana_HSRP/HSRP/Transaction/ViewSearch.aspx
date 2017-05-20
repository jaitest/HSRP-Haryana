
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewSearch.aspx.cs" Inherits="HSRP.Transaction.ViewSearch" %>



<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">
        function editpage(i) { //Define arbitrary function to run desired DHTML Window widget codes
            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPRecords.aspx?Mode=Edit&HSRPRecordID=" + i, "Update HSRP Record Details", "width=1050px,height=585px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }

        function LaserAssignViewpage(i) { //Define arbitrary function to run desired DHTML Window widget codes
            
            googlewin = dhtmlwindow.open("googlebox", "iframe", "LaserAssignCodeView.aspx?Mode=Edit&HSRPRecordID=" + i, "View HSRP Record Details", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }



        function VoidEntryViewpage(i) { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "VoidEntry.aspx?Mode=Edit&HSRPRecordID=" + i, "View HSRP Record Details", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //window.location.href = "ViewHSRPRecords.aspx";
                return true;
            }
        }



        function PrintChalan(i, S) {
            //Define arbitrary function to run desired DHTML Window widget codes
            //            alert("Hello");
            googlewin = dhtmlwindow.open("googlebox", "iframe", "ViewPrintInvoice.aspx?Mode=PrintChalan&HSRPRecordID=" + i + "&Status=" + S + "", "Print DELIVERY CHALLAN / AP", "width=400px,height=75px,resize=1,scrolling=1,center=2", "recal")
            googlewin.onclose = function () {
               // window.location.href = "ViewSearch.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "HSRPRecords.aspx?Mode=New", "Add HSRP Record Datails", "width=1050px,height=585px,resize=1,scrolling=1,center=1", "recal")
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
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=txtSearchAll.ClientID%>").value== "") {
                alert("Please Provide Search Content");
                document.getElementById("<%=txtSearchAll.ClientID%>").focus();
                return false;
            }
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
                                        <span class="headingmain">HSRP Search </span>
                                        <%--<asp:HiddenField ID="printflag" runat="server"></asp:HiddenField>--%>
                                    </td>
                                    
                                    <td  style="color:black; font: 12px tahoma,arial,verdana;">
                                         
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">

                                            <ContentTemplate>    
                                        <asp:RadioButton ID="RadioButtonVehicleRegNoSearch" 
                                                    Text="Search by Vehicle Registration No." runat="server" Checked="true"    
                                                    GroupName="Check"  /> 
                                        <asp:RadioButton ID="radiobuttonInventorySearch" Text="Search by Inventory (Enter Complete Laser No.)"   
                                                    runat="server" GroupName="Check"  />
                                        </ContentTemplate>
                                        </asp:UpdatePanel>

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
                                    <td valign="middle" class="form_text" nowrap="nowrap" style="width: 193px">
                                        <asp:Label Text="Select HSRP State:" runat="server" ID="labelHSRPState" />
                                    </td>
                                    <td valign="middle" style="width: 153px">
                                        <asp:DropDownList AutoPostBack="true" CausesValidation="false" ID="dropDownListHSRPState"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID">
                                        </asp:DropDownList>
                                    </td>
                                    <%--<td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Location:"   runat="server" ID="labelRTOLocation" />&nbsp;&nbsp;
                                                <asp:DropDownList  ID="dropDownListRTOLocation"
                                                    CausesValidation="false" runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                               
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="dropDownListHSRPState" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListRTOLocation" />
                                                
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        
                                    </td>--%>

                                    <td style="width:400px">
                                     <asp:Label ForeColor="Black" Font-Size="Medium" Text="Enter Search Text :"   runat="server" ID="label1" />&nbsp;&nbsp;
                                                 <asp:TextBox ID="txtSearchAll" runat="server"></asp:TextBox>
                                        
                                    
                                    </td>
                                    <td style=" margin-left:-50px; width: 141px;" align="left">
                                    <div align="left" style=" width:40px;">
                                    
                                    <asp:LinkButton ID="button" runat="server" class="button" Text=" GO " OnClientClick=" return validate()" 
                                            onclick="button_Click" > </asp:LinkButton>
                                    
                                    <%--<asp:Button ID="class="button" " runat="server" Text="GO" class="button" 
                                                onclick="ButtonGo_Click" />--%> </div>
                                    </td>
                                    <td height="35" align="left" valign="middle" class="footer">
                                       <%-- <a onclick="AddNewPop(); return false;"  title="Add New HSRP Record" class="button">Add
                                            HSRP Record</a>--%>
                                    <asp:Button ID="ButImpData" runat="server" class="button"
                                        Text="Generate Data For NIC" onclick="ButImpData_Click" />
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
                                            GroupingNotificationPosition="TopRight" FillContainer="true" Height="300px" OnItemCommand="Grid1_ItemCommand">
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
                                                         <ComponentArt:GridColumn DataField="OrderDate" HeadingText="Order Date" SortedDataCellCssClass="SortedDataCell" Width="90" />
                                                        <%--<ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationNo" HeadingText="Authorization No."
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" />

                                                        <ComponentArt:GridColumn DataField="HSRPRecord_AuthorizationDate" HeadingText="Authorization Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="115" />  --%>

                                                        <%--<ComponentArt:GridColumn DataField="OrderEmbossingDate" HeadingText="Embossing Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="90" /> --%> 


                                                       <%-- <ComponentArt:GridColumn DataField="InvoiceDateTime" HeadingText="Invoice Date"
                                                            SortedDataCellCssClass="SortedDataCell" Width="70" />  --%>  
                                                        <ComponentArt:GridColumn DataField="OwnerName" HeadingText="Owner Name" SortedDataCellCssClass="SortedDataCell"
                                                            Width="130" />

                                                           <%-- <ComponentArt:GridColumn DataField="MobileNo" HeadingText="Contact No" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />--%>

                                                        <ComponentArt:GridColumn DataField="VehicleRegNo" Visible="true" HeadingText="Reg No."
                                                            Width="70" />
                                                        <ComponentArt:GridColumn DataField="ChassisNo" HeadingText="Chassis No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="110" />
                                                            <%--<ComponentArt:GridColumn DataField="EngineNo" HeadingText="Engine No" SortedDataCellCssClass="SortedDataCell"
                                                            Width="110" />--%>
                                                            <ComponentArt:GridColumn DataField="VehicleType" HeadingText="Vehicle Type" SortedDataCellCssClass="SortedDataCell"
                                                            Width="70" />
                                                            <ComponentArt:GridColumn DataField="OrderStatus" HeadingText="Status" SortedDataCellCssClass="SortedDataCell"
                                                            Width="70" />

                                                            <ComponentArt:GridColumn DataField="hsrp_front_LaserCode" HeadingText="Front Laser No." SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                            <ComponentArt:GridColumn  DataField="hsrp_rear_LaserCode" HeadingText="Rear Laser No." SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                        <%--<ComponentArt:GridColumn DataField="MobileNo" HeadingText="Contact No" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" /> --%>

                                                            <ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="ViewDetail"  HeadingText="ViewDetail" SortedDataCellCssClass="SortedDataCell" Width="60" />


                                                            <%--<ComponentArt:GridColumn DataCellCssClass="maintext" DataCellClientTemplateId="VoidEntry"  HeadingText="VoidEntry" SortedDataCellCssClass="SortedDataCell" Width="60" />--%>

                                                            <%-- <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="EpsionCashReceipt"
                                                            HeadingText="EpsonCashReceipt" SortedDataCellCssClass="SortedDataCell" Width="70" />
                                                            
                                                             <ComponentArt:GridColumn AllowGrouping="False" DataCellServerTemplateId="CashReceipt"
                                                            HeadingText="CashReceipt" SortedDataCellCssClass="SortedDataCell" Width="70" />--%>
                                                        <ComponentArt:GridColumn DataCellClientTemplateId="DeliveryChallan" HeadingText="Invoice/dc"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                    </Columns>
                                                </ComponentArt:GridLevel>
                                            </Levels>
                                            <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="CashReceipt">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonCashReceipt" 
                                                            runat="server" Text="Cash Receipt" CommandName="CashReceipt"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates> 
                                            <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="EpsionCashReceipt">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonEpsionCashReceipt" 
                                                            runat="server" Text="Epson Cash Receipt" CommandName="EpsionCashReceipt"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates> 

                                            <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="Invoice">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonInvoice" 
                                                            runat="server" Text="Invoice" CommandName="Invoice"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates> 


                                           <%-- <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="VoidEntry">
                                                    <Template>
                                                        <asp:LinkButton ID="LinkButtonVoidEntry" 
                                                            runat="server" Text="VoidEntry" CommandName="VoidEntry"></asp:LinkButton>
                                                    </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates>--%>

                                            <ClientTemplates>
                                                <ComponentArt:ClientTemplate ID="DeliveryChallan">
                                                    <a style="color: Blue" onclick="javascript:PrintChalan(## DataItem.GetMember('HSRPRecordID').Value ##,'## DataItem.GetMember('OrderStatus').Value ##');">
                                                        Invoice/DC </a></ComponentArt:ClientTemplate>

                                                       <ComponentArt:ClientTemplate ID="ViewDetail">
                                                    <a style="color: Blue" onclick="javascript:LaserAssignViewpage(## DataItem.GetMember('HSRPRecordID').Value ##);" >
                                                        ViewDetail</a>
                                                   </ComponentArt:ClientTemplate>



                                                   
                                                       <ComponentArt:ClientTemplate ID="VoidEntry">
                                                    <a style="color: Blue" onclick="javascript:VoidEntryViewpage(## DataItem.GetMember('HSRPRecordID').Value ##);" >
                                                        VoidEntry</a>
                                                   </ComponentArt:ClientTemplate>


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
</asp:Content>
