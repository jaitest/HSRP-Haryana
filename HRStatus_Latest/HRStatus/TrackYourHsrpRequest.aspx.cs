using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class TrackYourHsrpRequest : System.Web.UI.Page
{
    public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
    String aapID = String.Empty;
    String SQLString = String.Empty;
    String Status = String.Empty;
    // string SQLString = String.Empty;
    string CnnString = String.Empty;
    string strURL = string.Empty;
    string strState;
    string strVechNo = string.Empty;
    DataTable dtresult = new DataTable();
    DataTable dtVechNo = new DataTable();


    MailHelper objMailSender = new MailHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
      
            
    }
       

    string vehicalRegNo;

    public void GetHostAddress()
    {
        aapID = HttpContext.Current.Request.UserHostAddress.ToLower();

    }

   

   

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;

    }

 

   


    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["REG"] = null;
        Response.Redirect("OrderStatus.aspx");
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string strsql = "select * from HR_OldDataRequest where RequestNo='" + txtRequestNo.Text + "'";
            dtresult = utils.GetDataTable(strsql, ConnectionString);


            if (dtresult.Rows.Count == 0)

            {
               lblMsg.Text="Request Not Valid";
                //ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Request Not Valid');", true);
            }

            else
            {
                strVechNo = dtresult.Rows[0]["VehicleRegNo"].ToString();

                string strsql1 = "select * from HSRPRecordsStaggingArea where VehicleRegNo='" + strVechNo + "'";
                dtVechNo = utils.GetDataTable(strsql, ConnectionString);
            }

            if (dtVechNo.Rows.Count == 0)
            {
                lblMsg.Text = "Request Pending to get data from vahan Please contact your RTO.";
                //ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Request Pending to get data from vahan Please contact your rto.');", true);
            }
            else
            {
                lblMsg.Text = "Your Request Number has been processed please visit HSRP center to pay HSRP fee or pay online. ";
                //ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Your Request Number has been processed please visit HSRP center to pay HSRP fee or pay online.');", true);
            }
        }

        catch(Exception ex)
        {

        }

       
        
    }
}
