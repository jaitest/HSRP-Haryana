<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ProductionSheetStatusReport.aspx.cs" Inherits="HSRP.Report.ProductionSheetStatusReport" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
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
    function validate() {

        if (document.getElementById("<%=ddlStateName.ClientID%>").value == "--Select State--") {
            alert("Select State");
            document.getElementById("<%=ddlStateName.ClientID%>").focus();
            return false;
        }


    }
    </script>
    
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr>
        <td colspan="8"> <span class="headingmain">Govt.Received Data Summary :</span></td>
      </tr>
      <tr> 
        <td nowrap="nowrap" style="width: 126px"> <asp:Label ID="lblState" runat="server" Text="State Name : " ForeColor="Black" Font-Bold="True"></asp:Label></td>
        <td style="width: 275px"><asp:DropDownList ID="ddlStateName" runat="server" 
                    DataTextField="HSRPStateName" DataValueField="Hsrp_StateId" Width="130px">
                </asp:DropDownList>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="ddlStateName" Display="Dynamic" 
                    ErrorMessage="Please Select State" InitialValue="--Select State--"></asp:RequiredFieldValidator></td>
       
        <td><asp:Button ID="btnstatus" runat="server" CssClass="button" 
                    onclick="btnstatus_Click" Text="Status" /></td>
      </tr>
      <tr>
        <td style="width: 126px"> <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label></td>
        <td style="width: 275px">&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
      </tr>
    </table>
</asp:Content>
