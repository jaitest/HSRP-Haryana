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
    public partial class DailyAll_MISReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
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



                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                        //FilldropDownListOrganization();
                        //FilldropDownListClient(); 
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

        //private void FilldropDownListOrganization()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
        //        Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
        //        // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
        //    }
        //    else
        //    {
        //        SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
        //        DataSet dts = Utils.getDataSet(SQLString, CnnString);
        //        DropDownListStateName.DataSource = dts;
        //        DropDownListStateName.DataBind();

        //    }
        //}

        //private void FilldropDownListClient()
        //{
        //    if (UserType.Equals(0))
        //    {
        //        int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";

        //        Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        //        // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        //    }
        //    else
        //    {
        //        // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'   Order by RTOLocationName";
        //        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";

        //        DataSet dss = Utils.getDataSet(SQLString, CnnString);
        //        dropDownListClient.DataSource = dss;
        //        dropDownListClient.DataBind();
        //    }
        //}

        #endregion

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
           // FilldropDownListClient();
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
                //int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();

                //int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "Report_Daily_AllMIS-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily MIS All State";
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


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style7 = book.Styles.Add("HeaderStyle7");
                style7.Font.FontName = "Tahoma";
                style7.Font.Size = 10;
                style7.Font.Bold = true;
                style7.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

                Worksheet sheet = book.Worksheets.Add("HSRP Daily MIS All State");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(195));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(90));

                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(162));
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
                WorksheetCell cell = row.Cells.Add("HSRP Daily MIS All State");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                 
                // Skip one row, and add some text
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("dd/M/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
 

                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("StateName", "HeaderStyle6")); 
                row.Cells.Add(new WorksheetCell("Daily Net Amount", "HeaderStyle6"));

                row.Cells.Add(new WorksheetCell("Daily New Order", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Daily Embossing", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Daily Closed", "HeaderStyle6"));

                row.Cells.Add(new WorksheetCell("Monthly Net Amount", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Monthly New Order", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Monthly Embossing", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Monthly Closed", "HeaderStyle6"));

                row.Cells.Add(new WorksheetCell("Yearly Net Amount", "HeaderStyle6")); 
                row.Cells.Add(new WorksheetCell("Yearly New Order", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Yearly Embossing", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Yearly Closed", "HeaderStyle6")); 
 
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty; 
                row.Index = 9;
 
               Int64 Dailycoll_amt = 0;
               Int64 Dailyneworder = 0;
               Int64 Dailyemborder = 0;
               Int64 Dailycloseorder = 0;

               Int64 moncoll_amt = 0;
               Int64 monneworder = 0;
               Int64 monemborder = 0;
               Int64 moncloseorder = 0;

               Int64 yrscoll_amt = 0;
               Int64 yrsneworder = 0;
               Int64 yrsemborder = 0;
               Int64 yrscloseorder = 0;




                    int sno = 0;


                    SQLString = "Report_Daily_AllMIS'" + OrderDate1 + "'";
                          
                        dt = Utils.GetDataTable(SQLString, CnnString); 
                        string RTOColName = string.Empty;
                        if (dt.Rows.Count > 0)
                        { 
                            
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;
                                
                                row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Statename"].ToString(), DataType.String, "HeaderStyle7"));

                                row.Cells.Add(new WorksheetCell(dtrows["Dailycoll_amt"].ToString(), DataType.Number, "HeaderStyle5")); 
                                row.Cells.Add(new WorksheetCell(dtrows["Dailyneworder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Dailyemborder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Dailycloseorder"].ToString(), DataType.Number, "HeaderStyle5"));

                                row.Cells.Add(new WorksheetCell(dtrows["moncoll_amt"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["monneworder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["monemborder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["moncloseorder"].ToString(), DataType.Number, "HeaderStyle5"));

                                row.Cells.Add(new WorksheetCell(dtrows["yrscoll_amt"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrsneworder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrsemborder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrscloseorder"].ToString(), DataType.Number, "HeaderStyle5")); 
                            }
                            row = sheet.Table.Rows.Add();
                        }   
                    Dailycoll_amt = 0;
                    Dailyneworder = 0;
                    Dailyemborder = 0;
                    Dailycloseorder = 0;

                    moncoll_amt = 0;
                    monneworder = 0;
                    monemborder = 0;
                    moncloseorder = 0;

                    yrscoll_amt = 0;
                    yrsneworder = 0;
                    yrsemborder = 0;
                    yrscloseorder = 0;

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
            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }



        
        public static String TitleCaseString(String s)
        {
            if (s == null) return s;

            String[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join(" ", words);
        }
    }
}