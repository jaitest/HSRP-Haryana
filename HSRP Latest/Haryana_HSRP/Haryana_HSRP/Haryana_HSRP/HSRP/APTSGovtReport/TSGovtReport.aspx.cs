using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web;
using System.Data.SqlClient;

namespace HSRP.APTSGovtReport
{
    public partial class TSGovtReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;

        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;
        DataTable dt12 = new DataTable();
        DataTable dtauth = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {


                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    //InitialSetting();
                    try
                    {

                        if (UserType == "0")
                        {
                            //labelOrganization.Visible = true;
                            // DropDownListStateName.Visible = true;
                            // DropDownListStateName.Enabled = true;

                            //FilldropDownListOrganization();

                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            //labelOrganization.Enabled = false;
                            //DropDownListStateName.Enabled = false;

                            //  FilldropDownListOrganization();


                            //buildGrid();
                        }

                        //ShowGrid();
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }



        //public void FileDetail()
        //{

        //}

        //private void FilldropDownListOrganization()
        //{
        //    if (UserType == "0")
        //    {
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
        //        Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
        //        // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

        //    }
        //    else
        //    {
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
        //        DataTable dts = Utils.GetDataTable(SQLString, CnnString);
        //        DropDownListStateName.DataSource = dts;
        //        DropDownListStateName.DataBind();
        //    }
        //}

        //#endregion

        //private void ShowGrid()
        //{

        //}

        //private void InitialSetting()
        //{
        //    string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
        //    OrderDate.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //}


        //protected void btnGO_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string type = "1";
        //        String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
        //        string MonTo = ("0" + StringAuthDate[0]);
        //        string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
        //        String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
        //        String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
        //        //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
        //        string AuthorizationDate = From + " 00:00:00";
        //        String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
        //        string Mon = ("0" + StringOrderDate[0]);
        //        string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
        //        String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
        //        String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
        //        String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
        //        string ToDate = From1 + " 23:59:59";

        //        string SQLString = "select rtolocationname as [RTOName],count(*) as Performance from hsrprecords a,rtolocation b where a.RTOLocationID=b.rtolocationid and a.HSRP_StateID='" + DropDownListStateName.SelectedValue.ToString() + "' and RecievedAtAffixationDateTime between '" + AuthorizationDate + "' and '" + ToDate + "'group by rtolocationname";
        //        DataTable dt = Utils.GetDataTable(SQLString, CnnString);
        //        if (dt.Rows.Count > 0)
        //        {
        //            GridView1.DataSource = dt;
        //            GridView1.DataBind();
        //        }
        //        else
        //        {

        //            lblErrMsg.Text = "Record Not Found";
        //            GridView1.DataSource = null;
        //            GridView1.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblErrMsg.Text = ex.ToString();
        //    }

        //}
        // TS GET AUTH NO from Govt
        private void TSGenerateExcelReport()
        {
            string RTOName = string.Empty;

            try
            {

                //SetFolder(RTOName, "AP", "Royalty and Tax-");
                string filename = "APRegnoNotRecieved-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".xls";
                Workbook book = new Workbook();

                #region Excel
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily Cash Collection";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
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

                Worksheet sheet = book.Worksheets.Add("AP Registration Number Not Received Records");
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
                #endregion

                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("AP Registration Number Not Received Records");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));


                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 5;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RTO", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("AuthNo", "HeaderStyle6"));


                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                string SQLString = string.Empty;
                row.Index = 7;



                string AuthNo = string.Empty;

                TSHSRPService.HSRPAuthorizationServiceSoapClient yy = new TSHSRPService.HSRPAuthorizationServiceSoapClient();
                string dateTo = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy");
                string AuthData = yy.GetAuthorizationnotRegistered("01/01/2015", dateTo);
                if (AuthData.Length > 1)
                {
                    dtauth.Columns.Add("Authno", typeof(string));
                    using (StringReader stringReader = new StringReader(AuthData))
                    using (XmlTextReader reader = new XmlTextReader(stringReader))
                    {
                        while (reader.Read())
                        {

                            if (reader.Name.ToString() == "AuthNo")
                            {
                                reader.Read();
                                if (AuthNo.Length == 0)
                                {
                                    AuthNo = "'" + reader.Value.ToString() + "'";
                                }
                                else
                                {
                                    AuthNo = AuthNo + ",'" + reader.Value.ToString() + "'";
                                    var Seperator = new string[] { "','" };
                                    string[] Seperator12 = AuthNo.Split(Seperator, StringSplitOptions.RemoveEmptyEntries);

                                    DataRow row1;
                                    for (int i = 0; i < Seperator12.Length; i++)
                                    {
                                        //dtauth.Clear();
                                        row1 = dtauth.NewRow();
                                        row1["Authno"] = Seperator12[i];
                                        dtauth.Rows.Add(row1);
                                    }
                                }
                                reader.Read();
                            }
                        }
                    }
                }



                if (dtauth.Rows.Count > 0)
                {
                    Int64 sno = 0;
                    foreach (DataRow dtrows in dtauth.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        if (sno == 43)
                        {
                            Int64 ssno = sno;
                        }
                        row.Cells.Add(new WorksheetCell(Convert.ToInt64(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["AuthNo"].ToString(), DataType.String, "HeaderStyle"));
                        row = sheet.Table.Rows.Add();
                        row = sheet.Table.Rows.Add();
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
                else
                {
                }
            }

            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }

        }


        protected void APGetAuthno_Click(object sender, EventArgs e)
        {
            TSGenerateExcelReport();
        }

        private void TSGenerateExcelReportTS(string strRepotType)
        {
            string RTOName = string.Empty;

            try
            {

                // SetFolder(RTOName, "AP", "Royalty and Tax-");
                string filename = "TSRegnoNotRecieved-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".xls";

                Workbook book = new Workbook();

                #region Excel
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily Cash Collection";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
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

                Worksheet sheet = book.Worksheets.Add("TS Registration Number Not Received Records");
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
                #endregion

                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("TS Registration Number Not Received Records");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));


                row = sheet.Table.Rows.Add();
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 5;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RTO", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("AuthNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("AuthDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Engine NO.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Chassis No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderDate", "HeaderStyle6"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                string SQLString = string.Empty;
                row.Index = 7;



                string AuthNo = string.Empty;
                APHSRPService.HSRPAuthorizationServiceSoapClient yy = new APHSRPService.HSRPAuthorizationServiceSoapClient();

                string dateTo = DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy");
                string AuthData = yy.GetAuthorizationnotRegistered("01/01/2015", dateTo);
                if (AuthData.Length > 1)
                {
                    using (StringReader stringReader = new StringReader(AuthData))
                    using (XmlTextReader reader = new XmlTextReader(stringReader))
                    {
                        while (reader.Read())
                        {

                            if (reader.Name.ToString() == "AuthNo")
                            {
                                reader.Read();
                                if (AuthNo.Length == 0)
                                {
                                    AuthNo = "'" + reader.Value.ToString() + "'";
                                }
                                else
                                {
                                    AuthNo = AuthNo + ",'" + reader.Value.ToString() + "'";
                                }
                                reader.Read();
                            }
                        }
                    }
                }
                if (AuthNo != string.Empty)
                {
                    if (strRepotType == "OnlineRecords")
                    {
                        SQLString = "select ROW_NUMBER() OVER (ORDER BY h.HSRPRecord_AuthorizationNo) AS [S.No.], r.Rtolocationname as RTO, h.HSRPRecord_AuthorizationNo as AuthNo ,(convert(varchar(15),h.HSRPRecord_AuthorizationDate,103)) as AuthDate,h.Engineno,h.Chassisno, h.VehicleType,h.vehicleclass, h.vehicleregno as RegNo,(convert(varchar(15),h.orderdate,103)) as OrderDate from HSRPRecords h,RTOLocation r where h.RTOLocationID=r.rtolocationid and h.HSRP_StateID=11 and Addrecordby='OnlineDealer' and isnull(h.vehicleregno,'')='' and h.HSRPRecord_AuthorizationNo not in (" + AuthNo + ")  Order by AuthNo,OrderDate";
                        //SaveAndDownloadFile(DataTable dt)
                        DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                        SaveAndDownloadFile(dt);
                        return;
                        //Utils.ExportToSpreadsheet(dt, "online Report");
                        //return;
                    }
                    else if (strRepotType == "OfflineRecords")
                    {
                        SQLString = "select r.Rtolocationname as RTO, h.HSRPRecord_AuthorizationNo as AuthNo ,(convert(varchar(15),h.HSRPRecord_AuthorizationDate,103)) as AuthDate,h.Engineno,h.Chassisno, h.VehicleType,h.vehicleclass, h.vehicleregno as RegNo,(convert(varchar(15),h.orderdate,103)) as OrderDate from HSRPRecords h,RTOLocation r where h.RTOLocationID=r.rtolocationid and h.HSRP_StateID=11 and Addrecordby!='OnlineDealer' and isnull(h.vehicleregno,'')='' and h.HSRPRecord_AuthorizationNo not in (" + AuthNo + ")  Order by AuthNo,OrderDate";
                    }

                }
                else
                {
                    if (strRepotType == "OnlineRecords")
                    {
                        SQLString = "select r.Rtolocationname as RTO, h.HSRPRecord_AuthorizationNo as AuthNo ,(convert(varchar(15),h.HSRPRecord_AuthorizationDate,103)) as AuthDate,h.Engineno,h.Chassisno, h.VehicleType,h.vehicleclass, h.vehicleregno as RegNo,(convert(varchar(15),h.orderdate,103)) as OrderDate from HSRPRecords h,RTOLocation r where h.RTOLocationID=r.rtolocationid and h.HSRP_StateID=11 and Addrecordby='OnlineDealer' and isnull(h.vehicleregno,'')=''  Order by AuthNo,OrderDate";
                        //DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                        //Utils.ExportToSpreadsheet(dt, "online Report");
                        //return;
                    }
                    else if (strRepotType == "OfflineRecords")
                    {
                        SQLString = "select r.Rtolocationname as RTO, h.HSRPRecord_AuthorizationNo as AuthNo ,(convert(varchar(15),h.HSRPRecord_AuthorizationDate,103)) as AuthDate,h.Engineno,h.Chassisno, h.VehicleType,h.vehicleclass, h.vehicleregno as RegNo,(convert(varchar(15),h.orderdate,103)) as OrderDate from HSRPRecords h,RTOLocation r where h.RTOLocationID=r.rtolocationid and h.HSRP_StateID=11 and Addrecordby!='OnlineDealer' and isnull(h.vehicleregno,'')=''  Order by AuthNo,OrderDate";
                    }
                }

                dt12 = Utils.GetDataTable(SQLString, CnnString);
                //Utils.ExportToSpreadsheet(dt12, "online Report");
                string RTOColName = string.Empty;
                decimal totalAmount = 0;
                double tax = 0.0;
                double exicse = 0.0;
                double royalty = 0.0;
                if (dt12.Rows.Count > 0)
                {
                    int sno = 0;
                    foreach (DataRow dtrows in dt12.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        if (sno == 43)
                        {
                            int ssno = sno;
                        }
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["RTO"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["AuthNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["AuthDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Engineno"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Chassisno"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["vehicleclass"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["RegNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle"));
                        row = sheet.Table.Rows.Add();
                    }
                    row = sheet.Table.Rows.Add();
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
                else
                {
                }
            }

            catch (Exception ex)
            {

            }

        }

        protected void BtnApreport_Click(object sender, EventArgs e)
        {
            TSGenerateExcelReportTS("OnlineRecords");
        }

        protected void btnOfflineTSReport_Click(object sender, EventArgs e)
        {
            TSGenerateExcelReportTS("OfflineRecords");
        }
        int icount = 0;
        private void SaveAndDownloadFile(DataTable dt)
        {            
            Workbook book = new Workbook();
            string filename = "TSRegnoNotRecieved" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(dt, book, 1);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }

        private void Export(DataTable dt, Workbook book, int iActiveSheet)
        {
            try
            {                
                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = iActiveSheet;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "TS Registration Number Not Received Records";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook

                #region Styles
                if (icount <= 0)
                {
                    icount++;
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
                    style9.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style9.Interior.Color = "#FCF6AE";
                    style9.Interior.Pattern = StyleInteriorPattern.Solid;

                }
                #endregion

                Worksheet sheet = book.Worksheets.Add("Report");
                if (dt.Rows.Count > 0)
                {
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("TS Registration Number Not Received Records");
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    //AddNewCell(row, "State:", "HeaderStyle2", 1);
                    //AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                    //AddNewCell(row, "Report Type:", "HeaderStyle2", 1);
                    //AddNewCell(row, DropDownListReportType.SelectedItem.ToString(), "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());


                    AddNewCell(row, "Report Date :", "HeaderStyle2", 1);
                    AddNewCell(row, System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    AddNewCell(row, "", "HeaderStyle2", 2);
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;
                    AddNewCell(row, "", "HeaderStyle6", 1);
                    row = sheet.Table.Rows.Add();
                    
                    row.Index = iIndex++;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    }
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                   
                    row.Index = iIndex++;
                    AddNewCell(row, "", "HeaderStyle", 1);
                    row = sheet.Table.Rows.Add();
                    
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle", 1);
                        }
                        row = sheet.Table.Rows.Add();
                    }
                }
                else
                {
                    //Label1.Visible = false;
                    //Label1.Text = "Record not Found";
                    return;
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void AddNewCell(WorksheetRow row, string strText, string strStyle, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }

        private static void AddColumnToSheet(Worksheet sheet, int iWidth, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }
    
    }
}
