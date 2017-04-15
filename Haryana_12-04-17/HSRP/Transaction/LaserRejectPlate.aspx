<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LaserRejectPlate.aspx.cs" Inherits="HSRP.Master.LaserRejectPlate" %>

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

        if (document.getElementById("<%=dropDownListHSRPState.ClientID%>").value == "--Select State--") {
            alert("Please Select State");
            document.getElementById("<%=dropDownListHSRPState.ClientID%>").focus();
            return false;
        }

        if (document.getElementById("<%=dropDownListRTOLocation.ClientID%>").value == "--Select RTO Location--") {
            alert("Please Select RTOLocation");
            document.getElementById("<%=dropDownListHSRPState.ClientID%>").focus();
            return false;
        }

        if (document.getElementById("<%=txtLaserNoRejected.ClientID%>").value == "") {
            alert("Please Enter Laser No");
            document.getElementById("<%=txtLaserNoRejected.ClientID%>").focus();
            return false;
        }

        if (document.getElementById("<%=textBoxVehicleRegNo.ClientID%>").value == "") {
            alert("Please Enter Vehicle Reg. No");
            document.getElementById("<%=textBoxVehicleRegNo.ClientID%>").focus();
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
                    <tr>
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
                                                                    
                                                                </asp:UpdatePanel>--%>
                                </tr>
                                 <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap"> </td>
                                <td valign="middle" class="form_text" nowrap="nowrap" colspan="3"><br /> </td>
                                </tr>
                               
                                 <tr>
                                <td valign="middle" class="form_text" nowrap="nowrap">Remarks :<span class="alert">* </span></td> 
                                <td valign="middle" class="form_text" nowrap="nowrap" colspan="3"> <asp:TextBox ID="txtRemarks" Width="622px" Height="125px" TextMode ="MultiLine" runat="server"></asp:TextBox> </td>
                                </tr> 
                   
                     <tr>
                       
                        <td   align="center"> <br /><br />
                         
                           <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                             <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                    </td> 
                        <td colspan="2" align="right" >
                       
                            <asp:Button ID="buttonRejectedSave" class="button" runat="server" OnClientClick=" return validate()" 
                                Text="SAVE Rejected LASER NO" onclick="buttonRejectedSave_Click"  />

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
