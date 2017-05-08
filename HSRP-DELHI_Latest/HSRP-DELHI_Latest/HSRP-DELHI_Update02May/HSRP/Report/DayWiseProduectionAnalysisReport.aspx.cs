using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;


namespace HSRP.Report
{
    public partial class DayWiseProduectionAnalysisReport : System.Web.UI.Page
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
                    InitialSetting();
                    if (UserType1.Equals(0))
                    {
                        FilldropDownListOrganization();
                        FilldropDownListClient();
                    }
                    else
                    {
                        FilldropDownListOrganization();
                        FilldropDownListClient();
                    }
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }
    }


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


    private void FilldropDownListClient()
    {

        string SQLString12 = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HSRP_StateID1 + " and navembid is not null  Order by EmbCenterName";
        Utils.PopulateDropDownList(ddlembcenter, SQLString12.ToString(), CnnString1, "--Select Embossing Center--");

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


    protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    
    protected void DropDownListyearName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private Boolean validate1()
    {
        Boolean blnvalid = true;
        String getvalue = string.Empty;
        getvalue = DropDownListStateName.SelectedItem.Text;
        if (getvalue == "--Select State--")
        {
            blnvalid = false;

            Label1.Text = "Please select State Name";

        }
        return blnvalid;

    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        Export(DropDownListStateName.SelectedValue, DropDownListStateName.SelectedItem.Text);
        
    }
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);        

    private void Export(string strStateId, string strStateName)
    {
        try
        {

            Workbook book = new Workbook();
            // Specify which Sheet should be opened and the size of window by default
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.WindowTopX = 100;
            book.ExcelWorkbook.WindowTopY = 200;
            book.ExcelWorkbook.WindowHeight = 7000;
            book.ExcelWorkbook.WindowWidth = 8000;

            // Some optional properties of the Document
            book.Properties.Author = "HSRP";
            book.Properties.Title = "AP-TS";
            book.Properties.Created = DateTime.Now;

            #region Fetch Data
            DataTable dt = new DataTable();

           string  FromDate = OrderDate.SelectedDate.ToString("MM/dd/yy 00:00:00");
           string ToDate = HSRPAuthDate.SelectedDate.ToString("MM/dd/yy 23:59:59");
           string SQLString = "select operatorname, MachineType,StartTime,EndTime,Duration, ProductDimesion200,ProductDimesion285,ProductDimesion300,ProductDimesion500,Total,Reject,PerHours,DownTime,Reason,Date,sheetno,centername from DayWiseProductionAnalysis where HSRP_Stateid='" + HSRP_StateID1 + "' and EmbID='" + ddlembcenter.SelectedValue.ToString() + "' and Date between '" + FromDate + "' and '"+ToDate+"'";

           dt = Utils.GetDataTable(SQLString, CnnString1);
            #endregion
            // Add some styles to the Workbook

            #region Styles

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

            WorksheetStyle styleHeader = book.Styles.Add("HeaderStyleHeader");
            styleHeader.Font.FontName = "Tahoma";
            styleHeader.Interior.Color = "Red";
            styleHeader.Font.Size = 10;
            styleHeader.Font.Bold = true;
            styleHeader.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            styleHeader.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            styleHeader.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            styleHeader.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            styleHeader.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

            Worksheet sheet = book.Worksheets.Add("Report");

            #region UpperPart of Excel
            AddColumnToSheet(sheet, 100, dt.Columns.Count);
            int iIndex = 3;
            WorksheetRow row = sheet.Table.Rows.Add();
            row.Index = iIndex++;
            //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));

            row.Cells.Add(new WorksheetCell("Day Wise Production Report", "HeaderStyle3"));
           


            row = sheet.Table.Rows.Add();
            row.Index = iIndex++;

            AddNewCell(row, "State:", "HeaderStyle2", 1);
            AddNewCell(row, strStateName, "HeaderStyle2", 1);
            row = sheet.Table.Rows.Add();
            row.Index = iIndex++;

            DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            AddNewCell(row, "Report Duration:", "HeaderStyle2", 1);
            AddNewCell(row, OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
            row = sheet.Table.Rows.Add();

            row.Index = iIndex++;

            //row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
            //row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
            //row.Cells.Add(new WorksheetCell("New Order :", "HeaderStyle3"));
            //WorksheetCell cell = row.Cells.Add("New Order");
            //cell.MergeAcross = 6; // Merge two cells together
            //cell.StyleID = "HeaderStyleHeader";

            //WorksheetCell cellEmb = row.Cells.Add("Embossing Done");
            //cellEmb.MergeAcross = 6; // Merge two cells together
            //cellEmb.StyleID = "HeaderStyleHeader";

            //WorksheetCell cellPendProd = row.Cells.Add("Production Under Process");
            //cellPendProd.MergeAcross = 6; // Merge two cells together
            //cellPendProd.StyleID = "HeaderStyleHeader";

            //WorksheetCell cellAffix = row.Cells.Add("Affixation");
            //cellAffix.MergeAcross = 6; // Merge two cells together
            //cellAffix.StyleID = "HeaderStyleHeader";

            //WorksheetCell cellPendAffix = row.Cells.Add("Pending for Affixation");
            //cellPendAffix.MergeAcross = 6; // Merge two cells together
            //cellPendAffix.StyleID = "HeaderStyleHeader";
            //row = sheet.Table.Rows.Add();

            row.Index = iIndex++;
            #endregion

            #region Column Creation and Assign Data
            string RTOColName = string.Empty;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyleHeader", 1);
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
            #endregion
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);

            //context.Response.ContentType = "text/csv";
            context.Response.ContentType = "application/vnd.ms-excel";
            string filename = "Status Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();
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