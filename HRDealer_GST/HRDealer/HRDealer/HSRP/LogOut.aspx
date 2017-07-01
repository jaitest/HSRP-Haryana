<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogOut.aspx.cs" Inherits="HSRP.LogOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="shortcut icon" type="image/ico" href="../images/favicon.ico" />
<link rel="shortcut icon" href="../images/logo.ico" type="image/x-icon" />  
    <title>HSRP</title>
    <link href="./css/style.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
   
    <script type="text/javascript">
        var totalCount = 8;

        function ChangeIt() {
            var num = Math.ceil(Math.random() * totalCount);
            document.body.background = 'bgimages/' + num + '.jpg';
            document.body.style.backgroundRepeat = "repeat";
        }
        function validateForm() {

            var username = document.getElementById("txtUserID").value;
            var pass = document.getElementById("txtUserPassword").value;

            if (username == '') {
                alert('Please Provide User Name.');
                document.getElementById("txtUserID").focus();
                return false;
            }
            if (pass == '') {
                alert('Please Provide Password.');
                document.getElementById("txtUserPassword").focus();
                return false;
            }
            return true;


        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:100%;vertical-align:">
        <script type="text/javascript">
            ChangeIt();
        </script>
        <table  >
        
            <tr>
            <td style=" color: red; left: 19px; position: absolute; width: 599px;">
            Your Sessin Expired. Please Launch Application Again.
            </td>
            </tr>
        </table>
      
    </div>
    </form>
</body>
</html>
