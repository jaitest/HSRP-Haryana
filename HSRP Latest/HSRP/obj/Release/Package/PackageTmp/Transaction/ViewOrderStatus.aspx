<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewOrderStatus.aspx.cs" Inherits="HSRP.Transaction.ViewOrderStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript" src="../javascript/common.js"></script>
    
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        table.imagetable
        {
            font-family: verdana,arial,sans-serif;
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #999999;
            border-collapse: collapse;
        }
        table.imagetable th
        {
            background: #b5cfd2 url('cell-blue.jpg');
            border-width: 1px;
            padding: 8px;
            border-style: solid;
            border-color: #999999;
        }
        table.imagetable td
        {
            background: #dcddc0 url('cell-grey.jpg');
            border-width: 1px;
            padding: 8px;
            border-style: solid;
            border-color: #999999;
        }
    </style>
    <style type="text/css">
        .Label_user1
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
            width: 150px;
        }
        
        legend
        {
            width: 193px;
            height: 28px;
            background-image: url(../img/legend.jpg);
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            
        }
        
        fieldset
        {
            border: 1px solid #3FAFBC;
           
        }
    </style>
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=TextBoxAuthorizationNo.ClientID%>").value == "") {
                alert("Please Provide Authorization No");
                document.getElementById("<%=TextBoxAuthorizationNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxMobileNo.ClientID%>").value == "") {
                alert("Please Provide Mobile No");
                document.getElementById("<%=TextBoxMobileNo.ClientID%>").focus();
                return false;
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


    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Order Status</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                          
        <br />
        <div align="center">
         <table style="margin-top: 20px;" id="Order" runat="server">
         <tr>
         <td>
         <div align="center"><h2 style=" color:Black; font-size:22px">HIGH SECURITY REGISTRATION PLATES</h2>
         <h2 style=" color:Black; font-size:15px">Enter Authorization No and Mobile No</h2>
         </div>
            <table style="margin-top: 20px;" >
                <tr>
                    <td class="Label_user1">
                       Authorization No
                    </td>
                    <td class="Label_user1">
                        <asp:TextBox ID="TextBoxAuthorizationNo" runat="server" class="form_textbox12"></asp:TextBox>
                    </td>
                    <td class="Label_user1" align="center">
                        Mobile No
                    </td>
                    <td class="Label_user1">
                        <asp:TextBox ID="TextBoxMobileNo" runat="server" MaxLength="10" onkeydown="return isNumberKey(event);"
                            class="form_textbox12"></asp:TextBox>
                    </td>
                    <td align="center" class="Label_user1">
                        <asp:Button ID="ButtonGo" runat="server" Text="Go" OnClick="ButtonGo_Click" OnClientClick="return validate()" />
                    </td>
                </tr>
            </table>
            <br /><br />
            <div align="center"><p>Please Click And View Your Order Status</p></div>
            <br /><br /><br />
            </td>
         </tr>
</table>
        </div>
        
        <div align="center">
        <table class="imagetable"  id="show" runat="server">
         <fieldset>
        <legend>
            <div style="margin-left: 10px; font-size: medium; color: Black">
               Order Status</div>
        </legend>
            <table class="imagetable">
                <tr>
                 <th style="width: 100px" align="center" class="Label_user1">
                        <b>Custumer Name</b>
                    </th>

                    <th style="width: 100px" align="center" class="Label_user1">
                        <b>Vehicle Reg No</b>
                    </th>
                    <th class="Label_user1" align="center">
                        <b>Order Booked On</b>
                    </th>
                    <th align="center" class="Label_user1">
                        <b>Cash Received </b>
                    </th>
                     <th style="width: 100px" align="center" class="Label_user1">
                    <b>Engine No</b>
                </th>
                <th style="width: 100px" align="center" class="Label_user1">
                    <b>Chassis No</b>
                </th>
                    <th style="width: 100px" align="center" class="Label_user1">
                        <b>Status</b>
                    </th>
                </tr>
                <tr>
                 <td style="width: 100px" align="center" class="Label_user1">
                        <asp:Label ID="LabelCustumerName" runat="server" Text=""></asp:Label>
                    </td>
                    
                    <td style="width: 100px" align="center" class="Label_user1">
                        <asp:Label ID="LabelVehicleNo" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 100px" align="center" class="Label_user1">
                        <asp:Label ID="LabelOrderBooked" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 100px" align="center" class="Label_user1">
                        <asp:Label ID="LabelCashRecieptNo" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 100px" align="center" class="Label_user1">
                    <asp:Label ID="LabelEngineNo" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 100px" align="center" class="Label_user1">
                    <asp:Label ID="LabelChassisNo" runat="server" Text=""></asp:Label>
                </td>
                    <td style="width: 100px" align="center" class="Label_user1">
                        <asp:Label ID="LabelStatus" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                <td>
                    <asp:Button ID="btnGoOrderSearch" runat="server" Text="Go Order Search" 
                        onclick="btnGoOrderSearch_Click" /></td>
                </tr>
            </table>
    </fieldset>
    </table>
    </div>
                        </td>
                    </tr>
                     
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
