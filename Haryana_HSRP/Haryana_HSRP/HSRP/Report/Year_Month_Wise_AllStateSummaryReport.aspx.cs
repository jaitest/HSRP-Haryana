using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarlosAg.ExcelXmlWriter;
using System.Data;

namespace HSRP.Report
{
    public partial class Year_Month_Wise_AllStateSummaryReport : System.Web.UI.Page
    {
        String strSqlString = String.Empty;
        String strCnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillState();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            strSqlString = "[Year_Month_Wise_ReportAllSatateComparison_WithStaggingData] '"+ddlStateName.SelectedValue+"'";
            DataTable dtResult = Utils.GetDataTable(strSqlString, strCnnString);
            if (dtResult.Rows.Count > 0)
            {
                GridView1.DataSource = dtResult;
                GridView1.DataBind();
            }
            else
            {
                lblError.Text = "No Record Found";
            }
        }

        #region Drop Dwon
        public void FillState()
        {
            strSqlString = "Select * from HsrpState where activestatus='Y'";
            Utils.PopulateDropDownList(ddlStateName, strSqlString, strCnnString, "--Select State--");
        }
        #endregion

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string filename = ddlStateName.SelectedItem.ToString()+"_Year_Month_Wise_Summary_report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
            Workbook book = new Workbook();
            #region WorkBook Size And Properties
            // Specify which Sheet should be opened and the size of window by default
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "HSRP Total Summary Report Year And Month Wise";
            book.Properties.Created = DateTime.Now;
            #endregion
            
            #region Style Of Work Book
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

            #endregion

            Worksheet sheet = book.Worksheets.Add("HSRP Total Summary Report Year And Month Wise");

            #region Column Declaration
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            #endregion           

            #region Row 2
            WorksheetRow row = sheet.Table.Rows.Add();

            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
            WorksheetCell cell = row.Cells.Add("HSRP Total Summary Report Year And Month Wise");
            cell.MergeAcross = 3; // Merge two cells together
            cell.StyleID = "HeaderStyle3";
            #endregion

            #region Row 3
            row = sheet.Table.Rows.Add();
            row.Index = 3;
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell(ddlStateName.SelectedItem.ToString(), "HeaderStyle2"));
            #endregion

            #region Row 4
            row = sheet.Table.Rows.Add();
            row.Index = 4;

            DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("Report Generated Date :", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MM/yyyy"), "HeaderStyle2"));
            #endregion

            #region Row 5
            row = sheet.Table.Rows.Add();

            row.Index = 5;
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
            #endregion        
       
            strSqlString = "[Year_Month_Wise_ReportAllSatateComparison_WithStaggingData] '" + ddlStateName.SelectedValue + "'";        
            DataTable dtResult = Utils.GetDataTable(strSqlString, strCnnString);

            string RTOColName = string.Empty;
            if (dtResult.Rows.Count > 0)
            {

                #region Row Column Creation
                row = sheet.Table.Rows.Add();
                row.Index = 6;
                for (int iColCount = 0; iColCount < dtResult.Columns.Count; iColCount++)
                {
                    row.Cells.Add(new WorksheetCell(dtResult.Columns[iColCount].ColumnName.ToString(), DataType.String, "HeaderStyle"));
                }               
                #endregion   

                row = sheet.Table.Rows.Add();

                #region Dynamic Row Creation
                for (int iRowCount=0;iRowCount<dtResult.Rows.Count;iRowCount++)
                {
                    for (int iColCount = 0; iColCount < dtResult.Columns.Count; iColCount++)
                    {
                        row.Cells.Add(new WorksheetCell(dtResult.Rows[iRowCount][iColCount].ToString(), DataType.String, "HeaderStyle"));
                    }
                    row = sheet.Table.Rows.Add();
                }
                #endregion

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
    }
}