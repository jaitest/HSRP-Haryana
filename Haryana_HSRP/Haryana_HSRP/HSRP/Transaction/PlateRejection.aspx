<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlateRejection.aspx.cs" Inherits="HSRP.Master.PlateRejection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>

<script language="javascript" type="text/javascript">
    function validate() {

        if (document.getElementById("<%=ddlEntryType.ClientID%>").value == "--Select Order Type--") {
            alert("Please Select Order Type");
            document.getElementById("<%=ddlEntryType.ClientID%>").focus();
            return false;
        }

        if (document.getElementById("<%=ddlRejectType.ClientID%>").value == "--Select Reject Type--") {
            alert("Please Select Reject Type");
            document.getElementById("<%=ddlRejectType.ClientID%>").focus();
            return false;
        }



        if (document.getElementById("<%=txtVehicleNo.ClientID%>").value == "") {
            alert("Please Enter Vehicle Reg. No");
            document.getElementById("<%=txtVehicleNo.ClientID%>").focus();
            return false;
        }

        if (document.getElementById("<%=txtRemarks.ClientID%>").value == "") {
            alert("Please Enter Remarks");
            document.getElementById("<%=txtRemarks.ClientID%>").focus();
            return false;
        }

        var message = document.getElementById("<%=txtRemarks.ClientID%>").value;
        var newMessage = new String('*', message.Length);
        


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
        .style5
        {
            color: Black;
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            text-decoration: none;
            nowrap: nowrap;
            height: 32px;
        }
        .style6
        {
            height: 32px;
        }
        .style7
        {
            color: Black;
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            text-decoration: none;
            nowrap: nowrap;
            height: 19px;
        }
        .style8
        {
            height: 19px;
        }
    </style>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Script" runat="server"></asp:ScriptManager>

    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Laser Reject
                </div>
            </legend>
            <div style="width: 880px; margin: 0px auto 0px auto">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                <tr>
                <td colspan="7">&nbsp;</td> 
                </tr>
                    <%--<tr>
                       <td valign="middle" class="form_text" nowrap="nowrap">
                             <asp:Label Text="Select HSRP State:" Visible="true" runat="server" ID="labelHSRPState" /><span class="alert">* </span>
                        </td>
                                    <td valign="middle">
                                        <asp:DropDownList AutoPostBack="true" Visible="true" Width="125px" CausesValidation="false" ID="dropDownListHSRPState"
                                            runat="server" DataTextField="HSRPStateName" DataValueField="HSRP_StateID" OnSelectedIndexChanged="dropDownListHSRPState_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="middle" class="form_text">
                                        <asp:UpdatePanel runat="server" ID="UpdateRTOLocation" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label Text="Location:" Visible="true" runat="server" ID="labelRTOLocation" /> <span class="alert">* </span>&nbsp;&nbsp;
                                                <asp:DropDownList Visible="true" Width="165px" ID="dropDownListRTOLocation" CausesValidation="false"
                                                    runat="server" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="dropDownListHSRPState" EventName="SelectedIndexChanged" />
                                                <asp:PostBackTrigger ControlID="dropDownListRTOLocation" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                 <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap"> </td>
                                <td valign="middle" class="form_text" nowrap="nowrap" colspan="3"> </td>
                                </tr>
                                 <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap"> </td>
                                <td valign="middle" class="form_text" nowrap="nowrap" colspan="3"><br /> </td>
                                </tr>
                                 <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap">Rejected Laser No :<span class="alert">* </span></td>
                                <td valign="middle" class="form_text" nowrap="nowrap" colspan="0"> <asp:TextBox ID="txtLaserNoRejected" MaxLength="16" Width="222px" runat="server"></asp:TextBox> 
                                    </td>
                                 <td valign="middle" class="form_text" nowrap="nowrap">Vehicle Reg. No :<span class="alert">* </span> 
                                     <asp:TextBox ID="textBoxVehicleRegNo" runat="server" class="form_textbox11" 
                                onlosefocus=" return sameLaserNo();" TabIndex="1" Width="150"></asp:TextBox></td>
--%>                                    
                                    <%--<asp:UpdatePanel runat="server" ID="UpdatePanelAuthNo" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox class="form_textbox11" onblur="this.value=removeSpaces(this.value);"
                                                                            Style="text-transform: uppercase" TabIndex="1" Width="160" ID="textBoxAuthorizationNo"
                                                                            runat="server"></asp:TextBox>
                                                                      <div class="demo">
                                                                            <div class="ui-widget">
                                                                                <asp:TextBox ID="tbAuto" class="tb" runat="server">
                                                                                </asp:TextBox>
                                                                            </div>
                                                                        </div> 
                                                                        <div id="divwidth">
                                                                        </div>
                                                                        <asp:AutoCompleteExtender UseContextKey="true" ServiceMethod="LaserNoPlateRejection" MinimumPrefixLength="1"
                                                                            ServicePath="~/WCFService/ServiceForSuggestion.svc" CompletionInterval="10" EnableCaching="false"
                                                                            TargetControlID="textBoxAuthorizationNo" ID="AutoCompleteExtender1" runat="server"
                                                                            FirstRowSelected="false" CompletionSetCount="12" CompletionListCssClass="AutoExtender"
                                                                            CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                            CompletionListElementID="divwidth">
                                                                        </asp:AutoCompleteExtender>
                                                                    </ContentTemplate>
                                                                    
                                                                </asp:UpdatePanel>--%><%--                                </tr>--%>
<tr>
<td valign="middle" class="style5" nowrap="nowrap">Vehicle No :<span class="alert">* </span></td> 
 <td valign="middle" class="style5" nowrap="nowrap" colspan="0"> 
     <asp:TextBox ID="txtVehicleNo" MaxLength="16" Width="140px" runat="server"></asp:TextBox> 
                                    
  &nbsp;&nbsp;
      <asp:Button ID="btnGo" runat="server" Text="Go" class="button" 
         onclick="Button1_Click" />   
                                    
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HiddenField ID="HiddenField3" 
         runat="server" />
&nbsp;<asp:HiddenField ID="HiddenField2" runat="server" />
                                    
     <asp:HiddenField ID="HiddenField4" runat="server" />
                                    
  </td>
  <td class="style6">
      </td>
</tr>
<tr>
<td valign="middle" class="form_text" nowrap="nowrap">Order Type :<span class="alert">*</span></td>
<td>
    <asp:Label ID="lblordertype" runat="server" Text="Label"></asp:Label>
    <br />
</td>
</tr>
<tr>
<td valign="middle" class="style7" nowrap="nowrap">Front Laser No :<span class="alert">* </span></td> 
<td class="style8">
    <asp:Label ID="lblFLNo" runat="server" Text="Label" Visible="true"></asp:Label>
    <br />
    </td>
</tr>
                               
                                 <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap"> Rear Laser No :<span class="alert">* </span> </td>
                                <td class="style8">
                                    <asp:Label ID="lblRLNo" runat="server" Text="Label" Visible="true"></asp:Label>
                                    <br />
                                    <br /> </td>
                                </tr>
                               <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap">Entry Type :<span class="alert">* </span> </td>
                                <td>
                                    <asp:DropDownList ID="ddlEntryType" runat="server" 
                                        onclientclick=" javascript:return vali();" ValidationGroup="v">
                                        <asp:ListItem>--Select Entry Type-- </asp:ListItem>
                                        <asp:ListItem>Embossing</asp:ListItem>
                                    </asp:DropDownList>
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ControlToValidate="ddlEntryType" ErrorMessage="Please Select Entry Type" 
                                        InitialValue="--Select Entry Type-- " ValidationGroup="v"></asp:RequiredFieldValidator>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                </td>
                               </tr>
                                <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap">Reject Type :<span class="alert">* </span> </td>
                                <td>
                                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>--%>
                                            <asp:DropDownList ID="ddlRejectType" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="ddlRejectType_SelectedIndexChanged" 
                                                ontextchanged="ddlRejectType_SelectedIndexChanged" 
                                        ValidationGroup="v">
                                                <asp:ListItem>--Select Reject Type--</asp:ListItem>
                                                <asp:ListItem>Front Plate</asp:ListItem>
                                                <asp:ListItem>Rear Plate</asp:ListItem>
                                                <asp:ListItem>Both</asp:ListItem>
                                            </asp:DropDownList>
                                        <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="ddlRejectType" ErrorMessage="Please Select Reject Type" 
                                        InitialValue="--Select Reject Type--" ValidationGroup="v"></asp:RequiredFieldValidator>
                                </td>
                               </tr>

                    <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap">Rejection Type :<span class="alert">* </span> </td>
                                <td>
                                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>--%>
                                            <asp:DropDownList ID="ddlrejectiontype" runat="server" AutoPostBack="True"  ValidationGroup="v">
                                                <asp:ListItem>--Select Rejection Type--</asp:ListItem>
                                                <asp:ListItem>Rejection</asp:ListItem>
                                                <asp:ListItem>Missing</asp:ListItem>
                                                <asp:ListItem>Shifted</asp:ListItem>
                                                
                                            </asp:DropDownList>
                                        <%--</ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                        ControlToValidate="ddlrejectiontype" ErrorMessage="Please Select Rejection Type" 
                                        InitialValue="--Select Rejection Type--" ValidationGroup="v"></asp:RequiredFieldValidator>
                                </td>
                               </tr>
                                 <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap">Remarks :<span class="alert">* </span></td> 
                                <td valign="middle" class="form_text" nowrap="nowrap" colspan="3"> <asp:TextBox ID="txtRemarks" Width="622px" Height="125px" TextMode ="MultiLine" runat="server"></asp:TextBox> </td>
                                </tr> 
                    <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap">Operator Name :<span class="alert">* </span></td> 
                                <td valign="middle" class="form_text" nowrap="nowrap" colspan="3">  
                                    <asp:TextBox ID="txtoperatorname" runat="server" Width="153px"></asp:TextBox></td>
                                </tr>
                     <tr>
                       
                        <td   align="center"> <br /><br />
                         
                           <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                             <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                    </td> 
                        <td colspan="2" align="right" >
                       
                            <asp:Button ID="buttonRejectedSave" class="button" runat="server" OnClientClick=" return validate()" 
                                Text="SAVE Rejected LASER NO" onclick="buttonRejectedSave_Click" 
                                ValidationGroup="v"  />

                     <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> 
                    
                    <input type="reset" class="button" value="Reset" />
                </td> 
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
    </form>
</body>
</html>
