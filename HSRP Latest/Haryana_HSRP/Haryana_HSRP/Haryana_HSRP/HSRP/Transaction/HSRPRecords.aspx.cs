using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HSRP;
using System.Data.SqlClient;
using System.Data;
using DataProvider;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using System.IO;
using System.Text;
using System.Net;
using HSRP.org.mptransport.oltp;

using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using System.Printing.Activation;
using System.Globalization;

namespace HSRP.Transaction
{
    public partial class HSRPRecords : System.Web.UI.Page
    {
        int temp = 0;

        //Authorization Info
        string AuthorizationNo = String.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        DateTime CashReceiptDate;
        string StringOrderNo = String.Empty;
        string StringDeliverChallanNo = String.Empty;
        string StringCustomerName = String.Empty;
        string StringOrderStatus = string.Empty;
        string StringAddress1 = String.Empty;
        string StringAddress2 = string.Empty;
        string StringMobileNo = String.Empty;
        string StringLandline = string.Empty;
        string StringEmailId = string.Empty;

        // Vehicle Info

        string StringVehicleClass = string.Empty;
        string StringManufacturerName = String.Empty;
        string StringManufacturerModel = String.Empty;
        string StringVehicleModel = string.Empty;
        string StringVehicleRegNo = String.Empty;
        string StringEngineNo = string.Empty;
        string StringChassisNo = string.Empty;

        // NUMBER PLATE INFO

        string StringOrderType = string.Empty;
        string StringFrontPlateSize = String.Empty;
        string StringRearPlateSize = String.Empty;
        string ISThirdStickerExists = String.Empty;
        string IScheckBoxFrontPlate = string.Empty;
        string IScheckBoxRearPlate = string.Empty;

        // FINANCIAL INFO

        string StringInvoiceNo = string.Empty;
        string StringCashReceiptNo_HHT = String.Empty;
        string StringCashReceiptNo = String.Empty;
        string StringFixingCharge = string.Empty;
        string StringTotalAmount = String.Empty;
        string StringVATPercentage = string.Empty;
        string StringVATAmount = string.Empty;
        string StringServiceTaxPercentage = string.Empty;
        string StringServiceTaxAmount = string.Empty;
        string StringNetTotal = string.Empty;
        string StringFrontPlatePrize = string.Empty;
        string StringRearPlatePrize = string.Empty;
        string StringStickerPrize = string.Empty;
        string StringScrewPrize = string.Empty;

        DataSet dataSetFillHSRPRecordDetail = new DataSet();
        DataSet dataSetFillHSRPRecord_HHT = new DataSet();

        int DID;
        string HSRPStateID;
        string RTOLocationID;
        int HSRPRecordID;
        int UID;
        int UserType;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string StringMode = String.Empty;
        String RTOs;
        String HSRPRecordIDXX;
        string MacAddress = string.Empty;

      //  msgBox objMsgBox = new msgBox();

        protected void Page_Load(object sender, EventArgs e)
        {

           
                ScriptManager.GetCurrent(this).RegisterPostBackControl(this.buttonGo);
                Page.Form.Attributes.Add("enctype", "multipart/form-data");

                ScriptManager.GetCurrent(this).RegisterPostBackControl(this.buttonSave);
                Page.Form.Attributes.Add("enctype", "multipart/form-data");

                TextBoxCashReceiptNo.Enabled = true;
                //LinkButtonCashReceipts.Visible = false;
                //form1.Disabled = true;
                Utils.GZipEncodePage();
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    int.TryParse(Session["UserType"].ToString(), out UserType);
                    HSRPStateID = Session["UserHSRPStateID"].ToString();
                    RTOLocationID = Session["UserRTOLocationID"].ToString();
                    int.TryParse(Session["UID"].ToString(), out UID);
                    MacAddress = Session["MacAddress"].ToString();
                    LabelUSER.Text = Session["UserName"].ToString();
                    //LabelCreatedID.Text = "Allowed RTO's :- ";
                    lblErrMess.Text = string.Empty;
                    lblSucMess.Text = string.Empty;
                }

                if (string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()))
                {
                    Response.Write("<script language='javascript'> {window.close();} </script>");
                }
                else
                {
                    StringMode = Request.QueryString["Mode"].ToString();
                }

                if (StringMode.Equals("Edit"))
                {
                    int.TryParse(Request.QueryString["HSRPRecordID"].ToString(), out HSRPRecordID);
                    buttonUpdate.Visible = true;
                    buttonSave.Visible = false;
                }
                else
                {
                    buttonSave.Visible = true;
                    // buttonUpdate.Visible = false;
                }

                if (!Page.IsPostBack)
                {
                    //InitialSetting();

                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    SQLString = "Select RTOLocationName from RTOLocation where RTOLocationID =" + RTOLocationID;
                    DataTable dtt = Utils.GetDataTable(SQLString, CnnString);
                    if (dtt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtt.Rows)
                        {
                            LabelUSer1.Text = "Dear User : " + Session["UserName"].ToString() + " you are assigned to Location : " + dr["RTOLocationName"].ToString();
                        }
                    }

