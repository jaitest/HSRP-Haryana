
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
using iTextSharp.text.pdf;
using System.Text;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class HRAffixationReport : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;

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
                    gridTD.Visible = false;
                    grd.Visible = false;
                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            DropDownListStateName.Visible = true;
                            FilldropDownListStateName();
                            Datefrom.SelectedDate = System.DateTime.Now;
                            Dateto.SelectedDate = System.DateTime.Now;
                        }
                        else
                        {
                            DropDownListStateName.Visible = true;
                            FilldropDownListStateName();
                            Datefrom.SelectedDate = System.DateTime.Now;
                            Dateto.SelectedDate = System.DateTime.Now;
                            //FilldropddlRTOLocation(HSRP_StateID1);
                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }

                   
                }
            }
        }

        private void FilldropDownListStateName()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=4 and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=4 and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }

        //private void FilldropddlRTOLocation(string StateID)
        //{
        //    if (UserType1.Equals(0))
        //    {
        //        SQLString1 = "Select RTOLocationID,RTOLocationName from RTOLocation where HSRP_StateID='" + StateID + "' and ActiveStatus='Y'";
        //        Utils.PopulateDropDownList(ddlRtoLocation, SQLString1.ToString(), CnnString1, "--Select Location--");
        //    }
        //    else
        //    {
        //        SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=" + StateID + " and ActiveStatus='Y' Order by HSRPStateName";
        //        DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
        //        DropDownListStateName.DataSource = dts;
        //        DropDownListStateName.DataBind();
        //    }
        //}

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            Datefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Datefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            Dateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Dateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
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

        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
              
                SqlConnection con = new SqlConnection(CnnString1);
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                //change sp Name
                cmd = new SqlCommand("HRCollection_TypeReport", con);//Business_ReportTypewisesummary_report

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
               // cmd.Parameters.Add(new SqlParameter("@ReportType", DropDownListReportType.SelectedValue));
               // cmd.Parameters.Add(new SqlParameter("@reportDate", Datefrom.SelectedDate));
                cmd.Parameters.Add(new SqlParameter("@reportTo", Dateto.SelectedDate));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                #endregion
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gridTD.Visible = true;
                    grd.DataSource = ds.Tables[0];
                    grd.DataBind();
                    grd.Visible = true;
                    DownloadDetailPanel.Visible = false;
                    if (ds.Tables.Count==2)
                    {

                        DownloadDetailPanel.Visible = true;
                        ddlLocation.DataSource = ds.Tables[1];
                        ddlLocation.DataTextField = "Location";
                        ddlLocation.DataValueField = "RTOLocationID";
                        ddlLocation.DataBind();
                        ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Location--", "0"));
                        ddlLocation.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "All"));
                        ddlUser.ClearSelection();
                        ddlUser.DataSource = "";
                        ddlUser.DataBind();
                        ddlUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select User--", "0"));
                        ddlUser.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "All"));

                       
                    }                    

                }
                else
                {
                    gridTD.Visible = false;
                    grd.DataSource = null;
                    grd.Visible = false;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DropDownListStateName_TextChanged(object sender, EventArgs e)
        {
            //FilldropddlRTOLocation(DropDownListStateName.SelectedValue);
            gridTD.Visible = false;
            grd.DataSource = null;
            grd.Visible = false;
        }
        int icount = 0;
        private void SaveAndDownloadFile()
        {
            Workbook book = new Workbook();
            string filename = "Summary report on Type Wise" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(strOrderType, book, 1, "HRCollection_TypeReport");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }
        private void SaveAndDownloadFile2()
        {
            Workbook book = new Workbook();
            string filename = "Detail report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;

            Export(strOrderType, book, 1, "HRCollection_TypeReport");

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            // Save the file and open it
            book.Save(Response.OutputStream);
            context.Response.ContentType = "application/vnd.ms-excel";

            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.End();

        }
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
                book.Properties.Title = "Summary report on Type Wise";
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

                //string strreptype = "E";
                #region Fetch Data
                DataSet ds = new DataSet();

                SqlCommand cmd = new SqlCommand();
                string strParameter = string.Empty;
                cmd = new SqlCommand(strProcName, con);


                cmd.CommandType = CommandType.StoredProcedure;
                if (btndownload1 != true)
                {
                    cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));                  
                   // cmd.Parameters.Add(new SqlParameter("@reportDate", Datefrom.SelectedDate));
                    cmd.Parameters.Add(new SqlParameter("@reportTo", Dateto.SelectedDate));                    
                }               
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
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle2"));
                    WorksheetCell cell = row.Cells.Add("Summary report on Type Wise");
                    cell.MergeAcross = 4; // Merge two cells together
                    cell.StyleID = "HeaderStyle2";

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    AddNewCell(row, "State:", "HeaderStyle2", 1);
                    AddNewCell(row, DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2", 1);
                    AddNewCell(row, "Report Type:", "HeaderStyle2", 1);
                   // AddNewCell(row, DropDownListReportType.SelectedItem.ToString(), "HeaderStyle2", 1);
                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());


                    AddNewCell(row, "Report Date from:", "HeaderStyle2", 1);
                    AddNewCell(row, Datefrom.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

                    AddNewCell(row, "Report Date To:", "HeaderStyle2", 1);
                    AddNewCell(row, Dateto.SelectedDate.ToString("dd/MMM/yyyy"), "HeaderStyle2", 1);

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
                }
                else
                {
                    Label1.Visible = false;
                    Label1.Text = "Record not Found";
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

        protected void btn_download_Click(object sender, EventArgs e)
        {
            #region Fetch Data
            SqlConnection con = new SqlConnection(CnnString1);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("HRCollection_TypeReport", con);


            cmd.CommandType = CommandType.StoredProcedure;
            if (btndownload1 != true)
            {
                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportTo", Dateto.SelectedDate));
            }
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            #endregion
            pdf(dt);
        }
        bool btndownload1=false;
        protected void btndownloadDetail_Click(object sender, EventArgs e)
        {
            btndownload1 = true;
            this.SaveAndDownloadFile2();
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUser.ClearSelection();
            if (ddlLocation.SelectedValue != "All")
            {
                string SQLQuery = "select distinct a.CreatedBy as UserID,(select userfirstname+''+UserLastName from users where userid=CreatedBy) Users from [dbo].[HSRPRecords] a,rtolocation b where a.hsrp_stateid='" + DropDownListStateName.SelectedValue + "'  and a.RTOLocationID=b.RTOLocationID and a.RTOLocationID='" + ddlLocation.SelectedValue + "'	and convert(date,HSRPRecord_CreationDate) between convert(date,'" + Datefrom.SelectedDate + "') and convert(date,'" + Dateto.SelectedDate + "') and a.HSRP_StateID='" + DropDownListStateName.SelectedValue + "'	and CreatedBy in (select USERID from HRAC_master) 	group by CreatedBy,rtolocationname,a.rtolocationid	order by 1";
                DataTable dt = new DataTable();
                dt = Utils.GetDataTable(SQLQuery, CnnString1);
                ddlUser.DataSource = dt;
                ddlUser.DataTextField = "Users";
                ddlUser.DataValueField = "UserID";
                ddlUser.DataBind();
                ddlUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select User--", "0"));
                ddlUser.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "All"));
            }

        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                LinkButton lnkView = (LinkButton)e.CommandSource;
                string RTOLocationID = lnkView.CommandArgument;
                if (RTOLocationID != "")
                {
                    Session["RTOLocationCode"] = RTOLocationID;
                   // Session["formdate"] = Datefrom.SelectedDate;
                    Session["todate"] = Dateto.SelectedDate;
                    Response.Redirect("HRAffixation_Detail_Report.aspx");
                    
                }
            }
        }

       

        public void PDFDisplay()
        {

            #region Fetch Data
            SqlConnection con = new SqlConnection(CnnString1);
            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("HRCollection_TypeReport", con);


            cmd.CommandType = CommandType.StoredProcedure;
            if (btndownload1 != true)
            {
                cmd.Parameters.Add(new SqlParameter("@StateId", DropDownListStateName.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@reportTo", Dateto.SelectedDate));
            }
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            #endregion

            // DataTable dt = Utils.GetDataTable(strSQL.ToString(), CnnString);
            if (dt.Rows.Count > 0)
            {
                float Amount = 0;
                string filename = "HSRPProductionSheet- " + RTOLocationID1 + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                StringBuilder bb = new StringBuilder();
                // Document document = new Document();
                //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                Document document = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                // Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                //Opens the document:
                document.Open();

                PdfPTable table;

                table = new PdfPTable(14);
                var colWidthPercentages = new[] { 55f, 125f, 55f, 200f, 75f, 165f, 165f, 155f, 110f, 210f, 105f, 210f, 105f, 125f };//7,14
                var colheightpercentage = new[] { 2f };

                table.SetWidths(colWidthPercentages);


                string strQueryDate = "SELECT CONVERT(VARCHAR,GETDATE(),103)";
                // DataTable dtDate = Utils.GetDataTable(strQueryDate, CnnString);

                table.TotalWidth = 800f;
                table.LockedWidth = true;





                PdfPCell cell1209111 = new PdfPCell(new Phrase("Report Generation Date: " + Convert.ToDateTime(Dateto).ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209111.Colspan = 6;
                cell1209111.BorderWidthLeft = 0f;
                cell1209111.BorderWidthRight = 0f;
                cell1209111.BorderWidthTop = 0f;
                cell1209111.BorderWidthBottom = 0f;
                //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1209111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209111);
              
                PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + DropDownListStateName.SelectedValue.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 6;
                cell12091.BorderWidthLeft = 1f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 1f;
                cell12091.BorderWidthBottom = 0f;
                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                PdfPCell cell120913 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell120913.Colspan = 6;
                cell120913.BorderWidthLeft = 1f;
                cell120913.BorderWidthRight = 0f;
                cell120913.BorderWidthTop = 0f;//1
                cell120913.BorderWidthBottom = 0f;
                cell120913.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120913);

                PdfPCell cell12095 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12095.Colspan = 4;//7
                cell12095.BorderWidthLeft = 0f;
                cell12095.BorderWidthRight = 0f;//1
                cell12095.BorderWidthTop = 0f;
                cell12095.BorderWidthBottom = 0f;

                cell12095.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12095);
                PdfPCell cell12092 = new PdfPCell(new Phrase("RTOLocation Name : " + dt.Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12092.Colspan = 6;//7
                cell12092.BorderWidthLeft = 1f;
                cell12092.BorderWidthRight = 0f;
                cell12092.BorderWidthTop = 0f;
                cell12092.BorderWidthBottom = 0f;

                cell12092.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12092);
                PdfPCell cell12094 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12094.Colspan = 4;//3
                cell12094.BorderWidthLeft = 0f;
                cell12094.BorderWidthRight = 0f;
                cell12094.BorderWidthTop = 0f;
                cell12094.BorderWidthBottom = 0f;

                cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12094);


                PdfPCell cell12 = new PdfPCell(new Phrase("SR.No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 1;
                cell12.BorderWidthLeft = 1f;
                cell12.BorderWidthRight = .8f;
                cell12.BorderWidthTop = 0.8f;
                cell12.BorderWidthBottom = 0.8f;
                cell12.FixedHeight = -1;
                cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1210 = new PdfPCell(new Phrase("RtoLocationName", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1210.Colspan = 1;
                cell1210.BorderWidthLeft = 0f;
                cell1210.BorderWidthRight = .8f;
                cell1210.BorderWidthTop = 0.8f;
                cell1210.BorderWidthBottom = 0.8f;

                cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1210);

                PdfPCell cell1213 = new PdfPCell(new Phrase("RTOLocationCode", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 0.8f;
                cell1213.BorderWidthBottom = 0.8f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);


                PdfPCell cell1209 = new PdfPCell(new Phrase("Count", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 0f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = 0.8f;
                cell1209.BorderWidthBottom = 0.8f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);

                PdfPCell cell12233 = new PdfPCell(new Phrase("Amount", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 0.8f;
                cell12233.BorderWidthBottom = 0.8f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                int j = 0;
                int total = 0;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                  //  j = j + 1;

                    //=========================================================================================================================================
                    //if (total == 44)
                    //{
                        

                       

                        PdfPCell cell12a = new PdfPCell(new Phrase("SR.No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell12a.Colspan = 1;
                        cell12a.BorderWidthLeft = 1f;
                        cell12a.BorderWidthRight = .8f;
                        cell12a.BorderWidthTop = 0.8f;
                        cell12a.BorderWidthBottom = 0.8f;

                        cell12a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12a);

                        PdfPCell cell1210a = new PdfPCell(new Phrase("RtoLocationName :" + dt.Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1210a.Colspan = 1;
                        cell1210a.BorderWidthLeft = 0f;
                        cell1210a.BorderWidthRight = .8f;
                        cell1210a.BorderWidthTop = 0.8f;
                        cell1210a.BorderWidthBottom = 0.8f;
                        cell1210a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1210a);


                        PdfPCell cell1213a = new PdfPCell(new Phrase("RTOLocationCode :" + dt.Rows[0]["RTOLocationCode"].ToString(), new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1213a.Colspan = 1;
                        cell1213a.BorderWidthLeft = 0f;
                        cell1213a.BorderWidthRight = .8f;
                        cell1213a.BorderWidthTop = 0.8f;
                        cell1213a.BorderWidthBottom = 0.8f;
                        cell1213a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1213a);


                        PdfPCell cell1209a = new PdfPCell(new Phrase("Count :" + dt.Rows[0]["Count"].ToString(), new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                        cell1209a.Colspan = 1;
                        cell1209a.BorderWidthLeft = 0f;
                        cell1209a.BorderWidthRight = .8f;
                        cell1209a.BorderWidthTop = 0.8f;
                        cell1209a.BorderWidthBottom = 0.8f;
                        cell1209a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1209a);

                        PdfPCell cell12233a = new PdfPCell(new Phrase("Amount :" + dt.Rows[0]["Amount"].ToString(), new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell12233a.Colspan = 1;
                        cell12233a.BorderWidthLeft = 0f;
                        cell12233a.BorderWidthRight = .8f;
                        cell12233a.BorderWidthTop = 0.8f;
                        cell12233a.BorderWidthBottom = 0.8f;
                        cell12233a.HorizontalAlignment = 0;
                        //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell12233a);
                       
                    }
                document.Add(table);
                document.Close();
                HttpContext context = HttpContext.Current;

                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();

                }


            //}
        }

        public void pdf(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                float Amount = 0;
                string filename = "HSRPProductionSheet- "+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                StringBuilder bb = new StringBuilder();
                // Document document = new Document();
                //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                Document document = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                // Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                //Opens the document:
                document.Open();

                PdfPTable table;

                table = new PdfPTable(5);
                var colWidthPercentages = new[] { 55f, 125f, 100f, 80f, 75f };
                var colheightpercentage = new[] { 2f };

                table.SetWidths(colWidthPercentages);


                string strQueryDate = "SELECT CONVERT(VARCHAR,GETDATE(),103)";
                DataTable dtDate = Utils.GetDataTable(strQueryDate, CnnString1);

                table.TotalWidth = 800f;
                table.LockedWidth = true;
                PdfPCell cell786a = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell786a.Colspan = 5;
                cell786a.BorderWidthLeft = 0f;
                cell786a.BorderWidthRight = 0f;
                cell786a.BorderWidthTop = 0f;
                cell786a.BorderWidthBottom = 0f;
                //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                cell786a.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell786a);

                PdfPCell cell120911 = new PdfPCell(new Phrase("HR Cash Collection Report", new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 4;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;
                //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                cell120911.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell1209111 = new PdfPCell(new Phrase("Report Generation Date: " + dtDate.Rows[0][0].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209111.Colspan = 3;
                cell1209111.BorderWidthLeft = 0f;
                cell1209111.BorderWidthRight = 0f;
                cell1209111.BorderWidthTop = 0f;
                cell1209111.BorderWidthBottom = 0f;
                //cell120911.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1209111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209111);

               

                PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + DropDownListStateName.SelectedItem.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 3;
                cell12091.BorderWidthLeft = 1f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 1f;
                cell12091.BorderWidthBottom = 0f;
                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date " + Dateto.SelectedDate.ToString("MM-dd-yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 2;
                cell12093.BorderWidthLeft = 0f;
                cell12093.BorderWidthRight = 0f;
                cell12093.BorderWidthTop = 1f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);

                PdfPCell cell12 = new PdfPCell(new Phrase("SR.No", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 1;
                cell12.BorderWidthLeft = 1f;
                cell12.BorderWidthRight = .8f;
                cell12.BorderWidthTop = 0.8f;
                cell12.BorderWidthBottom = 0.8f;
                cell12.FixedHeight = -1;
                cell12.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1210 = new PdfPCell(new Phrase("RtoLocationName", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1210.Colspan = 1;
                cell1210.BorderWidthLeft = 0f;
                cell1210.BorderWidthRight = .8f;
                cell1210.BorderWidthTop = 0.8f;
                cell1210.BorderWidthBottom = 0.8f;

                cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1210);

                PdfPCell cell1213 = new PdfPCell(new Phrase("RTOLocationCode", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 0.8f;
                cell1213.BorderWidthBottom = 0.8f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);


                PdfPCell cell1209 = new PdfPCell(new Phrase("Count", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK))); //6
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 0f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = 0.8f;
                cell1209.BorderWidthBottom = 0.8f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);

                PdfPCell cell12233 = new PdfPCell(new Phrase("Amount", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 0.8f;
                cell12233.BorderWidthBottom = 0.8f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);
                
                int j = 0;
                int total = 0;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    total = total + 1;
                    //============================================================ ajay end ======================================================================
                    PdfPCell cell13 = new PdfPCell(new Phrase(""+ dt.Rows[i]["S.NO"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell13.Colspan = 1;
                    cell13.BorderWidthLeft = 0.8f;
                    cell13.BorderWidthRight = 0f;
                    cell13.BorderWidthTop = 0f;
                    cell13.BorderWidthBottom = .5f;
                    cell13.MinimumHeight = 0f;//25
                    cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell13);

                    PdfPCell cell1212 = new PdfPCell(new Phrase(dt.Rows[i]["RtoLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1212.Colspan = 1;
                    cell1212.MinimumHeight = 0f;//25

                    cell1212.BorderWidthLeft = 0.8f;
                    cell1212.BorderWidthRight = .8f;
                    cell1212.BorderWidthTop = 0f;
                    cell1212.BorderWidthBottom = .5f;

                    cell1212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1212);



                    PdfPCell cell1214 = new PdfPCell(new Phrase(dt.Rows[i]["RTOLocationCode"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1214.Colspan = 1;
                    cell1214.MinimumHeight = 0f;//25

                    cell1214.BorderWidthLeft = 0f;
                    cell1214.BorderWidthRight = .8f;
                    cell1214.BorderWidthTop = 0f;
                    cell1214.BorderWidthBottom = .5f;
                    cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1214);


                    PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Rows[i]["Count"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK))); //6
                    cell1211.Colspan = 1;
                    cell1211.MinimumHeight = 0f;//25

                    cell1211.BorderWidthLeft = 0f;
                    cell1211.BorderWidthRight = 0.8f;
                    cell1211.BorderWidthTop = 0f;
                    cell1211.BorderWidthBottom = .5f;
                    cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1211);



                    PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Rows[i]["Amount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1219.Colspan = 1;
                    cell1219.MinimumHeight = 0f;//25
                    cell1219.BorderWidthLeft = 0f;
                    cell1219.BorderWidthRight = .8f;
                    cell1219.BorderWidthTop = 0f;
                    cell1219.BorderWidthBottom = .5f;
                    cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    table.AddCell(cell1219);
                   
                }

                try
                {                   
                    PdfPCell cell1209340 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1209340.Colspan = 16;
                    cell1209340.BorderWidthLeft = .8f;
                    cell1209340.BorderWidthRight = .8f;
                    cell1209340.BorderWidthTop = .8f;
                    cell1209340.BorderWidthBottom = .5f;
                    cell1209340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1209340);

                    PdfPCell cell12241 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12241.Colspan = 7;
                    cell12241.BorderWidthLeft = 0f;
                    cell12241.BorderWidthRight = 0f;
                    cell12241.BorderWidthTop = 0f;
                    cell12241.BorderWidthBottom = 0f;
                    cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12241);
                    document.Add(table);
                    document.Close();

                    HttpContext context = HttpContext.Current;
                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();

                }
                catch
                {

                }
            }
            else
            {
                //lblErrMsg.Text = "No Record Found!!";
            }
        }

    }
}