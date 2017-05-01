using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Master
{
    public partial class ViewCustomerRegistration : System.Web.UI.Page
    {
      
        string UserType = string.Empty;
        DataProvider.BAL blCustomer = new DataProvider.BAL();
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                ShowCustomerDetail();

            }

        }

        public void ShowCustomerDetail()
        {
            dt = blCustomer.ShowCustomerDetail();
            if (dt.Rows.Count > 0)
            {

                Grid1.DataSource = dt;
                Grid1.DataBind();
                //Grid1.RecordCount.ToString();
            }
        }
    }
}