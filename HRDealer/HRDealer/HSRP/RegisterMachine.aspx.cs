using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP
{
    public partial class RegisterMachine : System.Web.UI.Page
    {
        String MacAddress = String.Empty;
        String UserName = String.Empty;
        String Email = String.Empty;
        String MobileNo = String.Empty;
        String MachineName = String.Empty;
        String SqlString = String.Empty;
        String CnnString = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            if (!String.IsNullOrEmpty(Request.QueryString["X"].ToString()))
            {
                MacAddress = Request.QueryString["X"].ToString();
            }
            else
            {
                return;
            }
            if (!String.IsNullOrEmpty(Request.QueryString["N"].ToString()))
            {
                UserName = Request.QueryString["N"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["E"].ToString()))
            {
                Email = Request.QueryString["E"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["M"].ToString()))
            {
                MobileNo = Request.QueryString["M"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["C"].ToString()))
            {
                MachineName = Request.QueryString["C"].ToString();
            }
            ///MacAddress = "Do2788559dao";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SqlString = "Select Top(1) * From MACBase where MacAddress='" + MacAddress + "'";
            DataTable dt =Utils.GetDataTable(SqlString,CnnString);
            if (dt.Rows.Count>=1)
            {
                SqlString = "Update MACBase Set ActiveStatus='N' where MacAddress='" + MacAddress + "'";
                Utils.ExecNonQuery(SqlString, CnnString);
                LabelMessage.ForeColor = System.Drawing.Color.Red;
                LabelMessage.Text = "Machine already registered. Its Blocked now  get in touch with administrator.";
                return ;
            }

            SqlString = "Insert into MACBase(MacAddress,UserName,MachineName,Email,MobileNo,ActiveStatus) values ('" + MacAddress + "','" + UserName + "','" + MachineName + "','" + Email + "','" + MobileNo + "','N')";
            if (Utils.ExecNonQuery(SqlString, CnnString)>0)
            {
                LabelMessage.ForeColor = System.Drawing.Color.Blue;
                LabelMessage.Text = "Request Received Successfully. You will get response in next 12 Hrs.";
            }
            //X=CAF8DA35572C&N=Amit&E=amitbhargavain@gmail.com&M=9810509118
        }
    }
}