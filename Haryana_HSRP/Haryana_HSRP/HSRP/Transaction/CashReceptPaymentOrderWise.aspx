<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="CashReceptPaymentOrderWise.aspx.cs" Inherits="HSRP.Transaction.CashReceptPaymentOrderWise" %>

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
                  Download online Cash Collection Receipt
                </div>
            </legend>
          <div style="align-content:center;padding-left: 500px;">
          <table style="align-content:center">
        <tr>                      
            <td>
                <asp:Label Text="Search Type:" runat="server"   Visible="true"   ForeColor="Black" Font-Bold="True" />                                           
            </td>
            <td>                
               <asp:DropDownList ID="Ddl1" runat="server" Height="25px" Width="148px" AutoPostBack="true" OnSelectedIndexChanged="Ddl1_SelectedIndexChanged" >
               </asp:DropDownList>                        
            </td>  
            </tr>
              <tr>
                  <td></td>
                  <td></td>
              </tr>
              <tr>        
            <td>
                <asp:Label  runat="server"   Text="Enter Search Type:"  Visible="false" ID="lblsearch"   ForeColor="Black" Font-Bold="True" />
            </td>
              <td>
                   <asp:TextBox ID="TxtSearchtype" Visible="false"  runat="server" ForeColor="Black" Font-Bold="True" ></asp:TextBox> 
                  <br />
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtSearchtype" ErrorMessage="Enter Value" ValidationGroup="reciept"></asp:RequiredFieldValidator>
            </td>
            
            </tr>
              <tr>
                  <td>
                      <asp:Label ID="lblErrMess" runat="server" ForeColor="Red"></asp:Label></td>
                  <td>
              <asp:Button ID="BtnSearch" Visible="false" CssClass="button" Height="23px"  runat="server" Text="DownLoad Receipt" ValidationGroup="reciept" OnClick="BtnSearch_Click" />
            </td>
              </tr>

        </table>
          </div>
          <%--<table>              
              <tr>
                  <td style="padding-left: 314px;">
                      <asp:Label runat="server" ID="lblUserName" Text="Enter Payment Order Id.">
                      </asp:Label>
                      <span style="color:red">*</span>
                  </td>
                  <td style="padding-left: 10px;">
                      <asp:TextBox ID="txtVehicleregno" runat="server"></asp:TextBox>                       

                  </td>
                  <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtVehicleregno" ErrorMessage="Please Enter Vehicle Reg. No." ValidationGroup="save"></asp:RequiredFieldValidator></td>
              </tr>
              <tr>
                  <td></td>
                  <td style="padding-left: 10px;">
                  <asp:Button ID="Button3" runat="server" CssClass="button" Height="23px" onclick="Button3_Click" TabIndex="12" ValidationGroup="save" Text="DownLoad Receipt"/>
                  </td>
                  <td>
                      <asp:LinkButton ID="LinkButton1" runat="server" ValidationGroup="save" Visible="false" OnClick="LinkButton1_Click">View Detail</asp:LinkButton>

                 </td>
              </tr>
              <tr>
                  <td></td>
                  <td></td>
                  <td>
                  <asp:Label ID="lblErrMess" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
              </tr>
          </table>--%>          
    </fieldset>
</asp:Content>
