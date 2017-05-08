using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DataProvider;

namespace HSRP.Transaction
{
    public partial class HSRPRecordsVehicleTypeEdit : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string HSRPStateID = string.Empty;
        DataTable dtgetDetail = new DataTable();
        string DropDownListFrontPlateSize = string.Empty;
        string DropDownListRearPlateSize = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;
                lblmsg.Text = string.Empty;
                lblmsg.Visible = false;

                if (!IsPostBack)
                {
                    try
                    {
                        td_Vehicle.Visible = false;
                        lnkModifyVehicle.Visible = false;
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Visible = true; 
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;

                if (String.IsNullOrEmpty(txtVehicleNo.Text))
                {

                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please Enter Vehicle Registration No.";
                    return;
                }

                HSRPStateID = Session["UserHSRPStateID"].ToString();
                td_Vehicle.Visible = false;
                lnkModifyVehicle.Visible = false;

                txtOrderType.Text = "";
                ddlVehicleType.ClearSelection();
                ddlVehicleType.SelectedIndex = 0;

                dtgetDetail = Utils.GetDataTable(" GetVehicleDetails '" + HSRPStateID + "','" + txtVehicleNo.Text + "'", CnnString);
                if (dtgetDetail.Rows.Count <= 0)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Vehicle not found";
                    return;
                }
                else if (dtgetDetail.Columns.Count == 1)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = dtgetDetail.Rows[0]["msg"].ToString();
                    return;
                }
                else if (dtgetDetail.Columns.Count > 1)
                {
                    td_Vehicle.Visible = true;
                    lnkModifyVehicle.Visible = true;
                    btnGo.Visible = false;

                    HiddenRecordid.Value = dtgetDetail.Rows[0]["HSRPRecordID"].ToString();
                    txtOrderType.Text = dtgetDetail.Rows[0]["OrderType"].ToString();
                    txtVehicleClass.Text = dtgetDetail.Rows[0]["VehicleClass"].ToString();

                    if (String.IsNullOrEmpty(dtgetDetail.Rows[0]["VehicleType"].ToString()))
                    {
                        ddlVehicleType.SelectedIndex = 0;
                    }
                    else
                        ddlVehicleType.SelectedValue = dtgetDetail.Rows[0]["VehicleType"].ToString().ToUpper();

                    txtVehicleNo.Enabled = false;
                    fillCostWithBindType();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fillCostWithBindType()
        {
            if (String.IsNullOrEmpty(txtVehicleClass.Text))
            {
                return;
            }
            if (ddlVehicleType.SelectedValue.Equals("--Select Vehicle Type--"))
            {               
                return;
            }
            if (String.IsNullOrEmpty(txtOrderType.Text))
            {               
                return;
            }
            OrderSelectionChange();
        
        
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;
                lblmsg.Text = string.Empty;
                lblmsg.Visible = false;

                if (string.IsNullOrEmpty(txtOrderType.Text))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Order Type should not be blank";
                    return;
                }
                if (string.IsNullOrEmpty(txtVehicleClass.Text))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Vehicle Class should not be blank";
                    return;
                }
                if (ddlVehicleType.SelectedItem.Value.Equals("--Select Vehicle Type--"))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "please select Correct vehicle Type.";
                    return;
                }
                if (string.IsNullOrEmpty(txtapprovedBy.Text))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please Enter Approved By";
                    return;
                }
                //if ((!Regex.Match(txtapprovedBy.Text.ToString().Trim(), @"^[a-zA-Z\s\.]{1,100}$", RegexOptions.None).Success))
                //{
                //    lblErrMsg.Visible = true;
                //    lblErrMsg.Text = "Please enter vaild Name of Approved by";
                //    return;
                //}

                if (string.IsNullOrEmpty(txtRemarks.Text))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please Enter Remarks";
                    return;
                }
                //if ((!Regex.Match(txtRemarks.Text.ToString().Trim(), @"^[a-zA-Z0-9\@\!\$\&amp;\^\?\*\/\+\,\-\.\:\;\{\}\|\~\(\)\=\ \`\[\]\\\%]{1,250}$", RegexOptions.None).Success))
                //{
                //    lblErrMsg.Visible = true;
                //    lblErrMsg.Text = "Please enter vaild Remarks!";
                //    return;
                //}                

                string strUserID = string.Empty;
                strUserID = Session["UID"].ToString();
                string RTOLocationID = string.Empty;
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                DataTable dt = new DataTable();
                dt = GetTable(HSRPStateID, ddlVehicleType.SelectedValue, txtVehicleClass.Text, txtOrderType.Text);
                int i = Utils.ExecNonQuery(" UpdateVehicleType1 '" + HSRPStateID + "','" + RTOLocationID + "','" + HiddenRecordid.Value.ToString() + "', '" + txtVehicleNo.Text + "','" + txtOrderType.Text + "','" + txtVehicleClass.Text + "','" + ddlVehicleType.SelectedValue + "','" + txtapprovedBy.Text + "','" + txtRemarks.Text + "','" + Session["UID"].ToString() + "','" + dt.Rows[0]["FrontPlateID"].ToString() + "','" + dt.Rows[0]["RearPlateID"].ToString() + "','" + dt.Rows[0]["stickerflag"].ToString() + "','" + dt.Rows[0]["totalamount"].ToString() + "','" + dt.Rows[0]["netamount"].ToString() + "','" + dt.Rows[0]["Frontplatecost"].ToString() + "','" + dt.Rows[0]["rearplatecost"].ToString() + "','" + dt.Rows[0]["stickercost"].ToString() + "'", CnnString);
                if (i > 0)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Record saved successfully";
                    return;
                }
                else
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Record not saved";
                    return;
                }
                //UpdateData();

                //dtgetDetail = Utils.GetDataTable(" UpdateVehicleType '" + HSRPStateID + "','" + RTOLocationID + "','" + HiddenRecordid.Value.ToString() + "','" + txtVehicleNo.Text + "','" + txtOrderType.Text + "','" + txtVehicleClass.Text + "','" + ddlVehicleType.SelectedValue + "','" + txtapprovedBy.Text + "','" + txtRemarks.Text + "','" + strUserID + "'", CnnString);
                //if (dtgetDetail.Rows.Count <= 0)
                //{
                //    lblErrMsg.Visible = true;
                //    lblErrMsg.Text = "Vehicle not found";
                //    return;
                //}
                //else if (dtgetDetail.Columns.Count == 1)
                //{
                //    lblErrMsg.Visible = true;
                //    lblErrMsg.Text = dtgetDetail.Rows[0]["msg"].ToString();
                //    return;
                //}
                //else if (dtgetDetail.Columns.Count > 1)
                //{
                //    lblmsg.Visible = true;
                //    lblmsg.Text = dtgetDetail.Rows[0]["msg"].ToString();
                //    return;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lnkModifyVehicle_Click(object sender, EventArgs e)
        {
            lnkModifyVehicle.Visible = false;
            btnGo.Visible = true;
            txtVehicleNo.Enabled = true;
            td_Vehicle.Visible = false;
            ddlVehicleType.SelectedIndex = 0;
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;
            lblmsg.Text = string.Empty;
            lblmsg.Visible = false;
        }

        protected void ddlVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtVehicleClass.Text))
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "No Vehicle Class found.";
                return;
            }
            if (ddlVehicleType.SelectedValue.Equals("--Select Vehicle Type--"))
            {
                lblErrMsg.Visible = true; 
                lblErrMsg.Text = "please select correct Vehicle Type.";               
                return;
            }
            if (String.IsNullOrEmpty(txtOrderType.Text))
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "No order type found.";
                checkBoxThirdSticker.Checked = false;
                checkBoxSnapLock.Checked = false;
                return;
            }
           // OrderSelectionChange();
        }

        private void OrderSelectionChange()
        {
            labelFrontPlateSize.Text = String.Empty;
            labelRearPlateSize.Text = String.Empty;
            if (ddlVehicleType.SelectedValue.Equals("THREE WHEELER") || ddlVehicleType.SelectedValue.Equals("LMV") || ddlVehicleType.SelectedValue.Equals("LMV(CLASS)") || ddlVehicleType.SelectedValue.Equals("MCV/HCV/TRAILERS"))
            {
                checkBoxThirdSticker.Checked = true;
            }
            else
            {
                checkBoxThirdSticker.Checked = false;
            }

            textBoxTotalAmount.Text = string.Empty;
            textBoxVat.Text = string.Empty;
            textBoxVatAmount.Text = string.Empty;
            textBoxServiceTax.Text = string.Empty;
            textboxServiceTaxAmount.Text = string.Empty;
            textBoxNetTotal.Text = string.Empty;            

            Double Cost = 0.00;
            Double VatPercentage = 0.00;
            Double ServiceTaxPercentage = 0.00;
            Double SnapLock = 0.00;
            Double Sticker = 0.00;
            Double Discount = 0.00;
            Double NetTotal = 0.00;
            string SQLString = string.Empty;            

            SQLString = "Select FrontPlateID,RearPlateID,StickerID,OrderType From RegistrationPlateDetail Where HSRP_StateID=" + HSRPStateID + " and VehicleClass='" + txtVehicleClass.Text.ToString() + "' and VehicleType='" + ddlVehicleType.SelectedValue + "' and OrderType='" + txtOrderType.Text.ToString() + "'";
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
                DropDownListFrontPlateSize = dt.Rows[0]["FrontPlateID"].ToString();
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                labelFrontPlateSize.Text = "Front Plate :- " + Utils.getScalarValue(SQLString, CnnString);                

                SQLString = " Select Cost from ProductCost Where HSRP_StateID=" + HSRPStateID + " and ProductID=" + dt.Rows[0]["FrontPlateID"].ToString() + "";
                HiddenFieldFrontPlate.Text = Utils.getDataSingleValue(SQLString, CnnString, "Cost");
                Cost += Double.Parse(HiddenFieldFrontPlate.Text);
            }
            if (!String.IsNullOrEmpty(dt.Rows[0]["RearPlateID"].ToString()) || !String.IsNullOrWhiteSpace(dt.Rows[0]["RearPlateID"].ToString()))
            {
                SQLString = " Select ProductCode from Product Where ProductID=" + dt.Rows[0]["RearPlateID"].ToString() + "";
                HiddenFieldRearPlateCode.Text = dt.Rows[0]["RearPlateID"].ToString();
                DropDownListRearPlateSize = dt.Rows[0]["RearPlateID"].ToString();
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                labelRearPlateSize.Text = "Rear Plate :- " + Utils.getScalarValue(SQLString, CnnString);
                
                SQLString = " Select Cost from ProductCost Where HSRP_StateID=" + HSRPStateID + " and ProductID=" + dt.Rows[0]["RearPlateID"].ToString() + "";
                HiddenFieldRearPlate.Text = Utils.getDataSingleValue(SQLString, CnnString, "Cost");
                Cost += Double.Parse(HiddenFieldRearPlate.Text);
            }

            if (txtOrderType.Text.ToString().Equals("OS"))
            {
                checkBoxThirdSticker.Checked = true;
                checkBoxSnapLock.Checked = false;
            }
            else
            {
                checkBoxSnapLock.Checked = true;
            }            

            if (dt.Rows[0]["OrderType"].ToString().Equals("OS"))
            {
                               
            }
            else
            {
                SQLString = " Select ProductCost from Product Where HSRP_StateID=" + HSRPStateID + " and ProductID in (Select ProductID From Product Where ProductCode='SNAP LOCK')";
                HiddenFieldScrew.Text = Utils.getDataSingleValue(SQLString, CnnString, "ProductCost");
                Cost += Double.Parse(HiddenFieldScrew.Text);
            }

            /// >>>>>>>>>>Vat Part
            SQLString = "Select TaxPercentage From Tax Where TaxName='VAT' and HSRP_StateID='" + HSRPStateID + "'";
            VatPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxVat.Text = VatPercentage.ToString();           

            textBoxVatAmount.Text = Math.Round(Convert.ToDecimal(((Cost * VatPercentage) / 100).ToString()), 2).ToString();
            

            //>>> Discount
            if (txtOrderType.Text.ToString().Equals("NB") || txtOrderType.Text.ToString().Equals("OB") || txtOrderType.Text.ToString().Equals("DB"))
            {
                SQLString = "Select DiscountAmount From Discount Where VehicleType='" + ddlVehicleType.SelectedItem.Value + "' and HSRP_StateID='" + HSRPStateID + "'";
                Discount = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "DiscountAmount"));                
            }

            Cost += Discount;
            NetTotal = Cost + ((Cost * VatPercentage) / 100) + ((Cost * ServiceTaxPercentage) / 100);
            textBoxNetTotal.Text = Math.Round(Convert.ToDecimal(NetTotal.ToString()), 2).ToString();           

            textBoxVatAmount.Text = Math.Round(Convert.ToDecimal(((Cost * VatPercentage) / 100).ToString()), 2).ToString(); 

            ///>>> Excise Part
            SQLString = "Select TaxPercentage From Tax Where TaxName='SERVICE TAX' and HSRP_StateID='" + HSRPStateID + "'";
            ServiceTaxPercentage = Convert.ToDouble(Utils.getDataSingleValue(SQLString, CnnString, "TaxPercentage"));

            textBoxServiceTax.Text = ServiceTaxPercentage.ToString();            

            textboxServiceTaxAmount.Text = ((Cost * ServiceTaxPercentage) / 100).ToString();           

            //>>>>> Discount
            textBoxTotalAmount.Text = Math.Round(Convert.ToDecimal(Cost.ToString()), 2).ToString();            
        }

        public void UpdateData()
        {            
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            //>>> Vehicle Info 
            string StringVehicleType = string.Empty;
            string StringVehicleRegNo = string.Empty;
            string StringOrderType = string.Empty;
            string IScheckBoxFrontPlate = string.Empty;
            string IScheckBoxRearPlate = string.Empty;
            string ISThirdStickerExists = string.Empty;
            string RTOLocationID = string.Empty;
            int UID;
            string StringTotalAmount = string.Empty;
            string StringVATPercentage = string.Empty;
            string StringVATAmount = string.Empty;
            string StringServiceTaxPercentage = string.Empty;
            string StringServiceTaxAmount = string.Empty;
            string StringNetTotal = string.Empty;
            string StringFixingCharge = "0";
            string StringFrontPlatePrize = string.Empty;
            string StringRearPlatePrize = string.Empty;
            string StringStickerPrize = string.Empty;
            string StringScrewPrize = string.Empty;

            string StringFrontPlateSize = HiddenFieldFrontPlateCode.Text;
            string StringRearPlateSize = HiddenFieldRearPlateCode.Text;

            StringVehicleType = ddlVehicleType.SelectedItem.Value;
            StringVehicleRegNo = txtVehicleNo.Text.Trim().Replace("'", "''").ToString();
            //>>> Number Plate Info

            StringOrderType = txtOrderType.Text;                

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
           
            lblErrMsg.Text = String.Empty;
            lblmsg.Text = String.Empty;
            
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            int.TryParse(Session["UID"].ToString(), out UID);
            
            dtgetDetail = Utils.GetDataTable(" UpdateVehicleType '" + HSRPStateID + "','" + RTOLocationID + "','" + HiddenRecordid.Value.ToString() + "', '" + StringVehicleRegNo + "','" + StringOrderType + "','" + txtVehicleClass.Text + "','" + StringVehicleType + "','" + txtapprovedBy.Text + "','" + txtRemarks.Text + "','" + UID+ "','" + IScheckBoxFrontPlate + "','" + StringFrontPlateSize + "','" + IScheckBoxRearPlate + "','" + StringRearPlateSize + "','" + ISThirdStickerExists + "','" + StringVATPercentage + "','" + StringVATAmount + "','" + StringServiceTaxPercentage + "','" + StringServiceTaxAmount + "','" + StringTotalAmount + "','" + StringNetTotal + "','" + StringFixingCharge + "','" + StringFrontPlatePrize + "','" + StringRearPlatePrize + "','" + StringStickerPrize + "','" + StringScrewPrize+"'", CnnString);
            if (dtgetDetail.Rows.Count <= 0)
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "Vehicle not found";
                return;
            }
            else if (dtgetDetail.Columns.Count == 1)
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = dtgetDetail.Rows[0]["msg"].ToString();
                return;
            }
            else if (dtgetDetail.Columns.Count > 1)
            {
                lblmsg.Visible = true;
                lblmsg.Text = dtgetDetail.Rows[0]["msg"].ToString();
                return;
            }            
        }

        public DataTable GetTable(string HSRPStateId, string Vehicletype, string VehicleClass, string TransactionType)
        {
            string sql = "exec [getPlatesData] '" + HSRPStateId + "','" + Vehicletype.ToString() + "','" + VehicleClass.ToString() + "', '" + TransactionType.ToString() + "'";
            DataTable dt = Utils.GetDataTable(sql, CnnString);
            return dt;
        }
    }
}