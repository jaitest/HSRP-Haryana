using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataProvider;
using System.Data;

namespace HSRP.Transaction
{
    public partial class VIP_HSRPRecords : System.Web.UI.Page
    {
        //Authorization Info
        string AuthorizationNo = String.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
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
        protected void Page_Load(object sender, EventArgs e)
        {



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
                LabelUSER.Text = Session["UserName"].ToString();
               // LabelCreatedID.Text = "Allowed RTO's :- ";
                SQLString = "Select R.RTOLocationCode,R.RTOLocationID From RTOLocation R inner join UserRTOLocationMapping U on " +
                            "R.RTOLocationID=U.RTOLocationID Where U.UserID=" + UID + " order by R.RTOLocationCode";
                DataSet dds = Utils.getDataSet(SQLString, CnnString);
                if (dds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dds.Tables[0].Rows)
                    {
                       // LabelCreatedID.Text += dr["RTOLocationCode"].ToString() + " / ";
                        RTOs += dr["RTOLocationID"].ToString() + " / ";
                    }

                }
             //   LabelCreatedID.Text = LabelCreatedID.Text.Substring(0, LabelCreatedID.Text.Length - 2);
                RTOs = RTOs.Substring(0, RTOs.Length - 2);
                LabelCreatedDateTime.Text = DateTime.Now.ToString();

                AutoCompleteExtender1.ContextKey = RTOs;
                AutoCompleteExtender2.ContextKey = RTOs;

