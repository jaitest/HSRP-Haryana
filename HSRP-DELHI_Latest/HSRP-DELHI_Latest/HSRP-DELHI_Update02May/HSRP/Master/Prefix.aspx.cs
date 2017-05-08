using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DataProvider;
using System.Data;
using System.Data.SqlClient;

namespace HSRP.Master
{
    public partial class Prefix : System.Web.UI.Page
    { 
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty; 
        string Mode;
        string UserType;
        int PrefixID;
        int HSRPStateID;
        int RTOLocationID; 
        int intHSRPStateID;
        int intRTOLocationID; 
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!Page.IsPostBack)
            {
                lblErrMess.Text = "";
                lblSucMess.Text = "";
                try
                {
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    buttonUpdate.Visible = false;
                    Mode = Request.QueryString["Mode"].ToString();
                    RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                    HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                    UserType = Session["UserType"].ToString();
                }
                catch
                {
                }

                if (Mode == "Edit")
                {
                    buttonUpdate.Visible = true;
                    btnSave.Visible = false;
                    DropdownStateName.Enabled = false;
                    DropdownRTOName.Enabled = false;
                    PrefixID = Convert.ToInt16(Request.QueryString["PrefixID"]); 
                    PrefixEdit(PrefixID);

                }
                if (Mode == "New")
                {
                    FilldropDownState();
                    FilldropDownRTO();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            int IsExists = -1;
            int.TryParse(DropdownRTOName.SelectedValue, out intRTOLocationID);
            int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID); 
            string prefixFor =   DropdownFrefixFor.SelectedItem.ToString();
            BAL obj = new BAL();
            obj.InsertHSRPPrefix(intHSRPStateID, intRTOLocationID, prefixFor, textboxPrefiex.Text, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Update Successfully.";
            }
        }

        private void FilldropDownState()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName"; 
                Utils.PopulateDropDownList(DropdownStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                DropdownStateName.Enabled = false;
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID= " + HSRPStateID + "and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropdownStateName, SQLString.ToString(), CnnString, "--Select State--");
                DropdownStateName.SelectedIndex = DropdownStateName.Items.Count - 1;
            } 
        } 
        private void FilldropDownRTO()
        {
            if (UserType == "0")
            {
                int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
            }
            else
            {
                DropdownRTOName.Enabled = false; 
                SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where RTOLocationID = " + RTOLocationID + "and ActiveStatus='Y'";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
                DropdownRTOName.SelectedIndex = DropdownRTOName.Items.Count - 1;
            } 
        }
     
        protected void DropdownStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            UserType = Session["UserType"].ToString();
            FilldropDownRTO(); 
        } 
        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = ""; 
            int IsExists = -1;
             PrefixID = Convert.ToInt16(Request.QueryString["PrefixID"]);
            int.TryParse(DropdownRTOName.SelectedValue, out intRTOLocationID);
            int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID); 
            string prefixFor = DropdownFrefixFor.SelectedItem.ToString();
            BAL obj = new BAL();
            obj.UpdateHSRPPrefix(PrefixID, intHSRPStateID, intRTOLocationID, prefixFor, textboxPrefiex.Text, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Update Successfully.";
            }
        }

        private void PrefixEdit(int Prefix)
        {
            SQLString = "SELECT SerialPrefixID, HSRP_StateID,  RTOLocationID, PrefixFor, PrefixText, LastNo FROM Prefix where SerialPrefixID=" + PrefixID;
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);

            DropdownFrefixFor.SelectedItem.Text = dt.Rows[0]["PrefixFor"].ToString();
            textboxPrefiex.Text = dt.Rows[0]["PrefixText"].ToString();
            DropdownFrefixFor.SelectedValue = dt.Rows[0]["PrefixFor"].ToString();
            SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=" + dt.Rows[0]["HSRP_StateID"];
            Utils.PopulateDropDownList(DropdownStateName, SQLString.ToString(), CnnString, "--Select State--");
            DropdownStateName.SelectedIndex = DropdownStateName.Items.Count - 1;
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + dt.Rows[0]["RTOLocationID"].ToString();
            Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
            DropdownRTOName.SelectedIndex = DropdownRTOName.Items.Count - 1;
        }

    }
}