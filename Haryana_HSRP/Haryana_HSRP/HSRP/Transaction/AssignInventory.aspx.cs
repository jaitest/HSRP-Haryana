using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DataProvider;

namespace HSRP.Master
{
    public partial class AssignInventory : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType;
        int HSRPStateID;
        int RTOLocationID;
        string Prefix;
        int intPrefixID;
        int intBatchID;
        int intProductID;
         int intHSRPStateID;
         int intRTOLocationID;
         string Type;
         string OrderStatus = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType =  Session["UserType"].ToString();
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                 

                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress; 
                if (!IsPostBack)
                { 
                    try
                    {
                        RTOLocation();
                        FilldropDownRTO();
                        SQLString = "select ProductCode, ProductID from Product where ActiveStatus='Y' and HSRP_StateID='" + HSRPStateID + "'";
                        Utils.PopulateDropDownList(dropdownListProduct, SQLString.ToString(), CnnString, "--Select Product--");
                        // dropdownListStateName.SelectedIndex = dropdownListStateName.Items.Count - 1; 
                        product();
                        FilldropDownRTO();
                        PrefixName();
                        RTOLocation();
                    }
                    catch 
                    {
                         
                    }
                }
            }
        }

        protected void dropdownListProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            product();
        }
        private void product()
        { 
            int.TryParse(dropdownListProduct.SelectedValue, out intProductID);
            SQLString = "select BatchCode, BatchID from Batch where ProductID=" + intProductID;
            Utils.PopulateDropDownList(dropdownListBatch, SQLString.ToString(), CnnString, "--Select Batch--");
            //dropdownListBatch.SelectedIndex = dropdownListPrefix.Items.Count - 1;
        }
        private void FilldropDownRTO()
        { 

                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropdownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                //dropdownListStateName.SelectedIndex = dropdownListStateName.Items.Count - 1;

        }
        private void PrefixName()
        {
            int.TryParse(dropdownListBatch.SelectedValue, out intPrefixID);
            //SQLString = "select Prefix, InventoryID from RTOInventory where BatchID=" + intPrefixID;
            SQLString = "select distinct Prefix, BatchID from RTOInventory where BatchID=" + intPrefixID;
            Utils.PopulateDropDownList(dropdownListPrefix, SQLString.ToString(), CnnString, "--Select Prefix--");
           // dropdownListPrefix.SelectedIndex = dropdownListPrefix.Items.Count - 1;

        }
        private void RTOLocation()
        { 
        }

        protected void dropdownListBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrefixName();
        }

        protected void dropdownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            RTOLocation();
        }

        protected void dropdownListLocationType_SelectedIndexChanged(object sender, EventArgs e)
        {
             Type = dropdownListLocationType.SelectedItem.Text; 
             int.TryParse(dropdownListStateName.SelectedValue, out intHSRPStateID);
            //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where LocationType='" + Type + "' and ";
             SQLString = "select * from RTOLocation where LocationType='" + Type + "'and HSRP_StateID='" + intHSRPStateID + "'";
            Utils.PopulateDropDownList(dropdownListLocation, SQLString.ToString(), CnnString, "--Select RTO Location--");
            //dropdownListLocationType.SelectedIndex = dropdownListLocationType.Items.Count - 1;
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            int.TryParse(dropdownListBatch.SelectedValue, out intBatchID);
            int.TryParse(dropdownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropdownListLocation.SelectedValue, out intRTOLocationID);
            string Prefix = dropdownListPrefix.SelectedItem.Text;  
            Int64 LaserNoFrom = Convert.ToInt64( TextBoxLaserFrom.Text)-1;
            Int64 LaserNoTo = Convert.ToInt64( TextBoxLaserTo.Text);
            Int64 totalRecord = LaserNoTo - LaserNoFrom;
            BAL obj = new BAL();
            Int64 LaserNo=0;
            
            for(int i=1; i<=totalRecord; i++)
            {
                LaserNo = LaserNo + 1;
                Int64 LaserNoWithoutPrefix = LaserNoFrom + LaserNo;
                string LaserNotUpdated = Prefix + LaserNoWithoutPrefix.ToString();
                int IsExists = -1;
                obj.UpdateAssignInventory(intHSRPStateID, intRTOLocationID, intBatchID, Prefix, LaserNoWithoutPrefix);
                if (IsExists.Equals(1))
                {
                    LabelUpdated.Text = "Total Exist Record :";
                    lblExist.Visible = true; 
                }
                else
                {
                    Int64  UpdateRecord = +LaserNo;
                    LabelUpdated.Text = "Total Updated Record :";
                    lblTotalRecord.Text = UpdateRecord.ToString();
                }
            }

        }

        protected void dropdownListLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
         
    }
}