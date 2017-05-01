using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HSRP.Transaction
{
    public partial class MP_CustomerNotCame : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();
            }
            if (!IsPostBack)
            {
               
                Panel1.Visible = false;
               
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_regno.Text))
            {
                LabelError.Text = "Please Enter Registration No.";
                return;
            }

            SQLString = "Select * From MPCustomerNotCameSentSMS where VehicleRegno='" + txt_regno.Text + "'";

            DataTable dtRecord = Utils.GetDataTable(SQLString, CnnString);

            LabelError.Text = "";
            lblSuccess.Text = "";
            if (dtRecord.Rows.Count > 0)
            {
                ViewState["DtRecord"] = dtRecord;

                Panel1.Visible = true;
                lblRegNo.Text = dtRecord.Rows[0]["VehicleRegno"].ToString();
                lblAuthno.Text = dtRecord.Rows[0]["Authno"].ToString();

                lblAuthDate.Text = dtRecord.Rows[0]["AuthDate"].ToString();
                lblMobileNo.Text = dtRecord.Rows[0]["MobileNo"].ToString();
                lblSentDateTime.Text = dtRecord.Rows[0]["SentDateTime"].ToString();
                lblServerResponse.Text = dtRecord.Rows[0]["ServerResponse"].ToString();
                lblSentStatus.Text = dtRecord.Rows[0]["SentStatus"].ToString();
                lblCallRecievedOn.Text = dtRecord.Rows[0]["CallRecievedOn"].ToString();

                lblRemarks.Text = dtRecord.Rows[0]["Remarks"].ToString();
                lblRtoName.Text = dtRecord.Rows[0]["RtoName"].ToString();

                lblcashreceiptnoexist.Text = dtRecord.Rows[0]["cashreceiptnoexist"].ToString();
                lblhsrpfeepaid.Text = dtRecord.Rows[0]["hsrpfeepaid"].ToString();
                lblpaidto.Text = dtRecord.Rows[0]["paidto"].ToString();

                lblcashreceiptno.Text = dtRecord.Rows[0]["cashreceiptno"].ToString();
                lblhsrp_rear_lasercode.Text = dtRecord.Rows[0]["hsrp_rear_lasercode"].ToString();
                lblhsrp_front_lasercode.Text = dtRecord.Rows[0]["hsrp_front_lasercode"].ToString();
            }
            else
            {
                LabelError.Text = "No Record Found";
                Panel1.Visible = false;
            }
           
            
        }


        public void SaveRecord()
        {

         SQLString = "UPDATE MPCustomerNotCameSentSMS SET cashreceiptnoexist='"+ddl_cashrecipt.SelectedValue+"', hsrpfeepaid='"+ddl_Fee.SelectedValue+"', paidto='"+ddl_paidto.SelectedValue+"', cashreceiptno='"+txt_cashrecipt.Text+"', hsrp_rear_lasercode='"+txt_rearlaser.Text+"', hsrp_front_lasercode='"+txt_frontlaser.Text+"', Remarks='"+txt_remarks.Text+"' WHERE VehicleRegno = '"+txt_regno.Text+"'";
         if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
         {
             lblSuccess.Text = "Record Saved Successfully";
         }
         else
         {
             LabelError.Text = "Record Not Saved";
         }
        
        }

       

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }
    }
}