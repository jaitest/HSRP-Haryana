using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using CarlosAg.ExcelXmlWriter;

namespace HSRP.Report
{
    public partial class BlockVehicleReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;
        string OrderStatus = string.Empty;
        string ToDate;
       
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
                        //string State_ID = DropDownListStateName.SelectedValue.ToString();
                        InitialSetting();
                        if (UserType.Equals(0))
                        {
                           // FilldropDownListOrganization(); 
                        }
                        else
                        {
                           // FilldropDownListOrganization();
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

        #region DropDown

        //private void FilldropDownListOrganization()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='" + HSRPStateID + "' and ActiveStatus='Y' Order by HSRPStateName";
        //        DataTable dts = Utils.GetDataTable(SQLString, CnnString);
        //        DropDownListStateName.DataSource = dts;
        //        DropDownListStateName.DataBind();
        //    }
        //    else
        //    {
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='" + HSRPStateID + "' and ActiveStatus='Y' Order by HSRPStateName";              
        //        DataTable dts = Utils.GetDataTable(SQLString, CnnString);
        //        DropDownListStateName.DataSource = dts;
        //        DropDownListStateName.DataBind();

        //    }

        //}


        #endregion

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                //if (DropDownListStateName.Text == ("--Select State--"))
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select State..');", true);
                //    return;
                //}
                this.SaveAndDownloadFile();
            }
            catch (Exception ex)
            {

            }
        }


        int icount = 0;
        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            Export(book, 1, "BlockVehicleReport");


        }

        private void Export(Workbook book, int iActiveSheet, string strProcName)
        {
            try
            {
                LabelError.Visible = false;

                string filename = "Block Vehicle Registration Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

                string strOrderType = string.Empty;
                SqlConnection con = new SqlConnection(CnnString);


                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = iActiveSheet;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Block Vehicle Registration Report";
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


                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@stateid", Convert.ToInt32(DropDownListStateName.SelectedValue.ToString()) ));
                cmd.Parameters.Add(new SqlParameter("@fromdate", OrderDate.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@todate", HSRPAuthDate.SelectedDate));
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                #endregion

                if (dt.Rows.Count > 0)
                {
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Bucket Movement Report");
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;



                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Index = iIndex++;
                    //AddNewCell(row, "State:", "HeaderStyle2", 1);
                    //AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);

                    //row = sheet.Table.Rows.Add();
                    //row.Index = iIndex++;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());


                    AddNewCell(row, "Report Date from:", "HeaderStyle2", 1);
                    AddNewCell(row, OrderDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    AddNewCell(row, "Report Date To:", "HeaderStyle2", 1);
                    AddNewCell(row, HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

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
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
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
                    LabelError.Visible =true;
                    LabelError.Text = "Record not Found";
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