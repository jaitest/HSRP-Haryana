using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using System.Text;


namespace HSRP.Report
{
    public partial class DailyAssignEmbossing : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
       protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Convert.ToInt32(Session["UserType"]);
               

                
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                        if (UserType.Equals(0))
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                        }
                        else
                        {
                            
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            labelDate.Visible = false;
                           
                            FilldropDownListClient();
                            FilldropDownListOrganization(); 
                        }
                    }
                    catch (Exception err)
                    {
                        
                    }
                }
            }
        }
       
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
             
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";

                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                 
            }
        }
       
        #endregion

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
            LabelError.Visible = false;

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");

            String From1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

            // String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            String ReportDate = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
            string QDate = StringOrderDate[1] + "-" + StringOrderDate[0] + "-" + StringOrderDate[2].Split(' ')[0];
            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                 
            UserType = Convert.ToInt32(Session["UserType"]);
            if (UserType == 0)
            { 
                //SQLString = "SELECT  HSRPRecords.OwnerName,HSRPRecords.MobileNo,HSRPRecords.HSRPRecordID,HSRPRecords.FrontPlateSize,HSRPRecords.RearPlateSize ,HSRPRecords.OrderStatus, HSRPRecords.OrderDate, HSRPRecords.EngineNo, HSRPRecords.ChassisNo,RTOLocation.RTOLocationName,HSRPRecords.VehicleRegNo, HSRPRecords.VehicleType, HSRPRecords.VehicleClass, HSRPRecords.OwnerName, HSRPRecords.HSRP_StateID,HSRPState.HSRPStateName, HSRPRecords.RTOLocationID, RTOLocation.HSRP_StateID AS Expr1 FROM HSRPRecords INNER JOIN HSRPState ON HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where HSRPState.HSRP_StateID='" + intHSRPStateID + "' and RTOLocation.RTOLocationID='" + intRTOLocationID + "' and HSRPRecords.OrderStatus='New Order'  and convert(varchar, OrderDate, 105)  =convert(varchar, '" + From1 + "', 105)";
                SQLString = "SELECT     a.OwnerName, a.MobileNo, a.HSRPRecordID, a.OrderStatus, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName, a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, b.RTOLocationID, b.HSRP_StateID, Product_1.ProductCode as FrontProductCode, Product.ProductCode  as RearProductCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords a  INNER JOIN HSRPState s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN RTOLocation b ON a.RTOLocationID = a.RTOLocationID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID where s.HSRP_StateID='" + intHSRPStateID + "' and b.RTOLocationID='" + intRTOLocationID + "' and a.OrderStatus='New Order'  and convert(varchar, OrderDate, 105)  =convert(varchar, '" + From1 + "', 105)";
            }
            else
            {
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());

               // SQLString = "SELECT  HSRPRecords.OwnerName,HSRPRecords.MobileNo,HSRPRecords.HSRPRecordID,HSRPRecords.FrontPlateSize,HSRPRecords.RearPlateSize ,HSRPRecords.OrderStatus, HSRPRecords.OrderDate, HSRPRecords.EngineNo, HSRPRecords.ChassisNo,RTOLocation.RTOLocationName,HSRPRecords.VehicleRegNo, HSRPRecords.VehicleType, HSRPRecords.VehicleClass, HSRPRecords.OwnerName, HSRPRecords.HSRP_StateID,HSRPState.HSRPStateName, HSRPRecords.RTOLocationID, RTOLocation.HSRP_StateID AS Expr1 FROM HSRPRecords INNER JOIN HSRPState ON HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where HSRPState.HSRP_StateID='" + HSRPStateID + "' and RTOLocation.RTOLocationID='" + RTOLocationID + "' and HSRPRecords.OrderStatus='New Order' and convert(varchar, OrderDate, 105)  =convert(varchar, '" + From1 + "', 105)";
                SQLString = "SELECT     a.OwnerName, a.MobileNo, a.HSRPRecordID, a.OrderStatus, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName, a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, b.RTOLocationID, b.HSRP_StateID, Product_1.ProductCode as FrontProductCode, Product.ProductCode  as RearProductCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords a  INNER JOIN HSRPState s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN RTOLocation b ON a.RTOLocationID = a.RTOLocationID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID where s.HSRP_StateID='" + HSRPStateID + "' and b.RTOLocationID='" + RTOLocationID + "' and a.OrderStatus='New Order'  and convert(varchar, OrderDate, 105)  =convert(varchar, '" + From1 + "', 105)";
            }


              DataSet dt = Utils.getDataSet(SQLString, CnnString);

              if (dt.Tables[0].Rows.Count > 0)
              {

                  int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                  int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                  string filename = "DailyAssignEmbossing-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
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
                  sheet.Table.Columns.Add(new WorksheetColumn(120));
                  sheet.Table.Columns.Add(new WorksheetColumn(130));
                  sheet.Table.Columns.Add(new WorksheetColumn(100));
                  sheet.Table.Columns.Add(new WorksheetColumn(135));
                  sheet.Table.Columns.Add(new WorksheetColumn(130));
                  sheet.Table.Columns.Add(new WorksheetColumn(135));
                  sheet.Table.Columns.Add(new WorksheetColumn(130));
                  sheet.Table.Columns.Add(new WorksheetColumn(160));
                  sheet.Table.Columns.Add(new WorksheetColumn(135)); 
                  WorksheetRow row = sheet.Table.Rows.Add();
                  // row.
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                  row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                  WorksheetCell cell = row.Cells.Add("HSRP Daily Assign Embossing Report");
                  cell.MergeAcross = 3; // Merge two cells together
                  cell.StyleID = "HeaderStyle3";

                  //row.Cells.Add(<br>);
                  row = sheet.Table.Rows.Add();
                  row.Index = 3;
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                  row = sheet.Table.Rows.Add();

                  row.Index = 4;
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));
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
                  row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                  row = sheet.Table.Rows.Add();
                  


                  row.Index = 8;
                  //row.Cells.Add("Order Date");
                  row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Vehicle No", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Vehicle Model", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Front Plate Size", "HeaderStyle"));

                  row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Chassis No.", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Engine No.", "HeaderStyle"));

                  row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Mobile No.", "HeaderStyle")); 

                  row = sheet.Table.Rows.Add();

                  String StringField = String.Empty;
                  String StringAlert = String.Empty;


                  if (dt.Tables[0].Rows.Count <= 0)
                  {
                      LabelError.Text = string.Empty;
                      LabelError.Text = "Their is no selected data for the selected  date range.";
                      return;
                  }
                  row.Index = 9;
                  Int64 DailyTarget = 0;
                  Int64 monthtarget = 0;
                  Int64 dailyactual = 0;
                  Int64 monthactual = 0;

                  Int64 yrstarget = 0;
                  Int64 yrsactual = 0;
                  Int64 MonthlyRejection = 0;
                  Int64 DailyRejection = 0;
                  Int64 YearlyRejection = 0; 

                  int sno = 0;

                  foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
                  {
                      sno = sno + 1;
                      row = sheet.Table.Rows.Add();
                      row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                      row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle6"));
                      row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle6"));
                      row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle6"));

                      row.Cells.Add(new WorksheetCell(dtrows["FrontProductCode"].ToString(), DataType.String, "HeaderStyle6"));
                      row.Cells.Add(new WorksheetCell(dtrows["RearProductCode"].ToString(), DataType.String, "HeaderStyle6"));
                      row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle7"));
                      row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle7"));

                      row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle7"));
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
                LabelError.Visible = true;
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void btnExportToPDF_Click(object sender, EventArgs e)
        {

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");

            String From1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

            // String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            String ReportDate = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
            string QDate = StringOrderDate[1] + "-" + StringOrderDate[0] + "-" + StringOrderDate[2].Split(' ')[0];
            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);


            DataSet dt = new DataSet();
            DataSet dt1 = new DataSet();
            DataSet dt2 = new DataSet();
            //DataSet dt1 = new DataSet();
            int i = 0;

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
            UserType = Convert.ToInt32(Session["UserType"]);
            HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

            UserType = Convert.ToInt32(Session["UserType"]);
            if (UserType == 0)
            {
                //SQLString = "Report_Daily_Rejection'" + OrderDate1 + "','" + intHSRPStateID + "','" + intRTOLocationID + "'";
                //string SQLString = "SELECT  HSRPRecords.OwnerName,HSRPRecords.MobileNo,HSRPRecords.HSRPRecordID,HSRPRecords.FrontPlateSize,HSRPRecords.RearPlateSize ,HSRPRecords.OrderStatus, HSRPRecords.OrderDate, HSRPRecords.EngineNo, HSRPRecords.ChassisNo,RTOLocation.RTOLocationName,HSRPRecords.VehicleRegNo, HSRPRecords.VehicleType, HSRPRecords.VehicleClass, HSRPRecords.OwnerName, HSRPRecords.HSRP_StateID,HSRPState.HSRPStateName, HSRPRecords.RTOLocationID, RTOLocation.HSRP_StateID AS Expr1 FROM HSRPRecords INNER JOIN HSRPState ON HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where HSRPState.HSRP_StateID='" + HSRPStateID + "' and RTOLocation.RTOLocationID='" + RTOLocationID + "' and HSRPRecords.OrderStatus='New Order'";
                SQLString = "SELECT  HSRPRecords.OwnerName,HSRPRecords.MobileNo,HSRPRecords.HSRPRecordID,HSRPRecords.FrontPlateSize,HSRPRecords.RearPlateSize ,HSRPRecords.OrderStatus, HSRPRecords.OrderDate, HSRPRecords.EngineNo, HSRPRecords.ChassisNo,RTOLocation.RTOLocationName,HSRPRecords.VehicleRegNo, HSRPRecords.VehicleType, HSRPRecords.VehicleClass, HSRPRecords.OwnerName, HSRPRecords.HSRP_StateID,HSRPState.HSRPStateName, HSRPRecords.RTOLocationID, RTOLocation.HSRP_StateID AS Expr1 FROM HSRPRecords INNER JOIN HSRPState ON HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where HSRPState.HSRP_StateID='" + intHSRPStateID + "' and RTOLocation.RTOLocationID='" + intRTOLocationID + "' and HSRPRecords.OrderStatus='New Order'  and convert(varchar, OrderDate, 105)  =convert(varchar, '" + From1 + "', 105)";
            }
            else
            {
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());

                SQLString = "SELECT  HSRPRecords.OwnerName,HSRPRecords.MobileNo,HSRPRecords.HSRPRecordID,HSRPRecords.FrontPlateSize,HSRPRecords.RearPlateSize ,HSRPRecords.OrderStatus, HSRPRecords.OrderDate, HSRPRecords.EngineNo, HSRPRecords.ChassisNo,RTOLocation.RTOLocationName,HSRPRecords.VehicleRegNo, HSRPRecords.VehicleType, HSRPRecords.VehicleClass, HSRPRecords.OwnerName, HSRPRecords.HSRP_StateID,HSRPState.HSRPStateName, HSRPRecords.RTOLocationID, RTOLocation.HSRP_StateID AS Expr1 FROM HSRPRecords INNER JOIN HSRPState ON HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where HSRPState.HSRP_StateID='" + HSRPStateID + "' and RTOLocation.RTOLocationID='" + RTOLocationID + "' and HSRPRecords.OrderStatus='New Order' and convert(varchar, OrderDate, 105)  =convert(varchar, '" + From1 + "', 105)";
            }

            //string SQLString = "SELECT  HSRPRecords.OwnerName,HSRPRecords.MobileNo,HSRPRecords.HSRPRecordID,HSRPRecords.FrontPlateSize,HSRPRecords.RearPlateSize ,HSRPRecords.OrderStatus, HSRPRecords.OrderDate, HSRPRecords.EngineNo, HSRPRecords.ChassisNo,RTOLocation.RTOLocationName,HSRPRecords.VehicleRegNo, HSRPRecords.VehicleType, HSRPRecords.VehicleClass, HSRPRecords.OwnerName, HSRPRecords.HSRP_StateID,HSRPState.HSRPStateName, HSRPRecords.RTOLocationID, RTOLocation.HSRP_StateID AS Expr1 FROM         HSRPRecords INNER JOIN HSRPState ON HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where HSRPState.HSRP_StateID='" + HSRPStateID + "' and RTOLocation.RTOLocationID='" + RTOLocationID + "' and HSRPRecords.OrderStatus='New Order'";
            
            dt = Utils.getDataSet(SQLString, CnnString);
            if (dt.Tables[0].Rows.Count > 0)
            {

                i = dt.Tables[0].Rows.Count;


                string filename = "HSRP INVOICE" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

                //string SQLString = String.Empty;
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                StringBuilder bb = new StringBuilder();

                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(9);
                //actual width of table in points
                table.TotalWidth = 1500f;

                PdfPCell cell120911 = new PdfPCell(new Phrase("Jobs For Embossing", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 9;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;

                cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + dt.Tables[0].Rows[0]["HSRPStateName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 7;
                cell12091.BorderWidthLeft = 1f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 1f;
                cell12091.BorderWidthBottom = 0f;

                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

               // PdfPCell cell12093 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                PdfPCell cell12093 = new PdfPCell(new Phrase("Date : " + From1, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 2;
                cell12093.BorderWidthLeft = 0f;
                cell12093.BorderWidthRight = 1f;
                cell12093.BorderWidthTop = 1f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);

                PdfPCell cell12092 = new PdfPCell(new Phrase("RTOLocation Name : " + dt.Tables[0].Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12092.Colspan = 9;
                cell12092.BorderWidthLeft = 1f;
                cell12092.BorderWidthRight = 1f;
                cell12092.BorderWidthTop = 0f;
                cell12092.BorderWidthBottom = 0f;

                cell12092.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12092); 

                PdfPCell cell1209 = new PdfPCell(new Phrase("Vehicle No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 1f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = 1f;
                cell1209.BorderWidthBottom = 1f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);

                PdfPCell cell1213 = new PdfPCell(new Phrase("Vehicle Model", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 1f;
                cell1213.BorderWidthBottom = 1f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);



                PdfPCell cell12233 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 1f;
                cell12233.BorderWidthBottom = 1f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                PdfPCell cell122331 = new PdfPCell(new Phrase("Front Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122331.Colspan = 1;
                cell122331.BorderWidthLeft = 0f;
                cell122331.BorderWidthRight = .8f;
                cell122331.BorderWidthTop = 1f;
                cell122331.BorderWidthBottom = 1f;

                cell122331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122331);


                PdfPCell cell122332 = new PdfPCell(new Phrase("Rear Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122332.Colspan = 1;
                cell122332.BorderWidthLeft = 0f;
                cell122332.BorderWidthRight = .8f;
                cell122332.BorderWidthTop = 1f;
                cell122332.BorderWidthBottom = 1f;

                cell122332.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122332);

                PdfPCell cell1206 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1206.Colspan = 1;
                cell1206.BorderWidthLeft = 0f;
                cell1206.BorderWidthRight = .8f;
                cell1206.BorderWidthTop = 1f;
                cell1206.BorderWidthBottom = 1f;
                cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1206);

                PdfPCell cell1221 = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1221.Colspan = 1;
                cell1221.BorderWidthLeft = 0f;
                cell1221.BorderWidthRight = 1f;
                cell1221.BorderWidthTop = 1f;
                cell1221.BorderWidthBottom = 1f;

                cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1221);


                PdfPCell cell120933 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120933.Colspan = 1;
                cell120933.BorderWidthLeft = 1f;
                cell120933.BorderWidthRight = .8f;
                cell120933.BorderWidthTop = 1f;
                cell120933.BorderWidthBottom = 1f;

                cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120933);

                PdfPCell cell120934 = new PdfPCell(new Phrase("Mobile No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120934.Colspan = 1;
                cell120934.BorderWidthLeft = 1f;
                cell120934.BorderWidthRight = .8f;
                cell120934.BorderWidthTop = 1f;
                cell120934.BorderWidthBottom = 1f;

                cell120934.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120934);
                //PdfPCell cell1223 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1223.Colspan = 2;
                //cell1223.BorderWidthLeft = 0f;
                //cell1223.BorderWidthRight = 1f;
                //cell1223.BorderWidthTop = 1f;
                //cell1223.BorderWidthBottom = 1f;
                //cell1223.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1223);
                i = i - 1;
                while (i > 0)
                {





                    PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Tables[0].Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1211.Colspan = 1;
                    cell1211.BorderWidthLeft = 1f;
                    cell1211.BorderWidthRight = .8f;
                    cell1211.BorderWidthTop = .5f;
                    cell1211.BorderWidthBottom = .5f;

                    cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1211);




                    PdfPCell cell1214 = new PdfPCell(new Phrase(dt.Tables[0].Rows[i]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1214.Colspan = 1;
                    cell1214.BorderWidthLeft = 0f;
                    cell1214.BorderWidthRight = .8f;
                    cell1214.BorderWidthTop = .5f;
                    cell1214.BorderWidthBottom = .5f;

                    cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1214);














                    PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Tables[0].Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1219.Colspan = 1;
                    cell1219.BorderWidthLeft = 0f;
                    cell1219.BorderWidthRight = .8f;
                    cell1219.BorderWidthTop = .5f;
                    cell1219.BorderWidthBottom = .5f;

                    cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell1219);



                    string FPS = dt.Tables[0].Rows[i]["FrontPlateSize"].ToString();
                    string SQLString1 = "select * from Product where ProductID='" + FPS + "'";

                    dt1 = Utils.getDataSet(SQLString1, CnnString);




                    PdfPCell cell12193 = new PdfPCell(new Phrase(dt1.Tables[0].Rows[0]["ProductCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12193.Colspan = 1;
                    cell12193.BorderWidthLeft = 0f;
                    cell12193.BorderWidthRight = .8f;
                    cell12193.BorderWidthTop = .5f;
                    cell12193.BorderWidthBottom = .5f;

                    cell12193.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12193);


                    string FPS1 = dt.Tables[0].Rows[i]["RearPlateSize"].ToString();
                    string SQLString2 = "select * from Product where ProductID='" + FPS1 + "'";

                    dt2 = Utils.getDataSet(SQLString2, CnnString);

                    PdfPCell cell12194 = new PdfPCell(new Phrase(dt2.Tables[0].Rows[0]["ProductCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12194.Colspan = 1;
                    cell12194.BorderWidthLeft = 0f;
                    cell12194.BorderWidthRight = .8f;
                    cell12194.BorderWidthTop = .5f;
                    cell12194.BorderWidthBottom = .5f;

                    cell12194.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12194);


                    PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Tables[0].Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1216.Colspan = 1;
                    cell1216.BorderWidthLeft = 0f;
                    cell1216.BorderWidthRight = .8f;
                    cell1216.BorderWidthTop = .5f;
                    cell1216.BorderWidthBottom = .5f;

                    cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1216);


                    PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Tables[0].Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1222.Colspan = 1;
                    cell1222.BorderWidthLeft = 0f;
                    cell1222.BorderWidthRight = .8f;
                    cell1222.BorderWidthTop = .5f;
                    cell1222.BorderWidthBottom = .5f;

                    cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1222);




                    PdfPCell cell120935 = new PdfPCell(new Phrase(dt.Tables[0].Rows[i]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell120935.Colspan = 1;
                    cell120935.BorderWidthLeft = 0f;
                    cell120935.BorderWidthRight = .8f;
                    cell120935.BorderWidthTop = .5f;
                    cell120935.BorderWidthBottom = .5f;

                    cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120935);

                    PdfPCell cell120936 = new PdfPCell(new Phrase(dt.Tables[0].Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell120936.Colspan = 1;
                    cell120936.BorderWidthLeft = 0f;
                    cell120936.BorderWidthRight = .8f;
                    cell120936.BorderWidthTop = .5f;
                    cell120936.BorderWidthBottom = .5f;

                    cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120936);




                    //PdfPCell cell1224 = new PdfPCell(new Phrase(dt.Tables[0].Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell1224.Colspan = 2;
                    //cell1224.BorderWidthLeft = 0f;
                    //cell1224.BorderWidthRight = 1f;
                    //cell1224.BorderWidthTop = .5f;
                    //cell1224.BorderWidthBottom = .5f;

                    //cell1224.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell1224);



                    i--;



                }


                // document.Add(table);
                PdfPCell cell12241 = new PdfPCell(new Phrase("(Sign)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12241.Colspan = 7;
                cell12241.BorderWidthLeft = 0f;
                cell12241.BorderWidthRight = 0f;
                cell12241.BorderWidthTop = 0f;
                cell12241.BorderWidthBottom = 0f;

                cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12241);

                document.Add(table);
                // document.Add(table1);

                document.Close();
                HttpContext context = HttpContext.Current;

                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
            else
            {
                string closescript1 = "<script>alert('No records found for selected date.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            }
        }
    }

}