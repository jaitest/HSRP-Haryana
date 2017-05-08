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
using System.Text;
using System.Web.UI.HtmlControls;
using System.Net;


namespace HSRP.Transaction
{
    public partial class CashReceiptDataEntry : System.Web.UI.Page
    {

        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
        string SQLString = string.Empty;
        String StringMode = string.Empty;
        float NetAmountTax;
        string sqlstring1 = string.Empty;
        DataTable dt2;
        string CurrentDate = DateTime.Now.ToString();
        DateTime OrderDate1;
        DataTable dt1 = new DataTable();
        DataTable dtz = new DataTable();
        BAL obj = new BAL();
        DataTable dataSetFillDetails = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Panel2.Visible = true;
            //Panel3.Visible = false;
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
            AutoCompleteExtender1.ContextKey =  HSRPStateID;

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
                //VehicleModel();
                DropDownListVehicleModel.Items.Clear();
                DropDownListVehicleModel.Items.Add("--Select Vehicle Type--");
                VehicleClass();
                OrderType();
                FillVehicleModel();
                //btnDownloadReceipt.Visible = false;

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


            //Returns a string that can be used in a client event to cause postback to the server.
            //ClientScript.GetPostBackEventReference(this, string.Empty);

            //if (IsPostBack)
            //{
            //    string eventTarget = Request["__EVENTTARGET"] ?? string.Empty;
            //    string eventArgument = Request["__EVENTARGUMENT"] ?? string.Empty;

            //    switch (eventTarget)
            //    {
            //        case "MyConfirmationPostBackEventTarget":
            //            if (Convert.ToBoolean(eventArgument))
            //            {
            //                AfterUserConfirmationHandler();
            //            }
            //            break;
            //    }
            //}

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

