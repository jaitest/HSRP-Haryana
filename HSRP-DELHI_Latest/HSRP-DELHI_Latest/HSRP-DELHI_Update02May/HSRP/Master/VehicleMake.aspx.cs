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
    public partial class VehicleMake : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty; 
        string UserType = string.Empty; 
        String Mode = string.Empty;
        int VehicleMakeID;
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
                    VehicleMakeID = Convert.ToInt16(Request.QueryString["VehicleMakeID"]);
                    buttonUpdate.Visible = true;
                    btnSave.Visible = false;
                    VehicleMakeEdit(VehicleMakeID);   
                }
            }
        }
         
        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            string ActiveStatus;
            if (CheckBoxActive.Checked == true)
            {
                ActiveStatus = "Y";
            }
            else
            {
                ActiveStatus = "N";
            }
            userID = Convert.ToInt16(Session["UID"]);
            int VehicleID = Convert.ToInt16(Request.QueryString["VehicleMakeID"]);
            DateTime UpdateDate = System.DateTime.Now; 
            int IsExists = -1;  
            BAL obj = new BAL();
            obj.UpdateHSRPVehicleMake(VehicleID, userID, textboxVehicleMake.Text, UpdateDate, ActiveStatus, ref IsExists);
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
            string ActiveStatus;
            if (CheckBoxActive.Checked == true)
            {
                ActiveStatus = "Y";
            }
            else
            {
                ActiveStatus = "N";
            }

            int userID = Convert.ToInt16(Session["UID"]); 
            DateTime CreateDate = System.DateTime.Now;
            int IsExists = -1;
            BAL obj = new BAL();
            obj.InsertHSRPVehicleMake(userID, textboxVehicleMake.Text, CreateDate, ActiveStatus, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Saved Successfully."; 
            }
        }


        private void VehicleMakeEdit(int VehicleMakeID)
        {
            SQLString = "select VehicleMakerDescription,ActiveStatus from VehicleMakerMaster where VehicleMakerID=" + VehicleMakeID;
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            textboxVehicleMake.Text = dt.Rows[0]["VehicleMakerDescription"].ToString();
            if (dt.Rows[0]["ActiveStatus"].ToString() == "Y")
            {
                CheckBoxActive.Checked = true;
            }
            else
            {
                CheckBoxActive.Checked = false;
            }
        }
    }
}