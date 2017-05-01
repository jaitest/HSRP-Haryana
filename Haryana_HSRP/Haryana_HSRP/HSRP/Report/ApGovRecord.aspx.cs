using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Globalization;

namespace HSRP.Report
{
    public partial class ApGovRecord : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;

        string SQLString1 = string.Empty;

        string recordtype = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;




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
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            Datefrom.SelectedDate = System.DateTime.Now;
                            Dateto.SelectedDate = System.DateTime.Now;


                        }
                        else
                        {
                            Datefrom.SelectedDate = System.DateTime.Now;
                            Dateto.SelectedDate = System.DateTime.Now;
                            InitialSetting();

                        }
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }


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


        protected void btnexport_Click(object sender, EventArgs e)
        {
            Lblerror.Visible = false;
            Lblerror.Text = "";
            SaveAndDownloadFile();


        }


        private void SaveAndDownloadFile()
        {
            try
            {

                Lblerror.Visible = false;
                Lblerror.Text = "";

                string filename = "ApGovReports" + System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Tg Gov Report";
                book.Properties.Created = DateTime.Now;


                // Add some styles to the Workbook
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



                Worksheet sheet = book.Worksheets.Add("Reports");
                sheet.Table.Columns.Add(new WorksheetColumn(150));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(150));

                sheet.Table.Columns.Add(new WorksheetColumn(150));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(92));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(90));


                WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                style9.Font.FontName = "Tahoma";
                style9.Font.Size = 10;
                style9.Font.Bold = true;
                style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style9.Interior.Color = "#FCF6AE";
                style9.Interior.Pattern = StyleInteriorPattern.Solid;


                WorksheetRow row = sheet.Table.Rows.Add();

                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));

                WorksheetCell cell = row.Cells.Add("Andhra Pradesh Assignment Report");
                cell.MergeAcross = 1; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                row = sheet.Table.Rows.Add();


                row.Index = 3;

                row.Cells.Add(new WorksheetCell("State :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Andhra Pradesh", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));


                WorksheetCell cells = row.Cells.Add("Report Generation Date :");
                //cells.MergeAcross = 1; // Merge two cells together
                cells.StyleID = "HeaderStyle2";
                string currentDate = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();
                row.Cells.Add(new WorksheetCell(currentDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 5;
                row.Cells.Add(new WorksheetCell("Add Record By", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Embossing Center Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Total", "HeaderStyle6"));


                row = sheet.Table.Rows.Add();
                row.Index = 6;
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(CnnString1);
                SqlCommand com = new SqlCommand("GetGovRecord", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@StateId", Convert.ToInt32(9));
                com.Parameters.AddWithValue("@ErpassignFromdate", Datefrom.SelectedDate);
                com.Parameters.AddWithValue("@ErpassignTodate", Dateto.SelectedDate);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                dt.Load(dr);
                con.Close();

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dtt in dt.Rows) // Loop over the rows.
                    {
                        row.Cells.Add(new WorksheetCell(dtt["addrecordby"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtt["EmbCenterName"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtt["vehicleclass"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtt["Total"].ToString(), DataType.String, "HeaderStyle"));

                        row = sheet.Table.Rows.Add();
                    }


                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    // Save the file and open it
                    book.Save(Response.OutputStream);

                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
                else
                {
                    Lblerror.Text = "";
                    Lblerror.Visible = true;
                    Lblerror.Text = "Records Not Available";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}