<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="DailyAttendenceSheet.aspx.cs" Inherits="HSRP.Transaction.DailyAttendenceSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
            <tr>
                <td>
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="27" style="background-image: url(../images/midtablebg.jpg)">
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="26">
                                            <span class="headingmain">Daily Attendance Sheet</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="3" cellspacing="1">
                                    <tr>
                                        <td height="26" bgcolor="#FFFFFF" class="maintext">
                                            <table width="98%" border="0" align="center" cellpadding="3" cellspacing="3">
                                                <tr>
                                                    <td>
                                                        <span id="lblMsg" class="header"></span>
                                                    </td>
                                                    <td style="width: 13%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="label1" runat="server" Text="EmpName:"></asp:Label>
                                                    </td>
                                                    <td style="width: 13%">
                                                       <%-- <asp:TextBox ReadOnly="true" class="form_textbox" MaxLength="30" ID="textBoxUserName"
                                                            runat="server"></asp:TextBox>--%>
                                                        <asp:DropDownList ID="ddlZone" Height="20px" Width="228px" Tabindex="1"
                                                    runat="server" DataTextField="emp_name" AutoPostBack="false"
                                                    DataValueField="emp_id" >
                                                           
                                                </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="labelembcode" runat="server" Text="Emb.Code:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  MaxLength="10" TabIndex="1"
                                                            ID="txtEmbcode" runat="server"></asp:TextBox>
                                                        <br />
                                                    </td>
                                                </tr>--%>
                                               <%-- <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lblDesignation" runat="server" Text="Designation:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td width="30%">
                                                        <asp:TextBox class="form_textbox"  MaxLength="10" TabIndex="2"
                                                            ID="txtDesignation" runat="server"></asp:TextBox>
                                                     
                                                   </td>
                                                </tr>--%>
                                                <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="lbltimein" runat="server" Text="Time IN:"></asp:Label>
                                                        <span class="alert">* </span>
                                                    </td>
                                                    <td style="width: 13%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="txtTimein" runat="server" ></asp:TextBox>
                                                        
                                                        <%--<asp:RegularExpressionValidator ValidationExpression="^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$" ID="StartTimeValidator" runat="server" ValidationGroup="MyGroup"  ErrorMessage="You Must Supply Start Time in Correct format"  ControlToValidate="txtTimein" SetFocusOnError="False"></asp:RegularExpressionValidator>--%>
                                                   </td>
                                                    <td>
                                                        <asp:Label ID="lblam" runat="server" Text="AM"></asp:Label>
                                                      
                                                    </td>


                                                  <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtTimein" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                 
                                                 <%-- <asp:RegularExpressionValidator ID="revTime" runat="server" ControlToValidate="txtTimein" ErrorMessage="hh:mi" SetFocusOnError="True" ValidationExpression="^(23):(00)|([01][0-9]|2[0-3]):([0-5][0-9])$" ValidationGroup="MyGroup"></asp:RegularExpressionValidator>--%>


                                                </tr>

                                                 <tr>
                                                    <td width="20%" class="form_text">
                                                        <asp:Label ID="label2" runat="server" Text="Time OUT:"></asp:Label>
                                                        
                                                    </td>
                                                    <td style="width: 13%">
                                                        <asp:TextBox class="form_textbox"  TabIndex="3" MaxLength="10"
                                                            ID="txttimeout" runat="server" ></asp:TextBox>
                                                        <%--<asp:RegularExpressionValidator ValidationExpression="^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$" ID="EndTimeValidator" runat="server"  ErrorMessage="You Must Supply an END Time in Correct format"  ControlToValidate="txttimeout"></asp:RegularExpressionValidator>--%>
                                                   </td>
                                                     <td>
                                                        <asp:Label ID="Label3" runat="server" Text="PM"></asp:Label>
                                                        
                                                    </td>

                                                      <%--<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txttimeout" Type="Integer"
                                                         ErrorMessage="CompareValidator" ForeColor="Red" Operator="DataTypeCheck">Numbers Only are allowed..</asp:CompareValidator>--%>
                                                     
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 45px" class="form_text" align="center" colspan="2">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:Button ID="buttonSave" runat="server" Text="Save" class="button"  OnClick="buttonSave_Click" />&nbsp;&nbsp;
                                                        &nbsp;&nbsp;
                                                        <input type="reset" id="Reset" value="Reset" class="button" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="alert">
                                                        * Fields are mandatory.
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" class="FieldText">
                                                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
