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
    public partial class HaryanaHHTReport : System.Web.UI.Page
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
                            //labelOrganization.Visible = true;
                            //DropDownListStateName.Visible = true;

                            FilldropDownListOrganization();
                            DropDownListStateName.SelectedValue = "4";   


                        }
                        else
                        {
                            FilldropDownListOrganization();

                            labelOrganization.Enabled = true;
                            DropDownListStateName.Enabled = true;
                            DropDownListStateName.SelectedValue = "4";                           
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
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' and HSRP_StateID in (4,5) Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID in (4,5) and ActiveStatus='Y' Order by HSRPStateName";
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
                String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                String ReportDateTo = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];
               
                String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = StringAuthDate[2].Split(' ')[0] + "/" + StringAuthDate[0] + "/" + StringAuthDate[1] + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];

                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To   " + DatePrint2;
                String ReportDatefrom = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];
                
                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = StringOrderDate[2].Split(' ')[0]+"/"+StringOrderDate[0] + "/" + StringOrderDate[1]  +" 00:00:00";

                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = DropDownListStateName.SelectedItem+"_HHT_Report_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = DropDownListStateName.SelectedItem+"HHT Report";
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

                Worksheet sheet = book.Worksheets.Add("Haryana HHT Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));

                WorksheetRow row = sheet.Table.Rows.Add();
             

                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add(DropDownListStateName.SelectedItem+"HHT Report");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 5;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("form :"+ReportDatefrom.ToString() , "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("To :"+ReportDateTo.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 6;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                
                row.Index = 8;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RTO Location", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date Time", "HeaderStyle6"));              
          
                row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Registration No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));                
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));                
                row.Cells.Add(new WorksheetCell("Machine Id", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Status", "HeaderStyle6"));

                // b.VehicleClass,b.VehicleType,b.OrderType,b.MobileNo,b.EngineNo, b.ChassisNo,
                row = sheet.Table.Rows.Add();
                //String StringField = String.Empty;
                //String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);

                //string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                //Utils.ExecNonQuery(upsqlstring1, CnnString);

                string Statename = string.Empty;
                if (DropDownListStateName.SelectedValue.ToString() == "4")
                {
                    Statename = "HARYANA";

                }
                else
                {
                    Statename = "MP";
                }

                if (Statename == "HARYANA" || Statename=="MP")
                {
                    //SQLString = "SELECT distinct [machine_id],billno, [hrsp] as AuthNo,[name] as Owner_name,[mobileno],[regno],[vclass],[vtype],[ttype] as OrderType,[regid] as RtoName,[amount],convert(varchar(25),[dateandtime],105)+' '+convert(varchar(25),[dateandtime],108) as [dateandtime],[void_bit] FROM [collection_details] where stateid ='"+Statename+"' and dateandtime between '" + ReportDate1 + "' and  '" + ReportDate2 + "' order by regid,dateandtime";
                    SQLString = "SELECT distinct [machine_id],billno, [hrsp] as AuthNo,[name] as Owner_name,[mobileno],[regno],[vclass],[vtype],[ttype] as OrderType,[regid] as RtoName,[amount],convert(varchar(25),[dateandtime],105)+' '+convert(varchar(25),[dateandtime],108) as [dateandtime],[void_bit] FROM [collection_details] "+
                        "where stateid ='" + Statename + "' and convert(date,dateandtime) between '" + OrderDate.SelectedDate + "' and  '" + HSRPAuthDate.SelectedDate + "' order by regid,dateandtime";
                }
                else
                {
                    return;
                }

                   
                
                dt = Utils.GetDataTable(SQLString, CnnString);

                // b.VehicleClass,b.VehicleType,b.OrderType,b.MobileNo,b.EngineNo, b.ChassisNo,

                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RtoName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["dateandtime"].ToString(), DataType.String, "HeaderStyle1"));  
                        
                      //  row.Cells.Add(new WorksheetCell(dtrows["AuthNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Owner_name"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["regno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["mobileno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["vclass"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["vtype"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["amount"].ToString(), DataType.String, "HeaderStyle1"));                    
                        row.Cells.Add(new WorksheetCell(dtrows["machine_id"].ToString(), DataType.String, "HeaderStyle1"));  
                        row.Cells.Add(new WorksheetCell(dtrows["void_bit"].ToString(), DataType.String, "HeaderStyle1"));  

                    }


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
                    LabelError.Text = "No Record Found";
                }




            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void btnVoidSummary_Click(object sender, EventArgs e)
        {
            try
            {
                LabelError.Text = "";
                String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                String ReportDateTo = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];

                String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = StringAuthDate[2].Split(' ')[0] + "/" + StringAuthDate[0] + "/" + StringAuthDate[1] + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];

                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To   " + DatePrint2;
                String ReportDatefrom = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = StringOrderDate[2].Split(' ')[0] + "/" + StringOrderDate[0] + "/" + StringOrderDate[1] + " 00:00:00";

                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "Haryana_HHT_void_Summary_Report_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                Workbook book = new Workbook();

                // Specify which Sheet should be opened and the size of window by default
                book.ExcelWorkbook.ActiveSheetIndex = 1;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;

                // Some optional properties of the Document
                book.Properties.Author = "HSRP";
                book.Properties.Title = "Haryana HHT Report";
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

                Worksheet sheet = book.Worksheets.Add("Haryana HHT Report");
                sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(205));
                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(130));

                sheet.Table.Columns.Add(new WorksheetColumn(100));
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(112));
                sheet.Table.Columns.Add(new WorksheetColumn(109));

                WorksheetRow row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Haryana HHT Report");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row.Index = 4;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 5;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("form :" + ReportDatefrom.ToString(), "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("To :" + ReportDateTo.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 6;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString(), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();

                row.Index = 8;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("RTO Location", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Date Time", "HeaderStyle6"));

                row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Registration No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Amount", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Machine Id", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Status", "HeaderStyle6"));

                // b.VehicleClass,b.VehicleType,b.OrderType,b.MobileNo,b.EngineNo, b.ChassisNo,
                row = sheet.Table.Rows.Add();
                //String StringField = String.Empty;
                //String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);

                //string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                //Utils.ExecNonQuery(upsqlstring1, CnnString);

                if (DropDownListStateName.SelectedItem.ToString().ToUpper() == "HARYANA")
                {
                    SQLString = "SELECT [hrsp] as AuthNo,[name] as Owner_name,[mobileno],[regno],[vclass],[vtype],[ttype] as OrderType,[regid] as RtoName,[amount],convert(varchar(25),[dateandtime],105)+' '+convert(varchar(25),[dateandtime],108) as [dateandtime],[machine_id],[void_bit] FROM [collection_details] where stateid ='HARYANA' and void_bit='void' and dateandtime between '" + ReportDate1 + "' and  '" + ReportDate2 + "' order by regid,dateandtime";
                }
                else
                {
                    return;
                }


                dt = Utils.GetDataTable(SQLString, CnnString);

                // b.VehicleClass,b.VehicleType,b.OrderType,b.MobileNo,b.EngineNo, b.ChassisNo,

                string RTOColName = string.Empty;
                int sno = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RtoName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["dateandtime"].ToString(), DataType.String, "HeaderStyle1"));

                        //  row.Cells.Add(new WorksheetCell(dtrows["AuthNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["Owner_name"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["regno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["mobileno"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["vclass"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["vtype"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["amount"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["machine_id"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["void_bit"].ToString(), DataType.String, "HeaderStyle1"));

                    }


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
                    LabelError.Text = "No Record Found";
                }



            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

    
    }
}