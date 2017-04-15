<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UploadTgOnlinePaymentRecods.aspx.cs" Inherits="HSRP.Transaction.UploadTgOnlinePaymentRecods" %>

   <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
    <div style="margin: 20px; width: 50%; background-color: transparent; float: left; position: fixed"
        align="left">
        <fieldset>
            <legend><span class="headingmain"> Upload TG Online Payment </span> </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                       

                        <tr>
                            <td align="left" valign="top" class="form_text" nowrap="nowrap">Select File:  </td>
                            <td style="width: 165px">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                &nbsp;&nbsp;
                            </td>



                            <td>
                                <asp:Button ID="Btn" runat="server" class="button" Text="Upload Excel Data" OnClick="Button1_Click" Height="19px" />
                               

                            </td>
                            <td></td>
                        </tr>

                        <tr>
                             <td style="color: Black;"></td>
                <td colspan="3" align="left"
                    style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="lbltotaluploadrecords" runat="server" Visible="false" ForeColor="Black"></asp:Label>
                </td>

                        </tr>
   <tr>
                <td style="color: Black;"></td>
                <td colspan="3" align="left"
                    style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="lbltotladuplicaterecords" runat="server" Visible="false" ForeColor="Black"></asp:Label>
                  
                </td>
            </tr>

            <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left" style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    
                </td>
            </tr>

                    </table>
                </div>
            </div>
        </fieldset>

    </div>
    
      
    
    <div style="margin: 20px;" align="left">
        <table align="left">
          <%--  <tr>
                <td colspan="2">
                    <span style="color: black; font: verdana arial 12px;">Mandentory Validations:</span>
                </td>

            </tr>
            
         --%>


            <tr>
               <%-- <td style="color: Black;"></td>
                <td colspan="3" align="left"
                    style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="lbltotaluploadrecords" runat="server" Visible="false" ForeColor="Black"></asp:Label>
                </td>--%>
            </tr>
           <%-- <tr>
                <td style="color: Black;"></td>
                <td colspan="3" align="left"
                    style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="lbltotladuplicaterecords" runat="server" Visible="false" ForeColor="Black"></asp:Label>
                  
                </td>
            </tr>

            <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left" style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    
                </td>
            </tr>--%>
        </table>
    </div>
</asp:Content>
