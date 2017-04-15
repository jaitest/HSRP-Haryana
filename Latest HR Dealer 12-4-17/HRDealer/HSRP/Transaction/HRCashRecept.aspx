<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="HRCashRecept.aspx.cs" Inherits="HSRP.Transaction.HRCashRecept" %>

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
                  Download AC Cash Collection Receipt</div>
            </legend>
          <table>
              <tr>
                   <td style="padding-left: 314px;"><asp:Label ID="lblState" runat="server" Text="Select State:">
                      </asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td valign="middle" style="padding-left: 10px;">
                         <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">                        
                         </asp:DropDownList>                      
                  </td>
                  <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlState" InitialValue="0" ErrorMessage="Please Select State"></asp:RequiredFieldValidator></td>
                
              </tr>
              <tr>
                  <td style="padding-left: 314px;"><asp:Label runat="server" ID="lblUserName" Text="Select User Name">
                      </asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td style="padding-left: 10px;"> <asp:DropDownList runat="server" AutoPostBack="true" Visible="true" CausesValidation="false" ID="ddlUserName" >
                      </asp:DropDownList></td>
                  <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlUserName" InitialValue="0" ErrorMessage="Please Select UserName"></asp:RequiredFieldValidator></td>
              </tr>
              <tr><td></td><td style="padding-left: 10px;"><asp:Button ID="Button3" runat="server" CssClass="button" Height="23px" 
                            onclick="Button3_Click" TabIndex="12" Text="Epson Print"/></td><td></td></tr>
              <tr><td></td><td></td><td>
                  <asp:Label ID="lblErrMess" runat="server" Text="" Visible="false"></asp:Label>
                                    </td></tr>
          </table> 
    </fieldset>
</asp:Content>
