using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;
using System.Net;
using System.IO;
using System.Text;

namespace HSRP.Master
{
    public partial class CreateTSDealer : System.Web.UI.Page
    {
        int intHSRPStateID = 0;
        string SQLString1 = string.Empty;
        string StringDealerName = String.Empty;
        string StringDealerCode = String.Empty;
        string StringPersonName = String.Empty;
        string StringMobileNo = String.Empty;
        string StringAddress = String.Empty;
        string StringCity = String.Empty;
        string StringState = String.Empty;
        string StringAreaDealer = String.Empty;
        string StringrtolocationId = String.Empty;
        String StringMode = String.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string HsrpStateId = string.Empty;
        int UserType;
        protected void Page_Load(object sender, EventArgs e)
        {
            HsrpStateId = Session["UserHSRPStateID"].ToString();
            UserType = Convert.ToInt32(Session["UserType"]);
            Utils.GZipEncodePage();
            lblSucMess.Text = "";
            lblErrMess.Text = "";
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!Page.IsPostBack)
            {
                BL.blEntryDetail bl = new BL.blEntryDetail();
                bl.DailyEnityDeatail(Session["UID"].ToString(), "Dealer.aspx");
                FilldropDownListRTOLocation();
                FilldropDownListDealer();
            }
        }

        private void FilldropDownListRTOLocation()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where  HSRP_StateID=" + HsrpStateId + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
            }
            else
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HsrpStateId + " and ActiveStatus!='N' Order by RTOLocationName";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                DropdownRTOName.DataSource = dss;
                DropdownRTOName.DataTextField = "RTOLocationName";
                DropdownRTOName.DataValueField = "RTOLocationID";
                DropdownRTOName.DataBind();
                DropdownRTOName.Items.Insert(0, new ListItem("--Select RTO Location--", "--Select RTO Location--"));
            }
        }

        private void FilldropDownListDealer()
        {
            SQLString = "Select Dealerid,dealername from dealermaster where HSRP_StateID='" + HsrpStateId + "'";
            DataSet dss = Utils.getDataSet(SQLString, CnnString);
            ddldealerList.DataSource = dss;
            ddldealerList.DataTextField = "dealername";
            ddldealerList.DataValueField = "Dealerid";
            ddldealerList.DataBind();
            ddldealerList.Items.Insert(0, new ListItem("--Select Dealer Name--", "--Select Dealer Name--"));
        }

        private void UpdateUserDetail(int DealerID)
        {
            SQLString = "Select * From DealerMaster Where DealerID=" + DealerID.ToString();
            Utils dbLink = new Utils();
            dbLink.strProvider = CnnString;
            dbLink.CommandTimeOut = 600;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader PReader = dbLink.GetReader();
            while (PReader.Read())
            {          
                txtCompanyNamewithAddress.Text = PReader["dealername"].ToString();
                txtContactPersonName.Text = PReader["ContactPerson"].ToString();
                txtDesignation.Text = PReader["Designation"].ToString();
                txtContactPersonMobileNo.Text = PReader["ContactMobileNo"].ToString();
                txtRegisteredAddress.Text = PReader["Address"].ToString();
                txtDistrict.Text = PReader["City"].ToString();
                txtState.Text = PReader["State"].ToString();
                txtPinCode.Text = PReader["pincode"].ToString();
                txtTelephoneNumbers.Text = PReader["telephonenumber"].ToString();
                txtFaxNos.Text = PReader["fax"].ToString();
                txtCellPhoneNo.Text = PReader["cellphoneno"].ToString();
                txtemail.Text = PReader["emailid"].ToString();
                if (PReader["rtolocationid"].ToString() != "")
                    DropdownRTOName.SelectedValue = PReader["rtolocationid"].ToString();                
                txtNameofOEM.Text = PReader["OemName"].ToString();
                txtIncomeTaxPANNo.Text = PReader["PanNo"].ToString();
                txtCSTNo.Text = PReader["CstNo"].ToString();
                txtVATTINNo.Text = PReader["VatTinNo"].ToString();
                txtBusinessentity.Text = PReader["BusinessType"].ToString();
                txtFulldetails.Text = PReader["AreaOfDealer"].ToString();
                txtTypeofDealer.Text = PReader["DealerType"].ToString();
            }
            PReader.Close();
            dbLink.CloseConnection();
        }
        public static string dealerid;
        protected void ddldealerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUserDetail(Convert.ToInt32(ddldealerList.SelectedValue));
            dealerid = ddldealerList.SelectedValue;
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            if (ddldealerList.SelectedItem.ToString() == "--Select Dealer Name--")
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select Exist Dealer.";
                ddldealerList.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtCompanyNamewithAddress.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Company Name with Address.";
                txtCompanyNamewithAddress.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtContactPersonName.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Contact Person Name.";
                txtContactPersonName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtDesignation.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Designation.";
                txtDesignation.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtContactPersonMobileNo.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Contact Person Mobile No.";
                txtContactPersonMobileNo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtRegisteredAddress.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Registered Address.";
                txtRegisteredAddress.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtDistrict.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide District.";
                txtDistrict.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtState.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide State.";
                txtState.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtPinCode.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide PinCode.";
                txtPinCode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtTelephoneNumbers.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Telephone Numbers.";
                txtTelephoneNumbers.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtFaxNos.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide FaxNos.";
                txtFaxNos.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtCellPhoneNo.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Cell Phone No.";
                txtCellPhoneNo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtemail.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Email-Id.";
                txtemail.Focus();
                return;
            }
            if (DropdownRTOName.SelectedValue == "--Select RTO Location--")
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select Rto Location.";
                DropdownRTOName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtNameofOEM.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Name of OEM.";
                txtNameofOEM.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtIncomeTaxPANNo.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Income Tax PAN No.";
                txtIncomeTaxPANNo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtCSTNo.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide CST No.";
                txtCSTNo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtVATTINNo.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide VAT TIN No.";
                txtVATTINNo.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtBusinessentity.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Type of Business entity.";
                txtBusinessentity.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtFulldetails.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Full details.";
                txtFulldetails.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtTypeofDealer.Text.ToString()))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Provide Type of Dealer.";
                txtTypeofDealer.Focus();
                return;
            }
            string sqlquery = "UpdateTGDealer '"+dealerid+"','"+txtCompanyNamewithAddress.Text+"','"+txtRegisteredAddress.Text+"','"+txtDistrict.Text+"','"+txtState.Text+"','"+txtContactPersonName.Text+"','"+txtContactPersonMobileNo.Text+"','"+txtFulldetails.Text+"','"+DropdownRTOName.SelectedValue+"','"+txtTypeofDealer.Text+"','"+txtNameofOEM.Text+"','"+txtIncomeTaxPANNo.Text+"','"+txtCSTNo.Text+"','"+txtVATTINNo.Text+"','"+txtBusinessentity.Text+"','"+txtDesignation.Text+"','"+txtPinCode.Text+"','"+txtTelephoneNumbers.Text+"','"+txtFaxNos.Text+"','"+txtCellPhoneNo.Text+"','"+txtemail.Text+"'";
            int i=Utils.ExecNonQuery(sqlquery, CnnString);
            if (i > 0)
            {
                lblSucMess.Visible = true;
                lblSucMess.Text = "";
                lblSucMess.Text = "Record updated successfully";
                lblErrMess.Visible = false;
                lblErrMess.Text = "";
            }
            else
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Record Not Updated";
                lblSucMess.Visible = false;
                lblSucMess.Text = "";
            }



        }

        public void reset()
        {
            ddldealerList.ClearSelection();
            txtCompanyNamewithAddress.Text = "";
            txtContactPersonName.Text = "";
            txtDesignation.Text = "";
            txtContactPersonMobileNo.Text = "";
            txtRegisteredAddress.Text = "";
            txtDistrict.Text = "";
            txtState.Text = "";
            txtPinCode.Text = "";
            txtTelephoneNumbers.Text = "";
            txtFaxNos.Text = "";
            txtCellPhoneNo.Text = "";
            txtemail.Text = "";
            DropdownRTOName.ClearSelection();
            txtNameofOEM.Text = "";
            txtIncomeTaxPANNo.Text = "";
            txtCSTNo.Text = "";
            txtVATTINNo.Text = "";
            txtBusinessentity.Text = "";
            txtFulldetails.Text = "";
            txtTypeofDealer.Text = "";
            lblErrMess.Text = "";
            lblSucMess.Text="";
            dealerid = "";
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }
    }
}