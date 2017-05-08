using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Excel1 = Microsoft.Office.Interop.Excel;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
namespace HSRP.Report
{
    public partial class CollectionReportDealerAndNonDealer : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        int intHSRPStateID1;
        int intRTOLocationID1;
        string SQLString1 = string.Empty;
        string OrderType;
        string recordtype = string.Empty;
        //DateTime OrderDate1;
        string strDirectoryForFile = string.Empty;
        string path = string.Empty, zone = string.Empty;
        string statename = string.Empty;
        List<string> pathfile = null;
        List<string> st = null;
        int iSheetCount = 0;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();

                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            //FilldropDownListClient();
                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            OrderDateto.SelectedDate = System.DateTime.Now;


                        }
                        else
                        {

                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            //dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            //FilldropDownListClient();

                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            OrderDateto.SelectedDate = System.DateTime.Now;

                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        #region DropDown



        private void FilldropDownListOrganization()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString1, "--Select State--");
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }

        #endregion

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            OrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
        protected void LinkbuttonSearch_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            SaveAndDownloadFile();
            //AssignDirectory();
           // GenerateMPDataFile();
        }

        private void GenerateMPDataFile()
        {
            #region Create a Excel Wrkbook
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();

            app.Visible = false;
            app.UserControl = false;
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            
            // creating new Excelsheet in workbook 


            #endregion
          //  DeleteFile(strDirectoryForFile, "MPEP");


            GenerateFile(workbook, DropDownListStateName.SelectedValue, DropDownListStateName.SelectedItem.Text, 1, "Dealer");

            GenerateFile(workbook, DropDownListStateName.SelectedValue, DropDownListStateName.SelectedItem.Text, 1, "NonDealer");

            string filename = "Collection_Summary" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
         //   workbook.SaveAs(filename, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            workbook.Save();
            //context.Response.ContentType = "text/csv";
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();
            app.Quit();
        }

        private void GenerateFile(Microsoft.Office.Interop.Excel._Workbook workbook, string strStateId, string strStateName, int iSheet, string strReportType)
        {
            SqlConnection con = new SqlConnection(CnnString1);
            #region Fetch Data
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("[Report_Daily_General_DealerNNonDealerBetweenTwoDate]", con);
            if (strReportType.Equals("Dealer"))
            {
                
                strParameter = "D";
            }
            else if (strReportType.Equals("NonDealer"))
            {
                
                strParameter = "N";
            }

            cmd.CommandType = CommandType.StoredProcedure;
            if (!string.IsNullOrEmpty(strParameter))
                cmd.Parameters.Add(new SqlParameter("@flag", strParameter));
            cmd.Parameters.Add(new SqlParameter("@fromdate", OrderDatefrom.SelectedDate));
            cmd.Parameters.Add(new SqlParameter("@todate", OrderDateto.SelectedDate));


            cmd.Parameters.Add(new SqlParameter("@StateId", strStateId));

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);
            #endregion

            //if (strStateName.Equals("MP"))
            //{
            //    ds.Clear();
            //    ds.Tables.Add(dtMPData);
            //}
            st = new List<string>();
            int l = 0, iRowCnt = 1, m = 0;

            int kk = 0;
            int j = 0, r = 0;

            pathfile = new List<string>();

            kk = 0;
            // storing header part in Excel 
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // store its reference to worksheet 

            var xlSheets = workbook.Sheets as Excel1.Sheets;
            var xlNewSheet = (Excel1.Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
            //  xlNewSheet.Name = "newsheet";
            iSheetCount = iSheetCount + 1;
            // worksheet = workbook.Sheets["Sheet"+iSheet];

            worksheet = xlNewSheet;
            worksheet = workbook.ActiveSheet;



            // changing the name of active sheet 

            worksheet.Name = strStateName + strReportType + " Report";
            for (int k = 0; k < ds.Tables.Count; k++)
            {

                #region Column Creation in Excel
                if (k == 0)
                {
                    //But then this line changes every cell style back to left alignment

                    worksheet.Cells[iRowCnt, 1].Interior.Color = System.Drawing.Color.Yellow;
                    worksheet.Cells[iRowCnt, 2].Interior.Color = System.Drawing.Color.Yellow;
                    worksheet.Cells[iRowCnt, 3].Interior.Color = System.Drawing.Color.Yellow;

                    worksheet.Cells[iRowCnt, 1].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                    worksheet.Cells[iRowCnt, 2].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                    worksheet.Cells[iRowCnt, 3].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                    worksheet.Cells[iRowCnt, 1] = "Daily Collection, Embossing and Affixation Report: ";

                    worksheet.Cells[iRowCnt, 2] = "Report Date : ";

                    //  worksheet.Cells[iRowCnt, 3] = DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss tt");
                    worksheet.Cells[iRowCnt, 3] = DateTime.Now.AddDays(-1).ToOADate().ToString();
                    worksheet.get_Range("C1:C1").NumberFormat = "dd/MMM/yyyy";
                    iRowCnt = iRowCnt + 1;

                    worksheet.Cells[iRowCnt, 1] = strStateName;

                    iRowCnt = iRowCnt + 1;

                    worksheet.Cells[iRowCnt, 1].Interior.Color = System.Drawing.Color.Yellow;
                    worksheet.Cells[iRowCnt, 2].Interior.Color = System.Drawing.Color.Yellow;
                    worksheet.Cells[iRowCnt, 3].Interior.Color = System.Drawing.Color.Yellow;
                    worksheet.Cells[iRowCnt, 4].Interior.Color = System.Drawing.Color.Yellow;
                    worksheet.Cells[iRowCnt, 5].Interior.Color = System.Drawing.Color.Yellow;




                    worksheet.Cells[iRowCnt, 1].Font.Bold = true;
                    worksheet.Cells[iRowCnt, 2].Font.Bold = true;
                    worksheet.Cells[iRowCnt, 3].Font.Bold = true;
                    worksheet.Cells[iRowCnt, 4].Font.Bold = true;
                    worksheet.Cells[iRowCnt, 5].Font.Bold = true;



                    worksheet.Cells[iRowCnt, 1].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                    worksheet.Cells[iRowCnt, 2].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                    worksheet.Cells[iRowCnt, 3].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                    worksheet.Cells[iRowCnt, 4].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                    worksheet.Cells[iRowCnt, 5].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);


                    worksheet.Cells[iRowCnt, 1] = "Location Name";
                    worksheet.Cells[iRowCnt, 2] = "Collection";
                    worksheet.Cells[iRowCnt, 3] = "New Order";
                    worksheet.Cells[iRowCnt, 4] = "Production";
                    worksheet.Cells[iRowCnt, 5] = "Affixed";

                    iRowCnt = iRowCnt - 1;
                    l = l + 2;

                    // worksheet.Cells.Cells[1, 1].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                }

                #endregion

                string ss = ds.Tables[k].Rows.Count.ToString();

                for (m = 0; m < ds.Tables[k].Rows.Count; m++)
                {
                    #region Assign Values in Excel

                    if (!ds.Tables[k].Rows[m]["LocationName"].ToString().Equals("ZZZZZ"))
                    {
                        worksheet.Cells[iRowCnt + 2, r + 1].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                        worksheet.Cells[iRowCnt + 2, r + 2].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                        worksheet.Cells[iRowCnt + 2, r + 3].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                        worksheet.Cells[iRowCnt + 2, r + 4].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                        worksheet.Cells[iRowCnt + 2, r + 5].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);

                        if (kk == 0)
                        {
                            kk = 2;
                        }
                        worksheet.Cells[iRowCnt + 2, r + 1] = ds.Tables[k].Rows[m]["LocationName"].ToString();
                        worksheet.Cells[iRowCnt + 2, r + 2] = ds.Tables[k].Rows[m]["Collection"].ToString();
                        worksheet.Cells[iRowCnt + 2, r + 3] = int.Parse(ds.Tables[k].Rows[m]["NewOrder"].ToString());
                        worksheet.Cells[iRowCnt + 2, r + 4] = int.Parse(ds.Tables[k].Rows[m]["Production"].ToString());
                        worksheet.Cells[iRowCnt + 2, r + 5] = int.Parse(ds.Tables[k].Rows[m]["Affixed"].ToString());


                    #endregion

                        iRowCnt++;
                        l++;

                    }
                }

                for (int iCnt = 1; iCnt <= 5; iCnt++)
                {
                    worksheet.Cells[iRowCnt + 2, r + iCnt].Font.Bold = true;
                    worksheet.Cells[iRowCnt + 2, r + iCnt].BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);
                }

                string expression = "LocationName = 'ZZZZZ'";

                DataRow[] foundRows;

                // Use the Select method to find all rows matching the filter.
                foundRows = ds.Tables[0].Select(expression);

                worksheet.Cells[iRowCnt + 2, r + 1] = "Total";
                worksheet.Cells[iRowCnt + 2, r + 2] = foundRows[0]["Collection"];
                worksheet.Cells[iRowCnt + 2, r + 3] = foundRows[0]["NewOrder"];
                worksheet.Cells[iRowCnt + 2, r + 4] = foundRows[0]["Production"];
                worksheet.Cells[iRowCnt + 2, r + 5] = foundRows[0]["Affixed"];

                l = l + 2;
                r = r + 6;

            }
            #region set Format of Excel1
            iRowCnt = iRowCnt + 6;
            r = 0;
            iRowCnt = 1;
            worksheet.Range["A1", "E" + iRowCnt].Columns.EntireColumn.AutoFit();
            worksheet.Range["A1", "E" + iRowCnt].Cells.BorderAround(Excel1.XlLineStyle.xlContinuous, Excel1.XlBorderWeight.xlMedium, Excel1.XlColorIndex.xlColorIndexAutomatic, Excel1.XlColorIndex.xlColorIndexAutomatic);

            #endregion

            #region save the file

            st.Add(statename);
            pathfile.Add(path);

           



            #endregion
        }

        private void DeleteFile(string strDirectory, string strFileStartingWith)
        {

            var files = Directory.GetFiles(strDirectory + ":\\DailyMPReport", "*.*", SearchOption.AllDirectories)
    .Where(s => s.EndsWith(".xls") || s.EndsWith(".xlsx"));
            path = strDirectory + ":\\DailyMPReport\\" + strFileStartingWith + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + statename + ".xls";

            foreach (string file in files)
            {
                if (file.StartsWith(strDirectory + ":\\DailyMPReport\\" + strFileStartingWith))
                    File.Delete(file);
            }
        }

        private static bool CheckFile(string strDirectory, string strFileStartingWith)
        {
            // string strFileName = strDirectory+":\\DailyAPReport\\MPEP-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + ".xls";
            var files = Directory.GetFiles(strDirectory + ":\\DailyMPReport", "*.*", SearchOption.AllDirectories)
                           .Where(s => s.EndsWith(".xls") || s.EndsWith(".xlsx"));

            bool bFileExists = false;
            foreach (string file in files)
            {
                if (file.StartsWith(strDirectory + ":\\DailyMPReport\\" + strFileStartingWith + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString()))
                {
                    bFileExists = true;
                }
            }
            return bFileExists;
        }

        private void AssignDirectory()
        {
            strDirectoryForFile = "D";
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            string filename = "Collection_Summary" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            
            Export(DropDownListOrderType.SelectedValue, book, 1);
          //  Export("D", book, 2);

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();    
        
        }

        int icount = 0;

        private void Export(string strReportType, Workbook book,int iActiveSheet)
        {
                try
                {
                    SqlConnection con = new SqlConnection(CnnString1);

                  
                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = iActiveSheet;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Collection Summary";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
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
                    string strText = strReportType.Equals("N") ? "Non Dealer" : "Dealer";

                    switch (strReportType)
                    { 
                        case "D":
                            strText = "Dealer";
                            break;
                        case "N":
                            strText = "Non Dealer";
                            break;
                        case "B":
                            strText = "Dealer and Non Dealer";
                            break;                    
                    
                    }

                    Worksheet sheet = book.Worksheets.Add("Location Wise Summary Report");

                    
                    sheet.Table.Columns.Add(new WorksheetColumn(60));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));



                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("Location Wise Summary Report" + "(" + strText + ")");
                    cell.MergeAcross = 2; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Period:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(OrderDatefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2"));                    
                    row.Cells.Add(new WorksheetCell("To:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(OrderDateto.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2")); 
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                  
                    row.Cells.Add(new WorksheetCell("LOCATION", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Collection", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Order Booking", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Pending for Affixation", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Affixed", "HeaderStyle6"));
                   
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;


                    #region Fetch Data
                    DataSet ds = new DataSet();

                    SqlCommand cmd = new SqlCommand();
                    string strParameter = string.Empty;
                    cmd = new SqlCommand("[Report_Daily_General_DealerNNonDealerBetweenTwoDate]", con);
                    //if (strReportType.Equals("Dealer"))
                    //{

                    //    strParameter = "D";
                    //}
                    //else if (strReportType.Equals("NonDealer"))
                    //{

                    //    strParameter = "N";
                    //}

                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(strReportType))
                        cmd.Parameters.Add(new SqlParameter("@flag", strReportType));
                    cmd.Parameters.Add(new SqlParameter("@fromdate", OrderDatefrom.SelectedDate));
                    cmd.Parameters.Add(new SqlParameter("@todate", OrderDateto.SelectedDate));


                    cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    #endregion
                    string RTOColName = string.Empty;
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            if (!dtrows["locationname"].ToString().Equals("ZZZZZ"))
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle"));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell("Total", DataType.String, "HeaderStyle"));
                            }
                                row.Cells.Add(new WorksheetCell(dtrows["Collection"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["NewOrder"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Production"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Affixed"].ToString(), DataType.String, "HeaderStyle"));
                               // row.Index = iIndex++;
                            row = sheet.Table.Rows.Add();
                            
                        }                  
                        row = sheet.Table.Rows.Add();
                       
                       
                    }



                }

                catch (Exception ex)
                {
                    throw ex;
                }

        }

    }

}