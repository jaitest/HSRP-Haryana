<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InventoryNew.aspx.cs" Inherits="HSRP.Transaction.InvneotryNew" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" /> 
    <script src="../javascript/common.js" type="text/javascript"></script>
     
    <script type="text/javascript" language="javascript">
        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) 
            { 
                return true;
            }
            else
            {
                return false;
            } 
        }
    </script>
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropdownStateName.ClientID%>").value == "--Select State--") {
                alert("Select State Name");
                document.getElementById("<%=DropdownStateName.ClientID%>").focus();
                return false;
            }
            var leng = document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").value;
            
            if (leng.length > "11") {
                alert("Enter Minimum 10 Character");
                document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").focus();
                return false;
            }

            if (leng.length < "8") {
                alert("Enter Atleast 8 Character");
                document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").focus();
                return false;
            }


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

            if (document.getElementById("<%=DropDownListProductName.ClientID%>").value == "-- Select Product --") {
                alert("Select Product");
                document.getElementById("<%=DropDownListProductName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").value == "") {
                alert("Please Provide Laser Start No.");
                document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=DropDownListPrefixLaserNo.ClientID%>").value == "-- Select Prefix --") {
                alert("Please Select Prefix");
                document.getElementById("<%=DropDownListPrefixLaserNo.ClientID%>").focus();
                return false;
            }

        }
    </script>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth
        {
            width: 140px !important;
        }
        #divwidth div
        {
            width: 140px !important;
        }
    </style>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth2
        {
            width: 120px !important;
        }
        #divwidth2 div
        {
            width: 120px !important;
        }
    </style>
    <style>
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .9em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
        }
        #divwidth3
        {
            width: 150px !important;
        }
        #divwidth3 div
        {
            width: 150px !important;
        }
    </style>
    <style type="text/css">
        .modalPopup
        {
            background-color: #FFFFFF;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        }
    </style>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);

        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>
      
 <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <asp:Image ID="Image1" ImageUrl="../images/ajax-loader.gif" AlternateText="Processing"
                runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
        PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
  <asp:UpdatePanel ID="Update1" runat="server">
   <ContentTemplate> 
    <table width="100%" border="0" align="left">
     <tr>
     <td>
        <fieldset>
           <legend> 
                <div style="margin-left: 10px; font-size: medium; height:552px; color: Black">
                    <asp:Label ID="LabelFormName" runat="server" Text="Label"></asp:Label>
                </div>
            </legend> 
                <div>
                    <table style="background-color: #FFFFFF" border="0" align="left" width="100%" cellpadding="3" cellspacing="1">
            <tr>
                <td  class="form_text" style="padding-bottom: 10px">
                    <asp:Label ID="LabelStateName" runat="server" Text=" ">State Name : <span class="alert">* </span></asp:Label></td>
                <td > <asp:DropDownList ID="DropdownStateName" DataTextField="HSRPStateName" Width="150px" TabIndex="0"
                        DataValueField="HSRP_StateID" class="dropdown_css" runat="server" 
                          AutoPostBack="True" 
                        onselectedindexchanged="DropdownStateName_SelectedIndexChanged"> 
                    </asp:DropDownList>
                </td> 
                <td></td>
            <td  class="form_text" style="padding-bottom: 10px"> <asp:Label ID="LabelRTOLocation" runat="server" Text=" ">Location : <span class="alert">* </span></asp:Label> </td>
               <td>
                    <asp:DropDownList ID="DropdownRTOName" DataTextField="RTOLocationName"  DataValueField="RTOLocationID" Width="180px"  
                        runat="server" TabIndex="1" AutoPostBack="True" >
                        <asp:ListItem>--Select RTO Location--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
                        <caption>
                            &nbsp; &nbsp;
                            <tr>
                                <td class="form_text" style="padding-bottom: 10px">
                                    Product Name : <span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownListProductName" runat="server" 
                                        AutoPostBack="True" DataTextField="ProductCode" DataValueField="ProductID" 
                                        TabIndex="2" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td class="form_text" style="padding-bottom: 10px">
                                    Prefix Laser Code From : <span class="alert">* </span>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownListPrefixLaserNo" runat="server" 
                                        AutoPostBack="True" class="dropdown_css" DataTextField="Prefix" 
                                        DataValueField="PrefixID" TabIndex="3" Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </caption>
                </table>
             </div>
        </fieldset></td>
     </tr>
        <tr>
            <td class="form_text" style="padding-bottom: 10px">
                Laser Start No. : <span class="alert">* </span>
            </td>
            <td>
                <asp:TextBox ID="textboxLaserCodeFrom" runat="server" class="form_textbox" 
                    MaxLength="10" TabIndex="4"></asp:TextBox>
            </td>
            <td>
            </td>
            <td class="form_text" style="padding-bottom: 10px">
                Quantity : <span class="alert">* </span>
            </td>
            <td>
                <asp:TextBox ID="textboxQuantity" runat="server" class="form_textbox" 
                    MaxLength="4" onkeypress="return isNumberKey(event)" TabIndex="5"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_text" style="padding-bottom: 10px">
                Invoice No :
            </td>
            <td>
                <asp:TextBox ID="textboxInvNo" runat="server" class="form_textbox" TabIndex="6"></asp:TextBox>
            </td>
            <td>
            </td>
            <td class="form_text" style="padding-bottom: 10px">
                Invoice From :
            </td>
            <td>
                <asp:TextBox ID="textboxInvFrom" runat="server" class="form_textbox" 
                    TabIndex="7"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_text" style="padding-bottom: 10px">
                Remarks :
            </td>
            <td colspan="4" valign="top">
                <asp:TextBox ID="textboxRemarks" runat="server" class="form_textbox" 
                    Columns="25" Height="85px" Rows="15" TabIndex="8" TextMode="MultiLine" 
                    Width="600px"></asp:TextBox>
                <br />
                <%--<ComponentArt:Calendar ID="HSRPAuthDate" runat="server" Visible="false" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>--%>&nbsp;&nbsp;
                <img id="ImgPollution" alt="" class="calendar_button" style="visibility:hidden" 
                                                                    onclick="HSRPAuthDate_OnClick()"
                                                                    src="../images/btn_calendar.gif" tabindex="4" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td colspan="4">
                <asp:Label ID="lblErrorUpdate" runat="server" Font-Size="18px" ForeColor="Red" 
                    style="font-size: 12px; float: left;" Visible="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="5">
                <div style="border:0px solid; position:static;float: left; width:250px;">
                    <asp:Label ID="LabelUpdated" runat="server" Font-Size="18px" ForeColor="Blue" 
                        style="font-size: 18px; float: left;"></asp:Label>
                    <asp:Label ID="lblTotalRecord" runat="server" Font-Size="18px" ForeColor="Blue" 
                        style="font-size: 18px; float: left;"></asp:Label>
                    <br />
                    <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue" 
                        style="font-size: 18px; float: left;"></asp:Label>
                    <br />
                    <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red" 
                        style="font-size: 18px; float: left;"></asp:Label>
                    <br />
                    <asp:Label ID="lblExist" runat="server" Font-Size="18px" ForeColor="Red" 
                        style="font-size: 18px; float: left;"></asp:Label>
                    <br />
                    <br />
                    <asp:TextBox ID="TextBoxLaserNoError" runat="server" Columns="17" Rows="10" 
                        style="width: 290px;" TextMode="MultiLine"></asp:TextBox>
                </div>
                <asp:Button ID="ButtionSave" runat="server" class="button" 
                    onclick="ButtionSave_Click" OnClientClick="return validate();" TabIndex="4" 
                    Text="Save Prefix" Visible="false" />
                &nbsp;&nbsp;
                <asp:Button ID="btnSave" runat="server" class="button" onclick="btnSave_Click" 
                    OnClientClick="return validate();" TabIndex="4" Text="Save" />
                &nbsp;&nbsp;
                <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" />
                &nbsp;&nbsp;
                <input type="reset" class="button" value="Reset" />
            </td>
        </tr>
      </table>
       </div>
       </fieldset>
       </td>
       </tr>
       </table>
   </ContentTemplate>
   </asp:UpdatePanel>
    <br />
</asp:Content>
