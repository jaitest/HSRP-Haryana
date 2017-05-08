using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Master.InvoiceMaster
{
    public partial class InvoiceProduct : System.Web.UI.Page
    {
        string Mode;
        string UserType = string.Empty;
       
        DataProvider.BAL blProduct = new DataProvider.BAL();



        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }



                string Mode = Request.QueryString["Mode"].ToString();
                if (Mode == "Edit")
                {
                    btnSubmit.Visible = false;
                    btnUpdate.Visible = true;
                    ProductID.Value = Request.QueryString["ProductID"].ToString();
                    editProductdetail(ProductID.Value.ToString());
                }
                else if (Mode == "New")
                {
                    btnSubmit.Visible = true;
                    btnUpdate.Visible = false;
                }
            }

        }

        private void editProductdetail(string ProductID)
        {
           ds=blProduct.EditProductDetail(ProductID);
           if (ds.Tables[0].Rows.Count > 0)
           {
               txtProductName.Text = ds.Tables[0].Rows[0]["ProductName"].ToString();
               txtProductCode.Text = ds.Tables[0].Rows[0]["ProductCode"].ToString();
               txtProductColor.Text = ds.Tables[0].Rows[0]["ProductColor"].ToString();
               txtProductCost.Text = ds.Tables[0].Rows[0]["ProductCost"].ToString();
               txtMeasurementUnit.Text = ds.Tables[0].Rows[0]["P_MeasurementUnit"].ToString();
               txtProductDiscription.Text = ds.Tables[0].Rows[0]["ProductDescription"].ToString();
               string Active = ds.Tables[0].Rows[0]["ActiveStatus"].ToString();
               if (Active == "Y")
               {
                   chkActive.Checked = true;
               }
               else
               {
                   chkActive.Checked = false;
               }
           }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();

            lst.Add(txtProductName.Text);
            lst.Add(txtProductCode.Text);
            lst.Add(txtProductColor.Text);
            lst.Add(txtProductCost.Text);
            lst.Add(txtMeasurementUnit.Text);
            lst.Add(txtProductDiscription.Text);
           
            if (chkActive.Checked == true)
            {
                lst.Add("Y");
            }
            else
            {
                lst.Add("N");
            }
            lst.Add(ProductID.Value.ToString());
           
            int i = blProduct.UpdateInvoiceProduct(lst);
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();

            lst.Add(txtProductName.Text);
            lst.Add(txtProductCode.Text);
            lst.Add(txtProductColor.Text);
            lst.Add(txtProductCost.Text);
            lst.Add(txtMeasurementUnit.Text);
            lst.Add(txtProductDiscription.Text);
           
            if (chkActive.Checked == true)
            {
                lst.Add("Y");
            }
            else
            {
                lst.Add("N");
            }

            int i = blProduct.InsertInvoiceProduct(lst);
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

        private void refresh()
        {
            txtProductName.Text = "";
            txtProductCode.Text = "";
            txtProductColor.Text = "";
            txtProductCost.Text = "";
            txtMeasurementUnit.Text = "";
           txtProductDiscription.Text = "";
           chkActive.Checked = false;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            txtProductName.Text = "";
            txtProductCode.Text = "";
            txtProductColor.Text = "";
            txtProductCost.Text = "";
            txtMeasurementUnit.Text = "";
            txtProductDiscription.Text = "";
            chkActive.Checked = false;
            tblShow.Visible = false;
        }

       


    }
}