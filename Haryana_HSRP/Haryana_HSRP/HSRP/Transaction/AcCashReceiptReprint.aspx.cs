using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using DataProvider;
using System.Data.SqlClient;

namespace HSRP.Transaction
{
    public partial class AcCashReceiptReprint : System.Web.UI.Page
    {


        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;

        int UserId;
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

                UserId = Convert.ToInt32(Session["UID"]);

               lblErrMsg.Text = string.Empty;
               Lblsucess.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

              
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                  
                }
            }

        }


        int j;
        int k = 1;
        protected void Button1_Click(object sender, EventArgs e)
        {
            string txt = TextBox1.Text.Trim();

            string RegNo = txt.Replace("\r\n", "");

            string reg = RegNo.Replace("\t", "");

            String[] vehicleregNo = reg.Split(',');

            SqlConnection con = new SqlConnection(CnnString);
           
            for (int i = 0; i < vehicleregNo.Length; i++)
            {
                string  REg = vehicleregNo[i].Trim().ToString();
                if (string.IsNullOrWhiteSpace(REg))
                {
                    Lblsucess.Text = "";
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = " Record Not Updated Because Wrong Vechicle Registration No Found";
                    Lblsucess.Visible = false;
                    return;

                }
                else
                {

                    string query = "update hsrprecords set APwebserviceresp=NULL where vehicleregno ='" + REg + "'";
                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();
                    j = com.ExecuteNonQuery();
                    k = 1;
                    k = k + j;
                    con.Close();
 
                }              

            }


            if (k > 1)
            {
                lblErrMsg.Text = ""; 
                Lblsucess.Visible = true;
                lblErrMsg.Visible = false;
                Lblsucess.Text = "Record Updated Successfully ";
                TextBox1.Text = "";
                return;
            }
            else
            {
                Lblsucess.Text = "";
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "Record Not Updated";
                Lblsucess.Visible = false;
                return;
            }


          
        }
    }
}