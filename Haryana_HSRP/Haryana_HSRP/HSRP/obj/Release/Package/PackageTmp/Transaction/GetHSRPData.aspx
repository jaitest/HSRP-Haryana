<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="GetHSRPData.aspx.cs" Inherits="HSRP.Transaction.GetHSRPData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div style="margin: 20px; width: 65%; background-color: transparent; float: left;
        position: fixed" align="left" >
        <fieldset>
            <legend >Get HSRP Data </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                        <tr>
                            <td align="left" valign="top" class="form_text" nowrap="nowrap">
                                Select File:
                            </td>
                            <td style="width: 165px">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" class="button" Text="Upload Excel Data" OnClick="Button1_Click"   OnClientClick=" javascript:return vali();"/>
                                 &nbsp;
                               
                            
                             </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">Note: Please
                                    Upload data in .xlsx or .xls Format and should have only one WorkSheet of Defined Format.</span>
                            </td>
                             
                             
                            
                            <td align="center" rowspan="9" valign="middle">
                                
                            </td>
                           
                        </tr>
                        <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   1.     File must be in .xls or .xls Format.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   2.     All the records should be in the given format.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                  3.     There should not be any blank record in the file at end of the file too.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                4.     There should be only one sheet in the file.</span>
                            </td>
                        </tr>
                        
                        
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
5. There should not be any special character .(Eg. single quote ‘ )</span>
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
                               
                            </td>
                        </tr>
                      
                          <tr>
                         <%-- <td style=" color:Black;">Total Upload Records :</td>--%>
                            <td colspan="3" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="lbltotaluploadrecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                       <%-- <td colspan="" style=" color:Black; font-size:18px; float:right" align="right">
                            Example File Format <a href="../Data/HSRPDataformat.xlsx">Download</a>
 
                        </td>--%>
                        </tr>
                    </table>
                </div>
            </div> 
        </fieldset>
    </div>
</asp:Content>
