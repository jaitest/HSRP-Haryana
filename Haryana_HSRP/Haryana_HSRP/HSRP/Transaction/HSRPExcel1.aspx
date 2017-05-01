<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="HSRPExcel1.aspx.cs" Inherits="HSRP.Transaction.HSRPExcel1" %>

  
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
  <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
 <div style="margin: 20px; width:55%;background-color:transparent;float:left;position:fixed" align="left">
        <fieldset>
            <legend>
                    Upload Excel
            </legend>
            <div style="margin: 20px;" align="left">
                <div>
                    <table width="100%" border="0" align="left" cellpadding="3" cellspacing="1">
                       <%-- <tr>
                            <td class="form_text">
                                Date :<span class="alert">* </span>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td valign="top" onmouseup="OrderDate_OnMouseUp()" align="left">
                                            <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                ControlType="Picker" PickerCssClass="picker">
                                                <ClientEvents>
                                                    <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                </ClientEvents>
                                            </ComponentArt:Calendar>
                                        </td>
                                        <td valign="top" align="left">
                                            <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                          <tr>
                            <td class="form_text">
                                State Name :<span class="alert">* </span>
                            </td>
                            <td style="padding-left: 10px">
                                <asp:TextBox ID="txtStateName" BorderColor="Transparent" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                RTO Location Name :<span class="alert">* </span>
                            </td>
                            <td style="padding-left: 10px">
                                <asp:DropDownList ID="ddlRtoLocation" runat="server" Width="180">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top" class="form_text">
                                Select File:<span class="alert">* </span>
                            </td>
                            <td colspan="3">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form_text">
                                Remarks :
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" Height="68px" Width="291px"></asp:TextBox>
                            </td>
                        </tr>
                        <%--  <tr>
                            <td class="form_text">
                                ChequeDeliveredDate
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td valign="top" onmouseup="HSRPAuthDate1_OnMouseUp()" align="left">
                                            <ComponentArt:Calendar ID="CheckDeliveredDate" runat="server" PickerFormat="Custom"
                                                PickerCustomFormat="dd/MM/yyyy" ControlType="Picker" PickerCssClass="picker">
                                                <ClientEvents>
                                                    <SelectionChanged EventHandler="HSRPAuthDate1_OnDateChange" />
                                                </ClientEvents>
                                            </ComponentArt:Calendar>
                                        </td>
                                        <td valign="top" align="left">
                                            <img id="Img1" tabindex="4" alt="" onclick="HSRPAuthDate1_OnClick()" onmouseup="HSRPAuthDate1_OnMouseUp()"
                                                class="calendar_button" src="../images/btn_calendar.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="right" colspan="2" style="margin-right:300px">
                                <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                                <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <%--  <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8" OnClientClick=" javascript:return vali();"
                                    class="button" OnClick="buttonUpdate_Click" />--%>&nbsp;&nbsp;
                                    <%--OnClientClick=" javascript:return vali();"--%>
                                <asp:Button ID="btnSave" runat="server" Text="Upload Excel" TabIndex="4" class="button"
                                     OnClick="btnSave_Click" />&nbsp;&nbsp;
                              <%--  <input type="button" visible="false" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                    id="buttonClose" value="Close" class="button" />--%>
                                &nbsp;&nbsp;
                                <%--<input type="reset" class="button" value="Reset" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="alert">
                                * Fields are mandatory.
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </fieldset>
    </div>
   
</asp:Content>
