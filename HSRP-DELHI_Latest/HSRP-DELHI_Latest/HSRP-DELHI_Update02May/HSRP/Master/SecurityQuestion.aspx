<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecurityQuestion.aspx.cs"
    Inherits="HSRP.Master.SecurityQuestion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
 <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../javascript/common.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    function validate() {

        if (document.getElementById("<%=textboxQuestionText.ClientID%>").value == "") {
            alert("Please Provide Question Text");
            document.getElementById("<%=textboxQuestionText.ClientID%>").focus();
            return false;
        }
//        if (invalidChar(document.getElementById("textboxQuestionText"))) {
//            alert("Special Characters Not Allowed.");
//            document.getElementById("textboxQuestionText").focus();
//            return false;
//        }

    }
    
    </script>
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px;" align="center">
        <div style="margin-left: 10px; background-color: #FFFFFF; font-size:medium;color:Black">
            <fieldset>
                    
                <legend>
                <div style="margin-left: 10px; font-size:medium;color:Black">
                        Security Question
                </div>
                </legend>
                <br />
                <table  width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                    <tr> 
                     
                        <td class="form_text" style="padding-bottom: 10px"> Question Text :</td>
                        <td align="left" > <asp:TextBox ID="textboxQuestionText" runat="server" Width="430px" Height="80px" Columns="40" Rows="40" 
                                TabIndex="1" class="form_textbox" TextMode="MultiLine"  ></asp:TextBox> </td>
                    </tr> 
                   <tr>
                   <td></td></tr>
            <td> <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                  <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
              </td>
              
                <td colspan="5" align="right" >
                        <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()"
                            class="button" onclick="buttonUpdate_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="SAVE" TabIndex="4" class="button"  OnClientClick=" return validate()"
                            onclick="btnSave_Click" />&nbsp;&nbsp;
                    <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                    <input type="reset" class="button" value="Reset" />
                </td>
                </tr>
                     
                </table>
            </fieldset>
        </div>
    </div>
    </form>
</body>
</html>
