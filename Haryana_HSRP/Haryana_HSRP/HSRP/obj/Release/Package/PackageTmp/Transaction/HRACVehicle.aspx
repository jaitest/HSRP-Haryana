<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"  CodeBehind="HRACVehicle.aspx.cs" Inherits="HSRP.Transaction.HRACVehicle" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    
    
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../javascript/bootstrap.min.js" type="text/javascript"></script>
    <script src="../javascript/bootbox.min.js" type="text/javascript"></script>  
    
  
    
<%--<marquee class="mar1" direction="left">Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</marquee>--%>
    
      <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                  AC Vehicle </div>
            </legend>
          <table>
              <tr>
                   <td style="padding-left: 314px;">
                      <asp:Label ID="lblVehicle" runat="server" Text="Please Enter Vehicle No">
                      </asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td valign="middle" style="padding-left: 10px;">
                         <asp:TextBox ID="txtVehicle" runat="server" MaxLength="10"></asp:TextBox>                     
                  </td>
                  
                  <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVehicle" ValidationGroup="save" ErrorMessage="Please Enter Vehicle No."></asp:RequiredFieldValidator></td>
                
              </tr>
                                          <tr>
                  <td style="padding-left: 332px;"><asp:Label ID="lblAC" runat="server" Text="Please Enter AC NO." ></asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td valign="middle" style="padding-left: 10px;">
                   <asp:TextBox ID="txtAC" runat="server"></asp:TextBox>
                  </td>
                  <td>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAC" ValidationGroup="save" ErrorMessage="Please Enter AC NO."></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                  <td style="padding-left: 314px;">
                  <asp:Label ID="lblCounter" runat="server" Text=""></asp:Label>
                  </td><td style="padding-left: 10px;">
                      <asp:Button ID="Button3" runat="server" CssClass="button" Height="23px" 
                            onclick="Button3_Click" TabIndex="12" ValidationGroup="save" Text="Save"/></td>
                  <td>
                      <asp:LinkButton ID="lnkRefresh" runat="server" OnClick="lnkRefresh_Click">Export to Excel for validate</asp:LinkButton>

                                                                                    </td></tr>

              <tr>
                  <td></td>
                  <td>
                   <asp:Label ID="lblErrMess" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                  </td>
                  <td>
                      
                  </td>
              </tr>
              <tr>
                  <td></td>
                  <td></td>
                  <td><%--<asp:LinkButton ID="lnkSyncOnServer" runat="server" OnClick="lnkSyncOnServer_Click">Sync on Server</asp:LinkButton>--%></td>
              </tr>
          </table> 
          <div>
              <%--<asp:Label ID="lblvehicle1" runat="server" Text="" Visible="false"></asp:Label>--%>
          </div>
<%--          <table>
              <tr>
                  <td>              
                 <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns = "false" Font-Names = "Arial"  Font-Size = "11pt" AlternatingRowStyle-BackColor = "#C2D69B"  HeaderStyle-BackColor = "green" AllowPaging ="true" DataKeyNames = "SNO"  PageSize = "10" >
   <Columns>
    <asp:BoundField ItemStyle-Width = "150px" DataField = "SNO"
       HeaderText = "SNO"/>
    <asp:BoundField ItemStyle-Width = "150px" DataField = "VehicleRegNo"
       HeaderText = "VehicleRegNo"/>    
   </Columns>
   <AlternatingRowStyle BackColor="#C2D69B"  />
</asp:GridView>

                  </td>
                  <td><asp:Button ID="btnDeleteFromGridData" runat="server" Text="Delete All" Visible="false" OnClick="btnDeleteFromGridData_Click"/></td>
              </tr>
          </table>--%>
    </fieldset>
</asp:Content>
