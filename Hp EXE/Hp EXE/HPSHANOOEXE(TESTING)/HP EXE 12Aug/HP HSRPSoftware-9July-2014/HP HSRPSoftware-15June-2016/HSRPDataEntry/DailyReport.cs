using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using HSRPTransferData;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


using System.Data;
using HSRPDataEntryNew.WebReferenceHP;



namespace HSRPDataEntryNew
{
    public partial class DailyReport : Form
    {
        HSRPService objWebServiceHP = new HSRPService();
        public DailyReport()
        {
            InitializeComponent();
        }

        string CnnString = String.Empty;
        string SqlString = String.Empty;

        DataTable dt = new DataTable();

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DateTime dt = FromDate.Value;
            DateTime dtto = todate.Value;
            //dt.Month.ToString()+"/"+dt.Date.ToString()+"/"+dt.Year.ToString()
            String dd = dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString();
            String Starttime = dd + " 00:00:00";

            String dd1 = dtto.Month.ToString() + "/" + dtto.Day.ToString() + "/" + dtto.Year.ToString();
            String Endtime = dd1 + " 23:59:59";

            // string queryString = "SELECT distinct [BatchID], [RoutingPackageID],COUNT(*) as imageCount FROM [TaggingOutput] where TypeOfBill  in(32,33,1,62,39) and AllocationDateTime between '" + Starttime + "' and '" + Endtime + "'   GROUP BY [BatchID], [RoutingPackageID] HAVING count(*) > 3";
            //string queryString = "SELECT row_number() over (order by RoutingPackageID desc) as Sno,'" + dd.ToString() + "' as AllocationDate,RoutingPackageID,BatchID, count(*)as PageCount FROM AllocationBase where RoutingPackageID in (SELECT  distinct RoutingPackageID FROM [TaggingOutput] where TypeOfBill in(1,32,33,39,62) and AllocationDateTime between '" + Starttime + "' and '" + Endtime + "') GROUP BY RoutingPackageID,BatchID HAVING count(*) > 3";

            /****** Script for SelectTopNRows command from SSMS  ******/
            string queryString = "SELECT  [Record_CreationDate],[RTO_CD],[HSRPRecord_AuthorizationNo],[HSRPRecord_AuthorizationDate],[VehicleRegNo],[OwnerName],[address] ,[MobileNo],[VehicleClass],[OrderType],[VehicleType],[CashReceiptNo], [Amount],[ChassisNo] ,[EngineNo],[hsrp_front_lasercode],[hsrp_rear_lasercode],[OrderStatus],[OrderEmbossingDate],[OrderClosedDate],[FirstSMSSentFlag],[FirstSMSText],[FirstSMSDateTime],[FirstSMSServerResponseID],[FirstSMSServerResponseText],[SecondSMSText],[SecondSMSDateTime],[SecondSMSServerResponseID] ,[SecondSMSServerResponseText],[ThirdSMSText],[ThirdSMSDateTime],[ThirdSMSServerResponseID],[ThirdSMSServerResponseText] ,[IsProcessed]  FROM [HSRP_APP_HP].[dbo].[OrderBookingOffLine] where  (Record_CreationDate between '" + Starttime + "' and '" + Endtime + "') or (OrderEmbossingDate between '" + Starttime + "' and '" + Endtime + "') or (OrderClosedDate between '" + Starttime + "' and '" + Endtime + "') ";

