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
using System.IO;

namespace HSRP.BR1Reports
{
    public partial class HRMonthlyReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string strPath = string.Empty;
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
                    //InitialSetting();
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
            //string TodayDate = "04/01/2014 "; 
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string TodayDates = "04/01/2015";
          
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MinDate = (DateTime.Parse(TodayDates)).AddDays(-0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MinDate = (DateTime.Parse(TodayDates)).AddDays(-0.00);
            OrderDate.MaxDate = (DateTime.Parse(MaxDate)).AddDays(-0.00);


            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);           
            
        }
        
        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='1' and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='1' and ActiveStatus='Y' Order by HSRPStateName";
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


        private static void CreateFolder(string strRTO, string strState, string strRTOLocFolderPath)
        {
            Directory.CreateDirectory(strRTOLocFolderPath);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState + "\\" + strRTO);
        }
        private string SetFolder(string strRTO, string strState, string strFile)
        {
            string DateFolder = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();
            strPath = "D:\\TenderReports";
            if (!Directory.Exists(strPath))
            {
                CreateFolder(DateFolder, strState, strPath);
                Directory.CreateDirectory(strPath + "\\" + strState);
                Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);
            }
            else
            {
                if (!Directory.Exists(strPath + "\\" + strState))
                {

                    Directory.CreateDirectory(strPath + "\\" + strState);
                    Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                }
                else
                {
                    if (!Directory.Exists(strPath + "\\" + strState + "\\" + DateFolder))
                    {

                        Directory.CreateDirectory(strPath + "\\" + strState + "\\" + DateFolder);

                    }
                    else
                    {
                        var files = Directory.GetFiles(strPath + "\\" + strState + "\\" + DateFolder, "*.*", SearchOption.AllDirectories);

                    }
                }


            }
            return strPath = strPath + "\\" + strState + "\\" + DateFolder;
        }


        string FromDate, ToDate;
        DataSet ds = new DataSet();
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {

                 //DataTable dtrto = Utils.GetDataTable("select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + HSRPStateID + "')", CnnString);
                 //for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
                 //{
                 //    string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                 //    string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                 //    SetFolder(RTOName, "Bihar", "BIHAR Embossing Report");
                     LabelError.Text = "";

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
                    
                     string ToDate = From1 + " 23:59:59";


                     int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                     //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                     DataTable StateName;
                     DataTable dts;
                     DataTable dt = new DataTable();
                     DataTable dt1 = new DataTable();
                     DataTable dt2 = new DataTable();

                     int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                     
                     string filename = "Schedule-F-Monthly_Report_From_Concessionaire_to_Registering_Authority_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
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
                
                     Worksheet sheet = book.Worksheets.Add("Monthly Report From Concessionaire to Registering Authority");
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


                     row.Index = 2;
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                     row.Cells.Add(new WorksheetCell("SCHEDULE-F", "HeaderStyle3"));

                     row = sheet.Table.Rows.Add();

                     row.Index = 3;
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                     row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                     WorksheetCell cell = row.Cells.Add("Monthly Report From Concessionaire to Registering Authority");
                     cell.MergeAcross = 3; // Merge two cells together
                     cell.StyleID = "HeaderStyle3";

                     row = sheet.Table.Rows.Add();

                     row.Index = 5;
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                     row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                     row.Cells.Add(new WorksheetCell("BIHAR", "HeaderStyle2"));

                   
                     row = sheet.Table.Rows.Add();
                     //  Skip one row, and add some text
                     row.Index = 6;
                     String StringOrderDate1 = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
                     String StringAuthDate1 = HSRPAuthDate.SelectedDate.ToString("dd/MM/yyyy");

                     DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                     row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));

                     row.Cells.Add(new WorksheetCell(StringOrderDate1 + " - " + StringAuthDate1, "HeaderStyle2"));
                     row = sheet.Table.Rows.Add();
                     row.Index = 7;
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                     row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                     row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                     row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "HeaderStyle2"));
                     row = sheet.Table.Rows.Add();



                     row.Index = 8;
                     //row.Cells.Add("Order Date");
                     row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Laser Number (s) encoded on the HSRP Issued", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Name of the Vehicle Owner", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Address of the Vehicle Owner", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Type and make of vehicle", "HeaderStyle"));
                     //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Whether old or new vehicle", "HeaderStyle"));
                     // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Date of receipt of Authorization for HSRP", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Date of receipt of amount from the Owner of the Vehicle", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Date of Affixation of HSRP to Owner's vehicle", "HeaderStyle"));
                     row.Cells.Add(new WorksheetCell("Number of old registration plates in Stock at Affixing Station", "HeaderStyle"));
                     row = sheet.Table.Rows.Add();
                     String StringField = String.Empty;
                     String StringAlert = String.Empty;

                     //row.Index = 9;

                     UserType = Convert.ToInt32(Session["UserType"]);

                     string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                     Utils.ExecNonQuery(upsqlstring1, CnnString);
                    
                     SQLString = "SELECT   a.Address1,a.OrderType ,a.ManufacturerName, convert(varchar,a.OrderDate,110)as OrderDate ,convert(varchar,a.OrderClosedDate,110)as OrderClosedDate,convert(varchar,a.HSRPRecord_AuthorizationDate,110)as HSRPRecord_AuthorizationDate, b.ProductColor, a.HSRPRecord_AuthorizationNo, convert(varchar,a.OrderEmbossingDate,110)as OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize, a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '1' AND a.OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "'";
                     
                     dt = Utils.GetDataTable(SQLString, CnnString);


                     string RTOColName = string.Empty;
                     int sno = 0;
                     if (dt.Rows.Count > 0)
                     {
                         foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                         {
                             sno = sno + 1;
                             row = sheet.Table.Rows.Add();
                             row.Cells.Add(new WorksheetCell(sno.ToString(), DataType.String, "HeaderStyle1"));

                             row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString() + " - " + dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                             row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
                             row.Cells.Add(new WorksheetCell(dtrows["Address1"].ToString(), DataType.String, "HeaderStyle1"));
                             row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString() + "" + dtrows["ManufacturerName"].ToString(), DataType.String, "HeaderStyle1"));
                             // row.Cells.Add(new WorksheetCell(dtrows["ManufacturerName"].ToString(), DataType.String, "HeaderStyle1"));

                             row.Cells.Add(new WorksheetCell(dtrows["OrderType"].ToString(), DataType.String, "HeaderStyle1"));

                             if (dtrows["HSRPRecord_AuthorizationDate"].ToString() == "")
                             {
                                 row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                             }
                             else
                             {
                                 string authdate = (DateTime.Parse(dtrows["HSRPRecord_AuthorizationDate"].ToString())).ToString("dd/MM/yyyy");
                                 row.Cells.Add(new WorksheetCell(authdate, DataType.String, "HeaderStyle1"));
                             }

                             if (dtrows["OrderDate"].ToString() == "")
                             {
                                 row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                             }
                             else
                             {
                                 DateTime orderdate = DateTime.Parse(dtrows["OrderDate"].ToString());
                                 //string orderdate = (dtrows["OrderDate"].ToString());
                                 row.Cells.Add(new WorksheetCell(orderdate.ToString("dd/MM/yyyy"), DataType.String, "HeaderStyle1"));
                             }
                             if (dtrows["OrderClosedDate"].ToString() != "")
                             {
                                 DateTime orderclosedate = DateTime.Parse(dtrows["OrderClosedDate"].ToString());
                                 row.Cells.Add(new WorksheetCell(orderclosedate.ToString("dd/MM/yyyy"), DataType.String, "HeaderStyle1"));
                             }
                             else
                             {
                                 row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                             }
                             if (dtrows["OrderType"].ToString() == "NB" || dtrows["OrderType"].ToString() == "OB" || dtrows["OrderType"].ToString() == "DB")
                             {
                                 row.Cells.Add(new WorksheetCell("2", DataType.String, "HeaderStyle1"));
                             }
                             else if (dtrows["OrderType"].ToString() == "DR" || dtrows["OrderType"].ToString() == "DF")
                             {
                                 row.Cells.Add(new WorksheetCell("1", DataType.String, "HeaderStyle1"));
                             }
                             else if (dtrows["OrderType"].ToString() == "OS")
                             {
                                 row.Cells.Add(new WorksheetCell("0", DataType.String, "HeaderStyle1"));
                             }
                             else
                             {
                                 row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                             }
                           
                         }
                         //row = sheet.Table.Rows.Add();
                         //row = sheet.Table.Rows.Add();
                         //book.Save(strPath + "\\" + filename);

                         row = sheet.Table.Rows.Add();
                         HttpContext context = HttpContext.Current;
                         context.Response.Clear();
                         book.Save(Response.OutputStream);

                         context.Response.ContentType = "application/vnd.ms-excel";

                         context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                         context.Response.End();

                     }
                 //}



            }

            catch (Exception ex)
            {
                LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
    }
}