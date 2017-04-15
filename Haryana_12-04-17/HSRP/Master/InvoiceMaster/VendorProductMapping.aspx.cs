using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

namespace HSRP.Master.InvoiceMaster
{
    public partial class VendorProductMapping : System.Web.UI.Page
    {
        //int UserType;
        string CnnString = string.Empty;
        String SQLString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.GZipEncodePage();
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    //int.TryParse(Session["UserType"].ToString(), out UserType);
                    //int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                    //int.TryParse(Session["UID"].ToString(), out UserID);
                    lblErrMess.Text = string.Empty;
                    lblSucMess.Text = string.Empty;
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                }


                if (!Page.IsPostBack)
                {

                    //if (UserType.Equals(0))
                    //{
                    //   // dropDownListState.Enabled = true;
                    //    FilldropDownListState();
                    //    //  FilldropDownListClient();
                    //}
                    //else if (UserType.Equals(1))
                    //{
                    //    FilldropDownListState();
                    // //   dropDownListState.SelectedValue = HSRPStateID.ToString();
                    //   // dropDownListState.Enabled = false;
                    //    FilldropDownListVendor();
                    //    dropDownListVendor.Enabled = true;
                    //    dropDownListVendor.SelectedIndex = 0;
                    //}
                    //else
                    //{
                    //    FilldropDownListState();
                       // dropDownListState.SelectedValue = HSRPStateID.ToString();
                       // dropDownListState.Enabled = false;
                        FilldropDownListVendor();
                        dropDownListVendor.SelectedIndex = 0;
                   // }
                }
            }
            catch (Exception ee)
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Contact to admin.";
                lblErrMess.Text = lblErrMess.Text + " " + ee.Message;
            }
        }

        #region DropDown

        private void FilldropDownListVendor()
        {
            SQLString = "select isnull(Name,'') as Name,VendorID from Invoice_VendorRegistration  order by Name";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListVendor, SQLString.ToString(), CnnString, "--Select Vendor--");
            dropDownListVendor.SelectedIndex = dropDownListVendor.Items.Count - 1;
        }

        protected void dropDownListVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrMess.Text = String.Empty;
            lblSucMess.Text = String.Empty;
            checkBoxListGroup.Items.Clear();
            if (!dropDownListVendor.SelectedValue.Equals("--Select Vendor--"))
            {
                checkBoxListGroup.Visible = true;
                FillCheckBoxListProduct();
            }
            else
            {
                checkBoxListGroup.Visible = false;
                lblErrMess.Text = String.Empty;
                lblSucMess.Text = String.Empty;
            }
        }

        #endregion

        private void FillCheckBoxListProduct()
        {
           
                if (String.IsNullOrEmpty(dropDownListVendor.SelectedValue) || dropDownListVendor.SelectedValue.Equals("--Select Vendor--"))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Select Vendor.";
                    return;
                }
                SQLString = "Select P.ProductName,P.ProductID,VPM.VendorID as tt From Invoice_Product P  left join Invoice_VendorProductMapping VPM on  P.ProductID=VPM.ProductID where VPM.VendorID="+dropDownListVendor.SelectedValue+"";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            DataTable dtt = Utils.GetDataTable(SQLString, CnnString);

            SQLString = "Select ProductName,ProductID From Invoice_Product";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            string var = String.Empty;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var = dr["ProductID"].ToString();
                    bool flag = false;
                    foreach (DataRow dr1 in dtt.Rows)
                    {
                        if (dr1["ProductID"].ToString() == var)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        checkBoxListGroup.Items.Add(new ListItem(dr["ProductName"].ToString(), dr["ProductID"].ToString(), true));
                    }
                    else
                    {
                        checkBoxListGroup.Items.Add(new ListItem(dr["ProductName"].ToString(), dr["ProductID"].ToString(), false));
                    }
                }
            }
            foreach (ListItem item in checkBoxListGroup.Items)
            {
                if (item.Enabled == true)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Enabled = true;
                }
            }
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(dropDownListVendor.SelectedValue) || dropDownListVendor.SelectedValue.Equals("--Select Vendor--"))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select Vendor.";
                return;
            }
            CheckboxListSelections(checkBoxListGroup);
        }

        private void CheckboxListSelections(System.Web.UI.WebControls.CheckBoxList Checklist)
        {

            StringBuilder Query = new StringBuilder();
           // int.TryParse(dropDownListVendor.SelectedValue, out UserID);
            foreach (ListItem item in Checklist.Items)
            {
                if (item.Selected.Equals(true))
                {
                    Query.Append("insert into Invoice_VendorProductMapping(VendorID,ProductID,CreatedBy) values(" + dropDownListVendor.SelectedValue + "," + item.Value + ",1);");
                }
            }
            SQLString = "Delete from Invoice_VendorProductMapping Where VendorID=" + dropDownListVendor.SelectedValue;
            Utils.ExecNonQuery(SQLString, CnnString);
            if (Utils.ExecNonQuery(Query.ToString(), CnnString) > 0)
            {
                lblSucMess.Text = "Product Sucessfully assigned to Vendor:  " + dropDownListVendor.SelectedItem.Text;
            }
            else
            {
                lblErrMess.Text = "Product not assigned to Vendor:  " + dropDownListVendor.SelectedItem.Text;
            }
        }
    }
}