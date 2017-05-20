using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Master
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string StringOldPassword = string.Empty;
        string StringNewPassword = string.Empty;
        string StringConfirmPassword = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            if (!Page.IsPostBack)
            {
                textBoxUserName.Text = Session["UserName"].ToString();
            
            }
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            StringOldPassword = textOldPassword.Text;
            StringNewPassword = textNewPassword.Text;
            StringConfirmPassword = textConfirmPassword.Text;

            if (string.IsNullOrEmpty(StringOldPassword))
            {
                lblErrMess.Text = "Please Provide Old Password";
                textOldPassword.Focus();
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
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SQLString = "Select [Password] From Users where UserID='" + Convert.ToInt32(Session["UID"].ToString()) + "'";
            
            if (!Utils.getDataSingleValue(SQLString, CnnString, "Password").Equals(StringOldPassword))
            {
                lblErrMess.Text = "Old Password didn’t match.";
                textOldPassword.Text = string.Empty;
                textNewPassword.Text = string.Empty;
                textConfirmPassword.Text = string.Empty;
                textOldPassword.Focus();
                return;
            }
            if (!string.IsNullOrEmpty(StringNewPassword) && !string.IsNullOrEmpty(StringConfirmPassword))
            {
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                SQLString = "Update Users Set Password='" + StringNewPassword + "',PasswordChangedDate='" + DateTime.Now.ToString() + "' where UserID='" + Convert.ToInt32(Session["UID"].ToString()) + "'";
                if (Utils.ExecNonQuery(SQLString, CnnString) <= 0)
                {
                    lblErrMess.Text = "Password Not Changed.";
                }
                else
                {
                    lblSucMess.Text = "Password Changed sucessfully.";
                }

            }
        }
    }
}