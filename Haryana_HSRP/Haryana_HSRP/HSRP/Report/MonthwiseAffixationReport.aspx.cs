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
    public partial class MonthwiseAffixationReport : System.Web.UI.Page
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
        string ReportMonth = string.Empty;
        string ReportYear = string.Empty;
        int NoOFDays;
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

                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            FilldropDownListClient();
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
            string currentMonth = Utils.getDataSingleValue("select convert(char(3), getdate(), 0) as CurrMonth", CnnString, "CurrMonth");
            ddlMonth.Items.FindByValue(currentMonth).Selected = true;

            string currentYear = Utils.getDataSingleValue("Select datepart(year,getdate()) as CurrYear", CnnString, "CurrYear");
            ddlYear.Items.FindByValue(currentYear).Selected = true;
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
                dropDownListClient.Visible = false;
                labelClient.Visible = false;

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
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
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Visible = false;
                //DateTime dtimeOrderDate = Convert.ToDateTime(OrderDate.SelectedDate.ToString());
                //string OrderDate1 = dtimeOrderDate.ToString("dd-MMM-yyyy");
                ReportMonth = ddlMonth.SelectedItem.Text.Trim();
                ReportYear = ddlYear.SelectedItem.Text.Trim();
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);


                UserType = Convert.ToInt32(Session["UserType"]);
                if (UserType == 0)
                {
                    //Report_Monthly_Affixation 'AUG','2012',2,139,0
                    SQLString = "Report_Monthly_Affixation '" + ReportMonth + "','" + ReportYear + "','" + intHSRPStateID + "','" + intRTOLocationID + "',0";
                }
                else
                {
                    HSRPStateID =  Session["UserHSRPStateID"].ToString();
                    RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());

                    SQLString = "Report_Monthly_Affixation '" + ReportMonth + "','" + ReportYear + "','" + HSRPStateID + "','" + RTOLocationID + "',1";
                }

                DataSet dt = Utils.getDataSet(SQLString, CnnString);

                if (dt.Tables[0].Rows.Count > 0)
                {
                    NoOFDays = Convert.ToInt32(Utils.getDataSingleValue("Select DATENAME(DAY,DATEADD(DAY,-1,DATEADD(Month,1,'" + ReportMonth + " " + ReportYear + "'))) as NoOfDays", CnnString, "NoOfDays"));
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                    string filename = "MonthlyAffixationReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();
                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "HSRP Monthly Affixation Report";
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


                    Worksheet sheet = book.Worksheets.Add("HSRP Monthly Affixation Report");
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

                    row.Index = 1;
                    WorksheetCell cell = row.Cells.Add();
                    cell = row.Cells.Add("Report: ");
                    //cell.MergeAcross = 6;            // Merge two cells together
                    cell.StyleID = "HeaderStyle0";
                    cell = row.Cells.Add("HSRP Monthly Affixation Report");
                    cell.StyleID = "HeaderStyle0";
                    row = sheet.Table.Rows.Add();

                    row.Index = 2;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 3;
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

                    row.Index = 4;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("Generated Date:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd-MMM-yyyy"), "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 5;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell("Report For Month/Year:", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(ReportMonth+" - "+ReportYear, "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("RTO", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Perticulars", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("1ST", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("2ND", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("3RD", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("4TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("5TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("6TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("7TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("8TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("9TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("10TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("11TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("12TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("13TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("14TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("15TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("16TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("17TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("18TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("19TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("20TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("21ST", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("22ND", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("23RD", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("24TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("25TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("26TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("27TH", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("28TH", "HeaderStyleData1"));
                    if (NoOFDays == 29)
                    {
                        row.Cells.Add(new WorksheetCell("29TH", "HeaderStyleData1"));
                    }
                    if (NoOFDays == 30)
                    {
                        row.Cells.Add(new WorksheetCell("29TH", "HeaderStyleData1"));
                        row.Cells.Add(new WorksheetCell("30TH", "HeaderStyleData1"));
                    }
                    if (NoOFDays == 31)
                    {
                        row.Cells.Add(new WorksheetCell("29TH", "HeaderStyleData1"));
                        row.Cells.Add(new WorksheetCell("30TH", "HeaderStyleData1"));
                        row.Cells.Add(new WorksheetCell("31ST", "HeaderStyleData1"));
                    }
                    row.Cells.Add(new WorksheetCell("Total Affixation Done", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Stock In Hand As On Date", "HeaderStyleData1"));
                    row = sheet.Table.Rows.Add();

                    //Skip one cell:
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    // Skip one row:
                    //row.Index = 11;
                    //row = sheet.Table.Rows.Add();

                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    row.Index = 7;
                    int sno = 0;
                    int SNumber = 0;


                    string RTOColName = string.Empty; 
                    foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
                    {
                            sno = sno + 1;
                        row = sheet.Table.Rows.Add(); 
                        if (RTOColName != dtrows["RTO"].ToString())
                        {
                            SNumber = SNumber + 1;
                            RTOColName = dtrows["RTO"].ToString();
                            row.Cells.Add(new WorksheetCell(Convert.ToInt16(SNumber).ToString(), DataType.String, "HeaderStyleData2"));
                            row.Cells.Add(new WorksheetCell(dtrows["RTO"].ToString(), "HeaderStyleData2"));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell("", "HeaderStyleData2"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyleData2"));
                        }


                        row.Cells.Add(new WorksheetCell(dtrows["Perticulars"].ToString(), "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["1ST"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["2ND"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["3RD"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["4TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["5TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["6TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["7TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["8TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["9TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["10TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["11TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["12TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["13TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["14TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["15TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["16TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["17TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["18TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["19TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["20TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["21ST"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["22ND"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["23RD"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["24TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["25TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["26TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        row.Cells.Add(new WorksheetCell(dtrows["27TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                        if (NoOFDays == 28)
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["28TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            cell = row.Cells.Add();
                            cell.StyleID = "HeaderStyleData1";
                            cell.Formula = "=SUM(RC[-28]:RC[-1])";
                        }
                        if (NoOFDays == 29)
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["28TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            row.Cells.Add(new WorksheetCell(dtrows["29TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            cell = row.Cells.Add();
                            cell.StyleID = "HeaderStyleData1";
                            cell.Formula = "=SUM(RC[-29]:RC[-1])";
                        }
                        if (NoOFDays == 30)
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["28TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            row.Cells.Add(new WorksheetCell(dtrows["29TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            row.Cells.Add(new WorksheetCell(dtrows["30TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            cell = row.Cells.Add();
                            cell.StyleID = "HeaderStyleData1";
                            cell.Formula = "=SUM(RC[-30]:RC[-1])";
                        }
                        if (NoOFDays == 31)
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["28TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            row.Cells.Add(new WorksheetCell(dtrows["29TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            row.Cells.Add(new WorksheetCell(dtrows["30TH"].ToString(), DataType.Number, "HeaderStyleData2"));
                            row.Cells.Add(new WorksheetCell(dtrows["31ST"].ToString(), DataType.Number, "HeaderStyleData2"));
                            cell = row.Cells.Add();
                            cell.StyleID = "HeaderStyleData1";
                            cell.Formula = "=SUM(RC[-31]:RC[-1])";
                        }
                        row.Cells.Add(new WorksheetCell("", "HeaderStyleData1"));   //For--"Total Affixation Done" (Columns Total)
                    }
                    
                    //Logic for Last Total Row:
                    sno = sno + 1;
                    row = sheet.Table.Rows.Add();
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyleData1"));
                    row.Cells.Add(new WorksheetCell("Total :", "HeaderStyleData1"));
                    for (int i = 1; i <= NoOFDays; i++)
                    {
                        //formula on column:
                        cell = row.Cells.Add();
                        cell.StyleID = "HeaderStyleData1";
                        cell.Formula = "=SUM(R[-" + sno + "]C:R[-1]C)";
                    }
                    //For--"Total Affixation Done" (Rows Total)
                    cell = row.Cells.Add();
                    cell.StyleID = "HeaderStyleData1";
                    cell.Formula = "=SUM(R[-" + sno + "]C:R[-1]C)";

                    //For--"Total Stock In Hand As On Date" (Rows Total)
                    cell = row.Cells.Add();
                    cell.StyleID = "HeaderStyleData1";
                    cell.Formula = "=SUM(R[-" + sno + "]C:R[-1]C)";
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
                    //LabelError.Text = string.Empty;
                    //LabelError.Text = "No records found for selected date.";
                    string closescript1 = "<script>alert('No records found for selected Month/Year.')</script>";
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
    }
}