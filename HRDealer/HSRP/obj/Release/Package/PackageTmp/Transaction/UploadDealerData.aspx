<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"    CodeBehind="UploadDealerData.aspx.cs" Inherits="HSRP.Transaction.UploadDealerData" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span style="color: rgb(0, 0, 0); font-family: tahoma, arial, verdana; font-size: small; font-style: normal; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: normal; letter-spacing: normal; orphans: 2; text-align: start; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; background-color: rgb(240, 240, 240); display: inline !important; float: none;"></span><script type="text/javascript" language="javascript">

        function vali()
        {
            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select Client--") {
                alert("Please Provide RTO Location Name.");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }

            }
      </script><fieldset>
            <legend >                
                     <asp:Label runat="server" style=" margin-left: 10px; margin-top:200px; font-size: medium; color: Black;" Text="Upload Dealer Excel"></asp:Label>     

            </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                    <tr>
                    <td> <asp:Label Text="HSRP State:"  runat="server" ID="labelOrganization" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td valign="middle" class="Label_user_batch" style="width: 185px" >
                     
                                               <asp:DropDownList AutoPostBack="true" ID="DropDownListStateName" CausesValidation="false"
                                                    runat="server" DataTextField="HSRPStateName" 
                                                    DataValueField="HSRP_StateID" 
                                                 onselectedindexchanged="DropDownListStateName_SelectedIndexChanged" >
                                                </asp:DropDownList>
                    </td> 
                    </tr>

                    <tr>
                    <td> <asp:Label Text="RTO Location:"  runat="server" ID="labelClient" ForeColor="Black" />&nbsp;&nbsp;</td>

                    <td valign="middle" class="Label_user_batch" style="width: 185px">
                                                   
                                                      <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListStateName" EventName="SelectedIndexChanged" />
                                                            
                                                        </Triggers>
                                                        <ContentTemplate>
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
                   <%-- <td  style="width: 186px ; font-size:small; color: Black ">Today Booking:</td>
                    <td><asp:Label ID="lblCount" style="font-size:small; color: Black" runat="server">0</asp:Label>  </td>--%>

                          <td colspan="4" >
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">Note: Please Upload Excel file of .xlsx Format and Have only one WorkSheet of Defined Format.</span>
                            </td>    
                         



                  </tr>
                  <tr>
                    <%--<td style="width: 186px ; font-size:small; color: Black ">Today Collection Amount:</td>
                    <td><asp:Label ID="lblCollection"     style=" font-size:small; color: Black; "  runat="server">0</asp:Label></td>   --%>  
                      <td colspan="2" >
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   1.     File must be in .xls Format.</span>
                            </td>


                  </tr>

                     <tr>
                   <%-- <td style="width: 186px ; font-size:small; color: Black ">Available Amount:</td>
                    <td><asp:Label ID="lblAvailableAmount"     style=" font-size:small; color: Black; "  runat="server">0</asp:Label></td>--%>
                          <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   2.     All the records should be in the given format.</span>
                            </td>
                  </tr>
                    <tr>
                       
                   <%-- <td style="width: 186px ; font-size:small; color: Black "> Last Deposit Amount:</td>
                    <td><asp:Label ID="lbllastDepositAmount"     style=" font-size:small; color: Black; "  runat="server">0</asp:Label></td>--%>
                        <td colspan="2">
                          <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                          3.     There should not be any blank record in the file at end of the file too.</span>
                            </td>
                  </tr>
                
                <tr>
                   <%-- <td style="width: 186px ; font-size:small; color: Black ">Dealer Name:</td>
                    <td><asp:Label ID="lbldealername"  style=" font-size:small; color: Black; "  runat="server" Width="85px"></asp:Label></td>--%>
                    <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                4.     There should be only one sheet in the file.</span>
                            </td>

                  </tr>
                <tr>
                   <%-- <td  style="width: 186px ; font-size:small; color: Black " >Dealer Code:</td>
                    <td><asp:Label ID="lbldealercode"  style=" font-size:small; color: Black; " runat="server"></asp:Label></td>--%>
                     <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                               5.     Date should be in MM/DD/YYYY format.</span>
                            </td>
                  </tr>
                <tr>
                   <%-- <td  style="width: 186px ; font-size:small; color: Black "> Dealer Area:</td>
                    <td><asp:Label ID="lblarea"  style=" font-size:small; color: Black;" runat="server"></asp:Label></td>--%>
                    <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
6. There should not be any special character .(Eg. single quote ‘ )</span>
                            </td>
                  </tr>
                 <tr>
                    <%-- <td  style="width: 186px ; font-size:small; color: Black " nowrap="nowrap">Last Bank Deposit Date:</td>
                    <td><asp:Label style=" font-size:small; color: Black; " ID="lblLastDepositdate" runat="server">0</asp:Label></td>--%>
                      <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
7. Price Column should contain only number. (Eg.1000).</span>
                            </td>
                  </tr>
                        <tr>
                          <%--  <td  style="width: 186px ; font-size:small; color: Black " nowrap="nowrap"></td>
                              <td></td>--%>
                            <td colspan="2"> <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
