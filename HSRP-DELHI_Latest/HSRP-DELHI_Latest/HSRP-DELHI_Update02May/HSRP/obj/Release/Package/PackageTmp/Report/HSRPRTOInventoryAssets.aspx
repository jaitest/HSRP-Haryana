<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="HSRPRTOInventoryAssets.aspx.cs" Inherits="HSRP.Master.HSRPRTOInventoryAssets"%>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../javascript/common.js"></script>
    
   
    <%--<script language="javascript" type="text/javascript">

        function validate() {

            if (document.getElementById("<%=DepositDate.ClientID%>").value == "") {
                alert("Please Provide Deposit Date ");
                document.getElementById("<%=DepositDate.ClientID%>").focus();
                return false;
            }




            if (document.getElementById("<%=DropDownListBankName.ClientID%>").value == "0") {
                alert("Please Select  Bank Name ");
                document.getElementById("<%=DropDownListBankName.ClientID%>").focus();
                return false;
            }
            if (document.getElementById("<%=ddl_user.ClientID%>").value == "0") {
                alert("Please Select User Name ");
                document.getElementById("<%=ddl_user.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=ddlLocation.ClientID%>").value == "0") {
                alert("Please Select  Location Name ");
                document.getElementById("<%=ddlLocation.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=txtBranchName.ClientID%>").value == "") {
                alert("Please Provide  Branch Name ");
                document.getElementById("<%=txtBranchName.ClientID%>").focus();
                return false;
            }





            if (document.getElementById("<%=TextBoxDepositAmount.ClientID%>").value == "") {
                alert("Please Provide Deposit Amount ");
                document.getElementById("<%=TextBoxDepositAmount.ClientID%>").focus();
                return false;
            }




            if (document.getElementById("<%=TextBoxDepositby.ClientID%>").value == "") {
                alert("Please Provide Deposited by ");
                document.getElementById("<%=TextBoxDepositby.ClientID%>").focus();
                return false;
            }



            if (invalidChar(document.getElementById("TextBoxBankSlipNo"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("TextBoxBankSlipNo").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxDepositAmount"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("TextBoxDepositAmount").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxDepositby"))) {
                alert("Special Characters Not Allowed in Deposited by");
                document.getElementById("TextBoxDepositby").focus();
                return false;
            }



        }

        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function ischarKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 96 || charCode > 122) && (charCode < 65 || charCode > 90) && (charCode < 31 || charCode > 33))
                return false;

            return true;
        }

        //                var TCode = document.getElementById("TextBoxDepositby").value;
        //                if (/[^a-zA-Z]/.test(TCode)) {
        //                    alert('Input is not alphanumeric');
        //                    return false;
        //                }
        //                return true;


        //            }
              

  

 </script>--%>
    <style type="text/css">
        .style4
        {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 210px;
            padding-left: 20px;
        }
        .auto-style1 {
            color: black;
            font-weight: normal;
            text-decoration: none;
            nowrap: nowrap;
            font-style: normal;
            font-variant: normal;
            font-size: 11pt;
            line-height: normal;
            font-family: tahoma, arial, verdana;
            width: 210px;
            padding-left: 20px;
            height: 18px;
        }
        .auto-style2 {
            height: 18px;
        }
    </style>

    

      <div style=" width:100%;">
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    HSRP Assets At Center</div>
            </legend>
            <br />
            <br />
              <div style="width: 100%; margin: 0px auto 0px auto">


    <table  border="0" align="right"  style="height: 348px; width:85%;">
               <tr>
                    <td class="style4">
                        RTO Name <span class="alert">* </span>
                    </td>
                    <td>
                        <table id="Table1" runat="server" style="margin-left: 8px;" align="left" cellspacing="0"
                            cellpadding="0" border="0">
                            <tr>
                                <td valign="top" onmouseup="DepositDate_OnMouseUp()">
                                    <%--<ComponentArt:Calendar ID="DepositDate" runat="server" PickerFormat="Custom" 
                                        PickerCustomFormat="dd/MM/yyyy"  TabIndex="1"
                                        ControlType="Picker" PickerCssClass="picker" Visible="true" >
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="DepositDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>--%>
                                    <asp:Label ID="LblRTO" runat="server" ></asp:Label>
                                </td>
                                <td style="font-size: 10px;">
                                    &nbsp;
                                </td>
                                <td valign="top">
                                    <%--<img id="calendar_from_button" TabIndex="2" alt="" onclick="DepositDate_OnClick()"
                                        onmouseup="DepositDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />--%>
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                     <td class="style4"><i>(logged in user RTO Name)</i></td>
                          <td>
                  
                 
                                    &nbsp;</td>
                </tr>
                    <tr>
                <td class="style4">
                         Emb Center Name <span class="alert"> * </span>
                    </td>
                         <td class="style4">
                        <asp:Label ID="lblembname" runat="server" ></asp:Label>
                    </td>
                           <td class="style4"><i>(logged in user RTO Embossing Center Name)</i></td>
                    </tr>
                    <td class="style4">
                        Product
                        <%-- <span class="alert">* </span>--%>
                        *
                        <%-- <span class="alert">* </span>--%>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListProduct" runat="server" 
                            Style="margin-left: 8px" TabIndex="4" Width="165px" AutoPostBack="true" OnSelectedIndexChanged="DropDownListProduct_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select Product--</asp:ListItem>
                          
                            <asp:ListItem Value="1">DataCard</asp:ListItem>
                            <asp:ListItem Value="2">Laptop</asp:ListItem>
                            <asp:ListItem Value="3">Desktop</asp:ListItem>
                            <asp:ListItem Value="4">MiniDesktop</asp:ListItem>
                            <asp:ListItem Value="5">Inverter</asp:ListItem>
                            <asp:ListItem Value="6">UPS</asp:ListItem>
                            <asp:ListItem Value="7">Printer</asp:ListItem>
                            <asp:ListItem Value="8">LaserPrinter</asp:ListItem>
                            <asp:ListItem Value="9">ThirdStickerPrinter</asp:ListItem>
                            <asp:ListItem Value="10">Mobile</asp:ListItem>
                          <asp:ListItem Value="11">Modem</asp:ListItem>
                        </asp:DropDownList>

                    </td>

                     <%--<td class="style4">
                       Cheque.No
                        <%-- <span class="alert">* </span>--%>
                    </td>
                    <td class="style4"><i>(Product at&nbsp; available at center)</i></td>
                </tr>
                <tr>
                    <td class="style4">
                        Make <span class="alert"> *</span>
                    </td>
                    <td>
                        <%-- <asp:TextBox ID="txtBankName" runat="server" class="form_textbox12" MaxLength="30"></asp:TextBox>--%>
                        <asp:TextBox ID="txtBoxMake" runat="server" class="form_textbox12" MaxLength="30"></asp:TextBox>
                         
                    </td>

                        <td class="style4"><i>(Make Of the Product like Dlink/Spectra/RDP/Acer/Lenovo/BSNL/Sify etc)</i></td>
                      <td class="style4">
                       &nbsp;<%-- <span class="alert">* </span>--%></td>
                    &nbsp; &nbsp; &nbsp; &nbsp;
                      <td valign="top" onmouseup="DepositDate1_OnMouseUp()">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td style="font-size: 10px;">
                                    &nbsp;
                                </td>
                               <%-- <td valign="top">
                                    <td valign="top">
                                   
                                </td>
                                </td>--%>
                </tr>
             

                 <tr>
                    <td class="style4">
                        Model 
                        <%--<span class="alert"> </span>--%>
                        * 
                        <%--<span class="alert">* </span>--%>
                    </td>
                    <td>

                    <asp:TextBox ID="TextBoxModel" runat="server" class="form_textbox12" MaxLength="30"></asp:TextBox>
                       

                
                    </td>


                        <td class="style4">
                           <i>(Model of the Product )</i></td>
                    <td>
                        &nbsp;</td>
                </tr>
                  <tr>
                    <td class="auto-style1">
                       Serial No./Mobile Number
                        * 
                        <%--<span class="alert">* </span>--%>
                    </td>
                    <td class="auto-style2">

                    <asp:TextBox ID="txtBoxSerial" runat="server" class="form_textbox12" MaxLength="30" ></asp:TextBox>
                       

                
                    </td>


                        <td class="auto-style1">
                           <i>(Serial Number Of the product)</i></td>
                    <td class="auto-style2">
                        </td>
                </tr>
                  

                   <tr>
                    <td class="style4">
                        Working/Live Status&nbsp;&nbsp;
                        <span class="alert">* </span>
                    </td>
                    <td>
                       
                        <asp:DropDownList ID="DropDownListStation" runat="server" 
                            Style="margin-left: 8px" TabIndex="4" Width="165px" AutoPostBack="true" 
                            onselectedindexchanged="DropDownListProduct_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                          
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2">No</asp:ListItem>
                           
                        </asp:DropDownList>
                
                    </td>
                       <td class="auto-style1">
                           <i>(Live Status Of the product)</i></td>



