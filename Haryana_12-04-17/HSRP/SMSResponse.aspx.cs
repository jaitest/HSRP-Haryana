using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP
{
    public partial class SMSResponse : System.Web.UI.Page
    {
        string strWho = string.Empty;
        string strWhat = string.Empty;
        string strOperator = string.Empty;
        string strCircle = string.Empty;
        string strdate = string.Empty;
        string CnnString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            if (!IsPostBack)
            {
                try
                {
                    strWho = Request.QueryString["who"].ToString();
                    strWhat = Request.QueryString["what"].ToString();
                    strOperator = Request.QueryString["operator"].ToString();
                    strCircle = Request.QueryString["circle"].ToString();
                    strdate = Request.QueryString["datetime"].ToString();
                    string strQuery = "insert into HSRPSMSResponse (MobileNo,SmsMessage,Operator,Circel,ResponseDateTime) values ('" + strWho + "','" + strWhat + "','" + strOperator + "','" + strCircle + "','" + strdate + "')";
                    Utils.ExecNonQuery(strQuery, CnnString);
                }
                catch (Exception ex)
                {

                }
            }

        }
    }
}