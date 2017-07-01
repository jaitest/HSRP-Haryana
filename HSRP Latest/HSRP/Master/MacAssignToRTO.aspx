<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MacAssignToRTO.aspx.cs" Inherits="HSRP.Master.MacAssignToRTO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>  
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script> 
    <script language="javascript" type="text/javascript">
        function validate() { 
             
            if (document.getElementById("<%=dropdownlistStateName.ClientID%>").value == "--Select State--") {
                alert("Select State Name");
                document.getElementById("<%=dropdownlistStateName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=dropdownlistRtoName.ClientID%>").value == "--Select RTO Location--") {
                alert("Select RTO Location");
                document.getElementById("<%=dropdownlistRtoName.ClientID%>").focus();
                return false;
            } 
        }
    
    </script>
</head>
<body>
    <form id="form1" runat="server"> 
    <div style="margin: 20px;background-color: #FFFFFF;" align="center">
    <fieldset>

           <legend>
                <div  style="margin-left: 10px; font-size:medium;color:Black">
                    Message Ticker   </div>
                    </legend>
    
        <table width="100%" border="0" style="background-color: #FFFFFF"  align="left" cellpadding="3" cellspacing="1">
        <tr>
            <td class="form_text" style="padding-bottom: 10px"> State : <span class="alert">* </span></td>
                <td >
                    <asp:DropDownList ID="dropdownlistStateName" DataTextField="HSRPStateName" 
                        DataValueField="HSRP_StateID" class="dropdown_css" Width="170px" runat="server" 
                        TabIndex="4" AutoPostBack="True" 
                        onselectedindexchanged="dropdownlistStateName_SelectedIndexChanged1"  >
                    </asp:DropDownList>
                </td>
                 <td> &nbsp; &nbsp;</td>
                <td class="form_text" style="padding-bottom: 10px"> RTO Location : <span class="alert">* </span></td>
                <td >
                    <asp:DropDownList ID="dropdownlistRtoName" DataTextField="RTOLocationName"  DataValueField="RTOLocationID" class="dropdown_css" Width="170px"  runat="server" TabIndex="3"> 
                    </asp:DropDownList>
                </td> 
            </tr>
            <tr><td colspan="5"></td></tr>
            
            
           
            <tr>
            <td> <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                  <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
              </td>
              
                <td colspan="5" align="right" >
                        <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()"
                            class="button" onclick="buttonUpdate_Click"  />&nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="SAVE" TabIndex="4" class="button"  OnClientClick=" return validate()"
                            onclick="btnSave_Click" />&nbsp;&nbsp;
                    <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                    <input type="reset" class="button" value="Reset" />
                </td>
                </tr>
             
        </table>
    </fieldset>
    </div>
    </form>
</body>
</html>
