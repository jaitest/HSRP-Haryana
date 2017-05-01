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

namespace HSRP.Transaction
{
    public partial class Inventory : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string Mode = string.Empty;
        string UserID;
        string InventoryID;
        int StateID;
        int RTOLocationID;
        string UserType;
        int HSRPStateID; 
        int intHSRPStateID;
        int ProductID;
        int intRTOLocationID;
        string RTONameForDropdown; 
        DateTime AuthorizationDate;
        DateTime OrderDate1;

         
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();  
           // Mode = Request.QueryString["Mode"].ToString();
            HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
             UserID =  Session["UID"].ToString();
            

            if (!Page.IsPostBack)
            {
                 TextBoxLaserNoError.Visible = false; 
              UserType = Session["UserType"].ToString();
                FilldropDownListStateName();
                FilldropDownListRTOLocation();
                dropdownPrefixLaserNo();

                LabelFormName.Text = "Add New Inventory";
                //buttonUpdate.Visible = false;
                btnSave.Visible = true;
                dropdown();
               
 
            }
        }
        #region DropDown

        private void FilldropDownListStateName()
        {
            UserType = Session["UserType"].ToString();
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName"; 
                Utils.PopulateDropDownList(DropdownStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID= " + HSRPStateID + "and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropdownStateName, SQLString.ToString(), CnnString, "--Select State--");
                DropdownStateName.SelectedIndex = DropdownStateName.Items.Count - 1;
            }
        }

        private void FilldropDownListRTOLocation()
        {
            UserType = Session["UserType"].ToString();
            if (UserType == "0")
            {
                int.TryParse(DropdownStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
            }
            else
            {
                SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where RTOLocationID = " + RTOLocationID + "and ActiveStatus='Y'";
                Utils.PopulateDropDownList(DropdownRTOName, SQLString.ToString(), CnnString, "--Select RTO Location--");
                DropdownRTOName.SelectedIndex = DropdownRTOName.Items.Count - 1;
            }

        }


        #endregion
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserType = Session["UserType"].ToString();
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            LabelUpdated.Text = "";
            lblTotalRecord.Text = "";

            //if (textboxLaserCodeFrom.MaxLength < 8)
            //{
            //    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Please Enter Min 8 Length');</script>");
            //    return;
            //}

            Int64 LaserCodeTo = Convert.ToInt64(textboxLaserCodeFrom.Text )+ Convert.ToInt64( textboxQuantity.Text)-1; 
            
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
                string LaserNo = DropDownListPrefixLaserNo.SelectedItem.Text +Laser.ToString();
                obj.InsertHSRPPInventory(ProductID, UserID,textboxLaserCodeFrom.Text, Prefix, textboxQuantity.Text, CurrentCost, Weight, textboxInvNo.Text, textboxInvFrom.Text, intHSRPStateID, intRTOLocationID, Remarks, Laser, LaserNo, orderStatus, ref IsExists);
                if (IsExists.Equals(1))
                {
                    LabelUpdated.Text = "Total Exist Record :";
                    lblExist.Visible=true;


                    TextBoxLaserNoError.Visible = true;

                    SQLString = "select  LaserNo, (select hsrpStateName from hsrpstate where hsrp_StateID= a.hsrp_StateID) as StateName from rtoinventory a where LaserNo='" + LaserNo + "'";
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
                    TextBoxLaserNoError.Visible = true;
                    lblSucMess.Visible = true;
                    //lblExist.Text="";
                    totalRecord = totalRecord + 1;
                    LabelUpdated.Text = "Total Updated Record :";
                    lblTotalRecord.Text = totalRecord.ToString();
                     
                }
            }
            
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

        protected void DropdownRTOName_SelectedIndexChanged(object sender, EventArgs e)
        {

        } 
    }
}