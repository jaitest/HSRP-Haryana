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
    public partial class DailyStockStatusOfRawMaterialReport : System.Web.UI.Page
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
                //String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                //DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);

                DateTime dtimeOrderDate = Convert.ToDateTime(OrderDate.SelectedDate.ToString());
                string OrderDate1 = dtimeOrderDate.ToString("dd-MMM-yyyy");
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                //Report_Daily_Stock_StatusOf_RowMaterial '25-Aug-2012',2,138
                UserType = Convert.ToInt32(Session["UserType"]);
                if (UserType == 0)
                {
                    SQLString = "Report_Daily_Stock_StatusOf_RawMaterial '" + OrderDate1 + "','" + intHSRPStateID + "','" + intRTOLocationID + "'";
                }
                else
                {
                    HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                    RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());

                    SQLString = "Report_Daily_Stock_StatusOf_RawMaterial '" + OrderDate1 + "','" + HSRPStateID + "','" + RTOLocationID + "'";
                }

                DataSet dt = Utils.getDataSet(SQLString, CnnString);

                if (dt.Tables[0].Rows.Count > 0)
                {
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                    
                    string filename = "ReportDailyStockStatusOfRawMaterial-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "HSRP Daily Stock Status Of Raw Material Report";
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
                    
                    WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                    style2.Font.FontName = "Tahoma";
                    style2.Font.Size = 10;
                    style2.Font.Bold = false;

                    WorksheetStyle style3 = book.Styles.Add("HeaderStyleData1");
                    style3.Font.FontName = "Tahoma";
                    style3.Font.Size = 10;
                    style3.Font.Bold = true;
                    style3.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style3.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
                    style3.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
                    style3.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
                    style3.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
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

                    WorksheetStyle style5 = book.Styles.Add("HeaderStyleData3");
                    style5.Font.FontName = "Tahoma";
                    style5.Font.Size = 10;
                    style5.Font.Bold = false;
                    style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                    style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                    //style5.Interior.Color = "#DBE5F1";
                    //style5.Interior.Pattern = StyleInteriorPattern.Solid;

                    //-----------STYLE---------------------


                    Worksheet sheet = book.Worksheets.Add("HSRP Daily Stock Status Of Raw Material Report");
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


                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row = sheet.Table.Rows.Add();

                    //row.Index = 2;
                    row.Index = 1;
                    WorksheetCell cell = row.Cells.Add();
                    cell = row.Cells.Add("Report: ");
                    //cell.MergeAcross = 6;            // Merge two cells together
                    cell.StyleID = "HeaderStyle0";
                    cell = row.Cells.Add("HSRP Daily Stock Status Of Raw Material Report");
                    cell.StyleID = "HeaderStyle0";
                    row = sheet.Table.Rows.Add();
                    
                    row.Index = 2;
                    row = sheet.Table.Rows.Add();

                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 4;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle"));
                    if (dropDownListClient.SelectedItem.ToString().Trim() == "--Select Location--")
                    {
                        row.Cells.Add(new WorksheetCell("All", "HeaderStyle"));
                    }
                    else
                    {
                        row.Cells.Add(new WorksheetCell(TitleCaseString(dropDownListClient.SelectedItem.ToString()), "HeaderStyle"));
                    }
                    row = sheet.Table.Rows.Add();

                    row.Index = 5;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("Generated Date:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd-MMM-yyyy"), "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(OrderDate1, "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    //row.Index = 7;
                    //    WorksheetCell cell = row.Cells.Add("HSRP Daily Stock Report For " + OrderDate1);
                    //    cell.MergeAcross = 6;            // Merge 6 cells together
                    //    cell.StyleID = "HeaderStyle";
                    //    row = sheet.Table.Rows.Add();
                    
                    row.Index = 7;
                    row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Name Of Material", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("UOM", "HeaderStyleData1"));

                    cell = row.Cells.Add("Opening Stock");
                    cell.MergeAcross = 1;
                    cell.StyleID = "HeaderStyleData1";

                    cell = row.Cells.Add("Receipt");
                    cell.MergeAcross = 1;
                    cell.StyleID = "HeaderStyleData1";

                    cell = row.Cells.Add("Total");
                    cell.MergeAcross = 1;
                    cell.StyleID = "HeaderStyleData1";

                    cell = row.Cells.Add("Issued");
                    cell.MergeAcross = 1;
                    cell.StyleID = "HeaderStyleData1";

                    cell = row.Cells.Add("Balance");
                    cell.MergeAcross = 1;
                    cell.StyleID = "HeaderStyleData1";

                    row = sheet.Table.Rows.Add();

                    row.Index = 8;
                    cell = row.Cells.Add();
                    cell.MergeAcross = 2;
                    cell.StyleID = "HeaderStyleData1";
                    
                    row.Cells.Add(new WorksheetCell("QTY", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Value", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("QTY", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Value", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("QTY", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Value", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("QTY", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Value", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("QTY", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Value", "HeaderStyleData1"));
                    row = sheet.Table.Rows.Add();

                    //Skip one cell:
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    // Skip one row:
                    //row.Index = 11;
                    //row = sheet.Table.Rows.Add();

                    row.Index = 9;
                    int sno = 0;
                    foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["NameOfMaterial"].ToString(), "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["UOM"].ToString(), "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["OpeningStockQTY"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["OpeningStockValue"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["ReceiptQTY"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["ReceiptValue"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["TotalStockQTY"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["TotalStockValue"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["IssuedQTY"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["IssuedValue"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["BalanceQTY"].ToString(), DataType.Number, "HeaderStyleData3"));
                        row.Cells.Add(new WorksheetCell(dtrows["BalanceValue"].ToString(), DataType.Number, "HeaderStyleData3"));
                        
                    }

                    row = sheet.Table.Rows.Add();
                    LabelError.Text = "";
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
                    string closescript1 = "<script>alert('No records found for selected date and RTO.')</script>";
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