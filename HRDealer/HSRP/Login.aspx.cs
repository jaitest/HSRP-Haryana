using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace HSRP
{
    public partial class Login : System.Web.UI.Page
    {
        string CnnString = String.Empty;


       

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Redirect("http://43.242.124.23/HSRPALL/login.aspx?X=" + Request.QueryString["X"].ToString(),true);
            // Response.Redirect("http://203.122.58.217/vts/login.aspx", true);
            //Utils.GZipEncodePage();

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!IsPostBack)
            {
                txtUserID.Focus();
                Session["LoginAttempts"] = string.Empty;               

            }
            lblMsgBlue.Text = String.Empty;
            lblMsgRed.Text = String.Empty;
        }

        DataSet kk;
        DataTable dt = new DataTable();
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
                string RTOLocationName = string.Empty;
                string dbPassword = string.Empty;
                string dbUserName = string.Empty;
                string FirstLoginStatus = string.Empty;
                string macbaseflag = string.Empty;
                strUserName = txtUserID.Text.ToString();
                string macb = string.Empty;
                string dealerid = string.Empty;
                
                
                strPassword = txtUserPassword.Text.ToString();

                if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
                {
                    lblMsgRed.Text = "Please provide login details.";
                    txtUserID.Focus();
                    return;
                }

                SQLString = "Select * From Users where UserLoginName='" + strUserName + "'";
                dt = Utils.GetDataTable(SQLString, CnnString); //txtUserID
                //|| dt.Rows[0]["UserLoginName"].ToString() == "TCAP"
                if (dt.Rows[0]["withoutmac"].ToString().ToUpper() == "Y")
                {
                    macb = "CAF8DA35332B";
                }
                else
                {
                    macb = Request.QueryString["X"].ToString();
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
                        UserType = PReader["UserType"].ToString();
                        HSRPStateID = PReader["HSRP_StateID"].ToString();
                        RTOLocationID = PReader["RTOLocationID"].ToString();
                        dealerid = PReader["dealerid"].ToString();
                        if (UserType != "0")
                        {
                            if (userid == "7449")
                            {

                            }
                                //--

                            else { 

                            if (string.IsNullOrEmpty(PReader["dealerid"].ToString()))
                            {
                                lblMsgRed.Text = "You are Not Dealer.";
                                return;
                            }
                            if (PReader["dealerid"].ToString()=="0")
                            {
                                lblMsgRed.Text = "You are Not Dealer.";
                                return;
                            }

                                //--
                            }
                        }
                        
                        
                        dbUserName = PReader["UserFirstName"].ToString() + " " + PReader["UserLastName"].ToString();
                        dbPassword = PReader["Password"].ToString();
                        FirstLoginStatus = PReader["FirstLoginStatus"].ToString();
                        strDefaultPage = PReader["DefaultPage"].ToString();
                        macbaseflag = PReader["withoutMAC"].ToString();
                        Session["macbaseflag"] = macbaseflag;

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
                        SQLString = "select RTOLocationName from rtolocation where hsrp_stateid='" + HSRPStateID + "' and rtolocationid='" + RTOLocationID + "'";
                        DataTable dtrtoname = Utils.GetDataTable(SQLString, CnnString);
                        RTOLocationName = dtrtoname.Rows[0]["RTOLocationName"].ToString();

                        Session["UID"] = userid;
                        Session["dealerid"] = dealerid;
                        Session["UserHSRPStateID"] = HSRPStateID;
                        Session["UserRTOLocationID"] = RTOLocationID;
                        Session["UserType"] = UserType;
                        Session["UserName"] = dbUserName;
                        Session["RTOLocationName"] = RTOLocationName;

                      
                        if (FirstLoginStatus == "Y")
                        {
                            Response.Redirect("~/FirstLoginChangePassword.aspx", true);
                        }

                        if (macbaseflag.Trim() == "N" || macbaseflag.Trim() == "Y")
                        {
                            String MacAddress = String.Empty;
                            if (dt.Rows[0]["withoutmac"].ToString() == "Y")
                            {
                                MacAddress = "CAF8DA35332B";
                            }
                            else
                            {
                                MacAddress = Request.QueryString["X"].ToString();
                            }
                            Session["MacAddress"] = MacAddress;
                            SQLString = "Select ActiveStatus,SaveMacAddress From MACBase where MacAddress ='" + MacAddress + "'";
                            kk = Utils.getDataSet(SQLString, CnnString);
                            
                            Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='0', LastLoggedInDatetime ='1900-01-01 00:00:00.000' where  loggedinUserID is null and LastLoggedInDatetime is null", CnnString);

                            SQLString = "select count(*) as Records from MACBASE where  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) >30";
                            int getcounts = Utils.getScalarCount(SQLString, CnnString);
                            if (getcounts > 0)
                            {
                                Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) >30", CnnString);

                            }

                            Session["SaveMacAddress"] = kk.Tables[0].Rows[0]["SaveMacAddress"].ToString();
                            if (dt.Rows[0]["withoutmac"].ToString() == "Y")
                            {
                                Session["MacAddress"] = "CAF8DA35332B";
                            }
                            else
                            {
                                Session["MacAddress"] = Request.QueryString["X"].ToString();
                            }
                            string BrowserName = HttpContext.Current.Request.Browser.Type;
                            string ClientOSName = HttpContext.Current.Request.Browser.Platform;


                            Utils.ExecNonQuery("UPDATE users set lastLoginDatetime=GetDate() where UserLoginName='" + strUserName + "'", CnnString);

                              String Sq = "select  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) as record, MacAddress from MACBASE where LoggedInUserID='" + userid + "'";
                          
                            DataTable checkLogin = Utils.GetDataTable(Sq, CnnString);
                            if (checkLogin.Rows.Count > 0)
                            {
                                int csd = Convert.ToInt16(checkLogin.Rows[0]["record"].ToString());
                                String MacDataUser = checkLogin.Rows[0]["MacAddress"].ToString();
                                if (csd < 10)
                                {
                                    if (MacDataUser == MacAddress)
                                    {

                                        Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                        Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                        Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                        Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                        Response.Redirect(strDefaultPage, true);                                       
                                        lblMsgRed.Text = string.Empty;
                                    }
                                    else
                                    {
                                        CreateDuplicateLogin(userid);
                                        lblMsgRed.Text = "You are Already Loggged In";
                                        lblMsgRed.Visible = true;
                                    }
                                }
                                else
                                {

                                    Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                    Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                    Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                    Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                    Response.Redirect(strDefaultPage, true);                                    
                                    lblMsgRed.Text = string.Empty;
                                }
                            }
                            else
                            {

                                Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                Response.Redirect(strDefaultPage, true);                               
                                lblMsgRed.Text = string.Empty;
                            }

                         
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
        string logmacbaseID;
        private void CreateDuplicateLogin(string userid)
        {
            if (dt.Rows[0]["withoutmac"].ToString() == "Y")
            {
                logmacbaseID = "CAF8DA35332B";
            }
            else
            {
                logmacbaseID = Request.QueryString["X"].ToString();
            }

            string sqle = "Insert into DuplicateLoginLog (userID,UserName,Password,IP,MacbaseID) values ('" + userid + "','" + txtUserID.Text + "','" + txtUserPassword.Text + "','" + 11 + "','" + logmacbaseID + "')";
            Utils.ExecNonQuery(sqle, CnnString);
        }

        protected void LinkButtonForget_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPass.aspx", false);
        }


    }
}