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

namespace HSRP.Master
{
    public partial class GoodDispatchPlate : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string Mode;
        string UserID;
        string DispatchID;
        int StateID;
        int intStateID;
        int RTOLocationID;
        string UserType;
        int HSRPStateID; 
        int intDispatchID;
        int ProductID;
        int intRTOLocationID;
        int intRTOSUbLocationID;
        string RTONameForDropdown; 
        DateTime AuthorizationDate;
        DateTime OrderDate1;

         
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();  
            Mode = Request.QueryString["Mode"].ToString(); 
            UserType = Session["UserType"].ToString();
            HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
            UserID =  Session["UID"].ToString();
            

            if (!Page.IsPostBack)
            {
                dropdownStateName();
                 dropdown();
                 dropdownDispatchLocation();
                if (Mode == "Edit")
                {
                    LabelFormName.Text = "Edit Dispatch To RTO";
                    DispatchID = Request.QueryString["DispatchID"].ToString();
                    buttonUpdate.Visible = true;
                    btnSave.Visible = false;
                    BatchEdit(DispatchID); 
                    //dropdownDispatchLocation();
                }
                else
                {
                    LabelFormName.Text = "Goods Dispatch To RTO";
                    buttonUpdate.Visible = false;
                    btnSave.Visible = true;
                   // InitialSetting(); 
                    dropdown();
                }
            }
        }
        #region DropDown
 


        #endregion 
        public void dropdownStateName()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";

                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {

                HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"]);
                DropDownListStateName.Enabled = false;
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID= " + HSRPStateID + "and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                 DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
        }
        public void dropdown()
        {
            if (UserType == "0")
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intStateID);
                SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where HSRP_StateID='" + intStateID + "'  and ActiveStatus='Y' order by RTOLocationName ";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString, CnnString, "-- Select Embossing Location --");
                //DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
            }
            else
            {
                DropdownRTOName.Enabled = false;
                //RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
                RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
                SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where  RTOLocationID='" + RTOLocationID + "'  and  ActiveStatus='Y' order by RTOLocationName";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString, CnnString, "-- Select Embossing Location --");
                DropdownRTOName.SelectedIndex = DropdownRTOName.Items.Count - 1;
            }

        }
        public void dropdownDispatchLocation()
        {
            if (UserType == "0")
            {
                int.TryParse(DropdownRTOName.SelectedValue, out intRTOLocationID);
                SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where  distRelation='" + intRTOLocationID + "'  and  ActiveStatus='Y' order by RTOLocationName";
                Utils.PopulateDropDownList(DropdownRTOLocation, SQLString, CnnString, "-- Select Dispatch Location --");
                //DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
            }
            else
            {
                //DropdownRTOLocation.Enabled = false;
                RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
                //RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
                SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where  distRelation='" + RTOLocationID + "' and  ActiveStatus='Y' order by RTOLocationName";
                Utils.PopulateDropDownList(DropdownRTOLocation, SQLString, CnnString, "-- Select Dispatch Location --");
                //DropdownRTOLocation.SelectedIndex = DropdownRTOLocation.Items.Count - 1;
            }

        }

        public void dropdownAddress()
        {

            int.TryParse(DropdownRTOLocation.SelectedValue, out intRTOSUbLocationID);
            SQLString = "select RTOBillingAddress, RTOShippingAddress from RTOLocation where RTOLocationID='" + intRTOLocationID + "'";
             DataTable dt=   Utils.GetDataTable(SQLString, CnnString);
             if (dt.Rows.Count > 0)
             {
                 textboxBillingAddress.Text = dt.Rows[0]["RTOBillingAddress"].ToString();
                 textboxShippingAddress.Text = dt.Rows[0]["RTOShippingAddress"].ToString();
             } 
        }
       

        private void BatchEdit(string DispatchID)
        {
             
            SQLString = "select EmbossingLocation, DispatchLocation,BoxNo, HSRP_StateID, DispatchRefNo, BillingAddress,ShippingAddress,Remark from GoodDispatchPlate where GoodDispatchPlateID=" + DispatchID;
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);

            textboxBoxNo.Text = ds.Rows[0]["BoxNo"].ToString();
            textboxBoxNo.Enabled = false;
            textboxDespatchRefNo.Text = ds.Rows[0]["DispatchRefNo"].ToString();
            textboxBillingAddress.Text = ds.Rows[0]["BillingAddress"].ToString();
            textboxShippingAddress.Text = ds.Rows[0]["ShippingAddress"].ToString();
            textboxRemarks.Text = ds.Rows[0]["Remark"].ToString();

            SQLString = "select RTOLocationID, RTOLocationName from RTOLocation where RTOLocationID=" + ds.Rows[0]["DispatchLocation"];
            Utils.PopulateDropDownList(DropdownRTOLocation, SQLString, CnnString, "-- Select Dispatch Location --");
            DropdownRTOLocation.SelectedIndex = DropdownRTOLocation.Items.Count - 1;

            SQLString = "select RTOLocationID, RTOLocationName from RTOLocation where RTOLocationID=" + ds.Rows[0]["EmbossingLocation"];
            Utils.PopulateDropDownList(DropdownRTOName, SQLString, CnnString, "-- Select Dispatch Location --");
            DropdownRTOName.SelectedIndex = DropdownRTOName.Items.Count - 1;

            SQLString = "select HSRPStateName, HSRP_StateID from HSRPState where HSRP_StateID=" + ds.Rows[0]["HSRP_StateID"];
            Utils.PopulateDropDownList(DropDownListStateName, SQLString, CnnString, "-- Select Dispatch Location --");
            DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

        }

   

        protected void DropdownRTOName_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropdownDispatchLocation();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            int.TryParse(DropdownRTOName.SelectedValue, out intRTOLocationID);
            int.TryParse(DropdownRTOLocation.SelectedValue, out intDispatchID);
            int.TryParse(DropDownListStateName.SelectedValue, out intStateID);
            int IsExists = -1;
            BAL obj = new BAL();

            obj.InsertHSRPPGoodDispatchPlate(intStateID, intRTOLocationID, intDispatchID, textboxBoxNo.Text, textboxDespatchRefNo.Text, textboxBillingAddress.Text, textboxShippingAddress.Text, textboxRemarks.Text, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Update Successfully";

            }

        }

        protected void buttonUpdate_Click1(object sender, EventArgs e)
        {
            int.TryParse(DropdownRTOName.SelectedValue, out intRTOLocationID);
            int.TryParse(DropdownRTOLocation.SelectedValue, out intDispatchID);
            DispatchID = Request.QueryString["DispatchID"].ToString(); 
            int.TryParse(DropDownListStateName.SelectedValue, out intStateID);
            int IsExists = -1;
            BAL obj = new BAL();

            obj.UpdateHSRPPGoodDispatchPlate(intStateID, DispatchID, intRTOLocationID, intDispatchID, textboxDespatchRefNo.Text,  textboxBillingAddress.Text, textboxShippingAddress.Text, textboxRemarks.Text, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Update Successfully";

            }
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropdown();
        }

        protected void DropdownRTOLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropdownAddress();
        } 
    }
}