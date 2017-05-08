<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MP_CustomerNotCame.aspx.cs" Inherits="HSRP.Transaction.MP_CustomerNotCame" %>





<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
        <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

  
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td valign="top">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td background="../images/midtablebg.jpg" height="27" >
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="4">
                                            <span class="headingmain">MP Data Capture </span>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="topheader">
                                    <tr>
                                        <td>
                                            <table id="t1" width="99%" border="0" align="center" cellpadding="0" 
                                                cellspacing="0" class="topheader" style="font-family: Arial; font-size: 10px">
                                                
                                                <tr>
                                                <td nowrap="nowrap" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" >
                                                    Registration  No :</td>
                                                <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;">       
                                                    <asp:TextBox ID="txt_regno" runat="server"></asp:TextBox></td>
                                                    <td>
                                                    <asp:Button ID="btnGo" runat="server" 
                                                        Text="    Go    " ToolTip="Please Click TO Get The Record"
                                                             onclick="btnGo_Click" CausesValidation="False" 
                                                        BackColor="#00CC33" Font-Names="Arial" Font-Size="Medium"  />
                                                 </td>
                                                 <td nowrap="nowrap" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" >
                                                    &nbsp;
                                                    Cash Recipt Exits&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :</td>
                                                <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        width="100">       
                                                        
                                                    <asp:DropDownList ID="ddl_cashrecipt" Width="100px" runat="server">
                                                    <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem></asp:DropDownList>
                                                 
                                                 </td>
                                                 <td nowrap="nowrap" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Fee Paid&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :</td>
                                                <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        valign="middle">       
                                                        <asp:DropDownList ID="ddl_Fee" Width="100px" runat="server">
                                                        <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <%--<asp:CalendarExtender ID="TextBoxDate_CalendarExtender" runat="server" 
                                                            Enabled="True" TargetControlID="TextBoxDate">
                                                        </asp:CalendarExtender>--%>
                                                     
                                                 </td>
                                              
                                                 <td nowrap="nowrap" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" >
                                                    &nbsp; Paid To&nbsp; : <asp:DropDownList ID="ddl_paidto" Width="100px" runat="server">
                                                    <asp:ListItem>HsrpCenter</asp:ListItem>
                                                    <asp:ListItem>Agent</asp:ListItem>
                                                    <asp:ListItem>Dealer</asp:ListItem>
                                                            </asp:DropDownList></td>
                                                <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        valign="middle">   </td>    
                                                        
                                              <td nowrap="nowrap" align="left" dir="ltr" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;">       
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            
                                                        </ContentTemplate>
                                                      <%--  <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="TextBoxDate" EventName="TextChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlTestLane" 
                                                                EventName="SelectedIndexChanged" />
                                                        </Triggers>--%>
                                                    </asp:UpdatePanel>

                                                           
                                                    </td>
                                                
                                                </tr>
                                                <tr>
                                                <td nowrap="nowrap" 
                                                        style="font-size: small; font-weight: bold; color: #000000" colspan="2" >
                                                            &nbsp;</td>
                                                <td style="font-size: small; font-weight: bold; color: #000000">       
                                                            &nbsp;</td>
                                                <td style="font-size: small; font-weight: bold; color: #000000" valign="middle">       
                                                     
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                                            ControlToValidate="ddlTestLane" ErrorMessage="Please Select Test Lane " 
                                                            Display="Dynamic" InitialValue="--Select Test Lane--"></asp:RequiredFieldValidator>--%>
                                                        
                                                    </td>
                                                <td style="font-size: small; font-weight: bold; color: #000000">       
                                                     &nbsp;</td>
                                                <td style="font-size: small; font-weight: bold; color: #000000">       
                                                     
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                            ControlToValidate="TextBoxDate" ErrorMessage="Please Select Date." 
                                                            Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                                        
                                                    </td>
                                                <td nowrap="nowrap" align="left" dir="ltr" 
                                                        style="font-size: small; font-weight: bold; color: #000000">       

                                                           
                                                            &nbsp;</td>
                                                <td nowrap="nowrap" align="left" dir="ltr" 
                                                        style="font-size: small; font-weight: bold; color: #000000">       

                                                           
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                                ControlToValidate="ddlTimeSlot" ErrorMessage="Please Select Time Slot" 
                                                                InitialValue="--Select Time Slot--" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                                           
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td nowrap="nowrap" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        colspan="1" >Cash Recipt No :</td>
                                                <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;">       
                                                            <asp:TextBox ID="txt_cashrecipt" runat="server"></asp:TextBox>
                                                    </td>
                                                   <td nowrap="nowrap" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        colspan="1" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Rear Laser No :</td>
                                                <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;">       
                                                            <asp:TextBox ID="txt_rearlaser" runat="server"></asp:TextBox>
                                                    </td>

                                                      <td nowrap="nowrap" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        colspan="1" >&nbsp;&nbsp; Front Laser No :</td>


                                               
                                                <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        >       
                                                    <asp:TextBox ID="txt_frontlaser" runat="server" MaxLength="10"></asp:TextBox>
                                                    <br />
                                                    </td>

                                                    
                                                    <td nowrap="nowrap" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        colspan="1" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Remark :</td>


                                               
                                                <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                        >       
                                                    <asp:TextBox ID="txt_remarks" runat="server" MaxLength="10"></asp:TextBox>
                                                    <br />
                                                    </td>


                                               <%-- <td nowrap="nowrap" align="left" dir="ltr" 
                                                        
                                                        style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;">       

                                                           
                                                                &nbsp;</td>--%>
                                                     <td style="font-size: medium; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;">       

                                                           
                                                                <asp:Button ID="btnSave" runat="server" 
                                                                    onclick="btnSave_Click" Text="    Save    " BackColor="#EAB356" 
                                                                    Font-Names="Arial" Font-Size="Medium" />
                                                           
                                                    </td>
                                                <td nowrap="nowrap" align="left" dir="ltr" 
                                                        style="font-size: medium; font-weight: bold; color: #000000">       
                                                    <%--<asp:Button ID="Button1" runat="server" BackColor="#F0AB3A" Font-Names="Arial" 
                                                        Font-Size="Medium" onclick="btnpdf_Click" Text="    Print    " 
                                                        CausesValidation="False" />--%>
                                                    </td>
                                                </tr>
                                                    
                                                <tr>
                                                <td nowrap="nowrap"                                                         
                                                        style="font-size: medium; font-weight: bold; font-family: Arial, Helvetica, sans-serif;" 
                                                        colspan="2" >
                                                            <asp:Label ID="LabelError" runat="server" Font-Names="Tahoma" ForeColor="Red"></asp:Label>
                                                            <asp:Label ID="lblSuccess" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                                        </td>
                                                <td style="font-size: medium; font-weight: bold; font-family: Arial, Helvetica, sans-serif;" 
                                                        valign="middle">       
                                                     
                                                        &nbsp;</td>
                                               
                                                <td style="font-size: small; font-weight: bold; font-family: Arial, Helvetica, sans-serif;">       
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                        ControlToValidate="txtPhone" ErrorMessage="Please Enter Phone No."></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                <td nowrap="nowrap" align="left" dir="ltr" 
                                                        
                                                        
                                                        style="font-size: medium; font-weight: bold; font-family: Arial, Helvetica, sans-serif;">       

                                                           
                                                                &nbsp;</td>
                                                     <td style="font-size: medium; font-weight: bold; font-family: Arial, Helvetica, sans-serif;">       
                                                         <asp:HiddenField ID="BookingDate" runat="server" />
                                                    </td>
                                                <td nowrap="nowrap" align="left" dir="ltr" 
                                                        
                                                        style="font-size: medium; font-weight: bold; font-family: Arial, Helvetica, sans-serif;">       
                                                    <asp:HiddenField ID="BookingTime" runat="server" />
                                                    </td>
                                                </tr>
                                            
                                                <tr>
                                                <td colspan="8">
                                                <asp:Panel ID="Panel1" runat="server" Width="100%" BorderColor="#A6D4A2" 
                                                        BorderStyle="Groove" BorderWidth="3px">

                                                <table id="t11" width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                 
                                                    <tr>
                                                        <td style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; font-weight: normal; color: #0000FF" 
                                                            valign="middle" width="80">
                                                            &nbsp;</td>
                                                        <td style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160">
                                                            &nbsp;</td>
                                                        <td style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="170">
                                                            &nbsp;</td>
                                                        <td style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160">
                                                            &nbsp;</td>
                                                        <td nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="150">
                                                            &nbsp;</td>
                                                        <td nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; font-weight: normal; color: #0000FF" 
                                                            valign="middle" width="80" height="20">
                                                            &nbsp;</td>
                                                        <td style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            Registration No.:
                                                        </td>
                                                        <td style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="170" height="20">
                                                            <asp:Label ID="lblRegNo" runat="server"></asp:Label>
                                                        </td>
                                                        <td style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                           Remarks:
                                                        </td>
                                                        <td nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                            <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                                                        </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                
                                                    <td valign="middle" rowspan="5" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" 
                                                            >
                                                        &nbsp;</td>
                                                        <td rowspan="5" 
                                                            style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            Authorization No:
                                                        </td>
                                                    <td valign="middle" nowrap="nowrap" rowspan="5" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" width="170" 
                                                            height="20">
                                                        <asp:Label Visible="true" runat="server" ID="lblAuthno" />
                                                    </td>
                                    
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Owner Serial No.:
                                                    </td>--%>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblRtoName" />--%>
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>                                                    
                                                    <td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Rto Name:
                                                    </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <asp:Label Visible="true" runat="server" ID="lblRtoName" />
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        No Of Cylinder:
                                                    </td>--%>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblhsrpfeepaid" />--%>
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                       Cash Recipt No Exits:
                                                    </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <asp:Label  Visible="true" runat="server" ID="lblcashreceiptnoexist" />
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Laiden Weight:
                                                    </td>--%>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblLaidenWeight" />--%>
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            Authorization Date:
                                                        </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170">
                                                        <asp:Label Visible="true" runat="server" ID="lblAuthDate" />
                                                    </td>
                                                     <td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Fee Paid:
                                                    </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <asp:Label  Visible="true" runat="server" ID="lblhsrpfeepaid" />
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            Mobile No:
                                                        </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170">
                                                        <asp:Label Visible="true" runat="server" ID="lblMobileNo" />
                                                    </td>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Colour:
                                                    </td>--%>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblColor" />--%>
                                                    </td>



                                                
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>



                                                
                                                    </tr>
                                                    <tr>
                                                
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            Sent Date Time:
                                                        </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170">
                                                        <asp:Label  Visible="true" runat="server" ID="lblSentDateTime" />
                                                    </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Paid To:
                                                    </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <asp:Label Visible="true" runat="server" ID="lblpaidto" />
                                                    </td>

                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>

                                                    </tr>
                                                    <tr>
                                                
                                                    <td valign="middle" rowspan="5" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" 
                                                            >
                                                        &nbsp;</td>
                                                        <td rowspan="5" 
                                                            style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            Server Response:
                                                        </td>
                                                    <td valign="middle" nowrap="nowrap" rowspan="5" 
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="170" 
                                                            >
                                                        <asp:Label  Visible="true" runat="server" ID="lblServerResponse" />
                                                    </td>
                                                    <tr>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Fuel:
                                                    </td>--%>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblFuel" />--%>
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Cash Recipt No:
                                                    </td>
                                                     <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <asp:Label  Visible="true" runat="server" ID="lblcashreceiptno" />
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Fitness Valid Upto:
                                                    </td>--%>
                                                     <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblFitness" />--%>
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                       Rear Laser Code:
                                                    </td>
                                                     <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <asp:Label  Visible="true" runat="server" ID="lblhsrp_rear_lasercode" />
                                                    </td>

                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>

                                                    </tr>
                                                    </tr>
                                                    <tr>
                                                
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            Sent Status:
                                                        </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170" >
                                                        <asp:Label  Visible="true" runat="server" ID="lblSentStatus" />
                                                    </td>
                                               

                                                    </tr>
                                                    <tr>
                                                
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            Call Recieved On:
                                                        </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170" >
                                                        <asp:Label  Visible="true" runat="server" ID="lblCallRecievedOn" />
                                                    </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        Front Laser Code:
                                                    </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <asp:Label  Visible="true" runat="server" ID="lblhsrp_front_lasercode" />
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                        <tr>
                                                            <td height="20" 
                                                                style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                                valign="middle" width="80">
                                                                &nbsp;</td>
                                                            <td height="20" style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                                valign="middle" width="160">
                                                                &nbsp;</td>
                                                            <td height="20" nowrap="nowrap" 
                                                                
                                                                
                                                                style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                                width="170">
                                                                &nbsp;</td>
                                                            <td height="20" nowrap="nowrap" 
                                                                
                                                                
                                                                style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                                width="160">
                                                                &nbsp;</td>
                                                            <td height="20" nowrap="nowrap" 
                                                                
                                                                
                                                                style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                                width="150">
                                                                &nbsp;</td>
                                                            <td height="20" nowrap="nowrap" 
                                                                
                                                                style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                                width="80">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td height="20" 
                                                                style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                                valign="middle" width="80">
                                                                &nbsp;</td>
                                                            <%--<td height="20" 
                                                                style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif; text-decoration: underline;" 
                                                                valign="middle" width="160">Description &amp; Size Of Tyres
                                                            </td>--%>
                                                            <td height="20" nowrap="nowrap" 
                                                                
                                                                
                                                                style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                                width="170">
                                                            </td>
                                                            <%--<td height="20" nowrap="nowrap" 
                                                                style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif; text-decoration: underline;" 
                                                                valign="middle" width="160">Registered Axle Weight
                                                            </td>--%>
                                                            <td height="20" nowrap="nowrap" 
                                                                
                                                                
                                                                style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                                width="150">
                                                            </td>
                                                            <td height="20" nowrap="nowrap" 
                                                                
                                                                style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                                width="80">
                                                                &nbsp;</td>
                                                        </tr>
                                                    <tr>
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                        </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170" >
                                                    </td>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                    </td>
                                                    </tr>
                                                    </tr>
                                                    <tr>
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <%--<td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            (a) Front Axle Size:
                                                        </td>--%>
                                                   <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170" >
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblFrontTyreSize" />--%>
                                                   </td>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        (a) Front Axle Size:
                                                    </td>--%>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblFrontAxleReg" />--%>
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <%--<td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            (b) Rear Axle Size:
                                                        </td>--%>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" width="170" 
                                                            height="20" >
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblRearAxleSize" />--%>
                                                    </td>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        (b) Rear Axle Size:
                                                    </td>--%>
                                                    <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblRearAxleReg" />--%>
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <%--<td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            (c) Any Other Axle Size:
                                                        </td>--%>
                                                     <td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170" >
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblOtherAxleSize" />--%>
                                                    </td>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        (c) Any Other Axle Size:
                                                    </td>--%>
                                                     <td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <%--<asp:Label  Visible="true" runat="server" ID="lblOtherAxleReg" />--%>
                                                    </td>
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                    <td valign="middle" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" >
                                                        &nbsp;</td>
                                                        <%--<td style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="middle" width="160" height="20">
                                                            (d) Tandom Axle Size:
                                                        </td>--%>
                                                     <%--<td valign="middle" nowrap="nowrap" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" height="20" 
                                                            width="170" >
                                                        <asp:Label  Visible="true" runat="server" ID="lblTandonSize" />
                                                    </td>--%>
                                                    <%--<td valign="middle" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20">
                                                        (d) Tandom Axle Size:
                                                    </td>--%>
                                                     <%--<td valign="middle" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150">
                                                        <asp:Label  Visible="true" runat="server" ID="lblTandonReg" />
                                                    </td>--%>
                                                
                                                        <td height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="middle" 
                                                            width="80">
                                                            &nbsp;</td>
                                                
                                                    </tr>                                                   
                                                    <tr>
                                                    <td  align="left" valign="top" 
                                                            
                                                            style="font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #0000FF; font-weight: normal;" 
                                                            width="80" height="20" 
                                                            >
                                                            &nbsp;</td>
                                                        <td align="left" 
                                                            style="font-size: 14px; color: #0000FF; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" 
                                                            valign="top" width="160" height="20">
                                                            &nbsp;</td>
                                                    <td align="left" 
                                                            
                                                            
                                                            style="font-size: 14px; color: #000000; font-weight: normal; font-family: Arial, Helvetica, sans-serif;" valign="top" 
                                                            height="20" width="170" >
                                                        <br />
                                                        </td>
                                                    <td  align="left" valign="top" nowrap="nowrap" 
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" 
                                                            width="160" height="20" >
                                                        &nbsp;</td>
                                                        <%--<td  align="left" valign="top" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #000000; font-family: Arial, Helvetica, sans-serif;" 
                                                            height="20" width="150" >
                                                            
                                                           
                                                            &nbsp;</td>--%>
                                                            <td align="left" height="20" nowrap="nowrap" 
                                                            
                                                            style="font-size: 14px; font-weight: normal; color: #0000FF; font-family: Arial, Helvetica, sans-serif;" valign="top" 
                                                            width="80">
                                                                &nbsp;</td>
                                                            </tr>
                                                   
                                                    </table>
                                                    </asp:Panel>
                                                    </td>
                                                    </tr>


                                                    
                                                
                                                    <tr>
                                                        <td colspan="8" style="font-size: medium; font-weight: bold; color: #000000">
                                                            &nbsp;</td>
                                                   
                                                    </tr>                                             
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td
            </tr>
        </table>
    </asp:Content>
