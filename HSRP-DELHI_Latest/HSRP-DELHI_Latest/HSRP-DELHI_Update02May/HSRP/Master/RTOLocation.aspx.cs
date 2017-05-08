using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HSRP;
using DataProvider;

namespace HSRP.Master
{
    public partial class RTOLocation : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty; 
        string UserType = string.Empty;
        int RTOLocationID;
        string Mode;
        string UserID;

        protected void Page_Load(object sender, EventArgs e)
        { 
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            } 
            else
            { 
                UserType = Session["UserType"].ToString(); 
            }

            if (!Page.IsPostBack)
            {
                textboxMobileNo.Attributes.Add("onkeypress", "return isNumberKey(event);");
                textboxLandlineNo.Attributes.Add("onkeypress", "return isNumberKey(event);");

                  Mode = Request.QueryString["Mode"];
                 UserID = Request.QueryString["UserID"];
                 RTOLocationID = Convert.ToInt16(Request.QueryString["RTOID"]);

                RTOLocationEdit(RTOLocationID);

                if (Mode == "New")
                {
                    DropdownStateName(UserType);
                } 
            }
        }

        private void DropdownStateName(string UserType)
        {
            if (UserType == "0")
            {

                dropdownListStateName.Enabled = true;
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order By HSRPStateName";
                Utils.PopulateDropDownList(dropdownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // dropdownListStateName.SelectedIndex = dropdownListStateName.Items.Count - 1; 
            }
            else
            {

                dropdownListStateName.Enabled = false;
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' and HSRP_StateID=" + Session["UserHSRPStateID"];
                Utils.PopulateDropDownList(dropdownListStateName, SQLString.ToString(), CnnString, "--Select State--");
               dropdownListStateName.SelectedIndex = dropdownListStateName.Items.Count - 1; 
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            lblSucMess.Text = "";
            lblErrMess.Text="";

            if (dropdownListStateName.Text != "--Select State--")
            {

                string ActiveStatus;
                if (checkBoxActiveStatus.Checked == true)
                {
                    ActiveStatus = "Y";
                }
                else
                {
                    ActiveStatus = "N";
                }
                string EmbossingStation;
                if ( checkBoxEmbossingStation.Checked == true)
                {
                    EmbossingStation = "Y";
                }
                else
                {
                    EmbossingStation = "N";
                }
                string LocationType = dropdownListLocationType.SelectedItem.Text;

                int IsExists = -1;
                int StateID = Convert.ToInt16(dropdownListStateName.Text);
                BAL obj = new BAL();
                obj.Insert_Master_HSRP_RTOLocation(StateID,LocationType, textboxRtoLocationName.Text, textboxShippingAddress.Text, textboxBillingAddress.Text, textboxRTOLocationCode.Text, textboxRTOLocationAddress.Text, textboxContactPersonName.Text, textboxMobileNo.Text, textboxLandlineNo.Text, textboxEmailId.Text, ActiveStatus,EmbossingStation, ref IsExists);
                if (IsExists.Equals(1))
                {
                    lblErrMess.Text = "Record Already Exist!!"; 
                }
                else
                {
                    lblSucMess.Text = "Record Update Successfully.";
                }
            }
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            lblSucMess.Text = "";
            lblErrMess.Text = "";
            RTOLocationID = Convert.ToInt16(Request.QueryString["RTOID"]);

            if (dropdownListStateName.Text != "--Select State--")
            {

                string ActiveStatus;
                if (checkBoxActiveStatus.Checked == true)
                {
                    ActiveStatus = "Y";
                }
                else
                {
                    ActiveStatus = "N";
                }

                string EmbossingStation;
                if (checkBoxEmbossingStation.Checked == true)
                {
                    EmbossingStation = "Y";
                }
                else
                {
                    EmbossingStation = "N";
                }
                string LocationType = dropdownListLocationType.SelectedItem.Text;

                int StateID = Convert.ToInt16(dropdownListStateName.Text);
                int IsExists = -1;
                BAL obj = new BAL();

                obj.Update_Master_HSRP_RTOLocation(RTOLocationID, StateID, LocationType, textboxRtoLocationName.Text, textboxShippingAddress.Text, textboxBillingAddress.Text, textboxRTOLocationCode.Text, textboxRTOLocationAddress.Text, textboxContactPersonName.Text, textboxMobileNo.Text, textboxLandlineNo.Text, textboxEmailId.Text, ActiveStatus, EmbossingStation, ref IsExists);
                if (IsExists.Equals(1))
                {
                    lblErrMess.Text = "Record Already Exist!!";
                }
                else
                {
                    lblSucMess.Text = "Record Update Successfully.";

                }
            }
        }

        private void RTOLocationEdit(int RTOLocationID)
        {
            if (Mode == "Edit")
            {
                buttonUpdate.Visible = true;
                ButtonSave.Visible = false;
                dropdownListStateName.Enabled = false; 
                SQLString = "select * from dbo.RTOLocation where RTOLocationID=" + RTOLocationID;
                // SQLString = "SELECT  RTOLocation.*, HSRPState.HSRPStateName  FROM RTOLocation INNER JOIN HSRPState ON RTOLocation.HSRP_StateID = HSRPState.HSRP_StateID where RTOLocation.RTOLocationID="+RTOLocationID;
                DataTable ds = Utils.GetDataTable(SQLString, CnnString);

                textboxRtoLocationName.Text = ds.Rows[0]["RTOLocationName"].ToString();
                textboxRTOLocationCode.Text = ds.Rows[0]["RTOLocationCode"].ToString();
                dropdownListLocationType.SelectedItem.Text = ds.Rows[0]["LocationType"].ToString(); 
                textboxRTOLocationAddress.Text = ds.Rows[0]["RTOLocationAddress"].ToString();

                textboxShippingAddress.Text = ds.Rows[0]["RTOShippingAddress"].ToString();
                textboxBillingAddress.Text = ds.Rows[0]["RTOBillingAddress"].ToString(); 
                textboxContactPersonName.Text = ds.Rows[0]["ContactPersonName"].ToString();
                textboxMobileNo.Text = ds.Rows[0]["MobileNo"].ToString();
                textboxLandlineNo.Text = ds.Rows[0]["LandlineNo"].ToString();
                textboxEmailId.Text = ds.Rows[0]["EmailID"].ToString();
                string ActiveStatus = ds.Rows[0]["ActiveStatus"].ToString();
                if (ActiveStatus == "Y")
                {
                    checkBoxActiveStatus.Checked = true;
                }
                else
                {
                    checkBoxActiveStatus.Checked = false;
                }

                string EmbossingStation = ds.Rows[0]["IsEmbossingStation"].ToString();
                if (EmbossingStation == "Y")
                {
                    checkBoxEmbossingStation.Checked = true;
                }
                else
                {
                    checkBoxEmbossingStation.Checked = false;
                }
                 
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' and HSRP_StateID=" + ds.Rows[0]["HSRP_StateID"].ToString();
                Utils.PopulateDropDownList(dropdownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                dropdownListStateName.SelectedIndex = dropdownListStateName.Items.Count - 1;
 
            }
            else
            {
                buttonUpdate.Visible = false;
                ButtonSave.Visible = true;
            }
        }

        

         
    }
}