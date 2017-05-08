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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class HRMonthlyReport : System.Web.UI.Page
    {
        string strPath = string.Empty;
        string strMonth = string.Empty;
        string strYear = string.Empty;
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


                        }
                        else
                        {
                            FilldropDownListOrganization();

                           // labelOrganization.Enabled = false;
                           // DropDownListStateName.Enabled = false;
                           // FilldropDownListOrganization();
                        }
                    }
                    catch (Exception err)
                    {

                    }
                }
            }
        }

        #region Folder Creation
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



                        //foreach (string file in files)
                        //{
                        //    if (file.StartsWith(strPath + "\\" + strState + "\\" + DateFolder + "\\" + strFile))
                        //    {
                        //        File.Delete(file);
                        //    }
                        //}
                    }
                }


            }
            return strPath = strPath + "\\" + strState + "\\" + DateFolder;
        }

        private static void CreateFolder(string strRTO, string strState, string strRTOLocFolderPath)
        {
            Directory.CreateDirectory(strRTOLocFolderPath);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState);
            Directory.CreateDirectory(strRTOLocFolderPath + "\\" + strState + "\\" + strRTO);
        }

        private DataTable GetRtoLocation()
        {
            string sql = "select rtolocationname,rtolocationid from rtolocation where rtolocationid in (select distinct distrelation from rtolocation where hsrp_stateid='" + DropDownListStateName.SelectedValue + "') and RTOLocationID not in (148,331)";
            DataTable dtrto = Utils.GetDataTable(sql, CnnString);
            return dtrto;
        }


        #endregion

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            HSRPAuthDate.MinDate = DateTime.Parse("2014-06-01");
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            //OrderDate.MinDate = System.DateTime.Now.AddDays(-7);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MinDate = DateTime.Parse("2014-06-01");
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
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                //DataSet dts = Utils.getDataSet(SQLString, CnnString);
                //DropDownListStateName.DataSource = dts;
                //DropDownListStateName.DataBind();

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

        protected void DropDownListStateName_SelectedIndexChanged1(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        private void FilldropDownListClient()
        {
            SQLString = "select rtolocationname,rtolocationid from rtolocation  where hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and rtolocationid in (select distinct distrelation from rtolocation where  hsrp_stateid='" + DropDownListStateName.SelectedValue + "') Order by rtolocationname";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
        }



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



                Worksheet sheet11 = book.Worksheets.Add("SCHEDULE-F");
                sheet11.Table.Columns.Add(new WorksheetColumn(60));
                sheet11.Table.Columns.Add(new WorksheetColumn(205));
                sheet11.Table.Columns.Add(new WorksheetColumn(100));
                sheet11.Table.Columns.Add(new WorksheetColumn(130));

                sheet11.Table.Columns.Add(new WorksheetColumn(100));
                sheet11.Table.Columns.Add(new WorksheetColumn(120));
                sheet11.Table.Columns.Add(new WorksheetColumn(112));
                sheet11.Table.Columns.Add(new WorksheetColumn(109));
                sheet11.Table.Columns.Add(new WorksheetColumn(105));
                sheet11.Table.Columns.Add(new WorksheetColumn(160));
                

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
                row.Cells.Add(new WorksheetCell("HARYANA", "HeaderStyle2"));

                row = sheet.Table.Rows.Add();
                //  Skip one row, and add some text
                row.Index = 6;

                DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Report Date :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(OrderDate.SelectedDate.ToString("dd/MM/yyyy") + " - " + HSRPAuthDate.SelectedDate.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();
                row.Index = 7;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("Generated Date time:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("dd/MM/yyyy"), "HeaderStyle2"));
                row = sheet.Table.Rows.Add();



                row.Index = 8;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Laser Number (s) encoded on the HSRP Issued", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Address of the Vehicle Owner", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Type and make of vehicle", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Whether old or new vehicle", "HeaderStyle"));
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
              //  row.Cells.Add(new WorksheetCell("Date of receipt of Authorization for HSRP", "HeaderStyle"));
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

                //SQLString = "SELECT   a.Address1,a.OrderType ,a.ManufacturerName,a.OrderDate ,a.OrderClosedDate,a.HSRPRecord_AuthorizationDate, b.ProductColor, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND a.OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "'";
                SQLString = "SELECT   a.Address1,a.OrderType ,a.ManufacturerName, convert(varchar,a.OrderDate,110)as OrderDate ,convert(varchar,a.OrderClosedDate,110)as OrderClosedDate,convert(varchar,a.HSRPRecord_AuthorizationDate,110)as HSRPRecord_AuthorizationDate, b.ProductColor, a.HSRPRecord_AuthorizationNo, convert(varchar,a.OrderEmbossingDate,110)as OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize, a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND a.OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "'";
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
                        string authdate = (DateTime.Parse(dtrows["HSRPRecord_AuthorizationDate"].ToString())).ToString("dd/MM/yyyy");
                      //  row.Cells.Add(new WorksheetCell(authdate, DataType.String, "HeaderStyle1"));
                        string orderdate = (dtrows["OrderDate"].ToString());
                        row.Cells.Add(new WorksheetCell(orderdate, DataType.String, "HeaderStyle1"));
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
                        // row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        //row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString() + " - " + dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        //// row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        //row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
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

        protected void btnExportToPDF_Click(object sender, EventArgs e)
        {
            ExportToPDF();
        }

        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            if (!iRowHeight.Equals(0))
            {
                newCellPDF.FixedHeight = iRowHeight;
            }
            if (!iRowWidth.Equals(0))
            {
            }
            table.AddCell(newCellPDF);
        }
        private void ExportToPDF()
        {
            string FromDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
            string ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

            HttpContext context = HttpContext.Current;
            string filename = "SCHEDULE-F-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            string SQLString = String.Empty;
            Document document = new Document();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.Open();
            PdfPTable table = new PdfPTable(8);
            var colWidthPercentages = new[] { 25f, 70f, 45f, 50f, 45f, 45f, 60f, 60f};
            table.SetWidths(colWidthPercentages);
            //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
            string SqlQuery = string.Empty;


            SQLString = "SELECT ROW_NUMBER() over( order by  HSRPRecord_AuthorizationNo) as 'SNo',HSRP_Front_LaserCode as 'FrontLaserNo',HSRP_Rear_LaserCode as 'RearLaserNo', OwnerName,rtrim(ltrim(Address1)) as 'Address1',OrderType as 'OrderType'," +
                   " convert(varchar,OrderDate,103)as 'OrderDate' ,convert(varchar,OrderClosedDate,103)as 'ClosedDate' FROM HSRPRecords   WHERE  HSRP_StateID= '4' AND OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "'";
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);

            table.TotalWidth = 780f;
            table.LockedWidth = true;
            //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);
            GenerateCell(table, 8, 0, 0, 0, 0, 1, 0, "SCHEDULE-F : Monthly Report From Concessionaire to Registering Authority", 20, 0);
            GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "State Name : HARYANA", 20, 0);
            GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "RTO Name:      " + dropDownListClient.SelectedItem.ToString(), 15, 0);
            GenerateCell(table, 2, 0, 0, 0, 0, 0, 0, "Date Period  :" + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + " -  " + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);            
            GenerateCell(table, 8, 0, 0, 0, 0, 0, 0,"Report Generation Date :"+ System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
            GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "", 20, 0);
            #region Commented Old Name Of Headers
            //GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 40, 0);
            //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Laser Number (s) encoded on the HSRP Issued", 40, 0);
            //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
            //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Address of the Vehicle Owner", 40, 0);

            //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 40, 0);
            
            //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Whether old or new vehicle", 40, 0);
            ////
            //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Date of receipt of amount from the Owner of the Vehicle", 40, 0);
            //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Date of Affixation of HSRP to Owner's vehicle", 40, 0);
            //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of old registration plates in Stock at Affixing Station", 40, 0);
            #endregion

            GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 40, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front Laser No", 40, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear Laser No", 40, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Address", 40, 0);

            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Type", 40, 0);

            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Date", 40, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Closed Date", 40, 0);
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dt.Rows[i]["SNo"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["FrontLaserNo"].ToString() , 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["RearLaserNo"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OwnerName"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["Address1"].ToString(), 20, 0);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OrderType"].ToString(), 20, 0);

                //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "New", 20, 0);               

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OrderDate"].ToString(), 20, 0);

                if (dt.Rows[i]["ClosedDate"].ToString() != "")
                {                    
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["ClosedDate"].ToString(), 20, 0);
                }
                else
                {
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 20, 0);
                }
                #region Commented Code
                //if (dt.Rows[i]["OrderType"].ToString() == "NB" || dt.Rows[i]["OrderType"].ToString() == "OB" || dt.Rows[i]["OrderType"].ToString() == "DB")
                //{
                //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "2", 20, 0);
                //}
                //else if (dt.Rows[i]["OrderType"].ToString() == "DR" || dt.Rows[i]["OrderType"].ToString() == "DF")
                //{
                //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "1", 20, 0);
                //}
                //else if (dt.Rows[i]["OrderType"].ToString() == "OS")
                //{
                //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "0", 20, 0);
                //}
                //else
                //{
                //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 20, 0);
                //}            
                #endregion
            }
            document.Add(table);
            document.NewPage();

            document.Close();
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();


        }

        private void ExportToAllPDF()
        {
              DataTable dtrto = GetRtoLocation();
              for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
              {
                  string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                  string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                  string FromDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
                  string ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

                 // HttpContext context = HttpContext.Current;
                  //string filename = "SCHEDULE-F-" + RTOName + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                  string filename = "" + RTOName + "RA" + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + "HRS-F" + ".pdf";
                  string SQLString = String.Empty;
                  Document document = new Document();
                  SetFolder(RTOName, DropDownListStateName.SelectedItem.ToString(), "SCHEDULE-F-");

                  string PdfFolder = strPath + "\\" + filename;
                  BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                  //string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                  PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                  document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                  document.Open();
                  PdfPTable table = new PdfPTable(11);
                  var colWidthPercentages = new[] { 25f, 70f, 65f, 50f, 58f, 65f, 68f, 35f,57f,55f,45f };
                  table.SetWidths(colWidthPercentages);
                  //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
                  string SqlQuery = string.Empty;


                  SQLString = "SELECT ROW_NUMBER() over( order by  HSRPRecord_AuthorizationNo) as 'SNo',HSRP_Front_LaserCode as 'FrontLaserNo',HSRP_Rear_LaserCode as 'RearLaserNo',VehicleRegNo, OwnerName,rtrim(ltrim(Address1)) as 'Address1',vehicletype,OrderType as 'OrderType',case when hsrprecord_Authorizationdate ='1900-01-01 00:00:00.000' then '' else convert(varchar(20),hsrprecord_Authorizationdate,103) end as 'AuthorizationDate',convert(varchar,OrderDate,103)as 'OrderDate' ,convert(varchar,OrderClosedDate,103)as 'ClosedDate' FROM HSRPRecords   WHERE  HSRP_StateID= '4' and rtolocationid='" + RTOCode + "' AND hsrprecord_creationdate  between '" + FromDate + "' and '" + ToDate + "'";
                  DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                  table.TotalWidth = 815f;
                  table.LockedWidth = true;
                  //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);
                  GenerateCell(table, 12, 0, 0, 0, 0, 1, 0, "SCHEDULE-F : Monthly Report From Concessionaire to Registering Authority", 20, 0);
                  GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "State Name : HARYANA", 15, 0);
                  GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "RTO Name:      " + RTOName, 15, 0);
                  GenerateCell(table, 3, 0, 0, 0, 0, 0, 0, "Date Period :" + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);
                 // GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "Report Generation Date :" + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
                  GenerateCell(table, 8, 0, 0, 0, 0, 0, 0, "", 20, 0);
                  #region Commented Old Name Of Headers
                  //GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 40, 0);
                  //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Laser Number (s) encoded on the HSRP Issued", 40, 0);
                  //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
                  //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Address of the Vehicle Owner", 40, 0);

                  //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 40, 0);

                  //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Whether old or new vehicle", 40, 0);
                  ////
                  //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Date of receipt of amount from the Owner of the Vehicle", 40, 0);
                  //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Date of Affixation of HSRP to Owner's vehicle", 40, 0);
                  //GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Number of old registration plates in Stock at Affixing Station", 40, 0);
                  #endregion

                  GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 40, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front Laser No", 40, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear Laser No", 40, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle NO", 40, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Address", 40, 0);

                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 40, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Type", 40, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Authorization Date", 40, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Order Date", 40, 0);
                  GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Closed Date", 40, 0);
                  

                  for (int i = 0; i < dt.Rows.Count; i++)
                  {

                      GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dt.Rows[i]["SNo"].ToString(), 20, 0);
                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["FrontLaserNo"].ToString(), 20, 0);
                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["RearLaserNo"].ToString(), 20, 0);
                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OwnerName"].ToString(), 20, 0);
                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["VehicleRegNo"].ToString(), 20, 0);
                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["Address1"].ToString(), 20, 0);

                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["vehicletype"].ToString(), 20, 0);
                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OrderType"].ToString(), 20, 0);

                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["AuthorizationDate"].ToString(), 20, 0);
                      //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "New", 20, 0);               

                      GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OrderDate"].ToString(), 20, 0);

                      if (dt.Rows[i]["ClosedDate"].ToString() != "")
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["ClosedDate"].ToString(), 20, 0);
                      }
                      else
                      {
                          GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 20, 0);
                      }

                      //GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 20, 0);

                      #region Commented Code
                      //if (dt.Rows[i]["OrderType"].ToString() == "NB" || dt.Rows[i]["OrderType"].ToString() == "OB" || dt.Rows[i]["OrderType"].ToString() == "DB")
                      //{
                      //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "2", 20, 0);
                      //}
                      //else if (dt.Rows[i]["OrderType"].ToString() == "DR" || dt.Rows[i]["OrderType"].ToString() == "DF")
                      //{
                      //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "1", 20, 0);
                      //}
                      //else if (dt.Rows[i]["OrderType"].ToString() == "OS")
                      //{
                      //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "0", 20, 0);
                      //}
                      //else
                      //{
                      //    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 20, 0);
                      //}            
                      #endregion
                  }
                  document.Add(table);
                  document.NewPage();

                  document.Close();
                  //context.Response.ContentType = "Application/pdf";
                  //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                  //context.Response.WriteFile(PdfFolder);
                  //context.Response.End();
              }


        }

        protected void btnGo_Click(object sender, EventArgs e)
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
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string FromDate = From + " 00:00:01"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";
                SQLString = "SELECT ROW_NUMBER() over( order by  HSRPRecord_AuthorizationNo) as 'S No',HSRP_Front_LaserCode as 'Front Laser No',HSRP_Rear_LaserCode as 'Rear Laser No', OwnerName,rtrim(ltrim(Address1)) as 'Owner Address',OrderType as 'Order Type'," +
                   " convert(varchar,OrderDate,103)as 'Order Date' ,convert(varchar,OrderClosedDate,103)as 'Closed Date' FROM HSRPRecords   WHERE  HSRP_StateID= '4' AND OrderClosedDate  between '" + FromDate + "' and '" + ToDate + "'";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                LabelError.Text = "Please Select Seven Days Difference Between Dates ";
            }
        }

        protected void btnExportToallreportPDF_Click(object sender, EventArgs e)
        {
            ExportToAllPDF();
        }

       

     

    }
}