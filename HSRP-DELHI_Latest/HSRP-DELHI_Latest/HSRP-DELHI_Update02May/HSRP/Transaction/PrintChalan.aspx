<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintChalan.aspx.cs" Inherits="HSRP.Transaction.PrintChalan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<link href="../css/main.css" rel="stylesheet" type="text/css" />
<head runat="server">
    <title></title>
</head>
 <script type="text/javascript" language="javascript">

     function validate() {
         var check = document.getElementById("CheckBoxOldPlate").checked;
        
         if (check == false) { 
             alert("Please Check Received Old Plate");
             document.getElementById("CheckBoxOldPlate").focus();
             return false;
         }
     }
    </script>

<body>
    <center>
        <form id="form1" runat="server">
       
        <div>
            <table width="30%" style="background-color:White">
           
             <tr>
              
                     <td class="form_text" style="padding-bottom: 10px"> Received Old Plate : <span class="alert">* </span><asp:CheckBox ID="CheckBoxOldPlate" Checked="false" runat="server" /></td> 
                </tr>

                <tr>
                    <td align="center" nowrap="nowrap">
                      <%--  <asp:Button ID="buttonUpdate" runat="server" TabIndex="17" class="button" Visible="True"
                            Text="Print" OnClick="buttonUpdate_Click" Width="90px" />--%>
                        &nbsp;
                        <asp:Button ID="buttonSave" runat="server" TabIndex="18" class="button" Text="Print & Close Order"
                            Visible="True" OnClientClick="return validate()" OnClick="buttonSave_Click" />
                        &nbsp;
                        <input type="button" onclick="javascript:parent.googlewin.close();" tabindex="19"
                            name="buttonClose" id="buttonClose" value="Window Close" class="button" /> <br />
                    </td>
                    
                </tr>
               
            </table>
        </div>
        </form>
    </center>
</body>
</html>
