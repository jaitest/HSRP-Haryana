using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;

namespace HSRP.Report
{
    public partial class DailyLaserPlateProduction : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
       protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
               

                
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                        if (UserType =="0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                        }
                        else
                        {
                            
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            labelDate.Visible = false;
                           
                            FilldropDownListClient();
                            FilldropDownListOrganization(); 
                        }
                    }
                    catch (Exception err)
                    {
                        
                    }
                }
            }
        }
       
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
             
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType =="0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }

        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'  Order by RTOLocationName";

                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                 
            }
        }
       
        #endregion

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
            LabelError.Visible = false;

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            String ReportDate = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate); 
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

            SQLString = "Report_DailyPRODUCTION'" + OrderDate1 + "','" + intHSRPStateID + "','" + strUserID + "','" + UserType + "'";
                   

              DataTable dt = Utils.GetDataTable(SQLString, CnnString);

              if (dt.Rows.Count > 0)
              {

                  string filename = "DailyLaserPlateProductionReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                  Workbook book = new Workbook();

                  // Specify which Sheet should be opened and the size of window by default
                  book.ExcelWorkbook.ActiveSheetIndex = 1;
                  book.ExcelWorkbook.WindowTopX = 100;
                  book.ExcelWorkbook.WindowTopY = 200;
                  book.ExcelWorkbook.WindowHeight = 7000;
                  book.ExcelWorkbook.WindowWidth = 8000;

                  // Some optional properties of the Document
                  book.Properties.Author = "HSRP";
                  book.Properties.Title = "HSRP Daily Production Report";
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

                  WorksheetStyle style7 = book.Styles.Add("HeaderStyle7");
                  style7.Font.FontName = "Tahoma";
                  style7.Font.Size = 10;
                  style7.Font.Bold = true;
                  style7.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                  style7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                  style7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                  style7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                  style7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                  style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                  WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                  style3.Font.FontName = "Tahoma";
                  style3.Font.Size = 12;
                  style3.Font.Bold = true;
                  style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                  WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                  style9.Font.FontName = "Tahoma";
                  style9.Font.Size = 10;
                  style9.Font.Bold = true;
                  style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                  style9.Interior.Color = "#FCF6AE";
                  style9.Interior.Pattern = StyleInteriorPattern.Solid;


                  Worksheet sheet = book.Worksheets.Add("HSRP Daily Laser Plate Production Report");

                  sheet.Table.Columns.Add(new WorksheetColumn(60));
                  sheet.Table.Columns.Add(new WorksheetColumn(205));
                  sheet.Table.Columns.Add(new WorksheetColumn(100));
                  sheet.Table.Columns.Add(new WorksheetColumn(130));

                  //sheet.Range["A1"].Style.WrapText = true; 
                  sheet.Table.Columns.Add(new WorksheetColumn(100));
                  sheet.Table.Columns.Add(new WorksheetColumn(120));
                  sheet.Table.Columns.Add(new WorksheetColumn(112));
                  sheet.Table.Columns.Add(new WorksheetColumn(109));
                  sheet.Table.Columns.Add(new WorksheetColumn(105));
                  sheet.Table.Columns.Add(new WorksheetColumn(160));


                  WorksheetRow row = sheet.Table.Rows.Add();
                  // row.
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                  row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                  WorksheetCell cell = row.Cells.Add("HSRP Daily Laser Plate  Production Report");
                  cell.MergeAcross = 3;            // Merge two cells together
                  cell.StyleID = "HeaderStyle3";

                  //row.Cells.Add(<br>);
                  row = sheet.Table.Rows.Add();
                  row.Index = 3;
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                  row = sheet.Table.Rows.Add();
                  row.Index = 4;
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));
                  //row.Cells.Add(<br>); 
                  row = sheet.Table.Rows.Add();

                  // Skip one row, and add some text
                  row.Index = 5;
                  DateTime date = System.DateTime.Now;
                  string formatted = date.ToString("dd/MM/yyyy");

                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));

                  row.Cells.Add(new WorksheetCell(formatted, "HeaderStyle2"));
                  row = sheet.Table.Rows.Add();
                  row.Index = 6;
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                  row = sheet.Table.Rows.Add();


                  row.Index = 8;
                  //row.Cells.Add("Order Date");
                  row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("StateName", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Particulars of Product", "HeaderStyle")); 
                  row.Cells.Add(new WorksheetCell("Daily Actual", "HeaderStyle")); 
                  row.Cells.Add(new WorksheetCell("Month to Date Actual", "HeaderStyle")); 
                  row.Cells.Add(new WorksheetCell("Year to Date Actual", "HeaderStyle"));
                  row = sheet.Table.Rows.Add();

                  //row.Index = 9;
                  //row.Cells.Add(new WorksheetCell(dt.Rows[0]["uom"].ToString(), "HeaderStyle2"));
                  //row.Cells.Add(new WorksheetCell(dt.Rows[0]["machinename"].ToString(), "HeaderStyle2"));
                  //row = sheet.Table.Rows.Add();

                  //string SQLString = String.Empty;
                  String StringField = String.Empty;
                  String StringAlert = String.Empty;

                  if (dt.Rows.Count <= 0)
                  {
                      LabelError.Text = string.Empty;
                      LabelError.Text = "Their is no selected data for the selected  date range.";
                      return;
                  }
                  row.Index = 10;
                  Int64 DailyCount = 0;
                  Int64 MonthlyCount = 0;
                  Int64 YearlyCount = 0;

                  string StateName = string.Empty;
                  string StateNameSecond = string.Empty;
                          
                  int sno = 0;
                  
                  
                  foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                  {


                      DailyCount = +DailyCount + Convert.ToInt64(dtrows["DailyCount"]);
                      MonthlyCount = +MonthlyCount + Convert.ToInt64(dt.Rows[0]["MonthlyCount"]);
                      YearlyCount = +YearlyCount + Convert.ToInt64(dtrows["YearlyCount"]);
                      
                      row = sheet.Table.Rows.Add();
                      if (StateName != Convert.ToString(dtrows["StateName"]))
                      {

                          row = sheet.Table.Rows.Add();
                          row = sheet.Table.Rows.Add();

                          sno = sno + 1;
                          StateName = Convert.ToString(dtrows["StateName"]);
                          row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                          row.Cells.Add(new WorksheetCell(dtrows["StateName"].ToString(), DataType.String, "HeaderStyle"));
                      }
                      else
                      {
                          row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                          row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                      }

                      row.Cells.Add(new WorksheetCell(dtrows["RTOLocationName"].ToString(), DataType.String, "HeaderStyle7"));
                      row.Cells.Add(new WorksheetCell(dtrows["ProductCode"].ToString(), DataType.String, "HeaderStyle5"));
                      row.Cells.Add(new WorksheetCell(dtrows["DailyCount"].ToString(), DataType.String, "HeaderStyle5"));
                      row.Cells.Add(new WorksheetCell(dtrows["MonthlyCount"].ToString(), DataType.String, "HeaderStyle5"));
                      row.Cells.Add(new WorksheetCell(dtrows["YearlyCount"].ToString(), DataType.String, "HeaderStyle5"));
                       

                      
                      //row.Cells.Add(<br>); 
                     
                          //if (StateNameSecond != Convert.ToString(dtrows["StateName"]))
                          //{
                          //    StateNameSecond = Convert.ToString(dtrows["StateName"]);
                          //    row = sheet.Table.Rows.Add();
                          //    row = sheet.Table.Rows.Add();
                          //    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                          //    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                          //    row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle9"));
                          //    row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                          //    row.Cells.Add(new WorksheetCell(Convert.ToInt64(DailyCount).ToString(), "HeaderStyle9"));
                          //    row.Cells.Add(new WorksheetCell(Convert.ToInt64(MonthlyCount).ToString(), "HeaderStyle9"));
                          //    row.Cells.Add(new WorksheetCell(Convert.ToInt64(YearlyCount).ToString(), "HeaderStyle9"));

                          //    DailyCount = 0;
                          //    MonthlyCount = 0;
                          //    YearlyCount = 0;
                          //    row = sheet.Table.Rows.Add();
                          //    row = sheet.Table.Rows.Add();
                          //} 
                  }


                  //row.Cells.Add(<br>); 
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
              else
              {
                  string closescript1 = "<script>alert('No records found for selected date.')</script>";
                  Page.RegisterStartupScript("abc", closescript1);
                  return;
              } 
            }

            catch (Exception ex)
            {
                LabelError.Visible = true;
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
    }

}