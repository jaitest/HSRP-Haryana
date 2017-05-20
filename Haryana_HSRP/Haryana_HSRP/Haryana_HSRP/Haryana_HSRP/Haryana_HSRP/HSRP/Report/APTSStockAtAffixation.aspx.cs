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

namespace HSRP.DLReports
{
    public partial class APTSStockAtAffixation : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;
        // int RTOLocationID;
        int intHSRPStateID;
        // int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        string FromDate, ToDate;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
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
                            //FilldropDownListClient();
                            //Getlocation();
                            labelClient.Visible = false;
                            dropDownListClient.Visible = true;


                        }
                        else
                        {
                            FilldropDownListOrganization();
                            //FilldropDownListClient();
                            //Getlocation();
                            labelClient.Visible = false;
                            dropDownListClient.Visible = true;
                        }
                    }
                    catch (Exception err)
                    {
                        err.ToString();
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
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "ALL");
            }
            else
            {
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "ALL");
                //DataSet dss = Utils.getDataSet(SQLString, CnnString);
                //dropDownListClient.DataSource = dss;
                //dropDownListClient.DataBind();
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

        protected void btnexport_Click(object sender, EventArgs e)
        {


            try
            {
                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";


                DateTime StartDate = Convert.ToDateTime(HSRPAuthDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //int.TryParse(dropDownListUser.SelectedValue, out intUserID);

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
               // int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //int.TryParse(dropDownListUser.SelectedValue, out UserID);

                string filename = "APTSStockAffaxiactionStatus" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP APTSStockAffaxiactionStatus";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Bank Transaction Report");
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
                WorksheetCell cell = row.Cells.Add("HSRP APTSStockAffaxiactionStatus");
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
                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));

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

                row.Index = 7;

                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));
    

                row.Cells.Add(new WorksheetCell("Order date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleRegNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OwnerName", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleType", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleClass", "HeaderStyle6"));   // Daily Embossing
                row.Cells.Add(new WorksheetCell("OrderType", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderStatus", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
              
                row.Cells.Add(new WorksheetCell("Front_Lasercode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear_Lasercode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front_size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear_size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderEmbossingDate", "HeaderStyle6"));
        
                row.Cells.Add(new WorksheetCell("RoundOff_NetAmount", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 8;
                int totalAmount = 0;

               // if (dropDownListClient.SelectedItem.Text == "ALL")
                //{
                if (ddlTCReport.SelectedItem.Text == "Pending for Affixation")
                {

                    //SQLString = "SELECT [TransactionID] ,convert(varchar, DepositDate, 103) AS DepositDate,[BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],convert(varchar, CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],[AccountNo] FROM [dbo].[BankTransaction] WHERE DepositDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  StateID='" + DropDownListStateName.SelectedValue.ToString() + "' order by DepositDate desc";
                    SQLString = "select a.rtoLocationname,b.hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.MobileNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,b.OrderEmbossingDate,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + FromDate + "' and '" + ToDate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and '" + dropDownListClient.SelectedValue.ToString() + "' and orderstatus='Embossing Done' order by a.rtoLocationname";
                    dt = Utils.GetDataTable(SQLString, CnnString);
                    int sno = 0;
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno = sno + 1;
                            row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));

                            row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                            row = sheet.Table.Rows.Add();
                        }

                        //row = sheet.Table.Rows.Add();


                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                        HttpContext context = HttpContext.Current;
                        context.Response.Clear();
                        // Save the file and open it
                        book.Save(Response.OutputStream);

                        //context.Response.ContentType = "text/csv";
                        context.Response.ContentType = "application/vnd.ms-excel";

                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        context.Response.End();
                    }
                    // }
                }
                else
                {
                    //SQLString = "SELECT [TransactionID] ,convert(varchar, DepositDate, 103) AS DepositDate,[BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],convert(varchar, CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],[AccountNo] FROM [dbo].[BankTransaction] WHERE DepositDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  StateID='" + DropDownListStateName.SelectedValue.ToString() + "' AND RTOLocation='" + dropDownListClient.SelectedValue.ToString() + "' order by DepositDate desc";
                    SQLString = "SELECT t.[TransactionID] ,convert(varchar, t.DepositDate, 103) AS DepositDate,m.[BankName],(select RTOLocationName from RTOLocation where rtolocationid=t.RTOLocation) as Center,t.[BranchName],t.[DepositAmount],t.[DepositBy],t.[StateID],t.[RTOLocation],convert(varchar, CurrentDate, 103) as CurrentDate,t.[BankSlipNo],t.[Remarks],t.[AccountNo] FROM [dbo].[BankTransaction] as t inner join bankmaster as m on t.BankName=m.id  WHERE t.DepositDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  t.StateID='" + DropDownListStateName.SelectedValue.ToString() + "' AND t.RTOLocation='" + dropDownListClient.SelectedValue.ToString() + "' order by t.DepositDate desc";


                    dt1 = Utils.GetDataTable(SQLString, CnnString);

                    if (dt1.Rows.Count > 0)
                    {

                        foreach (DataRow dtrows in dt1.Rows) // Loop over the rows.
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["TransactionID"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["DepositDate"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["ORDERMONTH"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["BankName"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Center"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["BranchName"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["DepositAmount"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["DepositBy"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["UserID"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["CurrentDate"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                            row.Cells.Add(new WorksheetCell(dtrows["BankSlipNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["AccountNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Remarks"].ToString(), DataType.String, "HeaderStyle"));

                            totalAmount = totalAmount + int.Parse(dtrows["DepositAmount"].ToString());

                            row = sheet.Table.Rows.Add();
                        }

                        //row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

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
            }






            catch (Exception ex)
            {
                throw ex;
            } 


        }
                  
       
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTCReport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnexportall_Click(object sender, EventArgs e)
        {

            try
            {
                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";


                DateTime StartDate = Convert.ToDateTime(HSRPAuthDate.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //int.TryParse(dropDownListUser.SelectedValue, out intUserID);

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                // int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //int.TryParse(dropDownListUser.SelectedValue, out UserID);

                string orderdate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00";
                string fromdate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

                string filename = "StockinHandAtAffixaction Center" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "HSRP Stock in Hand At Affixaction Center";
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

                Worksheet sheet = book.Worksheets.Add("HSRP Bank Transaction Report");
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
                WorksheetCell cell = row.Cells.Add("HSRP APTSStockAffaxiactionStatus");
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
                row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));

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

                row.Index = 7;

                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle6"));

                row.Cells.Add(new WorksheetCell("RTO Location", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Order date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleRegNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OwnerName", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleType", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("VehicleClass", "HeaderStyle6"));   // Daily Embossing
                row.Cells.Add(new WorksheetCell("MobileNo", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front_Lasercode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear_Lasercode", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front_size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear_size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("OrderEmbossingDate", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RoundOff_NetAmount", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                row.Index = 8;
                int totalAmount = 0;

                // if (dropDownListClient.SelectedItem.Text == "ALL")
                //{
                if (ddlTCReport.SelectedItem.Text == "Pending for Affixation")
                {

                    //SQLString = "SELECT [TransactionID] ,convert(varchar, DepositDate, 103) AS DepositDate,[BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],convert(varchar, CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],[AccountNo] FROM [dbo].[BankTransaction] WHERE DepositDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  StateID='" + DropDownListStateName.SelectedValue.ToString() + "' order by DepositDate desc";
                    SQLString = "select a.rtoLocationname,convert(varchar(20),b.hsrprecord_creationdate,103) as hsrprecord_creationdate,b.VehicleRegNo,b.OwnerName,b.VehicleType,b.VehicleClass,b.MobileNo,b.hsrp_front_lasercode,b.hsrp_rear_lasercode,(select productcode from product where product.productid=b.frontplatesize) Front_lasercode,(select productcode from product where product.productid=b.rearplatesize) Rear_lasercode,convert(varchar(20),b.OrderEmbossingDate,103) as OrderEmbossingDate ,b.RoundOff_NetAmount from hsrprecords b,rtolocation a where b.rtolocationid=a.rtolocationid and hsrprecord_creationdate between '" + orderdate + "' and '" + fromdate + "' and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and b.rtolocationid='" + dropDownListClient.SelectedValue.ToString() + "' and orderstatus='Embossing Done' order by a.rtoLocationname";
                    dt = Utils.GetDataTable(SQLString, CnnString);
                    int sno = 0;
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                        {
                            sno = sno + 1;
                            row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["rtoLocationname"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle5"));

                            row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrp_front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["hsrp_rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["Front_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["Rear_lasercode"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle5"));
                            row.Cells.Add(new WorksheetCell(dtrows["RoundOff_NetAmount"].ToString(), DataType.String, "HeaderStyle5"));

                            row = sheet.Table.Rows.Add();
                        }

                        //row = sheet.Table.Rows.Add();


                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle6"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

                        HttpContext context = HttpContext.Current;
                        context.Response.Clear();
                        // Save the file and open it
                        book.Save(Response.OutputStream);

                        //context.Response.ContentType = "text/csv";
                        context.Response.ContentType = "application/vnd.ms-excel";

                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        context.Response.End();
                    }
                    // }
                }
                else
                {
                    //SQLString = "SELECT [TransactionID] ,convert(varchar, DepositDate, 103) AS DepositDate,[BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],convert(varchar, CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],[AccountNo] FROM [dbo].[BankTransaction] WHERE DepositDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  StateID='" + DropDownListStateName.SelectedValue.ToString() + "' AND RTOLocation='" + dropDownListClient.SelectedValue.ToString() + "' order by DepositDate desc";
                    SQLString = "SELECT t.[TransactionID] ,convert(varchar, t.DepositDate, 103) AS DepositDate,m.[BankName],(select RTOLocationName from RTOLocation where rtolocationid=t.RTOLocation) as Center,t.[BranchName],t.[DepositAmount],t.[DepositBy],t.[StateID],t.[RTOLocation],convert(varchar, CurrentDate, 103) as CurrentDate,t.[BankSlipNo],t.[Remarks],t.[AccountNo] FROM [dbo].[BankTransaction] as t inner join bankmaster as m on t.BankName=m.id  WHERE t.DepositDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  t.StateID='" + DropDownListStateName.SelectedValue.ToString() + "' AND t.RTOLocation='" + dropDownListClient.SelectedValue.ToString() + "' order by t.DepositDate desc";


                    dt1 = Utils.GetDataTable(SQLString, CnnString);

                    if (dt1.Rows.Count > 0)
                    {

                        foreach (DataRow dtrows in dt1.Rows) // Loop over the rows.
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["TransactionID"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["DepositDate"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["ORDERMONTH"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["BankName"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Center"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["BranchName"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["DepositAmount"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["DepositBy"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["UserID"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["CurrentDate"].ToString(), DataType.String, "HeaderStyle"));
                            //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                            row.Cells.Add(new WorksheetCell(dtrows["BankSlipNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["AccountNo"].ToString(), DataType.String, "HeaderStyle"));
                            row.Cells.Add(new WorksheetCell(dtrows["Remarks"].ToString(), DataType.String, "HeaderStyle"));

                            totalAmount = totalAmount + int.Parse(dtrows["DepositAmount"].ToString());

                            row = sheet.Table.Rows.Add();
                        }

                        //row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Total :", "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell((totalAmount).ToString(), "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));

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
            }



            catch (Exception ex)
            {
                throw ex;
            } 


        }
    
    }
}

           

        
   

 
