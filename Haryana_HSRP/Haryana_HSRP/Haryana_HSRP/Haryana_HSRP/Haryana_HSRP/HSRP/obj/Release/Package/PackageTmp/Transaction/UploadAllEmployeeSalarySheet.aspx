<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UploadAllEmployeeSalarySheet.aspx.cs" Inherits="HSRP.Transaction.UploadAllEmployeeSalarySheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <div style="margin: 20px; width: 65%; background-color: transparent; float: left;
        position: fixed" align="left" >
        <fieldset>
            <legend >Upload Salary Sheet </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                    <tr>
                    <td> <asp:Label Text="Company Name:"  runat="server" ID="lblCompanyName" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td style="width: 165px">                     
                        <asp:DropDownList  ID="DDlCompany_Name"  runat="server" DataTextField="CompanyName" DataValueField="CompanyId" >
                            </asp:DropDownList>
                    </td>
                    </tr>

                    <tr>
                    <td> <asp:Label Text="Month Name :"  runat="server" ID="lblMonth" ForeColor="Black" />&nbsp;&nbsp;</td>

                    <td valign="middle" class="Label_user_batch">
                                                   
                                                     <asp:DropDownList ID="DDLMonth"                                                           
                                                                runat="server" 
                                                               EnableTheming="False">
                                                         <asp:ListItem Text="--Select Month--" Value="--Select Month--" />
                                                         <asp:ListItem Text="January" Value="1" />
                                                         <asp:ListItem Text="February" Value="2" />
                                                          <asp:ListItem Text="March" Value="3" />
                                                          <asp:ListItem Text="April" Value="4" />
                                                          <asp:ListItem Text="May" Value="5" />
                                                          <asp:ListItem Text="June" Value="6" />
                                                          <asp:ListItem Text="July" Value="7" />
                                                          <asp:ListItem Text="August" Value="8" />
                                                          <asp:ListItem Text="September" Value="9" />
                                                          <asp:ListItem Text="October" Value="10" />
                                                          <asp:ListItem Text="NovemBer" Value="11" />
                                                          <asp:ListItem Text="December" Value="12" />
                                                          

                                                       
                                                            </asp:DropDownList>
                                                         </td>
                                                
                    </tr>

                    <tr>
                    <td> 
                        <asp:Label Text="Year:"  runat="server" ID="lblyear" 
                            ForeColor="Black" />
                    </td>
                    <td valign="middle" class="Label_user_batch" >
                                                         <asp:DropDownList ID="ddlyear"  runat="server" >
                                                          <asp:ListItem>--Select Year--</asp:ListItem>
                                                          <asp:ListItem>2016</asp:ListItem>
                                                          <asp:ListItem>2017</asp:ListItem>
                                                          <asp:ListItem>2018</asp:ListItem>
                                                          <asp:ListItem>2019</asp:ListItem>
                                                          <asp:ListItem>2020</asp:ListItem>
                                                          <asp:ListItem>2021</asp:ListItem>
                                                          <asp:ListItem>2022</asp:ListItem>
                                                          <asp:ListItem>2023</asp:ListItem>
                                                          <asp:ListItem>2024</asp:ListItem>
                                                          <asp:ListItem>2025</asp:ListItem>                                                         
                                                            </asp:DropDownList>
                                                         </td>
                    </tr>
                        <tr>
                            <td align="left" valign="top" class="form_text" nowrap="nowrap">
                                Select File:
                            </td>
                            <td style="width: 165px">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" class="button" Text="Upload Excel Data" OnClick="Button1_Click"   OnClientClick=" javascript:return vali();"/>
                                 &nbsp;
                                                             
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">Note: Please
                                    Upload Excel file of .xlsx Format and Have only one WorkSheet of Defined Format.</span>
                            </td>
                             
                             
                            
                            <td align="center" rowspan="9" valign="middle">
                                
                            </td>
                           
                        </tr>
                        <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   1.     File must be in .xls Format.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                   2.     All the records should be in the given format.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                  3.     There should not be any blank record in the file at end of the file too.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                4.     There should be only one sheet in the file.</span>
                            </td>
                        </tr>
                        <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                               5.     Date should be in MM/DD/YYYY format.</span>
                            </td>
                        </tr>
                        
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
6. There should not be any special character .(Eg. single quote ‘ )</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
7.Amount Column should contain only number. (Eg.1000).</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Maroon; font: verdana arial 12px;">
                                <br />
                                    &nbsp;</span></td>
                        </tr>
                       
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left" style="font-family: Verdana,tahoma, arial; font-size: medium;">
                                <asp:Label ID="llbMSGError" runat="server" visible="true" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:TextBox ID="txtduploicateempid" Visible="false" runat="server" Width="137px"></asp:TextBox>
                                
                                 <br />
                                <br />
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
                           <td style=" color:Black;">No. of Records already exist in database:</td>
                            <%--  <asp:Label ID="lblcoutduplicate" runat="server" ></asp:Label>--%>
                            <td colspan="3" align="left" 
                                  style="font-family: Verdana,tahoma, arial; font-size: medium;" >
                                <asp:Label ID="lbltotladuplicaterecords" runat="server" ForeColor="Black"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Text="0" Visible="False"></asp:Label>
                            </td>
                        </tr>
                   
                        <tr>
                        <td colspan="" style=" color:Black; font-size:18px; float:right" align="right">
                            Example File Format <a href="../Data/dealer%20data%20format.xlsx">Download</a>
                      
                        </td>
                        </tr>
                    </table>
                </div>
            </div> 
        </fieldset>
    </div>
</asp:Content>