        protected void DropDownListVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            VehicleModel();
            OrderType();
            txtAmount.Text = "";
        }
        private void GetRateForVehicle()
        {
            HiddenFieldRearPlatePrice.Value = "0";
            HiddenFieldFrontPlatePrice.Value = "0";
            hiddenfieldIsFrontPlate.Value = "";
            hiddenfieldIsRearPlate.Value = "";


            txtAmount.Text = "";
            SQLString = "select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue + "') as Amount";
            DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
            txtAmount.Text = dt.Rows[0]["Amount"].ToString();
            SQLString = "select FrontPlateID,RearPlateID,StickerID from RegistrationPlateDetail  where hsrp_stateid ='" + HSRPStateID + "' and vehicletype='" + DropDownListVehicleModel.SelectedItem.Text + "' and vehicleclass='" + DropDownListVehicleClass.SelectedItem.Text + "' and ordertype='" + DropDownListOrderType.SelectedValue + "'";
            DataTable dtPlateSize = Utils.GetDataTable(SQLString, ConnectionString);
            if (dtPlateSize.Rows.Count > 0)
            {
                if (dtPlateSize.Rows[0]["FrontPlateID"].ToString() != "")
                {
                    Utils.PopulateDropDownList(DropDownListFrontPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                    DropDownListFrontPlate.SelectedIndex = DropDownListFrontPlate.Items.Count - 1;
                    sqlstring1 = "select cost from ProductCost where productid = '" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "' and hsrp_stateid ='" + HSRPStateID + "'";
                    dt2 = Utils.GetDataTable(sqlstring1, ConnectionString);
                    if (dt2.Rows.Count > 0)
                    {
                        HiddenFieldFrontPlatePrice.Value = dt2.Rows[0]["Cost"].ToString();
                        hiddenfieldIsFrontPlate.Value = "Y";
                    }
                    else
                    {
                        HiddenFieldFrontPlatePrice.Value = "0";
                    }
                }
                if (dtPlateSize.Rows[0]["RearPlateID"].ToString() != "")
                {
                    Utils.PopulateDropDownList(DropDownListRearPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["RearPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");

                    DropDownListRearPlate.SelectedIndex = DropDownListRearPlate.Items.Count - 1;
                    sqlstring1 = "Select cost from productCost where productid='" + dtPlateSize.Rows[0]["RearPlateID"].ToString() + "' and hsrp_stateid='" + HSRPStateID + "'";
                    dt2 = Utils.GetDataTable(sqlstring1, ConnectionString);
                    if (dt2.Rows.Count > 0)
                    {
                        HiddenFieldRearPlatePrice.Value = dt2.Rows[0]["Cost"].ToString();
                        hiddenfieldIsRearPlate.Value = "Y";
                    }
                    else
                    {
                        HiddenFieldRearPlatePrice.Value = "0";
                    }
                }
                sqlstring1 = "select cost from ProductCost where productid ='" + dtPlateSize.Rows[0]["StickerID"].ToString() + "'  and hsrp_stateid ='" + HSRPStateID + "'";
                dt = Utils.GetDataTable(sqlstring1, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    HiddenFieldStickerID.Value = dt.Rows[0]["Cost"].ToString();
                }
                else
                {
                    HiddenFieldStickerID.Value = "0";
                }

                //screwrate
                sqlstring1 = "select productcost from Product where productcode='SNAP LOCK' and hsrp_stateid ='" + HSRPStateID + "'";
                dt = Utils.GetDataTable(sqlstring1, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    HiddenFieldscrewrate.Value = dt.Rows[0]["ProductCost"].ToString();
                }
                else
                {
                    HiddenFieldscrewrate.Value = "0";
                }
                sqlstring1 = "select discountamount from Discount where hsrp_stateid ='" + HSRPStateID + "' and vehicletype='" + DropDownListVehicleModel.SelectedItem.Text + "' and vehicleclass='" + DropDownListVehicleClass.SelectedItem.Text + "'";
                dt = Utils.GetDataTable(sqlstring1, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    hiddenfieldDiscount.Value = dt.Rows[0]["discountamount"].ToString();
                }
                else
                {
                    hiddenfieldDiscount.Value = "0";
                }
                sqlstring1 = " select taxpercentage from tax where hsrp_stateid ='" + HSRPStateID + "' and taxpercentage > 0";
                dt = Utils.GetDataTable(sqlstring1, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    hiddenfieldTax.Value = dt.Rows[0]["taxpercentage"].ToString();
                }
                else
                {
                    hiddenfieldTax.Value = "0";
                }
                float strnetamount1;
                //if (HiddenFieldFrontPlatePrice.Value == "")
                //{
                //    strnetamount1 = float.Parse(HiddenFieldRearPlatePrice.Value) + float.Parse(HiddenFieldscrewrate.Value) + float.Parse(HiddenFieldStickerID.Value) + float.Parse(hiddenfieldDiscount.Value);
                //}
                //else if (HiddenFieldRearPlatePrice.Value == "")
                //{
                //    strnetamount1 = float.Parse(HiddenFieldFrontPlatePrice.Value) + float.Parse(HiddenFieldscrewrate.Value) + float.Parse(HiddenFieldStickerID.Value) + float.Parse(hiddenfieldDiscount.Value);
                //}
                //else

                strnetamount1 = float.Parse(HiddenFieldFrontPlatePrice.Value) + float.Parse(HiddenFieldRearPlatePrice.Value) + float.Parse(HiddenFieldscrewrate.Value) + float.Parse(HiddenFieldStickerID.Value) + float.Parse(hiddenfieldDiscount.Value);

                hiddenfieldTotalAmount.Value = strnetamount1.ToString();
                NetAmountTax = (strnetamount1 / 100);
                NetAmountTax = NetAmountTax * (float.Parse(hiddenfieldTax.Value));


                // hiddenfieldTax.Value=NetAmountTax.ToString();
                decimal vatamount = Math.Round(Convert.ToDecimal(NetAmountTax), 2);
                hiddenfieldVATAMOUNT.Value = vatamount.ToString();

                float netAmount = (NetAmountTax) + (strnetamount1);
                Decimal NetAmountDecimal = Math.Round(Convert.ToDecimal(netAmount), 2);
                hiddenfieldNetAmount.Value = Math.Round(netAmount).ToString();

            }

        }
        public void FillVehicleModel()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(DropDownVehicleModel1, "Select VehicleModelID,VehicleModelDescription From VehicleModelMaster order by VehicleModelDescription", ConnectionString, "--Select Vehicle Model--");
            DropDownVehicleModel1.SelectedIndex = 0;
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
            //checkBoxThirdSticker.Checked = true;
            //if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("--Select Order Type--"))
            //{
            //    checkBoxThirdSticker.Checked = false;
            //    return;
            //}


            //else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("NB"))
            //{
            //    checkBoxThirdSticker.Checked = true;

            //}

            //else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("OB"))
            //{
            //    checkBoxThirdSticker.Checked = true;

            //}

            //else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("DB"))
            //{
            //    checkBoxThirdSticker.Checked = true;
            //}

            //else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("DF"))
            //{
            //    checkBoxThirdSticker.Checked = true;

            //}
            //else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("DR"))
            //{

            //    checkBoxThirdSticker.Checked = true;

            //}

            //else if (DropDownListOrderType.SelectedItem.Value.ToUpper().Equals("OS"))
            //{
            //    checkBoxThirdSticker.Checked = true;


            //    // Financial Information Counting

            //}
            // FinancialInfoForStickerOnly();

            //txtAmount.Text = "";
            //DataTable dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue + "') as Amount", ConnectionString);

            //txtAmount.Text = dt.Rows[0]["Amount"].ToString();

           
            //SQLString = "select FrontPlateID,RearPlateID from RegistrationPlateDetail  where hsrp_stateid ='" + HSRPStateID + "' and vehicletype='" + DropDownListVehicleModel.SelectedItem.Text + "' and vehicleclass='" + DropDownListVehicleClass.SelectedItem.Text + "' and ordertype='" + DropDownListOrderType.SelectedValue + "'";
            //DataTable dtPlateSize = Utils.GetDataTable(SQLString, ConnectionString);
            //if (dtPlateSize.Rows.Count > 0)
            //{
            //    if (dtPlateSize.Rows[0]["FrontPlateID"].ToString() != "")
            //    {
            //        Utils.PopulateDropDownList(DropDownListFrontPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
            //        DropDownListFrontPlate.SelectedIndex = DropDownListFrontPlate.Items.Count - 1;
            //    }
            //    if (dtPlateSize.Rows[0]["RearPlateID"].ToString() != "")
            //    {
            //        Utils.PopulateDropDownList(DropDownListRearPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["RearPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
            //        DropDownListRearPlate.SelectedIndex = DropDownListRearPlate.Items.Count - 1;
            //    }
            //}
            GetRateForVehicle();

        }

        protected void btnDuplicate_Click(object sender, EventArgs e)
        {

        }
        string Sticker = string.Empty, Vip = string.Empty, Query = string.Empty;
        String SqlCheck=string.Empty;
        int i = 0;
        //DateTime OrderDate1;
        StringBuilder sb = new StringBuilder();
        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {

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

            SqlCheck = "select COUNT(*) from dbo.HSRPRecords where VehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "'"; //and HSRPRecord_AuthorizationNo ='" + txtAuthorizationNo.Text + "'";
            int z = Utils.getScalarCount(SqlCheck, ConnectionString);
            if (z <= 0)
            {

                updateandsaverecords();
                //string Query = "select (prefixtext+right('00000000'+ convert(varchar,lastno+1),9)) as PrifixNo,(LastNo+1)as LastNo  from prefix where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                //dt1 = Utils.GetDataTable(Query, ConnectionString);

                //lblCashReceiptNo.Text = dt1.Rows[0]["PrifixNo"].ToString();
                //lblMessageCashReceipt.Text = "Cash Receipt No.";

                //if (hiddenfieldReferenceValue.Value == "StagingAreaRecordFound")
                //{
                //    Query = "INSERT INTO HSRPRecords (HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,isVIP,NetAmount,VehicleType,OrderStatus,CashReceiptNo,OwnerFatherName, EmailID, ChassisNo, EngineNo, HSRP_Front_LaserCode, HSRP_Rear_LaserCode,CreatedBy,Reference) values(GetDate(),'" + HSRPStateID + "','" + RTOLocationID + "','" + txtAuthorizationNo.Text + "','" + OrderDate1.ToString() + "','" + txtVehicleRegNo.Text.Trim().ToUpper() + "','" + txtOwnerName.Text + "','" + txtMobileNo.Text + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue.ToString() + "','" + Sticker + "','" + Vip + "','" + txtAmount.Text + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','New Order','" + dt1.Rows[0]["PrifixNo"].ToString() + "','" + txtFatherName.Text.Trim() + "', '" + txtEmailID.Text.Trim() + "', '" + txtChassisNo.Text.Trim() + "', '" + txtEngineNo.Text.Trim() + "', '" + DropDownListFrontPlate.SelectedItem.Text.Trim() + "', '" + DropDownListRearPlate.SelectedItem.Text.Trim() + "','" + Session["UID"].ToString() + "','" + txtReference.Text + "')";
                //    i = Utils.ExecNonQuery(Query, ConnectionString);
                //    if (i > 0)
                //    {
                //        SQLString = "update HSRPRecordsStaggingArea set orderStatus='Processed' where hsrprecordStaggingID='" + hiddenfieldhsrprecordStaggingID.Value + "'";
                //        Utils.ExecNonQuery(SQLString, ConnectionString);
                //    }
                //}
                //else
                //{
                //    if (hiddenfieldReferenceValue.Value == "DelhiOldData")
                //    {

                //        Query = "INSERT INTO HSRPRecords (HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,ownerFatherName,Address1,MobileNo,VehicleClass,OrderType,StickerMandatory,isVIP,NetAmount,VehicleType,OrderStatus,CashReceiptNo,CreatedBy,ISFrontPlateSize,ISRearPlateSize,FrontPlateSize,RearPlateSize,Reference) values(GetDate(),'" + HSRPStateID + "','" + RTOLocationID + "','" + txtAuthorizationNo.Text + "','" + OrderDate1.ToString() + "','" + txtVehicleRegNo.Text.Trim().ToUpper() + "','" + txtOwnerName.Text + "','" + txtFatherName.Text + "','" + txtAddress.Text + "','" + txtMobileNo.Text + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue.ToString() + "','" + Sticker + "','" + Vip + "','" + txtAmount.Text + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','New Order','" + dt1.Rows[0]["PrifixNo"].ToString() + "','" + Session["UID"].ToString() + "','Y','Y','" + DropDownListFrontPlate.SelectedValue + "','" + DropDownListRearPlate.SelectedValue + "','" + txtReference.Text + "')";
                //        i = Utils.ExecNonQuery(Query, ConnectionString);
                //    }
                //}
                //if (i > 0)
                //{
                //    Query = "update prefix set LastNo='" + dt1.Rows[0]["LastNo"].ToString() + "' where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                //    Utils.ExecNonQuery(Query, ConnectionString);
                //    hiddenfieldAuthorizationNo.Value = txtAuthorizationNo.Text;
                //    lblSucMess.Text = "Vehicle Added Successfully.";
                //    btnDownloadReceipt.Visible = true;
                //    allEnableTrueFalse();
                //    ////DepositDate = 
                //    //txtVehicleRegNo.Text = "";
                //}
                //else
                //{
                //    lblErrMess.Text = "Vehicle Already Exist.";
                //    btnDownloadReceipt.Visible = false;
                  

                
                //}
            }
            else
            {

                DataTable dt11 = Utils.GetDataTable("select  (select rtolocationname from rtolocation where rtolocationid =a.rtolocationid) 'Location Name',vehicleregno as 'Vehicle No.',convert(varchar(20),HSRPRecord_CreationDate,101)as 'Order Booking Date',HSRPRecord_AuthorizationNo as 'Authorization No',OwnerName as 'Owner Name',vehicletype as 'Vehicle Type',ordertype as 'Order Type',orderstatus as 'Order Status' from dbo.HSRPRecords a  where VehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "' order by hsrprecord_creationdate desc", ConnectionString);
                string  newdate = "05/12/2012 00:00:00";
               // if (dt11.Rows.Count > 0)
               // {
                    
               //     sb.Append("<table width='99%' border='1px' cellpadding='2' cellspacing='1' style='border-collapse:collapse;border-color:#64AAFD'><tr class='midboxtopcolor2'>");
               //     sb.Append("<td class='heading1' Location Name : >" + dt11.Rows[0]["locationname"].ToString() + "</td>");
               //     sb.Append("<td class='heading1' Cash Recipt Date : >" + dt11.Rows[0]["HSRPRecord_CreationDate"].ToString() + "</td>");
               //     sb.Append("</tr>");
               //     sb.Append("<tr>");
               //     sb.Append("<td class='maintext' nowrap='nowrap' Owner Name>" + dt11.Rows[0]["ownername"].ToString() + "</td>");
               //     sb.Append("<td class='maintext' nowrap='nowrap' Vehicle Type >" + dt11.Rows[0]["vehicletype"].ToString() + "</td>");
               //     sb.Append("</tr>");
               //     sb.Append("<tr>");
               //     sb.Append("<td class='maintext' nowrap='nowrap' Order Status>" + dt11.Rows[0]["orderstatus"].ToString() + "</td>");
               //     sb.Append("</tr>");
               //     sb.Append("</table>");
               //     vehshow.InnerHtml = sb.ToString();

                   
               
               // }
               //ModalPopupExtender1.Show();
               lblErrMess.Text = "Vehicle Already Exist.";

               //Panel2.Visible = false;
               //Panel3.Visible = true;
                if (Convert.ToDateTime(OrderDate1) >= Convert.ToDateTime(newdate))
                {
               GridView1.DataSource = dt11;
               GridView1.DataBind();

               lblMesageSave.Text =  + z + " Record(s) Already exists.";

               //ModalPopupExtender2.Show();
               ModalPopupExtender1.Show();
            }
                else
                {
                    lblErrMess.Text = "Please enter in old vehicle option";
                }

            }


          // sb1.AppendFormat("var foo = window.confirm('Allready " + z + " Records Do you want to proceed ?');\n");
          //// sb1.AppendFormat("var foo = dhtmlwindow.open('Allready " + z + " Records Do you want to proceed ?');\n");
          // sb1.Append("if (foo)\n");
          // sb1.Append("__doPostBack('MyConfirmationPostBackEventTarget', foo);\n");
          // ClientScript.RegisterStartupScript(GetType(), "MyScriptKey", sb1.ToString(), true);

          
          
        }

       
        //protected void AfterUserConfirmationHandler()
        //{
        //    updateandsaverecords();
        //}
        DataTable dt5 = new DataTable();
        public void updateandsaverecords()
        {
           
            string Query = "select (prefixtext+right('00000000'+ convert(varchar,lastno+1),9)) as PrifixNo,(LastNo+1)as LastNo  from prefix where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
            dt5 = Utils.GetDataTable(Query, ConnectionString);

            lblCashReceiptNo.Text = dt5.Rows[0]["PrifixNo"].ToString();
            lblMessageCashReceipt.Text = "Cash Receipt No.";
            string vehiclemodel = DropDownVehicleModel1.SelectedValue.ToString();
            if (vehiclemodel == "--Select Vehicle Model--")
            {
                vehiclemodel = "";
            }

            if (hiddenfieldReferenceValue.Value == "StagingAreaRecordFound")
            {



                Query = "INSERT INTO HSRPRecords (HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,isVIP,NetAmount,VehicleType,OrderStatus,CashReceiptNo,OwnerFatherName, EmailID, ChassisNo, EngineNo, HSRP_Front_LaserCode, HSRP_Rear_LaserCode,CreatedBy,Reference,ManufacturerModel,vehicleref,FrontplatePrize,RearPlatePrize,StickerPrize,ScrewPrize,TotalAmount,VAT_Amount,RoundOff_NetAmount,VAT_Percentage) values(GetDate(),'" + HSRPStateID + "','" + RTOLocationID + "','" + txtAuthorizationNo.Text + "','" + Convert.ToDateTime(OrderDate1).ToString("MM/dd/yyyy") + "','" + txtVehicleRegNo.Text.Trim().ToUpper() + "','" + txtOwnerName.Text + "','" + txtMobileNo.Text + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue.ToString() + "','" + Sticker + "','" + Vip + "','" + txtAmount.Text + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','New Order','" + dt5.Rows[0]["PrifixNo"].ToString() + "','" + txtFatherName.Text.Trim() + "', '" + txtEmailID.Text.Trim() + "', '" + txtChassisNo.Text.Trim() + "', '" + txtEngineNo.Text.Trim() + "', '" + DropDownListFrontPlate.SelectedItem.Text.Trim() + "', '" + DropDownListRearPlate.SelectedItem.Text.Trim() + "','" + Session["UID"].ToString() + "','" + txtReference.Text + "','" + vehiclemodel + "','New','" + HiddenFieldFrontPlatePrice.Value + "','" + HiddenFieldRearPlatePrice.Value + "','" + HiddenFieldStickerID.Value + "','" + HiddenFieldscrewrate.Value + "','" + hiddenfieldTotalAmount.Value + "','" + hiddenfieldVATAMOUNT.Value + "','" + hiddenfieldNetAmount.Value + "','" + hiddenfieldTax.Value + "')";
                i = Utils.ExecNonQuery(Query, ConnectionString);
                if (i > 0)
                {
                    SQLString = "update HSRPRecordsStaggingArea set orderStatus='Processed' where hsrprecordStaggingID='" + hiddenfieldhsrprecordStaggingID.Value + "'";
                    Utils.ExecNonQuery(SQLString, ConnectionString);

                    Query = "select max(neworder)+1 as neworder from rtolocation where rtolocationid='" + RTOLocationID + "'";
                    dt1 = Utils.GetDataTable(Query, ConnectionString);
                    Query = "update rtolocation set neworder='" + dt1.Rows[0]["neworder"].ToString() + "',lastUpdateOrder=getdate() where rtolocationid='" + RTOLocationID + "'";
                    Utils.ExecNonQuery(Query, ConnectionString);
                }
            }

            else
            {

                //if (hiddenfieldReferenceValue.Value == "DelhiOldData")

                Query = "INSERT INTO HSRPRecords (HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,ownerFatherName,Address1,MobileNo,VehicleClass,OrderType,StickerMandatory,isVIP,NetAmount,VehicleType,OrderStatus,CashReceiptNo,CreatedBy,ISFrontPlateSize,ISRearPlateSize,FrontPlateSize,RearPlateSize,Reference,ManufacturerModel,vehicleref,FrontplatePrize,RearPlatePrize,StickerPrize,ScrewPrize,TotalAmount,VAT_Amount,RoundOff_NetAmount,VAT_Percentage) values(GetDate(),'" + HSRPStateID + "','" + RTOLocationID + "','" + txtAuthorizationNo.Text + "','" + Convert.ToDateTime(OrderDate1).ToString("MM/dd/yyyy") + "','" + txtVehicleRegNo.Text.Trim().ToUpper() + "','" + txtOwnerName.Text + "','" + txtFatherName.Text + "','" + txtAddress.Text + "','" + txtMobileNo.Text + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue.ToString() + "','" + Sticker + "','" + Vip + "','" + txtAmount.Text + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','New Order','" + dt5.Rows[0]["PrifixNo"].ToString() + "','" + Session["UID"].ToString() + "','"+hiddenfieldIsFrontPlate.Value+"','"+hiddenfieldIsRearPlate.Value+"','" + DropDownListFrontPlate.SelectedValue + "','" + DropDownListRearPlate.SelectedValue + "','" + txtReference.Text + "','" + vehiclemodel + "','New','" + HiddenFieldFrontPlatePrice.Value + "','" + HiddenFieldRearPlatePrice.Value + "','" + HiddenFieldStickerID.Value + "','" + HiddenFieldscrewrate.Value + "','" + hiddenfieldTotalAmount.Value + "','" + hiddenfieldVATAMOUNT.Value + "','" + hiddenfieldNetAmount.Value + "','" + hiddenfieldTax.Value + "')";
                    i = Utils.ExecNonQuery(Query, ConnectionString);

                    Query = "select max(neworder)+1 as neworder from rtolocation where rtolocationid='" + RTOLocationID + "'";
                    dt1 = Utils.GetDataTable(Query, ConnectionString);
                    Query = "update rtolocation set neworder='" + dt1.Rows[0]["neworder"].ToString() + "',lastUpdateOrder=getdate() where rtolocationid='" + RTOLocationID + "'";
                    Utils.ExecNonQuery(Query, ConnectionString);
                

            }
            
            if (i > 0)
            {
                Query = "update prefix set LastNo='" + dt5.Rows[0]["LastNo"].ToString() + "' where hsrp_stateid='" + HSRPStateID + "' and rtolocationid ='" + RTOLocationID + "' and prefixfor='Cash Receipt No'";
                Utils.ExecNonQuery(Query, ConnectionString);
                hiddenfieldAuthorizationNo.Value = txtAuthorizationNo.Text;
                lblSucMess.Text = "Vehicle Added Successfully.";
                if (txtMobileNo.Text.Length > 0)
                {

                    // string SMSText = " Cash Rs."+ Math.Round(decimal.Parse(lblAmount.Text), 0)+" received against HSRP Authorization No. "+lblAuthNo.Text+" on "+System.DateTime.Now.ToString("dd/MM/yyyy")+" receipt number "+cashrc+". HSRP Team.";
                    //sms type 0 means old data.
                    string SMSText = " Cash Rs." + hiddenfieldNetAmount.Value + " collected against Reg No. " + txtVehicleRegNo.Text + " receipt number " + lblCashReceiptNo.Text + "dated  " + System.DateTime.Now.ToString("dd/MM/yyyy") + ".HSRP TEAM";
                    string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-delhsrp&password=delhsrp&type=0&dlr=1&destination=" + txtMobileNo.Text.ToString() + "&source=DLHSRP&message=" + SMSText;
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                    myRequest.Method = "GET";
                    WebResponse myResponse = myRequest.GetResponse();
                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                    string result = sr.ReadToEnd();
                    sr.Close();
                    myResponse.Close();
                    System.Threading.Thread.Sleep(350);

                    Utils.ExecNonQuery("insert into DLSMSDetail(RtoLocationID,VehicleRegNo,MobileNo,SentResponseCode,smstext,SmsType) values('" + RTOLocationID + "','" + txtVehicleRegNo.Text.Trim().ToUpper() + "'," + txtMobileNo.Text.ToString() + ",'" + result + "','" + SMSText + "','0')", ConnectionString);
                }
                btnDownloadReceipt.Visible = true;
                allEnableTrueFalse();
                ////DepositDate = 
                //txtVehicleRegNo.Text = "";
               // Refresh()
            }
            else
            {
                lblErrMess.Text = "Vehicle Already Exist.";
                btnDownloadReceipt.Visible = false;

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

            SQLString = "select hsrprecordID, HSRPRecord_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,IsVIP,NetAmount,VehicleType,CashReceiptNo from hsrprecords where vehicleRegNo='" + txtVehicleRegNo.Text.Trim() + "' and HSRPRecord_AuthorizationNo ='" + hiddenfieldAuthorizationNo.Value + "' order by HSRPRecord_AuthorizationNo,vehicleRegNo desc";
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

                string getTinNo = Utils.getScalarValue("select TinNo from hsrpstate where HSRP_StateID='" + HSRPStateID + "' ", ConnectionString);

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

                PdfPCell cell65 = new PdfPCell(new Phrase("  ", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell65.Colspan = 1;
                cell65.BorderWidthLeft = 0f;
                cell65.BorderWidthRight = 0f;
                cell65.BorderWidthTop = 0f;
                cell65.BorderWidthBottom = 0f;
                cell65.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell65);

                PdfPCell cell26 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell26.Colspan = 1;
                cell26.BorderWidthLeft = 0f;
                cell26.BorderWidthRight = 0f;
                cell26.BorderWidthTop = 0f;
                cell26.BorderWidthBottom = 0f;
                cell26.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell26);

                DateTime Orddate = Convert.ToDateTime(dataSetFillHSRPDeliveryChallan.Rows[0]["HSRPRecord_CreationDate"].ToString());


                PdfPCell cell265 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell265.Colspan = 1;
                cell265.BorderWidthLeft = 0f;
                cell265.BorderWidthRight = 0f;
                cell265.BorderWidthTop = 0f;
                cell265.BorderWidthBottom = 0f;
                cell265.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell265);

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

        protected void buttonGo_Click(object sender, EventArgs e)
        {
            lblRecordType.Visible = false;
            divAddress.Visible = false;
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            lblOld.Text = "";
            allEnabletrue();
            btnDownloadReceipt.Visible = true;
            String SqlCheck = "SELECT HSRPRecordStaggingID, RTOLocationID, HSRP_StateID, HSRPRecord_CreationDate, HSRPRecord_AuthorizationNo, HSRPRecord_AuthorizationDate, OwnerName, OwnerFatherName, Address1, MobileNo, EmailID,StickerMandatory, OrderType,OrderStatus, VehicleClass, VehicleType, ManufacturerName, ManufacturerModel, ChassisNo, EngineNo, Manufacturer, VehicleRegNo, NetAmount FROM HSRPRecordsStaggingArea  where hsrp_StateID='" + Session["UserHSRPStateID"].ToString() + "' and vehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "'";
            dtz = Utils.GetDataTable(SqlCheck, ConnectionString);
            try
            {

                if (dtz.Rows.Count > 0)
                {
                    hiddenfieldhsrprecordStaggingID.Value = dtz.Rows[0]["HSRPRecordStaggingID"].ToString();
                    hiddenfieldReferenceValue.Value = "StagingAreaRecordFound";

                    txtOwnerName.Text = dtz.Rows[0]["OwnerName"].ToString();
                    txtAuthorizationNo.Text = dtz.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                    txtMobileNo.Text = dtz.Rows[0]["MobileNo"].ToString();
                    txtFatherName.Text = dtz.Rows[0]["OwnerFatherName"].ToString();
                    txtEmailID.Text = dtz.Rows[0]["EmailID"].ToString();
                    txtEngineNo.Text = dtz.Rows[0]["EngineNo"].ToString();
                    txtChassisNo.Text = dtz.Rows[0]["ChassisNo"].ToString();
                    txtFatherName.Text = dtz.Rows[0]["OwnerFatherName"].ToString();
                    txtEmailID.Text = dtz.Rows[0]["EmailID"].ToString();

                    DropDownListVehicleClass.SelectedItem.Text = dtz.Rows[0]["VehicleClass"].ToString();
                    DropDownListVehicleClass.SelectedValue = dtz.Rows[0]["VehicleClass"].ToString();

                    string vehtype = dtz.Rows[0]["VehicleType"].ToString();
                    //if (vehtype != "" )
                    {
                        DropDownListVehicleModel.SelectedItem.Text = dtz.Rows[0]["VehicleType"].ToString();
                        DropDownListVehicleModel.SelectedValue = dtz.Rows[0]["VehicleType"].ToString();

                        if (dtz.Rows[0]["HSRPRecord_AuthorizationDate"].ToString() != "")
                        {
                            DepositDate.SelectedDate = Convert.ToDateTime(dtz.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                        }
                        if (DropDownListVehicleModel.SelectedItem.Text == "THREE WHEELER" || DropDownListVehicleModel.SelectedItem.Text == "LMV" || DropDownListVehicleModel.SelectedItem.Text == "LMV(CLASS)" || DropDownListVehicleModel.SelectedItem.Text == "MCV/HCV/TRAILERS")
                        {
                            checkBoxThirdSticker.Checked = true;
                        }
                        else
                        {
                            checkBoxThirdSticker.Checked = false;
                        }
                    }
                    if (dtz.Rows[0]["OrderType"].ToString() != "")
                    {
                        string Status = string.Empty;
                        string orderTypes = dtz.Rows[0]["OrderType"].ToString();
                        if (orderTypes == "NB")
                        {
                            Status = "NEW BOTH PLATES";
                        }
                        if (orderTypes == "DF")
                        {
                            Status = "DAMAGED FRONT PLATE";
                        }
                        if (orderTypes == "DB")
                        {
                            Status = "DAMAGED BOTH PLATES";
                        }
                        if (orderTypes == "OB")
                        {
                            Status = "OLD BOTH PLATES";
                        }
                        if (orderTypes == "DR")
                        {
                            Status = "DAMAGED REAR PLATE";
                        }
                        if (orderTypes == "OS")
                        {
                            Status = "ONLY STICKER";
                        }
                        DropDownListOrderType.SelectedItem.Text = Status;
                        DropDownListOrderType.SelectedValue = orderTypes;
                    }

                    DataTable dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue + "') as Amount", ConnectionString);
                    txtAmount.Text = dt.Rows[0]["Amount"].ToString();

                    SQLString = "select FrontPlateID,RearPlateID from RegistrationPlateDetail  where hsrp_stateid ='" + HSRPStateID + "' and vehicletype='" + DropDownListVehicleModel.SelectedItem.Text + "' and vehicleclass='" + DropDownListVehicleClass.SelectedItem.Text + "' and ordertype='" + DropDownListOrderType.SelectedValue + "'";
                    DataTable dtPlateSize = Utils.GetDataTable(SQLString, ConnectionString);
                    //if (dtPlateSize.Rows.Count > 0)
                    //{
                    //    if (dtPlateSize.Rows[0]["FrontPlateID"].ToString() != "")
                    //    {
                    //        Utils.PopulateDropDownList(DropDownListFrontPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                    //        DropDownListFrontPlate.SelectedIndex = DropDownListFrontPlate.Items.Count - 1;
                    //    }
                    //    if (dtPlateSize.Rows[0]["RearPlateID"].ToString() != "")
                    //    {
                    //        Utils.PopulateDropDownList(DropDownListRearPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                    //        DropDownListRearPlate.SelectedIndex = DropDownListRearPlate.Items.Count - 1;
                    //    }
                    //}
                    GetRateForVehicle();
                    if (txtAuthorizationNo.Text != "")
                    {
                        txtAuthorizationNo.Enabled = false;
                    }

                    if (txtOwnerName.Text != "")
                    {
                        txtOwnerName.Enabled = false;
                    }

                    if (txtMobileNo.Text != "")
                    {
                        txtMobileNo.Enabled = false;
                    }

                    if (DropDownListVehicleClass.Text != "")
                    {
                        DropDownListVehicleClass.Enabled = false;
                    }

                    if (txtEngineNo.Text != "")
                    {
                        txtEngineNo.Enabled = false;
                    }

                    if (txtChassisNo.Text != "")
                    {
                        txtChassisNo.Enabled = false;
                    }
                    if (txtFatherName.Text != "")
                    {
                        txtFatherName.Enabled = false;
                    }
                    if (txtEmailID.Text != "")
                    {
                        txtEmailID.Enabled = false;
                    }

                    if (DropDownListVehicleModel.SelectedItem.Text != "")
                    {
                        DropDownListVehicleModel.Enabled = false;
                    }

                    if (DropDownListOrderType.SelectedItem.Text != "")
                    {
                        DropDownListOrderType.Enabled = false;
                    }

                    if (DropDownListFrontPlate.SelectedItem.ToString() != "")
                    {
                        DropDownListFrontPlate.Enabled = false;
                    }

                    if (DropDownListRearPlate.SelectedItem.Text != "")
                    {
                        DropDownListRearPlate.Enabled = false;
                    }
                    checkBoxThirdSticker.Enabled = false;
                }
                else
                {
                    //if (Session["UserHSRPStateID"].ToString() == "2")
                    //{

                    //    string vehicleRegNo = txtVehicleRegNo.Text.Trim().Trim();
                    //    obj.FillDelString(vehicleRegNo, ref dataSetFillDetails);

                    //    if (dataSetFillDetails.Rows.Count > 0)
                    //    {
                    //        lblRecordType.Visible = true;
                    //        divAddress.Visible = true;
                    //        hiddenfieldReferenceValue.Value = "DelhiOldData";
                    //        lblOld.Text = "Old Both Plate";
                    //        txtAuthorizationNo.Text = "0";
                    //        txtOwnerName.Text = dataSetFillDetails.Rows[0]["OwnerName"].ToString();
                    //        txtFatherName.Text = dataSetFillDetails.Rows[0]["OwnerFatherName"].ToString();
                    //        txtEngineNo.Text = dataSetFillDetails.Rows[0]["EngineNo"].ToString();
                    //        txtChassisNo.Text = dataSetFillDetails.Rows[0]["ChassisNo"].ToString();
                    //        txtFatherName.Text = dataSetFillDetails.Rows[0]["OwnerFatherName"].ToString();
                    //        txtOldAddress.Text = dataSetFillDetails.Rows[0]["OlaAddress"].ToString();
                    //        lblRegDate.Text = dataSetFillDetails.Rows[0]["Reg_Date"].ToString();

                    //        if (DropDownListVehicleModel.SelectedItem.Text == "THREE WHEELER" || DropDownListVehicleModel.SelectedItem.Text == "LMV" || DropDownListVehicleModel.SelectedItem.Text == "LMV(CLASS)" || DropDownListVehicleModel.SelectedItem.Text == "MCV/HCV/TRAILERS")
                    //        {
                    //            checkBoxThirdSticker.Checked = true;
                    //        }
                    //        else
                    //        {
                    //            checkBoxThirdSticker.Checked = false;
                    //        }
                    //        string Status = "OLD BOTH PLATES";

                            //if (dtz.Rows[0]["OrderType"].ToString() != "")
                            //{
                            //    string Status = string.Empty;
                            //    string orderTypes = dtz.Rows[0]["OrderType"].ToString();
                            //    if (orderTypes == "NB")
                            //    {
                            //        Status = "NEW BOTH PLATES";
                            //    }
                            //    if (orderTypes == "DF")
                            //    {
                            //        Status = "DAMAGED FRONT PLATE";
                            //    }
                            //    if (orderTypes == "DB")
                            //    {
                            //        Status = "DAMAGED BOTH PLATES";
                            //    }
                            //    if (orderTypes == "OB")
                            //    {
                            //        Status = "OLD BOTH PLATES";
                            //    }
                            //    if (orderTypes == "DR")
                            //    {
                            //        Status = "DAMAGED REAR PLATE";
                            //    }
                            //    if (orderTypes == "OS")
                            //    {
                            //        Status = "ONLY STICKER";
                            //    }
                            //    DropDownListOrderType.SelectedItem.Text = Status;
                            //    DropDownListOrderType.SelectedValue = orderTypes;
                            //}

                            //DataTable dt = Utils.GetDataTable("select dbo.hsrpplateamt ('" + HSRPStateID + "','" + DropDownListVehicleModel.SelectedItem.ToString() + "','" + DropDownListVehicleClass.SelectedItem.ToString() + "','" + DropDownListOrderType.SelectedValue + "') as Amount", ConnectionString);
                            //txtAmount.Text = dt.Rows[0]["Amount"].ToString();

                            //SQLString = "select FrontPlateID,RearPlateID from RegistrationPlateDetail  where hsrp_stateid ='" + HSRPStateID + "' and vehicletype='" + DropDownListVehicleModel.SelectedItem.Text + "' and vehicleclass='" + DropDownListVehicleClass.SelectedItem.Text + "' and ordertype='" + DropDownListOrderType.SelectedValue + "'";
                            //DataTable dtPlateSize = Utils.GetDataTable(SQLString, ConnectionString);
                            //if (dtPlateSize.Rows.Count > 0)
                            //{
                            //    if (dtPlateSize.Rows[0]["FrontPlateID"].ToString() != "")
                            //    {
                            //        Utils.PopulateDropDownList(DropDownListFrontPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                            //        DropDownListFrontPlate.SelectedIndex = DropDownListFrontPlate.Items.Count - 1;
                            //    }
                            //    if (dtPlateSize.Rows[0]["RearPlateID"].ToString() != "")
                            //    {
                            //        Utils.PopulateDropDownList(DropDownListRearPlate, "Select  ProductCode,ProductID From Product Where productID='" + dtPlateSize.Rows[0]["FrontPlateID"].ToString() + "'", ConnectionString, "--Select Front Plate--");
                            //        DropDownListRearPlate.SelectedIndex = DropDownListRearPlate.Items.Count - 1;
                            //    }
                            //}
                            //OrderTypeForOldBoth();

                            //if (txtAuthorizationNo.Text != "")
                            //{
                            //    txtAuthorizationNo.Enabled = false;
                            //}

                            //if (txtOwnerName.Text != "")
                            //{
                            //    txtOwnerName.Enabled = false;
                            //}

                            //if (txtMobileNo.Text != "")
                            //{
                            //    txtMobileNo.Enabled = false;
                            //}

                            //if (DropDownListVehicleClass.Text != "")
                            //{
                            //    DropDownListVehicleClass.Enabled = false;
                            //}

                            //if (txtEngineNo.Text != "")
                            //{
                            //    txtEngineNo.Enabled = false;
                            //}

                            //if (txtChassisNo.Text != "")
                            //{
                            //    txtChassisNo.Enabled = false;
                            //}
                            //if (txtFatherName.Text != "")
                            //{
                            //    txtFatherName.Enabled = false;
                            //}
                            //if (txtEmailID.Text != "")
                            //{
                            //    txtEmailID.Enabled = false;
                            //}

                            //if (DropDownListVehicleModel.SelectedItem.Text != "")
                            //{
                            //    DropDownListVehicleModel.Enabled = false;
                            //}

                            //if (DropDownListOrderType.SelectedItem.Text != "")
                            //{
                            //    DropDownListOrderType.Enabled = false;
                            //}

                            //if (DropDownListFrontPlate.SelectedItem.Text != "")
                            //{
                            //    DropDownListFrontPlate.Enabled = false;
                            //}

                            //if (DropDownListRearPlate.SelectedItem.Text != "")
                            //{
                            //    DropDownListRearPlate.Enabled = false;
                            //}
                        //    checkBoxThirdSticker.Enabled = false;
                        //}
                    //    else
                    //    {
                    //        lblOld.Text = "";
                    //        txtAuthorizationNo.Enabled = true;
                    //        txtOwnerName.Enabled = true;
                    //        txtMobileNo.Enabled = true;
                    //        txtEngineNo.Enabled = true;

                    //        DropDownListVehicleClass.Enabled = true;
                    //        DropDownListVehicleModel.Enabled = true;
                    //        DropDownListOrderType.Enabled = true;
                    //        allEnableTrueFalse();
                    //    }
                    //}
                    //else
                    //{

                    //    lblOld.Text = "";
                    //    txtAuthorizationNo.Enabled = true;
                    //    txtOwnerName.Enabled = true;
                    //    txtMobileNo.Enabled = true;
                    //    txtEngineNo.Enabled = true;

                    //    DropDownListVehicleClass.Enabled = true;
                    //    DropDownListVehicleModel.Enabled = true;
                    //    DropDownListOrderType.Enabled = true;
                    //    allEnableTrueFalse();

                    //}
                }
                lblSucMess.Text = "";
                lblErrMess.Text = "";
            }
            catch (Exception ex)
            {
            }
        }

        
        public void allEnableTrueFalse()
        {
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            txtAuthorizationNo.Text = "";
            txtOwnerName.Text = "";
            txtFatherName.Text = "";
            txtAddress.Text = "";
            txtEmailID.Text = "";
            txtMobileNo.Text = "";
            txtEngineNo.Text = "";
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
            if (DropDownListVehicleClass.SelectedItem.ToString() == "Non-Transport")
            {
                dr["ID"] = "SCOOTER";
                dr["Value"] = "SCOOTER";
                dtcost.Rows.Add(dr);
                dr = dtcost.NewRow();

                dr["ID"] = "MOTOR CYCLE";
                dr["Value"] = "MOTOR CYCLE";
                dtcost.Rows.Add(dr);
                dr = dtcost.NewRow();
            }
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
            dr = dtcost.NewRow();

            dr["ID"] = "E-RICKSHAW";
            dr["Value"] = "E-RICKSHAW";
            dtcost.Rows.Add(dr);
            dr = dtcost.NewRow();
            
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

            //dr["Value"] = "OLD BOTH PLATES";
            //dr["ID"] = "OB";
            //dtcost.Rows.Add(dr);
            //dr = dtcost.NewRow();


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

        protected void btnOk_Click(object sender, EventArgs e)
        {
            int NewOrederCount=0;
            String OrderStatus  = String.Empty;
            String[] StringOrderDate = DepositDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            DataTable dt11 = Utils.GetDataTable("select  (select rtolocationname from rtolocation where rtolocationid =a.rtolocationid) 'Location Name',vehicleregno as 'Vehicle No.',convert(varchar(20),HSRPRecord_CreationDate,101)as 'Order Booking Date',HSRPRecord_AuthorizationNo as 'Authorization No',OwnerName as 'Owner Name',vehicletype as 'Vehicle Type',ordertype as 'Order Type',orderstatus as 'Order Status' from dbo.HSRPRecords a  where VehicleRegNo ='" + txtVehicleRegNo.Text.Trim().Trim() + "' order by hsrprecord_creationdate desc", ConnectionString);
            for (int i = 0; i < dt11.Rows.Count; i++)
            {
                if (dt11.Rows[i]["Order Status"].ToString() == "New Order")
                {
                    OrderStatus = "New Order";
                    NewOrederCount++;
                    //Panel3.Visible = false;
                   
                }
            }
            if (OrderStatus != "New Order")
            {
                updateandsaverecords();
                Panel2.Visible = true;
                Panel3.Visible = false;
            }
            else
            {
                if ("You Have " + NewOrederCount + " New Order First Close Them" == lblMesageSave.Text)
                {
                    ModalPopupExtender1.Hide();
                }
                else
                {

                    ModalPopupExtender1.Show();
                    lblMesageSave.Text = "You Have " + NewOrederCount + " New Order First Close Them";
                }
            }
        }

       

        protected void Button2_Click(object sender, EventArgs e)
        {
            //ModalPopupExtender1.Show();
            ModalPopupExtender1.Show();
        }

    

     
    }
}