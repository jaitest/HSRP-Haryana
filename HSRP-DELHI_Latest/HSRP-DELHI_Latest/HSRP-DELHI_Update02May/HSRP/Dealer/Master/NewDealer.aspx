<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewDealer.aspx.cs" Inherits="HSRP.Dealer.Master.NewDealer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/User.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <%--<script src="http://code.jquery.com/jquery-1.4.3.min.js" type="text/javascript"></script>--%>

    <script type="text/javascript">
        function invalidChar11(_objData1) {
            // debugger;
            var iChars = '!$%^*+=[]{}|"<>?';

            for (var i = 0; i < _objData1.value.length; i++) {
                if (iChars.indexOf(_objData1.value.charAt(i)) != -1) {
                    return true;
                }
            }
        }
    </script>
    <script type="text/javascript" language="javascript">

        function vali() {

            if (document.getElementById("<%=txtDealerName.ClientID%>").value == "") {
                alert("Please Provide Dealer Name.");
                document.getElementById("<%=txtDealerName.ClientID%>").focus();
                return false;
            }            

            if (document.getElementById("<%=txtArea.ClientID%>").value == "") {
                alert("Please Provide Person Name.");
                document.getElementById("<%=txtArea.ClientID%>").focus();
                return false;
            }
            

            if (document.getElementById("<%=txtMemberStatus.ClientID%>").value == "") {
                alert("Please Provide Mobile No.");
                document.getElementById("<%=txtMemberStatus.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtStatus.ClientID%>").value == "") {
                alert("Please Provide Address.");
                document.getElementById("<%=txtStatus.ClientID%>").focus();
                return false;
            }
           
            <%--if (document.getElementById("<%=txtPriortyPeriod.ClientID%>").value == "") {
                alert("Please Provide City.");
                document.getElementById("<%=txtPriortyPeriod.ClientID%>").focus();
                return false;
            }  --%>         

           <%-- if (document.getElementById("<%=txtRemarks.ClientID%>").value == "") {
                alert("Please Provide Dealer Area.");
                document.getElementById("<%=txtRemarks.ClientID%>").focus();
                return false;
            }--%>
        }

        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <style type="text/css">
        .style4 {
            width: 428px;
        }

        .style5 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 103px;
            padding-left: 20px;
        }

        .style6 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 184px;
            padding-left: 20px;
        }

        .style7 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 174px;
            padding-left: 20px;
        }
        .auto-style1 {
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            font-size: 12px;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            color: #ff0000;
            text-decoration: none;
            height: 14px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div style="margin: 20px;" align="left">
            <fieldset>
                <legend>
                    <div style="margin-left: 10px; font-size: medium; background: yellow; height: 22px; width: 119px; color: Black">
                        Dealer Profile
                    </div>
                </legend>
                <table style="background-color: #FFFFFF; height: 216px; width: 72%;" border="0" align="center" cellpadding="3" cellspacing="1">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="style7" style="width: 3%;">Dealer Name : <span class="alert">* </span>
                                    </td>
                                    <td style="width: 20%">
                                        <asp:TextBox MaxLength="50" ID="txtDealerName" runat="server" Text="" Width="257px"></asp:TextBox>
                                    </td>
                                    <td class="style5" style="width: 15%;">Area Of Dealer:<span class="alert">* </span>
                                    </td>
                                    <td class="style6">
                                        <asp:TextBox MaxLength="50" ID="txtArea" runat="server" Text="" Width="257px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap" class="style7" style="width: 3%;">Member Status : <span class="alert">* </span>
                                    </td>
                                    <td class="style4" style="width: 20%">
                                        <asp:TextBox MaxLength="50" ID="txtMemberStatus" runat="server" Text="" Width="257px"></asp:TextBox>
                                    </td>
                                    <td class="style5" nowrap="nowrap" style="width: 15%">Status<span class="alert">* </span>
                                    </td>
                                    <td class="style5" nowrap="nowrap">
                                        <asp:TextBox MaxLength="50" ID="txtStatus" runat="server" Text="" Width="257px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap" class="style7" style="width: 3%;">Priorty period : <span class="alert"></span>
                                    </td>
                                    <td class="style4" style="width: 20%">
                                        <asp:TextBox MaxLength="50" ID="txtPriortyPeriod" runat="server" Text="" Width="257px"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%">
                                    </td>
                                    <td>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label_user" style="width: 3%">
                                        <asp:CheckBox ID="chkActiveStatus" runat="server" Text="Active Status : "
                                            TabIndex="14" 
                                            TextAlign="Left" Checked="True" />
                                    </td>
                                    <td style="width: 20%"></td>
                                    <td class="style5" nowrap="nowrap" style="width: 15%">Remarks<span class="alert"> </span>
                                    </td>
                                    <td class="style5" nowrap="nowrap">
                                        <asp:TextBox MaxLength="100" ID="txtRemarks" runat="server"
                                            TabIndex="8" Width="255px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: center">
                                        <asp:Button ID="buttonSave" runat="server" TabIndex="16"  Text="Save"
                                            OnClientClick=" javascript:return vali();"  OnClick="buttonSave_Click" /></td>
                                </tr>
                            </table>
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="auto-style1">* Fields are mandatory.
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FieldText">
                                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
            </fieldset>
        </div>
    </form>
</body>
</html>
