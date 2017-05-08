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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class DailyHREmbossingReport : System.Web.UI.Page
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

                            //labelOrganization.Enabled = false;
                            //DropDownListStateName.Enabled = false;
                            //FilldropDownListOrganization();
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
            strPath = "E:\\TenderReports";
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
           DataTable dtrto = Utils.GetDataTable(sql,CnnString);
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
            //SQLString = "select rtolocationname,rtolocationid from rtolocation  where hsrp_stateid=5 and rtolocationid in (select distinct distrelation from rtolocation where  hsrp_stateid=5) Order by rtolocationname";
            //Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select RTO Location --");

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
      

        #endregion



        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
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

        #region Pdf Function
        private static void PDFRows(BaseFont bfTimes, PdfPTable table1, int iColSpan, String strText, int iFontsize, int ialignMent, string strRowType, int iBorderWidthLeft, int iBorderWidthRight, int iBorderWidthTop, int iBorderWidthBottom, int optionalHeight = 0, int optionalWidth = 0)
        {
            PdfPCell cell;
            if (strRowType == "B")
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            }
            else
            {
                cell = new PdfPCell(new iTextSharp.text.Phrase(strText, new iTextSharp.text.Font(bfTimes, iFontsize, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            cell.Colspan = iColSpan;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderWidthLeft = iBorderWidthLeft;
            cell.BorderWidthRight = iBorderWidthRight;
            cell.BorderWidthTop = iBorderWidthTop;
            cell.BorderWidthBottom = iBorderWidthBottom;
            cell.NoWrap = false;
            cell.HorizontalAlignment = ialignMent; //0=Left, 1=Centre, 2=Right
            table1.AddCell(cell);
        }
        #endregion

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
                newCellPDF = new PdfPCell(new Phrase(strText, new iTextSharp.text.Font(bfTimes1, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            }
            newCellPDF.Colspan = iSpan;
            newCellPDF.BorderWidthLeft = iLeftWidth;
            newCellPDF.BorderWidthRight = iRightWidth;
            newCellPDF.BorderWidthTop = iTopWidth;
            newCellPDF.BorderWidthBottom = iBottomWidth;
            newCellPDF.HorizontalAlignment = iAllign;
            newCellPDF.NoWrap = false;
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
            string filename = "Schedule-C-HR_Embossing_Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            string SQLString = String.Empty;
            Document document = new Document();
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.Open();
            PdfPTable table = new PdfPTable(10);
            var colWidthPercentages = new[] { 17f, 80f, 40f, 45f, 40f, 40f, 50f, 50f, 20f, 25f};
            table.SetWidths(colWidthPercentages);
            //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
            string SqlQuery = string.Empty;


            SQLString = "SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo,  b.ProductColor, a.HSRPRecord_AuthorizationNo as vh1,a.VehicleRegNo as vh2, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";
           DataTable dt = Utils.GetDataTable(SQLString, CnnString);

            table.TotalWidth = 750f;
            table.LockedWidth = true;
            //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);
            
            GenerateCell(table, 10, 0, 0, 0, 0,1, 0, "SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority", 20, 0);
            GenerateCell(table, 2, 0, 0, 0, 0, 1, 0, "State Name : HARYANA",20,0);
            GenerateCell(table, 2, 0, 0, 0, 0, 0, 0, "RTO Name:      " + dropDownListClient.SelectedItem.ToString(), 15, 0);
            GenerateCell(table, 3, 0, 0, 0, 0, 1, 0, "Date Period  :" + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);
            GenerateCell(table, 3, 0, 0, 0, 0, 1, 0, "Generated Date time: " + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
            GenerateCell(table, 10, 0, 0, 0, 0, 1, 0, "", 20, 0);
            GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Application No./Registration No.", 40, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
            GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "Laser Identification No", 20, 0);

            GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "RP Size", 20, 0);           
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "3rd RP Y/N", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Color", 20, 0);

            GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 40, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 20, 0);

            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 20, 0);
           
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Sticker", 20, 0);
            GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
            
            #region Dynamic Rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dt.Rows[i]["SNo"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["vh1"].ToString() + "/" + dt.Rows[i]["vh2"].ToString(), 30, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["VehicleType"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OwnerName"].ToString(), 20, 0);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRP_Front_LaserCode"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRP_Rear_LaserCode"].ToString(), 20, 0);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["FrontPlateSize"].ToString(), 20, 0);
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["RearPlateSize1"].ToString(), 20, 0);
            
                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["StickerMandatory"].ToString(), 20, 0);

                GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["ProductColor"].ToString(), 20, 0);
                 
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


        private void ExportToPDFAll()
        {
             DataTable dtrto = GetRtoLocation();
             for (int iRowNo = 0; iRowNo < dtrto.Rows.Count; iRowNo++)
             {
                 string RTOName = dtrto.Rows[iRowNo]["rtolocationname"].ToString();
                 string RTOCode = dtrto.Rows[iRowNo]["RTOLocationid"].ToString();
                 string FromDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
                 string ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";

                // HttpContext context = HttpContext.Current;
                 string filename = "Schedule-C-HR_Embossing_Report" + RTOName + "_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                 string SQLString = String.Empty;
                 Document document = new Document();
                 SetFolder(RTOName, DropDownListStateName.SelectedItem.ToString(), "Schedule-C-HR_Embossing_Report");

                 string PdfFolder = strPath + "\\" + filename; 
                 BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                // string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                 PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
                 document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                 document.Open();
                 PdfPTable table = new PdfPTable(10);
                 var colWidthPercentages = new[] { 17f, 80f, 40f, 45f, 40f, 40f, 50f, 50f, 20f, 25f };
                 table.SetWidths(colWidthPercentages);
                 //string sqlquery="select distrelation from rtolocation where rtolocationid='"+
                 string SqlQuery = string.Empty;


                 SQLString = "SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo,  b.ProductColor, a.HSRPRecord_AuthorizationNo as vh1,a.VehicleRegNo as vh2, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' and a.rtolocationid='" + RTOCode + "' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";
                 DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                 table.TotalWidth = 750f;
                 table.LockedWidth = true;
                 //  GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);

                 GenerateCell(table, 10, 0, 0, 0, 0, 1, 0, "SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority", 20, 0);
                 GenerateCell(table, 2, 0, 0, 0, 0, 1, 0, "State Name : HARYANA", 20, 0);
                 GenerateCell(table, 2, 0, 0, 0, 0, 0, 0, "RTO Name:      " + RTOName, 15, 0);
                 GenerateCell(table, 3, 0, 0, 0, 0, 1, 0, "Date Period  :" + OrderDate.SelectedDate.ToString("dd/MMM/yyyy") + "-" + HSRPAuthDate.SelectedDate.ToString("dd/MMM/yyyy"), 15, 0);
                // GenerateCell(table, 3, 0, 0, 0, 0, 1, 0, "Generated Date time: " + System.DateTime.Now.ToString("dd/MMM/yyyy"), 15, 0);
                 GenerateCell(table, 10, 0, 0, 0, 0, 1, 0, "", 20, 0);
                 GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "S.No:", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Application No./Registration No.", 40, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Vehicle Type", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Owner Name", 20, 0);
                 GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "Laser Identification No", 20, 0);

                 GenerateCell(table, 2, 0, 1, 1, 1, 1, 0, "RP Size", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "3rd RP Y/N", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Color", 20, 0);

                 GenerateCell(table, 1, 1, 1, 1, 1, 1, 0, "", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 40, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 20, 0);

                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Front", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Rear", 20, 0);

                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "Sticker", 20, 0);
                 GenerateCell(table, 1, 0, 1, 1, 1, 1, 0, "", 20, 0);

                 #region Dynamic Rows
                 for (int i = 0; i < dt.Rows.Count; i++)
                 {
                     GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, dt.Rows[i]["SNo"].ToString(), 20, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["vh1"].ToString() + "/" + dt.Rows[i]["vh2"].ToString(), 30, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["VehicleType"].ToString(), 20, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["OwnerName"].ToString(), 20, 0);

                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRP_Front_LaserCode"].ToString(), 20, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["HSRP_Rear_LaserCode"].ToString(), 20, 0);

                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["FrontPlateSize"].ToString(), 20, 0);
                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["RearPlateSize1"].ToString(), 20, 0);

                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["StickerMandatory"].ToString(), 20, 0);

                     GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, dt.Rows[i]["ProductColor"].ToString(), 20, 0);

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


                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                DataTable StateName;
                DataTable dts;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                //int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                string filename = "Schedule-C-HR_Embossing_Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
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
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                //row.Cells.Add(new WorksheetCell(" ", "HeaderStyle3"));
                //WorksheetCell cell = row.Cells.Add("SCHEDULE-C : Daily Report from Embossing Stations to Registering Authority");
                //cell.MergeAcross = 3; // Merge two cells together
                //cell.StyleID = "HeaderStyle3";

                row.Index = 1;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle6"));
                WorksheetCell cell5 = row.Cells.Add("SCHEDULE-C ");
                cell5.MergeAcross = 3; // Merge two cells togetherto 
                cell5.StyleID = "HeaderStyle6";

                row = sheet.Table.Rows.Add();


                row.Index = 2;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                WorksheetCell cell = row.Cells.Add("Daily Report from Embossing Stations to Registering Authority");
                cell.MergeAcross = 3; // Merge two cells togetherto 
                cell.StyleID = "HeaderStyle3";

                row = sheet.Table.Rows.Add();


                row.Index = 3;
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                row.Cells.Add(new WorksheetCell("HARYANA", "HeaderStyle2"));

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
                row.Cells.Add(new WorksheetCell("Sl.No.", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Application No", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Owners Name", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                WorksheetCell cell1 = row.Cells.Add("Laser Identification No");
                cell1.MergeAcross = 1; // Merge two cells together
                cell1.StyleID = "HeaderStyle6";
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
               // row.Cells.Add(new WorksheetCell("RP Size", "HeaderStyle"));

                WorksheetCell cell2 = row.Cells.Add("RP Size");
                cell2.MergeAcross = 1; // Merge two cells together
                cell2.StyleID = "HeaderStyle6";
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("3rd RP Y/N", "HeaderStyle6"));
               // row.Cells.Add(new WorksheetCell("Colour", "HeaderStyle"));
                WorksheetCell cell3 = row.Cells.Add("Colour");
                cell3.MergeAcross = 1; // Merge two cells together
                cell3.StyleID = "HeaderStyle6";
                row = sheet.Table.Rows.Add();
                String StringField = String.Empty;
                String StringAlert = String.Empty;


                row.Index = 7;
                //row.Cells.Add("Order Date");
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                //row.Cells.Add(new WorksheetCell("Rear Laser Code ", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Front", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("Rear", "HeaderStyle6"));
                // row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Sticker", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("Yellow", "HeaderStyle6"));
                row.Cells.Add(new WorksheetCell("White", "HeaderStyle6"));
                row = sheet.Table.Rows.Add();
                //String StringField = String.Empty;
                //String StringAlert = String.Empty;

                //row.Index = 9;

                UserType = Convert.ToInt32(Session["UserType"]);


                string upsqlstring1 = "update hsrprecords set   frontplatesize = Null where frontplatesize like '%Select%'" + " update hsrprecords set   rearplatesize = Null where rearplatesize like '%Select%'" + " update hsrprecords set   manufacturername = Null where manufacturername like '%Select%'" + " update hsrprecords set   manufacturermodel = Null where manufacturermodel like '%Select%'";
                Utils.ExecNonQuery(upsqlstring1, CnnString);

                SQLString = "SELECT    b.ProductColor, a.VehicleRegNo, a.OrderEmbossingDate, a.HSRPRecordID, a.OwnerName, a.VehicleType, a.HSRP_Front_LaserCode,Product_1.ProductColor,a.HSRP_Rear_LaserCode, a.VehicleClass, a.StickerMandatory, Product_1.ProductCode AS FrontPlateSize, a.RearPlateSize, b.ProductCode AS RearPlateSize1,a.FrontPlateSize AS FrontPlateSize ,a.Remarks FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";
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

                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString() , DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle1"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontPlateSize"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["RearPlateSize1"].ToString(), DataType.String, "HeaderStyle1"));
                        row.Cells.Add(new WorksheetCell(dtrows["StickerMandatory"].ToString(), DataType.String, "HeaderStyle1"));
                        if (dtrows["ProductColor"].ToString() == "Yellow")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                        }
                        else
                        {
                            row.Cells.Add(new WorksheetCell("", DataType.String, "HeaderStyle1"));
                            row.Cells.Add(new WorksheetCell(dtrows["ProductColor"].ToString(), DataType.String, "HeaderStyle1"));
                        }
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
             TimeSpan ts = HSRPAuthDate.SelectedDate - OrderDate.SelectedDate;
             if (ts.Days <= 7)
             {
                 ExportToPDF();
             }
             else
             {
                 LabelError.Text = "Please Select Seven Days Difference Between both dates";
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
                string FromDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate1 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string ToDate = From1 + " 23:59:59";
                BindGrid();
                GridView1.PageIndex = 0;
            }
            else
            {

                LabelError.Text = "Please Select Seven Days Difference Between Dates ";
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
                 

        }

        private void BindGrid()
        {
            string FromDate = OrderDate.SelectedDate.ToString("yyyy/MM/dd") + " 00:00:00"; // Convert.ToDateTime();
            string ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd") + " 23:59:59";
            SQLString = "SELECT  ROW_NUMBER() over( order by  VehicleRegNo) as SNo, a.HSRPRecord_AuthorizationNo +' / '+ a.VehicleRegNo as 'Application / Registration No',a.VehicleType,a.OwnerName,a.HSRP_Front_LaserCode as FrontLaserNo,a.HSRP_Rear_LaserCode as RearLaserNo,Product_1.ProductCode AS FrontPlateSize,b.ProductCode AS RearPlateSize1, a.StickerMandatory, b.ProductColor FROM HSRPRecords a INNER JOIN Product b ON a.RearPlateSize = b.ProductID INNER JOIN  Product Product_1 ON a.FrontPlateSize = Product_1.ProductID  WHERE  a.HSRP_StateID= '4' AND a.OrderEmbossingDate  between '" + FromDate + "' and '" + ToDate + "'";
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAllReportPDF_Click(object sender, EventArgs e)
        {
            ExportToPDFAll();
        }

       

        

       
    }
}