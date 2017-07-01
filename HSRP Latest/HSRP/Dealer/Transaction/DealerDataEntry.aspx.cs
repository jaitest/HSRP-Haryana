using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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


namespace HSRP.Dealer.Transaction
{
    public partial class DealerDataEntry : System.Web.UI.Page
    {
        string SQLString = string.Empty;
        string HSRPRecordID = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        static string Status = string.Empty;
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserType = Session["UserType"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            if (!IsPostBack)
            {
                BL.blEntryDetail bl = new BL.blEntryDetail();
                bl.DailyEnityDeatail(Session["UID"].ToString(), "DealerDataEntry.aspx");

                InitialSetting();
                FillVehicleMaker();
                RTOLocation();
            }
        }

        private void RTOLocation()
        {
            //Utils.PopulateDropDownList(ddlLocation, "select * from RTOlocation where HSRP_StateID='2' and locationType='Sub-Urban' order by RTOlocationName", CnnString, "--Select Location--");
            //ddlLocation.SelectedIndex = 0;

        }

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(00.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
        private void FillVehicleMaker()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownListVehicleMaker1, "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription", CnnString, "--Select Vehicle Maker--");
            DropDownListVehicleMaker1.SelectedIndex = 0;
            // DropDownListFrontPlateSize.Items.Count - 1;
            //  UpdatePanelVehicleMake.Update();
        }

        private void FillVehicleModel1()
        {
            //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //Utils.PopulateDropDownList(DropDownList44, "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster ", CnnString, "--Select Vehicle Maker--");
            //DropDownList44.SelectedIndex = 0;
            // DropDownListFrontPlateSize.Items.Count - 1;
            // DropDownListModel.Update();
        }

