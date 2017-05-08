using System;
using System.Collections.Generic;
using System.Linq;
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