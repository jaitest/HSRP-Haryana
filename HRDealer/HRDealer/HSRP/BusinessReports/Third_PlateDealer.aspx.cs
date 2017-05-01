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


namespace HSRP.BusinessReports
{
    public partial class Third_PlateDealer : System.Web.UI.Page
{
    int UserType1;
    string CnnString1 = string.Empty;
    string CnnString2 = string.Empty;
    string HSRP_StateID1 = string.Empty;
    string RTOLocationID1 = string.Empty;
    string strUserID1 = string.Empty;
    int intHSRPStateID1;
    int intRTOLocationID1;
    string SQLString1 = string.Empty;
    string OrderType;
    string recordtype = string.Empty;
    //DateTime OrderDate1;
    string strStateId = string.Empty;

   
    string day1date1 = string.Empty;
    string day1date = string.Empty;
    string day2date = string.Empty;
    string day3date = string.Empty;
    string day4date = string.Empty;
    string day5date = string.Empty;

    //SqlConnection con = null;
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    string TempStringConnection = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
       // CnnString2 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();


        
       
       
        //if (state == "9")
        //{
        //    TempStringConnection = CnnString1;
        //}
        //else if (state == "11")
        //{
        //    TempStringConnection = CnnString2;
        //}

       // con = new SqlConnection(TempStringConnection);
        if (Session["UID"] == null)
        {
            Response.Redirect("~/Login.aspx", true);
        }
        else
        {

            UserType1 = Convert.ToInt32(Session["UserType"]);
      

            strUserID1 = Session["UID"].ToString();

            HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
            RTOLocationID1 = Session["UserRTOLocationID"].ToString();
            if (!IsPostBack)
            {
                gridTD.Visible = false;
                grdthirdplate.Visible = false;
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
           // Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), con, "--Select State--");
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
         HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
       // HSRPAuthDate   OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
   // HSRPAuthDate    OrderDate.MaxDate = DateTime.Parse(MaxDate);
        CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
    }


    protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    
    protected void DropDownListyearName_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    //private Boolean validate1()
    //{
    //    Boolean blnvalid = true;
    //    String getvalue = string.Empty;
    //    getvalue = DropDownListStateName.SelectedItem.Text;
    //    if (getvalue == "--Select State--")
    //    {
    //        blnvalid = false;

    //        Label1.Text = "Please select State Name";

    //    }
    //    return blnvalid;

    //}

    protected void btnexport_Click(object sender, EventArgs e)
    {
        Export();
        
    }

    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);  
    private void Export()
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
            book.Properties.Title = "Report";
            book.Properties.Created = DateTime.Now;

