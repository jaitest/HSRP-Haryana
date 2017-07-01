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
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class DownloadProductionReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        BAL obj = new BAL();
        DataTable dtdownload;
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
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            // labelClient.Visible = false;
                            // dropDownListClient.Visible = false;

                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            labelOrganization.Enabled = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Enabled = true;
                            // labelDate.Visible = false;

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
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
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
                // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";

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
                LabelError.Text = "";
                string typesch = string.Empty;
                if (HSRP.Checked == true)
                {
                    typesch = "1";
                }
                if (Dealersc.Checked == true)
                {
                    typesch = "2";
                }
                String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];
                  

                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To    " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";

                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                string statename = DropDownListStateName.SelectedValue;
                string location = dropDownListClient.SelectedValue;
                string start = ReportDate1;
                string End = ReportDate2;

                DataTable dt = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "DailyProductionSheet-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP PRODUCTION REPORT";
                book.Properties.Created = DateTime.Now;
                
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

                Worksheet sheet = book.Worksheets.Add("HSRP PRODUCTION REPORT");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(150));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(92));
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

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HSRP PRODUCTION REPORT");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                //row = sheet.Table.Rows.Add();
                //row.Index = 4;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("dd/M/yyyy"), "HeaderStyle2"));
                //row = sheet.Table.Rows.Add();
                //row.Index = 6;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DatePrint, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 5;

                
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("User Name :", "HeaderStyle2"));

                row.Cells.Add(new WorksheetCell(Session["UserName"].ToString(), "HeaderStyle2"));
 
                row.Cells.Add(new WorksheetCell("Record Type :", "HeaderStyle2"));
                if (HSRP.Checked == true)
                {
                    row.Cells.Add(new WorksheetCell("HSRP Record", "HeaderStyle2"));
                }
                else
                {
                    row.Cells.Add(new WorksheetCell("Dealer Record", "HeaderStyle2"));
                }
                
                row = sheet.Table.Rows.Add();


                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));   
                row.Cells.Add(new WorksheetCell("Bill No", "HeaderStyle6")); 
                row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vechicle Type", "HeaderStyle6")); 
                row.Cells.Add(new WorksheetCell("Engine No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Chasis No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Sticker", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Plate Size Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front Laser", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Plate size Rear", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear Laser", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 8;

                UserType = Convert.ToInt32(Session["UserType"]);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                obj.GetdownloadReport(statename, location, start, End,typesch, ref dt);

                string RTOColName = string.Empty;
                decimal totalAmount = 0;
                if (dt.Rows.Count > 0)
                {
                    int sno = 0;
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        if (sno == 43)
                        {
                          int  ssno = sno;
                        }
                         

                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashReceiptNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle")); 
                        row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle"));
                        string amount = dtrows["NetAmount"].ToString();
                        if (amount == "")
                        {

                            row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle"));
                            //totalAmount = totalAmount + Math.Round(Convert.ToDecimal(amount.ToString()));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString())).ToString(), DataType.Number, "HeaderStyle5"));
                            totalAmount = totalAmount + Math.Round(Convert.ToDecimal(dtrows["NetAmount"].ToString()));
                        }

                        row.Cells.Add(new WorksheetCell(dtrows["FrontProductCode"].ToString(), DataType.String, "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["RearProductCode"].ToString(), DataType.String, "HeaderStyle2"));
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));

                        row = sheet.Table.Rows.Add();
                    }
                    row = sheet.Table.Rows.Add();
                    //row = sheet.Table.Rows.Add();
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle8")); 
                    //row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle8"));

                    totalAmount = 0;

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
            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void btnExportTOPDF_Click(object sender, EventArgs e)
        {
           // DownloadProductioninPDF();
        }
        //private void DownloadProductioninPDF()
        //{
        //    LabelError.Text = "";
        //    String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
        //    String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
        //    string ReportDate2 = ReportDateEnd + " 23:59:59";
        //    String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


        //    String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
        //    string Mon = ("0" + StringOrderDate[0]);
        //    string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
        //    String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

        //    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
        //    String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

        //    String DatePrint = DatePrint1 + "   To    " + DatePrint2;

        //    String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
        //    string ReportDate1 = ReportDate + " 00:00:00";

        //    OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

        //    DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
        //    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
        //    string statename = DropDownListStateName.SelectedValue;
        //    string location = dropDownListClient.SelectedValue;
        //    string start = ReportDate1;
        //    string End = ReportDate2;

        //    DataTable dt = new DataTable();

        //    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

        //    try
        //    {
        //        //SQLString = "select * from hsrprecords where FrontPlateSize ='--Select Front Plate--'";
        //        //Utils.ExecNonQuery(SQLString, CnnString);
        //        SQLString = "update hsrprecords set FrontPlateSize ='0' where FrontPlateSize ='--Select Front Plate--'";
        //        Utils.ExecNonQuery(SQLString, CnnString);

        //        SQLString = "update hsrprecords set RearPlateSize ='0' where RearPlateSize ='--Select Rear Plate--'";
        //        Utils.ExecNonQuery(SQLString, CnnString);

        //    }
        //    catch
        //    {
        //    }
        //    string typesch = "2";
        //    //SQLString = "Select  a.hsrprecordID, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID where a.hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and a.RTOLocationID='" + dropDownListClient.SelectedValue + "' and a.sendtoproductionStatus='N'";
        //    //SQLString = "Select   a.hsrprecordID, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate,  left(a.OwnerName,19) as OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, s.HSRPStateName, left (replace (Product_1.ProductCode,'MM-',''),9) AS RearProductCode, left (replace (Product.ProductCode,'MM-',''),9) AS FrontProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID where a.hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and a.RTOLocationID='" + dropDownListClient.SelectedValue + "' and a.sendtoproductionStatus='N'  and OrderStatus='New Order'";
        //    //DataTable dt = Utils.GetDataTable(SQLString, CnnString);
        //    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //    obj.GetdownloadReport(statename, location, start, End, typesch, ref dt);

        //    if (dt.Rows.Count > 0)
        //    {
        //        string filename = "Order_Open_Records" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
        //        String StringField = String.Empty;
        //        String StringAlert = String.Empty;
        //        StringBuilder bb = new StringBuilder();
        //        // Document document = new Document();
        //        //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
        //        Document document = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 3, 0);
        //        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


        //        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        //        // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
        //        //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
        //        string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
        //        PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
        //        //Opens the document:
        //        document.Open();

        //        //Adds content to the document:
        //        // document.Add(new Paragraph("Ignition Log Report"));
        //        PdfPTable table = new PdfPTable(14);
        //        //actual width of table in points
        //        var colWidthPercentages = new[] { 5f, 12f, 8f, 14f, 10f, 25f, 25f, 30f, 14f, 15f, 23f, 15f, 23f, 20f };
        //        table.SetWidths(colWidthPercentages);
        //        table.TotalWidth = 6900f;

        //        PdfPCell cell120911 = new PdfPCell(new Phrase("HSRP Order Booking Report", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell120911.Colspan = 14;
        //        cell120911.BorderWidthLeft = 0f;
        //        cell120911.BorderWidthRight = 0f;
        //        cell120911.BorderWidthTop = 0f;
        //        cell120911.BorderWidthBottom = 0f;
        //        //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell120911);

        //        PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + dt.Rows[0]["HSRPStateName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell12091.Colspan = 9;
        //        cell12091.BorderWidthLeft = 1f;
        //        cell12091.BorderWidthRight = 0f;
        //        cell12091.BorderWidthTop = 1f;
        //        cell12091.BorderWidthBottom = 0f;
        //        cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell12091);

        //        PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date From : " + ReportDateEnd , new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell12093.Colspan = 5;
        //        cell12093.BorderWidthLeft = 0f;
        //        cell12093.BorderWidthRight = 1f;
        //        cell12093.BorderWidthTop = 1f;
        //        cell12093.BorderWidthBottom = 0f;

        //        cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell12093);

        //        PdfPCell cell12092 = new PdfPCell(new Phrase("RTOLocation Name : " + dt.Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell12092.Colspan = 6;
        //        cell12092.BorderWidthLeft = 1f;
        //        cell12092.BorderWidthRight = 0f;
        //        cell12092.BorderWidthTop = 0f;
        //        cell12092.BorderWidthBottom = 0f;

        //        cell12092.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell12092);
        //        PdfPCell cell12094 = new PdfPCell(new Phrase("Order Status : New Order" + OrderStatus, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell12094.Colspan = 3;
        //        cell12094.BorderWidthLeft = 0f;
        //        cell12094.BorderWidthRight = 0f;
        //        cell12094.BorderWidthTop = 0f;
        //        cell12094.BorderWidthBottom = 0f;

        //        cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell12094);

        //        PdfPCell cell12095 = new PdfPCell(new Phrase("Report Date To : " + StartDate, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell12095.Colspan = 5;
        //        cell12095.BorderWidthLeft = 0f;
        //        cell12095.BorderWidthRight = 1f;
        //        cell12095.BorderWidthTop = 0f;
        //        cell12095.BorderWidthBottom = 0f;

        //        cell12095.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell12095);

        //        PdfPCell cell12 = new PdfPCell(new Phrase("S.R.No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell12.Colspan = 1;
        //        cell12.BorderWidthLeft = 1f;
        //        cell12.BorderWidthRight = .8f;
        //        cell12.BorderWidthTop = 0.8f;
        //        cell12.BorderWidthBottom = 0.8f;

        //        cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell12);

        //        PdfPCell cell1210 = new PdfPCell(new Phrase("Creation Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell1210.Colspan = 1;
        //        cell1210.BorderWidthLeft = 0f;
        //        cell1210.BorderWidthRight = .8f;
        //        cell1210.BorderWidthTop = 0.8f;
        //        cell1210.BorderWidthBottom = 0.8f;

        //        cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell1210);

        //        PdfPCell cell1213 = new PdfPCell(new Phrase("Vehicle Class", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell1213.Colspan = 1;
        //        cell1213.BorderWidthLeft = 0f;
        //        cell1213.BorderWidthRight = .8f;
        //        cell1213.BorderWidthTop = 0.8f;
        //        cell1213.BorderWidthBottom = 0.8f;

        //        cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell1213);


        //        PdfPCell cell1209 = new PdfPCell(new Phrase("Vehicle No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell1209.Colspan = 1;
        //        cell1209.BorderWidthLeft = 0f;
        //        cell1209.BorderWidthRight = .8f;
        //        cell1209.BorderWidthTop = 0.8f;
        //        cell1209.BorderWidthBottom = 0.8f;

        //        cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell1209);

        //        PdfPCell cell12233 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell12233.Colspan = 1;
        //        cell12233.BorderWidthLeft = 0f;
        //        cell12233.BorderWidthRight = .8f;
        //        cell12233.BorderWidthTop = 0.8f;
        //        cell12233.BorderWidthBottom = 0.8f;

        //        cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell12233);


        //        PdfPCell cell1206 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell1206.Colspan = 1;
        //        cell1206.BorderWidthLeft = 0f;
        //        cell1206.BorderWidthRight = .8f;
        //        cell1206.BorderWidthTop = 0.8f;
        //        cell1206.BorderWidthBottom = 0.8f;
        //        cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

        //        table.AddCell(cell1206);

        //        PdfPCell cell1221 = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell1221.Colspan = 1;
        //        cell1221.BorderWidthLeft = 0.8f;
        //        cell1221.BorderWidthRight = 0.8f;
        //        cell1221.BorderWidthTop = .8f;
        //        cell1221.BorderWidthBottom = 0.8f;

        //        cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell1221);


        //        PdfPCell cell120933 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell120933.Colspan = 1;
        //        cell120933.BorderWidthLeft = 0f;
        //        cell120933.BorderWidthRight = 0.8f;
        //        cell120933.BorderWidthTop = 0.8f;
        //        cell120933.BorderWidthBottom = 0f;

        //        cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell120933);

        //        PdfPCell cell120934 = new PdfPCell(new Phrase("Mobile No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell120934.Colspan = 1;
        //        cell120934.BorderWidthLeft = 0f;
        //        cell120934.BorderWidthRight = 0.8f;
        //        cell120934.BorderWidthTop = 0.8f;
        //        cell120934.BorderWidthBottom = 0f;

        //        cell120934.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell120934);

        //        PdfPCell cell120935 = new PdfPCell(new Phrase("Front Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell120935.Colspan = 1;
        //        cell120935.BorderWidthLeft = 0f;
        //        cell120935.BorderWidthRight = 0.8f;
        //        cell120935.BorderWidthTop = 0.8f;
        //        cell120935.BorderWidthBottom = 0f;

        //        cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell120935);

        //        PdfPCell cell120936 = new PdfPCell(new Phrase("Front Laser No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell120936.Colspan = 1;
        //        cell120936.BorderWidthLeft = 0f;
        //        cell120936.BorderWidthRight = 0.8f;
        //        cell120936.BorderWidthTop = 0.8f;
        //        cell120936.BorderWidthBottom = 0f;

        //        cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell120936);


        //        PdfPCell cell120937 = new PdfPCell(new Phrase("Rear Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell120937.Colspan = 1;
        //        cell120937.BorderWidthLeft = 0f;
        //        cell120937.BorderWidthRight = 0.8f;
        //        cell120937.BorderWidthTop = 0.8f;
        //        cell120937.BorderWidthBottom = 0f;

        //        cell120937.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell120937);

        //        PdfPCell cell120938 = new PdfPCell(new Phrase("Rear Laser No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell120938.Colspan = 1;
        //        cell120938.BorderWidthLeft = 0f;
        //        cell120938.BorderWidthRight = 0.8f;
        //        cell120938.BorderWidthTop = 0.8f;
        //        cell120938.BorderWidthBottom = 0f;

        //        cell120938.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell120938);

        //        PdfPCell cell120939 = new PdfPCell(new Phrase("Remarks", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell120939.Colspan = 1;
        //        cell120939.BorderWidthLeft = 0f;
        //        cell120939.BorderWidthRight = 0.8f;
        //        cell120939.BorderWidthTop = 0.8f;
        //        cell120939.BorderWidthBottom = 0f;

        //        cell120939.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell120939);

        //        int j = 0;
        //        int total = 0;
        //        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //        {

        //            j = j + 1;

        //            //=========================================================================================================================================
        //            if (total == 53)
        //            {
        //                total = 0;

        //                PdfPCell cell120911a = new PdfPCell(new Phrase("HSRP Order Booking Report", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell120911a.Colspan = 14;
        //                cell120911a.BorderWidthLeft = 0f;
        //                cell120911a.BorderWidthRight = 0f;
        //                cell120911a.BorderWidthTop = 0f;
        //                cell120911a.BorderWidthBottom = 0f;
        //                //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
        //                cell120911a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell120911a);

        //                PdfPCell cell12091a = new PdfPCell(new Phrase("State Name : " + dt.Rows[0]["HSRPStateName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //                cell12091a.Colspan = 9;
        //                cell12091a.BorderWidthLeft = 1f;
        //                cell12091a.BorderWidthRight = 0f;
        //                cell12091a.BorderWidthTop = 1f;
        //                cell12091a.BorderWidthBottom = 0f;
        //                cell12091a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell12091a);

        //                PdfPCell cell12093a = new PdfPCell(new Phrase("Report Date From : " + ReportDateEnd, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //                cell12093a.Colspan = 5;
        //                cell12093a.BorderWidthLeft = 0f;
        //                cell12093a.BorderWidthRight = 1f;
        //                cell12093a.BorderWidthTop = 1f;
        //                cell12093a.BorderWidthBottom = 0f;
        //                cell12093a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell12093a);

        //                PdfPCell cell12092a = new PdfPCell(new Phrase("RTOLocation Name : " + dt.Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //                cell12092a.Colspan = 6;
        //                cell12092a.BorderWidthLeft = 1f;
        //                cell12092a.BorderWidthRight = 0f;
        //                cell12092a.BorderWidthTop = 0f;
        //                cell12092a.BorderWidthBottom = 0f;
        //                cell12092a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell12092a);

        //                PdfPCell cell12094a = new PdfPCell(new Phrase("Order Status : New Order" + OrderStatus, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //                cell12094a.Colspan = 3;
        //                cell12094a.BorderWidthLeft = 0f;
        //                cell12094a.BorderWidthRight = 0f;
        //                cell12094a.BorderWidthTop = 0f;
        //                cell12094a.BorderWidthBottom = 0f;
        //                cell12094a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell12094a);

        //                PdfPCell cell12095a = new PdfPCell(new Phrase("Report Date To : " + StartDate, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //                cell12095a.Colspan = 5;
        //                cell12095a.BorderWidthLeft = 0f;
        //                cell12095a.BorderWidthRight = 1f;
        //                cell12095a.BorderWidthTop = 0f;
        //                cell12095a.BorderWidthBottom = 0f;
        //                cell12095a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell12095a);

        //                PdfPCell cell12a = new PdfPCell(new Phrase("S.R.No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell12a.Colspan = 1;
        //                cell12a.BorderWidthLeft = 1f;
        //                cell12a.BorderWidthRight = .8f;
        //                cell12a.BorderWidthTop = 0.8f;
        //                cell12a.BorderWidthBottom = 0.8f;

        //                cell12a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell12a);

        //                PdfPCell cell1210a = new PdfPCell(new Phrase("Creation Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell1210a.Colspan = 1;
        //                cell1210a.BorderWidthLeft = 0f;
        //                cell1210a.BorderWidthRight = .8f;
        //                cell1210a.BorderWidthTop = 0.8f;
        //                cell1210a.BorderWidthBottom = 0.8f;

        //                cell1210a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell1210a);

        //                PdfPCell cell1213a = new PdfPCell(new Phrase("Vehicle Class", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell1213a.Colspan = 1;
        //                cell1213a.BorderWidthLeft = 0f;
        //                cell1213a.BorderWidthRight = .8f;
        //                cell1213a.BorderWidthTop = 0.8f;
        //                cell1213a.BorderWidthBottom = 0.8f;
        //                cell1213a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell1213a);


        //                PdfPCell cell1209a = new PdfPCell(new Phrase("Vehicle No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell1209a.Colspan = 1;
        //                cell1209a.BorderWidthLeft = 0f;
        //                cell1209a.BorderWidthRight = .8f;
        //                cell1209a.BorderWidthTop = 0.8f;
        //                cell1209a.BorderWidthBottom = 0.8f;
        //                cell1209a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell1209a);

        //                PdfPCell cell12233a = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell12233a.Colspan = 1;
        //                cell12233a.BorderWidthLeft = 0f;
        //                cell12233a.BorderWidthRight = .8f;
        //                cell12233a.BorderWidthTop = 0.8f;
        //                cell12233a.BorderWidthBottom = 0.8f;
        //                cell12233a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell12233a);


        //                PdfPCell cell1206a = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell1206a.Colspan = 1;
        //                cell1206a.BorderWidthLeft = 0f;
        //                cell1206a.BorderWidthRight = .8f;
        //                cell1206a.BorderWidthTop = 0.8f;
        //                cell1206a.BorderWidthBottom = 0.8f;
        //                cell1206a.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell1206a);

        //                PdfPCell cell1221a = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell1221a.Colspan = 1;
        //                cell1221a.BorderWidthLeft = 0f;
        //                cell1221a.BorderWidthRight = 0.8f;
        //                cell1221a.BorderWidthTop = 0.8f;
        //                cell1221a.BorderWidthBottom = 0.8f;
        //                cell1221a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell1221a);


        //                PdfPCell cell120933a = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell120933a.Colspan = 1;
        //                cell120933a.BorderWidthLeft = 0f;
        //                cell120933a.BorderWidthRight = 0.8f;
        //                cell120933a.BorderWidthTop = 0.8f;
        //                cell120933a.BorderWidthBottom = 0f;
        //                cell120933a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell120933a);

        //                PdfPCell cell120934a = new PdfPCell(new Phrase("Mobile No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell120934a.Colspan = 1;
        //                cell120934a.BorderWidthLeft = 0f;
        //                cell120934a.BorderWidthRight = 0.8f;
        //                cell120934a.BorderWidthTop = 0.8f;
        //                cell120934a.BorderWidthBottom = 0f;
        //                cell120934a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell120934a);

        //                PdfPCell cell120935a = new PdfPCell(new Phrase("Front Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell120935a.Colspan = 1;
        //                cell120935a.BorderWidthLeft = 0f;
        //                cell120935a.BorderWidthRight = 0.8f;
        //                cell120935a.BorderWidthTop = 0.8f;
        //                cell120935a.BorderWidthBottom = 0f;
        //                cell120935a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell120935a);

        //                PdfPCell cell120936a = new PdfPCell(new Phrase("Front Laser No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell120936a.Colspan = 1;
        //                cell120936a.BorderWidthLeft = 0f;
        //                cell120936a.BorderWidthRight = 0.8f;
        //                cell120936a.BorderWidthTop = 0.8f;
        //                cell120936a.BorderWidthBottom = 0f;
        //                cell120936a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell120936a);


        //                PdfPCell cell120937a = new PdfPCell(new Phrase("Rear Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell120937a.Colspan = 1;
        //                cell120937a.BorderWidthLeft = 0f;
        //                cell120937a.BorderWidthRight = 0.8f;
        //                cell120937a.BorderWidthTop = 0.8f;
        //                cell120937a.BorderWidthBottom = 0f;
        //                cell120937a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell120937a);

        //                PdfPCell cell120938a = new PdfPCell(new Phrase("Front Laser No.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell120938a.Colspan = 1;
        //                cell120938a.BorderWidthLeft = 0f;
        //                cell120938a.BorderWidthRight = 0.8f;
        //                cell120938a.BorderWidthTop = 0.8f;
        //                cell120938a.BorderWidthBottom = 0f;
        //                cell120938a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell120938a);

        //                PdfPCell cell120939a = new PdfPCell(new Phrase("Remarks", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //                cell120939a.Colspan = 1;
        //                cell120939a.BorderWidthLeft = 0f;
        //                cell120939a.BorderWidthRight = 0.8f;
        //                cell120939a.BorderWidthTop = 0.8f;
        //                cell120939a.BorderWidthBottom = 0f;
        //                cell120939a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //                table.AddCell(cell120939a);

        //            }
        //            total = total + 1;
        //            //============================================================ ajay end ======================================================================
        //            PdfPCell cell13 = new PdfPCell(new Phrase("" + j, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell13.Colspan = 1;
        //            cell13.BorderWidthLeft = 0.8f;
        //            cell13.BorderWidthRight = 0f;
        //            cell13.BorderWidthTop = 0f;
        //            cell13.BorderWidthBottom = .5f;

        //            cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell13);

        //            PdfPCell cell1212 = new PdfPCell(new Phrase(dt.Rows[i]["OrderDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell1212.Colspan = 1;
        //            cell1212.BorderWidthLeft = 0.8f;
        //            cell1212.BorderWidthRight = .8f;
        //            cell1212.BorderWidthTop = 0f;
        //            cell1212.BorderWidthBottom = .5f;

        //            cell1212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1212);

        //            PdfPCell cell1214 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell1214.Colspan = 1;
        //            cell1214.BorderWidthLeft = 0f;
        //            cell1214.BorderWidthRight = .8f;
        //            cell1214.BorderWidthTop = 0f;
        //            cell1214.BorderWidthBottom = .5f;
        //            cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1214);

        //            PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell1211.Colspan = 1;
        //            cell1211.BorderWidthLeft = 0f;
        //            cell1211.BorderWidthRight = 0.8f;
        //            cell1211.BorderWidthTop = 0f;
        //            cell1211.BorderWidthBottom = .5f;
        //            cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1211);



        //            PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell1219.Colspan = 1;
        //            cell1219.BorderWidthLeft = 0f;
        //            cell1219.BorderWidthRight = .8f;
        //            cell1219.BorderWidthTop = 0f;
        //            cell1219.BorderWidthBottom = .5f;

        //            cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

        //            table.AddCell(cell1219);


        //            PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell1216.Colspan = 1;
        //            cell1216.BorderWidthLeft = 0f;
        //            cell1216.BorderWidthRight = .8f;
        //            cell1216.BorderWidthTop = 0f;
        //            cell1216.BorderWidthBottom = .5f;

        //            cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

        //            table.AddCell(cell1216);

        //            PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell1222.Colspan = 1;
        //            cell1222.BorderWidthLeft = 0f;
        //            cell1222.BorderWidthRight = .8f;
        //            cell1222.BorderWidthTop = 0f;
        //            cell1222.BorderWidthBottom = .5f;

        //            cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1222);


        //            PdfPCell cell1209315 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell1209315.Colspan = 1;
        //            cell1209315.BorderWidthLeft = 0f;
        //            cell1209315.BorderWidthRight = .8f;
        //            cell1209315.BorderWidthTop = .5f;
        //            cell1209315.BorderWidthBottom = .5f;

        //            cell1209315.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1209315);

        //            PdfPCell cell1209340 = new PdfPCell(new Phrase(dt.Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //            cell1209340.Colspan = 1;
        //            cell1209340.BorderWidthLeft = 0f;
        //            cell1209340.BorderWidthRight = .8f;
        //            cell1209340.BorderWidthTop = .5f;
        //            cell1209340.BorderWidthBottom = .5f;

        //            cell1209340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1209340);


        //            PdfPCell cell1209316 = new PdfPCell(new Phrase(dt.Rows[i]["FrontProductCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //            cell1209316.Colspan = 1;
        //            cell1209316.BorderWidthLeft = 0f;
        //            cell1209316.BorderWidthRight = .8f;
        //            cell1209316.BorderWidthTop = .5f;
        //            cell1209316.BorderWidthBottom = .5f;
        //            cell1209316.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1209316);


        //            PdfPCell cell1209317 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //            cell1209317.Colspan = 1;
        //            cell1209317.BorderWidthLeft = 0f;
        //            cell1209317.BorderWidthRight = .8f;
        //            cell1209317.BorderWidthTop = .5f;
        //            cell1209317.BorderWidthBottom = .5f;

        //            cell1209317.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1209317);


        //            PdfPCell cell1209318 = new PdfPCell(new Phrase(dt.Rows[i]["RearProductCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //            cell1209318.Colspan = 1;
        //            cell1209318.BorderWidthLeft = 0f;
        //            cell1209318.BorderWidthRight = .8f;
        //            cell1209318.BorderWidthTop = .5f;
        //            cell1209318.BorderWidthBottom = .5f;

        //            cell1209318.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1209318);


        //            PdfPCell cell1209319 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //            cell1209319.Colspan = 1;
        //            cell1209319.BorderWidthLeft = 0f;
        //            cell1209319.BorderWidthRight = .8f;
        //            cell1209319.BorderWidthTop = .5f;
        //            cell1209319.BorderWidthBottom = .5f;

        //            cell1209319.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1209319);

        //            PdfPCell cell1209329 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //            cell1209329.Colspan = 1;
        //            cell1209329.BorderWidthLeft = 0f;
        //            cell1209329.BorderWidthRight = .8f;
        //            cell1209329.BorderWidthTop = .5f;
        //            cell1209329.BorderWidthBottom = .5f;

        //            cell1209329.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1209329);



        //        }

        //        try
        //        {
        //            string mean = "MO.C = MOTOR CYCLE,  L.CL =LMV(CLASS),  TRAC =TRACTOR,  SCOO = SCOOTER,  T.Whe.= THREE WHEELER,  Trailers =MCV/HCV/TRAILERS,  T = Transport, N.T.= Non-Transport";
        //            PdfPCell cell1209340 = new PdfPCell(new Phrase(mean, new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //            cell1209340.Colspan = 14;
        //            cell1209340.BorderWidthLeft = .8f;
        //            cell1209340.BorderWidthRight = .8f;
        //            cell1209340.BorderWidthTop = .8f;
        //            cell1209340.BorderWidthBottom = .5f;
        //            cell1209340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell1209340);

        //            PdfPCell cell12241 = new PdfPCell(new Phrase("(Sign)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell12241.Colspan = 7;
        //            cell12241.BorderWidthLeft = 0f;
        //            cell12241.BorderWidthRight = 0f;
        //            cell12241.BorderWidthTop = 0f;
        //            cell12241.BorderWidthBottom = 0f;
        //            cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell12241);
        //            document.Add(table);
        //            // document.Add(table1); 

        //            document.Close();

        //            int count = 1;
        //            try
        //            {
        //                for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //                {
        //                    //SQLString = "update hsrprecords set sendtoProductionStatus='Y', PdfRunningNo='" + count + "', PdfDownloadDate=GetDate(), pdfFileName='" + filename + "', PDFDownloadUserID='" + Session["UID"].ToString() + "' where hsrprecordID='" + dt.Rows[i]["hsrprecordID"].ToString() + "'";
        //                    //count = count + 1;
        //                    //Utils.ExecNonQuery(SQLString, CnnString);
        //                }
        //            }
        //            catch
        //            {

        //            }

        //            HttpContext context = HttpContext.Current;

        //            context.Response.ContentType = "Application/pdf";
        //            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //            context.Response.WriteFile(PdfFolder);
        //            context.Response.End();

        //        }
        //        catch
        //        {

        //        }
        //    }
        //    //lblErrMsg.Text = "No Record Found!!";
        //}
    }
}