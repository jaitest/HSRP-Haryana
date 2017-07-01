<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MOProductivity.aspx.cs"
    Inherits="HSRP.Transaction.MOProductivity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../javascript/common.js"></script>
    <script type="text/javascript">
        function DepositDate_OnDateChange(sender, eventArgs) {
            var fromDate = DepositDate.getSelectedDate();
            CalendarDepositDate.setSelectedDate(fromDate);

        }

        function DepositDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarDepositDate.getSelectedDate();
            DepositDate.setSelectedDate(fromDate);

        }

        function DepositDate_OnClick() {
            if (CalendarDepositDate.get_popUpShowing()) {
                CalendarDepositDate.hide();
            }
            else {
                CalendarDepositDate.setSelectedDate(DepositDate.getSelectedDate());
                CalendarDepositDate.show();
            }
        }

        function DepositDate_OnMouseUp() {
            if (CalendarDepositDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function boxbatchRelesedDate_OnDateChange(sender, eventArgs) {
            var fromDate = boxbatchRelesedDate.getSelectedDate();
            CalendarboxbatchRelesedDate.setSelectedDate(fromDate);

        }

        function CalendarboxbatchRelesedDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarboxbatchRelesedDate.getSelectedDate();
            boxbatchRelesedDate.setSelectedDate(fromDate);

        }

        function boxbatchRelesedDate_OnClick() {
            if (CalendarboxbatchRelesedDate.get_popUpShowing()) {
                CalendarboxbatchRelesedDate.hide();
            }
            else {
                CalendarboxbatchRelesedDate.setSelectedDate(boxbatchRelesedDate.getSelectedDate());
                CalendarboxbatchRelesedDate.show();
            }
        }

        function boxbatchRelesedDate_OnMouseUp() {
            if (CalendarboxbatchRelesedDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function validate() {

            if (document.getElementById("<%=DropDownListPlantName.ClientID%>").value == "0") {
                alert("Please Provide Plant Name ");
                document.getElementById("<%=DropDownListPlantName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListMachine.ClientID%>").value == "0") {
                alert("Please Provide Machine Name  ");
                document.getElementById("<%=DropDownListMachine.ClientID%>").focus();
                return false;
            }



            if (document.getElementById("<%=TextBoxOperator.ClientID%>").value == "") {
                alert("Please Provide Operator Name  ");
                document.getElementById("<%=TextBoxOperator.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=DropDownListProduct.ClientID%>").value == "0") {
                alert("Please Provide Product");
                document.getElementById("<%=DropDownListProduct.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=TextBoxQuantity.ClientID%>").value == "") {
                alert("Please Provide Quantity ");
                document.getElementById("<%=TextBoxQuantity.ClientID%>").focus();
                return false;
            }


         


            if (document.getElementById("<%=TextBoxScrapQty.ClientID%>").value == "") {
                alert("Please Provide Scrap (Qty) ");
                document.getElementById("<%=TextBoxScrapQty.ClientID%>").focus();
                return false;
            }

          


            if (document.getElementById("<%=TextBoxQuantity.ClientID%>").value == "0") {
                alert("Please Provide Quantity ");
                document.getElementById("<%=TextBoxQuantity.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxScrapQty.ClientID%>").value == "0") {
                alert("Please Provide Scrap (Qty) ");
                document.getElementById("<%=TextBoxScrapQty.ClientID%>").focus();
                return false;
            }

          


            if (invalidChar(document.getElementById("TextBoxOperator"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("TextBoxOperator").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxQuantity"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("TextBoxQuantity").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxScrapQty"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("TextBoxScrapQty").focus();
                return false;
            }

            if (invalidChar(document.getElementById("TextBoxScrapWeight"))) {
                alert("Special Characters Not Allowed.");
                document.getElementById("TextBoxScrapWeight").focus();
                return false;
            }



        }

         
        
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <fieldset>
            <legend>
                <div style="margin-left: 10px; font-size: medium; color: Black">
                    Operator Productivity</div>
            </legend>
            <br />
            <br />
            <table border="0" align="center" cellpadding="3" cellspacing="3" style="height: 348px;">
                <tr>
                    <td class="Label_user">
                        Plant Name
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListPlantName" runat="server" Style="margin-left: 8px"
                            TabIndex="4" Width="165px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Machine <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListMachine" runat="server" Style="margin-left: 8px"
                            TabIndex="4" Width="165px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Operator Name <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxOperator" runat="server" class="form_textbox12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Product <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListProduct" runat="server" Style="margin-left: 8px"
                            TabIndex="4" Width="165px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Quantity <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxQuantity" runat="server" onkeypress="return isNumberKey(event)"
                            class="form_textbox12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Scrap(Qty) <span class="alert">* </span>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxScrapQty" runat="server" onkeypress="return isNumberKey(event)"
                            class="form_textbox12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Scrap(Weight)
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxScrapWeight" runat="server" onkeypress="return isNumberKey(event)"
                            class="form_textbox12"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="Label_user">
                        Remarks
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxRemarks" runat="server" Height="91px" TextMode="MultiLine"
                            class="form_textbox12" Width="231px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap" class="FieldText" colspan="2" align="center" style="margin-right: 200px">
                        <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>&nbsp;&nbsp;
                        <asp:Button ID="buttonUpdate" runat="server" Text="Update" class="button" OnClick="buttonUpdate_Click" />&nbsp;&nbsp;
                        <asp:Button ID="buttonSave" runat="server" TabIndex="18" class="button" Text="Save"
                            OnClientClick=" return validate()" Visible="false" OnClick="ButtonSubmit_Click" />&nbsp;&nbsp;
                        <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                            id="buttonClose" value="Close" class="button" />
                        &nbsp;&nbsp;
                        <input type="reset" class="button" value="Reset" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    </form>
</body>
</html>
