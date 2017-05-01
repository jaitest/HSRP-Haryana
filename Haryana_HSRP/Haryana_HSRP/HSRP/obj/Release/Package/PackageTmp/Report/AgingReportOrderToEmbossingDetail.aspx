<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgingReportOrderToEmbossingDetail.aspx.cs" Inherits="HSRP.Report.AgingReportOrderToEmbossingDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function PrintGridData() {
            var prtGrid = document.getElementById("Grid");
            prtGrid.border = 0;
            var prtwin = window.open('', 'PrintGridViewData', 'left=100,top=100,width=1000,height=1000,tollbar=0,scrollbars=1,status=0,resizable=1');
            prtwin.document.write(prtGrid.outerHTML);
            prtwin.document.close();
            prtwin.focus();
            prtwin.print();
            prtwin.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <div id="Grid" class="modal-body">


            <div align="center" >
            <label >Aging Report Order To Embossing Detail</label>
          
                        
             </div>
                                       <asp:GridView ID="DetailsView1" runat="server" CellPadding="4" ForeColor="#333333" 
                 GridLines="both" Width="100%">
                     <FooterStyle BackColor="#507CD1" Font-Bold="True" Wrap="true" ForeColor="White" 
                                                HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <HeaderStyle BackColor="#507CD1" Wrap="true" Font-Bold="True" ForeColor="White" 
                                                HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" Wrap="true" HorizontalAlign="Center" VerticalAlign="Middle" />                                        
                </asp:GridView>  
        

               
                          <%--  <a onclick="PrintGridData()" class="btn btn-info" shape="circle">Print</a>--%>

                           <%-- <asp:Button id="PrintBtn" runat="server" Text="Print"  OnClick="printbtn_click"/>--%>

                    <asp:Button ID="Button1" runat="server" OnClientClick="PrintGridData()"  Text="Print" />

                </div>

            </div>
    </form>
</body>
</html>
