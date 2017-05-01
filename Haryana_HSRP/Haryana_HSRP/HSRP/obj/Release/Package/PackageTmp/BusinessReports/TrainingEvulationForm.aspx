<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TrainingEvulationForm.aspx.cs" Inherits="HSRP.BusinessReports.TrainingEvulationForm" %>
<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
      
    <script type="text/javascript">
        function Datefrom_OnDateChange(sender, eventArgs) {
            var fromDate = Datefrom.getSelectedDate();
            CalendarDatefrom.setSelectedDate(fromDate);

        }

        function Datefrom_OnChange(sender, eventArgs) {
            var fromDate = CalendarDatefrom.getSelectedDate();
            Datefrom.setSelectedDate(fromDate);

        }

        function Datefrom_OnClick() {
            if (CalendarDatefrom.get_popUpShowing()) {
                CalendarDatefrom.hide();
            }
            else {
                CalendarDatefrom.setSelectedDate(Datefrom.getSelectedDate());
                CalendarDatefrom.show();
            }
        }

        function Datefrom_OnMouseUp() {
            if (CalendarDatefrom.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <div style=" width: 85%; background-color: transparent; "> 
 
        <fieldset>  <b>Traning Evaluation Form</b> 
          <%--  <legend ></legend>--%>
            <div style="margin: 20px;" align="left">
                <div>
                    <table align="center" width="100%">

                    <tr>
                    <td> <asp:Label Text="Date:"  Font-Bold="true" runat="server" ID="labelOrganization" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td >     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <ComponentArt:Calendar ID="Datefrom" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                                        ControlType="Picker" PickerCssClass="picker">
                                                        <ClientEvents>
                                                            <SelectionChanged EventHandler="Datefrom_OnDateChange" />
                                                        </ClientEvents>
                                                    </ComponentArt:Calendar>
                                                    <img id="calendar_from_button" alt="" onclick="Datefrom_OnClick()"
                                                        onmouseup="Datefrom_OnMouseUp()" class="calendar_button"
                                                        src="../images/btn_calendar.gif" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                  
                        
                                 
                    </td>
                    </tr>

                    <tr>
                    <td> <asp:Label Text="Name:"   Font-Bold="true" runat="server" ID="label1" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td >                  
                                        
                    </td>
                    </tr>

                    <tr>
                    <td> <asp:Label Text="Tiitle and location of training:"   Font-Bold="true"  runat="server" ID="labelClient" ForeColor="Black" />&nbsp;&nbsp;</td>
                    <td valign="middle" class="Label_user_batch" style="width: 165px">                                                  
                                                      
                      </td>
                    </tr>

                    <tr>
                    <td> 
                        <asp:Label Text="Trainer :"   Font-Bold="true"  runat="server" ID="lblEmb" 
                            ForeColor="Black" Visible="False" />
                    </td>
                    <td valign="middle" class="Label_user_batch" style="width: 165px">                                                  
                                                    
                                                         </td>
                    </tr>
                      
                        <tr>
                            <td colspan="3">
                                <span nowrap="nowrap" style="color:maroon;   font: verdana arial 40px;"> <b>Instruction:</b> Please
                                    indicate your leevel ogbagreement with the statements listed below in #1-11.</span>
                            </td>
                             
                             
                            
                            <td align="center" rowspan="9" valign="middle">
                                
                            </td>
                           
                        </tr>
                        <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color:Black; font: verdana arial 12px;">
                                   1. The objectives of the training were clearly defined.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color:Black; font: verdana arial 12px;">
                                   2. Participation and interaction were encouraged.</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
                                  3. The topics coovered were relevant to me .</span>
                            </td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
                                4. The content&nbsp; was organized and easy to follow .</span>
                            </td>
                        </tr>
                        <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
                               5.&nbsp; The materials distributed were helpful .</span></td>
                        </tr>
                        
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
6. This training experiencce will be useful in my work.</span></td>
                        </tr>
                         <tr>
                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
7. The trainer was knowledgeable about the training topics.</span></td>
                        </tr>
                        <tr>

                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
8. The trainer was well&nbsp; prepered.</span></td>
                        </tr>
                        <tr>

                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
9. The Training objectives were met.</span></td>
                        </tr>
                        <tr>

                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
10. The time alloted for the training was sufficiant.</span></td>
                        </tr>
                        <tr>

                         <td colspan="2">
                                <span nowrap="nowrap" style="color: Black; font: verdana arial 12px;">
11. The meeting room and facilities were adequate and comfortable.</span></td>
                        </tr>

                         
                      <tr>
                          <td>
                                <ComponentArt:Calendar runat="server" ID="CalendarDatefrom" AllowMultipleSelection="false"
                                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                                    PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif"
                                    NextImageUrl="cal_nextMonth.gif" Height="172px" Width="200px">
                                    <ClientEvents>
                                        <SelectionChanged EventHandler="Datefrom_OnChange" />
                                    </ClientEvents>
                                </ComponentArt:Calendar>       
                          </td>
                      </tr>
                      
                   
                    </table>
                </div>
            </div> 
        </fieldset>
   
        
         </div>

</asp:Content>
