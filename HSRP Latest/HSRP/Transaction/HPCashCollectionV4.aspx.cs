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
    public partial class HPCashCollectionV4 : System.Web.UI.Page
    {
        public static string AddRecordBy = "";
        public static string CounterNo = "";
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        SqlConnection con = new SqlConnection(ConnectionString);
        string StrNICVehicleType = string.Empty;
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
            ddlVehicleType.Visible = false;
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            USERID = Session["UID"].ToString();
            macbase = Session["MacAddress"].ToString();

            string SqlQuery = "exec GetTodayCollectionUserWise '" + HSRPStateID + "','" + RTOLocationID + "','" + USERID + "','" + System.DateTime.Today.ToString("yyyy-MM-dd") + "'";
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
                lblLastAmont.Visible = true;
                lblLastAmont.Text = "0";

            }
            else
            {
                lblLastAmont.Visible = true;
                lblLastAmont.Text = strDepositAmount;
            }
           
        }
     

        protected void btnGo_Click(object sender, EventArgs e)
        {


            lblErrMess.Text = string.Empty;
            string RegistrationDate = string.Empty;
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
            string StrNICVehicleType1 = string.Empty;

            Button3.Visible = false;
           // btnDownload.Visible = false;

            SQLString = "select Rtolocationname from rtolocation where rtolocationid='" + RTOLocationID + "'";
            RTOlocationName = Utils.getScalarValue(SQLString, ConnectionString);
            try
            {
                string strVehicleNo = txtRegNo.Text.Trim();
                string strCheck = strVehicleNo.Substring(0, 2);
              
                if (strCheck.ToUpper() != "HP")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                    return;
                }             
                

                bln3rdSticker.Checked = false;
              
                        
                String SqlQuery = "SELECT top(1)  HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,OwnerName,Address1,MobileNo,EmailID,OrderType,VehicleClass,NICvehicletype,ManufacturerName,ManufacturerModel,ChassisNo,EngineNo,VehicleRegNo FROM HSRPRecordsStaggingArea where VehicleRegNo='" + txtRegNo.Text + "' and HSRP_StateID='" + HSRPStateID + "' order by  HSRPRecord_CreationDate desc";
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

                    StrNICVehicleType1 = dt.Rows[0]["NICvehicletype"].ToString().Split('*')[0];

                    ViewState["StrNICVehicleType1"] = StrNICVehicleType1;
                    StrNICVehicleType = ViewState["StrNICVehicleType1"].ToString();

                    //if (StrNICVehicleType1.ToUpper().Trim() == "E-RICKSHAW(P)(3WT)" || StrNICVehicleType1.Trim() == "e-Rickshaw(P)(3WT)")
                    //{
                    //    lblVehicleType.Visible = false;
                    //    lblVehicleTypeERicksha.Text = "E-RICKSHAW";
                    //    lblVehicleTypeERicksha.Visible = true;
                    //    ddlVehicleType.Visible = false;

                    //}
                    //else
                    //{
                    //    lblVehicleType.Visible = true;
                    //    lblVehicleTypeERicksha.Visible = false;
                    //}


                    if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
                    {
                        ddlVehicleType.SelectedItem.Text = "SCOOTER";
                        ddlVehicleType.Visible = true;
                        lblVehicleType.Visible = false;

                    }
                    else
                    {
                        ddlVehicleType.Visible = false;
                        lblVehicleType.Visible = true;
                    }


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

                    lblAuthNo.Text = strAuthno;

                    lblRTOName.Text = StrRtoName;
                    lblEmail.Text = stremail;
                    lblOwnerName.Text = StrOwnerName;
                    lblAddress.Text = StrOwnerAddress;
                    lblVehicleType.Text = StrVehicleType;
                    ddlOrderType.SelectedValue = StrTransactonType.ToString().ToUpper();
                    ddlVehicleClass.SelectedItem.Text = StrVehicleClassType;
                    lblAuthDate.Text = StrAuthdate;
                    txtMobileno.Text = StrMobileNo;

                    lblMfgName.Text = StrManufacturarName;
                    lblModelName.Text = StrModelName;
                    lblRegNo.Text = StrRegistrationNo;
                    lblEngineNo.Text = StrEngineNo;
                    lblChasisNo.Text = StrChasisNo;

                    if (StrTransactonType.ToString().ToUpper() == "NB")
                    {
                        string sqlquery = "select count (vehicleregno)as no from hsrprecords where  hsrp_stateid = 3 and  LTRIM(RTRIM(vehicleregno))= '" + txtRegNo.Text.ToString().Trim() + "'  ";
                        int no = Utils.getScalarCount(sqlquery, ConnectionString);

                        if (no > 0)
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "";
                            lblErrMess.Text = "Duplicate Vehicle Reg No .";
                            return;

                        }

                    }
                    else
                    {
                        string sqlquery = "select count (vehicleregno)as no from hsrprecords where  hsrp_stateid = 3 and  LTRIM(RTRIM(vehicleregno))= '" + txtRegNo.Text.ToString().Trim() + "' and orderstatus='closed' ";
                        int no = Utils.getScalarCount(sqlquery, ConnectionString);
                        if (no < 0)
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "";
                            lblErrMess.Text = " Vehicle Reg No Not Exist .";
                            return;

                        }
                    }

                    string SQLString2 = string.Empty;
                 
                    if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
                    {
                        SQLString2 = "exec [getPlatesData] '" + HSRPStateID + "','" + ddlVehicleType.SelectedItem.Text.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "', '" + ddlOrderType.SelectedValue.ToString() + "'";
                        DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                        lblAmount.Text = dt1.Rows[0]["netamount"].ToString();
                        lblAmount.Text = Convert.ToString(Math.Round(decimal.Parse(lblAmount.Text), 0));


                    }
                    else
                    {
                        SQLString2 = "exec [getPlatesData] '" + HSRPStateID + "','" + lblVehicleType.Text.Trim() + "','" + ddlVehicleClass.SelectedItem.ToString() + "', '" + ddlOrderType.SelectedValue.ToString() + "'";
                        DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                        lblAmount.Text = dt1.Rows[0]["netamount"].ToString();
                        lblAmount.Text = Convert.ToString(Math.Round(decimal.Parse(lblAmount.Text), 0));


                    }
                    


                    if ((StrVehicleType == "MCV/HCV/TRAILERS") || (StrVehicleType == "LMV(CLASS)") || (StrVehicleType == "LMV") || (StrVehicleType == "THREE WHEELER"))
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
                   
                    txtRegNo.ReadOnly = true;

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
                lblErrMess.Text = "Please check your vehicleregno ";

            }
        }


        public void refresh()
        {
            txtRegNo.ReadOnly = false;
            txtRegNo.Text = "";
            lblAuthNo.Text = "";
            lblRTOName.Text = "";
            lblEmail.Text = "";
            lblOwnerName.Text = "";
            lblAddress.Text = "";
            lblVehicleType.Text = "";
            ddlOrderType.SelectedValue = "-Select Order Type-";
            lblAuthDate.Text = "";
            txtMobileno.Text = "";
            ddlVehicleClass.Text="--Select Vehicle Class--";
            ddlVehicleType.Text = "--Select Vehicle Type--";
            lblMfgName.Text = "";
            lblModelName.Text = "";
            lblRegNo.Text = "";
            lblEngineNo.Text = "";
            lblChasisNo.Text = "";
            lblAmount.Text = "";
            lblErrMess.Text = "";
            lblSucMess.Text = "";
           
            

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

            string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);
            
            string SQLString = " select top 1  hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType from hsrprecords where  vehicleRegNo ='" + txtRegNo.Text.Trim().ToString() + "' order by  HSRPRecord_CreationDate desc";
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

                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 4;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

               

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

                PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss tt"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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


                string Message = "\u2022" + " Please verify the details on Cash Receipt, before leaving the counter.";

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

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 4;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table5.AddCell(cell62);

               
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

        #region MobileNo Check Validation
        public enum MobileNoCheck : long
        {
            Zero = 0000000000,
            One = 1111111111,
            Two = 2222222222,
            Three = 3333333333,
            Four = 4444444444,
            Five = 5555555555,
            Six = 6666666666,
            Seven = 7777777777,
            Eight = 8888888888,
            Nine = 9999999999
        }
        #endregion


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
               if (string.IsNullOrEmpty(USERID) || USERID == "0" || Convert.ToInt32(USERID) == 0)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "No Collection Allowed By This User .";
                    return;
                }


                if (RTOLocationID == "0" || string.IsNullOrEmpty(RTOLocationID) || Convert.ToInt32(RTOLocationID) == 0)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "No Collection Allowed By This User .";
                    return;
                }



                string ToDay = System.DateTime.Today.DayOfWeek.ToString();
                if (ToDay == "Sunday")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "No Collection Allowed On Sunday";
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



                if (txtMobileno.Text.Trim() == "" || txtMobileno.Text.Trim() == null || (txtMobileno.Text.Trim().Length < 10) || (txtMobileno.Text.Trim().Length > 10))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Valid Mobile No.";
                    return;
                }

                try
                {
                    long temp = Convert.ToInt64(txtMobileno.Text);
                }
                catch (Exception ex)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Valid Mobile No.";
                    return;
                }


                double dmobile = unchecked(Convert.ToDouble(txtMobileno.Text));
                bool checkValidation = Enum.IsDefined(typeof(MobileNoCheck), (long)Math.Round(dmobile));
                if (checkValidation)
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
                string strVehicleNo = txtRegNo.Text.Trim();
                string strCheck = strVehicleNo.Substring(0, 2);
                string strRTOLocationCodeCheck = strVehicleNo.Substring(0, 4);

                if (strCheck.ToUpper() != "HP")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                    return;
                }
                if (ddlOrderType.SelectedValue.ToString() == "-Select Order Type-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Select Order Type.";
                    return;
 
                }
                if (ddlOrderType.SelectedValue.ToString().ToUpper().Trim() == "NB")
                {
                    string sqlquery = "select count (vehicleregno)as no from hsrprecords where  hsrp_stateid = 3 and  LTRIM(RTRIM(vehicleregno))= '" + txtRegNo.Text.ToString().Trim() + "' ";
                    int no = Utils.getScalarCount(sqlquery, ConnectionString);

                    if (no > 0)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = "Duplicate Vehiicle Reg No.";
                        return;

                    }

                }
                else
                {
                    string sqlquery = "select count (vehicleregno)as no from hsrprecords where  hsrp_stateid = 3 and  LTRIM(RTRIM(vehicleregno))= '" + txtRegNo.Text.ToString().Trim() + "' and orderstatus='Closed' ";
                    int no = Utils.getScalarCount(sqlquery, ConnectionString);

                    if (no < 0)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = "Vehicle Reg No Not Exist ";
                        return;

                    }


                }

                string strtxtVehRegNo = txtRegNo.Text.ToString().Trim();
                char[] chararrinput = strtxtVehRegNo.ToCharArray();

                for (int k = 0; k < chararrinput.Length; k++)
                {
                    if (chararrinput[k].ToString() == "." || chararrinput[k].ToString() == "|" || chararrinput[k].ToString() == ">" || chararrinput[k].ToString() == "<" || chararrinput[k].ToString() == "!" || chararrinput[k].ToString() == "@" || chararrinput[k].ToString() == "," || chararrinput[k].ToString() == "#" || chararrinput[k].ToString() == "*" || chararrinput[k].ToString() == "%" || chararrinput[k].ToString() == "&" || chararrinput[k].ToString() == "~" || chararrinput[k].ToString() == "-" || chararrinput[k].ToString() == "_" || chararrinput[k].ToString() == "(" || chararrinput[k].ToString() == ")" || chararrinput[k].ToString() == "`" || chararrinput[k].ToString() == " " || chararrinput[k].ToString() == "=")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = "Special Character Not Allow In Vehicle Registration No.";
                        return;

                    }

                }
               
                if (lblVehicleType.Text.ToString().Trim() == "LMV" || lblVehicleType.Text.ToString().Trim() == "L.M.V. (CAR)" || lblVehicleType.Text.ToString().Trim() == "LMV(CLASS)" || lblVehicleType.Text.ToString().Trim() == "MCV/HCV/TRAILERS" || lblVehicleType.Text.ToString().Trim() == "THREE WHEELER")
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
               
                StrNICVehicleType = ViewState["StrNICVehicleType1"].ToString();

                if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
                {

                    ddlVehicleType.Visible = true;

                    if (ddlVehicleType.SelectedItem.Text== "--Select Vehicle Type--")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = " Please Select VehicleType.";
                        ddlVehicleType.Visible = true;
                        return;
                    }                                     


                }                        
                


                string sql = string.Empty;

                if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
                {
                 
                    sql = "exec [getPlatesData] '" + HSRPStateID + "','" + ddlVehicleType.SelectedItem.Text.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "', '" + ddlOrderType.SelectedValue.ToString() + "'";
                }
                else
                {
                    sql = "exec [getPlatesData] '" + HSRPStateID + "','" + lblVehicleType.Text + "','" + ddlVehicleClass.SelectedItem.ToString() + "', '" + ddlOrderType.SelectedValue.ToString() + "'";
                }


                DataTable dt = Utils.GetDataTable(sql, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    lblSucMess.Visible = false;
                    if (dt.Rows[0]["totalamount"] == "0" || dt.Rows[0]["totalamount"]=="")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "";
                        lblErrMess.Text = " Please Select VehicleType.";
                        ddlVehicleType.Visible = true;
                        return;
                    }


                }

                else 
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = " Please  Contact To Administrator.";
                     return;
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

                Invoice = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                Invoice = Utils.getScalarValue(Invoice, ConnectionString);
                DC = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as Receiptno from prefix  where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                DC = Utils.getScalarValue(DC, ConnectionString);
                lblAddress.Text = lblAddress.Text.Replace("'", "").Trim();

                

                String strquery1 = "select [dbo].GetAffxDate_insert_new('" + HSRPStateID + "') as Date";
                string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");

                if (!string.IsNullOrEmpty(lblAmount.Text))
                {
                    decimal dd = decimal.Parse(lblAmount.Text);
                }
                else
                {
                    lblAmount.Text = "0";
                    lblErrMess.Visible = true;
                    lblErrMess.Text = " Please Contact to Administrator.";

                    return;
                }

                con.Open();
                String strFrontPrize = "0.00";
                String strRearPrize = "0.00";



                if (!string.IsNullOrEmpty(dt.Rows[0]["FrontPlateCost"].ToString().Trim()))
                {
                    strFrontPrize = dt.Rows[0]["FrontPlateCost"].ToString();
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["RearPlateCost"].ToString().Trim()))
                {
                    strRearPrize = dt.Rows[0]["RearPlateCost"].ToString();
                }

                if (Math.Round(decimal.Parse(lblAmount.Text), 0) == 0)
                {
                    lblErrMess.Text = " Please Contact to Administrator.";
                    lblErrMess.Visible = true;
                    return;
                }


                SqlCommand cmd = new SqlCommand("New_InsertDataHP_V4", con);
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
                cmd.Parameters.AddWithValue("@OrderType", ddlOrderType.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@StickerMandatory", sticker1);
                cmd.Parameters.AddWithValue("@isVIP", VIP);
                cmd.Parameters.AddWithValue("@NetAmount", lblAmount.Text);
                StrNICVehicleType = ViewState["StrNICVehicleType1"].ToString();
                if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)" || StrNICVehicleType == "")
                {
                  cmd.Parameters.AddWithValue("@VehicleType", ddlVehicleType.SelectedItem.Text.ToString());
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
                cmd.Parameters.AddWithValue("@addrecordby", AddRecordBy);
                cmd.Parameters.AddWithValue("@CounterNo", CounterNo);
                cmd.Parameters.AddWithValue("@UserRTOLocationID", Convert.ToInt32(RTOLocationID));
                cmd.Parameters.Add("@Message", SqlDbType.Char, 1);
                cmd.Parameters["@Message"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                string message = cmd.Parameters["@Message"].Value.ToString();
                con.Close();
                if (message == "Y")
                {

                    lblSucMess.Visible = true;
                    lblErrMess.Visible = false;
                    //btnDownload.Visible = true;
                    Button3.Visible = true;
                    lblSucMess.Text = "Record Saved Successfully";
                    string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                    Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                    int u = Utils.ExecNonQuery(Query, ConnectionString);
                    Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Invoice No'";
                    u = Utils.ExecNonQuery(Query, ConnectionString);
                    Query = "update prefix set lastno=lastno+1 where  rtolocationid ='" + RTOLocationID + "' and prefixfor='Delivery Challan No'";
                    u = Utils.ExecNonQuery(Query, ConnectionString);

                    if (txtMobileno.Text.Length > 0)
                    {
                        string SMSText = "Cash Rs. " + Math.Round(decimal.Parse(lblAmount.Text), 0) + " received against HSRP  " + lblRegNo.Text.Trim().ToUpper() + " on " + System.DateTime.Now.ToString("dd/MM/yyyy") + "  receipt number  " + cashrc + ". Transport Dept HP.";
                                          
                        string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + txtMobileno.Text.ToString() + "&message=" + SMSText + "";
                        HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                        MyRequest.Method = "GET";
                        WebResponse myRespose = MyRequest.GetResponse();
                        StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
                        string result = sr.ReadToEnd();
                        sr.Close();
                        myRespose.Close();

                        Utils.ExecNonQuery("insert into SMSlog_HP([hsrp_stateid],SMSLogID,[MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[CashReceiptSmsText],[CashReceiptSMSDateTime],[CashReceiptSMSServerResponseText])  values('" + HSRPStateID + "',  (select  top 1  SMSLogID    from  SMSlog_HP order by  [CashReceiptSMSDateTime] desc)+1 ,'" + txtMobileno.Text + "', '" + lblRegNo.Text.Trim().ToUpper() + "','" + lblAuthNo.Text + "', '" + ddlOrderType.SelectedValue.ToString() + "', '" + SMSText + "', getdate(),'" + result + "')", ConnectionString);
                    }
                    lblSucMess.Text = "Record Saved Successfully..";


                    //btnDownload.Visible = true;
                }
                else
                {
                    lblSucMess.Visible = false;
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "VehicleRegNo. Already Exist";
                }
            }

            catch (Exception ex)
            {
                lblSucMess.Text = "Message : " + ex;
                return;
            }
        }


        protected void ddlVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            String SqlQuery = "SELECT top(1) HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,OwnerName,Address1,MobileNo,EmailID,OrderType,VehicleClass,NICvehicletype,ManufacturerName,ManufacturerModel,ChassisNo,EngineNo,VehicleRegNo FROM HSRPRecordsStaggingArea where VehicleRegNo='" + txtRegNo.Text + "' and HSRP_StateID='" + HSRPStateID + "' order by  HSRPRecord_CreationDate desc";
            dt = Utils.GetDataTable(SqlQuery, ConnectionString);
            StrNICVehicleType = dt.Rows[0]["NICvehicletype"].ToString().Split('*')[0];

            if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
            {
                ddlVehicleType.Visible = true;
               
                string SQLString3 = "exec [getPlatesData] '" + HSRPStateID + "','" + ddlVehicleType.SelectedItem.Text.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + ddlOrderType.SelectedValue.ToString() + "' ";
                DataTable dt2 = Utils.GetDataTable(SQLString3, ConnectionString);
                 
                lblAmount.Text = dt2.Rows[0]["netamount"].ToString();
                lblAmount.Text = Convert.ToString(Math.Round(decimal.Parse(lblAmount.Text), 0));

          
            }
            else
            {
               
                string SQLString2 = "exec [getPlatesData]'" + HSRPStateID + "','" + lblVehicleType.Text + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + ddlOrderType.SelectedValue.ToString() + "'";
                DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                lblAmount.Text = dt1.Rows[0]["netamount"].ToString();
                lblAmount.Text = Convert.ToString(Math.Round(decimal.Parse(lblAmount.Text), 0));



            }
            ddlVehicleType.Visible = true;
        }

        protected void ddlVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {


            String SqlQuery = "SELECT top(1) HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,OwnerName,Address1,MobileNo,EmailID,OrderType,VehicleClass,NICvehicletype,ManufacturerName,ManufacturerModel,ChassisNo,EngineNo,VehicleRegNo FROM HSRPRecordsStaggingArea where VehicleRegNo='" + txtRegNo.Text + "' and HSRP_StateID='" + HSRPStateID + "' order by  HSRPRecord_CreationDate desc";
            dt = Utils.GetDataTable(SqlQuery, ConnectionString);
            StrNICVehicleType = dt.Rows[0]["NICvehicletype"].ToString().Split('*')[0];

            if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
            {
                ddlVehicleType.Visible = true;
               
                string SQLString3 = "exec [getPlatesData]'" + HSRPStateID + "','" + ddlVehicleType.SelectedItem.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + ddlOrderType.SelectedValue.ToString() + "'";
                DataTable dt2 = Utils.GetDataTable(SQLString3, ConnectionString);
                lblAmount.Text = dt2.Rows[0]["netamount"].ToString();
                lblAmount.Text = Convert.ToString(Math.Round(decimal.Parse(lblAmount.Text), 0));


            }
            else
            {
                string SQLString2 = "exec [getPlatesData]'"  + HSRPStateID + "','" + lblVehicleType.Text + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + ddlOrderType.SelectedValue.ToString() + "'";
                DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                lblAmount.Text = dt1.Rows[0]["netamount"].ToString();
                lblAmount.Text = Convert.ToString(Math.Round(decimal.Parse(lblAmount.Text), 0));

            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            EpsionPrint();
        }


        //public void EpsionPrint()
        //{
        //    String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //    DataTable GetAddress;
        //    string Address;
        //    string AffAddress = string.Empty;
        //    string sqlquery = string.Empty;
        //    GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + Session["UserHSRPStateID"].ToString() + "'", ConnectionString);

        //    if (GetAddress.Rows[0]["pincode"].ToString() != "" || GetAddress.Rows[0]["pincode"] != null)
        //    {
        //        Address = " - " + GetAddress.Rows[0]["pincode"];
        //    }
        //    else
        //    {
        //        Address = "";
        //    }

        //    string SQLString = " select top 1 hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType from hsrprecords where  vehicleRegNo ='" + txtRegNo.Text.Trim() + "' order by  HSRPRecord_CreationDate desc";
        //    DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);

        //    DataProvider.BAL obj = new DataProvider.BAL();
        //    if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
        //    {

        //        string strquery1 = "select top 1 convert(varchar(15),PlateAffixationDate,103) as Date from hsrprecords where HSRPRecord_AuthorizationNo='" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "' order by  HSRPRecord_CreationDate desc";
        //        string date1 = Utils.getDataSingleValue(strquery1, ConnectionString, "Date");


        //        string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


        //        String StringField = String.Empty;
        //        String StringAlert = String.Empty;


        //        //Creates an instance of the iTextSharp.text.Document-object:
        //        Document document = new Document();
        //        float imageWidth = 216;
        //        float imageHeight = 360;
        //        document.SetMargins(0, 0, 5, 0);
        //        document.SetPageSize(new iTextSharp.text.Rectangle(imageWidth, imageHeight));

        //        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        //        // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
        //        //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
        //        string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
        //        //PdfWriter.GetInstance(document, new FileStream(Server.MapPath(PdfFolder), FileMode.Create));
        //        PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

        //        //Opens the document:
        //        document.Open();

        //        //Adds content to the document:
        //        // document.Add(new Paragraph("Ignition Log Report"));
        //        PdfPTable table = new PdfPTable(2);
        //        PdfPTable table1 = new PdfPTable(2);
        //        PdfPTable table2 = new PdfPTable(2);
        //        //actual width of table in points
        //        //table.TotalWidth = 100f;

        //        //fix the absolute width of the table

        //        PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell312.Colspan = 2;
        //        cell312.BorderColor = BaseColor.WHITE;
        //        cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table1.AddCell(cell312);

        //        PdfPCell cell312a = new PdfPCell(new Phrase("Duplicate", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        cell312a.Colspan = 2;
        //        cell312a.BorderColor = BaseColor.WHITE;
        //        cell312a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table2.AddCell(cell312a);

        //        PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell12.Colspan = 2;
        //        cell12.BorderColor = BaseColor.WHITE;
        //        cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell12);

        //        PdfPCell cell1203 = new PdfPCell(new Phrase(AffAddress, new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell1203.Colspan = 2;
        //        cell1203.BorderWidthLeft = 0f;
        //        cell1203.BorderWidthRight = 0f;
        //        cell1203.BorderWidthTop = 0f;
        //        cell1203.BorderWidthBottom = 0f;
        //        cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell1203);



        //        PdfPCell cell0 = new PdfPCell(new Phrase("HSRP CASH RECEIPT", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
        //        cell0.Colspan = 2;
        //        cell0.BorderWidthLeft = 0f;
        //        cell0.BorderWidthRight = 0f;
        //        cell0.BorderWidthTop = 0f;
        //        cell0.BorderWidthBottom = 0f;

        //        cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell0);


        //        PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell1.Colspan = 2;
        //        cell1.BorderWidthLeft = 0f;
        //        cell1.BorderWidthRight = 0f;
        //        cell1.BorderWidthTop = 0f;
        //        cell1.BorderWidthBottom = 0f;

        //        cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell1);




        //        PdfPCell cellInv2 = new PdfPCell(new Phrase("CASH RECEIPT No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cellInv2.Colspan = 0;

        //        cellInv2.BorderWidthLeft = 0f;
        //        cellInv2.BorderWidthRight = 0f;
        //        cellInv2.BorderWidthTop = 0f;
        //        cellInv2.BorderWidthBottom = 0f;
        //        cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cellInv2);



        //        PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cellInv22111.Colspan = 0;
        //        cellInv22111.BorderWidthLeft = 0f;
        //        cellInv22111.BorderWidthRight = 0f;
        //        cellInv22111.BorderWidthTop = 0f;
        //        cellInv22111.BorderWidthBottom = 0f;
        //        cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cellInv22111);


        //        PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell21.Colspan = 0;

        //        cell21.BorderWidthLeft = 0f;
        //        cell21.BorderWidthRight = 0f;
        //        cell21.BorderWidthTop = 0f;
        //        cell21.BorderWidthBottom = 0f;
        //        cell21.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell21);
        //        string CashReceiptDateTime = string.Empty;

        //        if (dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString() == "")
        //        {
        //            CashReceiptDateTime = DateTime.Now.ToString("dd/MM/yyyy");
        //        }
        //        else
        //        {
        //            CashReceiptDateTime = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("dd/MM/yyyy");
        //        }
        //        PdfPCell cell212 = new PdfPCell(new Phrase(": " + CashReceiptDateTime, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell212.Colspan = 0;

        //        cell212.BorderWidthLeft = 0f;
        //        cell212.BorderWidthRight = 0f;
        //        cell212.BorderWidthTop = 0f;
        //        cell212.BorderWidthBottom = 0f;
        //        cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell212);



        //        PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell2.Colspan = 0;

        //        cell2.BorderWidthLeft = 0f;
        //        cell2.BorderWidthRight = 0f;
        //        cell2.BorderWidthTop = 0f;
        //        cell2.BorderWidthBottom = 0f;
        //        cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell2);

        //        string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

        //        PdfPCell cell22111 = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell22111.Colspan = 0;
        //        cell22111.BorderWidthLeft = 0f;
        //        cell22111.BorderWidthRight = 0f;
        //        cell22111.BorderWidthTop = 0f;
        //        cell22111.BorderWidthBottom = 0f;
        //        cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell22111);

        //        PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell22.Colspan = 0;

        //        cell22.BorderWidthLeft = 0f;
        //        cell22.BorderWidthRight = 0f;
        //        cell22.BorderWidthTop = 0f;
        //        cell22.BorderWidthBottom = 0f;
        //        cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell22);

        //        PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss tt"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell222.Colspan = 0;

        //        cell222.BorderWidthLeft = 0f;
        //        cell222.BorderWidthRight = 0f;
        //        cell222.BorderWidthTop = 0f;
        //        cell222.BorderWidthBottom = 0f;
        //        cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell222);



        //        PdfPCell cell5 = new PdfPCell(new Phrase("AUTH NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell5.Colspan = 0;

        //        cell5.BorderWidthLeft = 0f;
        //        cell5.BorderWidthRight = 0f;
        //        cell5.BorderWidthTop = 0f;
        //        cell5.BorderWidthBottom = 0f;
        //        cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell5);

        //        PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell55.Colspan = 0;

        //        cell55.BorderWidthLeft = 0f;
        //        cell55.BorderWidthRight = 0f;
        //        cell55.BorderWidthTop = 0f;
        //        cell55.BorderWidthBottom = 0f;
        //        cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell55);

        //        PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell25.Colspan = 0;

        //        cell25.BorderWidthLeft = 0f;
        //        cell25.BorderWidthRight = 0f;
        //        cell25.BorderWidthTop = 0f;
        //        cell25.BorderWidthBottom = 0f;
        //        cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell25);
             
        //        string auths = string.Empty;
        //        auths = dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString();
        //        if (auths == "")
        //        {
        //            PdfPCell cell255 = new PdfPCell(new Phrase(": " + auths, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell255.Colspan = 0;

        //            cell255.BorderWidthLeft = 0f;
        //            cell255.BorderWidthRight = 0f;
        //            cell255.BorderWidthTop = 0f;
        //            cell255.BorderWidthBottom = 0f;
        //            cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell255);

        //        }
        //        else
        //        {
        //            DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
        //            PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //            cell255.Colspan = 0;

        //            cell255.BorderWidthLeft = 0f;
        //            cell255.BorderWidthRight = 0f;
        //            cell255.BorderWidthTop = 0f;
        //            cell255.BorderWidthBottom = 0f;
        //            cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //            table.AddCell(cell255);

        //        }
        //        PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell7.Colspan = 0;
        //        cell7.BorderWidthLeft = 0f;
        //        cell7.BorderWidthRight = 0f;
        //        cell7.BorderWidthTop = 0f;
        //        cell7.BorderWidthBottom = 0f;
        //        cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell7);

        //        PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell75.Colspan = 0;
        //        cell75.BorderWidthLeft = 0f;
        //        cell75.BorderWidthRight = 0f;
        //        cell75.BorderWidthTop = 0f;
        //        cell75.BorderWidthBottom = 0f;
        //        cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell75);

        //        PdfPCell cell29 = new PdfPCell(new Phrase("CONTACT NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell29.Colspan = 0;
        //        cell29.BorderWidthLeft = 0f;
        //        cell29.BorderWidthRight = 0f;
        //        cell29.BorderWidthTop = 0f;
        //        cell29.BorderWidthBottom = 0f;
        //        cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell29);

        //        PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell295.Colspan = 0;
        //        cell295.BorderWidthLeft = 0f;
        //        cell295.BorderWidthRight = 0f;
        //        cell295.BorderWidthTop = 0f;
        //        cell295.BorderWidthBottom = 0f;
        //        cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell295);



        //        PdfPCell cell9 = new PdfPCell(new Phrase("VEHICLE REG.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell9.Colspan = 0;
        //        cell9.BorderWidthLeft = 0f;
        //        cell9.BorderWidthRight = 0f;
        //        cell9.BorderWidthTop = 0f;
        //        cell9.BorderWidthBottom = 0f;
        //        cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell9);

        //        PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell95.Colspan = 0;
        //        cell95.BorderWidthLeft = 0f;
        //        cell95.BorderWidthRight = 0f;
        //        cell95.BorderWidthTop = 0f;
        //        cell95.BorderWidthBottom = 0f;
        //        cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell95);

        //        PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell10.Colspan = 0;
        //        cell10.BorderWidthLeft = 0f;
        //        cell10.BorderWidthRight = 0f;
        //        cell10.BorderWidthTop = 0f;
        //        cell10.BorderWidthBottom = 0f;
        //        cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell10);

        //        PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell105.Colspan = 0;
        //        cell105.BorderWidthLeft = 0f;
        //        cell105.BorderWidthRight = 0f;
        //        cell105.BorderWidthTop = 0f;
        //        cell105.BorderWidthBottom = 0f;
        //        cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell105);

        //        PdfPCell cell11 = new PdfPCell(new Phrase(" VEHICLE CLASS ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell11.Colspan = 0;
        //        cell11.BorderWidthLeft = 0f;
        //        cell11.BorderWidthRight = 0f;
        //        cell11.BorderWidthTop = 0f;
        //        cell11.BorderWidthBottom = 0f;
        //        cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell11);

        //        PdfPCell cell115 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell115.Colspan = 0;
        //        cell115.BorderWidthLeft = 0f;
        //        cell115.BorderWidthRight = 0f;
        //        cell115.BorderWidthTop = 0f;
        //        cell115.BorderWidthBottom = 0f;
        //        cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell115);



        //        PdfPCell cellNet120 = new PdfPCell(new Phrase("NET AMOUNT (Rs.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cellNet120.Colspan = 0;
        //        cellNet120.BorderWidthLeft = 0f;
        //        cellNet120.BorderWidthRight = 0f;
        //        cellNet120.BorderWidthTop = 0f;
        //        cellNet120.BorderWidthBottom = 0f;
        //        cellNet120.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cellNet120);



        //        PdfPCell cell1205 = new PdfPCell(new Phrase(" " + dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell1205.Colspan = 0;
        //        cell1205.BorderWidthLeft = 0f;
        //        cell1205.BorderWidthRight = 0f;
        //        cell1205.BorderWidthTop = 0f;
        //        cell1205.BorderWidthBottom = 0f;
        //        cell1205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell1205);

        //        PdfPCell celldupCash401 = new PdfPCell(new Phrase("ROUND OF AMOUNT", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        celldupCash401.Colspan = 0;
        //        celldupCash401.BorderWidthLeft = 0f;
        //        celldupCash401.BorderWidthRight = 0f;
        //        celldupCash401.BorderWidthTop = 0f;
        //        celldupCash401.BorderWidthBottom = 0f;
        //        celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(celldupCash401);


        //        decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
        //        roundAmt = Math.Round(roundAmt, 0);

        //        PdfPCell celldupCash402 = new PdfPCell(new Phrase(" " + roundAmt, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        celldupCash402.Colspan = 0;
        //        celldupCash402.BorderWidthLeft = 0f;
        //        celldupCash402.BorderWidthRight = 0f;
        //        celldupCash402.BorderWidthTop = 0f;
        //        celldupCash402.BorderWidthBottom = 0f;
        //        celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(celldupCash402);
        //        PdfPCell celldupCash40z1 = new PdfPCell(new Phrase("(Inclusive of All Tax)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        celldupCash40z1.Colspan = 4;
        //        celldupCash40z1.BorderWidthLeft = 0f;
        //        celldupCash40z1.BorderWidthRight = 0f;
        //        celldupCash40z1.BorderWidthTop = 0f;
        //        celldupCash40z1.BorderWidthBottom = 0f;
        //        celldupCash40z1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(celldupCash40z1);




        //        PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("Disclaimer :", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
        //        celldupRouCash402.Colspan = 2;
        //        celldupRouCash402.BorderWidthLeft = 0f;
        //        celldupRouCash402.BorderWidthRight = 0f;
        //        celldupRouCash402.BorderWidthTop = 0f;
        //        celldupRouCash402.BorderWidthBottom = 0f;
        //        celldupRouCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(celldupRouCash402);


        //        string Message = "\u2022" + " Vehicle Owner is requested to please check the Correctness of the cash receipt.";

        //        PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell64.Colspan = 2;
        //        cell64.BorderWidthLeft = 0f;
        //        cell64.BorderWidthRight = 0f;
        //        cell64.BorderWidthTop = 0f;
        //        cell64.BorderWidthBottom = 0f;
        //        cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell64);


        //        string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt. The company shall not be responsible for any clarrical mistake what so ever.";

        //        PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell65a.Colspan = 4;
        //        cell65a.BorderWidthLeft = 0f;
        //        cell65a.BorderWidthRight = 0f;
        //        cell65a.BorderWidthTop = 0f;
        //        cell65a.BorderWidthBottom = 0f;
        //        cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell65a);

        //        PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell63.Colspan = 2;
        //        cell63.BorderWidthLeft = 0f;
        //        cell63.BorderWidthRight = 0f;
        //        cell63.BorderWidthTop = 0f;
        //        cell63.BorderWidthBottom = 0f;
        //        cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell63);

                       

        //        PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cell62.Colspan = 2;
        //        cell62.BorderWidthLeft = 0f;
        //        cell62.BorderWidthRight = 0f;
        //        cell62.BorderWidthTop = 0f;
        //        cell62.BorderWidthBottom = 0f;
        //        cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cell62);




        //        PdfPCell cellsp1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
        //        cellsp1.Colspan = 2;
        //        cellsp1.BorderWidthLeft = 0f;
        //        cellsp1.BorderWidthRight = 0f;
        //        cellsp1.BorderWidthTop = 0f;
        //        cellsp1.BorderWidthBottom = 0f;
        //        cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
        //        table.AddCell(cellsp1);


        //        document.Add(table1);
        //        document.Add(table);

        //        document.Add(table2);
        //        document.Add(table);

        //        document.Close();
        //        HttpContext context = HttpContext.Current;
        //        context.Response.ContentType = "Application/pdf";
        //        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        //        context.Response.WriteFile(PdfFolder);
        //        context.Response.End();
        //    }
        //}

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

            string SQLString = " select top 1  hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,convert(varchar(12),OwnerName)as OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType , EngineNo , SGSTPer,	SGSTAmount,	CGSTPer	,CGSTAmount	 , gstbasicamount from hsrprecords where  vehicleRegNo ='" + txtRegNo.Text.Trim() + "' order by  HSRPRecord_CreationDate desc";
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


                //PdfPCell cell312 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell312.Colspan = 2;
                //cell312.BorderColor = BaseColor.WHITE;
                //cell312.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table1.AddCell(cell312);


                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 2;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);
                //AffAddress
                PdfPCell cell1203 = new PdfPCell(new Phrase(GetAddress.Rows[0]["address1"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1203.Colspan = 4;
                cell1203.BorderWidthLeft = 0f;
                cell1203.BorderWidthRight = 0f;
                cell1203.BorderWidthTop = 0f;
                cell1203.BorderWidthBottom = 0f;
                cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1203);



                PdfPCell cell0 = new PdfPCell(new Phrase("HSRP CASH RECEIPT", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK)));
                cell0.Colspan = 2;
                cell0.BorderWidthLeft = 0f;
                cell0.BorderWidthRight = 0f;
                cell0.BorderWidthTop = 0f;
                cell0.BorderWidthBottom = 0f;

                cell0.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell0);


                //PdfPCell cell1 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell1.Colspan = 2;
                //cell1.BorderWidthLeft = 0f;
                //cell1.BorderWidthRight = 0f;
                //cell1.BorderWidthTop = 0f;
                //cell1.BorderWidthBottom = 0f;

                //cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1);

                PdfPCell cellInv2 = new PdfPCell(new Phrase("RECEIPT NO.", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellInv2.Colspan = 0;

                cellInv2.BorderWidthLeft = 0f;
                cellInv2.BorderWidthRight = 0f;
                cellInv2.BorderWidthTop = 0f;
                cellInv2.BorderWidthBottom = 0f;
                cellInv2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellInv2);

                PdfPCell cellInv22111 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellInv22111.Colspan = 0;
                cellInv22111.BorderWidthLeft = 0f;
                cellInv22111.BorderWidthRight = 0f;
                cellInv22111.BorderWidthTop = 0f;
                cellInv22111.BorderWidthBottom = 0f;
                cellInv22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellInv22111);



                PdfPCell cell21 = new PdfPCell(new Phrase("DATE", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
                PdfPCell cell212 = new PdfPCell(new Phrase(": " + CashReceiptDateTime, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell212.Colspan = 0;

                cell212.BorderWidthLeft = 0f;
                cell212.BorderWidthRight = 0f;
                cell212.BorderWidthTop = 0f;
                cell212.BorderWidthBottom = 0f;
                cell212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell212);



                PdfPCell cell2 = new PdfPCell(new Phrase("GSTIN", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2.Colspan = 0;

                cell2.BorderWidthLeft = 0f;
                cell2.BorderWidthRight = 0f;
                cell2.BorderWidthTop = 0f;
                cell2.BorderWidthBottom = 0f;
                cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2);

                string getTinNo = Utils.getScalarValue("select GSTIN from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

                PdfPCell cell22111 = new PdfPCell(new Phrase(": " + getTinNo.ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22111.Colspan = 0;
                cell22111.BorderWidthLeft = 0f;
                cell22111.BorderWidthRight = 0f;
                cell22111.BorderWidthTop = 0f;
                cell22111.BorderWidthBottom = 0f;
                cell22111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22111);

                PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell22.Colspan = 0;

                cell22.BorderWidthLeft = 0f;
                cell22.BorderWidthRight = 0f;
                cell22.BorderWidthTop = 0f;
                cell22.BorderWidthBottom = 0f;
                cell22.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell22);

                PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm tt"), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell222.Colspan = 0;

                cell222.BorderWidthLeft = 0f;
                cell222.BorderWidthRight = 0f;
                cell222.BorderWidthTop = 0f;
                cell222.BorderWidthBottom = 0f;
                cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell222);




                PdfPCell cell5 = new PdfPCell(new Phrase("AUTH. NO.", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell5.Colspan = 0;

                cell5.BorderWidthLeft = 0f;
                cell5.BorderWidthRight = 0f;
                cell5.BorderWidthTop = 0f;
                cell5.BorderWidthBottom = 0f;
                cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell5);

                PdfPCell cell55 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell55.Colspan = 0;

                cell55.BorderWidthLeft = 0f;
                cell55.BorderWidthRight = 0f;
                cell55.BorderWidthTop = 0f;
                cell55.BorderWidthBottom = 0f;
                cell55.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell55);

                PdfPCell cell25 = new PdfPCell(new Phrase("AUTH. DATE", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell25.Colspan = 0;

                cell25.BorderWidthLeft = 0f;
                cell25.BorderWidthRight = 0f;
                cell25.BorderWidthTop = 0f;
                cell25.BorderWidthBottom = 0f;
                cell25.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell25);


                string auths = string.Empty;
                auths = dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString();
                if (auths == "")
                {
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + auths, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
                    PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell255.Colspan = 0;

                    cell255.BorderWidthLeft = 0f;
                    cell255.BorderWidthRight = 0f;
                    cell255.BorderWidthTop = 0f;
                    cell255.BorderWidthBottom = 0f;
                    cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell255);

                }
                PdfPCell cell7 = new PdfPCell(new Phrase("OWNER NAME", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell7.Colspan = 0;
                cell7.BorderWidthLeft = 0f;
                cell7.BorderWidthRight = 0f;
                cell7.BorderWidthTop = 0f;
                cell7.BorderWidthBottom = 0f;
                cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell7);

                PdfPCell cell75 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell75.Colspan = 0;
                cell75.BorderWidthLeft = 0f;
                cell75.BorderWidthRight = 0f;
                cell75.BorderWidthTop = 0f;
                cell75.BorderWidthBottom = 0f;
                cell75.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell75);

                PdfPCell cell29 = new PdfPCell(new Phrase("CONTACT NO.", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell29.Colspan = 0;
                cell29.BorderWidthLeft = 0f;
                cell29.BorderWidthRight = 0f;
                cell29.BorderWidthTop = 0f;
                cell29.BorderWidthBottom = 0f;
                cell29.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell29);

                PdfPCell cell295 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell295.Colspan = 0;
                cell295.BorderWidthLeft = 0f;
                cell295.BorderWidthRight = 0f;
                cell295.BorderWidthTop = 0f;
                cell295.BorderWidthBottom = 0f;
                cell295.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell295);



                PdfPCell cell9 = new PdfPCell(new Phrase("ENGINE NO.", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell9.Colspan = 0;
                cell9.BorderWidthLeft = 0f;
                cell9.BorderWidthRight = 0f;
                cell9.BorderWidthTop = 0f;
                cell9.BorderWidthBottom = 0f;
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell9);

                PdfPCell cell95 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell95.Colspan = 0;
                cell95.BorderWidthLeft = 0f;
                cell95.BorderWidthRight = 0f;
                cell95.BorderWidthTop = 0f;
                cell95.BorderWidthBottom = 0f;
                cell95.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell95);

                PdfPCell cell999 = new PdfPCell(new Phrase("VEHICLE REG. NO.", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell999.Colspan = 0;
                cell999.BorderWidthLeft = 0f;
                cell999.BorderWidthRight = 0f;
                cell999.BorderWidthTop = 0f;
                cell999.BorderWidthBottom = 0f;
                cell999.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell999);

                PdfPCell cell950 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell950.Colspan = 0;
                cell950.BorderWidthLeft = 0f;
                cell950.BorderWidthRight = 0f;
                cell950.BorderWidthTop = 0f;
                cell950.BorderWidthBottom = 0f;
                cell950.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell950);




                PdfPCell cell10 = new PdfPCell(new Phrase("VEHICLE MODEL", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell10.Colspan = 0;
                cell10.BorderWidthLeft = 0f;
                cell10.BorderWidthRight = 0f;
                cell10.BorderWidthTop = 0f;
                cell10.BorderWidthBottom = 0f;
                cell10.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell10);

                PdfPCell cell105 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell105.Colspan = 0;
                cell105.BorderWidthLeft = 0f;
                cell105.BorderWidthRight = 0f;
                cell105.BorderWidthTop = 0f;
                cell105.BorderWidthBottom = 0f;
                cell105.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell105);

                PdfPCell cell11 = new PdfPCell(new Phrase("VEHICLE CLASS ", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11.Colspan = 0;
                cell11.BorderWidthLeft = 0f;
                cell11.BorderWidthRight = 0f;
                cell11.BorderWidthTop = 0f;
                cell11.BorderWidthBottom = 0f;
                cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11);

                PdfPCell cell115 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell115.Colspan = 0;
                cell115.BorderWidthLeft = 0f;
                cell115.BorderWidthRight = 0f;
                cell115.BorderWidthTop = 0f;
                cell115.BorderWidthBottom = 0f;
                cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell115);


                PdfPCell cell11goods = new PdfPCell(new Phrase("DESCRIPTION OF GOODS ", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11goods.Colspan = 0;
                cell11goods.BorderWidthLeft = 0f;
                cell11goods.BorderWidthRight = 0f;
                cell11goods.BorderWidthTop = 0f;
                cell11goods.BorderWidthBottom = 0f;
                cell11goods.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11goods);

                PdfPCell cell11577 = new PdfPCell(new Phrase(": HSRP (HSN8310)", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11577.Colspan = 0;
                cell11577.BorderWidthLeft = 0f;
                cell11577.BorderWidthRight = 0f;
                cell11577.BorderWidthTop = 0f;
                cell11577.BorderWidthBottom = 0f;
                cell11577.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11577);



                PdfPCell cellNet120 = new PdfPCell(new Phrase("HSRP Fee (Rs.)", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellNet120.Colspan = 0;
                cellNet120.BorderWidthLeft = 0f;
                cellNet120.BorderWidthRight = 0f;
                cellNet120.BorderWidthTop = 0f;
                cellNet120.BorderWidthBottom = 0f;
                cellNet120.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellNet120);

                decimal roundAmt = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["NetAmount"].ToString());
                roundAmt = Math.Round(roundAmt, 0);

                PdfPCell cell1205 = new PdfPCell(new Phrase(":" + Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["gstbasicamount"].ToString()), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1205.Colspan = 0;
                cell1205.BorderWidthLeft = 0f;
                cell1205.BorderWidthRight = 0f;
                cell1205.BorderWidthTop = 0f;
                cell1205.BorderWidthBottom = 0f;
                cell1205.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1205);



                PdfPCell cellNet120CGSTPer = new PdfPCell(new Phrase("CGST@" + Convert.ToInt32(dataSetFillHSRPDeliveryChallan.Rows[0]["CGSTPer"]) + "%", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellNet120CGSTPer.Colspan = 0;
                cellNet120CGSTPer.BorderWidthLeft = 0f;
                cellNet120CGSTPer.BorderWidthRight = 0f;
                cellNet120CGSTPer.BorderWidthTop = 0f;
                cellNet120CGSTPer.BorderWidthBottom = 0f;
                cellNet120CGSTPer.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellNet120CGSTPer);



                PdfPCell cell1205CGSTAmount = new PdfPCell(new Phrase(":" + Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["CGSTAmount"].ToString()), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1205CGSTAmount.Colspan = 0;
                cell1205CGSTAmount.BorderWidthLeft = 0f;
                cell1205CGSTAmount.BorderWidthRight = 0f;
                cell1205CGSTAmount.BorderWidthTop = 0f;
                cell1205CGSTAmount.BorderWidthBottom = 0f;
                cell1205CGSTAmount.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1205CGSTAmount);

                PdfPCell cellNet120SGST = new PdfPCell(new Phrase("SGST@" + Convert.ToInt32(dataSetFillHSRPDeliveryChallan.Rows[0]["SGSTPer"]) + "%", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellNet120SGST.Colspan = 0;
                cellNet120SGST.BorderWidthLeft = 0f;
                cellNet120SGST.BorderWidthRight = 0f;
                cellNet120SGST.BorderWidthTop = 0f;
                cellNet120SGST.BorderWidthBottom = 0f;
                cellNet120SGST.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellNet120SGST);


                PdfPCell cell1205SGST = new PdfPCell(new Phrase(":" + Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["SGSTAmount"]), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1205SGST.Colspan = 0;
                cell1205SGST.BorderWidthLeft = 0f;
                cell1205SGST.BorderWidthRight = 0f;
                cell1205SGST.BorderWidthTop = 0f;
                cell1205SGST.BorderWidthBottom = 0f;
                cell1205SGST.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1205SGST);

                PdfPCell celldupCash401 = new PdfPCell(new Phrase("TOTAL AMOUNT(ROUND OFF)", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                celldupCash401.Colspan = 0;
                celldupCash401.BorderWidthLeft = 0f;
                celldupCash401.BorderWidthRight = 0f;
                celldupCash401.BorderWidthTop = 0f;
                celldupCash401.BorderWidthBottom = 0f;
                celldupCash401.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash401);




                PdfPCell celldupCash402 = new PdfPCell(new Phrase(":" + roundAmt, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                celldupCash402.Colspan = 0;
                celldupCash402.BorderWidthLeft = 0f;
                celldupCash402.BorderWidthRight = 0f;
                celldupCash402.BorderWidthTop = 0f;
                celldupCash402.BorderWidthBottom = 0f;
                celldupCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupCash402);


                PdfPCell celldupRouCash402 = new PdfPCell(new Phrase("Disclaimer :", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                celldupRouCash402.Colspan = 2;
                celldupRouCash402.BorderWidthLeft = 0f;
                celldupRouCash402.BorderWidthRight = 0f;
                celldupRouCash402.BorderWidthTop = 0f;
                celldupRouCash402.BorderWidthBottom = 0f;
                celldupRouCash402.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(celldupRouCash402);


                string Message = "\u2022" + " Vehicle owner is requested to please check the correctness of the cash slip.";

                PdfPCell cell64 = new PdfPCell(new Phrase(Message, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell64.Colspan = 2;
                cell64.BorderWidthLeft = 0f;
                cell64.BorderWidthRight = 0f;
                cell64.BorderWidthTop = 0f;
                cell64.BorderWidthBottom = 0f;
                cell64.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell64);


                string MessageSec = "\u2022" + " Before leaving the counter verify your data on cash receipt. The company shall not be responsible for any clerical mistake what so ever.";

                PdfPCell cell65a = new PdfPCell(new Phrase(MessageSec, new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65a.Colspan = 4;
                cell65a.BorderWidthLeft = 0f;
                cell65a.BorderWidthRight = 0f;
                cell65a.BorderWidthTop = 0f;
                cell65a.BorderWidthBottom = 0f;
                cell65a.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65a);

                PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell63.Colspan = 2;
                cell63.BorderWidthLeft = 0f;
                cell63.BorderWidthRight = 0f;
                cell63.BorderWidthTop = 0f;
                cell63.BorderWidthBottom = 0f;
                cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell63);



                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 2;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell62);

               

               


                document.Add(table1);
                document.Add(table);

                //document.Add(table2);
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
            refresh();
        }

        protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            String SqlQuery = "SELECT top(1) HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,OwnerName,Address1,MobileNo,EmailID,OrderType,VehicleClass,NICvehicletype,ManufacturerName,ManufacturerModel,ChassisNo,EngineNo,VehicleRegNo FROM HSRPRecordsStaggingArea where VehicleRegNo='" + txtRegNo.Text + "' and HSRP_StateID='" + HSRPStateID + "' order by  HSRPRecord_CreationDate desc";
            dt = Utils.GetDataTable(SqlQuery, ConnectionString);
            StrNICVehicleType = dt.Rows[0]["NICvehicletype"].ToString().Split('*')[0];

            if (StrNICVehicleType == "Motor Cycle/Scooter(2WN)" || StrNICVehicleType == "M-Cycle/Scooter(2WN)")
            {
                ddlVehicleType.Visible = true;
              
                string SQLString3 = "exec [getPlatesData]'" + HSRPStateID + "','" + ddlVehicleType.SelectedItem.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + ddlOrderType.SelectedValue.ToString() + "'";
                DataTable dt2 = Utils.GetDataTable(SQLString3, ConnectionString);
                lblAmount.Text = dt2.Rows[0]["netamount"].ToString();
                lblAmount.Text = Convert.ToString(Math.Round(decimal.Parse(lblAmount.Text), 0));


            }
            else
            {
               
                string SQLString2 = "exec [getPlatesData]'" + HSRPStateID + "','" + lblVehicleType.Text + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + ddlOrderType.SelectedValue.ToString() + "'";
                DataTable dt1 = Utils.GetDataTable(SQLString2, ConnectionString);
                lblAmount.Text = dt1.Rows[0]["netamount"].ToString();
                lblAmount.Text = Convert.ToString(Math.Round(decimal.Parse(lblAmount.Text), 0));

            }

        }

      
    }
}