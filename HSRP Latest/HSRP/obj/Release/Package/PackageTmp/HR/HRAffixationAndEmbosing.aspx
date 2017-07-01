<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HRAffixationAndEmbosing.aspx.cs" Inherits="HSRP.HR.HRAffixationAndEmbosing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                    Affixation Entry
                </div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td colspan="5">
                        <table style="background-color: #FFFFFF" width="85%" border="0" align="center" cellpadding="3"
                            cellspacing="1">
                            <tr>

                                <td align="left" class="form_text"></td>
                                <td align="left"></td>
                            </tr>
                            <tr>
                                <td align="left" >Vechile No: <span class="alert"></span>
                                </td>
                                <td >
                                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                                     <span><asp:Button runat="server" style="margin-left:30px;" CssClass="btn" ID="btnSearch" ValidationGroup="Validate" Text="Go" OnClick="btnSearch_Click" /></span> 
                                    <asp:RequiredFieldValidator style="margin-left:30px;" ID="txtSearchValidate" runat="server" ControlToValidate="txtSearch" ValidationGroup="Validate" ErrorMessage="Enter Vechile NO"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                     <asp:GridView ID="gridRecords" runat="server" AutoGenerateColumns="false" OnRowCommand="gridRecords_RowCommand" >
                                         <Columns>
                                            
                                          <asp:BoundField Visible="false"    DataField="HSRPRecordID" />
                                             <asp:BoundField DataField="VehicleRegNo" HeaderText="Vechile Reg NO" />
                                              <asp:BoundField DataField="HSRP_Front_LaserCode" HeaderText="Front Laser Code" />
                                                      <asp:BoundField DataField="HSRP_rear_LaserCode" HeaderText="Rear Laser Code" />
                                             <asp:ButtonField CommandName="Update"   HeaderText="Edit"  ButtonType="Button" Text="Closed" />
                                         </Columns>
                                     </asp:GridView>

                                </td>

                            </tr>
                            <tr>
                                <td colspan="2" align="left" >
                                       <asp:Label style="margin-left:30px;" CssClass="error" runat="server" ID="lblerror" ></asp:Label>
                                    <asp:Label style="margin-left:30px;" CssClass="sucess" runat="server" ID="lblsuccess" ></asp:Label>
                                    <br />
                                    <br />
                                  <span  class="sucess">Help Item</span> 
                                    <ol class="sucess" >

                                        <li>Please provide vehicle registration number.</li>
                                        <br />
                                        <li>Ensure plate is recieved at center if not please do that first.</li>
                                        <br />
                                        <li>If vehicle has multiple entries and are not closed please close them as well.</li>
                                    </ol>
                                </td>
                                <td></td>
                            </tr>

                        </table>
                        
                    </td>
                </tr>
            </table>
            
            
        </fieldset>
    </div>



</asp:Content>
