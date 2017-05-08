<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DispatchFromWareHouse.aspx.cs" Inherits="HSRP.Transaction.DispatchFromWhereHouse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        function validation() {

            if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State Name");
                document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select RTO Name--") {
                alert("Please Select RTO Location Name");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=ddlSize.ClientID%>").value == "--Select Product Size--") {
                alert("Please Select Product Size");
                document.getElementById("<%=ddlSize.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=ddlPrifix.ClientID%>").value == "--Select Prefix--") {
                alert("Please Select Prefix");
                document.getElementById("<%=ddlPrifix.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtlasercodeFrom.ClientID%>").value == "") {
                alert("Please Fill Laser From");
                document.getElementById("<%=txtlasercodeFrom.ClientID%>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtlasercodeto.ClientID%>").value == "") {
                alert("Please Fill Laser To");
                document.getElementById("<%=txtlasercodeto.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=txtQty.ClientID%>").value == "") {
                alert("Please Fill Qty");
                document.getElementById("<%=txtQty.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=txtManulQty.ClientID%>").value == "" || document.getElementById("<%=txtManulQty.ClientID%>").value == "0") {
                alert("Please Fill Manul Qty");
                document.getElementById("<%=txtManulQty.ClientID%>").focus();
                return false;
            }

            else if (document.getElementById("<%=txtlasercodetoManul.ClientID%>").value > document.getElementById("<%=txtlasercodeto.ClientID%>").value) {
                var val = document.getElementById("<%=txtlasercodeto.ClientID%>").value - document.getElementById("<%=txtlasercodeFrom.ClientID%>").value
                alert("Please Fill Less Quentity .Only You cant give quentity "+val);
                document.getElementById("<%=txtManulQty.ClientID%>").focus();
                return false;
            }

            else {
                return true;
            }
        }


        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <style>
        .alignheader
        {
            text-align: left;
        }
        .bgheadergrid
        {
            background: #e6f0a3; /* Old browsers */
            background: -moz-linear-gradient(top,  #e6f0a3 0%, #d2e638 50%, #c3d825 51%, #dbf043 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#e6f0a3), color-stop(50%,#d2e638), color-stop(51%,#c3d825), color-stop(100%,#dbf043)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top,  #e6f0a3 0%,#d2e638 50%,#c3d825 51%,#dbf043 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top,  #e6f0a3 0%,#d2e638 50%,#c3d825 51%,#dbf043 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top,  #e6f0a3 0%,#d2e638 50%,#c3d825 51%,#dbf043 100%); /* IE10+ */
            background: linear-gradient(to bottom,  #e6f0a3 0%,#d2e638 50%,#c3d825 51%,#dbf043 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#e6f0a3', endColorstr='#dbf043',GradientType=0 ); /* IE6-9 */
        }
    </style>
    <div style="margin: 20px;" align="center">
        <fieldset>
            <legend style=" background-color:Yellow">
                <div style="margin-left: 10px; font-size: medium; color: Black; width:250px; " >
                    Dispatch REG From Warehouse
                </div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td style="width: 100px;">
                    </td>
                    <td class="form_text" align="left">
                        State Name <span class="alert">* </span>
                    </td>
                    <td align="left">
                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="DropDownListStateName"
                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="form_text" align="left">
                        Rtolocaiton Name <span class="alert">* </span>
                    </td>
                    <td align="left">
                        <asp:DropDownList AutoPostBack="True" Visible="true" ID="dropDownListClient" CausesValidation="false"
                            Width="140px" runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID"
                            OnSelectedIndexChanged="dropDownListClient_SelectedIndexChanged">
                            <asp:ListItem>--Select RTO Name--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;">
                    </td>
                    <td class="form_text" align="left">
                        Shipping Address
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtAddress" runat="server" TabIndex="6" TextMode="MultiLine" Columns="30"
                            Rows="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <table width="100%">
                            <tr 
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                
                                style="font-size: larger; font-weight: bold; background: #e6f0a3; /* old browsers */
background: -moz-linear-gradient(top,  #e6f0a3 0%, #d2e638 50%, #c3d825 51%, #dbf043 100%); /* ff3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#e6f0a3), color-stop(50%,#d2e638), color-stop(51%,#c3d825), color-stop(100%,#dbf043));
                                /* chrome,safari4+ */
background: -webkit-linear-gradient(top,  #e6f0a3 0%,#d2e638 50%,#c3d825 51%,#dbf043 100%); /* chrome10+,safari5.1+ */
background: -o-linear-gradient(top,  #e6f0a3 0%,#d2e638 50%,#c3d825 51%,#dbf043 100%); /* opera 11.10+ */
background: -ms-linear-gradient(top,  #e6f0a3 0%,#d2e638 50%,#c3d825 51%,#dbf043 100%); /* ie10+ */
background: linear-gradient(to bottom,  #e6f0a3 0%,#d2e638 50%,#c3d825 51%,#dbf043 100%); /* w3c */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#e6f0a3', endColorstr='#dbf043',GradientType=0 );">
                                <td style="color: Black">
                                    Size <span class="alert">* </span>
                                </td>
                                <td style="color: Black">
                                    Prefix <span class="alert">* </span>
                                </td>
                                <td style="color: Black">
                                    Laser Code From <span class="alert">* </span>
                                </td>
                                <td style="color: Black">
                                    Laser Code To Fix <span class="alLaser Code To Fix <span class="alert">*        </td>
                                <td style="color: Black">
                                    Laser Code To Manual <span class="aLaser Code To Manual <span class="alert">*        </td>
                                <td style="color: Black">
                                    Fix QTY <span class="alert">* </span>
                                </td>
                                <td style="color: Black">
                                    Manual QTY <span class="alert">* </Manual QTY <span class="alert">*        </td>
                                <td style="color: Black">
                                   ADD
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlSize" runat="server" DataTextField="productsize" 
                                        DataValueField="productsize" AutoPostBack="True" 
                                        onselectedindexchanged="ddlSize_SelectedIndexChanged">
                                        <asp:ListItem>--Select Product Size--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPrifix" runat="server" DataTextField="prefix" DataValueField="prefix"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlPrifix_SelectedIndexChanged">
                                        <asp:ListItem>--Select Prefix--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtlasercodeFrom" runat="server" Enabled="False"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtlasercodeto" runat="server" Enabled="False"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtlasercodetoManul" runat="server" Enabled="False"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQty" runat="server" Enabled="False"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtManulQty" runat="server" AutoPostBack="true"  onkeypress="return isNumberKey(event)" OnTextChanged="txtManulQty_TextChanged"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="ADD" CssClass="button" OnClick="btnSave_Click"
                                        OnClientClick="return validation()" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" style="width: 100%">
                                    <asp:Panel ID="Panel1" runat="server" Visible="false">
                                        <fieldset>
                                            <legend>
                                                <div style="margin-left: 10px; font-size: medium; color: Black">
                                                    ITEMS
                                                    <%--<asp:HiddenField ID="printflag" runat="server"></asp:HiddenField>--%>
                                                </div>
                                            </legend>
                                            <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" Width="100%" AutoGenerateColumns="false"
                                                GridLines="None">
                                                <AlternatingRowStyle BackColor="White" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                                                <HeaderStyle Font-Bold="True" ForeColor="Black" CssClass="bgheadergrid alignheader" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#EFF3FB" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                <Columns>
                                                  <%-- <asp:BoundField DataField="stateid" HeaderText="stateid" SortExpression="stateid" Visible="false" />
                                                  <asp:BoundField DataField="rtlolocaitonid" HeaderText="rtlolocaitonid" SortExpression="rtlolocaitonid" Visible="false" />
                                                  --%>
                                                  <%--  <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Select
                                                            <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" /></HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CHKSelect" runat="server" Checked="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                 <%--   stateid, stateName, rtlolocaitonid, rtlolocaitonName,  address, productsizeid, qty, prifix, lasercodefrom, lasercodeend;--%>
                                                    <asp:TemplateField  HeaderText="ID">
                                                    
                                                        <ItemTemplate>
                                                             <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="State Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblquentity" runat="server" Text='<%#Eval("stateName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPreQuantity" runat="server" Text='<%#Eval("rtlolocaitonName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAddress" Enabled="false" runat="server" Text='<%#Eval("address") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="Product Size">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProductSize" Enabled="false" runat="server" Text='<%#Eval("productsizeid") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="Prefix">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtprifix" Enabled="false" runat="server" Text='<%#Eval("prifix") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="Laser Code From">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtlasercodefrom" Enabled="false" runat="server" Text='<%#Eval("lasercodefrom") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="Laser Code End">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtlasercodeend" Enabled="false" runat="server" Text='<%#Eval("lasercodeend") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Quantity">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtqty" Enabled="false" runat="server" Text='<%#Eval("qty") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                              <asp:TemplateField HeaderText="State Name" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstateid" runat="server" Text='<%#Eval("stateid") %>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               <asp:TemplateField HeaderText="State Name" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrtlolocaitonid" runat="server" Text='<%#Eval("rtlolocaitonid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                         
                                        
                                             <asp:TemplateField HeaderText="Delete" >
                                                <ItemTemplate>
                                                  <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" CommandArgument ='<%#Eval("Id") %>'
                                        onclick="btnDelete_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </fieldset>
                                    </asp:Panel>

                                     
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="LblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                                <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300"
                                                    Font-Bold="True" />
                                            </td>
                                            <td align="center">
                                                <asp:Button ID="btnSaveDispatch" runat="server" Text="Save ALL" 
                                                    CssClass="button" onclick="btnSaveDispatch_Click" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
    </div>
</asp:Content>
