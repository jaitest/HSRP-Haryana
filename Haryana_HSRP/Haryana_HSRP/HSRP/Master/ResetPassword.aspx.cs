using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Master
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        string StringOldPassword = string.Empty;
        string StringNewPassword = string.Empty;
        string StringConfirmPassword = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string HSRPStateID = string.Empty;
        string SqlQuery = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            if (!Page.IsPostBack)
            {
                GetUsername();
                //textBoxUserName.Text = Session["UserName"].ToString();

            }
        }

        public void GetUsername()
        {
            SqlQuery = "select UserID,ISNULL(UserFirstName,'')+Space(2)+ISNULL(UserLastName,'') + SPACE(3)+isNULL(UserLoginName,'') as Names from Users where HSRP_StateID='" + HSRPStateID + "' order by UserFirstName asc";
            Utils.PopulateDropDownList(ddlUserid, SqlQuery.ToString(), CnnString, "--Select User Name--");
        }


        protected void buttonSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            //StringOldPassword = textOldPassword.Text;
            StringNewPassword = textNewPassword.Text;
            StringConfirmPassword = textConfirmPassword.Text;

            if (ddlUserid.SelectedItem.ToString().Equals("--Select User Name--"))
            {
                lblErrMess.Text = "Please Provide User Name";
                //textOldPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(StringNewPassword))
            {
                lblErrMess.Text = "Please Provide New Password";
                textNewPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(StringConfirmPassword))
            {
                lblErrMess.Text = "Please Provide Confirm Password";
                textConfirmPassword.Focus();
                return;
            }
            // CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //SQLString = "Select [Password] From Users where UserID='" + ddlUserid.SelectedValue.ToString() + "'";

            //if (!Utils.getDataSingleValue(SQLString, CnnString, "Password").Equals(StringNewPassword))
            //{
            //    lblErrMess.Text = "Old Password didn’t match.";
            //    // textOldPassword.Text = string.Empty;
            //    textNewPassword.Text = string.Empty;
            //    textConfirmPassword.Text = string.Empty;
            //    // textOldPassword.Focus();
            //    return;
            //}
            if (StringNewPassword == StringConfirmPassword)
            {
                if (!string.IsNullOrEmpty(StringNewPassword) && !string.IsNullOrEmpty(StringConfirmPassword))
                {
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    SQLString = "Update Users Set Password='" + StringNewPassword + "',PasswordChangedDate='" + DateTime.Now.ToString() + "' where UserID='" + ddlUserid.SelectedValue.ToString() + "'";
                    if (Utils.ExecNonQuery(SQLString, CnnString) <= 0)
                    {
                        lblErrMess.Text = "Password Not Changed.";

                    }
                    else
                    {
                        if (HSRPStateID.ToString() == "11" || HSRPStateID.ToString() == "9")
                        {
                            string sqlquery = "Select * from users where UserID='" + ddlUserid.SelectedValue.ToString() + "' and isnull(dealerid,'')!='' and isnull(MobileNo,'')!=''";
                            DataTable dt = Utils.GetDataTable(sqlquery, CnnString);
                            if(dt.Rows.Count>0)
                            { 
                                if (dt.Rows[0]["MobileNo"].ToString().Length > 0)
                                {
                                    string SMSText = "Dear Customer, Your password has been changed on your request your new password is " + StringNewPassword.ToString() + ".Kindly do not share your account details with anybody.  Visit  www.hsrpts.com/hsrpts/login.aspx Thank You - LAPL";
                                    //string SMSText = " Cash Rs." + Math.Round(decimal.Parse(lblAmount.Text), 0) + " received against HSRP Authorization No. " + lblAuthNo.Text + " on " + System.DateTime.Now.ToString("dd/MM/yyyy") + " receipt number " + cashrc + ". HSRP Team.";
                                    string sendURL = "http://quick.smseasy.in:8080/bulksms/bulksms?username=sse-tlhsrp1&password=tlhsrp1&type=0&dlr=1&destination=" + dt.Rows[0]["MobileNo"].ToString() + "&source=TSHSRP&message=" + SMSText;
                                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                                    myRequest.Method = "GET";
                                    WebResponse myResponse = myRequest.GetResponse();
                                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                                    string result = sr.ReadToEnd();
                                    sr.Close();
                                    myResponse.Close();
                                    System.Threading.Thread.Sleep(350);
                                    Utils.ExecNonQuery("insert into TSSMSDetail(RtoLocationID,MobileNo,SentResponseCode,smstext) values('" + dt.Rows[0]["RtoLocationID"].ToString() + "'," + dt.Rows[0]["MobileNo"].ToString() + ",'" + result + "','" + SMSText + "')", CnnString);
                                }
                            }
                        }
                        lblSucMess.Text = "Password Has been Reset sucessfully.";
                    }

                }
            }

            else
            {
                lblErrMess.Text = "Password didn’t match.";
            }


        }
    }
}