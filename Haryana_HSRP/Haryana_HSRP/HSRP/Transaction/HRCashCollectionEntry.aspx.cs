﻿using System;
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

namespace HSRP.Transaction
{
    public partial class HRCashCollectionEntry : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        SqlConnection con = new  SqlConnection(ConnectionString);
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string ProductivityID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string dealerid = string.Empty;
        string macbase = string.Empty;
        string sql = string.Empty;
        string sql1 = string.Empty;
        //string DepositAmount = "0";
        string Dealeardetail = string.Empty;
        string totcoll = "0";
        //int intDepositAmount = 0;
        decimal inttotcoll = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblSucMess.Visible = false;
                lblErrMess.Visible = false;
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    UserType = Session["UserType"].ToString();
                   

                }
                InitialSetting();               

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
            dealerid = Session["DealerID"].ToString();

        
           
            if (!IsPostBack)
            {   
           
             
            }
           

        }



        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            CalendarDepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDepositDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            DepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            DepositDate.MaxDate = DateTime.Parse(MaxDate);
        }
        public void refresh()
        {
           
            txtEngineNo.Text = "";
            txtChassisno.Text = "";
            txtOwnerName.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            txtMobileNo.Text = "";
            ddlVehicletype.SelectedItem.Text = "";
            ddlVehicleclass.SelectedItem.Text = "";

            lblAmount.Text = "";
        }

        protected void btnDownloadReceipt_Click(object sender, EventArgs e)
        {

            DataTable GetAddress;
            string Address;
            string TIN;

            DataTable rtoaddr = new DataTable();
            rtoaddr = Utils.GetDataTable("select r.RTOLocationAddress from users as u inner join rtolocation as r on u.RTOLocationID=r.RTOLocationID where userid='" + Session["UID"].ToString() + "'", ConnectionString);

            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", ConnectionString);

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];

            }
            else
            {
                Address = "";
            }
            //string getTinNo = GetAddress.Rows[0]["pincode"].ToString();
            string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

            string SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo , Engineno  from hsrprecords where  Engineno ='" + txtEngineNo.Text + "'";
            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);


            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {


                string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


                String StringField = String.Empty;
                String StringAlert = String.Empty;


                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
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

                PdfPCell cell312a = new PdfPCell(new Phrase("Duplicate", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell312a.Colspan = 4;
                cell312a.BorderColor = BaseColor.WHITE;
                cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell312a);

               /* PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 4;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);*/

                //PdfPCell cell1203 = new PdfPCell(new Phrase("" + AffAddress, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1203.Colspan = 4;
                //cell1203.BorderWidthLeft = 0f;
                //cell1203.BorderWidthRight = 0f;
                //cell1203.BorderWidthTop = 0f;
                //cell1203.BorderWidthBottom = 0f;
                //cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1203);

                //PdfPCell cell = new PdfPCell(new Phrase("WE HEREBY CONFIRM TO HAVE INSTALLED THE HSRP SET AS DETAILED BELOW : ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell.Colspan = 4;
                //cell.BorderColor = BaseColor.WHITE;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell);

                PdfPCell cell0 = new PdfPCell(new Phrase("HSRP CASH RECEIPT", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
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




                PdfPCell cellInv2 = new PdfPCell(new Phrase("CASH RECEIPT No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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




                //string getExciseNo1 = Utils.getScalarValue("select ExciseNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
                PdfPCell cell221 = new PdfPCell(new Phrase("Engine NO ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell221.Colspan = 1;
                cell221.BorderWidthLeft = 0f;
                cell221.BorderWidthRight = 0f;
                cell221.BorderWidthTop = 0f;
                cell221.BorderWidthBottom = 0f;
                cell221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell221);

                PdfPCell cell2221 = new PdfPCell(new Phrase(": " + txtEngineNo.Text, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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


                //if (txtEngineNo.Text != "")
                //{
                // PdfPCell cell9 = new PdfPCell(new Phrase("Engine No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
                //}

                //else
                //{

                //    PdfPCell cell9 = new PdfPCell(new Phrase("Engine No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //    cell9.Colspan = 1;
                //    cell9.BorderWidthLeft = 0f;
                //    cell9.BorderWidthRight = 0f;
                //    cell9.BorderWidthTop = 0f;
                //    cell9.BorderWidthBottom = 0f;
                //    cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell9);


                //    PdfPCell cell95 = new PdfPCell(new Phrase(": " , new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //    cell95.Colspan = 1;
                //    cell95.BorderWidthLeft = 0f;
                //    cell95.BorderWidthRight = 0f;
                //    cell95.BorderWidthTop = 0f;
                //    cell95.BorderWidthBottom = 0f;
                //    cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //    table.AddCell(cell95);
                
                
                
                //}
               
               
               

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

                PdfPCell celldupCash402 = new PdfPCell(new Phrase(" " + roundAmt, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupCash402.Colspan = 1;
                celldupCash402.BorderWidthLeft = 0f;
                celldupCash402.BorderWidthRight = 0f;
                celldupCash402.BorderWidthTop = 0f;
                celldupCash402.BorderWidthBottom = 0f;
                celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash402);

                //-----------------------------


                //-----------------------------


                string Message = "\u2022" + " Please verify the details on Cash Receipt.";

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


                /* string MessageSec = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day, a confirmation SMS will be sent to the registered mobile number provided by the customer.";

                 PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                 cell65a.Colspan = 4;
                 cell65a.BorderWidthLeft = 0f;
                 cell65a.BorderWidthRight = 0f;
                 cell65a.BorderWidthTop = 0f;
                 cell65a.BorderWidthBottom = 0f;
                 cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                 table.AddCell(cell65a);

                 PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                 cell63.Colspan = 4;
                 cell63.BorderWidthLeft = 0f;
                 cell63.BorderWidthRight = 0f;
                 cell63.BorderWidthTop = 0f;
                 cell63.BorderWidthBottom = 0f;
                 cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                 table.AddCell(cell63);*/

                //PdfPCell cell163 = new PdfPCell(new Phrase("--Affixation--", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell163.Colspan = 4;
                //cell163.BorderWidthLeft = 0f;
                //cell163.BorderWidthRight = 0f;
                //cell163.BorderWidthTop = 0f;
                //cell163.BorderWidthBottom = 0f;
                //cell163.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell163);

                // String strquery1 = "select [dbo].GetAffxDate('" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dataSetFillHSRPDeliveryChallan.Rows[0]["hsrp_stateId"].ToString() + "') as Date";

                //Expected Date through Query
                //string strquery1 = "select convert(varchar(15),PlateAffixationDate,103) as Date from hsrprecords where HSRPRecord_AuthorizationNo='" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                //string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");
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
                //PdfPCell cell13 = new PdfPCell(new Phrase("Date : " + date1 + " dd/mm/yyyy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell13.Colspan = 4;
                //cell13.BorderWidthLeft = 0f;
                //cell13.BorderWidthRight = 0f;
                //cell13.BorderWidthTop = 0f;
                //cell13.BorderWidthBottom = 0f;
                //cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell13);
                //PdfPCell cell1213 = new PdfPCell(new Phrase("Time : 2:00 PM - 6:00 PM", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1213.Colspan = 4;
                //cell1213.BorderWidthLeft = 0f;
                //cell1213.BorderWidthRight = 0f;
                //cell1213.BorderWidthTop = 0f;
                //cell1213.BorderWidthBottom = 0f;
                //cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1213);
                //PdfPCell cell123 = new PdfPCell(new Phrase("Place : " + AffAddress, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell123.Colspan = 4;
                //cell123.BorderWidthLeft = 0f;
                //cell123.BorderWidthRight = 0f;
                //cell123.BorderWidthTop = 0f;
                //cell123.BorderWidthBottom = 0f;
                //cell123.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell123);                


                PdfPCell cellsp5 = new PdfPCell(new Phrase("Customer Copy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp5.Colspan = 4;
                cellsp5.BorderWidthLeft = 0f;
                cellsp5.BorderWidthRight = 0f;
                cellsp5.BorderWidthTop = 0f;
                cellsp5.BorderWidthBottom = 0f;
                cellsp5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table3.AddCell(cellsp5);

                PdfPCell cellsp5a = new PdfPCell(new Phrase("Office Copy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp5a.Colspan = 4;
                cellsp5a.BorderWidthLeft = 0f;
                cellsp5a.BorderWidthRight = 0f;
                cellsp5a.BorderWidthTop = 0f;
                cellsp5a.BorderWidthBottom = 0f;
                cellsp5a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table4.AddCell(cellsp5a);

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 4;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table5.AddCell(cell62);

                //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                PdfPCell cell2195 = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2195.Colspan = 4;
                cell2195.BorderWidthLeft = 0f;
                cell2195.BorderWidthRight = 0f;
                cell2195.BorderWidthTop = 0f;
                cell2195.BorderWidthBottom = 0f;
                cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table5.AddCell(cell2195);

                document.Add(table1);
                document.Add(table);
                document.Add(table3);
                document.Add(table5);


                document.Add(table2);
                document.Add(table);
                document.Add(table4);
                document.Add(table5);



                document.Close();
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }

        }

        public void Cleardatasave()
        {
            txtAddress.Text = "";
            txtChassisno.Text = "";
            txtEmail.Text = "";
           // txtEngineNo.Text = "";
            txtexprice.Text = "";
            txtMobileNo.Text = "";
            txtmodel.Text = "";
            txtOwnerName.Text = "";
           // txtrecno.Text = "";
            txtRegNumber.Text = "";
            txtEmail.ReadOnly = false;
            txtOwnerName.ReadOnly = false;
            txtAddress.ReadOnly = false;
            txtmodel.ReadOnly = false;
            txtRegNumber.ReadOnly = false; ;
            txtEngineNo.ReadOnly = false;
            txtChassisno.ReadOnly = false;
            ddlVehicletype.Enabled = true;
            ddlVehicleclass.Enabled = true;
            ddlVehicletype.ClearSelection();
            ddlVehicleclass.ClearSelection();
        }
        string Query = string.Empty;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRegNumber.Text.Trim()!="")
                {
                    string strVehicleNo = txtRegNumber.Text.Trim();
                    if (strVehicleNo.Length < 4)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                        return;
                    }


                    string strCheck = strVehicleNo.Substring(0, 2);
                    if (strCheck.ToUpper().Trim() != "HR")
                    {
                        if (strCheck.ToUpper().Trim() != "HY")
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                            return;
                        }
                    }

                    if (strVehicleNo.Length>10)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                        return;
                    }

                   
                                       
                }
               

                //
               
                if (string.IsNullOrEmpty(txtEngineNo.Text.Trim()))
                {
                    lblErrMess.Visible = true;
                     lblErrMess.Text = "Please Enter Valid  Engine No.";
                     return;
                }              
                           
                 Query = "select Engineno from hsrprecords where hsrp_stateid='" + HSRPStateID + "' and Engineno ='"+txtEngineNo.Text.Trim()+"'";
               
                DataTable dtResult = Utils.GetDataTable(Query, ConnectionString);
                if (dtResult.Rows.Count > 0)
                {
                    lblSucMess.Visible = false;
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "EngineNo. Already Exist";
                    return;
                }

                if( string.IsNullOrEmpty(txtexprice.Text.ToString()))
                {
                    lblSucMess.Visible = false;
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Ex Showroom Price";
                    return;

                }

               
              
                string fixcharge = string.Empty;
              

                string SQLfixcharge = " select Dealerid from dealermaster where dealerid='" + dealerid.ToString() + "'";
                DataTable dt11 = Utils.GetDataTable(SQLfixcharge, ConnectionString);
                if (dt11.Rows.Count > 0)
                {
                    string excharge = "exec  Business_Rule_DealerHR " + Convert.ToInt32(dealerid.ToString()) + " , '" + ddlVehicletype.SelectedItem.Text.Trim() + "','" + ddlVehicleclass.SelectedItem.Text.ToUpper().Trim() + "'," + txtexprice.Text.ToString() + "";
                    DataTable dtexcharge = Utils.GetDataTable(excharge, ConnectionString);
                    if (dtexcharge.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dtexcharge.Rows[0]["extracharges"].ToString()))
                        {
                            lblErrMess.Text ="Please Contact To administrator.";
                            return;
                        }
                        else
                        {
                            fixcharge = dtexcharge.Rows[0]["extracharges"].ToString();
                        }
                    }
                    else
                    {
                        if (ddlVehicleclass.SelectedItem.Text.ToUpper().Trim() == "NON-TRANSPORT" && ddlVehicletype.SelectedItem.Text.Trim().ToUpper() == "SCOOTER" || ddlVehicletype.SelectedItem.Text.Trim().ToUpper() == "MOTOR CYCLE")
                        {
                            fixcharge = "115.00";
                        }

                        if (ddlVehicleclass.SelectedItem.Text.Trim() == "NON-TRANSPORT" && ddlVehicletype.SelectedItem.Text.Trim() == "L.M.V. (CAR)" || ddlVehicletype.SelectedItem.Text.Trim().ToUpper() == "LMV" || ddlVehicletype.SelectedItem.Text.Trim().ToUpper() == "LMV(CLASS)" || ddlVehicletype.SelectedItem.Text.Trim() == "MCV/HCV/TRAILERS" || ddlVehicletype.SelectedItem.Text.Trim() == "TRACTOR")
                        {
                            fixcharge = "287.50";
                        }
                        if (ddlVehicleclass.SelectedItem.Text.ToUpper().Trim() == "TRANSPORT")
                        {
                            fixcharge = "345.00";
                        }


                    }

                }

                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "You Are Not Dealer.";
                    return;
                }

                if (string.IsNullOrEmpty(fixcharge.ToString().Trim()))
                {
                    lblErrMess.Text = "Please Contact To administrator.";
                    return;
                }



                 string sticker1 = Sticker;

                string sql = "exec [getPlatesData] '" + HSRPStateID + "','" + ddlVehicletype.SelectedItem.ToString() + "','" + ddlVehicleclass.SelectedItem.ToString() + "','NB'";
                DataTable dt = Utils.GetDataTable(sql, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    lblSucMess.Visible = false;
                }



                DataTable dt5 = new DataTable();
                string cashrc = string.Empty;
                string authdate = string.Empty;

                

                string Invoice = string.Empty;
                string DC = string.Empty;
                cashrc = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                cashrc = Utils.getScalarValue(cashrc, ConnectionString);
               
                Invoice = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                Invoice = Utils.getScalarValue(Invoice, ConnectionString);
                DC = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                DC = Utils.getScalarValue(DC, ConnectionString);
                txtAddress.Text = txtAddress.Text.Replace("'", "");


                String strquery1 = "select [dbo].GetAffxDate_insert_new('" + HSRPStateID + "') as Date";
                string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");

                if (!string.IsNullOrEmpty(Convert.ToString(lblAmount.Text)))
                {
                    decimal dd = decimal.Parse(lblAmount.Text);
                }
               
                else
                {
                  //  lblAmount.Text = "0";
                    lblSucMess.Visible = false;
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Contact With Administrator.";
                    return;
                   
                }
                                
               // decimal vehamount = decimal.Parse(lblAmount.Text.ToString());
                decimal vehamount = decimal.Parse(lblAmount.Text.ToString()) + decimal.Parse(fixcharge);                            
               
               
                string sqlquery = "select isnull(sum(DepositAmount),0) as DepositAmount from [BankTransaction] where approvedstatus='Y' and userid='" + USERID + "'";               
                string DepositAmount=  Utils.getScalarValue(sqlquery, ConnectionString);
                if (DepositAmount.ToString().Trim() == "")
                {
                    DepositAmount = "0";
                }
               decimal  intDepositAmount = decimal.Parse(DepositAmount);

                string sqlq = "select isnull(sum(roundoff_netamount),0) as amount from HSRPRecords where createdby='" + USERID + "' and HSRP_StateID=4 ";

                string collamt = Utils.getScalarValue(sqlq, ConnectionString);

                string sqlfixcharge = "select isnull(sum(fixingcharge),0) as fixingcharge from HSRPRecords where createdby='" + USERID + "' and HSRP_StateID=4 ";

                string fixamt = Utils.getScalarValue(sqlfixcharge, ConnectionString);
               // inttotcoll =  Convert.ToInt32(Math.Round(decimal.Parse(collamt), 0)) +  Convert.ToInt32(Math.Round(decimal.Parse(fixamt), 0));

                inttotcoll = decimal.Parse(collamt)+ decimal.Parse(fixamt);

                 decimal intavailableAmount = intDepositAmount - inttotcoll;

                if (intavailableAmount < vehamount)
                  {
                    lblSucMess.Visible = false;
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Contact With Administrator.";
                    return;
                 }
               string StickerMandatory = string.Empty;
               if (ddlVehicletype.SelectedItem.Text.Trim() == "L.M.V. (CAR)" || ddlVehicletype.SelectedItem.Text.Trim() == "LMV" || ddlVehicletype.SelectedItem.Text.Trim() == "LMV(CLASS)" || ddlVehicletype.SelectedItem.Text.Trim() == "MCV/HCV/TRAILERS" || ddlVehicletype.SelectedItem.Text.Trim() == "THREE WHEELER")
               {
                   StickerMandatory = "Y";
               }
               else
               {
                   StickerMandatory = "N";
               }
                              
               string strQuery = "insert into HSRPRecords (hsrprecord_authorizationno,hsrprecord_authorizationdate,SaveMacAddress,DeliveryChallanNo,ISFrontPlateSize,ISRearPlateSize,invoiceno, address1," +
                       " HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,OwnerName,MobileNo," +
                       "VehicleClass,OrderType,NetAmount,VehicleType,OrderStatus,CashReceiptNo,EmailID,ChassisNo, EngineNo," +
                       "frontplatesize, rearplatesize,CreatedBy,vehicleref,FrontplatePrize,RearPlatePrize,StickerPrize,ScrewPrize,TotalAmount," +
                       "VAT_Amount,RoundOff_NetAmount,VAT_Percentage,PlateAffixationDate,DateOfInsurance,ReceiptNo,Exshowroomprice,vehicleregno,dealerid,addrecordby,fixingcharge, StickerMandatory ) " +
                       "values('0',GetDate(),'" + macbase + "','" + DC + "', '" + dt.Rows[0]["frontplateflag"].ToString() + "','" + dt.Rows[0]["rearplateflag"].ToString() + "', '" + Invoice +
                       "','" + txtAddress.Text + "',GetDate(),'" + HSRPStateID + "','" + RTOLocationID + "','" + txtOwnerName.Text + "','" + txtMobileNo.Text + "','" + ddlVehicleclass.SelectedItem.ToString().ToUpper() +
                       "','NB','" + lblAmount.Text + "','" + ddlVehicletype.SelectedItem.ToString() + "','New Order','" + cashrc +
                       "', '" + txtEmail.Text.Trim() + "', '" + txtChassisno.Text.Trim() + "', '" + txtEngineNo.Text.Trim() + "', '" +
                       dt.Rows[0]["frontplateID"].ToString() + "', '" + dt.Rows[0]["RearPlateID"].ToString() + "','" + USERID +
                       "','New','" + dt.Rows[0]["FrontPlateCost"].ToString() + "','" + dt.Rows[0]["rearplatecost"].ToString() + "','" + dt.Rows[0]["stickercost"].ToString() +
                       "','" + dt.Rows[0]["snaplockcost"].ToString() + "','" + dt.Rows[0]["totalamount"].ToString() + "','" + dt.Rows[0]["vatamount"].ToString() + "','" +
                       Math.Round(decimal.Parse(lblAmount.Text), 0) + "','" + dt.Rows[0]["vatper"].ToString() + "','" + date1 + "','" + DepositDate.SelectedDate + "','','" + txtexprice.Text + "','" + txtRegNumber.Text + "','" + dealerid + "','Dealer', " + fixcharge + " ,'" + StickerMandatory + "')";
                int i = Utils.ExecNonQuery(strQuery, ConnectionString);
                if (i > 0)
                {
                    lblSucMess.Visible = true;
                    lblErrMess.Visible = false;
                    btnDownload.Visible = false;
                   
                    lblSucMess.Text = "Record Saved Successfully";
                    Button3.Visible = true;
                    string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                    Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                    int u = Utils.ExecNonQuery(Query, ConnectionString);
                    Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                    u = Utils.ExecNonQuery(Query, ConnectionString);
                    Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                    u = Utils.ExecNonQuery(Query, ConnectionString);                 

                    lblSucMess.Text = "Record Saved Successfully..";

                    if (txtMobileNo.Text.Length > 0)
                    {

                     
                        string SMSText = "Received Rs. " + Math.Round(decimal.Parse(lblAmount.Text), 0) + " against HSRP For Engine No. " + txtEngineNo.Text.Trim().ToUpper() + " RECEIPT No. " + cashrc + " dated  " + System.DateTime.Now.ToString("dd/MM/yyyy") + "-HSRP Team ";
                        string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-hsrphr&password=hsrphr&type=0&dlr=1&destination=" + txtMobileNo.Text.ToString() + "&source=HRHSRP&message=" + SMSText;

                        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                        myRequest.Method = "GET";
                        WebResponse myResponse = myRequest.GetResponse();
                        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                        string result = sr.ReadToEnd();

                        var array = result.Split();

                        sr.Close();
                        myResponse.Close();
                        System.Threading.Thread.Sleep(350);

                        Utils.ExecNonQuery("insert into HRSMSDetail(RtoLocationID,VehicleRegNo,MobileNo,SentResponseCode,smstext) values('" + RTOLocationID + "','" + txtRegNumber.Text.Trim().ToUpper() + "'," + txtMobileNo.Text.ToString() + ",'" + result + "','" + SMSText + "')", ConnectionString);
                    }
                  
                    Cleardatasave();

                    btnDownload.Visible = true;
                    sql = "select isnull(sum(roundoff_netamount),0) as amount from HSRPRecords where createdby='" + USERID + "'";

                    totcoll = Utils.getScalarValue(sql, ConnectionString);
                    inttotcoll = int.Parse(totcoll);
                }
            }
            catch (Exception ex)
            {
                lblSucMess.Text = "Message : " + ex;
                return;
            }

            
            

        }
        protected void Button3_Click(object sender, EventArgs e)
        {

            EpsionPrint();
            Cleardatasave();
        }

        

        public void EpsionPrint()
        {
            String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            DataTable GetAddress;
            string Address;
            string AffAddress = string.Empty;
            string sqlquery = string.Empty;
            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + Session["UserHSRPStateID"].ToString() + "'", ConnectionString);

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }

            string SQLString = "select hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType , EngineNo from hsrprecords where  EngineNo ='" + txtEngineNo.Text + "'";
            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);

            DataProvider.BAL obj = new DataProvider.BAL();
            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {

                string strquery1 = "select convert(varchar(15),PlateAffixationDate,103) as Date from hsrprecords where HSRPRecord_AuthorizationNo='" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");

              
                string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


                String StringField = String.Empty;
                String StringAlert = String.Empty;

                Document document = new Document();
                float imageWidth = 216;
                float imageHeight = 360;
                document.SetMargins(0, 0, 5, 0);
                document.SetPageSize(new iTextSharp.text.Rectangle(imageWidth, imageHeight));

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:

                //string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                string path = ConfigurationManager.AppSettings["PdfFolder"].ToString();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath(PdfFolder), FileMode.Create));
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(2);
                PdfPTable table1 = new PdfPTable(2);
                //PdfPTable table2 = new PdfPTable(2);
                //actual width of table in points
                //table.TotalWidth = 100f;

                //fix the absolute width of the table

                PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell312.Colspan = 2;
                cell312.BorderColor = BaseColor.WHITE;
                cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell312);

                //PdfPCell cell312a = new PdfPCell(new Phrase("Duplicate", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell312a.Colspan = 2;
                //cell312a.BorderColor = BaseColor.WHITE;
                //cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table2.AddCell(cell312a);

                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 2;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1203 = new PdfPCell(new Phrase(AffAddress, new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1203.Colspan = 2;
                cell1203.BorderWidthLeft = 0f;
                cell1203.BorderWidthRight = 0f;
                cell1203.BorderWidthTop = 0f;
                cell1203.BorderWidthBottom = 0f;
                cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1203);



                PdfPCell cell0 = new PdfPCell(new Phrase("HSRP CASH RECEIPT", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                cell0.Colspan = 2;
                cell0.BorderWidthLeft = 0f;
                cell0.BorderWidthRight = 0f;
                cell0.BorderWidthTop = 0f;
                cell0.BorderWidthBottom = 0f;

                cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell0);


                PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1.Colspan = 2;
                cell1.BorderWidthLeft = 0f;
                cell1.BorderWidthRight = 0f;
                cell1.BorderWidthTop = 0f;
                cell1.BorderWidthBottom = 0f;

                cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1);




                PdfPCell cellInv2 = new PdfPCell(new Phrase("CASH RECEIPT No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellInv2.Colspan = 0;

                cellInv2.BorderWidthLeft = 0f;
                cellInv2.BorderWidthRight = 0f;
                cellInv2.BorderWidthTop = 0f;
                cellInv2.BorderWidthBottom = 0f;
                cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellInv2);



                PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellInv22111.Colspan = 0;
                cellInv22111.BorderWidthLeft = 0f;
                cellInv22111.BorderWidthRight = 0f;
                cellInv22111.BorderWidthTop = 0f;
                cellInv22111.BorderWidthBottom = 0f;
                cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellInv22111);







                PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell21.Colspan = 0;

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
                cell212.Colspan = 0;

                cell212.BorderWidthLeft = 0f;
                cell212.BorderWidthRight = 0f;
                cell212.BorderWidthTop = 0f;
                cell212.BorderWidthBottom = 0f;
                cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell212);



                PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2.Colspan = 0;

                cell2.BorderWidthLeft = 0f;
                cell2.BorderWidthRight = 0f;
                cell2.BorderWidthTop = 0f;
                cell2.BorderWidthBottom = 0f;
                cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2);

                string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

                PdfPCell cell22111 = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22111.Colspan = 0;
                cell22111.BorderWidthLeft = 0f;
                cell22111.BorderWidthRight = 0f;
                cell22111.BorderWidthTop = 0f;
                cell22111.BorderWidthBottom = 0f;
                cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22111);

                PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22.Colspan = 0;

                cell22.BorderWidthLeft = 0f;
                cell22.BorderWidthRight = 0f;
                cell22.BorderWidthTop = 0f;
                cell22.BorderWidthBottom = 0f;
                cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22);

                PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm tt"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell222.Colspan = 0;

                cell222.BorderWidthLeft = 0f;
                cell222.BorderWidthRight = 0f;
                cell222.BorderWidthTop = 0f;
                cell222.BorderWidthBottom = 0f;
                cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell222);

                //string getExciseNo = Utils.getScalarValue("select ExciseNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
                //PdfPCell cell221 = new PdfPCell(new Phrase("EXCISE NO. ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell221.Colspan = 0;
                //cell221.BorderWidthLeft = 0f;
                //cell221.BorderWidthRight = 0f;
                //cell221.BorderWidthTop = 0f;
                //cell221.BorderWidthBottom = 0f;
                //cell221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell221);

                //PdfPCell cell2221 = new PdfPCell(new Phrase(": " + getExciseNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell2221.Colspan = 0;

                //cell2221.BorderWidthLeft = 0f;
                //cell2221.BorderWidthRight = 0f;
                //cell2221.BorderWidthTop = 0f;
                //cell2221.BorderWidthBottom = 0f;
                //cell2221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell2221);



                PdfPCell cell5 = new PdfPCell(new Phrase("AUTH NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell5.Colspan = 0;

                cell5.BorderWidthLeft = 0f;
                cell5.BorderWidthRight = 0f;
                cell5.BorderWidthTop = 0f;
                cell5.BorderWidthBottom = 0f;
                cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell5);

                PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell55.Colspan = 0;

                cell55.BorderWidthLeft = 0f;
                cell55.BorderWidthRight = 0f;
                cell55.BorderWidthTop = 0f;
                cell55.BorderWidthBottom = 0f;
                cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell55);

                PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell25.Colspan = 0;

                cell25.BorderWidthLeft = 0f;
                cell25.BorderWidthRight = 0f;
                cell25.BorderWidthTop = 0f;
                cell25.BorderWidthBottom = 0f;
                cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell25);

                //DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                string auths = string.Empty;
                auths = dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString();
                if (auths == "")
                {
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + auths, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 0;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);

                }
                else
                {
                    DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 0;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);

                }
                PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell7.Colspan = 0;
                cell7.BorderWidthLeft = 0f;
                cell7.BorderWidthRight = 0f;
                cell7.BorderWidthTop = 0f;
                cell7.BorderWidthBottom = 0f;
                cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell7);

                PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell75.Colspan = 0;
                cell75.BorderWidthLeft = 0f;
                cell75.BorderWidthRight = 0f;
                cell75.BorderWidthTop = 0f;
                cell75.BorderWidthBottom = 0f;
                cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell75);

                PdfPCell cell29 = new PdfPCell(new Phrase("CONTACT NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell29.Colspan = 0;
                cell29.BorderWidthLeft = 0f;
                cell29.BorderWidthRight = 0f;
                cell29.BorderWidthTop = 0f;
                cell29.BorderWidthBottom = 0f;
                cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell29);

                PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell295.Colspan = 0;
                cell295.BorderWidthLeft = 0f;
                cell295.BorderWidthRight = 0f;
                cell295.BorderWidthTop = 0f;
                cell295.BorderWidthBottom = 0f;
                cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell295);



                PdfPCell cell9 = new PdfPCell(new Phrase("ENGINE NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell9.Colspan = 0;
                cell9.BorderWidthLeft = 0f;
                cell9.BorderWidthRight = 0f;
                cell9.BorderWidthTop = 0f;
                cell9.BorderWidthBottom = 0f;
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell9);

                PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell95.Colspan = 0;
                cell95.BorderWidthLeft = 0f;
                cell95.BorderWidthRight = 0f;
                cell95.BorderWidthTop = 0f;
                cell95.BorderWidthBottom = 0f;
                cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell95);

                PdfPCell cell999 = new PdfPCell(new Phrase("VehicleReg NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell9.Colspan = 0;
                cell9.BorderWidthLeft = 0f;
                cell9.BorderWidthRight = 0f;
                cell9.BorderWidthTop = 0f;
                cell9.BorderWidthBottom = 0f;
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell999);

                PdfPCell cell950 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell95.Colspan = 0;
                cell95.BorderWidthLeft = 0f;
                cell95.BorderWidthRight = 0f;
                cell95.BorderWidthTop = 0f;
                cell95.BorderWidthBottom = 0f;
                cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell950);




                //PdfPCell cell99 = new PdfPCell(new Phrase(" Vehicle Reg NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell9.Colspan = 0;
                //cell9.BorderWidthLeft = 0f;
                //cell9.BorderWidthRight = 0f;
                //cell9.BorderWidthTop = 0f;
                //cell9.BorderWidthBottom = 0f;
                //cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell99);

                //PdfPCell cell959 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell95.Colspan = 0;
                //cell95.BorderWidthLeft = 0f;
                //cell95.BorderWidthRight = 0f;
                //cell95.BorderWidthTop = 0f;
                //cell95.BorderWidthBottom = 0f;
                //cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell959);



                PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell10.Colspan = 0;
                cell10.BorderWidthLeft = 0f;
                cell10.BorderWidthRight = 0f;
                cell10.BorderWidthTop = 0f;
                cell10.BorderWidthBottom = 0f;
                cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell10);

                PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell105.Colspan = 0;
                cell105.BorderWidthLeft = 0f;
                cell105.BorderWidthRight = 0f;
                cell105.BorderWidthTop = 0f;
                cell105.BorderWidthBottom = 0f;
                cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell105);

                PdfPCell cell11 = new PdfPCell(new Phrase(" VEHICLE CLASS ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11.Colspan = 0;
                cell11.BorderWidthLeft = 0f;
                cell11.BorderWidthRight = 0f;
                cell11.BorderWidthTop = 0f;
                cell11.BorderWidthBottom = 0f;
                cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11);

                PdfPCell cell115 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell115.Colspan = 0;
                cell115.BorderWidthLeft = 0f;
                cell115.BorderWidthRight = 0f;
                cell115.BorderWidthTop = 0f;
                cell115.BorderWidthBottom = 0f;
                cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell115);



                PdfPCell cellNet120 = new PdfPCell(new Phrase("NET AMOUNT (Rs.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellNet120.Colspan = 0;
                cellNet120.BorderWidthLeft = 0f;
                cellNet120.BorderWidthRight = 0f;
                cellNet120.BorderWidthTop = 0f;
                cellNet120.BorderWidthBottom = 0f;
                cellNet120.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellNet120);



                PdfPCell cell1205 = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1205.Colspan = 0;
                cell1205.BorderWidthLeft = 0f;
                cell1205.BorderWidthRight = 0f;
                cell1205.BorderWidthTop = 0f;
                cell1205.BorderWidthBottom = 0f;
                cell1205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1205);

                PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OF AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                celldupCash401.Colspan = 0;
                celldupCash401.BorderWidthLeft = 0f;
                celldupCash401.BorderWidthRight = 0f;
                celldupCash401.BorderWidthTop = 0f;
                celldupCash401.BorderWidthBottom = 0f;
                celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash401);


                decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                roundAmt = Math.Round(roundAmt, 0);

                PdfPCell celldupCash402 = new PdfPCell(new Phrase(" " + roundAmt, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                celldupCash402.Colspan = 0;
                celldupCash402.BorderWidthLeft = 0f;
                celldupCash402.BorderWidthRight = 0f;
                celldupCash402.BorderWidthTop = 0f;
                celldupCash402.BorderWidthBottom = 0f;
                celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash402);
                PdfPCell celldupCash40z1 = new PdfPCell(new Phrase("(Inclusive of All Tax)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                celldupCash40z1.Colspan = 4;
                celldupCash40z1.BorderWidthLeft = 0f;
                celldupCash40z1.BorderWidthRight = 0f;
                celldupCash40z1.BorderWidthTop = 0f;
                celldupCash40z1.BorderWidthBottom = 0f;
                celldupCash40z1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash40z1);




                PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("Disclaimer :", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash402.Colspan = 2;
                celldupRouCash402.BorderWidthLeft = 0f;
                celldupRouCash402.BorderWidthRight = 0f;
                celldupRouCash402.BorderWidthTop = 0f;
                celldupRouCash402.BorderWidthBottom = 0f;
                celldupRouCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash402);

                //string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day from the date of  issuance of cash receipt.";
                string Message = "\u2022" + " Vehicle Owner is requested to please check the Correctness of the cash slip.";

                PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell64.Colspan = 2;
                cell64.BorderWidthLeft = 0f;
                cell64.BorderWidthRight = 0f;
                cell64.BorderWidthTop = 0f;
                cell64.BorderWidthBottom = 0f;
                cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64);


                string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt. The company shall not be responsible for any clarrical mistake what so ever.";

                PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65a.Colspan = 4;
                cell65a.BorderWidthLeft = 0f;
                cell65a.BorderWidthRight = 0f;
                cell65a.BorderWidthTop = 0f;
                cell65a.BorderWidthBottom = 0f;
                cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65a);

                PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell63.Colspan = 2;
                cell63.BorderWidthLeft = 0f;
                cell63.BorderWidthRight = 0f;
                cell63.BorderWidthTop = 0f;
                cell63.BorderWidthBottom = 0f;
                cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell63);

                //PdfPCell cellsp4 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cellsp4.Colspan = 2;
                //cellsp4.BorderWidthLeft = 0f;
                //cellsp4.BorderWidthRight = 0f;
                //cellsp4.BorderWidthTop = 0f;
                //cellsp4.BorderWidthBottom = 0f;
                //cellsp4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cellsp4);

                //PdfPCell cellsp5 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cellsp5.Colspan = 2;
                //cellsp5.BorderWidthLeft = 0f;
                //cellsp5.BorderWidthRight = 0f;
                //cellsp5.BorderWidthTop = 0f;
                //cellsp5.BorderWidthBottom = 0f;
                //cellsp5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cellsp5);
                //PdfPCell cell163 = new PdfPCell(new Phrase("--Affixation--", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell163.Colspan = 4;
                //cell163.BorderWidthLeft = 0f;
                //cell163.BorderWidthRight = 0f;
                //cell163.BorderWidthTop = 0f;
                //cell163.BorderWidthBottom = 0f;
                //cell163.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell163);

                //PdfPCell cell13 = new PdfPCell(new Phrase("Date : " + date1 + " dd/mm/yyyy", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell13.Colspan = 4;
                //cell13.BorderWidthLeft = 0f;
                //cell13.BorderWidthRight = 0f;
                //cell13.BorderWidthTop = 0f;
                //cell13.BorderWidthBottom = 0f;
                //cell13.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell13);
                //PdfPCell cell1213 = new PdfPCell(new Phrase("Time : 2:00 PM - 6:00 PM", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1213.Colspan = 4;
                //cell1213.BorderWidthLeft = 0f;
                //cell1213.BorderWidthRight = 0f;
                //cell1213.BorderWidthTop = 0f;
                //cell1213.BorderWidthBottom = 0f;
                //cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1213);
                //PdfPCell cell123 = new PdfPCell(new Phrase("Place : " + AffAddress, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell123.Colspan = 4;
                //cell123.BorderWidthLeft = 0f;
                //cell123.BorderWidthRight = 0f;
                //cell123.BorderWidthTop = 0f;
                //cell123.BorderWidthBottom = 0f;
                //cell123.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell123);                

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 2;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell62);






                //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                //PdfPCell cell2195 = new PdfPCell(new Phrase("---------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell2195.Colspan = 2;
                //cell2195.BorderWidthLeft = 0f;
                //cell2195.BorderWidthRight = 0f;
                //cell2195.BorderWidthTop = 0f;
                //cell2195.BorderWidthBottom = 0f;
                //cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell2195);

                PdfPCell cellsp1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp1.Colspan = 2;
                cellsp1.BorderWidthLeft = 0f;
                cellsp1.BorderWidthRight = 0f;
                cellsp1.BorderWidthTop = 0f;
                cellsp1.BorderWidthBottom = 0f;
                cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp1);


                document.Add(table1);
                document.Add(table);

                //document.Add(table2);
                //document.Add(table);

                document.Close();
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
        }

        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePanel2.Update();
            ddlVehicleclass.ClearSelection();
            //if (ddlVehicletype.SelectedItem.ToString() == "-Select Vehicle Type-" || ddlVehicleclass.SelectedItem.ToString() == "-Select Vehicle Class-")
            //{
            //    return;
            //}
            //else
            //{
            //    GetAmount();
            //}
        }

        protected void ddlVehicleclass_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                GetAmount();

        }


        public void GetAmount()
        {
            string sql = "exec [getPlatesData] '" + HSRPStateID + "','" + ddlVehicletype.SelectedItem.ToString() + "','" + ddlVehicleclass.SelectedItem.ToString() + "','NB'";
            DataTable dt = Utils.GetDataTable(sql, ConnectionString);
            lblAmount.Text = Math.Round(Convert.ToDecimal(dt.Rows[0]["netamount"]), 2).ToString();

        }
        
        protected void ddlVehicletype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehicleclass.SelectedItem.ToString() == "-Select Vehicle Class-")
            {
                return;
            }
            else
            {
                GetAmount();
            }
        }

        protected void btngo_Click(object sender, EventArgs e)
        {

            lblErrMess.Text = string.Empty;
            string strAuthno = string.Empty;
            string StrRtoLocationCode = string.Empty;
            string StrRtoName = string.Empty;
            string StrAuthorizationNo = string.Empty;
            string StrOwnerName = string.Empty;
            string StrOwnerAddress = string.Empty;
            string StrVehicleType = string.Empty;
            string StrTransactonType = string.Empty;
            string StrAuthdate = string.Empty;
            string StrMobileNo = string.Empty;
            string StrVehicleClassType = string.Empty;
            string StrManufacturarName = string.Empty;
            string StrModelName = string.Empty;
            string StrRegistrationNo = string.Empty;
            string StrEngineNo = string.Empty;
            string StrChasisNo = string.Empty;
            string stremail = string.Empty;
            string SQLString = string.Empty;
            string RTOlocationName = string.Empty;
             // int inttotcoll = 0;
            Button3.Visible = false;
            btnDownload.Visible = false;

            SQLString = "select Rtolocationname from rtolocation where rtolocationid='" + RTOLocationID + "'";
            RTOlocationName = Utils.getScalarValue(SQLString, ConnectionString);

            try
            {
                if (string.IsNullOrEmpty(txtRegNumber.Text.Trim()))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Vehicle Registration No.";
                    return;
                }
                string strVehicleNo = txtRegNumber.Text.Trim();
                if (strVehicleNo.Length < 4)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                    return;
                }


                string strCheck = strVehicleNo.Substring(0, 2);
             
                if (strCheck.ToUpper() != "HR")
                {
                    if (strCheck.ToUpper() != "HY")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                        return;
                    }
                    
                }

                string strRTOLocationCodeCheck = strVehicleNo.Substring(0, 4);
                if (strRTOLocationCodeCheck.ToUpper() == "HR55")
                {
                    if (Convert.ToInt32(RTOLocationID) != 483 && Convert.ToInt32(RTOLocationID) != 484 && Convert.ToInt32(RTOLocationID) != 485)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = "Please Check Vehicle Registration No.";
                        return;
                    }

                }


                String SqlQry = "select distinct VehicleregNo  from  hsrpexcelupload WHERE  ISNULL(VehicleregNo,'')!=''";
                DataTable dtbl = Utils.GetDataTable(SqlQry, ConnectionString);
                if (dtbl.Rows.Count > 0)
                {

                    string VehicleNo = Convert.ToString(txtRegNumber.Text).Trim();

                    for (int i = 0; i < dtbl.Rows.Count; i++)
                    {
                        if (VehicleNo.ToUpper().Trim() == Convert.ToString(dtbl.Rows[i]["VehicleregNo"]).ToUpper().Trim())
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "";
                            lblErrMess.Text = "Please  Contact With Administrator";
                            return;
                        }


                    }
                }

                            
              //  string ACno;
                if (txtRegNumber.Text == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Required Registration No..";
                    return;
                }
             
                    String SqlQuery1 = "SELECT top(1) VehicleClass FROM HSRPRecordsStaggingArea where VehicleRegNo='" + txtRegNumber.Text.Trim() + "' and HSRP_StateID='" + HSRPStateID.Trim() + "' order by  HSRPRecord_CreationDate desc";
                    DataTable dt1 = Utils.GetDataTable(SqlQuery1, ConnectionString);
                    string result = txtRegNumber.Text.Substring(0, 4);
                    String SqlQuery2 = "Select * from HR_Assignment_Days_RTO where rtocode='" + result + "'";
                    DataTable dt2 = Utils.GetDataTable(SqlQuery2, ConnectionString);
                    if (dt2.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["VehicleClass"].ToString().ToUpper() == "TRANSPORT")
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "";
                            lblErrMess.Text = "No Record Found";
                            refresh();
                            return;
                        }
                    }
              //  }
                String SqlQuery = "SELECT top(1) HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,OwnerName,Address1,MobileNo,EmailID,OrderType,VehicleClass,NICvehicletype,ManufacturerName,ManufacturerModel,ChassisNo,EngineNo,VehicleRegNo FROM HSRPRecordsStaggingArea where VehicleRegNo='" + txtRegNumber.Text + "' and HSRP_StateID='" + HSRPStateID + "' order by  HSRPRecord_CreationDate desc";
                DataTable dt = Utils.GetDataTable(SqlQuery, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    strAuthno = dt.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                    StrAuthdate = dt.Rows[0]["HSRPRecord_AuthorizationDate"].ToString();
                    StrRegistrationNo = dt.Rows[0]["VehicleRegNo"].ToString();
                    if (string.IsNullOrEmpty(StrRegistrationNo))
                    {
                        string closescript1 = "<script>alert('Please Provide Vehicle Registration No.')</script>";
                        Page.RegisterStartupScript("abc", closescript1);
                        return;
                    }
                    StrRtoName = RTOlocationName.ToString();
                    StrOwnerName = dt.Rows[0]["OwnerName"].ToString();
                    stremail = dt.Rows[0]["EmailID"].ToString();
                    StrOwnerAddress = dt.Rows[0]["Address1"].ToString();

                    string StrNICVehicleType = dt.Rows[0]["NICvehicletype"].ToString();
                    string strquery = "select top 1 upper(ourValue) as ourValue from  [dbo].[Mapping_Vahan_HSRP] where vahanvalue ='" + StrNICVehicleType + "'";
                    StrVehicleType = Utils.getDataSingleValue(strquery, ConnectionString, "ourValue");
                    if (StrVehicleType == "0")
                    {
                        lblErrMess.Text = "";
                        lblErrMess.Visible = true;
                        lblErrMess.Text = StrNICVehicleType + " Vehicle Type Not Found";
                        return;
                    }
                    StrTransactonType = dt.Rows[0]["OrderType"].ToString();
                    StrVehicleClassType = dt.Rows[0]["VehicleClass"].ToString();
                    StrMobileNo = dt.Rows[0]["MobileNo"].ToString();
                    StrManufacturarName = dt.Rows[0]["ManufacturerName"].ToString();
                    StrModelName = dt.Rows[0]["ManufacturerModel"].ToString();
                    StrEngineNo = dt.Rows[0]["EngineNo"].ToString();
                    StrChasisNo = dt.Rows[0]["ChassisNo"].ToString();

                 
                       txtEmail.Text = stremail;
                       txtOwnerName.Text = StrOwnerName;
                       txtAddress.Text = StrOwnerAddress;
                       ddlVehicletype.Text = StrVehicleType;
                       // lblTransactionType.Text = StrTransactonType;
                       txtMobileNo.Text = StrMobileNo;
                       ddlVehicleclass.Text = StrVehicleClassType;
                       //// lblMfgName.Text = StrManufacturarName;
                       txtmodel.Text = StrModelName;
                       txtRegNumber.Text = StrRegistrationNo;
                       txtEngineNo.Text = StrEngineNo;
                       txtChassisno.Text = StrChasisNo;
                       if (!string.IsNullOrEmpty(txtOwnerName.Text.Trim()))
                       {
                           txtOwnerName.ReadOnly = true;
                       }

                       if (!string.IsNullOrEmpty(ddlVehicletype.Text.Trim()))
                       {
                           ddlVehicletype.Enabled = false;
                       }
                    

                       if (!string.IsNullOrEmpty(ddlVehicleclass.Text.Trim()))
                       {
                           ddlVehicleclass.Enabled = false;
                       }

                       if (!string.IsNullOrEmpty(txtAddress.Text.Trim()))
                       {
                           txtAddress.ReadOnly = true;
                       }

                       //if (!string.IsNullOrEmpty(ddlVehicletype.Text.Trim()))
                       //{
                       //    ddlVehicletype.ReadOnly = true;
                       //}

                       if (!string.IsNullOrEmpty(txtEngineNo.Text.Trim()))
                       {
                           txtEngineNo.ReadOnly = true;
                       }

                       if (!string.IsNullOrEmpty(txtChassisno.Text.Trim()))
                       {
                           txtChassisno.ReadOnly = true;
                       }

                       if (!string.IsNullOrEmpty(txtmodel.Text.Trim()))
                       {
                           txtmodel.ReadOnly = true;
                       }
                       
                      


                    string SQLString2 = string.Empty;
                    SQLString2 = "select dbo.hsrpplateamt ('" + HSRPStateID + "','" + StrVehicleType + "','" + StrVehicleClassType + "','" + StrTransactonType + "') as Amount";
                    DataTable dtamt = Utils.GetDataTable(SQLString2, ConnectionString);
                    lblAmount.Text = dtamt.Rows[0]["Amount"].ToString();


                    string query = "select userid from HRAC_master where hsrp_stateid = 4 and activestatus = 'Y'";

                    DataTable dtAcuserid = Utils.GetDataTable(query, ConnectionString);

                    if (dtAcuserid.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtAcuserid.Rows.Count; i++)
                        {

                            if (USERID == dtAcuserid.Rows[i]["userid"].ToString().Trim())
                            {

                                for (int j = 0; j < dtAcuserid.Rows.Count; j++)
                                {

                                    string sqlquery = "select count (engineno)as no from hsrprecords where  hsrp_stateid = 4 and  LTRIM(RTRIM(engineno))= '" + txtEngineNo.Text.ToString().Trim() + "' and createdby = '" + dtAcuserid.Rows[j]["userid"].ToString().Trim() + "'";
                                    int no = Utils.getScalarCount(sqlquery, ConnectionString);

                                    if (no > 0)
                                    {
                                        lblErrMess.Visible = true;
                                        lblErrMess.Text = "";
                                        lblErrMess.Text = "Duplicate  Engine No.";
                                        return;

                                    }

                                }


                            }


                        }

                    }

                    txtRegNumber.ReadOnly = true;

                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "No Record Found";

                    return;
                }
            }
            catch (Exception ex)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "RTA Server is not responding... Pls contact to System Administrator  " + ex.ToString();
            }


        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            Response.Redirect("UploadDealerData.aspx");
          
        }

     
    }
  
}