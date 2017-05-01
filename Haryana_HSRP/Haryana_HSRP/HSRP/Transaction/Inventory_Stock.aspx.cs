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

namespace HSRP.Transaction
{
    public partial class Inventory_Stock : System.Web.UI.Page
    {
     
        
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
             
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string Mode = string.Empty;
        string UserID;
        string InventoryID;
        int StateID;
      
        int UserType;
      
        int intHSRPStateID;
        int ProductID;
        int intRTOLocationID;
        string RTONameForDropdown;
        DateTime AuthorizationDate;
        DateTime OrderDate1;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
               
                // Mode = Request.QueryString["Mode"].ToString();
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                // Mode = Request.QueryString["Mode"].ToString();

                HSRP_StateID = Session["UserHSRPStateID"].ToString();
              
                UserID = Session["UID"].ToString();

                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType.Equals(0))
                        {
                          

                            TextBoxLaserNoError.Visible = false;
                            DropdownStateName.Visible = true;
                            DropdownRTOName.Visible = true;
                           
                            FilldropDownListStateName();
                            FilldropDownListRTOLocation();
                            dropdownPrefixLaserNo();

                            LabelFormName.Text = " Add Inventory Stock";
                            //buttonUpdate.Visible = false;
                            btnSave.Visible = true;
                            dropdown(); 

                          

                        }
                        else
                        {

                            TextBoxLaserNoError.Visible = false;
                            DropdownStateName.Visible = true;
                            DropdownRTOName.Visible = true;
                         
                            FilldropDownListStateName();
                            FilldropDownListRTOLocation();
                            dropdownPrefixLaserNo();

                            LabelFormName.Text = " Add Inventory Stock";
                            //buttonUpdate.Visible = false;
                            btnSave.Visible = true;
                            dropdown(); 
                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
               
            }

        }
        #region DropDown

        

        private void FilldropDownListStateName()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropdownStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropdownStateName.DataSource = dts;
                DropdownStateName.DataBind();
            }
        }

        private void FilldropDownListRTOLocation()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
            }
            else
            {
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                //Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                DropdownRTOName.DataSource = dss;
                DropdownRTOName.DataBind();
            }
        }
        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
          
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            LabelUpdated.Text = "";
            lblTotalRecord.Text = "";
            TextBoxLaserNoError.Text = "";
            TextBoxLaserNoError.Visible = false;

             
            Int64 LaserCodeTo = Convert.ToInt64(textboxLaserCodeFrom.Text) + Convert.ToInt64(textboxQuantity.Text) - 1;

            int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(DropdownRTOName.SelectedValue, out intRTOLocationID);

            int BatchCheckedBy = Convert.ToInt16(Session["UID"]);
            string orderStatus = "New Order";
            int.TryParse(DropDownListProductName.SelectedValue, out ProductID);
            //  string Prefix = textboxPrefixFrom.Text;
            string Prefix = DropDownListPrefixLaserNo.SelectedItem.Text;
            Int64 LaserCodeFrom = Convert.ToInt64(textboxLaserCodeFrom.Text);
            decimal CurrentCost = 50;
            decimal Weight = 50;
            string Remarks = textboxRemarks.Text;
            DateTime CreateDateTime = System.DateTime.Now;
            int totalPlateInbox = Convert.ToInt32(LaserCodeTo) - Convert.ToInt32(textboxLaserCodeFrom.Text);
            BAL obj = new BAL();
            int IsExists = -1;

            int totalRecord = 0;
            int TotalExistRecord = 0;
            Int64 Laser = Convert.ToInt32(textboxLaserCodeFrom.Text) - 1;
            for (int i = 0; i <= totalPlateInbox; i++)
            {
                Laser = +Laser + 1;
                string LaserNo = DropDownListPrefixLaserNo.SelectedItem.Text + Laser.ToString();
                obj.InsertHSRPPInventory1(ProductID, UserID, textboxLaserCodeFrom.Text, Prefix, textboxQuantity.Text, CurrentCost, Weight, intHSRPStateID, intRTOLocationID, Remarks, Laser, LaserNo, orderStatus, ref IsExists);
                if (IsExists.Equals(1))
                {
                    LabelUpdated.Text = "Total Exist Record :";
                    lblExist.Visible = true;


                    TextBoxLaserNoError.Visible = true;

                    SQLString = "select  LaserNo, (select hsrpStateName from hsrpstate where hsrp_StateID= a.hsrp_StateID) as StateName from rtoinventory a where a.LaserNo='" + LaserNo + "'";
                    DataTable InventoryState = Utils.GetDataTable(SQLString.ToString(), CnnString);
                    if (InventoryState.Rows.Count > 0)
                    {
                        //TextBoxLaserNoError.Text += (LaserNo).ToString();

                        TextBoxLaserNoError.Text += InventoryState.Rows[0]["LaserNo"].ToString() + "   " + InventoryState.Rows[0]["StateName"].ToString();
                    }
                    TextBoxLaserNoError.Text += System.Environment.NewLine;
                    lblTotalRecord.Text = totalRecord.ToString();


                }
                else
                {
                    // lblExist.Visible=false;
                    //TextBoxLaserNoError.Visible = true;
                    lblSucMess.Visible = true;
                    //lblExist.Text="";
                    totalRecord = totalRecord + 1;
                    LabelUpdated.Text = "Total Updated Record :";
                    lblTotalRecord.Text = totalRecord.ToString();

                }
            }
            textboxLaserCodeFrom.Text = "";
            btnSave.Enabled = true;
            lblErrMess.Text = "";
        }
        public void dropdownPrefixLaserNo()
        {

            int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID);
            SQLString = "Select PrefixID, Prefix from PrefixLaserNo where ActiveStatus='Y' and HSRP_StateID='" + intHSRPStateID + "' order by Prefix ";
            Utils.PopulateDropDownList(DropDownListPrefixLaserNo, SQLString, CnnString, "-- Select Prefix --");
            //DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
        }

        public void dropdown()
        {
            int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID);
            SQLString = "select ProductCode, ProductID from Product where ActiveStatus='Y' and HSRP_StateID='" + intHSRPStateID + "' order by ProductCode ";
            Utils.PopulateDropDownList(DropDownListProductName, SQLString, CnnString, "-- Select Product --");
            //DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
        }

        protected void DropdownStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListRTOLocation();
            dropdownPrefixLaserNo();
            dropdown();
        }

        protected void ButtionSave_Click(object sender, EventArgs e)
        {

            int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(DropdownRTOName.SelectedValue, out intRTOLocationID);
            string orderStatus = "New Order";
            int.TryParse(DropDownListProductName.SelectedValue, out ProductID);

            string Prefix = DropDownListPrefixLaserNo.SelectedItem.Text;

            decimal CurrentCost = 50;
            decimal Weight = 50;
            string Remarks = textboxRemarks.Text;

            BAL obj = new BAL();
            int IsExists = -1;

            //obj.HSRPPInventoryLaserNo(ProductID, Session["UID"].ToString(), textboxLaserCodeFrom.Text, Prefix, textboxQuantity.Text, CurrentCost, Weight, textboxInvNo.Text, textboxInvFrom.Text, intHSRPStateID, intRTOLocationID, Remarks, textboxLaserCodeFrom.Text, orderStatus, ref IsExists);


        } 
    }
}