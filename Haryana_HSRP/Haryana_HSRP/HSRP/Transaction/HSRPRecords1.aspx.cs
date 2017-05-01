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

namespace HSRP.Transaction
{
    public partial class HSRPRecords1 : System.Web.UI.Page
    {
          //Authorization Info
        string AuthorizationNo = String.Empty;
        DateTime AuthorizationDate ;
        DateTime OrderDate1 ;
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
        string StringVehicleMake = String.Empty;
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
        String RTOLocationID;
        int HSRPRecordID;
        int UID;
        int UserType;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string StringMode = String.Empty;
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
                HSRPStateID=Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                int.TryParse(Session["UID"].ToString(), out UID);

                LabelCreatedID.Text = Session["UserName"].ToString();
                LabelCreatedDateTime.Text = DateTime.Now.ToString();
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

                textBoxMobileNo.Attributes.Add("onkeypress", "return isNumberKey(event);");
                textBoxLandline.Attributes.Add("onkeypress", "return isNumberKey(event);");
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
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
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
                    FillHSRPRecordDetails(textBoxAuthorizationNo.Text);
	            }
        }

        void BlankFields()
        {
            textBoxAddress1.Text = String.Empty;
            textBoxAddress2.Text = String.Empty;
            textBoxAuthorizationNo.Text = String.Empty;
            textBoxCustomerName.Text = String.Empty;
            textBoxOrderStatus.Text = String.Empty;
            textBoxMobileNo.Text = String.Empty;
            textBoxLandline.Text = String.Empty;
            textBoxEmailId.Text = String.Empty;
            textBoxVehicleMake.Text = String.Empty;
            TextBoxVehicleRegNo.Text = String.Empty;
            textboxEngineNo.Text = String.Empty;
            textBoxChassisNo.Text = String.Empty;
            textBoxInvoiceNo.Text = String.Empty;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
            textBoxCashReceiptNo.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            DropDownListVehicleClass.SelectedValue = "--Select Vehicle Class--";
            DropDownListVehicleModel.SelectedValue = "--Select Vehicle Model--";
            DropDownListOrderType.SelectedValue = "--Select Order Type--";
            DropDownListFrontPlateSize.SelectedValue = "--Select Front Plate--";
            DropDownListRearPlateSize.SelectedValue = "--Select Rear Plate--";
            checkBoxFrontPlate.Checked = false;
            checkBoxRearPlate.Checked = false;
            checkBoxThirdSticker.Checked = false;

            InitialSetting();
            textBoxAuthorizationNo.Text = String.Empty;
        }

        private void FillHSRPRecordDetails( String HSRPAuthNo)
        {
            BAL obj = new BAL();
            if (obj.FillHSRPRecordDetail(HSRPAuthNo, ref dataSetFillHSRPRecordDetail))
            {
                if (dataSetFillHSRPRecordDetail.Tables[0].Rows.Count < 1)
                {
                    BlankFields();
                    lblErrMess.Text = "No Such Record Exists.";
                    UpdatePanelMessage.Update();
                    return;
                    //string script = "<script type=\"text/javascript\">  alert('No Such Record Exists.');</script>";
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);


                    //string script1 = "<script type='text/javascript' language='javascript'>alert('No Such Record Exists.'); document.getElementById('textBoxAuthorizationNo').focus(); return false;</script>";
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alertjj", script1,true);
                    ////lblErrMess.Text = "No Such Record Exists.";
                    ////UpdatePanelMessage.Update();
                    //return;

                    //String csname1 = "PopupScript";

                    //if (!IsClientScriptBlockRegistered(csname1))
                    //{
                    //    String cstext1 = "<script type=\"text/javascript\">" +
                    //        "alert('Hello World');</" + "script>";
                    //    RegisterStartupScript(csname1, cstext1);
                    //}

                    //Alert.Show("No Such Record Exists");
                    return ;
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

                textBoxOrderNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderNo"].ToString();
                textBoxOrderStatus.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderStatus"].ToString();
                if (!String.IsNullOrEmpty(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderDate"].ToString()))
                {
                    string[] DateOrder = (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderDate"].ToString()).Split(' ');
                    string[] DateOrderrr = DateOrder[0].Split('/');
                    DateTime OrderDD = DateTime.Parse(DateOrderrr[0] + "/" + DateOrderrr[1] + "/" + DateOrderrr[2]);
                    HSRPAuthDate.SelectedDate = OrderDD;
                    CalendarHSRPAuthDate.SelectedDate = OrderDD;
                    CalendarHSRPAuthDate.VisibleDate = OrderDD;
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
                    DropDownListVehicleClass.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString();
                }
                textBoxVehicleMake.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ManufacturerName"].ToString();
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
                }
                if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString().Equals("Transport"))
                {

                    ///>>>> if it is New Record with vehicle type Transport
                    if (String.IsNullOrWhiteSpace(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ISFrontPlateSize"].ToString()) && String.IsNullOrWhiteSpace(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ISRearPlateSize"].ToString()))
                    {
                        if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("NB")||dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("OB")||dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("DB"))
                        {
                            if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("SCOOTER")||dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MOTOR CYCLE")||dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("TRACTOR"))
                            {
                                checkBoxThirdSticker.Checked = false;
                                UpdatePanelThirdSticker.Update();
                            }

                            else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("THREE WHEELER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV(CLASS)") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MCV/HCV/TRAILERS"))
                            {
                                checkBoxThirdSticker.Checked = true;
                                UpdatePanelThirdSticker.Update();
                            }

                            checkBoxFrontPlate.Checked = true;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            checkBoxRearPlate.Checked = true;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindFrontPlateSizeDropDownListTransport();
                            DropDownListFrontPlateSize.Enabled = true;
                            UpdatePanelDropDownListFrontPlate.Update();
                            BindRearPlateSizeDropDownListTransport();
                            DropDownListRearPlateSize.Enabled = true;
                            UpdatePanelDropDownListRearPlate.Update();
                        }

                        else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("DF"))
                        {
                            if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("SCOOTER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MOTOR CYCLE") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("TRACTOR"))
                            {
                                checkBoxThirdSticker.Checked = false;
                                UpdatePanelThirdSticker.Update();
                            }

                            else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("THREE WHEELER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV(CLASS)") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MCV/HCV/TRAILERS"))
                            {
                                checkBoxThirdSticker.Checked = true;
                                UpdatePanelThirdSticker.Update();
                            }
                            checkBoxFrontPlate.Checked = true;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            checkBoxRearPlate.Checked = false;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindFrontPlateSizeDropDownListTransport();
                            DropDownListFrontPlateSize.Enabled = true;
                            UpdatePanelDropDownListFrontPlate.Update();
                            BindRearPlateSizeDropDownListTransport();
                            DropDownListRearPlateSize.Enabled = false;
                            UpdatePanelDropDownListRearPlate.Update();
                        }

                        else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("DR"))
                        {
                            if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("SCOOTER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MOTOR CYCLE") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("TRACTOR"))
                            {
                                checkBoxThirdSticker.Checked = false;
                                UpdatePanelThirdSticker.Update();
                            }

                            else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("THREE WHEELER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV(CLASS)") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MCV/HCV/TRAILERS"))
                            {
                                checkBoxThirdSticker.Checked = true;
                                UpdatePanelThirdSticker.Update();
                            }
                            checkBoxFrontPlate.Checked = true;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            checkBoxRearPlate.Checked = false;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindFrontPlateSizeDropDownListTransport();
                            DropDownListFrontPlateSize.Enabled = false;
                            UpdatePanelDropDownListFrontPlate.Update();
                            BindRearPlateSizeDropDownListTransport();
                            DropDownListRearPlateSize.Enabled = true;
                            UpdatePanelDropDownListRearPlate.Update();
                        }
                        else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("OS"))
                        {
                            if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("SCOOTER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MOTOR CYCLE") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("TRACTOR"))
                            {
                                checkBoxThirdSticker.Checked = false;
                                UpdatePanelThirdSticker.Update();
                            }

                            else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("THREE WHEELER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV(CLASS)") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MCV/HCV/TRAILERS"))
                            {
                                checkBoxThirdSticker.Checked = true;
                                UpdatePanelThirdSticker.Update();
                            }
                            checkBoxFrontPlate.Checked = false;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            checkBoxRearPlate.Checked = false;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindFrontPlateSizeDropDownListTransport();
                            DropDownListFrontPlateSize.Enabled = false;
                            UpdatePanelDropDownListFrontPlate.Update();
                            BindRearPlateSizeDropDownListTransport();
                            DropDownListRearPlateSize.Enabled = false;
                            UpdatePanelDropDownListRearPlate.Update();



                            // Financial Information Counting
                            FinancialInfoForStickerOnly();
                        }
                    }

                  else
                    {

                        if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ISFrontPlateSize"].ToString().Equals("Y"))
                        {
                            checkBoxFrontPlate.Checked = true;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            BindFrontPlateSizeDropDownListTransport();
                            DropDownListFrontPlateSize.SelectedValue= dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["FrontPlateSize"].ToString();
                            UpdatePanelDropDownListFrontPlate.Update();
                        }
                        if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ISRearPlateSize"].ToString().Equals("Y"))
                        {
                            checkBoxRearPlate.Checked = true;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindRearPlateSizeDropDownListTransport();
                            DropDownListRearPlateSize.SelectedValue = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["RearPlateSize"].ToString();
                            UpdatePanelDropDownListRearPlate.Update();
                        }
                    }
                }
               else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleClass"].ToString().Equals("Non-Transport"))
                {

                    ///>>>> if it is New Record with vehicle type Non-Transport
                    if (String.IsNullOrWhiteSpace(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ISFrontPlateSize"].ToString()) && String.IsNullOrWhiteSpace(dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ISRearPlateSize"].ToString()))
                    {
                        if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("NB") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("OB") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("DB"))
                        {
                            if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("SCOOTER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MOTOR CYCLE") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("TRACTOR"))
                            {
                                checkBoxThirdSticker.Checked = false;
                                UpdatePanelThirdSticker.Update();
                            }

                            else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("THREE WHEELER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV(CLASS)") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MCV/HCV/TRAILERS"))
                            {
                                checkBoxThirdSticker.Checked = true;
                                UpdatePanelThirdSticker.Update();
                            }

                            checkBoxFrontPlate.Checked = true;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            checkBoxRearPlate.Checked = true;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindFrontPlateSizeDropDownListNonTransport();
                            DropDownListFrontPlateSize.Enabled = true;
                            UpdatePanelDropDownListFrontPlate.Update();
                            BindRearPlateSizeDropDownListNonTransport();
                            DropDownListRearPlateSize.Enabled = true;
                            UpdatePanelDropDownListRearPlate.Update();
                        }

                        else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("DF"))
                        {
                            if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("SCOOTER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MOTOR CYCLE") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("TRACTOR"))
                            {
                                checkBoxThirdSticker.Checked = false;
                                UpdatePanelThirdSticker.Update();
                            }

                            else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("THREE WHEELER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV(CLASS)") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MCV/HCV/TRAILERS"))
                            {
                                checkBoxThirdSticker.Checked = true;
                                UpdatePanelThirdSticker.Update();
                            }
                            checkBoxFrontPlate.Checked = true;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            checkBoxRearPlate.Checked = false;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindFrontPlateSizeDropDownListNonTransport();
                            DropDownListFrontPlateSize.Enabled = true;
                            UpdatePanelDropDownListFrontPlate.Update();
                            BindRearPlateSizeDropDownListNonTransport();
                            DropDownListRearPlateSize.Enabled = false;
                            UpdatePanelDropDownListRearPlate.Update();
                        }

                        else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("DR"))
                        {
                             if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("SCOOTER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MOTOR CYCLE") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("TRACTOR"))
                            {
                                checkBoxThirdSticker.Checked = false;
                                UpdatePanelThirdSticker.Update();
                            }

                             else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("THREE WHEELER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV(CLASS)") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MCV/HCV/TRAILERS"))
                            {
                                checkBoxThirdSticker.Checked = true;
                                UpdatePanelThirdSticker.Update();
                            }
                            checkBoxFrontPlate.Checked = true;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            checkBoxRearPlate.Checked = false;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindFrontPlateSizeDropDownListNonTransport();
                            DropDownListFrontPlateSize.Enabled = false;
                            UpdatePanelDropDownListFrontPlate.Update();
                            BindRearPlateSizeDropDownListNonTransport();
                            DropDownListRearPlateSize.Enabled = true;
                            UpdatePanelDropDownListRearPlate.Update();
                        }
                        else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["OrderType"].ToString().Equals("OS"))
                        {
                            if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("SCOOTER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MOTOR CYCLE") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("TRACTOR"))
                            {
                                checkBoxThirdSticker.Checked = false;
                                UpdatePanelThirdSticker.Update();
                            }

                            else if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("THREE WHEELER") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("LMV(CLASS)") || dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VehicleType"].ToString().Equals("MCV/HCV/TRAILERS"))
                            {
                                checkBoxThirdSticker.Checked = true;
                                UpdatePanelThirdSticker.Update();
                            }
                            checkBoxFrontPlate.Checked = false;
                            UpdatePanelCheckBoxFrontPlate.Update();
                            checkBoxRearPlate.Checked = false;
                            UpdatePanelCheckBoxRearPlate.Update();
                            BindFrontPlateSizeDropDownListNonTransport();
                            DropDownListFrontPlateSize.Enabled = false;
                            UpdatePanelDropDownListFrontPlate.Update();
                            BindRearPlateSizeDropDownListNonTransport();
                            DropDownListRearPlateSize.Enabled = false;
                            UpdatePanelDropDownListRearPlate.Update();



                            // Financial Information Counting
                            FinancialInfoForStickerOnly();
                        }
                    }


                    if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ISFrontPlateSize"].ToString().Equals("Y"))
                    {
                        checkBoxFrontPlate.Checked = true;
                        UpdatePanelCheckBoxFrontPlate.Update();
                        BindFrontPlateSizeDropDownListNonTransport();
                        DropDownListFrontPlateSize.SelectedValue= dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["FrontPlateSize"].ToString();
                        UpdatePanelDropDownListFrontPlate.Update();
                    }
                    if (dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ISRearPlateSize"].ToString().Equals("Y"))
                    {
                        checkBoxRearPlate.Checked = true;
                        UpdatePanelCheckBoxRearPlate.Update();
                        BindRearPlateSizeDropDownListNonTransport();
                        DropDownListRearPlateSize.SelectedValue= dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["RearPlateSize"].ToString();
                        UpdatePanelDropDownListRearPlate.Update();
                    }
                }
                if( dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["StickerMandatory"].ToString().Equals("Y"))
                {
                    checkBoxThirdSticker.Checked = true;
                    UpdatePanelThirdSticker.Update();
                }
                //>>>>> Financial Info
                textBoxInvoiceNo.Text= dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["InvoiceNo"].ToString();
                textBoxCashReceiptNo.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["CashReceiptNo"].ToString();
                textBoxVat.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VAT_Percentage"].ToString();
                textBoxVatAmount.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["VAT_Amount"].ToString();
                textBoxServiceTax.Text= dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ServiceTax_Percentage"].ToString();
                textboxServiceTaxAmount.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["ServiceTax_Amount"].ToString();
                textBoxTotalAmount.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["TotalAmount"].ToString();
                textBoxNetTotal.Text = dataSetFillHSRPRecordDetail.Tables[0].Rows[0]["NetAmount"].ToString();
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

            String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            //>> Authorization Info
            AuthorizationNo = textBoxAuthorizationNo.Text.Trim().Replace("'", "''").ToString();
            if (StringMode.Equals("New"))
            {
                SQLString = "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as OrderNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID ="+RTOLocationID+" and PrefixFor = 'Order No'";

                StringOrderNo = Utils.getDataSingleValue(SQLString,CnnString,"OrderNo");
            }
            else if (StringMode.Equals("Edit"))
            {
                StringOrderNo = textBoxOrderNo.Text.Trim().Replace("'", "''").ToString();
            }

            if (StringMode.Equals("New"))
            {
                SQLString = "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as DeliveryChalanNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID =" + RTOLocationID + " and PrefixFor = 'Delivery Chalan'";

                StringDeliverChallanNo = Utils.getDataSingleValue(SQLString, CnnString, "DeliveryChalanNo");
            }
            else if (StringMode.Equals("Edit"))
            {
                StringDeliverChallanNo = textBoxOrderNo.Text.Trim().Replace("'", "''").ToString();
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
		            lblErrMess.Text = "please select Correct vehicle model.";
                    UpdatePanelMessage.Update();
                    return;		 
	            }
            StringVehicleMake = textBoxVehicleMake.Text.Trim().Replace("'", "''").ToString();
            StringVehicleModel = DropDownListVehicleModel.SelectedItem.Value;
            
                if (StringVehicleModel.Equals("--Select Vehicle Model--"))
	            {
		            lblErrMess.Text = "please select Correct vehicle model.";
                    UpdatePanelMessage.Update();
                    return;		 
	            }
            StringVehicleRegNo = TextBoxVehicleRegNo.Text.Trim().Replace("'", "''").ToString();
            StringEngineNo= textboxEngineNo.Text.Trim().Replace("'", "''").ToString();
            StringChassisNo = textBoxChassisNo.Text.Trim().Replace("'", "''").ToString();
            //>>> Number Plate Info

            
            StringOrderType = DropDownListOrderType.SelectedItem.Value;

            if (StringOrderType.Equals("--Select Order Type--"))
	            {
		            lblErrMess.Text = "please select Correct Order Type.";
                    UpdatePanelMessage.Update();
                    return;		 
	            }

                if (checkBoxFrontPlate.Checked)
                {
                   StringFrontPlateSize= DropDownListFrontPlateSize.SelectedItem.Value;
                    if (StringFrontPlateSize.Equals("--Select Front Plate--"))
	                    {
                            lblErrMess.Text = "please select Correct Front Plate Size.";
                            UpdatePanelMessage.Update();
                            return;
                         //string script = "<script type=\"text/javascript\">  alert('please select Correct Front Plate Size..'); return false;</script>";
                         //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", script);
                         //return;
	                    }

                    IScheckBoxFrontPlate="Y";
                }
                else
	            {
                   IScheckBoxFrontPlate="N";
	            }

                if (checkBoxRearPlate.Checked)
                {
                   StringRearPlateSize= DropDownListRearPlateSize.SelectedItem.Value;
                    if (StringRearPlateSize.Equals("--Select Rear Plate--"))
	                    {
                         lblErrMess.Text = "please select Correct Rear Plate Size.";
                         UpdatePanelMessage.Update();
                         return;		 
	                    }

                    IScheckBoxRearPlate="Y";
                }
                else
	            {
                   IScheckBoxRearPlate="N";
	            }
                
                if (checkBoxThirdSticker.Checked)
                {
                    ISThirdStickerExists = "Y";
                }
                else
                {
                    ISThirdStickerExists = "N";
                }
                
            //>>>> Financial Info
            if (StringMode.Equals("New"))
            {
                SQLString = "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as InvoiceNo  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID =" + RTOLocationID + " and PrefixFor = 'Invoice No'";

                StringOrderNo = Utils.getDataSingleValue(SQLString, CnnString, "InvoiceNo");
            }
            else if (StringMode.Equals("Edit"))
            {
                StringInvoiceNo = textBoxInvoiceNo.Text.Trim().Replace("'", "''").ToString();
            }

            if (StringMode.Equals("New"))
            {
                SQLString = "SELECT PrefixText+RIGHT('00000000'+convert(varchar,(LastNo+1)), 8) as CashReceipt  from Prefix where HSRP_StateID =" + HSRPStateID + " and RTOLocationID =" + RTOLocationID + " and PrefixFor = 'Cash Receipt'";

                StringOrderNo = Utils.getDataSingleValue(SQLString, CnnString, "CashReceipt");
                SQLString = "Update Prefix Set LastNo=LastNo+1";
                Utils.ExecNonQuery(SQLString, CnnString);
            }
            else if (StringMode.Equals("Edit"))
            {
                StringCashReceiptNo = textBoxCashReceiptNo.Text.Trim().Replace("'", "''").ToString();
            }
            StringTotalAmount = textBoxTotalAmount.Text.Trim().Replace("'", "''").ToString();
            StringVATPercentage = textBoxVat.Text.Trim().Replace("'", "''").ToString();
            StringVATAmount = textBoxVatAmount.Text.Trim().Replace("'", "''").ToString();
            StringServiceTaxPercentage = textBoxServiceTax.Text.Trim().Replace("'", "''").ToString();
            StringServiceTaxAmount = textboxServiceTaxAmount.Text.Trim().Replace("'", "''").ToString();
            StringNetTotal = textBoxNetTotal.Text.Trim().Replace("'", "''").ToString();

            lblErrMess.Text = String.Empty;
            lblSucMess.Text = String.Empty;

            BAL obj = new BAL();
            //if (obj.InsertHSRPRecords(AuthorizationNo, AuthorizationDate, StringOrderNo,
            //   StringCustomerName, StringOrderStatus, StringAddress1, StringAddress2, StringMobileNo, StringLandline,
            //   StringEmailId, StringVehicleClass, StringVehicleMake, StringVehicleModel, StringVehicleRegNo, StringEngineNo,
            //   StringChassisNo, StringOrderType, IScheckBoxFrontPlate, StringFrontPlateSize, IScheckBoxRearPlate,
            //   StringRearPlateSize, ISThirdStickerExists, StringInvoiceNo, StringCashReceiptNo, StringVATPercentage,
            //   StringVATAmount, StringServiceTaxPercentage, StringServiceTaxAmount, StringTotalAmount, StringNetTotal, 
            //   HSRPStateID, RTOLocationID,StringDeliverChallanNo,StringFixingCharge, StringFrontPlatePrize, StringRearPlatePrize,
            //   StringStickerPrize, StringScrewPrize))
            //{
            //    SQLString = "Update HSRPRecordsStaggingArea Set OrderStatus='Processed' Where HSRPRecord_AuthorizationNo='" + AuthorizationNo + "'";
            //    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //    Utils.ExecNonQuery(SQLString, CnnString);
            //    lblSucMess.Text = "Record Added Sucessfully.";
            //    BlankFields();
            //   // string script = "<script type=\"text/javascript\">  alert('Record Save Successfully.');</script>";
            //   // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                
            //}
            //else
            //{
            //    lblErrMess.Text = "Record not Added.";
            //}
        }

        #region DropDown
        /// <summary>
        /// for Commercial Front Plate Size
        /// </summary>
        private void BindFrontPlateSizeDropDownListTransport()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownListFrontPlateSize, "Select ProductCode,ProductID From Product Where ProductColor='YELLOW' order by ProductCode desc", CnnString, "--Select Front Plate--");
            DropDownListFrontPlateSize.SelectedIndex = 0;// DropDownListFrontPlateSize.Items.Count - 1;
            UpdatePanelDropDownListFrontPlate.Update();
        }
        /// <summary>
        /// for Commercial Front Plate Size
        /// </summary>
        private void BindRearPlateSizeDropDownListTransport()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownListRearPlateSize, "Select ProductCode,ProductID From Product Where ProductColor='YELLOW' order by ProductCode desc", CnnString, "--Select Rear Plate--");
            DropDownListRearPlateSize.SelectedIndex = 0;// DropDownListFrontPlateSize.Items.Count - 1;
            UpdatePanelDropDownListRearPlate.Update();
        }
        /// <summary>
        ///  for Front Plate Size Non Commercial
        /// </summary>
        private void BindFrontPlateSizeDropDownListNonTransport()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select ProductCode,ProductID From Product Where ProductColor='WHITE' order by ProductCode desc";
            Utils.PopulateDropDownList(DropDownListFrontPlateSize, SQLString, CnnString, "--Select Front Plate--");
            DropDownListFrontPlateSize.SelectedIndex = 0;// DropDownListFrontPlateSize.Items.Count - 1;
            UpdatePanelDropDownListFrontPlate.Update();
        }
        /// <summary>
        ///  for Rear Plate Size Non Commercial
        /// </summary>
        private void BindRearPlateSizeDropDownListNonTransport()
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownListRearPlateSize, "Select ProductCode,ProductID From Product Where ProductColor='WHITE' order by ProductCode desc", CnnString, "--Select Rear Plate--");
            DropDownListRearPlateSize.SelectedIndex = 0;// DropDownListFrontPlateSize.Items.Count - 1;
            UpdatePanelDropDownListRearPlate.Update();
        }
        /// <summary>
        /// Vehicle class Selection Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownListVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListVehicleClass.SelectedItem.Value.ToUpper().Equals("--Select Vehicle Class--"))
            {
                lblErrMess.Text = "Please Select Correct Vehicle Class";
                UpdatePanelMessage.Update();
                textBoxVehicleMake.TabIndex = 11;
                return;

            }
            else if (DropDownListVehicleClass.SelectedItem.Value.Equals("Transport"))
            {
                BindFrontPlateSizeDropDownListTransport();
                UpdatePanelDropDownListFrontPlate.Update();
                BindRearPlateSizeDropDownListTransport();
                UpdatePanelDropDownListRearPlate.Update();
                textBoxVehicleMake.TabIndex = 11;
            }

            else if (DropDownListVehicleClass.SelectedItem.Value.Equals("Non-Transport"))
            {
                BindFrontPlateSizeDropDownListNonTransport();
                UpdatePanelDropDownListFrontPlate.Update();
                BindRearPlateSizeDropDownListNonTransport();
                UpdatePanelDropDownListRearPlate.Update();
                textBoxVehicleMake.TabIndex = 11;
            }
        }
        /// <summary>
        /// Order Type Selection Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownListOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBoxThirdSticker.Checked = true;


            if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("--Select Order Type--"))
            {
                checkBoxRearPlate.Checked = false;
                checkBoxRearPlate.Enabled = false;
                UpdatePanelCheckBoxRearPlate.Update();
                checkBoxFrontPlate.Checked = false;
                checkBoxFrontPlate.Enabled = false;
                UpdatePanelCheckBoxFrontPlate.Update();

                DropDownListFrontPlateSize.Enabled = false;
                DropDownListFrontPlateSize.SelectedIndex = 0;
                UpdatePanelDropDownListFrontPlate.Update();
                DropDownListRearPlateSize.Enabled = false;
                DropDownListRearPlateSize.SelectedIndex = 0;
                UpdatePanelDropDownListRearPlate.Update();

                checkBoxThirdSticker.Checked = false;
                UpdatePanelThirdSticker.Update();
                lblErrMess.Text = "Please select Correct Order type.";
                UpdatePanelMessage.Update();
                TextBoxVehicleRegNo.TabIndex=13;
                return;
            }

            
                else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("NB"))
            {
                checkBoxRearPlate.Checked = true;
                checkBoxRearPlate.Enabled = false;
                UpdatePanelCheckBoxRearPlate.Update();
                checkBoxFrontPlate.Checked = true;
                checkBoxFrontPlate.Enabled = false;
                UpdatePanelCheckBoxFrontPlate.Update();

                DropDownListFrontPlateSize.Enabled = true;
                DropDownListFrontPlateSize.SelectedIndex = 0;
                UpdatePanelDropDownListFrontPlate.Update();
                DropDownListRearPlateSize.Enabled = true;
                DropDownListRearPlateSize.SelectedIndex = 0;
                UpdatePanelDropDownListRearPlate.Update();

                checkBoxThirdSticker.Checked = true;
                UpdatePanelThirdSticker.Update();
               
            }

            else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("OB"))
            {
                checkBoxRearPlate.Checked = true;
                checkBoxRearPlate.Enabled = false;
                UpdatePanelCheckBoxRearPlate.Update();
                checkBoxFrontPlate.Checked = true;
                checkBoxFrontPlate.Enabled = false;
                UpdatePanelCheckBoxFrontPlate.Update();

                DropDownListFrontPlateSize.Enabled = true;
                UpdatePanelDropDownListFrontPlate.Update();
                DropDownListRearPlateSize.Enabled = true;
                UpdatePanelDropDownListRearPlate.Update();

                checkBoxThirdSticker.Checked = true;
                UpdatePanelThirdSticker.Update();
               
            }

            else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("DB"))
            {
                checkBoxRearPlate.Checked = true;
                checkBoxRearPlate.Enabled = false;
                UpdatePanelCheckBoxRearPlate.Update();
                checkBoxFrontPlate.Checked = true;
                checkBoxFrontPlate.Enabled = false;
                UpdatePanelCheckBoxFrontPlate.Update();

                DropDownListFrontPlateSize.Enabled = true;
                UpdatePanelDropDownListFrontPlate.Update();
                DropDownListRearPlateSize.Enabled = true;
                UpdatePanelDropDownListRearPlate.Update();

                checkBoxThirdSticker.Checked = true;
                UpdatePanelThirdSticker.Update();
               
            }

            else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("DF"))
            {
                checkBoxRearPlate.Checked = false;
                checkBoxRearPlate.Enabled = false;
                UpdatePanelCheckBoxRearPlate.Update();
                checkBoxFrontPlate.Checked = true;
                checkBoxFrontPlate.Enabled = false;
                UpdatePanelCheckBoxFrontPlate.Update();

                DropDownListFrontPlateSize.Enabled = true;
                UpdatePanelDropDownListFrontPlate.Update();
                DropDownListRearPlateSize.Enabled = false;
                UpdatePanelDropDownListRearPlate.Update();

                checkBoxThirdSticker.Checked = true;
                UpdatePanelThirdSticker.Update();
               
            }
            else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("DR"))
            {
                checkBoxRearPlate.Checked = true;
                checkBoxRearPlate.Enabled = false;
                UpdatePanelCheckBoxRearPlate.Update();
                checkBoxFrontPlate.Checked = false;
                checkBoxFrontPlate.Enabled = false;
                UpdatePanelCheckBoxFrontPlate.Update();

                DropDownListFrontPlateSize.Enabled = false;
                UpdatePanelDropDownListFrontPlate.Update();
                DropDownListRearPlateSize.Enabled = true;
                UpdatePanelDropDownListRearPlate.Update();

                checkBoxThirdSticker.Checked = true;
                UpdatePanelThirdSticker.Update();
               
            }

            else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("OS"))
            {
                checkBoxThirdSticker.Checked = true;
                checkBoxRearPlate.Checked = false;
                checkBoxRearPlate.Enabled = false;
                UpdatePanelCheckBoxRearPlate.Update();
                checkBoxFrontPlate.Checked = false;
                checkBoxFrontPlate.Enabled = false;
                UpdatePanelCheckBoxFrontPlate.Update();

                DropDownListFrontPlateSize.Enabled = false;
                UpdatePanelDropDownListFrontPlate.Update();
                DropDownListRearPlateSize.Enabled = false;
                UpdatePanelDropDownListRearPlate.Update();

                checkBoxThirdSticker.Checked = true;
                UpdatePanelThirdSticker.Update();

                // Financial Information Counting
               
                FinancialInfoForStickerOnly();
                

            }

            DropDownListFrontPlateSize.SelectedIndex = 0;
            DropDownListRearPlateSize.SelectedIndex = 0;
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
           
        }
        /// <summary>
        /// Front Plate Selection Change For Financial Calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownListFrontPlateSize_SelectedIndexChanged(object sender, EventArgs e)
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

            if (checkBoxFrontPlate.Checked.Equals(false))
            {
                return;
            }
            String SQLString = string.Empty;

             if (checkBoxFrontPlate.Checked)
            {
                if (DropDownListFrontPlateSize.SelectedItem.Value.Equals("--Select Front Plate--"))
                {
                    lblErrMess.Text = "Plesae Select Correct Front Plate Size";
                    UpdatePanelMessage.Update();
                    return;
                }
                else
                {
                    FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
                    RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

                    SQLString = "Select Cost From ProductCost Where ProductID=" + FrontPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
                    Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
                }
            }
            if (checkBoxRearPlate.Checked)
            {
                if (!DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
                {
                    FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
                    RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

                    SQLString = "Select Cost From ProductCost Where ProductID=" + FrontPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
                    Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
                    SQLString = "Select Cost From ProductCost Where ProductID=" + RearPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
                    Cost = Cost + Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
                }
                else
                {
                    if (!DropDownListFrontPlateSize.SelectedItem.Value.Equals("--Select Front Plate--"))
                    {
                        FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
                        RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

                        SQLString = "Select Cost From ProductCost Where ProductID=" + FrontPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
                        Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
                    }
                    else
                    {
                        lblErrMess.Text = "Please Select Correct Front plate Size.";
                        UpdatePanelMessage.Update();
                        return;
                    }
                }
            }


           

            ///>>>Sticker Cost
            if (checkBoxThirdSticker.Checked)
            {
                SQLString = "Select Cost From ProductCost Where ProductID=9 and HSRP_StateID='" + HSRPStateID + "'";
                Sticker = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
            }

            //>>>> Snap Lock
            SQLString = "Select Cost From ProductCost Where ProductID=10 and HSRP_StateID='" + HSRPStateID + "'";
            SnapLock = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
            
            Cost += SnapLock + Sticker;
            textBoxTotalAmount.Text = Cost.ToString();
            UpdatePanelTotalAmount.Update();


            //>>vat part
            SQLString = "Select TaxPercentage From Tax Where TaxName='VAT' and HSRP_StateID='" + HSRPStateID + "'";
            VatPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxVat.Text = VatPercentage.ToString();
            UpdatePanelVat.Update();

            textBoxVatAmount.Text = ((Cost * VatPercentage) / 100).ToString();
            UpdatePanelVatAmount.Update();

            ///>>> Excise Part
            SQLString = "Select TaxPercentage From Tax Where TaxName='SERVICE TAX' and HSRP_StateID='" + HSRPStateID + "'";
            ServiceTaxPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxServiceTax.Text = ServiceTaxPercentage.ToString();
            UpdatePanelServiceTax.Update();

            textboxServiceTaxAmount.Text = ((Cost * ServiceTaxPercentage) / 100).ToString();
            UpdatePanelServiceTax.Update();

            //>>>>> Discount
            //if (DropDownListVehicleModel.SelectedItem.Value.Equals("TRACTOR"))
            //{
            //SQLString = "Select DiscountAmount From Discount Where VehicleType='TRACTOR'";
            //Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
            //}
            //SQLString = "Select Cost From Discount Where ProductID=10";
            //Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));

            NetTotal = Cost + ((Cost * VatPercentage) / 100) + ((Cost * ServiceTaxPercentage) / 100) + Discount;
            textBoxNetTotal.Text = NetTotal.ToString();
            UpdatePanelNetTotal.Update();

        }
        /// <summary>
        /// Rear Plate Selection Change For Financial Calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownListRearPlateSize_SelectedIndexChanged(object sender, EventArgs e)
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

            if (checkBoxRearPlate.Checked.Equals(false))
            {
                return;
            }
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            String SQLString = string.Empty;
            if (checkBoxRearPlate.Checked)
            {
                if (DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
                {
                    lblErrMess.Text = "Plesae Select Correct Rear Plate Size";
                    UpdatePanelMessage.Update();
                    return;
                }
                else
                {
                    FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
                    RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

                    SQLString = "Select Cost From ProductCost Where ProductID=" + RearPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
                    Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
                }
            }
            
            if (checkBoxFrontPlate.Checked)
            {
                if (!DropDownListFrontPlateSize.SelectedItem.Value.Equals("--Select Front Plate--"))
                {
                    FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
                    RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

                    SQLString = "Select Cost From ProductCost Where ProductID=" + FrontPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
                    Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
                    SQLString = "Select Cost From ProductCost Where ProductID=" + RearPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
                    Cost = Cost + Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
                }
                else
                {
                    if (!DropDownListRearPlateSize.SelectedItem.Value.Equals("--Select Rear Plate--"))
                    {
                        FrontPlateSizeProductID = DropDownListFrontPlateSize.SelectedValue;
                        RearPlateSizeProductID = DropDownListRearPlateSize.SelectedValue;

                        SQLString = "Select Cost From ProductCost Where ProductID=" + FrontPlateSizeProductID + " and HSRP_StateID='" + HSRPStateID + "'";
                        Cost = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
                    }
                    else
                    {
                        lblErrMess.Text = "Please Select Correct Rear plate Size.";
                        UpdatePanelMessage.Update();
                        return;
                    }
                }
            }


          

            ///>>>Sticker Cost
            if (checkBoxThirdSticker.Checked)
            {
                SQLString = "Select Cost From ProductCost Where ProductID=9 and HSRP_StateID='" + HSRPStateID + "'";
                Sticker = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));
            }

            //>>>> Snap Lock
            SQLString = "Select Cost From ProductCost Where ProductID=10 and HSRP_StateID='" + HSRPStateID + "'";
            SnapLock = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "Cost"));

            Cost += Sticker + SnapLock;
            
            textBoxTotalAmount.Text = Cost.ToString();
            UpdatePanelTotalAmount.Update();

            //>>vat part
            SQLString = "Select TaxPercentage From Tax Where TaxName='VAT' and HSRP_StateID='" + HSRPStateID + "'";
            VatPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxVat.Text = VatPercentage.ToString();
            UpdatePanelVat.Update();

            textBoxVatAmount.Text = ((Cost * VatPercentage) / 100).ToString();
            UpdatePanelVatAmount.Update();
            
           
            ///>>> Excise Part
            SQLString = "Select TaxPercentage From Tax Where TaxName='SERVICE TAX' and HSRP_StateID='" + HSRPStateID + "'";
            ServiceTaxPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxServiceTax.Text = ServiceTaxPercentage.ToString();
            UpdatePanelServiceTax.Update();

            textboxServiceTaxAmount.Text = ((Cost * ServiceTaxPercentage) / 100).ToString();
            UpdatePanelServiceTax.Update();

           
            //>>>>> Discount
            if (checkBoxFrontPlate.Checked && checkBoxRearPlate.Checked)
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
                else if (DropDownListVehicleModel.SelectedItem.Value.Equals("SCOOTER"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='SCOOTER' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }
                else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV"))
                {
                    SQLString = "Select DiscountAmount From Discount Where VehicleType='LMV' and HSRP_StateID='" + HSRPStateID + "'";
                    Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));
                }
            }


            NetTotal = Cost + ((Cost * VatPercentage) / 100) + ((Cost * ServiceTaxPercentage) / 100) +Discount;
            textBoxNetTotal.Text = NetTotal.ToString();
            UpdatePanelNetTotal.Update();
            if (DropDownListVehicleModel.SelectedItem.Value.Equals("MOTOR CYCLE"))
            {
                Cost += Discount;
            }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV(CLASS)"))
            {
                Cost += Discount;
            }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("SCOOTER"))
            {
                Cost += Discount;
            }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV"))
            {
                Cost += Discount;
            }
            textBoxTotalAmount.Text = Cost.ToString();
            UpdatePanelTotalAmount.Update();

        }
        /// <summary>
        /// Vehicle Model Selection Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownListVehicleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListVehicleModel.SelectedItem.Value.Equals("SCOOTER")) 
            {
                checkBoxThirdSticker.Checked = false;
                checkBoxThirdSticker.Enabled = false;
                UpdatePanelThirdSticker.Update();
            }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("MOTOR CYCLE"))
	        {
                checkBoxThirdSticker.Checked = false;
                checkBoxThirdSticker.Enabled = false;
                UpdatePanelThirdSticker.Update();		 
	        }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("TRACTOR"))
            {
                checkBoxThirdSticker.Checked = false;
                checkBoxThirdSticker.Enabled = false;
                UpdatePanelThirdSticker.Update();		 
            }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("THREE WHEELER"))
            {
                checkBoxThirdSticker.Checked = true;
                checkBoxThirdSticker.Enabled = false;
                UpdatePanelThirdSticker.Update();
            }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV"))
            {
                checkBoxThirdSticker.Checked = true;
                checkBoxThirdSticker.Enabled = false;
                UpdatePanelThirdSticker.Update();
            }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("LMV(CLASS)"))
            {
                checkBoxThirdSticker.Checked = true;
                checkBoxThirdSticker.Enabled = false;
                UpdatePanelThirdSticker.Update();
            }
            else if (DropDownListVehicleModel.SelectedItem.Value.Equals("MCV/HCV/TRAILERS"))
            {
                checkBoxThirdSticker.Checked = true;
                checkBoxThirdSticker.Enabled = false;
                UpdatePanelThirdSticker.Update();
            }
            if (DropDownListVehicleClass.SelectedItem.Text != "--Select Vehicle Class--" && DropDownListVehicleClass.SelectedItem.Text != "")
            {
                DropDownListFrontPlateSize.SelectedIndex = 0;
                DropDownListRearPlateSize.SelectedIndex = 0;
                DropDownListOrderType.SelectedIndex = 0;
            }
            textBoxTotalAmount.Text = String.Empty;
            textBoxNetTotal.Text = String.Empty;
            textBoxVat.Text = String.Empty;
            textboxServiceTaxAmount.Text = String.Empty;
            textBoxVatAmount.Text = String.Empty;
            textBoxServiceTax.Text = String.Empty;
        }

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

            //>>vat part
            SQLString = "Select TaxPercentage From Tax Where TaxName='VAT' and HSRP_StateID='" + HSRPStateID + "'";
            VatPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxVat.Text = VatPercentage.ToString();
            UpdatePanelVat.Update();

            textBoxVatAmount.Text = ((Cost * VatPercentage) / 100).ToString();
            UpdatePanelVatAmount.Update();

            ///>>> Excise Part
            SQLString = "Select TaxPercentage From Tax Where TaxName='SERVICE TAX' and HSRP_StateID='" + HSRPStateID + "'";
            ServiceTaxPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxServiceTax.Text = ServiceTaxPercentage.ToString();
            UpdatePanelServiceTax.Update();

            textboxServiceTaxAmount.Text = ((Cost * ServiceTaxPercentage) / 100).ToString();
            UpdatePanelServiceTax.Update();

            ///>>>Sticker Cost
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
    }

    public static class Alert
    {

        /// <summary> 
        /// Shows a client-side JavaScript alert in the browser. 
        /// </summary> 
        /// <param name="message">The message to appear in the alert.</param> 
        public static void Show(string message)
        {
            // Cleans the message to allow single quotation marks 
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page 
            Page page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page 
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
            }
        }
    }

}