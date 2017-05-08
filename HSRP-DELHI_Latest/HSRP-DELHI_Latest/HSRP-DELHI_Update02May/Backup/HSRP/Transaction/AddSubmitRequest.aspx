<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AddSubmitRequest.aspx.cs" Inherits="HSRP.Transaction.AddSubmitRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function validate() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_dropDownListOrg").value == "--Select State--") {
                alert("Select State");
                document.getElementById("ctl00_ContentPlaceHolder1_dropDownListOrg").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_dropDownListClient").value == "--Select RTO--") {
                alert("Select RTO");
                document.getElementById("ctl00_ContentPlaceHolder1_dropDownListClient").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_txtRequestName").value == "") {
                alert("Please Fill Request Name");
                document.getElementById("ctl00_ContentPlaceHolder1_txtRequestName").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_DDLReqType").value == "--Select Request Type--") {
                alert("Please Select Request Type");
                document.getElementById("ctl00_ContentPlaceHolder1_DDLReqType").focus();
                return false;
            }
            if (document.getElementById("ctl00_ContentPlaceHolder1_DDLPriority").value == "--Select Priority--") {
                alert("Please Select Priority");
                document.getElementById("ctl00_ContentPlaceHolder1_DDLPriority").focus();
                return false;
            }
            else {
                return true;
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
                    Submit New Request
                </div>
            </legend>
            <br />
            <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td colspan="5">
                        <table style="background-color: #FFFFFF" width="85%" border="0" align="center" cellpadding="3"
                            cellspacing="1">
                              
                           
                            <tr>
                                <td colspan="4">
                                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                                        <tr valign="top">
                                            <td colspan="2" align="left" style="margin-left: 50px" class="form_text">
                                                <%--<b>AUTHORZIATION INFORMATION</b>--%>
                                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    &nbsp;<asp:Label ID="llbMSGError" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="186">
                                                Request Generated By:
                                            </td>
                                            <td width="170">
                                                <asp:Label ID="LabelCreatedID" ForeColor="Blue" runat="server" />
                                            </td>
                                            <td class="form_text" nowrap="nowrap" align="left" width="160">
                                                Record Date:
                                            </td>
                                            <td width="177">
                                                <asp:Label ID="LabelCreatedDateTime" ForeColor="Blue" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;
                                    
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="form_text">
                                    Select State: <span class="alert">* </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" AutoPostBack="true"
                                        ID="dropDownListOrg" class="form_textbox" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                               
                                <td align="left" class="form_text">
                                    Select Location:<span class="alert">* </span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" ID="dropDownListClient"
                                        class="form_textbox" runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
            <td class="form_text">Request Name <span class="alert">* </span> : </td>
            <td align="left" colspan="3" ><asp:TextBox Style="margin-left: 0px" Font-Size="Small" Width="180px" ID="txtRequestName" class="form_textbox" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="form_text">Request Type <span class="alert">* </span> : </td>
            <td colspan="3">
            <asp:Label ID="lblRequest" runat="server" Font-Bold="True" ForeColor="Black" ></asp:Label>
                &nbsp;</td>
        </tr>
            
        <tr>
            <td class="form_text">Upload File : </td>
            <td colspan="3" align="left">
               <asp:FileUpload Style="margin-left: 8px" ID="FileUpload1"  runat="server"  />
                <%--<a href=""  id="ShowFile" target="_self" visible="false" runat="server">Download</a>
                <asp:LinkButton ID="ShowFile" runat="server" visible="false" Text="Download" 
                    onclick="ShowFile_Click"> </asp:LinkButton>--%>
            </td>
        </tr>
        <tr>
            <td class="form_text">Priority <span class="alert">* </span> : </td>
            <td colspan="3">
                <asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" ID="DDLPriority" class="form_textbox" runat="server">
                    <asp:ListItem Text="--Select Priority--" Value="--Select Priority--"></asp:ListItem>
                    <asp:ListItem Text="High" Value="High"></asp:ListItem>
                    <asp:ListItem Text="Normal" Value="Normal"></asp:ListItem>
                    <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
            
        <tr>
            <td class="form_text" valign="top">Remarks : </td>
            <td colspan="3" align="left">
                <asp:TextBox Style="margin-left: 0px" Font-Size="Small" ID="textboxRemarks" class="form_textbox" runat="server" Columns="5" Rows="5" Height="85px" Width="300px"
                                                                    TabIndex="6" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                                    &nbsp;</td>
            <td>
                <br />
            </td>
        </tr>
                            <tr>
                                <td colspan="4" align="left">&nbsp;
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    <asp:Button ID="Button1" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Send Request" OnClick="Button1_Click" />
                                    <%--<asp:Button ID="ButImpData" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Generate Data For NIC" onclick="ButImpData_Click" />--%>
                                </td>
                            </tr>
                                <tr>
                                    <td colspan="4" align="left">&nbsp;
                                    </td>
                                </tr>
                            
                            <tr>
                                <td colspan="4" align="left">
                                &nbsp;&nbsp;
                                    
                                    </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">&nbsp;
                                    </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">&nbsp;
                                    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
