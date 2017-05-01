using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace HSRP.Transaction
{
    public partial class CashReceptPaymentOrderWise : System.Web.UI.Page
    {
        public static string AddRecordBy = "";
        public static string CounterNo = "";
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        SqlConnection con = new SqlConnection(ConnectionString);

        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string ProductivityID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string macbase = string.Empty;
        string Query = string.Empty;
        int intDepositAmount = 0;
        int inttotcoll = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    UserType = Session["UserType"].ToString();

                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            USERID = Session["UID"].ToString();
            macbase = Session["MacAddress"].ToString();
            if (!IsPostBack)
            {
                if (UserType.Equals("0"))
                {
                    FillDropDown();
                }
                else
                {
                    FillDropDown();
                }
            }
            else
            {

            }
        }
        string SQLString = string.Empty;


        public void DownloadReceipt(DataTable dt)
        {
            //string SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where  HSRP_StateID=11 and paymentorderid='" + TxtSearchtype.Text + "'";
            DataTable dataSetFillHSRPDeliveryChallan = dt;// Utils.GetDataTable(SQLString, ConnectionString);            
            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {
                DataTable GetAddress;
                string Address;


                DataTable rtoaddr = new DataTable();
                rtoaddr = Utils.GetDataTable("select r.RTOLocationAddress from users as u inner join rtolocation as r on u.RTOLocationID=r.RTOLocationID where userid='" + Session["UID"].ToString() + "'", ConnectionString);

                string sqlquery = "select Address1 from AffixationCenters where Rto_Id='" + dataSetFillHSRPDeliveryChallan.Rows[0]["RTOLocationID"].ToString() + "'";
                string AffAddress = Utils.getDataSingleValue(sqlquery, ConnectionString, "Address1");


                GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", ConnectionString);
                string getTinNo = GetAddress.Rows[0]["TinNo"].ToString();

                if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
                {
                    Address = " - " + GetAddress.Rows[0]["pincode"];

                }
                else
                {
                    Address = "";
                }

                if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
                {


                    string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


                    String StringField = String.Empty;
                    String StringAlert = String.Empty;


                    //Creates an instance of the iTextSharp.text.Document-object:
                    Document document = new Document();

                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                    // iTextSharp.text.Font times = new F ont(bfTimes, 12, Font.ITALIC, Color.Black);
                    //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                    string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                    PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                    //Opens the document:
                    document.Open();

                    //Adds content to the document:
                    // document.Add(new Paragraph("Ignition Log Report"));
                    PdfPTable table = new PdfPTable(4);
                    //actual width of table in points
                    table.TotalWidth = 585f;

                    PdfPTable table1 = new PdfPTable(4);
                    //actual width of table in points
                    table1.TotalWidth = 585f;

                    PdfPTable table2 = new PdfPTable(4);
                    //actual width of table in points
                    table2.TotalWidth = 585f;

                    PdfPTable table3 = new PdfPTable(4);
                    //actual width of table in points
                    table3.TotalWidth = 585f;

                    PdfPTable table4 = new PdfPTable(4);
                    //actual width of table in points
                    table4.TotalWidth = 585f;

                    PdfPTable table5 = new PdfPTable(4);
                    //actual width of table in points
                    table5.TotalWidth = 585f;

                    //fix the absolute width of the table


                    PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell312.Colspan = 4;
                    cell312.BorderColor = BaseColor.WHITE;
                    cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table1.AddCell(cell312);

                    //PdfPCell cell312a = new PdfPCell(new Phrase("Duplicate", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //cell312a.Colspan = 4;
                    //cell312a.BorderColor = BaseColor.WHITE;
                    //cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table2.AddCell(cell312a);

                    PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12.Colspan = 4;
                    cell12.BorderColor = BaseColor.WHITE;
                    cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell12);

                    PdfPCell cell1203 = new PdfPCell(new Phrase("" + AffAddress, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1203.Colspan = 4;
                    cell1203.BorderWidthLeft = 0f;
                    cell1203.BorderWidthRight = 0f;
                    cell1203.BorderWidthTop = 0f;
                    cell1203.BorderWidthBottom = 0f;
                    cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1203);

                    //PdfPCell cell = new PdfPCell(new Phrase("WE HEREBY CONFIRM TO HAVE INSTALLED THE HSRP SET AS DETAILED BELOW : ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell.Colspan = 4;
                    //cell.BorderColor = BaseColor.WHITE;
                    //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell);

                    PdfPCell cell0 = new PdfPCell(new Phrase("HSRP FEE RECEIPT", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                    cell0.Colspan = 4;
                    cell0.BorderWidthLeft = 0f;
                    cell0.BorderWidthRight = 0f;
                    cell0.BorderWidthTop = 0f;
                    cell0.BorderWidthBottom = 0f;

                    cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell0);


                    PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1.Colspan = 4;
                    cell1.BorderWidthLeft = 0f;
                    cell1.BorderWidthRight = 0f;
                    cell1.BorderWidthTop = 0f;
                    cell1.BorderWidthBottom = 0f;

                    cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1);




                    PdfPCell cellInv2 = new PdfPCell(new Phrase("ORDER No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv2.Colspan = 1;

                    cellInv2.BorderWidthLeft = 0f;
                    cellInv2.BorderWidthRight = 0f;
                    cellInv2.BorderWidthTop = 0f;
                    cellInv2.BorderWidthBottom = 0f;
                    cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv2);

                    PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellInv22111.Colspan = 1;
                    cellInv22111.BorderWidthLeft = 0f;
                    cellInv22111.BorderWidthRight = 0f;
                    cellInv22111.BorderWidthTop = 0f;
                    cellInv22111.BorderWidthBottom = 0f;
                    cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellInv22111);

                    PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell21.Colspan = 1;

                    cell21.BorderWidthLeft = 0f;
                    cell21.BorderWidthRight = 0f;
                    cell21.BorderWidthTop = 0f;
                    cell21.BorderWidthBottom = 0f;
                    cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell21);
                    string CashReceiptDateTime = string.Empty;

                    if (dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString() == "")
                    {
                        CashReceiptDateTime = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        CashReceiptDateTime = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("dd/MM/yyyy");
                    }
                    PdfPCell cell212 = new PdfPCell(new Phrase(": " + CashReceiptDateTime, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell212.Colspan = 1;

                    cell212.BorderWidthLeft = 0f;
                    cell212.BorderWidthRight = 0f;
                    cell212.BorderWidthTop = 0f;
                    cell212.BorderWidthBottom = 0f;
                    cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell212);



                    PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2.Colspan = 1;

                    cell2.BorderWidthLeft = 0f;
                    cell2.BorderWidthRight = 0f;
                    cell2.BorderWidthTop = 0f;
                    cell2.BorderWidthBottom = 0f;
                    cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2);

                    // = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

                    PdfPCell cell22111 = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22111.Colspan = 1;
                    cell22111.BorderWidthLeft = 0f;
                    cell22111.BorderWidthRight = 0f;
                    cell22111.BorderWidthTop = 0f;
                    cell22111.BorderWidthBottom = 0f;
                    cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22111);




                    PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell22.Colspan = 1;

                    cell22.BorderWidthLeft = 0f;
                    cell22.BorderWidthRight = 0f;
                    cell22.BorderWidthTop = 0f;
                    cell22.BorderWidthBottom = 0f;
                    cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell22);

                    PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell222.Colspan = 1;

                    cell222.BorderWidthLeft = 0f;
                    cell222.BorderWidthRight = 0f;
                    cell222.BorderWidthTop = 0f;
                    cell222.BorderWidthBottom = 0f;
                    cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell222);

                    string getExciseNo1 = Utils.getScalarValue("select ExciseNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
                    PdfPCell cell221 = new PdfPCell(new Phrase("EXCISE NO ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell221.Colspan = 1;
                    cell221.BorderWidthLeft = 0f;
                    cell221.BorderWidthRight = 0f;
                    cell221.BorderWidthTop = 0f;
                    cell221.BorderWidthBottom = 0f;
                    cell221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell221);

                    PdfPCell cell2221 = new PdfPCell(new Phrase(": " + getExciseNo1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell2221.Colspan = 3;

                    cell2221.BorderWidthLeft = 0f;
                    cell2221.BorderWidthRight = 0f;
                    cell2221.BorderWidthTop = 0f;
                    cell2221.BorderWidthBottom = 0f;
                    cell2221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2221);

                    PdfPCell cell15 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell15.Colspan = 4;
                    cell15.BorderWidthLeft = 0f;
                    cell15.BorderWidthRight = 0f;
                    cell15.BorderWidthTop = 0f;
                    cell15.BorderWidthBottom = 0f;
                    table.AddCell(cell15);


                    PdfPCell cell5 = new PdfPCell(new Phrase("AUTH NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell5.Colspan = 1;

                    cell5.BorderWidthLeft = 0f;
                    cell5.BorderWidthRight = 0f;
                    cell5.BorderWidthTop = 0f;
                    cell5.BorderWidthBottom = 0f;
                    cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell5);

                    PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell55.Colspan = 1;

                    cell55.BorderWidthLeft = 0f;
                    cell55.BorderWidthRight = 0f;
                    cell55.BorderWidthTop = 0f;
                    cell55.BorderWidthBottom = 0f;
                    cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell55);

                    PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell25.Colspan = 1;

                    cell25.BorderWidthLeft = 0f;
                    cell25.BorderWidthRight = 0f;
                    cell25.BorderWidthTop = 0f;
                    cell25.BorderWidthBottom = 0f;
                    cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell25);

                    DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 1;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);


                    DateTime Orddate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString());

                    PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell7.Colspan = 1;
                    cell7.BorderWidthLeft = 0f;
                    cell7.BorderWidthRight = 0f;
                    cell7.BorderWidthTop = 0f;
                    cell7.BorderWidthBottom = 0f;
                    cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell7);

                    PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell75.Colspan = 1;
                    cell75.BorderWidthLeft = 0f;
                    cell75.BorderWidthRight = 0f;
                    cell75.BorderWidthTop = 0f;
                    cell75.BorderWidthBottom = 0f;
                    cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell75);

                    PdfPCell cell29 = new PdfPCell(new Phrase("OWNER CONTACT NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell29.Colspan = 1;
                    cell29.BorderWidthLeft = 0f;
                    cell29.BorderWidthRight = 0f;
                    cell29.BorderWidthTop = 0f;
                    cell29.BorderWidthBottom = 0f;
                    cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell29);

                    PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell295.Colspan = 1;
                    cell295.BorderWidthLeft = 0f;
                    cell295.BorderWidthRight = 0f;
                    cell295.BorderWidthTop = 0f;
                    cell295.BorderWidthBottom = 0f;
                    cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell295);



                    PdfPCell cell8 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell8.Colspan = 1;
                    cell8.BorderWidthLeft = 0f;
                    cell8.BorderWidthRight = 0f;
                    cell8.BorderWidthTop = 0f;
                    cell8.BorderWidthBottom = 0f;
                    cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell8);

                    PdfPCell cell85 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell85.Colspan = 3;
                    cell85.BorderWidthLeft = 0f;
                    cell85.BorderWidthRight = 0f;
                    cell85.BorderWidthTop = 0f;
                    cell85.BorderWidthBottom = 0f;
                    cell85.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell85);

                    PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell9.Colspan = 1;
                    cell9.BorderWidthLeft = 0f;
                    cell9.BorderWidthRight = 0f;
                    cell9.BorderWidthTop = 0f;
                    cell9.BorderWidthBottom = 0f;
                    cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell9);

                    PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell95.Colspan = 1;
                    cell95.BorderWidthLeft = 0f;
                    cell95.BorderWidthRight = 0f;
                    cell95.BorderWidthTop = 0f;
                    cell95.BorderWidthBottom = 0f;
                    cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell95);

                    PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell10.Colspan = 1;
                    cell10.BorderWidthLeft = 0f;
                    cell10.BorderWidthRight = 0f;
                    cell10.BorderWidthTop = 0f;
                    cell10.BorderWidthBottom = 0f;
                    cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell10);

                    PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell105.Colspan = 1;
                    cell105.BorderWidthLeft = 0f;
                    cell105.BorderWidthRight = 0f;
                    cell105.BorderWidthTop = 0f;
                    cell105.BorderWidthBottom = 0f;
                    cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell105);

                    PdfPCell cell11 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell11.Colspan = 1;
                    cell11.BorderWidthLeft = 0f;
                    cell11.BorderWidthRight = 0f;
                    cell11.BorderWidthTop = 0f;
                    cell11.BorderWidthBottom = 0f;
                    cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell11);

                    PdfPCell cell115 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell115.Colspan = 1;
                    cell115.BorderWidthLeft = 0f;
                    cell115.BorderWidthRight = 0f;
                    cell115.BorderWidthTop = 0f;
                    cell115.BorderWidthBottom = 0f;
                    cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell115);

                    PdfPCell cell1113 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1113.Colspan = 1;
                    cell1113.BorderWidthLeft = 0f;
                    cell1113.BorderWidthRight = 0f;
                    cell1113.BorderWidthTop = 0f;
                    cell1113.BorderWidthBottom = 0f;
                    cell1113.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1113);

                    PdfPCell cell11135 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell11135.Colspan = 1;
                    cell11135.BorderWidthLeft = 0f;
                    cell11135.BorderWidthRight = 0f;
                    cell11135.BorderWidthTop = 0f;
                    cell11135.BorderWidthBottom = 0f;
                    cell11135.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell11135);

                    PdfPCell cell1112 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1112.Colspan = 4;
                    cell1112.BorderWidthLeft = 0f;
                    cell1112.BorderWidthRight = 0f;
                    cell1112.BorderWidthTop = 0f;
                    cell1112.BorderWidthBottom = 0f;
                    cell1112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1112);


                    PdfPCell cellspa12 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellspa12.Colspan = 1;
                    cellspa12.BorderWidthLeft = 0f;
                    cellspa12.BorderWidthRight = 0f;
                    cellspa12.BorderWidthTop = 0f;
                    cellspa12.BorderWidthBottom = 0f;
                    cellspa12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellspa12);


                    PdfPCell cell112 = new PdfPCell(new Phrase("DESCRIPTION", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell112.Colspan = 1;
                    cell112.BorderWidthLeft = 0f;
                    cell112.BorderWidthRight = 0f;
                    cell112.BorderWidthTop = 0f;
                    cell112.BorderWidthBottom = 0f;
                    cell112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell112);

                    PdfPCell cellspa1s2 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellspa1s2.Colspan = 1;
                    cellspa1s2.BorderWidthLeft = 0f;
                    cellspa1s2.BorderWidthRight = 0f;
                    cellspa1s2.BorderWidthTop = 0f;
                    cellspa1s2.BorderWidthBottom = 0f;
                    cellspa1s2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellspa1s2);

                    PdfPCell cell119 = new PdfPCell(new Phrase("AMOUNT(RS)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell119.Colspan = 1;
                    cell119.BorderWidthLeft = 0f;
                    cell119.BorderWidthRight = 0f;
                    cell119.BorderWidthTop = 0f;
                    cell119.BorderWidthBottom = 0f;
                    cell119.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell119);

                    PdfPCell cellDesc = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellDesc.Colspan = 1;
                    cellDesc.BorderWidthLeft = 0f;
                    cellDesc.BorderWidthRight = 0f;
                    cellDesc.BorderWidthTop = 0f;
                    cellDesc.BorderWidthBottom = 0f;
                    cellDesc.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellDesc);

                    PdfPCell cellDescSet = new PdfPCell(new Phrase("SET OF HSRP PLATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellDescSet.Colspan = 1;
                    cellDescSet.BorderWidthLeft = 0f;
                    cellDescSet.BorderWidthRight = 0f;
                    cellDescSet.BorderWidthTop = 0f;
                    cellDescSet.BorderWidthBottom = 0f;
                    cellDescSet.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellDescSet);

                    PdfPCell cellDesc1Sp = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellDesc1Sp.Colspan = 1;
                    cellDesc1Sp.BorderWidthLeft = 0f;
                    cellDesc1Sp.BorderWidthRight = 0f;
                    cellDesc1Sp.BorderWidthTop = 0f;
                    cellDesc1Sp.BorderWidthBottom = 0f;
                    cellDesc1Sp.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellDesc1Sp);

                    PdfPCell cellDescSp = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString() + "*", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cellDescSp.Colspan = 1;
                    cellDescSp.BorderWidthLeft = 0f;
                    cellDescSp.BorderWidthRight = 0f;
                    cellDescSp.BorderWidthTop = 0f;
                    cellDescSp.BorderWidthBottom = 0f;
                    cellDescSp.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellDescSp);


                    PdfPCell cell1195 = new PdfPCell(new Phrase("________________________________________________________________________", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1195.Colspan = 4;
                    cell1195.BorderWidthLeft = 0f;
                    cell1195.BorderWidthRight = 0f;
                    cell1195.BorderWidthTop = 0f;
                    cell1195.BorderWidthBottom = 0f;
                    cell1195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1195);

                    PdfPCell cell1201 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1201.Colspan = 4;
                    cell1201.BorderWidthLeft = 0f;
                    cell1201.BorderWidthRight = 0f;
                    cell1201.BorderWidthTop = 0f;
                    cell1201.BorderWidthBottom = 0f;
                    cell1201.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1201);

                    PdfPCell cell1202 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1202.Colspan = 4;
                    cell1202.BorderWidthLeft = 0f;
                    cell1202.BorderWidthRight = 0f;
                    cell1202.BorderWidthTop = 0f;
                    cell1202.BorderWidthBottom = 0f;
                    cell1202.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1202);


                    PdfPCell celldupRouCash401 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupRouCash401.Colspan = 1;
                    celldupRouCash401.BorderWidthLeft = 0f;
                    celldupRouCash401.BorderWidthRight = 0f;
                    celldupRouCash401.BorderWidthTop = 0f;
                    celldupRouCash401.BorderWidthBottom = 0f;
                    celldupRouCash401.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupRouCash401);

                    PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OF AMOUNT ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupCash401.Colspan = 1;
                    celldupCash401.BorderWidthLeft = 0f;
                    celldupCash401.BorderWidthRight = 0f;
                    celldupCash401.BorderWidthTop = 0f;
                    celldupCash401.BorderWidthBottom = 0f;
                    celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupCash401);

                    PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupRouCash402.Colspan = 1;
                    celldupRouCash402.BorderWidthLeft = 0f;
                    celldupRouCash402.BorderWidthRight = 0f;
                    celldupRouCash402.BorderWidthTop = 0f;
                    celldupRouCash402.BorderWidthBottom = 0f;
                    celldupRouCash402.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupRouCash402);

                    decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                    roundAmt = Math.Round(roundAmt, 0);

                    PdfPCell celldupCash402 = new PdfPCell(new Phrase(" " + roundAmt + ".00", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    celldupCash402.Colspan = 1;
                    celldupCash402.BorderWidthLeft = 0f;
                    celldupCash402.BorderWidthRight = 0f;
                    celldupCash402.BorderWidthTop = 0f;
                    celldupCash402.BorderWidthBottom = 0f;
                    celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(celldupCash402);

                    //-----------------------------


                    //-----------------------------

                    string Messagefff = "\u2022" + " This receipt issued, subject to realization of amount.";

                    PdfPCell cell64fff = new PdfPCell(new Phrase(Messagefff, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell64fff.Colspan = 4;
                    cell64fff.BorderWidthLeft = 0f;
                    cell64fff.BorderWidthRight = 0f;
                    cell64fff.BorderWidthTop = 0f;
                    cell64fff.BorderWidthBottom = 0f;
                    cell64fff.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell64fff);



                    string Messagenew = "\u2022" + " Payment against your authorization has been recieved. HSRP will be processed once the vehicle registration no.  will be provided by the department.";

                    PdfPCell cellnew = new PdfPCell(new Phrase(Messagenew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellnew.Colspan = 4;
                    cellnew.BorderWidthLeft = 0f;
                    cellnew.BorderWidthRight = 0f;
                    cellnew.BorderWidthTop = 0f;
                    cellnew.BorderWidthBottom = 0f;
                    cellnew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cellnew);




                    string Messageff = "\u2022" + " Fitment Charges Included.";

                    PdfPCell cell64ff = new PdfPCell(new Phrase(Messageff, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell64ff.Colspan = 4;
                    cell64ff.BorderWidthLeft = 0f;
                    cell64ff.BorderWidthRight = 0f;
                    cell64ff.BorderWidthTop = 0f;
                    cell64ff.BorderWidthBottom = 0f;
                    cell64ff.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell64ff);


                    string Message = "\u2022" + " Please verify the details on Order No and Contact our Customer Care immediately in case details found not correct.";

                    PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell64.Colspan = 4;
                    cell64.BorderWidthLeft = 0f;
                    cell64.BorderWidthRight = 0f;
                    cell64.BorderWidthTop = 0f;
                    cell64.BorderWidthBottom = 0f;
                    cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell64);

                    string MessageSec11 = "\u2022" + " The prices are inclusive of Taxes.";

                    PdfPCell cell631 = new PdfPCell(new Phrase(MessageSec11, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell631.Colspan = 4;
                    cell631.BorderWidthLeft = 0f;
                    cell631.BorderWidthRight = 0f;
                    cell631.BorderWidthTop = 0f;
                    cell631.BorderWidthBottom = 0f;
                    cell631.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell631);


                    string MessageSec = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day, a confirmation SMS will be sent to the registered mobile number provided by the customer.";

                    PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell65a.Colspan = 4;
                    cell65a.BorderWidthLeft = 0f;
                    cell65a.BorderWidthRight = 0f;
                    cell65a.BorderWidthTop = 0f;
                    cell65a.BorderWidthBottom = 0f;
                    cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell65a);

                    string MessageSecee = "\u2022" + " All disputes are subject to Jurisdiction of Hyderabad (Telengana) only..";

                    PdfPCell cell65aaa = new PdfPCell(new Phrase(MessageSecee, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell65aaa.Colspan = 4;
                    cell65aaa.BorderWidthLeft = 0f;
                    cell65aaa.BorderWidthRight = 0f;
                    cell65aaa.BorderWidthTop = 0f;
                    cell65aaa.BorderWidthBottom = 0f;
                    cell65aaa.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell65aaa);

                    string MessageSece = "\u2022" + " This is Computer generated receipt hence signature not required.";

                    PdfPCell cell65aa = new PdfPCell(new Phrase(MessageSece, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell65aa.Colspan = 4;
                    cell65aa.BorderWidthLeft = 0f;
                    cell65aa.BorderWidthRight = 0f;
                    cell65aa.BorderWidthTop = 0f;
                    cell65aa.BorderWidthBottom = 0f;
                    cell65aa.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell65aa);

                    PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell63.Colspan = 4;
                    cell63.BorderWidthLeft = 0f;
                    cell63.BorderWidthRight = 0f;
                    cell63.BorderWidthTop = 0f;
                    cell63.BorderWidthBottom = 0f;
                    cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell63);

                    PdfPCell cell6311 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell6311.Colspan = 4;
                    cell6311.BorderWidthLeft = 0f;
                    cell6311.BorderWidthRight = 0f;
                    cell6311.BorderWidthTop = 0f;
                    cell6311.BorderWidthBottom = 0f;
                    cell6311.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell6311);

                    //PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell62.Colspan = 4;
                    //cell62.BorderWidthLeft = 0f;
                    //cell62.BorderWidthRight = 0f;
                    //cell62.BorderWidthTop = 0f;
                    //cell62.BorderWidthBottom = 0f;
                    //cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right                    
                    //table5.AddCell(cell62);


                    if (dataSetFillHSRPDeliveryChallan.Rows[0]["vehicleregno"].ToString() != "")
                    {


                        PdfPCell cell163 = new PdfPCell(new Phrase("--Affixation--", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell163.Colspan = 4;
                        cell163.BorderWidthLeft = 0f;
                        cell163.BorderWidthRight = 0f;
                        cell163.BorderWidthTop = 0f;
                        cell163.BorderWidthBottom = 0f;
                        cell163.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell163);

                        // String strquery1 = "select [dbo].GetAffxDate('" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dataSetFillHSRPDeliveryChallan.Rows[0]["hsrp_stateId"].ToString() + "') as Date";

                        //Expected Date through Query
                        string strquery1 = "select convert(varchar(15),PlateAffixationDate,103) as Date from hsrprecords where HSRPRecord_AuthorizationNo='" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                        string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");
                        //DateTime date;

                        //if (DateTime.TryParse(date1, out date))
                        //{
                        //}
                        //else
                        //{
                        //    if (DateTime.Now.AddDays(2).DayOfWeek == DayOfWeek.Sunday)
                        //    {
                        //        date = DateTime.Now.AddDays(3);
                        //    }
                        //    else
                        //    {
                        //        date = System.DateTime.Now.AddDays(2);
                        //    }
                        //}
                        PdfPCell cell13 = new PdfPCell(new Phrase("Date : " + date1 + " dd/mm/yyyy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell13.Colspan = 4;
                        cell13.BorderWidthLeft = 0f;
                        cell13.BorderWidthRight = 0f;
                        cell13.BorderWidthTop = 0f;
                        cell13.BorderWidthBottom = 0f;
                        cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell13);
                        PdfPCell cell1213 = new PdfPCell(new Phrase("Time : 2:00 PM - 6:00 PM", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell1213.Colspan = 4;
                        cell1213.BorderWidthLeft = 0f;
                        cell1213.BorderWidthRight = 0f;
                        cell1213.BorderWidthTop = 0f;
                        cell1213.BorderWidthBottom = 0f;
                        cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell1213);
                        PdfPCell cell123 = new PdfPCell(new Phrase("Place : " + AffAddress, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        cell123.Colspan = 4;
                        cell123.BorderWidthLeft = 0f;
                        cell123.BorderWidthRight = 0f;
                        cell123.BorderWidthTop = 0f;
                        cell123.BorderWidthBottom = 0f;
                        cell123.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell123);
                    }


                    PdfPCell cellsp5 = new PdfPCell(new Phrase("Customer CARE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp5.Colspan = 4;
                    cellsp5.BorderWidthLeft = 0f;
                    cellsp5.BorderWidthRight = 0f;
                    cellsp5.BorderWidthTop = 0f;
                    cellsp5.BorderWidthBottom = 0f;
                    cellsp5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp5);

                    PdfPCell cellsp51 = new PdfPCell(new Phrase("https://hsrpts.com", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp51.Colspan = 4;
                    cellsp51.BorderWidthLeft = 0f;
                    cellsp51.BorderWidthRight = 0f;
                    cellsp51.BorderWidthTop = 0f;
                    cellsp51.BorderWidthBottom = 0f;
                    cellsp51.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp51);

                    PdfPCell cellsp52 = new PdfPCell(new Phrase("Email:online@hsrpts.com", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp52.Colspan = 4;
                    cellsp52.BorderWidthLeft = 0f;
                    cellsp52.BorderWidthRight = 0f;
                    cellsp52.BorderWidthTop = 0f;
                    cellsp52.BorderWidthBottom = 0f;
                    cellsp52.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp52);
                    PdfPCell cellsp53 = new PdfPCell(new Phrase("Contact Info:", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp53.Colspan = 4;
                    cellsp53.BorderWidthLeft = 0f;
                    cellsp53.BorderWidthRight = 0f;
                    cellsp53.BorderWidthTop = 0f;
                    cellsp53.BorderWidthBottom = 0f;
                    cellsp53.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp53);
                    PdfPCell cellsp54 = new PdfPCell(new Phrase("9396844878", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp54.Colspan = 4;
                    cellsp54.BorderWidthLeft = 0f;
                    cellsp54.BorderWidthRight = 0f;
                    cellsp54.BorderWidthTop = 0f;
                    cellsp54.BorderWidthBottom = 0f;
                    cellsp54.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp54);

                    PdfPCell cellsp55 = new PdfPCell(new Phrase("9396884878", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp55.Colspan = 4;
                    cellsp55.BorderWidthLeft = 0f;
                    cellsp55.BorderWidthRight = 0f;
                    cellsp55.BorderWidthTop = 0f;
                    cellsp55.BorderWidthBottom = 0f;
                    cellsp55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp55);

                    PdfPCell cellsp56 = new PdfPCell(new Phrase("9100026696", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp56.Colspan = 4;
                    cellsp56.BorderWidthLeft = 0f;
                    cellsp56.BorderWidthRight = 0f;
                    cellsp56.BorderWidthTop = 0f;
                    cellsp56.BorderWidthBottom = 0f;
                    cellsp56.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp56);

                    PdfPCell cellsp57 = new PdfPCell(new Phrase("9100026691", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cellsp57.Colspan = 4;
                    cellsp57.BorderWidthLeft = 0f;
                    cellsp57.BorderWidthRight = 0f;
                    cellsp57.BorderWidthTop = 0f;
                    cellsp57.BorderWidthBottom = 0f;
                    cellsp57.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table3.AddCell(cellsp57);

                    //PdfPCell cellsp5a = new PdfPCell(new Phrase("Office Copy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cellsp5a.Colspan = 4;
                    //cellsp5a.BorderWidthLeft = 0f;
                    //cellsp5a.BorderWidthRight = 0f;
                    //cellsp5a.BorderWidthTop = 0f;
                    //cellsp5a.BorderWidthBottom = 0f;
                    //cellsp5a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table4.AddCell(cellsp5a);               

                    //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                    //PdfPCell cell2195 = new PdfPCell(new Phrase("Note: Only first time Original Receipt could be generated. No option to give for generation of receipt after this.", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    //cell2195.Colspan = 4;
                    //cell2195.BorderWidthLeft = 0f;
                    //cell2195.BorderWidthRight = 0f;
                    //cell2195.BorderWidthTop = 0f;
                    //cell2195.BorderWidthBottom = 0f;
                    //cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table5.AddCell(cell2195);

                    document.Add(table1);
                    document.Add(table);
                    document.Add(table3);
                    document.Add(table5);


                    //document.Add(table2);
                    //document.Add(table);
                    //document.Add(table4);
                    //document.Add(table5);



                    document.Close();
                    HttpContext context = HttpContext.Current;
                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();
                }
            }
            else
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Record Not Found";
            }

        }
        DataTable ds = new DataTable();
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            cmd = new SqlCommand("OnlineCashReceiptDownLoad", con);//Business_ReportTypewisesummary_report
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@reporttype", Convert.ToString(Ddl1.SelectedValue)));
            cmd.Parameters.Add(new SqlParameter("@rep_filter", Convert.ToString(TxtSearchtype.Text.Trim())));
            cmd.Parameters.Add(new SqlParameter("@userid", USERID));
            con.Open();
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            ds.Load(dr);
            con.Close();
            if (ds.Rows.Count > 0)
            {
                DownloadReceipt(ds);
            }
            else
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Records Not Available .";
            }
        }

        private void FillDropDown()
        {
            SqlConnection con = new SqlConnection(ConnectionString);

            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            //change sp Name
            cmd = new SqlCommand("OnlineCashReceiptDownLoad", con);//Business_ReportTypewisesummary_report

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@reporttype", "db"));
            cmd.Parameters.Add(new SqlParameter("@rep_filter", "A"));
            cmd.Parameters.Add(new SqlParameter("@userid", USERID));


            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            Ddl1.DataSource = ds.Tables[0];
            Ddl1.DataTextField = ds.Tables[0].Columns[0].ToString();
            Ddl1.DataValueField = ds.Tables[0].Columns[1].ToString();
            Ddl1.DataBind();
            Ddl1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));


        }
        protected void Ddl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Ddl1.Items.ToString() == "--Select--")
            {
                lblsearch.Visible = false;
                lblsearch.Visible = false;
                TxtSearchtype.Visible = false;
                BtnSearch.Visible = false;
                lblErrMess.Text = "";
            }
            else
            {
                lblsearch.Text = "Enter " + Ddl1.SelectedItem.ToString();
                TxtSearchtype.Text = "";
                lblsearch.Visible = true;
                TxtSearchtype.Visible = true;
                BtnSearch.Visible = true;
                lblErrMess.Text = "";
            }
        }
    }
}