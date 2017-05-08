<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="collectionagging.aspx.cs" Inherits="HSRP.Report.collectionagging" %>
<%@ Register assembly="ComponentArt.Web.UI" namespace="ComponentArt.Web.UI" tagprefix="ComponentArt" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">
    <link href="../css/main.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/User.js" type="text/javascript"></script>
    <link href="../css/legend.css" rel="stylesheet" type="text/css" />
    <link href="../../css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../javascript/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />



<script type="text/javascript">
    function validate() {

        if (document.getElementById("<%=DropDownListStateName.ClientID%>").value == "--Select State--") {
            alert("Select State");
            document.getElementById("<%=DropDownListStateName.ClientID%>").focus();
            return false;
        }


    }
    </script>

    <script type="text/javascript">

        function OrderDatefrom_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDatefrom.getSelectedDate();
            CalendarOrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDatefrom.getSelectedDate();
            OrderDatefrom.setSelectedDate(fromDate);

        }

        function OrderDatefrom_OnClick() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                CalendarOrderDatefrom.hide();
            }
            else {
                CalendarOrderDatefrom.setSelectedDate(OrderDatefrom.getSelectedDate());
                CalendarOrderDatefrom.show();
            }
        }

        function OrderDatefrom_OnMouseUp() {
            if (CalendarOrderDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function OrderDateto_OnDateChange(sender, eventArgs) {   
            var fromDate = OrderDateto.getSelectedDate();
            CalendarOrderDateto.setSelectedDate(fromDate);

        } 

        function OrderDateto_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDateto.getSelectedDate();
            OrderDateto.setSelectedDate(fromDate);

        } 


        function OrderDateto_OnClick() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                CalendarOrderDateto.hide();
            }
            else {
                CalendarOrderDateto.setSelectedDate(OrderDateto.getSelectedDate());
                CalendarOrderDateto.show();
            }
        }

        

        function OrderDateto_OnMouseUp() {
            if (CalendarOrderDateto.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        
    </script>
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td valign="top">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="27" background="../images/midtablebg.jpg">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <span class="headingmain">Collection And Agging Report</span>
                                    </td>
                                    <td width="300px" height="26" align="center" nowrap>
                                    </td>
                                </tr>
                            </table>
                            </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            </table>

   
    <table style="height: 51px;" width="80%">
        <tr>
            <td style="height: 40px;" width="20">
                </td>
            <td style="height: 40px;" valign="middle" width="40">
                <asp:Label Text="HSRP State:" runat="server" ID="labelOrganization1" 
                                            ForeColor="Black" Font-Bold="True" Width="100px" />
            </td>
            <td style="width: 150px; height: 40px;" valign="middle">               
                   
                        <asp:DropDownList ID="DropDownListStateName" runat="server" AutoPostBack="True" 
                                                                              
                        CausesValidation="false" DataTextField="HSRPStateName" 
                                                                              DataValueField="HSRP_StateID" 
                                                                              
                                   
                                    Height="22px" Width="120px">
                        </asp:DropDownList>                    
                
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="DropDownListStateName" ErrorMessage="Select State" 
                            InitialValue="--Select State--"></asp:RequiredFieldValidator>
                
            </td>
            <td style="width: 50px; height: 40px;">               
                   
                    &nbsp;</td>
            <td style="height: 40px;" width="80">               
                   
                    &nbsp;</td>
         
                                                <td style="height: 40px" width="20"> &nbsp;</td>
                                                <td style="height: 40px" width="140">
                                                                &nbsp;</td>
                                                <td style="height: 40px" width="30">
                                                    <asp:Button ID="btnexport" runat="server" onclick="btnexport_Click" Text="Export" 
                                                        Font-Bold="True" ForeColor="#3333FF"/>
                                                </td>
        </tr>
        <td>
            &nbsp;</td>
        <td colspan="6">
            <asp:Label ID="Label1" runat="server" ForeColor="#CC3300" Visible="False"></asp:Label>
        </td>
        <tr>
             <td>
                                                    &nbsp;</td>
             
                                                        <asp:GridView ID="GridView1" runat="server" BackColor="#669999" BorderColor="White" AutoGenerateColumns="false"
                            BorderStyle="Solid" ForeColor="Yellow">
                    <Columns>

   <asp:TemplateField HeaderText="S.No.">
     <itemtemplate>
          <%#Container.DataItemIndex + 1 %>                                                    
     </itemtemplate>
</asp:TemplateField>
                 <asp:BoundField ReadOnly="True" 
                                HeaderText="Location"
                                DataField="Location"
                                SortExpression="Location"/>
                <asp:BoundField HeaderText="BalanceCash" 
                                DataField="Balance Cash"
                                SortExpression="BalanceCash"/>  
                        
                        <asp:BoundField ReadOnly="True" 
                                HeaderText="One Day Balance"
                                DataField="OneDayBalance"
                                SortExpression="OneDayBalance"/>
                <asp:BoundField HeaderText="Two day Balance" 
                                DataField="TwoDayBalance"
                                SortExpression="TwoDayBalance"/>  
                        
                         <asp:BoundField HeaderText="ThreeDayBalance" 
                                DataField="ThreeDayBalance"
                                SortExpression="ThreeDayBalance"/>  
                         <asp:BoundField HeaderText="FourDaybalance" 
                                DataField="FourDaybalance"
                                SortExpression="FourDaybalance"/> 

                         <asp:BoundField HeaderText="FiveDayBalance" 
                                DataField="FiveDayBalance"
                                SortExpression="FiveDayBalance"/> 

                         <asp:BoundField HeaderText="Morethan5 day Balance" 
                                DataField="Morethan5dayBalance"
                                SortExpression="Morethan5dayBalance"/> 
                                    
            </Columns>
                            </asp:GridView>
                                            </tr>
                                            <tr>
                                                
                                                    <td>
        </td>
                                               
        </tr>
                                                <%-- <td height="35" align="right" valign="middle" class="footer">
                                        <a onclick="AddNewPop(); return false;" title="Add New Hub" class="button">Add New Laser</a>
                                    </td>--%>
                                            
                                            
                                        </table>
    
                                            
</asp:Content>

