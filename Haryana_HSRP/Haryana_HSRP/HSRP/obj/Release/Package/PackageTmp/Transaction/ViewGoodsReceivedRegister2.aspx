﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewGoodsReceivedRegister2.aspx.cs" Inherits="HSRP.Transaction.ViewGoodsReceivedRegister" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">
        function editpage(i) { //Define arbitrary function to run desired DHTML Window widget codes
            //  alert(i);
            googlewin = dhtmlwindow.open("googlebox", "iframe", "GoodsReceivedRegister.aspx?Mode=Edit&UserID=" + i, "Update Goods Received Register Details", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewGoodsReceivedRegister.aspx";
                return true;
            }
        }

        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "GoodsReceivedRegister.aspx?Mode=New", "Add New Goods Received Register", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'ViewGoodsReceivedRegister.aspx';
                return true;
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Goods Received Register?")) {

                return true;
            }
            else {
                return false;
            }

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
                                        <span class="headingmain">Goods Received Register Master </span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                <tr>
                                    <td valign="middle" class="form_text" nowrap="nowrap">
                                        <asp:Label Text="HSRP State:" Visible="true" runat="server" ID="labelOrganization" />
                                    </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" CausesValidation="false" ID="dropDownListOrg"
                                            runat="server" DataTextField="OrgName" DataValueField="OrgID">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateClient" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="RTO Location:" Visible="false" runat="server" ID="labelClient" />&nbsp;&nbsp;
                                                <asp:DropDownList AutoPostBack="true" Visible="false" ID="dropDownListClient" CausesValidation="false"
                                                    runat="server" DataTextField="ClientName" DataValueField="ClientID" >
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="dropDownListOrg" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListClient" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Goods Received Register</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                    </td>
                                </tr>
                             
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
