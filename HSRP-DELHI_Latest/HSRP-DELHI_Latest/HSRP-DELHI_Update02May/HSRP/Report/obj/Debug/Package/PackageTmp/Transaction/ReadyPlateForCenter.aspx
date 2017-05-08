<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="ReadyPlateForCenter.aspx.cs" Inherits="HSRP.Master.ReadyPlateForCenter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>

    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
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
            if (document.getElementById("<%=txtVehicleRegNo.ClientID%>").value) {
                alert("Please Provide Vehicle Reg. No");
                document.getElementById("<%=txtVehicleRegNo.ClientID%>").focus();
                return false;
            }
            

        } 

    </script>
     

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

  
            if (document.getElementById("<%=txtVehicleRegNo.ClientID%>").value == "") {
                alert("Provide Vehicle Reg. No.");
                document.getElementById("<%=txtVehicleRegNo.ClientID%>").focus(); 
                return false;
            }
            if (invalidChar(document.getElementById("<%=txtVehicleRegNo.ClientID%>"))) {
                //        
                alert("Special Characer Not Allow!!");
                document.getElementById("<%=txtVehicleRegNo.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=textBoxFrontPlate.ClientID%>".trim()).value == "") {
                alert("Provide Plate I");
                document.getElementById("<%=textBoxFrontPlate.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=textBoxFrontPlate.ClientID%>"))) {
                //        
                alert("Special Characer Not Allow!!");
                document.getElementById("<%=textBoxFrontPlate.ClientID%>").focus();
                return false;
            } 

            if (document.getElementById("<%=textBoxRearPlate.ClientID%>").value == "") {
                alert("Provide Plate II");
                document.getElementById("<%=textBoxRearPlate.ClientID%>").focus();
                return false;
            }
            if (invalidChar(document.getElementById("<%=textBoxRearPlate.ClientID%>"))) {
                //        
                alert("Special Characer Not Allow!!");
                document.getElementById("<%=textBoxRearPlate.ClientID%>").focus();
                return false;
            }

            var FrontPlate = document.getElementById("<%=textBoxFrontPlate.ClientID%>").value;
            var RearPlate = document.getElementById("<%=textBoxRearPlate.ClientID%>").value;

            if (FrontPlate == RearPlate) {
                alert("Same Laser Plate Can't Save!!");
                document.getElementById("<%=textBoxFrontPlate.ClientID%>").focus();
                return false;
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
                    Stock in Hand
                </div>
            </legend>
            <br />
    <table style="background-color:White; height:223px"  width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
      
       <tr>
                        <td class="form_text_View"  style="padding-bottom: 10px">
                         <b>  <font color="black">    </font>  <asp:Label ID="lbltotalSavedRecord" runat="server" CssClass="Label_user_batch" Visible="false" > </asp:Label></b>

                        </td>
                        <td></td>
                        <td>
                            
                    <%--<asp:Button ID="btnGO" runat="server"   class="button"  Text="GO" 
                                TabIndex="4"  OnClientClick=" return validate()" onclick="btnGO_Click"  />--%>
                        </td>   
                    
                         
                    </tr> 

                <tr>
                        <td class="form_text_View"  style="padding-bottom: 10px; width: 233px;">
                           <font color="black" size="3px"> Vehicle Registration No. :
                            <span class="alert">* </span> </font>
                        </td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtVehicleRegNo" TabIndex="0" onblur="this.value=removeSpaces(this.value);" Style="text-transform: uppercase" runat="server" CssClass="form_textbox11"></asp:TextBox>   &nbsp;&nbsp;    
                    <%--<asp:Button ID="btnGO" runat="server"   class="button"  Text="GO" 
                                TabIndex="4"  OnClientClick=" return validate()" onclick="btnGO_Click"  />--%>
                        </td>   
                    
                    </tr> 
                    <tr>
                        <td class="form_text_View" style="padding-bottom: 10px"> <font color="black"  size="3px">Plate I: <span class="alert">* </span></font> 
                         
                        </td>
                        <td></td>
                        <td>  
                                                               
                        <asp:TextBox class="form_textbox11" TabIndex="1" Width="150" ID="textBoxFrontPlate" onblur="this.value=removeSpaces(this.value);"  EnableViewState="true" runat="server"></asp:TextBox>
                                                                     
                        </td>   
                   </tr>
                   <tr>
                        <td class="form_text_View" style="padding-bottom: 10px"> <font color="black" size="3px">Plate II : <span class="alert">* </span></font>  </td>
                        <td></td>
                        <td>
                                                                <asp:TextBox class="form_textbox11" onblur="this.value=removeSpaces(this.value);"  TabIndex="1" Width="150" ID="textBoxRearPlate" onlosefocus=" return sameLaserNo();"
                                                                    runat="server"></asp:TextBox>
                                                                 
                                                            </td>   
                    </tr>
                    <tr> <td colspan="4"></td></trt> <tr>
                        <td colspan="4">
                        </td>
                        </trt>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                               <%-- <asp:Button ID="btnSave" runat="server" class="button" onclick="btnSave_Click" 
                                    OnClientClick=" return ValidationQuick()" TabIndex="4" 
                                    Text="Confirm Stock in Hand" />--%>
                                <asp:LinkButton ID="btnSaveStockinHand" runat="server" class="button" 
                                    onclick="btnSaveStockinHand_Click" OnClientClick=" return ValidationQuick()" 
                                    TabIndex="4" Text="Confirm Stock in Hand"></asp:LinkButton>
                                &nbsp;&nbsp;
                                <%--<input type="reset" class="button" value="Reset" />--%>
                                <asp:LinkButton ID="LinkButton1" runat="server" class="button"  TabIndex="5" 
                                    Text="Reset" onclick="LinkButton1_Click"></asp:LinkButton>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
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
                         <%--<td colspan="7" style="background:yellow"; > <font color="black" > <i> Please Make Sure to do the New order booking entry on <b>Daily Production Entry</b> screen. </i>  </font> </td>--%>
                        <asp:HiddenField ID="hiddenfieldStateID" runat="server" />
                        <asp:HiddenField ID="hiddenfieldRTOLocationID" runat="server" />
                        <asp:HiddenField ID="hiddenfieldFrontFrontCode" runat="server" />
                        <asp:HiddenField ID="hiddenfieldRearCode" runat="server" />
                        <asp:HiddenField ID="hiddenfieldHSRPRecordID" runat="server" />
                </table>
     </div>
                                 </table>
                 </ContentTemplate>
                             </asp:UpdatePanel>
    <br />
    </fieldset>
</asp:Content>
