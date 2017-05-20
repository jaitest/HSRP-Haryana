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
    public partial class ZonewiseReport : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
       
        string SQLString1 = string.Empty;
       
        string recordtype = string.Empty;
        string day1date1 = string.Empty;
        string day1date = string.Empty;
        string day2date = string.Empty;
        string day3date = string.Empty;
        string day4date = string.Empty;
        string day5date = string.Empty;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtsession = new DataTable();

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
                           FilldropDownZone();
                           InitialSetting();
                           BindReportTypeddl();
                        }
                        else
                        {
                            FilldropDownZone();
                            InitialSetting();
                            BindReportTypeddl();
                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        private void FilldropDownZone()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select distinct zone from rtolocation where hsrp_stateid = 4  and isnull(zone,'')!='' and  ActiveStatus='Y' Order by zone";
                Utils.PopulateDropDownList(DropDownListzone, SQLString1.ToString(), CnnString1, "--Select Zone--");
            }
            else
            {
                SQLString1 = "SELECT  zone FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" +strUserID1 + "' and ActiveStatus!='N' order by zone ";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListzone.DataSource = dts;
                DropDownListzone.DataBind();
            }
        }

        //private void FilldropDownListRtolocation()
        //{
        //    if (UserType1.Equals(0))
        //    {
        //        SQLString1 = "select RTOLocationID, RTOLocationName from rtolocation where hsrp_stateid = 4  and isnull(zone,'')!='' and  ActiveStatus='Y'  and zone='" + DropDownListzone.SelectedValue.ToString() + "' Order by  RTOLocationName";
        //        Utils.PopulateDropDownList(DropDownListRtolocation, SQLString1.ToString(), CnnString1, "--Select RtoLocation--");
        //    }
        //    else
        //    {
        //        SQLString1 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName,  a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.zone ='" + DropDownListzone.SelectedValue.ToString() + "'  and ActiveStatus!='N' order by RTOLocationName ";
        //        DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
        //        DropDownListRtolocation.DataSource = dts;
        //        DropDownListRtolocation.DataBind();
        //    }
        //}

        //private void FilldropDownListDealer()
        //{
        //    //if (UserType1.Equals(0))
        //    //{
        //        SQLString1 = "select Dealerid,DealerName  from dealermaster  where hsrp_stateid = 4 and RTOLocationID='" + DropDownListRtolocation.SelectedValue.ToString()+ "'  Order by DealerName";
        //        Utils.PopulateDropDownList(DropDownListDealer, SQLString1.ToString(), CnnString1, "--Select Dealer Name--");
        //    //}
        //    //else
        //    //{
        //    //    SQLString1 = "select Dealerid,DealerName  from dealermaster  where hsrp_stateid = 4 and RTOLocationID='" + DropDownListRtolocation.SelectedValue.ToString() + "'  Order by DealerName";
        //    //    DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
        //    //    DropDownListDealer.DataSource = dts;
        //    //    DropDownListDealer.DataBind();
        //    //}
        //}

        private void BindReportTypeddl()
        {
            string sqlstring = "[Business_ReportTypewisesummary_onlinereport_zonewise]   '', '" + Datefrom.SelectedDate + "','" + Dateto.SelectedDate + "','DB','4','0','0','0' ";
           DataTable  dt11 = new DataTable();
            dt11 = Utils.GetDataTable(sqlstring, CnnString1);
            DdlReportType.DataSource = dt11;
            DdlReportType.DataTextField = "ReportType";
            DdlReportType.DataValueField = "Code";
            DdlReportType.DataBind();
            DdlReportType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Report Type--", "--Select Report Type--"));
        }


        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MinDate = 2017 + "-" + 02+ "-" + 01;


            Datefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Datefrom.MinDate = DateTime.Parse(MinDate);
            Datefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            Dateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Dateto.MinDate = DateTime.Parse(MinDate);
            Dateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }
    
       

        protected void btnexport_Click(object sender, EventArgs e)
        {
            Lblerror.Visible = false;
            if (DropDownListzone.SelectedItem.Text.ToString() == "--Select Zone--")
            {
                Lblerror.Visible = true;
                Lblerror.Text = "Please Select Zone.";
                return;
            }


            //if (DropDownListRtolocation.SelectedItem.Text.ToString() == "--Select RtoLocation--")
            //{
            //    Lblerror.Visible = true;
            //    Lblerror.Text = "Please Select Rtolocation .";
            //    return;
            //}
          
            //if (DropDownListDealer.SelectedItem.Text.ToString() == "--Select Dealer Name--")
            //{
            //    Lblerror.Visible = true;
            //    Lblerror.Text = "Please Dealer Name .";
            //    return;
            //}

            this.SaveAndDownloadFile();

           

          
        }


        private void SaveAndDownloadFile()
        {
            Lblerror.Text = String.Empty;
          

            Workbook book = new Workbook();
            string filename = "Summary report on Zone Wise" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";

            string strOrderType = string.Empty;


            Export(strOrderType, book, 1, "Business_ReportTypewisesummary_onlinereport_zonewise");

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
                book.Properties.Title = "Haryana  Zone Wise Report";
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
              

                cmd.Parameters.Add(new SqlParameter("@zone", DropDownListzone.SelectedValue.ToString()));
                cmd.Parameters.Add(new SqlParameter("@reportdate", Convert.ToDateTime(Datefrom.SelectedDate)));
                cmd.Parameters.Add(new SqlParameter("@reportto", Convert.ToDateTime(Dateto.SelectedDate)));
                cmd.Parameters.Add(new SqlParameter("@reporttype", DdlReportType.SelectedValue));                     
                cmd.Parameters.Add(new SqlParameter("@stateid",  Convert.ToInt32(4)));
                cmd.Parameters.Add(new SqlParameter("@RTOLocationId", Convert.ToInt32(Session["UserRTOLocationID"])));
                cmd.Parameters.Add(new SqlParameter("@paymentgateway",""));
                cmd.Parameters.Add(new SqlParameter("@dealerid", ""));
                //DropDownListDealer.SelectedValue.ToString()
                     

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //  dt = new DataTable();
                da.Fill(dt);
                #endregion

                if (dt.Rows.Count > 0)
                {
                    AddColumnToSheet(sheet, 100, dt.Columns.Count);
                    int iIndex = 3;
                    WorksheetRow row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    row.Cells.Add(new WorksheetCell("Report :   Haryana Zone Wise Report - ", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DdlReportType.SelectedItem.Text.ToString().Trim(), "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    row.Index = iIndex++;
                    AddNewCell(row, "Report Date from:", "HeaderStyle2", 1);
                    AddNewCell(row, Datefrom.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2", 1);

                    AddNewCell(row, "Report Date To:", "HeaderStyle2", 1);
                    AddNewCell(row, Dateto.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2", 1);

                    row.Cells.Add(new WorksheetCell("Report Generated Date:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), "HeaderStyle2"));
                    AddNewCell(row, "", "HeaderStyle2", 2);
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
                            if (dt.Rows[j][i].ToString().Trim() == "99999999")
                            {
                                AddNewCell(row, "Total", "HeaderStyle6", 1);
                            }
                            else
                            {
                                AddNewCell(row, dt.Rows[j][i].ToString(), "HeaderStyle6", 1);
                            }

                        }
                        row = sheet.Table.Rows.Add();

                    }
                }
                else
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = "Record not Found";
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


        protected void DropDownListzone_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (DropDownListzone.SelectedItem.Text.ToString() == "--Select Zone--")
            //{
            //    Lblerror.Visible = true;
            //    Lblerror.Text = "Please Select Zone";
            //    return;
            //}

            //else
            //{
            //    Lblerror.Visible = false;
            //    FilldropDownListRtolocation();
            //}
           

        }

        //protected void DropDownListRtolocation_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DropDownListRtolocation.SelectedItem.Text.ToString() == "--Select RtoLocation--")
        //    {
        //        Lblerror.Visible = true;
        //        Lblerror.Text = "Please Select Rtolocation.";
        //        return;
        //    }

        //    else
        //    {
        //        Lblerror.Visible = false;
        //        FilldropDownListDealer();
        //    }
           
        //}

        //protected void DropDownListDealer_SelectedIndexChanged(object sender, EventArgs e)
        //{
          
        //}

    
    }
}