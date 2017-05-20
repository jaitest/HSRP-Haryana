using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP
{
    public partial class ForgotPass : System.Web.UI.Page
    {
        string CnnString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!IsPostBack)
            {
                txtusername.Attributes.Add("onfocusout", "UserName(this,event)");
                txtusername.Attributes.Add("onfocus", "UserName(this,event)");
                txtSecAnswer.Attributes.Add("onfocusout", "SecurityAnswer(this,event)");
                txtSecAnswer.Attributes.Add("onfocus", "SecurityAnswer(this,event)");
                tr0.Visible = true;
                tr1.Visible = true;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string SQLString = string.Empty;
                SQLString = "Select  S.Question From Users As U  inner join SecurityQuestion As S on S.QueID = U.SecurityQuestionID  where U.UserLoginName= '" + txtusername.Text + "' and U.ActiveStatus='Y'";
                string Question = Utils.getDataSingleValue(SQLString, CnnString, "Question");
                if (!String.IsNullOrEmpty(Question))
                {
                    labelQuestion.Text = Question;
                    tr0.Visible = false;
                    tr1.Visible = false;
                    tr2.Visible = true;
                    tr3.Visible = true;
                    tr4.Visible = true;
                    tr5.Visible = true;
                }
            }
            catch (Exception ee)
            {
                lblErrMess.Text = ee.Message;
            }
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            string SQLString = string.Empty;
            SQLString = "Select SecurityQuestionAnswer From Users where UserLoginName= '" + txtusername.Text + "'";
            if (Utils.getDataSingleValue(SQLString, CnnString, "SecurityQuestionAnswer") == txtSecAnswer.Text)
            {
                txtResetUsername.Text = txtusername.Text;
                tr2.Visible = false;
                tr3.Visible = false;
                tr4.Visible = false;
                tr5.Visible = false;

                tr6.Visible = true;
                tr7.Visible = true;
                tr8.Visible = true;
                tr9.Visible = true;
                tr10.Visible = true;
                
            }
            else
            {
                lblErrMess.Text = "Answer provided by you is not correct";
                return;
            }

        }

        protected void btnBack1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void btn_Proceed_Click(object sender, EventArgs e)
        {
            String SQLString = string.Empty;
            SQLString = "Update Users Set  Password ='" + txtResetPassword.Text + "' where UserLoginName='" + txtusername.Text + "'";
            if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
            {
                tr11.Visible = true;
                tr6.Visible = false;
                tr7.Visible = false;
                tr8.Visible = false;
                tr9.Visible = false;
                tr10.Visible = false;
            }
            else
            {
                lblErrMess.Text = "Some DB Level Error occurs.";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
}