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
    public partial class MacAssignToRTO : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;  
        string UserType = string.Empty; 
        int HSRPStateID;
        int RTOLocationID; 
        int intHSRPStateID;
        int intRTOID;
        int MacBaseRequestsID;
        string Mode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                Mode = Request.QueryString["Mode"].ToString(); 
                UserType = Session["UserType"].ToString();
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                buttonUpdate.Visible = false;
                
                MacBaseRequestsID = Convert.ToInt16(Request.QueryString["MacBaseRequestsID"]);
                if (Mode == "InActive")
                {
                    string ActiveStatus = "InActive";
                    BAL obj = new BAL();
                    obj.MacInActive(MacBaseRequestsID, ActiveStatus); 
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "keyname", "javascript:parent.googlewin.close();", true);

                }
                if (Mode == "MacBlock")
                {
                    string ActiveStatus = "MacBlock";
                    BAL obj = new BAL();
                    obj.MacInActive(MacBaseRequestsID, ActiveStatus);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "keyname", "javascript:parent.googlewin.close();", true);

                }

                if (Mode == "Edit")
                { 
                    //dropdownlistRtoName.Enabled = false;
                    //dropdownlistStateName.Enabled = false;
                    buttonUpdate.Visible = true;
                    btnSave.Visible = false;
                    //MacBaseRequestsID = Convert.ToInt16(Request.QueryString["MacBaseRequestsID"]);
                    MessageTickerEdit(MacBaseRequestsID);

                    FilldropDownState();
                    FilldropDownRTO();
                } 
                else 
                {
                    FilldropDownState();
                    FilldropDownRTO();
                } 
            }
        }
     
        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
             MacBaseRequestsID = Convert.ToInt16(Request.QueryString["MacBaseRequestsID"]);  
            int.TryParse(dropdownlistRtoName.SelectedValue, out intRTOID);
            int.TryParse(dropdownlistStateName.SelectedValue, out intHSRPStateID); 
            int IsExists = -1; 
            
            DateTime UpdateDate = Convert.ToDateTime(System.DateTime.Now);
            BAL obj = new BAL();
            obj.UpdateMacBaser(MacBaseRequestsID, intHSRPStateID, intRTOID);
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
            int.TryParse(dropdownlistRtoName.SelectedValue, out intRTOID);
            int.TryParse(dropdownlistStateName.SelectedValue, out intHSRPStateID); 
            int IsExists = -1; 
            
            DateTime CreatedDateTime =  System.DateTime.Now;
            BAL obj = new BAL();
            //obj.InsertHSRPMessageTicker(textboxMessageText.Text, textboxMessageTextURL.Text, intHSRPStateID, intRTOID, CreatedDateTime, Active, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Save Successfully."; 
            }

        }

        protected void dropdownlistStateName_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            UserType = Session["UserType"].ToString();
            FilldropDownRTO();
        }

        private void FilldropDownState()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName"; 
                Utils.PopulateDropDownList(dropdownlistStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                dropdownlistStateName.Enabled = false;
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID= " + HSRPStateID + "and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropdownlistStateName, SQLString.ToString(), CnnString, "--Select State--");
                dropdownlistStateName.SelectedIndex = dropdownlistStateName.Items.Count - 1;
            } 
        }

        private void FilldropDownRTO()
        {
            if (UserType == "0")
            {
                int.TryParse(dropdownlistStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropdownlistRtoName, SQLString.ToString(), CnnString, "--Select RTO Location--");
            }
            else
            {
                dropdownlistRtoName.Enabled = false;
                SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where RTOLocationID = " + RTOLocationID + "and ActiveStatus='Y'";
                Utils.PopulateDropDownList(dropdownlistRtoName, SQLString.ToString(), CnnString, "--Select RTO Location--");
                dropdownlistRtoName.SelectedIndex = dropdownlistRtoName.Items.Count - 1;
            } 
        }


        private void MessageTickerEdit(int MacBaseRequestsID)
        {
            SQLString = "select Hsrp_StateID,RTOLocationID from MACBase where MacBaseID=" + MacBaseRequestsID;
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                //SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=" + dt.Rows[0]["HSRP_StateID"];
                //Utils.PopulateDropDownList(dropdownlistStateName, SQLString.ToString(), CnnString, "--Select State--");
                //dropdownlistStateName.SelectedIndex = dropdownlistStateName.Items.Count - 1;

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + dt.Rows[0]["RTOLocationID"].ToString();
                //Utils.PopulateDropDownList(dropdownlistRtoName, SQLString.ToString(), CnnString, "--Select RTO Location--");
                //dropdownlistRtoName.SelectedIndex = dropdownlistRtoName.Items.Count - 1;
            }
             
        }
    }
}