            #region Fetch Data
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("[Business_report_AssignmentVS3rdplateProduction_SummaryDealer]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
           // cmd.Parameters.Add(new SqlParameter("@datefrom", OrderDate.SelectedDate));
            cmd.Parameters.Add(new SqlParameter("@reportDate", HSRPAuthDate.SelectedDate));
           // cmd.Parameters.Add(new SqlParameter("@addrecordby", ddlBothDealerHHT.SelectedItem.Text));

          
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

            Worksheet sheet = book.Worksheets.Add("Report");

            #region UpperPart of Excel
            AddColumnToSheet(sheet, 100, dt.Columns.Count);
            int iIndex = 3;
            WorksheetRow row = sheet.Table.Rows.Add();
            row.Index = iIndex++;
            //  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
            row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
           
           row.Cells.Add(new WorksheetCell("3rdPlate Report", "HeaderStyle3"));
           


            row = sheet.Table.Rows.Add();
            row.Index = iIndex++;

            AddNewCell(row, "State:", "HeaderStyle2", 1);
            AddNewCell(row, DropDownListStateName.SelectedItem.Text, "HeaderStyle2", 1);
            row = sheet.Table.Rows.Add();
            row.Index = iIndex++;

            DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            AddNewCell(row, "Report Duration:", "HeaderStyle2", 1);
            AddNewCell(row, HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
            row = sheet.Table.Rows.Add();

            row.Index = iIndex++;


            row.Index = iIndex++;
            #endregion

            #region Column Creation and Assign Data
            string RTOColName = string.Empty;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                AddNewCell(row, dt.Columns[i].ColumnName.ToString().Remove(dt.Columns[i].ColumnName.ToString().Length - 2, 0), "HeaderStyleHeader", 1);
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
                row = sheet.Table.Rows.Add();

            
            #endregion
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);

            //context.Response.ContentType = "text/csv";
            context.Response.ContentType = "application/vnd.ms-excel";
            string filename = "Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

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


        //For Details Third Plate Report

    protected void btndetails_Click(object sender, EventArgs e)
    {
        try
        {
           

           // SqlConnection con = new SqlConnection(TempStringConnection);
            #region Fetch Data
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            //change sp Name
            //cmd = new SqlCommand("Business_report_RTOwiseaffixation_Summary", con);
            cmd = new SqlCommand("[Business_report_AssignmentVS3rdplateProduction_SummaryDealer]", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
            //cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDate.SelectedDate));
            cmd.Parameters.Add(new SqlParameter("@reportDate", HSRPAuthDate.SelectedDate));
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            #endregion
            if (dt.Rows.Count > 0)
            {
                gridTD.Visible = true;
                grdthirdplate.DataSource = dt;
                grdthirdplate.DataBind();
                grdthirdplate.Visible = true;
            }
            else
            {
                gridTD.Visible = false;
                grdthirdplate.DataSource = null;
                grdthirdplate.Visible = false;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

   

    protected void grdthirdplate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string embcentername = string.Empty;
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            StringBuilder strSQL = new StringBuilder();
           // DateTime ReportDate2 = OrderDate.SelectedDate;
           
            DateTime ReportDate2 = HSRPAuthDate.SelectedDate;
            string ReportDate3 = Convert.ToString(ReportDate2);
            string state = DropDownListStateName.SelectedValue;
            string hsrprecord_creationdate = string.Empty;
            if (state == "9" || state == "11")
            {
                hsrprecord_creationdate = "01-jan-2001";
            }


            if (e.CommandName == "TodayProd")//if (e.CommandName == "Opening")
            {
                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)oItem.Cells[1].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
               // strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode, OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo, PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate, RecievedAtAffixationDateTime from hsrprecords a ,rtolocation b where a.HSRP_StateID='" + state + "' and a.rtolocationid=b.rtolocationid and convert(date,erpassigndate,103)='" + ReportDate3 + "' and a.rtolocationid=b.rtolocationid	 and embcentername='" + embcentername + "' and StickerMandatory='Y'  and hsrprecordid not in(select distinct HSRPRecordID from StickerLog where SaveDate<'" + ReportDate3 + "')");
            }

            if (e.CommandName == "Day1")
            {
                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)oItem.Cells[1].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();            
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode, OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo, PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate, RecievedAtAffixationDateTime from hsrprecords a ,rtolocation b where a.HSRP_StateID='" + state + "' and a.rtolocationid=b.rtolocationid and convert(date,erpassigndate,103)='" + ReportDate3 + "' and a.rtolocationid=b.rtolocationid	 and embcentername='" + embcentername + "' and StickerMandatory='Y'  and hsrprecordid not in(select distinct HSRPRecordID from StickerLog where SaveDate<'" + ReportDate3 + "')");
            }

            else if(e.CommandName == "Day2")
            {
                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)oItem.Cells[1].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
             
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode,hsrp_rear_lasercode, OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate, RecievedAtAffixationDateTime  from hsrprecords a ,rtolocation b where a.HSRP_StateID='" + state + "' and a.rtolocationid=b.rtolocationid	 and convert(date,erpassigndate,103)=dateadd(day,-1,'" + ReportDate3 + "') and embcentername='" + embcentername + "' and StickerMandatory='Y' and hsrprecordid not in(select distinct HSRPRecordID from StickerLog where SaveDate<'" + ReportDate3 + "')");
            }
            else if (e.CommandName == "Day3")
            {
                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)oItem.Cells[1].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
             
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode, OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate, RecievedAtAffixationDateTime  from hsrprecords a ,rtolocation b where a.HSRP_StateID='" + state + "' and a.rtolocationid=b.rtolocationid and convert(date,erpassigndate,103)=dateadd(day,-2,'" + ReportDate3 + "') and embcentername='" + embcentername + "' and StickerMandatory='Y' and hsrprecordid not in(select distinct HSRPRecordID from StickerLog where SaveDate<'" + ReportDate3 + "')");
            }
            else if (e.CommandName == "Day4")
            {
                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)oItem.Cells[1].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();            
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode, OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo, PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate, RecievedAtAffixationDateTime from hsrprecords a ,rtolocation b  where a.HSRP_StateID='" + state + "'and a.rtolocationid=b.rtolocationid and convert(date,erpassigndate,103)=dateadd(day,-3,'" + ReportDate3 + "') and embcentername='" + embcentername + "' and StickerMandatory='Y' and hsrprecordid not in(select distinct HSRPRecordID from StickerLog where SaveDate<'" + ReportDate3 + "')");
            }

