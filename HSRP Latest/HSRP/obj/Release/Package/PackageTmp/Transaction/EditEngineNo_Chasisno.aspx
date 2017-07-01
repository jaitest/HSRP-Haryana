<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EditEngineNo_Chasisno.aspx.cs" Inherits="HSRP.Transaction.EditEngineNo_Chasisno" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
   
 
    <script language="javascript" type="text/javascript">
        function validate()
        {
            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State.");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtserchtext.ClientID%>").value == "") {
                alert("Please Search Text.");
                document.getElementById("<%=txtserchtext.ClientID%>").focus();
                return false;
            }
        }

    </script>
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr id="TR1" runat="server">
                                    <td>
                                        <asp:Label ID="Label4" class="headingmain" runat="server">Edit Engine No/Chassis No </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>

                                </tr>
                                <tr>
                                    
                                   <td valign="middle" class="form_text" nowrap="nowrap">
                                       <asp:RadioButton ID="RadioBtnchassisnosearch" 
                                                    Text="Search by Chassis No." runat="server"    Checked="true"   
                                                    GroupName="Check"  />
                                       <asp:RadioButton ID="RadioButtonEngineNoSearch" 
                                                    Text="Search by Engine No." runat="server"   
                                                    GroupName="Check"  />
                                                 
                                       </td>
                                </tr>
                               
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0px solid" align="center" cellpadding="0" cellspacing="0"
                                class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text=" Search Text:" Visible="true" runat="server" ID="labelClient" />
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:TextBox ID="txtserchtext" runat="server" Width="204px"></asp:TextBox>

                                        <%--<asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:DropDownList Visible="true" ID="dropDownRtoLocation" CausesValidation="false"
                                                    Width="140px" runat="server" DataTextField="userloginname" AutoPostBack="false"
                                                    DataValueField="userid" OnSelectedIndexChanged="dropDownRtoLocation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownRtoLocation" />
                                            </Triggers>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                    
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Button ID="btnGO" Width="58px" runat="server" Text="GO" ToolTip="Please Click for Report"
                                            class="button" OnClientClick=" return validate()" OnClick="btnGO_Click" />
                                        &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                                    </td>
                                   
                                  
                                    
                                </tr>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                
                                    <td align="left">
                                        <asp:Label ID="LblMessage" Font-Names="Arial" Font-Size="10pt"  runat="server" Text=""></asp:Label>
                                        &nbsp;&nbsp;
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="padding: 10px">

                  <asp:GridView ID="gvhsrpedit" runat="server" Width="100%" AutoGenerateColumns="false" ShowFooter="true" ForeColor="Black" Font-Bold="True" BorderColor="#FF9966" BorderStyle="Groove" BorderWidth="1px" onrowupdating="gvhsrpedit_RowUpdating" onrowcancelingedit="gvhsrpedit_RowCancelingEdit"  onrowediting="gvhsrpedit_RowEditing">
                <Columns> 

          
                    <asp:TemplateField HeaderText="HSRP Record ID" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblhsrprecordid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "hsrprecordid") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:Label ID="lblEdithsrprecordid" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "hsrprecordid") %>'></asp:Label>           
                        </EditItemTemplate>
                      
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="OrderDate">
                          <ItemTemplate>
                               <asp:Label ID="lblOrderDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OrderDate") %>'></asp:Label>
                          </ItemTemplate>
                          </asp:TemplateField>

                     <%--  <asp:TemplateField HeaderText="OwnerName">
                          <ItemTemplate>
                               <asp:Label ID="lblOwnerName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OwnerName") %>'></asp:Label>
                          </ItemTemplate>
                          </asp:TemplateField>--%>
                      <asp:TemplateField HeaderText="VehicleType">
                          <ItemTemplate>
                               <asp:Label ID="lblVehicleType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "VehicleType") %>'></asp:Label>
                          </ItemTemplate>
                          </asp:TemplateField>

                     <asp:TemplateField HeaderText="vehicleclass">
                          <ItemTemplate>
                               <asp:Label ID="lblvehicleclass" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "vehicleclass") %>'></asp:Label>
                          </ItemTemplate>
                          </asp:TemplateField>

                      <asp:TemplateField HeaderText="OrderStatus">
                          <ItemTemplate>
                               <asp:Label ID="lblOrderStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OrderStatus") %>'></asp:Label>
                          </ItemTemplate>
                          </asp:TemplateField>

                     <asp:TemplateField HeaderText="NetAmount">
                          <ItemTemplate>
                               <asp:Label ID="lblNetAmount" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "NetAmount") %>'></asp:Label>
                          </ItemTemplate>
                          </asp:TemplateField>

                

                      <asp:TemplateField HeaderText="VehicleReg No">
                        <ItemTemplate>
                         <asp:Label ID="lblVehicleRegNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "VehicleRegNo") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                         <asp:TextBox ID="txtVehicleRegNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "VehicleRegNo") %>'></asp:TextBox>           
                        </EditItemTemplate>
                        </asp:TemplateField>

                    
 
                     <asp:TemplateField HeaderText="Owner Name">
                        <ItemTemplate>
                           <asp:Label ID="lblOwnerName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OwnerName") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtOwnerName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "OwnerName") %>'></asp:TextBox>           
                        </EditItemTemplate>
                           </asp:TemplateField>
          
                    
                    
                     <asp:TemplateField HeaderText="ChassisNo">
                        <ItemTemplate>
                            <asp:Label ID="lblChassisNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ChassisNo") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtChassisNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ChassisNo") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="EngineNo">
                        <ItemTemplate>
                            <asp:Label ID="lblEngineNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EngineNo") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>           
                            <asp:TextBox ID="txtEngineNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EngineNo") %>'></asp:TextBox>           
                        </EditItemTemplate>
                       
                    </asp:TemplateField>
 
              
 
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                       
                            <asp:Button ID="ButtonEdit" runat="server" CommandName="Edit"  Text="Edit"  />
                            <%--<asp:Button ID="ButtonDelete" runat="server" CommandName="Delete"  Text="Delete"  />--%>
                           
                        </ItemTemplate>
                        <EditItemTemplate>
                         
                             <asp:Button ID="ButtonUpdate" runat="server" CommandName="Update"  Text="Update"  />
                              <asp:Button ID="ButtonCancel" runat="server" CommandName="Cancel"  Text="Cancel" />
                        </EditItemTemplate>
                       
                    </asp:TemplateField>                    
                </Columns>        
      <FooterStyle BackColor="#CCCC99" />
                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                            <RowStyle BackColor="#F7F7DE" />
                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#FBFBF2" />
                            <SortedAscendingHeaderStyle BackColor="#848384" />
                            <SortedDescendingCellStyle BackColor="#EAEAD3" />
                            <SortedDescendingHeaderStyle BackColor="#575357" />   
            </asp:GridView>
                                    </td>
                                </tr>
                               
                               
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>

