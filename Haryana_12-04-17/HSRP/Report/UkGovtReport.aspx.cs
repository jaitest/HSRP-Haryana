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
    public partial class UkGovtReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;
        int RTOLocationID;
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
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID =Convert.ToInt32(Session["UserRTOLocationID"].ToString());

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
                            FilldropDownListClient();


                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                            labelOrganization.Enabled = true;
                            DropDownListStateName.Enabled = true;
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
            string TodayDates = "12-10-2016";
            string Maxdates = "12-10-2016";
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(Maxdates);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(Maxdates);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDates)).AddDays(0.00);
            OrderDate.MinDate = (DateTime.Parse(TodayDates)).AddDays(-619.00);
        }
        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=6 and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='6' and ActiveStatus='Y' Order by HSRPStateName";
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
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                ddlRtoLocation.DataSource = dss;
                ddlRtoLocation.DataTextField = "RTOLocationName";
                ddlRtoLocation.DataValueField = "RTOLocationID";
                ddlRtoLocation.DataBind();
                ddlRtoLocation.Items.Insert(0, new ListItem("All", "0"));
                ddlRtoLocation.Items.Insert(0, new ListItem("--Select Location--", "--Select Location--"));
                
                //Utils.PopulateDropDownList(ddlRtoLocation, SQLString.ToString(), CnnString, "--Select Location--");
                //ddlRtoLocation.Items.Insert(0, new ListItem("All", "0"));

                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'  Order by RTOLocationName";
               // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " ) and ActiveStatus!='N'  Order by RTOLocationName";

                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                ddlRtoLocation.DataSource = dss;
                ddlRtoLocation.DataBind();

            }
        }

        #endregion



        string FromDate, ToDate;
        DataSet ds = new DataSet();
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";
                int ISum = 0;
                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string FromDate = From + " 00:00:01"; // Convert.ToDateTime();

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

                string filename = "UkGovtReport" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "UK Govt Report";
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

                Worksheet sheet = book.Worksheets.Add("UK Report");
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
                // row.
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("LINK UTSAV HSRP PVT. LTD");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

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
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Application No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Vehicle Registration No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("FrontLaser", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("HSRP Front Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("RearLaser", "HeaderStyle"));                
                row.Cells.Add(new WorksheetCell("HSRP Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("3rd Stiker", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Cash Receipt No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Cash Receipt Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Embossing Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Authorization Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Affixed or Not", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Stock Recd. At Center Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Colour Background", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Production Date", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Recieved At Affixation Center Date", "HeaderStyle"));

               
              
                //row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                UserType = Convert.ToInt32(Session["UserType"]);

               

                if (ddlRtoLocation.SelectedValue.ToString() != "0")
                {
                    //SQLString = "select a.vehicleregno,a.VehicleType,a.HSRPRecord_AuthorizationNo,convert(varchar,a.HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,a.cashreceiptno,a.roundoff_netamount,convert(date,a.hsrprecord_creationdate,103) as hsrprecord_creationdate,convert(varchar,a.OrderEmbossingDate,103) as OrderEmbossingDate,convert(varchar,a.RecievedAtAffixationDateTime,103) as RecievedAtAffixationDateTime,convert(varchar,a.ordercloseddate,103) as ordercloseddate from HSRPRecords a,rtolocation b where a.rtolocationid=b.rtolocationid and a.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and b.rtolocationid='" + ddlRtoLocation.SelectedValue + "' and convert(date,hsrprecord_creationdate) between '" + OrderDate.SelectedDate + "' and '" + HSRPAuthDate.SelectedDate + "' order by hsrprecord_creationdate ";
                    SQLString = "select a.HSRPRecord_AuthorizationNo,a.VehicleType,a.Ownername,a.vehicleregno,a.HSRP_Front_LaserCode,(select productcode from product where frontplatesize=productid) FrontPlateSize,HSRP_Rear_LaserCode,(select productcode from product where frontplatesize=productid) RearPlateSize,a.StickerMandatory,a.CashReceiptNo,a.roundoff_netamount,convert(varchar,a.hsrprecord_creationdate,103) as hsrprecord_creationdate,convert(varchar,a.OrderEmbossingDate,103) as OrderEmbossingDate,convert(varchar,a.HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,(case a.orderstatus when 'Closed' then 'Y' else 'N' end) as 'RecievedAffixationStatus',convert(varchar,a.RecievedAtAffixationDateTime,103) as RecievedAtAffixationDateTime,(select ProductColor from product where frontplatesize=productid) as 'Color' from HSRPRecords a,rtolocation b where a.rtolocationid=b.rtolocationid and a.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and b.rtolocationid='" + ddlRtoLocation.SelectedValue + "' and convert(date,hsrprecord_creationdate) between '" + OrderDate.SelectedDate + "' and '" + HSRPAuthDate.SelectedDate + "' order by hsrprecord_creationdate";
                    
                }
                else
                {
                    //SQLString = "select a.HSRPRecord_AuthorizationNo,a.VehicleType,a.Ownername,a.vehicleregno,a.HSRP_Front_LaserCode,(select productcode from product where frontplatesize=productid) FrontPlateSize,HSRP_Rear_LaserCode,(select productcode from product where frontplatesize=productid) RearPlateSize,a.StickerMandatory,a.CashReceiptNo,a.roundoff_netamount,convert(varchar,a.hsrprecord_creationdate,103) as hsrprecord_creationdate,convert(varchar,a.OrderEmbossingDate,103) as OrderEmbossingDate,convert(varchar,a.HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,(case a.orderstatus when 'Closed' then 'Y' else 'N' end) as 'RecievedAffixationStatus',convert(varchar,a.RecievedAtAffixationDateTime,103) as RecievedAtAffixationDateTime,(select ProductColor from product where frontplatesize=productid) as 'Color' from HSRPRecords a,rtolocation b where a.rtolocationid=b.rtolocationid and a.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and b.rtolocationid='" + ddlRtoLocation.SelectedValue + "' and convert(date,hsrprecord_creationdate) between '" + OrderDate.SelectedDate + "' and '" + HSRPAuthDate.SelectedDate + "' order by hsrprecord_creationdate";
                    SQLString = "select a.HSRPRecord_AuthorizationNo,a.VehicleType,a.Ownername,a.vehicleregno,a.HSRP_Front_LaserCode,(select productcode from product where frontplatesize=productid) FrontPlateSize,HSRP_Rear_LaserCode,(select productcode from product where frontplatesize=productid) RearPlateSize,a.StickerMandatory,a.CashReceiptNo,a.roundoff_netamount,convert(varchar,a.hsrprecord_creationdate,103) as hsrprecord_creationdate,convert(varchar,a.OrderEmbossingDate,103) as OrderEmbossingDate,convert(varchar,a.HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,(case a.orderstatus when 'Closed' then 'Y' else 'N' end) as 'RecievedAffixationStatus',convert(varchar,a.RecievedAtAffixationDateTime,103) as RecievedAtAffixationDateTime,(select ProductColor from product where frontplatesize=productid) as 'Color' from HSRPRecords a,rtolocation b where a.rtolocationid=b.rtolocationid and a.hsrp_stateid='" + DropDownListStateName.SelectedValue + "'  and convert(date,hsrprecord_creationdate) between '" + OrderDate.SelectedDate + "' and '" + HSRPAuthDate.SelectedDate + "' order by hsrprecord_creationdate";
                    //SQLString = "select a.vehicleregno,a.VehicleType,a.HSRPRecord_AuthorizationNo,convert(varchar,a.HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,a.cashreceiptno,a.roundoff_netamount,convert(date,a.hsrprecord_creationdate,103) as hsrprecord_creationdate,convert(varchar,a.OrderEmbossingDate,103) as OrderEmbossingDate,convert(varchar,a.RecievedAtAffixationDateTime,103) as RecievedAtAffixationDateTime,convert(varchar,a.ordercloseddate,103) as ordercloseddate from HSRPRecords a,rtolocation b where a.rtolocationid=b.rtolocationid and a.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and convert(date,hsrprecord_creationdate) between '" + OrderDate.SelectedDate + "' and '" + HSRPAuthDate.SelectedDate + "' order by hsrprecord_creationdate ";   
                }
                    
                dt = Utils.GetDataTable(SQLString, CnnString);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["vehicleregno"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["Hsrp_Front_laserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["Hsrp_Rear_laserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["RearPlateSize"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["cashreceiptno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["roundoff_netamount"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["hsrprecord_creationdate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["HSRPRecord_AuthorizationDate"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["RecievedAffixationStatus"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["RecievedAtAffixationDateTime"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dt.Rows[i]["Color"].ToString(), DataType.String, "HeaderStyle1"));

                        // row.Cells.Add(new WorksheetCell(dt.Rows[i]["ordercloseddate"].ToString(), DataType.String, "HeaderStyle1"));
                       // row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                       

                    }
                    LabelError.Text = "";
                    LabelError.Visible = false;


                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
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
                    LabelError.Visible = true;
                    LabelError.Text = "Record Not Found";
                }

            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void btnSummary_Click(object sender, EventArgs e)
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
                string FromDate = From + " 00:00:01"; // Convert.ToDateTime();

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

                string filename = "Rejection Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Rejection Summary";
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

                Worksheet sheet = book.Worksheets.Add("Rejection Summary Report");
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
                // row.
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Rejection Summary Report");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

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



                row.Index = 6;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Locations", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Rejection Entry Date", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Entry Type", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Rejection Type", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front Laser Code ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Rear Laser Code", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Vehicle Reg No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Remarks", "HeaderStyle"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);

                SQLString = "select rtolocationname,convert(varchar,b.entrydate,106) as RejectionDate,EntryType,rejectionType,frontlasercode,RearLasercode as Lasercode ,h.VehicleRegNo,b.reasonforrejection from rejectplatEntry b,hsrpdemo.dbo.RTOLocation c ,hsrprecords h where b.rtolocationid=c.RTOLocationID and b.OriginalRequestID=h.hsrprecordid and b.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and b.EntryDate between '" + FromDate + "' and '" + ToDate + "' order by 1,2 "; 
                dt = Utils.GetDataTable(SQLString, CnnString);

                int IFront = 0;
                int IRear = 0;
                
                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["rtolocationname"].ToString(), DataType.String, "HeaderStyle1"));
                        string RejectDate = (DateTime.Parse(dtrows["RejectionDate"].ToString())).ToString("MM/dd/yyyy");
                        row.Cells.Add(new WorksheetCell(RejectDate, DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["EntryType"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["rejectionType"].ToString(), DataType.String, "HeaderStyle1"));
                        if (string.IsNullOrEmpty(dtrows["frontlasercode"].ToString()))
                        {
                            IFront = IFront + 0;
                            row.Cells.Add(new WorksheetCell(dtrows["frontlasercode"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["frontlasercode"].ToString(), DataType.String, "HeaderStyle1"));
                            IFront = IFront + 1;
                        }
                        if (string.IsNullOrEmpty(dtrows["Lasercode"].ToString()))
                        {
                            IRear = IRear + 0;
                            row.Cells.Add(new WorksheetCell(dtrows["Lasercode"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["Lasercode"].ToString(), DataType.String, "HeaderStyle1"));
                            IRear = IRear + 1;
                        }
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["reasonforrejection"].ToString(), DataType.String, "HeaderStyle1"));
                        
                        
                    }


                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Total", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(IFront.ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(IRear.ToString(), DataType.String, "HeaderStyle1"));
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

       
        protected void DropDownListStateName_SelectedIndexChanged1(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }
    }
}