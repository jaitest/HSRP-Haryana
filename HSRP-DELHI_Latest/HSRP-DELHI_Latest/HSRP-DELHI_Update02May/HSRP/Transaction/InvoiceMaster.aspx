<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InvoiceMaster.aspx.cs" Inherits="HSRP.Transaction.InvoiceMaster" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size:medium;color:Black">
                    HSRP Invoice No</div>
            </legend>
            <br />
            <center>
            <table>
                <tr>
                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" /> &nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                     <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <td valign="middle" class="form_text" nowrap="nowrap">
                                    <asp:Label Text="Location:" Visible="true" runat="server" ID="labelClient" /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                     <td valign="middle" align="center">
                                    
                                             <asp:DropDownList Visible="true" ID="dropDownListClient" CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="RTOLocationName" AutoPostBack="false"
                                                    DataValueField="RTOLocationID">
                                                </asp:DropDownList>    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                      
                                    </td>
                    <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text=" Invoice No:" Visible="true" runat="server" ID="lblinvoiceno" /> &nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>--%>
                   <%-- <td class ="lable_style">
                        Invoice No :</td>--%>
                   <%-- <td>
                        <asp:TextBox ID="textboxBoxHSRPState" runat="server"  class="form_textbox" ></asp:TextBox>
                    </td>--%>
                    <td>
                        <asp:Button ID="buttonUpdateinvoice" runat="server" Text="Generate New Transfer" TabIndex="1"  class="button" OnClick="buttonUpdateinvoice_Click"  />

                    </td>
                </tr>
                <%--<tr class ="lable_style">
                    <td >
                        Active Status:
                    </td>
                    <td>
                        <asp:CheckBox ID="checkBoxActiveStatus" TextAlign="Left" runat="server" 
                             TabIndex="1" />
                    </td>
                </tr>--%>
                <tr align="right" style="margin-right:10px">
                <td>
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                    <td colspan="2">
                        <br />
                      <%--  <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()" 
                                class="button" onclick="buttonUpdate_Click" />&nbsp;&nbsp;
                        <asp:Button ID="ButtonSave" runat="server" Text="Save" TabIndex="2" OnClientClick=" return validate()" 
                            class="button" onclick="ButtonSave_Click1" />&nbsp;&nbsp;
                             <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                             <input type="reset" class="button" value="Reset" />--%>
                    </td>
                    
                </tr>
                
                
              
               <%-- <tr>
                    <td>
                        <asp:GridView ID="grdinvoice" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" Width="100%">

                            <Columns>

                            </Columns>
                        </asp:GridView>                  

                    </td>
                </tr>--%>
            </table>
                </center>
            <center>
                    <div>
                        <asp:Label ID="lblinvoicemsg" runat="server"></asp:Label>
                    </div>
                 </center>
        </fieldset>
    </div>
</asp:Content>
