<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HSRPLaserReport.aspx.cs" Inherits="HSRP.Transaction.HSRPLaserReport" %>
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

    <script language="javascript" type="text/javascript">



        function validate() {

            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select Location--") {
                alert("Select Location");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListPlateSize.ClientID%>").value == "--Select Plate Size--") {
                alert("Select Plate Size");
                document.getElementById("<%=DropDownListPlateSize.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListPrefix.ClientID%>").value == "--Select Prefix--") {
                alert("Select Prefix");
                document.getElementById("<%=DropDownListPrefix.ClientID%>").focus();
                return false;
            }


        }

      
           
         
         
    </script>

    <table>
    <tr>
    <td>
        <table style="width: 100%" id="tblelaser" runat="server">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 124px">
                                                    <asp:Label ID="lblprefix" runat="server" Text="Prefix" ForeColor="Black" 
                                                        Font-Bold="True"></asp:Label>
                                                 </td>
                            <td style="width: 29px">
                                &nbsp;</td>
                            <td style="width: 164px">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DropDownListPrefix" runat="server" AutoPostBack="True" 
                                            DataTextField="Prefix" DataValueField="prefix" 
                                            onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 28px">
                                &nbsp;</td>
                            <td style="width: 45px">
                                &nbsp;</td>
                            <td style="width: 4px">
                                &nbsp;</td>
                            <td style="width: 147px">
                                &nbsp;</td>
                            <td style="width: 34px">
                                &nbsp;</td>
                            <td style="width: 377px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 124px">
                                &nbsp;</td>
                            <td style="width: 29px">
                                &nbsp;</td>
                            <td style="width: 164px">
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                         ControlToValidate="DropDownListPrefix" ErrorMessage="Please enter Prefix" 
                                                         SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                     </td>
                            <td style="width: 28px">
                                &nbsp;</td>
                            <td style="width: 45px">
                                &nbsp;</td>
                            <td style="width: 4px">
                                &nbsp;</td>
                            <td style="width: 147px">
                                &nbsp;</td>
                            <td style="width: 34px">
                                &nbsp;</td>
                            <td style="width: 377px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 124px">
                                                    <asp:Label ID="lblfrom" runat="server" Text="Laser No From" ForeColor="Black" 
                                                        Font-Bold="True"></asp:Label>
                                                 </td>
                            <td style="width: 29px">
                                &nbsp;</td>
                            <td style="width: 164px">
                                                     <asp:TextBox ID="txtfrom" runat="server" ontextchanged="txtfrom_TextChanged" 
                                                         MaxLength="10"></asp:TextBox>
                                                     </td>
                            <td style="width: 28px">
                                &nbsp;</td>
                            <td style="width: 45px">
                                                    <asp:Label ID="lblto" runat="server" Text="TO" ForeColor="Black" 
                                                        Font-Bold="True"></asp:Label>
                                                 </td>
                            <td style="width: 4px">
                                &nbsp;</td>
                            <td style="width: 147px">
                                                    <asp:TextBox ID="txtto" runat="server" MaxLength="10"
                                                    ontextchanged="txtto_TextChanged"></asp:TextBox>
                                                 
                                                    </td>
                            <td style="width: 34px">
                                                 
                                                    <asp:Button ID="btngo" runat="server" onclick="btngo_Click" Text="GO" ForeColor="#0000CC" 
                                                                    Height="24px" Width="60px" />
                                                                </td>
                            <td style="width: 377px">
                                                 
                                                    &nbsp;</td>
                        </tr>
                        <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                                         ControlToValidate="txtfrom" ErrorMessage=" Only Enter Numeric Value" 
                                                         ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                                        ControlToValidate="txtto" ErrorMessage=" Only Enter Numeric Value" 
                                                        ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                                 
                                </td>
                                <td>
                                </td>
                                <td>
                                                 
                                                    <asp:Label ID="lblerror" runat="server" Text="lblerror" Visible="False" 
                                                         ForeColor="#CC0000"></asp:Label>
                                </td>
                        </tr>
                        <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                         ControlToValidate="txtfrom" ErrorMessage="Please enter Laser No From" 
                                                         SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                        ControlToValidate="txtto" ErrorMessage="Please Enter To" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%" id="tblecount" runat="server" visible="false">
                        <tr>
                            <td style="width: 117px; height: 18px">
                                                    <asp:Label ID="lblplatecount" runat="server" 
                                    Text="Plate Count" ForeColor="Black" Font-Bold="True"></asp:Label>
                                                 </td>
                            <td style="width: 30px; height: 18px">
                                </td>
                            <td style="width: 163px; height: 18px">
                                <asp:Label ID="lblpcount" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                            <td style="width: 26px; height: 18px">
                                </td>
                            <td style="width: 5px; height: 18px;">
                                </td>
                            <td style="width: 71px; height: 18px">
                                                    <asp:Label ID="lblusedplate" runat="server" 
                                    Text="Used Plate" ForeColor="Black" Font-Bold="True"></asp:Label>
                                                 </td>
                            <td style="height: 18px; width: 4px">
                                </td>
                            <td style="height: 18px; width: 507px;">
                                <asp:Label ID="lblusdplate" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 117px">
                                &nbsp;</td>
                            <td style="width: 30px">
                                &nbsp;</td>
                            <td style="width: 163px">
                                &nbsp;</td>
                            <td style="width: 26px">
                                &nbsp;</td>
                            <td style="width: 5px">
                                &nbsp;</td>
                            <td style="width: 71px">
                                &nbsp;</td>
                            <td style="width: 4px">
                                &nbsp;</td>
                            <td style="width: 507px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 117px">
                                <asp:Label ID="lblpsize" runat="server" ForeColor="Black" Text="Plate Size" 
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td style="width: 30px">
                                &nbsp;</td>
                            <td style="width: 163px">
                                <asp:Label ID="lblps" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                            <td style="width: 26px">
                                &nbsp;</td>
                            <td style="width: 5px">
                                &nbsp;</td>
                            <td style="width: 71px">
                                <asp:Label ID="lblcolor" runat="server" ForeColor="Black" Text="Color" 
                                    Font-Bold="True"></asp:Label>
                            </td>
                            <td style="width: 4px">
                                &nbsp;</td>
                            <td style="width: 507px">
                                <asp:Label ID="lblc" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 18px">
                    <table style="width: 100%" id="tbleupdate" runat="server" visible="false">
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 180px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 56px">
                                                    <asp:Button ID="btnupdate" runat="server" Text="Update" 
                                                        onclick="btnupdate_Click" ForeColor="#0000CC" 
                                                        Height="28px" />
                                                    </td>
                            <td style="width: 114px">
                                                    &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%" id="tblegrid" runat="server" visible="false">
                        <tr>
                            <td>
                                                    <ComponentArt:DataGrid ID="DataGrid1" runat="server" Width="500">

                                                        <Levels>
                                                                <ComponentArt:GridLevel  DataKeyField="HSRPStateName" RowCssClass="Row" ColumnReorderIndicatorImageUrl="reorder.gif"
                                                                DataCellCssClass="DataCell" HeadingCellCssClass="HeadingCell" HeadingCellHoverCssClass="HeadingCellHover"
                                                                HeadingCellActiveCssClass="HeadingCellActive" HeadingRowCssClass="HeadingRow"
                                                                HeadingTextCssClass="HeadingCellText" SelectedRowCssClass="SelectedRow" GroupHeadingCssClass="GroupHeading" 
                                                                SortAscendingImageUrl="asc.gif" SortDescendingImageUrl="desc.gif" SortImageWidth="10"
                                                                SortImageHeight="19">

                                                        <Columns>
                                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="HSRPStateName" Visible="False" />
                                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="HSRPStateName" HeadingText="State"
                                                                SortedDataCellCssClass="SortedDataCell" IsSearchable="True" />
                                                                <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="RtolocationName" HeadingText=" location"
                                                                SortedDataCellCssClass="SortedDataCell"  Align="Right" />
                                                                 <ComponentArt:GridColumn DataCellCssClass="maintext" DataField="Laser No Count" HeadingText="Laser No Count"
                                                                SortedDataCellCssClass="SortedDataCell"  Align="Right" />
                                                                
                                                  
                                                                                                    
                                                     </Columns>
                                                     </ComponentArt:GridLevel>
                                                    

                                            </Levels>

                                                   </ComponentArt:DataGrid>
                                                    </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%" id="tbledropstate" runat="server" visible="false">
                        <tr>
                            <td style="width: 115px; height: 18px">
                                                                                                                           
                                                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" Font-Bold="True" 
                                                                 ForeColor="Black" />
                                                         </td>
                            <td style="height: 18px; width: 34px">
                                </td>
                            <td style="height: 18px; width: 163px">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="true" 
                                            CausesValidation="false" DataTextField="HSRPStateName" 
                                            DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="height: 18px; width: 38px">
                                </td>
                            <td style="height: 18px; width: 75px">
                                <asp:Label ID="labelClient" runat="server" Text="Location:" Font-Bold="True" 
                                                                         ForeColor="Black" />
                                                                         
                                                         </td>
                            <td style="height: 18px; width: 11px">
                                </td>
                            <td style="height: 18px; width: 150px">
                                                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                         
                                                                          <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                            
                                                        </Triggers>
                                                                         <ContentTemplate>
                                                                             <asp:DropDownList ID="dropDownListClient" 
                                    runat="server" AutoPostBack="True" 
                                                                         
    CausesValidation="false" DataTextField="RTOLocationName" 
                                                                         DataValueField="RTOLocationID" 
                                                                         onselectedindexchanged="dropDownListClient_SelectedIndexChanged" 
                                                                         
    Height="16px" Width="136px">
                                                                             </asp:DropDownList>
                                                                         </ContentTemplate>
                                                                     </asp:UpdatePanel>
                                                                 </td>
                            <td style="height: 18px; width: 32px">
                                </td>
                            <td style="height: 18px; width: 76px">
                                                                    
                                                                   
                                                                    
                                                                    <asp:Label ID="lblplatesize" runat="server" Text="Plate Size:" 
                                                                        ForeColor="Black" Font-Bold="True"></asp:Label>
                                                                    
                                                                   
                                                                    
                                                         </td>
                            <td style="height: 18px; width: 16px">
                                </td>
                            <td style="height: 18px">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                   
                                    <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="dropDownListClient" EventName="SelectedIndexChanged" />
                                                            
                                                        </Triggers>
                                    <ContentTemplate>
                                        <asp:DropDownList ID="DropDownListPlateSize" runat="server" AutoPostBack="True" 
                                            DataTextField="ProductCode" DataValueField="ProductID" Height="16px" 
                                            onselectedindexchanged="DropDownListPlateSize_SelectedIndexChanged" 
                                            Width="156px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%" id="tblesubmit" runat="server" visible="false">
                        <tr>
                            <td>
                                                                    &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 4px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 847px">
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="btnsubmit" 
                                                                        runat="server" Text="Submit" 
                                                                        onclick="btnsubmit_Click" ForeColor="#0000CC" Height="26px" />
                                                         </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 18px">
                                                                    <asp:Label ID="lblsuccess" 
                        runat="server" ForeColor="#CC0000" 
                                                                        Visible="False"></asp:Label>
                            &nbsp;<asp:Label ID="lblerror1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
                            </td>
            </tr>
        </table>
        </td>
        </tr>
        
    </table>
</asp:Content>
