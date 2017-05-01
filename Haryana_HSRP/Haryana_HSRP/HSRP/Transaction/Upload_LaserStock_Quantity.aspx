<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="Upload_LaserStock_Quantity.aspx.cs" Inherits="HSRP.Transaction.Upload_LaserStock_Quantity" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <div style="margin: 20px; width: 85%; background-color: transparent; float: left;
        position: fixed" align="left">
        <fieldset>
            <legend style="color: #000000">Upload Laser Stock Excel </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    &nbsp;&nbsp;
                    <table align="center" width="100%" style="margin-top: 7px">

                    <tr>
                   <td width="20%" class="form_text">
                                                    <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization" 
                                                        ForeColor="Black" />
                                                &nbsp;&nbsp;
                                                    <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" Width="125px" ID="DropDownListStateName"
                                                        runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                    <td width="30%" class="form_text">
                                                        <asp:Label ID="label1" runat="server" Text="Embossing Center:"></asp:Label>
                                                   <%-- </td>

                                                     <td width="25%">&nbsp;--%>
                                                        <asp:DropDownList ID="dropDownListClient" Height="20px" Width="125px" Tabindex="1"
                                                        runat="server" DataTextField="EmbCenterName" AutoPostBack="false"
                                                        DataValueField="NAVEMBID">
                                                        </asp:DropDownList>
                                                    </td>
                       <%-- <td width="25%" class="form_text">
                            <asp:Label Text="Location Name:" runat="server" ID="lbllocationname" />&nbsp;
                            <asp:TextBox ID="txtlocation" runat="server" ></asp:TextBox>
                        </td>--%>

                                                  <%--  <td width="20%" class="form_text">                                     
                                                        <asp:Label Text="Product Code:" runat="server" ID="lblErpProductCode" />
                                                        <span style="color:red">*</span>
                                                   
                                                        <asp:DropDownList AutoPostBack="True" Width="125px" ID="ddlErpProductCode"
                                                            runat="server" ClientIDMode="Static">
                                                        </asp:DropDownList>
                                                    </td>--%>
                                               
                    </tr>

                        <tr>
                            <td align="left" valign="top" class="Label_user_batch" nowrap="nowrap" 
                                style="width: 313px">
                                Select File:
                            </td>
                            <td>
                                <asp:FileUpload ID="FileUpload2" runat="server" />
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnuploadhrexceldata" runat="server" 
                                    onclick="btnuploadhrexceldata_Click" onclientclick=" javascript:return vali();" 
                                    Text="Upload Laser Stock" />
                            </td>
                        </tr>
                      
                        <tr>
                            <td colspan="3">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">Note: Please
                                    Upload Excel file of .xls Format and Have only one WorkSheet of Defined Format.</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left">
                                <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="left" 
                                style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                                 <br />
                                <asp:TextBox ID="txtduploicateempid" Visible="false" runat="server" Width="137px"></asp:TextBox>
                                
                                 <br />
                                <br />
                            </td>
                        </tr>
                          <tr>
                          <td style=" color:Black; width: 313px;">Total Upload Records :</td>
                            <td colspan="2" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="lbltotaluploadrecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                          <tr>
                           <td style=" color:Black; width: 313px;">No. of Records already exist in database:</td>
                            <td colspan="2" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;" >
                                <asp:Label ID="lbltotladuplicaterecords" runat="server" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                    <%-- <tr>
                        <td colspan="" style=" color:Black; font-size:18px; float:right; width: 313px;" 
                             align="right">

                        Example File Format <a href="../Data/HR%20Entry%20fornat.xlsx">Download</a>
                       
                            
                        </td>
                     </tr>--%>
                    </table>
                   
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
