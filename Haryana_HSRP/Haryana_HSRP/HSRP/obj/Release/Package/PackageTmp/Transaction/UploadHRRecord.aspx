<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="UploadHRRecord.aspx.cs" Inherits="HSRP.Transaction.UploadHRRecord" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">

      function vali() {

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
            <legend style="color: #000000">Upload HR Excel </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    &nbsp;&nbsp;
                    <table align="center" width="100%" style="margin-top: 7px">

                    <tr>
                    <td class="style3" style="width: 313px"> 
                                                    <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" 
                                                        ForeColor="Black" />
                                                &nbsp;&nbsp;
                                                    <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                 <td valign="middle">
                                                     &nbsp;</td>
                    </tr>

                    <tr>
                   <td valign="middle" class="Label_user_batch" style="width: 313px">
                                                   
                                                      <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                            
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient"  AssociatedControlID = "DropDownListStateName"/>&nbsp;&nbsp;
                                                            <asp:DropDownList ID="dropDownListClient" 
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
                            <td align="left" valign="top" class="Label_user_batch" nowrap="nowrap" 
                                style="width: 313px">
                                Select File:
                            </td>
                            <td>
                                <asp:FileUpload ID="FileUpload2" runat="server" />
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnuploadhrexceldata" runat="server" 
                                    onclick="btnuploadhrexceldata_Click" onclientclick=" javascript:return vali();" 
                                    Text="Upload HR Excel Data" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">Note: Please
                                    Upload Excel file of .xlsx Format and Have only one WorkSheet of Defined Format.</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" 
                                style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                          <tr>
                          <td style=" color:Black; width: 313px;">Total Upload Records :</td>
                            <td colspan="2" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="lbltotaluploadrecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                          <tr>
                           <td style=" color:Black; width: 313px;">No. of Records already exist in database:</td>
                            <td colspan="2" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;" >
                                <asp:Label ID="lbltotladuplicaterecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                     <tr>
                        <td colspan="" style=" color:Black; font-size:18px; float:right; width: 313px;" 
                             align="right">

                        Example File Format <a href="../Data/HR%20Entry%20fornat.xlsx">Download</a>
                       
                            
                        </td>
                     </tr>
                    </table>
                   
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
