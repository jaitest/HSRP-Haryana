using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class VerifyAmount : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string ExpenceID = string.Empty;
        string Mode = string.Empty;

        //string UserID;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;
        int UserID;

        int RTOLocationIDAssign;
        int StateIDAssign;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            } 

            if (!Page.IsPostBack)
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"].ToString());
                Mode = Request.QueryString["Mode"];
                if (Mode == "Edit")
                {
                    ExpenceID = Request.QueryString["ExpenseID"].ToString();
                    AssignLaserEdit(ExpenceID);

                }
            }
        }

        private void AssignLaserEdit(string ExpenceID)
        {
            SQLString = "SELECT CONVERT (varchar(20),BillDate,103) as BillDate,VatAmount,ServiceAmount,ExciseAmount,ClaimedBy,OtherAmount,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,VerifiedAmount,VendorName,((BillAmount + isnull(VatAmount,0) + isnull(ServiceAmount,0) + isnull(ExciseAmount,0) + isnull(OtherAmount,0))) as Balance,(select HSRPStateName from HSRPState where HSRPState.HSRP_StateID=ExpenseSave.HSRP_StateID) as StateName,(select RTOLocationName from RTOLocation where RTOLocationID=LocationID) as RTOLocationName,BillNo,BillAmount,Remarks,ExpenseStatus FROM ExpenseSave where ExpenseSaveID=" + ExpenceID;
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            if (ds.Rows.Count > 0)
            {
                LabelStateID.Text = ds.Rows[0]["StateName"].ToString();
                LabelRTOLocationID.Text = ds.Rows[0]["RTOLocationName"].ToString();
                LabelBillDate.Text = ds.Rows[0]["BillDate"].ToString();
                LabelExpenseName.Text = ds.Rows[0]["ExpenseName"].ToString();
                LabelBillNo.Text = ds.Rows[0]["BillNo"].ToString();
                LabelBillAmount.Text = ds.Rows[0]["BillAmount"].ToString();
                Remarks.Text = ds.Rows[0]["Remarks"].ToString();
                billAmt.Value = ds.Rows[0]["VerifiedAmount"].ToString();

                RemainAmt.Value = ds.Rows[0]["BillAmount"].ToString();
                //RemainAmt.Value = ds.Rows[0]["Balance"].ToString();

                lblBalance.Text = ds.Rows[0]["Balance"].ToString();
                lblVendor.Text = ds.Rows[0]["VendorName"].ToString();
                lblVat.Text = ds.Rows[0]["VatAmount"].ToString();
                lblService.Text = ds.Rows[0]["ServiceAmount"].ToString();
                lblExcise.Text = ds.Rows[0]["ExciseAmount"].ToString();
                txtOthers.Text = ds.Rows[0]["OtherAmount"].ToString();
                OtherAmount.Value = ds.Rows[0]["OtherAmount"].ToString();
                lblClaimedBy.Text = ds.Rows[0]["ClaimedBy"].ToString();
                




                if (ds.Rows[0]["Balance"].ToString()=="0.00")
                {
                    btnSave.Enabled = false;
                }
                
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ExpenceID = Request.QueryString["ExpenseID"].ToString();
            UserID = Convert.ToInt32(Session["UID"].ToString());
            string Status=dropDownListorderStatus.SelectedValue;
            
            //if (Convert.ToDecimal(txtVerifyAmt.Text) == Convert.ToDecimal(billAmt.Value))
            //{
            //    Status = "Clear";
            //}
            decimal verifyAmt;
           
            if (billAmt.Value != "")
            {
                verifyAmt = (Convert.ToDecimal(billAmt.Value) + Convert.ToDecimal(txtVerifyAmt.Text));
               // billAmount = Convert.ToDecimal(LabelBillAmount.Text) - verifyAmt;
            }
            else
            {
                verifyAmt = Convert.ToDecimal(txtVerifyAmt.Text);
               // billAmount = Convert.ToDecimal(LabelBillAmount.Text) - verifyAmt;
            }

            decimal billAmount = (Convert.ToDecimal(LabelBillAmount.Text) - Convert.ToDecimal(txtVerifyAmt.Text));
            decimal VAt = Convert.ToDecimal(lblVat.Text) - ((Convert.ToDecimal(lblVat.Text) * Convert.ToDecimal(txtVerifyAmt.Text)) / Convert.ToDecimal(LabelBillAmount.Text));
            decimal Service = Convert.ToDecimal(lblService.Text) - ((Convert.ToDecimal(lblService.Text) * Convert.ToDecimal(txtVerifyAmt.Text)) / Convert.ToDecimal(LabelBillAmount.Text));
            decimal ExciseAmount = Convert.ToDecimal(lblExcise.Text) - ((Convert.ToDecimal(lblExcise.Text) * Convert.ToDecimal(txtVerifyAmt.Text)) / Convert.ToDecimal(LabelBillAmount.Text));
            decimal OtherAmt = (Convert.ToDecimal(OtherAmount.Value) - Convert.ToDecimal(txtOthers.Text));

            SQLString = "UPDATE ExpenseSave SET ExpenseStatus='" + Status + "',BillAmount='" + billAmount + "',VatAmount='" + VAt + "',ServiceAmount='" + Service + "',ExciseAmount='" + ExciseAmount + "',OtherAmount='" + OtherAmt + "' , VerifiedBy='" + UserID + "' ,VerifiedAmount='" + verifyAmt + "' ,VerifiedRemarks='" + txtRemarks.Text + "' , VerifiedDate=getdate()  where ExpenseSaveID=" + ExpenceID;
            Utils.ExecNonQuery(SQLString,CnnString);

            SQLString = "SELECT ServiceAmount,ExciseAmount,OtherAmount,VatAmount,BillAmount,((BillAmount + isnull(VatAmount,0) + isnull(ServiceAmount,0) + isnull(ExciseAmount,0) + isnull(OtherAmount,0) )) as Balance FROM ExpenseSave where ExpenseSaveID=" + ExpenceID;
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            
            string showText = string.Empty;
            if (dt.Rows[0]["Balance"].ToString() != "0.00")
            {
                showText = " – Rs. " + dt.Rows[0]["Balance"].ToString() + " pending.";
                lblBalance.Text = dt.Rows[0]["Balance"].ToString();
                lblService.Text = dt.Rows[0]["ServiceAmount"].ToString();
                lblExcise.Text = dt.Rows[0]["ExciseAmount"].ToString();
                txtOthers.Text = dt.Rows[0]["OtherAmount"].ToString();
                lblVat.Text = dt.Rows[0]["VatAmount"].ToString();
                LabelBillAmount.Text = dt.Rows[0]["BillAmount"].ToString();
            }
            else
            {
                showText = " – Entire Bill Amount cleared";
                lblBalance.Text = dt.Rows[0]["Balance"].ToString();
                lblService.Text = dt.Rows[0]["ServiceAmount"].ToString();
                lblExcise.Text = dt.Rows[0]["ExciseAmount"].ToString();
                txtOthers.Text = dt.Rows[0]["OtherAmount"].ToString();
                lblVat.Text = dt.Rows[0]["VatAmount"].ToString();
                LabelBillAmount.Text = dt.Rows[0]["BillAmount"].ToString();
            }


            lblSucMess.Text = "Record Updated Successfully" +showText;
            btnSave.Enabled = false;

        }

        
    }
}