            using (SqlConnection c = new SqlConnection(utils.getCnnHSRPApp.ToString()))
            {
                c.Open();
                // 2
                // Create new DataAdapter
                using (SqlDataAdapter a = new SqlDataAdapter(queryString, c))
                {
                    // 3
                    // Use DataAdapter to fill DataTable
                    DataTable t = new DataTable();
                    a.Fill(t);
                    // 4
                    // Render data onto the screen
                    dataGridView2.DataSource = t;
                }
                c.Close();
            }

        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnpdfprint_Click(object sender, EventArgs e)
        {
            DateTime dtfrom = FromDate.Value;
            DateTime dtto = todate.Value;
            //dt.Month.ToString()+"/"+dt.Date.ToString()+"/"+dt.Year.ToString()
            String dd = dtfrom.Month.ToString() + "/" + dtfrom.Day.ToString() + "/" + dtfrom.Year.ToString();
            String Starttime = dd + " 00:00:00";

            String dd1 = dtto.Month.ToString() + "/" + dtto.Day.ToString() + "/" + dtto.Year.ToString();
            String Endtime = dd1 + " 23:59:59";
            string SQLString = "SELECT  [RTO_CD],[HSRPRecord_AuthorizationNo],[VehicleRegNo],[OwnerName],[address], [MobileNo],[VehicleClass],[OrderType],[VehicleType], [Amount],[ChassisNo] ,[EngineNo],[hsrp_front_lasercode],[hsrp_rear_lasercode],[OrderStatus],[OrderEmbossingDate],[OrderClosedDate]  FROM [HSRP_APP_HP].[dbo].[OrderBookingOffLine] where  (Record_CreationDate between '" + Starttime + "' and '" + Endtime + "') or (OrderEmbossingDate between '" + Starttime + "' and '" + Endtime + "') or (OrderClosedDate between '" + Starttime + "' and '" + Endtime + "')";

            DataTable dt = (DataTable) utils.GetDataTable(SQLString, utils.getCnnHSRPApp.ToString());
          //utils.GetDataTable(SQLString, utils.getCnnHSRPApp.ToString());
            utils.ExportToPdf(dt);
            //  DateTime dtfrom = FromDate.Value;
            //  DateTime dtto = todate.Value;
            //  //dt.Month.ToString()+"/"+dt.Date.ToString()+"/"+dt.Year.ToString()
            //  String dd = dtfrom.Month.ToString() + "/" + dtfrom.Day.ToString() + "/" + dtfrom.Year.ToString();
            //  String Starttime = dd + " 00:00:00";

            //  String dd1 = dtto.Month.ToString() + "/" + dtto.Day.ToString() + "/" + dtto.Year.ToString();
            //  String Endtime = dd1 + " 23:59:59";
            //  string strquerry4 = string.Empty;
            //  string SQLString = string.Empty;

            ////  ConvertIndianCurrencyToWord objCur = new ConvertIndianCurrencyToWord();

            //  SQLString ="SELECT  [Record_CreationDate],[RTO_CD],[HSRPRecord_AuthorizationNo],[HSRPRecord_AuthorizationDate],[VehicleRegNo],[OwnerName],[address]  FROM [HSRP_APP_HP].[dbo].[OrderBookingOffLine] where  (Record_CreationDate between '" + Starttime + "' and '" + Endtime + "') or (OrderEmbossingDate between '" + Starttime + "' and '" + Endtime + "') or (OrderClosedDate between '" + Starttime + "' and '" + Endtime + "')";

            //  DataTable dt = utils.GetDataTable(SQLString, utils.getCnnHSRPApp.ToString());

            //  if (dt.Rows.Count > 0)
            //  {

            //      string filename = "Report" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

            //      //string PdfFolder = "D:\\PdfFile\\" + filename;
            //      //PdfReader reader = new PdfReader(PdfFolder);

            //      Document document = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            //      document.SetPageSize(iTextSharp.text.PageSize.A4);

            //      BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            //      //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
            //      //string PdfFolder = Path.GetDirectoryName(Application.ExecutablePath) +filename;
            //      string PdfFolder = "D:\\PdfFile\\" + filename;
            //      PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));
            //      //Opens the document:
            //      document.Open();

            //      //Adds content to the document:
            //      // document.Add(new Paragraph("Ignition Log Report"));
            //      PdfPTable table = new PdfPTable(7);

            //      //actual width of table in points
            //      table.TotalWidth = 1000f;

            //      PdfPCell cell1 = new PdfPCell(new Phrase("Report:", new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            //      cell1.Colspan = 7;
            //      cell1.BorderWidthLeft = .8f;
            //      cell1.BorderWidthRight = .8f;
            //      cell1.BorderWidthTop = .8f;
            //      cell1.BorderWidthBottom = .8f;

            //      cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell1);

            //      PdfPCell cell2 = new PdfPCell(new Phrase("State:HP", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      cell2.Colspan = 3;
            //      cell2.BorderWidthLeft = .8f;
            //      cell2.BorderWidthRight = .0f;
            //      cell2.BorderWidthTop = .0f;
            //      cell2.BorderWidthBottom = .8f;

            //      cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell2);

            //      PdfPCell cell3 = new PdfPCell(new Phrase("Location:", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.BLACK)));
            //      cell3.Colspan = 4;
            //      cell3.BorderWidthLeft = .0f;
            //      cell3.BorderWidthRight = .8f;
            //      cell3.BorderWidthTop = 0f;
            //      cell3.BorderWidthBottom = .8f;

            //      cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell3);

            //      PdfPCell cell4 = new PdfPCell(new Phrase("Report From:", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      cell4.Colspan = 3;
            //      cell4.BorderWidthLeft = .8f;
            //      cell4.BorderWidthRight = .0f;
            //      cell4.BorderWidthTop = 0f;
            //      cell4.BorderWidthBottom = .8f;

            //      cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell4);


            //      //PdfPCell cell5 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.BLACK)));
            //      //cell5.Colspan = 4;
            //      //cell5.BorderWidthLeft = .8f;
            //      //cell5.BorderWidthRight = .8f;
            //      //cell5.BorderWidthTop = 0f;
            //      //cell5.BorderWidthBottom = .0f;

            //      //cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell5);

            //      PdfPCell cell6 = new PdfPCell(new Phrase("Report To:", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      cell6.Colspan = 4;
            //      cell6.BorderWidthLeft = 0f;
            //      cell6.BorderWidthRight = .8f;
            //      cell6.BorderWidthTop = 0f;
            //      cell6.BorderWidthBottom = .8f;

            //      cell6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell6);





            //      PdfPCell cell18 = new PdfPCell(new Phrase("Record Creation Date :" + dt.Rows[0]["Record_CreationDate"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      cell18.Colspan = 2;
            //      cell18.BorderWidthLeft = 0f;
            //      cell18.BorderWidthRight = .8f;
            //      cell18.BorderWidthTop = 0f;
            //      cell18.BorderWidthBottom = .8f;

            //      cell18.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell18);

            //      //PdfPCell cell19 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell19.Colspan = 3;
            //      //cell19.BorderWidthLeft = .8f;
            //      //cell19.BorderWidthRight = .8f;
            //      //cell19.BorderWidthTop = 0f;
            //      //cell19.BorderWidthBottom = 0f;

            //      //cell19.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell19);

            //      PdfPCell cell20 = new PdfPCell(new Phrase("RTO CODE : " + dt.Rows[0]["RTO_CD"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      cell20.Colspan = 2;
            //      cell20.BorderWidthLeft = 0f;
            //      cell20.BorderWidthRight = .8f;
            //      cell20.BorderWidthTop = 0f;
            //      cell20.BorderWidthBottom = 0f;

            //      cell20.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell20);


            //      //PdfPCell cell21 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell21.Colspan = 3;
            //      //cell21.BorderWidthLeft = .8f;
            //      //cell21.BorderWidthRight = .8f;
            //      //cell21.BorderWidthTop = 0f;
            //      //cell21.BorderWidthBottom = 0f;

            //      //cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell21);


            //      PdfPCell cell22 = new PdfPCell(new Phrase("HSRPRecord Authorization No : " + dt.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      cell22.Colspan = 2;
            //      cell22.BorderWidthLeft = 0f;
            //      cell22.BorderWidthRight = .8f;
            //      cell22.BorderWidthTop = 0f;
            //      cell22.BorderWidthBottom = 0f;

            //      cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell22);


            //      //PdfPCell cell23 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell23.Colspan = 3;
            //      //cell23.BorderWidthLeft = .8f;
            //      //cell23.BorderWidthRight = .8f;
            //      //cell23.BorderWidthTop = 0f;
            //      //cell23.BorderWidthBottom = 0f;

            //      //cell23.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell23);



            //      //PdfPCell cell25 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell25.Colspan = 3;
            //      //cell25.BorderWidthLeft = .8f;
            //      //cell25.BorderWidthRight = .8f;
            //      //cell25.BorderWidthTop = 0f;
            //      //cell25.BorderWidthBottom = .8f;

            //      //cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell25);



            //      //PdfPCell cell27 = new PdfPCell(new Phrase("Consignee :", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell27.Colspan = 3;
            //      //cell27.BorderWidthLeft = .8f;
            //      //cell27.BorderWidthRight = .8f;
            //      //cell27.BorderWidthTop = 0f;
            //      //cell27.BorderWidthBottom = 0f;

            //      //cell27.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell27);


            //      PdfPCell cell28 = new PdfPCell(new Phrase("Vehicle Reg No:" + dt.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      cell28.Colspan = 1;
            //      cell28.BorderWidthLeft = 0f;
            //      cell28.BorderWidthRight = .8f;
            //      cell28.BorderWidthTop = 0f;
            //      cell28.BorderWidthBottom = 0f;

            //      cell28.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell28);


            //      PdfPCell cell29 = new PdfPCell(new Phrase("Owner Name"+dt.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            //      cell29.Colspan = 1;
            //      cell29.BorderWidthLeft = .8f;
            //      cell29.BorderWidthRight = .8f;
            //      cell29.BorderWidthTop = 0f;
            //      cell29.BorderWidthBottom = 0f;

            //      cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell29);




            //       PdfPCell cell31 = new PdfPCell(new Phrase("address" + dt.Rows[0]["address"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
            //      cell31.Colspan = 1;
            //      cell31.BorderWidthLeft = .8f;
            //      cell31.BorderWidthRight = .8f;
            //      cell31.BorderWidthTop = 0f;
            //      cell31.BorderWidthBottom = 0f;

            //      cell31.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      table.AddCell(cell31);



            //      //PdfPCell cell32 = new PdfPCell(new Phrase("MobileNo:" + dt.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell32.Colspan = 4;
            //      //cell32.BorderWidthLeft = 0f;
            //      //cell32.BorderWidthRight = .8f;
            //      //cell32.BorderWidthTop = 0f;
            //      //cell32.BorderWidthBottom = .8f;

            //      //cell32.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell32);






            //      //PdfPCell cell34 = new PdfPCell(new Phrase("Vehicle Class.: " + dt.Rows[0]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell34.Colspan = 4;
            //      //cell34.BorderWidthLeft = 0f;
            //      //cell34.BorderWidthRight = .8f;
            //      //cell34.BorderWidthTop = 0f;
            //      //cell34.BorderWidthBottom = 0f;

            //      //cell34.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell34);


            //      //PdfPCell cell35 = new PdfPCell(new Phrase("OrderType"+dt.Rows[0]["OrderType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell35.Colspan = 3;
            //      //cell35.BorderWidthLeft = .8f;
            //      //cell35.BorderWidthRight = .8f;
            //      //cell35.BorderWidthTop = 0f;
            //      //cell35.BorderWidthBottom = 0f;

            //      //cell35.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell35);


            //      //PdfPCell cell36 = new PdfPCell(new Phrase("VehicleType: " + dt.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell36.Colspan = 4;
            //      //cell36.BorderWidthLeft = 0f;
            //      //cell36.BorderWidthRight = .8f;
            //      //cell36.BorderWidthTop = 0f;
            //      //cell36.BorderWidthBottom = 0f;

            //      //cell36.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell36);

            //      //PdfPCell cell38 = new PdfPCell(new Phrase("Cash Receipt No" + dt.Rows[0]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell38.Colspan = 2;
            //      //cell38.BorderWidthLeft = 0f;
            //      //cell38.BorderWidthRight = 0f;
            //      //cell38.BorderWidthTop = 0f;
            //      //cell38.BorderWidthBottom = 0f;

            //      //cell38.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell38);

            //      //PdfPCell cell38abc = new PdfPCell(new Phrase("Amount : " + dt.Rows[0]["Amount"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell38abc.Colspan = 2;
            //      //cell38abc.BorderWidthLeft = 0f;
            //      //cell38abc.BorderWidthRight = .8f;
            //      //cell38abc.BorderWidthTop = 0f;
            //      //cell38abc.BorderWidthBottom = 0f;

            //      //cell38abc.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell38abc);

            //      //PdfPCell cell39 = new PdfPCell(new Phrase("Chassis No" + dt.Rows[0]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell39.Colspan = 2;
            //      //cell39.BorderWidthLeft = 0f;
            //      //cell39.BorderWidthRight = 0f;
            //      //cell39.BorderWidthTop = 0f;
            //      //cell39.BorderWidthBottom = 0f;

            //      //cell39.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell39);

            //      //PdfPCell cell40 = new PdfPCell(new Phrase("Engine No" + dt.Rows[0]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell40.Colspan = 2;
            //      //cell40.BorderWidthLeft = 0f;
            //      //cell40.BorderWidthRight = 0f;
            //      //cell40.BorderWidthTop = 0f;
            //      //cell40.BorderWidthBottom = 0f;

            //      //cell40.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell40);

            //      //PdfPCell cell41 = new PdfPCell(new Phrase("hsrp Front lasercode" + dt.Rows[0]["hsrp_front_lasercode"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell41.Colspan = 2;
            //      //cell41.BorderWidthLeft = 0f;
            //      //cell41.BorderWidthRight = 0f;
            //      //cell41.BorderWidthTop = 0f;
            //      //cell41.BorderWidthBottom = 0f;

            //      //cell41.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell41);

            //      //PdfPCell cell42 = new PdfPCell(new Phrase("hsrp Rear lasercode" + dt.Rows[0]["hsrp_rear_lasercode"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell42.Colspan = 2;
            //      //cell42.BorderWidthLeft = 0f;
            //      //cell42.BorderWidthRight = 0f;
            //      //cell42.BorderWidthTop = 0f;
            //      //cell42.BorderWidthBottom = 0f;

            //      //cell42.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell42);

            //      // PdfPCell cell43 = new PdfPCell(new Phrase("OrderStatus" + dt.Rows[0]["OrderStatus"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell43.Colspan = 2;
            //      //cell43.BorderWidthLeft = 0f;
            //      //cell43.BorderWidthRight = 0f;
            //      //cell43.BorderWidthTop = 0f;
            //      //cell43.BorderWidthBottom = 0f;

            //      //cell42.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell43);

            //      //PdfPCell cell44 = new PdfPCell(new Phrase("Order Embossing Date" + dt.Rows[0]["OrderEmbossingDate"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell44.Colspan = 2;
            //      //cell44.BorderWidthLeft = 0f;
            //      //cell44.BorderWidthRight = 0f;
            //      //cell44.BorderWidthTop = 0f;
            //      //cell44.BorderWidthBottom = 0f;

            //      //cell44.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell44);

            //      //PdfPCell cell45 = new PdfPCell(new Phrase("Order Closed Date" + dt.Rows[0]["OrderClosedDate"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell45.Colspan = 2;
            //      //cell45.BorderWidthLeft = 0f;
            //      //cell45.BorderWidthRight = 0f;
            //      //cell45.BorderWidthTop = 0f;
            //      //cell45.BorderWidthBottom = 0f;

            //      //cell45.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell45);

            //      //PdfPCell cell46 = new PdfPCell(new Phrase("FirstSMSSentFlag" + dt.Rows[0]["FirstSMSSentFlag"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell46.Colspan = 2;
            //      //cell46.BorderWidthLeft = 0f;
            //      //cell46.BorderWidthRight = 0f;
            //      //cell46.BorderWidthTop = 0f;
            //      //cell46.BorderWidthBottom = 0f;

            //      //cell46.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell46);

            //      //PdfPCell cell47 = new PdfPCell(new Phrase("FirstSMSText" + dt.Rows[0]["FirstSMSText"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell47.Colspan = 2;
            //      //cell47.BorderWidthLeft = 0f;
            //      //cell47.BorderWidthRight = 0f;
            //      //cell47.BorderWidthTop = 0f;
            //      //cell47.BorderWidthBottom = 0f;

            //      //cell47.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell47);

            //      //PdfPCell cell48 = new PdfPCell(new Phrase("FirstSMSDateTime" + dt.Rows[0]["FirstSMSDateTime"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell48.Colspan = 2;
            //      //cell48.BorderWidthLeft = 0f;
            //      //cell48.BorderWidthRight = 0f;
            //      //cell48.BorderWidthTop = 0f;
            //      //cell48.BorderWidthBottom = 0f;

            //      //cell48.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell48);

            //      //PdfPCell cell49 = new PdfPCell(new Phrase("FirstSMSServerReponseID" + dt.Rows[0]["FirstSMSServerReponseID"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell49.Colspan = 2;
            //      //cell49.BorderWidthLeft = 0f;
            //      //cell49.BorderWidthRight = 0f;
            //      //cell49.BorderWidthTop = 0f;
            //      //cell49.BorderWidthBottom = 0f;

            //      //cell49.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell49);

            //      //PdfPCell cell50 = new PdfPCell(new Phrase("FirstSMSServerResponseText" + dt.Rows[0]["FirstSMSServerResponseText"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell50.Colspan = 2;
            //      //cell50.BorderWidthLeft = 0f;
            //      //cell50.BorderWidthRight = 0f;
            //      //cell50.BorderWidthTop = 0f;
            //      //cell50.BorderWidthBottom = 0f;

            //      //cell50.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell50);

            //      //PdfPCell cell51 = new PdfPCell(new Phrase("ThirdSMSText" + dt.Rows[0]["ThirdSMSText"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell51.Colspan = 2;
            //      //cell51.BorderWidthLeft = 0f;
            //      //cell51.BorderWidthRight = 0f;
            //      //cell51.BorderWidthTop = 0f;
            //      //cell51.BorderWidthBottom = 0f;

            //      //cell51.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell51);

            //      //PdfPCell cell52 = new PdfPCell(new Phrase("ThirdSMSDateTime" + dt.Rows[0]["ThirdSMSDateTime"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell52.Colspan = 2;
            //      //cell52.BorderWidthLeft = 0f;
            //      //cell52.BorderWidthRight = 0f;
            //      //cell52.BorderWidthTop = 0f;
            //      //cell52.BorderWidthBottom = 0f;

            //      //cell52.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell52);

            //      // PdfPCell cell53 = new PdfPCell(new Phrase("ThirdSMSServerReponseID" + dt.Rows[0]["ThirdSMSServerReponseID"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      //cell53.Colspan = 2;
            //      //cell53.BorderWidthLeft = 0f;
            //      //cell53.BorderWidthRight = 0f;
            //      //cell53.BorderWidthTop = 0f;
            //      //cell53.BorderWidthBottom = 0f;

            //      //cell53.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      //table.AddCell(cell53);

            //      // PdfPCell cell54 = new PdfPCell(new Phrase("ThirdSMSServerResponseText" + dt.Rows[0]["ThirdSMSServerResponseText"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      // cell54.Colspan = 2;
            //      // cell54.BorderWidthLeft = 0f;
            //      // cell54.BorderWidthRight = 0f;
            //      // cell54.BorderWidthTop = 0f;
            //      // cell54.BorderWidthBottom = 0f;

            //      // cell54.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      // table.AddCell(cell54);
            //      // PdfPCell cell55 = new PdfPCell(new Phrase("IsProcessed" + dt.Rows[0]["IsProcessed"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
            //      // cell55.Colspan = 2;
            //      // cell55.BorderWidthLeft = 0f;
            //      // cell55.BorderWidthRight = 0f;
            //      // cell55.BorderWidthTop = 0f;
            //      // cell55.BorderWidthBottom = 0f;

            //      // cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //      // table.AddCell(cell55);

            //      document.Add(table);
            //      bool result = document.NewPage();


            //      document.Close();

            //      System.Diagnostics.Process.Start(PdfFolder);
            //  }
            //  else
            //  {
            //      MessageBox.Show("No Record Found!");
            //  }
        }

