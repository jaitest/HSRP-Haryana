<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ApTgReport.aspx.cs" Inherits="HSRP.BusinessReports.ApTgReport" %>
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

      if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--")
         {
            alert("Select State");
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
                                        <span class="headingmain"> Ap/TG Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            </table>

   
    <table style="height: 51px;" width="50%">
        <tr>
            <td style="height: 40px;" width="20">
                </td>
            <td style="height: 30px;" valign="middle" width="80">
                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1" 
                                            ForeColor="Black" Font-Bold="True" Width="100px" />
            </td>
            <td style="width: 50px; height: 40px;" valign="middle">               
                   
                     <asp:DropDownList ID="DropDownListStateName" runat="server" Height="22px" Width="150px"  AutoPostBack="True" ></asp:DropDownList>
                                   
                
                               
            </td>                   <td style="height: 20px" width="20">
               <asp:Button ID="btnexport" runat="server"   OnClientClick="return validate()" onclick="btnexport_Click" Text="Export"   Font-Bold="True" ForeColor="#3333FF"/>
                                                </td>

                                                
        </tr>

        <tr>
           <td>
            <asp:Label ID="Lblerror" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        </td>
        </tr>
                                            
                                            
       </table>
    
                                            
</asp:Content>


