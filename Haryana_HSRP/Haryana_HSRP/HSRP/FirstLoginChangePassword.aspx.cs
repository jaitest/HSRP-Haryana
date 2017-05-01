using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string UserID = string.Empty;
        string HSRPStateID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            UserID = Session["UID"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            if (!Page.IsPostBack)
            {
                txtusername.Text = Session["UserName"].ToString();
                fillDDl_Question();
            }

        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                string CnnString = string.Empty;
                String SQLString = string.Empty;
                string SQLStringChkPwd = string.Empty;

                if ((!String.IsNullOrEmpty(ddlSecurityQuestion.Text)) && (!String.IsNullOrEmpty(txtSecurityAnswer.Text)) && (!String.IsNullOrEmpty(txtpassword.Text)) && (!String.IsNullOrEmpty(txtNewPassword.Text)) && (!String.IsNullOrEmpty(txtCpassword.Text)))
                {

                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    SQLStringChkPwd = "Select Password From Users where UserID='" + UserID + "'";
                    if (Utils.getDataSingleValue(SQLStringChkPwd, CnnString, "Password") == txtpassword.Text)
                    {
                        SQLString = "Update Users Set  SecurityQuestionID='" + Convert.ToInt32(ddlSecurityQuestion.SelectedValue) + "' , SecurityQuestionAnswer ='" + txtSecurityAnswer.Text + "', Password ='" + txtNewPassword.Text + "', FirstLoginStatus ='N'  where UserID='" + UserID + "'";
                        if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
                        {
                            Response.Redirect("~/Login.aspx");
                        }
                        else
                        {
                            lblErrMess.Text = " Some DB Level Error is their in Execution. your New Password not saved </br> ";
                        }
                    }

                    else
                    {
                        lblErrMess.Text = "The Password provided by You is Incorrect.";
                        BlankField();
                    }

                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        private void BlankField()
        {
            txtpassword.Text = string.Empty;
        }

        private void fillDDl_Question()
        {
            try
            {
                string CnnString = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                Utils.PopulateDropDownList(ddlSecurityQuestion, "select QuestionID,QuestionText from SecurityQuestion", CnnString, "--Select Question--");
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
}