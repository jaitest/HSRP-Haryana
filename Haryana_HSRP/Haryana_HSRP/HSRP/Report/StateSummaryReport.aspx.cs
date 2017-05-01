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
    public partial class StateSummaryReport : System.Web.UI.Page
    {
        int UserType;
        string CnnString = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string strUserID = string.Empty;
        int intHSRPStateID;
        int intRTOLocationID;
        string SQLString = string.Empty;

        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();

                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType.Equals(0))
                        {
                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                         
                            FilldropDownListOrganization();
                        
                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            OrderDateto.SelectedDate = System.DateTime.Now;


                        }
                        else
                        {

                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                           
                            FilldropDownListOrganization();
                       

                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            OrderDateto.SelectedDate = System.DateTime.Now;

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
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            OrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }

        protected void btngetdata_Click(object sender, EventArgs e)
        {
            if (DropDownListStateName.SelectedItem.Text != "--Select State--")
               {
                //Label1.Visible = false;
                BuildGrid();

               }

          
           else
              {
                    //Label1.Visible = false;
                    DataGrid1.Items.Clear();

                    return;
              }
       }


        public void BuildGrid()
        {
            try
            {


                String[] StringAuthDate = OrderDateto.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "-" + StringAuthDate[1] + "-" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = OrderDatefrom.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String ReportDate3 = StringOrderDate[0] + "-" + StringOrderDate[1] + "-" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate3 + " 00:00:00";


                SQLString = "select rtolocation.Rtolocationname as rtolocation,"
                            +"SUM(CASE WHEN  (VehicleType='MOTOR CYCLE' or VehicleType='SCOOTER') AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order') THEN 1 ELSE 0 END ) AS [c2W],"
                            +"SUM(CASE WHEN  (VehicleType ='THREE WHEELER') AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order') THEN 1 ELSE 0 END ) AS [c3W],"
                            +"SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType ='LMV(CLASS)' OR VehicleType ='TRACTOR') AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order') THEN 1 ELSE 0 END ) AS [c4W],"
                            +"SUM(CASE WHEN  (VehicleType = 'MCV/HCV/TRAILERS' )AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order')THEN 1 ELSE 0 END ) AS cComm,"
                            +"Sum(CASE WHEN  ((VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER')or (VehicleType ='THREE WHEELER')or (VehicleType = 'LMV' OR VehicleType ='LMV(CLASS)' or VehicleType ='TRACTOR')or (VehicleType = 'MCV/HCV/TRAILERS')) AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order') THEN 1 ELSE 0 END) as cTotal,"

                            +"sum(CASE WHEN  (VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER') AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close')THEN 1 else 0 END ) AS [a2W],"
                            +"sum(CASE WHEN  (VehicleType ='THREE WHEELER') AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close') THEN 1 else 0 END ) AS [a3W],"
                            +"sum(CASE WHEN  (VehicleType = 'LMV' OR VehicleType ='LMV(CLASS)') AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close') THEN 1 else 0 END ) AS [a4W],"
                            +"sum(CASE WHEN  (VehicleType = 'MCV/HCV/TRAILERS' or VehicleType ='TRACTOR') AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close') THEN 1 else 0 END ) AS aComm,"
                            +"sum(CASE WHEN  ((VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER' or VehicleType ='THREE WHEELER' or VehicleType = 'LMV' or VehicleType ='LMV(CLASS)' or VehicleType = 'MCV/HCV/TRAILERS'or VehicleType ='TRACTOR')) AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close') THEN 1 else 0  END) as aTotal,"

                            +"sum(CASE WHEN  (VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER') AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0 END ) AS [s2W],"
                            +"sum(CASE WHEN  (VehicleType ='THREE WHEELER') AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0 END ) AS [s3W],"
                            +"sum(CASE WHEN  (VehicleType = 'LMV' OR VehicleType ='LMV(CLASS)')  AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0 END ) AS [s4W],"
                            +"sum(CASE WHEN  (VehicleType = 'MCV/HCV/TRAILERS' or VehicleType ='TRACTOR') AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0 END ) AS sComm,"
                            +"sum(CASE WHEN  ((VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER' or VehicleType ='THREE WHEELER' or VehicleType = 'LMV' or VehicleType ='LMV(CLASS)' or VehicleType = 'MCV/HCV/TRAILERS'or VehicleType ='TRACTOR')) AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0  END) as sTotal "

                            +"from hsrprecords inner join  Rtolocation on hsrprecords.rtolocationid=rtolocation.rtolocationid "
                            + "where hsrprecords.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' "
                            +"GROUP BY  rtolocation.rtolocationname order by rtolocation.rtolocationname";

               

                dt = Utils.GetDataTable(SQLString, CnnString);

                DataGrid1.DataSource = dt;
                DataGrid1.SearchOnKeyPress = true;

                DataGrid1.DataBind();




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
                {



                    String[] StringAuthDate = OrderDateto.SelectedDate.ToString().Split('/');
                    String ReportDateEnd = StringAuthDate[0] + "-" + StringAuthDate[1] + "-" + StringAuthDate[2].Split(' ')[0];
                    string ReportDate2 = ReportDateEnd + " 23:59:59";
                    String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                    String[] StringOrderDate = OrderDatefrom.SelectedDate.ToString().Split('/');
                    string Mon = ("0" + StringOrderDate[0]);
                    string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                    String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                    String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                    String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                    String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                    String ReportDate3 = StringOrderDate[0] + "-" + StringOrderDate[1] + "-" + StringOrderDate[2].Split(' ')[0];
                    string ReportDate1 = ReportDate3 + " 00:00:00";

                    DateTime StartDate = Convert.ToDateTime(OrderDatefrom.SelectedDate);
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                   
                    //int.TryParse(dropDownListUser.SelectedValue, out intUserID);

                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    
                    //int.TryParse(dropDownListUser.SelectedValue, out UserID);

                    string filename = "State_Summary_Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "State Summary Report";
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

                    WorksheetStyle style21 = book.Styles.Add("HeaderStyle21");
                    style21.Font.FontName = "Tahoma";
                    style21.Font.Size = 12;
                    style21.Font.Bold = true;
                    style21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style21.Interior.Color = "#FCF6AE";


                    WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                    style3.Font.FontName = "Tahoma";
                    style3.Font.Size = 12;
                    style3.Font.Bold = true;
                    style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    
                    DataTable GetAddress;
                    string Address;
                    GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + DropDownListStateName.SelectedValue + "'", CnnString);

                    Worksheet sheet = book.Worksheets.Add("State Summary Report");
                    sheet.Table.Columns.Add(new WorksheetColumn(60));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));

                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(92));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
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
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("State Summary Report");
                    cell.MergeAcross = 3; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                    //row = sheet.Table.Rows.Add();
                    //row.Index = 4;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  

                    row = sheet.Table.Rows.Add();
                    row.Index = 4;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Date Generated from:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DatePrint1, "HeaderStyle2"));
                    //row = sheet.Table.Rows.Add();
                    //row.Index = 6;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    row.Cells.Add(new WorksheetCell("To:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DatePrint2, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 5;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                  
                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));

                    //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
                    WorksheetCell cell1 = row.Cells.Add("Total Colletion Till: "+DatePrint2);
                    cell1.MergeAcross = 4; // Merge two cells together
                    cell1.StyleID = "HeaderStyle21";
                    WorksheetCell cell12 = row.Cells.Add("Total Affixation Till: " + DatePrint2);   
                    cell12.MergeAcross = 4; // Merge two cells together
                    cell12.StyleID = "HeaderStyle21";
                    WorksheetCell cell13 = row.Cells.Add("Stok In Hand Till: "+ReportDate2);
                    cell13.MergeAcross = 4; // Merge two cells together
                    cell13.StyleID = "HeaderStyle21";
                    row = sheet.Table.Rows.Add();
                    row.Index = 7;
                  
                  
                    row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("2W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("3W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("4W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Comm", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Total", "HeaderStyle6"));

                    row.Cells.Add(new WorksheetCell("2W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("3W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("4W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Comm", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Total", "HeaderStyle6"));

                    row.Cells.Add(new WorksheetCell("2W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("3W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("4W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Comm", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Total", "HeaderStyle6"));
                   

                    row = sheet.Table.Rows.Add();
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    row.Index = 8;


                SQLString = "select rtolocation.Rtolocationname as rtolocation,"
                            + "SUM(CASE WHEN  (VehicleType='MOTOR CYCLE' or VehicleType='SCOOTER') AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order') THEN 1 ELSE 0 END ) AS [c2W],"
                            + "SUM(CASE WHEN  (VehicleType ='THREE WHEELER') AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order') THEN 1 ELSE 0 END ) AS [c3W],"
                            + "SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType ='LMV(CLASS)' OR VehicleType ='TRACTOR') AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order') THEN 1 ELSE 0 END ) AS [c4W],"
                            + "SUM(CASE WHEN  (VehicleType = 'MCV/HCV/TRAILERS' )AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order')THEN 1 ELSE 0 END ) AS cComm,"
                            + "Sum(CASE WHEN  ((VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER')or (VehicleType ='THREE WHEELER')or (VehicleType = 'LMV' OR VehicleType ='LMV(CLASS)' or VehicleType ='TRACTOR')or (VehicleType = 'MCV/HCV/TRAILERS')) AND (OrderDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='New Order') THEN 1 ELSE 0 END) as cTotal,"

                            + "sum(CASE WHEN  (VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER') AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close')THEN 1 else 0 END ) AS [a2W],"
                            + "sum(CASE WHEN  (VehicleType ='THREE WHEELER') AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close') THEN 1 else 0 END ) AS [a3W],"
                            + "sum(CASE WHEN  (VehicleType = 'LMV' OR VehicleType ='LMV(CLASS)') AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close') THEN 1 else 0 END ) AS [a4W],"
                            + "sum(CASE WHEN  (VehicleType = 'MCV/HCV/TRAILERS' or VehicleType ='TRACTOR') AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close') THEN 1 else 0 END ) AS aComm,"
                            + "sum(CASE WHEN  ((VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER' or VehicleType ='THREE WHEELER' or VehicleType = 'LMV' or VehicleType ='LMV(CLASS)' or VehicleType = 'MCV/HCV/TRAILERS'or VehicleType ='TRACTOR')) AND (OrderClosedDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Closed' or OrderStatus='Close') THEN 1 else 0  END) as aTotal,"

                            + "sum(CASE WHEN  (VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER') AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0 END ) AS [s2W],"
                            + "sum(CASE WHEN  (VehicleType ='THREE WHEELER') AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0 END ) AS [s3W],"
                            + "sum(CASE WHEN  (VehicleType = 'LMV' OR VehicleType ='LMV(CLASS)')  AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0 END ) AS [s4W],"
                            + "sum(CASE WHEN  (VehicleType = 'MCV/HCV/TRAILERS' or VehicleType ='TRACTOR') AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0 END ) AS sComm,"
                            + "sum(CASE WHEN  ((VehicleType='MOTOR CYCLE' OR VehicleType='SCOOTER' or VehicleType ='THREE WHEELER' or VehicleType = 'LMV' or VehicleType ='LMV(CLASS)' or VehicleType = 'MCV/HCV/TRAILERS'or VehicleType ='TRACTOR')) AND (OrderEmbossingDate between '" + ReportDate3 + "' and '" + ReportDate2 + "')and (orderstatus='Embossing Done') THEN 1 else 0  END) as sTotal "

                            + "from hsrprecords inner join  Rtolocation on hsrprecords.rtolocationid=rtolocation.rtolocationid "
                            + "where hsrprecords.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' "
                            + "GROUP BY  rtolocation.rtolocationname order by rtolocation.rtolocationname";
                       
                        dt = Utils.GetDataTable(SQLString, CnnString);
                       
                if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["rtolocation"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["c2W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["c3W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["c4W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["cComm"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["cTotal"].ToString(), DataType.String, "HeaderStyle"));

                                row.Cells.Add(new WorksheetCell(dtrows["a2W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["a3W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["a4W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["aComm"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["aTotal"].ToString(), DataType.String, "HeaderStyle"));

                                row.Cells.Add(new WorksheetCell(dtrows["s2W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["s3W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["s4W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["sComm"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["sTotal"].ToString(), DataType.String, "HeaderStyle"));
                               

                                row = sheet.Table.Rows.Add();
                            }

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
                        return;

                    }
                

                catch (Exception ex)
                {
                    throw ex;
                }

            }
        
    }
}