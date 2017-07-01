<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AddAPDealer.aspx.cs" Inherits="HSRP.Master.AddAPDealer" %>

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

            if (document.getElementById("<%=ddlDealermaster.ClientID%>").value == "--Select Dealer Name--") {
                alert("Please Select Dealer Name.");
                document.getElementById("<%=ddlDealermaster.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropdownRTOName.ClientID%>").value == "--Select RTO Location--") {
                alert("Select Rto Location");
                document.getElementById("<%=DropdownRTOName.ClientID%>").focus();
                return false;
            }

            //if (invalidChar(document.getElementById("txtDealerloginid"))) {
            //    alert("Please provide valid dealer name.");
            //    //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
            //    document.getElementById("txtDealerloginid").focus();
            //    return false;
            //}


            if (document.getElementById("<%=textBoxPersonName.ClientID%>").value == "") {
                alert("Please Provide Person Name.");
                document.getElementById("<%=textBoxPersonName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=txtEmailid.ClientID%>").value == "") {
                alert("Please Provide Email-Id.");
                document.getElementById("<%=txtEmailid.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxPersonName"))) {
                alert("Please provide valid person name.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxPersonName").focus();
                return false;
            }

            if (document.getElementById("<%=txtDealerloginid.ClientID%>").value == "") {
                alert("Please Provide Dealer Login Id.");
                document.getElementById("<%=txtDealerloginid.ClientID%>").focus();
                return false;
            }
            if (invalidChar11(document.getElementById("txtERPClientCode"))) {
                alert("Please provide valid ERP Client Code.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("txtERPClientCode").focus();
                return false;
            }
            <%--if (document.getElementById("<%=txtERPEmbossingCode.ClientID%>").value == "") {
                alert("Please Provide ERP Embossing Code.");
                document.getElementById("<%=txtERPEmbossingCode.ClientID%>").focus();
                return false;
            }--%>
            //if (invalidChar11(document.getElementById("txtERPEmbossingCode"))) {
            //    alert("Please provide valid ERP Embossing Code.");
            //    //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
            //    document.getElementById("txtERPEmbossingCode").focus();
            //    return false;
            //}

            if (document.getElementById("<%=textBoxMobileNo.ClientID%>").value == "") {
                alert("Please Provide Mobile No.");
                document.getElementById("<%=textBoxMobileNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=textBoxAddress.ClientID%>").value == "") {
                alert("Please Provide Address.");
                document.getElementById("<%=textBoxAddress.ClientID%>").focus();
                return false;
            }
            if (invalidChar11(document.getElementById("textBoxAddress"))) {
                alert("Please provide valid address.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxAddress").focus();
                return false;
            }
            if (document.getElementById("<%=textBoxCity.ClientID%>").value == "") {
                alert("Please Provide City.");
                document.getElementById("<%=textBoxCity.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("textBoxCity"))) {
                alert("Please provide valid city name.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxCity").focus();
                return false;
            }

            <%--if (document.getElementById("<%=textBoxDealerArea.ClientID%>").value == "") {
                alert("Please Provide Dealer Area.");
                document.getElementById("<%=textBoxDealerArea.ClientID%>").focus();
                return false;
            }--%>


            if (invalidChar(document.getElementById("textBoxDealerArea"))) {
                alert("Please provide valid dealer area.");
                //jAlert("Please Provide Valid Vehicle Chassis No.", "Order Form : Requirement");
                document.getElementById("textBoxDealerArea").focus();
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
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">State : <span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <asp:DropDownList Visible="true" CausesValidation="false" ID="DropDownListStateName" AutoPostBack="true"
                                                                runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" class="form_textbox12" OnSelectedIndexChanged="DropDownListStateName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <br />
                                                        </td>
                                                        <td style="width: 12px">&nbsp;
                                                        </td>
                                                        <td class="style5" nowrap="nowrap" style="padding-bottom: 10px;">
                                                            Dealer Name : <span class="alert">* </span>
                                                            <br />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlDealermaster" DataTextField="dealername" DataValueField="Dealerid" Width="180px"
                                                                runat="server" TabIndex="1" AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlDealermaster_SelectedIndexChanged">
                                                                <asp:ListItem>--Select Dealer--</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <br />
                                                        </td>
                                                    </tr>

                                                    <tr>

                                                        
                         
                                                             
                                                        <td style="width: 12px">&nbsp;</td>
                                                        
                                                    </tr>
                                                    <tr>

                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px;">RTO Location:<span class="alert">*</span>
                                                        </td>
                                                        <td class="style4">
                                                            <asp:DropDownList ID="DropdownRTOName" DataTextField="Rtolocationname" DataValueField="Rtolocationid" Width="180px"
                                                                runat="server" TabIndex="1" AutoPostBack="True">
                                                                <asp:ListItem>--Select RTO Location--</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 12px">&nbsp;</td>
                                                        <td class="style5" nowrap="nowrap" style="padding-bottom: 10px; ">
                                                            Email-Id  <span class="alert">* </span>
                                                            &nbsp;</td>
                                                        <td>
                                                            <asp:TextBox ID="txtEmailid" MaxLength="50" TabIndex="3" class="form_textbox12" runat="server"></asp:TextBox>
                                                            &nbsp;
                                                            <br />
                                                            &nbsp;&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmailid" ErrorMessage="Enter your full email address, including the '@'." ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>


                                                    <tr>
                                                        <td nowrap="nowrap" class="style7" style="padding-bottom: 10px">Contact Person Name: <span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <asp:TextBox class="form_textbox12" ID="textBoxPersonName" MaxLength="50" TabIndex="3"
                                                                runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td class="style5" nowrap="nowrap">Contact Person Mobile: <span class="alert">* </span>

                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" MaxLength="10" ID="textBoxMobileNo" TabIndex="4"
                                                                runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style7">Address : <span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <asp:TextBox class="form_textbox12" ID="textBoxAddress" TabIndex="5" runat="server"></asp:TextBox>
                                                            <br />
                                                        </td>
                                                        <td></td>
                                                        <td class="style5">City <span class="alert">* </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="textBoxCity" TabIndex="6"
                                                                runat="server"></asp:TextBox><br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style7">Dealer Username :<span class="alert">* </span>
                                                        </td>
                                                        <td class="style4">
                                                            <asp:TextBox class="form_textbox12" MaxLength="30" ID="txtDealerloginid" TabIndex="7" runat="server"></asp:TextBox>

                                                            <br />
                                                        </td>
                                                        <td class="style8">&nbsp;
                                                        </td>
                                                        <%--<td class="style5" nowrap="nowrap">
                                                            Area Of Dealer: <span class="alert">* </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox class="form_textbox12" MaxLength="100" ID="textBoxDealerArea" runat="server"
                                                                TabIndex="8"></asp:TextBox><br />
                                                        </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:CheckBox ID="radioButtonShowRoom" runat="server"
                                                                Text="Fixation At Dealer ShowRoom : " TextAlign="Left"
                                                                OnCheckedChanged="radioButtonShowRoom_CheckedChanged" Visible="false" AutoPostBack="true" />


                                                            <%--    <asp:RadioButton ID="radioButtonShowRoom" runat="server" TabIndex="12" GroupName="kk"
                                                                Text="Fixation At Dealer ShowRoom : " TextAlign="Left" /><br />
                                                            <asp:RadioButton ID="radioButtonCenter" runat="server" GroupName="kk" TabIndex="13"
                                                                Text="Fixation At Our Center : " TextAlign="Left" />--%>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="radioButtonCenter" runat="server"
                                                                Text="Fixation At Our Center : " Visible="false" TextAlign="Left" AutoPostBack="true"
                                                                OnCheckedChanged="radioButtonCenter_CheckedChanged" />
                                                        </td>
                                                    </tr>

                                                    <tr>

                                                        <td colspan="2">
                                                            <table id="radioButtonShowRoom1" runat="server" visible="false">
                                                                <tr>
                                                                    <td>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td nowrap="nowrap">
                                                                                    <asp:CheckBox Text="Deals In Two wheeler : " TabIndex="9" TextAlign="Left"
                                                                                        ID="checkBoxTwoWheeler1" AutoPostBack="true"
                                                                                        runat="server" OnCheckedChanged="checkBoxTwoWheeler1_CheckedChanged" Visible="false" /></td>
                                                                                <td></td>
                                                                                <td>
                                                                                    <table style="width: 100%; background-color: #F6CECE" id="TwoWheeler1" runat="server" visible="false">
                                                                                        <tr>
                                                                                            <td nowrap="nowrap">Two Wheeler Rate</td>
                                                                                            <td nowrap="nowrap">
                                                                                                <asp:TextBox ID="txtTwoWheelerRate1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>


                                                                        <br />
                                                                        <asp:CheckBox Text="Deals In Four wheeler : " TabIndex="10"
                                                                            ID="checkBoxFourWheeler1" runat="server"
                                                                            TextAlign="Left" AutoPostBack="true" OnCheckedChanged="checkBoxFourWheeler1_CheckedChanged" />

                                                                        <table style="width: 100%; background-color: #CEF6EC" id="FourWheeler1" runat="server" visible="false">
                                                                            <tr>
                                                                                <td colspan="2"><b>Non Transpost Rate LMV</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap="nowrap">Up To 10 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVUpTo10Lac1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">10 To 25 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMV10To25Lac1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap="nowrap">25 To 50 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMV25To50Lac1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">More Than 50 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVMoreThan50Lac1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><b>Non Transpost Rate LMV(Class)</b></td>

                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap="nowrap">Up To 10 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClassUpTo10Lac1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">10 To 25 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClass10To25Lac1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap="nowrap">25 To 50 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClass25To50Lac1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">More than 50 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClassMoreThan50Lac1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <asp:CheckBox Text="Deals In Commercial Vehicle : " TabIndex="11"
                                                                            ID="checkBoxCommercialVehicle1" AutoPostBack="true"
                                                                            TextAlign="Left" runat="server" OnCheckedChanged="checkBoxCommercialVehicle1_CheckedChanged" />
                                                                        <table style="background: yellow" width="100%" id="Commercialvehicle1" runat="server" visible="false">
                                                                            <tr>
                                                                                <td nowrap="nowrap"><b>Commercial Vehicle Price </b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>LMV</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMV1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">LMV (Class)</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClass1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Auto</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAuto1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td>LCV</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLCV1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>MCV</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtMVC1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td>HCV</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtHCV1" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>

                                                        <td valign="top" colspan="3" class="Label_user">
                                                            <table id="radioButtonCenter1" runat="server" visible="false">
                                                                <tr>
                                                                    <td>

                                                                        <table style="width: 100%; padding-top: 11px;">
                                                                            <tr>
                                                                                <td nowrap="nowrap">
                                                                                    <asp:CheckBox Text="Deals In Two wheeler : " TabIndex="9" TextAlign="Left"
                                                                                        ID="checkBoxTwoWheeler" AutoPostBack="true"
                                                                                        runat="server" OnCheckedChanged="checkBoxTwoWheeler_CheckedChanged" /></td>
                                                                                <td></td>
                                                                                <td nowrap="nowrap">
                                                                                    <table style="width: 100%; background-color: #F6CECE" id="TwoWheeler" runat="server" visible="false">
                                                                                        <tr>
                                                                                            <td nowrap="nowrap">Two Wheeler Rate</td>
                                                                                            <td nowrap="nowrap">
                                                                                                <asp:TextBox ID="txtTwoWheelerRate" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>

                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>


                                                                        <br />
                                                                        <asp:CheckBox Text="Deals In Four wheeler : " TabIndex="10"
                                                                            ID="checkBoxFourWheeler" runat="server"
                                                                            TextAlign="Left" AutoPostBack="true"
                                                                            OnCheckedChanged="checkBoxFourWheeler_CheckedChanged" />

                                                                        <table style="width: 100%; background-color: #CEF6EC" id="FourWheeler" runat="server" visible="false">
                                                                            <tr>
                                                                                <td colspan="2"><b>Non Transpost Rate LMV</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap="nowrap">Up To 10 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVUpTo10Lac" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">10 To 25 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMV10To25Lac" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap="nowrap">25 To 50 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMV25To50Lac" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">More Than 50 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVMoreThan50Lac" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><b>Non Transpost Rate LMV(Class)</b></td>

                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap="nowrap">Up To 10 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClassUpTo10Lac" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">10 To 25 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClass10To25Lac" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap="nowrap">25 To 50 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClass25To50Lac" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">More than 50 Lac</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClassMoreThan50Lac" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <asp:CheckBox Text="Deals In Commercial Vehicle : " TabIndex="11" ID="checkBoxCommercialVehicle" AutoPostBack="true"
                                                                            TextAlign="Left" runat="server"
                                                                            OnCheckedChanged="checkBoxCommercialVehicle_CheckedChanged" />
                                                                        <table style="background: yellow" width="100%" id="Commercialvehicle" runat="server" visible="false">
                                                                            <tr>
                                                                                <td nowrap="nowrap"><b>Commercial Vehicle Price </b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>LMV</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMV" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td nowrap="nowrap">LMV (Class)</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLMVClass" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Auto</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAuto" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td>LCV</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtLCV" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>MCV</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtMVC" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                                <td>HCV</td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtHCV" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox></td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                        <td class="Label_user" valign="top"></td>
                                                    </tr>




                                                    <tr>
                                                        <td colspan="5" class="Label_user">
                                                            <asp:CheckBox ID="checkBoxStatus" runat="server" Text="Active Status : "
                                                                TabIndex="14" Visible="false"
                                                                TextAlign="Left" Checked="True" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" nowrap="nowrap" align="right" style="margin-right: 200px">
                                                            <%--<asp:Button ID="buttonUpdate" runat="server" class="button" TabIndex="15" Visible="false"
                                                                OnClientClick=" javascript:return vali();" Text="Update" OnClick="buttonUpdate_Click" />--%>
                                                            &nbsp;
                                                            <asp:Button ID="buttonSave" runat="server" TabIndex="16" class="button" Text="Save"
                                                                OnClientClick=" javascript:return vali();" Visible="false" OnClick="buttonUpdate_Click" />&nbsp;
                                                            <%--<input type="button" onclick="javascript: parent.googlewin.close();" name="buttonClose"
                                                                id="buttonClose" value="Close" class="button" />--%>
                                                            &nbsp;
                                                            <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
                                                            <%--<input type="reset" class="button" value="Reset" />--%>                                                            
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
