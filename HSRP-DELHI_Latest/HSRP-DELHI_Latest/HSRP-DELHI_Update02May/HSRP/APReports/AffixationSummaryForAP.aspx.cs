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
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace HSRP.APReport
{
    public partial class AffixationSummaryForAP : System.Web.UI.Page
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

            SqlCommand cmd = new SqlCommand("[Report_LocationAndVehicleWise_Pendency]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StateId", strStateId));
            cmd.Parameters.Add(new SqlParameter("@FDate", OrderDate.SelectedDate));
            cmd.Parameters.Add(new SqlParameter("@LDate", HSRPAuthDate.SelectedDate));
          
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

            Worksheet sheet = book.Worksheets.Add("MPOnlineReport");

            #region UpperPart of Excel
            AddColumnToSheet(sheet, 100, dt.Columns.Count);
            int iIndex = 3;
            WorksheetRow row = sheet.Table.Rows.Add();
            row.Index = iIndex++;
            //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
           
                row.Cells.Add(new WorksheetCell("Collection and Pendency Report", "HeaderStyle3"));
           


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

            row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
            row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
          //  row.Cells.Add(new WorksheetCell("New Order :", "HeaderStyle3"));
            WorksheetCell cell = row.Cells.Add("New Order");
            cell.MergeAcross = 6; // Merge two cells together
            cell.StyleID = "HeaderStyleHeader";

            WorksheetCell cellEmb = row.Cells.Add("Embossing Done");
            cellEmb.MergeAcross = 6; // Merge two cells together
            cellEmb.StyleID = "HeaderStyleHeader";

            WorksheetCell cellPendProd = row.Cells.Add("Pending for Production");
            cellPendProd.MergeAcross = 6; // Merge two cells together
            cellPendProd.StyleID = "HeaderStyleHeader";

            WorksheetCell cellAffix = row.Cells.Add("Affixation");
            cellAffix.MergeAcross = 6; // Merge two cells together
            cellAffix.StyleID = "HeaderStyleHeader";

            WorksheetCell cellPendAffix = row.Cells.Add("Pending for Affixation");
            cellPendAffix.MergeAcross = 6; // Merge two cells together
            cellPendAffix.StyleID = "HeaderStyleHeader";
            row = sheet.Table.Rows.Add();

            row.Index = iIndex++;
            #endregion

            #region Column Creation and Assign Data
            string RTOColName = string.Empty;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                AddNewCell(row, dt.Columns[i].ColumnName.ToString().Remove(dt.Columns[i].ColumnName.ToString().Length - 2, 2), "HeaderStyleHeader", 1);
            }
            row = sheet.Table.Rows.Add();
            row.Index = iIndex++;
            for (int j = 0; j < dt.Rows.Count; j++)
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    if (dt.Rows[j]["LocationNE"].ToString().Equals("ZZZZZ") && i.Equals(0))
                    {
                        AddNewCell(row, "Total", "HeaderStyle6", 1);
                    }
                    else if (dt.Rows[j]["LocationNE"].ToString().Equals("ZZZZZ") && i.Equals(1))
                    {
                        AddNewCell(row, "", "HeaderStyle6", 1);
                    }
                    else
                    {
                        if (dt.Columns[i].ColumnName.ToString().Equals("DateNE"))
                        {
                            AddNewCell(row, (Convert.ToDateTime(dt.Rows[j][i].ToString())).ToString("dd/MMM/yyyy"), "HeaderStyle6", 1);
                        }
                        else
                        {
                            AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                        }
                    }
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
            string filename = "CollectionSummaryAndPendency" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

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

    private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
    {
        PdfPCell newCellPDF = null;
        BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        if (iFont.Equals(0))
        {
            newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        }
        else if (iFont.Equals(1))
        {
            newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        }
        newCellPDF.Colspan = iSpan;
        newCellPDF.BorderWidthLeft = iLeftWidth;
        newCellPDF.BorderWidthRight = iRightWidth;
        newCellPDF.BorderWidthTop = iTopWidth;
        newCellPDF.BorderWidthBottom = iBottomWidth;
        newCellPDF.HorizontalAlignment = iAllign;
        if (!iRowHeight.Equals(0))
        {
            newCellPDF.FixedHeight = iRowHeight;
        }
        if (!iRowWidth.Equals(0))
        {
        }
        table.AddCell(newCellPDF);
    }
    private static void GenerateCell1(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
    {
        PdfPCell newCellPDF = null;
        BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        if (iFont.Equals(0))
        {
            newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.BLACK)));
        }
        else if (iFont.Equals(1))
        {
            newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        }
        newCellPDF.Colspan = iSpan;
        newCellPDF.BorderWidthLeft = iLeftWidth;
        newCellPDF.BorderWidthRight = iRightWidth;
        newCellPDF.BorderWidthTop = iTopWidth;
        newCellPDF.BorderWidthBottom = iBottomWidth;
        newCellPDF.HorizontalAlignment = iAllign;
        if (!iRowHeight.Equals(0))
        {
            newCellPDF.FixedHeight = iRowHeight;
        }
        if (!iRowWidth.Equals(0))
        {
        }
        table.AddCell(newCellPDF);
    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        string strQuery = string.Empty;
        
        int I2WTotal = 0;
        int I3WTotal = 0;
        int I4WTotal = 0;
        int I4WWTotal = 0;
        int ICommTotal = 0;
        int ItractorTotal = 0;
        int ISubTotal = 0;

        String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
        String ReportDateEnd = StringAuthDate[0] + "-" + StringAuthDate[1] + "-" + StringAuthDate[2].Split(' ')[0];
        string ReportDate2 = ReportDateEnd + " 23:59:59";
        String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


        String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
        string Mon = ("0" + StringOrderDate[0]);
        string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
        String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

        String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
        String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

        String DatePrint = DatePrint1 + "   To   " + DatePrint2;

        String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
        string ReportDate1 = ReportDate + " 00:00:00";

        strQuery = "select (select rtolocationname from rtolocation where rtolocationid=a.rtolocationid) locationname,SUM(CASE WHEN (a.VehicleType='SCOOTER' or a.VehicleType='MOTOR CYCLE')  THEN 1 ELSE 0 END ) AS [2WCount],SUM(CASE WHEN (a.VehicleType = 'Three Wheeler') THEN 1 ELSE 0 END ) AS [3WCount],SUM(CASE WHEN (a.VehicleType = 'Tractor' )  THEN 1 ELSE 0 END ) AS [TractorCount],SUM(CASE WHEN ((a.VehicleType = 'LMV' or VehicleType='LMV(Class)') and VehicleClass ='Non-Transport')   THEN 1 ELSE 0 END ) AS [4WCount],SUM(CASE WHEN ((a.VehicleType = 'LMV' or VehicleType='LMV(Class)') and VehicleClass ='Transport')  THEN 1 ELSE 0 END ) AS [4Wtcount],SUM(CASE WHEN (a.Vehicletype ='MCV/HCV/TRAILERS')  THEN 1 ELSE 0 END ) AS [HeavyCount] from hsrprecords a  where  HSRP_StateID=9 and a.ordercloseddate between '"+ ReportDate1 +"' and '"+ ReportDate2 +"' and netamount >0 group by a.rtolocationid order by (select rtolocationname from rtolocation where rtolocationid=a.rtolocationid)";
        DataTable dt = Utils.GetDataTable(strQuery, CnnString1);
        if (dt.Rows.Count > 0)
        {
            string filename1 = "AffixationSummary" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            String StringField1 = String.Empty;
            String StringAlert1 = String.Empty;
            StringBuilder bb1 = new StringBuilder();
            Document document1 = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            document1.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            string PdfFolder1 = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename1;
            PdfWriter.GetInstance(document1, new FileStream(PdfFolder1, FileMode.Create));
            //Opens the document:
            document1.Open();

            //Adds content to the document:
            PdfPTable table;

            table = new PdfPTable(10);
            var colWidthPercentages = new[] { 20f, 35f, 20f, 20f, 20f, 20f, 33f, 35f, 35f,35f};
            table.SetWidths(colWidthPercentages);
            table.TotalWidth = 1000f;
            string period = "From :-" + OrderDate.SelectedDate.ToString("dd-MM-yyyy") + " To " + HSRPAuthDate.SelectedDate.ToString("dd-MM-yyyy");
            //GenerateCell(table, 10, 1, 1, 1, 1, 1, 0, "Andhra Pradesh", 30, 0);
            string cname = Utils.getDataSingleValue("select CompanyName from HSRPState where hsrp_stateid=9", CnnString1, "CompanyName");
            string add = Utils.getDataSingleValue("select Address1 from HSRPState where hsrp_stateid=9", CnnString1, "Address1");
            //GenerateCell(table, 10, 1, 1, 1, 1, 1, 0, "Andhra Pradesh", 30, 0);
            GenerateCell(table, 10, 1, 1, 1, 0, 1, 0, cname, 30, 0);
            //GenerateCell(table, 10, 1, 1, 1, 1, 1, 0, add, 30, 0);
            GenerateCell(table, 10, 1, 1, 0, 1, 1, 0, "Affixation Summary Report " +period, 30, 0);
            //GenerateCell(table, 3, 1, 1, 0, 1, 1, 0, "", 30, 0);
            //GenerateCell(table, 7, 0, 1, 0, 1, 1, 0, "", 30, 0);
            GenerateCell(table, 3, 1, 1, 0, 1, 1, 0, "Location", 30, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "2W", 30, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "3W", 30, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "4W", 30, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "4WT", 30, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "Comm", 30, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "Tractor", 30, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, "Total", 30, 0);

            for (int ICount = 0; ICount < dt.Rows.Count; ICount++)
            {
                int I2W = 0;
                int I3W = 0;
                int I4W = 0;
                int I4WT = 0;
                int IComm = 0;
                int ITractor = 0;
                int Itotal = 0;
                I2WTotal = I2WTotal + int.Parse(dt.Rows[ICount]["2WCount"].ToString());
                I3WTotal = I3WTotal + int.Parse(dt.Rows[ICount]["3WCount"].ToString());
                I4WTotal = I4WTotal + int.Parse(dt.Rows[ICount]["4WCount"].ToString());
                I4WWTotal = I4WWTotal + int.Parse(dt.Rows[ICount]["4Wtcount"].ToString());
                ItractorTotal = ItractorTotal + int.Parse(dt.Rows[ICount]["TractorCount"].ToString());
                ICommTotal = ICommTotal + int.Parse(dt.Rows[ICount]["HeavyCount"].ToString());

                I2W = int.Parse(dt.Rows[ICount]["2WCount"].ToString());
                I3W = int.Parse(dt.Rows[ICount]["3WCount"].ToString());
                I4W = int.Parse(dt.Rows[ICount]["4WCount"].ToString());
                I4WT = int.Parse(dt.Rows[ICount]["4Wtcount"].ToString());
                ITractor = int.Parse(dt.Rows[ICount]["TractorCount"].ToString());
                IComm = int.Parse(dt.Rows[ICount]["HeavyCount"].ToString());
                Itotal = I2W + I3W + I4W + I4WT + ITractor + IComm;
                ISubTotal = ISubTotal + Itotal;
                GenerateCell(table, 3, 1, 1, 0, 1, 0, 0, dt.Rows[ICount]["locationname"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[ICount]["2WCount"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[ICount]["3WCount"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[ICount]["4WCount"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[ICount]["4Wtcount"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[ICount]["HeavyCount"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[ICount]["TractorCount"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, Itotal.ToString(), 0, 0);
            }
            GenerateCell(table, 3, 1, 1, 0, 1, 1, 0, "Total", 30, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, I2WTotal.ToString(), 10, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, I3WTotal.ToString(), 10, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, I4WTotal.ToString(), 10, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, I4WWTotal.ToString(), 10, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, ICommTotal.ToString(), 10, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, ItractorTotal.ToString(), 10, 0);
            GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, ISubTotal.ToString(), 10, 0);

            GenerateCell(table, 10, 1, 1, 0, 1, 1, 0, "* 2W-Two Wheeler, 3W- Three Wheeler, 4W- LMV, 4WT-LMV Transport, Comm- Heavy Vehicle", 30, 0);
            string sdat = "select getdate() as gdate";
            string reportdt = Utils.getDataSingleValue(sdat, CnnString1, "gdate");
            string uname = "select userfirstname from users where userid='" + strUserID1 + "'";
            string username = Utils.getDataSingleValue(uname, CnnString1, "userfirstname");
            GenerateCell1(table, 10, 1, 1, 0, 0, 2, 0, "Report Generated Datetime : " + reportdt, 30, 0);
            GenerateCell1(table, 10, 1, 1, 0, 1, 2, 0, "Downloaded by : " + username, 30, 0);


            document1.Add(table);
            document1.Close();
            HttpContext context1 = HttpContext.Current;
            context1.Response.ContentType = "Application/pdf";
            context1.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename1);
            context1.Response.WriteFile(PdfFolder1);
            context1.Response.End();
        }
        else
        {
            Label1.Visible = true;
            Label1.Text = "";
            Label1.Text = "No Record Found";
        }

    } 


}
}