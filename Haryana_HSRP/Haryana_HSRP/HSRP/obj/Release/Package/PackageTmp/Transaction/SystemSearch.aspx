
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemSearch.aspx.cs" Inherits="HSRP.Transaction.SystemSearch" %>



<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    

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
                                        <%--<asp:RadioButton ID="RadioButtonVehicleRegNoSearch" 
                                                    Text="Search by Vehicle Registration No." runat="server" Checked="true"    
                                                    GroupName="Check"  />--%> 
                                        <%--<asp:RadioButton ID="radiobuttonInventorySearch" Text="Search by Inventory (Enter Complete Laser No.)"   
                                                    runat="server" GroupName="Check"  />--%>
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
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                           >
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

                                    <td style="width:298px">
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
                                    <%--<asp:Button ID="ButImpData" runat="server" class="button"
                                        Text="Generate Data For NIC" onclick="ButImpData_Click" />--%>
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

                                    <asp:GridView ID="Grdsearch" runat="server" BackColor="White" AutoGenerateColumns="false"
                                PageSize="25" AllowPaging="false" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" >
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                                <Columns>
                                <%--<asp:TemplateField>
                                        <HeaderTemplate>
                                            Select
                                            <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" /></HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CHKSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="S.No">
                                       <ItemTemplate>
                                     <%# Container.DataItemIndex + 1 %>
                                       </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                            Order Date</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblorder" runat="server" Text='<%#Eval("OrderDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    


                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                           Authorization No</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAuthorizationno" runat="server" Text='<%#Eval("Hsrprecord_authorizationno") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    

                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Reg no</HeaderTemplate>
                                        <ItemTemplate>
                                          
                                           <asp:Label ID="lblvehicleregno" runat="server" Text='<%#Eval("Vehicleregno") %>'></asp:Label>
 
                                            <%-- <asp:TextBox ID="txtFLaserCode" runat="server"></asp:TextBox>--%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                           Embossing Date</HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:Label ID="lblembdate" runat="server" Text='<%#Eval("OrderEmbossingDate") %>'></asp:Label>
                                            
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Order Status</HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:Label ID="lblorderstatus" runat="server" Text='<%#Eval("orderstatus") %>'></asp:Label>
                                            
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                            Challanno</HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:Label ID="lblchallan" runat="server" Text='<%#Eval("challanno") %>'></asp:Label>
                                            
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                            Front Laser code </HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:Label ID="lblchallan" runat="server" Text='<%#Eval("Hsrp_front_lasercode") %>'></asp:Label>
                                            
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                           Rear Laser Code</HeaderTemplate>
                                        <ItemTemplate>
                                        <asp:Label ID="lblchallan" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>'></asp:Label>
                                            
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </asp:TemplateField>





                                </Columns>
                            </asp:GridView>
                            
                            <tr>
                            <td align="center" >
                            <asp:Label ID="lblsuccess" runat="server" Font-Names="Arial"  Font-Size="10pt" ForeColor="#FF3300" />
                            </td>
                            </tr>

                            


                                                 
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
