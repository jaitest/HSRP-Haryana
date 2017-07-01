<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="AssignInventory.aspx.cs" Inherits="HSRP.Master.AssignInventory" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">


        function edit(i) { // Define This function of Send Assign Laser ID 
            //alert("AssignLaser" + i);
//            var usertype = document.getElementById('username').value;
//            alert(usertype);

            googlewin = dhtmlwindow.open("googlebox", "iframe", "Batch.aspx?Mode=Edit&BatchID=" + i, "Batch Laser Assigen", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location.href = "ViewBatch.aspx";
                return true;
            }
        }
         
        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes

            googlewin = dhtmlwindow.open("googlebox", "iframe", "Batch.aspx?Mode=New", "Add New Batch", "width=950px,height=500px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                 window.location = 'ViewBatch.aspx';
                return true;
            }
        }
    </script>
    <script type="text/javascript" language="javascript">

        function validate() {

            if (document.getElementById("<%=dropdownListProduct.ClientID%>").value == "--Select Product--") {
                alert("Please Select Product");
                document.getElementById("<%=dropdownListProduct.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropdownListBatch.ClientID%>").value == "--Select Batch--") {
                alert("Please Select Batch");
                document.getElementById("<%=dropdownListBatch.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropdownListPrefix.ClientID%>").value == "--Select Prefix--") {
                alert("Please Select Prefix");
                document.getElementById("<%=dropdownListPrefix.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropdownListStateName.ClientID%>").value == "--Select State--") {
                alert("Please Select State");
                document.getElementById("<%=dropdownListStateName.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropdownListLocationType.ClientID%>").value == "--Select Location Type--") {
                alert("Please Select Location Type");
                document.getElementById("<%=dropdownListLocationType.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=dropdownListLocation.ClientID%>").value == "--Select Location--") {
                alert("Please Select Location");
                document.getElementById("<%=dropdownListLocation.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxLaserFrom.ClientID%>").value == "") {
                alert("Please Provide Laser No");
                document.getElementById("<%=TextBoxLaserFrom.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=TextBoxLaserTo.ClientID%>").value == "") {
                alert("Please Provide Laser No");
                document.getElementById("<%=TextBoxLaserTo.ClientID%>").focus();
                return false;
            } 
        }




        function ConfirmOnActivateUser() {
            if (confirm("Confirm!. Do you really want to change Secure Devices status?")) {

                return true;
            }
            else {
                return false;
            }

        }
    </script>
    <%--<table width="99%" border="0"  align="center" cellpadding="0" cellspacing="0" class="midtable">--%>
     
      <table width="95%" style="background-color: #FFFFFF; border:1px solid; margin: 26px 20px 20px 32px; padding: 31px 49px 56px 248px; position: absolute; " align="center" cellpadding="0" cellspacing="0" class="borderinner">    
              <asp:UpdatePanel ID="UpdatePannel1" runat="server">
              <ContentTemplate>
              
                <tr>
                 <td  class="form_text" style="padding-bottom: 10px" ></td>
               
                    <td class="form_text" style="padding-bottom: 10px"> Product Type :  <span class="alert">* </span> </td>
                
                    <td >  <asp:DropDownList ID="dropdownListProduct" DataTextField="ProductCode" 
                            AutoPostBack ="true" DataValueField="ProductID" runat="server" 
                            onselectedindexchanged="dropdownListProduct_SelectedIndexChanged" > </asp:DropDownList> </td>
                </tr> 
                <tr>
                    <td  class="form_text" style="padding-bottom: 10px" ></td>
                        <td class="form_text" style="padding-bottom: 10px"> Batch :  <span class="alert">* </span> </td> 
                        <td  > 
                            <asp:DropDownList ID="dropdownListBatch" DataTextField="BatchCode" AutoPostBack ="true"
                                DataValueField="BatchID" runat="server" 
                                onselectedindexchanged="dropdownListBatch_SelectedIndexChanged" > 
                            </asp:DropDownList>
                        </td>
                </tr> 

                <tr>
                        <td  class="form_text" style="padding-bottom: 10px" ></td>
                        <td class="form_text" style="padding-bottom: 10px"> Prefix Name :  <span class="alert">* </span> </td> 
                        <td > 
                            <asp:DropDownList ID="dropdownListPrefix" DataTextField="Prefix" AutoPostBack ="true"
                                DataValueField="BatchID" runat="server" >
                            </asp:DropDownList>
                        </td>
                </tr> 
               <tr>
                     <td  class="form_text" style="padding-bottom: 10px" ></td>
                    <td class="form_text" style="padding-bottom: 10px"> State Name : <span class="alert">* </span> </td> 
                    <td> 
                        <asp:DropDownList ID="dropdownListStateName" DataTextField="HSRPStateName" AutoPostBack ="false"
                            DataValueField="HSRP_StateID" runat="server" 
                            onselectedindexchanged="dropdownListStateName_SelectedIndexChanged" >
                        </asp:DropDownList>
                    </td>
                </tr>
                    <tr>
                     <td  class="form_text" style="padding-bottom: 10px" ></td>
                     <td class="form_text" style="padding-bottom: 10px"> Location Type : <span class="alert">* </span> </td> 
                    <td > 
                        <asp:DropDownList ID="dropdownListLocationType"  AutoPostBack ="true" 
                            runat="server" 
                            onselectedindexchanged="dropdownListLocationType_SelectedIndexChanged" >
                        <asp:ListItem>--Select Location Type--</asp:ListItem>
                        <asp:ListItem>Central</asp:ListItem>
                        <asp:ListItem>District</asp:ListItem>
                        <asp:ListItem>Sub-Urban</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                 <td  class="form_text" style="padding-bottom: 10px" ></td>
                    <td class="form_text" style="padding-bottom: 10px"> Location :  <span class="alert">* </span> </td > 
                    <td>
                        <asp:DropDownList ID="dropdownListLocation" DataTextField="RTOLocationName" AutoPostBack ="false"
                            DataValueField="RTOLocationID" runat="server" 
                            onselectedindexchanged="dropdownListLocation_SelectedIndexChanged" > 
                        <asp:ListItem>--Select Location--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                 <td  class="form_text" style="padding-bottom: 10px" ></td>
                    <td class="form_text" style="padding-bottom: 10px"> Laser From :  <span class="alert">* </span> </td > 
                    <td>
                        <asp:TextBox ID="TextBoxLaserFrom" runat="server"></asp:TextBox>
                    </td>
                    <tr>
                     <td  class="form_text" style="padding-bottom: 10px" ></td>
                    <td class="form_text" style="padding-bottom: 10px"> Laser To :  <span class="alert">* </span> </td > 
                    <td>
                        <asp:TextBox ID="TextBoxLaserTo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td colspan="6"></td></tr>
                <tr>
                <td>
                 <asp:Label ID="lblSucMess" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                          <asp:Label ID="lblErrMess" runat="server" ForeColor="Red" Font-Size="18px"></asp:Label>
                </td>
                </tr>
                 <tr>
             <td> <asp:Label ID="LabelUpdated" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label>
                         <asp:Label ID="lblTotalRecord" runat="server" ForeColor="Blue" Font-Size="18px"></asp:Label></td>
             </tr>
             <tr>
            <td>
                          <asp:Label ID="lblExist" runat="server" ForeColor="Red" 
                    Font-Size="18px"></asp:Label> 
</td>
            </tr>
                <tr align="center">
                    <td></td>
                    <td>
                        
                        </td>
                        <td colspan="3" align="right">
                            <br />
                            <asp:Button ID="buttonUpdate" runat="server" Text="Update" TabIndex="8"  OnClientClick=" return validate()" 
                                class="button" onclick="buttonUpdate_Click" />&nbsp;&nbsp; 
                            <input type="button" onclick="javascript:parent.googlewin.close();" name="buttonClose"
                                                id="buttonClose" value="Close" class="button" /> &nbsp;&nbsp;
                                            <input type="reset"   id="Reset" value="Reset" class="button" /> 
                                             
                        </td>
                        <td>
                        </td>
                        </tr>
                    </tr> 
               
              </ContentTemplate>
              </asp:UpdatePanel>                                              
    </table>
    <br />
</asp:Content>
