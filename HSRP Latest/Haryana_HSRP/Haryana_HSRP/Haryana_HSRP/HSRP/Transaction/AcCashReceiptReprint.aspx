<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AcCashReceiptReprint.aspx.cs" Inherits="HSRP.Transaction.AcCashReceiptReprint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <script language="javascript" type="text/javascript">
         function validate() {

             if (document.getElementById("<%=TextBox1.ClientID%>").value == "") {
                alert("Please Enter Vehicle Registration No ");
                document.getElementById("<%=TextBox1.ClientID%>").focus();
                return false;
            }

           

        }

    </script>
 <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
     <tr>

         <td>

              <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"  class="topheader">

                                <tr>
                                  
                                     <td valign="middle" class="form_text" colspan="2" nowrap="nowrap">
                                         </td>
                                    <td>
                                     

                                    </td>
                                 
                                    <td valign="middle" class="form_text" nowrap="nowrap"></td>                                      


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
                                        <span class="headingmain">Ac Cash Receipt Reprint</span>
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
                                    <td  class="form_text" >
                                        Vechicle Registration No                                        
                                    </td>
                                    <td align="left" valign="middle">
                                        <asp:TextBox ID="TextBox1"  Height="80px" Width="180px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td valign="left" class="form_text">
                                        <asp:Button ID="Button1" OnClientClick=" return validate()"  runat="server" Text="Update" OnClick="Button1_Click" />
                                    </td>
                                    



                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>

                                 <tr>
                                    <td>
                                        <asp:Label ID="Lblsucess" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Blue" />
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
