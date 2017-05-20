<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateSubmitRequest.aspx.cs" Inherits="HSRP.Master.UpdateSubmitRequest" %>

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
    <%--<script language="javascript" type="text/javascript">

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
    </script>--%>
    <style type="text/css">
        .style1
        {
            width: 102px;
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
            width: 256px;
        }
        .style4
        {
            width: 256px;
        }
        .style5
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
            width: 256px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    
    <div style="margin:auto;"  align="center">
        <fieldset>
            <legend>
                <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                    Update Request
                </div>
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
                                            <td colspan="1" align="left" style="margin-left: 50px" width="200px" class="form_text">
                                                
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
                                
                            </tr>
                            <tr>
                                <td align="left" class="style5">
                                    State: 
                                </td>
                                <td align="left" class="style1">
                                    <%--<asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" AutoPostBack="true"
                                        ID="dropDownListOrg" runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID"
                                        OnSelectedIndexChanged="dropDownListOrg_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                    <asp:Label ID="lblState" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                                </td>
                              
                                <td align="left" class="form_text" width="160px">
                                   
                                    Location:</td>
                                <td align="left">
                                    <%--<asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" ID="dropDownListClient"
                                        runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                    </asp:DropDownList>--%>
                                    <asp:Label ID="lblSelectLocation" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
            <td class="style2" >Request Name <span class="alert">&nbsp;</span>:</td>
            <td align="left"><%--<asp:TextBox ID="txtRequestName" runat="server"></asp:TextBox>--%>
            <asp:Label ID="labRequestName" runat="server" Text="Label" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style2">Request Type <span class="alert">&nbsp;</span>: </td>
            <td align="left">
                <%--<asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" ID="DDLReqType" runat="server">
                <asp:ListItem Text="--Select Request Type--" Value="--Select Request Type--"></asp:ListItem>
                    <asp:ListItem Text="Error" Value="Error"></asp:ListItem>
                    <asp:ListItem Text="New Features" Value="New Features"></asp:ListItem>
                </asp:DropDownList>--%>
                <asp:Label ID="lblRequestType" runat="server" Text="Label" Font-Bold="true"></asp:Label>
            </td>
        </tr>
            
        <tr>
            <td class="style2">File Path: </td>
            <td align="left">
                <%--<asp:FileUpload ID="FileUpload1" runat="server"  />--%>
                <asp:Label ID="lblUploadFile" runat="server" Text="Label" Font-Bold="true"></asp:Label>
                <%--<a href=""  id="ShowFile" target="_self" visible="false" runat="server">Download</a>
                <asp:LinkButton ID="ShowFile" runat="server" visible="false" Text="Download" 
                    onclick="ShowFile_Click"> </asp:LinkButton>--%>
            </td>
        </tr>
        <tr>
            <td class="style2">Priority <span class="alert">&nbsp;</span>: </td>
            <td align="left">
                <%--<asp:DropDownList Style="margin-left: 0px" Font-Size="Small" Width="180px" ID="DDLPriority" runat="server">
                    <asp:ListItem Text="--Select Priority--" Value="--Select Priority--"></asp:ListItem>
                    <asp:ListItem Text="High" Value="High"></asp:ListItem>
                    <asp:ListItem Text="Normal" Value="Normal"></asp:ListItem>
                    <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                </asp:DropDownList>--%>
                <asp:Label ID="lblPriority" runat="server" Text="Label" Font-Bold="true"></asp:Label>
            </td>
        </tr>
            
        <tr>
            <td class="style2" valign="top">Remarks : </td>
            <td align="left">
                <%--<asp:TextBox ID="textboxRemarks" class="form_textbox" runat="server" Columns="5" Rows="5" Height="85px" Width="300px"
                                                                    TabIndex="6" TextMode="MultiLine"></asp:TextBox>--%>
                                                                    <asp:Label ID="lblRemark" runat="server" Text="Label" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style4">
                                    <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="llbMSGError" runat="server" ForeColor="Red" Font-Bold="True" 
                                        style="text-align: left"></asp:Label>
                                                        </td>
           
               
        </tr>
        <tr>
        <td class="style4"></td>
        <td class="style1"></td>
        <td align="left" class="form_text">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Remark:<span class="alert">*</span>
                                </td>
                                <td align="left" class="form_text">
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox><span class="alert"></span>
                                </td>
        </tr>
                            <tr>
                                <td class="style4">
                                    <asp:Button ID="Button2" OnClientClick="return validate()" runat="server" class="button" Width="150px"
                                        Text="Update" OnClick="Button1_Click" /></td>
                                <td class="style1">
                                    &nbsp;
                                </td>
                                <td align="left" class="form_text">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Status:<span class="alert">* </span>
                                </td>
                                <td align="left" class="form_text">
                                    <asp:DropDownList AutoPostBack="false" Visible="true" 
                                            ID="dropDownListorderStatus" CausesValidation="false" runat="server" Width="150px" >
                                        <asp:ListItem>-- Select Status --</asp:ListItem>
                                        <asp:ListItem  Value="New Order"  >New</asp:ListItem> 
                                        <asp:ListItem  Value="Pending"  >Pending</asp:ListItem>
                                        <asp:ListItem Value="Closed" >Closed</asp:ListItem> 
                                                </asp:DropDownList>
                                </td>
                            </tr>
                          
                            <tr>
                                <%--<td colspan="4">
                                    <asp:Button ID="Button1" OnClientClick="return validate()" runat="server" class="button" Width="150px"
                                        Text="Update" OnClick="Button1_Click" />
                                    <%--<asp:Button ID="ButImpData" OnClientClick="return validate()" runat="server" class="button"
                                        Text="Generate Data For NIC" onclick="ButImpData_Click" />--%>
                                <%--</td>--%>
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
