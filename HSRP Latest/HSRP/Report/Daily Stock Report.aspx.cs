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
    public partial class Daily_Stock_Report : System.Web.UI.Page
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
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";

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
                DataTable dt = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "DailyStockReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily Stock Report";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Daily Stock Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(195));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
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
                WorksheetCell cell = row.Cells.Add("HSRP Daily Stock Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
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
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Plate Size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Opening Balance Of Blank Plates", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Total Received", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Total Produced", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Total Rejection", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Closing Balance Of Blank Plates", "HeaderStyle6"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);
                if (UserType == 0)
                {
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and   a.HSRP_StateID='" + intHSRPStateID + "'";
                }
                else
                {
                    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where a.LocationType!='District' and  UserRTOLocationMapping.UserID='" + strUserID + "' ";
                }

                StateName = Utils.GetDataTable(SQLString.ToString(), CnnString);
                if (StateName.Rows.Count > 0)
                { 
                    float totOpBal = 0;
                    float totRecv = 0;
                    float totProdc = 0;
                    float totRej = 0;
                    float totClosingBal = 0;

                    for (int i = 0; i <= StateName.Rows.Count - 1; i++)
                    {
                        //SQLString = "Report_StockReport'" + OrderDate1 + "','" + intHSRPStateID + "','" + StateName.Rows[i]["RTOLocationID"] + "'";
                        UserType = Convert.ToInt32(Session["UserType"]);
                        if (UserType == 0)
                        {
                            SQLString = "Report_StockReport '" + OrderDate1 + "','" + intHSRPStateID + "','" + StateName.Rows[i]["RTOLocationID"] + "',0";
                        }
                        else
                        {
                            HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                            RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());

                            SQLString = "Report_StockReport '" + OrderDate1 + "','" + HSRPStateID + "','" + StateName.Rows[i]["RTOLocationID"] + "',1";
                        }
                        dt = Utils.GetDataTable(SQLString, CnnString);

                        string RTOColName = string.Empty;
                        int sno = 0;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            { 
                                sno = sno + 1;
                                row = sheet.Table.Rows.Add();

                                totOpBal = totOpBal + Convert.ToInt64(dtrows["Opening Balance Of Blank Plates"]);
                                totRecv = totRecv + Convert.ToInt64(dtrows["Total Received"]);
                                totProdc = totProdc + Convert.ToInt64(dtrows["Total Produced"]);
                                totRej = totRej + Convert.ToInt64(dtrows["Total Rejection"]);
                                totClosingBal = totClosingBal + Convert.ToInt64(dtrows["Closing Balance Of Blank Plates"]); 

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

                                row.Cells.Add(new WorksheetCell(dtrows["Plate Size"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Opening Balance Of Blank Plates"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Total Received"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Total Produced"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Total Rejection"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Closing Balance Of Blank Plates"].ToString(), DataType.String, "HeaderStyle5"));
                                row = sheet.Table.Rows.Add();

                            }
                            row = sheet.Table.Rows.Add();
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                            row.Cells.Add(new WorksheetCell("Grand Total :", "HeaderStyle9"));
                            row.Cells.Add(new WorksheetCell("", "HeaderStyle9"));
                            row.Cells.Add(new WorksheetCell((totOpBal).ToString(), "HeaderStyle9"));
                            row.Cells.Add(new WorksheetCell((totRecv).ToString(), "HeaderStyle9"));
                            row.Cells.Add(new WorksheetCell((totProdc).ToString(), "HeaderStyle9"));
                            row.Cells.Add(new WorksheetCell((totRej).ToString(), "HeaderStyle9"));
                            row.Cells.Add(new WorksheetCell((totClosingBal).ToString(), "HeaderStyle9"));

                            totOpBal = 0;
                            totRecv = 0;
                            totProdc = 0;
                            totRej = 0;
                            totClosingBal = 0;
                            row = sheet.Table.Rows.Add();
                        }
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
            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        //protected void btnExportToExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LabelError.Visible = false;
        //        //String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
        //        //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
        //        //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
        //        //DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);

        //        DateTime dtimeOrderDate = Convert.ToDateTime(OrderDate.SelectedDate.ToString());
        //        string OrderDate1 = dtimeOrderDate.ToString("dd-MMM-yyyy");
        //        int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //        int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);


        //        UserType = Convert.ToInt32(Session["UserType"]);
        //        if (UserType == 0)
        //        {
        //            SQLString = "Report_StockReport '" + OrderDate1 + "','" + intHSRPStateID + "','" + intRTOLocationID + "',0";
        //        }
        //        else
        //        {
        //            HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
        //            RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                    
        //            SQLString = "Report_StockReport '" + OrderDate1 + "','" + HSRPStateID + "','" + RTOLocationID + "',1";
        //        }

        //        DataSet dt = Utils.getDataSet(SQLString, CnnString);

        //        if (dt.Tables[0].Rows.Count > 0)
        //        {
        //            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

        //            string filename = "DailyStockReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
        //            Workbook book = new Workbook();
        //            // Specify which Sheet should be opened and the size of window by default
        //            book.ExcelWorkbook.ActiveSheetIndex = 1;
        //            book.ExcelWorkbook.WindowTopX = 100;
        //            book.ExcelWorkbook.WindowTopY = 200;
        //            book.ExcelWorkbook.WindowHeight = 7000;
        //            book.ExcelWorkbook.WindowWidth = 8000;

        //            // Some optional properties of the Document
        //            book.Properties.Author = "HSRP";
        //            book.Properties.Title = "HSRP Daily Stock Report";
        //            book.Properties.Created = DateTime.Now;

        //            //-----------STYLE---------------------
        //            WorksheetStyle style0 = book.Styles.Add("HeaderStyle0");
        //            style0.Font.FontName = "Tahoma";
        //            style0.Font.Size = 12;
        //            //style.Workbook=white-space:nowrap;
        //            style0.Font.Bold = true;
        //            style0.Alignment.Horizontal = StyleHorizontalAlignment.Left;

        //            WorksheetStyle style = book.Styles.Add("HeaderStyle");
        //            style.Font.FontName = "Tahoma";
        //            style.Font.Size = 10;
        //            //style.Workbook=white-space:nowrap;
        //            style.Font.Bold = true;
        //            style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //            //style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //            //style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //            //style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //            //style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        //            //style.Font.Color = "White";
        //            //style.Interior.Color = "#7F7F7F";
        //            //style.Interior.Pattern = StyleInteriorPattern.Solid;

        //            WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
        //            style2.Font.FontName = "Tahoma";
        //            style2.Font.Size = 10;
        //            style2.Font.Bold = false;

        //            WorksheetStyle style3 = book.Styles.Add("HeaderStyleData1");
        //            style3.Font.FontName = "Tahoma";
        //            style3.Font.Size = 10;
        //            style3.Font.Bold = true;
        //            style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //            style3.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //            style3.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //            style3.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //            style3.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        //            //style3.Interior.Color = "#C5BE97";
        //            //style3.Interior.Pattern = StyleInteriorPattern.Solid;

        //            WorksheetStyle style4 = book.Styles.Add("HeaderStyleData2");
        //            style4.Font.FontName = "Tahoma";
        //            style4.Font.Size = 10;
        //            style4.Font.Bold = false;
        //            style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        //            style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //            style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //            style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //            style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
        //            //style4.Interior.Color = "#DBE5F1";
        //            //style4.Interior.Pattern = StyleInteriorPattern.Solid;

        //            //-----------STYLE---------------------


        //            Worksheet sheet = book.Worksheets.Add("HSRP Daily Stock Report");
        //            //sheet.Range["A1"].Style.WrapText = true;
        //            sheet.Table.Columns.Add(new WorksheetColumn(50));
        //            sheet.Table.Columns.Add(new WorksheetColumn(120));
        //            sheet.Table.Columns.Add(new WorksheetColumn(130));
        //            sheet.Table.Columns.Add(new WorksheetColumn(100));
        //            sheet.Table.Columns.Add(new WorksheetColumn(115));
        //            sheet.Table.Columns.Add(new WorksheetColumn(130));
        //            sheet.Table.Columns.Add(new WorksheetColumn(115));
        //            //sheet.Table.Columns.Add(new WorksheetColumn(130));
        //            WorksheetRow row = sheet.Table.Rows.Add();

                    
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //            //row = sheet.Table.Rows.Add();

        //            //row.Index = 2;
        //            row.Index = 1;
        //            WorksheetCell cell = row.Cells.Add();
        //            cell = row.Cells.Add("Report: ");
        //            //cell.MergeAcross = 6;            // Merge two cells together
        //            cell.StyleID = "HeaderStyle0";
        //            cell = row.Cells.Add("HSRP Daily Stock Report");
        //            cell.StyleID = "HeaderStyle0";
        //            row = sheet.Table.Rows.Add();

        //            row.Index = 2;
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

        //            row.Cells.Add(new WorksheetCell("State:", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle"));
        //            row = sheet.Table.Rows.Add();

        //            row.Index = 3;
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

        //            row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle"));
        //            if (dropDownListClient.SelectedItem.ToString().Trim() == "--Select Location--")
        //            {
        //                row.Cells.Add(new WorksheetCell("All", "HeaderStyle"));
        //            }
        //            else
        //            {
        //                row.Cells.Add(new WorksheetCell(TitleCaseString(dropDownListClient.SelectedItem.ToString()), "HeaderStyle"));
        //            }
        //            row = sheet.Table.Rows.Add();

        //            row.Index = 4;
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

        //            row.Cells.Add(new WorksheetCell("Generated Date:", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd-MMM-yyyy"), "HeaderStyle"));
        //            row = sheet.Table.Rows.Add();

        //            row.Index = 5;
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

        //            row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle"));
        //            row.Cells.Add(new WorksheetCell(OrderDate1, "HeaderStyle"));
        //            row = sheet.Table.Rows.Add();

        //            //row.Index = 7;
        //            //    WorksheetCell cell = row.Cells.Add("HSRP Daily Stock Report For " + OrderDate1);
        //            //    cell.MergeAcross = 6;            // Merge two cells together
        //            //    cell.StyleID = "HeaderStyle";
        //            //    row = sheet.Table.Rows.Add();

        //            row.Index = 6;
        //            row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell("Plate Size", "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell("Opening Balance Of Blank Plates", "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell("Total Received", "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell("Total Produced", "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell("Total Rejection", "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell("Closing Balance Of Blank Plates", "HeaderStyleData1"));
        //            row = sheet.Table.Rows.Add();

        //            //Skip one cell:
        //            //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

        //            // Skip one row:
        //            //row.Index = 11;
        //            //row = sheet.Table.Rows.Add();

        //            String StringField = String.Empty;
        //            String StringAlert = String.Empty;

        //            row.Index = 7;
        //            Int32 totOpBal = 0;
        //            Int32 totRecv = 0;
        //            Int32 totProdc = 0;
        //            Int32 totRej = 0;
        //            Int32 totClosingBal = 0;
        //            int sno = 0;

        //            foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
        //            {
        //                sno = sno + 1;
        //                row = sheet.Table.Rows.Add();
        //                //row.Cells.Add(new WorksheetCell(dtrows["Location"].ToString(), DataType.String));
        //                row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), "HeaderStyleData2"));
        //                row.Cells.Add(new WorksheetCell(dtrows["Plate Size"].ToString(), "HeaderStyleData2"));
        //                row.Cells.Add(new WorksheetCell(dtrows["Opening Balance Of Blank Plates"].ToString(), "HeaderStyleData2"));
        //                row.Cells.Add(new WorksheetCell(dtrows["Total Received"].ToString(), "HeaderStyleData2"));
        //                row.Cells.Add(new WorksheetCell(dtrows["Total Produced"].ToString(), "HeaderStyleData2"));
        //                row.Cells.Add(new WorksheetCell(dtrows["Total Rejection"].ToString(), "HeaderStyleData2"));
        //                row.Cells.Add(new WorksheetCell(dtrows["Closing Balance Of Blank Plates"].ToString(), "HeaderStyleData2"));

        //                totOpBal = +totOpBal + Convert.ToInt32(dt.Tables[0].Rows[0]["Opening Balance Of Blank Plates"]);
        //                totRecv = +totRecv + Convert.ToInt32(dt.Tables[0].Rows[0]["Total Received"]);
        //                totProdc = +totProdc + Convert.ToInt32(dt.Tables[0].Rows[0]["Total Produced"]);
        //                totRej = +totRej + Convert.ToInt32(dt.Tables[0].Rows[0]["Total Rejection"]);
        //                totClosingBal = +totClosingBal + Convert.ToInt32(dt.Tables[0].Rows[0]["Closing Balance Of Blank Plates"]);

        //            }

        //            row = sheet.Table.Rows.Add();
        //            row.Cells.Add(new WorksheetCell("", "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell("Total :", "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell(Convert.ToInt32(totOpBal).ToString(), "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell(Convert.ToInt32(totRecv).ToString(), "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell(Convert.ToInt32(totProdc).ToString(), "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell(Convert.ToInt32(totRej).ToString(), "HeaderStyleData1"));
        //            row.Cells.Add(new WorksheetCell(Convert.ToInt32(totClosingBal).ToString(), "HeaderStyleData1"));
        //            row = sheet.Table.Rows.Add();
        //            HttpContext context = HttpContext.Current;
        //            context.Response.Clear();
        //            // Save the file and open it
        //            book.Save(Response.OutputStream);
        //            context.Response.ContentType = "application/vnd.ms-excel";

        //            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //            context.Response.End();
        //        }
        //        else
        //        {
        //            //LabelError.Text = string.Empty;
        //            //LabelError.Text = "No records found for selected date.";
        //            string closescript1 = "<script>alert('No records found for selected date.')</script>";
        //            Page.RegisterStartupScript("abc", closescript1);
        //            return;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        LabelError.Visible = true;
        //        LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
        //    }
        //}
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