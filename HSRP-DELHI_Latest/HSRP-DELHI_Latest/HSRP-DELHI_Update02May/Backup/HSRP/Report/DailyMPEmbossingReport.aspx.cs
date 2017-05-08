using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System;
using System.Collections;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Drawing;

namespace HSRP.Report
{
    public partial class DailyMPEmbossingReport : System.Web.UI.Page
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


                        }
                        else
                        {
                            FilldropDownListOrganization();

                            labelOrganization.Enabled = false;
                            //DropDownListStateName.Enabled = false;
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
        
            SQLString = "select rtolocationname,rtolocationid from rtolocation  where hsrp_stateid=5 and rtolocationid in (select distinct distrelation from rtolocation where  hsrp_stateid=5) Order by rtolocationname";
            Utils.PopulateDropDownList(DropDownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location --");
              
        }      

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
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";


                //int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                //int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "MP Embossing Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
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

                Worksheet sheet = book.Worksheets.Add("Data Embossing Report");
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
                WorksheetCell cell = row.Cells.Add("SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority");
                cell.MergeAcross = 3; // Merge two cells together
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();

                row = sheet.Table.Rows.Add();
                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("MADHYA PRADESH", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("RTO LOCATION :", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell(DropDownRTOName.SelectedItem.ToString(), "HeaderStyle2"));

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
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Application No", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Owners Name", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Laser Identification No", "HeaderStyle"));
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("RP Size", "HeaderStyle"));
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("3rd RP Y/N", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Colour", "HeaderStyle"));

                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);




                string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                Utils.ExecNonQuery(upsqlstring1, CnnString);
                 //string upsqlstring2= " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'";
                 //string upsqlstring3= " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'";
                 //string upsqlstring4 = " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";


                SQLString = "SELECT    b.ProductColor, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '5' and rtolocationid='"+DropDownRTOName.SelectedValue+"' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";
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

                        row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString() + " - " + dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));
                        // row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString() + " - " + dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        // row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
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


        private static void GenerateCell(PdfPTable table, int iSpan, int iLeftWidth, int iRightWidth, int iTopWidth, int iBottomWidth, int iAllign, int iFont, string strText, int iRowHeight, int iRowWidth)
        {
            PdfPCell newCellPDF = null;
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (iFont.Equals(0))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else if (iFont.Equals(1))
            {
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
        protected void btnGo_Click(object sender, EventArgs e)
        {
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
            HttpContext context = HttpContext.Current;
            string filename = "SCHEDULE-C_DailyMP_EmbossingReport-"+DropDownRTOName.SelectedItem.ToString() + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            string SQLString = String.Empty;
            Document document = new Document();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.Open();
            PdfPTable table = new PdfPTable(11);
            var colWidthPercentages = new[] { 12f, 35f, 20f, 25f, 25f, 35f, 25f, 35f, 15f, 25f, 25f };
            table.SetWidths(colWidthPercentages);
            //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
            string SqlQuery = string.Empty;

            SqlQuery = "SELECT  ROW_NUMBER() over( order by  HSRPRecord_AuthorizationNo) as SNo,  b.ProductColor, a.HSRPRecord_AuthorizationNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductDimension +' MM' AS FrontPlateSize, a.RearPlateSize, b.ProductDimension +' MM' AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '5' and rtolocationid in (Select rtolocationid from rtolocation where distrelation='" + DropDownRTOName.SelectedValue + "') AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "' and ownername is not null and hsrp_front_lasercode not in ( 'AA271055126','AA271072995','AA170855418','AA210591749','AA210597574','AA210599883','AA170855406','AA170871225','AA210599855','AA170850011','AA251393551','AA170857134','AA170855432','AAA10861157','AA210599873','AA251569860','AA170855444','AA251315167','AA210597584','AA170873704','AA271074025','AA220216982','AA220216956','AA170857252','AA210597560','AA271223713','AA170856472','AA210597552','AA170850164','AA170855430','AA210599869','AA210597558','AA210584310','AA170535357','AA210599851','AA170856464','AA170855429','AA220220312','AA170855414','AA170855412','AA210597564','AA170856484','AA270954164','AA170908478','AA170855407','AAC40050146','AA210599853','AA210915750','AA210597570','AA210599875','AA170855421','AA170856498','AA170856480','AA170855435','AA271020815','AA210597554','AA170855417','AA251611685','AA170855442','AA251197251','AA220220310','AA271032822','AA170855401','AA210597586','AA170911124','AA170855408','AA210597580','AA271248115','AA270942098','AA170855419','AA220220306','AA210584322','AA251395073','AA170856478','AA271062786','AA210597556','AA271049246','AA170855405','AA271055122','AA210597592','AA270739527','AA170855402','AA210597576','AA170855428','AA270758806','AA251385101','AA170851105','AA210597578','AA170856485','AA170855413','AA251203769','AAC60082639','AA170855409','AA210597572','AA271150518','AA210599877','AA170855425','AA170855415','AA170855416','AA270951141','AA271262087','AA170850176','AA210573349','AA210589716','AA210597596','AA210599879','AA170856495','AA210597594','AA210599881','AA251648245','AA170850169','AA270966578','AA270888770','AA170850182','AA270888762','AA210597588','AA170856479','AA170908931','AA270969695','AA210594113','AA170861370','AA210581973','AA271243277','AA170855404','AA170856494','AA271045500','AA170855410','AA170855431','AA170850177','AA120022126','AA251322504','AA271075549','AA170856491','AA220220314','AA210597582','AA220220308','AA170856469','AA170855440','AA210597566','AA170855423','AA210597568','AA210597562','AA170855426','AA170856470') and netamount >0  order by HSRPRecord_AuthorizationNo";
            
            DataTable dt = Utils.GetDataTable(SqlQuery, CnnString);
          
            table.TotalWidth = 1000f;
            GenerateCell(table, 11, 0, 0, 0, 0, 1, 1, "", 50, 0);
            GenerateCell(table, 11, 0, 0, 0, 0, 0, 0, "SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority", 15, 0);
            GenerateCell(table, 11, 0, 0, 0, 0, 0, 0, "RTO Name :   " + DropDownRTOName.SelectedItem.ToString(), 15, 0);
            GenerateCell(table, 11, 0, 0, 0, 0, 0, 0, "Date Period  :     " + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") +"-"+ HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);
            
           // GenerateCell(table, 5, 0, 0, 0, 0, 0, 0, System.DateTime.Now.Day.ToString(), 15, 0);
            GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Application No.", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "FP Size", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front Laser Code", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "RP Size", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear Laser Code", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "3rd RP Y/N", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Color", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Background Remarks", 20, 0);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GenerateCell(table, 1, 1, 1, 0, 1, 1, 0, dt.Rows[i]["SNo"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["HSRPRecord_AuthorizationNo"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["VehicleType"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["OwnerName"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["FrontPlateSize"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["HSRP_Front_LaserCode"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["RearPlateSize1"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["HSRP_Rear_LaserCode"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["StickerMandatory"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["ProductColor"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 0, dt.Rows[i]["Remarks"].ToString(), 20, 0);
               

            }
            document.Add(table);
            document.NewPage();

            document.Close();
            context.Response.ContentType = "Application/pdf";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            context.Response.WriteFile(PdfFolder);
            context.Response.End();













        }
    }
}