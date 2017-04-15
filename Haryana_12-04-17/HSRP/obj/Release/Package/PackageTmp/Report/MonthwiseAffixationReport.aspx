<%@ Page Language="C#"  MasterPageFile="~/Main.Master" title="Month Wise Affixation Reprot" AutoEventWireup="true" CodeBehind="MonthwiseAffixationReport.aspx.cs" Inherits="HSRP.Report.MonthwiseAffixationReport" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                        <span class="headingmain">Monthly Affixation Report</span>
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
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr> 
                                <td></td>
                                 <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                    </td>

                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                            onselectedindexchanged="DropDownListStateName_SelectedIndexChanged"  >
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateClient" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                             <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListClient" CausesValidation="false"
                                                    runat="server" DataTextField="RTOLocationName" 
                                                    DataValueField="RTOLocationID"   >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers> 
                                                <asp:PostBackTrigger ControlID="dropDownListClient" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td style="color: #828282; font: 11pt tahoma,arial,verdana; text-decoration: none; width: 97px;";> 
                                        <asp:Label Text="Select Month:" runat="server" ID="labelDate" /> </td>
                                    <td style="width:105px";>
                                                               
                                                            <asp:DropDownList ID="ddlMonth" runat="server">
                                                                <asp:ListItem>Jan</asp:ListItem>
                                                                <asp:ListItem>Feb</asp:ListItem>
                                                                <asp:ListItem>Mar</asp:ListItem>
                                                                <asp:ListItem>Apr</asp:ListItem>
                                                                <asp:ListItem>May</asp:ListItem>
                                                                <asp:ListItem>Jun</asp:ListItem>
                                                                <asp:ListItem>Jul</asp:ListItem>
                                                                <asp:ListItem>Aug</asp:ListItem>
                                                                <asp:ListItem>Sep</asp:ListItem>
                                                                <asp:ListItem>Oct</asp:ListItem>
                                                                <asp:ListItem>Nov</asp:ListItem>
                                                                <asp:ListItem>Dec</asp:ListItem>
                                                            </asp:DropDownList>
                                                               
                                                            </td>
                                                            <td style="font-size: 10px;">
                                                                    <asp:DropDownList ID="ddlYear" runat="server">
                                                                    <asp:ListItem>2011</asp:ListItem>
                                                                    <asp:ListItem>2012</asp:ListItem>
                                                                    <asp:ListItem>2013</asp:ListItem>
                                                                    <asp:ListItem>2014</asp:ListItem>
                                                                    <asp:ListItem>2015</asp:ListItem>
                                                                    <asp:ListItem>2016</asp:ListItem>
                                                                    <asp:ListItem>2017</asp:ListItem>
                                                                    <asp:ListItem>2018</asp:ListItem>
                                                                    <asp:ListItem>2019</asp:ListItem>
                                                                    <asp:ListItem>2020</asp:ListItem>
                                                                    <asp:ListItem>2021</asp:ListItem>
                                                                    <asp:ListItem>2022</asp:ListItem>
                                                                    <asp:ListItem>2023</asp:ListItem>
                                                                    <asp:ListItem>2024</asp:ListItem>
                                                                    <asp:ListItem>2025</asp:ListItem>
                                                                    <asp:ListItem>2026</asp:ListItem>
                                                                    <asp:ListItem>2027</asp:ListItem>
                                                                    <asp:ListItem>2028</asp:ListItem>
                                                                    <asp:ListItem>2029</asp:ListItem>
                                                                    <asp:ListItem>2030</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td valign="top" style="width:44px";>
                                                               
                                                            </td>  
                                    
                                    <td style="Position:relative; right:0px; top: 0px;"> 
                                        
                                        <asp:Button ID="btnExportToExcel" runat="server" Text="Report In Excel" ToolTip="Please Click for Report"
                                            class="button" onclick="btnExportToExcel_Click"/>
                                    </td>
                                    <tr>
                                    <td colspan="9">
                                    <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                    </td>
                                    </tr>

                                   <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                </tr>
                            </table>
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
                                                    
                                                </td>
                                            </tr> 
                </table>
            </td>
        </tr>
    </table>
</asp:Content>



