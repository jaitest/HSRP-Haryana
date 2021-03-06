﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ReadyforInstallationCashinHand.aspx.cs" Inherits="HSRP.Master.ReadyforInstallationCashinHand" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script> 
    <script src="../javascript/common.js" type="text/javascript"></script>
     
    <script src="../Scripts/User.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    
   
     

    

   <script type="text/javascript">

       function removeSpaces(string) {
           return string.split(' ').join('');
       }

       function remove(string) {
           return trims(string);
       }



       function trims(s) {
           return rtrim(ltrim(s));
       }

       function ltrim(s) {
           var l = 0;
           while (l < s.length && s[l] == ' ')
           { l++; }
           return s.substring(l, s.length);
       }

       function rtrim(s) {
           var r = s.length - 1;
           while (r > 0 && s[r] == ' ')
           { r -= 1; }
           return s.substring(0, r + 1);
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
     

    <script language="javascript" type="text/javascript">
        function ValidationQuick() {


//            if (document.getElementById("<%=txtboxLaserNo.ClientID%>").value == "") {
//                alert("Provide Front Laser Plate Code");
//                document.getElementById("<%=txtboxLaserNo.ClientID%>").focus();
//                return false;
//            }
//            if (invalidChar(document.getElementById("<%=txtboxLaserNo.ClientID%>"))) {
//                //        
//                alert("Special Characer Not Allow!!");
//                document.getElementById("<%=txtboxLaserNo.ClientID%>").focus();
//                return false;
//            }

//            if (document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").value == "") {
//                alert("Provide Front Laser Plate Code");
//                document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").focus();
//                return false;
//            }
//            if (invalidChar(document.getElementById("<%=textboxLaserCodeFrom.ClientID%>"))) {
//                //        
//                alert("Special Characer Not Allow!!");
//                document.getElementById("<%=textboxLaserCodeFrom.ClientID%>").focus();
//                return false;
//            }
        }

        function ValidationPrefixVisible() {

            var dd = document.getElementById("<%=RadioButtonIndualPlate.ClientID%>").checked;
            var ID = document.getElementById("<%=RadioButtonCompleteBox.ClientID%>").checked;

         if (dd == true) {
             
//             document.getElementById("<%=lblPlateI.ClientID%>").innerHTML = "Plate I";
//             document.getElementById("<%=DropDownListPrefixLaserNo.ClientID%>").style.visibility = 'hidden';
             document.getElementById("<%=divFirst.ClientID%>").style.visibility = 'visible';
             document.getElementById("<%=divSecond.ClientID%>").style.visibility = 'hidden';

             document.getElementById("<%=divThird.ClientID%>").style.visibility = 'hidden';
             
         }
         if (ID == true) {
             
//             document.getElementById("<%=lblPlateI.ClientID%>").innerHTML = "Start Laser No Without Prefix";
//             document.getElementById("<%=DropDownListPrefixLaserNo.ClientID%>").style.visibility = 'visible';
             document.getElementById("<%=divFirst.ClientID%>").style.visibility = 'hidden';
             document.getElementById("<%=divSecond.ClientID%>").style.visibility = 'visible';
             document.getElementById("<%=divThird.ClientID%>").style.visibility = 'visible';
           
         }
             
        } 
     </script>

    <asp:UpdatePanel ID="updatepannel1" runat="server">
      <ContentTemplate>
     <%--<div id="DivBorder" runat="server" class="assignQuick" >--%>
     <div style="margin: 20px;" align="left">
        <fieldset>
            <legend>
                <div align="left" style="margin-left: 10px; font-size: medium; color: Black">
                    Blank Plate Stock in Hand
                </div>
            </legend>
            <br />
    <table style="background-color:White; height:223px"  width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
      
       <tr>
                        <td class="form_text_View"  style="padding-bottom: 10px">
                         <b>  <font color="black">    </font>  <asp:Label ID="lbltotalSavedRecord" runat="server" CssClass="Label_user_batch" Visible="false" > </asp:Label></b>
                        </td>
                        <td colspan="0">
                            <asp:RadioButton ID="RadioButtonIndualPlate"  Text="Save Single Plate" AutoPostBack="true"
                               Checked="true" runat="server" 
                                GroupName="LaserNo" 
                                oncheckedchanged="RadioButtonIndualPlate_CheckedChanged1"  />
                            </td>
                            <td>
                            <asp:RadioButton ID="RadioButtonCompleteBox" Text="Save Complete box" AutoPostBack="True"
                                   runat="server" GroupName="LaserNo" oncheckedchanged="RadioButtonCompleteBox_CheckedChanged1" 
                                       /></td>
                        <td>
                        
                            
                    <%--<asp:Button ID="btnGO" runat="server"   class="button"  Text="GO" 
                                TabIndex="4"  OnClientClick=" return validate()" onclick="btnGO_Click"  />--%>
                        </td>   
                    
                         
                    </tr> 

                <%--<tr>
                        <td class="form_text_View"  style="padding-bottom: 10px; width: 233px;">
                           <font color="black" size="3px"> Vehicle Registration No. :
                            <span class="alert">* </span> </font>
                        </td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtVehicleRegNo" TabIndex="0" onblur="this.value=removeSpaces(this.value);" Style="text-transform: uppercase" runat="server" CssClass="form_textbox11"></asp:TextBox>   &nbsp;&nbsp;    
                        </td>   
                    
                    </tr> --%>
                    <tr id="divFirst"   runat="server">
                        <td class="form_text_View" style="padding-bottom: 10px"> <font color="black"  size="3px"><asp:label ID="Label1" runat="server">Laser No<span class="alert">* </span></asp:label></font> </td> 
                         
                        <td> <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="txtboxLaserNo"  EnableViewState="true" runat="server"></asp:TextBox> </td>   
                   </tr>
                    <tr id="divSecond"  runat="server">
                        <td class="form_text_View" style="padding-bottom: 10px"> <font color="black"  size="3px"><asp:label ID="lblPlateI" runat="server">Prefix :<span class="alert">* </span></asp:label></font> 
                         
                        </td>
                        
                    <td> 
                            <asp:DropDownList ID="DropDownListPrefixLaserNo" runat="server"  AutoPostBack="True" class="dropdown_css" DataTextField="Prefix" onchange ="return ValidationPrefixVisible()"  DataValueField="PrefixID" TabIndex="3" Width="150px">
                            </asp:DropDownList> 
                        </td> 
                        
                         <td class="form_text_View" style="padding-bottom: 10px"> <font color="black"  size="3px"><asp:label ID="Label2" runat="server">Laser No<span class="alert">* </span></asp:label></font> 
                         
                        </td>
                        <td>                              
                        <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="textboxLaserCodeFrom"  onkeypress="return isNumberKey(event)" EnableViewState="true" runat="server"></asp:TextBox>                                
                        </td>   
                   </tr>

                   <tr id="divThird"   runat="server">
                   <td></td>
                   <td></td>
                        <td class="form_text_View" style="padding-bottom: 10px"> <font color="black" size="3px"><asp:label ID="lblPlateII" runat="server">Total Plate<span class="alert">* </span></asp:label> </font>  </td>
                        
                        <td>
                           <%--<asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="textBoxRearPlate" onlosefocus=" return sameLaserNo();" runat="server"></asp:TextBox>--%>
                           <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="textboxQuantity" onkeypress="return isNumberKey(event)"  onlosefocus=" return sameLaserNo();" runat="server"></asp:TextBox>
                         </td>  
                    </tr>
                    <tr> <td colspan="4">
                    
                    </td></trt> <tr>
                        <td colspan="4">
                        </td>
                        </trt>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                               <%-- <asp:Button ID="btnSave" runat="server" class="button" onclick="btnSave_Click" 
                                    OnClientClick=" return ValidationQuick()" TabIndex="4" Text="Confirm Stock in Hand" />--%>
                                     
                                <asp:LinkButton ID="btnSaveStockinHand" runat="server" class="button" 
                                    onclick="btnSaveStockinHand_Click" 
                                    TabIndex="4" Text="Save"></asp:LinkButton> 
                                &nbsp;&nbsp;
                               <%-- <input type="reset" class="button" value="Reset" />--%>
                                 <asp:LinkButton ID="btnSaveStockinHand0" runat="server" class="button" Visible="false"
                                    onclick="btnSaveStockinHand_Click"  
                                    TabIndex="4" Text="Save Plate"></asp:LinkButton> &nbsp;&nbsp;
                                 <asp:LinkButton ID="LinkButton1" runat="server" class="button"  
                                    TabIndex="4" Text="Reset" onclick="LinkButton1_Click"></asp:LinkButton>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br />
                                
                            </td>
                            <td>
                                

                            </td>
                        </tr>
                        <tr>
                            <td style="width:350px">
                                <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                <asp:Label ID="lblTotalRecord" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblErrMessvehicleFront" runat="server" Font-Size="12px" 
                                    ForeColor="Red"></asp:Label>
                            </td>
                            <td align="center" colspan="2">
                                &nbsp;<asp:Label ID="lblErrMessvehicle" runat="server" Font-Size="12px" 
                                    ForeColor="Red"></asp:Label>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                        <td>
                        <asp:TextBox ID="TextBoxLaserNoError" runat="server" Columns="17" Rows="10" style="width: 290px;"
                              TextMode="MultiLine"></asp:TextBox>
                        </td>
                        </tr>
                     <tr> 
                         <%--<td colspan="7" style="background:yellow"; > <font color="black" > <i> Please Make Sure to do the New order booking entry on <b>Daily Production Entry</b> screen. </i>  </font> </td>--%>
                        <asp:HiddenField ID="hiddenfieldStateID" runat="server" />
                        <asp:HiddenField ID="hiddenfieldRTOLocationID" runat="server" />
                        <asp:HiddenField ID="hiddenfieldFrontFrontCode" runat="server" />
                        <asp:HiddenField ID="hiddenfieldRearCode" runat="server" />
                        <asp:HiddenField ID="hiddenfieldHSRPRecordID" runat="server" />
                </table>
     </div>
                                 </table>
    </fieldset>
                 </ContentTemplate>
                             </asp:UpdatePanel>
    <br />
</asp:Content>
