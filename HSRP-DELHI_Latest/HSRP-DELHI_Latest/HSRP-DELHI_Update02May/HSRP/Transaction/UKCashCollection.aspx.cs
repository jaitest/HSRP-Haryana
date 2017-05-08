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

namespace HSRP.Transaction
{
    public partial class UKCashCollection : System.Web.UI.Page
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
        string StrTransactonType = string.Empty;
        DataTable dt = new DataTable();
        string macbase = string.Empty;
        string SQLString = string.Empty;
        string StrNICVehicleType = string.Empty;
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
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
            ddlVehicleType.Visible = false;;
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            USERID = Session["UID"].ToString();
            macbase = Session["MacAddress"].ToString();
            string SqlQuery = "exec GetTodayCollectionUserWise '" + HSRPStateID + "','" + RTOLocationID + "','" + USERID + "','"+System.DateTime.Today.ToString("yyyy-MM-dd")+"'";
            DataTable dtCount = Utils.GetDataTable(SqlQuery, ConnectionString);
            string strVehicle = dtCount.Rows[0]["TodayVehicleCount"].ToString();
            string strAmount = dtCount.Rows[0]["TodayCollection"].ToString();
            string strDepositAmount = dtCount.Rows[0]["DepositAmount"].ToString();
            if (dtCount.Rows[0]["DepositDate"].ToString() == "")
            {
                lblLastDepositdate.Visible = true;
                lblLastDepositdate.Text = "Never Deposit";
            }
            else
            {
                string strDepositDate = Convert.ToDateTime(dtCount.Rows[0]["DepositDate"]).ToString("MM/dd/yyyy");
                lblLastDepositdate.Visible = true;
                lblLastDepositdate.Text = strDepositDate;
            }
            lblCount.Visible = true;
            lblCount.Text = strVehicle;
            lblCollection.Visible = true;
            if (strAmount == "")
            {
                lblCollection.Text = "0";
            }
            else
            {
                lblCollection.Text = strAmount;
            }

            
            if (strDepositAmount == "")
            {
                lblLastAmount.Visible = true;
                lblLastAmount.Text = "0";

            }
            else
            {

                lblLastAmount.Visible = true;
                lblLastAmount.Text = strDepositAmount;
            }
            if (!IsPostBack)
            {

                //fillAffixationCenter();
                //FillVehicleType();
            }
            else
            {

            }
        }



        //private void FillVehicleType()
        //{
        //    SQLString = "select VEHICLETYPE from VEHICLETYPE  where SHORTNAME!='' and VEHICLETYPE in ('SCOOTER','MOTOR CYCLE')";
        //    DataTable dt12 = Utils.GetDataTable(SQLString, ConnectionString);
        //    ddlVehicleType.DataSource = dt12;
        //    ddlVehicleType.DataTextField = "VEHICLETYPE";
        //    ddlVehicleType.DataValueField = "VEHICLETYPE";
        //    ddlVehicleType.DataBind();
        //    //this.ViewState["lstCustomers_SelectedValue"] = this.ddlVehicleType.SelectedItem;
        //    //ddlVehicleType.Items.Insert(0, "--Select Vehicle Type--");
            

        //    //ViewState["Dropdown"].ToString = dt12;
        //}

        protected void btnGo_Click(object sender, EventArgs e)
        {

            string strAuthno = string.Empty;
            string StrRtoLocationCode = string.Empty;
            string StrRtoName = string.Empty;
            string StrAuthorizationNo = string.Empty;
            string StrOwnerName = string.Empty;
            string StrOwnerAddress = string.Empty;
            string StrVehicleType = string.Empty;
            //string StrTransactonType = string.Empty;
            string StrAuthdate = string.Empty;
            string StrMobileNo = string.Empty;
            string StrVehicleClassType = string.Empty;
            string StrManufacturarName = string.Empty;
            string StrModelName = string.Empty;
            string StrRegistrationNo = string.Empty;
            string StrEngineNo = string.Empty;
            string StrChasisNo = string.Empty;
            // string strAuthNo = txtAuthNo.Text.Trim();
            string stremail = string.Empty;
            string SQLString = string.Empty;
            string RTOlocationName = string.Empty;
            DataTable dt = new DataTable();
            string StrNICVehicleType1 = string.Empty;
            string StrNICVehicleType2 = string.Empty;
            ddlVehicleClass.SelectedIndex = 0;
            SQLString = "select Rtolocationname from rtolocation where rtolocationid='" + RTOLocationID + "'";
            RTOlocationName = Utils.getScalarValue(SQLString, ConnectionString);

            try
            {

                string strVehicleNo = txtRegNo.Text;
                string strCheck = strVehicleNo.Substring(0, 2);
                if (strCheck != "UK" && strCheck != "uk")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Valid Vehicle RegNo.";
                    return;
                }

                string strDate = System.DateTime.Now.ToString("hh:mm tt");
                string strnine = "09:00 AM";
                string strsix = "06:00 PM";
                if (DateTime.Parse(strDate) < DateTime.Parse(strnine) || DateTime.Parse(strDate) > DateTime.Parse(strsix))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Cash Collection Timing is  Between 9 AM TO 6 PM ";
                    return;
                }   

                bln3rdSticker.Checked = false;
                String SqlQuery = "SELECT top(1) HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,OwnerName,Address1,MobileNo,EmailID,OrderType,VehicleClass,NICvehicletype,ManufacturerName,ManufacturerModel,ChassisNo,EngineNo,VehicleRegNo FROM HSRPRecordsStaggingArea where VehicleRegNo='" + txtRegNo.Text + "' and HSRP_StateID='" + HSRPStateID + "' order by  HSRPRecord_CreationDate desc";
                dt = Utils.GetDataTable(SqlQuery, ConnectionString);
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

                    StrNICVehicleType1 = dt.Rows[0]["NICvehicletype"].ToString().Split('*')[0];
                    ViewState["StrNICVehicleType1"] = StrNICVehicleType1;
                    StrNICVehicleType = ViewState["StrNICVehicleType1"].ToString();
                    if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
                    {
                       // FillVehicleType();
                        ddlVehicleType.Visible = true;

                    }
                    string strquery = "select top 1 upper(ourValue) as ourValue from  [dbo].[Mapping_Vahan_HSRP] where vahanvalue ='" + StrNICVehicleType1 + "'";
                    StrVehicleType = Utils.getDataSingleValue(strquery, ConnectionString, "ourValue");
                    if (StrVehicleType == "0")
                    {
                        lblErrMess.Text = "";
                        lblErrMess.Visible = true;
                        lblErrMess.Text = StrVehicleType + " Vehicle Type Not Found";
                        return;
                    }
                StrTransactonType= dt.Rows[0]["OrderType"].ToString();
                 ViewState["ordertype"] = StrTransactonType;
                // StrTransactonType = ViewState["ordertype"].ToString();

                    StrVehicleClassType = ddlVehicleClass.SelectedItem.ToString();
                    StrMobileNo = dt.Rows[0]["MobileNo"].ToString();
                    StrManufacturarName = dt.Rows[0]["ManufacturerName"].ToString();
                    StrModelName = dt.Rows[0]["ManufacturerModel"].ToString();
                    StrEngineNo = dt.Rows[0]["EngineNo"].ToString();
                    StrChasisNo = dt.Rows[0]["ChassisNo"].ToString();

                    lblAuthNo.Text = strAuthno;

                    lblRTOName.Text = StrRtoName;
                    lblEmail.Text = stremail;
                    lblOwnerName.Text = StrOwnerName;
                    lblAddress.Text = StrOwnerAddress;
                    lblVehicleType.Text = StrVehicleType;
                    lblTransactionType.Text = StrTransactonType;
                    lblAuthDate.Text = StrAuthdate;
                    //if (StrMobileNo.Trim() == "" || StrMobileNo.Trim() == null)
                    //{
                    //    txtMobileno.Text = "0";
                    //}
                    //else
                    //{
                        txtMobileno.Text = StrMobileNo;
                  //  }
                    //lblVehicleClassType.Text = StrVehicleClassType;
                    lblMfgName.Text = StrManufacturarName;
                    lblModelName.Text = StrModelName;
                    lblRegNo.Text = StrRegistrationNo;
                    lblEngineNo.Text = StrEngineNo;
                    lblChasisNo.Text = StrChasisNo;




                    if ((StrVehicleType == "MCV/HCV/TRAILERS") || (StrVehicleType == "LMV(CLASS)") || (StrVehicleType == "LMV") || StrVehicleType == "THREE WHEELER")
                    {
                        bln3rdSticker.Checked = true;
                    }
                    else
                    {
                        bln3rdSticker.Checked = false;
                    }

                    if (bln3rdSticker.Checked == true)
                    {
                        Sticker = "Y";
                    }
                    else
                    {
                        Sticker = "N";
                    }

                    if (blnVIP.Checked == true)
                    {
                        VIP = "Y";
                    }
                    else
                    {
                        VIP = "N";
                    }
                    btnDownload.Visible = false;
                }
                else
                {
                    lblErrMess.Text = "";
                    lblErrMess.Text = "No Record Found";
                    refresh();
                    return;
                }
            }
            catch (Exception ex)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "RTA Server is not responding... Pls contact to System Administrator  " + ex.ToString();
            }
        }


        public void refresh()
        {
            lblAuthNo.Text = "";

            lblRTOName.Text = "";
            lblEmail.Text = "";
            lblOwnerName.Text = "";
            lblAddress.Text = "";
            lblVehicleType.Text = "";
            lblTransactionType.Text = "";
            lblAuthDate.Text = "";
            txtMobileno.Text = "";
            //lblVehicleClassType.Text = "";
            lblMfgName.Text = "";
            lblModelName.Text = "";
            lblRegNo.Text = "";
            lblEngineNo.Text = "";
            lblChasisNo.Text = "";
            lblAmount.Text = "";
        }


        protected void btnDownloadReceipt_Click(object sender, EventArgs e)
        {

            DataTable GetAddress;
            string Address;
            string TIN;

            DataTable rtoaddr = new DataTable();
            rtoaddr = Utils.GetDataTable("select r.RTOLocationAddress from users as u inner join rtolocation as r on u.RTOLocationID=r.RTOLocationID where userid='" + Session["UID"].ToString() + "'", ConnectionString);

            //  string sqlquery = "select Address1 from AffixationCenters where Rto_Id='" + ddlaffixation.SelectedValue.ToString() + "'";
            //string  AffAddress = Utils.getDataSingleValue(sqlquery, ConnectionString, "Address1");


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

            string SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where  VehicleRegNo ='" + txtRegNo.Text + "'";
            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);



            string username = " select UserLoginName from users where UserID='" + USERID + "'";
            DataTable dtusername = Utils.GetDataTable(username, ConnectionString);
            string userloginname = dtusername.Rows[0]["UserLoginName"].ToString();


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

                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 4;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);



                DataTable GetAddressukbr = new DataTable();
                if (HSRPStateID == "6")
                {
                    GetAddressukbr = Utils.GetDataTable("select rtolocationid,rtolocationname,rtolocationaddress from rtolocation WHERE HSRP_StateID='" + HSRPStateID + "' and rtolocationid='" + RTOLocationID + "'", ConnectionString);

                }
                PdfPCell cell1203 = new PdfPCell(new Phrase("For Affixation kindly visit :-  " + GetAddressukbr.Rows[0]["rtolocationname"].ToString() + " , " + GetAddressukbr.Rows[0]["rtolocationaddress"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1203.Colspan = 4;
                cell1203.BorderWidthLeft = 0f;
                cell1203.BorderWidthRight = 0f;
                cell1203.BorderWidthTop = 0f;
                cell1203.BorderWidthBottom = 0f;
                cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1203);



                string getTinNonew = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
                PdfPCell cellTinNo = new PdfPCell(new Phrase("TIN NO." + getTinNonew.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellTinNo.Colspan = 4;
                cellTinNo.BorderColor = BaseColor.WHITE;
                cellTinNo.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellTinNo);




                PdfPCell cellRtoLocation = new PdfPCell(new Phrase("RTOLocation :" + GetAddressukbr.Rows[0]["rtolocationname"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellRtoLocation.Colspan = 4;
                cellRtoLocation.BorderColor = BaseColor.WHITE;
                cellRtoLocation.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellRtoLocation);



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




                PdfPCell cell22 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22.Colspan = 1;

                cell22.BorderWidthLeft = 0f;
                cell22.BorderWidthRight = 0f;
                cell22.BorderWidthTop = 0f;
                cell22.BorderWidthBottom = 0f;
                cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22);

                PdfPCell cell222 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell222.Colspan = 1;

                cell222.BorderWidthLeft = 0f;
                cell222.BorderWidthRight = 0f;
                cell222.BorderWidthTop = 0f;
                cell222.BorderWidthBottom = 0f;
                cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell222);

                //string getExciseNo1 = Utils.getScalarValue("select ExciseNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
                //PdfPCell cell221 = new PdfPCell(new Phrase("EXCISE NO ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                // cell221.Colspan = 1;
                // cell221.BorderWidthLeft = 0f;
                // cell221.BorderWidthRight = 0f;
                // cell221.BorderWidthTop = 0f;
                // cell221.BorderWidthBottom = 0f;
                // cell221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // table.AddCell(cell221);

                // PdfPCell cell2221 = new PdfPCell(new Phrase(": " + getExciseNo1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                // cell2221.Colspan = 3;

                // cell2221.BorderWidthLeft = 0f;
                // cell2221.BorderWidthRight = 0f;
                // cell2221.BorderWidthTop = 0f;
                // cell2221.BorderWidthBottom = 0f;
                // cell2221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // table.AddCell(cell2221);

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


               // decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
               // roundAmt = Math.Round(roundAmt, 0);

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


                //string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day from the date of  issuance of cash receipt.";
                string Message = "\u2022" + " Vehicle Owner is requested to please check the Correctness of the cash slip.";

                PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell64.Colspan = 4;
                cell64.BorderWidthLeft = 0f;
                cell64.BorderWidthRight = 0f;
                cell64.BorderWidthTop = 0f;
                cell64.BorderWidthBottom = 0f;
                cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64);


                string MessageSec = "\u2022" + "Before leaving the counter verify your data on cash receipt.The company shall not be responsible for any clerical mistake what so ever.";

                PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65a.Colspan = 4;
                cell65a.BorderWidthLeft = 0f;
                cell65a.BorderWidthRight = 0f;
                cell65a.BorderWidthTop = 0f;
                cell65a.BorderWidthBottom = 0f;
                cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65a);


               

                if (Session["UserHSRPStateID"].ToString() == "6")
                {
                    string MessageSecnew = "\u2022" + " For Any Query, complaint and suggestion and Tracking Your Vehicle Please Visit WWW.hsrpuk.com";

                    PdfPCell cell65anew = new PdfPCell(new Phrase(MessageSecnew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell65anew.Colspan = 4;
                    cell65anew.BorderWidthLeft = 0f;
                    cell65anew.BorderWidthRight = 0f;
                    cell65anew.BorderWidthTop = 0f;
                    cell65anew.BorderWidthBottom = 0f;
                    cell65anew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell65anew);

                }

                PdfPCell cellsp1 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp1.Colspan = 4;
                cellsp1.BorderWidthLeft = 0f;
                cellsp1.BorderWidthRight = 0f;
                cellsp1.BorderWidthTop = 0f;
                cellsp1.BorderWidthBottom = 0f;
                cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp1);

                PdfPCell cellsp2 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["Address1"].ToString(), new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp2.Colspan = 4;
                cellsp2.BorderWidthLeft = 0f;
                cellsp2.BorderWidthRight = 0f;
                cellsp2.BorderWidthTop = 0f;
                cellsp2.BorderWidthBottom = 0f;
                cellsp2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp2);

                PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell63.Colspan = 4;
                cell63.BorderWidthLeft = 0f;
                cell63.BorderWidthRight = 0f;
                cell63.BorderWidthTop = 0f;
                cell63.BorderWidthBottom = 0f;
                cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell63);



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

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.) : - " + userloginname, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
        string Query = string.Empty;
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ToDay = System.DateTime.Today.DayOfWeek.ToString();
                if (ToDay == "Sunday")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "No Collection Allowed On Sunday";

                    return;
                }
                if (txtMobileno.Text.Trim() == "" || txtMobileno.Text.Trim() == null || txtMobileno.Text.Trim() == "0000000000" || txtMobileno.Text.Trim() == "1111111111")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Valid Mobile No.";
                    return;
                }

                if (ddlVehicleClass.SelectedItem.ToString() == "--Select Vehicle Class--")
                {

                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Select Vehicle Class.";
                    return;
                }
               string StrTransactonType2= ViewState["ordertype"].ToString();
               if (StrTransactonType2 == "DF" || StrTransactonType2 == "DR")
                {

                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Convert Tranasaction Type Into NB or OB.";
                    return;
                
                }

                string strVehicleNo = txtRegNo.Text;
                string strCheck = strVehicleNo.Substring(4, 2);
                if (ddlVehicleClass.Text == "NON-TRANSPORT")
                {
                    if (strCheck == "CA" || strCheck == "ca" || strCheck == "PA" || strCheck == "pa" || strCheck == "TA" || strCheck == "ta")
                    {


                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";

                        lblErrMess.Text = "Vehicle RegNo Must be Transport.";
                        return;


                    }
                }
               


                    //Query = "select VehicleRegNo,HsrpRecord_AuthorizationNo from hsrprecords where VehicleRegNo ='" + txtRegNo.Text.Trim() + "' and HsrpRecord_AuthorizationNo='" + lblAuthNo.Text.Trim() + "'";
                    ////int co = Utils.getScalarCount(Query, ConnectionString);
                    //DataTable dtResult = Utils.GetDataTable(Query, ConnectionString);
                    //if (dtResult.Rows.Count > 0)
                    //{
                    //    lblSucMess.Visible = false;
                    //    lblErrMess.Visible = true;
                    //    lblErrMess.Text = "VehicleRegNo. Already Exist";
                    //    return;
                    //}

                    //if (ddlaffixation.Text == "--Select Affixation Center--")
                    //{
                    //    lblErrMess.Text = "Please select Affixation Center";
                    //    ddlaffixation.Focus();
                    //    return;
                    //}


                    if (bln3rdSticker.Checked == true)
                    {
                        Sticker = "Y";
                    }
                    else
                    {
                        Sticker = "N";
                    }

                    if (blnVIP.Checked == true)
                    {
                        VIP = "Y";
                    }
                    else
                    {
                        VIP = "N";
                    }

                    string sticker1 = Sticker;

                    string sql = "exec [getPlatesData] '" + HSRPStateID + "','" + lblVehicleType.Text + "','" + ddlVehicleClass.SelectedItem.ToString() + "', '" + lblTransactionType.Text + "'";
                    DataTable dt = Utils.GetDataTable(sql, ConnectionString);
                    if (dt.Rows.Count > 0)
                    {
                        lblSucMess.Visible = false;
                    }



                    DataTable dt5 = new DataTable();
                    string cashrc = string.Empty;
                    string authdate = string.Empty;

                    string[] arrauthdate = lblAuthDate.Text.Replace("T", " ").Split('.');
                    authdate = arrauthdate[0].ToString();
                    string[] arrauthdate1 = arrauthdate[0].ToString().Split('+');
                    authdate = arrauthdate1[0].ToString();

                    string Invoice = string.Empty;
                    string DC = string.Empty;
                    cashrc = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                    cashrc = Utils.getScalarValue(cashrc, ConnectionString);
                    //cashrc = dt5.Rows[0]["Receiptno"].ToString();
                    Invoice = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                    Invoice = Utils.getScalarValue(Invoice, ConnectionString);
                    DC = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                    DC = Utils.getScalarValue(DC, ConnectionString);
                    lblAddress.Text = lblAddress.Text.Replace("'", "");


                    String strquery1 = "select [dbo].GetAffxDate_insert_new('" + HSRPStateID + "') as Date";
                    string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");
                    //select FrontPlateID,RearPlateID,StickerID,Fcost as frontplatecost,FProductCode as frontproductcode,fflag as frontplateflag,Rcost as rearplatecost,RProductCode as rearproductcode,Rflag as rearplateflag,Sflag as stickerflag,Slcost as snaplockcost,Scost as stickercost,tax,discountamount,totalamount,vatamount,netamount,vatper
                    // Query = "INSERT INTO HSRPRecords (HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,isVIP,NetAmount,VehicleType,OrderStatus,CashReceiptNo,OwnerFatherName, EmailID, ChassisNo, EngineNo, HSRP_Front_LaserCode, HSRP_Rear_LaserCode,CreatedBy,Reference,ManufacturerModel,vehicleref,FrontplatePrize,RearPlatePrize,StickerPrize,ScrewPrize,TotalAmount,VAT_Amount,RoundOff_NetAmount,VAT_Percentage) values(GetDate(),'" + HSRPStateID + "','" + RTOLocationID + "','" + lblAuthNo.Text + "',getdate(),'" + lblRegNo.Text.Trim().ToUpper() + "','" + lblOwnerName.Text + "','" + lblMobileNo.Text + "','" + lblVehicleClassType.Text + "','" + lblTransactionType.Text + "','" + Sticker + "','" + VIP + "','" + lblAmount.Text + "','" + lblVehicleType.Text + "','New Order','', '" + lblEmail.Text.Trim() + "', '" + lblChasisNo.Text.Trim() + "', '" + lblEngineNo.Text.Trim() + "', '" + dt.Rows[0]["frontplateID"].ToString() + "', '" + dt.Rows[0]["RearPlateID"].ToString() + "','" + Session["UID"].ToString() + "','" + remarks.Text + "','" + lblModelName + "','New Order','" + cashrc + "','" + dt.Rows[0]["FrontPlateCost"].ToString() + "','" + dt.Rows[0]["rearplatecost"].ToString() + "','" + dt.Rows[0]["stickercost"].ToString() + "','" + dt.Rows[0]["snaplockcost"].ToString() + "','" + dt.Rows[0]["totalamount"].ToString() + "','" + dt.Rows[0]["vatamount"].ToString() + "','" + lblAmount.Text + "','" + dt.Rows[0]["vatper"].ToString() + "')";
                    if (!string.IsNullOrEmpty(lblAmount.Text))
                    {
                        decimal dd = decimal.Parse(lblAmount.Text);
                    }
                    else
                    {
                        lblAmount.Text = "0";
                    }
                    if (authdate == "")
                    {
                        authdate = System.DateTime.Now.ToString();
                    }
                    con.Open();
                    String strFrontPrize = "0.00";
                    String strRearPrize = "0.00";
                    if (!string.IsNullOrEmpty(dt.Rows[0]["FrontPlateCost"].ToString()))
                    {
                        strFrontPrize = dt.Rows[0]["FrontPlateCost"].ToString();
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[0]["RearPlateCost"].ToString()))
                    {
                        strRearPrize = dt.Rows[0]["RearPlateCost"].ToString();
                    }

                    SqlCommand cmd = new SqlCommand("New_InsertDataUK", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MacBase", macbase);
                    cmd.Parameters.AddWithValue("@DeliveryChallanNo", DC);
                    cmd.Parameters.AddWithValue("@ISFrontPlateSize", dt.Rows[0]["frontplateflag"].ToString());
                    cmd.Parameters.AddWithValue("@ISRearPlateSize", dt.Rows[0]["rearplateflag"].ToString());
                    cmd.Parameters.AddWithValue("@invoiceno", Invoice);
                    cmd.Parameters.AddWithValue("@Address", lblAddress.Text);
                    cmd.Parameters.AddWithValue("@ManufacturerName", lblMfgName.Text);
                    cmd.Parameters.AddWithValue("@HSRPStateID", HSRPStateID);
                    cmd.Parameters.AddWithValue("@RTOLocationID", RTOLocationID);
                    cmd.Parameters.AddWithValue("@HSRPRecordAuthorizationNo", lblAuthNo.Text);
                    cmd.Parameters.AddWithValue("@HSRPRecord_AuthorizationDate", authdate);
                    cmd.Parameters.AddWithValue("@VehicleRegNo", lblRegNo.Text.Trim().ToUpper());
                    cmd.Parameters.AddWithValue("@OwnerName", lblOwnerName.Text);
                    cmd.Parameters.AddWithValue("@MobileNo", txtMobileno.Text);
                    cmd.Parameters.AddWithValue("@VehicleClass", ddlVehicleClass.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@OrderType", lblTransactionType.Text);
                    cmd.Parameters.AddWithValue("@StickerMandatory", sticker1);
                    cmd.Parameters.AddWithValue("@isVIP", VIP);
                    cmd.Parameters.AddWithValue("@NetAmount", lblAmount.Text);
                    StrNICVehicleType = ViewState["StrNICVehicleType1"].ToString();
                    if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)"||  StrNICVehicleType == "M-Cycle/Scooter(2WN)")
                    {
                        cmd.Parameters.AddWithValue("@VehicleType", ddlVehicleType.Text.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@VehicleType", lblVehicleType.Text);
                    }
                    
                    cmd.Parameters.AddWithValue("@OrderStatus", "New Order");
                    cmd.Parameters.AddWithValue("@CashReceiptNo", cashrc);
                    cmd.Parameters.AddWithValue("@OwnerFatherName", "");
                    cmd.Parameters.AddWithValue("@EmailID", lblEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@ChassisNo", lblChasisNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@EngineNo", lblEngineNo.Text.Trim());
                    cmd.Parameters.AddWithValue("@frontplatesize", dt.Rows[0]["frontplateID"].ToString());
                    cmd.Parameters.AddWithValue("@rearplatesize", dt.Rows[0]["RearPlateID"].ToString());
                    cmd.Parameters.AddWithValue("@CreatedBy", USERID);
                    cmd.Parameters.AddWithValue("@Reference", remarks.Text);
                    cmd.Parameters.AddWithValue("@ManufacturerModel", lblModelName.Text);
                    cmd.Parameters.AddWithValue("@vehicleref", "New");
                    cmd.Parameters.AddWithValue("@FrontplatePrize", strFrontPrize);
                    cmd.Parameters.AddWithValue("@RearPlatePrize", strRearPrize);
                    cmd.Parameters.AddWithValue("@StickerPrize", dt.Rows[0]["stickercost"].ToString());
                    cmd.Parameters.AddWithValue("@ScrewPrize", dt.Rows[0]["snaplockcost"].ToString());
                    cmd.Parameters.AddWithValue("@TotalAmount", dt.Rows[0]["totalamount"].ToString());
                    cmd.Parameters.AddWithValue("@VAT_Amount", dt.Rows[0]["vatamount"].ToString());
                    cmd.Parameters.AddWithValue("@RoundOff_NetAmount", Math.Round(decimal.Parse(lblAmount.Text), 0));
                    cmd.Parameters.AddWithValue("@VAT_Percentage", dt.Rows[0]["vatper"].ToString());
                    cmd.Parameters.AddWithValue("@remarks", remarks.Text);
                    cmd.Parameters.AddWithValue("@PlateAffixationDate", date1);
                    cmd.Parameters.Add("@Message", SqlDbType.Char, 1);
                    cmd.Parameters["@Message"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    string message = cmd.Parameters["@Message"].Value.ToString();

                    con.Close();
                    if (message == "S")
                    {

                        lblSucMess.Visible = true;
                        lblErrMess.Visible = false;
                        btnDownload.Visible = true;
                        Button3.Visible = true;
                        lblSucMess.Text = "Record Saved Successfully";
                        string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                        Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                        int u = Utils.ExecNonQuery(Query, ConnectionString);
                        Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                        u = Utils.ExecNonQuery(Query, ConnectionString);
                        Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                        u = Utils.ExecNonQuery(Query, ConnectionString);


                        lblSucMess.Text = "Record Saved Successfully..";
                        ddlVehicleClass.SelectedIndex = 0;
                        // refresh();

                        btnDownload.Visible = true;
                       if (txtMobileno.Text.Length == 10)
                       {
             

                        string SMSText = " Cash Rs." + Math.Round(decimal.Parse(lblAmount.Text), 0) + " collected against HSRP For Vehicle No. " + lblRegNo.Text.Trim().ToUpper() + " receipt number " + cashrc + "dated  " + System.DateTime.Now.ToString("dd/MM/yyyy") + ". HSRP Team";
                            
                        string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-ukhsrp&password=ukhsrp&type=0&dlr=1&destination=91" + txtMobileno.Text + "&source=ukhsrp&message=" + SMSText;
                        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                        myRequest.Method = "GET";
                        WebResponse myResponse = myRequest.GetResponse();
                        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                        string result = sr.ReadToEnd();
                        sr.Close();
                        myResponse.Close();
                        System.Threading.Thread.Sleep(350);
                        //sb.Append();

                        Utils.ExecNonQuery("insert into UKSMSDetail(RtoLocationID,VehicleRegNo,MobileNo,SentResponseCode,smstext,smstype) values('" + RTOLocationID + "','" + lblRegNo.Text.Trim().ToUpper() + "'," + txtMobileno.Text + ",'" + result + "','" + SMSText + "','1')", ConnectionString);
                        }

                    }
                    else if (message == "D")
                    {
                        lblSucMess.Visible = false;
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "VehicleRegNo. Already Exist";
                        return;
                    }
                    else
                    {
                        lblSucMess.Visible = false;
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please Contact Head Office";
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
        }
        //public void fillAffixationCenter()
        //{

        //    string sqlquery = "select Rto_Id ,AffixCenterDesc from AffixationCenters where State_Id='" + Session["UserHSRPStateID"].ToString() + "'";
        //    Utils.PopulateDropDownList(ddlaffixation, sqlquery, ConnectionString, "--Select Affixation Center--");
        //}

        public void EpsionPrint()
        {
            String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            DataTable rtoaddr = new DataTable();
            rtoaddr = Utils.GetDataTable("select r.RTOLocationAddress,r.rtolocationname from users as u inner join rtolocation as r on u.RTOLocationID=r.RTOLocationID where userid='" + Session["UID"].ToString() + "'", ConnectionString);
            string RtoLocationName = rtoaddr.Rows[0]["RTOLocationName"].ToString();

            string RTOAddress = rtoaddr.Rows[0]["RTOLocationAddress"].ToString();

            string username = " select UserLoginName from users where UserID='" + USERID + "'";
            DataTable dtusername = Utils.GetDataTable(username, ConnectionString);
            string userloginname = dtusername.Rows[0]["UserLoginName"].ToString();

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

            //sqlquery = "select * from AffixationCenters where State_Id='" + Session["UserHSRPStateID"].ToString() + "' and Rto_Id='" + Session["UserRTOLocationID"].ToString() + "'";
            // sqlquery = "select Address1 from AffixationCenters where Rto_Id='" + ddlaffixation.SelectedValue.ToString() + "'";
            // AffAddress = Utils.getDataSingleValue(sqlquery, ConnectionString, "Address1");

            //SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where vehicleRegNo='" + txtVehicleRegNo.Text.Trim() + "'";
            string SQLString = "select hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType from hsrprecords where  vehicleRegNo ='" + txtRegNo.Text + "' order by  HSRPRecord_CreationDate desc";

           // string SQLString = "select hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType from hsrprecords where  vehicleRegNo ='UK04CA5922' order by  HSRPRecord_CreationDate desc";

           

            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);

            DataProvider.BAL obj = new DataProvider.BAL();
            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {

                string strquery1 = "select convert(varchar(15),PlateAffixationDate,103) as Date from hsrprecords where HSRPRecord_AuthorizationNo='" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'";
                string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");

                //string filename = "CASH RECEIPT-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


                String StringField = String.Empty;
                String StringAlert = String.Empty;


                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();
                float imageWidth = 216;
                float imageHeight = 450;
                document.SetMargins(0, 0, 5, 0);
                document.SetPageSize(new iTextSharp.text.Rectangle(imageWidth, imageHeight));

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath(PdfFolder), FileMode.Create));
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(2);
                PdfPTable table1 = new PdfPTable(2);
                PdfPTable table2 = new PdfPTable(2);
                //actual width of table in points
                //table.TotalWidth = 100f;

                //fix the absolute width of the table

                PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell312.Colspan = 2;
                cell312.BorderColor = BaseColor.WHITE;
                cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table1.AddCell(cell312);

                PdfPCell cell312a = new PdfPCell(new Phrase("Duplicate", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell312a.Colspan = 2;
                cell312a.BorderColor = BaseColor.WHITE;
                cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table2.AddCell(cell312a);

                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 2;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);



                PdfPCell cell1234 = new PdfPCell(new Phrase("RTOLocation  : " + RtoLocationName, new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1234.Colspan = 2;
                cell1234.BorderColor = BaseColor.WHITE;
                cell1234.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1234);



                //string getTinNo1 = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
                //PdfPCell cell12345 = new PdfPCell(new Phrase("TIN NO. " + getTinNo1, new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell12345.Colspan = 1;
                //cell12345.BorderColor = BaseColor.WHITE;
                //cell12345.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell12345);





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

                //PdfPCell cell22 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell22.Colspan = 0;

                //cell22.BorderWidthLeft = 0f;
                //cell22.BorderWidthRight = 0f;
                //cell22.BorderWidthTop = 0f;
                //cell22.BorderWidthBottom = 0f;
                //cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell22);

                //PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell222.Colspan = 0;

                //cell222.BorderWidthLeft = 0f;
                //cell222.BorderWidthRight = 0f;
                //cell222.BorderWidthTop = 0f;
                //cell222.BorderWidthBottom = 0f;
                //cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell222);

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



                PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell9.Colspan = 0;
                cell9.BorderWidthLeft = 0f;
                cell9.BorderWidthRight = 0f;
                cell9.BorderWidthTop = 0f;
                cell9.BorderWidthBottom = 0f;
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell9);

                PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell95.Colspan = 0;
                cell95.BorderWidthLeft = 0f;
                cell95.BorderWidthRight = 0f;
                cell95.BorderWidthTop = 0f;
                cell95.BorderWidthBottom = 0f;
                cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell95);

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

                PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OFF AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
                PdfPCell celldupCash40z1 = new PdfPCell(new Phrase("(The prices are inclusive of Taxes)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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

                ////string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the fourth working day from the date of  issuance of cash receipt.";
                //string Message = "\u2022" + " Vehicle Owner is requested to please check the Correctness of the cash slip.";

                //PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell64.Colspan = 2;
                //cell64.BorderWidthLeft = 0f;
                //cell64.BorderWidthRight = 0f;
                //cell64.BorderWidthTop = 0f;
                //cell64.BorderWidthBottom = 0f;
                //cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell64);


                //string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt. The company shall not be responsible for any clarrical mistake what so ever.";

                //PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell65a.Colspan = 4;
                //cell65a.BorderWidthLeft = 0f;
                //cell65a.BorderWidthRight = 0f;
                //cell65a.BorderWidthTop = 0f;
                //cell65a.BorderWidthBottom = 0f;
                //cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell65a);



                string Message = "\u2022" + " Please verify the details on Cash Receipt, before leaving the counter.There after company is not responsible for any error.";

                PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell64.Colspan = 4;
                cell64.BorderWidthLeft = 0f;
                cell64.BorderWidthRight = 0f;
                cell64.BorderWidthTop = 0f;
                cell64.BorderWidthBottom = 0f;
                cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64);




                string MessageSec = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the sixth working day, at THE BELOW MENTIONED affixation center.";

                PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell65a.Colspan = 4;
                cell65a.BorderWidthLeft = 0f;
                cell65a.BorderWidthRight = 0f;
                cell65a.BorderWidthTop = 0f;
                cell65a.BorderWidthBottom = 0f;
                cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65a);



                string MessageSecnNew = "\u2022" + "Mobile number of VEHICLE OWNER is required for the SMS alert that ``Your Plate is ready for affixation``";

                PdfPCell cellmsg = new PdfPCell(new Phrase(MessageSecnNew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellmsg.Colspan = 4;
                cellmsg.BorderWidthLeft = 0f;
                cellmsg.BorderWidthRight = 0f;
                cellmsg.BorderWidthTop = 0f;
                cellmsg.BorderWidthBottom = 0f;
                cellmsg.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellmsg);



                string MessageSecNew1 = "\u2022" + " For any Suggestion/Complaints/Query kindly visit www.hsrpuk.com OR toll free number 1800-2121-265.";

                PdfPCell cellmsg1 = new PdfPCell(new Phrase(MessageSecNew1, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellmsg1.Colspan = 4;
                cellmsg1.BorderWidthLeft = 0f;
                cellmsg1.BorderWidthRight = 0f;
                cellmsg1.BorderWidthTop = 0f;
                cellmsg1.BorderWidthBottom = 0f;
                cellmsg1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellmsg1);



                string space = ",";

                PdfPCell cell631 = new PdfPCell(new Phrase("Affixation Center : " + RtoLocationName, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell631.Colspan = 4;
                cell631.BorderWidthLeft = 0f;
                cell631.BorderWidthRight = 0f;
                cell631.BorderWidthTop = 0f;
                cell631.BorderWidthBottom = 0f;
                cell631.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell631);



                PdfPCell cellESPN= new PdfPCell(new Phrase("\u2022" + "Corporate office : " + "Dehradun," + RTOAddress, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellESPN.Colspan = 4;
                cellESPN.BorderWidthLeft = 0f;
                cellESPN.BorderWidthRight = 0f;
                cellESPN.BorderWidthTop = 0f;
                cellESPN.BorderWidthBottom = 0f;
                cellESPN.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellESPN);


                PdfPCell cellofficeESPN = new PdfPCell(new Phrase("\u2022" + "Registered office : " + " B-84, Phase – I, Mayapuri Industrial Area, New Delhi - 110064 ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellofficeESPN.Colspan = 4;
                cellofficeESPN.BorderWidthLeft = 0f;
                cellofficeESPN.BorderWidthRight = 0f;
                cellofficeESPN.BorderWidthTop = 0f;
                cellofficeESPN.BorderWidthBottom = 0f;
                cellofficeESPN.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellofficeESPN);






                PdfPCell cell63 = new PdfPCell(new Phrase("FOR" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH.SIGH.))", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 2;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell62);




                //PdfPCell cellname = new PdfPCell(new Phrase("CASHIER-(uknikhil/R300)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cellname.Colspan = 2;
                //cellname.BorderWidthLeft = 0f;
                //cellname.BorderWidthRight = 0f;
                //cellname.BorderWidthTop = 0f;
                //cellname.BorderWidthBottom = 0f;
                //cellname.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cellname);






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

                document.Add(table2);
                document.Add(table);

                document.Close();
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
        }

        protected void ddlVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            
         
            String SqlQuery = "SELECT top(1) HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,OwnerName,Address1,MobileNo,EmailID,OrderType,VehicleClass,NICvehicletype,ManufacturerName,ManufacturerModel,ChassisNo,EngineNo,VehicleRegNo FROM HSRPRecordsStaggingArea where VehicleRegNo='" + txtRegNo.Text + "' and HSRP_StateID='" + HSRPStateID + "' order by  HSRPRecord_CreationDate desc";
            dt = Utils.GetDataTable(SqlQuery, ConnectionString);
            StrNICVehicleType = dt.Rows[0]["NICvehicletype"].ToString().Split('*')[0];
            //string ddlvaluetype = this.ViewState["lstCustomers_SelectedValue"].ToString();
            if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
            {
               // FillVehicleType();
                ddlVehicleType.Visible = true;
                string SQLString3 = "select dbo.hsrpplateamt ('" + HSRPStateID + "','" + ddlVehicleType.SelectedItem.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + lblTransactionType.Text + "') as Amount";
                DataTable dt2 = Utils.GetDataTable(SQLString3, ConnectionString);
                lblAmount.Text = dt2.Rows[0]["Amount"].ToString();

            }
            else
            {
                string SQLString2 = "select dbo.hsrpplateamt ('" + HSRPStateID + "','" + lblVehicleType.Text + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + lblTransactionType.Text + "') as Amount";
                DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                lblAmount.Text = dt1.Rows[0]["Amount"].ToString();
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
           // EpsionPrint();



            DataTable GetAddress;
            string Address;
            string TIN;

            DataTable rtoaddr = new DataTable();
            rtoaddr = Utils.GetDataTable("select r.RTOLocationAddress,r.rtolocationname from users as u inner join rtolocation as r on u.RTOLocationID=r.RTOLocationID where userid='" + Session["UID"].ToString() + "'", ConnectionString);


            //  string sqlquery = "select Address1 from AffixationCenters where Rto_Id='" + ddlaffixation.SelectedValue.ToString() + "'";
            //string  AffAddress = Utils.getDataSingleValue(sqlquery, ConnectionString, "Address1");


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

            string SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where  VehicleRegNo ='" + txtRegNo.Text + "'";
            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);



           // string rtoid = "select r.rtolocationname  as RtoLocation,r.rtolocationid  from hsrprecords as h inner join rtolocation as r on r.rtolocationid=h.rtolocationid where userid='" + Session["UID"].ToString() + "'";
//DataTable Rtlocation = Utils.GetDataTable(rtoid, ConnectionString);
            string RtoLocationName = rtoaddr.Rows[0]["RTOLocationName"].ToString();
            string RTOAddress = rtoaddr.Rows[0]["RTOLocationAddress"].ToString();
           // string RtoLocationID = Rtlocation.Rows[0]["RTOLocationID"].ToString();

            // string AffixactionCenter = "select * from AffixationCenters";

            string username = " select UserLoginName from users where UserID='" + USERID + "'";
            DataTable dtusername = Utils.GetDataTable(username, ConnectionString);
            string userloginname = dtusername.Rows[0]["UserLoginName"].ToString();


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
                table.TotalWidth =650f;

                PdfPTable table1 = new PdfPTable(4);
                //actual width of table in points
                table1.TotalWidth = 600f;

                PdfPTable table2 = new PdfPTable(4);
                //actual width of table in points
                table2.TotalWidth = 600f;

                PdfPTable table3 = new PdfPTable(4);
                //actual width of table in points
                table3.TotalWidth = 600f;

                PdfPTable table4 = new PdfPTable(4);
                //actual width of table in points
                table4.TotalWidth = 600f;

                PdfPTable table5 = new PdfPTable(4);
                //actual width of table in points
                table5.TotalWidth = 600f;



                PdfPTable table6 = new PdfPTable(4);
                //actual width of table in points
                table6.TotalWidth = 600f;



                PdfPTable table7 = new PdfPTable(4);
                //actual width of table in points
                table7.TotalWidth = 585f;

                //fix the absolute width of the table


                //PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell312.Colspan = 4;
                //cell312.BorderColor = BaseColor.WHITE;
                //cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table1.AddCell(cell312);






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





                //PdfPCell cellrtoname = new PdfPCell(new Phrase("RTO Location :" + RtoLocationName, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cellrtoname.Colspan = 4;
                //cellrtoname.BorderColor = BaseColor.WHITE;
                //cellrtoname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table6.AddCell(cellrtoname);



                PdfPCell cellrtoname = new PdfPCell(new Phrase("RTO Location : " + RtoLocationName, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellrtoname.Colspan = 4;

                cellrtoname.BorderWidthLeft = 0f;
                cellrtoname.BorderWidthRight = 0f;
                cellrtoname.BorderWidthTop = 0f;
                cellrtoname.BorderWidthBottom = 0f;
                cellrtoname.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellrtoname);



                PdfPCell cellTIN_No = new PdfPCell(new Phrase("TIN NO :" + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellTIN_No.Colspan = 4;

                cellTIN_No.BorderWidthLeft = 0f;
                cellTIN_No.BorderWidthRight = 0f;
                cellTIN_No.BorderWidthTop = 0f;
                cellTIN_No.BorderWidthBottom = 0f;
                cellTIN_No.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellTIN_No);

                // = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

                ////PdfPCell cellTIN = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                ////cellTIN.Colspan = 4;
                ////cellTIN.BorderWidthLeft = 0f;
                ////cellTIN.BorderWidthRight = 0f;
                ////cellTIN.BorderWidthTop = 0f;
                ////cellTIN.BorderWidthBottom = 0f;
                ////cellTIN.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                ////table6.AddCell(cellTIN);

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

                //PdfPCell cell21 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell21.Colspan = 1;

                //cell21.BorderWidthLeft = 0f;
                //cell21.BorderWidthRight = 0f;
                //cell21.BorderWidthTop = 0f;
                //cell21.BorderWidthBottom = 0f;
                //cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell21);
                //string CashReceiptDateTime = string.Empty;

                //if (dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString() == "")
                //{
                //    CashReceiptDateTime = DateTime.Now.ToString("dd/MM/yyyy");
                //}
                //else
                //{
                //    CashReceiptDateTime = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("dd/MM/yyyy");
                //}
                //PdfPCell cell212 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell212.Colspan = 1;

                //cell212.BorderWidthLeft = 0f;
                //cell212.BorderWidthRight = 0f;
                //cell212.BorderWidthTop = 0f;
                //cell212.BorderWidthBottom = 0f;
                //cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell212);



                //PdfPCell cell2 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell2.Colspan = 1;

                //cell2.BorderWidthLeft = 0f;
                //cell2.BorderWidthRight = 0f;
                //cell2.BorderWidthTop = 0f;
                //cell2.BorderWidthBottom = 0f;
                //cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell2);

                //// = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

                //PdfPCell cell22111 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell22111.Colspan = 1;
                //cell22111.BorderWidthLeft = 0f;
                //cell22111.BorderWidthRight = 0f;
                //cell22111.BorderWidthTop = 0f;
                //cell22111.BorderWidthBottom = 0f;
                //cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell22111);




                PdfPCell cell22 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22.Colspan = 1;

                cell22.BorderWidthLeft = 0f;
                cell22.BorderWidthRight = 0f;
                cell22.BorderWidthTop = 0f;
                cell22.BorderWidthBottom = 0f;
                cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22);

                PdfPCell cell222 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell222.Colspan = 1;

                cell222.BorderWidthLeft = 0f;
                cell222.BorderWidthRight = 0f;
                cell222.BorderWidthTop = 0f;
                cell222.BorderWidthBottom = 0f;
                cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell222);

                //string getExciseNo1 = Utils.getScalarValue("select ExciseNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
                //PdfPCell cell221 = new PdfPCell(new Phrase("EXCISE NO ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                // cell221.Colspan = 1;
                // cell221.BorderWidthLeft = 0f;
                // cell221.BorderWidthRight = 0f;
                // cell221.BorderWidthTop = 0f;
                // cell221.BorderWidthBottom = 0f;
                // cell221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // table.AddCell(cell221);

                // PdfPCell cell2221 = new PdfPCell(new Phrase(": " + getExciseNo1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                // cell2221.Colspan = 3;

                // cell2221.BorderWidthLeft = 0f;
                // cell2221.BorderWidthRight = 0f;
                // cell2221.BorderWidthTop = 0f;
                // cell2221.BorderWidthBottom = 0f;
                // cell2221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                // table.AddCell(cell2221);

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



                //PdfPCell cell8 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell8.Colspan = 1;
                //cell8.BorderWidthLeft = 0f;
                //cell8.BorderWidthRight = 0f;
                //cell8.BorderWidthTop = 0f;
                //cell8.BorderWidthBottom = 0f;
                //cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell8);

                //PdfPCell cell85 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell85.Colspan = 3;
                //cell85.BorderWidthLeft = 0f;
                //cell85.BorderWidthRight = 0f;
                //cell85.BorderWidthTop = 0f;
                //cell85.BorderWidthBottom = 0f;
                //cell85.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell85);

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




                ////PdfPCell cellbb = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                ////cellbb.Colspan = 4;
                ////cellbb.BorderWidthLeft = 0f;
                ////cellbb.BorderWidthRight = 0f;
                ////cellbb.BorderWidthTop = 0f;
                ////cellbb.BorderWidthBottom = 0f;
                ////cellbb.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                ////table.AddCell(cellbb);






                PdfPCell cellnameofcashier = new PdfPCell(new Phrase("Name of Cashier ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellnameofcashier.Colspan = 1;
                cellnameofcashier.BorderWidthLeft = 0f;
                cellnameofcashier.BorderWidthRight = 0f;
                cellnameofcashier.BorderWidthTop = 0f;
                cellnameofcashier.BorderWidthBottom = 0f;
                cellnameofcashier.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellnameofcashier);

                PdfPCell cell951 = new PdfPCell(new Phrase(": " + "uknikhil", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell951.Colspan = 1;
                cell951.BorderWidthLeft = 0f;
                cell951.BorderWidthRight = 0f;
                cell951.BorderWidthTop = 0f;
                cell951.BorderWidthBottom = 0f;
                cell951.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell951);




                PdfPCell cell109 = new PdfPCell(new Phrase("ID of Cashier ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell109.Colspan = 1;
                cell109.BorderWidthLeft = 0f;
                cell109.BorderWidthRight = 0f;
                cell109.BorderWidthTop = 0f;
                cell109.BorderWidthBottom = 0f;
                cell109.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell109);

                PdfPCell cell1051 = new PdfPCell(new Phrase(": " + "R300", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1051.Colspan = 1;
                cell1051.BorderWidthLeft = 0f;
                cell1051.BorderWidthRight = 0f;
                cell1051.BorderWidthTop = 0f;
                cell1051.BorderWidthBottom = 0f;
                cell1051.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1051);




                //PdfPCell cellnameofcashier = new PdfPCell(new Phrase("Name of Cashier = " + "(uknikhil)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cellnameofcashier.Colspan = 4;
                //cellnameofcashier.BorderWidthLeft = 0f;
                //cellnameofcashier.BorderWidthRight = 0f;
                //cellnameofcashier.BorderWidthTop = 0f;
                //cellnameofcashier.BorderWidthBottom = 0f;
                //cellnameofcashier.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cellnameofcashier);





                ////PdfPCell cellbbb = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                ////cellbbb.Colspan = 4;
                ////cellbbb.BorderWidthLeft = 0f;
                ////cellbbb.BorderWidthRight = 0f;
                ////cellbbb.BorderWidthTop = 0f;
                ////cellbbb.BorderWidthBottom = 0f;
                ////cellbbb.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                ////table.AddCell(cellbbb);








                //PdfPCell cellid = new PdfPCell(new Phrase("ID of Cashier = " + "(R300)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cellid.Colspan = 4;
                //cellid.BorderWidthLeft = 0f;
                //cellid.BorderWidthRight = 0f;
                //cellid.BorderWidthTop = 0f;
                //cellid.BorderWidthBottom = 0f;
                //cellid.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                ////table.AddCell(cellid);




                //PdfPCell cell11 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell11.Colspan = 1;
                //cell11.BorderWidthLeft = 0f;
                //cell11.BorderWidthRight = 0f;
                //cell11.BorderWidthTop = 0f;
                //cell11.BorderWidthBottom = 0f;
                //cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell11);

                //PdfPCell cell115 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell115.Colspan = 1;
                //cell115.BorderWidthLeft = 0f;
                //cell115.BorderWidthRight = 0f;
                //cell115.BorderWidthTop = 0f;
                //cell115.BorderWidthBottom = 0f;
                //cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell115);

                //PdfPCell cell1113 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell1113.Colspan = 1;
                //cell1113.BorderWidthLeft = 0f;
                //cell1113.BorderWidthRight = 0f;
                //cell1113.BorderWidthTop = 0f;
                //cell1113.BorderWidthBottom = 0f;
                //cell1113.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1113);

                //PdfPCell cell11135 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell11135.Colspan = 1;
                //cell11135.BorderWidthLeft = 0f;
                //cell11135.BorderWidthRight = 0f;
                //cell11135.BorderWidthTop = 0f;
                //cell11135.BorderWidthBottom = 0f;
                //cell11135.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell11135);

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


                // decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                // roundAmt = Math.Round(roundAmt, 0);

                PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OFF AMOUNT ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupCash401.Colspan = 1;
                celldupCash401.BorderWidthLeft = 0f;
                celldupCash401.BorderWidthRight = 0f;
                celldupCash401.BorderWidthTop = 0f;
                celldupCash401.BorderWidthBottom = 0f;
                celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash401);




                PdfPCell celldupRouCash4021 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash4021.Colspan = 1;
                celldupRouCash4021.BorderWidthLeft = 0f;
                celldupRouCash4021.BorderWidthRight = 0f;
                celldupRouCash4021.BorderWidthTop = 0f;
                celldupRouCash4021.BorderWidthBottom = 0f;
                celldupRouCash4021.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash4021);




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





                PdfPCell celldupRouCash40213 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash40213.Colspan = 1;
                celldupRouCash40213.BorderWidthLeft = 0f;
                celldupRouCash40213.BorderWidthRight = 0f;
                celldupRouCash40213.BorderWidthTop = 0f;
                celldupRouCash40213.BorderWidthBottom = 0f;
                celldupRouCash40213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash40213);





                

                PdfPCell celldupRouCash4022 = new PdfPCell(new Phrase("(The prices are inclusive of Taxes.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash4022.Colspan = 2;
                celldupRouCash4022.BorderWidthLeft = 0f;
                celldupRouCash4022.BorderWidthRight = 0f;
                celldupRouCash4022.BorderWidthTop = 0f;
                celldupRouCash4022.BorderWidthBottom = 0f;
                celldupRouCash4022.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash4022);






                
                PdfPCell celldupRouCash402134 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash402134.Colspan = 1;
                celldupRouCash402134.BorderWidthLeft = 0f;
                celldupRouCash402134.BorderWidthRight = 0f;
                celldupRouCash402134.BorderWidthTop = 0f;
                celldupRouCash402134.BorderWidthBottom = 0f;
                celldupRouCash402134.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash402134);








                //-----------------------------


                //-----------------------------

                PdfPCell celldupRouCash40211 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash40211.Colspan = 4;
                celldupRouCash40211.BorderWidthLeft = 0f;
                celldupRouCash40211.BorderWidthRight = 0f;
                celldupRouCash40211.BorderWidthTop = 0f;
                celldupRouCash40211.BorderWidthBottom = 0f;
                celldupRouCash40211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash40211);




                PdfPCell celldupRouCash402112 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash402112.Colspan = 4;
                celldupRouCash402112.BorderWidthLeft = 0f;
                celldupRouCash402112.BorderWidthRight = 0f;
                celldupRouCash402112.BorderWidthTop = 0f;
                celldupRouCash402112.BorderWidthBottom = 0f;
                celldupRouCash402112.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash402112);



                //PdfPCell celldupRouCash4021123 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //celldupRouCash4021123.Colspan = 4;
                //celldupRouCash4021123.BorderWidthLeft = 0f;
                //celldupRouCash4021123.BorderWidthRight = 0f;
                //celldupRouCash4021123.BorderWidthTop = 0f;
                //celldupRouCash4021123.BorderWidthBottom = 0f;
                //celldupRouCash4021123.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(celldupRouCash4021123);

                string Message = "\u2022" + " Please verify the details on Cash Receipt, before leaving the counter.There after company is not responsible for any error.";

                PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell64.Colspan = 4;
                cell64.BorderWidthLeft = 0f;
                cell64.BorderWidthRight = 0f;
                cell64.BorderWidthTop = 0f;
                cell64.BorderWidthBottom = 0f;
                cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64);



                string MessageSec = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP, on the sixth working days THE BELOW MENTIONED affixation center.";

                PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell65a.Colspan = 4;
                cell65a.BorderWidthLeft = 0f;
                cell65a.BorderWidthRight = 0f;
                cell65a.BorderWidthTop = 0f;
                cell65a.BorderWidthBottom = 0f;
                cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65a);


                string MessageSecnNew = "\u2022" + "Mobile number of VEHICLE OWNER is required for the SMS alert that `` Your Plate is ready for affixation.``";

                PdfPCell cellmsg = new PdfPCell(new Phrase(MessageSecnNew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellmsg.Colspan = 4;
                cellmsg.BorderWidthLeft = 0f;
                cellmsg.BorderWidthRight = 0f;
                cellmsg.BorderWidthTop = 0f;
                cellmsg.BorderWidthBottom = 0f;
                cellmsg.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellmsg);


                string MessageSecNew1 = "\u2022" + " For any Suggestion/Complaints/Query kindly visit www.hsrpuk.com or toll free number 1800-2121-265.";

                PdfPCell cellmsg1 = new PdfPCell(new Phrase(MessageSecNew1, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cellmsg1.Colspan = 4;
                cellmsg1.BorderWidthLeft = 0f;
                cellmsg1.BorderWidthRight = 0f;
                cellmsg1.BorderWidthTop = 0f;
                cellmsg1.BorderWidthBottom = 0f;
                cellmsg1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellmsg1);




                string MessageSec11 = "\u2022" + "Affixation Center :" + RtoLocationName;
                //+ RTOAddress

                PdfPCell cell631 = new PdfPCell(new Phrase(MessageSec11, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell631.Colspan = 4;
                cell631.BorderWidthLeft = 0f;
                cell631.BorderWidthRight = 0f;
                cell631.BorderWidthTop = 0f;
                cell631.BorderWidthBottom = 0f;
                cell631.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell631);




                PdfPCell cell62 = new PdfPCell(new Phrase("\u2022"+"Corporate office = " + "Dehradun," + RTOAddress, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 4;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell62);


                PdfPCell celloffice = new PdfPCell(new Phrase("\u2022" + "Registered  office =" + " Mayapuri Delhi", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celloffice.Colspan = 4;
                celloffice.BorderWidthLeft = 0f;
                celloffice.BorderWidthRight = 0f;
                celloffice.BorderWidthTop = 0f;
                celloffice.BorderWidthBottom = 0f;
                celloffice.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celloffice);
               


                PdfPCell cell63 = new PdfPCell(new Phrase("FOR " + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell63.Colspan = 4;
                cell63.BorderWidthLeft = 0f;
                cell63.BorderWidthRight = 0f;
                cell63.BorderWidthTop = 0f;
                cell63.BorderWidthBottom = 0f;
                cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell63);

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






                PdfPCell cellname = new PdfPCell(new Phrase("Cashier-(uknikhil/R300)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellname.Colspan = 4;
                cellname.BorderWidthLeft = 0f;
                cellname.BorderWidthRight = 0f;
                cellname.BorderWidthTop = 0f;
                cellname.BorderWidthBottom = 0f;
                cellname.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table5.AddCell(cellname);

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
                document.Add(table6);
                document.Add(table);
                
                document.Add(table3);
                document.Add(table5);


                document.Add(table2);
                document.Add(table);
                document.Add(table6);
                
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

        protected void ddlVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillVehicleType();
            ddlVehicleType.Visible = true;
        }

        //protected void ddlVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillVehicleType();
        //}

      
    }
}