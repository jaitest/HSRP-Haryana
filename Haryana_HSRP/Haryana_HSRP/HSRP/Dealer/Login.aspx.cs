using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace HSRP.Dealer
{
    public partial class Login : System.Web.UI.Page
    {
        string CnnString = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
           // Response.Redirect("http://203.122.58.217/hsrp/login.aspx?X=" + Request.QueryString["X"].ToString(), true);
           // Response.Redirect("http://203.122.58.217/vts/login.aspx", true);
            Utils.GZipEncodePage();
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!IsPostBack)
            {
                txtUserID.Focus();
                Session["LoginAttempts"] = string.Empty;
            }
            lblMsgBlue.Text = String.Empty;
            lblMsgRed.Text = String.Empty;
        } 
        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string strDefaultPage = string.Empty;
                string strUserName = string.Empty;
                string strPassword = string.Empty;
                string SQLString = string.Empty;
                string ActiveStatus = string.Empty;
                string userid = string.Empty;
                string HSRPStateID = string.Empty;
                string UserType = string.Empty;
                string UserName = string.Empty;
                string RTOLocationID = string.Empty;
                string dbPassword = string.Empty;
                string dbUserName = string.Empty;
                string FirstLoginStatus = string.Empty;

                strUserName = txtUserID.Text.ToString();
                strPassword = txtUserPassword.Text.ToString();

                if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
                {
                    lblMsgRed.Text = "Please provide login details.";
                    txtUserID.Focus();
                    return;
                }

                SQLString = "Select * From Users where UserLoginName='" + strUserName + "'";

                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
                dbLink.CommandTimeOut = 600;
                dbLink.sqlText = SQLString.ToString();
                SqlDataReader PReader = dbLink.GetReader();


                if (PReader.HasRows)
                {
                    while (PReader.Read())
                    {
                        ActiveStatus = PReader["ActiveStatus"].ToString();
                        userid = PReader["UserID"].ToString();
                        dbUserName = PReader["UserFirstName"].ToString() + " " + PReader["UserLastName"].ToString();
                        dbPassword = PReader["Password"].ToString();
                        FirstLoginStatus = PReader["FirstLoginStatus"].ToString();
                        strDefaultPage = PReader["DefaultPage"].ToString();
                        HSRPStateID = PReader["HSRP_StateID"].ToString();
                        RTOLocationID = PReader["RTOLocationID"].ToString();

                        Session["UserRTOLocationID"] = PReader["RTOLocationID"].ToString();
                        Session["UserType"] = PReader["UserID"].ToString();
                        Session["UserHSRPStateID"] = PReader["HSRP_StateID"].ToString();
                    }
                }
                else
                {

                    txtUserPassword.Text = string.Empty;
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "Your credential did not matched.";
                    txtUserPassword.Focus();

                    return;
                }

                PReader.Close();
                dbLink.CloseConnection();

                if (strPassword.Equals(dbPassword))
                {
                    if (ActiveStatus.Equals("N"))
                    {
                        lblMsgRed.Text = "Please Contact System Administrator.";
                        return;
                    }
                    else
                    {
                        Session["UID"] = userid;
                        Session["UserName"] = dbUserName;
                        if (FirstLoginStatus=="N")
                        {
                            Response.Redirect("Master/Dealer.aspx");
                            //Response.Redirect("http://localhost:52743/Master/Dealer.aspx");
                            lblMsgRed.Text = string.Empty;
                        }
                        else
                        {
                        Response.Redirect(strDefaultPage, true);
                        lblMsgRed.Text = string.Empty;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Session["LoginAttempts"].ToString()))
                    {
                        Session["LoginAttempts"] = 1;
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not matched.";
                        txtUserPassword.Focus();
                    }
                    if (Session["LoginAttempts"].ToString().Equals("5"))
                    {
                        SQLString = "Update Users Set ActiveStatus='N' where UserLoginName='" + strUserName + "'";
                        Utils.ExecNonQuery(SQLString, CnnString);

                        lblMsgRed.Text = "Your Account is blocked : Contact Admin.";

                    }
                    else
                    {
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not matched.";
                        Session["LoginAttempts"] = Convert.ToInt32(Session["LoginAttempts"].ToString()) + 1;
                        txtUserPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsgRed.Text = "Contact Administrator : " + ex.Message.ToString();
            }
        }
         
        protected void LinkButtonForget_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPass.aspx", false);
        }

       
    }
}