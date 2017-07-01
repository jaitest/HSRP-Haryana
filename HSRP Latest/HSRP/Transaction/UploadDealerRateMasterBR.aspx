<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UploadDealerRateMasterBR.aspx.cs" Inherits="HSRP.Transaction.UploadDealerRateMasterBR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div style="margin: 20px; width: 65%; background-color: transparent; float: left;
        position: fixed" align="left" >
        <fieldset>
            <legend >Upload Bihar Dealer Rate Master </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                    <tr>
                    <td> <asp:Label Text="HSRP State:"  runat="server" ID="labelOrganization" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td style="width: 165px">
                     
                                             <asp:DropDownList ID="DropDownListStateName" CausesValidation="false"
                                                    runat="server" DataTextField="HSRPStateName" 
                                                    DataValueField="HSRP_StateID" >
                                                </asp:DropDownList>
                    </td>
                    </tr>

                 

                        <tr>
                            <td align="left" valign="top" class="form_text" nowrap="nowrap">
                                Select File:
                            </td>
                            <td style="width: 165px">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" class="button" Text="Upload Excel Data" OnClick="Button1_Click" />
                                
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
                      
                          <tr>
                          <td style=" color:Black;">Total Upload Records :</td>
                            <td colspan="3" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="lbltotaluploadrecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                          <tr>
                           <td style=" color:Black;">No. of Records Already Exist:</td>
                            <td colspan="3" align="left"    style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="lbltotladuplicaterecords" runat="server" ForeColor="Black" ></asp:Label>
                              
                            </td>
                        </tr>
                    <tr>
                        <td colspan="3" style=" color:Black; font-size:18px;">
                            Example File Format <a href="../Data/DealerRateMasterBihar.xls">Download</a>
                       
                        </td>
                        </tr>
                       
                    </table>
                </div>
            </div> 
        </fieldset>
    </div>
</asp:Content>

