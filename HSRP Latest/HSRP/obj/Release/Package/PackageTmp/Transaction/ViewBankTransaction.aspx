<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewBankTransaction.aspx.cs" Inherits="HSRP.Transaction.ViewBankTransaction" %>


<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript">


        function edit(i) { // Define This function of Send Assign Laser ID 
            //alert("AssignLaser" + i);
            //            var usertype = document.getElementById('username').value;
            //            alert(usertype);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransaction.aspx?Mode=Edit&TransactionID=" + i, "Update BankTransaction.aspx", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
               window.location.href = "ViewBankTransaction.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "BankTransaction.aspx?Mode=New", "Add New Bank Transaction", "width=700px,height=480px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
               window.location = 'ViewBankTransaction.aspx';
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

         <td>

              <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"
                                class="topheader">
                                <tr>
                                   <%-- <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap" style="visibility: hidden;"> 
                                        <asp:Label Text="Location:" runat="server" ID="labelClient"  Visible="true" />
                                    </td>
                                    <td valign="middle" class="form_text" style="visibility: hidden;">
                                        <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional" >
                                            <ContentTemplate>
                                                <asp:DropDownList Visible="true" ID="dropDownListClient" CausesValidation="false"
                                                    Width="140px" runat="server" DataTextField="RTOLocationName" AutoPostBack="false"
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>--%>
                                     <td valign="middle" class="form_text" colspan="2" nowrap="nowrap">
                                         </td>
                                    <td>
                                       <%-- <asp:DropDownList Visible="true" ID="ddlBothDealerHHT" CausesValidation="false" Width="140px"
                                            runat="server">
                                            <asp:ListItem Text="Both">Both</asp:ListItem>
                                            <asp:ListItem Text="Dealer Data">Dealer Data</asp:ListItem>
                                            <asp:ListItem Text="Other">Other</asp:ListItem>
                                        </asp:DropDownList>--%>

                                       <%-- <asp:DropDownList Visible="false" ID="ddlBothDealerHHT"  CausesValidation="false" Width="140px"
                                         DataTextField="dealername" DataValueField="dealerid"  
                                            AutoPostBack="false" runat="server">  
                                        </asp:DropDownList>
--%>

                                        

                                    </td>
                                  <%--  <td valign="middle" class="form_text" colspan="2" nowrap="nowrap">
                                        <asp:Label Text="Vehicle Type :" runat="server" ID="labelvehicletype" Visible="true" />
                                    </td>
                                    <td>
                                        <asp:DropDownList Visible="true" ID="DropDownList1" CausesValidation="false" Width="140px"
                                            runat="server" >
                                            <asp:ListItem Text="All">All</asp:ListItem>
                                            <asp:ListItem Text="Two Wheeler">Two Wheeler</asp:ListItem>
                                            <asp:ListItem Text="Three Wheeler">Three Wheeler</asp:ListItem>
                                            <asp:ListItem Text="Four Wheeler">Four Wheeler</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>--%>
                                    <%--<td valign="middle" visible="false" class="form_text" nowrap="nowrap">
                                        &nbsp;&nbsp;
                                        <asp:Label Text="Date:" runat="server" ID="labelDate" />
                                    </td>
                                    <td valign="middle" onmouseup="OrderDate_OnMouseUp()" align="left">
                                        <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>
                                    <td valign="middle" align="left">
                                        <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                            onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        &nbsp;&nbsp;<asp:Label Text="To:" runat="server" ID="labelTO" Visible="False" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle" onmouseup="HSRPAuthDate_OnMouseUp()" align="left">
                                        <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                            ControlType="Picker" PickerCssClass="picker" Visible="False">
                                            <ClientEvents>
                                                <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                            </ClientEvents>
                                        </ComponentArt:Calendar>
                                    </td>--%>
                                   <%-- <td valign="middle" align="left" visible="false">
                                       
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="Order Status" Visible="false" runat="server" ID="labelOrderStatus" />&nbsp;&nbsp;
                                    </td>
                                    <td valign="middle"  class="form_text" nowrap="nowrap">
                                        <asp:DropDownList AutoPostBack="false" DataTextField="pdffilename" DataValueField="pdffilename"
                                            Visible="false" ID="dropdownDuplicateFIle" CausesValidation="false" runat="server">
                                        </asp:DropDownList>
                                    </td>--%>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                       <%-- <asp:Button ID="btnGO" Width="58px" runat="server" Visible="false" Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClick="btnGO_Click1"  />
                                   --%>     
                                        </td>
                                      <%--  <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnDownloadRecords" Width="114px" runat="server" Text="Download In PDF"
                                            ToolTip="Please Click for Report" class="button" OnClientClick="validate()" OnClick="btnDownloadRecords_Click" />
                                            <br />
                                        
                                    </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnAllpdf" Width="114px" runat="server" Text="Download All PDF"
                                            ToolTip="Please Click for Report" class="button" 
                                            onclick="btnAllpdf_Click"  />
                                            <br />
                                        
                                    </td>

                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnrejected" Width="153px" runat="server" Text="Download Reassign Orders "
                                            ToolTip="Please Click for Report" class="button" OnClick="btnrejected_Click"  />
                                            <br />
                                        
                                    </td>--%>


                                </tr>
                            </table>
         </td>

     </tr>
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">View Bank Transaction</span>
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
                                    <td valign="left" class="form_text" >
                                       
                                    </td>
                                    <td align="left" valign="middle">
                                        
                                    </td>
                                    <td valign="left" class="form_text">
                                        
                                    </td>
                                    <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Bank Transaction" class="button">Add New Bank Transaction</a>
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
                                    <td>


                                     <asp:GridView ID="Grid1" runat="server" OnRowDataBound="Grid1_RowDataBound" AllowPaging="true" PageSize="15" CellPadding="4" ForeColor="Black" GridLines="Vertical"  AutoGenerateColumns="false" OnPageIndexChanging="Grid1_PageIndexChanging" ShowHeader="true" Width="100%" BackColor="White" BorderColor="#FFCC99" BorderStyle="Solid"
                                            BorderWidth="1px" OnSelectedIndexChanged="Grid1_SelectedIndexChanged">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>


                                                <asp:BoundField HeaderText="TransactionID" DataField="TransactionID" />
                                                <asp:BoundField HeaderText="Deposit Date" DataField="Deposit Date" />
                                                <asp:BoundField HeaderText="BankName" DataField="BankName" />
                                                <asp:BoundField HeaderText="BranchName" DataField="BranchName" />
                                                <asp:BoundField HeaderText="DepositAmount" DataField="DepositAmount" />
                                                <asp:BoundField HeaderText="DepositBy" DataField="DepositBy" />
                                                <asp:BoundField HeaderText="StateID" DataField="StateID" />
                                                <asp:BoundField HeaderText="RTOLocation" DataField="RTOLocation" />
                                                <asp:BoundField HeaderText="UserID" DataField="UserID" />
                                                <asp:BoundField HeaderText="CurrentDate" DataField="CurrentDate" />
                                                <asp:BoundField HeaderText="BankSlipNo" DataField="BankSlipNo" />
                                                <%--<asp:BoundField HeaderText="Remarks" DataField="Remarks" /> --%> 
                                                <%-- <asp:BoundField HeaderText="AccountNo" DataField="AccountNo" />--%>

                                                

                                            </Columns>

                                            <FooterStyle BackColor="#CCCC99" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <RowStyle BackColor="#F7F7DE" />
                                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                            <SortedAscendingHeaderStyle BackColor="#848384" />
                                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                            <SortedDescendingHeaderStyle BackColor="#575357" />
                                        </asp:GridView>


                                         

                                    <%--  <ComponentArt:Grid ID="Grid1" RTOLocationIDMode="AutoID" runat="server" ImagesBaseUrl="~/images"
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
                                                <ComponentArt:GridLevel DataKeyField="TransactionID" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                    DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                    HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                    HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading"
                                                    SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                    SortImageHeight="19">
                                                    <Columns>
                                                         <ComponentArt:GridColumn DataField="TransactionID" Visible="False" />
                                                        <ComponentArt:GridColumn DataField="Deposit Date" HeadingText="Deposit Date" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                             <ComponentArt:GridColumn DataField="BankSlipNo" HeadingText="Bank Slip No"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                       
                                                        <ComponentArt:GridColumn DataField="BankName" HeadingText="Bank Name"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        <ComponentArt:GridColumn DataField="BranchName" Visible="true" HeadingText="Branch Name"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="DepositAmount" HeadingText="Deposit Amount" SortedDataCellCssClass="SortedDataCell"
                                                            Width="100" />
                                                        <ComponentArt:GridColumn DataField="DepositBy" HeadingText="Deposit By" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                            <ComponentArt:GridColumn DataField="DepositLocation" HeadingText="Deposit Location" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                            <ComponentArt:GridColumn DataCellClientTemplateId="DeliveryChallan" HeadingText="Status"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />

                                                       <ComponentArt:GridColumn DataField="StateID" HeadingText="State ID" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />


                                                            <ComponentArt:GridColumn DataField="RTOLocation" HeadingText="RTOLocation" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="100" />
                                                        <ComponentArt:GridColumn DataField="UserID" HeadingText="User ID" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                            <ComponentArt:GridColumn DataField="CurrentDate" HeadingText="Current Date" AllowGrouping="False"
                                                            SortedDataCellCssClass="SortedDataCell" Width="80" />
                                                      
                                                        

                                      <ServerTemplates>
                                                <ComponentArt:GridServerTemplate ID="Invoice">
                                                    <Template>
                                            <asp:LinkButton runat="server">LinkButton</asp:LinkButton>
                                                   </Template>
                                                </ComponentArt:GridServerTemplate>
                                            </ServerTemplates> 
                                            <ClientTemplates>
                                               

                                              


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
                                        </ComponentArt:Grid> --%>
                                        
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
