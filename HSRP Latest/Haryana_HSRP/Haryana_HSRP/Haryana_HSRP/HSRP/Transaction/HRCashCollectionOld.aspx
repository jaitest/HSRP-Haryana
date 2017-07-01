<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="HRCashCollectionOld.aspx.cs" Inherits="HSRP.Transaction.HRCashCollectionOld" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/table.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />    
    
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../javascript/bootstrap.min.js" type="text/javascript"></script>
    <script src="../javascript/bootbox.min.js" type="text/javascript"></script>  
    
    <%--<script type="text/javascript">
       
        
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnSave").hide();


        });

        $(document).on("click", ".alert1", function (e) {


            var regno = $('#<%=txtRegNumber.ClientID %>').val()
            var txt_Ac = $("#" + '<%= HiddenField1.ClientID %>').val();
            if (regno == "")
            {

                bootbox.alert("Please Enter Registration No. and Click On Go Button !", function ()
                {

                });
            }
            else
            {
                

                    bootbox.confirm("<B>Please Confirm !</b> <BR><i>Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</i>", function (result) {

                        if (result) {
                            $("#save1").hide();
                            $("#ctl00_ContentPlaceHolder1_btnSave").show();
                        }
                    });
                }
            
        });
       
    </script>--%>
    
    <script type="text/javascript">


        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnSave").hide();
        });

        $(document).on("click", ".alert1", function (e)
        {
           var regno = $('#<%=txtRegNumber.ClientID %>').val()
            var txt_Ac = $("#" + '<%= HiddenField1.ClientID %>').val();

            if (regno == "")
            {
                bootbox.alert("Please Enter Registration No. and Click On Go Button !", function ()
                {

                });
                $('#<%=txtRegNumber.ClientID %>').focus();
            }
            else if (txt_Ac != "") 
                {
                if ($('#<%=txtAC.ClientID %>').val() == "") 
                    {
                    bootbox.alert("Please Enter AC No. and Click On Go Button !", function () {

                    });
                    $('#<%=txtAC.ClientID %>').focus();
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
    
      <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                  HSRP Fee Collection  </div>
            </legend>
            <br />
          
    <table id="Table1" width="80%" align="center" style="font-size: 16px; color:Black; font-family:@Batang; background-color:#fff" >
        
                    
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
                <tr>
                    
             <td  >                
                Vehicle Registration No
            </td>
            <td >
                <asp:TextBox ID="txtRegNumber" ClientIDMode="Static" CssClass="text_box" runat="server" Visible="true"></asp:TextBox>
                
            </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtRegNumber" ErrorMessage="Please Enter Vehicle Reg No"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    <asp:HiddenField ID="HiddenField1" ClientIDMode="Static" runat="server" />
                    </td>
               </tr>
                
        
        <tr>
            <td class="form_text">
                Authorization Number
             </td>
            <td>
                <asp:TextBox ID="txtAuthorizationNo" runat="server" CssClass="text_box"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtAuthorizationNo" 
                    ErrorMessage="Please Enter Authorization No"></asp:RequiredFieldValidator>
            </td>
            <td class="form_text">
                Authorization Date
            </td>
            <td>
                <asp:TextBox ID="txtAuthDate" runat="server" CssClass="text_box"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtAuthDate" ErrorMessage="Please Enter Authorization Date"></asp:RequiredFieldValidator>
                <asp:CalendarExtender ID="txtAuthDate_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txtAuthDate">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>

             <td class="form_text">
                Engine No
            </td>
            <td>
                <asp:TextBox ID="txtEngineNo" runat="server" CssClass="text_box"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                    ControlToValidate="txtEngineNo" ErrorMessage="Please Enter Engine No"></asp:RequiredFieldValidator>
            </td>

           <%-- <td class="form_text" style="height: 22px">
                Registration No
            </td>
            <td style="height: 22px">
                <asp:TextBox ID="txtRegNumber" CssClass="text_box" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtRegNumber" ErrorMessage="Please Enter Vehicle Reg No"></asp:RequiredFieldValidator>
            </td>--%>
            <td class="form_text" style="height: 22px">
                Owner Email</td>
            <td style="height: 22px">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="text_box"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Owner Name
            </td>
            <td>
                <asp:TextBox ID="txtOwnerName" runat="server" CssClass="text_box"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="txtOwnerName" ErrorMessage="Please Enter Owner Name"></asp:RequiredFieldValidator>
            </td>
            <td class="form_text">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
        <td class="form_text">
            <asp:Label ID="lblAC" runat="server" Text="AC" Visible="false"></asp:Label>
        </td>
        <td colspan="3">
            <asp:TextBox ID="txtAC" runat="server" CssClass="text_box" MaxLength="10" Visible="false" ></asp:TextBox>            
            <asp:Label ID="lblACError" runat="server"  Text="" ForeColor="Red" Visible="false" ></asp:Label>
          </td>
      </tr>
        <tr>
            <td class="form_text">
                Address
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtAddress" runat="server" CssClass="text_box"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"  ControlToValidate="txtAddress" ErrorMessage="Please Enter Address"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="form_text" align="left">
                Vehicle Type
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlVehicletype" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="ddlVehicletype_SelectedIndexChanged">
                    <asp:ListItem Value="-Select Vehicle Type-"></asp:ListItem>
                    <asp:ListItem>SCOOTER</asp:ListItem>
                    <asp:ListItem>MOTOR CYCLE</asp:ListItem>
                    <asp:ListItem>TRACTOR</asp:ListItem>
                    <asp:ListItem>THREE WHEELER</asp:ListItem>
                    <asp:ListItem>LMV</asp:ListItem>
                    <asp:ListItem>LMV(CLASS)</asp:ListItem>
                    <asp:ListItem>MCV/HCV/TRAILERS</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="ddlVehicletype" ErrorMessage="Please Select Vehicle Type"></asp:RequiredFieldValidator>
            </td>
            <td class="form_text">
                Transaction Type
            </td>
            <td>
                <asp:DropDownList ID="ddlTransactionType" runat="server" AutoPostBack="True" onselectedindexchanged="ddlTransactionType_SelectedIndexChanged">
                    <asp:ListItem>-Select Transaction Type-</asp:ListItem>
                    <asp:ListItem>NB</asp:ListItem>
                    <asp:ListItem>OB</asp:ListItem>
                    <asp:ListItem>DB</asp:ListItem>
                   
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                    ControlToValidate="ddlTransactionType" 
                    ErrorMessage="Please Select Transaction type"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Vehicle Class Type
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlVehicleclass" runat="server" CssClass="form_text" 
                    AutoPostBack="True" 
                    onselectedindexchanged="ddlVehicleclass_SelectedIndexChanged">
                    <asp:ListItem>-Select Vehicle Class-</asp:ListItem>
                    <asp:ListItem>Transport</asp:ListItem>
                    <asp:ListItem>Non-Transport</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                    ControlToValidate="ddlVehicleclass" ErrorMessage="Please Select VehicleClass"></asp:RequiredFieldValidator>
            </td>
            <td class="form_text">
                Mobile No
            </td>
            <td>
                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="text_box" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Manufacturer Name
            </td>
            <td>
                <asp:TextBox ID="txtManufactureName" runat="server" CssClass="text_box"></asp:TextBox>
            </td>
            <td class="form_text">
                Model Name
            </td>
            <td>
                <asp:TextBox ID="txtModelName" runat="server" CssClass="text_box"></asp:TextBox>
            </td>
        </tr>
        <tr>


           <%-- <td class="form_text">
                Engine No
            </td>
            <td>
                <asp:TextBox ID="txtEngineNo" runat="server" CssClass="text_box"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                    ControlToValidate="txtEngineNo" ErrorMessage="Please Enter Engine No"></asp:RequiredFieldValidator>
            </td>--%>
            <td class="form_text">
                Chassis No
            </td>
            <td>
                <asp:TextBox ID="txtChassisno" runat="server" CssClass="text_box"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                    ControlToValidate="txtChassisno" ErrorMessage="Please Enter Chassis No"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="form_text">
                Amount
            </td>
            <td>
                <asp:Label ID="lblAmount" runat="server"></asp:Label>
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
                <asp:Button ID="btnGo" runat="server" Text="  Go  "
                   CssClass="button" Visible="False"/>
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
