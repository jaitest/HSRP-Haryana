<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UploadSMSDetails.aspx.cs" Inherits="HSRP.HR.UploadSMSDetails" %>

   <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
    <div style="margin: 20px; width: 50%; background-color: transparent; float: left; position: fixed"
        align="left">
        <fieldset>
            <legend><span class="headingmain"> Upload SMS Details </span> </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                        <tr>
                            <td class="style3"> 
                            <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" ForeColor="Black" />
                            </td>
                            &nbsp;&nbsp;
                            <td>
                            <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName" runat="server" DataTextField="HSRPStateName" DataValueField="HSRPStateName" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged1" >
                                <asp:ListItem>--Select State Type--</asp:ListItem>                              
                               <asp:ListItem>BIHAR</asp:ListItem> 
                                <asp:ListItem>HARYANA</asp:ListItem>
                                <asp:ListItem>UTTARAKHAND</asp:ListItem>
                                <asp:ListItem>HIMACHAL PRADESH</asp:ListItem>                                  
                            </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" valign="top" class="form_text" nowrap="nowrap">Select File:  </td>
                            <td style="width: 165px">
                                <asp:FileUpload ID="FileUpload1" runat="server" Style="color: black; font-size: 14px;" />
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="Btn" runat="server" class="button" Text="Upload Excel Data" OnClick="Button1_Click" Height="19px" Visible="false" />
                                  &nbsp;
                                 <asp:Button ID="btnhp" runat="server" class="button" Text="Upload Excel HP Data"  Height="19px" OnClick="btnhp_Click" Visible="false"/>
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
