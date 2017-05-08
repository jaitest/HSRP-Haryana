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
    public partial class InvoiceSummaryReport : System.Web.UI.Page
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
                    }
                    else
                    {
                        FilldropDownListOrganization();
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
            book.Properties.Title = "Collection Summary";
            book.Properties.Created = DateTime.Now;

            #region Fetch Data
            DataTable dt = new DataTable();
            string sql = "select challanno as InvoiceNo,convert(date,challandate,106) as InvoiceDate,RTOLocationName as Location,SUM(CASE WHEN (VehicleType = 'MOTOR CYCLE' OR VehicleType='SCOOTER')  THEN 1 ELSE 0 END ) as '2W', SUM(CASE WHEN (VehicleType = 'Three Wheeler') THEN 1 ELSE 0 END ) '3W',SUM(CASE WHEN (VehicleType = 'LMV'  OR VehicleType='LMV(Class)') and  Vehicleclass ='Non-Transport'  THEN 1 ELSE 0 END ) as 'LMV',SUM(CASE WHEN (VehicleType = 'LMV'  OR VehicleType='LMV(Class)') and  Vehicleclass ='Transport'  THEN 1 ELSE 0 END ) as 'LMV Transport', SUM(CASE WHEN (Vehicletype ='MCV/HCV/TRAILERS')  THEN 1 ELSE 0 END ) as MCV, SUM(CASE WHEN (VehicleType = 'Tractor' )  THEN 1 ELSE 0 END ) as Tractor from HSRPRecords a ,rtolocation b where a.rtolocationid=b.rtolocationid  and a.HSRP_StateID='" + strStateId + "' and convert(date,challandate,106) between '" + OrderDate.SelectedDate + "' and '" + HSRPAuthDate.SelectedDate + "' and challanno is not null and challandate is not null group by challanno, convert(date,challandate,106),RTOLocationName order by convert(date,challandate,106) ,challanno ";
            

            SqlCommand cmd = new SqlCommand(sql, con);



            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);



            da.Fill(dt);


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

            Worksheet sheet = book.Worksheets.Add("Invoice Summary");

            #region UpperPart of Excel
            AddColumnToSheet(sheet, 100, dt.Columns.Count);
            int iIndex = 3;
            WorksheetRow row = sheet.Table.Rows.Add();
            row.Index = iIndex++;
            //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));

            row.Cells.Add(new WorksheetCell("Invoice Summary Report", "HeaderStyle3"));



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
            row.Index = iIndex++;


            row.Index = 6;
            //row.Cells.Add("Order Date");
            row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("InvoiceNo", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("InvoiceDate", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("2W", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("3W", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("LMV", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("LMV TRANSPORT", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("MCV", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("TRACTOR", "HeaderStyle6"));
            

            row = sheet.Table.Rows.Add();

           
         
            #endregion

            #region Column Creation and Assign Data
            //string RTOColName = string.Empty;
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    AddNewCell(row, dt.Columns[i].ColumnName.ToString().Remove(dt.Columns[i].ColumnName.ToString().Length - 2, 2), "HeaderStyleHeader", 1);
            //}
            //row = sheet.Table.Rows.Add();
            //row.Index = iIndex++;
            if (dt.Rows.Count > 0)
            {
                 
                int sno = 0;
                foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                {
                    sno = sno + 1;
                    row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell(dtrows["InvoiceNo"].ToString(), DataType.String, "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle"));

                    //row.Cells.Add(new WorksheetCell(dtrows["Dailyneworder"].ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["InvoiceDate"].ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["2W"].ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["3W"].ToString(), DataType.String, "HeaderStyle"));

                    row.Cells.Add(new WorksheetCell(dtrows["LMV"].ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["LMV Transport"].ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["MCV"].ToString(), DataType.String, "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell(dtrows["Tractor"].ToString(), DataType.String, "HeaderStyle"));
                  

                    row = sheet.Table.Rows.Add();
                }
                row = sheet.Table.Rows.Add();
                row = sheet.Table.Rows.Add();
                row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
            #endregion
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                // Save the file and open it
                book.Save(Response.OutputStream);

                //context.Response.ContentType = "text/csv";
                context.Response.ContentType = "application/vnd.ms-excel";
                string filename = "Invoice Summary Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.End();
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