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


namespace HSRP.Report
{
    public partial class DailyRevenueCollectionVehCentreWise : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
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
                            FilldropDownListOrganization();
                        }
                        else
                        {


                            FilldropDownListOrganization();
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelDate.Visible = false;
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
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-1);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-1);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(-1);
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

        #endregion

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Visible = false;
                //String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                //DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);

                DateTime dtimeOrderDate = Convert.ToDateTime(OrderDate.SelectedDate.ToString());
                string OrderDate1 = dtimeOrderDate.ToString("dd-MMM-yyyy");
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                


                UserType = Convert.ToInt32(Session["UserType"]);
                if (UserType == 0)
                {
                    //Report_Daily_RevenueColltnVehCentreWise '19-Sep-2012',4
                    SQLString = "Report_Daily_RevenueColltnVehCentreWise '" + OrderDate1 + "'," + intHSRPStateID + "";
                }
                else
                {
                   // HSRPStateID = Session["UserHSRPStateID"].ToString();
                    RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());

                    SQLString = "Report_Daily_RevenueColltnVehCentreWise '" + OrderDate1 + "'," + HSRPStateID + "";
                }

                DataSet dt = Utils.getDataSet(SQLString, CnnString);

                if (dt.Tables[0].Rows.Count > 0)
                {
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    

                    string filename = "RevenueCollectionReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "HSRP Revenue Collection Report";
                    book.Properties.Created = DateTime.Now;

                    //-----------STYLE---------------------
                    WorksheetStyle style0 = book.Styles.Add("HeaderStyle0");
                    style0.Font.FontName = "Tahoma";
                    style0.Font.Size = 12;
                    //style.Workbook=white-space:nowrap;
                    style0.Font.Bold = true;
                    style0.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                    WorksheetStyle style = book.Styles.Add("HeaderStyle");
                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 10;
                    //style.Workbook=white-space:nowrap;
                    style.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    //style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    //style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    //style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    //style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                    //style.Font.Color = "White";
                    //style.Interior.Color = "#7F7F7F";
                    //style.Interior.Pattern = StyleInteriorPattern.Solid;

                    WorksheetStyle styleMerge = book.Styles.Add("HeaderStyleMerge");
                    styleMerge.Font.FontName = "Tahoma";
                    styleMerge.Font.Size = 11;
                    //style.Workbook=white-space:nowrap;
                    styleMerge.Font.Bold = true;
                    styleMerge.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    styleMerge.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                    styleMerge.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                    styleMerge.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                    styleMerge.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);

                    WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                    style2.Font.FontName = "Tahoma";
                    style2.Font.Size = 10;
                    style2.Font.Bold = false;

                    WorksheetStyle style3 = book.Styles.Add("HeaderStyleData1");
                    style3.Font.FontName = "Tahoma";
                    style3.Font.Size = 10;
                    style3.Font.Bold = true;
                    style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style3.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style3.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style3.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style3.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                    //style3.Interior.Color = "#C5BE97";
                    //style3.Interior.Pattern = StyleInteriorPattern.Solid;

                    WorksheetStyle style4 = book.Styles.Add("HeaderStyleData2");
                    style4.Font.FontName = "Tahoma";
                    style4.Font.Size = 10;
                    style4.Font.Bold = false;
                    style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                    //style4.Interior.Color = "#DBE5F1";
                    //style4.Interior.Pattern = StyleInteriorPattern.Solid;

                    //-----------STYLE---------------------


                    Worksheet sheet = book.Worksheets.Add("HSRP Revenue Collection Report");
                    //sheet.Range["A1"].Style.WrapText = true;
                    sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(115));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(115));
                    //sheet.Table.Columns.Add(new WorksheetColumn(130));
                    WorksheetRow row = sheet.Table.Rows.Add();

                    row.Index = 1;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report: ", "HeaderStyle0"));
                    row.Cells.Add(new WorksheetCell("Daily Vehicle & Location - Wise Summary", "HeaderStyle0"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle"));
                    row = sheet.Table.Rows.Add();


                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Generated Date:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd-MMM-yyyy"), "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 4;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(OrderDate1, "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 5;
                    WorksheetCell cell = row.Cells.Add("");
                    cell.MergeAcross = 1;            // Merge two cells together
                    cell.StyleID = "HeaderStyleMerge";
                    WorksheetCell cell2 = row.Cells.Add("Daily");
                    cell2.MergeAcross = 3;
                    cell2.StyleID = "HeaderStyleMerge";
                    WorksheetCell cell3 = row.Cells.Add("Month To Date");
                    cell3.MergeAcross = 3;
                    cell3.StyleID = "HeaderStyleMerge";
                    WorksheetCell cell4 = row.Cells.Add("Year To Date");
                    cell4.MergeAcross = 3;
                    cell4.StyleID = "HeaderStyleMerge";


                    row = sheet.Table.Rows.Add();

                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Name of Centre", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("2W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("3W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("4W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("TRACTOR", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("2W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("3W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("4W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("TRACTOR", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("2W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("3W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("4W", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("TRACTOR", "HeaderStyleData1"));
                   
                    row = sheet.Table.Rows.Add();

                    //Skip one cell:
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    // Skip one row:
                    //row.Index = 11;
                    //row = sheet.Table.Rows.Add();

                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    row.Index = 7;
                    decimal D2W = 0;
                    decimal D3W = 0;
                    decimal D4W = 0;
                    decimal DTRACTOR = 0;
                    decimal M2W = 0;
                    decimal M3W = 0;
                    decimal M4W = 0;
                    decimal MTRACTOR = 0;
                    decimal Y2W = 0;
                    decimal Y3W = 0;
                    decimal Y4W = 0;
                    int sno =0;
                    decimal YTRACTOR = 0;

                    foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyleData2"));
                       // row.Cells.Add(new WorksheetCell(dtrows["ID"].ToString(),DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["Centre"].ToString(), "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["D2W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["D3W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["D4W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["DTRACTOR"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["M2W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["M3W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["M4W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["MTRACTOR"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["Y2W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["Y3W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["Y4W"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["YTRACTOR"].ToString(), DataType.Number, "HeaderStyleData2"));

                        D2W = D2W + Convert.ToDecimal(dtrows["D2W"].ToString());
                        D3W = D3W + Convert.ToDecimal(dtrows["D3W"].ToString());
                        D4W = D4W + Convert.ToDecimal(dtrows["D4W"].ToString());
                        DTRACTOR = DTRACTOR + Convert.ToDecimal(dtrows["DTRACTOR"].ToString());

                        M2W = M2W + Convert.ToDecimal(dtrows["M2W"].ToString());
                        M3W = M3W + Convert.ToDecimal(dtrows["M3W"].ToString());
                        M4W = M4W + Convert.ToDecimal(dtrows["M4W"].ToString());
                        MTRACTOR = MTRACTOR + Convert.ToDecimal(dtrows["MTRACTOR"].ToString());

                        Y2W = Y2W + Convert.ToDecimal(dtrows["Y2W"].ToString());
                        Y3W = Y3W + Convert.ToDecimal(dtrows["Y3W"].ToString());
                        Y4W = Y4W + Convert.ToDecimal(dtrows["Y4W"].ToString());
                        YTRACTOR = YTRACTOR + Convert.ToDecimal(dtrows["YTRACTOR"].ToString());
                    }

                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Total Amount (INR.)", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(D2W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(D3W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(D4W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(DTRACTOR.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(M2W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(M3W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(M4W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(MTRACTOR.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(Y2W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(Y3W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(Y4W.ToString(), DataType.Number, "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell(YTRACTOR.ToString(), DataType.Number, "HeaderStyleData1"));

                    row = sheet.Table.Rows.Add();
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
                    //LabelError.Text = string.Empty;
                    //LabelError.Text = "No records found for selected date.";
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
        public static String TitleCaseString(String s)
        {
            if (s == null) return s;

            String[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join(" ", words);
        }
    }
}