                String[] kk = DateTime.Now.ToString().Split('/');
                if (kk[0].Length == 1)
                {
                    kk[0] = "0" + kk[0];
                }
                LabelCreatedDateTime.Text = kk[1] + "/" + kk[0] + "/" + kk[2];
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
                buttonUpdate.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                //InitialSetting();
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
            }
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
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void buttonGo_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxAuthorizationNo.Text))
            {
                lblErrMess.Text = String.Empty;
                lblSucMess.Text = String.Empty;
                BlankFieldsAuth();
                FillHSRPRecordDetails(textBoxAuthorizationNo.Text);
            }
        }

        protected void LinkButtonGo2_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(TextBoxVehicleRegNo.Text))
            {
                lblErrMess.Text = String.Empty;
                lblSucMess.Text = String.Empty;
                BlankFieldsVehicleReg();
                FillHSRPRecordDetails2(TextBoxVehicleRegNo.Text);
            }
        }

        protected void LinkButtonGo3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBoxCashReceiptNo.Text))
            {
                lblErrMess.Text = String.Empty;
                lblSucMess.Text = String.Empty;
                BlankFieldsCashReceipt();
                FillHSRPRecordDetails3(TextBoxCashReceiptNo.Text);
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
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
            InitialSetting();
            textBoxAuthorizationNo.Text = String.Empty;
        }

        void BlankFieldsAuth()
        {
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
            DropDownListVehicleModel.SelectedValue = "--Select Vehicle Model--";
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
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
            InitialSetting();
            textBoxAuthorizationNo.Text = String.Empty;
        }

        void BlankFieldsCashReceipt()
        {
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
            DropDownListVehicleMaker1.SelectedValue = "--Select Vehicle Maker--";
            DropDownListModel.SelectedValue = "--Select Vehicle Model--";
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            checkBoxThirdSticker.Checked = false;
            checkBoxSnapLock.Checked = false;
            InitialSetting();
            textBoxAuthorizationNo.Text = String.Empty;
        }

        private void FillHSRPRecordDetails(String HSRPAuthNo)
        {
            DropDownListVehicleMaker1.Items.Clear();
            DropDownListModel.Items.Clear();
            BAL obj = new BAL();

            if (HSRPStateID.ToString().Equals("5"))
            {
                SQLString = "Select HSRPRecord_AuthorizationNo From  HSRPRecords where HSRPRecord_AuthorizationNo='" + HSRPAuthNo + "'";
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count == 0)
                {
                    if (HSRPStateID.ToString().Equals("5"))
                    {
                        SQLString = "Select * From View_MP_HSRPRecordsStagging where HSRPRecord_AuthorizationNo ='" + HSRPAuthNo + "'";
                        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringMP"].ToString();
                        dataSetFillHSRPRecordDetail = Utils.getDataSet(SQLString, CnnString);
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
                        BlankFieldsAuth();

                        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                        SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";
                        DataTable VehicleMaker1 = Utils.GetDataTable(SQLString, CnnString);
                        foreach (DataRow dr in VehicleMaker1.Rows)
                        {
                            DropDownListVehicleMaker1.Items.Add(new ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
                        }
                        ListItem li1 = new ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
                        DropDownListVehicleMaker1.Items.Insert(0, li1);


                        lblErrMess.Text = "No Such Record Exists.";
                        UpdatePanelMessage.Update();
                        textBoxAuthorizationNo.Focus();
                        return;
                    }
                }
            }
            int ISExists = -1;

          //  if (obj.CheckHSRPRecordAuthNo(HSRPAuthNo, ref ISExists))
            if (obj.CheckHSRPRecordAuthNo(textBoxAuthorizationNo.Text, TextBoxVehicleRegNo.Text.Trim(), HSRPStateID, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                    lblErrMess.Text = "Record Already Exist!!";
                }
                else
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
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

            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString()))
            {
                DropDownListVehicleClass.SelectedIndex = 0;
            }
            else
            {
                DropDownListVehicleClass.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString();
            }

            //textBoxVehicleMake.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString();
            // FillVehicleMaker();

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";

            DataTable VehicleMaker = Utils.GetDataTable(SQLString, CnnString);

            foreach (DataRow dr in VehicleMaker.Rows)
            {
                DropDownListVehicleMaker1.Items.Add(new ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
            }

            ListItem li = new ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
            DropDownListVehicleMaker1.Items.Insert(0, li);

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
                        DropDownListModel.Items.Add(new ListItem(dr["VehicleModelDescription"].ToString(), dr["VehicleModelID"].ToString()));
                    }

                    ListItem lii = new ListItem("--Select Vehicle Model--", "--Select Vehicle Model--");
                    DropDownListModel.Items.Insert(0, lii);

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
                DropDownListVehicleModel.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString();
            }
            TextBoxVehicleRegNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleRegNo"].ToString();
            textboxEngineNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["EngineNo"].ToString();
            textBoxChassisNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ChassisNo"].ToString();


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
            BAL obj = new BAL();
            DropDownListVehicleMaker1.Items.Clear();
            DropDownListModel.Items.Clear();


            if (HSRPStateID.ToString().Equals("5"))
            {
                SQLString = "Select HSRPRecord_AuthorizationNo From  HSRPRecords where VehicleRegNo='" + VehicleRegNo + "'";
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count == 0)
                {
                    if (HSRPStateID.ToString().Equals("5"))
                    {
                        SQLString = "Select * From View_MP_HSRPRecordsStagging where VehicleRegNo='" + VehicleRegNo + "'";
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
                        BlankFieldsVehicleReg();
                        CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                        SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";
                        DataTable VehicleMaker2 = Utils.GetDataTable(SQLString, CnnString);
                        foreach (DataRow dr in VehicleMaker2.Rows)
                        {
                            DropDownListVehicleMaker1.Items.Add(new ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
                        }
                        ListItem li2 = new ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
                        DropDownListVehicleMaker1.Items.Insert(0, li2);
                        lblErrMess.Text = "No Such Record Exists.";
                        UpdatePanelMessage.Update();
                        TextBoxVehicleRegNo.Focus();
                        return;
                    }
                }
            }

            int ISExists = -1;

            if (obj.CheckHSRPRecordHSRPVehicleReg(VehicleRegNo, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                }
                else
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
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
            if (String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString()))
            {
                DropDownListVehicleModel.SelectedIndex = 0;
            }
            else
            {
                DropDownListVehicleModel.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString();
            }
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";

            DataTable VehicleMaker = Utils.GetDataTable(SQLString, CnnString);

            foreach (DataRow dr in VehicleMaker.Rows)
            {
                DropDownListVehicleMaker1.Items.Add(new ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
            }

            ListItem li = new ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
            DropDownListVehicleMaker1.Items.Insert(0, li);

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
                        DropDownListModel.Items.Add(new ListItem(dr["VehicleModelDescription"].ToString(), dr["VehicleModelID"].ToString()));
                    }

                    ListItem lii = new ListItem("--Select Vehicle Model--", "--Select Vehicle Model--");
                    DropDownListModel.Items.Insert(0, lii);

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
                SQLString = "Select HSRPRecord_AuthorizationNo From HSRPRecords where VehicleRegNo='" + CashReceiptNo + "'";
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
                            DropDownListVehicleMaker1.Items.Add(new ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
                        }
                        ListItem li3 = new ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
                        DropDownListVehicleMaker1.Items.Insert(0, li3);
                        lblErrMess.Text = "No Such Record Exists.";
                        UpdatePanelMessage.Update();
                        TextBoxCashReceiptNo.Focus();
                        return;
                    }
                }
            }

            int ISExists = -1;

            if (obj.CheckHSRPRecordHSRPVehicleReg(CashReceiptNo, ref ISExists))
            {
                if (ISExists.Equals(1))
                {
                    buttonSave.Enabled = true;
                    buttonSave.Visible = true;
                }
                else
                {
                    buttonSave.Enabled = false;
                    buttonSave.Visible = false;
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
                DropDownListVehicleMaker1.Items.Add(new ListItem(dr["VehicleMakerDescription"].ToString(), dr["VehicleMakerID"].ToString()));
            }

            ListItem li = new ListItem("--Select Vehicle Maker--", "--Select Vehicle Maker--");
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
                    DropDownListModel.Items.Add(new ListItem(dr["VehicleModelDescription"].ToString(), dr["VehicleModelID"].ToString()));
                }

                ListItem lii = new ListItem("--Select Vehicle Model--", "--Select Vehicle Model--");
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
            if (OrderDate.SelectedDate.ToString().Equals("1/1/0001 12:00:00 AM"))
            {
                // Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Please select Authrization Date.');</script>");
                lblErrMess.Text = "Please select Authrization Date.";
                return;
            }
            //if (OrderDate.SelectedDate.ToString().Equals("1/1/0001 12:00:00 AM"))
            //{
            //    string closescript1 = "<script>alert('Please select Authrization Date .')</script>";
            //        Page.RegisterStartupScript("abc", closescript1);
            //        return;
            //}
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
                //if (StringManufacturerName.Equals("--Select Vehicle Maker--"))
                //{
                //    lblErrMess.Text = "please select Correct vehicle Maker.";
                //    UpdatePanelMessage.Update();
                //    return;
                //}
                if (StringManufacturerName.Equals("--Select Vehicle Maker--"))
                {
                    StringManufacturerModel = "--Select Vehicle Model--";
                }
                else
                {
                    StringManufacturerModel = DropDownListModel.SelectedItem.Value;
                }
                //if (StringManufacturerModel.Equals("--Select Vehicle Model--"))
                //{
                //    lblErrMess.Text = "please select Correct vehicle Model.";
                //    UpdatePanelMessage.Update();
                //    return;
                //}
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

                //bool IsValid = false;
                //string mmmm = StringVehicleRegNo.Substring(0, 4);
                //if (char.IsNumber(StringVehicleRegNo, 3))
                //{
                //    SQLString = "Select R.RTOLocationCode,R.RTOLocationID From RTOLocation R inner join UserRTOLocationMapping U on " +
                //               "R.RTOLocationID=U.RTOLocationID Where U.UserID=" + UID + "";
                //    DataSet dds = Utils.getDataSet(SQLString, CnnString);
                //    if (dds.Tables[0].Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dds.Tables[0].Rows)
                //        {
                //            if (dr["RTOLocationCode"].ToString().ToLower().Equals(mmmm.ToLower()))
                //            {
                //                IsValid = true;
                //                break;
                //            }
                //        }
                //    }
                //}

                //else
                //{
                //    mmmm = StringVehicleRegNo.Substring(0, 3);
                //    SQLString = "Select R.RTOLocationCode,R.RTOLocationID From RTOLocation R inner join UserRTOLocationMapping U on " +
                //               "R.RTOLocationID=U.RTOLocationID Where U.UserID=" + UID + "";
                //    DataSet ddss = Utils.getDataSet(SQLString, CnnString);
                //    if (ddss.Tables[0].Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in ddss.Tables[0].Rows)
                //        {
                //            if (dr["RTOLocationCode"].ToString().ToLower().Equals(mmmm.ToLower()))
                //            {
                //                IsValid = true;
                //                break;
                //            }
                //        }
                //    }
                //}

                //if (!IsValid)
                //{
                //    lblErrMess.Text = "Please enter correct Vehicle Registration No.";
                //    UpdatePanelMessage.Update();
                //    return;
                //}

                ////>>>> Financial Info

                ////>>>>> For Correct Invoice No,OrderNo etc.

                //string mm = StringVehicleRegNo.Substring(0, 4);
                //string RTOForInvoice = string.Empty;
                //if (char.IsNumber(StringVehicleRegNo, 3))
                //{
                //    SQLString = "Select R.RTOLocationCode,R.RTOLocationID From RTOLocation R inner join UserRTOLocationMapping U on " +
                //               "R.RTOLocationID=U.RTOLocationID Where U.UserID=" + UID + "";
                //    DataSet dds = Utils.getDataSet(SQLString, CnnString);
                //    if (dds.Tables[0].Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dds.Tables[0].Rows)
                //        {
                //            if (dr["RTOLocationCode"].ToString().ToLower().Equals(mm.ToLower()))
                //            {
                //                RTOForInvoice = dr["RTOLocationID"].ToString();
                //                break;
                //            }

                //        }
                //    }
                //}

                //else
                //{
                //    mm = StringVehicleRegNo.Substring(0, 3);
                //    SQLString = "Select R.RTOLocationCode,R.RTOLocationID From RTOLocation R inner join UserRTOLocationMapping U on " +
                //               "R.RTOLocationID=U.RTOLocationID Where U.UserID=" + UID + "";
                //    DataSet dds = Utils.getDataSet(SQLString, CnnString);
                //    if (dds.Tables[0].Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dds.Tables[0].Rows)
                //        {
                //            if (dr["RTOLocationCode"].ToString().ToLower().Equals(mm.ToLower()))
                //            {
                //                RTOForInvoice = dr["RTOLocationID"].ToString();
                //                break;
                //            }

                //        }
                //    }
                //}

                if (StringMode.Equals("New"))
                {
                    SQLString = "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as InvoiceNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID=" + RTOLocationID + "  and PrefixFor = 'Invoice No';" +
                        "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as CashReceipt  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID=" + RTOLocationID + " and PrefixFor = 'Cash Receipt No';" +
                        "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as OrderNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID=" + RTOLocationID + "  and PrefixFor = 'Order No';" +
                        "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as DeliveryChalanNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID=" + RTOLocationID + " and PrefixFor = 'Delivery Challan No'";

                    DataSet ds = Utils.getDataSet(SQLString, CnnString);
                    StringInvoiceNo = ds.Tables[0].Rows[0][0].ToString();
                    StringCashReceiptNo = ds.Tables[1].Rows[0][0].ToString();
                    StringOrderNo = ds.Tables[2].Rows[0][0].ToString();
                    StringDeliverChallanNo = ds.Tables[3].Rows[0][0].ToString();

                    SQLString = "Update Prefix Set LastNo=LastNo+1 where HSRP_StateID =" + HSRPStateID + " and RTOLocationID =" + RTOLocationID + "";
                    Utils.ExecNonQuery(SQLString, CnnString);
                }
                else if (StringMode.Equals("Edit"))
                {
                    // StringInvoiceNo = textBoxInvoiceNo.Text.Trim().Replace("'", "''").ToString();
                    // StringOrderNo = textBoxOrderNo.Text.Trim().Replace("'", "''").ToString();
                    // StringCashReceiptNo = textBoxCashReceiptNo.Text.Trim().Replace("'", "''").ToString();
                    // StringDeliverChallanNo = textBoxDelieveryChallan.Text.Trim().Replace("'", "''").ToString();

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
                BAL obj = new BAL();
                if (obj.InsertVIPHSRPRecords(AuthorizationNo, AuthorizationDate, StringOrderNo,
                    StringCustomerName, StringOrderStatus, StringAddress1, StringAddress2, StringMobileNo, StringLandline,
                   StringEmailId, StringVehicleClass, StringManufacturerName, StringManufacturerModel, StringVehicleModel, StringVehicleRegNo, StringEngineNo,
                   StringChassisNo, StringOrderType, IScheckBoxFrontPlate, StringFrontPlateSize, IScheckBoxRearPlate,
                   StringRearPlateSize, ISThirdStickerExists, StringInvoiceNo, StringCashReceiptNo, StringVATPercentage,
                   StringVATAmount, StringServiceTaxPercentage, StringServiceTaxAmount, StringTotalAmount, StringNetTotal, HSRPStateID,
                   RTOLocationID, StringDeliverChallanNo, StringFixingCharge, StringFrontPlatePrize, StringRearPlatePrize,
                   StringStickerPrize, StringScrewPrize,ref Isexists))
                {
                    if (Isexists == 1)
                    {
                        lblErrMess.Text = "Duplicate Record.";
                        return;
                        //BlankFields();
                    }
                    SQLString = "Update HSRPRecordsStaggingArea Set OrderStatus='Processed' Where HSRPRecord_AuthorizationNo='" + AuthorizationNo + "'";
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    Utils.ExecNonQuery(SQLString, CnnString);
                    lblSucMess.Text = "Record Added Sucessfully.";
                    BlankFields();
                    textBoxAuthorizationNo.Focus();
                    // string script = "<script type=\"text/javascript\">  alert('Record Save Successfully.');</script>";
                    // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                }
                else
                {
                    lblErrMess.Text = "Record not Added.";
                }
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
        }

        protected void DropDownListVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BlankFieldsVehicleClass();


            if (DropDownListVehicleClass.SelectedItem.Value.ToUpper().Equals("--Select Vehicle Class--"))
            {
                lblErrMess.Text = "Please Select Correct Vehicle Class";
                UpdatePanelMessage.Update();
                return;
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
        }
    }
}