        private void FillVehicleModel()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownListModel, "Select VehicleModelID,VehicleModelDescription From VehicleModelMaster where VehicleMakerID=" + DropDownListVehicleMaker1.SelectedValue + " order by VehicleModelDescription", CnnString, "--Select Vehicle Model--");
            DropDownListModel.SelectedIndex = 0;
            UpdatePanelVehicleModel1.Update();
            // DropDownListFrontPlateSize.Items.Count - 1;
            // DropDownListModel.Update();
        }

        protected void DropDownListVehicleMaker1_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (DropDownListVehicleMaker1.SelectedValue.Equals("--Select Vehicle Maker--"))
            {
                //lblErrMess.Text = "Please Select Correct Vehicle Maker.";
                //UpdatePanelMessage.Update();
                DropDownListVehicleMaker1.Focus();
                return;

            }
            else
            {
                FillVehicleModel();
            }
        }

        protected void DropDownListVehicleMaker1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListVehicleMaker1.SelectedValue.Equals("--Select Vehicle Maker--"))
            {
                //lblErrMess.Text = "Please Select Correct Vehicle Maker.";
                //UpdatePanelMessage.Update();
                DropDownListVehicleMaker1.Focus();
                return;

            }
            else
            {
                FillVehicleModel();
            }
        }

        protected void DropDownList44_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblErrMess.Text = "";
            //UpdatePanelMessage.Update();
            if (DropDownListVehicleMaker1.SelectedValue.Equals("--Select Vehicle Maker--"))
            {
                //lblErrMess.Text = "Please Select Correct Vehicle Maker.";
                //UpdatePanelMessage.Update();
                DropDownListVehicleMaker1.Focus();
                return;

            }
            else
            {
                FillVehicleModel();
            }

        }
        string Query = string.Empty;
        int i=0;
        string StringManufacturerModel = string.Empty, StringManufacturerName=string.Empty;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            string Query = "select (prefixtext+right('00000000'+ convert(varchar,lastno+1),9)) as PrifixNo,(LastNo+1)as LastNo  from prefix where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
            dt1 = Utils.GetDataTable(Query, CnnString);

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            //string Mon = ("0" + StringOrderDate[0]);
            //string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            //string FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

            //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            ////OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            //string OrderDate1 = From1 + " 23:59:59";


            string SaveTime = string.Empty;
            string ReportTimees = "select left(CONVERT(char(5), GETDATE(), 108),2) currenttime";
            DataTable dtreport = Utils.GetDataTable(ReportTimees, CnnString);
            int time = Convert.ToInt16(dtreport.Rows[0]["currenttime"].ToString());
            if (time < 1)
            {
                SaveTime = "1";
            }
            else
            {
                SaveTime = "8";
            }

            DataTable dt = new DataTable();
            Query = "Select * from HSRPRecords where VehicleRegNo='" + txtVehicleRegNo.Text + "' ";
            dt = Utils.GetDataTable(Query, CnnString);
            
            if (dt.Rows.Count > 0)
            {
                //lblSucMess.Text = "Duplicate Records";      
                lblErrMess.Text = "Vehicle Already Exist";
                lblMesageSave.Text = + dt.Rows.Count + " Record(s) Already exist.Click on Ok To Save Another";
                GridView1.DataSource = dt;
                GridView1.DataBind();
                ModalPopupExtender1.Show();
            }
            else
            {
                
                if (StringManufacturerModel == "--Select Vehicle Model--")
                {
                    StringManufacturerModel = null;
                }
                else
                {
                    StringManufacturerModel = DropDownListVehicleModel.SelectedValue.ToString();
                }
                if (StringManufacturerName == "--Select Vehicle Maker--")
                {
                    StringManufacturerName = null;
                }
                else
                {
                    StringManufacturerName = DropDownListVehicleMaker1.SelectedValue.ToString();
                }
                Query = "insert into dbo.HSRPRecords(HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationDate ,VehicleRegNo,VehicleClass,VehicleType,ManufacturerName,ManufacturerModel,EngineNo,ChassisNo,HSRPRecord_AuthorizationNo,OwnerName,Address1,MobileNo,NetAmount,OrderType,CashReceiptNo)" +
                                                   " values('" + HSRPStateID.ToString() + "','" + RTOLocationID + "','" + OrderDate.SelectedDate.ToString() + "','" + txtVehicleRegNo.Text + "','" + DropDownListVehicleClass.SelectedValue.ToString() + "','" + StringManufacturerName + "','" + StringManufacturerModel + "','" + DropDownListModel.SelectedValue.ToString() + "','" + txtEngineNo.Text + "','" + txtchassisNo.Text + "','" + txtAuthNo.Text + "','" + txtCustomerName.Text + "','" + txtAddress.Text + "','" + txtMobileNo.Text + "','" + txtRoomPrice.Text + "','" + lblStatus.Text + "','" + dt1.Rows[0]["PrifixNo"].ToString() + "') ";
                  i = Utils.ExecNonQuery(Query, CnnString);

               Query = "update prefix set LastNo='" + dt1.Rows[0]["LastNo"].ToString() + "' where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
               Utils.ExecNonQuery(Query, CnnString);
               lblSucMess.Text = "Vehicle Added Successfully";
            }
            if (i > 0)
            {
                lblErrMess.Text = "";
                DataTable dt2= Utils.GetDataTable("Select HSRPRecordID from HsrpRecords where VehicleRegNo='"+txtVehicleRegNo.Text+"'",CnnString);
                HSRPRecordID = dt2.Rows[0]["HSRPRecordID"].ToString();
                CashReceipt(HSRPRecordID,"y");
                lblSucMess.Text = "Record Saved Successfully";
            }
        }

        private void Inserrt_Vendor_HsrpRecords()
        {
            
        }

        public void Refresh()
        {
            InitialSetting();
            FillVehicleMaker();
            RTOLocation();
            txtVehicleRegNo.Text="";
            txtEngineNo.Text ="";
            txtchassisNo.Text="";
            txtAuthNo.Text="";
            txtCustomerName.Text="";
            txtAddress.Text="";
            txtMobileNo.Text="";
            txtRoomPrice.Text = "";
        }

        public void CashReceipt(string HSRPRecordID, string OrderStatus)
        {

            DataTable GetAddress;
            string Address;
            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", CnnString);

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }

            SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where HSRPRecordID='"+HSRPRecordID+"' order by HSRPRecord_AuthorizationNo,vehicleRegNo desc";
            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, CnnString);


            BAL obj = new BAL();
            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {


                //string filename = "CASH RECEIPT-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
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

                //fix the absolute width of the table



                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 4;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1203 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["Address1"].ToString() + " , " + GetAddress.Rows[0]["city"].ToString() + Address.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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

                string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);

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



                PdfPCell cell3 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell3.Colspan = 1;
                cell3.BorderWidthLeft = 0f;
                cell3.BorderWidthRight = 0f;
                cell3.BorderWidthTop = 0f;
                cell3.BorderWidthBottom = 0f;
                cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell3);

                PdfPCell cell31 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell31.Colspan = 3;
                cell31.BorderWidthLeft = 0f;
                cell31.BorderWidthRight = 0f;
                cell31.BorderWidthTop = 0f;
                cell31.BorderWidthBottom = 0f;
                cell31.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell31);

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

                PdfPCell cellDescSp = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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


                PdfPCell cell120 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120.Colspan = 1;
                cell120.BorderWidthLeft = 0f;
                cell120.BorderWidthRight = 0f;
                cell120.BorderWidthTop = 0f;
                cell120.BorderWidthBottom = 0f;
                cell120.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120);


                PdfPCell cellNet120 = new PdfPCell(new Phrase("NET AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellNet120.Colspan = 1;
                cellNet120.BorderWidthLeft = 0f;
                cellNet120.BorderWidthRight = 0f;
                cellNet120.BorderWidthTop = 0f;
                cellNet120.BorderWidthBottom = 0f;
                cellNet120.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellNet120);

                PdfPCell cellAmt1205 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellAmt1205.Colspan = 1;
                cellAmt1205.BorderWidthLeft = 0f;
                cellAmt1205.BorderWidthRight = 0f;
                cellAmt1205.BorderWidthTop = 0f;
                cellAmt1205.BorderWidthBottom = 0f;
                cellAmt1205.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellAmt1205);

                PdfPCell cell1205 = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1205.Colspan = 1;
                cell1205.BorderWidthLeft = 0f;
                cell1205.BorderWidthRight = 0f;
                cell1205.BorderWidthTop = 0f;
                cell1205.BorderWidthBottom = 0f;
                cell1205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1205);

                PdfPCell celldupRouCash401 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash401.Colspan = 1;
                celldupRouCash401.BorderWidthLeft = 0f;
                celldupRouCash401.BorderWidthRight = 0f;
                celldupRouCash401.BorderWidthTop = 0f;
                celldupRouCash401.BorderWidthBottom = 0f;
                celldupRouCash401.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash401);

                PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OF AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
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

                string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day from the date of  issuance of cash receipt.";

                PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell64.Colspan = 4;
                cell64.BorderWidthLeft = 0f;
                cell64.BorderWidthRight = 0f;
                cell64.BorderWidthTop = 0f;
                cell64.BorderWidthBottom = 0f;
                cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64);


                string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt.";

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
                table.AddCell(cell63);

                PdfPCell cellsp4 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp4.Colspan = 4;
                cellsp4.BorderWidthLeft = 0f;
                cellsp4.BorderWidthRight = 0f;
                cellsp4.BorderWidthTop = 0f;
                cellsp4.BorderWidthBottom = 0f;
                cellsp4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp4);

                PdfPCell cellsp5 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp5.Colspan = 4;
                cellsp5.BorderWidthLeft = 0f;
                cellsp5.BorderWidthRight = 0f;
                cellsp5.BorderWidthTop = 0f;
                cellsp5.BorderWidthBottom = 0f;
                cellsp5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp5);

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 4;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell62);






                //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                PdfPCell cell2195 = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2195.Colspan = 4;
                cell2195.BorderWidthLeft = 0f;
                cell2195.BorderWidthRight = 0f;
                cell2195.BorderWidthTop = 0f;
                cell2195.BorderWidthBottom = 0f;
                cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2195);

                PdfPCell cellsp1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp1.Colspan = 4;
                cellsp1.BorderWidthLeft = 0f;
                cellsp1.BorderWidthRight = 0f;
                cellsp1.BorderWidthTop = 0f;
                cellsp1.BorderWidthBottom = 0f;
                cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp1);

                PdfPCell cellsp2 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp2.Colspan = 4;
                cellsp2.BorderWidthLeft = 0f;
                cellsp2.BorderWidthRight = 0f;
                cellsp2.BorderWidthTop = 0f;
                cellsp2.BorderWidthBottom = 0f;
                cellsp2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp2);

                PdfPCell cellsp3 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp3.Colspan = 4;
                cellsp3.BorderWidthLeft = 0f;
                cellsp3.BorderWidthRight = 0f;
                cellsp3.BorderWidthTop = 0f;
                cellsp3.BorderWidthBottom = 0f;
                cellsp3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp3);

                document.Add(table);
                document.Add(table);

                document.Close();
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        protected void ButtonVehicleGo_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
          //  allEnabletrue();
            string SqlCheck = "select * from [dbo].[View_MP_HSRPRecordsStagging_New] where vehicleRegNo='" + txtVehicleRegNo.Text.Trim().Trim() + "'";
           // String SqlCheck = "SELECT Top(1)  RTOLocationID, HSRP_StateID, HSRPRecord_CreationDate, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, OwnerName, OwnerFatherName, Address1, MobileNo, EmailID,StickerMandatory, OrderType,OrderStatus, VehicleClass, VehicleType, ManufacturerName, ManufacturerModel, ChassisNo, EngineNo, Manufacturer, VehicleRegNo, NetAmount FROM HSRPRecords  where hsrp_StateID='" + Session["UserHSRPStateID"].ToString() + "' and vehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "'";
            DataTable dt = Utils.GetDataTable(SqlCheck, CnnString);


            if (dt.Rows.Count > 0)
            {
                txtCustomerName.Text = dt.Rows[0]["OwnerName"].ToString();
                txtAuthNo.Text = dt.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                txtEngineNo.Text = dt.Rows[0]["EngineNo"].ToString();
                txtchassisNo.Text = dt.Rows[0]["ChassisNo"].ToString();
                txtAddress.Text = dt.Rows[0]["Address1"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                txtRoomPrice.Text = dt.Rows[0]["NetAmount"].ToString();
                if (dt.Rows[0]["ManufacturerName"].ToString() != "" || dt.Rows[0]["ManufacturerName"].ToString() == null)
                {
                    DropDownListVehicleMaker1.SelectedValue = dt.Rows[0]["ManufacturerName"].ToString();
                    DropDownListModel.SelectedValue = dt.Rows[0]["ManufacturerModel"].ToString();
                }
               // DropDownListVehicleClass.Text = dt.Rows[0]["VehicleClass"].ToString();
                DropDownListVehicleClass.SelectedValue = dt.Rows[0]["VehicleClass"].ToString();

           //     DropDownListVehicleModel.SelectedItem.Text = dt.Rows[0]["VehicleType"].ToString();
                DropDownListVehicleModel.SelectedValue = dt.Rows[0]["VehicleType"].ToString();
                

                if (DropDownListVehicleModel.SelectedItem.Text == "THREE WHEELER" || DropDownListVehicleModel.SelectedItem.Text == "LMV" || DropDownListVehicleModel.SelectedItem.Text == "LMV(CLASS)" || DropDownListVehicleModel.SelectedItem.Text == "MCV/HCV/TRAILERS")
                {
                    checkBoxThirdSticker.Checked = true;
                }
                else
                {
                    checkBoxThirdSticker.Checked = false;
                }
                if (dt.Rows[0]["OrderType"].ToString() != "")
                {
                    lblStatus.Text = dt.Rows[0]["OrderType"].ToString();
                    if (lblStatus.Text == "NB")
                    {
                        LblOrderType.Text = "NEW BOTH PLATES";
                    }
                    if (lblStatus.Text == "DF")
                    {
                        LblOrderType.Text = "DAMAGED FRONT PLATE";
                    }
                    if (lblStatus.Text == "DB")
                    {
                        LblOrderType.Text = "DAMAGED BOTH PLATES";
                    }
                    if (lblStatus.Text == "OB")
                    {
                        LblOrderType.Text = "OLD BOTH PLATES";
                    }
                    if (lblStatus.Text == "DR")
                    {
                        LblOrderType.Text = "DAMAGED REAR PLATE";
                    }
                    if (lblStatus.Text == "OS")
                    {
                        LblOrderType.Text = "ONLY STICKER";
                    }                    
                    DataTable dt1 = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedValue.ToString() + "','" + lblStatus.Text + "') as Amount", CnnString);
                    txtRoomPrice.Text = dt1.Rows[0]["Amount"].ToString();
                }
            }
            else
            {
                lblErrMess.Text = "No Record Found";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {           
            ModalPopupExtender1.Hide();
        }

        protected void ButtonAuthorizationNo_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            //  allEnabletrue();
            string SqlCheck = "select * from [dbo].[View_MP_HSRPRecordsStagging_New] where HSRPRecord_AuthorizationNo='" + txtAuthNo.Text.Trim().Trim() + "'";
            // String SqlCheck = "SELECT Top(1)  RTOLocationID, HSRP_StateID, HSRPRecord_CreationDate, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, OwnerName, OwnerFatherName, Address1, MobileNo, EmailID,StickerMandatory, OrderType,OrderStatus, VehicleClass, VehicleType, ManufacturerName, ManufacturerModel, ChassisNo, EngineNo, Manufacturer, VehicleRegNo, NetAmount FROM HSRPRecords  where hsrp_StateID='" + Session["UserHSRPStateID"].ToString() + "' and vehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "'";
            DataTable dt = Utils.GetDataTable(SqlCheck, CnnString);


            if (dt.Rows.Count > 0)
            {
                txtCustomerName.Text = dt.Rows[0]["OwnerName"].ToString();
                txtVehicleRegNo.Text = dt.Rows[0]["vehicleRegNo"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                txtEngineNo.Text = dt.Rows[0]["EngineNo"].ToString();
                txtchassisNo.Text = dt.Rows[0]["ChassisNo"].ToString();
                txtAddress.Text = dt.Rows[0]["Address1"].ToString();
                txtMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
                txtRoomPrice.Text = dt.Rows[0]["NetAmount"].ToString();
                if (dt.Rows[0]["ManufacturerName"].ToString() != "" || dt.Rows[0]["ManufacturerName"].ToString() == null)
                {
                    DropDownListVehicleMaker1.SelectedValue = dt.Rows[0]["ManufacturerName"].ToString();
                    DropDownListModel.SelectedValue = dt.Rows[0]["ManufacturerModel"].ToString();
                }
                DropDownListVehicleClass.SelectedItem.Text = dt.Rows[0]["VehicleClass"].ToString();
                DropDownListVehicleClass.SelectedValue = dt.Rows[0]["VehicleClass"].ToString();

                DropDownListVehicleModel.SelectedItem.Text = dt.Rows[0]["VehicleType"].ToString();
                DropDownListVehicleModel.SelectedValue = dt.Rows[0]["VehicleType"].ToString();

                if (DropDownListVehicleModel.SelectedItem.Text == "THREE WHEELER" || DropDownListVehicleModel.SelectedItem.Text == "LMV" || DropDownListVehicleModel.SelectedItem.Text == "LMV(CLASS)" || DropDownListVehicleModel.SelectedItem.Text == "MCV/HCV/TRAILERS")
                {
                    checkBoxThirdSticker.Checked = true;
                }
                else
                {
                    checkBoxThirdSticker.Checked = false;
                }
                if (dt.Rows[0]["OrderType"].ToString() != "")
                {
                    lblStatus.Text = dt.Rows[0]["OrderType"].ToString();
                    if (lblStatus.Text == "NB")
                    {
                        LblOrderType.Text = "NEW BOTH PLATES";
                    }
                    if (lblStatus.Text == "DF")
                    {
                        LblOrderType.Text = "DAMAGED FRONT PLATE";
                    }
                    if (lblStatus.Text == "DB")
                    {
                        LblOrderType.Text = "DAMAGED BOTH PLATES";
                    }
                    if (lblStatus.Text == "OB")
                    {
                        LblOrderType.Text = "OLD BOTH PLATES";
                    }
                    if (lblStatus.Text == "DR")
                    {
                        LblOrderType.Text = "DAMAGED REAR PLATE";
                    }
                    if (lblStatus.Text == "OS")
                    {
                        LblOrderType.Text = "ONLY STICKER";
                    }
                    DataTable dt1 = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedValue.ToString() + "','" + lblStatus.Text + "') as Amount", CnnString);
                    txtRoomPrice.Text = dt1.Rows[0]["Amount"].ToString();
                }
            }
            else
            {
                lblErrMess.Text = "No Record Found";
            }
        }

        protected void DropDownListVehicleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt1 = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedValue.ToString() + "','" + lblStatus.Text + "') as Amount", CnnString);
            txtRoomPrice.Text = dt1.Rows[0]["Amount"].ToString();
            if (DropDownListVehicleModel.SelectedItem.Text == "SCOOTER" || DropDownListVehicleModel.SelectedItem.Text == "MOTOR CYCLE" || DropDownListVehicleModel.SelectedItem.Text == "TRACTOR")
            {
                checkBoxThirdSticker.Enabled = false;
            }
        }



    }
}