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



namespace HSRP.EmbossingData
{
    public partial class OtherLocationInvoice : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string SQlQuery = string.Empty;
        string ExicseAmount = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        string strStateId = string.Empty;
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {


                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {

                        if (UserType == "0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            //buildGrid();
                        }

                        //ShowGrid();
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        public void FileDetail()
        {

        }

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");

                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);

                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID and a.activestatus='Y' where UserRTOLocationMapping.UserID='" + UserID + "' order by a.rtolocationname ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);

                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }
                    lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));

                }
            }
        }

        #endregion

        DataTable dtInvoiceData = null;
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        DataTable dt = new DataTable();
        string strInvoiceNo = null;
        private void GetRecords()
        {
            strInvoiceNo = string.Empty;
            dtInvoiceData = new DataTable();
            if (!string.IsNullOrEmpty(txtInvoiceNo.Text))
            {
                string SQLString = "select row_number() over(order by VehicleClass,VehicleType,VehicleRegNo) as SN,ChallanNo,challandate,convert(varchar,HSRPRecord_CreationDate,103) as HSRPRecord_CreationDate,VehicleType,convert(varchar,OrderEmbossingDate,103) as OrderEmbossingDate,hsrp_stateid,rtolocationid" +
                                    ",hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus " +
                                    "from hsrprecords where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and [ChallanNo]='" + txtInvoiceNo.Text + "' order by VehicleClass,VehicleType,VehicleRegNo";
                
                dtInvoiceData = Utils.GetDataTable(SQLString, CnnString);
                strInvoiceNo = txtInvoiceNo.Text;
            }
            else if (!dropdownDuplicateFIle.SelectedItem.Text.Equals("--Select Challan No--"))
            {
                string SQLString = "select row_number() over(order by VehicleClass,VehicleType,VehicleRegNo) AS SN,ChallanNo,challandate,convert(varchar,HSRPRecord_CreationDate,103) as HSRPRecord_CreationDate,VehicleType,convert(varchar,OrderEmbossingDate,103) as OrderEmbossingDate,hsrp_stateid,rtolocationid" +
                                    ",hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus " +
                                    "from hsrprecords where hsrp_stateid='"+DropDownListStateName.SelectedValue.ToString()+"' and [ChallanNo]='" + dropdownDuplicateFIle.SelectedItem.Text + "' order by VehicleClass,VehicleType,VehicleRegNo";
                dtInvoiceData = Utils.GetDataTable(SQLString, CnnString);
                strInvoiceNo = dropdownDuplicateFIle.SelectedItem.Text;
            }
            else
            {
                return;
            }

           
                if (dtInvoiceData.Rows.Count > 0)
                {
                   
                    btnChalan.Visible = true;
                   
                  //  Button2.Visible = true;
                   
                }
                else
                {
                    btnChalan.Visible = false;
                   
                  //  Button2.Visible = false;
                    lblErrMsg.Text = "Record Not Found";
                    

                }

           
        }


       
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

        }
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            txtInvoiceNo.Text = string.Empty;
            try
            {                
                    labelOrderStatus.Visible = true;
                    dropdownDuplicateFIle.Visible = true;

                    string type = "1";

                    string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
                    String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
                    string MonTo = ("0" + StringAuthDate[0]);
                    string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                    String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
                    String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                    //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                    string AuthorizationDate = From + " 00:00:00";
                    String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
                    string Mon = ("0" + StringOrderDate[0]);
                    string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                    String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                    String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                    string ToDate = From1 + " 23:59:59";


                    string strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
                    string strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 23:59:59";

                    SQLString = "select  Distinct ChallanNo   from hsrprecords  where hsrp_StateID='" + DropDownListStateName.SelectedValue +
                        "' and rtolocationID='" + dropDownListClient.SelectedValue + "' and  ChallanNo is not null and ChallanDate between '" +
                        strFrmDateString + "' and  '" + strToDateString + "' ";


                
                labelOrderStatus.Visible = true;
                dropdownDuplicateFIle.Visible = true;
                    Utils.PopulateDropDownList(dropdownDuplicateFIle, SQLString.ToString(), CnnString, "--Select Challan No--");
               
                
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
            }

        }
        
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            GetRecords();
            if (dtInvoiceData.Rows.Count > 0)
            {
                gvRecords.DataSource = dtInvoiceData;
                gvRecords.DataBind();
                btnChalan.Visible = true;
                Button1.Visible = true;
            }
            else
            {
                btnChalan.Visible = false;
                Button1.Visible = false;
                gvRecords.DataSource = null;
                gvRecords.DataBind();
            }
        }


        protected void btnChalan_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validation
             //   Button2.Visible = true;
                if (string.IsNullOrEmpty(txtInvoiceNo.Text) && (dropdownDuplicateFIle.SelectedIndex<=0))
                {
                    Response.Write("<script> alert('Please Enter Invoice No.')</script>");
                    return;
                }
               
                #endregion
                GetRecords();
                string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                string RtoName = string.Empty;
                RtoName = dropDownListClient.SelectedItem.ToString();
                HttpContext context = HttpContext.Current;
                string filename = "HSRP-Challan" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

                string SQLString = String.Empty;
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                StringBuilder bb = new StringBuilder();

                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();



                Int64 totalamount = 0;

                float exciseduty = 0;
                object Exicse;
                object Vatamt;
                object TotalAmount;
                object TotalAMT;
                object Qty;
                object SecondaryCess;
                bool Check = false;
                object Educess;
                //Opens the document:
                string strHsrpRecordId = string.Empty;

                string vehicle = string.Empty;
                string strInvDate =Convert.ToDateTime(dtInvoiceData.Rows[0]["ChallanDate"]).ToString("dd/MM/yyyy");
                #region Set InvoiceNo
                //string strSelectEmbStation = "SELECT DISTINCT [NAVEMBID],[EmbCenterName],address1,city FROM [vw_RTOLocationWiseEmbosingCenters] WHERE RTOLocationId='" + dropDownListClient.SelectedValue + "'";

                string strSelectEmbStation = @"select em.Emb_Center_Id as [NAVEMBID],em.[EmbCenterName],em.Address1,em.city  from EmbossingCenters em 
                where Emb_Center_Id=(select distinct navembid from hsrprecords where challanno='" + dropdownDuplicateFIle.SelectedItem.Text + "')";
                DataTable dtEmbData = Utils.GetDataTable(strSelectEmbStation, CnnString);

                if (dtEmbData.Rows.Count <= 0)
                {
                    lblErrMsg.Text = "Embossing Center Not found";
                    return;
                }
                string strEmbStationName = dtEmbData.Rows[0]["EmbCenterName"].ToString();
                string strEmbAddress = dtEmbData.Rows[0]["address1"].ToString();
                string strEmbCity = dtEmbData.Rows[0]["city"].ToString();
                string strEmbId = dtEmbData.Rows[0]["NAVEMBID"].ToString();                
               
                for (int i = 0; i < dtInvoiceData.Rows.Count; i++)
                {

                    string lblVehicleRegNo = dtInvoiceData.Rows[i]["VehicleRegNo"].ToString();
                    strInvDate = Convert.ToDateTime(dtInvoiceData.Rows[0]["ChallanDate"]).ToString("dd/MM/yyyy");
                    string OrderStatus = dtInvoiceData.Rows[i]["OrderStatus"].ToString();

                    string strRecordId = dtInvoiceData.Rows[i]["hsrprecordid"].ToString();
                    string flaser = dtInvoiceData.Rows[i]["HSRP_Front_LaserCode"].ToString();
                    string rlaser = dtInvoiceData.Rows[i]["hsrp_rear_lasercode"].ToString();
                    if (strHsrpRecordId == "")
                    {
                        strHsrpRecordId = "'" + strRecordId + "'";
                    }
                    else
                    {
                        strHsrpRecordId = strHsrpRecordId + ",'" + strRecordId + "'";
                    }

                }
                string strSelInvoiceData = "Select transporter,lorryno from invoicemaster where InvoiceNo='"+strInvoiceNo+"'";
                DataTable dtInvoice = Utils.GetDataTable(strSelInvoiceData,CnnString);

                string strStateId=dtInvoiceData.Rows[0]["hsrp_stateid"].ToString();
                string strRTOLocationId = dtInvoiceData.Rows[0]["RTOLocationId"].ToString();

                DataTable GetAddress;
                string Address;

               // string strTransporter=dtInvoice.Rows[0]["Transporter"].ToString();
               // string strLorryNo = dtInvoice.Rows[0]["LorryNo"].ToString();
                GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + strStateId + "'", CnnString);

                if ((GetAddress.Rows[0]["pincode"].ToString() != "") || (GetAddress.Rows[0]["pincode"] != null))
                {
                    Address = " - " + GetAddress.Rows[0]["pincode"];
                }
                else
                {
                    Address = "";
                }
                string sqlstring = string.Empty;

                string strSelectAddress = "SELECT RTOLocationAddress FROM [RTOLocation] WHERE RTOLocationId='" + strRTOLocationId + "'";
                DataTable dtAddress = Utils.GetDataTable(strSelectAddress, CnnString);

                string strRTOAddress = dtAddress.Rows[0]["RTOLocationAddress"].ToString();

                PdfPTable table2 = new PdfPTable(10);
                PdfPTable table1 = new PdfPTable(10);
                PdfPTable table = new PdfPTable(10);
               PdfPTable table3 = new PdfPTable(2);

                //actual width of table in points
                table.TotalWidth = 1000f;
                HSRPStateID = DropDownListStateName.SelectedValue;
                string OldRegPlate = string.Empty;



                DataTable dt = new DataTable();

               // SQlQuery = "select vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*)qty,round((sum(EXCISEBASIC)/count(*)),3) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and ordertype in ('DF') and HsrpRecordID in(" + strHsrpRecordId + ") group by vehicletype union   select vehicletype+' (Damage Rear)',count(*)qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and  ordertype in ('DR') group by vehicletype union select vehicletype+' (Both Plates)',(count(*)*2)qty,round((sum(EXCISEBASIC)/(count(*)*2)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and ordertype in ('NB','OB','DB') group by vehicletype";
                    // Declare an object variable.
                //SQlQuery= "select vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*)qty,round((sum(EXCISEBASIC)/count(*)),3) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and ordertype in ('DF') and HsrpRecordID in(" + strHsrpRecordId + ") group by vehicletype union   select vehicletype+' (Damage Rear)',count(*)qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and  ordertype in ('DR') group by vehicletype union select vehicletype+' (Both Plates)',(count(*)*2)qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and ordertype in ('NB','OB','DB') group by vehicletype";

                SQlQuery = "select vehicletype+' (Damage Front)' as DescriptionOfGoods,count(*) as qty,round((sum(EXCISEBASIC)/count(*)),3) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess  from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and ordertype in ('DF') and HsrpRecordID in(" + strHsrpRecordId + ") group by vehicletype union   select vehicletype+' (Damage Rear)',count(*) as qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and  ordertype in ('DR') group by vehicletype union select vehicletype+' (Both Plates)',(count(*)*2)/2 qty,round((sum(EXCISEBASIC)/count(*)),2) amt,round(sum(EXCISEBASIC),2)bamt,round(sum(Vat_Amount),2)Vatamt,round(sum(EXCISEAMT),2) excise,round(sum(cessamt),2)cess,round(sum(shecessamt),2)shecess   from hsrprecords  where hsrp_stateid='" + HSRPStateID + "' and HsrpRecordID in(" + strHsrpRecordId + ") and ordertype in ('NB','OB','DB') group by vehicletype";
                
                dt = Utils.GetDataTable(SQlQuery, CnnString);




                BAL obj = new BAL();
                string UserID = Session["UID"].ToString();

                string sqluser = "select UserLoginName from users where userid='" + UserID + "'";
                string username = Utils.getDataSingleValue(sqluser, CnnString, "UserLoginName");
                decimal dTotalAmount = 0;
                decimal Amount;
                decimal SubTotal = 0;

                decimal Vat;
                if (true)
                {
                    #region Upper Part of PDF

                    strStateId = DropDownListStateName.SelectedValue;
                    string strLocId = dropDownListClient.SelectedValue;



                    DataTable dataSetFillHSRPDeliveryChallan = new DataTable();


                    string strEccNo = GetAddress.Rows[0]["ExciseNo"].ToString();
                    string strDivision = GetAddress.Rows[0]["Division"].ToString();
                    string strRange = GetAddress.Rows[0]["Range"].ToString();
                    string strCommissionerate = GetAddress.Rows[0]["Commissionerate"].ToString();
                    string strTin = GetAddress.Rows[0]["Tinno"].ToString();
                    string strTariffHead = GetAddress.Rows[0]["CH"].ToString();

                    GenerateCell(table, 10, 0, 0, 0, 0, 1, 1, "", 50, 0);

                    GenerateCell(table, 10, 1, 1, 1, 1, 1, 1, "Retail Invoice(For removal of Excisable goods Under Rule-11 of C E Rules-2002)", 30, 0);

                    string strCompName = GetAddress.Rows[0]["CompanyName"].ToString();
                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, strCompName, 0, 0);

                    GenerateCell(table, 3, 0, 1, 0, 0, 1, 0, "Invoice No", 0, 0);

                    GenerateCell(table, 4, 0, 1, 0, 0, 0, 1, "Dated", 0, 0);

                    string strPhone = string.Empty;
                    if (DropDownListStateName.SelectedValue.Equals("9"))
                    {
                        strPhone = "Phone No : 04042226000";
                    }
                    else if (DropDownListStateName.SelectedValue.Equals("2"))
                    {

                    }
                    else if (DropDownListStateName.SelectedValue.Equals("4"))
                    {

                    }
                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 1, GetAddress.Rows[0]["Address1"].ToString() + " , " +
                    GetAddress.Rows[0]["city"].ToString() + Address.ToString() + " E-Mail : info@linkutsav.com \r\n" + strPhone, 0, 0);


                    if (HSRPStateID == "9")
                    {
                        GenerateCell(table, 3, 0, 1, 0, 0, 1, 1, "AP/ " + strInvoiceNo, 0, 0);
                    }
                    else if (HSRPStateID == "11")
                    {
                        GenerateCell(table, 3, 0, 1, 0, 0, 1, 1, "TG/ " + strInvoiceNo, 0, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 0, 1, 0, 0, 1, 1, strInvoiceNo, 0, 0);
                    }
                    

                    GenerateCell(table, 4, 0, 1, 0, 0, 0, 1, strInvDate, 0, 0);

                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 1, "", 0, 0);

                    GenerateCell(table, 3, 0, 1, 0, 0, 0, 1, "", 0, 0);

                    GenerateCell(table, 4, 0, 1, 0, 0, 0, 1, "Other Reference(s)", 0, 0);

                    if (!strEmbStationName.Equals("Chintal"))
                    {
                        GenerateCell(table, 3, 1, 1, 1, 0, 0, 1, "Embossing Station at:", 20, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 1, 1, 1, 0, 0, 1, "", 20, 0);
                    }

                    GenerateCell(table, 7, 0, 1, 1, 1, 1, 0, "Registration Details", 0, 0);


                    if (!strEmbStationName.Equals("Chintal"))
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 1, strEmbStationName + " RTO PREMISES", 20, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 1, 1, 0, 1, 0, 1, "", 20, 0);
                    }
                    GenerateCell(table, 1, 0, 0, 0, 1, 0, 1, "", 20, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 0, 0, "Company's", 20, 100);


                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);

                    GenerateCell(table, 1, 0, 0, 0, 1, 0, 0, "ECC No.", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 0, 1, strEccNo, 0, 0);




                    if (strEmbStationName.Equals("Chintal"))
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, strEmbAddress, 0, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);
                    }

                    GenerateCell(table, 1, 0, 0, 0, 1, 0, 0, "Division-", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 0, 1, strDivision, 0, 0);




                    if (strEmbStationName.Equals("Chintal"))
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, strEmbCity, 0, 0);
                    }
                    else
                    {
                        GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, strRTOAddress, 0, 0);
                    }


                    GenerateCell(table, 1, 0, 0, 0, 1, 0, 0, "Range", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 0, 1, strRange, 0, 0);

                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 0, 0, "Comm.", 0, 0);
                    GenerateCell(table, 6, 1, 1, 0, 0, 0, 1, strCommissionerate, 0, 0);




                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 1, 0, 0, "TIN-", 0, 0);
                    GenerateCell(table, 6, 1, 1, 1, 1, 0, 1, strTin, 0, 0);


                    GenerateCell(table, 3, 1, 1, 0, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 7, 0, 1, 0, 0, 0, 0, "Transporter:" +"strTransporter", 0, 0);




                    GenerateCell(table, 3, 1, 1, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 7, 0, 1, 1, 0, 0, 0, "Lorry No: " +"strLorryNo", 0, 0);



                    #endregion

                    #region BlankRow
                    GenerateCell(table, 10, 1, 1, 1, 0, 0, 1, "", 20, 0);
                    #endregion

                    #region Column Heading Creation

                    GenerateCell(table, 1, 1, 0, 1, 0, 1, 0, "SI.NO.", 0, 0);

                    GenerateCell(table, 5, 1, 1, 1, 0, 1, 0, "Description of Goods", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 1, 0, "Tariff Head", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 1, 0, "Qty-:Set", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Rate", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 1, 0, "Amount (Rs.)", 0, 0);


                    #endregion

                    // 0=Bold,1=Normal
                    #region 1st Row

                    GenerateCell(table, 1, 1, 0, 1, 0, 1, 1, "1", 0, 0);

                    GenerateCell(table, 5, 1, 1, 1, 0, 1, 0, "HSRP SET FOR:", 0, 0);
                    GenerateCell(table, 1, 0, 0, 1, 0, 0, 0, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 0, 0, "", 0, 0);
                    for (int i = 0; i < 2; i++)
                    {
                        GenerateCell(table, 1, 0, 1, 1, 0, 0, 0, "", 0, 0);
                    }
                    #endregion

                    #region 2nd Row

                    Exicse = dt.Compute("sum(excise)", "");
                    ExicseAmount = String.Format("{0:0.00}", Exicse);



                    TotalAmount = dt.Compute("sum(bamt)", "");
                    Vatamt = dt.Compute("sum(Vatamt)", "");
                    TotalAMT = dt.Compute("sum(amt)", "");
                    Qty = dt.Compute("sum(qty)", "");
                    Educess = dt.Compute("sum(cess)", "");
                    SecondaryCess = dt.Compute("sum(shecess)", "");
                    string Newamount = String.Format("{0:0.00}", TotalAmount);
                    string VatamtNew = String.Format("{0:0.00}", Vatamt);
                    string strTotalAMT = String.Format("{0:0.00}", TotalAMT);
                    string strEducess = String.Format("{0:0.00}", Educess);
                    string strSecondaryCess = string.Format("{0:0.00}", SecondaryCess);

                    for (int iResult = 0; iResult < dt.Rows.Count; iResult++)
                    {
                        GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 5, 1, 1, 0, 0, 1, 0, dt.Rows[iResult]["DescriptionOfGoods"].ToString(), 0, 0);
                     
                        GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, strTariffHead, 0, 0);
                        
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, dt.Rows[iResult]["qty"].ToString(), 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, dt.Rows[iResult]["amt"].ToString(), 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, String.Format("{0:0.00}", dt.Rows[iResult]["bamt"]), 0, 0);

                        Amount = Convert.ToDecimal(dt.Rows[iResult]["bamt"].ToString());
                    }


                    decimal PreTotalAmount = 0;
                    PreTotalAmount = PreTotalAmount + Convert.ToDecimal(ExicseAmount) + Convert.ToDecimal(Newamount) + Convert.ToDecimal(strEducess) + Convert.ToDecimal(strSecondaryCess);
                    dTotalAmount = PreTotalAmount + Convert.ToDecimal(VatamtNew);

                    Int64 Damount = Convert.ToInt64(Math.Round(dTotalAmount, 2));

                    #endregion








                    #region Blank


                    for (int j = 0; j < 4; j++)
                    {
                        GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                        GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                        GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);

                    }
                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Sub Total", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 2, 1, Newamount, 0, 0);

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);


                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Excise Duty", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, GetAddress.Rows[0]["ExciseDuty"].ToString() + "%", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, ExicseAmount.ToString(), 0, 0);

                    //GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Educational Cess", 0, 0);
                    //GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, GetAddress.Rows[0]["EducationCess"].ToString() + "%", 0, 0);
                    //GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, strEducess, 0, 0);


                    //GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Secondary Educational Cess", 0, 0);
                    //GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, GetAddress.Rows[0]["HEducationCess"].ToString() + "%", 0, 0);
                    //GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    //GenerateCell(table, 1, 0, 1, 0, 0, 2, 1, strSecondaryCess, 0, 0);



                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "Total", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 1, 2, 1, PreTotalAmount.ToString(), 0, 0);



                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    GenerateCell(table, 5, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 1, 1, "", 0, 0);



                    GenerateCell(table, 1, 1, 0, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 5, 1, 1, 0, 1, 1, 1, "VAT", 0, 0);
                    GenerateCell(table, 1, 0, 0, 0, 1, 1, 1, GetAddress.Rows[0]["CSTVAT"].ToString() + "%", 0, 0);
                    GenerateCell(table, 1, 1, 1, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 1, 1, "", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 1, 2, 1, Vatamt.ToString(), 0, 0);


                    #endregion

                    #region Bottom

                    #region Total Price

                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);

                    HSRP.Transaction.InvoiceTransaction.NumToWord trans = new HSRP.Transaction.InvoiceTransaction.NumToWord();
                    string totalinwords = trans.changeNumericToWords(Damount);

                    GenerateCell(table, 8, 1, 1, 0, 0, 2, 1, "Total Invoice Value", 0, 0);

                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 0, dTotalAmount.ToString(), 0, 0);
                    GenerateCell(table, 1, 1, 0, 0, 0, 1, 1, "", 0, 0);
                    GenerateCell(table, 8, 1, 1, 0, 0, 2, 1, "Total RoundOff Invoice Value", 0, 0);
                    GenerateCell(table, 1, 0, 1, 0, 0, 2, 0, Damount.ToString(), 0, 0);
                    #endregion

                    #region Declaration

                    GenerateCell(table, 10, 1, 1, 1, 0, 0, 1, "Declaration: Certified that the particulars given above are true & correct and the amount indicated represents the price actually charged and there is no additional flow of money either directly or indirectly from the buyer.", 0, 0);

                    #endregion

                    #region Vat

                    double dnetval = (Damount * 100.0) / (114.5);

                    double dVatval = Math.Round(Damount - dnetval);

                    string strVatinWords = trans.changeNumericToWords(dVatval);

                    GenerateCell(table, 4, 1, 0, 1, 0, 0, 1, "", 0, 0);
                    GenerateCell(table, 6, 1, 1, 1, 0, 2, 1, "E. & O.E", 0, 0);

                    #endregion




                    #region LinkAutoTech

                    GenerateCell(table, 4, 1, 0, 0, 0, 0, 1, "Total amount in words : " + totalinwords, 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 0, 2, 0, "For " + GetAddress.Rows[0]["CompanyName"].ToString(), 0, 0);

                    #endregion

                    #region Blank

                    //1

                    for (int i = 0; i < 5; i++)
                    {
                        GenerateCell(table, 4, 1, 0, 0, 0, 1, 0, "", 0, 0);

                        GenerateCell(table, 6, 1, 1, 0, 0, 2, 0, "", 0, 0);

                    }

                    #endregion

                    #region Sign

                    GenerateCell(table, 4, 1, 0, 0, 1, 1, 0, "", 0, 0);

                    GenerateCell(table, 6, 1, 1, 0, 1, 2, 0, "Authorized Signatory", 0, 0);

                    #endregion

                    #endregion

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);
                    GenerateCell(table3, 1, 0, 0, 0, 0, 0, 0, "", 30, 0);

                   
                   

                   // GenerateCell(table3, 2, 0, 0, 0, 0, 0, 1, "Created By :- " + username, 30, 0);


                    GenerateCell(table3, 2, 0, 0, 0, 0, 2, 1, UserID, 0, 0);





                    document.Add(table1);
                    document.Add(table);

                    document.Add(table3);
                    
                    document.NewPage();

                    document.Close();
                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();

                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message;
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
               // newCellPDF.Width = iRowWidth;
            }
            table.AddCell(newCellPDF);
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceNo.Text) && (dropdownDuplicateFIle.SelectedIndex <= 0))
            {
                Response.Write("<script> alert('Please Enter Invoice No.')</script>");
                return;
            }
            HttpContext context = HttpContext.Current;
            string filename = "HSRP-INVOICE" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";

            string SQLString = String.Empty;
            String StringField = String.Empty;
            String StringAlert = String.Empty;

            StringBuilder bb = new StringBuilder();

            //Creates an instance of the iTextSharp.text.Document-object:
            Document document = new Document();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);


            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

            //Opens the document:
            document.Open();



            //Opens the document:
            GetRecords();
            strStateId = dtInvoiceData.Rows[0]["hsrp_stateid"].ToString();
            string strRTOLocationId = dtInvoiceData.Rows[0]["RTOLocationId"].ToString();

            for (int i = 0; i < dtInvoiceData.Rows.Count; i++)
            {
                //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                // tmpbillno = Session["UID"].ToString()+ System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() ;
               

                string lblVehicleRegNo = dtInvoiceData.Rows[i]["VehicleRegNo"].ToString();

                string OrderStatus = dtInvoiceData.Rows[i]["OrderStatus"].ToString();

                string strRecordId = dtInvoiceData.Rows[i]["hsrprecordid"].ToString();
                string flaser = dtInvoiceData.Rows[i]["HSRP_Front_LaserCode"].ToString();
                string rlaser = dtInvoiceData.Rows[i]["hsrp_rear_lasercode"].ToString();
                String HSRPRecordID = dtInvoiceData.Rows[i]["hsrprecordid"].ToString();
                    //  string OrderStatus = e.Item["OrderStatus"].ToString();
                    PdfPTable table2 = new PdfPTable(7);
                    PdfPTable table1 = new PdfPTable(7);
                    PdfPTable table = new PdfPTable(7);

                    //actual width of table in points
                    table.TotalWidth = 1000f;
                    HSRPStateID = DropDownListStateName.SelectedValue;
                    string OldRegPlate = string.Empty;
                    //if (CheckBoxOldPlate.Checked == true)
                    //{
                    //    OldRegPlate = "Y";
                    //}
                    //else
                    //{
                    //    OldRegPlate = "N";
                    //}

                    DataTable GetAddress;
                    string Address;
                    GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + strStateId + "'", CnnString);

                    if (GetAddress.Rows[0]["pincode"] != "" || GetAddress.Rows[0]["pincode"] != null)
                    {
                        Address = " - " + GetAddress.Rows[0]["pincode"];
                    }
                    else
                    {
                        Address = "";
                    }



                    BAL obj = new BAL();
                    //  HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                    //  string OrderStatus = Request.QueryString["Status"].ToString();
                    string UserID = Session["UID"].ToString();

                   
                        DataTable dataSetFillHSRPDeliveryChallan = new DataTable();

                        if (obj.FillHSRPRecordDeliveryChallan2(HSRPRecordID, ref dataSetFillHSRPDeliveryChallan))
                        {

                            //fix the absolute width of the table
                            PdfPCell cell1211 = new PdfPCell(new Phrase("Original", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1211.Colspan = 1;
                            cell1211.BorderWidthLeft = 1f;
                            cell1211.BorderWidthRight = 1f;
                            cell1211.BorderWidthTop = 1f;
                            cell1211.BorderWidthBottom = 1f;

                            cell1211.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table1.AddCell(cell1211);
                            PdfPCell cell12111 = new PdfPCell(new Phrase("INVOICE", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12111.Colspan = 6;
                            cell12111.BorderWidthLeft = 0f;
                            cell12111.BorderWidthRight = 0f;
                            cell12111.BorderWidthTop = 0f;
                            cell12111.BorderWidthBottom = 0f;

                            cell12111.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table1.AddCell(cell12111);

                            PdfPCell cell13699 = new PdfPCell(new Phrase("----------------------------------------------------------------------------------------------------------------------------", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell13699.Colspan = 7;

                            cell13699.BorderWidthLeft = 0f;
                            cell13699.BorderWidthRight = 0f;
                            cell13699.BorderWidthBottom = 0f;
                            cell13699.BorderColor = BaseColor.WHITE;
                            cell13699.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table2.AddCell(cell13699);

                            PdfPCell cell12112 = new PdfPCell(new Phrase("Assessee", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12112.Colspan = 1;
                            cell12112.BorderWidthLeft = 1f;
                            cell12112.BorderWidthRight = 1f;
                            cell12112.BorderWidthTop = 1f;
                            cell12112.BorderWidthBottom = 1f;

                            cell12112.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table2.AddCell(cell12112);
                            PdfPCell cell12113 = new PdfPCell(new Phrase("INVOICE", new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12113.Colspan = 6;
                            cell12113.BorderWidthLeft = 0f;
                            cell12113.BorderWidthRight = 0f;
                            cell12113.BorderWidthTop = 0f;
                            cell12113.BorderWidthBottom = 0f;

                            cell12113.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table2.AddCell(cell12113);


                            PdfPCell cell12 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12.Colspan = 5;
                            cell12.BorderWidthLeft = 1f;
                            cell12.BorderWidthRight = .8f;
                            cell12.BorderWidthTop = .8f;
                            cell12.BorderWidthBottom = 0f;

                            cell12.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12);

                            PdfPCell cell1207 = new PdfPCell(new Phrase("C. E. R/C", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1207.Colspan = 1;
                            cell1207.BorderWidthLeft = 0f;
                            cell1207.BorderWidthRight = 0f;
                            cell1207.BorderWidthTop = .8f;
                            cell1207.BorderWidthBottom = 0f;

                            cell1207.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1207);


                            PdfPCell cell1208 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["CERC"].ToString(), new iTextSharp.text.Font(bfTimes, 5f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1208.Colspan = 1;
                            cell1208.BorderWidthLeft = 0f;
                            cell1208.BorderWidthRight = 1f;
                            cell1208.BorderWidthTop = .8f;
                            cell1208.BorderWidthBottom = 0f;

                            cell1208.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1208);



                            PdfPCell cell1203 = new PdfPCell(new Phrase("" + GetAddress.Rows[0]["Address1"].ToString() + " , " + GetAddress.Rows[0]["city"].ToString() + Address.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1203.Colspan = 5;
                            cell1203.BorderWidthLeft = 1f;
                            cell1203.BorderWidthRight = .8f;
                            cell1203.BorderWidthTop = 0f;
                            cell1203.BorderWidthBottom = 0f;
                            cell1203.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1203);

                            PdfPCell cell2 = new PdfPCell(new Phrase("TIN NO.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell2.Colspan = 1;

                            cell2.BorderWidthLeft = 0f;
                            cell2.BorderWidthRight = 0f;
                            cell2.BorderWidthTop = .8f;
                            cell2.BorderWidthBottom = 0f;
                            cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell2);

                            // string getTinNo1 = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", CnnString);

                            PdfPCell cell21111 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["TinNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell21111.Colspan = 1;
                            cell21111.BorderWidthLeft = 0f;
                            cell21111.BorderWidthRight = 1f;
                            cell21111.BorderWidthTop = .8f;
                            cell21111.BorderWidthBottom = 0f;
                            cell21111.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell21111);


                            PdfPCell cell1204 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1204.Colspan = 5;
                            cell1204.BorderWidthLeft = 1f;
                            cell1204.BorderWidthRight = .8f;
                            cell1204.BorderWidthTop = 0f;
                            cell1204.BorderWidthBottom = 0f;
                            cell1204.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1204);


                            PdfPCell cell1209 = new PdfPCell(new Phrase("COMMODITY", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1209.Colspan = 1;
                            cell1209.BorderWidthLeft = 0f;
                            cell1209.BorderWidthRight = 0f;
                            cell1209.BorderWidthTop = .8f;
                            cell1209.BorderWidthBottom = 0f;

                            cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1209);


                            PdfPCell cell12115 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Commodity"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell12115.Colspan = 1;
                            cell12115.BorderWidthLeft = 0f;
                            cell12115.BorderWidthRight = 1f;
                            cell12115.BorderWidthTop = .8f;
                            cell12115.BorderWidthBottom = 0f;

                            cell12115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell12115);


                            PdfPCell cell1205 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1205.Colspan = 5;
                            cell1205.BorderWidthLeft = 1f;
                            cell1205.BorderWidthRight = .8f;
                            cell1205.BorderWidthTop = 0f;
                            cell1205.BorderWidthBottom = 0f;
                            cell1205.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1205);


                            PdfPCell cell1213 = new PdfPCell(new Phrase("C. H.", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1213.Colspan = 1;
                            cell1213.BorderWidthLeft = 0f;
                            cell1213.BorderWidthRight = 0f;
                            cell1213.BorderWidthTop = .8f;
                            cell1213.BorderWidthBottom = 0f;

                            cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1213);


                            PdfPCell cell1214 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["CH"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1214.Colspan = 1;
                            cell1214.BorderWidthLeft = 0f;
                            cell1214.BorderWidthRight = 1f;
                            cell1214.BorderWidthTop = .8f;
                            cell1214.BorderWidthBottom = 0f;

                            cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1214);

                            PdfPCell cell1206 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            cell1206.Colspan = 5;
                            cell1206.BorderWidthLeft = 1f;
                            cell1206.BorderWidthRight = .8f;
                            cell1206.BorderWidthTop = 0f;
                            cell1206.BorderWidthBottom = 0f;
                            cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1206);


                            PdfPCell cell1215 = new PdfPCell(new Phrase("RANGE", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1215.Colspan = 1;
                            cell1215.BorderWidthLeft = 0f;
                            cell1215.BorderWidthRight = 0f;
                            cell1215.BorderWidthTop = .8f;
                            cell1215.BorderWidthBottom = 0f;

                            cell1215.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1215);


                            PdfPCell cell1216 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Range"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1216.Colspan = 1;
                            cell1216.BorderWidthLeft = 0f;
                            cell1216.BorderWidthRight = 1f;
                            cell1216.BorderWidthTop = .8f;
                            cell1216.BorderWidthBottom = 0f;

                            cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1216);



                            PdfPCell cell1217 = new PdfPCell(new Phrase("Book No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1217.Colspan = 1;
                            cell1217.BorderWidthLeft = 1f;
                            cell1217.BorderWidthRight = .8f;
                            cell1217.BorderWidthTop = .8f;
                            cell1217.BorderWidthBottom = 0f;

                            cell1217.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1217);


                            PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1218.Colspan = 2;
                            cell1218.BorderWidthLeft = 0f;
                            cell1218.BorderWidthRight = .8f;
                            cell1218.BorderWidthTop = .8f;
                            cell1218.BorderWidthBottom = 0f;

                            cell1218.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1218);

                            PdfPCell cell1219 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1219.Colspan = 1;
                            cell1219.BorderWidthLeft = 0f;
                            cell1219.BorderWidthRight = .8f;
                            cell1219.BorderWidthTop = .8f;
                            cell1219.BorderWidthBottom = 0f;

                            cell1219.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1219);

                            PdfPCell cell1220 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1220.Colspan = 1;
                            cell1220.BorderWidthLeft = 0f;
                            cell1220.BorderWidthRight = .8f;
                            cell1220.BorderWidthTop = .8f;
                            cell1220.BorderWidthBottom = 0f;

                            cell1220.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1220);

                            PdfPCell cell1221 = new PdfPCell(new Phrase("DIVISION", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1221.Colspan = 1;
                            cell1221.BorderWidthLeft = 0f;
                            cell1221.BorderWidthRight = 0f;
                            cell1221.BorderWidthTop = .8f;
                            cell1221.BorderWidthBottom = 0f;

                            cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1221);

                            PdfPCell cell1222 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Division"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1222.Colspan = 1;
                            cell1222.BorderWidthLeft = 0f;
                            cell1222.BorderWidthRight = 1f;
                            cell1222.BorderWidthTop = .8f;
                            cell1222.BorderWidthBottom = 0f;

                            cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1222);







                            PdfPCell cell1223 = new PdfPCell(new Phrase("Invoice No", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1223.Colspan = 1;
                            cell1223.BorderWidthLeft = 1f;
                            cell1223.BorderWidthRight = .8f;
                            cell1223.BorderWidthTop = 0f;
                            cell1223.BorderWidthBottom = 0f;

                            cell1223.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1223);


                            PdfPCell cell1224 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["ChallanNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1224.Colspan = 2;
                            cell1224.BorderWidthLeft = 0f;
                            cell1224.BorderWidthRight = .8f;
                            cell1224.BorderWidthTop = 0f;
                            cell1224.BorderWidthBottom = 0f;

                            cell1224.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1224);

                            PdfPCell cell1225 = new PdfPCell(new Phrase("Date", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1225.Colspan = 1;
                            cell1225.BorderWidthLeft = 0f;
                            cell1225.BorderWidthRight = .8f;
                            cell1225.BorderWidthTop = 0f;
                            cell1225.BorderWidthBottom = 0f;

                            cell1225.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1225);

                            string OrderClosedDate;
                            if (dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == "1/1/1900 12:00:00 AM" || dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == null || dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"].ToString() == "")
                            {
                                OrderClosedDate = DateTime.Now.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                OrderClosedDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderClosedDate"]).ToString("dd/MM/yyyy");
                            }

                            PdfPCell cell1226 = new PdfPCell(new Phrase("" + OrderClosedDate.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1226.Colspan = 1;
                            cell1226.BorderWidthLeft = 0f;
                            cell1226.BorderWidthRight = .8f;
                            cell1226.BorderWidthTop = 0f;
                            cell1226.BorderWidthBottom = 0f;

                            cell1226.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1226);

                            PdfPCell cell1227 = new PdfPCell(new Phrase("Commissionerate", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1227.Colspan = 1;
                            cell1227.BorderWidthLeft = 0f;
                            cell1227.BorderWidthRight = 0f;
                            cell1227.BorderWidthTop = .8f;
                            cell1227.BorderWidthBottom = 0f;

                            cell1227.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1227);

                            PdfPCell cell1228 = new PdfPCell(new Phrase(": " + GetAddress.Rows[0]["Commissionerate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1228.Colspan = 1;
                            cell1228.BorderWidthLeft = 0f;
                            cell1228.BorderWidthRight = 1f;
                            cell1228.BorderWidthTop = .8f;
                            cell1228.BorderWidthBottom = 0f;

                            cell1228.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1228);




                            PdfPCell cell1229 = new PdfPCell(new Phrase("P.O. No.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1229.Colspan = 1;
                            cell1229.BorderWidthLeft = 1f;
                            cell1229.BorderWidthRight = .8f;
                            cell1229.BorderWidthTop = .8f;
                            cell1229.BorderWidthBottom = 0f;

                            cell1229.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1229);


                            PdfPCell cell1230 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1230.Colspan = 2;
                            cell1230.BorderWidthLeft = 0f;
                            cell1230.BorderWidthRight = .8f;
                            cell1230.BorderWidthTop = .8f;
                            cell1230.BorderWidthBottom = 0f;

                            cell1230.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1230);

                            PdfPCell cell1231 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1231.Colspan = 1;
                            cell1231.BorderWidthLeft = 0f;
                            cell1231.BorderWidthRight = .8f;
                            cell1231.BorderWidthTop = .8f;
                            cell1231.BorderWidthBottom = 0f;

                            cell1231.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1231);

                            PdfPCell cell1232 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1232.Colspan = 1;
                            cell1232.BorderWidthLeft = 0f;
                            cell1232.BorderWidthRight = .8f;
                            cell1232.BorderWidthTop = .8f;
                            cell1232.BorderWidthBottom = 0f;

                            cell1232.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1232);

                            PdfPCell cell1233 = new PdfPCell(new Phrase("Party C. E. R/C NO", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1233.Colspan = 1;
                            cell1233.BorderWidthLeft = 0f;
                            cell1233.BorderWidthRight = 0f;
                            cell1233.BorderWidthTop = .8f;
                            cell1233.BorderWidthBottom = 0f;

                            cell1233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1233);

                            PdfPCell cell1234 = new PdfPCell(new Phrase(": NA", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1234.Colspan = 1;
                            cell1234.BorderWidthLeft = 0f;
                            cell1234.BorderWidthRight = 1f;
                            cell1234.BorderWidthTop = .8f;
                            cell1234.BorderWidthBottom = 0f;

                            cell1234.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1234);





                            PdfPCell cell1235 = new PdfPCell(new Phrase("RR/GR NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1235.Colspan = 1;
                            cell1235.BorderWidthLeft = 1f;
                            cell1235.BorderWidthRight = .8f;
                            cell1235.BorderWidthTop = .8f;
                            cell1235.BorderWidthBottom = 0f;

                            cell1235.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1235);


                            PdfPCell cell1236 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1236.Colspan = 2;
                            cell1236.BorderWidthLeft = 0f;
                            cell1236.BorderWidthRight = .8f;
                            cell1236.BorderWidthTop = .8f;
                            cell1236.BorderWidthBottom = 0f;

                            cell1236.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1236);

                            PdfPCell cell1237 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1237.Colspan = 1;
                            cell1237.BorderWidthLeft = 0f;
                            cell1237.BorderWidthRight = .8f;
                            cell1237.BorderWidthTop = .8f;
                            cell1237.BorderWidthBottom = 0f;

                            cell1237.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1237);

                            PdfPCell cell1238 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1238.Colspan = 1;
                            cell1238.BorderWidthLeft = 0f;
                            cell1238.BorderWidthRight = .8f;
                            cell1238.BorderWidthTop = .8f;
                            cell1238.BorderWidthBottom = 0f;

                            cell1238.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1238);

                            PdfPCell cell1239 = new PdfPCell(new Phrase("Party CST/TIN No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1239.Colspan = 1;
                            cell1239.BorderWidthLeft = 0f;
                            cell1239.BorderWidthRight = 0f;
                            cell1239.BorderWidthTop = .8f;
                            cell1239.BorderWidthBottom = 0f;

                            cell1239.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1239);

                            PdfPCell cell1240 = new PdfPCell(new Phrase(": NA", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1240.Colspan = 1;
                            cell1240.BorderWidthLeft = 0f;
                            cell1240.BorderWidthRight = 1f;
                            cell1240.BorderWidthTop = .8f;
                            cell1240.BorderWidthBottom = 0f;

                            cell1240.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1240);




                            PdfPCell cell1241 = new PdfPCell(new Phrase("Consignee", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1241.Colspan = 1;
                            cell1241.BorderWidthLeft = 1f;
                            cell1241.BorderWidthRight = .8f;
                            cell1241.BorderWidthTop = .8f;
                            cell1241.BorderWidthBottom = 0f;

                            cell1241.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1241);

                            PdfPCell cell1242 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["Address1"].ToString().ToUpper(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1242.Colspan = 5;
                            cell1242.BorderWidthLeft = 0f;
                            cell1242.BorderWidthRight = .8f;
                            cell1242.BorderWidthTop = .8f;
                            cell1242.BorderWidthBottom = 0f;

                            cell1242.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1242);

                            PdfPCell cell1243 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1243.Colspan = 1;
                            cell1243.BorderWidthLeft = 0f;
                            cell1243.BorderWidthRight = 1f;
                            cell1243.BorderWidthTop = .8f;
                            cell1243.BorderWidthBottom = 0f;

                            cell1243.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1243);




                            PdfPCell cell1248 = new PdfPCell(new Phrase("S.N0.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1248.Colspan = 1;
                            cell1248.BorderWidthLeft = 1f;
                            cell1248.BorderWidthRight = .8f;
                            cell1248.BorderWidthTop = .8f;
                            cell1248.BorderWidthBottom = 0f;

                            cell1248.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1248);


                            PdfPCell cell1249 = new PdfPCell(new Phrase("Description & Specifications of Goods", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1249.Colspan = 2;
                            cell1249.BorderWidthLeft = 0f;
                            cell1249.BorderWidthRight = .8f;
                            cell1249.BorderWidthTop = .8f;
                            cell1249.BorderWidthBottom = 0f;

                            cell1249.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1249);

                            PdfPCell cell1250 = new PdfPCell(new Phrase("No. Of Pkg", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1250.Colspan = 1;
                            cell1250.BorderWidthLeft = 0f;
                            cell1250.BorderWidthRight = .8f;
                            cell1250.BorderWidthTop = .8f;
                            cell1250.BorderWidthBottom = 0f;

                            cell1250.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1250);

                            PdfPCell cell1251 = new PdfPCell(new Phrase("Quantity", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1251.Colspan = 1;
                            cell1251.BorderWidthLeft = 0f;
                            cell1251.BorderWidthRight = .8f;
                            cell1251.BorderWidthTop = .8f;
                            cell1251.BorderWidthBottom = 0f;

                            cell1251.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1251);

                            PdfPCell cell1252 = new PdfPCell(new Phrase("Price Per Unit", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1252.Colspan = 1;
                            cell1252.BorderWidthLeft = 0f;
                            cell1252.BorderWidthRight = .8f;
                            cell1252.BorderWidthTop = .8f;
                            cell1252.BorderWidthBottom = 0f;

                            cell1252.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1252);

                            PdfPCell cell1253 = new PdfPCell(new Phrase("Amount Rs.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1253.Colspan = 1;
                            cell1253.BorderWidthLeft = 0f;
                            cell1253.BorderWidthRight = 1f;
                            cell1253.BorderWidthTop = .8f;
                            cell1253.BorderWidthBottom = 0f;

                            cell1253.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1253);





                            PdfPCell cell1255 = new PdfPCell(new Phrase("HSRP NUMBER PLATE SET", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1255.Colspan = 3;
                            cell1255.BorderWidthLeft = 1f;
                            cell1255.BorderWidthRight = .8f;
                            cell1255.BorderWidthTop = .8f;
                            cell1255.BorderWidthBottom = 0f;

                            cell1255.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1255);

                            PdfPCell cell1256 = new PdfPCell(new Phrase("1", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1256.Colspan = 1;
                            cell1256.BorderWidthLeft = 0f;
                            cell1256.BorderWidthRight = .8f;
                            cell1256.BorderWidthTop = .8f;
                            cell1256.BorderWidthBottom = 0f;

                            cell1256.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1256);

                            int FQt = 0;
                            if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != "NULL" || dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() != " ")
                            {

                                if (dataSetFillHSRPDeliveryChallan.Rows[0]["ISFrontPlateSize"].ToString() == "Y")
                                {
                                    FQt = 1;
                                }
                                else
                                {
                                    FQt = 0;
                                }
                            }

                            PdfPCell cell1257 = new PdfPCell(new Phrase(FQt.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1257.Colspan = 1;
                            cell1257.BorderWidthLeft = 0f;
                            cell1257.BorderWidthRight = .8f;
                            cell1257.BorderWidthTop = .8f;
                            cell1257.BorderWidthBottom = 0f;

                            cell1257.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1257);

                            decimal amount4 = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["TotalAmount"]);
                            decimal amount5 = Math.Round(amount4, 2);
                            PdfPCell cell1258 = new PdfPCell(new Phrase(amount5.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1258.Colspan = 1;
                            cell1258.BorderWidthLeft = 0f;
                            cell1258.BorderWidthRight = .8f;
                            cell1258.BorderWidthTop = .8f;
                            cell1258.BorderWidthBottom = 0f;

                            cell1258.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1258);



                            PdfPCell cell1259 = new PdfPCell(new Phrase(amount5.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1259.Colspan = 1;
                            cell1259.BorderWidthLeft = 0f;
                            cell1259.BorderWidthRight = 1f;
                            cell1259.BorderWidthTop = .8f;
                            cell1259.BorderWidthBottom = 0f;

                            cell1259.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1259);



                            PdfPCell cell1308 = new PdfPCell(new Phrase("Vehicle Reg No./Auth No :" + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString() + "/" + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1308.Colspan = 4;
                            cell1308.BorderWidthLeft = 1f;
                            cell1308.BorderWidthRight = .8f;
                            cell1308.BorderWidthTop = .8f;
                            cell1308.BorderWidthBottom = 0f;

                            cell1308.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1308);





                            PdfPCell cell1310 = new PdfPCell(new Phrase("Sub Total", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1310.Colspan = 1;
                            cell1310.BorderWidthLeft = 0f;
                            cell1310.BorderWidthRight = .8f;
                            cell1310.BorderWidthTop = .8f;
                            cell1310.BorderWidthBottom = 0f;

                            cell1310.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1310);

                            PdfPCell cell1311 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1311.Colspan = 1;
                            cell1311.BorderWidthLeft = 0f;
                            cell1311.BorderWidthRight = .8f;
                            cell1311.BorderWidthTop = .8f;
                            cell1311.BorderWidthBottom = 0f;

                            cell1311.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1311);

                            decimal amount = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["TotalAmount"]);
                            decimal amount1 = Math.Round(amount, 2);


                            PdfPCell cell1312 = new PdfPCell(new Phrase(amount1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1312.Colspan = 1;
                            cell1312.BorderWidthLeft = 0f;
                            cell1312.BorderWidthRight = 1f;
                            cell1312.BorderWidthTop = .8f;
                            cell1312.BorderWidthBottom = 0f;

                            cell1312.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1312);





                            PdfPCell cell1313 = new PdfPCell(new Phrase("Vehicle Type : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleType"].ToString() + " Vehicle Class : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 7f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1313.Colspan = 4;
                            cell1313.BorderWidthLeft = 1f;
                            cell1313.BorderWidthRight = .8f;
                            cell1313.BorderWidthTop = .8f;
                            cell1313.BorderWidthBottom = 0f;

                            cell1313.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1313);




                            PdfPCell cell1315 = new PdfPCell(new Phrase("TAX ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1315.Colspan = 1;
                            cell1315.BorderWidthLeft = 0f;
                            cell1315.BorderWidthRight = .8f;
                            cell1315.BorderWidthTop = .8f;
                            cell1315.BorderWidthBottom = 0f;

                            cell1315.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1315);

                            PdfPCell cell1316 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1316.Colspan = 1;
                            cell1316.BorderWidthLeft = 0f;
                            cell1316.BorderWidthRight = .8f;
                            cell1316.BorderWidthTop = .8f;
                            cell1316.BorderWidthBottom = 0f;

                            cell1316.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1316);

                            PdfPCell cell1317 = new PdfPCell(new Phrase(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Amount"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1317.Colspan = 1;
                            cell1317.BorderWidthLeft = 0f;
                            cell1317.BorderWidthRight = 1f;
                            cell1317.BorderWidthTop = .8f;
                            cell1317.BorderWidthBottom = 0f;

                            cell1317.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1317);

                            PdfPCell cell1318 = new PdfPCell(new Phrase("Front Laser Code : " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRP_Front_LaserCode"].ToString() + "  Rear Laser Code : " + dataSetFillHSRPDeliveryChallan.Rows[0]["HSRP_Rear_LaserCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1318.Colspan = 4;
                            cell1318.BorderWidthLeft = 1f;
                            cell1318.BorderWidthRight = .8f;
                            cell1318.BorderWidthTop = .8f;
                            cell1318.BorderWidthBottom = 0f;

                            cell1318.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1318);





                            PdfPCell cell1320 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1320.Colspan = 1;
                            cell1320.BorderWidthLeft = 0f;
                            cell1320.BorderWidthRight = .8f;
                            cell1320.BorderWidthTop = .8f;
                            cell1320.BorderWidthBottom = 0f;

                            cell1320.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1320);

                            PdfPCell cell1321 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1321.Colspan = 1;
                            cell1321.BorderWidthLeft = 0f;
                            cell1321.BorderWidthRight = .8f;
                            cell1321.BorderWidthTop = .8f;
                            cell1321.BorderWidthBottom = 0f;

                            cell1321.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1321);

                            PdfPCell cell1322 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1322.Colspan = 1;
                            cell1322.BorderWidthLeft = 0f;
                            cell1322.BorderWidthRight = 1f;
                            cell1322.BorderWidthTop = .8f;
                            cell1322.BorderWidthBottom = 0f;

                            cell1322.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1322);




                            PdfPCell cell1328 = new PdfPCell(new Phrase("Chassis No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["ChassisNo"].ToString() + "  Engine No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1328.Colspan = 4;
                            cell1328.BorderWidthLeft = 1f;
                            cell1328.BorderWidthRight = .8f;
                            cell1328.BorderWidthTop = .8f;
                            cell1328.BorderWidthBottom = 0f;

                            cell1328.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1328);





                            PdfPCell cell1330 = new PdfPCell(new Phrase("Sub total", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1330.Colspan = 1;
                            cell1330.BorderWidthLeft = 0f;
                            cell1330.BorderWidthRight = .8f;
                            cell1330.BorderWidthTop = .8f;
                            cell1330.BorderWidthBottom = 0f;

                            cell1330.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1330);



                            PdfPCell cell1331 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1331.Colspan = 1;
                            cell1331.BorderWidthLeft = 0f;
                            cell1331.BorderWidthRight = .8f;
                            cell1331.BorderWidthTop = .8f;
                            cell1331.BorderWidthBottom = 0f;

                            cell1331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1331);

                            decimal vatPer = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Percentage"]);
                            decimal vat = Convert.ToDecimal(dataSetFillHSRPDeliveryChallan.Rows[0]["VAT_Amount"]);
                            decimal TotAmt = amount + vat;
                            decimal totalAmount = System.Decimal.Round(TotAmt, 2);
                            PdfPCell cell1332 = new PdfPCell(new Phrase(totalAmount.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1332.Colspan = 1;
                            cell1332.BorderWidthLeft = 0f;
                            cell1332.BorderWidthRight = 1f;
                            cell1332.BorderWidthTop = .8f;
                            cell1332.BorderWidthBottom = 0f;

                            cell1332.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1332);





                            PdfPCell cell1338 = new PdfPCell(new Phrase("Terms", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1338.Colspan = 1;
                            cell1338.BorderWidthLeft = 1f;
                            cell1338.BorderWidthRight = .8f;
                            cell1338.BorderWidthTop = .8f;
                            cell1338.BorderWidthBottom = 0f;

                            cell1338.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1338);

                            PdfPCell cell1339 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1339.Colspan = 1;
                            cell1339.BorderWidthLeft = 0f;
                            cell1339.BorderWidthRight = .8f;
                            cell1339.BorderWidthTop = .8f;
                            cell1339.BorderWidthBottom = 0f;

                            cell1339.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1339);

                            PdfPCell cell1340 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1340.Colspan = 1;
                            cell1340.BorderWidthLeft = 0f;
                            cell1340.BorderWidthRight = .8f;
                            cell1340.BorderWidthTop = .8f;
                            cell1340.BorderWidthBottom = 0f;

                            cell1340.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1340);

                            PdfPCell cell1341 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1341.Colspan = 1;
                            cell1341.BorderWidthLeft = 0f;
                            cell1341.BorderWidthRight = .8f;
                            cell1341.BorderWidthTop = .8f;
                            cell1341.BorderWidthBottom = 0f;

                            cell1341.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1341);

                            PdfPCell cell1342 = new PdfPCell(new Phrase("Invoice Value Round of ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1342.Colspan = 2;
                            cell1342.BorderWidthLeft = 0f;
                            cell1342.BorderWidthRight = .8f;
                            cell1342.BorderWidthTop = .8f;
                            cell1342.BorderWidthBottom = 0f;

                            cell1342.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1342);


                            decimal totalAmount1 = System.Decimal.Round(TotAmt, 0);
                            PdfPCell cell1344 = new PdfPCell(new Phrase(totalAmount1.ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1344.Colspan = 1;
                            cell1344.BorderWidthLeft = 0f;
                            cell1344.BorderWidthRight = 1f;
                            cell1344.BorderWidthTop = .8f;
                            cell1344.BorderWidthBottom = 0f;

                            cell1344.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1344);




                            PdfPCell cell1345 = new PdfPCell(new Phrase("1:It is certified that the particulars given above are true and correct and amount indicated represents that prices actually charged and that there is no aditional inflow of any consideration directly or indirectly from the buyer", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1345.Colspan = 7;
                            cell1345.BorderWidthLeft = 1f;
                            cell1345.BorderWidthRight = .8f;
                            cell1345.BorderWidthTop = .8f;
                            cell1345.BorderWidthBottom = 0f;

                            cell1345.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1345);





                            PdfPCell cell1348 = new PdfPCell(new Phrase("2: All disputes arising out of it will be subject to Delhi jurisdiction only", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1348.Colspan = 7;
                            cell1348.BorderWidthLeft = 1f;
                            cell1348.BorderWidthRight = .8f;
                            cell1348.BorderWidthTop = .8f;
                            cell1348.BorderWidthBottom = 0f;

                            cell1348.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1348);




                            PdfPCell cell1353 = new PdfPCell(new Phrase("3: Interest @24% will be charged on all account remaining unpaid after 30 days/due as agreed", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1353.Colspan = 4;
                            cell1353.BorderWidthLeft = 1f;
                            cell1353.BorderWidthRight = .8f;
                            cell1353.BorderWidthTop = .8f;
                            cell1353.BorderWidthBottom = 0f;

                            cell1353.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1353);



                            PdfPCell cell1358 = new PdfPCell(new Phrase(GetAddress.Rows[0]["CompanyName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1358.Colspan = 3;
                            cell1358.BorderWidthLeft = 0f;
                            cell1358.BorderWidthRight = 1f;
                            cell1358.BorderWidthTop = .8f;
                            cell1358.BorderWidthBottom = 0f;

                            cell1358.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1358);





                            PdfPCell cell1356 = new PdfPCell(new Phrase("4:Goods once sold & accepted will not be taken back", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1356.Colspan = 4;
                            cell1356.BorderWidthLeft = 1f;
                            cell1356.BorderWidthRight = .8f;
                            cell1356.BorderWidthTop = .8f;
                            cell1356.BorderWidthBottom = 0f;

                            cell1356.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1356);


                            PdfPCell cell1361 = new PdfPCell(new Phrase("(AUTH. SIGN.)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            //cell1361.PaddingRight = 100f;
                            cell1361.Colspan = 3;
                            cell1361.PaddingRight = 0f;
                            cell1361.BorderWidthLeft = 0f;
                            cell1361.BorderWidthRight = 1f;
                            cell1361.BorderWidthTop = 0f;
                            cell1361.BorderWidthBottom = 0f;

                            cell1361.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1361);



                            PdfPCell cell1359 = new PdfPCell(new Phrase("5:Any discrepancy in quality & quantity should be reported within 24 hrs , of receipt of goods", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1359.Colspan = 7;
                            cell1359.BorderWidthLeft = 1f;
                            cell1359.BorderWidthRight = .8f;
                            cell1359.BorderWidthTop = .8f;
                            cell1359.BorderWidthBottom = 0f;

                            cell1359.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1359);





                            PdfPCell cell1362 = new PdfPCell(new Phrase("We hereby certify that goods recived are as per order & requirement and we abide to above said terms", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1362.Colspan = 7;
                            cell1362.BorderWidthLeft = 1f;
                            cell1362.BorderWidthRight = .8f;
                            cell1362.BorderWidthTop = .8f;
                            cell1362.BorderWidthBottom = 1f;

                            cell1362.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1362);





                            PdfPCell cell1366 = new PdfPCell(new Phrase("CUSTOMER'S NAME : " + dataSetFillHSRPDeliveryChallan.Rows[0]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1366.Colspan = 7;
                            cell1366.PaddingRight = 60f;
                            cell1366.BorderWidthLeft = 0f;
                            cell1366.BorderWidthRight = .8f;
                            cell1366.BorderWidthTop = 0f;
                            cell1366.BorderWidthBottom = 1f;
                            cell1366.BorderColor = BaseColor.WHITE;
                            cell1366.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1366);

                            PdfPCell cell1367 = new PdfPCell(new Phrase("(SIGN)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1367.Colspan = 7;
                            cell1367.PaddingRight = 100f;
                            cell1367.PaddingTop = 5f;
                            cell1367.BorderWidthLeft = 0f;
                            cell1367.BorderWidthRight = .8f;
                            cell1367.BorderWidthTop = .8f;
                            cell1367.BorderWidthBottom = 1f;
                            cell1367.BorderColor = BaseColor.WHITE;
                            cell1367.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1367);




                            PdfPCell cell1369 = new PdfPCell(new Phrase("VEHICLE No : " + dataSetFillHSRPDeliveryChallan.Rows[0]["VehicleRegNo"].ToString() + " Date :", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell1369.Colspan = 7;
                            cell1369.PaddingRight = 70f;
                            cell1369.BorderWidthLeft = 0f;
                            cell1369.BorderWidthRight = 1f;
                            cell1369.BorderWidthBottom = 1f;
                            cell1369.BorderColor = BaseColor.WHITE;
                            cell1369.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell1369);



                            document.Add(table1);
                            document.Add(table);

                            document.Add(table2);
                            document.Add(table);

                            //document.Add(table);

                            document.NewPage();

                            //ReadFile(PdfFolder);

                        }
                   
                
            }

            try
            {

                document.Close();


                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
            catch (Exception)
            {

            }
        }
        protected void txtVehicleNo_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInvoiceNo.Text) && (dropdownDuplicateFIle.SelectedIndex <= 0))
            {
                Response.Write("<script> alert('Please Enter Invoice No.')</script>");
                return;
            }
            GetRecords();
            string chkb = string.Empty;

                       float Amount = 0;
            string filename1 = "Order_Open_Records" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
            String StringField1 = String.Empty;
            String StringAlert1 = String.Empty;
            StringBuilder bb1 = new StringBuilder();
            // Document document = new Document();
            //  iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
            Document document1 = new Document(new iTextSharp.text.Rectangle(188f, 124f), -30, -30, 8, 0);
            document1.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bfTimes1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
            //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
            string PdfFolder1 = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename1;
            PdfWriter.GetInstance(document1, new FileStream(PdfFolder1, FileMode.Create));
            //Opens the document:
            document1.Open();

            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 6900f;

            GenerateCell(table, 6, 0, 0, 0, 0, 1, 0, "HSRP Order Booking Report", 0, 0);

            GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, "SN", 0, 0);
            GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, "Vehicle No", 0, 0);

            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Vehicle Type", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Collection Date", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Embossing Date", 0, 0);
            GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, "Invoice No", 0, 0);

            foreach (DataRow di in dtInvoiceData.Rows)
            {
                List<string> lblvehicleRegNo = new List<string>();
                ArrayList VehicleRegNo = new ArrayList();
                ArrayList LaserFront = new ArrayList();
                ArrayList LaserRear = new ArrayList();

                string strSN = di["SN"].ToString();
                string lblVehicleRegNo = di["VehicleRegNo"].ToString();
                string strVehType = di["VehicleType"].ToString();
                string strCollectionDate = di["HSRPRecord_CreationDate"].ToString();
                string strEmbDate = di["OrderEmbossingDate"].ToString();
                string strInvoiceNo = di["ChallanNo"].ToString();

                string OrderStatus = di["OrderStatus"].ToString();
                string strRecordId = di["hsrprecordid"].ToString();
                string flaser = di["HSRP_Front_LaserCode"].ToString();
                string rlaser = di["hsrp_rear_lasercode"].ToString();

                if (di["VehicleRegNo"].ToString().Equals(dtInvoiceData.Rows[dtInvoiceData.Rows.Count - 1]["VehicleRegNo"].ToString()))
                {
                    GenerateCell(table, 1, 1, 1, 1, 1, 0, 1, strSN, 0, 0);
                GenerateCell(table, 1, 1, 1, 1, 1, 0, 1, lblVehicleRegNo, 0, 0);

                GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strVehType, 0, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strCollectionDate, 0, 0);
                GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strEmbDate, 0, 0);
                
                    GenerateCell(table, 1, 0, 1, 1, 1, 0, 1, strInvoiceNo, 0, 0);
                }
                else
                {
                    GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, strSN, 0, 0);
                    GenerateCell(table, 1, 1, 1, 1, 0, 0, 1, lblVehicleRegNo, 0, 0);

                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, strVehType, 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, strCollectionDate, 0, 0);
                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, strEmbDate, 0, 0);

                    GenerateCell(table, 1, 0, 1, 1, 0, 0, 1, strInvoiceNo, 0, 0);
                }
               
            }
            document1.Add(table);
            document1.Close();
            HttpContext context1 = HttpContext.Current;
            context1.Response.ContentType = "Application/pdf";
            context1.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename1);
            context1.Response.WriteFile(PdfFolder1);
            context1.Response.End();
        }

        protected void gvRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRecords.EditIndex = e.NewPageIndex;
            BindGrid();
        }
        }
    }
                #endregion