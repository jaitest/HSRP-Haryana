<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="CreateTSDealer.aspx.cs" Inherits="HSRP.Master.CreateTSDealer" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/User.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../javascript/common.js" type="text/javascript"></script>
    <%--<script src="http://code.jquery.com/jquery-1.4.3.min.js" type="text/javascript"></script>--%>

    <script type="text/javascript">
        function invalidChar(_objData1) {
            // debugger;
            var iChars = '!$%^*+=[]{}|"<>?';

            for (var i = 0; i < _objData1.value.length; i++) {
                if(iChars.indexOf(_objData1.value.charAt(i)) != -1) {
                    return true;
                }
            }
        }
    </script>
    <script type="text/javascript" language="javascript">

        function vali()
        {
            if(document.getElementById("<%=ddldealerList.ClientID%>").value == "--Select Dealer Name--") {
                alert("Select Dealer Name");
                document.getElementById("<%=ddldealerList.ClientID%>").focus();
                return false;
            }
            if(document.getElementById("<%=txtCompanyNamewithAddress.ClientID%>").value == "") {
                alert("Please Company Name with Address.");
                document.getElementById("<%=txtCompanyNamewithAddress.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtCompanyNamewithAddress"))) {
                alert("Please provide Company Name with Address.");
                document.getElementById("txtCompanyNamewithAddress").focus();
                return false;
            }
            if(document.getElementById("<%=txtContactPersonName.ClientID%>").value == "") {
                alert("Please Contact Person Name.");
                document.getElementById("<%=txtContactPersonName.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtContactPersonName"))) {
                alert("Please provide valid Contact Person Name.");
                document.getElementById("txtContactPersonName").focus();
                return false;
            }           

            if(document.getElementById("<%=txtDesignation.ClientID%>").value == "") {
                alert("Please Provide Designation.");
                document.getElementById("<%=txtDesignation.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtDesignation"))) {
                alert("Please provide valid Designation.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtDesignation").focus();
                return false;
            }
            if(document.getElementById("<%=txtContactPersonMobileNo.ClientID%>").value == "") {
                alert("Please Provide Contact Person Mobile No.");
                document.getElementById("<%=txtContactPersonMobileNo.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtContactPersonMobileNo"))) {
                alert("Please provide valid Contact Person Mobile No.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtContactPersonMobileNo").focus();
                return false;
            }

            if(document.getElementById("<%=txtRegisteredAddress.ClientID%>").value == "") {
                alert("Please Provide Registered Address.");
                document.getElementById("<%=txtRegisteredAddress.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtRegisteredAddress"))) {
                alert("Please provide valid Registered Address.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtRegisteredAddress").focus();
                return false;
            }
            if(document.getElementById("<%=txtContactPersonMobileNo.ClientID%>").value == "") {
                alert("Please Provide Contact Person Mobile No.");
                document.getElementById("<%=txtContactPersonMobileNo.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtContactPersonMobileNo"))) {
                alert("Please provide valid Contact Person Mobile No.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtContactPersonMobileNo").focus();
                return false;
            }
            if(document.getElementById("<%=txtDistrict.ClientID%>").value == "") {
                alert("Please Provide District.");
                document.getElementById("<%=txtDistrict.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtDistrict"))) {
                alert("Please provide valid District.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtDistrict").focus();
                return false;
            }
            if(document.getElementById("<%=txtState.ClientID%>").value == "") {
                alert("Please Provide State.");
                document.getElementById("<%=txtState.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtState"))) {
                alert("Please provide valid State.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtState").focus();
                return false;
            }
            if(document.getElementById("<%=txtPinCode.ClientID%>").value == "") {
                alert("Please Provide PinCode.");
                document.getElementById("<%=txtPinCode.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtPinCode"))) {
                alert("Please provide valid PinCode.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtPinCode").focus();
                return false;
            }
            if(document.getElementById("<%=txtTelephoneNumbers.ClientID%>").value == "") {
                alert("Please Provide Telephone Numbers.");
                document.getElementById("<%=txtTelephoneNumbers.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtTelephoneNumbers"))) {
                alert("Please provide valid Telephone Numbers.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtTelephoneNumbers").focus();
                return false;
            }
            if(document.getElementById("<%=txtFaxNos.ClientID%>").value == "") {
                alert("Please Provide Fax Nos.");
                document.getElementById("<%=txtFaxNos.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtFaxNos"))) {
                alert("Please provide valid Fax Nos.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtFaxNos").focus();
                return false;
            }
            if(document.getElementById("<%=txtCellPhoneNo.ClientID%>").value == "") {
                alert("Please Provide Cell Phone No.");
                document.getElementById("<%=txtCellPhoneNo.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtCellPhoneNo"))) {
                alert("Please provide valid Cell Phone No.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtCellPhoneNo").focus();
                return false;
            }
            if(document.getElementById("<%=txtemail.ClientID%>").value == "") {
                alert("Please Provide email.");
                document.getElementById("<%=txtemail.ClientID%>").focus();
                return false;
            }

            if(document.getElementById("<%=DropdownRTOName.ClientID%>").value == "--Select Dealer Name--") {
                alert("Select Rto Name");
                document.getElementById("<%=DropdownRTOName.ClientID%>").focus();
                return false;
            }
            if(document.getElementById("<%=txtNameofOEM.ClientID%>").value == "") {
                alert("Please Provide Name of OEM.");
                document.getElementById("<%=txtNameofOEM.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtNameofOEM"))) {
                alert("Please provide valid Name of OEM.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtNameofOEM").focus();
                return false;
            }
            if(document.getElementById("<%=txtIncomeTaxPANNo.ClientID%>").value == "") {
                alert("Please Provide Income Tax PAN No.");
                document.getElementById("<%=txtIncomeTaxPANNo.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtIncomeTaxPANNo"))) {
                alert("Please provide valid Income Tax PAN No.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtIncomeTaxPANNo").focus();
                return false;
            }
            if(document.getElementById("<%=txtCSTNo.ClientID%>").value == "") {
                alert("Please Provide CST No.");
                document.getElementById("<%=txtCSTNo.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtCSTNo"))) {
                alert("Please provide valid CST No.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtCSTNo").focus();
                return false;
            }
            if(document.getElementById("<%=txtVATTINNo.ClientID%>").value == "") {
                alert("Please Provide VAT TIN No.");
                document.getElementById("<%=txtVATTINNo.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtVATTINNo"))) {
                alert("Please provide valid VAT TIN No.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtVATTINNo").focus();
                return false;
            }
            if(document.getElementById("<%=txtBusinessentity.ClientID%>").value == "") {
                alert("Please Provide Business entity.");
                document.getElementById("<%=txtBusinessentity.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtBusinessentity"))) {
                alert("Please provide valid txtBusinessentity.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtBusinessentity").focus();
                return false;
            }
            if(document.getElementById("<%=txtFulldetails.ClientID%>").value == "") {
                alert("Please Provide Full details.");
                document.getElementById("<%=txtFulldetails.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtFulldetails"))) {
                alert("Please provide valid Full details.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtFulldetails").focus();
                return false;
            }
            if(document.getElementById("<%=txtTypeofDealer.ClientID%>").value == "") {
                alert("Please Provide Type of Dealer.");
                document.getElementById("<%=txtTypeofDealer.ClientID%>").focus();
                return false;
            }
            if(invalidChar(document.getElementById("txtTypeofDealer"))) {
                alert("Please provide valid Type of Dealer.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtTypeofDealer").focus();
                return false;
            }
            
        }

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if(charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
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
    </style>

    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; background: yellow; height: 22px; width: 119px; color: Black">
                    Dealer Profile
                </div>
            </legend>
            <table style="background-color: #F0F0F0" width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                    <td>
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table border="0" align="center" cellpadding="3" cellspacing="1" style="height: 385px; width: 100%">
                                        <tr>
                                            <td style="margin-left: 10px; font-size: medium; color: Black">
                                                <table border="0" align="center" cellpadding="3" cellspacing="3" style="height: 348px; width: 100%">
                                                    <tr>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">Select Exist Dealer : <span class="alert">* </span></td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                            <asp:DropDownList ID="ddldealerList" style="height:20px; width:160px; padding-left:2px; top: -2px; position: relative;  left: 8px;" TabIndex="1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldealerList_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                            Company Name with Address : <span class="alert">* </span>
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                            <asp:TextBox class="form_textbox12" ID="txtCompanyNamewithAddress" MaxLength="50" ReadOnly="true" TabIndex="2" runat="server">
                                                            </asp:TextBox>                                                            
                                                        </td>   
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">Contact Person Name: <span class="alert">* </span>
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" ID="txtContactPersonName" MaxLength="50" TabIndex="3"   runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                            Designation : <span class="alert">* </span>
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                            <asp:TextBox class="form_textbox12" ID="txtDesignation" MaxLength="50" TabIndex="4" runat="server">
                                                            </asp:TextBox>                                                            
                                                        </td>   
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">Contact Person Mobile No.: <span class="alert">* </span>
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" ID="txtContactPersonMobileNo" MaxLength="10" TabIndex="5" onkeypress="return isNumber(event)" runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                
                                                    <tr>
                                                    <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                        Registered Address: <span class="alert">* </span>
                                                        <br />
                                                        District: <span class="alert">* </span>
                                                        <br />
                                                        State: <span class="alert">* </span>
                                                        <br />
                                                        Pin Code: <span class="alert">* </span>
                                                    </td>         
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" ID="txtRegisteredAddress" TextMode="MultiLine" Height="30px" MaxLength="50" TabIndex="6"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                            <asp:TextBox class="form_textbox12" ID="txtDistrict" MaxLength="50" TabIndex="7"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                            <asp:TextBox class="form_textbox12" ID="txtState" MaxLength="50" TabIndex="8"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                            <asp:TextBox class="form_textbox12" ID="txtPinCode" MaxLength="50" TabIndex="9" onkeypress="return isNumber(event)"  runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>         
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                        Telephone Numbers: <span class="alert">* </span>
                                                        <br />
                                                        Fax Nos.: <span class="alert">* </span>
                                                        <br />
                                                        Cell Phone No: <span class="alert">* </span>
                                                        <br />
                                                        E-Mail ID: <span class="alert">* </span>
                                                    </td>         
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" ID="txtTelephoneNumbers" onkeypress="return isNumber(event)" MaxLength="20" TabIndex="10"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                            <asp:TextBox class="form_textbox12" ID="txtFaxNos" onkeypress="return isNumber(event)" MaxLength="20" TabIndex="11"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                            <asp:TextBox class="form_textbox12" ID="txtCellPhoneNo" onkeypress="return isNumber(event)" MaxLength="10" TabIndex="12"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                            <asp:TextBox class="form_textbox12" ID="txtemail" MaxLength="50" TabIndex="13"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>                                                           
                                                    </tr>
                                                    <tr>

                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                            RTO Location & Code:<span class="alert">*</span>
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                            <asp:DropDownList ID="DropdownRTOName" DataTextField="Rtolocationname" DataValueField="Rtolocationid" style="height:20px; width:160px; padding-left:2px; top: -2px; position: relative;  left: 8px;"  runat="server" TabIndex="14" AutoPostBack="True">
                                                                <asp:ListItem>--Select RTO Location--</asp:ListItem>
                                                            </asp:DropDownList> <span class="alert">* </span>
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                            Name of OEM <span class="alert">* </span></td>     
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">
                                                        <asp:TextBox class="form_textbox12" ID="txtNameofOEM" MaxLength="50" TabIndex="15"
                                                                runat="server"></asp:TextBox></td>                                                            
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            Income Tax PAN No.: <span class="alert">* </span>
                                                            <br />                                                            
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" ID="txtIncomeTaxPANNo" MaxLength="50" TabIndex="16"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>                                                        
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            CST No: <span class="alert">* </span>
                                                            <br />
                                                            VAT TIN No. <span class="alert">* </span>
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" MaxLength="10" ID="txtCSTNo" TabIndex="17"
                                                                runat="server" onkeypress="return isNumber(event)"></asp:TextBox>
                                                            <br />
                                                            <asp:TextBox class="form_textbox12" MaxLength="10" ID="txtVATTINNo" TabIndex="18"
                                                                runat="server" onkeypress="return isNumber(event)"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            Type of Business entity: <span class="alert">* </span><br />	Sole Proprietorship Private Ltd. Co.,<br />	Partnership Public Ltd. Co.,<br />	Other (please specify)
                                                         </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" ID="txtBusinessentity" TextMode="MultiLine" Height="70px" TabIndex="19" runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            Full details of: <span class="alert">* </span><br />	Proprietor/Partners/Directors<br />	Name <br />	Address <br />	Contact No<br />
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" MaxLength="30" TextMode="MultiLine" Height="70px" ID="txtFulldetails" TabIndex="20"
                                                                runat="server"></asp:TextBox><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                           Type of Dealer (2W, 3W, 4W) <span class="alert">* </span>
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                            <asp:TextBox class="form_textbox12" MaxLength="30"  ID="txtTypeofDealer" TabIndex="21"
                                                                runat="server"></asp:TextBox><br />
                                                        </td>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                        </td>  
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">
                                                        </td>                                                               
                                                    </tr>                                                   
                                                    <tr>
                                                        <td colspan="5" nowrap="nowrap" align="right" style="margin-right: 200px">
                                                            &nbsp;
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="22" class="button" Text="Save" OnClientClick=" javascript:return vali();" OnClick="buttonSave_Click" Height="21px"/>
                                                            &nbsp;
                                                            &nbsp;
                                                            <asp:Button ID="btnReset" runat="server" TabIndex="23" Text="Reset" OnClick="btnReset_Click"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="alert">* Fields are mandatory.
                                </td>
                            </tr>
                            <tr>
                                <td class="FieldText">
                                    <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblUserNamePassword" runat="server" ForeColor="Red" Text="" Font-Size="18px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </fieldset>
    </div>
</asp:Content>
