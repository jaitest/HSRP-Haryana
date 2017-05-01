using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;
using HSRP;
 
namespace HSRP.Master
{
    public partial class UpdateRemarks : System.Web.UI.Page
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

            HSRP_StateID =  Convert.ToInt16( Session["UserHSRPStateID"]);
            UserType = Session["UserType"].ToString();
            if (!Page.IsPostBack)
            {
                buttonUpdate.Visible = false;
              string  remarks = Request.QueryString["vehicleregno"];
              if (remarks == "vehicleregno")
                {
                    buttonUpdate.Visible = true;
                    ButtonSave.Visible = false;
                    EditStateID = Convert.ToInt16( Request.QueryString["StateID"]);
                    SQLString = "select * from HSRPState where HSRP_StateID=" + EditStateID;
                    DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
                    textboxBoxHSRPState.Text = ds.Rows[0]["HSRPStateName"].ToString();
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
            lblSucMess.Text="";
            String  EditStateID = Request.QueryString["StateID"].ToString(); 
                string ActiveStatus;
                if (checkBoxActiveStatus.Checked == true)
                {
                    ActiveStatus = "Y";
                }
                else
                {
                    ActiveStatus = "N";
                }

                int IsExists = -1;

                BAL obj = new BAL();

                obj.Update_Master_HSRPState(EditStateID, textboxBoxHSRPState.Text, ActiveStatus, ref IsExists);
                if (IsExists.Equals(1))
                {
                    lblErrMess.Text = "Record Already Exist!!";  
                }
                else
                {
                    lblSucMess.Text = "Record Update Successfully.";
                    //string script1 = "<script type='text/javascript' language='javascript'>if(confirm(\"Record save sucessfully . click ok to close form\")) { parent.googlewin.close();} else {return false;}</script>";
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "confirm", script1); 
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
            int IsExists = -1;
            string StateName = textboxBoxHSRPState.Text;
            BAL obj = new BAL();
            obj.InsertHSRPState(StateName, ActiveStatus, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                string script1 = "<script type='text/javascript' language='javascript'>if(confirm(\"Record save sucessfully . click ok to close form\")) { parent.googlewin.close();} else {return false;}</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "confirm", script1); 
            }
        }
    }
}