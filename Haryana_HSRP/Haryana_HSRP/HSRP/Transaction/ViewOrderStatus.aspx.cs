using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace HSRP.Transaction
{
    public partial class ViewOrderStatus : System.Web.UI.Page
    {
        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string SQLString = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Order.Visible = true;
                show.Visible = false;
            }
        }

        protected void ButtonGo_Click(object sender, EventArgs e)
        {

            string date1;
            SQLString = "SELECT  [OrderDate] ,[VehicleRegNo],[CashReceiptDateTime],[EngineNo],[ChassisNo],[OwnerName],[TotalAmount], [OrderStatus],[HSRP_Front_LaserCode],[HSRP_Rear_LaserCode] FROM [HSRPRecords] where HSRPRecord_AuthorizationNo='" + TextBoxAuthorizationNo.Text + "' and MobileNo='" + TextBoxMobileNo.Text + "'";
            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            if (ds.Rows.Count > 0)
            {

                LabelCashRecieptNo.Text = ds.Rows[0]["OwnerName"].ToString();
               // date1 = ds.Rows[0]["CashReceiptDateTime"].ToString();
               
                //string[] strSplitArr = date1.Split(' ');
                LabelCashRecieptNo.Text = ds.Rows[0]["TotalAmount"].ToString();
                //strSplitArr[0].ToString();
                //string ss = ds.Rows[0]["OrderDate"].ToString("");
                DateTime Orddate = Convert.ToDateTime(ds.Rows[0]["OrderDate"].ToString());
                LabelOrderBooked.Text = Orddate.ToString("dd/MM/yyyy");
                LabelStatus.Text = ds.Rows[0]["OrderStatus"].ToString();
                LabelVehicleNo.Text = ds.Rows[0]["VehicleRegNo"].ToString();
                LabelEngineNo.Text = ds.Rows[0]["EngineNo"].ToString();
                LabelChassisNo.Text = ds.Rows[0]["ChassisNo"].ToString();
                show.Visible = true;
                Order.Visible = false;
            }
            else
            {
                LabelCashRecieptNo.Text = "";
                LabelOrderBooked.Text = "";
                LabelStatus.Text = "";
                LabelVehicleNo.Text = "";
                LabelEngineNo.Text = "";
                LabelChassisNo.Text = "";
                string script = "<script type=\"text/javascript\">  alert('Record not available');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
            }
        }

        protected void btnGoOrderSearch_Click(object sender, EventArgs e)
        {
            Order.Visible =true;
            show.Visible = false;
        }

       
    }
}