using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataProvider;

namespace HSRP.Master
{
    public partial class ExpenseMaster : System.Web.UI.Page
    {
        string Mode;
        int HSRP_StateID;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        int EditStateID;
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            HSRP_StateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            UserType = Session["UserType"].ToString();
            if (!Page.IsPostBack)
            {
                buttonUpdate.Visible = false;
                Mode = Request.QueryString["Mode"];
                if (Mode == "Edit")
                {
                    buttonUpdate.Visible = true;
                    ButtonSave.Visible = false;
                    EditStateID = Convert.ToInt16(Request.QueryString["ExpenceID"]);
                    SQLString = "select * from ExpenseMaster where ExpenceID=" + EditStateID;
                    DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
                    textboxBoxHSRPState.Text = ds.Rows[0]["ExpenseName"].ToString();
                    string check = ds.Rows[0]["ActiveStatus"].ToString();
                    if (check == "Y")
                    {
                        checkBoxActiveStatus.Checked = true;
                    }
                    else
                    {
                        checkBoxActiveStatus.Checked = false;
                    }
                }

            }
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            String EditStateID = Request.QueryString["ExpenceID"].ToString();
            string ActiveStatus;
            if (checkBoxActiveStatus.Checked == true)
            {
                ActiveStatus = "Y";
            }
            else
            {
                ActiveStatus = "N";
            }

            int Count = Utils.getScalarCount("select COUNT(*) from ExpenseMaster where ExpenseName='" + textboxBoxHSRPState.Text + "' and ActiveStatus='" + ActiveStatus + "'", ConnectionString);

            if (Count>0)
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                string UpdateExpence = "UPDATE ExpenseMaster SET ExpenseName='" + textboxBoxHSRPState.Text + "',ActiveStatus='" + ActiveStatus + "' where ExpenceID='" + EditStateID + "'";
                Utils.ExecNonQuery(UpdateExpence, ConnectionString);
                lblSucMess.Text = "Record Update Successfully.";
            }

        }

        protected void ButtonSave_Click1(object sender, EventArgs e)
        {

            lblErrMess.Text = "";
            lblSucMess.Text = "";

            string ActiveStatus;
            if (checkBoxActiveStatus.Checked == true)
            {
                ActiveStatus = "Y";
            }
            else
            {
                ActiveStatus = "N";
            }

            string ExpenseName = textboxBoxHSRPState.Text;
            //BAL obj = new BAL();
            //obj.InsertHSRPState(StateName, ActiveStatus, ref IsExists);

            int Count = Utils.getScalarCount("select COUNT(*) from ExpenseMaster where ExpenseName='" + ExpenseName + "'", ConnectionString);


            if (Count>0)
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                string InsertExpence = "INSERT INTO ExpenseMaster (ExpenseName,ActiveStatus) VALUES ('" + ExpenseName + "','" + ActiveStatus + "')";
                Utils.ExecNonQuery(InsertExpence, ConnectionString);
                lblSucMess.Text = "Save Successfully.";
                //string script1 = "<script type='text/javascript' language='javascript'>if(confirm(\"Record save sucessfully . click ok to close form\")) { parent.googlewin.close();} else {return false;}</script>";
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "confirm", script1);
            }
        }
    }
}