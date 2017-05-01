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


namespace HSRP.BusinessReports
{
    public partial class ApTgReport : System.Web.UI.Page
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
                            FilldropDownList();

                        }
                        else
                        {
                            FilldropDownList();


                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        private void FilldropDownList()
        {

            SqlConnection con = new SqlConnection(CnnString1);

            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID in(9,11) and  ActiveStatus='Y' Order by HSRPStateName", con);//Business_ReportTypewisesummary_report             
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DropDownListStateName.DataSource = ds.Tables[0];
            DropDownListStateName.DataTextField = ds.Tables[0].Columns[0].ToString();
            DropDownListStateName.DataValueField = ds.Tables[0].Columns[1].ToString();

            DropDownListStateName.DataBind();
            DropDownListStateName.Items.Insert(0, new ListItem("--Select State--", "--Select State--"));







        }




        protected void btnexport_Click(object sender, EventArgs e)
        {

            SaveAndDownloadFile();
        }


        private void SaveAndDownloadFile()
        {
            try
            {
                int i = Convert.ToInt32(DropDownListStateName.SelectedValue);

                string filename = "ApTgReports" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Ap Tg Report";
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
               
                WorksheetCell cell = row.Cells.Add("AP/ TG Report");
                cell.MergeAcross = 2; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                row = sheet.Table.Rows.Add();
             

                row.Index = 3;
              
                row.Cells.Add(new WorksheetCell("State :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));


                WorksheetCell cells = row.Cells.Add("Report Generation Date :");
               // cells.MergeAcross = 1; // Merge two cells together
                cells.StyleID = "HeaderStyle2";
                string currentDate = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();

                row.Cells.Add(new WorksheetCell(currentDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
              



               


                row.Index = 5;

                row.Cells.Add(new WorksheetCell("HSRPRecord Authorization No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Payment Gateway", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OnlinePayment ID", "HeaderStyle6"));

                row = sheet.Table.Rows.Add();

                row.Index = 6;
                if (i == 9)
                {
                    SQLString = "select HSRPRecord_AuthorizationNo,convert(date,HSRPRecord_CreationDate) as Date ,PaymentGateway,VehicleRegNo,OnlinePaymentID from APOnlinePayment where HSRP_StateID = 9 and OnlinePaymentStatus='Y' and HSRPRecord_AuthorizationNo not in(select HSRPRecord_Authorizationno from hsrprecords where HSRPRecord_AuthorizationNo in (select HSRPRecord_AuthorizationNo from APOnlinePayment where HSRP_StateID=9 and OnlinePaymentStatus='Y'))and HSRPRecord_AuthorizationNo not in ('AP31/1000001270/2016/HS','AP31/1000001313/2016/HS','AP31/1000001334/2016/HS','AP31/1000001345/2016/HS','AP31/1000001251/2016/HS')order by 2";

                }
                else
                {
                    SQLString = "select HSRPRecord_AuthorizationNo,convert(date,HSRPRecord_CreationDate) as Date ,PaymentGateway,VehicleRegNo,OnlinePaymentID from tgOnlinePayment where HSRP_StateID= 11 and OnlinePaymentStatus='Y' and HSRPRecord_AuthorizationNo not in(select HSRPRecord_Authorizationno from hsrprecords where HSRPRecord_AuthorizationNo in (select HSRPRecord_AuthorizationNo from tgOnlinePayment where HSRP_StateID=11 and OnlinePaymentStatus='Y'))order by 2";

                }
                 DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dtt in dt.Rows) // Loop over the rows.
                    {
                        row.Cells.Add(new WorksheetCell(dtt["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtt["Date"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtt["PaymentGateway"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtt["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtt["OnlinePaymentID"].ToString(), DataType.String, "HeaderStyle"));

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
                    Lblerror.Text = "No Records Available";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}