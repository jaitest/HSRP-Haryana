<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="UploadHSRPBooking.aspx.cs" Inherits="HSRP.Transaction.UploadHSRPBooking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">

        function vali() {

           
        }
    </script>
    <div style="margin: 20px; width: 50%; background-color: transparent; float: left; position: fixed"
        align="left">
        <fieldset>
            <legend><span class="headingmain">Upload Order Booking  </span></legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                        <tr>
                            <td>
                                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" ForeColor="Black" Visible="false" />&nbsp;&nbsp;</td>
                            <td style="width: 165px">

                                <asp:DropDownList  ID="DropDownListStateName" Visible="false" 
                                    runat="server" DataTextField="HSRPStateName"
                                    DataValueField="HSRP_StateID"
                                   >
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>

                            <td>
                                <asp:Label Text="Embossing Center:" runat="server" Width="100px" nowrap="nowrap" ID="lblEmb"
                                    ForeColor="Black" Visible="False" />
                            </td>
                            <td valign="middle" class="Label_user_batch" style="width: 165px">

                                <asp:DropDownList ID="ddlEmbossingCenter"
                                    runat="server"
                                   Visible="False" >
                                    <asp:ListItem>--Select Embossing Center--</asp:ListItem>
                                    <asp:ListItem>BURARI</asp:ListItem>
                                    <asp:ListItem>MAYAPURI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                         

                        </tr>

                        <tr>
                            <td align="left" valign="top" class="form_text" nowrap="nowrap">Select File:
                            </td>
                            <td style="width: 165px">
                                <asp:FileUpload ID="FileUpload1" runat="server"  />
                                &nbsp;&nbsp;
                            </td>



                            <td>
                                <asp:Button ID="Button1" runat="server" class="button" Text="Upload Excel Data" OnClick="Button1_Click" OnClientClick=" javascript:return vali();" />
                                &nbsp;
                                 <asp:Button ID="btnSync" runat="server" class="button" Text="Sync on Server" Visible="false" OnClick="btnSync_Click" />

                            </td>
                            <td></td>
                        </tr>

                    </table>
                </div>
            </div>
        </fieldset>
    </div>
    

        <%--<table align="right">
            <tr>
                <td>


                    <asp:GridView ID="grdid" runat="server" align="Bottom" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false">
                        <AlternatingRowStyle BackColor="White" />



                        <Columns>


                            <asp:BoundField HeaderText="DealerName" ItemStyle-HorizontalAlign="Center" DataField="DealerName" />
                            <asp:BoundField HeaderText="DealerCode" ItemStyle-HorizontalAlign="Center" DataField="DealerCode" />
                            <asp:BoundField HeaderText="VehicleClass" ItemStyle-HorizontalAlign="Center" DataField="VehicleClass" />
                            <asp:BoundField HeaderText="OrderType" ItemStyle-HorizontalAlign="Center" DataField="OrderType" />
                            <asp:BoundField HeaderText="AffixationCenter" ItemStyle-HorizontalAlign="Center" DataField="AffixationCenter" />
                            <asp:BoundField HeaderText="VehicleRegNo" ItemStyle-HorizontalAlign="Center" DataField="VehicleRegNo" />
                            <asp:BoundField HeaderText="OwnerName" ItemStyle-HorizontalAlign="Center" DataField="OwnerName" />
                            <asp:BoundField HeaderText="Address" ItemStyle-HorizontalAlign="Center" DataField="Address" />
                            <asp:BoundField HeaderText="MobileNo" ItemStyle-HorizontalAlign="Center" DataField="MobileNo" />
                            <asp:BoundField HeaderText="VehicleType" ItemStyle-HorizontalAlign="Center" DataField="VehicleType" />
                            <asp:BoundField HeaderText="HSRPApplicationNo" ItemStyle-HorizontalAlign="Center" DataField="HSRPApplicationNo" />
                            <asp:BoundField HeaderText="EngineNo" ItemStyle-HorizontalAlign="Center" DataField="EngineNo" />
                            <asp:BoundField HeaderText="ChassisNo" ItemStyle-HorizontalAlign="Center" DataField="ChassisNo" />
                            <asp:BoundField HeaderText="vehiclemake" ItemStyle-HorizontalAlign="Center" DataField="vehiclemake" />
                            <asp:BoundField HeaderText="ModelName" ItemStyle-HorizontalAlign="Center" DataField="ModelName" />
                            <asp:BoundField HeaderText="ExShowRoomPrice" ItemStyle-HorizontalAlign="Center" DataField="ExShowRoomPrice" />





                        </Columns>

                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </td>
            </tr>

        </table>--%>
    
    <div style="margin: 20px;" align="right">
        <table align="right">
            <tr>
                <td colspan="2">
                    <span style="color: black; font: verdana arial 12px;">Mandentory Validations:</span>
                </td>

            </tr>
            <tr>

                <td colspan="3" style="color: Maroon; font: verdana arial 12px;">
                Validations Codes while File Upload:	<br />
(a) Registration no cannot be greater than 10. <br />
(b) The combination of Application no and registration no is not valid.<br />
(c) For Order type NB/OB is mandatory.<br />
(d) Rows cannot have null values.<br />
(e) Column Name in xls not be changed has to be as per format attached.<br/>
(f) Application no length is greater than 16.<br />
(g) File must be in .xls Format only.<br />
(h) All the records should be in the given format.<br />
(i) There should not be any blank record in the file at end of the file too.<br />
(j) There should be only one sheet in the file.<br />
(k) Date should be in MM/DD/YYYY format.<br />
(l) There should not be any special character .(Eg. single quote ‘ -) in vehicleregno.<br />
(m) Price Column should contain only number. (Eg.1000).<br />
(n) If there is any error please ractify it and reload the xls.
(o) Allowed Vehicle TYpe are only 

     </td>

            </tr>
            
            <tr>
                <td colspan="2">
                    <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;"" >
                        Duplicate Records :<asp:TextBox runat="server" ID="txtDuplicateRecords" Visible="true"></asp:TextBox></span>
                    <br />
                </td>
            </tr>



            <tr>
                <td style="color: Black;"></td>
                <td colspan="3" align="left"
                    style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="lbltotaluploadrecords" runat="server" Visible="false" ForeColor="Black"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="color: Black;"></td>
                <td colspan="3" align="left"
                    style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="lbltotladuplicaterecords" runat="server" Visible="false" ForeColor="Black"></asp:Label>
                    <asp:Label ID="Label1" runat="server" Text="" Visible="False"></asp:Label>
                </td>
            </tr>

            <tr>
                <td>
                    <%--<asp:LinkButton runat="server" ID="lnkView">Help</asp:LinkButton>--%>
                </td>
                <td colspan="" style="color: Black; font-size: 18px; float: right" align="right">
                    <%--<a href="../Data/dealer%20data%20format.xlsx">Download XLS File Format</a>--%>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left">
                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="left" style="font-family: Verdana,tahoma, arial; font-size: medium;">
                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="llbMSGError0" runat="server" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label ID="lblVehicleRegNo" runat="server" Font-Bold="True"
                        ForeColor="Blue" Text="VehicleRegNo=" Visible="False"></asp:Label>
                    <br />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