8.     After uploading SYNC Confirm is compulsory.<br />                            
                            </td>
                        </tr>

                        <tr>
                            <td align="left"  style="width: 186px ; font-size:small; color: Black "  nowrap="nowrap">
                                Select File:
                            </td>
                            <td style="width: 165px">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" class="button" Text="Upload Excel Data" OnClick="Button1_Click"   OnClientClick=" javascript:return vali();"/>
                                 &nbsp;
                                 <asp:Button ID="btnSync" runat="server" class="button" Text="Confirm"  onclick="btnSync_Click" Width="160px"  />
                            
                            </td>
                        </tr>
                                                                  
                     
                         <tr>
                         <td> <asp:Label Text="Duplicate Records :"  runat="server" ID="label2" ForeColor="Black" />&nbsp;&nbsp; </td>
                          <td>   <asp:TextBox runat="server" id="txtDuplicateRecords"></asp:TextBox> </td>
                        </tr> 

                        <%--<tr>
                            <td> <asp:Label Text="Block Records :"  runat="server" ID="label3" ForeColor="Black" />&nbsp;&nbsp; </td> 
                            <td> <asp:TextBox runat="server" id="txtblockrecords"></asp:TextBox>  </td>
                        </tr> --%>

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
                                <asp:Label ID="llbMSGError2" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                               <%-- <asp:Label ID="llbMSGError0" runat="server" ForeColor="Red"></asp:Label>
                                <br />--%>
                                  <asp:Label ID="lblChassisNo" runat="server" Font-Bold="True"  ForeColor="Blue" Text=" Chassis No=" Visible="False"></asp:Label>
                                <br />
                            </td>
                        </tr> 
                         
                                             
                       
                    </table>
                     

                </div>
                <table id="tblgrid">
                   
                      <tr>

                         <td align="center" style="padding: 10px">
                                        <asp:GridView ID="GridView1" visible="false" runat="server" BackColor="White" AutoGenerateColumns="false"
                                            OnPageIndexChanging="GridView1_PageIndexChanging"  AllowPaging="true" PageSize="1000"
                                            BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                            DataKeyNames="hsrprecordid">
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                            <PagerSettings Visible="False" Mode="NextPreviousFirstLast" PageButtonCount="20" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <RowStyle ForeColor="#000066" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Select
                                                        <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" /></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CHKSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                 <asp:TemplateField>
                                                    <HeaderTemplate>
                                                       Dealer Id</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldealerid" runat="server" Text='<%#Eval("dealerid") %>'
                                                            Enabled="false"></asp:Label>
                                                      
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                     Dealer Code</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldealercode" runat="server" Text='<%#Eval("dealercode") %>'
                                                            Enabled="false"></asp:Label>
                                                      
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>

                                                  <asp:TemplateField>
                                                <HeaderTemplate >
                                                Chassis No
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label  ID="lblChasisNo" runat="server" Text='<%#Eval("ChassisNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                   Rto Location Name 
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                    <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("rtolocationname") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                <HeaderTemplate >
                                                Owner Name
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label  ID="lblownername" runat="server" Text='<%#Eval("ownername") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                <HeaderTemplate >
                                               Address
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label  ID="lbladdress1" runat="server" Text='<%#Eval("address1") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField >
                                                <HeaderTemplate >
                                               Vehicle Class
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label ID="lblvehicleclass" runat="server" Text='<%#Eval("vehicleclass") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                         
                                                <asp:TemplateField>
                                                <HeaderTemplate >
                                                 Vehicle Type 
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label  ID="lblvehicletype" runat="server" Text='<%#Eval("vehicletype") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                 <asp:TemplateField>
                                                <HeaderTemplate >
                                                Order Type 
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label  ID="lblordertype" runat="server" Text='<%#Eval("ordertype") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                <HeaderTemplate >
                                                Engine No
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label ID="lblengineno" runat="server" Text='<%#Eval(" EngineNo") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                <HeaderTemplate >
                                                Fix Charge No
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label ID="lblfixcharge" runat="server" Text='<%#Eval("fixingcharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField Visible="false">
                                                <HeaderTemplate >
                                                Mobile No
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label ID="lblMobile" runat="server" Text='<%#Eval("Mobileno") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                <HeaderTemplate >
                                                Vehicleregno No
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <asp:Label ID="lblvehicleregno" runat="server" Text='<%#Eval("vehicleregno") %>'></asp:Label>
                                                </ItemTemplate>
                                                </asp:TemplateField>
                                               

                                              


                                               
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                    </tr>
                      <tr>
                        <td align="center">
                            <asp:Button ID="btnsave"   visible="false" runat="server" Text="Save" OnClick="btnsave_Click" />
                        </td>

                    </tr>
                      <tr>
                          <td style=" color:Black;">Total Upload Records :</td>
                            <td colspan="3" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="lbltotaluploadrecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                      <tr>
                           <td style=" color:Black;">No. of Records Already Exist:</td>
                            <td colspan="3" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;" >
                                <asp:Label ID="lbltotladuplicaterecords" runat="server" ForeColor="Black"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                        </tr>                   
                      <tr>
                        <td colspan="" style=" color:Black; font-size:18px; float:right" align="right">
                             Example File Format <a href="../Data/DealerDataFormat.xls">Download</a>
                           
                        </td>

                        </tr>

                </table>
            </div> 
        </fieldset>
   
</asp:Content>
