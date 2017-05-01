<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="HPCashCollection.aspx.cs" Inherits="HSRP.Transaction.HPCashCollection" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    
    
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../javascript/bootstrap.min.js" type="text/javascript"></script>
    <script src="../javascript/bootbox.min.js" type="text/javascript"></script>  
     <script type="text/javascript">
         //Function to allow only numbers to textbox
         function validate(key) {
             //getting key code of pressed key
             var keycode = (key.which) ? key.which : key.keyCode;
             var phn = document.getElementById('txtPhn');
             //comparing pressed keycodes
             if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
                 return false;
             }
             else {
                 //Condition to check textbox contains ten numbers or not
                 if (phn.value.length < 10) {
                     return true;
                 }
                 else {
                     return false;
                 }
             }
         }
</script>
    <script type="text/javascript">
       
        
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnSave").hide();


        });

        $(document).on("click", ".alert1", function (e) {


            var regno = $('#<%=txtRegNo.ClientID %>').val()
            if (regno == "") {

                bootbox.alert("Please Enter Registration No. and Click On Go Button !", function () {

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
            
        });
       
    </script>
    
<marquee class="mar1" direction="left" >Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</marquee>
    
      <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                  Cash Collection  </div>
            </legend>
            <br />
            <br />
    <table id="Table1" width="80%" align="center" style="font-size: 16px; color:Black; font-family:@Batang; background-color:#fff" >
        
        <tr>
            <td>
                Registration No.
            </td>
            <td valign="top">
                <asp:TextBox ID="txtRegNo" runat="server" CssClass="form_textbox2"></asp:TextBox>&nbsp;&nbsp;<asp:Button ID="btnGo" runat="server" Text="  Go  " OnClick="btnGo_Click" 
                   CssClass="button"/>
                <br />
            &nbsp;</td>
            <td>
                
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="form_text" style="height: 22px">
                Authorization Number
             </td>
            <td style="height: 22px">
                <asp:Label ID="lblAuthNo" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text" style="height: 22px">
                Authorization Date
            </td>
            <td style="height: 22px">
                <asp:Label ID="lblAuthDate" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_text" style="height: 22px">
                Registration No
            </td>
            <td style="height: 22px">
                <asp:Label ID="lblRegNo" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text" style="height: 22px">
                RTO Location Name
            </td>
            <td style="height: 22px">
                <asp:Label
                    ID="lblRTOName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_text" style="height: 22px">
                Owner Name
            </td>
            <td style="height: 22px">
                <asp:Label ID="lblOwnerName" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text" style="height: 22px">
                Owner Email
            </td>
            <td style="height: 22px">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="text_box"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Address
            </td>
            <td colspan="3">
                <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Vehicle Type
            </td>
            <td>
                <asp:Label ID="lblVehicleType" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text">
                Transaction Type
            </td>
            <td>
                <asp:Label ID="lblTransactionType" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Vehicle Class Type
            </td>
            <td>
                <asp:Label ID="lblVehicleClassType" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text">
                Mobile No *
            </td>
            <td align="center">
                <asp:TextBox ID="txtMobileNo" runat="server" MaxLength=10 Width="154px" onkeypress="return validate(event)"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Manufacturer Name
            </td>
            <td>
                <asp:Label ID="lblMfgName" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text">
                Model Name
            </td>
            <td>
                <asp:Label ID="lblModelName" runat="server" Text=""></asp:Label>
                o</td>
        </tr>
        <tr>
            <td class="form_text">
                Engine No
            </td>
            <td>
                <asp:Label ID="lblEngineNo" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text">
                Chassis No
            </td>
            <td>
                <asp:Label ID="lblChasisNo" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Amount
            </td>
            <td>
                <asp:Label ID="lblAmount" runat="server" Text=""></asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Third Sticker
            </td>
            <td>
                <asp:CheckBox ID="bln3rdSticker" runat="server" Enabled="False" />
            </td>
            <td class="form_text">
                VIP
            </td>
            <td>
                <asp:CheckBox ID="blnVIP" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Remarks
            </td>
            <td>
                <asp:TextBox ID="remarks" runat="server" Height="66px" TextMode="MultiLine" Width="241px"></asp:TextBox>
            </td>
           <%-- <td>
            Affixation Center Name :<span style="color:Red">*</span>
            </td>--%>
            <td>
           <%-- <asp:DropDownList ID="ddlaffixation" runat="server" Height="25px" 
                                     Width="187px" DataTextField="AffixCenterDesc" 
                                    DataValueField="Rto_Id" Visible="False">
                                </asp:DropDownList>--%>
            </td>
        </tr>
        <tr>
            <td>
            <a class="alert1" id="save1">Confirm</a>
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" 
                    BackColor="Orange" Width="57px"    />
            </td>
            <td>
            <asp:Button ID="Button3" runat="server" CssClass="button" Height="23px" 
                            onclick="Button3_Click" TabIndex="12" Text="Epson Print" 
                    Width="77px" Visible="False" />
             <asp:Button ID="btnDownload" runat="server" Text="Download Receipt" 
                    OnClick="btnDownloadReceipt_Click" CssClass="button" Visible="false" />
            </td>
            <td colspan="2">
             <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
              <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                                                           
           
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
      <br />
            <br />
    </fieldset>
</asp:Content>
