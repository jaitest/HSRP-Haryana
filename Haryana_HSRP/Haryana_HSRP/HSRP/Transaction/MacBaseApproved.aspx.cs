using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Transaction
{
    public partial class MacBaseApproved : System.Web.UI.Page
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
      
        string StringEmbcode = string.Empty;
        string StringDesignation = string.Empty;
        string StringTimeIN = string.Empty;
        string StringTimeout = string.Empty;
        string SQLString = string.Empty;
        string sqlQuery12 = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string USERID = string.Empty;
        string strtimeinto = string.Empty;
        string strtimeoutto = string.Empty; 
        string strempid =string.Empty;
        string Empidd = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                USERID = Session["UID"].ToString();
            }

            if (!Page.IsPostBack)
            {
                // textBoxUserName.Text = Session["UserName"].ToString();   
                
            }
            else
            {
                //FilldropDownListClient();
            }


          
        }


       

      


        protected void buttonSave_Click(object sender, EventArgs e)
        {


            SQLString = "update MACBase set activestatus='Y' where username='" + textBoxUserName.Text + "'";
            int result = Utils.ExecNonQuery(SQLString, CnnString);
            if (result > 0)
            {
                lblSucMess.Visible = true;
                lblSucMess.Text = "";
                lblSucMess.Text = "Record Save Successfully...";
            }
            


        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            SQLString = "select MacAddress,UserName,MachineName from MACBase  where UserName='" + textBoxUserName.Text + "' ";
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);

            if (dt.Rows.Count > 0)
            {

                buttonSave.Visible = true;
            }

            else
            {
                lblErrMess.Text = "Record Not Found.";
            }
        }

       
        
    }
}
