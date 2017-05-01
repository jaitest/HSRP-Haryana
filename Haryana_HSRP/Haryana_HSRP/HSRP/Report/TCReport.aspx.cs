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

namespace HSRP.DLReports
{
    public partial class TCReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;
        // int RTOLocationID;
        int intHSRPStateID;
        // int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        string FromDate, ToDate;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataProvider.BAL bl = new DataProvider.BAL();

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
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                     
                        InitialSetting();
                        if (UserType.Equals(0))
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            //Getlocation();
                            labelClient.Visible = false;
                            dropDownListClient.Visible = false;


                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            //Getlocation();
                            labelClient.Visible = false;
                            dropDownListClient.Visible = false;
                        }
                    }
                    catch (Exception err)
                    {
                        err.ToString();
                    }
                }
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
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }

    
      

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {

            if (DropDownListStateName.SelectedItem.ToString() == ("--Select State--"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select State..');", true);
            }

           else if (ddl_zone.SelectedItem.ToString() == ("--Select Zone--"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select Zone..');", true);
            }

            //else if (dropDownListClient.SelectedItem.ToString() == ("--Select Location--"))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select Location..');", true);
            //}


            else if (ddlTCReport.SelectedItem.ToString() == ("--Select Orders--"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select Order status..');", true);
            }


            else
            {
                //GridView1.Visible = true;
                 ExportToExcel();

            }
        }

        

        public void ExportToExcel()
        {
            try
            {
                LabelError.Text = "";

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);

                FromDate = OrderDate.SelectedDate.ToString("MM/dd/yy 00:00:00");
                ToDate = HSRPAuthDate.SelectedDate.ToString("MM/dd/yy 23:59:59");
                String ReportDate = FromDate;
                String reportdate12 = ToDate;

               // DataTable StateName;
                //DataTable dts;
                DataTable dt = new DataTable();


                string filename = "Order Detail Report-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Order Detail Report";
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


                WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = false;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style7 = book.Styles.Add("HeaderStyle7");
                style7.Font.FontName = "Tahoma";
                style7.Font.Size = 10;
                style7.Font.Bold = true;
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

                Worksheet sheet = book.Worksheets.Add("Order Detail Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(195));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(90));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(162));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));



                WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                style9.Font.FontName = "Tahoma";
                style9.Font.Size = 10;
                style9.Font.Bold = true;
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Order Detail Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text


                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Order Status", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ReportDate + " To " + reportdate12, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row = sheet.Table.Rows.Add();



                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Zone.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RtoLocationname", "HeaderStyle6"));

                row.Cells.Add(new WorksheetCell("Authorizationdate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleRegNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OwnerName", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleType", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleClass", "HeaderStyle6"));   // Daily Embossing
                row.Cells.Add(new WorksheetCell("OrderType", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderStatus", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("EngineNo", "HeaderStyle6"));                  // Daily Closed
                row.Cells.Add(new WorksheetCell("ChassisNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front_Lasercode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear_Lasercode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front_size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear_size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderEmbossingDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderClosedDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("StickerMandatory", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RoundOff_NetAmount", "HeaderStyle6"));





                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                row.Index = 9;

               
                int sno = 0;
                if (ddl_zone.SelectedItem.Text == "All")
                {

                    if (ddlTCReport.SelectedItem.Text == "Order Booked")
                    {

                        //SQLString = "[Report_Daily_Consol_AllState_New1]'" + OrderDate1 + "'";
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus, b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode, b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' order by a.rtoLocationname";
                        dt = Utils.GetDataTable(SQLString, CnnString);
                        //string RTOColName = string.Empty;
                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));
                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }

                    }



                    if (ddlTCReport.SelectedItem.Text == "Embossing")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode, b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and orderembossingdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' order by a.rtoLocationname";
                        dt = Utils.GetDataTable(SQLString, CnnString);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }
                    }

                    if (ddlTCReport.SelectedItem.Text == "Affixation")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus, b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' order by a.rtoLocationname";
                        dt = Utils.GetDataTable(SQLString, CnnString);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }
                    }

                    if (ddlTCReport.SelectedItem.Text == "Pending for Embossing")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and orderstatus='New Order'order by a.rtoLocationname";
                        dt = Utils.GetDataTable(SQLString, CnnString);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }
                    }

                    if (ddlTCReport.SelectedItem.Text == "Pending for Affixation")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and orderstatus='Embossing Done' order by a.rtoLocationname";
                        dt = Utils.GetDataTable(SQLString, CnnString);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }
                    }
                }
                else
                
                { 


                if (ddlTCReport.SelectedItem.Text == "Order Booked")
                {

                    //SQLString = "[Report_Daily_Consol_AllState_New1]'" + OrderDate1 + "'";
                    SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus, b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode, b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.zone='" + ddl_zone.SelectedValue.ToString() + "' order by 1,2";
                    dt = Utils.GetDataTable(SQLString, CnnString);
                    //string RTOColName = string.Empty;
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno = sno + 1;

                            // row = sheet.Table.Rows.Add();
                            row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                            row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                            row = sheet.Table.Rows.Add();

                        }
                    }

                }
                    if (ddlTCReport.SelectedItem.Text == "Embossing")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode, b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and orderembossingdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.zone='" + ddl_zone.SelectedValue.ToString() + "' order by 1,2";
                        dt = Utils.GetDataTable(SQLString, CnnString);
                       
                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }
                    }

                    if (ddlTCReport.SelectedItem.Text == "Affixation")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus, b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.zone='" + ddl_zone.SelectedValue.ToString() + "' order by 1,2";
                        dt = Utils.GetDataTable(SQLString, CnnString);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }
                    }

                    if (ddlTCReport.SelectedItem.Text == "Pending for Embossing")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.zone='" + ddl_zone.SelectedValue.ToString() + "' and orderstatus='New Order'order by 1,2";
                        dt = Utils.GetDataTable(SQLString, CnnString);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }
                    }

                    if (ddlTCReport.SelectedItem.Text == "Pending for Affixation")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.zone='" + ddl_zone.SelectedValue.ToString() + "' and orderstatus='Embossing Done'order by 1,2";
                        dt = Utils.GetDataTable(SQLString, CnnString);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                                row = sheet.Table.Rows.Add();

                            }
                        }
                    }
                }

                row = sheet.Table.Rows.Add();
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                // Save the file and open it
                book.Save(Response.OutputStream);

                //context.Response.ContentType = "text/csv";
                context.Response.ContentType = "application/vnd.ms-excel";

                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.End();

            }
            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        


        protected void ddlTCReport_SelectedIndexChanged(object sender, EventArgs e)
        {

           // ExportToExcel();
            //Getorders();
        }
       

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where   ActiveStatus='Y' Order by HSRPStateName";
                //Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='" + HSRPStateID + "' and ActiveStatus='Y' Order by HSRPStateName";
               // Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
            
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
          
        }

        private void FilldropDownListClient()
        {
            SQLString = "select distinct zone from RTOLocation where hsrp_stateid='"+DropDownListStateName.SelectedValue+"' and zone is not null order by zone";
            Utils.PopulateDropDownList(ddl_zone, SQLString.ToString(), CnnString, "All");
                       
        }

        protected void ddl_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddl_zone.SelectedItem.Text=="All")
            {
                labelClient.Visible = false;
                dropDownListClient.Visible = false;
            }
            else
            { 
            //Getlocation();
            labelClient.Visible = false;
            dropDownListClient.Visible = false;
            }
        }

        public void Getlocation()
        {
            SQLString = "select rtolocationid,rtolocationname from rtolocation where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and zone='" + ddl_zone.SelectedValue + "'";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        }
       

        #endregion



        private void ExportRecordExcel(Workbook book12, DataTable dt, string RtoName = null)
        {
            try
            {
                LabelError.Text = "";
                string strRtoName = String.Empty;
                string strRtoCode = String.Empty;
                if (string.IsNullOrEmpty(RtoName))
                {
                    strRtoName = dropDownListClient.SelectedItem.ToString();
                }
                else
                {
                    strRtoName = RtoName;
                }

                FromDate = OrderDate.SelectedDate.ToString("MM/dd/yy 00:00:00");
                ToDate = HSRPAuthDate.SelectedDate.ToString("MM/dd/yy 23:59:59");
                String ReportDate = FromDate;

                //string filename = "Order Detail Report-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book1 = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book1.ExcelWorkbook.ActiveSheetIndex = 1;
                book1.ExcelWorkbook.WindowTopX = 100;
                book1.ExcelWorkbook.WindowTopY = 200;
                book1.ExcelWorkbook.WindowHeight = 7000;
                book1.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book1.Properties.Author = "HSRP";
                book1.Properties.Title = "Order Detail Report";
                book1.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book1.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style8 = book1.Styles.Add("HeaderStyle8");
                style8.Font.FontName = "Tahoma";
                style8.Font.Size = 10;
                style8.Font.Bold = true;
                style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style8.Interior.Color = "#D4CDCD";
                style8.Interior.Pattern = StyleInteriorPattern.Solid;

                WorksheetStyle style5 = book1.Styles.Add("HeaderStyle5");
                style5.Font.FontName = "Tahoma";
                style5.Font.Size = 10;
                style5.Font.Bold = false;
                style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book1.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style7 = book1.Styles.Add("HeaderStyle7");
                style7.Font.FontName = "Tahoma";
                style7.Font.Size = 10;
                style7.Font.Bold = true;
                style7.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style2 = book1.Styles.Add("HeaderStyle2");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                WorksheetStyle style3 = book1.Styles.Add("HeaderStyle3");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 12;
                style3.Font.Bold = true;
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

               // Worksheet sheet = book1.Worksheets.Add("Order Detail Report");
                Worksheet sheet = book12.Worksheets.Add(strRtoName);
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(195));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(90));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(162));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));

                

                WorksheetStyle style9 = book1.Styles.Add("HeaderStyle9");
                style9.Font.FontName = "Tahoma";
                style9.Font.Size = 10;
                style9.Font.Bold = true;
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Order Detail Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text


                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("dd/M/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Order Status", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row = sheet.Table.Rows.Add();



                row.Index = 7;
                //row.Cells.Add("Order Date");

                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Zone.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RtoLocationname", "HeaderStyle6"));

                row.Cells.Add(new WorksheetCell("Authorizationdate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleRegNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OwnerName", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleType", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleClass", "HeaderStyle6"));   // Daily Embossing
                row.Cells.Add(new WorksheetCell("OrderType", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderStatus", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("EngineNo", "HeaderStyle6"));                  // Daily Closed
                row.Cells.Add(new WorksheetCell("ChassisNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front_Lasercode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear_Lasercode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front_size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear_size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderEmbossingDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderClosedDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("StickerMandatory", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RoundOff_NetAmount", "HeaderStyle6"));

                row = sheet.Table.Rows.Add();


                int sno = 0;
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;

                        // row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(Convert.ToInt32(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Zone"].ToString(), DataType.String, "HeaderStyle7"));

                        row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_authorizationdate"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle5"));
                        row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                        row = sheet.Table.Rows.Add();

                    }

                }
            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        private static void StyleForTheFirstTime(Workbook book)
        {
            // Specify which Sheet should be opened and the size of window by default
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "HSRP Affixation Report";
            book.Properties.Created = DateTime.Now;

            #region Style
            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 9;
            style.Font.Bold = false;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
            style8.Font.FontName = "Tahoma";
            style8.Font.Size = 10;
            style8.Font.Bold = true;
            style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style8.Interior.Color = "#D4CDCD";
            style8.Interior.Pattern = StyleInteriorPattern.Solid;

            WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
            style5.Font.FontName = "Tahoma";
            style5.Font.Size = 10;
            style5.Font.Bold = false;
            style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
            style6.Font.FontName = "Tahoma";
            style6.Font.Size = 10;
            style6.Font.Bold = true;
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

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

            WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
            style9.Font.FontName = "Tahoma";
            style9.Font.Size = 10;
            style9.Font.Bold = true;
            style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style9.Interior.Color = "#FCF6AE";
            style9.Interior.Pattern = StyleInteriorPattern.Solid;
            #endregion
        }
        protected void btnexportall_Click(object sender, EventArgs e)
        {
            if (DropDownListStateName.SelectedItem.ToString() == ("--Select State--"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select State..');", true);
            }
            else if (ddlTCReport.SelectedItem.ToString() == ("--Select Orders--"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select Order status..');", true);
            }
            else
            {
                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);

                FromDate = OrderDate.SelectedDate.ToString("MM/dd/yy 00:00:00");
                ToDate = HSRPAuthDate.SelectedDate.ToString("MM/dd/yy 23:59:59");
                String ReportDate = FromDate;
                DataTable dtRecord = new DataTable();
                int iCheckAllRtoHasNoRecord = 0;
                String StringOrderDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
                String StringAuthDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";
                Workbook book = new Workbook();
                //StyleForTheFirstTime(book);
                string filename = DropDownListStateName.SelectedItem.ToString() + "_Order Detail Report_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                DataTable dtrto = Utils.GetDataTable("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + DropDownListStateName.SelectedValue + "') and RTOLocationID not in (148)", CnnString);
                for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                {
                    string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                    string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                    // SQLString = "SELECT CONVERT(varchar(20), a.HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,convert(varchar,ordercloseddate,103) as ordercloseddate,  CONVERT(varchar(20), a.OrderDate, 103) AS OrderDate, a.HSRPRecordID,a.CashReceiptNo,a.InvoiceNo,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime, VehicleClass,CONVERT(numeric,round(a.RoundOff_NetAmount,0)) as NetAmount, a.HSRPRecord_AuthorizationNo, a.OwnerName, a.VehicleClass, a.VehicleType, a.VehicleRegNo, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.StickerMandatory,  (select P.productCode from Product P Where P.ProductID =a.FrontPlateSize) as FrontPlateCode, (select P.productCode from Product P Where P.ProductID =a.rearPlateSize) as RearPlateCode  FROM HSRPRecords a  where a.HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and a.RTOLocationID='" + RTOCode + "' and a.OrderClosedDate between '" + StringOrderDate + "' and '" + StringAuthDate + "' and orderstatus='closed' and  a.OwnerName is not null and a.OwnerName <> '' and Address1 is not null and Address1 <> '' order by OrderClosedDate";

                    if (ddlTCReport.SelectedItem.Text == "Order Booked")
                    {

                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus, b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode, b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.rtolocationid='" + RTOCode + "' order by 1,2";
                        dtRecord = Utils.GetDataTable(SQLString, CnnString);
                    }


                    if (ddlTCReport.SelectedItem.Text == "Embossing")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode, b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and orderembossingdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.rtolocationid='" + RTOCode + "' order by 1,2";
                        dtRecord = Utils.GetDataTable(SQLString, CnnString);
                    }


                    if (ddlTCReport.SelectedItem.Text == "Affixation")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus, b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.rtolocationid='" + RTOCode + "' order by 1,2";
                        dtRecord = Utils.GetDataTable(SQLString, CnnString);
                    }


                    if (ddlTCReport.SelectedItem.Text == "Pending for Embossing")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.rtolocationid='" + RTOCode + "' and orderstatus='New Order'order by 1,2";
                        dtRecord = Utils.GetDataTable(SQLString, CnnString);
                    }

                    if (ddlTCReport.SelectedItem.Text == "Pending for Affixation")
                    {
                        SQLString = "select a.Zone,a.rtoLocationname,b.hsrprecord_authorizationdate,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.OrderType,b.OrderStatus,b.MobileNo,b.EngineNo,b.ChassisNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.OrderClosedDate,b.StickerMandatory,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and a.rtolocationid='" + RTOCode + "' and orderstatus='Embossing Done'order by 1,2";
                        dtRecord = Utils.GetDataTable(SQLString, CnnString);
                    }
                    if (dtRecord.Rows.Count > 0)
                    {
                        iCheckAllRtoHasNoRecord++;
                        ExportRecordExcel(book, dtRecord, RTOName);
                    }
                }

                if (iCheckAllRtoHasNoRecord > 0)
                {
               
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);
                    context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();

                }
                else
                {
                    LabelError.Text = "No Record For the Selected Date.";
                }

            }
        }

    }
}

           

        
   

 