<%--                     <td class="style4">
                        Deposit Type <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDepositType" class="form_textbox12" runat="server" 
                            onkeypress="return isNumberKey(event)"></asp:TextBox>
                    </td>--%>
                </tr>

                <tr>
                    <td class="style4">
                       User Name <span class="alert"> *</span>
                    </td>
                    <td>
                  
                        <asp:TextBox ID="TextBoxUser" class="form_textbox12" runat="server" 
                            MaxLength="30" TabIndex="7" ></asp:TextBox>
                    </td>
                    <td class="auto-style1">
                           <i>(User Name)</i></td>
                </tr>
                <tr>
                    <td class="style4">
                        User Mobile No <span class="alert">* </span>
                    </td>
                    <td>
                  
                        <asp:TextBox ID="TextBoxMobile" class="form_textbox12" runat="server" 
                            MaxLength="30" TabIndex="7" ></asp:TextBox>
                    
                    </td>

                    <td class="auto-style1">
                           <i>(Mobile No of the User)</i></td>

                </tr>
                <tr>
                    <td class="style4">
                        &nbsp;</td>
                    <td>
                  
                        <%--<asp:DropDownList ID="ddlLocation" runat="server" 
                            DataTextField="RTOLocationName" DataValueField="RTOLocationID" Height="20px" 
                            style="margin-left: 10px" Width="163px">
                        </asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        Remarks 
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxRemarks" runat="server" Height="56px" 
                            class="form_textbox12" TextMode="MultiLine" MaxLength="30"
                            Width="223px" TabIndex="8"></asp:TextBox>
                    </td>
                </tr>
              
            </table>
            <div style=" clear:both">
            </div>
            <table border="0"  cellpadding="2" cellspacing="4" width="490px" align="center" >
            
             <tr>
                  <td class="style4">
                                                           
                                                            <%--<asp:Button ID="buttonUpdate" runat="server" TabIndex="10" class="button" Visible="false"
                                                                 Text="Update" onclick="buttonUpdate_Click"  OnClientClick="return validate()" 
                                                                 />--%>
                                                            &nbsp;<asp:Button ID="buttonSave" runat="server" TabIndex="9" class="button" Text="Save"
                                                                OnClientClick=" return validate()" Visible="false" 
                                                                OnClick="ButtonSubmit_Click" />&nbsp;&nbsp;<%--
                                                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                                id="buttonClose" value="Close" class="button" TabIndex="11"/>--%>
                                                                <asp:Button ID="btnclose" runat="server" TabIndex="9" class="button" 
                                                                Text="Close" onclick="btnclose_Click" />
                                                            &nbsp;&nbsp;
                                                            <%-- <input type="reset" class="button" value="Reset" />--%>
                                                            <asp:Button ID="btnReset" runat="server"  class="button" Text="Reset" 
                                                                onclick="btnReset_Click" TabIndex="12" />
                                                        </td>
                    
                
                    
                </tr>

                  <tr>
                  <td class="style4">
                   <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                  <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                      </td>

              </tr>

              
           </table>
            </div>
           
           <table>
             <tr>
                  <td>  &nbsp;</td>
                        <td>
                            &nbsp;</td>
                </tr>



                  <tr>
                  <td>  &nbsp;</td>
                        <td>
                            &nbsp;</td>
                </tr>

           </table>
</asp:Content>
