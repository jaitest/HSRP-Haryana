<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UploadLaserData.aspx.cs" Inherits="HSRP.Transaction.UploadLaserData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type ="text/javascript" language="javascript">
   function vali() {

       if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
           alert("Please Provide State Name.");
           document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
           return false;
       }
        
        
         if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select Client--") {
             alert("Please Provide RTO Location Name.");
             document.getElementById("<%=dropDownListClient.ClientID%>").focus();
             return false;
         }
     }
      </script>
    <div style="margin: 20px; width: 65%; background-color: transparent; float: left;
        position: fixed" align="left">
        <fieldset>
            <legend>Upload Laser Data </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" style="width: 113%">

                    <tr>
                    <td colspan="4"> <asp:Label Text="HSRP State:"  runat="server" ID="labelOrganization" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td style="width: 165px" colspan="2">
                     
                                             <asp:DropDownList AutoPostBack="true" ID="DropDownListStateName" CausesValidation="false"
                                                    runat="server" DataTextField="HSRPStateName" 
                                                    DataValueField="HSRP_StateID" 
                                                 onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" >
                                                </asp:DropDownList>
                    </td>
                    </tr>

                    <tr>
                    <td colspan="4"> <asp:Label Text="RTO Location:"  runat="server" ID="labelClient" ForeColor="Black" />&nbsp;&nbsp;</td>
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
                            <td align="left" valign="top" class="form_text" nowrap="nowrap" colspan="4">
                                Select File:
                            </td>
                            <td align="left" valign="top" class="form_text" nowrap="nowrap" colspan="1">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>       
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                                
                                    </ContentTemplate>
                                    <Triggers>
                                               <asp:PostBackTrigger ControlID="btnupload" />
                                    </Triggers>
                                </asp:UpdatePanel></td>
                            <td align="left">
                                    
                              <asp:Button ID="btnupload" runat="server" class="button" 
                                            OnClick="btnupload_Click" OnClientClick=" javascript:return vali();" 
                                            Text="Upload" />

                                          
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">Note: Please
                                    Upload Excel file of .xlsx Format and Have only one WorkSheet of Defined Format.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="5">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   1.     File must be in .xls Format.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="5">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   2.     All the records should be in the given format.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="5">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                  3.     There should not be any blank record in the file at end of the file too.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="5">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                4.     There should be only one sheet in the file.</span>
                            </td>
                        </tr>
                        <tr>
                         <td colspan="5">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                               5.     Date should be in MM/DD/YYYY format.</span>
                            </td>
                        </tr>
                        
                         <tr>
                         <td colspan="5">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
6. There should not be any special character .(Eg. single quote ‘ )</span>
                            </td>
                        </tr>
                         
                        <tr>
                            <td colspan="5" align="left">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left" 
                                style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                &nbsp;</td>
                        </tr>
                          <tr>
                          <td style=" color:Black;" colspan="4">Total Upload Records :</td>
                            <td align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="lbltotaluploadrecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                          <tr>
                           <td style=" color:Black;" colspan="4">No. of Records already exist in database:</td>
                            <td align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;" >
                                <asp:Label ID="lbltotladuplicaterecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left" 
                                style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                          
                        <tr>
                            <td colspan="5" align="center" 
                                style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" >
                                   <ProgressTemplate>
                                   <asp:Image ID="Image1" runat="server" Height="82px" ImageAlign="Middle" ImageUrl="~/img/loading_gif.gif" Width="155px" />
                                   </ProgressTemplate>
                                   
                                   </asp:UpdateProgress>                                              
                                   
                            </td>
                        </tr>
                          
                     <tr>
                        <td style=" color:Black; font-size:18px; float:right" align="right">

                        Example File Format <a href="../Data/Laser%20Data%20Format.xlsx">Download</a>
                                                
                        </td>
                        <td style=" color:Black; font-size:18px; float:right" align="right">

                            &nbsp;</td>
                        <td style=" color:Black; font-size:18px; float:right" align="right">

                            &nbsp;</td>
                        <td style=" color:Black; font-size:18px; float:right" align="right">

                            &nbsp;</td>
                     </tr>
                    </table>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
