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
    public partial class HSRPSummeryReport : System.Web.UI.Page
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
        DataProvider.BAL bl = new DataProvider.BAL();
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
                //GetFinYear();
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
        DataTable dt1 = new DataTable();
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {          
            try
            {
                LabelError.Text = "";

                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String ReportDate = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
                String MonthDate = "1/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
                //String YearlyDate = "1/1/"  + StringOrderDate[2].Split(' ')[0];
                String YearlyDate = GetFinYear();
                GetFinYear();
                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                
              //  OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[1]), Convert.ToInt32(StringOrderDate[1]));


                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);

                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();


                string filename = "Report_DailyHSRPSummary-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily HSRP Summary";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Daily HSRP Summary");
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
                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Daily HSRP Summary");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";
                row = sheet.Table.Rows.Add();
                // Skip one row, and add some text
                

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dates.ToString("dd/M/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Day Summary", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row = sheet.Table.Rows.Add();

 

                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("State Name", "HeaderStyle6"));
               
                row.Cells.Add(new WorksheetCell("New Order", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Opening Balance", "HeaderStyle6"));  
                row.Cells.Add(new WorksheetCell("Collection", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Bank Deposit", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Amount in Hand", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Production", "HeaderStyle6"));   // Daily Embossing
                row.Cells.Add(new WorksheetCell("Rejection", "HeaderStyle6"));
              //  row.Cells.Add(new WorksheetCell("Blank Plate Inventory", "HeaderStyle6"));
               // row.Cells.Add(new WorksheetCell("Pending Affixation", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Affixation", "HeaderStyle6"));      // Daily Closed
                
               
                
               
               
 
 
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty; 
                row.Index = 9;

                Int64? Dailyneworder = 0;
                Int64? DailyTotalProduction = 0;
                Int64? Dailycloseorder = 0;
                Int64? Dailyemborder = 0;
                Int64? DailyRejection = 0;
                Int64? DailyBlankPlateInStock = 0;
                Int64? Dailycoll_amt = 0;
                Int64? DailyBankDeposite = 0;
                Int64? DailyTotalCashInHand = 0;
                Int64? DayOpenBal = 0;
                Int64? MonOpenBal = 0;
                Int64? YrsOpenBal = 0;
                Int64? monneworder = 0;
                Int64? monTotalProduction = 0;
                Int64? moncloseorder = 0;
                Int64? monemborder = 0;
                Int64? monRejection = 0;
                Int64? monBlankPlateInStock = 0;
                Int64? moncoll_amt = 0;
                Int64? monBankDeposite = 0;
                Int64? monTotalCashInHand = 0;

                Int64? yrsneworder = 0;
                Int64? yrsTotalProduction = 0;
                Int64? yrscloseorder = 0;
                Int64? yrsemborder = 0;
                Int64? yrsRejection = 0;
                Int64? yrsBlankPlateInStock = 0;
                Int64? yrscoll_amt = 0;
                Int64? yrsBankDeposite = 0;
                Int64? yrsTotalCashInHand = 0;





                    int sno = 0;


                    SQLString = "[Report_Daily_Consol_AllState_New1]'" + OrderDate1 + "'";
                    dt = Utils.GetDataTable(SQLString, CnnString); 
                    string RTOColName = string.Empty;
                    if (dt.Rows.Count > 0)
                        { 
                            
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;
                                
                               // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Statename"].ToString(), DataType.String, "HeaderStyle7"));                               

                                row.Cells.Add(new WorksheetCell(dtrows["Dailyneworder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["DayOpenBal"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Dailycoll_amt"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["DailyBankDeposite"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["DailyTotalCashInHand"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["DailyTotalProduction"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["DailyRejection"].ToString(), DataType.Number, "HeaderStyle5"));
                              //  row.Cells.Add(new WorksheetCell(dtrows["DailyBlankPlateInStock"].ToString(), DataType.Number, "HeaderStyle5"));
                             //   row.Cells.Add(new WorksheetCell(dtrows["Dailyemborder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Dailycloseorder"].ToString(), DataType.Number, "HeaderStyle5"));
                                
                               
                               
                                
                       
                                Dailyneworder = Dailyneworder + checked1(dtrows["Dailyneworder"].ToString());
                                DayOpenBal = DayOpenBal + checked1(dtrows["DayOpenBal"].ToString());
                                DailyTotalProduction = DailyTotalProduction + checked1(dtrows["DailyTotalProduction"].ToString());
                                Dailycloseorder = Dailycloseorder + checked1(dtrows["Dailycloseorder"].ToString());
                                Dailyemborder = Dailyemborder + checked1(dtrows["Dailyemborder"].ToString());
                                DailyRejection = DailyRejection + checked1(dtrows["DailyRejection"].ToString());
                                DailyBlankPlateInStock = DailyBlankPlateInStock + checked1(dtrows["DailyBlankPlateInStock"].ToString());
                                Dailycoll_amt = Dailycoll_amt + checked1(dtrows["Dailycoll_amt"].ToString());
                                DailyBankDeposite = DailyBankDeposite + checked1(dtrows["DailyBankDeposite"].ToString());
                                DailyTotalCashInHand = DailyTotalCashInHand + checked1(dtrows["DailyTotalCashInHand"].ToString());

                              
                                row = sheet.Table.Rows.Add();
                                 
                            }

                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Total", DataType.String, "HeaderStyle7"));
                           
                            row.Cells.Add(new WorksheetCell(Dailyneworder.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(DayOpenBal.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(Dailycoll_amt.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(DailyBankDeposite.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(DailyTotalCashInHand.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(DailyTotalProduction.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(DailyRejection.ToString(), DataType.Number, "HeaderStyle5"));
                          //  row.Cells.Add(new WorksheetCell(DailyBlankPlateInStock.ToString(), DataType.Number, "HeaderStyle5"));
                           // row.Cells.Add(new WorksheetCell(Dailyemborder.ToString(), DataType.Number, "HeaderStyle5"));
                           
                            row.Cells.Add(new WorksheetCell(Dailycloseorder.ToString(), DataType.Number, "HeaderStyle5"));
                            row = sheet.Table.Rows.Add();
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell("Month Summary", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell("From ", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell(MonthDate, "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell(" To ", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));

                            row = sheet.Table.Rows.Add();
                            row = sheet.Table.Rows.Add();


                            row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("State Name", "HeaderStyle6"));
                            

                            row.Cells.Add(new WorksheetCell("New Order", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Opening Balance", "HeaderStyle6"));  
                            row.Cells.Add(new WorksheetCell("Collection", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Bank Deposit", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Amount in Hand", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Production", "HeaderStyle6"));   // Daily Embossing
                            row.Cells.Add(new WorksheetCell("Rejection", "HeaderStyle6"));
                           // row.Cells.Add(new WorksheetCell("Blank Plate Inventory", "HeaderStyle6"));
                           // row.Cells.Add(new WorksheetCell("Pending Affixation", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Affixation", "HeaderStyle6"));      // Daily Closed
                             
                            row = sheet.Table.Rows.Add();

                            
                            sno = 0;
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                 
                                sno = sno + 1;

                                //row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Statename"].ToString(), DataType.String, "HeaderStyle7"));
                                

                                row.Cells.Add(new WorksheetCell(dtrows["monneworder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["MonOpenBal"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["moncoll_amt"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["monBankDeposite"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["monTotalCashInHand"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["monTotalProduction"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["monRejection"].ToString(), DataType.Number, "HeaderStyle5"));
                              //  row.Cells.Add(new WorksheetCell(dtrows["monBlankPlateInStock"].ToString(), DataType.Number, "HeaderStyle5"));
                              //  row.Cells.Add(new WorksheetCell(dtrows["monemborder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["moncloseorder"].ToString(), DataType.Number, "HeaderStyle5"));
                                
                               
                                
                               

                                monneworder = monneworder + checked1(dtrows["monneworder"].ToString());
                                MonOpenBal = MonOpenBal + checked1(dtrows["MonOpenBal"].ToString());
                                moncoll_amt = moncoll_amt + checked1(dtrows["moncoll_amt"].ToString());
                                monBankDeposite = monBankDeposite + checked1(dtrows["monBankDeposite"].ToString());
                                monTotalCashInHand = monTotalCashInHand + checked1(dtrows["monTotalCashInHand"].ToString());
                                monTotalProduction = monTotalProduction + checked1(dtrows["monTotalProduction"].ToString());
                                monRejection = monRejection + checked1(dtrows["monRejection"].ToString());
                                moncloseorder = moncloseorder + checked1(dtrows["moncloseorder"].ToString());
                                monemborder = monemborder + checked1(dtrows["monemborder"].ToString());
                               
                                monBlankPlateInStock = monBlankPlateInStock + checked1(dtrows["monBlankPlateInStock"].ToString());
                                


                                row = sheet.Table.Rows.Add();
                            }
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Total", DataType.String, "HeaderStyle7"));
                            
                            row.Cells.Add(new WorksheetCell(monneworder.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(MonOpenBal.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(moncoll_amt.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(monBankDeposite.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(monTotalCashInHand.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(monTotalProduction.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(monRejection.ToString(), DataType.Number, "HeaderStyle5"));
                           // row.Cells.Add(new WorksheetCell(monBlankPlateInStock.ToString(), DataType.Number, "HeaderStyle5"));
                          //  row.Cells.Add(new WorksheetCell(monemborder.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(moncloseorder.ToString(), DataType.Number, "HeaderStyle5"));
                            
                            
                            
                            

                            row = sheet.Table.Rows.Add();
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell("Year Summary ", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell("From ", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell(YearlyDate, "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell(" To ", "HeaderStyle2"));
                            row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));

                            row = sheet.Table.Rows.Add();
                            row = sheet.Table.Rows.Add();



                            row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("State Name", "HeaderStyle6"));
                             

                            row.Cells.Add(new WorksheetCell("New Order", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Opening Balance", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Net Collection", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Bank Deposit", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Amount in Hand", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Production", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Rejection", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Plate Inventory", "HeaderStyle6"));
                            row.Cells.Add(new WorksheetCell("Pending Production", "HeaderStyle6"));   // Daily Embossing
                            row.Cells.Add(new WorksheetCell("Affixation", "HeaderStyle6"));      // Daily Closed
                           
                           
                           


                            row = sheet.Table.Rows.Add();
                            sno = 0;
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                sno = sno + 1;

                                // row = sheet.Table.Rows.Add();
                                row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Statename"].ToString(), DataType.String, "HeaderStyle7"));
                               
                                row.Cells.Add(new WorksheetCell(dtrows["yrsneworder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["YrsOpenBal"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrscoll_amt"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrsBankDeposite"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrsTotalCashInHand"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrsTotalProduction"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrsRejection"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrsBlankPlateInStock"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrsemborder"].ToString(), DataType.Number, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["yrscloseorder"].ToString(), DataType.Number, "HeaderStyle5"));

                                yrsneworder = yrsneworder + checked1(dtrows["yrsneworder"].ToString());
                                YrsOpenBal = YrsOpenBal + checked1(dtrows["YrsOpenBal"].ToString());
                                yrsTotalProduction = yrsTotalProduction + checked1(dtrows["yrsTotalProduction"].ToString());
                                yrscloseorder = yrscloseorder + checked1(dtrows["yrscloseorder"].ToString());
                                yrsemborder = yrsemborder + checked1(dtrows["yrsemborder"].ToString());
                                yrsRejection = yrsRejection + checked1(dtrows["yrsRejection"].ToString());
                                yrsBlankPlateInStock = yrsBlankPlateInStock + checked1(dtrows["yrsBlankPlateInStock"].ToString());
                                yrscoll_amt = yrscoll_amt + checked1(dtrows["yrscoll_amt"].ToString());
                                yrsBankDeposite = yrsBankDeposite + checked1(dtrows["yrsBankDeposite"].ToString());
                                yrsTotalCashInHand = yrsTotalCashInHand + checked1(dtrows["yrsTotalCashInHand"].ToString());
                                row = sheet.Table.Rows.Add();
                            }
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("Total", DataType.String, "HeaderStyle7"));
                            
                            row.Cells.Add(new WorksheetCell(yrsneworder.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(YrsOpenBal.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(yrscoll_amt.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(yrsBankDeposite.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(yrsTotalCashInHand.ToString(), DataType.Number, "HeaderStyle5"));

                            row.Cells.Add(new WorksheetCell(yrsTotalProduction.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(yrsRejection.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(yrsBlankPlateInStock.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(yrsemborder.ToString(), DataType.Number, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(yrscloseorder.ToString(), DataType.Number, "HeaderStyle5"));
                            
                           
                            

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

        public string GetFinYear()
        {
            string finyear = "";
            DateTime s =CalendarOrderDate.SelectedDate;

            int m = s.Month;
            int y = s.Year;
            if (m > 3)
            {
                finyear = "01/04/" + y.ToString();
            }
            else
            {
                finyear = "01/04/"+Convert.ToString((y - 1));
            }
            return finyear;

        }

    }
}