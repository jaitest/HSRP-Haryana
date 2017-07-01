<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind=" MonthlyAPAffixingReport.aspx.cs" Inherits="HSRP.Report.MonthlyAPAffixingReport" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script language="javascript" type="text/javascript">



        function validate() {


            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
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
                                        <span class="headingmain">Andhra Pradesh: Monthly Report form Affixing Stations to Registering Authority</span>
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
                                                <td>
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                    <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" />
                                                </td>
                                                <td valign="middle" style="width: 154px">
                                                    <asp:DropDownList ID="DropDownListStateName"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" 
                                                        onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" 
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                &nbsp;&nbsp;
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                    &nbsp;&nbsp;
                                                    <asp:Label Text="Month :" Visible="true" runat="server" ID="labelDate" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td valign="top" align="left" style="width: 120px">
                                                  
                                                    <asp:DropDownList ID="ddlmonth" runat="server" Height="16px" Width="101px">
                                                        <asp:ListItem Value="1">January</asp:ListItem>
                                                        <asp:ListItem Value="2">February</asp:ListItem>
                                                        <asp:ListItem Value="3">March</asp:ListItem>
                                                        <asp:ListItem Value="4">April</asp:ListItem>
                                                        <asp:ListItem Value="5">May</asp:ListItem>
                                                        <asp:ListItem Value="6">June</asp:ListItem>
                                                        <asp:ListItem Value="7">July</asp:ListItem>
                                                        <asp:ListItem Value="8">August</asp:ListItem>
                                                        <asp:ListItem Value="9">September</asp:ListItem>
                                                        <asp:ListItem Value="10">October</asp:ListItem>
                                                        <asp:ListItem Value="11">November</asp:ListItem>
                                                        <asp:ListItem Value="12">December</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>                                 
                                               <td valign="middle" align="center" class="form_text" nowrap="nowrap" style="width: 100px">
                                                    &nbsp;&nbsp;
                                                    <asp:Label Text="Year :" Visible="true" runat="server" ID="label2" />
                                                  
                                                  
                                                </td>
                                                  <td>
                                                    <asp:DropDownList ID="ddlyear" runat="server">
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
                                                    </asp:DropDownList>
                                                    </td>
                                             
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                    <asp:Button ID="btnGo" runat="server" Text="GO" ToolTip="GO"
                                                        class="button" />
                                                </td>
                                                <td valign="middle" class="form_text" nowrap="nowrap" style="width: 100px">
                                                <asp:Label ID="Label1" runat="server" CssClass="form_text" Text="Location :" 
                                                        Visible="False"></asp:Label>
                                                </td>
                                                <td valign="middle" style="width: 254px">
                                                    
                                                    <asp:DropDownList ID="ddllocation" runat="server" Height="17px" 
                                                        onselectedindexchanged="DropDownList1_SelectedIndexChanged" Width="127px" 
                                                        DataTextField="RTOLocationName" DataValueField="RTOLocationID" Visible="False">
                                                    </asp:DropDownList>
                                                </td>
                                           
                                                
                                                                                               
                                                <td style="position: relative; right: 0px; top: 0px;">
                                                    <asp:Button ID="btnExportToExcel" runat="server" Text="Report In Excel" ToolTip="Please Click for Report"
                                                        class="button"  OnClick="btnExportToExcel_Click" />
                                                </td>
                                                <tr>
                                                    <td colspan="9">
                                                        <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            
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
                    <tr>
                        <td colspan="6">
                           
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