            else if (e.CommandName == "Day5orMore")
            {
                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)oItem.Cells[1].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode,hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo, PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate, RecievedAtAffixationDateTime from hsrprecords a,rtolocation b where a.HSRP_StateID='" + state + "' and a.RTOLocationID=b.rtolocationid and  embcentername='" + embcentername + "' and convert(date,erpassigndate,103)<=dateadd(day,-4,'" + ReportDate3 + "') and StickerMandatory='Y' and hsrprecordid not in(select distinct HSRPRecordID from StickerLog where SaveDate<'" + ReportDate3 + "')and hsrprecord_creationdate>'08/oct/2015'");
                //strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode,hsrp_rear_lasercode, OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo, PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate, RecievedAtAffixationDateTime from hsrprecords a ,rtolocation b where a.HSRP_StateID='" + state + "'and a.rtolocationid=b.rtolocationid	and convert(date,erpassigndate,103)=dateadd(day,-4,'" + ReportDate3 + "') and embcentername='" + embcentername + "' and StickerMandatory='Y' and hsrprecordid not in(select distinct HSRPRecordID from StickerLog where SaveDate<'" + ReportDate3 + "')");
            }
            else
            {
                //When you get some details then access in else condition
            }

            string strVehicleType = string.Empty;
            DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString1);
            if (dt.Rows.Count > 0)
            {
                string filename = "Export_Data" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Export Data";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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



                Worksheet sheet11 = book.Worksheets.Add("Report");
                sheet11.Table.Columns.Add(new WorksheetColumn(60));
                sheet11.Table.Columns.Add(new WorksheetColumn(205));
                sheet11.Table.Columns.Add(new WorksheetColumn(100));
                sheet11.Table.Columns.Add(new WorksheetColumn(130));

                sheet11.Table.Columns.Add(new WorksheetColumn(100));
                sheet11.Table.Columns.Add(new WorksheetColumn(120));
                sheet11.Table.Columns.Add(new WorksheetColumn(112));
                sheet11.Table.Columns.Add(new WorksheetColumn(109));
                sheet11.Table.Columns.Add(new WorksheetColumn(105));
                sheet11.Table.Columns.Add(new WorksheetColumn(160));


                Worksheet sheet = book.Worksheets.Add("Export Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));
                sheet.Table.Columns.Add(new WorksheetColumn(105));
                sheet.Table.Columns.Add(new WorksheetColumn(160));


                WorksheetRow row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Rport", "HeaderStyle3"));

                row = sheet.Table.Rows.Add();

                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Export Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 6;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 7;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 8;
                row.Cells.Add(new WorksheetCell("S.No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("VehicleReceivedDate", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("ERPAssignDate", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("embcentername", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("rtolocationname", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("vehicleregno", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("hsrp_front_lasercode", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("hsrp_rear_lasercode ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Order Status", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Challan No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Challan Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("RejectFlag ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Prod. Sheet No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Prod Sheet Date", "HeaderStyle"));
               // row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;

                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["aptgvehrecdate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["erpassigndate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["embcentername"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["rtolocationname"].ToString(), DataType.String, "HeaderStyle1"));
                        // row.Cells.Add(new WorksheetCell(dtrows["ManufacturerName"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["vehicleregno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["challanno"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["challandate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RejectFlag"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["NewPdfRunningNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["PdfDownloadDate"].ToString(), DataType.String, "HeaderStyle1"));
                       // row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle1"));

                    }

                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grdthirdplate_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        string embcenterindex = grdthirdplate.SelectedRow.RowIndex.ToString();
    } 


}
}