using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace HSRP.Master
{
    public partial class VendorRegistration : System.Web.UI.Page
    {

        string Mode;
        string UserType = string.Empty;
        DataProvider.BAL blVendor = new DataProvider.BAL();
        
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                string Mode = Request.QueryString["Mode"].ToString();
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }

                if (Mode == "Edit")
                {
                    btnSubmit.Visible = false;
                    btnUpdate.Visible = true;
                    VendorID.Value = Request.QueryString["VendorID"].ToString();
                    editVendordetail(VendorID.Value.ToString());
                }
                else if (Mode == "New")
                {
                    btnSubmit.Visible = true;
                    btnUpdate.Visible = false;
                }
            }
          
        }

        private void editVendordetail(string VendorID)
        {
            ds = blVendor.EditVendorDetail(VendorID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtVendorName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                txtBillingaddress.Text = ds.Tables[0].Rows[0]["Address1"].ToString();
                //txtShippingAddress.Text = ds.Tables[0].Rows[0]["Address2"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                txtState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                txtCountry.Text = ds.Tables[0].Rows[0]["Country"].ToString();
                txtPin.Text = ds.Tables[0].Rows[0]["PinNo"].ToString();
                txtContactPerson.Text = ds.Tables[0].Rows[0]["ContactPerson"].ToString();
                txtMobileNo.Text = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                txtLandlineNo.Text = ds.Tables[0].Rows[0]["LandlineNo"].ToString();
                txtEmailID.Text = ds.Tables[0].Rows[0]["EmailID"].ToString();
               
                txtTinNo.Text = ds.Tables[0].Rows[0]["TinNo"].ToString();
                txtVatNo.Text = ds.Tables[0].Rows[0]["VatNo"].ToString();
                txtCST.Text = ds.Tables[0].Rows[0]["CST"].ToString();
                txtExciseNo.Text = ds.Tables[0].Rows[0]["ExciseNo"].ToString();
                txtRemark.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                //txtBillingCity.Text = ds.Tables[0].Rows[0]["BillingCity"].ToString();
                //txtBillingState.Text = ds.Tables[0].Rows[0]["BillingState"].ToString();
                //txtBillingCountry.Text = ds.Tables[0].Rows[0]["BillingCountry"].ToString();
                //txtBillingPinNo.Text = ds.Tables[0].Rows[0]["BillingPinNo"].ToString();


                string Active = ds.Tables[0].Rows[0]["ActiveMachine"].ToString();
                if (Active=="Y")
                {
                    chkActive.Checked = true;
                }
                else
                {
                    chkActive.Checked = false;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();

            lst.Add(txtVendorName.Text);

            lst.Add(txtBillingaddress.Text);

         

            //lst.Add(txtShippingAddress.Text);
            lst.Add(txtCity.Text);
            lst.Add(txtState.Text);
            lst.Add(txtCountry.Text);
            lst.Add(txtPin.Text);
            lst.Add(txtContactPerson.Text);
           
            lst.Add(txtMobileNo.Text);
           
            lst.Add(txtLandlineNo.Text);
            lst.Add(txtEmailID.Text);
            lst.Add(txtTinNo.Text);
            lst.Add(txtVatNo.Text);
            lst.Add(txtCST.Text);
            lst.Add(txtExciseNo.Text);
            lst.Add(txtRemark.Text);
            if (chkActive.Checked == true)
            {
                lst.Add("Y");
            }
            else
            {
                lst.Add("N");
            }

            //lst.Add(txtBillingCity.Text);
            //lst.Add(txtBillingState.Text);
            //lst.Add(txtBillingCountry.Text);
            //lst.Add(txtBillingPinNo.Text);

            int i = blVendor.InsertVendorRegistration(lst);
            if (i > 0)
            {
                tblShow.Visible = true;
                lblErrorMessageBox.Text = "";
                lblMessageBox.Text = "Record Save Successfully";
                refresh();
            }
            else
            {
                tblShow.Visible = true;
                lblMessageBox.Text = "";
                lblErrorMessageBox.Text = "Record Already Exist!!";
                refresh();
            }

        }

        public void refresh()
        {
            txtVendorName.Text = "";
            txtBillingaddress.Text = "";
           // txtShippingAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtCountry.Text = "";
            txtPin.Text = "";
            txtContactPerson.Text = "";
            txtMobileNo.Text = "";
            txtEmailID.Text = "";
            txtLandlineNo.Text = "";
            txtTinNo.Text = "";
            txtVatNo.Text = "";
            txtCST.Text = "";
            txtExciseNo.Text = "";
            txtRemark.Text = "";
          // txtBillingCity.Text="";
          //txtBillingState.Text="";
          // txtBillingCountry.Text="";
          // txtBillingPinNo.Text = "";

            chkActive.Checked = false;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            txtVendorName.Text = "";
            txtBillingaddress.Text = "";
            //txtShippingAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtCountry.Text = "";
            txtPin.Text = "";
            txtContactPerson.Text = "";
            txtMobileNo.Text = "";
            txtEmailID.Text = "";
            txtLandlineNo.Text = "";
            txtTinNo.Text = "";
            txtVatNo.Text = "";
            txtCST.Text = "";
            txtExciseNo.Text = "";
            txtRemark.Text = "";
            //txtBillingCity.Text = "";
            //txtBillingState.Text = "";
            //txtBillingCountry.Text = "";
            //txtBillingPinNo.Text = "";
            chkActive.Checked = false;
            tblShow.Visible = false;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();

            lst.Add(txtVendorName.Text);
            lst.Add(txtBillingaddress.Text);
           // lst.Add(txtShippingAddress.Text);
            lst.Add(txtCity.Text);
            lst.Add(txtState.Text);
            lst.Add(txtCountry.Text);
            lst.Add(txtPin.Text);
            lst.Add(txtContactPerson.Text);
            lst.Add(txtMobileNo.Text);
            lst.Add(txtLandlineNo.Text);
            lst.Add(txtEmailID.Text);
            
            lst.Add(txtTinNo.Text);
            lst.Add(txtVatNo.Text);
            lst.Add(txtCST.Text);
            lst.Add(txtExciseNo.Text);
            lst.Add(txtRemark.Text);
           
            if (chkActive.Checked == true)
            {
                lst.Add("Y");
            }
            else
            {
                lst.Add("N");
            }
            //lst.Add(txtBillingCity.Text);
            //lst.Add(txtBillingState.Text);
            //lst.Add(txtBillingCountry.Text);
            //lst.Add(txtBillingPinNo.Text);
            lst.Add(VendorID.Value.ToString());

          

            int i = blVendor.UpdateVendorRegistration(lst);
            if (i > 0)
            {
                tblShow.Visible = true;
                lblErrorMessageBox.Text = "";
                lblMessageBox.Text = "Record Update Successfully";
                refresh();
            }
            else
            {
                tblShow.Visible = true;
                lblMessageBox.Text = "";
                lblErrorMessageBox.Text = "Record Already Exist!!";
                refresh();
            }
        }
    }
}