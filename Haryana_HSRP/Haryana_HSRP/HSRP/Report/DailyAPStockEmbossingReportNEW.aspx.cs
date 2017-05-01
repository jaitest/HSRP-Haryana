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
    public partial class DailyAPStockEmbossingReportNEW : System.Web.UI.Page
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
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                      
                            FilldropDownListOrganization();
                           


                        }
                        else
                        {
                            FilldropDownListOrganization();
                            Label1.Visible = false;
                            ddllocation.Visible = false;
                            labelOrganization.Enabled = true;
                            DropDownListStateName.Enabled = false;
                            DropDownListStateName.SelectedValue = "9";
                           // FilldropDownListLocation();
                            //ddllocation.Visible = true;
                            //Label1.Visible = true;
                           
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
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='9' AND  ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='9' and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();

            }
        }
        
        private void FilldropDownListLocation()
        {
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation where Hsrp_StateId='9' and ActiveStatus='Y' order by RTOLocationName";
            Utils.PopulateDropDownList(ddllocation, SQLString.ToString(), CnnString, "--Select Location--");
        }

        private void FilldropDownListAllLocation()
        {
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation where Hsrp_StateId='9' order by RTOLocationName";
            Utils.PopulateDropDownList(ddllocation, SQLString.ToString(), CnnString, "--Select Location--");
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
                float totOpBal = 0;
                float totRecv = 0;
                float totProdc = 0;
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

                string filename = "Andhra Pradesh Affixing Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
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
                style.Font.Size = 10;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                WorksheetStyle style4 = book.Styles.Add("HeaderStyle1");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Font.Bold = false;
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                style4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                style6.Font.FontName = "Tahoma";
                style6.Font.Size = 10;
                style6.Font.Bold = true;
                style6.Alignment.Horizontal = StyleHorizontalAlignment.Left;
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
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                Worksheet sheet = book.Worksheets.Add("AP Data Affixing Report");
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
                
                row.Index = 1;
         
                row = sheet.Table.Rows.Add();


                row.Index = 2;
         
                row = sheet.Table.Rows.Add();


                row.Index = 3;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                //row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Daily Stock Report From Embossing Station");
                cell.MergeAcross =5; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                //row.Index = 4;
                //row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 5;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("Andhra Pradesh", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Unit Name :", "HeaderStyle2"));
                WorksheetCell cell5 = row.Cells.Add(ddllocation.SelectedItem.ToString());

                WorksheetCell cell6 = row.Cells.Add("Report Date: " + From.ToString() + " - " + From1.ToString());

                cell6.MergeAcross = 2; // Merge two cells togetherto 
                cell6.StyleID = "HeaderStyle2";

                cell5.MergeAcross = 1; // Merge two cells togetherto 
                cell5.StyleID = "HeaderStyle2";
                row = sheet.Table.Rows.Add();
                row.Index = 6;
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                //row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                //DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
              
               
                row = sheet.Table.Rows.Add();
             

              



                row.Index = 7;
                 
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("HSRP Size", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Opening Balance", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Sent to Affixing Station", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Closing Balance","HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Remarks", "HeaderStyle6"));
                
                
            
                String StringField = String.Empty;
                String StringAlert = String.Empty;


            

                UserType = Convert.ToInt32(Session["UserType"]);

               // string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
               // Utils.ExecNonQuery(upsqlstring1, CnnString);

                SQLString = @"select t.Product,sum(t.senttoaffixing)as senttoaffixing,sum(t.Closing) as Closing,sum(t.Opening) as Opening from (select convert(varchar(7),pd) as Product,sum(isnull([Embossing done],0)) as 'senttoaffixing',sum(isnull([New Order],0)) as 'Closing',sum(isnull([Embossing done],0))+sum(isnull([New Order],0)) as 'Opening' from(select * from (select ProductCode as pd,InventoryStatus as inn ,count(InventoryStatus) as rr from RTOInventory inner join Product p on RTOInventory.productid=p.productid  where RTOInventory.HSRP_StateID=999 and  RTOInventory.InventoryStatus in ('Embossing Done','New Order') and statusdate  between '" + FromDate + "' and '" + ToDate + "' and RTOInventory.RTOLocationID='"+ddllocation.SelectedValue+"'  group by p.ProductCode,RTOInventory.InventoryStatus)  as kk PIVOT(sum(rr) for inn in([Embossing Done],[New Order])) as pivot55)newtable group by convert(varchar(7),pd) union select '200X100' as pd,'0' as senttoaffixing,'0' as closing,'0' as opening union select '340X200' as pd,'0' as senttoaffixing,'0' as closing,'0' as opening union select '285X45' as pd,'0' as senttoaffixing,'0' as closing,'0' as opening union select '500X120' as pd,'0' as senttoaffixing,'0' as closing,'0' as opening) T group by t.Product";
                dt = Utils.GetDataTable(SQLString, CnnString);


                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        totOpBal = totOpBal + Convert.ToInt64(dtrows["Opening"]);
                        totRecv = totRecv + Convert.ToInt64(dtrows["senttoaffixing"]);
                        totProdc = totProdc + Convert.ToInt64(dtrows["Closing"]);

                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["Product"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Opening"].ToString(), DataType.String, "HeaderStyle1"));
                        

                        row.Cells.Add(new WorksheetCell(dtrows["senttoaffixing"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Closing"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                       
                    }
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("5", DataType.String, "HeaderStyle1"));

                    row.Cells.Add(new WorksheetCell("TRP", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));


                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("6", DataType.String, "HeaderStyle1"));

                    row.Cells.Add(new WorksheetCell("Snap Locks Pair", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));


                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));


                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("Grand Total", DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(totOpBal.ToString(), DataType.String, "HeaderStyle1"));


                    row.Cells.Add(new WorksheetCell(totRecv.ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell(totProdc.ToString(), DataType.String, "HeaderStyle1"));
                    row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));


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

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label1.Visible = true;
            ddllocation.Visible = true;
           // FilldropDownListLocation();
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            DateTime date1 = OrderDate.SelectedDate;
            DateTime date2 = new DateTime(2014, 06, 02);
            int result = DateTime.Compare(date1, date2);
            if (result < 0)
            {
                Label1.Visible = true;
                ddllocation.Visible = true;
                btnExportToExcel.Visible = true;
                FilldropDownListAllLocation();
            }
            else
            {
                Label1.Visible = true;
                ddllocation.Visible = true;
                btnExportToExcel.Visible = true;
                FilldropDownListLocation();
            }
 
        }
    }
}