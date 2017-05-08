using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataProvider;


namespace HSRP.Master
{
    public partial class LaserRejectPlate : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
       
        string HSRPRecordID = string.Empty;
        string Mode;
        string orderstatus;
        string frontLaser;
        string RearLaser;
        string UserID;
          
        string strUserID = string.Empty;
        string ComputerIP = string.Empty; 
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname, pp;
        string transtr, statename1; 
        string fontpath;

        protected void Page_Load(object sender, EventArgs e)
        {
             
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            if (!Page.IsPostBack)
            {
                Mode = Request.QueryString["Mode"];
                UserID = Session["UID"].ToString();  
                
                UserType = Session["UserType"].ToString();
                FilldropDownListHSRPState();
                FilldropDownListRTOLocation();
            }
        }

        #region DropDown

        private void FilldropDownListHSRPState()
        {
            
            UserType = Session["UserType"].ToString();
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListHSRPState, SQLString.ToString(), CnnString, "--Select State--");
                dropDownListHSRPState.SelectedIndex = 0;
            }
            else
            {
                dropDownListHSRPState.Enabled = false;
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + Session["UserHSRPStateID"].ToString() + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                dropDownListHSRPState.DataSource = dss;
                dropDownListHSRPState.DataBind();

                 FilldropDownListRTOLocation();
            }

        }

        private void FilldropDownListRTOLocation()
        {
            UserType = Session["UserType"].ToString();
            if (dropDownListHSRPState.SelectedValue != "--Select State--")
            {
                if (UserType == "0")
                {

                    SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + dropDownListHSRPState.SelectedValue.ToString() + " and ActiveStatus!='N' Order by RTOLocationName";
                    //SQLString = "select RTOLocation.RTOLocationName,RTOLocation.RTOLocationID from RTOLocation inner join UserRTOLocationMapping on RTOLocation.RTOLocationID=UserRTOLocationMapping.RTOLocationID Where RTOLocation.HSRP_StateID=" + intHSRPStateID + " and UserRTOLocationMapping.UserID='" + strUserID + "' and ActiveStatus!='N' Order by RTOLocation.RTOLocationName";

                }
                else
                {
                    dropDownListRTOLocation.Visible = true;
                    labelRTOLocation.Visible = true;
                    UpdateRTOLocation.Update();

                   // SQLString = "select RTOLocation.RTOLocationName,RTOLocation.RTOLocationID from RTOLocation inner join UserRTOLocationMapping on RTOLocation.RTOLocationID=UserRTOLocationMapping.RTOLocationID Where RTOLocation.HSRP_StateID=" + dropDownListHSRPState.SelectedValue.ToString() + " and UserRTOLocationMapping.UserID='" + strUserID + "' and ActiveStatus!='N' Order by RTOLocation.RTOLocationName";
                    string UserID = Convert.ToString(Session["UID"]);
                    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";

                }
                Utils.PopulateDropDownList(dropDownListRTOLocation, SQLString.ToString(), CnnString, "--Select RTO Location--");
                dropDownListRTOLocation.SelectedIndex = 0;
            }
        }

        protected void dropDownListHSRPState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListHSRPState.SelectedItem.Text != "--Select State--")
            {
                dropDownListRTOLocation.Visible = true;
                labelRTOLocation.Visible = true;
                FilldropDownListRTOLocation();
                UpdateRTOLocation.Update();
            }
            else
            {
                 
                dropDownListHSRPState.Visible = false;
                labelRTOLocation.Visible = false;
                UpdateRTOLocation.Update();
               
            }
        }


        #endregion

        protected void buttonRejectedSave_Click(object sender, EventArgs e)
        {
            UserID = Session["UID"].ToString();

            SQLString = "select inventoryStatus from rtoinventory where laserNo='" + txtLaserNoRejected.Text.Trim().ToUpper() + "'";
            DataTable dcheck = Utils.GetDataTable(SQLString.ToString(), CnnString);
            if (dcheck.Rows.Count > 0)
            {
                if (dcheck.Rows[0]["inventoryStatus"].ToString() == "New Order")
                {

                    SQLString = "update rtoinventory set InventoryStatus='Reject',RejectDate=GetDate(), UserID='" + UserID + "', Remarks='" + txtRemarks.Text + "' where inventoryStatus='New Order' and LaserNo='" + txtLaserNoRejected.Text + "' and  hsrp_StateID='" + dropDownListHSRPState.SelectedValue.Trim() + "' ";
                    int i = Utils.ExecNonQuery(SQLString.ToString(), CnnString);
                    if (i > 0)
                    {
                        lblErrMess.Text = "";
                        lblSucMess.Text = "Record Updated Successfully.";
                        txtLaserNoRejected.Text = "";
                        txtRemarks.Text = "";
                        FilldropDownListHSRPState();
                        FilldropDownListRTOLocation();
                    }
                }
                else
                {
                    lblErrMess.Text = "Laser No Already " + dcheck.Rows[0]["inventoryStatus"].ToString();
                }
            }
            else
            {
                lblErrMess.Text = "Enter Current Laser No.";
            }
        }

       
          
    }
}