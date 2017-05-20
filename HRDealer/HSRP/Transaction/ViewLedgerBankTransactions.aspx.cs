using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Globalization;

namespace HSRP.Transaction
{


    public partial class ViewLedgerBankTransactions : System.Web.UI.Page
    {

        Utils bl = new Utils();
        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        SqlConnection con;// = new SqlConnection();
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string userdealerid = string.Empty;
        int UserType = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.GZipEncodePage();

                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {

                    HSRPStateID = Session["UserHSRPStateID"].ToString();
                    UserType = Convert.ToInt32(Session["UserType"].ToString());
                    con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    strUserID = Session["UID"].ToString();
                    userdealerid = Session["dealerid"].ToString();
                    ComputerIP = Request.UserHostAddress;


                    if (!IsPostBack)
                    {
                        try
                        {
                            // orderno();
                            InitialSetting();

                            // Utils.user_log(strUserID, "View Organization", ComputerIP, "Page load", CnnString);
                        }
                        catch (Exception err)
                        {
                            lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //private void InitialSetting1()
        //{

        //    string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    string TodayDates = "01-02-2017";
        //    string Maxdates =  "01-02-2017";
        //    HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
        //    HSRPAuthDate.MaxDate = DateTime.Parse(Maxdates);
        //    CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
        //    CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
        //    OrderDate.SelectedDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
        //    OrderDate.MaxDate = DateTime.Parse(Maxdates);
        //    CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
        //    CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
        //    OrderDate.MinDate = (DateTime.Parse(TodayDates)).AddDays(-639.00);
        //}

        private void InitialSetting()
        {
            try
            {
                string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
                string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
                string MinDate= "2017-02-01";


                StartDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);              
                StartDate.MaxDate = DateTime.Parse(MaxDate);
                StartDate.MinDate = DateTime.Parse(MinDate);


                CalendarStartDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
                CalendarStartDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

                EndDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
                EndDate.MaxDate = DateTime.Parse(MaxDate);
                EndDate.MinDate = DateTime.Parse(MinDate);

                CalendarEndDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
                CalendarEndDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }



        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                ShowGrid();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShowGrid()
        {
            try
            {
                
               
                //DataTable dt = Utils.GetDataTable("DealerLedgerBankTransaction 'Y'," + strUserID + "," + HSRPStateID + ",'" + Convert.ToDateTime(StartDate.SelectedDate).ToShortDateString() + "','" + Convert.ToDateTime(EndDate.SelectedDate).ToShortDateString() + "'", CnnString); 
                DataTable dt = Utils.GetDataTable("DealerLedgerBankTransaction 'Y'," + strUserID + "," + HSRPStateID + ",'" + Convert.ToDateTime(StartDate.SelectedDate).ToShortDateString() + "','" + Convert.ToDateTime(EndDate.SelectedDate).ToShortDateString() + "'", CnnString); 
                //objUtils.GetRecords("DealerLedgerBankTransaction", sqlParameter, con);
                if (dt.Rows.Count > 0)
                {
                    llbMSGError.Text = "";
                    llbMSGError.Visible = false;
                    btnExportExcel.Visible = true;
                    grdid.DataSource = dt;
                    grdid.DataBind();
                    grdid.Visible = true;

                }
                else
                {
                    llbMSGError.Text = "";
                    llbMSGError.Visible = true;
                    llbMSGError.Text = " Record Not Found.";
                    btnExportExcel.Visible = false;
                    grdid.Visible = false;

                }



            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Export();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveAndDownloadFile()
        {
            try
            {
                SQLString = "select dealername , city from dealerMaster where dealerid = '" + userdealerid + "'";
                DataTable dtt = Utils.GetDataTable(SQLString, CnnString);
                string Dealername = dtt.Rows[0]["dealername"].ToString();
                string City = dtt.Rows[0]["city"].ToString();

                string filename = "LedgerBankTransactions" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Ledger Bank Transaction Report";
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



                Worksheet sheet = book.Worksheets.Add("HSRP Bank Transaction Report");
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
                // row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Link Utsav Registration Plates Pvt Ltd, Haryana ");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                row = sheet.Table.Rows.Add();
                row = sheet.Table.Rows.Add();
                row.Index = 4;

                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Period from :", "HeaderStyle2"));

                string startday = Convert.ToString(StartDate.SelectedDate.Day);
                string startmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(StartDate.SelectedDate.Month));
                string startyear = Convert.ToString(StartDate.SelectedDate.Year);
                string startdate = startday + "-" + startmonth + "-" + startyear;
                row.Cells.Add(new WorksheetCell(startdate, "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("To:", "HeaderStyle2"));


                string endday = Convert.ToString(EndDate.SelectedDate.Day);
                string endmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(EndDate.SelectedDate.Month));
                string endyear = Convert.ToString(EndDate.SelectedDate.Year);
                string enddate = endday + "-" + endmonth + "-" + endyear;

                row.Cells.Add(new WorksheetCell(enddate.ToString(), "HeaderStyle2"));

                WorksheetCell cell4 = row.Cells.Add("Report Generation Date :");
                cell4.MergeAcross = 1; // Merge two cells together
                cell4.StyleID = "HeaderStyle2";

                string currentDate = System.DateTime.Now.Day.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Year.ToString();


                row.Cells.Add(new WorksheetCell(currentDate, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                row.Index = 5;

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Dealer Name :", "HeaderStyle2"));
                WorksheetCell cell1 = row.Cells.Add(Dealername);
                cell1.MergeAcross = 1; // Merge two cells together
                cell1.StyleID = "HeaderStyle2";

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("City :", "HeaderStyle2"));
                WorksheetCell cell2 = row.Cells.Add(City);
                cell2.MergeAcross = 1; // Merge two cells together
                cell2.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();

                row.Index = 7;

                row.Cells.Add(new WorksheetCell("S No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Particular", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Dr", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cr", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Balance", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 8;
                //  int totalAmount = 0;
                SqlParameter[] sqlParameter = {
                new SqlParameter("@ApprovedStatus","Y"),
                new SqlParameter("@depositby",strUserID),
                new SqlParameter("@stateid",HSRPStateID),
                new SqlParameter("@TransactionStartDate",StartDate.SelectedDate),
                new SqlParameter("@TransactionEndDate",EndDate.SelectedDate)
                };
                Utils objUtils = new Utils();
                DataTable dt = objUtils.GetRecords("DealerLedgerBankTransaction", sqlParameter, con);

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        row.Cells.Add(new WorksheetCell(dtrows["ID"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["TransactionDate"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Particular"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Dr"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Cr"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["Balance"].ToString(), DataType.String, "HeaderStyle"));
                        //  string strbal = dtrows["Balance"].ToString();
                        // string[] stringArray = strbal.Split(' ');

                        //  row.Cells.Add(new WorksheetCell(stringArray[0].ToString(), DataType.String, "HeaderStyle"));



                        // totalAmount += totalAmount + Convert.ToInt32(stringArray[0]);
                        row = sheet.Table.Rows.Add();
                    }
                    
                    // row = sheet.Table.Rows.Add();
                    // WorksheetCell cell3= row.Cells.Add("Vechicle No Could also be provided in stead or Order Number Order No is preferred as same refrence number Aggeregators are ussing and Ledger Could be inifcrm for both.");
                    // cell2.MergeAcross = 8; // Merge two cells together
                    // cell2.StyleID = "HeaderStyle6";
                    
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
                    llbMSGError.Text = "";
                    llbMSGError.Visible = true;
                    llbMSGError.Text = " Record Not Found.";
                    btnExportExcel.Visible = false;
                    grdid.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string StateName()
        {
            try
            {
                SQLString = "select HSRPStateName from HSRPState where  HSRP_StateID='" + HSRPStateID + "'";
                return Utils.GetDataTable(SQLString.ToString(), CnnString.ToString()).Rows[0]["HSRPStateName"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        int icount = 0;
        private void Export()
        {
            try
            {
                SqlConnection con = new SqlConnection(CnnString);
                #region Fetch Data

                DataTable dt = Utils.GetDataTable("DealerLedgerBankTransaction 'Y'," + strUserID + "," + HSRPStateID + ",'" + Convert.ToDateTime(StartDate.SelectedDate).ToShortDateString() + "','" + Convert.ToDateTime(EndDate.SelectedDate).ToShortDateString() + "'", CnnString); 
               

                SQLString = "select dealername,city from dealerMaster where dealerid = '" + userdealerid + "'";
                 DataTable dtt = Utils.GetDataTable(SQLString, CnnString);
                 string Dealername = string.Empty;
                 string City = string.Empty;
                 if (dtt.Rows.Count > 0)
                 {
                      Dealername = dtt.Rows[0]["dealername"].ToString();
                      City = dtt.Rows[0]["city"].ToString();
                 }
                

                #endregion
                
                if (dt.Rows.Count > 0)
                {
                    Workbook book = new Workbook();
                    string filename = "LedgerBankTransactions" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "HSRP Ledger Bank Transaction Report";
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

                    Worksheet sheet = book.Worksheets.Add("HSRP Bank Transaction Report");
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 3;
                   
                    WorksheetRow row = sheet.Table.Rows.Add();
                    
                    
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("Link Utsav Registration Plates Pvt Ltd ,Haryana");
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Index = iIndex++;
                    
                    AddNewCell(row, "Period from :", "HeaderStyle2", 1);
                    AddNewCell(row, StartDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    AddNewCell(row, "To:", "HeaderStyle2", 1);
                    AddNewCell(row, EndDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    row.Cells.Add(new WorksheetCell("Report Generation Date :", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    AddNewCell(row, "Dealer Name :", "HeaderStyle2", 1);
                    AddNewCell(row, Dealername.ToString(), "HeaderStyle2", 1);

                    AddNewCell(row, "City :", "HeaderStyle2", 1);
                    AddNewCell(row, City.ToString(), "HeaderStyle2", 1);

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    AddNewCell(row, "", "HeaderStyle2", 1);
                    AddNewCell(row, "", "HeaderStyle2", 1);

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    }
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle", 1);
                        }
                        row = sheet.Table.Rows.Add();

                    }
                    
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);
                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
                else
                {
                    lblErrMsg.Visible = false;
                    lblErrMsg.Text = "Record not Found";
                    return;
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static void AddColumnToSheet(Worksheet sheet, int iWidth, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                sheet.Table.Columns.Add(new WorksheetColumn(iWidth));
        }

        private static void AddNewCell(WorksheetRow row, string strText, string strStyle, int iCnt)
        {
            for (int i = 0; i < iCnt; i++)
                row.Cells.Add(new WorksheetCell(strText, strStyle));
        }
    }
}