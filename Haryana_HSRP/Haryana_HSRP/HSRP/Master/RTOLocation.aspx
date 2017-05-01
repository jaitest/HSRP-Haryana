<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RTOLocation.aspx.cs" Inherits="HSRP.Master.RTOLocation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function checkEmail() {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(textboxEmailId.value)) { 
                return true;
            }
            return false;
        }


        function validate() {

            if (document.getElementById("<%=dropdownListStateName .ClientID%>").value == "--Select State--") {
                alert("Select State Name ");
                document.getElementById("<%=dropdownListStateName .ClientID%>").focus();
                return false;
            }
             
            if (document.getElementById("<%=textboxRtoLocationName.ClientID%>").value == "") {
                alert("Please Provide RTO Location Name");
                document.getElementById("<%=textboxRtoLocationName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxRTOLocationCode.ClientID%>").value == "") {
                alert("Please Provide RTO Location Code");
                document.getElementById("<%=textboxRTOLocationCode.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=textboxRTOLocationAddress.ClientID%>").value == "") {
                alert("Please Provide RTO Location Address");
                document.getElementById("<%=textboxRTOLocationAddress.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=textboxContactPersonName.ClientID%>").value == "") {
                alert("Please Provide Contact Person Name");
                document.getElementById("<%=textboxContactPersonName.ClientID%>").focus();
                return false;
            }
             
            if (document.getElementById("<%=textboxMobileNo .ClientID%>").value == "") {
                alert("Please Provide Mobile Number");
                document.getElementById("<%=textboxMobileNo .ClientID%>").focus();
                return false;
            }
            var emailID = document.getElementById("textboxEmailId").value;
            if (emailID != "") {
                if (emailcheck(emailID) == false) {
                    document.getElementById("textboxEmailId").value = "";
                    document.getElementById("textboxEmailId").focus();
                    return false;
                }
            } 

            if (invalidChar(document.getElementById("textboxRtoLocationName"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxRtoLocationName").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxRTOLocationCode"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("textboxRTOLocationCode").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxRTOLocationAddress"))) {
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("textboxRTOLocationAddress").focus();
                return false;
            }

            if (invalidChar(document.getElementById("textboxContactPersonName"))) {
                alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
                document.getElementById("textboxContactPersonName").focus();
                return false;
            }
            var MobileNo = document.getElementById("textBoxMobileNo").value;

            if (MobileNo != "") {
                if (MobileNo.length != 10) {
                    alert("Please Provide Correct Mobile No.");
                    document.getElementById("textBoxMobileNo").focus();
                    return false;
                }
            }

             

        }



        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        //invalidChar
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="margin: 20px;" align="center">
            <fieldset>
                    <legend>
                <div style="margin-left: 10px; font-size:medium;color:Black">
                     Location
                   </div>
            </legend>

                <br />
                <table  width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                <tr>
                <td class="form_text" style="padding-bottom: 10px"> State Name : 
                <span class="alert">* </span>
                </td>
                
                <td  > 
                    <asp:DropDownList ID="dropdownListStateName" DataTextField="HSRPStateName" AutoPostBack ="true"
                        DataValueField="HSRP_StateID" runat="server" >
                    </asp:DropDownList>
                </td>
                <td></td>
                <td class="form_text" style="padding-bottom: 10px"> Location Type : 
                <span class="alert">* </span>
                </td>
                
                <td  > 
                    <asp:DropDownList ID="dropdownListLocationType" DataTextField="HSRPStateName" AutoPostBack ="true"
                        DataValueField="HSRP_StateID" runat="server" >
                        <asp:ListItem>--Select Location--</asp:ListItem>
                        <asp:ListItem>Central</asp:ListItem>
                        <asp:ListItem>District</asp:ListItem>
                        <asp:ListItem>Sub-Urban</asp:ListItem>
                    </asp:DropDownList>
                </td>
                </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px"> Location Name : <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="textboxRtoLocationName" runat="server" class="form_textbox" ></asp:TextBox> </td> 
                       <td> &nbsp;</td>
                        <td class="form_text" style="padding-bottom: 10px"> Location Code : <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="textboxRTOLocationCode" runat="server" TabIndex="1" class="form_textbox"></asp:TextBox> </td>
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px"> Location Address :
                            <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="textboxRTOLocationAddress" runat="server" TabIndex="2" 
                                class="form_textbox"></asp:TextBox> </td> 
                         <td> &nbsp;</td>
                        <td class="form_text" style="padding-bottom: 10px"> Shipping Address :  </td>
                        <td> <asp:TextBox ID="textboxShippingAddress" runat="server" TabIndex="2" 
                                class="form_textbox"></asp:TextBox> </td> 
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px"> Billing Address : <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="textboxBillingAddress" runat="server" TabIndex="4" class="form_textbox"  ></asp:TextBox> </td> 
                         <td> &nbsp;</td>
                        <td class="form_text" style="padding-bottom: 10px"> Landline No :  </td>
                        <td> <asp:TextBox ID="textboxLandlineNo" runat="server" TabIndex="5" class="form_textbox"  onkeydown="return isNumberKey(event);"></asp:TextBox> </td>
                    </tr>
                    <tr>
                        <td class="form_text" style="padding-bottom: 10px"> Email Id </td>
                        <td> <asp:TextBox ID="textboxEmailId" runat="server" TabIndex="6" class="form_textbox"></asp:TextBox> </td> 
                         <td> &nbsp;</td>
                       <td class="form_text" style="padding-bottom: 10px"> Contact Person Name : <span class="alert">* </span></td>
                        <td> <asp:TextBox ID="textboxContactPersonName" runat="server" TabIndex="3" class="form_textbox"></asp:TextBox> </td>
                    </tr>
                     <tr>
                        <td class="form_text" style="padding-bottom: 10px"> Mobile No : <span class="alert">* </span> </td>
                        <td> <asp:TextBox ID="textboxMobileNo" runat="server" TabIndex="4" class="form_textbox" onkeydown="return isNumberKey(event);" MaxLength="10"></asp:TextBox> </td> 
                         <td> &nbsp;</td>
                       <td class="form_text" style="padding-bottom: 10px"> Active Status </td>
                        <td> <asp:CheckBox ID="checkBoxActiveStatus" TextAlign="Left" runat="server"  TabIndex="7" /> </td>
                    </tr>
                     <tr> 
                        <td class="form_text" style="padding-bottom: 10px"> &nbsp;Embossing Station </td>
                        <td> <asp:CheckBox ID="checkBoxEmbossingStation" TextAlign="Left" runat="server"  TabIndex="7" /> </td>
                    </tr>
                    <tr align="center">
                    <td></td>
                    <td>
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                        <td colspan="3" align="right">
                            <br />
                            <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()" 
                                class="button" onclick="buttonUpdate_Click" />&nbsp;&nbsp;
                            <asp:Button ID="ButtonSave" runat="server" Text="Save" TabIndex="8"  OnClientClick=" return validate()" 
                                class="button" onclick="ButtonSave_Click" 
                                 />&nbsp;&nbsp; <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                                            <%--<input type="reset"   id="Reset" value="Reset" class="button" />--%>
                                            <input type="reset" class="button" value="Reset" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    </form>
</body>
</html>
