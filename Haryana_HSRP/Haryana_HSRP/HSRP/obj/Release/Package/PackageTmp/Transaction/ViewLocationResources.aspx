<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewLocationResources.aspx.cs" Inherits="HSRP.Transaction.ViewLocationResources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link href="../css1/style.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
   
    <script type="text/javascript">
        function editpage(i) { //Define arbitrary function to run desired DHTML Window widget codes
            //  alert(i);
            googlewin = dhtmlwindow.open("googlebox", "iframe", "Embossing.aspx?Mode=Edit&UserID=" + i, "Update User Details", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewEmbossing.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "Embossing.aspx?Mode=New", "Add New User", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'ViewEmbossing.aspx';
                return true;
            }
        }
    </script>

    

    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change user status?")) {

                return true;
            }
            else {
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
                                <tr>
                                    <td>
                                        <span class="headingmain">Location Resources </span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                                    Font-Names="Arial" Font-Size="11pt" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    AllowPaging="True" ShowFooter="True" OnPageIndexChanging="OnPaging" OnRowEditing="EditResources"
                                    OnRowUpdating="UpdateResources" OnRowCancelingEdit="CancelEdit">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Resources ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResID" runat="server" Text='<%# Eval("ResID")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblResID" runat="server" Text='<%# Eval("ResID")%>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblResID" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="30px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="Resources Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResourcesType" runat="server" Text='<%# Eval("ResourcesType")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DropDownList1" runat="server">
                                                    <asp:ListItem>System</asp:ListItem>
                                                    <asp:ListItem>Printer</asp:ListItem>
                                                    <asp:ListItem>UPS</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DropDownList1" runat="server">
                                                    <asp:ListItem>System</asp:ListItem>
                                                    <asp:ListItem>Printer</asp:ListItem>
                                                    <asp:ListItem>UPS</asp:ListItem>
                                                </asp:DropDownList>
                                            </FooterTemplate>
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblquantity" runat="server" Text='<%# Eval("quantity")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtquantity" runat="server" Text='<%# Eval("quantity")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtquantity" runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Resources Detail">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResourcesDetail" runat="server" Text='<%# Eval("ResourcesDetail")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtResourcesDetail" runat="server" Text='<%# Eval("ResourcesDetail")%>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtResourcesDetail" runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Created By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("UserFirstName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <%-- <EditItemTemplate>
                <asp:TextBox ID="txtCreatedBy" runat="server" Text='<%# Eval("CreatedBy")%>'></asp:TextBox>
            </EditItemTemplate>  --%>
                                            <%-- <FooterTemplate>
                <asp:TextBox ID="txtCreatedBy" runat="server"></asp:TextBox>
            </FooterTemplate> --%>
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Created Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate")%>'></asp:Label>
                                            </ItemTemplate>
                                           
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Updated Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompany" runat="server" Text='<%# Eval("UpdatedDate")%>'></asp:Label>
                                            </ItemTemplate>
                                           
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:TemplateField>
                                      
                                        
                                        <asp:CommandField ShowEditButton="True" HeaderText="Edit/Update/Cancle"  HeaderStyle-Width="120px"  />
                                    
                                      <asp:TemplateField  ItemStyle-Width="30px" HeaderText="Add">
                                           <%-- <ItemTemplate>
                                                <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument='<%# Eval("ResID")%>'
                                                    OnClientClick="return confirm('Do you want to delete?')" Text="Delete" OnClick="DeleteResources"></asp:LinkButton>
                                            </ItemTemplate>--%>
                                            <FooterTemplate>
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pgr"></PagerStyle>
                                </asp:GridView>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
