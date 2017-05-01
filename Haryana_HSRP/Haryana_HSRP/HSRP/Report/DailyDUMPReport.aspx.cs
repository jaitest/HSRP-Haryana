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
    public partial class DailyDUMPReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string ConnectionStringRTO = string.Empty;
        string HSRPStateID = string.Empty;
        // int RTOLocationID;
        int intHSRPStateID;
        // int intRTOLocationID;
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
                ConnectionStringRTO = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringRTO"].ToString();
                
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
                            //labelOrganization.Visible = true;
                            //DropDownListStateName.Visible = true;

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



        string FromDate, ToDate;
        DataSet ds = new DataSet();
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From +" 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1  +" 23:59:59";


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "DailyDUMPReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily DUMP Report";
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

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style18 = book.Styles.Add("HeaderStyle18");
                style18.Font.FontName = "Tahoma";
                style18.Font.Size = 10;
                style18.Font.Bold = false;
                style18.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                style5.Font.Bold = true;
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

                Worksheet sheet = book.Worksheets.Add("Daily DUMP Report");
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
                 

                row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("DUMP Data Report");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.Text, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                
                row.Index = 7; 
                row.Cells.Add(new WorksheetCell("SNo.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Authorization No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Authorization Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cash Receipt No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Receipt Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("VEHICLE TYPE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Embossing Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front Laser Code", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear Laser Code", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Closed Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Status", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Record Type", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();
                //String StringField = String.Empty;
                //String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);


                //string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                //Utils.ExecNonQuery(upsqlstring1, CnnString);

               // SQLString = "SELECT    b.ProductColor, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";

                SQLString = "select  (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID) as locationname,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as EntryDate,HSRPRecord_AuthorizationNo as AuthorizationNo,CONVERT(varchar(12),HSRPRecord_AuthorizationDate,106) as AuthorizationDate, VehicleRegNo,OwnerName,MobileNo,CashReceiptNo,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as ReceiptDate, CONVERT(varchar(12),OrderEmbossingDate,106) as Embossingdate,VEHICLETYPE,HSRP_Front_LaserCode,HSRP_Rear_LaserCode, CONVERT(varchar(12),OrderClosedDate,106) as OrderClosedDate,OrderStatus,RoundOff_NetAmount as amount,oldorderid,addrecordby from HSRPRecords a where  HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and HSRPRecord_CreationDate between '" + FromDate + "' and '" + ToDate + "' and rtolocationid !=0   order by (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID),HSRPRecord_CreationDate";
                dt = Utils.GetDataTable(SQLString, CnnString);

                Int64 totalamount = 0;
                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["AuthorizationNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["AuthorizationDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["EntryDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashReceiptNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ReceiptDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VEHICLETYPE"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["Embossingdate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["amount"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["addrecordby"].ToString(), DataType.String, "HeaderStyle1"));
                        totalamount = totalamount + checked1(dtrows["amount"].ToString());
                    }

                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("TOTAL ", DataType.String, "HeaderStyle18"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));                    
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(totalamount.ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle1"));


                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

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

        protected void dealerreport()
        {
            try
            {
                LabelError.Text = "";

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "DailyDUMPReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily DUMP Report";
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

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style18 = book.Styles.Add("HeaderStyle18");
                style18.Font.FontName = "Tahoma";
                style18.Font.Size = 10;
                style18.Font.Bold = false;
                style18.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                style5.Font.Bold = true;
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

                Worksheet sheet = book.Worksheets.Add("Daily DUMP Report");
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


                row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("DUMP Data Report");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.Text, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 7;
                row.Cells.Add(new WorksheetCell("SNo.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Authorization No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Authorization Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Cash Receipt No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Receipt Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("VEHICLE TYPE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Embossing Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Front Laser Code", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear Laser Code", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Closed Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Status", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();
                //String StringField = String.Empty;
                //String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);


                //string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                //Utils.ExecNonQuery(upsqlstring1, CnnString);

                // SQLString = "SELECT    b.ProductColor, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";

                SQLString = "select  (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID) as locationname,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as EntryDate,HSRPRecord_AuthorizationNo as AuthorizationNo,CONVERT(varchar(12),HSRPRecord_AuthorizationDate,106) as AuthorizationDate, VehicleRegNo,OwnerName,MobileNo,CashReceiptNo,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as ReceiptDate, CONVERT(varchar(12),OrderEmbossingDate,106) as Embossingdate,VEHICLETYPE,HSRP_Front_LaserCode,HSRP_Rear_LaserCode, CONVERT(varchar(12),OrderClosedDate,106) as OrderClosedDate,OrderStatus,RoundOff_NetAmount as amount,oldorderid from HSRPRecords a where  HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and HSRPRecord_CreationDate between '" + FromDate + "' and '" + ToDate + "' and rtolocationid !=0  and Addrecordby='Dealer' order by (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID),HSRPRecord_CreationDate";
                dt = Utils.GetDataTable(SQLString, CnnString);

                Int64 totalamount = 0;
                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["AuthorizationNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["AuthorizationDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["EntryDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["CashReceiptNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ReceiptDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VEHICLETYPE"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["Embossingdate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderStatus"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["amount"].ToString(), DataType.String, "HeaderStyle1"));
                        totalamount = totalamount + checked1(dtrows["amount"].ToString());
                    }
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("TOTAL", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle18"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle18"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle18"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(totalamount.ToString(), DataType.String, "HeaderStyle1"));
                   


                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

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

        protected void btnDealer_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";
                dealerreport();
                //    String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                //    string MonTo = ("0" + StringAuthDate[0]);
                //    string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                //    String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                //    String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //    //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                //    string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                //    String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                //    string Mon = ("0" + StringOrderDate[0]);
                //    string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                //    string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                //    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //    //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                //    string ToDate = From1 + " 23:59:59";


                //    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //    //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //    DataTable StateName;
                //    DataTable dts;
                //    DataTable dt = new DataTable();
                //    DataTable dt1 = new DataTable();
                //    DataTable dt2 = new DataTable();

                //    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //    //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                //    string filename = "DailyDUMPReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                //    Workbook book = new Workbook();

                //    // Specify which Sheet should be opened and the size of window by default
                //    book.ExcelWorkbook.ActiveSheetIndex = 1;
                //    book.ExcelWorkbook.WindowTopX = 100;
                //    book.ExcelWorkbook.WindowTopY = 200;
                //    book.ExcelWorkbook.WindowHeight = 7000;
                //    book.ExcelWorkbook.WindowWidth = 8000;

                //    // Some optional properties of the Document
                //    book.Properties.Author = "HSRP";
                //    book.Properties.Title = "Daily DUMP Report";
                //    book.Properties.Created = DateTime.Now;


                //    // Add some styles to the Workbook
                //    WorksheetStyle style = book.Styles.Add("HeaderStyle");
                //    style.Font.FontName = "Tahoma";
                //    style.Font.Size = 10;
                //    style.Font.Bold = true;
                //    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                //    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                //    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                //    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                //    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                //    WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                //    style4.Font.FontName = "Tahoma";
                //    style4.Font.Size = 10;
                //    style4.Font.Bold = false;
                //    style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                //    style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                //    style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                //    style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                //    style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                //    WorksheetStyle style18 = book.Styles.Add("HeaderStyle18");
                //    style18.Font.FontName = "Tahoma";
                //    style18.Font.Size = 10;
                //    style18.Font.Bold = false;
                //    style18.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                //    style18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                //    style18.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                //    style18.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                //    style18.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                //    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                //    style6.Font.FontName = "Tahoma";
                //    style6.Font.Size = 10;
                //    style6.Font.Bold = true;
                //    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                //    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                //    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                //    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                //    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                //    WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                //    style8.Font.FontName = "Tahoma";
                //    style8.Font.Size = 10;
                //    style8.Font.Bold = true;
                //    style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                //    style8.Interior.Color = "#D4CDCD";
                //    style8.Interior.Pattern = StyleInteriorPattern.Solid;

                //    WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                //    style5.Font.FontName = "Tahoma";
                //    style5.Font.Size = 10;
                //    style5.Font.Bold = true;
                //    style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                //    style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                //    style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                //    style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                //    style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                //    WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                //    style2.Font.FontName = "Tahoma";
                //    style2.Font.Size = 10;
                //    style2.Font.Bold = true;
                //    style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                //    WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                //    style3.Font.FontName = "Tahoma";
                //    style3.Font.Size = 12;
                //    style3.Font.Bold = true;
                //    style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                //    Worksheet sheet = book.Worksheets.Add("Daily DUMP Report-DealerData");
                //    sheet.Table.Columns.Add(new WorksheetColumn(60));
                //    sheet.Table.Columns.Add(new WorksheetColumn(205));
                //    sheet.Table.Columns.Add(new WorksheetColumn(100));
                //    sheet.Table.Columns.Add(new WorksheetColumn(130));

                //    sheet.Table.Columns.Add(new WorksheetColumn(100));
                //    sheet.Table.Columns.Add(new WorksheetColumn(120));
                //    sheet.Table.Columns.Add(new WorksheetColumn(112));
                //    sheet.Table.Columns.Add(new WorksheetColumn(109));
                //    sheet.Table.Columns.Add(new WorksheetColumn(105));
                //    sheet.Table.Columns.Add(new WorksheetColumn(160));

                //    WorksheetRow row = sheet.Table.Rows.Add();


                //    row = sheet.Table.Rows.Add();


                //    row.Index = 2;
                //    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                //    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                //    WorksheetCell cell = row.Cells.Add("DUMP Data Report");
                //    cell.MergeAcross = 3; // Merge two cells togetherto 
                //    cell.StyleID = "HeaderStyle3";

                //    row = sheet.Table.Rows.Add();


                //    row.Index = 3;
                //    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.Text, "HeaderStyle2"));

                //    row = sheet.Table.Rows.Add();
                //    //  Skip one row, and add some text
                //    row.Index = 4;

                //    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                //    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                //    row = sheet.Table.Rows.Add();
                //    row.Index = 5;
                //    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                //    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                //    row = sheet.Table.Rows.Add();

                //    row.Index = 7;
                //    row.Cells.Add(new WorksheetCell("SNo.", "HeaderStyle"));
                //    row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                //    row.Cells.Add(new WorksheetCell("Entry Date", "HeaderStyle6"));
                //    row.Cells.Add(new WorksheetCell("Authorization No", "HeaderStyle"));
                //    row.Cells.Add(new WorksheetCell("Authorization Date", "HeaderStyle"));
                //    row.Cells.Add(new WorksheetCell("Vehicle Reg. No", "HeaderStyle"));
                //    row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle6"));
                //    row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle6"));
                //      row.Cells.Add(new WorksheetCell("VEHICLE TYPE", "HeaderStyle"));
                //      row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                //    row = sheet.Table.Rows.Add();
                //    //String StringField = String.Empty;
                //    //String StringAlert = String.Empty;

                //    //row.Index = 9;

                //    UserType = Convert.ToInt32(Session["UserType"]);


                //    //string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                //    //Utils.ExecNonQuery(upsqlstring1, CnnString);

                //    // SQLString = "SELECT    b.ProductColor, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";

                //   // SQLString = "select  (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID) as locationname,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as EntryDate,HSRPRecord_AuthorizationNo as AuthorizationNo,CONVERT(varchar(12),HSRPRecord_AuthorizationDate,106) as AuthorizationDate, VehicleRegNo,OwnerName,MobileNo,CashReceiptNo,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as ReceiptDate, CONVERT(varchar(12),OrderEmbossingDate,106) as Embossingdate,VEHICLETYPE,HSRP_Front_LaserCode,HSRP_Rear_LaserCode, CONVERT(varchar(12),OrderClosedDate,106) as OrderClosedDate,OrderStatus,RoundOff_NetAmount as amount,oldorderid from HSRPRecords a where  HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and HSRPRecord_CreationDate between '" + FromDate + "' and '" + ToDate + "'  order by (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID),HSRPRecord_CreationDate";
                //    SQLString = " select  (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID) as locationname,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as EntryDate,HSRPRecord_AuthorizationNo as AuthorizationNo,CONVERT(varchar(12),HSRPRecord_AuthorizationDate,106) as AuthorizationDate, VehicleRegNo,OwnerName,MobileNo,VEHICLETYPE, exshowroom_Amount as amount from vendor_HSRPRecords a where HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and HSRPRecord_AuthorizationDate between '" + FromDate + "'  and '" + ToDate + "' order by (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID),HSRPRecord_AuthorizationDate";
                //    dt = Utils.GetDataTable(SQLString, CnnString);

                //    string RTOColName = string.Empty;
                //    int sno = 0;
                //    if (dt.Rows.Count > 0)
                //    {
                //        foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                //        {
                //            sno = sno + 1;
                //            row = sheet.Table.Rows.Add();
                //            row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                //            row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle18"));
                //            row.Cells.Add(new WorksheetCell(dtrows["EntryDate"].ToString(), DataType.String, "HeaderStyle18"));
                //            row.Cells.Add(new WorksheetCell(dtrows["AuthorizationNo"].ToString(), DataType.String, "HeaderStyle1"));
                //            row.Cells.Add(new WorksheetCell(dtrows["AuthorizationDate"].ToString(), DataType.String, "HeaderStyle1")); 
                //            row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                //            row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle18"));
                //            row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle1"));
                //            row.Cells.Add(new WorksheetCell(dtrows["VEHICLETYPE"].ToString(), DataType.String, "HeaderStyle18"));
                //            row.Cells.Add(new WorksheetCell(dtrows["amount"].ToString(), DataType.String, "HeaderStyle1"));
                //        }
                //        row = sheet.Table.Rows.Add();
                //        HttpContext context = HttpContext.Current;
                //        context.Response.Clear();
                //        book.Save(Response.OutputStream);
                //        context.Response.ContentType = "application/vnd.ms-excel";
                //        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                //        context.Response.End();
                //    }
            }
            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }



        protected void btnExpence_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "DailyDUMPReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily DUMP Report";
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

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style18 = book.Styles.Add("HeaderStyle18");
                style18.Font.FontName = "Tahoma";
                style18.Font.Size = 10;
                style18.Font.Bold = false;
                style18.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                style5.Font.Bold = true;
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

                Worksheet sheet = book.Worksheets.Add("Daily DUMP Report");
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


                row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("DUMP Data Report");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.Text, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 7;
                row.Cells.Add(new WorksheetCell("SNo.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("State", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Location", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Expense Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Creation Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Bill No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Bill Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vat Amount", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Bill Amount", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vendor Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Remarks", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Claimed By", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Varified Amount", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Varified Remarks", "HeaderStyle")); 
                row.Cells.Add(new WorksheetCell("Varified Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Expense Status", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Verified Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("ExpenseID ", "HeaderStyle6"));
               
                row = sheet.Table.Rows.Add();
                //String StringField = String.Empty;
                //String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);


               //SQLString = "select  (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID) as locationname,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as EntryDate,HSRPRecord_AuthorizationNo as AuthorizationNo,CONVERT(varchar(12),HSRPRecord_AuthorizationDate,106) as AuthorizationDate, VehicleRegNo,OwnerName,MobileNo,CashReceiptNo,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as ReceiptDate, CONVERT(varchar(12),OrderEmbossingDate,106) as Embossingdate,VEHICLETYPE,HSRP_Front_LaserCode,HSRP_Rear_LaserCode, CONVERT(varchar(12),OrderClosedDate,106) as OrderClosedDate,OrderStatus,RoundOff_NetAmount as amount,oldorderid from HSRPRecords a where  HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and HSRPRecord_CreationDate between '" + FromDate + "' and '" + ToDate + "'  order by (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID),HSRPRecord_CreationDate";
                SQLString = "select a.ExpenseSaveID, (select  hsrpstatename from hsrpstate where hsrp_stateid=a.hsrp_stateid)State,(select RTOLocationName from RTOLocation where RTOLocationID=a.LocationID) as locationname,b.expensename,billno,CONVERT(varchar(12),billdate,106) as billdate,vatamount,billamount,vendorname,remarks,ClaimedBy,verifiedamount,verifiedremarks,CONVERT(varchar(12),verifieddate,106) as verifieddate,CONVERT(varchar(12),entrydate,106) as entrydate,expensestatus,(select userfirstname from users where userid=verifiedby) verifiedName from Expensesave a inner join Expensemaster as b on a.expenseid=b.expenceid where a.hsrp_stateid ='" + DropDownListStateName.SelectedValue + "' and a.billdate between '" + FromDate + "' and '" + ToDate + "' order by (select  hsrpstatename from hsrpstate where hsrp_stateid=a.hsrp_stateid),(select RTOLocationName from RTOLocation where RTOLocationID=a.LocationID)";
                dt = Utils.GetDataTable(SQLString, CnnString);

                Int64 billAmount = 0;
                Int64 vatAmount = 0;
                Int64 VerifiedAmount = 0;
                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                        
                        row.Cells.Add(new WorksheetCell(dtrows["State"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["locationname"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["expensename"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["entrydate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["billno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["billdate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["vatamount"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["billamount"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["vendorname"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["remarks"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["claimedby"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["verifiedamount"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["verifiedremarks"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["verifieddate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["expensestatus"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["verifiedname"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ExpenseSaveID"].ToString(), DataType.String, "HeaderStyle1"));
                        billAmount = billAmount + checked1(dtrows["billamount"].ToString());
                        VerifiedAmount = VerifiedAmount + checked1(dtrows["vatamount"].ToString());
                        vatAmount = vatAmount + checked1(dtrows["verifiedamount"].ToString());
                    }

                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("TOTAL", DataType.String, "HeaderStyle1"));

                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle18"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle18"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));

                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(vatAmount.ToString(), DataType.String, "HeaderStyle18"));
                    row.Cells.Add(new WorksheetCell(billAmount.ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(VerifiedAmount.ToString(), DataType.String, "HeaderStyle18"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                     
                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

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

        protected void HHTData_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "DailyDUMPReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily DUMP Report";
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

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style18 = book.Styles.Add("HeaderStyle18");
                style18.Font.FontName = "Tahoma";
                style18.Font.Size = 10;
                style18.Font.Bold = false;
                style18.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                style5.Font.Bold = true;
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

                Worksheet sheet = book.Worksheets.Add("Daily HHT Report");
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


                row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HHT Data Report");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.Text, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 7;
                row.Cells.Add(new WorksheetCell("SNo.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Location Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Machine Id", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Bill No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Collecation Date", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Bill Amount", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Vendor Name", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Remarks", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Claimed By", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Varified Amount", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Varified Remarks", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Varified Date", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Expense Status", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Verified Name", "HeaderStyle6"));

                row = sheet.Table.Rows.Add();
              
                UserType = Convert.ToInt32(Session["UserType"]);
                
                //SQLString = "select  (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID) as locationname,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as EntryDate,HSRPRecord_AuthorizationNo as AuthorizationNo,CONVERT(varchar(12),HSRPRecord_AuthorizationDate,106) as AuthorizationDate, VehicleRegNo,OwnerName,MobileNo,CashReceiptNo,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as ReceiptDate, CONVERT(varchar(12),OrderEmbossingDate,106) as Embossingdate,VEHICLETYPE,HSRP_Front_LaserCode,HSRP_Rear_LaserCode, CONVERT(varchar(12),OrderClosedDate,106) as OrderClosedDate,OrderStatus,RoundOff_NetAmount as amount,oldorderid from HSRPRecords a where  HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and HSRPRecord_CreationDate between '" + FromDate + "' and '" + ToDate + "'  order by (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID),HSRPRecord_CreationDate";
                SQLString = "SELECT regid as Locationname,machine_id,regno as vehicleregno,(Prefix+convert(varchar(10),billno)) as Billno, CONVERT(varchar(12),dateandtime,106) as collecationdate,a.amount FROM collection_details a where a.void_bit ='Normal'  and dateandtime between '" + FromDate + "' and '" + ToDate + "' and stateid ='Delhi' order by a.regid";
                dt = Utils.GetDataTable(SQLString, ConnectionStringRTO);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                      //  row.Cells.Add(new WorksheetCell(dtrows["State"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["Locationname"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["machine_id"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["vehicleregno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Billno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["collecationdate"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["amount"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["vendorname"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["remarks"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["claimedby"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["verifiedamount"].ToString(), DataType.String, "HeaderStyle18"));
                        //row.Cells.Add(new WorksheetCell(dtrows["verifiedremarks"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["verifieddate"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["expensestatus"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["verifiedname"].ToString(), DataType.String, "HeaderStyle1"));
                    }


                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

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

        protected void HHTSummary_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "DailyDUMPReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Daily DUMP Report";
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

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style18 = book.Styles.Add("HeaderStyle18");
                style18.Font.FontName = "Tahoma";
                style18.Font.Size = 10;
                style18.Font.Bold = false;
                style18.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style18.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


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
                style5.Font.Bold = true;
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

                Worksheet sheet = book.Worksheets.Add("HHT Summary Report");
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


                row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("HHT Summary Report");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.Text, "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 4;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(From.ToString() + " - " + From1.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 5;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 7;
                row.Cells.Add(new WorksheetCell("SNo.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Location Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Vehicle Count", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Collecation", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Bill No", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Collecation Date", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Bill Amount", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Vendor Name", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Remarks", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Claimed By", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Varified Amount", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Varified Remarks", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Varified Date", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Expense Status", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Verified Name", "HeaderStyle6"));

                row = sheet.Table.Rows.Add();

                UserType = Convert.ToInt32(Session["UserType"]);

                //SQLString = "select  (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID) as locationname,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as EntryDate,HSRPRecord_AuthorizationNo as AuthorizationNo,CONVERT(varchar(12),HSRPRecord_AuthorizationDate,106) as AuthorizationDate, VehicleRegNo,OwnerName,MobileNo,CashReceiptNo,CONVERT(varchar(12),HSRPRecord_CreationDate,106) as ReceiptDate, CONVERT(varchar(12),OrderEmbossingDate,106) as Embossingdate,VEHICLETYPE,HSRP_Front_LaserCode,HSRP_Rear_LaserCode, CONVERT(varchar(12),OrderClosedDate,106) as OrderClosedDate,OrderStatus,RoundOff_NetAmount as amount,oldorderid from HSRPRecords a where  HSRP_StateID ='" + DropDownListStateName.SelectedValue + "' and HSRPRecord_CreationDate between '" + FromDate + "' and '" + ToDate + "'  order by (select RTOLocationName from RTOLocation where RTOLocationID=a.RTOLocationID),HSRPRecord_CreationDate";
                SQLString = "SELECT regid as Locationname,count(regno) as vehcount, sum(a.amount) as collecation FROM collection_details a where a.void_bit ='Normal'  and dateandtime between '" + FromDate + "' and '" + ToDate + "' and stateid ='Delhi' group by a.regid order by a.regid";
                dt = Utils.GetDataTable(SQLString, ConnectionStringRTO);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        //  row.Cells.Add(new WorksheetCell(dtrows["State"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["Locationname"].ToString(), DataType.String, "HeaderStyle18"));
                        row.Cells.Add(new WorksheetCell(dtrows["vehcount"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["collecation"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["Billno"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["collecationdate"].ToString(), DataType.String, "HeaderStyle18"));
                        //row.Cells.Add(new WorksheetCell(dtrows["amount"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["vendorname"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["remarks"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["claimedby"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["verifiedamount"].ToString(), DataType.String, "HeaderStyle18"));
                        //row.Cells.Add(new WorksheetCell(dtrows["verifiedremarks"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["verifieddate"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["expensestatus"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["verifiedname"].ToString(), DataType.String, "HeaderStyle1"));
                    }


                    row = sheet.Table.Rows.Add();
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    book.Save(Response.OutputStream);

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
    }
}