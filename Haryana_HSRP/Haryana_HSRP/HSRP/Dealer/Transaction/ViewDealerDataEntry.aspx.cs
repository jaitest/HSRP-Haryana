using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;


namespace HSRP.Dealer.Transaction
{
    public partial class ViewDealerDataEntry : System.Web.UI.Page
    {
        string RTOLocationID = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        

        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialSetting();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            UserType = Session["UserType"].ToString();
            if (!IsPostBack)
            {
               
            }
        }
        string Query = string.Empty;
        DataTable dt = new DataTable();
        private void viewrecords()
        {
            //Query = "Select * from Vendor_HSRPRecords";
            //dt = Utils.GetDataTable(Query, CnnString);
            //Grid1.DataSource = dt;
            //Grid1.DataBind();
        }

        protected void btnOldData_Click(object sender, EventArgs e)
        {
            viewrecords();
        }
        private void ClearGrid()
        {
            Grid1.Items.Clear();
            lblErrMsg.Text = String.Empty;
            return;
        }
        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            ClearGrid();
            Grid1.Items.Clear();

            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00"; // Convert.ToDateTime();

            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            string FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            string OrderDate1 = From1 + " 23:59:59";

            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
            DateTime EndDate = Convert.ToDateTime(HSRPAuthDate.SelectedDate);
            
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
             
            string UID = Session["UID"].ToString();



            //string SQLString = "Select  HSRPRecordID, (select HsrpStateName from hsrpstate where hsrp_StateID=Vendor_HSRPRecords.hsrp_StateID) as StateName, (select RTOLocationName from rtolocation where RTOLocationID=Vendor_HSRPRecords.RTOLocationID)as RTOLocationName, dealername, HSRPRecord_CreationDate, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, OwnerName, Address1, MobileNo, OrderType, OrderStatus, VehicleClass, VehicleType, vehiclemake, modelname, ChassisNo, EngineNo, VehicleRegNo, ExShowroom_Amount from Vendor_HSRPRecords where hsrp_StateID='" + HSRPStateID + "' and rtolocationID='" + RTOLocationID + "' and hsrpRecord_CreationDate between '" + AuthorizationDate + "'  and '" + OrderDate1 + "'";

