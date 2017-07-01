 <%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UploadAgentData.aspx.cs" Inherits="HSRP.Transaction.UploadAgentData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    ad<script type="text/javascript" language="javascript">

     function vali() {

         if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select Client--") {
             alert("Please Provide RTO Location Name.");
             document.getElementById("<%=dropDownListClient.ClientID%>").focus();
             return false;
         }
     }
      </script><div style="margin: 20px; width: 65%; background-color: transparent; float: left;
        position: fixed" align="left">
        <fieldset>
            <legend>Upload Agent Data </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                    <tr>
                    <td> <asp:Label Text="HSRP State:"  runat="server" ID="labelOrganization" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td style="width: 165px">
                     
                                             <asp:DropDownList AutoPostBack="true" ID="DropDownListStateName" CausesValidation="false"
                                                    runat="server" DataTextField="HSRPStateName" 
                                                    DataValueField="HSRP_StateID" 
                                                 onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" >
                                                </asp:DropDownList>
                    </td>
                    </tr>

                    <tr>
                    <td> <asp:Label Text="RTO Location:"  runat="server" ID="labelClient" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td valign="middle" class="Label_user_batch" style="width: 165px">
                                                   
                                                      <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                            
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            &nbsp;<asp:DropDownList ID="dropDownListClient" 
                                                                CausesValidation="false"  AutoPostBack="true"
                                                                runat="server" DataTextField="RTOLocationName" 
                                                                DataValueField="RTOLocationID" 
                                                                onselectedindexchanged="dropDownListClient_SelectedIndexChanged" 
                                                                EnableTheming="False">
                                                            </asp:DropDownList>
                                                         </ContentTemplate>
                                                       </asp:UpdatePanel>
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
                            <td style="width: 73px">
                                <asp:Button ID="btnupload" runat="server" class="button" Text="Upload" 
                                    OnClick="btnupload_Click"   OnClientClick=" javascript:return vali();" 
                                    Width="64px"/>
                                    <asp:Button ID="btnsyn" runat="server" class="button" Text="Sync With Server" 
                                    onclick="btnsyn_Click" Visible="False" 
                                   />
                            </td>
                            <td style="width: 6px">
                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">Note: Please
                                    Upload Excel file of .xlsx Format and Have only one WorkSheet of Defined Format.</span>
                            </td>
                           
                            <td colspan="2" align="center" rowspan="9" valign="middle">
                                
                            </td>
                           
                        </tr>
                        <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   1.     File should be in .xlsx Format.</span>
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
                               5.     Date should be in MM/DD/YYYY format.</span>
                            </td>
                        </tr>
                        
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
6. There should not be any special character .(Eg. single quote ‘ )</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
7. Price Column should contain only number. (Eg.1000).</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
8.     After uploading SYNC ON SERVER is compulsory.</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                
                                
                                <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                                                <br />
                                <asp:Label ID="Label1" runat="server" ForeColor="Black"></asp:Label>
                                <asp:Label ID="vehicleno1" runat="server" ForeColor="Black" Visible="False"></asp:Label>
                                <br />
                                <asp:Label ID="Label2" runat="server" ForeColor="Black"></asp:Label>
                                <br />
                                <asp:Label ID="Label3" runat="server" ForeColor="Black"></asp:Label>
                                <br />
                                <asp:Label ID="Label4" runat="server" ForeColor="Black"></asp:Label>
                             

                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left" style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                          <tr>
                          <td style=" color:Black;">Total Upload Records :</td>
                            <td colspan="3" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="lbltotaluploadrecords" runat="server" ForeColor="Black"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                          <tr>
                           <td style=" color:Black;">No. of Records already exist in database:</td>
                            <td colspan="3" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;" >
                                <asp:Label ID="lbltotladuplicaterecords" runat="server" ForeColor="Black"></asp:Label>
                                 <asp:Label ID="ShowDuplicateRecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                            
                        </tr>
                    <%--     <tr>
                           <td style=" color:Black;"> Duplicate Records in Excel File :</td>
                            <td colspan="4" align="left" style="font-family: Verdana,tahoma, arial; font-size: medium;" >
                                <asp:Label ID="lbltotlaexcel" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>--%>
                    </table>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