        private void btnexporttoexcel_Click(object sender, EventArgs e)
        {

            DateTime dtfrom = FromDate.Value;
            DateTime dtto = todate.Value;
            //dt.Month.ToString()+"/"+dt.Date.ToString()+"/"+dt.Year.ToString()
            String dd = dtfrom.Month.ToString() + "/" + dtfrom.Day.ToString() + "/" + dtfrom.Year.ToString();
            String Starttime = dd + " 00:00:00";

            String dd1 = dtto.Month.ToString() + "/" + dtto.Day.ToString() + "/" + dtto.Year.ToString();
            String Endtime = dd1 + " 23:59:59";
            string queryString1 = "SELECT [Record_CreationDate],[RTO_CD],[HSRPRecord_AuthorizationNo],[HSRPRecord_AuthorizationDate],[VehicleRegNo],[OwnerName],[address] ,[MobileNo],[VehicleClass],[OrderType],[VehicleType],[CashReceiptNo], [Amount],[ChassisNo] ,[EngineNo],[hsrp_front_lasercode],[hsrp_rear_lasercode],[OrderStatus],[OrderEmbossingDate],[OrderClosedDate],[FirstSMSSentFlag],[FirstSMSText],[FirstSMSDateTime],[FirstSMSServerResponseID],[FirstSMSServerResponseText],[SecondSMSText],[SecondSMSDateTime],[SecondSMSServerResponseID] ,[SecondSMSServerResponseText],[ThirdSMSText],[ThirdSMSDateTime],[ThirdSMSServerResponseID],[ThirdSMSServerResponseText] ,[IsProcessed]  FROM [HSRP_APP_HP].[dbo].[OrderBookingOffLine] where  (Record_CreationDate between '" + Starttime + "' and '" + Endtime + "') or (OrderEmbossingDate between '" + Starttime + "' and '" + Endtime + "') or (OrderClosedDate between '" + Starttime + "' and '" + Endtime + "') ";

            DataTable dt = (DataTable)utils.GetDataTable(queryString1, utils.getCnnHSRPApp.ToString());
            utils.ExportToExcel(dt);

        }
    }
}
