﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"    CodeBehind="ImpExpData.aspx.cs" Inherits="HSRP.Transaction.ImpExpData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function validate() {
             
            if (document.getElementById("<%=dropDownListOrg.ClientID%>").value == "--Select State--") {
                alert("Select State");
                document.getElementById("<%=dropDownListOrg.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropDownListClient.ClientID%>").value == "--Select RTO--") {
                alert("Select RTO");
                document.getElementById("<%=dropDownListClient.ClientID%>").focus();
                return false;
            }              

        }
    </script>
  
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>
    <style type="text/css">
        .modalPopup
        {
            background-color: #FFFFFF;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }
    </style>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="../images/ajax-loader.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />

    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                    Import/Export Data
                </div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td colspan="5">
                        <table style="background-color: #FFFFFF" width="85%" border="0" align="center" cellpadding="3"
                            cellspacing="1">
                              
                            <tr runat="server">
                                    <td width="100%" colspan="7">
                                    <div width="800px">
                                    <table>
                                    <tr>
                                    <td valign="middle">
                                    <asp:Label ID="dataLabellbl" class="headingmain" runat="server"  >Allowed RTO's :</asp:Label> 
                                            <asp:Label   ID="lblRTOCode" ForeColor="OrangeRed" runat="server"  >Allowed RTO's :</asp:Label> 
                                    
                                    </td>
                                    </tr>
                                    </table>
                                    </div>
                                          </td>
                                </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                        <tr valign="top">
                                            <td colspan="2" align="left" style="margin-left: 50px" width="258px" class="form_text">
                                                <%--<b>AUTHORZIATION INFORMATION</b>--%>
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="150px">
                                                File Generated By:
                                            </td>
                                            <td width="170px">
                                                <asp:Label ID="LabelCreatedID" ForeColor="Blue" runat="server" />
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="160px">
                                                Record Date:
                                            </td>
                                            <td>
                                                <asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="form_text">
                                    Select State: <span class="alert">* </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" Width="170px" AutoPostBack="true"
                                        ID="dropDownListOrg" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                               
                                <td align="left" class="form_text">
                                    Select Location:<span class="alert">* </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList Style="margin-left: 8px" Font-Size="Small" Width="170px" ID="dropDownListClient" AutoPostBack="true"
                                        runat="server" DataTextField="RTOLocationName" 
                                        DataValueField="RTOLocationID" onselectedindexchanged="dropDownListClient_SelectedIndexChanged" 
                                        >
                                    </asp:DropDownList>
                                </td>
                            </tr>
                           <tr id="divLocationFileName" runat="server">
                                <td align="left" class="form_text"> 
                                    
                                    <asp:Label ID="LlbSelectFilename" Visible="true" runat="server" Text="Select File Name:">
                                    </asp:Label>
                                    
                                </td>
                                <td colspan="3" align="left" class="form_text"> 
                                    <asp:DropDownList Style="margin-left: 8px" Visible="true" Font-Size="Small"  Width="170px" ID="dropDownListFilenameforlocation"
                                        runat="server" DataTextField="filename" 
                                        DataValueField="filename">
                                    </asp:DropDownList>
                                </td>
                            </tr> 
                            <tr>
                              <td align="left" class="form_text">
                                    Select File
                                </td>
                                <td colspan="3">
                                <asp:FileUpload ID="FileUpload1" runat="server" /> &nbsp;&nbsp;
                                    <asp:Button ID="Button1" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Upload Data" OnClick="Button1_Click" />
                                    <%--<asp:LinkButton ID="GenerateAlertReport" Text="Go" runat="server" OnClick="GenerateAlertReport_Click"
                                                    CssClass="button"></asp:LinkButton>&nbsp;&nbsp;--%>
                                &nbsp;
                                    <asp:Button ID="ButImpData" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Generate Data For NIC" onclick="ButImpData_Click" />
                                </td>
                               
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    &nbsp;
                                </td>
                            </tr>
                                <tr>
                                    <td colspan="4" align="left">
                                        <asp:Label ID="llbFileName" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    &nbsp;</td>
                                </tr>
                            
                            <tr>
                                <td colspan="4" align="left">
                                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                &nbsp;&nbsp;
                                    
                                    </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
