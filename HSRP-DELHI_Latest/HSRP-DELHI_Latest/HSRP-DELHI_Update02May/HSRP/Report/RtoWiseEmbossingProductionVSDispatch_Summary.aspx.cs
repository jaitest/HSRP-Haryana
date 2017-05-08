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


namespace HSRP.Report
{
    public partial class RtoWiseEmbossingProductionVSDispatch_Summary : System.Web.UI.Page
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
                            DropDownListStateName.Visible = true;
                            FilldropDownListOrganization();
                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                        }
                        else
                        {

                            DropDownListStateName.Visible = true;
                            FilldropDownListOrganization();

                            OrderDatefrom.SelectedDate = System.DateTime.Now;
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

            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

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
            SaveAndDownloadFile();
        }

        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            string filename = "Production VS Dispatch" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(strOrderType, book, 1, "report_Business_ProductionVSDispatch_Summary");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }

        int icount = 0;

        private void Export(string strReportType, Workbook book, int iActiveSheet, string strProcName)
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

                string strreptype="E";
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@reptype", strreptype));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);



                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("Production VS Dispatch");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, OrderDatefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                AddNewCell(row, "", "HeaderStyle6", 1);
                // WorksheetCell cell1 = row.Cells.Add("Today's Collection");
                //cell1.MergeAcross = 3; // Merge cells together
                // cell1.StyleID = "HeaderStyle6";
                //WorksheetCell cell2 = row.Cells.Add("Collection and Deposit MTD");
                //cell2.MergeAcross = 3; // Merge cells together
                //cell2.StyleID = "HeaderStyle6";
                //WorksheetCell cell3 = row.Cells.Add("Collection and Deposit Fy YTD");
                //cell3.MergeAcross = 3; // Merge cells together
                //cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2"))
                    //{
                    //   // string strCol = dt.Columns[i].ColumnName.ToString();
                    //   // AddNewCell(row, strCol.Remove(strCol.Length - 1), "HeaderStyle6", 1);
                    //}
                    //else
                    //{
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    //}
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        //if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                        //{
                        //    AddNewCell(row, "Total", "HeaderStyle6", 1);
                        //}
                        //else
                        //{
                        AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);

                        //}
                    }
                    row = sheet.Table.Rows.Add();

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




        private void SaveAndDownloadFileAssignment()
        {
            Workbook book = new Workbook();
            string filename = "ASSIGNMENT VS PRODUCTION" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            ExportAssignment(strOrderType, book, 1, "report_AssignmentVSProduction_Summary");



            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);

            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();



        }

        int icount1 = 0;

        private void ExportAssignment(string strReportType, Workbook book, int iActiveSheet, string strProcName)
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

                #region Styles
                if (icount1 <= 0)
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

                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
                // cmd.Parameters.Add(new SqlParameter("@flag", DropDownListOrderType.SelectedValue));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();


                da.Fill(dt);



                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);



                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("ASSIGNMENT VS PRODUCTION");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, OrderDatefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                AddNewCell(row, "", "HeaderStyle6", 1);
                // WorksheetCell cell1 = row.Cells.Add("Today's Collection");
                //cell1.MergeAcross = 3; // Merge cells together
                // cell1.StyleID = "HeaderStyle6";
                //WorksheetCell cell2 = row.Cells.Add("Collection and Deposit MTD");
                //cell2.MergeAcross = 3; // Merge cells together
                //cell2.StyleID = "HeaderStyle6";
                //WorksheetCell cell3 = row.Cells.Add("Collection and Deposit Fy YTD");
                //cell3.MergeAcross = 3; // Merge cells together
                //cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2"))
                    //{
                    //   // string strCol = dt.Columns[i].ColumnName.ToString();
                    //   // AddNewCell(row, strCol.Remove(strCol.Length - 1), "HeaderStyle6", 1);
                    //}
                    //else
                    //{
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    //}
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        //if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                        //{
                        //    AddNewCell(row, "Total", "HeaderStyle6", 1);
                        //}
                        //else
                        //{
                        AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);

                        //}
                    }
                    row = sheet.Table.Rows.Add();

                }


            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnassign_Click(object sender, EventArgs e)
        {
            SaveAndDownloadFileAssignment();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(CnnString1);
            #region Fetch Data
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("report_AssignmentVSProduction_Summary", con);


            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
            cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
            // cmd.Parameters.Add(new SqlParameter("@flag", DropDownListOrderType.SelectedValue));

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // dt = new DataTable();
            da.Fill(dt);


            #endregion
            if (dt.Rows.Count > 0)
            {
                grdid.DataSource = dt;
                grdid.DataBind();
                grdid.Visible = true;
            }
        }



        protected void grdid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string embcentername = string.Empty;
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            StringBuilder strSQL = new StringBuilder();
            DateTime ReportDate2 = OrderDatefrom.SelectedDate;
            string ReportDate3 = Convert.ToString(ReportDate2);

            string state = DropDownListStateName.SelectedValue;
            if (e.CommandName == "Day5orMore")
            {
                
                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,OrderEmbossingDate,103)<=dateadd(day,-4,'" + ReportDate2 + "') and  hsrprecord_creationdate>'1-jan-2015 00:00:00' and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");
               
            }


            if (e.CommandName == "Day1")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,OrderEmbossingDate,103)=dateadd(day,0,'" + ReportDate2 + "') and  hsrprecord_creationdate>'1-jan-2015 00:00:00'  and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");
            }
            if (e.CommandName == "Day2")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,OrderEmbossingDate,103)=dateadd(day,-1,'" + ReportDate2 + "') and  hsrprecord_creationdate>'1-jan-2015 00:00:00'  and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");
            }
            if (e.CommandName == "Day3")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,OrderEmbossingDate,103)=dateadd(day,-2,'" + ReportDate2 + "')  and  hsrprecord_creationdate>'1-jan-2015 00:00:00' and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");
            }
            if (e.CommandName == "Day4")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label embcenter = (Label)grdid.Rows[RowIndex].FindControl("lblembname");
                embcentername = embcenter.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and embcentername='" + embcentername + "' and convert(date,OrderEmbossingDate,103)=dateadd(day,-3,'" + ReportDate2 + "') and  hsrprecord_creationdate>'1-jan-2015 00:00:00'  and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");


            }

                string strVehicleType = string.Empty;
                
                //OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
                ////string selectdate = OrderDatefrom.SelectedDate.AddDays(-4.00).ToString();
                //String[] StringAuthDate = OrderDatefrom.SelectedDate.ToString().Split('/');
                //String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
              





                //strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID=11 and a.RTOLocationID='" + RTOCode + "' and convert(date,erpassigndate) ='" + OrderDatefrom.SelectedDate.ToString() + "' and isnull(newpdfrunningno,'')='' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null)  and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') ");


                //strSQL.Append("select convert(date,erpassigndate) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode from hsrprecords a, rtolocation b where a.rtolocationid=b.RTOLocationID and b.HSRP_StateID='" + state + "' and OrderStatus in ('New Order') and convert(date,erpassigndate) <='" + ReportDate2 + "' and embcentername='" + embcentername + "' and isnull(orderembossingdate,'')='' and isnull(vehicleregno,'')!=''  and isnull(hsrp_front_lasercode,'')!='' and  isnull(hsrp_front_lasercode,'')!='' order by 1,2,3");


                // strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID='" + strStateID + "' and a.RTOLocationID='" + RTOLocationID + "' and convert(date,hsrprecord_creationdate) between '2014-12-31 00:00:00' and '2015-02-19 23:59:59' and orderstatus='New Order' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null)   and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') ");

                // strSQL.Append("select convert(date,erpassigndate) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode from hsrprecords a, rtolocation b where a.rtolocationid=b.RTOLocationID and b.HSRP_StateID='" + state + "' and OrderStatus in ('New Order') and embcentername='" + embcentername + "' and isnull(orderembossingdate,'')='' and isnull(vehicleregno,'')!='' and isnull(hsrp_front_lasercode,'')!='' and  isnull(HSRP_Rear_LaserCode,'')!='' and isnull(erpassigndate,'')!='' and hsrprecordid not in (select hsrprecordid from hsrprecords a, rtolocation b where a.rtolocationid=b.rtolocationid and a.HSRP_StateID='" + state + "' and convert(date,erpassigndate,103)>'" + ReportDate2 + "' and OrderStatus='New Order' and embcentername='" + embcentername + "')");

               


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
                    row.Cells.Add(new WorksheetCell("erpassigndate", "HeaderStyle"));
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
                    row.Cells.Add(new WorksheetCell("Vehicle reg.rec date", "HeaderStyle"));


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
                            row.Cells.Add(new WorksheetCell(dtrows["aptgvehrecdate"].ToString(), DataType.String, "HeaderStyle1"));


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
   


               // }
            
        }

        protected void grdid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string embcenter11 = grdid.SelectedRow.RowIndex.ToString();
        }

        protected void btnDetail_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(CnnString1);
            #region Fetch Data
            DataSet ds = new DataSet();
            string strreptype = "E";
            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("report_Business_ProductionVSDispatch_Summary", con);


            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
            cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
            cmd.Parameters.Add(new SqlParameter("@reptype", strreptype));

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // dt = new DataTable();
            da.Fill(dt);


            #endregion
            if (dt.Rows.Count > 0)
            {
                grdid.DataSource = dt;
                grdid.DataBind();
                grdid.Visible = true;
            }
        }

        private void SaveAndDownloadFile12()
        {
            Workbook book = new Workbook();
            string filename = "Production VS Dispatch" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export12(strOrderType, book, 1, "report_Business_ProductionVSDispatch_Summary");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }

       

        private void Export12(string strReportType, Workbook book, int iActiveSheet, string strProcName)
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

                string strreptype = "A";
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@reptype", strreptype));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                #endregion

                AddColumnToSheet(sheet, 100, dt.Columns.Count);



                int iIndex = 3;
                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                WorksheetCell cell = row.Cells.Add("Production VS Dispatch");
                cell.MergeAcross = 4; // Merge two cells together
                cell.StyleID = "HeaderStyle2";

                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                AddNewCell(row, "State:", "HeaderStyle2", 1);
                AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                AddNewCell(row, "Report Date:", "HeaderStyle2", 1);
                AddNewCell(row, OrderDatefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);
                row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd/MMM/yyyy"), "HeaderStyle2"));

                AddNewCell(row, "", "HeaderStyle2", 2);
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                AddNewCell(row, "", "HeaderStyle6", 1);
                // WorksheetCell cell1 = row.Cells.Add("Today's Collection");
                //cell1.MergeAcross = 3; // Merge cells together
                // cell1.StyleID = "HeaderStyle6";
                //WorksheetCell cell2 = row.Cells.Add("Collection and Deposit MTD");
                //cell2.MergeAcross = 3; // Merge cells together
                //cell2.StyleID = "HeaderStyle6";
                //WorksheetCell cell3 = row.Cells.Add("Collection and Deposit Fy YTD");
                //cell3.MergeAcross = 3; // Merge cells together
                //cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();

                row.Index = iIndex++;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //if (dt.Columns[i].ColumnName.ToString().EndsWith("1") || dt.Columns[i].ColumnName.ToString().EndsWith("2"))
                    //{
                    //   // string strCol = dt.Columns[i].ColumnName.ToString();
                    //   // AddNewCell(row, strCol.Remove(strCol.Length - 1), "HeaderStyle6", 1);
                    //}
                    //else
                    //{
                    AddNewCell(row, dt.Columns[i].ColumnName.ToString(), "HeaderStyle6", 1);

                    //}
                }
                row = sheet.Table.Rows.Add();
                row.Index = iIndex++;
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {

                        //if (dt.Rows[j]["Location"].ToString().Equals("ZZZZZ") && i.Equals(0))
                        //{
                        //    AddNewCell(row, "Total", "HeaderStyle6", 1);
                        //}
                        //else
                        //{
                        AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);

                        //}
                    }
                    row = sheet.Table.Rows.Add();

                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void btnproctiondispatch_Click(object sender, EventArgs e)
        {
            SaveAndDownloadFile12();
        }

      


        protected void btnnDetails_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(CnnString1);
            #region Fetch Data
            DataSet ds = new DataSet();
            string strreptype = "A";
            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("report_Business_ProductionVSDispatch_Summary", con);


            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
            cmd.Parameters.Add(new SqlParameter("@reportDate", OrderDatefrom.SelectedDate));
            cmd.Parameters.Add(new SqlParameter("@reptype", strreptype));

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // dt = new DataTable();
            da.Fill(dt);


            #endregion
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string embcenter11 = GridView1.SelectedRow.RowIndex.ToString();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string rtolocationname = string.Empty;
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            StringBuilder strSQL = new StringBuilder();
            DateTime ReportDate2 = OrderDatefrom.SelectedDate;
            string ReportDate3 = Convert.ToString(ReportDate2);

            string state = DropDownListStateName.SelectedValue;
            if (e.CommandName == "Day5orMore")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                rtolocationname = rtolocation.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,OrderEmbossingDate,103)<=dateadd(day,-4,'" + ReportDate2 + "') and orderembossingdate>'01-jan-2015' and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");

            }


            if (e.CommandName == "Day1")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                rtolocationname = rtolocation.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,OrderEmbossingDate,103)=dateadd(day,0,'" + ReportDate2 + "')  and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");
            }
            if (e.CommandName == "Day2")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                rtolocationname = rtolocation.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,OrderEmbossingDate,103)=dateadd(day,-1,'" + ReportDate2 + "')  and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");
            }
            if (e.CommandName == "Day3")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                rtolocationname = rtolocation.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,OrderEmbossingDate,103)=dateadd(day,-2,'" + ReportDate2 + "')  and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");
            }
            if (e.CommandName == "Day4")
            {

                GridViewRow oItem = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                int RowIndex = oItem.RowIndex;
                Label rtolocation = (Label)GridView1.Rows[RowIndex].FindControl("lblRtolocation");
                rtolocationname = rtolocation.Text.ToString();
                strSQL.Append("select convert(date,erpassigndate,103) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode,OrderStatus,challanno,challandate,RejectFlag,isnull(NewPdfRunningNo,PdfRunningNo) as NewPdfRunningNo,PdfDownloadDate,isnull(convert(varchar(15),aptgvehrecdate,103),convert(varchar(15),OrderDate,103)) aptgvehrecdate from hsrprecords a, rtolocation b  where a.rtolocationid=b.rtolocationid and  a.HSRP_StateID='" + state + "' and RTOLocationname='" + rtolocationname + "' and convert(date,OrderEmbossingDate,103)=dateadd(day,-3,'" + ReportDate2 + "')  and (convert(date,ChallanDate)>'" + ReportDate2 + "' or isnull(ChallanDate,'')='')");


            }

            string strVehicleType = string.Empty;

            //OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            ////string selectdate = OrderDatefrom.SelectedDate.AddDays(-4.00).ToString();
            //String[] StringAuthDate = OrderDatefrom.SelectedDate.ToString().Split('/');
            //String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];






            //strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID=11 and a.RTOLocationID='" + RTOCode + "' and convert(date,erpassigndate) ='" + OrderDatefrom.SelectedDate.ToString() + "' and isnull(newpdfrunningno,'')='' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null)  and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') ");


            //strSQL.Append("select convert(date,erpassigndate) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode from hsrprecords a, rtolocation b where a.rtolocationid=b.RTOLocationID and b.HSRP_StateID='" + state + "' and OrderStatus in ('New Order') and convert(date,erpassigndate) <='" + ReportDate2 + "' and embcentername='" + embcentername + "' and isnull(orderembossingdate,'')='' and isnull(vehicleregno,'')!=''  and isnull(hsrp_front_lasercode,'')!='' and  isnull(hsrp_front_lasercode,'')!='' order by 1,2,3");


            // strSQL.Append("Select a.hsrprecordID,a.roundoff_netamount,a.OrderStatus, a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.dealerid as ID, left(a.OwnerName,19) as OwnerName, a.MobileNo,(select  AffixCenterDesc from AffixationCenters where Affix_id= a.affix_id ) as AffixCenterDesc, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDateAuth, a.OrderDate, a.EngineNo, a.ChassisNo, (select rtolocationname from rtolocation where  rtolocationid =a.rtolocationid) as RTOLocationName,  a.VehicleRegNo,  case a.VehicleType when 'MCV/HCV/TRAILERS' then 'Trailers' when 'THREE WHEELER' then 'T.Whe.' when 'SCOOTER' then 'SCOO' when 'TRACTOR' then 'TRAC' when 'LMV(CLASS)' then 'L.CL' when 'LMV' then 'LMV'  when 'MOTOR CYCLE' then 'MO.C' end as VehicleType, case a.VehicleClass when 'Transport' then 'T' else 'N.T.' end as VehicleClass,  a.HSRP_StateID, (select HSRPStateName from hsrpstate where HSRP_StateID=a.HSRP_StateID)  as HSRPStateName, (select replace(ProductCode,'MM-','') from Product where productid= a.RearPlateSize) AS RearProductCode,(select replace(ProductCode,'MM-','') from Product where productid= a.FrontPlateSize) AS FrontProductCode,a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a   where  a.hsrp_StateID='" + strStateID + "' and a.RTOLocationID='" + RTOLocationID + "' and convert(date,hsrprecord_creationdate) between '2014-12-31 00:00:00' and '2015-02-19 23:59:59' and orderstatus='New Order' and ([HSRP_Front_LaserCode] is not null or [HSRP_Rear_LaserCode] is not null)   and ([HSRP_Front_LaserCode] !='' or [HSRP_Rear_LaserCode] !='') ");

            // strSQL.Append("select convert(date,erpassigndate) as erpassigndate,embcentername,rtolocationname,vehicleregno,hsrp_front_lasercode, hsrp_rear_lasercode from hsrprecords a, rtolocation b where a.rtolocationid=b.RTOLocationID and b.HSRP_StateID='" + state + "' and OrderStatus in ('New Order') and embcentername='" + embcentername + "' and isnull(orderembossingdate,'')='' and isnull(vehicleregno,'')!='' and isnull(hsrp_front_lasercode,'')!='' and  isnull(HSRP_Rear_LaserCode,'')!='' and isnull(erpassigndate,'')!='' and hsrprecordid not in (select hsrprecordid from hsrprecords a, rtolocation b where a.rtolocationid=b.rtolocationid and a.HSRP_StateID='" + state + "' and convert(date,erpassigndate,103)>'" + ReportDate2 + "' and OrderStatus='New Order' and embcentername='" + embcentername + "')");




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
                row.Cells.Add(new WorksheetCell("erpassigndate", "HeaderStyle"));
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
                row.Cells.Add(new WorksheetCell("Vehicle reg.rec date", "HeaderStyle"));

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
                        row.Cells.Add(new WorksheetCell(dtrows["aptgvehrecdate"].ToString(), DataType.String, "HeaderStyle1"));



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

        
    }
}
            
       