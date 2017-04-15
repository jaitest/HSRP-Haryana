﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComplainReportResolution.aspx.cs" Inherits="HSRP.Transaction.ComplainReportResolution" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
  <%--  #myDiv img
{
max-width:100%; 
max-height:100%;
margin:auto;
display:block;
}--%>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
   
    <style type="text/css">
        .style1
        {
            width: 211px;
        }
        .style2
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            text-align: left;
            width: 171px;
        }
        .style4
        {
            width: 171px;
        }
        .style7
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            text-align: left;
            width: 136px;
        }
        .style8
        {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 136px;
        }
        .style9
        {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 227px;
        }
        .style10
        {
            color: Black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            text-align: left;
            width: 227px;
        }
        .style11
        {
            width: 227px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    
    <div style="margin:auto;"  align="center">
        <fieldset>
            <legend>
                <div align="left" style="margin-left: 10px; font-size: medium; color: Black" >
                   ComplaintReportResolution</div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td colspan="5">
                        <table style="background-color: #FFFFFF" width="85%" border="0" align="center" cellpadding="3" cellspacing="1">
                           
                              
                           
                            <tr>
                                <td colspan="4">
                                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                        <tr valign="top">
                                            <td colspan="1" nowrap="nowrap" align="left" style="margin-left: 50px" width="200px" class="form_text">
                                                
                                                Request Generated By:
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="150px">
                                                <asp:Label ID="LabelCreatedID" ForeColor="Blue" runat="server" />
                                            </td>
                                            
                                            <td class="form_text" nowrap="nowrap" align="left" width="200px">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                Record Date:
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="200px">
                                                <asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="style9">
                                    ComplaintDateTime: 
                                </td>
                                <td align="left" class="style1">
                                    <asp:Label ID="lblCompalaintDate" runat="server" Text="label" ForeColor="Black" Font-Bold="True" ></asp:Label>
                                </td>
                              
                                <td align="left" class="style8">
                                   
                                    ChasisNo:<td align="left">
                                    
                                    <asp:Label ID="lblChaissNo" runat="server" Text="LabelChassisNo" 
                                        Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
            <td class="style10" >Name <span class="alert">&nbsp;</span>:</td>
            <td align="left" class="style1">
            <asp:Label ID="lblName" runat="server" Text="LabelName" ForeColor="Black" Font-Bold="True" ></asp:Label>
            </td>

             <td class="style7" >Complaint:</td>
            <td align="left">
            <asp:Label ID="lblRemarks" runat="server" Text="LabelRemarks" Font-Bold="True" 
                    ForeColor="Black"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style10">Mobile No <span class="alert">&nbsp;</span>: </td>
            <td align="left" class="style1">
                <asp:Label ID="lblMobileNo" runat="server" Text="LabelMobileNo"  Font-Bold="True" 
                    ForeColor="Black"></asp:Label>
            </td>


            <td class="style7">Status <span class="alert">&nbsp;</span>: </td>
            <td align="left">
                <asp:Label ID="lblStatus" runat="server" Text="Status" ForeColor="Black" Font-Bold="True" ></asp:Label>
            </td>
        </tr>
            
        <tr>
            <td class="style10">Email ID: </td>
            <td align="left" class="style1">
               <asp:Label ID="lblEmail" runat="server" Text="LabelEmail" ForeColor="Black" Font-Bold="True" ></asp:Label>
            </td>

            <td class="style2">Resolution: </td>
            <td align="left" class="style1">
               <asp:TextBox ID="txtresolution" runat="server" TextMode="MultiLine" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style10">Reg No: </td>
            <td align="left" class="style1">
                
                <asp:Label ID="lblRegNo" runat="server" Text="LabelRegNo" Font-Bold="True" 
                    ForeColor="Black"></asp:Label>
            </td>
           
        </tr>
            
        <tr>
            <td class="style10" valign="top">Engine No : </td>
            <td align="left" class="style1">
                <asp:Label ID="lblEngineNo" runat="server" Text="LabelEngineNo" 
                    Font-Bold="True" ForeColor="Black"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style11">
                                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red" Font-Bold="True" 
                                        style="text-align: left"></asp:Label>
                                                        </td>
           
               
        </tr>
        <tr>
        <td colspan="4" class="style4">
        
        <asp:Button ID="btnUpdate" runat="server" Text="Update"  Width="150px" 
                onclick="btnUpdate_Click"/>
        </td>
        <td class="style1"></td>
      
        </tr>
                            <tr>
                                <td class="style11">
                                    <%--<asp:Button ID="Button2" OnClientClick="return validate()" runat="server" class="button" Width="150px"
                                        Text="Update" OnClick="Button1_Click" />--%></td>
                                <td class="style1">
                                    &nbsp;
                                </td>
                             
                            </tr>
                          
                            
                                </table>

                                </table>


                            </tr>
                          
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
   
    </form>
</body>
</html>
