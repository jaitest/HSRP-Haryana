using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Printing;
using iTextSharp.text;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration; 
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System.Configuration;
using System.IO;
using System.Data;
using DataProvider;
using System.Drawing.Printing;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Text;

namespace HSRP.Transaction
{
    public partial class DelhiDealerOldDateCashCollection : System.Web.UI.Page
    {

        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
        string SQLString = string.Empty;
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        DataTable dt1 = new DataTable();
        DataTable dtz = new DataTable();
        BAL obj = new BAL();
        DataTable dataSetFillDetails = new DataTable();
        string MacAddressType = string.Empty;
        string MacAddress = string.Empty;
        BaseFont basefont;
        string fontpath;
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

            MacAddressType = Session["SaveMacAddress"].ToString();
            MacAddress = Session["MacAddress"].ToString();

            HSRPStateID = Session["UserHSRPStateID"].ToString();
            //AutoCompleteExtender1.ContextKey =  HSRPStateID;
               DivCounterNo.Visible = true;
                divlabelCounterNo.Visible = true;
             

            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            if (!IsPostBack)
            {
                lblRecordType.Visible = false;
                checkBoxThirdSticker.Enabled = false;
                hiddenfieldReferenceValue.Value = "";
                hiddenfieldhsrprecordStaggingID.Value = "";
                hiddenfieldAuthorizationNo.Value = "";
                divAddress.Visible = false;
                allEnableTrueFalse();
                VehicleModel();
                VehicleClass();
                OrderType();
               btnDownloadReceipt.Visible = false;


                InitialSetting();
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                { 
                    //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                }
            }

        }
        private void InitialSetting()
        { 
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            CalendarDepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-5.00);
            CalendarDepositDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(-5.00);

            DepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-5.00);
            DepositDate.MaxDate = DateTime.Parse(MaxDate); 
        }

        protected void DropDownListVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //VehicleModel();
            //OrderType();
            txtAmount.Text = "";
            GetRateForVehicle();

        }
        protected void DropDownListVehicleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListVehicleModel.SelectedItem.ToString() == "LMV" || DropDownListVehicleModel.SelectedItem.ToString() == "LMV(CLASS)" || DropDownListVehicleModel.SelectedItem.ToString() == "MCV/HCV/TRAILERS")
            { 
                checkBoxThirdSticker.Checked = true;
            }
            else
            {
                checkBoxThirdSticker.Checked = false;
            }
        }

        protected void DropDownListOrderType_SelectedIndexChanged(object sender, EventArgs e)
        { 
            GetRateForVehicle(); 
        }

        private void GetRateForVehicle()
        {
            txtAmount.Text = "";
            DataTable dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue + "') as Amount", ConnectionString);

            txtAmount.Text = dt.Rows[0]["Amount"].ToString();


            SQLString = "select FrontPlateID,RearPlateID from RegistrationPlateDetail  where hsrp_stateid ='" + HSRPStateID + "' and vehicletype='" + DropDownListVehicleModel.SelectedItem.Text + "' and vehicleclass='" + DropDownListVehicleClass.SelectedItem.Text + "' and ordertype='" + DropDownListOrderType.SelectedValue + "'";
            DataTable dtPlateSize = Utils.GetDataTable(SQLString, ConnectionString);
            if (dtPlateSize.Rows.Count > 0)
            {
                if (dtPlateSize.Rows[0]["FrontPlateID"].ToString() != "")
                {
                    Utils.PopulateDropDownList(DropDownListFrontPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                    DropDownListFrontPlate.SelectedIndex = DropDownListFrontPlate.Items.Count - 1;
                }
                if (dtPlateSize.Rows[0]["RearPlateID"].ToString() != "")
                {
                    Utils.PopulateDropDownList(DropDownListRearPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["RearPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                    DropDownListRearPlate.SelectedIndex = DropDownListRearPlate.Items.Count - 1;
                }
            }
        }

        protected void btnDuplicate_Click(object sender, EventArgs e)
        { 
        }
        string Sticker = string.Empty, Vip = string.Empty, Query = string.Empty;

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (txtAmount.Text.Trim() != "")
            {
                DateTime OrderDate1;
                int i =0;
                String[] StringOrderDate = DepositDate.SelectedDate.ToString().Split('/');
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                if (checkBoxThirdSticker.Checked == true)
                {
                    Sticker = "Y";
                }
                else
                {
                    Sticker = "N";
                }

                if (chkVip.Checked == true)
                {
                    Vip = "Y";
                }
                else
                {
                    Vip = "N";
                }

                string valueamount = txtAmount.Text;
                decimal Round_OffAmount = Math.Round(Convert.ToDecimal(valueamount));

                string oname = txtOwnerName.Text;
                string ownername = oname.Replace("'", "");
                string father = txtFatherName.Text;
                string ownFatherName = father.Replace("'", "");
                string Address = txtAddress.Text;
                string ownAddress = Address.Replace("'", "");

                //String SqlCheck = "select COUNT(*) from dbo.HSRPRecords where VehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "' and HSRPRecord_AuthorizationNo ='" + txtAuthorizationNo.Text + "'";
                String SqlCheck = "select COUNT(*) from dbo.HSRPRecords where VehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "'  and  (OrderType='NB' or ordertype ='OB')";

                int z = Utils.getScalarCount(SqlCheck, ConnectionString);
                if (z <= 0)
                {
                    string Query = "select (prefixtext+right('00000000'+ convert(varchar,lastno+1),9)) as PrifixNo,(LastNo+1)as LastNo  from prefix where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                    dt1 = Utils.GetDataTable(Query, ConnectionString);

                    lblCashReceiptNo.Text = dt1.Rows[0]["PrifixNo"].ToString();
                    lblMessageCashReceipt.Text = "Cash Receipt No.";

                    if (hiddenfieldReferenceValue.Value == "DelhiOldData")
                    {
                        string SaveMacAdd = string.Empty;
                        if (MacAddressType == "Y")
                        {
                            SaveMacAdd = txtCounterNo.Text;
                        }
                        else
                        {
                            SaveMacAdd = "NULL";
                        }
                        SaveMacAdd = txtCounterNo.Text;
                        Query = "INSERT INTO HSRPRecords (HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,ownerFatherName,Address1,MobileNo,VehicleClass,OrderType,StickerMandatory,isVIP,NetAmount,Roundoff_NetAmount,VehicleType,OrderStatus,CashReceiptNo,ChassisNo, EngineNo,DealerCode,CreatedBy,SaveMacAddress,Addrecordby,ISFrontPlateSize,ISRearPlateSize,FrontPlateSize,RearPlateSize,Reference) values('" + OrderDate1 + "','" + HSRPStateID + "','" + RTOLocationID + "','" + txtAuthorizationNo.Text + "',GetDate()-5,'" + txtVehicleRegNo.Text.Trim().ToUpper() + "','" + ownername + "','" + ownFatherName + "','" + ownAddress + "','" + txtMobileNo.Text + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue.ToString() + "','" + Sticker + "','" + Vip + "','" + txtAmount.Text + "','" + Round_OffAmount + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','New Order','" + dt1.Rows[0]["PrifixNo"].ToString() + "', '" + txtChassisNo.Text.Trim() + "', '" + txtEngineRegNo.Text.Trim() + "','" + SaveMacAdd + "','" + Session["UID"].ToString() + "','" + Session["MacAddress"].ToString() + "','DelhiDealerOldData','Y','Y','" + DropDownListFrontPlate.SelectedValue + "','" + DropDownListRearPlate.SelectedValue + "','"+txtReference.Text+"')";

                        i = Utils.ExecNonQuery(Query, ConnectionString);
                        Query = "select max(neworder)+1 as neworder from rtolocation where rtolocationid='" + RTOLocationID + "'";
                        dt1 = Utils.GetDataTable(Query, ConnectionString);
                        Query = "update rtolocation set neworder='" + dt1.Rows[0]["neworder"].ToString() + "',lastupdateorder=getdate() where rtolocationid='" + RTOLocationID + "'";
                        Utils.ExecNonQuery(Query, ConnectionString);
                    }
                    if (i > 0)
                    {
                        Query = "update prefix set LastNo='" + dt1.Rows[0]["LastNo"].ToString() + "' where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                        Utils.ExecNonQuery(Query, ConnectionString);
                        hiddenfieldAuthorizationNo.Value = txtAuthorizationNo.Text;
                        lblSucMess.Text = "Record Saved";
                        btnDownloadReceipt.Visible = true;
                        btnSticker.Visible = true;
                        btnYellowSticker.Visible = true;
                        allEnableTrueFalse();
                       
                    }
                    else
                    {
                        lblErrMess.Text = "Duplicate Reg No";
                        btnDownloadReceipt.Visible = false;
                    }
                }
                else
                { 
                    lblErrMess.Text = "Duplicate Vehicle Reg No";
                }

            }
            else
            {
                
            }

        }  



      

        private void Refresh()
        {
            txtAmount.Text = "";
            txtAuthorizationNo.Text = "";
            txtMobileNo.Text = "";
            txtOwnerName.Text = "";
            txtTax.Text = "";
            txtVehicleRegNo.Text = "";
            
            InitialSetting();

            chkVip.Checked = false;

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Refresh();
        }
       
         
        

        protected void btnDownloadReceipt_Click(object sender, EventArgs e)
        {

            DataTable GetAddress;
            string Address;
            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", ConnectionString);

            if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }

            //SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where vehicleRegNo='" + txtVehicleRegNo.Text.Trim() + "'";
            SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where vehicleRegNo='" + txtVehicleRegNo.Text.Trim() + "'";
            DataTable dataSetFillHSRPDeliveryChallan = Utils.GetDataTable(SQLString, ConnectionString);


            BAL obj = new BAL();
            if (dataSetFillHSRPDeliveryChallan.Rows.Count > 0)
            {


                //string filename = "CASH RECEIPT-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";


                String StringField = String.Empty;
                String StringAlert = String.Empty;


                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();
                float imageWidth = 216;
                float imageHeight =360 ;
                document.SetMargins(0, 0, 5, 0);
                document.SetPageSize(new iTextSharp.text.Rectangle(imageWidth, imageHeight));

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(2);
                //actual width of table in points
                //table.TotalWidth = 100f;

                //fix the absolute width of the table



                PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12.Colspan = 2;
                cell12.BorderColor = BaseColor.WHITE;
                cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12);

                PdfPCell cell1203 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["Address1"].ToString() + " , " + GetAddress.Rows[0]["city"].ToString() + Address.ToString(), new iTextSharp.text.Font(bfTimes, 9f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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




                //PdfPCell cell22 = new PdfPCell(new Phrase("TIME", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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

                DateTime AuthDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                PdfPCell cell255 = new PdfPCell(new Phrase(": " + AuthDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell255.Colspan = 0;

                cell255.BorderWidthLeft = 0f;
                cell255.BorderWidthRight = 0f;
                cell255.BorderWidthTop = 0f;
                cell255.BorderWidthBottom = 0f;
                cell255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell255);

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


                string MessageSecnew = "\u2022" + " Delhi HSRP HelpLine/Customer Care No: 1800-1200-201";

                PdfPCell cell65anew = new PdfPCell(new Phrase(MessageSecnew, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65anew.Colspan = 4;
                cell65anew.BorderWidthLeft = 0f;
                cell65anew.BorderWidthRight = 0f;
                cell65anew.BorderWidthTop = 0f;
                cell65anew.BorderWidthBottom = 0f;
                cell65anew.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65anew);



                string MessageSecnew1 = "\u2022" + "WebSite: WWW.hsrpdelhi.com";

                PdfPCell cell65anew1 = new PdfPCell(new Phrase(MessageSecnew1, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65anew1.Colspan = 4;
                cell65anew1.BorderWidthLeft = 0f;
                cell65anew1.BorderWidthRight = 0f;
                cell65anew1.BorderWidthTop = 0f;
                cell65anew1.BorderWidthBottom = 0f;
                cell65anew1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65anew1);


                PdfPCell cell63 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell63.Colspan = 2;
                cell63.BorderWidthLeft = 0f;
                cell63.BorderWidthRight = 0f;
                cell63.BorderWidthTop = 0f;
                cell63.BorderWidthBottom = 0f;
                cell63.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell63);

                PdfPCell cellsp4 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp4.Colspan = 2;
                cellsp4.BorderWidthLeft = 0f;
                cellsp4.BorderWidthRight = 0f;
                cellsp4.BorderWidthTop = 0f;
                cellsp4.BorderWidthBottom = 0f;
                cellsp4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp4);

                PdfPCell cellsp5 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp5.Colspan = 2;
                cellsp5.BorderWidthLeft = 0f;
                cellsp5.BorderWidthRight = 0f;
                cellsp5.BorderWidthTop = 0f;
                cellsp5.BorderWidthBottom = 0f;
                cellsp5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp5);

                PdfPCell cell62 = new PdfPCell(new Phrase("(AUTH. SIGH.)", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell62.Colspan = 2;
                cell62.BorderWidthLeft = 0f;
                cell62.BorderWidthRight = 0f;
                cell62.BorderWidthTop = 0f;
                cell62.BorderWidthBottom = 0f;
                cell62.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell62);






                //////////////////////////////////////Duplicate CashRecipt/////////////////////////////////////////////////////////////////////////////////////////////


                PdfPCell cell2195 = new PdfPCell(new Phrase("---------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell2195.Colspan = 2;
                cell2195.BorderWidthLeft = 0f;
                cell2195.BorderWidthRight = 0f;
                cell2195.BorderWidthTop = 0f;
                cell2195.BorderWidthBottom = 0f;
                cell2195.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell2195);

                PdfPCell cellsp1 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cellsp1.Colspan = 2;
                cellsp1.BorderWidthLeft = 0f;
                cellsp1.BorderWidthRight = 0f;
                cellsp1.BorderWidthTop = 0f;
                cellsp1.BorderWidthBottom = 0f;
                cellsp1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cellsp1);

               

                document.Add(table);
                //document.Add(table);

                document.Close();
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
        }

        protected void buttonGo_Click(object sender, EventArgs e)
        {
            VehicleClass();
            txtAmount.Text = "";
            lblRecordType.Visible = false;
            divAddress.Visible = false;
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            lblOld.Text = "";
            allEnabletrue();
            btnDownloadReceipt.Visible = false;
            //String SqlCheck = "SELECT HSRPRecordStaggingID, RTOLocationID, HSRP_StateID, HSRPRecord_CreationDate, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, OwnerName, OwnerFatherName, Address1, MobileNo, EmailID,StickerMandatory, OrderType,OrderStatus, VehicleClass, VehicleType, ManufacturerName, ManufacturerModel, ChassisNo, EngineNo, Manufacturer, VehicleRegNo, NetAmount FROM HSRPRecordsStaggingArea  where hsrp_StateID='" + Session["UserHSRPStateID"].ToString() + "' and vehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "'";
            // dtz = Utils.GetDataTable(SqlCheck, ConnectionString);

            obj.FillDelEngString(txtEngineRegNo.Text.Trim().Trim(), ref dataSetFillDetails);
            if (dataSetFillDetails.Rows.Count > 0)
            {
                lblRecordType.Visible = true;
                divAddress.Visible = true;
                hiddenfieldReferenceValue.Value = "DelhiOldData";
                lblOld.Text = "Old Both Plate";
                txtAuthorizationNo.Text = "0";
                txtOwnerName.Text = dataSetFillDetails.Rows[0]["OwnerName"].ToString();
                txtFatherName.Text = dataSetFillDetails.Rows[0]["OwnerFatherName"].ToString();
                txtVehicleRegNo.Text = dataSetFillDetails.Rows[0]["VehicleRegNo"].ToString();
                txtChassisNo.Text = dataSetFillDetails.Rows[0]["ChassisNo"].ToString(); 
                txtOldAddress.Text = dataSetFillDetails.Rows[0]["OlaAddress"].ToString();
                lblRegDate.Text = dataSetFillDetails.Rows[0]["Reg_Date"].ToString();
                

                string vehicletype = dataSetFillDetails.Rows[0]["vehicletype"].ToString();
               // txtReference.Text = dataSetFillDetails.Rows[0]["Reference"].ToString();
                if (vehicletype == "")
                {
                    VehicleModel();
                }
                else
                { 
                    DropDownListVehicleModel.SelectedItem.Text = dataSetFillDetails.Rows[0]["vehicletype"].ToString();
                    DropDownListVehicleModel.SelectedValue = dataSetFillDetails.Rows[0]["vehicletype"].ToString();
                }

                DropDownListOrderType.SelectedItem.Text = "OLD BOTH PLATES";
                DropDownListOrderType.SelectedValue = "OB";
              

                if (DropDownListVehicleModel.SelectedItem.Text == "THREE WHEELER" || DropDownListVehicleModel.SelectedItem.Text == "LMV" || DropDownListVehicleModel.SelectedItem.Text == "LMV(CLASS)" || DropDownListVehicleModel.SelectedItem.Text == "MCV/HCV/TRAILERS")
                {
                    checkBoxThirdSticker.Checked = true;
                }
                else
                {
                    checkBoxThirdSticker.Checked = false;
                }
                string Status = "OLD BOTH PLATES";

                DataTable dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue + "') as Amount", ConnectionString);
                txtAmount.Text = dt.Rows[0]["Amount"].ToString();

                SQLString = "select FrontPlateID,RearPlateID from RegistrationPlateDetail  where hsrp_stateid ='" + HSRPStateID + "' and vehicletype='" + DropDownListVehicleModel.SelectedItem.Text + "' and vehicleclass='" + DropDownListVehicleClass.SelectedItem.Text + "' and ordertype='" + DropDownListOrderType.SelectedValue + "'";
                DataTable dtPlateSize = Utils.GetDataTable(SQLString, ConnectionString);
                if (dtPlateSize.Rows.Count > 0)
                {
                    if (dtPlateSize.Rows[0]["FrontPlateID"].ToString() != "")
                    {
                        Utils.PopulateDropDownList(DropDownListFrontPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                        DropDownListFrontPlate.SelectedIndex = DropDownListFrontPlate.Items.Count - 1;
                    }
                    if (dtPlateSize.Rows[0]["RearPlateID"].ToString() != "")
                    {
                        Utils.PopulateDropDownList(DropDownListRearPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                        DropDownListRearPlate.SelectedIndex = DropDownListRearPlate.Items.Count - 1;
                    }
                }
                 OrderTypeForOldBoth();


                checkBoxThirdSticker.Enabled = false;

                if (txtChassisNo.Text != "")
                {
                    txtChassisNo.Enabled = false;
                }
                if (txtAuthorizationNo.Text != "0")
                {
                    txtAuthorizationNo.Enabled = false;
                }
                if (txtOwnerName.Text != "")
                {
                    txtOwnerName.Enabled = false;
                }
                if (txtFatherName.Text != "")
                {
                    txtFatherName.Enabled = false;
                }
                if (txtVehicleRegNo.Text != "")
                {
                    txtVehicleRegNo.Enabled = false;
                }

                if (txtChassisNo.Text != "")
                {
                    txtChassisNo.Enabled = false;
                }
                //if (txtReference.Text != "")
                //{
                //    txtReference.Enabled = false;
                //}

            }
            else
            {
                lblOld.Text = "";
                txtAuthorizationNo.Enabled = true;
                txtOwnerName.Enabled = true;
                txtMobileNo.Enabled = true;
                txtVehicleRegNo.Enabled = true;

                DropDownListVehicleClass.Enabled = true;
                DropDownListVehicleModel.Enabled = true;
                DropDownListOrderType.Enabled = true;
                allEnableTrueFalse();


                lblOld.Text = "";
                txtAuthorizationNo.Enabled = true;
                txtOwnerName.Enabled = true;
                txtMobileNo.Enabled = true;
                txtVehicleRegNo.Enabled = true;
                //txtReference.Enabled = true;
                DropDownListVehicleClass.Enabled = true;
                DropDownListVehicleModel.Enabled = true;
                DropDownListOrderType.Enabled = true;
                allEnableTrueFalse();
            }

            lblSucMess.Text = "";
            lblErrMess.Text = "";
        }
      
        public void allEnableTrueFalse()
        {
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            txtOldAddress.Text = "";
            txtAuthorizationNo.Text = "";
            txtOwnerName.Text = "";
            txtFatherName.Text = "";
            txtAddress.Text = "";
            txtEmailID.Text = "";
            txtMobileNo.Text = ""; 
            txtChassisNo.Text = "";
            txtAmount.Text = "";
            chkVip.Enabled = false;
            checkBoxThirdSticker.Checked = false;
            VehicleClass();
            VehicleModel();
            OrderType();
        }
        public void allEnabletrue()
        { 
            div1.Visible = true;
            div2.Visible = true;
            div3.Visible = true; 
        }
        public void VehicleClass()
        {
            DataTable dtcost = new DataTable("tblTest");

            DataRow dr;

            dtcost.Columns.Add("ID", typeof(string));
            dtcost.Columns.Add("Value", typeof(string));

            dr = dtcost.NewRow();

            dr["ID"] = "0";
            dr["Value"] = "--Select Vehicle Class--";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "Transport";
            dr["Value"] = "Transport";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "Non-Transport";
            dr["Value"] = "Non-Transport";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            DropDownListVehicleClass.DataSource = dtcost;
            DropDownListVehicleClass.DataTextField = "Value";
            DropDownListVehicleClass.DataValueField = "ID";
            DropDownListVehicleClass.DataBind();
        }
        public void VehicleModel()
        {
            DataTable dtcost = new DataTable("tblTest");

            DataRow dr;

            dtcost.Columns.Add("ID", typeof(string));
            dtcost.Columns.Add("Value", typeof(string));

            dr = dtcost.NewRow();

            dr["ID"] = "0";
            dr["Value"] = "--Select Vehicle Type--";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "SCOOTER";
            dr["Value"] = "SCOOTER";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "MOTOR CYCLE";
            dr["Value"] = "MOTOR CYCLE";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "TRACTOR";
            dr["Value"] = "TRACTOR";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "THREE WHEELER";
            dr["Value"] = "THREE WHEELER";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "LMV";
            dr["Value"] = "LMV";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "LMV(CLASS)";
            dr["Value"] = "LMV(CLASS)";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["ID"] = "MCV/HCV/TRAILERS";
            dr["Value"] = "MCV/HCV/TRAILERS";
            dtcost.Rows.Add(dr);
            
            DropDownListVehicleModel.DataSource = dtcost;
            DropDownListVehicleModel.DataTextField = "Value";
            DropDownListVehicleModel.DataValueField = "ID";
            DropDownListVehicleModel.DataBind();
        } 
        public void OrderTypeForScooter()
        {
            DataTable dtcost = new DataTable("tblTest");

            DataRow dr;

            dtcost.Columns.Add("ID", typeof(string));
            dtcost.Columns.Add("Value", typeof(string));

            dr = dtcost.NewRow();

            dr["Value"] = "--Select Order Type--";
            dr["ID"] = "--Select Order Type--";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "NEW BOTH PLATES";
            dr["ID"] = "NB";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "OLD BOTH PLATES";
            dr["ID"] = "OB";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();


            dr["Value"] = "DAMAGED BOTH PLATES";
            dr["ID"] = "DB";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "DAMAGED FRONT PLATE";
            dr["ID"] = "DF";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "DAMAGED REAR PLATE";
            dr["ID"] = "DR";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow(); 

            DropDownListOrderType.DataSource = dtcost;
            DropDownListOrderType.DataTextField = "Value";
            DropDownListOrderType.DataValueField = "ID";
            DropDownListOrderType.DataBind();
        }

        public void OrderType()
        {
            DataTable dtcost = new DataTable("tblTest");

            DataRow dr;

            dtcost.Columns.Add("ID", typeof(string));
            dtcost.Columns.Add("Value", typeof(string));

            dr = dtcost.NewRow();

            dr["Value"] = "--Select Order Type--";
            dr["ID"] = "--Select Order Type--";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "NEW BOTH PLATES";
            dr["ID"] = "NB";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "OLD BOTH PLATES";
            dr["ID"] = "OB";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();


            dr["Value"] = "DAMAGED BOTH PLATES";
            dr["ID"] = "DB";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "DAMAGED FRONT PLATE";
            dr["ID"] = "DF";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "DAMAGED REAR PLATE";
            dr["ID"] = "DR";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();

            dr["Value"] = "ONLY STICKER";
            dr["ID"] = "OS";
            dtcost.Rows.Add(dr);

            DropDownListOrderType.DataSource = dtcost;
            DropDownListOrderType.DataTextField = "Value";
            DropDownListOrderType.DataValueField = "ID";
            DropDownListOrderType.DataBind();
        }
        public void OrderTypeForOldBoth()
        {
            DataTable dtcost = new DataTable("tblTest");

            DataRow dr;

            dtcost.Columns.Add("ID", typeof(string));
            dtcost.Columns.Add("Value", typeof(string));

            dr = dtcost.NewRow();

            dr["Value"] = "--Select Order Type--";
            dr["ID"] = "--Select Order Type--";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();
             

            dr["Value"] = "OLD BOTH PLATES";
            dr["ID"] = "OB";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();
                

            DropDownListOrderType.DataSource = dtcost;
            DropDownListOrderType.DataTextField = "Value";
            DropDownListOrderType.DataValueField = "ID";
            DropDownListOrderType.DataBind();
        }

        protected void DropDownListVehicleModel_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (DropDownListVehicleModel.SelectedItem.Text == "THREE WHEELER" || DropDownListVehicleModel.SelectedItem.Text == "LMV" || DropDownListVehicleModel.SelectedItem.Text == "LMV(CLASS)" || DropDownListVehicleModel.SelectedItem.Text == "MCV/HCV/TRAILERS")
            {
                checkBoxThirdSticker.Checked = true;

                
                    OrderType(); 
            }
            else
            {
                checkBoxThirdSticker.Checked = false;
                OrderTypeForScooter();
            }
            if (lblOld.Text == "Old Both Plate")
            {
                OrderTypeForOldBoth();
            }
                

            txtAmount.Text = "";
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            VehicleClass();
            txtAmount.Text = "";
            lblRecordType.Visible = false;
            divAddress.Visible = false;
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            lblOld.Text = "";
            allEnabletrue();
            btnDownloadReceipt.Visible = false;
            //String SqlCheck = "SELECT HSRPRecordStaggingID, RTOLocationID, HSRP_StateID, HSRPRecord_CreationDate, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, OwnerName, OwnerFatherName, Address1, MobileNo, EmailID,StickerMandatory, OrderType,OrderStatus, VehicleClass, VehicleType, ManufacturerName, ManufacturerModel, ChassisNo, EngineNo, Manufacturer, VehicleRegNo, NetAmount FROM HSRPRecordsStaggingArea  where hsrp_StateID='" + Session["UserHSRPStateID"].ToString() + "' and vehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "'";
            // dtz = Utils.GetDataTable(SqlCheck, ConnectionString);

            obj.FillDelString(txtVehicleRegNo.Text.Trim().Trim(), ref dataSetFillDetails);
            if (dataSetFillDetails.Rows.Count > 0)
            {
                lblRecordType.Visible = true;
                divAddress.Visible = true;
                hiddenfieldReferenceValue.Value = "DelhiOldData";
                lblOld.Text = "Old Both Plate";
                txtAuthorizationNo.Text = "0";
                txtOwnerName.Text = dataSetFillDetails.Rows[0]["OwnerName"].ToString();
                txtFatherName.Text = dataSetFillDetails.Rows[0]["OwnerFatherName"].ToString();
                txtEngineRegNo.Text = dataSetFillDetails.Rows[0]["EngineNo"].ToString();
                txtChassisNo.Text = dataSetFillDetails.Rows[0]["ChassisNo"].ToString();
                txtOldAddress.Text = dataSetFillDetails.Rows[0]["OlaAddress"].ToString();
                lblRegDate.Text = dataSetFillDetails.Rows[0]["Reg_Date"].ToString();

                string vehicletype = dataSetFillDetails.Rows[0]["vehicletype"].ToString();
                //txtReference.Text = dataSetFillDetails.Rows[0]["Reference"].ToString();

                if (vehicletype == "")
                {
                    VehicleModel();
                }
                else
                {
                    DropDownListVehicleModel.SelectedItem.Text = dataSetFillDetails.Rows[0]["vehicletype"].ToString();
                    DropDownListVehicleModel.SelectedValue = dataSetFillDetails.Rows[0]["vehicletype"].ToString();
                }
                DropDownListOrderType.SelectedItem.Text = "OLD BOTH PLATES";
                DropDownListOrderType.SelectedValue = "OB";


                if (DropDownListVehicleModel.SelectedItem.Text == "THREE WHEELER" || DropDownListVehicleModel.SelectedItem.Text == "LMV" || DropDownListVehicleModel.SelectedItem.Text == "LMV(CLASS)" || DropDownListVehicleModel.SelectedItem.Text == "MCV/HCV/TRAILERS")
                {
                    checkBoxThirdSticker.Checked = true;
                }
                else
                {
                    checkBoxThirdSticker.Checked = false;
                }
                string Status = "OLD BOTH PLATES";

                DataTable dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue + "') as Amount", ConnectionString);
                txtAmount.Text = dt.Rows[0]["Amount"].ToString();

                SQLString = "select FrontPlateID,RearPlateID from RegistrationPlateDetail  where hsrp_stateid ='" + HSRPStateID + "' and vehicletype='" + DropDownListVehicleModel.SelectedItem.Text + "' and vehicleclass='" + DropDownListVehicleClass.SelectedItem.Text + "' and ordertype='" + DropDownListOrderType.SelectedValue + "'";
                DataTable dtPlateSize = Utils.GetDataTable(SQLString, ConnectionString);
                if (dtPlateSize.Rows.Count > 0)
                {
                    if (dtPlateSize.Rows[0]["FrontPlateID"].ToString() != "")
                    {
                        Utils.PopulateDropDownList(DropDownListFrontPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                        DropDownListFrontPlate.SelectedIndex = DropDownListFrontPlate.Items.Count - 1;
                    }
                    if (dtPlateSize.Rows[0]["RearPlateID"].ToString() != "")
                    {
                        Utils.PopulateDropDownList(DropDownListRearPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                        DropDownListRearPlate.SelectedIndex = DropDownListRearPlate.Items.Count - 1;
                    }
                }
                OrderTypeForOldBoth();


                checkBoxThirdSticker.Enabled = false;

                if (txtChassisNo.Text != "")
                {
                    txtChassisNo.Enabled = false;
                }
                if (txtAuthorizationNo.Text != "0")
                {
                    txtAuthorizationNo.Enabled = false;
                }
                if (txtOwnerName.Text != "")
                {
                    txtOwnerName.Enabled = false;
                }
                if (txtFatherName.Text != "")
                {
                    txtFatherName.Enabled = false;
                }
                if (txtVehicleRegNo.Text != "")
                {
                    txtVehicleRegNo.Enabled = false;
                }

                if (txtChassisNo.Text != "")
                {
                    txtChassisNo.Enabled = false;
                }
                //if (txtReference.Text != "")
                //{
                //    txtReference.Enabled = false;
                //}

            }
            else
            {
                lblOld.Text = "";
                txtAuthorizationNo.Enabled = true;
                txtOwnerName.Enabled = true;
                txtMobileNo.Enabled = true;
                txtVehicleRegNo.Enabled = true;

                DropDownListVehicleClass.Enabled = true;
                DropDownListVehicleModel.Enabled = true;
                DropDownListOrderType.Enabled = true;
                allEnableTrueFalse();


                lblOld.Text = "";
                txtAuthorizationNo.Enabled = true;
                txtOwnerName.Enabled = true;
                txtMobileNo.Enabled = true;
                txtVehicleRegNo.Enabled = true;
                //txtReference.Enabled = true;
                DropDownListVehicleClass.Enabled = true;
                DropDownListVehicleModel.Enabled = true;
                DropDownListOrderType.Enabled = true;
                allEnableTrueFalse();
            }

            lblSucMess.Text = "";
            lblErrMess.Text = "";
        }

        protected void btnSticker_Click(object sender, EventArgs e)
        {
            if (checkBoxThirdSticker.Checked == false)
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow');</script>");

            }
            else
            {
                SQLString = " select a.HSRP_Sticker_LaserCode,a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID where  VehicleRegNo='" + txtVehicleRegNo.Text.Trim() + "' and  a.HSRP_Front_LaserCode is not null and a.HSRP_Front_LaserCode!=''";

                DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
                if (ds.Rows.Count > 0)
                {
                    string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                    if (Stricker == "Y")
                    {
                        string filename = "Sticker" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";


                        String StringField = String.Empty;
                        String StringAlert = String.Empty;

                        StringBuilder bb = new StringBuilder();

                        Document document = new Document(PageSize.A4, 0, 0, 212, 0);
                        document.SetPageSize(new iTextSharp.text.Rectangle(500, 400));
                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                        string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                        PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));



                        //Opens the document:
                        document.Open();

                        PdfPTable table = new PdfPTable(1);

                        table.TotalWidth = 300f;

                        StringBuilder sbtrnasportname = new StringBuilder();
                        string trnasportname = "TRANSPORT DEPARTMENT";


                        PdfPCell cell6 = new PdfPCell(new Phrase(trnasportname, new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell6.PaddingTop = 8f;
                        cell6.BorderColor = BaseColor.WHITE;
                        cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell6);
                        StringBuilder sb = new StringBuilder();
                        StringBuilder sb1 = new StringBuilder();


                        string statename = ds.Rows[0]["statetext"].ToString().ToUpper();
                        string HSRPStateName = ds.Rows[0]["HSRPStateName"].ToString().ToUpper();
                        if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell.PaddingTop = 8f;
                            cell.BorderColor = BaseColor.WHITE;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }

                        else if (HSRPStateName == "HARYANA")
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell.PaddingTop = 8f;
                            cell.BorderColor = BaseColor.WHITE;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }
                        else
                        {

                            PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 20f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell.PaddingTop = 8f;
                            cell.BorderColor = BaseColor.WHITE;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }


                        StringBuilder sbVehicleRegNo = new StringBuilder();

                        string VehicleRegNo = ds.Rows[0]["VehicleRegNo"].ToString().ToUpper();



                        PdfPCell cell2 = new PdfPCell(new Phrase(VehicleRegNo, new iTextSharp.text.Font(basefont, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell2.PaddingTop = 8f;
                        cell2.BorderColor = BaseColor.WHITE;
                        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell2);
                        StringBuilder sbHSRP_Front_LaserCode = new StringBuilder();
                        StringBuilder sbHSRP_Rear_LaserCode = new StringBuilder();



                        string HSRP_Front_LaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString().ToUpper();
                        string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();
                        PdfPCell cell3 = new PdfPCell(new Phrase("" + HSRP_Front_LaserCode + " - " + HSRP_Rear_LaserCode, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell3.PaddingTop = 8f;
                        cell3.PaddingLeft = 193f;
                        cell3.BorderColor = BaseColor.WHITE;
                        cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell3);

                        StringBuilder sbEngineNo = new StringBuilder();
                        string EngineNo = "ENGINE NO - " + ds.Rows[0]["EngineNo"].ToString().ToUpper();


                        PdfPCell cell4 = new PdfPCell(new Phrase(EngineNo, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell4.PaddingTop = 8f;
                        cell4.PaddingLeft = 193f;
                        cell4.BorderColor = BaseColor.WHITE;
                        cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell4);

                        StringBuilder sbChassisNo = new StringBuilder();
                        string ChassisNo = "CHASIS NO - " + ds.Rows[0]["ChassisNo"].ToString().ToUpper();
                        PdfPCell cell5 = new PdfPCell(new Phrase(ChassisNo, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell5.PaddingTop = 8f;
                        cell5.PaddingLeft = 193f;
                        cell5.BorderColor = BaseColor.WHITE;
                        cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell5);

                        document.Add(table);

                        document.Close();
                        HttpContext context = HttpContext.Current;

                        context.Response.ContentType = "Application/pdf";
                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                        context.Response.WriteFile(PdfFolder);
                        context.Response.End();
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                    }
                }
            }
        }

        protected void btnYellowSticker_Click(object sender, EventArgs e)
        {
            
             
                if (checkBoxThirdSticker.Checked == false)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow');</script>");

                }
                else
                {
                    SQLString = " select a.HSRP_Sticker_LaserCode,a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID where  VehicleRegNo='" + txtVehicleRegNo.Text.Trim() + "' and  a.HSRP_Front_LaserCode is not null and a.HSRP_Front_LaserCode!=''";
                    DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
    
                    
                    if (ds.Rows.Count > 0)
                    {
                        string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                        if (Stricker == "Y")
                        {
                            string filename = "Sticker" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";


                            String StringField = String.Empty;
                            String StringAlert = String.Empty;

                            StringBuilder bb = new StringBuilder();

                            Document document = new Document(PageSize.A4, 0, 0, 220, 0);

                            document.SetPageSize(new iTextSharp.text.Rectangle(400, 300));
                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));



                            //Opens the document:
                            document.Open();

                            //Adds content to the document:
                            // document.Add(new Paragraph("Ignition Log Report"));
                            PdfPTable table = new PdfPTable(1);
                            //actual width of table in points
                            table.TotalWidth = 300f;

                            //iTextSharp.text.Font ft = new iTextSharp.text.Font();
                            //FontFactory.Register(@"../bin/PRSANSR.TTF", "PRMirror");
                            //ft = FontFactory.GetFont("PRMirror");


                            // fontpath = Environment.GetEnvironmentVariable("http://localhost:51047") + "C:\\Users\\Ambrish Singh\\Desktop\\ambrish\\HSRPViewOrder new printer according sticker\\HSRP\\bin\\PRSANSR.TTF";

                            fontpath = ConfigurationManager.AppSettings["DataFolder"].ToString() + "PRSANSR.TTF";
                            basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);

                            StringBuilder sbtrnasportname = new StringBuilder();
                            string trnasportname = "TRANSPORT DEPARTMENT";

                            for (int i = trnasportname.Length - 1; i >= 0; i--)
                            {
                                sbtrnasportname.Append(trnasportname[i].ToString());
                            }

                            //fix the absolute width of the table
                            PdfPCell cell6 = new PdfPCell(new Phrase(sbtrnasportname.ToString(), new iTextSharp.text.Font(basefont, 17f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell6.PaddingRight = 240f;
                            cell6.BorderColor = BaseColor.WHITE;
                            cell6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell6);
                            StringBuilder sb = new StringBuilder();
                            StringBuilder sb1 = new StringBuilder();
                            string statename = ds.Rows[0]["statetext"].ToString().ToUpper();

                            for (int i = statename.Length - 1; i >= 0; i--)
                            {
                                sb.Append(statename[i].ToString());
                            }

                            string HSRPStateName = ds.Rows[0]["HSRPStateName"].ToString().ToUpper();

                            for (int i = HSRPStateName.Length - 1; i >= 0; i--)
                            {
                                sb1.Append(HSRPStateName[i].ToString());
                            }
                            if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                cell.PaddingRight = 240f;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }
                            else if (HSRPStateName == "HARYANA")
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell.PaddingRight = 243f;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 18f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                //cell.PaddingRight = 240f;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            } 
                            StringBuilder sbVehicleRegNo = new StringBuilder();

                            string VehicleRegNo = ds.Rows[0]["VehicleRegNo"].ToString().ToUpper();

                            for (int i = VehicleRegNo.Length - 1; i >= 0; i--)
                            {
                                sbVehicleRegNo.Append(VehicleRegNo[i].ToString());
                            }

                            PdfPCell cell2 = new PdfPCell(new Phrase(sbVehicleRegNo.ToString(), new iTextSharp.text.Font(basefont, 27f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell2.PaddingTop = 8f;
                            cell2.BorderColor = BaseColor.WHITE;
                            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell2);
                            StringBuilder sbHSRP_Front_LaserCode = new StringBuilder();
                            StringBuilder sbHSRP_Rear_LaserCode = new StringBuilder();
                            string HSRP_Front_LaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString().ToUpper();

                            for (int i = HSRP_Front_LaserCode.Length - 1; i >= 0; i--)
                            {
                                sbHSRP_Front_LaserCode.Append(HSRP_Front_LaserCode[i].ToString());
                            }

                            string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();

                            for (int i = HSRP_Rear_LaserCode.Length - 1; i >= 0; i--)
                            {
                                sbHSRP_Rear_LaserCode.Append(HSRP_Rear_LaserCode[i].ToString());
                            }

                            PdfPCell cell3 = new PdfPCell(new Phrase("" + sbHSRP_Rear_LaserCode.ToString() + " - " + sbHSRP_Front_LaserCode.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell3.PaddingTop = 8f;
                            cell3.BorderColor = BaseColor.WHITE;
                            cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell3);

                            StringBuilder sbEngineNo = new StringBuilder();
                            string EngineNo = "ENGINE NO - " + ds.Rows[0]["EngineNo"].ToString().ToUpper();

                            for (int i = EngineNo.Length - 1; i >= 0; i--)
                            {
                                sbEngineNo.Append(EngineNo[i].ToString());
                            }


                            PdfPCell cell4 = new PdfPCell(new Phrase(sbEngineNo.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell4.PaddingTop = 8f;
                            cell4.PaddingRight = 245f;
                            cell4.BorderColor = BaseColor.WHITE;
                            cell4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell4);

                            StringBuilder sbChassisNo = new StringBuilder();
                            string ChassisNo = "CHASIS NO - " + ds.Rows[0]["ChassisNo"].ToString().ToUpper();

                            for (int i = ChassisNo.Length - 1; i >= 0; i--)
                            {
                                sbChassisNo.Append(ChassisNo[i].ToString());
                            }

                            PdfPCell cell5 = new PdfPCell(new Phrase(sbChassisNo.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell5.PaddingTop = 8f;
                            cell5.PaddingRight = 245f;
                            cell5.BorderColor = BaseColor.WHITE;
                            cell5.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell5);

                            document.Add(table);

                            document.Close();
                            HttpContext context = HttpContext.Current;

                            context.Response.ContentType = "Application/pdf";
                            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                            context.Response.WriteFile(PdfFolder);
                            context.Response.End();
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                        }
                    }
                }
        }
         
    }
}