                    SQLString = "Select R.RTOLocationCode,R.RTOLocationID From RTOLocation R inner join UserRTOLocationMapping U on " +
                                "R.RTOLocationID=U.RTOLocationID Where U.UserID=" + UID + " order by R.RTOLocationCode";
                    DataSet dds = Utils.getDataSet(SQLString, CnnString);
                    if (dds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dds.Tables[0].Rows)
                        {
                            LabelCreatedID.Text += dr["RTOLocationCode"].ToString() + " / ";
                            RTOs += dr["RTOLocationID"].ToString() + " / ";
                        }

                    }
                    LabelCreatedID.Text = LabelCreatedID.Text.Substring(0, LabelCreatedID.Text.Length - 2);
                    RTOs = RTOs.Substring(0, RTOs.Length - 2);

                    LabelCreatedDateTime.Text = DateTime.Now.ToString() + "ajay1";

                    AutoCompleteExtender1.ContextKey = RTOs + "^" + HSRPStateID;
                    AutoCompleteExtender2.ContextKey = RTOs + "^" + HSRPStateID;

                    //String[] kk = DateTime.Now.ToString().Split('/');
                    //if (kk[0].Length == 1)
                    //{
                    //    kk[0] = "0" + kk[0];
                    //}
                    //LabelCreatedDateTime.Text = kk[1] + "/" + kk[0] + "/" + kk[2];
                    LabelCreatedDateTime.Text = DateTime.Now.ToShortDateString();

                    FillVehicleMaker();
                    FillVehicleModel1();

                    textBoxMobileNo.Attributes.Add("onkeypress", "return isNumberKey(event);");
                    textBoxLandline.Attributes.Add("onkeypress", "return isNumberKey(event);");
                    //  DropDownListVehicleMaker1.Attributes.Add("onChange", "return OnSelectedIndexChangeVehicleMaker();");
                    //  DropDownListVehicleModel.Attributes.Add("onChange", "return OnSelectedIndexChangeVehicleModel();");
                    //DropDownListOrderType.Attributes.Add("onChange", "return OnSelectedIndexChangeVehicleOrder();");
                    DropDownListModel.Attributes.Add("onChange", "return OnSelectedIndexChangeModel();");
                    //BindFrontPlateSizeDropDownList();
                    //BindRearPlateSizeDropDownList();
                    if (StringMode.Equals("Edit"))
                    {
                        //UpdateHSRPRecordDetail(HSRPRecordID);
                    }
                    FilldropDownListRTOLocation();
                
            }
        }

        private void FilldropDownListRTOLocation()
        {
            int.TryParse(Session["UID"].ToString(), out UID);
            SQLString = "Select R.RTOLocationName,R.RTOLocationID From RTOLocation R inner join UserRTOLocationMapping U on R.RTOLocationID=U.RTOLocationID Where U.UserID=" + UID + " order by R.RTOLocationName";
            // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + RTOLocationID + " and ActiveStatus='Y' Order by RTOLocationName";
            DataSet dss = Utils.getDataSet(SQLString, CnnString);
            dropDownListRTOLocation.DataSource = dss;
            dropDownListRTOLocation.DataBind();
            dropDownListRTOLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select RTO Location--", "--Select RTO Location--"));
            dropDownListRTOLocation.SelectedValue = RTOLocationID.ToString();
        }

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            // HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            // CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            // CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            //OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CashDate.Text =System.DateTime.Now.ToString("dd/MM/yyyy");

            //CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        private void WriteToFile(string path, string x)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(x);

                sw.Close();
            }
        }

        protected void buttonGo_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxAuthorizationNo.Text))
            {
                lblErrMess.Text = String.Empty;
                lblSucMess.Text = String.Empty;
                BlankFieldsAuth();
                try
                {
                    FillHSRPRecordDetails(textBoxAuthorizationNo.Text);
                }
                catch (Exception ex)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append(Environment.NewLine);
                    sb.Append("--**==================================================****");
                    sb.Append("--**" + DateTime.Now.ToLongDateString().ToString() + "****" + Environment.NewLine);
                    sb.Append(ex.Message.ToString() + Environment.NewLine);
                    sb.Append(ex.StackTrace + Environment.NewLine + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("--**==================================================****");
                    sb.Append(Environment.NewLine);

                   // WriteToFile("e:\\log\\getdatalog.log", sb.ToString());

                    string login = "~/Login.aspx?X=" + Session["MacAddress"].ToString();
                    Session.Abandon();
                    Session.Clear();
                    Response.CacheControl = "no-cache";
                    Response.Redirect(login);
                }
            }
        }

        protected void LinkButtonGo2_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(TextBoxVehicleRegNo.Text))
            {
                lblErrMess.Text = String.Empty;
                lblSucMess.Text = String.Empty;
                BlankFieldsVehicleReg();
                try
                {
                    FillHSRPRecordDetails2(TextBoxVehicleRegNo.Text);
                }
                catch (Exception ex)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append(Environment.NewLine);
                    sb.Append("--**==================================================****");
                    sb.Append("--**" + DateTime.Now.ToLongDateString().ToString() + "****" + Environment.NewLine);
                    sb.Append(ex.Message.ToString() + Environment.NewLine);
                    sb.Append(ex.StackTrace + Environment.NewLine + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("--**==================================================****");
                    sb.Append(Environment.NewLine);

                   // WriteToFile("e:\\log\\getdatalog.log", sb.ToString());

                    string login = "~/Login.aspx?X=" + Session["MacAddress"].ToString();
                    Session.Abandon();
                    Session.Clear();
                    Response.CacheControl = "no-cache";
                    Response.Redirect(login);
                }
            }
        }

        protected void LinkButtonGo3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBoxCashReceiptNo.Text))
            {
                lblErrMess.Text = String.Empty;
                lblSucMess.Text = String.Empty;
                BlankFieldsCashReceipt();
                try
                {
                    FillHSRPRecordDetails3(TextBoxCashReceiptNo.Text);
                }
                catch (Exception ex)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append(Environment.NewLine);
                    sb.Append("--**==================================================****");
                    sb.Append("--**" + DateTime.Now.ToLongDateString().ToString() + "****" + Environment.NewLine);
                    sb.Append(ex.Message.ToString() + Environment.NewLine);
                    sb.Append(ex.StackTrace + Environment.NewLine + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("--**==================================================****");
                    sb.Append(Environment.NewLine);

                   // WriteToFile("e:\\log\\getdatalog.log", sb.ToString());

                    string login = "~/Login.aspx?X=" + Session["MacAddress"].ToString();
                    Session.Abandon();
                    Session.Clear();
                    Response.CacheControl = "no-cache";
                    Response.Redirect(login);
                }
            }
        }

        void BlankFields()
        {
            TextBoxCashReceiptNo.Text = String.Empty;
            textBoxAddress1.Text = String.Empty;
            textBoxAddress2.Text = String.Empty;
            textBoxAuthorizationNo.Text = String.Empty;
            textBoxCustomerName.Text = String.Empty;
            textBoxOrderStatus.Text = String.Empty;
            textBoxMobileNo.Text = String.Empty;
            textBoxLandline.Text = String.Empty;
            textBoxEmailId.Text = String.Empty;
            // textBoxVehicleMake.Text = String.Empty;
            TextBoxVehicleRegNo.Text = String.Empty;
            textboxEngineNo.Text = String.Empty;
            textBoxChassisNo.Text = String.Empty;
            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            DropDownListFrontPlateSize.SelectedIndex = 0;
            DropDownListRearPlateSize.SelectedIndex = 0;
            // textBoxInvoiceNo.Text = String.Empty;
            // textBoxOrderNo.Text = String.Empty;
            //textBoxCashReceiptNo.Text = String.Empty;
            //textBoxDelieveryChallan.Text = String.Empty;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            DropDownListVehicleMaker1.SelectedValue = "--Select Vehicle Maker--";
            DropDownListModel.SelectedValue = "--Select Vehicle Model--";
            DropDownListVehicleClass.SelectedValue = "--Select Vehicle Class--";
            DropDownListVehicleModel.SelectedValue = "--Select Vehicle Model--";
            //DropDownListVehicleModel.SelectedValue = "--Select Vehicle Type--";
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
            InitialSetting();
            textBoxAuthorizationNo.Text = String.Empty;
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
        }

        void BlankFieldsSave()
        {
            TextBoxCashReceiptNo.Text = String.Empty;
            textBoxAddress1.Text = String.Empty;
            textBoxAddress2.Text = String.Empty;
            textBoxAuthorizationNo.Text = String.Empty;
            textBoxCustomerName.Text = String.Empty;
            textBoxOrderStatus.Text = String.Empty;
            textBoxMobileNo.Text = String.Empty;
            textBoxLandline.Text = String.Empty;
            textBoxEmailId.Text = String.Empty;
            // textBoxVehicleMake.Text = String.Empty;
            TextBoxVehicleRegNo.Text = String.Empty;
            textboxEngineNo.Text = String.Empty;
            textBoxChassisNo.Text = String.Empty;
            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            DropDownListFrontPlateSize.SelectedIndex = 0;
            DropDownListRearPlateSize.SelectedIndex = 0;
            // textBoxInvoiceNo.Text = String.Empty;
            // textBoxOrderNo.Text = String.Empty;
            //textBoxCashReceiptNo.Text = String.Empty;
            //textBoxDelieveryChallan.Text = String.Empty;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            DropDownListVehicleMaker1.SelectedValue = "--Select Vehicle Maker--";
            DropDownListModel.SelectedValue = "--Select Vehicle Model--";
            DropDownListVehicleClass.SelectedValue = "--Select Vehicle Class--";
            DropDownListVehicleModel.SelectedValue = "--Select Vehicle Model--";
           // DropDownListVehicleModel.SelectedValue = "--Select Vehicle Type--";
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
            InitialSetting();
            textBoxAuthorizationNo.Text = String.Empty;

        }



        void BlankFieldsAuth()
        {
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            TextBoxCashReceiptNo.Text = String.Empty;
            textBoxAddress1.Text = String.Empty;
            textBoxAddress2.Text = String.Empty;
            //textBoxAuthorizationNo.Text = String.Empty;
            textBoxCustomerName.Text = String.Empty;
            textBoxOrderStatus.Text = String.Empty;
            textBoxMobileNo.Text = String.Empty;
            textBoxLandline.Text = String.Empty;
            textBoxEmailId.Text = String.Empty;
            // textBoxVehicleMake.Text = String.Empty;
            TextBoxVehicleRegNo.Text = String.Empty;
            textboxEngineNo.Text = String.Empty;
            textBoxChassisNo.Text = String.Empty;
            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            // textBoxInvoiceNo.Text = String.Empty;
            // textBoxOrderNo.Text = String.Empty;
            //textBoxCashReceiptNo.Text = String.Empty;
            //textBoxDelieveryChallan.Text = String.Empty;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
            //textBoxCashReceiptNo.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            DropDownListVehicleClass.SelectedValue = "--Select Vehicle Class--";
           // DropDownListVehicleModel.SelectedValue = "--Select Vehicle Model--";
            //DropDownListVehicleModel.SelectedValue = "--Select Vehicle Type--";
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            DropDownListVehicleMaker1.SelectedValue = "--Select Vehicle Maker--";
            DropDownListModel.SelectedValue = "--Select Vehicle Model--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
            InitialSetting();
            //textBoxAuthorizationNo.Text = String.Empty;
        }

        void BlankFieldsVehicleReg()
        {
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            textBoxAddress1.Text = String.Empty;
            textBoxAddress2.Text = String.Empty;
            textBoxAuthorizationNo.Text = String.Empty;
            textBoxCustomerName.Text = String.Empty;
            textBoxOrderStatus.Text = String.Empty;
            textBoxMobileNo.Text = String.Empty;
            textBoxLandline.Text = String.Empty;
            textBoxEmailId.Text = String.Empty;
            //textBoxVehicleMake.Text = String.Empty;
            DropDownListVehicleMaker1.SelectedValue = "--Select Vehicle Maker--";
            DropDownListModel.SelectedValue = "--Select Vehicle Model--";
            TextBoxCashReceiptNo.Text = String.Empty;
            // TextBoxVehicleRegNo.Text = String.Empty;
            textboxEngineNo.Text = String.Empty;
            textBoxChassisNo.Text = String.Empty;
            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            // textBoxInvoiceNo.Text = String.Empty;
            // textBoxOrderNo.Text = String.Empty;
            //textBoxCashReceiptNo.Text = String.Empty;
            //textBoxDelieveryChallan.Text = String.Empty;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
            //textBoxCashReceiptNo.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            DropDownListVehicleClass.SelectedValue = "--Select Vehicle Class--";
            DropDownListVehicleModel.SelectedValue = "--Select Vehicle Model--";
           // DropDownListVehicleModel.SelectedValue = "--Select Vehicle Type--";           // code comment
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
            InitialSetting();
            textBoxAuthorizationNo.Text = String.Empty;
        }

        void BlankFieldsCashReceipt()
        {
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            textBoxAddress1.Text = String.Empty;
            textBoxAddress2.Text = String.Empty;
            textBoxAuthorizationNo.Text = String.Empty;
            textBoxCustomerName.Text = String.Empty;
            textBoxOrderStatus.Text = String.Empty;
            textBoxMobileNo.Text = String.Empty;
            textBoxLandline.Text = String.Empty;
            textBoxEmailId.Text = String.Empty;
            //textBoxVehicleMake.Text = String.Empty;
            TextBoxVehicleRegNo.Text = String.Empty;
            textboxEngineNo.Text = String.Empty;
            textBoxChassisNo.Text = String.Empty;
            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            //TextBoxCashReceiptNo.Text = String.Empty;
            // textBoxInvoiceNo.Text = String.Empty;
            // textBoxOrderNo.Text = String.Empty;
            //textBoxCashReceiptNo.Text = String.Empty;
            //textBoxDelieveryChallan.Text = String.Empty;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
            //textBoxCashReceiptNo.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            DropDownListVehicleClass.SelectedValue = "--Select Vehicle Class--";
            DropDownListVehicleModel.SelectedValue = "--Select Vehicle Model--";
           // DropDownListVehicleModel.SelectedValue = "--Select Vehicle Type--";
            DropDownListVehicleMaker1.SelectedValue = "--Select Vehicle Maker--";
            DropDownListModel.SelectedValue = "--Select Vehicle Model--";
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
            InitialSetting();
            textBoxAuthorizationNo.Text = String.Empty;
        }

        void FillMakerAndModel()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster where activestatus='Y' order by VehicleMakerDescription";

            DataTable VehicleMaker = Utils.GetDataTable(SQLString, CnnString);

            foreach (DataRow dr in VehicleMaker.Rows)
            {
                DropDownListVehicleMaker1.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
            }

            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
            DropDownListVehicleMaker1.Items.Insert(0, li);
        }

        private void FillHSRPRecordDetails(String HSRPAuthNo)
        {
            buttonSave.Enabled = true;
            buttonSave.Visible = true;

            DropDownListVehicleMaker1.Items.Clear();
            DropDownListModel.Items.Clear();
            BAL obj = new BAL();

            if (HSRPStateID.ToString().Equals("5"))
            {
                SQLString = "Select HSRPRecord_AuthorizationNo From  HSRPRecords  where hsrp_stateid ='5' and  HSRPRecord_AuthorizationNo='" + HSRPAuthNo + "'";
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count == 0)// yudhvir > on ==
                {
                    if (HSRPStateID.ToString().Equals("5"))
                    {
                        SQLString = "Select * From View_MP_HSRPRecordsStagging_New where HSRPRecord_AuthorizationNo ='" + HSRPAuthNo + "'";
                        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                        dataSetFillHSRPRecordDetail = Utils.getDataSet(SQLString, CnnString);
                        if (dataSetFillHSRPRecordDetail.Tables[0].Rows.Count == 0)//>==
                        {
                            string SecretKey = "JSACNLAY*MY$MKSS";
                            org.mptransport.oltp.HSRP MPDataSerivce = new org.mptransport.oltp.HSRP();
                            DataSet ds = MPDataSerivce.GetOnDemandReference(SecretKey, HSRPAuthNo);
                            if (ds.Tables[0].Rows.Count > 0)
                            {          
                                 string hasid            = ds.Tables[0].Rows[0]["HAS_ID"].ToString();
                                 string rtocode          =ds.Tables[0].Rows[0]["RTO_CODE"].ToString();
                                 string rtoname          =ds.Tables[0].Rows[0]["RTO_NAME"].ToString();
                                 string Refno            = ds.Tables[0].Rows[0]["REFERENCE_NO"].ToString();
                                 string ownername        = ds.Tables[0].Rows[0]["OWNER_NAME"].ToString();
                                 string owneraddress     = ds.Tables[0].Rows[0]["OWNER_ADDRESS"].ToString();
                                 string vehicletype      = ds.Tables[0].Rows[0]["VEHICLE_TYPE"].ToString();
                                 string transtype        = ds.Tables[0].Rows[0]["TRANS_TYPE"].ToString();
                                 string authorizationdate = ds.Tables[0].Rows[0]["AUTHORIZATION_DATE"].ToString();
                                 string contactno        = ds.Tables[0].Rows[0]["CONTACT_NO"].ToString();
                                 string vechtype         = ds.Tables[0].Rows[0]["VEH_TYPE"].ToString();
                                 string vehclasstype     = ds.Tables[0].Rows[0]["VEH_CLASS_TYPE"].ToString();
                                 string mrfsname         = ds.Tables[0].Rows[0]["MFRS_NAME"].ToString();
                                 string modelname        = ds.Tables[0].Rows[0]["MODEL_NAME"].ToString();
                                 string regno            = ds.Tables[0].Rows[0]["REG_NO"].ToString();
                                 string engineno         = ds.Tables[0].Rows[0]["ENGINE_NO"].ToString();
                                 string chassisno        = ds.Tables[0].Rows[0]["CHASSIS_NO"].ToString();
                                 string tokenid          = ds.Tables[0].Rows[0]["TOKEN_ID"].ToString();

                                 

                                // HSRP_StagingData
                                 SQLString = "insert into HSRP_StagingData(RTO_CODE,HAS_ID, AUTHORIZATION_DATE, REFERENCE_NO ,OWNER_NAME,TRANS_TYPE,OWNER_ADDRESS,CONTACT_NO,REG_NO,ENGINE_NO,CHASSIS_NO,mfrs_name,model_name,CRM_VEHICLE_TYPE,CRM_VEH_CLASS_TYPE,TOKEN_ID) values('" + rtocode + "','" + hasid + "','" + authorizationdate + "','" + Refno + "','" + ownername + "','" + transtype + "','" + owneraddress + "','" + contactno + "','" + regno + "','" + engineno + "','" + chassisno + "','" + mrfsname + "','" + modelname + "','" + vechtype + "',(select  mapping_value from  [dbo].[HSRP_Mappings] where mapping_key ='VEH_CLASS_TYPE' and key_value= '" + vehclasstype + "'),'" + tokenid + "')";
                                Utils.ExecNonQuery(SQLString, CnnString);
                                // lblErrMess.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                                SQLString = "Select * From View_MP_HSRPRecordsStagging_New where HSRPRecord_AuthorizationNo ='" + HSRPAuthNo + "'";
                                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                                dataSetFillHSRPRecordDetail = Utils.getDataSet(SQLString, CnnString);
                        
                            }
                        }   
                    }
                }
                else
                {
                    obj.FillHSRPRecordDetail(HSRPAuthNo, ref dataSetFillHSRPRecordDetail);
                }
                
            }


            else
            {
                obj.FillHSRPRecordDetail(HSRPAuthNo, ref dataSetFillHSRPRecordDetail);
            }

            if (dataSetFillHSRPRecordDetail.Tables[0].Rows.Count < 1)
            {
                if (obj.FillHSRPRecordDetailXX(HSRPAuthNo, ref dataSetFillHSRPRecordDetail))
                {
                    if (dataSetFillHSRPRecordDetail.Tables[0].Rows.Count < 1)
                    {
                        //>>> Cash ReceiptDetail
                        DataTable dttt = new DataTable();
                        string ConnectionCashReceiptno = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringCashReceipt"].ToString();
                        string cmd = "Select 'HHT/'+Prefix+RIGHT('00000000'+convert(varchar,(billno)), 8) as CashReceiptNo,name,mobileno,hrsp,regno from collection_details Where hrsp='" + HSRPAuthNo + "'";
                        dttt = Utils.GetDataTable(cmd, ConnectionCashReceiptno);
                        if (dttt.Rows.Count <= 0)
                        {
                            BlankFieldsAuth();
                            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                            SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";
                            DataTable VehicleMaker1 = Utils.GetDataTable(SQLString, CnnString);
                            foreach (DataRow dr in VehicleMaker1.Rows)
                            {
                                DropDownListVehicleMaker1.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
                            }
                            System.Web.UI.WebControls.ListItem li1 = new System.Web.UI.WebControls.ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
                            DropDownListVehicleMaker1.Items.Insert(0, li1);

                                
                            lblErrMess.Text = "No Such Record Exists.";
                            UpdatePanelMessage.Update();
                            textBoxAuthorizationNo.Focus();
                            return;
                        }
                        else
                        {
                            lblErrMess.Text = "";
                            TextBoxCashReceiptNo.Text = dttt.Rows[0]["CashReceiptNo"].ToString();
                            if (TextBoxCashReceiptNo.Text.Equals("0"))
                            {
                                TextBoxCashReceiptNo.Text = string.Empty;
                                TextBoxCashReceiptNo.Enabled = false;
                            }
                            else
                            {
                                TextBoxCashReceiptNo.Enabled = true;

                            }
                            textBoxCustomerName.Text = dttt.Rows[0]["name"].ToString();
                            textBoxMobileNo.Text = dttt.Rows[0]["mobileno"].ToString();
                            textBoxAuthorizationNo.Text = dttt.Rows[0]["hrsp"].ToString();
                            TextBoxVehicleRegNo.Text = dttt.Rows[0]["regno"].ToString();


                            if (string.IsNullOrEmpty(TextBoxCashReceiptNo.Text))
                            {
                                TextBoxCashReceiptNo.Enabled = true;
                            }
                            else
                            {
                                TextBoxCashReceiptNo.Enabled = false;
                            }
                            /////>>>>>> Vehicle Maker And Model DropdownBind
                            /////
                            FillMakerAndModel();
                            return;
                        }
                    }
                }
            }
            int ISExists = -1;

            //if (obj.CheckHSRPRecordAuthNo(HSRPAuthNo, ref ISExists))
            if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, TextBoxVehicleRegNo.Text.Trim(), HSRPStateID, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
                    lblErrMess.Text = "Record Already Exist!!";
                }
                else
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                }
            }

            //>>> CashReceiptDetail

            string ConnectionCashReceiptno1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringCashReceipt"].ToString();
            //string cmd1 = "Select 'HHT/'+Prefix+RIGHT('00000000'+convert(varchar,(billno)), 8) as CashReceiptNo from collection_details Where hrsp='" + HSRPAuthNo + "'";
            obj.FillHSRPRecord_HHT(HSRPAuthNo, ref dataSetFillHSRPRecord_HHT);

            if (dataSetFillHSRPRecord_HHT.Tables[0].Rows.Count > 0)
            {
                TextBoxCashReceiptNo.Text = dataSetFillHSRPRecord_HHT.Tables[0].Rows[0]["CashReceiptNo"].ToString();
            }
            else
            {
                TextBoxCashReceiptNo.Text = string.Empty;
            }



            //TextBoxCashReceiptNo.Text = dataSetFillHSRPRecord_HHT.Tables[0].Rows[0]["CashReceiptNo"].ToString();  // Utils.getDataSingleValue(cmd1, ConnectionCashReceiptno1, "CashReceiptNo");
            
            //if (TextBoxCashReceiptNo.Text.Equals("0"))
            //{
            //    TextBoxCashReceiptNo.Text = string.Empty;
            //}

            //>>>>> Authorization Detail

            textBoxAuthorizationNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
            if (!String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationDate"].ToString()))
            {
                string[] DateHSRP = (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationDate"].ToString()).Split(' ');
                string[] DateHSRPpp = DateHSRP[0].Split('/');
                DateTime HSRPdd = DateTime.Parse(DateHSRPpp[0] + "/" + DateHSRPpp[1] + "/" + DateHSRPpp[2]);
                OrderDate.SelectedDate = HSRPdd;
                CalendarOrderDate.SelectedDate = HSRPdd;
                CalendarOrderDate.VisibleDate = HSRPdd;
            }
            if (!String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptDatetime_HHT"].ToString()))
            {
                if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptDatetime_HHT"].ToString().Equals("1/1/1900 12:00:00 AM"))
                {
                    string[] DateCash = (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptDatetime_HHT"].ToString()).Split(' ');
                    string[] DateCashpp = DateCash[0].Split('/');
                    DateTime Cashdd = DateTime.Parse(DateCashpp[0] + "/" + DateCashpp[1] + "/" + DateCashpp[2]);
                   // CashDate.SelectedDate = System.DateTime.Now;
                   // CalendarCashDate.SelectedDate = System.DateTime.Now;
                   // CalendarCashDate.VisibleDate = System.DateTime.Now;
                }
                else
                {
                    string[] DateCash = (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptDatetime_HHT"].ToString()).Split(' ');
                    string[] DateCashpp = DateCash[0].Split('/');
                    DateTime Cashdd = DateTime.Parse(DateCashpp[0] + "/" + DateCashpp[1] + "/" + DateCashpp[2]);
                   // CashDate.SelectedDate = Cashdd;
                    CalendarCashDate.SelectedDate = Cashdd;
                    CalendarCashDate.VisibleDate = Cashdd;
                }
            }

            if (string.IsNullOrEmpty(TextBoxCashReceiptNo.Text))
            {
                TextBoxCashReceiptNo.Enabled = true;
            }
            else
            {
                TextBoxCashReceiptNo.Enabled = false;
            }

            //textBoxOrderNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderNo"].ToString();
            textBoxOrderStatus.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderStatus"].ToString();

            if (!string.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptNo_HHT"].ToString()))
            {
                TextBoxCashReceiptNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptNo_HHT"].ToString();
            }

            textBoxCustomerName.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OwnerName"].ToString();
            textBoxOrderStatus.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderStatus"].ToString();
            textBoxAddress1.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["Address1"].ToString();
            textBoxAddress2.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["Address2"].ToString();
            textBoxMobileNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["MobileNo"].ToString();
            textBoxLandline.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["LandlineNo"].ToString();
            textBoxEmailId.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["EmailID"].ToString();
            ///>>>> Vehicle Info
            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString()))
            {
                DropDownListVehicleClass.SelectedIndex = 0;
            }
            else
            {
                string strVehClass = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString().ToUpper().Equals("TRANSPORT") ? "Transport" : "Non-Transport";
                DropDownListVehicleClass.SelectedValue = strVehClass;
            }

            //textBoxVehicleMake.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString();
            // FillVehicleMaker();
            //  FillMakerAndModel();

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";

            DataTable VehicleMakerr = Utils.GetDataTable(SQLString, CnnString);

            foreach (DataRow dr in VehicleMakerr.Rows)
            {
                DropDownListVehicleMaker1.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
            }

            System.Web.UI.WebControls.ListItem liis = new System.Web.UI.WebControls.ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
            DropDownListVehicleMaker1.Items.Insert(0, liis);

            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString()) || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString().Equals("--Select Vehicle Maker--"))
            {
                DropDownListVehicleMaker1.SelectedIndex = 0;
            }
            else
            {
                DropDownListVehicleMaker1.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString();
                // FillVehicleModel();
                if (!DropDownListVehicleMaker1.SelectedValue.Equals("--Select Vehicle Maker--"))
                {
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    SQLString = "Select VehicleModelID,VehicleModelDescription From VehicleModelMaster where VehicleMakerID=" + DropDownListVehicleMaker1.SelectedValue + " order by VehicleModelDescription";

                    DataTable VehicleModel = Utils.GetDataTable(SQLString, CnnString);

                    foreach (DataRow dr in VehicleModel.Rows)
                    {
                        DropDownListModel.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleModelDescription"].ToString(), dr["VehicleModelID"].ToString()));
                    }

                    System.Web.UI.WebControls.ListItem liie = new System.Web.UI.WebControls.ListItem("--Select Vehicle Model--", "--Select Vehicle Model--");
                    DropDownListModel.Items.Insert(0, liie);

                    if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerModel"].ToString()) || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerModel"].ToString().Equals("--Select Vehicle Model--"))
                    {
                        DropDownListModel.SelectedIndex = 0;
                    }
                    else
                    {
                        DropDownListModel.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerModel"].ToString();
                    }
                }
            }



            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString()))
            {
                DropDownListVehicleModel.SelectedIndex = 0;
            }
            else
            {
                string sqlquery = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString();
               // DropDownListVehicleModel.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString();
                DropDownListVehicleModel.SelectedValue = sqlquery.ToUpper();
            }
            TextBoxVehicleRegNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleRegNo"].ToString().Replace(" ", "");
            textboxEngineNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["EngineNo"].ToString();
            textBoxChassisNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ChassisNo"].ToString();
            ////>>>> Number Plate Info


            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString()))
            {
                DropDownListOrderType.SelectedIndex = 0;
            }
            else
            {
                DropDownListOrderType.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString();
                if ((DropDownListVehicleClass.SelectedIndex != 0) && (DropDownListVehicleModel.SelectedIndex != 0))
                {
                    if (DropDownListOrderType.SelectedIndex != 0)
                    {
                        OrderSelectionChange();
                    }
                    else
                    {
                        textBoxVat.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VAT_Percentage"].ToString();
                        textBoxVatAmount.Text = textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VAT_Amount"].ToString()), 2).ToString();
                        textBoxServiceTax.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ServiceTax_Percentage"].ToString();
                        textboxServiceTaxAmount.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ServiceTax_Amount"].ToString();
                        textBoxTotalAmount.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["TotalAmount"].ToString()), 2).ToString();
                        textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["NetAmount"].ToString()), 2).ToString();
                    }
                }
            }
        }

        private void FillHSRPRecordDetails2(String VehicleRegNo)
        {
            buttonSave.Enabled = true;
            buttonSave.Visible = true;
            BAL obj = new BAL();
            DropDownListVehicleMaker1.Items.Clear();
            DropDownListModel.Items.Clear();

            if (TextBoxVehicleRegNo.Text.Length > 5)
            {
                if (TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09HG" || TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09GG" || TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09LP" || TextBoxVehicleRegNo.Text.Substring(0, 5) == "MP09R" || TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09TA" || TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09FA")
                {
                    if (TextBoxVehicleRegNo.Text == "MP09GG0414" || TextBoxVehicleRegNo.Text == "MP09GG0441" || TextBoxVehicleRegNo.Text == "MP09GG0423" || TextBoxVehicleRegNo.Text == "mp09gg1011" || TextBoxVehicleRegNo.Text == "mp09hg9565" || TextBoxVehicleRegNo.Text == "mp09hg9825" || TextBoxVehicleRegNo.Text == "mp09hg9631" || TextBoxVehicleRegNo.Text == "mp09hg9731" || TextBoxVehicleRegNo.Text == "mp09hg9531" || TextBoxVehicleRegNo.Text == "mp09gg0011" || TextBoxVehicleRegNo.Text == "mp09gg0786" || TextBoxVehicleRegNo.Text == "mp09hg9473" || TextBoxVehicleRegNo.Text == "mp09hg9573" || TextBoxVehicleRegNo.Text == "mp09gf1234")
                        lblErrMess.Text = "";
                    else
                        lblErrMess.Text = "";
                    return;
                }

            }

            if (HSRPStateID.ToString().Equals("5"))
            {
                 //left(vehicleregno,6)  not IN ('MP09HG','MP09GG','MP09LP','MP09TA','MP09FA') and left(vehicleregno,5)  not IN ('MP09R')
                SQLString = "Select HSRPRecord_AuthorizationNo From  HSRPRecords where hsrp_stateid ='5' and  VehicleRegNo='" + VehicleRegNo + "' ";
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count == 0)
                {
                    if (HSRPStateID.ToString().Equals("5"))
                    {
                        SQLString = "Select * From View_MP_HSRPRecordsStagging_New where VehicleRegNo='" + VehicleRegNo + "'  ";
                        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringMP"].ToString();
                        dataSetFillHSRPRecordDetail = Utils.getDataSet(SQLString, CnnString);
                        
                    }
                  
                }
                else
                {
                    obj.FillHSRPRecordDetail2(VehicleRegNo, ref dataSetFillHSRPRecordDetail);
                }
            }
            else
            {
                obj.FillHSRPRecordDetail2(VehicleRegNo, ref dataSetFillHSRPRecordDetail);
            }

            if (dataSetFillHSRPRecordDetail.Tables[0].Rows.Count < 1)
            {
                if (obj.FillHSRPRecordDetail2XX(VehicleRegNo, ref dataSetFillHSRPRecordDetail))
                {
                    if (dataSetFillHSRPRecordDetail.Tables[0].Rows.Count < 1)
                    {

                        //>>> Cash ReceiptDetail
                        DataTable dttt = new DataTable();
                        string ConnectionCashReceiptno = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringCashReceipt"].ToString();
                       //string cmd = "Select 'HHT/'+Prefix+RIGHT('00000000'+convert(varchar,(billno)), 8) as CashReceiptNo,name,mobileno,hrsp,regno from collection_details Where regno='" + VehicleRegNo + "'";
                        obj.FillHSRPRecord_HHT(VehicleRegNo, ref dataSetFillHSRPRecord_HHT);
                       // dttt = Utils.GetDataTable(cmd, ConnectionCashReceiptno);
                        if (dataSetFillHSRPRecord_HHT.Tables[0].Rows.Count <= 0)
                        {
                            BlankFieldsVehicleReg();
                            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                            SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster where activestatus='Y' order by VehicleMakerDescription";
                            DataTable VehicleMaker2 = Utils.GetDataTable(SQLString, CnnString);
                            foreach (DataRow dr in VehicleMaker2.Rows)
                            {
                                DropDownListVehicleMaker1.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
                            }
                            System.Web.UI.WebControls.ListItem li2 = new System.Web.UI.WebControls.ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
                            DropDownListVehicleMaker1.Items.Insert(0, li2);
                            lblErrMess.Text = "No Such Record Exists.";
                            UpdatePanelMessage.Update();
                            TextBoxVehicleRegNo.Focus();
                            return;
                        }
                        else
                        {
                            lblErrMess.Text = "";
                            TextBoxCashReceiptNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptNo"].ToString();
                            if (TextBoxCashReceiptNo.Text.Equals("0"))
                            {
                                TextBoxCashReceiptNo.Text = string.Empty;
                            }

                            textBoxCustomerName.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["name"].ToString();
                            textBoxMobileNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["mobileno"].ToString();
                            textBoxAuthorizationNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["hrsp"].ToString();
                           // textBoxAuthorizationNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationNo"].ToString();

                            TextBoxVehicleRegNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["regno"].ToString();

                            if (string.IsNullOrEmpty(TextBoxCashReceiptNo.Text))
                            {
                                TextBoxCashReceiptNo.Enabled = true;
                            }
                            else
                            {
                                TextBoxCashReceiptNo.Enabled = false;
                            }
                            /////>>>>>> Vehicle Maker And Model DropdownBind
                            /////
                            FillMakerAndModel();
                            return;
                        }
                    }
                }
            }

            int ISExists = -1;

            if (obj.CheckHSRPRecordHSRPVehicleReg(VehicleRegNo, ref ISExists))
            {
                lblMessageError.Text = "";
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
                    buttonUpdate.Visible = false;
                }
                else if (ISExists.Equals(2))
                {
                    buttonUpdate.Visible = true;
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
                    lblErrMess.Text = "For Update This Vehicle Reg. No. Please Use Edit Cash Collection Menu";
                    lblMessageError.Text = "For Update This Vehicle Reg. No. Please Use Edit Cash Collection Menu";
                     
                }
                else
                {
                    lblMessageError.Text = "";
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                    buttonUpdate.Visible = false;
                }
            }

            //>> CashReceiptDetail

            string ConnectionCashReceiptno1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringCashReceipt"].ToString();
           // string cmd1 = "Select top 1 'HHT/'+Prefix+RIGHT('00000000'+convert(varchar,(billno)), 8) as CashReceiptNo from collection_details Where regno='" + VehicleRegNo + "'";
            //string cmd1 = "Select count(*) from collection_details Where regno='" + VehicleRegNo + "'";

            obj.FillHSRPRecord_HHT(VehicleRegNo, ref dataSetFillHSRPRecord_HHT);
            if (dataSetFillHSRPRecord_HHT.Tables[0].Rows.Count>0)
                TextBoxCashReceiptNo.Text = dataSetFillHSRPRecord_HHT.Tables[0].Rows[0]["CashReceiptNo"].ToString();  // Utils.getDataSingleValue(cmd1, ConnectionCashReceiptno1, "CashReceiptNo");
            else
                TextBoxCashReceiptNo.Text = string.Empty;

            //>>>>> Authorization Detail
            //HSRPRecordIDXX = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecordID"].ToString();
            //HiddenFieldHSRPRecords.Text = HSRPRecordIDXX;
            textBoxAuthorizationNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
            if (!String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationDate"].ToString()))
            {
                string[] DateHSRP = (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationDate"].ToString()).Split(' ');
                string[] DateHSRPpp = DateHSRP[0].Split('/');
                DateTime HSRPdd = DateTime.Parse(DateHSRPpp[0] + "/" + DateHSRPpp[1] + "/" + DateHSRPpp[2]);
                OrderDate.SelectedDate = HSRPdd;
                CalendarOrderDate.SelectedDate = HSRPdd;
                CalendarOrderDate.VisibleDate = HSRPdd;
            }
            if (!String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptDatetime_HHT"].ToString()))
            {
                if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptDatetime_HHT"].ToString().Equals("1/1/1900 12:00:00 AM"))
                {
                    string[] DateCash = (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptDatetime_HHT"].ToString()).Split(' ');
                    string[] DateCashpp = DateCash[0].Split('/');
                    DateTime Cashdd = DateTime.Parse(DateCashpp[0] + "/" + DateCashpp[1] + "/" + DateCashpp[2]);
                   // CashDate.SelectedDate = System.DateTime.Now;
                  //  CalendarCashDate.SelectedDate = System.DateTime.Now;
                  //  CalendarCashDate.VisibleDate = System.DateTime.Now;
                }
                else
                {
                    string[] DateCash = (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptDatetime_HHT"].ToString()).Split(' ');
                    string[] DateCashpp = DateCash[0].Split('/');
                    DateTime Cashdd = DateTime.Parse(DateCashpp[0] + "/" + DateCashpp[1] + "/" + DateCashpp[2]);
                   // CashDate.SelectedDate = Cashdd;
                    CalendarCashDate.SelectedDate = Cashdd;
                    CalendarCashDate.VisibleDate = Cashdd;
                }
            }

            if (string.IsNullOrEmpty(TextBoxCashReceiptNo.Text))
            {
                TextBoxCashReceiptNo.Enabled = true;
            }
            else
            {
                TextBoxCashReceiptNo.Enabled = false;
            }

            //textBoxOrderNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderNo"].ToString();
            textBoxOrderStatus.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderStatus"].ToString();
            if (!string.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptNo_HHT"].ToString()))
            {
                TextBoxCashReceiptNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptNo_HHT"].ToString();
            }


            textBoxCustomerName.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OwnerName"].ToString();
            textBoxOrderStatus.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderStatus"].ToString();
            textBoxAddress1.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["Address1"].ToString();
            textBoxAddress2.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["Address2"].ToString();
            textBoxMobileNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["MobileNo"].ToString();
            textBoxLandline.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["LandlineNo"].ToString();
            textBoxEmailId.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["EmailID"].ToString();
            ///>>>> Vehicle Info
            ///

            string str = string.Empty;
             if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString() == "NON-TRANSPORT")
             {
                 str = "Non-Transport";
             }
             else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString() == "TRANSPORT")
             {
                 str = "Transport";
             }
             else
             {
                 str = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString();
             }

          //if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString()))
             if (String.IsNullOrEmpty(str))
            {
                DropDownListVehicleClass.SelectedIndex = 0;
            }
            else
            {
               // DropDownListVehicleClass.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString();
                //DropDownListVehicleClass.SelectedItem.Text = str;
                  DropDownListVehicleClass.SelectedValue = str;
            }
            //textBoxVehicleMake.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString();
            // FillVehicleMaker();
            //  FillMakerAndModel();

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";

            DataTable VehicleMaker = Utils.GetDataTable(SQLString, CnnString);

            foreach (DataRow dr in VehicleMaker.Rows)
            {
                DropDownListVehicleMaker1.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
            }

            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
            DropDownListVehicleMaker1.Items.Insert(0, li);

            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString()) || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString().Equals("--Select Vehicle Maker--"))
            {
                DropDownListVehicleMaker1.SelectedIndex = 0;
            }
            else
            {
               // DropDownListVehicleMaker1.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString();
                // FillVehicleModel();
                if (!DropDownListVehicleMaker1.SelectedValue.Equals("--Select Vehicle Maker--"))
                {
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    SQLString = "Select VehicleModelID,VehicleModelDescription From VehicleModelMaster where VehicleMakerID=" + DropDownListVehicleMaker1.SelectedValue + " order by VehicleModelDescription";

                    DataTable VehicleModel = Utils.GetDataTable(SQLString, CnnString);                    

                    foreach (DataRow dr in VehicleModel.Rows)
                    {
                        DropDownListModel.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleModelDescription"].ToString(), dr["VehicleModelID"].ToString()));
                    }

                    System.Web.UI.WebControls.ListItem lii = new System.Web.UI.WebControls.ListItem("--Select Vehicle Model--", "--Select Vehicle Model--");
                    DropDownListModel.Items.Insert(0, lii);

                    if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerModel"].ToString()) || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerModel"].ToString().Equals("--Select Vehicle Model--") || VehicleModel.Rows.Count==0)
                    {
                        DropDownListModel.SelectedIndex = 0;
                    }
                    else
                    {
                        DropDownListModel.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerModel"].ToString();
                    }
                }
            }
            
            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString()))
            {
                DropDownListVehicleModel.SelectedIndex = 0;
            }
            else
            {
              //  DropDownListVehicleModel.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().ToUpper();
            }
            TextBoxVehicleRegNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleRegNo"].ToString().Replace(" ", "");
            textboxEngineNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["EngineNo"].ToString();
            textBoxChassisNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ChassisNo"].ToString();
            ////>>>> Number Plate Info


            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString()))
            {
                DropDownListOrderType.SelectedIndex = 0;
            }
            else
            {
                DropDownListOrderType.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString();
                if ((DropDownListVehicleClass.SelectedIndex != 0) && (DropDownListVehicleModel.SelectedIndex != 0))
                {
                    if (DropDownListOrderType.SelectedIndex != 0)
                    {
                        OrderSelectionChange();
                    }
                    else
                    {
                        textBoxVat.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VAT_Percentage"].ToString();
                        textBoxVatAmount.Text = textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VAT_Amount"].ToString()), 2).ToString();
                        textBoxServiceTax.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ServiceTax_Percentage"].ToString();
                        textboxServiceTaxAmount.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ServiceTax_Amount"].ToString();
                        textBoxTotalAmount.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["TotalAmount"].ToString()), 2).ToString();
                        textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["NetAmount"].ToString()), 2).ToString();
                    }
                }
            }
        }

        private void FillHSRPRecordDetails3(String CashReceiptNo)
        {
            DropDownListVehicleMaker1.Items.Clear();
            DropDownListModel.Items.Clear();
            BAL obj = new BAL();


            //SQLString = "Select CashReceiptNo From  HSRPRecords where VehicleRegNo='" + CashReceiptNo + "'";
            //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //if (string.IsNullOrEmpty(Utils.GetDataTable(SQLString, CnnString).Rows[0]["CashReceiptNo"].ToString()))
            //{
            //    if (HSRPStateID.ToString().Equals("5"))
            //    {
            //        SQLString = "Select * From View_MP_HSRPRecordsStagging where CashReceiptNo= '" + CashReceiptNo + "'";
            //        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //        dataSetFillHSRPRecordDetail = Utils.getDataSet(SQLString, CnnString);
            //    }
            //}


            if (HSRPStateID.ToString().Equals("5"))
            {
                SQLString = "Select HSRPRecord_AuthorizationNo From HSRPRecords where hsrp_sateid ='5' and VehicleRegNo='" + CashReceiptNo + "'";
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count == 0)
                {
                    if (HSRPStateID.ToString().Equals("5"))
                    {
                        SQLString = "Select * From View_MP_HSRPRecordsStagging where CashReceiptNo= '" + CashReceiptNo + "'";
                        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringMP"].ToString();
                        dataSetFillHSRPRecordDetail = Utils.getDataSet(SQLString, CnnString);
                    }
                }
                else
                {
                    obj.FillHSRPRecordDetail3(CashReceiptNo, ref dataSetFillHSRPRecordDetail);
                }
            }


            else
            {

                obj.FillHSRPRecordDetail3(CashReceiptNo, ref dataSetFillHSRPRecordDetail);
            }
            if (dataSetFillHSRPRecordDetail.Tables[0].Rows.Count < 1)
            {
                if (obj.FillHSRPRecordDetail2XX(CashReceiptNo, ref dataSetFillHSRPRecordDetail))
                {
                    if (dataSetFillHSRPRecordDetail.Tables[0].Rows.Count < 1)
                    {
                        BlankFieldsCashReceipt();
                        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                        SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";
                        DataTable VehicleMaker3 = Utils.GetDataTable(SQLString, CnnString);
                        foreach (DataRow dr in VehicleMaker3.Rows)
                        {
                            DropDownListVehicleMaker1.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
                        }
                        System.Web.UI.WebControls.ListItem li3 = new System.Web.UI.WebControls.ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
                        DropDownListVehicleMaker1.Items.Insert(0, li3);
                        lblErrMess.Text = "No Such Record Exists.";
                        UpdatePanelMessage.Update();
                        TextBoxCashReceiptNo.Focus();
                        return;
                    }
                    else
                    {
                        lblErrMess.Text = "";
                    }
                }
            }

            int ISExists = -1;

            if (obj.CheckHSRPRecordHSRPVehicleReg(CashReceiptNo, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
                }
                else
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                }
            }

            //>>>>> Authorization Detail
            textBoxAuthorizationNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
            if (!String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationDate"].ToString()))
            {
                string[] DateHSRP = (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["HSRPRecord_AuthorizationDate"].ToString()).Split(' ');
                string[] DateHSRPpp = DateHSRP[0].Split('/');
                DateTime HSRPdd = DateTime.Parse(DateHSRPpp[0] + "/" + DateHSRPpp[1] + "/" + DateHSRPpp[2]);
                OrderDate.SelectedDate = HSRPdd;
                CalendarOrderDate.SelectedDate = HSRPdd;
                CalendarOrderDate.VisibleDate = HSRPdd;
            }

            //textBoxOrderNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderNo"].ToString();
            textBoxOrderStatus.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderStatus"].ToString();
            textBoxCustomerName.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OwnerName"].ToString();
            textBoxOrderStatus.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderStatus"].ToString();
            textBoxAddress1.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["Address1"].ToString();
            textBoxAddress2.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["Address2"].ToString();
            textBoxMobileNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["MobileNo"].ToString();
            textBoxLandline.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["LandlineNo"].ToString();
            textBoxEmailId.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["EmailID"].ToString();
            ///>>>> Vehicle Info
            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString()))
            {
                DropDownListVehicleClass.SelectedIndex = 0;
            }
            else
            {
                DropDownListVehicleClass.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString();
            }

            //textBoxVehicleMake.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString();



            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";

            DataTable VehicleMaker = Utils.GetDataTable(SQLString, CnnString);

            foreach (DataRow dr in VehicleMaker.Rows)
            {
                DropDownListVehicleMaker1.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
            }

            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
            DropDownListVehicleMaker1.Items.Insert(0, li);

            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString()))
            {
                DropDownListVehicleMaker1.SelectedIndex = 0;
            }
            else
            {
                DropDownListVehicleMaker1.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString();
                // FillVehicleModel();

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                SQLString = "Select VehicleModelID,VehicleModelDescription From VehicleModelMaster where VehicleMakerID=" + DropDownListVehicleMaker1.SelectedValue + " order by VehicleModelDescription";

                DataTable VehicleModel = Utils.GetDataTable(SQLString, CnnString);

                foreach (DataRow dr in VehicleModel.Rows)
                {
                    DropDownListModel.Items.Add(new System.Web.UI.WebControls.ListItem(dr["VehicleModelDescription"].ToString(), dr["VehicleModelID"].ToString()));
                }

                System.Web.UI.WebControls.ListItem lii = new System.Web.UI.WebControls.ListItem("--Select Vehicle Model--", "--Select Vehicle Model--");
                DropDownListModel.Items.Insert(0, lii);

                if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerModel"].ToString()))
                {
                    DropDownListModel.SelectedIndex = 0;
                }
                else
                {
                    DropDownListModel.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerModel"].ToString();
                }
            }



            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString()))
            {
                DropDownListVehicleModel.SelectedIndex = 0;
            }
            else
            {
                DropDownListVehicleModel.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString();
            }
            TextBoxVehicleRegNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleRegNo"].ToString();
            textboxEngineNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["EngineNo"].ToString();
            textBoxChassisNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ChassisNo"].ToString();
            ////>>>> Number Plate Info


            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString()))
            {
                DropDownListOrderType.SelectedIndex = 0;
            }
            else
            {
                DropDownListOrderType.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString();
                if ((DropDownListVehicleClass.SelectedIndex != 0) && (DropDownListVehicleModel.SelectedIndex != 0))
                {
                    if (DropDownListOrderType.SelectedIndex != 0)
                    {
                        OrderSelectionChange();
                    }
                    else
                    {
                        textBoxVat.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VAT_Percentage"].ToString();
                        textBoxVatAmount.Text = textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VAT_Amount"].ToString()), 2).ToString();
                        textBoxServiceTax.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ServiceTax_Percentage"].ToString();
                        textboxServiceTaxAmount.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ServiceTax_Amount"].ToString();
                        textBoxTotalAmount.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["TotalAmount"].ToString()), 2).ToString();
                        textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["NetAmount"].ToString()), 2).ToString();
                    }
                }
            }
        }


        //private void UpdateHSRPRecordDetail(int HSRPRecordID)
        //{
        //    dropDownListOrganization.Enabled = false;
        //    dropDownListClient.Enabled = false;
        //    textBoxDeviceID.ReadOnly = true;

        //    SQLString = "Select * From DeviceMaster Where DID=" + Convert.ToInt32(DID);
        //    Utils dbLink = new Utils();
        //    dbLink.strProvider = CnnString;
        //    dbLink.CommandTimeOut = 600;
        //    dbLink.sqlText = SQLString.ToString();
        //    SqlDataReader PReader = dbLink.GetReader();
        //    while (PReader.Read())
        //    {
        //        textBoxDeviceID.Text = PReader["DeviceID"].ToString();
        //        textBoxDeviceID.ReadOnly = true;
        //        textBoxBatchNo.Text = PReader["BatchNo"].ToString();
        //        textBoxMobileNo.Text = PReader["MobileNo"].ToString();
        //        textBoxSerialNo.Text = PReader["SerialNo"].ToString();
        //        textBoxIMEINo.Text = PReader["IMEINo"].ToString();
        //        if (PReader["InstallationStatus"].ToString() == "NotInstalled")
        //        {
        //            labelInstallationStatus.Text = " Not Installed";
        //        }
        //        else
        //        {
        //            labelInstallationStatus.Text = "Installed";
        //        }
        //        if (PReader["DeviceStatus"].ToString() == "Y")
        //        {
        //            checkBoxDeviceStatus.Checked = true;
        //        }
        //        else
        //        {
        //            checkBoxDeviceStatus.Checked = false;
        //        }
        //        //FilldropDownListOrganization(UserType);
        //        //dropDownListOrg.SelectedValue = PReader["OrgID"].ToString();
        //        //FilldropDownListClient(Convert.ToInt32(PReader["OrgID"].ToString()));
        //        //dropDownListClient.Enabled = true;
        //        //dropDownListClient.SelectedValue = PReader["ClientID"].ToString();
        //    }

        //    PReader.Close();
        //    dbLink.CloseConnection();
        //}

        //protected void buttonUpdate_Click(object sender, EventArgs e)
        //{
        //    StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
        //    StringIMEINo = textBoxIMEINo.Text.Trim().Replace("'", "''").ToString();
        //    StringBatchNo = textBoxBatchNo.Text.Trim().Replace("'", "''").ToString();
        //    StringSerialNo = textBoxSerialNo.Text.Trim().Replace("'", "''").ToString();
        //    StringDeviceID = textBoxDeviceID.Text.Trim().Replace("'", "''").ToString();
        //    lblErrMess.Text = String.Empty;

        //    if (string.IsNullOrEmpty(StringDeviceID))
        //    {
        //        lblErrMess.Text = "Please Provide Device ID.";
        //        textBoxDeviceID.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(StringMobileNo))
        //    {
        //        lblErrMess.Text = "Please Provide Mobile Number.";
        //        textBoxDeviceID.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(StringIMEINo))
        //    {
        //        lblErrMess.Text = "Please provide IMEI No.";
        //        textBoxDeviceID.Focus();
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(StringSerialNo))
        //    {
        //        lblErrMess.Text = "Please Provide Mobile Number.";
        //        textBoxDeviceID.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(StringMobileNo))
        //    {
        //        lblErrMess.Text = "Please provide IMEI No.";
        //        textBoxDeviceID.Focus();
        //        return;
        //    }

        //    SQLString = "Select Count(*) From dbo.DeviceMaster Where DeviceID='" + StringDeviceID + "' and DID!= " + DID;
        //    if (Utils.getScalarCount(SQLString, CnnString) > 0)
        //    {
        //        lblErrMess.Text = "Device already exists.";
        //        textBoxDeviceID.Focus();
        //        return;
        //    }
        //    SQLString = "Select Count(*) From dbo.DeviceMaster Where MobileNo='" + StringMobileNo + "' and DID!= " + DID;
        //    if (Utils.getScalarCount(SQLString, CnnString) > 0)
        //    {
        //        lblErrMess.Text = "Mobile Number already in Use.";
        //        textBoxMobileNo.Focus();
        //        return;
        //    }
        //    SQLString = "Select Count(*) From dbo.DeviceMaster Where SerialNo='" + StringSerialNo + "' and DID!= " + DID;
        //    if (Utils.getScalarCount(SQLString, CnnString) > 0)
        //    {
        //        lblErrMess.Text = "Serial Number already in Use.";
        //        textBoxSerialNo.Focus();
        //        return;
        //    }
        //    if (checkBoxDeviceStatus.Checked == true)
        //    {
        //        StringDeviceStatus = "Y";
        //    }
        //    else
        //    {
        //        StringDeviceStatus = "N";
        //    }

        //    SQLString = "Update DeviceMaster Set SerialNo='" + StringSerialNo + "',MobileNo='" + StringMobileNo + "',BatchNo='" + StringBatchNo + "',IMEINo='" + StringIMEINo + "',DeviceStatus='" + StringDeviceStatus + "' where DID=" + DID + " ;Update CurrentVehicleString Set MobileNumber='" + StringMobileNo + "' where DeviceID='" + textBoxDeviceID.Text.Trim() + "'";
        //    if (Utils.ExecNonQuery(SQLString, CnnString) <= 0)
        //    {
        //        lblErrMess.Text = "Device Not Successfully Updated.";
        //    }
        //    else
        //    {
        //        lblSucMess.Text = "Device is Successfully Updated.";
        //    }


        //}

        protected void buttonSave_Click(object sender, EventArgs e)
        {

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            string strGetData = "Select Top 1 hsrp_stateid,rtolocationid,[VehicleType],[OrderStatus] from hsrprecords"+
                " where [VehicleRegNo]='" + TextBoxVehicleRegNo.Text + "' and orderstatus in('New Order','Embossing Done') order by [HSRPRecord_CreationDate] desc";
           DataTable dtData=Utils.GetDataTable(strGetData, CnnString);

           if (dtData.Rows.Count > 0)
           {
               string strGetRTOLocation = "Select rtolocationname from rtolocation where rtolocationid='" + dtData.Rows[0]["rtolocationid"].ToString() + "'";
               string strGetLoc = Utils.getScalarValue(strGetRTOLocation, CnnString);

               ViewState["Name"] = textBoxCustomerName.Text;
               ViewState["Address"] = textBoxAddress1.Text;
               string str = "Please Enter VehicleRegNo.";
               lblLocation.Text = "<b>Location: </b>" + strGetLoc;
               lblVehType.Text = "<b>Vehicle Type:</b>" + dtData.Rows[0]["VehicleType"].ToString();
               lblVehId.Text = "<b>Vehicle No:</b>" + TextBoxVehicleRegNo.Text;
               lblStatus.Text = "<b>Order Status:</b>" + dtData.Rows[0]["OrderStatus"].ToString();
               mpeSave.Show();
              
           }
           else if (dtData.Rows.Count.Equals(0))
           {
               SaveData();
           }

            
            //String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            //String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
           
          //  SaveData();
        }

        private void SaveData()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));

            string strAuthDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
            
            
           // DateTime dateAuth = Convert.ToDateTime(strAuthDate);

            DateTime dateAuth =DateTime.ParseExact(strAuthDate,"dd/MM/yyyy",CultureInfo.InvariantCulture);
            String[] StringCashReceiptDate =CashDate.Text.Split('-');
           // String From2 = StringCashReceiptDate[0] + "/" + StringCashReceiptDate[1] + "/" + StringCashReceiptDate[2].Split(' ')[0];
           // CashReceiptDate = new DateTime(Convert.ToInt32(StringCashReceiptDate[2].Split(' ')[0]), Convert.ToInt32(StringCashReceiptDate[0]), Convert.ToInt32(StringCashReceiptDate[1]));
            String From2 = CashDate.Text.ToString();
            CashReceiptDate = DateTime.ParseExact(From2, "dd/MM/yyyy", CultureInfo.InvariantCulture); ;

          //  CashReceiptDate = new DateTime(Convert.ToInt32(CashDate.Text));
           //  CashReceiptDate = DateTime.ParseExact(CashDate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            

            //>> Authorization Info
            // return;


            String StringCashReceiptDate1 = CashReceiptDate.ToString();
            if (StringCashReceiptDate1.Contains("0001"))
            {
                CashReceiptDate = Convert.ToDateTime("1/1/1900 12:00:00 AM");
            }

            if (CashReceiptDate.ToString()=="01/01/01 12:00:00 AM")
            {
                //CashReceiptDate = Convert.ToDateTime("1/1/1900 12:00:00 AM");
                CashReceiptDate = System.DateTime.Now;
            }

            StringCustomerName = textBoxCustomerName.Text.Trim().Replace("'", "''").ToString();
            StringOrderStatus = textBoxOrderStatus.Text.Trim().Replace("'", "''").ToString();
            StringOrderStatus = "New Order";
            StringAddress1 = textBoxAddress1.Text.Trim().Replace("'", "''").ToString();
            StringAddress2 = textBoxAddress2.Text.Trim().Replace("'", "''").ToString();
            StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
            StringLandline = textBoxLandline.Text.Trim().Replace("'", "''").ToString();
            StringEmailId = textBoxEmailId.Text.Trim().Replace("'", "''").ToString();
            //>>> Vehicle Info
            StringVehicleClass = DropDownListVehicleClass.SelectedItem.Value;
            if (StringVehicleClass.Equals("--Select Vehicle Class--"))
            {
                lblErrMess.Text = "please select Correct vehicle Class.";
                UpdatePanelMessage.Update();
                return;
            }
            StringManufacturerName = DropDownListVehicleMaker1.SelectedItem.Value;

            if (StringManufacturerName.Equals("--Select Vehicle Maker--"))
            {
                StringManufacturerModel = "";
                StringManufacturerName = "";
            }
            else
            {
                StringManufacturerModel = DropDownListModel.SelectedItem.Value;
            }

            if (DropDownListOrderType.SelectedItem.Value.Equals("OS"))
            {

            }
            else if (DropDownListOrderType.SelectedItem.Value.Equals("DF"))
            {
                if (DropDownListFrontPlateSize.SelectedItem.Value.Equals("--Select Front Plate--"))
                {
                    lblErrMess.Text = "Please select correct front plate.";
                    UpdatePanelDropDownListFrontPlate.Update();
                    return;
                }
            }
            else if (DropDownListOrderType.SelectedItem.Value.Equals("DR"))
            {
                if (DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
                {
                    lblErrMess.Text = "Please select correct rear plate.";
                    UpdatePanelDropDownListFrontPlate.Update();
                    return;
                }
            }
            else
            {
                if (DropDownListFrontPlateSize.SelectedItem.Value.Equals("--Select Front Plate--"))
                {
                    lblErrMess.Text = "Please select correct front plate.";
                    UpdatePanelDropDownListFrontPlate.Update();
                    return;
                }
                if (DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
                {
                    lblErrMess.Text = "Please select correct rear plate.";
                    UpdatePanelDropDownListFrontPlate.Update();
                    return;
                }
            }
            StringVehicleModel = DropDownListVehicleModel.SelectedItem.Value;

            StringVehicleRegNo = TextBoxVehicleRegNo.Text.Trim().Replace("'", "''").ToString();
            StringEngineNo = textboxEngineNo.Text.Trim().Replace("'", "''").ToString();
            StringChassisNo = textBoxChassisNo.Text.Trim().Replace("'", "''").ToString();
            //>>> Number Plate Info 

            StringOrderType = DropDownListOrderType.SelectedItem.Value;

            if (StringOrderType.Equals("--Select Order Type--"))
            {
                lblErrMess.Text = "please select Correct Order Type.";
                UpdatePanelMessage.Update();
                return;
            }

            if (String.IsNullOrEmpty(labelFrontPlateSize.Text))
            {
                IScheckBoxFrontPlate = "N";
            }
            else
            {
                IScheckBoxFrontPlate = "Y";
            }

            if (String.IsNullOrEmpty(labelRearPlateSize.Text))
            {
                IScheckBoxRearPlate = "N";
            }
            else
            {
                IScheckBoxRearPlate = "Y";
            }

            if (checkBoxThirdSticker.Checked)
            {
                ISThirdStickerExists = "Y";
            }
            else
            {
                ISThirdStickerExists = "N";
            }

            


            int ISExists = -1;
            BAL obj = new BAL();
            if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, TextBoxVehicleRegNo.Text.Trim(), HSRPStateID, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    //buttonSave.Enabled = false;
                    //buttonSave.Visible = false;
                    lblErrMess.Text = "Duplicate Record ";
                    return;
                }
            }

            if (StringMode.Equals("New"))
            {

                SQLString = "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as InvoiceNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID=" + dropDownListRTOLocation.SelectedValue + "  and PrefixFor = 'Invoice No';" +
                    "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as CashReceipt  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID=" + dropDownListRTOLocation.SelectedValue + " and PrefixFor = 'Cash Receipt No';" +
                    "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as OrderNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID=" + dropDownListRTOLocation.SelectedValue + "  and PrefixFor = 'Order No';" +
                    "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as DeliveryChalanNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID=" + dropDownListRTOLocation.SelectedValue + " and PrefixFor = 'Delivery Challan No'";

                DataSet ds = Utils.getDataSet(SQLString, CnnString);
                StringInvoiceNo = ds.Tables[0].Rows[0][0].ToString();
                StringCashReceiptNo = ds.Tables[1].Rows[0][0].ToString();
                StringOrderNo = ds.Tables[2].Rows[0][0].ToString();
                StringDeliverChallanNo = ds.Tables[3].Rows[0][0].ToString();

                //SQLString = "Update Prefix Set LastNo=LastNo+1 where HSRP_StateID =" + HSRPStateID + " and RTOLocationID =" + dropDownListRTOLocation.SelectedValue + "";
                //Utils.ExecNonQuery(SQLString, CnnString);
            }
            else if (StringMode.Equals("Edit"))
            {
                // StringInvoiceNo = textBoxInvoiceNo.Text.Trim().Replace("'", "''").ToString();
                // StringOrderNo = textBoxOrderNo.Text.Trim().Replace("'", "''").ToString();
                // StringCashReceiptNo = textBoxCashReceiptNo.Text.Trim().Replace("'", "''").ToString();
                // StringDeliverChallanNo = textBoxDelieveryChallan.Text.Trim().Replace("'", "''").ToString();

            }

            StringCashReceiptNo_HHT = TextBoxCashReceiptNo.Text.Trim().Replace("'", "''").ToString();
            AuthorizationNo = textBoxAuthorizationNo.Text.Trim().Replace("'", "''").ToString();
            StringTotalAmount = textBoxTotalAmount.Text.Trim().Replace("'", "''").ToString();
            StringVATPercentage = textBoxVat.Text.Trim().Replace("'", "''").ToString();
            StringVATAmount = textBoxVatAmount.Text.Trim().Replace("'", "''").ToString();
            StringServiceTaxPercentage = textBoxServiceTax.Text.Trim().Replace("'", "''").ToString();
            StringServiceTaxAmount = textboxServiceTaxAmount.Text.Trim().Replace("'", "''").ToString();
            StringNetTotal = textBoxNetTotal.Text.Trim().Replace("'", "''").ToString();

            StringFixingCharge = "0";//HiddenFieldFixing.Text.Trim().Replace("'", "''").ToString();
            StringFrontPlatePrize = HiddenFieldFrontPlate.Text.Trim().Replace("'", "''").ToString();
            StringRearPlatePrize = HiddenFieldRearPlate.Text.Trim().Replace("'", "''").ToString();
            StringStickerPrize = HiddenFieldSticker.Text.Trim().Replace("'", "''").ToString();
            StringScrewPrize = HiddenFieldScrew.Text.Trim().Replace("'", "''").ToString();

            StringFrontPlateSize = HiddenFieldFrontPlateCode.Text;
            StringRearPlateSize = HiddenFieldRearPlateCode.Text;

            StringFrontPlateSize = DropDownListFrontPlateSize.SelectedValue;
            StringRearPlateSize = DropDownListRearPlateSize.SelectedValue;

            lblErrMess.Text = String.Empty;
            lblSucMess.Text = String.Empty;

            obj = new BAL();

            try
            {

                //string aa = TextBoxVehicleRegNo.ToString().Substring(0, 5);
                //string aa2 = TextBoxVehicleRegNo.Text.Substring(0, 4);

                if (TextBoxVehicleRegNo.Text.Length > 5)
                {
                    if (TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09HG" || TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09GG" || TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09LP" || TextBoxVehicleRegNo.Text.Substring(0, 5) == "MP09R" || TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09TA" || TextBoxVehicleRegNo.Text.Substring(0, 6) == "MP09FA")
                    {
                        if (TextBoxVehicleRegNo.Text == "MP09GG0414" || TextBoxVehicleRegNo.Text == "MP09GG0441" || TextBoxVehicleRegNo.Text == "MP09GG0423" || TextBoxVehicleRegNo.Text == "mp09gg1011" || TextBoxVehicleRegNo.Text == "mp09hg9565" || TextBoxVehicleRegNo.Text == "mp09hg9825" || TextBoxVehicleRegNo.Text == "mp09hg9631" || TextBoxVehicleRegNo.Text == "mp09hg9731" || TextBoxVehicleRegNo.Text == "mp09hg9531" || TextBoxVehicleRegNo.Text == "mp09gg0011" || TextBoxVehicleRegNo.Text == "mp09gg0786" || TextBoxVehicleRegNo.Text == "mp09hg9473" || TextBoxVehicleRegNo.Text == "mp09hg9573" || TextBoxVehicleRegNo.Text == "mp09gf1234")
                            lblErrMess.Text = "";
                        else
                            lblErrMess.Text = "";
                        return;
                    }
                }

                if (StringManufacturerModel == "--Select Vehicle Model--")
                {
                    //StringManufacturerModel = "0";
                    StringManufacturerModel = "";
                }
                else
                {
                }
                if (StringManufacturerName == "--Select Vehicle Maker--")
                {
                    //StringManufacturerName = "0";
                    StringManufacturerName = "";
                }

                int Isexists = -1;
                string HSRPRecordID = "";
                //SqlConnection con=new SqlConnection(CnnString);
                //con.Open();
                //SqlCommand cmd = new SqlCommand("Transaction_HSRPRecordInsertXX_New2",con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@MacbaseAddress", MacAddress);
                //cmd.Parameters.AddWithValue("@HSRPRecord_AuthorizationNo", AuthorizationNo);
                //cmd.Parameters.AddWithValue("@HSRPRecord_AuthorizationDate", AuthorizationDate);
                //cmd.Parameters.AddWithValue("@OrderNo", StringOrderNo);
                //cmd.Parameters.AddWithValue("@OwnerName", StringCustomerName);
                //cmd.Parameters.AddWithValue("@OrderStatus", StringOrderStatus);
                //cmd.Parameters.AddWithValue("@Address1", StringAddress1);
                //cmd.Parameters.AddWithValue("@Address2", StringAddress2);
                //cmd.Parameters.AddWithValue("@MobileNo", StringMobileNo);
                //cmd.Parameters.AddWithValue("@LandlineNo", StringLandline);
                //cmd.Parameters.AddWithValue("@EmailID", StringEmailId);
                //cmd.Parameters.AddWithValue("@VehicleClass", StringVehicleClass);
                //cmd.Parameters.AddWithValue("@ManufacturerName", StringManufacturerName);
                //cmd.Parameters.AddWithValue("@ManufacturerModel", StringManufacturerModel);
                //cmd.Parameters.AddWithValue("@VehicleType", StringVehicleModel);
                //cmd.Parameters.AddWithValue("@VehicleRegNo", StringVehicleRegNo);
                //cmd.Parameters.AddWithValue("@EngineNo", StringEngineNo);
                //cmd.Parameters.AddWithValue("@ChassisNo", StringChassisNo);
                //cmd.Parameters.AddWithValue("@OrderType", StringOrderType);
                //cmd.Parameters.AddWithValue("@ISFrontPlateSize", IScheckBoxFrontPlate);
                //cmd.Parameters.AddWithValue("@FrontPlateSize", StringFrontPlateSize);
                //cmd.Parameters.AddWithValue("@ISRearPlateSize", IScheckBoxRearPlate);
                //cmd.Parameters.AddWithValue("@RearPlateSize", StringRearPlateSize);
                //cmd.Parameters.AddWithValue("@StickerMandatory", ISThirdStickerExists);
                //cmd.Parameters.AddWithValue("@InvoiceNo", StringInvoiceNo);
                //cmd.Parameters.AddWithValue("@CashReceiptNo", StringCashReceiptNo);
                //cmd.Parameters.AddWithValue("@CashReceiptNo_HHT", StringCashReceiptNo_HHT);
                //cmd.Parameters.AddWithValue("@CashReceiptDatetime_HHT", CashReceiptDate.ToString());
                //cmd.Parameters.AddWithValue("@VAT_Percentage", StringVATPercentage);
                //cmd.Parameters.AddWithValue("@VAT_Amount", StringVATAmount);
                //cmd.Parameters.AddWithValue("@ServiceTax_Percentage", StringServiceTaxPercentage);
                //cmd.Parameters.AddWithValue("@ServiceTax_Amount", StringServiceTaxAmount);
                //cmd.Parameters.AddWithValue("@TotalAmount", StringTotalAmount);
                //cmd.Parameters.AddWithValue("@NetAmount", StringNetTotal);
                //cmd.Parameters.AddWithValue("@HSRP_StateID", HSRPStateID);
                //cmd.Parameters.AddWithValue("@RTOLocationID", dropDownListRTOLocation.SelectedValue);
                //cmd.Parameters.AddWithValue("@DeliveryChallan", StringDeliverChallanNo);
                //cmd.Parameters.AddWithValue("@FixingCharge", StringFixingCharge);
                //cmd.Parameters.AddWithValue("@FrontPlatePrize", StringFrontPlatePrize);
                //cmd.Parameters.AddWithValue("@RearPlatePrize", StringRearPlatePrize);
                //cmd.Parameters.AddWithValue("@StickerPrize", StringStickerPrize);
                //cmd.Parameters.AddWithValue("@ScrewPrize", StringScrewPrize);
                //cmd.Parameters.AddWithValue("@Remarks", textBoxRemarks.Text);
                //cmd.Parameters.AddWithValue("@UID", UID);
                //cmd.Parameters.AddWithValue("@RTOLocationIDd", dropDownListRTOLocation.SelectedValue);

                //cmd.Parameters.Add("@IsExists", SqlDbType.Int,1);
                //cmd.Parameters["@IsExists"].Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("@HSRPRecordID", SqlDbType.VarChar, 100);
                //cmd.Parameters["@HSRPRecordID"].Direction = ParameterDirection.Output;
                //cmd.ExecuteNonQuery();
                //int Isexists =Convert.ToInt16(cmd.Parameters["@IsExists"].Value);
                //string HSRPRecordID = cmd.Parameters["@HSRPRecordID"].Value.ToString();
                //con.Close();

                if (obj.InsertHSRPRecords(MacAddress, AuthorizationNo, dateAuth, StringOrderNo,
                    StringCustomerName, StringOrderStatus, StringAddress1, StringAddress2, StringMobileNo, StringLandline,
                   StringEmailId, StringVehicleClass, StringManufacturerName, StringManufacturerModel, StringVehicleModel, StringVehicleRegNo, StringEngineNo,
                   StringChassisNo, StringOrderType, IScheckBoxFrontPlate, StringFrontPlateSize, IScheckBoxRearPlate,
                   StringRearPlateSize, ISThirdStickerExists, StringInvoiceNo, StringCashReceiptNo, StringCashReceiptNo_HHT, CashReceiptDate.ToString(), StringVATPercentage,
                   StringVATAmount, StringServiceTaxPercentage, StringServiceTaxAmount, StringTotalAmount, StringNetTotal, HSRPStateID,
                   dropDownListRTOLocation.SelectedValue, StringDeliverChallanNo, StringFixingCharge, StringFrontPlatePrize, StringRearPlatePrize,
                   StringStickerPrize, StringScrewPrize, textBoxRemarks.Text, UID, dropDownListRTOLocation.SelectedValue, ref Isexists, ref HSRPRecordID))
                {
                    temp++;
                    if (Isexists.Equals(1))
                    {
                        lblErrMess.Text = "Duplicate Record.";
                        return;
                        //BlankFields();
                    }
                    else
                    {

                        SQLString = "Update HSRPRecordsStaggingArea Set OrderStatus='Processed' Where HSRPRecord_AuthorizationNo='" + AuthorizationNo + "'";
                        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                        Utils.ExecNonQuery(SQLString, CnnString);
                        temp++;
                        //SQLString = "Update collection_details set IsProcessed='Y' where regno='" + StringVehicleRegNo + "'";
                        //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringCashReceipt"].ToString();
                        //Utils.ExecNonQuery(SQLString, CnnString);
                        //temp++;

                        lblSucMess.Text = "Record Added Sucessfully.";
                        BlankFieldsSave();
                        temp++;
                        textBoxAuthorizationNo.Focus();
                        //PrintCashReceipt(HSRPRecordID);
                        ViewState["HSRPRecordID"] = HSRPRecordID;
                        temp++;
                        LknBtnCashReceipts.Visible = true;
                        LinkButton1Epson.Visible = true;
                        //LinkButtonCashReceiptss.HRef = "http://180.151.100.242/Data/" + HiddenFieldCashReceipt.Text;
                        temp++;

                        //For new order
                        string Query = "select max(neworder)+1 as neworder from rtolocation where rtolocationid='" + RTOLocationID + "'";
                        DataTable dt1 = new DataTable();
                        dt1 = Utils.GetDataTable(Query, CnnString);
                        Query = "update rtolocation set neworder='" + dt1.Rows[0]["neworder"].ToString() + "',lastupdateorder=getdate() where rtolocationid='" + RTOLocationID + "'";
                        int i = Utils.ExecNonQuery(Query, CnnString);

                    }
                }
            }
            //else
            //{
            //    lblErrMess.Text = "Record not Added.";

                //}


            catch (Exception ex)
            {
                //Response.Write(ex.Message.ToString());
                //Response.Write(ex.Message.ToString()+"ssssssssss-:"+temp.ToString());
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine);
                sb.Append("--**==================================================****");
                sb.Append("--**" + DateTime.Now.ToLongDateString().ToString() + "****" + Environment.NewLine);
                sb.Append(ex.Message.ToString() + Environment.NewLine);
                sb.Append(ex.StackTrace + Environment.NewLine + Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("--**==================================================****");
                sb.Append(Environment.NewLine);

                WriteToFile("e:\\log\\getdatalog.log", sb.ToString());

                string login = "~/Login.aspx?X=" + Session["MacAddress"].ToString();
                Session.Abandon();
                Session.Clear();
                Response.CacheControl = "no-cache";
                Response.Redirect(login);
            }
        }


        private void PrintCashReceipt(string HSRPRecordID)
        {
            //  String HSRPRecordID = e.Item["HSRPRecordID"].ToString();
            //  string OrderStatus = e.Item["OrderStatus"].ToString();
            temp = 20;
            DataTable GetAddress;
            string Address;
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            GetAddress = Utils.GetDataTable("select * from HSRPState WHERE HSRP_StateID='" + HSRPStateID + "'", CnnString);
            temp++;

            if (GetAddress.Rows[0]["pincode"].ToString() != "" || GetAddress.Rows[0]["pincode"].ToString() != null)
            {
                Address = " - " + GetAddress.Rows[0]["pincode"];
            }
            else
            {
                Address = "";
            }
            temp++;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            temp++;
            // string Invoice = Grid1.Items[e].DataItem.ToString();
            // string _tempvalue = ((LinkButton)Grid1.Rows[row.RowIndex].FindControl("LinkButtonStatus")).Text;
            if (String.IsNullOrEmpty(HSRPRecordID))
            {
                lblErrMess.Text = "that is not valid Record.";
                return;
            }


            //if (e.Control.ID.ToString() == "LinkButtonCashReceipts")
            //{
            DataTable dataSetFillHSRPDeliveryChallan = new DataTable();
            BAL obj = new BAL();
            if (obj.FillHSRPRecordDeliveryChallan(HSRPRecordID, ref dataSetFillHSRPDeliveryChallan))
            {
                temp++;
                if (dataSetFillHSRPDeliveryChallan.Rows.Count < 1)
                {
                    lblErrMess.Text = "No Such Record Exists.";
                    return;
                }
                else
                {
                    lblErrMess.Text = "";
                }


                //string filename = "CASHRECEIPT_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                //string filename = "CASHRECEIPT_No-" + dataSetFillHSRPDeliveryChallan.Rows[0]["CashReceiptNo"].ToString().Replace("/", "-") + ".pdf";

                string filename = "CASHRECEIPT_No-" + HSRPRecordID + ".pdf";

                HiddenFieldCashReceipt.Text = filename;
                string SQLString = String.Empty;
                String StringField = String.Empty;
                String StringAlert = String.Empty;

                StringBuilder bb = new StringBuilder();

                //Creates an instance of the iTextSharp.text.Document-object:
                Document document = new Document();

                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                temp++;
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                temp++;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();
                temp++;
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




                PdfPCell cell6 = new PdfPCell(new Phrase("ORDER BOOKING NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell6.Colspan = 1;
                cell6.BorderWidthLeft = 0f;
                cell6.BorderWidthRight = 0f;
                cell6.BorderWidthTop = 0f;
                cell6.BorderWidthBottom = 0f;
                cell6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell6);

                PdfPCell cell65 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["OrderNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65.Colspan = 1;
                cell65.BorderWidthLeft = 0f;
                cell65.BorderWidthRight = 0f;
                cell65.BorderWidthTop = 0f;
                cell65.BorderWidthBottom = 0f;
                cell65.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65);


                if (Session["UserHSRPStateID"].ToString() == "4")
                {
                    try
                    {
                        PdfPCell cell26 = new PdfPCell(new Phrase("Affixation Date", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        //PdfPCell cell26 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        cell26.Colspan = 1;
                        cell26.BorderWidthLeft = 0f;
                        cell26.BorderWidthRight = 0f;
                        cell26.BorderWidthTop = 0f;
                        cell26.BorderWidthBottom = 0f;
                        cell26.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell26);

                        //DateTime PlateAffixationDate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["PlateAffixationDate"]);
                        DateTime PlateAffixationDateother = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderDate"]);
                        DateTime PlateAffixationDate = PlateAffixationDateother.AddDays(4);
                        string[] dat = PlateAffixationDate.ToString().Split(' ');
                        string d = dat[0].ToString();
                        if (d != "1/1/1900")
                        {

                            string PlateAffixationDateNew = Convert.ToString(dat[0]);
                            int z = 1;

                            DateTime updatedate = Convert.ToDateTime(d);
                            for (int i = 1; i <= z; i++)
                            {
                                string[] dates = updatedate.ToString().Split(' ');
                                string des = dates[0].ToString();

                                string SQLData = "select count(*) as blockDate from HolidayDateTime where blockDate between '" + des + " 00:00:00' and '" + des + " 23:59:59'";
                                int dtdate = Utils.getScalarCount(SQLData, CnnString);
                                if (dtdate > 0)
                                {

                                    //PlateAffixationDate.AddDays(i).ToString();
                                    updatedate = Convert.ToDateTime(des).AddDays(i);
                                    z = z + 1;
                                    PdfPCell cell265 = new PdfPCell(new Phrase(": " + updatedate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                    cell265.Colspan = 1;
                                    cell265.BorderWidthLeft = 0f;
                                    cell265.BorderWidthRight = 0f;
                                    cell265.BorderWidthTop = 0f;
                                    cell265.BorderWidthBottom = 0f;
                                    cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    table.AddCell(cell265);
                                }
                                else
                                {
                                    PdfPCell cell265 = new PdfPCell(new Phrase(": " + updatedate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                                    cell265.Colspan = 1;
                                    cell265.BorderWidthLeft = 0f;
                                    cell265.BorderWidthRight = 0f;
                                    cell265.BorderWidthTop = 0f;
                                    cell265.BorderWidthBottom = 0f;
                                    cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                                    table.AddCell(cell265);
                                }
                            }
                        }
                        else
                        {
                            PdfPCell cell265 = new PdfPCell(new Phrase(" ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                            cell265.Colspan = 1;
                            cell265.BorderWidthLeft = 0f;
                            cell265.BorderWidthRight = 0f;
                            cell265.BorderWidthTop = 0f;
                            cell265.BorderWidthBottom = 0f;
                            cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell265);
                        }

                        // PdfPCell cell265 = new PdfPCell(new Phrase(": " + PlateAffixationDate.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        //PdfPCell cell265 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                        //cell265.Colspan = 1;
                        //cell265.BorderWidthLeft = 0f;
                        //cell265.BorderWidthRight = 0f;
                        //cell265.BorderWidthTop = 0f;
                        //cell265.BorderWidthBottom = 0f;
                        //cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        //table.AddCell(cell265);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    PdfPCell cell26 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell26.Colspan = 1;
                    cell26.BorderWidthLeft = 0f;
                    cell26.BorderWidthRight = 0f;
                    cell26.BorderWidthTop = 0f;
                    cell26.BorderWidthBottom = 0f;
                    cell26.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell26);

                    DateTime Orddate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["OrderDate"].ToString());


                    PdfPCell cell265 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell265.Colspan = 1;
                    cell265.BorderWidthLeft = 0f;
                    cell265.BorderWidthRight = 0f;
                    cell265.BorderWidthTop = 0f;
                    cell265.BorderWidthBottom = 0f;
                    cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell265);
                }

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



                PdfPCell cell8 = new PdfPCell(new Phrase("OWNER ADDRESS", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell8.Colspan = 1;
                cell8.BorderWidthLeft = 0f;
                cell8.BorderWidthRight = 0f;
                cell8.BorderWidthTop = 0f;
                cell8.BorderWidthBottom = 0f;
                cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell8);

                PdfPCell cell85 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["Address1"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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



                PdfPCell cell11 = new PdfPCell(new Phrase("ENGINE NO.", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell11.Colspan = 1;
                cell11.BorderWidthLeft = 0f;
                cell11.BorderWidthRight = 0f;
                cell11.BorderWidthTop = 0f;
                cell11.BorderWidthBottom = 0f;
                cell11.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell11);

                PdfPCell cell115 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell115.Colspan = 1;
                cell115.BorderWidthLeft = 0f;
                cell115.BorderWidthRight = 0f;
                cell115.BorderWidthTop = 0f;
                cell115.BorderWidthBottom = 0f;
                cell115.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell115);



                PdfPCell cell1113 = new PdfPCell(new Phrase("CHASSIS NO", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell1113.Colspan = 1;
                cell1113.BorderWidthLeft = 0f;
                cell1113.BorderWidthRight = 0f;
                cell1113.BorderWidthTop = 0f;
                cell1113.BorderWidthBottom = 0f;
                cell1113.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1113);

                PdfPCell cell11135 = new PdfPCell(new Phrase(": " + dataSetFillHSRPDeliveryChallan.Rows[0]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
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
                //PdfPCell cell66 = new PdfPCell(new Phrase("RECEIVED WITH THANKS RUPEES THREE HUNDRED THIRTY FORU ONLY FROM MR KAMODH SINGH ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                //cell66.Colspan = 4;

                //cell66.BorderWidthLeft = 0f;
                //cell66.BorderWidthRight = 0f;
                //cell66.BorderWidthTop = 0f;
                //cell66.BorderWidthBottom = 0f;
                //cell66.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell66);

                string Message = "\u2022" + " Vehicle Owner is required to visit along with vehicle for the affixation of HSRP , on the Sixth working day from the date of  issuance of cash receipt.";

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

                PdfPCell cell65ab = new PdfPCell(new Phrase("Please ensure to collect 3rd sticker", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell65ab.Colspan = 4;
                cell65ab.BorderWidthLeft = 0f;
                cell65ab.BorderWidthRight = 0f;
                cell65ab.BorderWidthTop = 0f;
                cell65ab.BorderWidthBottom = 0f;
                cell65ab.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65ab);



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

                //HttpContext context = HttpContext.Current;
                //context.Response.WriteFile(PdfFolder);
                //context.Response.ContentType = "Application/pdf";
                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);

                //context.Response.Flush();

                // System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                //  response.ClearContent();
                //  response.Clear();
                //response.ContentType = "text/plain";
                //response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ";");
                //response.TransmitFile(PdfFolder);
                // response.Flush();
                // response.End();


            }

        }

        #region DropDown

        /// <summary>
        /// for Commercial Front Plate Size
        /// </summary>

        private void BindFrontPlateSizeDropDownListTransport()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownListFrontPlateSize, "Select ProductCode,ProductID From Product Where HSRP_StateID=" + HSRPStateID + " and ProductColor='YELLOW' order by ProductCode desc", CnnString, "--Select Front Plate--");
            DropDownListFrontPlateSize.SelectedIndex = 0;// DropDownListFrontPlateSize.Items.Count - 1;
            UpdatePanelDropDownListFrontPlate.Update();
        }

        private void BindRearPlateSizeDropDownListTransport()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownListRearPlateSize, "Select ProductCode,ProductID From Product Where HSRP_StateID=" + HSRPStateID + " and ProductColor='YELLOW' order by ProductCode desc", CnnString, "--Select Rear Plate--");
            DropDownListRearPlateSize.SelectedIndex = 0;// DropDownListFrontPlateSize.Items.Count - 1;
            UpdatePanelDropDownListRearPlate.Update();
        }

        /// <summary>
        ///  for Rear Plate Size Non Commercial
        /// </summary>
        private void BindFrontPlateSizeDropDownListNonTransport()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select ProductCode,ProductID From Product Where HSRP_StateID=" + HSRPStateID + " and ProductColor='WHITE' order by ProductCode desc";
            Utils.PopulateDropDownList(DropDownListFrontPlateSize, SQLString, CnnString, "--Select Front Plate--");
            DropDownListFrontPlateSize.SelectedIndex = 0;// DropDownListFrontPlateSize.Items.Count - 1;
            UpdatePanelDropDownListFrontPlate.Update();
        }

        private void BindRearPlateSizeDropDownListNonTransport()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownListRearPlateSize, "Select ProductCode,ProductID From Product Where HSRP_StateID=" + HSRPStateID + " and ProductColor='WHITE' order by ProductCode desc", CnnString, "--Select Rear Plate--");
            DropDownListRearPlateSize.SelectedIndex = 0;// DropDownListFrontPlateSize.Items.Count - 1;
            UpdatePanelDropDownListRearPlate.Update();
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
                lblErrMess.Text = "Please Select Correct Vehicle Maker.";
                UpdatePanelMessage.Update();
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
                lblErrMess.Text = "Please Select Correct Vehicle Maker.";
                UpdatePanelMessage.Update();
                DropDownListVehicleMaker1.Focus();
                return;

            }
            else
            {
                FillVehicleModel();
            }
        }

        protected void DropDownList1_SelectedIndexChanged1(object sender, EventArgs e)
        {
            OrderSelectionChange();
        }

        private void OrderSelectionChange()
        {


            if (DropDownListVehicleClass.SelectedItem.Value.Equals("Transport"))
            {
                BindFrontPlateSizeDropDownListTransport();
                UpdatePanelDropDownListFrontPlate.Update();
                BindRearPlateSizeDropDownListTransport();
                UpdatePanelDropDownListRearPlate.Update();
            }

            else if (DropDownListVehicleClass.SelectedItem.Value.Equals("Non-Transport"))
            {
                BindFrontPlateSizeDropDownListNonTransport();
                UpdatePanelDropDownListFrontPlate.Update();
                BindRearPlateSizeDropDownListNonTransport();
                UpdatePanelDropDownListRearPlate.Update();
            }


            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            if (DropDownListVehicleModel.SelectedValue.Equals("THREE WHEELER") || DropDownListVehicleModel.SelectedValue.Equals("LMV") || DropDownListVehicleModel.SelectedValue.Equals("LMV(CLASS)") || DropDownListVehicleModel.SelectedValue.Equals("MCV/HCV/TRAILERS"))
            {
                checkBoxThirdSticker.Checked = true;
            }
            else
            {
                checkBoxThirdSticker.Checked = false;
            }
            if (DropDownListOrderType.SelectedValue.Equals("OS"))
            {
                checkBoxThirdSticker.Checked = true;
                checkBoxSnapLock.Checked = false;
            }
            else
            {
                checkBoxSnapLock.Checked = true;
            }
            textBoxTotalAmount.Text = string.Empty;
            textBoxVat.Text = string.Empty;
            textBoxVatAmount.Text = string.Empty;
            textBoxServiceTax.Text = string.Empty;
            textboxServiceTaxAmount.Text = string.Empty;
            textBoxNetTotal.Text = string.Empty;

            if (DropDownListOrderType.SelectedValue.Equals("--Select Order Type--"))
            {
                lblErrMess.Text = "Please select correct order type.";
                checkBoxThirdSticker.Checked = false;
                checkBoxSnapLock.Checked = false;
                return;
            }

            Double Cost = 0.00;
            Double VatPercentage = 0.00;
            Double ServiceTaxPercentage = 0.00;
            Double SnapLock = 0.00;
            Double Sticker = 0.00;
            Double Discount = 0.00;
            Double NetTotal = 0.00;

            SQLString = "Select FrontPlateID,RearPlateID,StickerID,OrderType From RegistrationPlateDetail Where HSRP_StateID=" + HSRPStateID + " and VehicleClass='" + DropDownListVehicleClass.SelectedValue + "' and VehicleType='" + DropDownListVehicleModel.SelectedValue + "' and OrderType='" + DropDownListOrderType.SelectedValue + "'";
            string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);

            if (!String.IsNullOrEmpty(dt.Rows[0]["StickerID"].ToString()) || !String.IsNullOrWhiteSpace(dt.Rows[0]["StickerID"].ToString()))
            {
                checkBoxThirdSticker.Checked = true;
                SQLString = " Select Cost from ProductCost Where HSRP_StateID=" + HSRPStateID + " and ProductID=" + dt.Rows[0]["StickerID"].ToString() + "";
                Cost = Double.Parse((Utils.getDataSingleValue(SQLString, CnnString, "Cost")));
                HiddenFieldSticker.Text = Cost.ToString();
            }

            if (!String.IsNullOrEmpty(dt.Rows[0]["FrontPlateID"].ToString()) || !String.IsNullOrWhiteSpace(dt.Rows[0]["FrontPlateID"].ToString()))
            {
                SQLString = " Select ProductCode from Product Where ProductID=" + dt.Rows[0]["FrontPlateID"].ToString() + "";
                HiddenFieldFrontPlateCode.Text = dt.Rows[0]["FrontPlateID"].ToString();
                DropDownListFrontPlateSize.SelectedValue = dt.Rows[0]["FrontPlateID"].ToString();
                // DropDownListFrontPlateSize.Visible = true;
                //DropDownListRearPlateSize.Visible = false;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                labelFrontPlateSize.Text = "Front Plate :- " + Utils.getScalarValue(SQLString, CnnString);
                UpdatePanelDropDownListFrontPlate.Update();

                SQLString = " Select Cost from ProductCost Where HSRP_StateID=" + HSRPStateID + " and ProductID=" + dt.Rows[0]["FrontPlateID"].ToString() + "";
                HiddenFieldFrontPlate.Text = Utils.getDataSingleValue(SQLString, CnnString, "Cost");
                Cost += Double.Parse(HiddenFieldFrontPlate.Text);
            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["RearPlateID"].ToString()) || !String.IsNullOrWhiteSpace(dt.Rows[0]["RearPlateID"].ToString()))
            {
                SQLString = " Select ProductCode from Product Where ProductID=" + dt.Rows[0]["RearPlateID"].ToString() + "";
                HiddenFieldRearPlateCode.Text = dt.Rows[0]["RearPlateID"].ToString();
                DropDownListRearPlateSize.SelectedValue = dt.Rows[0]["RearPlateID"].ToString();
                // DropDownListRearPlateSize.Visible = true;
                // DropDownListFrontPlateSize.Visible = false;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                labelRearPlateSize.Text = "Rear Plate :- " + Utils.getScalarValue(SQLString, CnnString);
                UpdatePanelDropDownListRearPlate.Update();

                SQLString = " Select Cost from ProductCost Where HSRP_StateID=" + HSRPStateID + " and ProductID=" + dt.Rows[0]["RearPlateID"].ToString() + "";
                HiddenFieldRearPlate.Text = Utils.getDataSingleValue(SQLString, CnnString, "Cost");
                Cost += Double.Parse(HiddenFieldRearPlate.Text);
            }

            if (dt.Rows[0]["OrderType"].ToString().Equals("NB") || dt.Rows[0]["OrderType"].ToString().Equals("DB") || dt.Rows[0]["OrderType"].ToString().Equals("OB"))
            {
                DropDownListRearPlateSize.Visible = true;
                DropDownListFrontPlateSize.Visible = true;
            }

            else if (dt.Rows[0]["OrderType"].ToString().Equals("DF"))
            {
                DropDownListRearPlateSize.Visible = false;
                DropDownListFrontPlateSize.Visible = true;
            }
            else if (dt.Rows[0]["OrderType"].ToString().Equals("DR"))
            {
                DropDownListRearPlateSize.Visible = true;
                DropDownListFrontPlateSize.Visible = false;
            }

            if (dt.Rows[0]["OrderType"].ToString().Equals("OS"))
            {
                DropDownListRearPlateSize.Visible = false;
                DropDownListFrontPlateSize.Visible = false;
                checkBoxSnapLock.Checked = false;
                checkBoxThirdSticker.Checked = true;
                UpdatePanelThirdSticker.Update();
            }
            else
            {

                SQLString = " Select ProductCost from Product Where HSRP_StateID=" + HSRPStateID + " and ProductID in (Select ProductID From Product Where ProductCode='SNAP LOCK')";
                HiddenFieldScrew.Text = Utils.getDataSingleValue(SQLString, CnnString, "ProductCost");
                Cost += Double.Parse(HiddenFieldScrew.Text);
            }


            SQLString = "Select TaxPercentage From Tax Where TaxName='VAT' and HSRP_StateID='" + HSRPStateID + "'";
            VatPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxVat.Text = VatPercentage.ToString();
            UpdatePanelVat.Update();

            textBoxVatAmount.Text = Math.Round(Convert.ToDecimal(((Cost * VatPercentage) / 100).ToString()), 2).ToString();
            UpdatePanelVatAmount.Update();



            if (DropDownListOrderType.SelectedValue.Equals("NB") || DropDownListOrderType.SelectedValue.Equals("OB") || DropDownListOrderType.SelectedValue.Equals("DB"))
            {

                if (DropDownListVehicleModel.SelectedItem.Value.Equals("MOTOR CYCLE"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='MOTOR CYCLE' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }

                else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV(CLASS)"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='LMV(CLASS)' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }
                else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='LMV' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }
                else if (DropDownListVehicleModel.SelectedItem.Value.Equals("SCOOTER"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='SCOOTER' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }
                else if (DropDownListVehicleModel.SelectedItem.Value.Equals("TRACTOR"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='TRACTOR' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }
                else if (DropDownListVehicleModel.SelectedItem.Value.Equals("THREE WHEELER"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='THREE WHEELER' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }
                else if (DropDownListVehicleModel.SelectedItem.Value.Equals("MCV/HCV/TRAILERS"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='MCV/HCV/TRAILERS' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }
            }

            Cost += Discount;
            NetTotal = Cost + ((Cost * VatPercentage) / 100) + ((Cost * ServiceTaxPercentage) / 100);
            textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(NetTotal.ToString()), 2).ToString();
            UpdatePanelNetTotal.Update();

            textBoxVatAmount.Text = Math.Round(Convert.ToDecimal(((Cost * VatPercentage) / 100).ToString()), 2).ToString();
            UpdatePanelVatAmount.Update();



            SQLString = "Select TaxPercentage From Tax Where TaxName='SERVICE TAX' and HSRP_StateID='" + HSRPStateID + "'";
            ServiceTaxPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxServiceTax.Text = ServiceTaxPercentage.ToString();
            UpdatePanelServiceTax.Update();

            textboxServiceTaxAmount.Text = ((Cost * ServiceTaxPercentage) / 100).ToString();
            UpdatePanelServiceTax.Update();



            textBoxTotalAmount.Text = Math.Round(Convert.ToDecimal(Cost.ToString()), 2).ToString();

            if (HSRPStateID =="5")
            {
                if((DropDownListOrderType.SelectedValue=="DR" || DropDownListOrderType.SelectedValue=="DF"))
                {
                    string qr = "select platerate,sticker,total as basicamt,tax,vat,Amount FROM [ProductRateFix] where hsrp_stateid=5 and vehicletype='" + DropDownListVehicleModel.SelectedValue + "'  and ordertype ='" + DropDownListOrderType.SelectedValue + "'";
                    DataTable dtNewRate = Utils.GetDataTable(qr, CnnString);
                    if (dtNewRate.Rows.Count > 0)
                    {
                        textBoxTotalAmount.Text = dtNewRate.Rows[0]["basicamt"].ToString(); ;
                        textBoxVat.Text = dtNewRate.Rows[0]["vat"].ToString(); ;
                        textBoxVatAmount.Text = dtNewRate.Rows[0]["tax"].ToString(); ;
                        textBoxNetTotal.Text = dtNewRate.Rows[0]["Amount"].ToString(); ;
                    }
                }
            }
            UpdatePanelTotalAmount.Update();
        }

        //protected void DropDownListRearPlateSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    RearPlateSelectionChange();
        //}

        //private void RearPlateSelectionChange()
        //{
        //    String FrontPlateSizeProductID = String.Empty;
        //    String RearPlateSizeProductID = String.Empty;
        //    Double Cost = 0.00;
        //    Double VatPercentage = 0.00;
        //    Double ServiceTaxPercentage = 0.00;
        //    Double SnapLock = 0.00;
        //    Double Sticker = 0.00;
        //    Double Discount = 0.00;
        //    Double NetTotal = 0.00;

        //    if (checkBoxRearPlate.Checked.Equals(false))
        //    {
        //        return;
        //    }
        //    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //    String SQLString = string.Empty;
        //    if (checkBoxRearPlate.Checked)
        //    {
        //        if (DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
        //        {
        //            lblErrMess.Text = "Plesae Select Correct Rear Plate Size";
        //            UpdatePanelMessage.Update();
        //            return;
        //        }
        //        else
        //        {
        //            FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
        //            RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

        //            SQLString = "Select Cost From ProductCost Where ProductID=" + RearPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
        //            Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
        //            HiddenFieldRearPlate.Text = Cost.ToString();
        //        }
        //    }

        //    if (checkBoxFrontPlate.Checked)
        //    {
        //        if (!DropDownListFrontPlateSize.SelectedItem.Value.Equals("--Select Front Plate--"))
        //        {
        //            FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
        //            RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

        //            SQLString = "Select Cost From ProductCost Where ProductID=" + FrontPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
        //            Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
        //            SQLString = "Select Cost From ProductCost Where ProductID=" + RearPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
        //            Cost = Cost + Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
        //        }
        //        else
        //        {
        //            if (!DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
        //            {
        //                FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
        //                RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

        //                SQLString = "Select Cost From ProductCost Where ProductID=" + FrontPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
        //                Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
        //            }
        //            else
        //            {
        //                lblErrMess.Text = "Please Select Correct Rear plate Size.";
        //                UpdatePanelMessage.Update();
        //                return;
        //            }
        //        }
        //    }




        //    ///>>>Sticker Cost
        //    if (checkBoxThirdSticker.Checked)
        //    {
        //        SQLString = "Select Cost From ProductCost Where ProductID=9 and HSRP_StateID='" + HSRPStateID + "'";
        //        Sticker = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
        //    }

        //    //>>>> Snap Lock
        //    SQLString = "Select Cost From ProductCost Where ProductID=10 and HSRP_StateID='" + HSRPStateID + "'";
        //    SnapLock = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));

        //    Cost += Sticker + SnapLock;

        //    textBoxTotalAmount.Text = Math.Round(Convert.ToDecimal(Cost.ToString()), 2).ToString();
        //    UpdatePanelTotalAmount.Update();

        //    //>>vat part
        //    SQLString = "Select TaxPercentage From Tax Where TaxName='VAT' and HSRP_StateID='" + HSRPStateID + "'";
        //    VatPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

        //    textBoxVat.Text = VatPercentage.ToString();
        //    UpdatePanelVat.Update();

        //    //>>> Discount
        //    if (checkBoxFrontPlate.Checked && checkBoxRearPlate.Checked)
        //    {
        //        if (DropDownListVehicleModel.SelectedItem.Value.Equals("MOTOR CYCLE"))
        //        {
        //            SQLString = "Select DiscountAmount From Discount Where VehicleType='MOTOR CYCLE' and HSRP_StateID='" + HSRPStateID + "'";
        //            Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
        //        }

        //        else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV(CLASS)"))
        //        {
        //            SQLString = "Select DiscountAmount From Discount Where VehicleType='LMV(CLASS)' and HSRP_StateID='" + HSRPStateID + "'";
        //            Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
        //        }
        //        else if (DropDownListVehicleModel.SelectedItem.Value.Equals("SCOOTER"))
        //        {
        //            SQLString = "Select DiscountAmount From Discount Where VehicleType='SCOOTER' and HSRP_StateID='" + HSRPStateID + "'";
        //            Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
        //        }
        //        else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV"))
        //        {
        //            SQLString = "Select DiscountAmount From Discount Where VehicleType='LMV' and HSRP_StateID='" + HSRPStateID + "'";
        //            Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
        //        }
        //    }
        //    Double cc;
        //    cc = Cost;

        //    if (DropDownListVehicleModel.SelectedItem.Value.Equals("MOTOR CYCLE"))
        //    {
        //        Cost += Discount;
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV(CLASS)"))
        //    {
        //        Cost += Discount;
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("SCOOTER"))
        //    {
        //        Cost += Discount;
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV"))
        //    {
        //        Cost += Discount;
        //    }
        //    NetTotal = Cost + ((Cost * VatPercentage) / 100) + ((Cost * ServiceTaxPercentage) / 100);
        //    textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(NetTotal.ToString()), 2).ToString();
        //    UpdatePanelNetTotal.Update();

        //    textBoxVatAmount.Text = Math.Round(Convert.ToDecimal(((Cost * VatPercentage) / 100).ToString()), 2).ToString();
        //    UpdatePanelVatAmount.Update();


        //    ///>>> Excise Part
        //    SQLString = "Select TaxPercentage From Tax Where TaxName='SERVICE TAX' and HSRP_StateID='" + HSRPStateID + "'";
        //    ServiceTaxPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

        //    textBoxServiceTax.Text = ServiceTaxPercentage.ToString();
        //    UpdatePanelServiceTax.Update();

        //    textboxServiceTaxAmount.Text = ((Cost * ServiceTaxPercentage) / 100).ToString();
        //    UpdatePanelServiceTax.Update();


        //    //>>>>> Discount

        //    textBoxTotalAmount.Text = Math.Round(Convert.ToDecimal(Cost.ToString()), 2).ToString();
        //    UpdatePanelTotalAmount.Update();

        //}

        /// <summary>
        /// Vehicle Model Selection Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        //protected void DropDownListVehicleModel_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    VehicleModelSelectionChange();
        //}

        //private void VehicleModelSelectionChange()
        //{
        //    if (DropDownListVehicleModel.SelectedItem.Value.Equals("SCOOTER"))
        //    {
        //        checkBoxThirdSticker.Checked = false;
        //        checkBoxThirdSticker.Enabled = false;
        //        UpdatePanelThirdSticker.Update();
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("MOTOR CYCLE"))
        //    {
        //        checkBoxThirdSticker.Checked = false;
        //        checkBoxThirdSticker.Enabled = false;
        //        UpdatePanelThirdSticker.Update();
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("TRACTOR"))
        //    {
        //        checkBoxThirdSticker.Checked = false;
        //        checkBoxThirdSticker.Enabled = false;
        //        UpdatePanelThirdSticker.Update();
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("THREE WHEELER"))
        //    {
        //        checkBoxThirdSticker.Checked = true;
        //        checkBoxThirdSticker.Enabled = false;
        //        UpdatePanelThirdSticker.Update();
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV"))
        //    {
        //        checkBoxThirdSticker.Checked = true;
        //        checkBoxThirdSticker.Enabled = false;
        //        UpdatePanelThirdSticker.Update();
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV(CLASS)"))
        //    {
        //        checkBoxThirdSticker.Checked = true;
        //        checkBoxThirdSticker.Enabled = false;
        //        UpdatePanelThirdSticker.Update();
        //    }
        //    else if (DropDownListVehicleModel.SelectedItem.Value.Equals("MCV/HCV/TRAILERS"))
        //    {
        //        checkBoxThirdSticker.Checked = true;
        //        checkBoxThirdSticker.Enabled = false;
        //        UpdatePanelThirdSticker.Update();
        //    }
        //    if (DropDownListVehicleClass.SelectedItem.Text != "--Select Vehicle Class--" && DropDownListVehicleClass.SelectedItem.Text != "")
        //    {
        //        DropDownListFrontPlateSize.SelectedIndex = 0;
        //        DropDownListRearPlateSize.SelectedIndex = 0;
        //        DropDownList1.SelectedIndex = 0;
        //    }
        //    textBoxTotalAmount.Text = String.Empty;
        //    textBoxNetTotal.Text = String.Empty;
        //    textBoxVat.Text = String.Empty;
        //    textboxServiceTaxAmount.Text = String.Empty;
        //    textBoxVatAmount.Text = String.Empty;
        //    textBoxServiceTax.Text = String.Empty;
        //}

        ///// <summary>
        ///// CheckBox Front Plate Check Change
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void checkBoxFrontPlate_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxFrontPlate.Checked)
        //    {
        //        DropDownListFrontPlateSize.Enabled = true;
        //    }
        //    else
        //    {
        //        DropDownListFrontPlateSize.Enabled = false;
        //    }
        //}
        ///// <summary>
        ///// CheckBox Rear Plate Check Change
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void checkBoxRearPlate_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBoxFrontPlate.Checked)
        //    {
        //        DropDownListRearPlateSize.Enabled = true;
        //    }
        //    else
        //    {
        //        DropDownListRearPlateSize.Enabled = false;
        //    }
        //}
        #endregion

        private void FinancialInfoForStickerOnly()
        {

            String FrontPlateSizeProductID = String.Empty;
            String RearPlateSizeProductID = String.Empty;
            Double Cost = 0.00;
            Double VatPercentage = 0.00;
            Double ServiceTaxPercentage = 0.00;
            Double SnapLock = 0.00;
            Double Sticker = 0.00;
            Double Discount = 0.00;
            Double NetTotal = 0.00;


            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            String SQLString = string.Empty;

            textBoxTotalAmount.Text = Cost.ToString();
            UpdatePanelTotalAmount.Update();

            SQLString = "Select TaxPercentage From Tax Where TaxName='VAT' and HSRP_StateID='" + HSRPStateID + "'";
            VatPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxVat.Text = VatPercentage.ToString();
            UpdatePanelVat.Update();

            textBoxVatAmount.Text = ((Cost * VatPercentage) / 100).ToString();
            UpdatePanelVatAmount.Update();

            SQLString = "Select TaxPercentage From Tax Where TaxName='SERVICE TAX' and HSRP_StateID='" + HSRPStateID + "'";
            ServiceTaxPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxServiceTax.Text = ServiceTaxPercentage.ToString();
            UpdatePanelServiceTax.Update();

            textboxServiceTaxAmount.Text = ((Cost * ServiceTaxPercentage) / 100).ToString();
            UpdatePanelServiceTax.Update();

            if (checkBoxThirdSticker.Checked)
            {
                SQLString = "Select Cost From ProductCost Where ProductID=9 and HSRP_StateID='" + HSRPStateID + "'";
                Sticker = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
            }

            NetTotal = ((Sticker * VatPercentage) / 100) + ((Sticker * ServiceTaxPercentage) / 100) + Sticker + SnapLock;
            textBoxNetTotal.Text = NetTotal.ToString();
            UpdatePanelNetTotal.Update();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BlankFields();
        }

        protected void DropDownList44_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            UpdatePanelMessage.Update();
            if (DropDownListVehicleMaker1.SelectedValue.Equals("--Select Vehicle Maker--"))
            {
                lblErrMess.Text = "Please Select Correct Vehicle Maker.";
                UpdatePanelMessage.Update();
                DropDownListVehicleMaker1.Focus();
                return;

            }
            else
            {
                FillVehicleModel();
            }
            int ISExists = -1;
            BAL obj = new BAL();
            // if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, ref ISExists))
            if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, TextBoxVehicleRegNo.Text.Trim(), HSRPStateID, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
                    lblErrMess.Text = "Record Already Exist!!";
                }
                else
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                }
            }
        }

        protected void DropDownListOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListVehicleClass.SelectedValue.Equals("--Select Vehicle Class--"))
            {
                lblErrMess.Text = "please select correct Vehicle Class.";
                UpdatePanelMessage.Update();
                return;
            }
            if (DropDownListVehicleModel.SelectedValue.Equals("--Select Vehicle Model--"))
            {
                lblErrMess.Text = "please select correct Vehicle Type.";
                UpdatePanelMessage.Update();
                return;
            }
            OrderSelectionChange();
            int ISExists = -1;
            BAL obj = new BAL();
            // if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, ref ISExists))
               if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, TextBoxVehicleRegNo.Text.Trim(), HSRPStateID, ref ISExists))
               {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
                    lblErrMess.Text = "Record Already Exist!! (Please Close the Existing Order First)";
                }
                else
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                }
            }
        }

        protected void DropDownListVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BlankFieldsVehicleClass();
            HSRPStateID = Session["UserHSRPStateID"].ToString();

            if (DropDownListVehicleClass.SelectedItem.Value.ToUpper().Equals("--Select Vehicle Class--"))
            {
                lblErrMess.Text = "Please Select Correct Vehicle Class";
                UpdatePanelMessage.Update();
                return;
            }

            int ISExists = -1;
            BAL obj = new BAL();
            if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, TextBoxVehicleRegNo.Text.Trim(), HSRPStateID, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
                    lblErrMess.Text = "Record Already Exist!!";
                }
                else
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                }
            }
            if (DropDownListVehicleClass.SelectedItem.ToString() == "Transport")
            {


                DropDownListVehicleModel.Items.Clear();
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Vehicle Model--", "--Select Vehicle Model--"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("TRACTOR", "TRACTOR"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("THREE WHEELER", "THREE WHEELER"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("LMV", "LMV"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("LMV(CLASS)", "LMV(CLASS)"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("MCV/HCV/TRAILERS", "MCV/HCV/TRAILERS"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("E-RICKSHAW", "E-RICKSHAW"));


            }
            else
            {
                DropDownListVehicleModel.Items.Clear();
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Vehicle Model--", "--Select Vehicle Model--"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("SCOOTER", "SCOOTER"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("MOTOR CYCLE", "MOTOR CYCLE"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("TRACTOR", "TRACTOR"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("THREE WHEELER", "THREE WHEELER"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("LMV", "LMV"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("LMV(CLASS)", "LMV(CLASS)"));
                DropDownListVehicleModel.Items.Add(new System.Web.UI.WebControls.ListItem("MCV/HCV/TRAILERS", "MCV/HCV/TRAILERS"));
            }
        }

        void BlankFieldsVehicleClass()
        {
            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            DropDownListVehicleModel.SelectedValue = "--Select Vehicle Model--";
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            DropDownListVehicleMaker1.SelectedValue = "--Select Vehicle Maker--";
            DropDownListModel.SelectedValue = "--Select Vehicle Model--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
        }

        void BlankFieldsVehicleModel()
        {
            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            DropDownListVehicleMaker1.SelectedValue = "--Select Vehicle Maker--";
            DropDownListModel.SelectedValue = "--Select Vehicle Model--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
        }

        protected void DropDownListVehicleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            BlankFieldsVehicleModel();
            int ISExists = -1;
            BAL obj = new BAL();
            //if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, ref ISExists))
            if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, TextBoxVehicleRegNo.Text.Trim(), HSRPStateID, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
                    lblErrMess.Text = "Record Already Exist!!";
                }
                else
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                }
            }
        }

        protected void LinkButtonCashReceipts_Click(object sender, EventArgs e)
        {
            string filename = HiddenFieldCashReceipt.Text;

            string filename1 = System.Configuration.ConfigurationManager.AppSettings["HTTPDataFolder"].ToString();
            filename1 = filename1 + filename;
            Response.Redirect(filename1, false);
            Response.End();
            //save_file_from_url(filename, filename1);

        }

        public void save_file_from_url(string file_name, string url)
        {
            byte[] content;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();

            Stream stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                content = br.ReadBytes(500000);
                br.Close();
            }
            response.Close();

            FileStream fs = new FileStream(file_name, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(content);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }
        }

        protected void LknBtnCashReceipts_Click(object sender, EventArgs e)
        {
            string HSRPRecordID = ViewState["HSRPRecordID"].ToString();
           // PrintDocument(HSRPRecordID);
            PrintCashReceipt(HSRPRecordID);
            //string HSRPRecordID = "1909";
          
        }

        public void EpsionPrint(string HSRPRecordID)
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
            // sqlquery="select * from AffixationCenters where State_Id='"+Session["UserHSRPStateID"].ToString()+"' and Rto_Id='"+Session["UserRTOLocationID"].ToString()+"'";


            //sqlquery = "select Address1 from AffixationCenters where Affix_Id='" + ddlaffixation.SelectedValue.ToString() + "'";
            //AffAddress = Utils.getDataSingleValue(sqlquery, ConnectionString, "Address1");

            //SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where vehicleRegNo='" + txtVehicleRegNo.Text.Trim() + "'";
            string SQLString = "select hsrprecordID,CashReceiptNo, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where hsrprecordid='" + HSRPRecordID + "' order by HSRPRecord_AuthorizationNo,vehicleRegNo desc";
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
                float imageHeight = 360;
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

                PdfPCell cell222 = new PdfPCell(new Phrase(": " + Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"]).ToString("hh:mm:ss"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell222.Colspan = 0;

                cell222.BorderWidthLeft = 0f;
                cell222.BorderWidthRight = 0f;
                cell222.BorderWidthTop = 0f;
                cell222.BorderWidthBottom = 0f;
                cell222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell222);



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

    


      
        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (OrderDate.SelectedDate.ToString().Equals("1/1/0001 12:00:00 AM"))
            {
                // Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Please select Authrization Date.');</script>");
                lblErrMess.Text = "Please select Authrization Date.";
                return;
            }
            else
            {
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                //String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                //String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                String From1 = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                
                //>> Authorization Info

                StringCustomerName = textBoxCustomerName.Text.Trim().Replace("'", "''").ToString();
                StringOrderStatus = textBoxOrderStatus.Text.Trim().Replace("'", "''").ToString();
                StringOrderStatus = "New Order";
                StringAddress1 = textBoxAddress1.Text.Trim().Replace("'", "''").ToString();
                StringAddress2 = textBoxAddress2.Text.Trim().Replace("'", "''").ToString();
                StringMobileNo = textBoxMobileNo.Text.Trim().Replace("'", "''").ToString();
                StringLandline = textBoxLandline.Text.Trim().Replace("'", "''").ToString();
                StringEmailId = textBoxEmailId.Text.Trim().Replace("'", "''").ToString();
                //>>> Vehicle Info
                StringVehicleClass = DropDownListVehicleClass.SelectedItem.Value;
                if (StringVehicleClass.Equals("--Select Vehicle Class--"))
                {
                    lblErrMess.Text = "please select Correct vehicle Class.";
                    UpdatePanelMessage.Update();
                    return;
                }
                StringManufacturerName = DropDownListVehicleMaker1.SelectedItem.Value;
                if (StringManufacturerName.Equals("--Select Vehicle Maker--"))
                {
                    StringManufacturerModel = "";
                    StringManufacturerName = "";
                }
                else
                {
                    StringManufacturerModel = DropDownListModel.SelectedItem.Value;
                }
                if (DropDownListOrderType.SelectedItem.Value.Equals("OS"))
                {

                }
                else if (DropDownListOrderType.SelectedItem.Value.Equals("DF"))
                {
                    if (DropDownListFrontPlateSize.SelectedItem.Value.Equals("--Select Front Plate--"))
                    {
                        lblErrMess.Text = "Please select correct front plate.";
                        UpdatePanelDropDownListFrontPlate.Update();
                        return;
                    }
                }
                else if (DropDownListOrderType.SelectedItem.Value.Equals("DR"))
                {
                    if (DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
                    {
                        lblErrMess.Text = "Please select correct rear plate.";
                        UpdatePanelDropDownListFrontPlate.Update();
                        return;
                    }
                }
                else
                {
                    if (DropDownListFrontPlateSize.SelectedItem.Value.Equals("--Select Front Plate--"))
                    {
                        lblErrMess.Text = "Please select correct front plate.";
                        UpdatePanelDropDownListFrontPlate.Update();
                        return;
                    }
                    if (DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
                    {
                        lblErrMess.Text = "Please select correct rear plate.";
                        UpdatePanelDropDownListFrontPlate.Update();
                        return;
                    }
                }
                StringVehicleModel = DropDownListVehicleModel.SelectedItem.Value;


                StringVehicleRegNo = TextBoxVehicleRegNo.Text.Trim().Replace("'", "''").ToString();
                StringEngineNo = textboxEngineNo.Text.Trim().Replace("'", "''").ToString();
                StringChassisNo = textBoxChassisNo.Text.Trim().Replace("'", "''").ToString();
                //>>> Number Plate Info


                StringOrderType = DropDownListOrderType.SelectedItem.Value;

                if (StringOrderType.Equals("--Select Order Type--"))
                {
                    lblErrMess.Text = "please select Correct Order Type.";
                    UpdatePanelMessage.Update();
                    return;
                }

                if (String.IsNullOrEmpty(labelFrontPlateSize.Text))
                {
                    IScheckBoxFrontPlate = "N";
                }
                else
                {
                    IScheckBoxFrontPlate = "Y";
                }

                if (String.IsNullOrEmpty(labelRearPlateSize.Text))
                {
                    IScheckBoxRearPlate = "N";
                }
                else
                {
                    IScheckBoxRearPlate = "Y";
                }

                if (checkBoxThirdSticker.Checked)
                {
                    ISThirdStickerExists = "Y";
                }
                else
                {
                    ISThirdStickerExists = "N";
                }

                AuthorizationNo = textBoxAuthorizationNo.Text.Trim().Replace("'", "''").ToString();
                StringTotalAmount = textBoxTotalAmount.Text.Trim().Replace("'", "''").ToString();
                StringVATPercentage = textBoxVat.Text.Trim().Replace("'", "''").ToString();
                StringVATAmount = textBoxVatAmount.Text.Trim().Replace("'", "''").ToString();
                StringServiceTaxPercentage = textBoxServiceTax.Text.Trim().Replace("'", "''").ToString();
                StringServiceTaxAmount = textboxServiceTaxAmount.Text.Trim().Replace("'", "''").ToString();
                StringNetTotal = textBoxNetTotal.Text.Trim().Replace("'", "''").ToString();

                StringFixingCharge = "0";//HiddenFieldFixing.Text.Trim().Replace("'", "''").ToString();
                StringFrontPlatePrize = HiddenFieldFrontPlate.Text.Trim().Replace("'", "''").ToString();
                StringRearPlatePrize = HiddenFieldRearPlate.Text.Trim().Replace("'", "''").ToString();
                StringStickerPrize = HiddenFieldSticker.Text.Trim().Replace("'", "''").ToString();
                StringScrewPrize = HiddenFieldScrew.Text.Trim().Replace("'", "''").ToString();

                StringFrontPlateSize = HiddenFieldFrontPlateCode.Text;
                StringRearPlateSize = HiddenFieldRearPlateCode.Text;

                StringFrontPlateSize = DropDownListFrontPlateSize.SelectedValue;
                StringRearPlateSize = DropDownListRearPlateSize.SelectedValue;
                lblErrMess.Text = String.Empty;
                lblSucMess.Text = String.Empty;

                int Isexists = -1;
                string HSRPRecordID = "";
                BAL obj = new BAL();


                HSRPRecordIDXX = HiddenFieldHSRPRecords.Text;

                if (obj.EditHSRPRecords(AuthorizationNo, AuthorizationDate, StringOrderNo,
                    StringCustomerName, StringOrderStatus, StringAddress1, StringAddress2, StringMobileNo, StringLandline,
                   StringEmailId, StringVehicleClass, StringManufacturerName, StringManufacturerModel, StringVehicleModel, StringVehicleRegNo, StringEngineNo,
                   StringChassisNo, StringOrderType, IScheckBoxFrontPlate, StringFrontPlateSize, IScheckBoxRearPlate,
                   StringRearPlateSize, ISThirdStickerExists, StringInvoiceNo, StringCashReceiptNo, StringVATPercentage,
                   StringVATAmount, StringServiceTaxPercentage, StringServiceTaxAmount, StringTotalAmount, StringNetTotal, HSRPStateID,
                   dropDownListRTOLocation.SelectedValue, StringDeliverChallanNo, StringFixingCharge, StringFrontPlatePrize, StringRearPlatePrize,
                   StringStickerPrize, StringScrewPrize, textBoxRemarks.Text, UID, dropDownListRTOLocation.SelectedValue, ref Isexists, HSRPRecordIDXX))
                {
                    if (Isexists.Equals(1))
                    {
                        lblErrMess.Text = "Duplicate Record!!";
                        return;
                    }
                    else
                    {
                        string script = "<script type=\"text/javascript\">  alert('Record Updated Sucessfully.');</script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

                        lblSucMess.Text = "Record Updated Sucessfully.";
                        BlankFields();
                        textBoxAuthorizationNo.Focus();
                        PrintCashReceipt(HSRPRecordIDXX);
                        //   LinkButtonCashReceipt.Visible = true;
                        // LinkButtonCashReceipt.HRef = "http://180.151.100.242/Data/" + HiddenFieldCashReceipt.Text;
                    }

                }
                else
                {
                    lblErrMess.Text = "Record not Updated.";
                }
            }
        }

        protected void LinkButton1Epson_Click(object sender, EventArgs e)
        {
            string HSRPRecordID = ViewState["HSRPRecordID"].ToString();
            EpsionPrint(HSRPRecordID);
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            mpeSave.Hide();
            textBoxCustomerName.Text=ViewState["Name"].ToString();
            textBoxAddress1.Text=ViewState["Address"].ToString();
            SaveData();
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {

            mpeSave.Hide();
            textBoxCustomerName.Text = ViewState["Name"].ToString();
            textBoxAddress1.Text = ViewState["Address"].ToString();
        }
    }
}