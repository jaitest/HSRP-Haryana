<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" 
    CodeBehind="APCashCollection_Check.aspx.cs" Inherits="HSRP.Transaction.APCashCollection_check" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    
    
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../javascript/bootstrap.min.js" type="text/javascript"></script>
    <script src="../javascript/bootbox.min.js" type="text/javascript"></script>    
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
    
&nbsp;<fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                  Cash Collection  Verification and Resync to Transport Screen</div>
            </legend>
            <br />
            <br />
    <table id="Table1" width="80%" align="center" style="font-size: 16px; color:Black; font-family:@Batang; background-color:#fff" >
        
        <tr>
            <td class="form_text">
                Authorization Number
            </td>
            <td>
                <asp:TextBox ID="txtAuthNo" runat="server" class="form_textbox12"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp; 
                <asp:Button ID="btnGo" runat="server" Text="  Go  " OnClick="btnGo_Click" 
                    CssClass="button" Width="62px" />
            </td>
            <td>
                
                Cash Receipt No</td>
            <td>
                <asp:Label ID="lblmsg1" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Authorization Number
            </td>
            <td>
                <asp:Label ID="lblAuthNo" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text">
                Authorization Date
            </td>
            <td>
                <asp:Label ID="lblAuthDate" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Registration No
            </td>
            <td>
                <asp:Label ID="lblRegNo" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text">
                RTO Location Code/Name
            </td>
            <td>
                <asp:Label ID="lblRTOLocationCode" runat="server" Text=""></asp:Label><asp:Label
                    ID="lblRTOName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Owner Name
            </td>
            <td>
                <asp:Label ID="lblOwnerName" runat="server" Text=""></asp:Label>
            </td>
            <td class="form_text">
                Owner Email
            </td>
            <td>
                <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
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
                Mobile No
            </td>
            <td>
                <asp:Label ID="lblMobileNo" runat="server" Text=""></asp:Label>
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
            </td>
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
                <asp:CheckBox ID="blnVIP" runat="server" Enabled="False" />
            </td>
        </tr>
        <tr>
            <%--<a class="alert1" id="save1">Confirm</a>--%>
            <td>
                <asp:TextBox ID="remarks" runat="server" Height="66px" TextMode="MultiLine" 
                    Width="241px" Visible="False"></asp:TextBox>
            </td>
            <%--<td>
            Affixation Center Name :<span style="color:Red">*</span>
            </td>--%>
            <td>
            <asp:DropDownList ID="ddlaffixation" runat="server" Height="25px" 
                                     Width="187px" DataTextField="AffixCenterDesc" 
                                    DataValueField="Rto_Id" Visible="False">
                                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            <%--<a class="alert1" id="save1">Confirm</a>--%>
                <%--<asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" 
                    BackColor="Orange" Width="57px" Visible="False"/>--%>
            &nbsp;<asp:Button ID="btnSync" runat="server" Text="Re-Sync With Transport" OnClick="btnSync_Click" 
                    BackColor="Orange" Width="165px"    />
            </td>
            <td>
            <asp:Button ID="Button3" runat="server" CssClass="button" Height="23px" 
                            onclick="Button3_Click" TabIndex="12" Text="Epson Print" 
                    Width="77px" Visible="False" />
             <asp:Button ID="btnDownload" runat="server" Text="Download Receipt" 
                    OnClick="btnDownloadReceipt_Click" CssClass="button" Visible="False" />
            </td>
            <td colspan="2">
            <%-- <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
              <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>--%>
                                                           
           
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
            <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
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
             <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
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
