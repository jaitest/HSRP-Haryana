<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HSRP.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="shortcut icon" type="image/ico" href="../images/favicon.ico" />
    <link rel="shortcut icon" href="../images/logo.ico" type="image/x-icon" />
    <title>HSRP Application Ver1.0</title>
    <link href="./css/style.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%--  <script type="text/javascript">

        var slowLoad = window.setTimeout(function () {

            alert("the page is taking longer time in loading please check internet connection.");

        }, 10);


        document.addEventListener('load', function () {

            window.clearTimeout(slowLoad);

        }, false);

    </script>--%>
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
    <div style="height: 100%; vertical-align: ">
        <script type="text/javascript">
            ChangeIt();
        </script>
        <table width="300" border="0" align="right" cellpadding="0" cellspacing="0" class="x-window-mc">
            <tr>
                <td valign="top">
                    <table width="90%" border="0" align="center" cellpadding="1" cellspacing="0">
                        <tr>
                            <td class="form_text">
                                &nbsp;
                            </td>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td height="5" class="form_text">
                                <img src="images/trans.gif" width="1" height="5" />
                            </td>
                            <td align="right">
                                <span class="form_text">
                                    <img src="images/trans.gif" width="1" height="5" /></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                User Name:
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtUserID" CssClass="text_box" TabIndex="1" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Password:
                            </td>
                            <td align="right">
                                <asp:TextBox ID="txtUserPassword" CssClass="text_box" TabIndex="2" TextMode="Password"
                                    runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bluelink" align="left">
                                <%--                                <asp:LinkButton ID="LinkButtonForget" runat="server" CssClass="bluelink" Text="Forgot password ?"
                                    OnClick="LinkButtonForget_Click"></asp:LinkButton>--%>
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="btnLogin" ImageUrl="~/images/login.png" Width="75" Height="22"
                                    runat="server" OnClick="btnLogin_Click" OnClientClick="javascript: return validateForm();" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsgBlue" CssClass="bluelink" runat="server" Text="Label"></asp:Label>
                                <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--  <div style="vertical-align:bottom;"  id="footerbg">
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" height="60px">
                <tr>
                    <td width="75%" class="topheaderboldtext">
                        &nbsp;&nbsp; Linkpoint Infrastructure Private Limited
                    </td>
                    <td width="25%" valign="middle">
                        <span class="maintext">137, Udyog Vihar, Phase-l, Gurgaon-122001 (Haryana)<br />
                            &nbsp;Support Toll Free: 1800-120-3030,<br />
                            &nbsp;Email: contact@hsrp.com</span>
                    </td>
                </tr>
            </table>
        </div>--%>
        <div>
            <table width="70%" border="0" cellspacing="0" cellpadding="0" 
                style="height: 100%">
                <tr>
                    <td>
                        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top" bgcolor="#777777">
                                    <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                        <tr>
                                            <td align="justify" width="3%" height="19" valign="top" bgcolor="#00d8cc" class="form_text">
                                                <font color="Black">Important Information and mandatory instructions for employees to
                                                    follow in High Security Registration Plates Implementation Project </font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="20" bgcolor="#ececef" class="maintext">
                                                <p>
                                                    &nbsp;</p>
                                                <marquee onmouseover="javascript:stop()" onmouseout="javascript:start()" direction="up"
                                                    hspace="8" loop="loop" scrollamount="2" scrolldelay="50" height="566px">
		<table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                        <tr>
                          <td height="26" valign="top" class="maintext">1.</td>
                          <td class="maintext"><font color="Black"><b>Office to be opened punctually at 09:30 Hrs.</b> </font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">2.</td>
                          <td class="maintext"><font color="Black">Employees assigned at RTO offices have to ensure that they carry Proper ID Cards issued by the employer/company at RTO Offices in State. </font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">3.</td>
                          <td class="maintext"><font color="Black">Ensure Authorisation letter to be issued to the Branch Co-ordinator. </font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">4.</td>
                          <td class="maintext"><font color="Black"> Ensure to verify that HSRP Authorisation letter recd. from the Vendor is properly signed and sealed by the designated Authorised RTO Official. </font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">5.</td>
                          <td class="maintext"><font color="Black"> In every case, assignment of Number Plate should be done after physical Verification of the Plate. The Assignee person should personally check the Plate for assignment. </font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">6.</td>
                          <td class="maintext"><font color="Black"> Ensure that no provisional receipt is issued to any Vehicle Owner. </font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">7.</td>
                          <td class="maintext"><font color="Black">Installation of Registration Plates to be done in presence of Authorised Registering official. Without the Presence of the RTO Official no HSRP is to be affixed. No installation of HSRP on the vehicle to be done out of the RTO premise.</font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">8.</td>
                          <td class="maintext"><font color="Black">For Old Number Plates, employees have to ensure that a proper database is managed and the materials are to be cut in pieces immediately on the same day and collected in scrap bag.</font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">9.</td>
                          <td class="maintext"><font color="Black">Ensure that HSRP must be fitted in the front of Vehicle Owner and RTO official and Authorisation slip after fitment is duly required to be signed by Vehicle Owner and RTO official. Proper data base for the authorisation slip must be kept.</font></td>
                        </tr>
                        <tr>
                         <td height="26" valign="top" class="maintext" style="font-weight: bold">10.</td>
                          <td class="maintext" style="font-weight: bold"><font color="Black">Plates are to be affixed strictly as per vehicle Category: </font></td>
                        </tr>
                        <tr>
                          <td height="26" class="maintext">&nbsp;</td>
                          <td class="maintext"><strong><font color="Black" style="font-weight: bold">White Colour Plates (Non-Transport) </font></strong></td>
                        </tr>
                        <tr style='style="font-weight: bold"'>
                          <td height="26" class="maintext">&nbsp;</td>
                          <td class="maintext" style="font-weight: bold"><table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black"> Scooter 200 X 100 mm (Front and Back) +Snap Lock </font></td>
                            </tr>
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black">Motorcycle 285 X 45 mm (F) 200 X 100 mm (B) + Snap Lock </font></td>
                            </tr>
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black">Tractor 285 X 45 mm (F) 200 X 100 mm (B) + Snap Lock </font></td>
                            </tr>
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black"><b>3 Wheelers 200 X 100 mm (F and B) +3rd Sticker + Snap Lock</b> </font></td>
                            </tr>
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black">Four Wheelers 500 X 120 mm (F and B) +3rd Sticker +Snap Lock (Ensure that there are Vehicle few Vehicles wherein Back Plate is of 340 X 200 mm such as Ambasador Car, Tavera, Innova, SUV, Scorpio) Ensure that the Vehicle should be inspected before the preparation of HSRP </font></td>
                            </tr>
                          </table></td>
                        </tr>
                        <tr style="font-weight: bold"'>
                          <td height="26" class="maintext">&nbsp;</td>
                          <td class="maintext"><strong><font color="Black">Yellow Colour Plates (Commercial/Transport)</font></strong></td>
                        </tr>
                        <tr style="font-weight: bold"'>
                          <td height="26" class="maintext">&nbsp;</td>
                          <td class="maintext" style="font-weight: bold"><table width="100%" border="0" align="center" cellpadding="3" cellspacing="1" style="font-weight: bold">
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black">Tractor 285 X 45 mm (F) 200 X 100 mm (B) + Snap Lock </font></td>
                            </tr>
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black">3 Wheelers 200 X 100 mm (F and B) +3rd Sticker + Snap Lock </font></td>
                            </tr>
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black"> Four Wheelers 500 X 120 mm (F and B) +3rd Sticker +Snap Lock (Ensure that there are Vehicle few Vehicles wherein Back Plate is of 340 X 200 mm such as Ambassador Car, Tavera, Innova, SUV, Scorpio) Ensure that the Vehicle should be inspected before the preparation of HSRP </font></td>
                            </tr>
                            <tr>
                              <td><font color="Black"><strong>*</strong></font></td>
                              <td><font color="Black">Comm./Heavy/Trpt. Vehicles  340 X 200 mm (F and B) + 3rd Sticker + Snap Lock </font></td>
                            </tr>
                          </table></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">11.</td>
                          <td class="maintext"><font color="Black">No communication/commitment to any Transport Department Official to be issued without the proper consent/authority of the management/company.</font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">12.</td>
                          <td class="maintext"><font color="Black">No official correspondence from branch to be given to any officer of the transport authority without prior approval of the management/company in that regard.</font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">13.</td>
                          <td class="maintext"><font color="Black"> Concerned employees have to ensure that the money collected during the collection Hrs. should be banked (in exact amount) with the bank as per the collection report. Daily report for installation, collection to be submitted with the concerned RTO office.(This report can be generated from the Report Server) </font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">14.</td>
                          <td class="maintext"><font color="Black"> Ensure that Vehicle Chassis No. And Engine No. Should be verified before affixation of HSRP on to the Vehicle. </font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">15.</td>
                          <td class="maintext"><font color="Black"> Ensure that the exact amount is collected from the Vehicle Owner. No extra amount is to be collected with cash receipt and no installation charges to be collected.</font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">16.</td>
                          <td class="maintext"><font color="Black"> Daily installation report after submitting with the department should be required to mail at respective ids.</font></td>
                        </tr>
                        <tr>
                          <td height="26" valign="top" class="maintext">17.</td>
                          <td class="maintext">COMMUNICATION SUPPORT Numbers are to be mentioned at every RTO office for the concerned RTO Head, so as in case of any problem support persons can be contacted.<br />
                           
                            </font></td>
                        </tr>
                      </table></marquee>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </table>
        </div>
    </div>
    </form>
</body>
</html>
