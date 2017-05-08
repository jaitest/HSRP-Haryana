<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="APCashCollection.aspx.cs" Inherits="HSRP.Transaction.APCashCollection" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    
    
    <script  src="../javascript/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../javascript/bootstrap.min.js" type="text/javascript"></script>
    <script src="../javascript/bootbox.min.js" type="text/javascript"></script> 
    
    
     <style type="text/css">
         
          .button-success {
            background: rgb(28, 184, 65); /* this is a green */
        }
      </style>
      
    <script type="text/javascript">

        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnSave").hide();
            
        });

        $(document).on("click", ".alert1", function (e) {

            var text = $("#ctl00_ContentPlaceHolder1_txtAuthNo").val()
            var regno = $("#ctl00_ContentPlaceHolder1_lblRegNo").val()
            debugger;
            if (text == "" && regno == "") {

                bootbox.alert("Please Enter Authorization No. and Click On Go Button !", function () {
                    
                });
            }
            else {
                var ddlaffix = $("#ctl00_ContentPlaceHolder1_ddlaffixation").val()
                if (ddlaffix == "--Select Affixation Center--") {
                    bootbox.alert("Please Select Affixation Center !", function () {
                        
                    });
                }
                else {

                    bootbox.confirm("<B>Please Confirm !</b> <BR><i>Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</i>", function (result) {

                        if (result) {
                            $("#save1").hide();
                            $("#ctl00_ContentPlaceHolder1_btnSave").show();
                        }
                    });
                }
            }
        });
       
    </script>
   <%--   <script language="javascript" type="text/javascript">
          function printDiv(divID) {
              //Get the HTML of div
              var divElements = document.getElementById(divID).innerHTML;
              alert(divElements);
              //Get the HTML of whole page
              var oldPage = document.body.innerHTML;

              //Reset the page's HTML with div's HTML only
              document.body.innerHTML =
              "<html><head><title></title></head><body>" +
              divElements + "</body>";

              //Print Page
              window.print();

              //Restore orignal HTML
              document.body.innerHTML = oldPage;


          }
    </script>--%>


       <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $("#btnPrint").live("click", function () {
            var divContents = $("#ctl00_ContentPlaceHolder1_printablediv").html();
            var printWindow = window.open('', '', 'height=400,width=800');
            printWindow.document.write('<html><head><title>Cash Reciept</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(divContents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
        });
function btnPrint_onclick() {

}

    </script>
<marquee class="mar1" direction="left" >Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</marquee>
    
      <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                 AP Cash Collection  </div>
            </legend>
        <table width="95%" border="0" cellspacing="0" cellpadding="0" align="center">
              <tr>
                <td width="900"><table id="Table1" width="100%" align="center" style="font-size: 16px; color:Black; font-family:@Batang; background-color:#fff" >
                  <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                  </tr>
                  <tr>
                    <td class="form_text"> Authorization Number </td>
                    <td><asp:TextBox ID="txtAuthNo" runat="server" class="form_textbox12"></asp:TextBox>
                      &nbsp;&nbsp;&nbsp;&nbsp;
                      <asp:Button ID="btnGo" runat="server" Text="  Go  " OnClick="btnGo_Click" 
                    CssClass="button" Width="62px" /></td>
                    <td></td>
                    <td></td>
                  </tr>
                  <tr>
                    <td class="form_text" style="height: 22px"> Authorization Number </td>
                    <td style="height: 22px"><asp:Label ID="lblAuthNo" runat="server" Text=""></asp:Label></td>
                    <td class="form_text" style="height: 22px"> Authorization Date </td>
                    <td style="height: 22px"><asp:Label ID="lblAuthDate" runat="server" Text=""></asp:Label></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Registration No </td>
                    <td><asp:Label ID="lblRegNo" runat="server" Text=""></asp:Label></td>
                    <td class="form_text"> RTO Location Code/Name </td>
                    <td><asp:Label ID="lblRTOLocationCode" runat="server" Text=""></asp:Label>
                      <asp:Label
                    ID="lblRTOName" runat="server" Text=""></asp:Label></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Owner Name </td>
                    <td><asp:Label ID="lblOwnerName" runat="server" Text=""></asp:Label></td>
                    <td class="form_text"> Owner Email </td>
                    <td><asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Address </td>
                    <td colspan="3"><asp:Label ID="lblAddress" runat="server" Text=""></asp:Label></td>
                  </tr>
                  <tr>
                    <td class="form_text" style="height: 22px"> Vehicle Type </td>
                    <td style="height: 22px"><asp:Label ID="lblVehicleType" runat="server" Text=""></asp:Label></td>
                    <td class="form_text" style="height: 22px"> Transaction Type </td>
                    <td style="height: 22px"><asp:Label ID="lblTransactionType" runat="server" Text=""></asp:Label></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Vehicle Class Type </td>
                    <td><asp:Label ID="lblVehicleClassType" runat="server" Text=""></asp:Label></td>
                    <td class="form_text"> Mobile No </td>
                    <td><asp:Label ID="lblMobileNo" runat="server" Text=""></asp:Label></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Manufacturer Name </td>
                    <td><asp:Label ID="lblMfgName" runat="server" Text=""></asp:Label></td>
                    <td class="form_text"> Model Name </td>
                    <td><asp:Label ID="lblModelName" runat="server" Text=""></asp:Label></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Engine No </td>
                    <td><asp:Label ID="lblEngineNo" runat="server" Text=""></asp:Label></td>
                    <td class="form_text"> Chassis No </td>
                    <td><asp:Label ID="lblChasisNo" runat="server" Text=""></asp:Label></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Amount </td>
                    <td><asp:Label ID="lblAmount" runat="server" Text=""></asp:Label></td>
                    <td></td>
                    <td></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Third Sticker </td>
                    <td><asp:CheckBox ID="bln3rdSticker" runat="server" Enabled="False" /></td>
                    <td class="form_text"> VIP </td>
                    <td><asp:CheckBox ID="blnVIP" runat="server" /></td>
                  </tr>
                  <tr>
                    <td class="form_text"> Remarks </td>
                    <td><asp:TextBox ID="remarks" runat="server" Height="66px" TextMode="MultiLine" Width="241px"></asp:TextBox></td>
                    <td> Affixation Center Name :<span style="color:Red">*</span></td>
                    <td><asp:DropDownList ID="ddlaffixation" runat="server" Height="25px" 
                                     Width="187px" DataTextField="AffixCenterDesc" 
                                    DataValueField="Rto_Id"> </asp:DropDownList></td>
                  </tr>
                  <tr>
                    <td><a class="alert1" id="save1" >Confirm</a>
                      <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" 
                        BackColor="Orange" Width="57px" /></td>
                    <td><asp:Button ID="Button3" runat="server" CssClass="button-success" 
                            onclick="Button3_Click" TabIndex="12" Text="Epson Print" Visible="false"
                     />                
                      <asp:Button ID="btnDownload" runat="server" Text="Download Receipt" 
                    OnClick="btnDownloadReceipt_Click" CssClass="button-success" Visible="false"  />                
                      <%--<input type="button" id="btnPrint" value="Print TVSE Reciept" CssClass="button-success"   onclick="javascript:printDiv('ctl00_ContentPlaceHolder1_printablediv');" onclick="return btnPrint_onclick()" />--%></td>
                    <td colspan="2"><asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                      <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label></td>
                  </tr>
                  <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                  </tr>
                  <tr>
                    <td></td>
                    <td>
                    <div id="printablediv" runat="server"></div>
                      </td>
                    <td></td>
                    <td></td>
                  </tr>
                  <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                  </tr>
                </table></td>
                <td valign="top"><table width="80%" border="0" align="center" cellpadding="0" cellspacing="0">
                  <tr>
                    <td colspan="2" nowrap="nowrap"><strong>Today Summary:</strong></td>
                  </tr>
                  <tr>
                    <td class="input-xlarge" style="width: 186px">Today Booking Count:</td>
                    <td><asp:Label ID="lblCount" runat="server">0</asp:Label></td>
                  </tr>
                  <tr>
                    <td class="input-xlarge" style="width: 186px">Today Collection :</td>
                    <td><asp:Label ID="lblCollection" runat="server">0</asp:Label></td>
                  </tr>
                  <tr>
                    <td class="input-xlarge" style="width: 186px"nowrap="nowrap">Last Bank Deposit Date:</td>
                    <td><asp:Label ID="lblLastDepositdate" runat="server">0</asp:Label></td>
                  </tr>
                  <tr>
                    <td class="input-xlarge" style="width: 186px"nowrap="nowrap">Last Bank Deposit Amount :</td>
                    <td><asp:Label ID="lblLastAmount" runat="server">0</asp:Label></td>
                  </tr>
                  <tr>
                    <%--<td class="input-xlarge" style="width: 186px"nowrap="nowrap">Last Cash Receipet No :</td>
                    <td><asp:Label ID="lblReceipet" runat="server">0</asp:Label></td>>--%>
                  </tr>
                </table></td>
          </tr>
        </table>
        <br />
            <br />
    </fieldset>
</asp:Content>
