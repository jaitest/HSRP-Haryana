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
    public partial class DailyOperatorProductionReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
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
                UserType = Convert.ToInt32(Session["UserType"]);
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
               

                
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                        if (UserType.Equals(0))
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
                            labelDate0.Visible = false;
                           
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
            //HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
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
            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";

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
                LabelError.Text = "";

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            String ReportDate = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            
            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate); 
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID); 
              DataTable StateName;
              DataTable dts;
              DataSet dt = new DataSet();

                  int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                  int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                  string filename = "REPORT_DAILY_OPERATOR_PRODUCTION-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                  Workbook book = new Workbook();

                  // Specify which Sheet should be opened and the size of window by default
                  book.ExcelWorkbook.ActiveSheetIndex = 1;
                  book.ExcelWorkbook.WindowTopX = 100;
                  book.ExcelWorkbook.WindowTopY = 200;
                  book.ExcelWorkbook.WindowHeight = 7000;
                  book.ExcelWorkbook.WindowWidth = 8000;

                  // Some optional properties of the Document
                  book.Properties.Author = "HSRP";
                  book.Properties.Title = "HSRP Daily Operator Production Report";
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

                  Worksheet sheet = book.Worksheets.Add("HSRP Daily Operator Production Report");
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
                  WorksheetCell cell = row.Cells.Add("HSRP Daily Operator Production Report");
                  cell.MergeAcross = 3; // Merge two cells together
                  cell.StyleID = "HeaderStyle3";

                  row = sheet.Table.Rows.Add();
                  row.Index = 3;
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                  row = sheet.Table.Rows.Add();

                  //row.Index = 4;
                  //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  //row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                  //row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));
                  ////row.Cells.Add(<br>); 
                  //row = sheet.Table.Rows.Add();

                  // Skip one row, and add some text
                  row.Index = 4;


                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(DateTime.Now.ToShortDateString(), "HeaderStyle2"));
                  row = sheet.Table.Rows.Add();
                  row.Index = 5;
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                  row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                  row = sheet.Table.Rows.Add();


                  row.Index = 9;
                  //row.Cells.Add("Order Date");
                  row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Operator Name", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Daily Target", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Daily Actual Target", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Month Target", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Month Actual Target", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Year Target", "HeaderStyle"));
                  row.Cells.Add(new WorksheetCell("Year Actual Target", "HeaderStyle"));
                  row = sheet.Table.Rows.Add(); 
                  String StringField = String.Empty;
                  String StringAlert = String.Empty;

                  row.Index = 11;

                  UserType = Convert.ToInt32(Session["UserType"]);
                  if (UserType == 0)
                  {
                      int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                      SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where a.HSRP_StateID='" + intHSRPStateID + "'";
                  }
                  else
                  {
                      SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";
                      
                  }
                 
                  StateName = Utils.GetDataTable(SQLString.ToString(), CnnString);
                  if (StateName.Rows.Count > 0)
                  {

                      for (int i = 0; i <= StateName.Rows.Count - 1; i++)
                      {
                          SQLString = "Report_OperatorWisePRODUCTION'" + OrderDate1 + "','" + intHSRPStateID + "','" + StateName.Rows[i]["RTOLocationID"] + "'";
                          dt = Utils.getDataSet(SQLString, CnnString);

                          string RTOColName = string.Empty;
                          Int64 DailyTarget = 0;
                          Int64 monthtarget = 0;
                          Int64 dailyactual = 0;
                          Int64 monthactual = 0;

                          Int64 yrstarget = 0;
                          Int64 yrsactual = 0;
                          int sno = 0;
                          if (dt.Tables[0].Rows.Count > 0)
                          {
                              foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
                              {

                                  sno = sno + 1;
                                  row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                  if (RTOColName != StateName.Rows[i]["RTOLocationName"].ToString())
                                  {
                                      RTOColName = StateName.Rows[i]["RTOLocationName"].ToString();
                                      row.Cells.Add(new WorksheetCell(StateName.Rows[i]["RTOLocationName"].ToString(), "HeaderStyle"));
                                  }
                                  else
                                  {
                                      row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                                  }

                                  row.Cells.Add(new WorksheetCell(dtrows["optname"].ToString(), DataType.String, "HeaderStyle"));
                                  row.Cells.Add(new WorksheetCell(dtrows["daily_target"].ToString(), DataType.String, "HeaderStyle5"));
                                  row.Cells.Add(new WorksheetCell(dtrows["daily_actual"].ToString(), DataType.String, "HeaderStyle5"));
                                  row.Cells.Add(new WorksheetCell(dtrows["month_target"].ToString(), DataType.String, "HeaderStyle5"));
                                  row.Cells.Add(new WorksheetCell(dtrows["month_actual"].ToString(), DataType.String, "HeaderStyle5"));
                                  row.Cells.Add(new WorksheetCell(dtrows["yrs_target"].ToString(), DataType.String, "HeaderStyle5"));
                                  row.Cells.Add(new WorksheetCell(dtrows["yrs_actual"].ToString(), DataType.String, "HeaderStyle5"));
                                  row = sheet.Table.Rows.Add();

                                  DailyTarget = DailyTarget + checked1(dt.Tables[0].Rows[0]["daily_target"].ToString());
                                  dailyactual = dailyactual + checked1(dtrows["daily_actual"].ToString());
                                  monthtarget = monthtarget + checked1(dt.Tables[0].Rows[0]["month_target"].ToString());
                                  monthactual = monthactual + checked1(dtrows["month_actual"].ToString());
                                  yrstarget = yrstarget + checked1(dt.Tables[0].Rows[0]["yrs_target"].ToString());
                                  yrsactual = yrsactual + checked1(dtrows["yrs_actual"].ToString());
                              }
                              row = sheet.Table.Rows.Add();

                              row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                              row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle8"));
                              row.Cells.Add(new WorksheetCell("", "HeaderStyle8"));
                              row.Cells.Add(new WorksheetCell(Convert.ToInt64(DailyTarget).ToString(), "HeaderStyle8"));
                              row.Cells.Add(new WorksheetCell(Convert.ToInt64(dailyactual).ToString(), "HeaderStyle8"));
                              row.Cells.Add(new WorksheetCell(Convert.ToInt64(monthtarget).ToString(), "HeaderStyle8"));
                              row.Cells.Add(new WorksheetCell(Convert.ToInt64(monthactual).ToString(), "HeaderStyle8"));
                              row.Cells.Add(new WorksheetCell(Convert.ToInt64(yrstarget).ToString(), "HeaderStyle8"));
                              row.Cells.Add(new WorksheetCell(Convert.ToInt64(yrsactual).ToString(), "HeaderStyle8"));
                              row = sheet.Table.Rows.Add();

                              DailyTarget = 0;
                              monthtarget = 0;
                              dailyactual = 0;
                              monthactual = 0;
                              yrstarget = 0;
                              yrsactual = 0;
                              row = sheet.Table.Rows.Add();
                          }
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
            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        public static Int64 checked1(string aa)
        {
            Int64 k = 0;
            if (Int64.TryParse(aa, out k))
            {
            }
            else
            {
                k = 0;
            }
            return k;
        }
    }

}