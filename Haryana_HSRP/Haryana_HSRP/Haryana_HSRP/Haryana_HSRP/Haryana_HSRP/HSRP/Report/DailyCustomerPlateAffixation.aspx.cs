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
    public partial class DailyCustomerPlateAffixation : System.Web.UI.Page
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
                HSRPStateID =  Session["UserHSRPStateID"].ToString();
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
                            labelClient.Visible = false;
                            dropDownListClient.Visible = false;

                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
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
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
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

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                //String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
               // string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";
                
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "DailyCustomerPlateAffixationReportCustomer-wise-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Daily Plate Affixation Report Customer-Wise";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Daily Plate Affixation Report Customer-Wise");
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
                WorksheetCell cell = row.Cells.Add("HSRP Daily Plate Affixation Report Customer-Wise");
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
                row.Cells.Add(new WorksheetCell(dates.ToString("MM/dd/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("From :"+FromDate, "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(" To :" + ToDate, "HeaderStyle2"));
                row = sheet.Table.Rows.Add();


                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Name of Customer", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Registration No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Type of Vechicle", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Authorization No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cash Receipt No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cash Receipt Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order Closed Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);
                if (UserType == 0)
                {
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and   a.HSRP_StateID='" + intHSRPStateID + "' order by a.RTOLocationName";
                }
                else
                {
                    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where a.LocationType!='District' and  UserRTOLocationMapping.UserID='" + strUserID + "' order by a.RTOLocationName ";
                }

                StateName = Utils.GetDataTable(SQLString.ToString(), CnnString);
                if (StateName.Rows.Count > 0)
                {

                    int sno = 0;

                    for (int i = 0; i <= StateName.Rows.Count - 1; i++)
                    {
                        //SQLString = "SELECT OwnerName as [Name of Customer] ,VehicleRegNo  as [Regn. No.],VehicleType   as [Type of Vehicle], HSRPRecord_AuthorizationNo as [Authorisation No],CashReceiptNo  as [Cash Receipt No.], convert(varchar, OrderDate, 105)  as [Cash Receipt Date],'' as [Delay in no. of Days], HSRP_StateID, RTOLocationID   FROM  HSRPRecords where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID='" + StateName.Rows[i]["RTOLocationID"] + "' and OrderDate ='" + OrderDate1 + "'order by VehicleRegNo";
                        //SQLString = "SELECT OwnerName as [Name of Customer] ,VehicleRegNo  as [Regn. No.],VehicleType   as [Type of Vehicle], HSRPRecord_AuthorizationNo as [Authorisation No],CashReceiptNo  as [Cash Receipt No.], convert(varchar, OrderDate, 105)  as [Cash Receipt Date],'' as [Delay in no. of Days], HSRP_StateID, RTOLocationID   FROM  HSRPRecords where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID='" + StateName.Rows[i]["RTOLocationID"] + "' and   convert(varchar, OrderDate, 105) = convert(varchar, '" + From2 + "', 105) order by OrderDate, VehicleRegNo";
                        SQLString = "SELECT OwnerName as [Name of Customer] ,VehicleRegNo  as [Regn. No.],VehicleType   as [Type of Vehicle], HSRPRecord_AuthorizationNo as [Authorisation No],CashReceiptNo  as [Cash Receipt No.], convert(varchar, HSRPRecord_CreationDate, 105)  as [Cash Receipt Date],convert(varchar, OrderClosedDate, 105)  as [Order Closed Date],DATEDIFF(day,HSRPRecord_CreationDate,OrderClosedDate) as [dealy in No of Days], HSRP_StateID, RTOLocationID   FROM  HSRPRecords where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID='" + StateName.Rows[i]["RTOLocationID"] + "' and   OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "' order by OrderDate, VehicleRegNo";

                        dt = Utils.GetDataTable(SQLString, CnnString);

                        string RTOColName = string.Empty;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {

                               
                                if (RTOColName != StateName.Rows[i]["RTOLocationName"].ToString())
                                { 
                                    sno = sno + 1;
                                    row = sheet.Table.Rows.Add();
                                    row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                                    RTOColName = StateName.Rows[i]["RTOLocationName"].ToString();
                                    row.Cells.Add(new WorksheetCell(StateName.Rows[i]["RTOLocationName"].ToString(), "HeaderStyle"));
                                }
                                else
                                {
                                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                                    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                                }

                                row.Cells.Add(new WorksheetCell(dtrows["Name of Customer"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Regn. No."].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Type of Vehicle"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Authorisation No"].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Cash Receipt No."].ToString(), DataType.String, "HeaderStyle5"));
                                row.Cells.Add(new WorksheetCell(dtrows["Cash Receipt Date"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["Order Closed Date"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["dealy in No of Days"].ToString(), DataType.String, "HeaderStyle5")); 
                                row = sheet.Table.Rows.Add();
                            }
                            row = sheet.Table.Rows.Add();
                        }
                        else
                        {
                            sno = sno + 1;
                            row = sheet.Table.Rows.Add();
                            row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));

                            row.Cells.Add(new WorksheetCell(StateName.Rows[i]["RTOLocationName"].ToString(), "HeaderStyle"));

                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle")); 
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle")); 
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
        //    LabelError.Visible = false;

        //    String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
        //    string Mon = ("0" + StringOrderDate[0]);
        //    string Monthdate = Mon.Replace("00", "0").Replace("01","1"); 
        //    String From1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
        //    String ReportDate = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
        //    OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
        //    DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate); 
        //    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
            
        //      UserType = Convert.ToInt32(Session["UserType"]);
        //      if (UserType == 0)
        //      {
        //           SQLString = "SELECT OwnerName as [Name of Customer] ,VehicleRegNo  as [Regn. No.],VehicleType   as [Type of Vehicle], HSRPRecord_AuthorizationNo as [Authorisation No],CashReceiptNo  as [Cash Receipt No.], convert(varchar, OrderDate, 105)  as [Cash Receipt Date],'' as [Delay in no. of Days], HSRP_StateID, RTOLocationID   FROM  HSRPRecords where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID='" + intRTOLocationID + "' and OrderDate >='" + OrderDate1 + "' ";
        //      }
        //      else
        //      {
        //          HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
        //          RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
        //          SQLString = "select ownername as [Name of Customer],VehicleRegNo as [Regn. No.],ModelNo  as [Type of Vehicle],'' as [Authorisation No],CashReceiptNo as [Cash Receipt No.],convert(varchar, OrderDate, 105) as [Cash Receipt Date],'' as [Delay in no. of Days]  from CashReceipt where Date =   '" + OrderDate1 + "'  and HSRP_StateID='" + HSRPStateID + "' and RTOLocationID='" + RTOLocationID + "'";
        //      }

        //      DataSet dt = Utils.getDataSet(SQLString, CnnString);

        //      if (dt.Tables[0].Rows.Count > 0)
        //      {

        //          int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
        //          int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

        //          string filename = "DailyCustomerPlateAffixationReport-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
        //          Workbook book = new Workbook(); 
        //          book.ExcelWorkbook.ActiveSheetIndex = 1;
        //          book.ExcelWorkbook.WindowTopX = 100;
        //          book.ExcelWorkbook.WindowTopY = 200;
        //          book.ExcelWorkbook.WindowHeight = 7000;
        //          book.ExcelWorkbook.WindowWidth = 8000; 
        //          book.Properties.Author = "HSRP";
        //          book.Properties.Title = "HSRP Daily Rejection Report";
        //          book.Properties.Created = DateTime.Now; 

        //          WorksheetStyle style = book.Styles.Add("HeaderStyle");
        //          style.Font.FontName = "Tahoma";
        //          style.Font.FontName = "Tahoma";
        //          style.Font.Size = 10;
        //          style.Font.Bold = true;
        //          style.Alignment.Horizontal = StyleHorizontalAlignment.Center;


        //          WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
        //          style5.Font.FontName = "Tahoma";
        //          style5.Font.Size = 10;
        //          style5.Font.Bold = false;
        //          style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        //          style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //          style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //          style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //          style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


        //          style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
        //          style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        //          style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
        //          style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

        //          WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
        //          style2.Font.FontName = "Tahoma";
        //          style2.Font.Size = 10;
        //          style2.Font.Bold = true;
        //          style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;

        //          WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
        //          style3.Font.FontName = "Tahoma";
        //          style3.Font.Size = 12;
        //          style3.Font.Bold = true;
        //          style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

        //          Worksheet sheet = book.Worksheets.Add("HSRP Daily Customer Plate Affixation Report");

        //          sheet.Table.Columns.Add(new WorksheetColumn(50));
        //          sheet.Table.Columns.Add(new WorksheetColumn(120));
        //          sheet.Table.Columns.Add(new WorksheetColumn(130));
        //          sheet.Table.Columns.Add(new WorksheetColumn(110));
        //          sheet.Table.Columns.Add(new WorksheetColumn(115));
        //          sheet.Table.Columns.Add(new WorksheetColumn(130));
        //          sheet.Table.Columns.Add(new WorksheetColumn(115));
        //          sheet.Table.Columns.Add(new WorksheetColumn(130));
        //          WorksheetRow row = sheet.Table.Rows.Add();
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
        //          row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3")); 
        //          WorksheetCell cell = row.Cells.Add("HSRP Daily Plate Affixation Report Customer-Wise ");
        //          cell.MergeAcross = 3;            // Merge two cells together
        //          cell.StyleID = "HeaderStyle3";

        //          row = sheet.Table.Rows.Add();

        //          row.Index = 3;
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2")); 
        //          row = sheet.Table.Rows.Add();

        //          row.Index = 4;
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));
        //          //row.Cells.Add(<br>); 
        //          row = sheet.Table.Rows.Add();

        //          // Skip one row, and add some text
        //          row.Index = 5;

        //          DateTime date = System.DateTime.Now;
        //          string formatted = date.ToString("dd/MM/yyyy");

        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
        //          // row.Cells.Add(new WorksheetCell(DateTime.Now.ToShortDateString(), "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell(formatted, "HeaderStyle2"));

        //          row = sheet.Table.Rows.Add();
        //          row.Index = 6;
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell("Report Date:", "HeaderStyle2"));
        //          //row.Cells.Add(new WorksheetCell(From1.ToShortDateString(), "HeaderStyle2"));
        //          row.Cells.Add(new WorksheetCell(ReportDate, "HeaderStyle2"));
        //          row = sheet.Table.Rows.Add();


        //          row.Index = 8;
        //          //row.Cells.Add("Order Date");
        //          row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
        //          row.Cells.Add(new WorksheetCell("Name of Customer", "HeaderStyle"));
        //          row.Cells.Add(new WorksheetCell("Registration No.", "HeaderStyle"));
        //          row.Cells.Add(new WorksheetCell("Type of Vechicle", "HeaderStyle"));
        //          row.Cells.Add(new WorksheetCell("Authorization No.", "HeaderStyle"));
        //          row.Cells.Add(new WorksheetCell("Cash Receipt No.", "HeaderStyle"));
        //          row.Cells.Add(new WorksheetCell("Cash Receipt Date", "HeaderStyle"));
        //          //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));
        //          row = sheet.Table.Rows.Add(); 
        //          String StringField = String.Empty;
        //          String StringAlert = String.Empty;


        //          if (dt.Tables[0].Rows.Count <= 0)
        //          {
        //              LabelError.Text = string.Empty;
        //              LabelError.Text = "Their is no selected data for the selected  date range.";
        //              return;
        //          }
        //          row.Index = 9; 
        //          int sno = 0;

        //          foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
        //          {
        //              sno = sno + 1;
        //              row = sheet.Table.Rows.Add();
        //              row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Name of Customer"].ToString(), DataType.String, "HeaderStyle"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Regn. No."].ToString(), DataType.String, "HeaderStyle5"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Type of Vehicle"].ToString(), DataType.String, "HeaderStyle"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Authorisation No"].ToString(), DataType.String, "HeaderStyle5"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Cash Receipt No."].ToString(), DataType.String, "HeaderStyle5"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Cash Receipt Date"].ToString(), DataType.String, "HeaderStyle"));
        //              row.Cells.Add(new WorksheetCell(dtrows["Delay in no. of Days"].ToString(), DataType.String, "HeaderStyle5")); 
        //          } 
        //          row = sheet.Table.Rows.Add();
        //          HttpContext context = HttpContext.Current;
        //          context.Response.Clear();
        //          // Save the file and open it
        //          book.Save(Response.OutputStream);

        //          //context.Response.ContentType = "text/csv";
        //          context.Response.ContentType = "application/vnd.ms-excel";

        //          context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //          context.Response.End();
        //      }
        //      else
        //      { 
        //          string closescript1 = "<script>alert('No records found for selected Date.')</script>";
        //          Page.RegisterStartupScript("abc", closescript1);
        //          return;
        //      }
        //    }

        //    catch (Exception ex)
        //    {
        //        LabelError.Visible = true;
        //        LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
        //    }
        //}
    }

}