<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Prefix.aspx.cs" Inherits="HSRP.Master.Prefix" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     
<script language="javascript" type="text/javascript">
    function validate() {
        var ed = document.getElementById("<%=DropdownStateName.ClientID%>").value;
       // alert(ed);
        if (document.getElementById("<%=DropdownStateName.ClientID%>").value == "--Select State--") {
            alert("Select State Name");
            document.getElementById("<%=DropdownStateName.ClientID%>").focus();
            return false;
        }
        if (document.getElementById("<%=DropdownRTOName.ClientID%>").value == "--Select RTO Location--") {
            alert("Select RTO Location");
            document.getElementById("<%=DropdownRTOName.ClientID%>").focus();
            return false;
        }
       

        if (document.getElementById("<%=DropdownFrefixFor.ClientID%>").value == "--Select Prefix For--") {
            alert("Select Prefix For");
            document.getElementById("<%=DropdownFrefixFor.ClientID%>").focus();
            return false;
        } 

        if (document.getElementById("<%=DropdownFrefixFor.ClientID%>").value == "-- Select Prefix For --") {
            alert("Select Prefix For");
            document.getElementById("<%=DropdownFrefixFor.ClientID%>").focus();
            return false;
        } 

        
        if (document.getElementById("<%=textboxPrefiex.ClientID%>").value == "") {
            alert("Please Provide Prefix Text");
            document.getElementById("<%=textboxPrefiex.ClientID%>").focus();
            return false;
        }


        if (invalidChar(document.getElementById("textboxPrefiex"))) {
            alert("Your can't enter special characters. \nThese are not allowed.\n Please remove them.");
            document.getElementById("textboxPrefiex").focus();
            return false;
        }
    }
    
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px; background-color: #FFFFFF;" align="center">
    <fieldset>
                <legend>
                <div style="margin-left: 10px; font-size:medium;color:Black">
                    Prefix </div>
            </legend>
            <br />
    <div>
        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
            <tr>
                <td class="form_text" style="padding-bottom: 10px"> State Name : <span class="alert">* </span> </td>
                <td > <asp:DropDownList ID="DropdownStateName" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" class="dropdown_css" runat="server" 
                        onselectedindexchanged="DropdownStateName_SelectedIndexChanged" AutoPostBack="True"> 
                    </asp:DropDownList>
                </td> 
                <td>&nbsp; &nbsp;</td>
                <td class="form_text" style="padding-bottom: 10px"> RTO Location : <span class="alert">* </span> </td>
                <td>
                    <asp:DropDownList ID="DropdownRTOName" DataTextField="RTOLocationName"  DataValueField="RTOLocationID"
                        runat="server" TabIndex="1" AutoPostBack="True" >
                        <asp:ListItem>--Select RTO Location--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="form_text" style="padding-bottom: 10px"> Prefix For : <span class="alert">* </span> </td>
                <td>
                <asp:DropDownList ID="DropdownFrefixFor" class="dropdown_css" runat="server">
                        <asp:ListItem>--Select Prefix For--</asp:ListItem> 
                        <asp:ListItem >Invoice No</asp:ListItem> 
                        <asp:ListItem >Delivery Challan No</asp:ListItem>
                        <asp:ListItem >Cash Receipt No</asp:ListItem>
                         <asp:ListItem >Order No</asp:ListItem>
                    </asp:DropDownList> 
                </td>
                <td>&nbsp; &nbsp;</td>
                <td class="form_text" style="padding-bottom: 10px" > Prefiex Text : <span class="alert">* </span> </td>
                <td  > <asp:TextBox ID="textboxPrefiex" runat="server" class="form_textbox"  TabIndex="3"></asp:TextBox> </td> 
            </tr>
            <tr>
                  <td>
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                        </td>
                <td colspan="5" align="right" >
                        <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()"
                            class="button" onclick="buttonUpdate_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="SAVE" TabIndex="4" class="button"  OnClientClick=" return validate()"
                            onclick="btnSave_Click"   />&nbsp;&nbsp;
                    <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                    
                   <input type="reset" class="button" value="Reset" />
                </td>
            </tr>
             
        </table>
    </div>
   
    </fieldset></div>
    </form>
</body>
</html>
