<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LaserAssignCode.aspx.cs" Inherits="HSRP.Master.LaserAssignCode" %>

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
    
    
   
    <script language="javascript" type="text/javascript">
        function sameLaserNo() {


        }

        function GetProjectManagerDetails(sender, eventArgs) {
            var item = eventArgs.get_item();
            var UserID = item.get_value();
            MyWebService.GetProjectManagerDetailByUserID(UserID, BindProjectManager);
        }

        function validate() {

            var ds = document.getElementById("LabelOrderType").innerHTML;
           // alert(ds);
            if (ds == "NB" || ds == "OB" || ds == "DB") { 
                    if (document.getElementById("<%=textBoxFrontPlate.ClientID%>").value == "") {
                        alert("Please Provide Front Plate Laser No");
                        document.getElementById("<%=textBoxFrontPlate.ClientID%>").focus();
                        return false;
                    }

                    if (document.getElementById("<%=textBoxLaserPlateBoxNo.ClientID%>").value == "") {
                        alert("Please Provide Laser Plate Box No");
                        document.getElementById("<%=textBoxLaserPlateBoxNo.ClientID%>").focus();
                        return false;
                    }

                    if (document.getElementById("<%=textBoxRearPlate.ClientID%>").value == "") {
                        alert("Please Provide Rear Plate Laser No");
                        document.getElementById("<%=textBoxRearPlate.ClientID%>").focus();
                        return false;
                    } 
                }
                if (document.getElementById("<%=textBoxFrontPlate.ClientID%>").value == document.getElementById("<%=textBoxRearPlate.ClientID%>").value) {
                    alert("Same Laser No Could Not be Assigned!!");
                }
                if (ds == "DF") {
                     
                    if (document.getElementById("<%=textBoxFrontPlate.ClientID%>").value == "") {
                        alert("Please Provide Front Plate Laser No");
                        document.getElementById("<%=textBoxFrontPlate.ClientID%>").focus();
                        return false;
                    }
                }
                if (ds == "DR") {

                    if (document.getElementById("<%=textBoxRearPlate.ClientID%>").value == "") {
                        alert("Please Provide Rear Plate Laser No");
                        document.getElementById("<%=textBoxFrontPlate.ClientID%>").focus();
                        return false;
                    }

                    if (document.getElementById("<%=textBoxLaserPlateBoxNo.ClientID%>").value == "") {
                        alert("Please Provide Laser Plate Box No");
                        document.getElementById("<%=textBoxLaserPlateBoxNo.ClientID%>").focus();
                        return false;
                    }

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
            width: 150px !important;
        }
        #divwidth div
        {
            width: 150px !important;
        }
    
select
{
  font-family: Verdana; 
  font-size:11px;
}
  

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Laser Code Assign 
                </div>
            </legend>
            <div style="width: 880px; margin: 0px auto 0px auto">
             <table style="background-color:White" width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                <tr>
                <td colspan="7">
                    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
                    </asp:ScriptManager>
                    </td> 
                </tr>
                    <tr>
                        <td class="form_text_View"  style="padding-bottom: 10px">
                            HSRP Authorization No :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelAuthorizationNo" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            State Name :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelStateID" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            HSRP Authorization Date :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelAuthorizationDate" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Order Book Date :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelOrderBookDate" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            HSRP Embossing Date :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelEmbossingDate" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            HSRP Invoice Date :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelInvoiceDate" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>


                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px"> RTO Location Name : </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelRTOLocationID" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Vehicle Owner Name :</td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelVehicleOwnerName" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Mobile No :</td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelMobileNo" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Vehicle Class :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelVehicleClass" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Vehicle Type:
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelVehicleType" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Vehicle Registration No :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelVehicleRegNo" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Engine Number :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelEngineNumber" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                   <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Chasis No :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelChasisNo" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Cash Receipt No : 
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelCashReceiptNo" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Total Amount :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelTotalAmount" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Front Plate Size And Color :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelFrontPlateSizeAndColor" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Rear Plate Size And Color :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelRealPlateSizeAndColor" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Vehicle Maker :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="labelvehicleMaker" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                            Vehicle Model :
                        </td>
                        <td></td>
                        <td>
                            <asp:Label ID="labelvehicleModel" runat="server" class="form_text_View" ></asp:Label>
                        </td>   
                    </tr>
                    
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px">
                        <asp:Label ID="LabelFrontPlateLeserNo" runat="server" class="form_text_View" Text=" Front Plate Laser No : " ></asp:Label> <span class="alert">* </span>
                           
                        </td>
                        <td></td>
                        <td>  
                                                               
                                                               <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="textBoxFrontPlate" EnableViewState="true" 
                                                                    runat="server"></asp:TextBox>
                                                                     
                                                                <div id="divwidth">
                                                                </div>
                                                                 
                                                               <asp:AutoCompleteExtender ServiceMethod="LaserNo" MinimumPrefixLength="1" ServicePath="~/WCFService/ServiceForSuggestion.svc"
                                                                    CompletionInterval="10" EnableCaching="false" TargetControlID="textBoxFrontPlate" UseContextKey = "true"
                                                                    ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false" CompletionSetCount="12"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                                                    CompletionListElementID="divwidth">
                                                                </asp:AutoCompleteExtender>
                                                            </td>  
                   <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px">
                      <asp:Label ID="LabelRearPlateLaserNo" runat="server" class="form_text_View" Text=" Rear Plate Laser No :"></asp:Label> <span class="alert">* </span>
                             
                        </td>
                        <td></td>
                        <td>
                                                                <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="textBoxRearPlate" onlosefocus=" return sameLaserNo();"
                                                                    runat="server"></asp:TextBox>
                                                                <div id="div1">
                                                                </div>
                                                                 
                                                               <asp:AutoCompleteExtender ServiceMethod="RearLaserNo" MinimumPrefixLength="1" ServicePath="~/WCFService/ServiceForSuggestion.svc"
                                                                    CompletionInterval="10" EnableCaching="false" TargetControlID="textBoxRearPlate" UseContextKey = "true"
                                                                    ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false" CompletionSetCount="12"
                                                                    CompletionListCssClass="AutoExtender" CompletionListItemCssClass="AutoExtenderList"
                                                                    CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                                                    CompletionListElementID="div1">
                                                                </asp:AutoCompleteExtender>
                                                            </td>   
                    </tr>
                    <tr>
                   <td class="form_text_View" style="padding-bottom: 10px"> Laser Plate Box No. </td> 
                        <td  > </td> 
                        <td class="form_text_View" style="padding-bottom: 10px">
                            <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" Text="0" ID="textBoxLaserPlateBoxNo" EnableViewState="true" 
                                                                    runat="server"></asp:TextBox>
                          </td>
                        <td></td>

                        <td class="form_text_View" style="padding-bottom: 10px"> Order Type: </td> 
                        <td  > </td> 
                        <td class="form_text_View" style="padding-bottom: 10px">
                            <asp:Label ID="LabelOrderType" runat="server" class="form_text_View" ></asp:Label>
                          </td>
                          
                         
                    </tr>
                     <tr>
                      <td class="form_text_View" style="padding-bottom: 10px"> <asp:Label ID="LabelSticker" runat="server" class="form_text_View" Text="Sticker :"></asp:Label></td> 
                        <td  > </td> 
                        <td class="form_text_View" style="padding-bottom: 10px">
                            <asp:CheckBox ID="CheckBoxSticker" Enabled="false" runat="server" />
                          </td>
                        <td></td>

                        <td class="form_text_View" style="padding-bottom: 10px"> Embossing Done By Operator :<span class="alert"> </span>
                             
                         </td> 
                        <td  > </td> 
                        <td class="form_text_View" style="padding-bottom: 10px">
                                             <asp:DropDownList AutoPostBack="false" Visible="true" ID="dropDownListOperator" 
                                                    CausesValidation="false" Width="140px"
                                                    runat="server" DataTextField="OperatorName" 
                                                    DataValueField="OperatorID" 
                                                     >
                                                </asp:DropDownList>
                                                        </td>
                          
                         
                    </tr>
                    <tr>
                      <%--<td  class="form_text_View" style="padding-bottom: 10px"> Remarks :</td> --%>
                        <td></td>
                            <%--<td colspan="5">
                            <asp:Label ID="lblRemarks"  Width="600px" runat="server" class="form_text_View" ></asp:Label>
                        </td> --%>
                           
                    </tr>


                     <tr>
                       <td> <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                             <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>

                            
                             <asp:Label ID="lblErrMessvehicleFront" runat="server" ForeColor="Red" 
                                 Font-Size="12px"></asp:Label>

                            
                    </td>
                        <td colspan="3" align="center">

                            
                        &nbsp;<asp:Label ID="lblErrMessvehicle" runat="server" ForeColor="Red" Font-Size="12px"></asp:Label>

                            
                         </td>  
                        <td colspan="3" align="right" > 

                       
                    <asp:Button ID="btnSave" runat="server"   class="button"  Text="Assign & Embossing" TabIndex="4"  OnClientClick=" return validate()"
                            onclick="btnSave_Click" />&nbsp;&nbsp;
                     <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose" id="buttonClose" value="Close" class="button" /> 
                   <%-- <asp:Button ID="btnClose" runat="server" class="button"  Text="Close" TabIndex="4" 
                                onclick="btnClose_Click"  />--%> &nbsp;&nbsp;
                    <input type="reset" class="button" value="Reset" />
                </td> 
                    </tr>
                    <asp:HiddenField ID="hiddenfieldStateID" runat="server" />
                    <asp:HiddenField ID="hiddenfieldRTOLocationID" runat="server" />
                    <asp:HiddenField ID="hiddenfieldFrontFrontCode" runat="server" />
                    <asp:HiddenField ID="hiddenfieldRearCode" runat="server" />
                </table>
            </div>
        </fieldset>
    </div>
     
    </form>
</body>
</html>
