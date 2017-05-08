<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="HSRP.Transaction.Inventory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script> 
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />


    <script language="javascript" type="text/javascript">
        function validate() {
            
            if (document.getElementById("<%=DropdownStateName.ClientID%>").value == "--Select State--") {
                alert("Select State Name");
                document.getElementById("<%=DropdownStateName.ClientID%>").focus();
                return false;
            }
            var leng = document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").value;
            
            if (leng.length <"8") {
                alert("Enter Minimum 8 Character");
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

    <style type="text/css">
        .modalPopup
        {
            background-color: #FFFFFF;
            filter: alpha(opacity=40);
            opacity: 0.7;
            xindex: -1;
        } 
    </style>

    <%--<script type="text/javascript">
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
    </script>--%>
   
</head>
<body>
    <form id="form1" runat="server">
   
    <div style="margin: 20px; background-color: #FFFFFF; " align="left">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    <asp:Label ID="LabelFormName" runat="server" Text="Label"></asp:Label>
                </div>
            </legend>
            <div style="margin: 20px;" align="left">
     
   <div>
   
        <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
         <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress" PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup" />
        <tr>
                <td  class="form_text" style="padding-bottom: 10px">
                    <asp:Label ID="LabelStateName" runat="server" Text=" ">State Name : <span class="alert">* </span></asp:Label></td>
                <td > <asp:DropDownList ID="DropdownStateName" DataTextField="HSRPStateName" Width="150px"
                        DataValueField="HSRP_StateID" class="dropdown_css" runat="server" 
                          AutoPostBack="True" 
                        onselectedindexchanged="DropdownStateName_SelectedIndexChanged"> 
                    </asp:DropDownList>
                </td> 
                <td></td>
            <td  class="form_text" style="padding-bottom: 10px"> <asp:Label ID="LabelRTOLocation" runat="server" Text=" ">Location : <span class="alert">* </span></asp:Label> </td>
               <td>
                    <asp:DropDownList ID="DropdownRTOName" DataTextField="RTOLocationName"  
                        DataValueField="RTOLocationID" Width="180px"
                        runat="server" TabIndex="1" AutoPostBack="True" 
                        onselectedindexchanged="DropdownRTOName_SelectedIndexChanged">
                        <asp:ListItem>--Select RTO Location--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="form_text" style="padding-bottom: 10px"> Product Name : <span class="alert">* </span></td> &nbsp; &nbsp;
                <td> <asp:DropDownList ID="DropDownListProductName" runat="server"  Width="150px"
                        DataTextField="ProductCode" DataValueField="ProductID" AutoPostBack="True"  > </asp:DropDownList> </td>
                <td></td>
                 <td class="form_text" style="padding-bottom: 10px"> Laser Start No. : <span class="alert">* </span></td>
                <td> <asp:TextBox ID="textboxLaserCodeFrom" class="form_textbox" runat="server" 
                        MaxLength="8" ></asp:TextBox> </td>
            </tr>
            <tr>
            
                <td  class="form_text" style="padding-bottom: 10px"> Prefix Laser Code 
                    From : <span class="alert">* </span></td>
                <%--<td > <asp:TextBox ID="textboxPrefixFrom" class="form_textbox" runat="server"  TabIndex="2"></asp:TextBox> </td>--%>
                <td>  <asp:DropDownList ID="DropDownListPrefixLaserNo" DataTextField="Prefix"  Width="150px"
                        DataValueField="PrefixID" class="dropdown_css" runat="server" 
                          AutoPostBack="True" > 
                    </asp:DropDownList> </td>
                <td></td>
                <td class="form_text" style="padding-bottom: 10px"> Quantity : <span class="alert">* </span></td>
                <td > <asp:TextBox ID="textboxQuantity" class="form_textbox" MaxLength="15" onkeypress="return isNumberKey(event)" runat="server" TabIndex="1"></asp:TextBox> </td>
                
            </tr>
            <%--<tr >
             <td  class="form_text" style="padding-bottom: 10px"> Weight : <span class="alert">* </span></td>
                <td  > <asp:TextBox ID="textboxWeight" class="form_textbox"  onkeypress="return isNumberKey(event)"  runat="server" TabIndex="5"></asp:TextBox> </td> 
                <td></td>
                <td class="form_text" style="padding-bottom: 10px"> Current Cost : <span class="alert">* </span></td>
                <td > <asp:TextBox ID="textboxCurrentCost" class="form_textbox" runat="server" TabIndex="4"></asp:TextBox> </td>
            </tr>--%>
            <tr>
             <td  class="form_text" style="padding-bottom: 10px"> Invoice No : </td>
                <td  > <asp:TextBox ID="textboxInvNo" class="form_textbox"  runat="server" TabIndex="5"></asp:TextBox> </td> 
                <td></td>
                <td class="form_text" style="padding-bottom: 10px"> Invoice From : </td>
                <td > <asp:TextBox ID="textboxInvFrom" class="form_textbox" runat="server" TabIndex="4"></asp:TextBox> </td>
            </tr>
          
            
             
            <tr>
                 
                <td class="form_text" style="padding-bottom: 10px"> Remarks : </td> 
                <td colspan="4" valign="top" onmouseup="HSRPAuthDate_OnMouseUp()">
                                                                <asp:TextBox ID="textboxRemarks" class="form_textbox" runat="server" Columns="25" Rows="15" Height="85px" Width="600px"
                                                                    TabIndex="6" TextMode="MultiLine"></asp:TextBox> 
                                                                <br />
                                                                <%--<ComponentArt:Calendar ID="HSRPAuthDate" runat="server" Visible="false" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                                    ControlType="Picker" PickerCssClass="picker">
                                                                    <ClientEvents>
                                                                        <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                                    </ClientEvents>
                                                                </ComponentArt:Calendar>--%>
                                                            &nbsp;&nbsp;
                                                                <img id="ImgPollution" alt="" class="calendar_button" style="visibility:hidden" 
                                                                    onclick="HSRPAuthDate_OnClick()" onmouseup="HSRPAuthDate_OnMouseUp()" 
                                                                    src="../images/btn_calendar.gif" tabindex="4" /></td>
            </tr>
            
            <tr>
            <td colspan="6"></td>
            </tr>
             <tr>
             <td> </td>
             </tr>
             <tr>
                  
                         
                <td colspan="5" align="right" > 
                    <asp:UpdateProgress ID="UpdateProgress" style="display: none; position: absolute; height: 220px; left: 120px; background-color: white;" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                            <ProgressTemplate >
                            <asp:Image ID="load_img" runat="server" Height="70px" Width="70px" ImageUrl="~/img/ajax-loader.gif" Visible="true" AlternateText="Processing" />
                    </ProgressTemplate> 
                    </asp:UpdateProgress>
                    
                     <asp:UpdatePanel ID="UpdatePanel1"  UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                        
                        <div style="border:0px solid; position:static;float: left; width:250px;">
                        <asp:Label ID="LabelUpdated"   runat="server" ForeColor="Blue" style="font-size: 18px; float: left;" Font-Size="18px"></asp:Label> 
                         <asp:Label ID="lblTotalRecord" runat="server" ForeColor="Blue"   style="font-size: 18px; float: left;" Font-Size="18px"></asp:Label>  <br />
                             
                         <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue"  style="font-size: 18px; float: left;" Font-Size="18px"></asp:Label>  <br />
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red"  style="font-size: 18px; float: left;" Font-Size="18px"></asp:Label>  <br />
                          <asp:Label ID="lblExist" runat="server" ForeColor="Red" style="font-size: 18px; float: left;"  Font-Size="18px"></asp:Label> <br /> <br />
                          
                           <asp:TextBox ID="TextBoxLaserNoError" runat="server" Columns="17" Rows="10" style="width: 290px;"
                              TextMode="MultiLine"></asp:TextBox>

                              </div>
                              <asp:Button ID="ButtionSave" runat="server" Text="Save Prefix" TabIndex="4" Visible="false"
                                class="button"  OnClientClick="return validate();" onclick="ButtionSave_Click"  
                             />&nbsp;&nbsp;

                            <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="4" class="button"  OnClientClick="return validate();"  
                            onclick="btnSave_Click" />&nbsp;&nbsp;

                                   
                   <%-- <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" visible="false" /> &nbsp;&nbsp;--%>
                    
                    <%--<input type="reset" class="button" value="Reset" />--%>

                         </ContentTemplate>
                        </asp:UpdatePanel>
                   
                </td>
            </tr>
            <tr>
            <td>
              
                          
                </td>
            </tr>
 

           
        </table>
    </div> 
    </div>
        </fieldset>
    </div>
    </form>
</body>
</html>