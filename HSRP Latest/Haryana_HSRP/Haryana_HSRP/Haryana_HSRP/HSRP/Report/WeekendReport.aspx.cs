using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HSRP;
using DataProvider;
using CarlosAg.ExcelXmlWriter;

namespace HSRP.Report
{
    public partial class WeekendReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty; 
        int intHSRPStateID;    
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

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        InitialSetting();
                        if (UserType.Equals(0))
                        {                           
                            FilldropDownListOrganization();
                        }
                        else
                        {
                            FilldropDownListOrganization();

                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
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
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }


        #endregion

        string FromDate, ToDate;
        DataSet ds = new DataSet();
        private void ExportExcel()
        {
            try
            {
                 LabelError.Text = "";            
            TimeSpan ts = HSRPAuthDate.SelectedDate - OrderDate.SelectedDate;
            if (ts.Days <= 7)
            {

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];

                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];

                string ToDate = From1 + " 23:59:59";


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                DataTable dtrows = new DataTable();
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                UserType = Convert.ToInt32(Session["UserType"]);
                SQLString = "SELECT * from Hsrp_Weekend_Report  WHERE  HSRP_StateID= '" + DropDownListStateName.SelectedValue.ToString() + "' AND Entry_Date  between '" + FromDate + "' and '" + ToDate + "' order by id desc ";
                dtrows = Utils.GetDataTable(SQLString, CnnString);

                string RTOColName = string.Empty;
                int sno = 0;
                if (dtrows.Rows.Count > 0)
                {
                    string filename = "Weekend Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Daily Embossing Report";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
                    WorksheetStyle style = book.Styles.Add("HeaderStyle");
                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 11;
                    style.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style01 = book.Styles.Add("HeaderStyle01");
                    style01.Font.FontName = "Tahoma";
                    style01.Font.Size = 10;
                    style01.Font.Bold = false;
                    style01.Alignment.Vertical = StyleVerticalAlignment.Center;
                    style01.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style01.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style01.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style01.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                    style4.Font.FontName = "Tahoma";
                    style4.Font.Size = 10;
                    style4.Font.Bold = false;
                    style4.Alignment.Vertical = StyleVerticalAlignment.Center;
                    style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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

                    Worksheet sheet = book.Worksheets.Add("Weekend Report");
                    sheet.Table.Columns.Add(new WorksheetColumn(60));
                    sheet.Table.Columns.Add(new WorksheetColumn(205));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(112));
                    sheet.Table.Columns.Add(new WorksheetColumn(109));
                    sheet.Table.Columns.Add(new WorksheetColumn(105));
                    sheet.Table.Columns.Add(new WorksheetColumn(160));

                    WorksheetRow row = sheet.Table.Rows.Add();
                    WorksheetCell cell = row.Cells.Add("Weekend Report");
                    cell.MergeAcross = 3; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();

                    row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    //  Skip one row, and add some text
                    row.Index = 4;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 5;
                    // row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));           
                    row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 7;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Status", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Amt", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();
                    row.Index = 8;
                    row.Cells.Add(new WorksheetCell("1", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Collection", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_collectionno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_collectionamt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 9;
                    row.Cells.Add(new WorksheetCell("2", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Embossed", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_embossedno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_embossedamt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 10;
                    row.Cells.Add(new WorksheetCell("3", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Dispatch", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_dispatchno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_dispatchamt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 11;
                    row.Cells.Add(new WorksheetCell("4", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Received", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_receivedno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_receivedamt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 12;
                    row.Cells.Add(new WorksheetCell("5", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Affixed", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_affixedno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_affixedamt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 13;
                    row.Cells.Add(new WorksheetCell("6", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Balance at AFC", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_balanceno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_balanceamt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 14;
                    row.Cells.Add(new WorksheetCell(" ", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("  ", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(" ", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("  ", "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 15;
                    row.Cells.Add(new WorksheetCell("7", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Manpower Count Upto (04.09.2016)", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_manpowerno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_manpoweramt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 16;
                    row.Cells.Add(new WorksheetCell("8", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Net Addition(+) / Reduction(-) On CTC", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_Netaddno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_Netaddamt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 17;
                    row.Cells.Add(new WorksheetCell("9", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Manpower As on Date", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_manpowerdateno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["hsrp_manpowerdateamt"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 18;
                    row.Cells.Add(new WorksheetCell(" ", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("  "));
                    row.Cells.Add(new WorksheetCell(" "));
                    row.Cells.Add(new WorksheetCell("  ", "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 19;
                    row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Status", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("No", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("%", "HeaderStyle"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 20;
                    row.Cells.Add(new WorksheetCell("10", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Failure - Embossing", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["FailureEmbno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["Failureembmodule"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 21;
                    row.Cells.Add(new WorksheetCell("11", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell("Failure - Received Invoicing", "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["Failurerecno"].ToString(), DataType.String, "HeaderStyle01"));
                    row.Cells.Add(new WorksheetCell(dtrows.Rows[0]["Failurerecmodule"].ToString(), DataType.String, "HeaderStyle01"));
                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);
                    context.Response.ContentType = "application/vnd.ms-excel";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();

                }
                else
                {
                    LabelError.Text = "Record Not Found";
                }
            }
                else
            {
                LabelError.Text = "Please Select Seven Days Difference Between Dates";
            }
            



            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        public void displayrecords()
        {
            try
            {

            LabelError.Text = "";            
            TimeSpan ts = HSRPAuthDate.SelectedDate - OrderDate.SelectedDate;
            if (ts.Days <= 7)
            {

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];

                string FromDate = From;///+ " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];

                string ToDate = From1;// +" 23:59:59";

                string SQLString1 = "exec bussiness_report_weekend '" + DropDownListStateName.SelectedValue.ToString() + "','" + FromDate + "','" + ToDate + "'";
                DataTable dtrecord = Utils.GetDataTable(SQLString1, CnnString);

                if (dtrecord.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dtrecord.Rows)
                    {
                        txtcollectionNo.Text = dtrow["Collectionno"].ToString();
                        txtcollectionamt.Text = dtrow["CollectionAmt"].ToString();
                        txtembno.Text = dtrow["EmbossedNo"].ToString();
                        txtembamt.Text = "NA";
                        txtdispatchno.Text = dtrow["DispatchNo"].ToString();
                        txtdispatchamt.Text = "NA";
                        txtrecno.Text = dtrow["Receivedno"].ToString();
                        txtrecamt.Text = "NA";
                        txtaffino.Text = dtrow["affixedno"].ToString();
                        txtaffiamt.Text = "NA";

                    }
                }
            }
            else
            {

                LabelError.Text = "Please Select Seven Days Difference Between Dates";
            }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            displayrecords();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
             TimeSpan ts = HSRPAuthDate.SelectedDate - OrderDate.SelectedDate;
             if (ts.Days <= 7)
             {
                 ExportExcel();
             }
             else
             {
                 LabelError.Text = "Please Select Seven Days Difference Between Dates";
             }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                lblmsg.Text = "";
            LabelError.Text = "";            
            TimeSpan ts = HSRPAuthDate.SelectedDate - OrderDate.SelectedDate;
            if (ts.Days <= 7)
            {
                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];

                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];

                string ToDate = From1 + " 23:59:59";

                if (txtbalanceno.Text.ToString().Trim() == "" || txtbalanceamt.Text.ToString().Trim() == "")
                {
                    lblmsg.Text = "Please Fill Balance No or Amt";
                    return;
                }

                if (txtmanpowerno.Text.ToString().Trim() == "" || txtmanpoweramt.Text.ToString().Trim() == "")
                {
                    lblmsg.Text = "Please Fill ManPower Count No or Amt";
                    return;
                }

                if (txtnetadditionno.Text.ToString().Trim() == "" || txtnetadditionamt.Text.ToString().Trim() == "")
                {
                    lblmsg.Text = "Please Fill Net Addition No or Amt";
                    return;
                }

                if (txtmanpowerdateno.Text.ToString().Trim() == "" || txtmanpowerdateamt.Text.ToString().Trim() == "")
                {
                    lblmsg.Text = "Man Power on Date";
                    return;
                }

                string sdate = ToDate.ToString();//DateTime.Now.ToString();
                string sql = "INSERT INTO Hsrp_Weekend_Report(HSRP_StateID,hsrp_collectionno,hsrp_collectionamt,hsrp_embossedno,hsrp_embossedamt,hsrp_dispatchno,hsrp_dispatchamt,hsrp_receivedno,hsrp_receivedamt,hsrp_affixedno,hsrp_affixedamt,hsrp_balanceno,hsrp_balanceamt,hsrp_manpowerno,hsrp_manpoweramt,hsrp_Netaddno,hsrp_Netaddamt,hsrp_manpowerdateno,hsrp_manpowerdateamt,FailureEmbno,Failureembmodule,Failurerecno,Failurerecmodule,Entry_Date)VALUES ('" + DropDownListStateName.SelectedValue.ToString() + "','" + txtcollectionNo.Text + "','" + txtcollectionamt.Text + "','" + txtembno.Text + "','" + txtembamt.Text + "','" + txtdispatchno.Text + "','" + txtdispatchamt.Text.ToString() + "','" + txtrecno.Text.ToString() + "','" + txtrecamt.Text.ToString() + "','" + txtaffino.Text.ToString() + "','" + txtaffiamt.Text.ToString() + "','" + txtbalanceno.Text.ToString() + "','" + txtbalanceamt.Text.ToString() + "','" + txtmanpowerno.Text.ToString() + "','" + txtmanpoweramt.Text.ToString() + "','" + txtnetadditionno.Text.ToString() + "','" + txtnetadditionamt.Text.ToString() + "','" + txtmanpowerdateno.Text.ToString() + "','" + txtmanpowerdateamt.Text.ToString() + "','" + txtfailureno.Text.ToString() + "','" + txtfailuremodule.Text.ToString() + "','" + txtfailurereceivedno.Text.ToString() + "','" + txtfailurereceivedmodule.Text.ToString() + "','" + sdate + "')";
                Utils.ExecNonQuery(sql, CnnString);
                clera();
                lblmsg.Text = "Record Save Successfully";
            }
            else
            {

                LabelError.Text = "Please Select Seven Days Difference Between Dates";
            }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void clera()
        {
            txtcollectionamt.Text = "";
            txtcollectionNo.Text = "";
            txtembamt.Text = "";
            txtembno.Text = "";
            txtdispatchamt.Text = "";
            txtdispatchno.Text = "";
            txtrecamt.Text = "";
            txtrecno.Text = "";
            txtaffiamt.Text = "";
            txtaffino.Text = "";
            txtbalanceamt.Text = "";
            txtbalanceno.Text = "";
            txtmanpoweramt.Text = "";
            txtmanpowerno.Text = "";
            txtmanpowerdateamt.Text = "";
            txtmanpowerdateno.Text = "";
            txtnetadditionamt.Text = "";
            txtnetadditionno.Text = "";
            txtfailureno.Text = "0";
            txtfailuremodule.Text = "0";
            txtfailurereceivedno.Text = "0";
            txtfailurereceivedmodule.Text = "0";
        }
      
    }
}