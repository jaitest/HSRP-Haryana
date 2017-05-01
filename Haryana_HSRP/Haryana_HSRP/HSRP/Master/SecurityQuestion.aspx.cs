using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using DataProvider;
using HSRP;

namespace HSRP.Master
{
    public partial class SecurityQuestion : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty; 
        string UserType = string.Empty; 
        String Mode = string.Empty;
        int QuestionID;
        int userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                buttonUpdate.Visible = false;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                Mode = Convert.ToString( Request.QueryString["Mode"]);   
                if (Mode == "Edit")
                { 
                     QuestionID = Convert.ToInt16( Request.QueryString["QID"]);
                    buttonUpdate.Visible = true;
                    btnSave.Visible = false;
                    SecurityQuestionEdit(QuestionID);   
                }
            }
        }
         
        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
             userID = Convert.ToInt16(Session["UID"]); 
            QuestionID = Convert.ToInt16(Request.QueryString["QID"]);
            DateTime UpdateDate = System.DateTime.Now; 
            int IsExists = -1;  
            BAL obj = new BAL();
            obj.UpdateHSRPSecurityQuestion(QuestionID, userID, textboxQuestionText.Text, UpdateDate, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Update Successfully.";  
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = ""; 
            int userID = Convert.ToInt16(Session["UID"]); 
            DateTime CreateDate = System.DateTime.Now;
            int IsExists = -1;
            BAL obj = new BAL();
            obj.InsertHSRPSecurityQuestion(userID, textboxQuestionText.Text, CreateDate, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Saved Successfully."; 
            }
        }


        private void SecurityQuestionEdit(int QuestionID)
        {
            SQLString = "select * from SecurityQuestion where QuestionID=" + QuestionID;
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            textboxQuestionText.Text = dt.Rows[0]["QuestionText"].ToString();
        }
    }
}