            string SQLString = "Select  HSRPRecordID, (select HsrpStateName from hsrpstate where hsrp_StateID=Vendor_HSRPRecords.hsrp_StateID) as StateName, (select RTOLocationName from rtolocation where RTOLocationID=Vendor_HSRPRecords.RTOLocationID)as RTOLocationName, dealername, HSRPRecord_CreationDate, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, OwnerName, Address1, MobileNo, OrderType, OrderStatus, VehicleClass, VehicleType, vehiclemake, modelname, ChassisNo, EngineNo, VehicleRegNo, ExShowroom_Amount from Vendor_HSRPRecords where CreatedBy='" + UID + "' and hsrpRecord_CreationDate between '" + AuthorizationDate + "'  and '" + OrderDate1 + "'";
                
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                Grid1.RecordCount.ToString();
            }
            else
            {
                Grid1.Items.Clear();
                ClearGrid();
            }

        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //LabelError.Visible = false;
                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string AuthorizationDate = From + " 00:00:00";

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string OrderDate1 = From1 + " 23:59:59";
                string UID = Session["UID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                string RTOLocationName = string.Empty;
                string Statename = string.Empty;
                string sqlRTOLocation = "Select RtolocationName from rtolocation where rtolocationID='" + Session["UserRTOLocationID"] + "'";
                 DataTable dtRTO = Utils.GetDataTable(sqlRTOLocation, CnnString);
                 if (dtRTO.Rows.Count > 0)
                 {
                     RTOLocationName = dtRTO.Rows[0]["RtolocationName"].ToString();
                 }
                 string sqlStateName = "Select hsrpStateName from hsrpState where hsrp_StateID='" + Session["UserHSRPStateID"] + "'";
                 DataTable dtState = Utils.GetDataTable(sqlStateName, CnnString);
                 if (dtState.Rows.Count > 0)
                 {
                     Statename = dtState.Rows[0]["hsrpStateName"].ToString();
                 }


                 string sqlsqu = "Select  HSRPRecordID, (select HsrpStateName from hsrpstate where hsrp_StateID=Vendor_HSRPRecords.hsrp_StateID) as StateName, (select RTOLocationName from rtolocation where RTOLocationID=Vendor_HSRPRecords.RTOLocationID)as RTOLocationName, dealername, convert(varchar, HSRPRecord_CreationDate,110) as HSRPRecord_CreationDate, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, OwnerName, Address1, MobileNo, OrderType, OrderStatus, VehicleClass, VehicleType, vehiclemake, modelname, ChassisNo, EngineNo, VehicleRegNo, ExShowroom_Amount from Vendor_HSRPRecords where hsrp_StateID='" + HSRPStateID + "' and rtolocationID='" + RTOLocationID + "' and hsrpRecord_CreationDate between '" + AuthorizationDate + "'  and '" + OrderDate1 + "'";
                //string  SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ")  and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";

                DataTable dt = Utils.GetDataTable(sqlsqu, CnnString);

                if (dt.Rows.Count > 0)
                {
                    string filename = "DealerReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Daily Assign Embossing Report";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
                    WorksheetStyle style = book.Styles.Add("HeaderStyle");
                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 10;
                    style.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = false;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style7 = book.Styles.Add("HeaderStyle7");
                    style7.Font.FontName = "Tahoma";
                    style7.Font.Size = 10;
                    style7.Font.Bold = false;
                    style7.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                    style2.Font.FontName = "Tahoma";
                    style2.Font.Size = 10;
                    style2.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                    WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                    style3.Font.FontName = "Tahoma";
                    style3.Font.Size = 12;
                    style3.Font.Bold = true;
                    style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                    Worksheet sheet = book.Worksheets.Add("HSRP Daily Assign Embossing Report");

                    sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(110));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(115));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(160));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    WorksheetRow row = sheet.Table.Rows.Add();
                    // row.
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("Dealer Order Book Report");
                    cell.MergeAcross = 3; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    //row.Cells.Add(<br>);
                    row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State :", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(Statename, "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();

                    row.Index = 4;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(RTOLocationName, "HeaderStyle2")); 
                    //row.Cells.Add(<br>); 
                    row = sheet.Table.Rows.Add();

                    // Skip one row, and add some text
                    row.Index = 5;
                    DateTime date = System.DateTime.Now;
                    string formatted = date.ToString("dd/MM/yyyy");

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(formatted, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date From:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(ReportDateFrom, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 7;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date TO:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(ReportDateTo, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 8;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(<br>); 
                    row = sheet.Table.Rows.Add();


                    row.Index = 10;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Creation Date", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("Authorization No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Address", "HeaderStyle")); 
                    row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Vehicle Maker", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Vehicle Model", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Chassis No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Engine No.", "HeaderStyle"));  
                    row.Cells.Add(new WorksheetCell("ExShow Room Amount", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle"));

                     

                    row = sheet.Table.Rows.Add();

                    String StringField = String.Empty;
                    String StringAlert = String.Empty;


                    if (dt.Rows.Count <= 0)
                    {
                        string closescript1 = "<script>alert('No records found for selected Sate.')</script>";
                        Page.RegisterStartupScript("abc", closescript1);
                        return;
                    }
                    row.Index = 11;
                    int sno = 0;

                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_CreationDate"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle6")); 
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["Address1"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["vehiclemake"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["modelname"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["ExShowroom_Amount"].ToString(), DataType.String, "HeaderStyle6")); 
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle6"));
 

                    }
                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);

                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
                else
                {
                    string closescript1 = "<script>alert('No records found for selected date.')</script>";
                    Page.RegisterStartupScript("abc", closescript1);
                    return;
                }
            }

            catch (Exception ex)
            {
                //LabelError.Visible = true;
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

    }
}