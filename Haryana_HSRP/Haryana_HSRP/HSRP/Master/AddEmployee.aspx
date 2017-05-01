<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AddEmployee.aspx.cs" Inherits="HSRP.Master.AddEmployee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript">

        function validate() {
            if (document.getElementById("<%=ddlUserAccount.ClientID%>").value == "--Select username--") {
                alert("Please select username ");
                document.getElementById("<%=ddlUserAccount.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txtempname.ClientID%>").value == "") {
                alert("Please provide employee name ");
                document.getElementById("<%=txtempname.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtEmail.ClientID%>").value == "") {
                alert("Please provide email-id ");
                document.getElementById("<%=txtEmail.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtmobileno.ClientID%>").value == "") {
                alert("Please provide mobile no ");
                document.getElementById("<%=txtmobileno.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtempcode.ClientID%>").value == "") {
                alert("Please provide employee code ");
                document.getElementById("<%=txtempcode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDesignation.ClientID%>").value == "") {
                alert("Please provide designation");
                document.getElementById("<%=txtDesignation.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlhead.ClientID%>").value == "--Select State Head--") {
                alert("Please select State Head ");
                document.getElementById("<%=ddlhead.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddlResponsibility.ClientID%>").value == "--Select Role--") {
                alert("Please select Role ");
                document.getElementById("<%=ddlResponsibility.ClientID%>").focus();
                return false;
            }

        }
    </script>
    <style type="text/css">
        .style4 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 210px;
            padding-left: 20px;
        }
    </style>

    <div style="width: 100%;">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Employee Master
                </div>
            </legend>
            <br />
            <br />
            <div style="width: 100%; margin: 0px auto 0px auto">

                <table border="0" align="right" style="height: 348px; width: 85%;">

                    <tr>
                        <td class="style4">User Name <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUserAccount" runat="server" Style="margin-left: 8px" TabIndex="1" Width="165px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Employee Name <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtempname" class="form_textbox12" runat="server" MaxLength="30" TabIndex="2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Email-Id <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" class="form_textbox12" TabIndex="3" MaxLength="50"></asp:TextBox>

                        </td>
                    </tr>

                    <tr>
                        <td class="style4">Mobile No<span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmobileno" runat="server" class="form_textbox12" MaxLength="10" TabIndex="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Employee Code<span class="alert">* </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtempcode" runat="server" class="form_textbox12" MaxLength="7" TabIndex="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Designation <span class="alert">* </span>
                        </td>
                        <td>

                            <asp:TextBox ID="txtDesignation" runat="server" class="form_textbox12" MaxLength="30" TabIndex="6"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td class="style4">State Head <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlhead" runat="server" Style="margin-left: 8px" TabIndex="7" Width="165px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">Role <span class="alert">* </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlResponsibility" runat="server" Style="margin-left: 8px" TabIndex="8" Width="165px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label><br />
                            <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>

                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="button1" runat="server" TabIndex="9" class="button" Text="Save" OnClientClick=" return validate()" OnClick="ButtonSubmit_Click" />
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="btnReset" runat="server" class="button" Text="Reset" TabIndex="10" OnClick="btnReset_Click" />
                        </td>
                    </tr>

                </table>
                <div style="clear: both"></div>

            </div>


        </fieldset>
    </div>

</asp:Content>
