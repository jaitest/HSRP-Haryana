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
    public partial class RTOInventory : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string Mode;
        string UserID;
        string BatchID;
        int StateID;
        int RTOLocationID;
        string UserType;
        int HSRPStateID;  
        int ProductID;
        int intRTOLocationID;
        string RTONameForDropdown; 
        DateTime AuthorizationDate;
        DateTime OrderDate1;

         
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();  
            Mode = Request.QueryString["Mode"].ToString();

            HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
             UserID =  Session["UID"].ToString();
            if (!Page.IsPostBack)
            { 
                if (Mode == "Edit")
                {
                    BatchID =  Request.QueryString["InventoryID"].ToString();
                    buttonUpdate.Visible = true;
                    btnSave.Visible = false;
                    BatchEdit(BatchID); 
                }
                else
                {
                    buttonUpdate.Visible = false;
                    btnSave.Visible = true; 
                    dropdown();
                }
            } 
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           
                lblErrMess.Text = "";
                lblSucMess.Text = "";
                LabelUpdated.Text = "";
                lblTotalRecord.Text = "";

                String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                int BatchCheckedBy = Convert.ToInt16( Session["UID"]);
                string orderStatus = "New";
                int.TryParse(DropDownListProductName.SelectedValue, out ProductID);
                string BatchCode = textboxBatchCode.Text;
                string Prefix = textboxPrefixFrom.Text;
                Int64 LaserCodeFrom = Convert.ToInt64(textboxLaserCodeFrom.Text);
                Int64 LaserCodeTo = Convert.ToInt64(textboxLaserCodeTo.Text);
                DateTime DateOfManufacturing = Convert.ToDateTime(OrderDate.SelectedDate);
                decimal CurrentCost = Convert.ToDecimal(textboxCurrentCost.Text);
                decimal Weight = Convert.ToDecimal(textboxWeight.Text);
                int TotalBoxUnits = Convert.ToInt16(textboxTotalBoxUnits.Text);
                int NoOfPlateInBoxUnit = Convert.ToInt16(textboxNoofPlateinBox.Text);
                string Remarks = textboxRemarks.Text;
                DateTime CreateDateTime = System.DateTime.Now;
                int totalPlateInbox = Convert.ToInt32(textboxLaserCodeTo.Text) - Convert.ToInt32(textboxLaserCodeFrom.Text);
                BAL obj = new BAL();
                int IsExists = -1;
                int BatchID;
                obj.InsertHSRPPBatch(ProductID, BatchCode, Prefix, LaserCodeFrom, LaserCodeTo, OrderDate1, Weight, BatchCheckedBy, CreateDateTime, Remarks, ref IsExists);
                if (IsExists.Equals(1))
                {
                    lblErrMess.Text = "Record Already Exist!!"; 
                }
                else
                {
                    lblSucMess.Text = "Record Save Successfully"; 
                } 
                SQLString = "select MAX(BatchID) as BatchID from Batch";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                BatchID = Convert.ToInt16(dt.Rows[0]["InventoryID"]); 
                int totalRecord = 0;
                int Laser = Convert.ToInt32(textboxLaserCodeFrom.Text) - 1;
                for (int i = 0; i <= totalPlateInbox; i++)
                {
                    Laser = +Laser + 1;
                    string LaserNo = textboxPrefixFrom.Text + Laser.ToString();
                    obj.InsertRTOInventory(ProductID, UserID, BatchID,Prefix, Laser, LaserNo, HSRPStateID, RTOLocationID, orderStatus, Remarks, ref IsExists);
                    if (IsExists.Equals(1))
                    {
                        LabelUpdated.Text = "Total Updated Record :";
                        lblTotalRecord.Text = totalRecord.ToString();
                    }
                    else
                    {
                        totalRecord = +totalRecord + 1;
                        LabelUpdated.Text = "Total Updated Record :";
                        lblTotalRecord.Text = totalRecord.ToString();
                    }
                }
        }
        public void dropdown()
        {

            int.TryParse(DropDownListProductName.SelectedValue, out ProductID);
            SQLString = "select ProductCode, ProductID from Product where ActiveStatus='Y' and HSRP_StateID='" + HSRPStateID + "' order by ProductCode ";
            Utils.PopulateDropDownList(DropDownListProductName, SQLString, CnnString, "-- Select Product --");
            //DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
        } 
        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";

            BatchID = Request.QueryString["InventoryID"].ToString(); 
            int.TryParse(DropDownListProductName.SelectedValue, out ProductID);  
            int BatchCode = Convert.ToInt16(textboxBatchCode.Text);
            string LaserCodeFrom = textboxLaserCodeFrom.Text;
            string LaserCodeTo = textboxLaserCodeTo.Text;
            DateTime DateOfManufacturing = Convert.ToDateTime(OrderDate.SelectedDate);
            decimal CurrentCost = Convert.ToDecimal(textboxCurrentCost.Text);
            decimal Weight = Convert.ToDecimal(textboxWeight.Text);
            int TotalBoxUnits = Convert.ToInt16(textboxTotalBoxUnits.Text);
            int NoOfPlateInBoxUnit = Convert.ToInt16(textboxNoofPlateinBox.Text);
            string Remarks = textboxRemarks.Text; 
            DateTime CreateDateTime = System.DateTime.Now;

            int IsExists = -1;
            BAL obj = new BAL();
          //  obj.UpdateHSRPPBatch(BatchID, ProductID, BatchCode, LaserCodeFrom, LaserCodeTo, DateOfManufacturing, CurrentCost, Weight, TotalBoxUnits, NoOfPlateInBoxUnit, Remarks, CreateDateTime, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Save Successfully";

            }

        } 
         
        private void BatchEdit(string BatchID)
        {

            SQLString = "SELECT Batch.BatchCode, Product.ProductCode, Batch.ProductID,Batch.Prefix, Batch.LaserCodeFrom, Batch.LaserCodeTo, Batch.DateOfManufacturing, Batch.CurrentCost, Batch.Weight, Batch.TotalBoxUnits, Batch.NoOfPlateInBoxUnit,  Batch.Remarks, Batch.CreateDateTime FROM Batch INNER JOIN Product ON Batch.ProductID = Product.ProductID where BatchID =" + BatchID;
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            if (ds.Rows.Count > 0)
            {
                textboxBatchCode.Text = ds.Rows[0]["BatchCode"].ToString();
                textboxPrefixFrom.Text = ds.Rows[0]["Prefix"].ToString();
                textboxLaserCodeFrom.Text = ds.Rows[0]["LaserCodeFrom"].ToString();
                textboxLaserCodeTo.Text = ds.Rows[0]["LaserCodeTo"].ToString();
                OrderDate.SelectedDate = Convert.ToDateTime(ds.Rows[0]["DateOfManufacturing"]);
                textboxCurrentCost.Text = ds.Rows[0]["CurrentCost"].ToString();
                textboxWeight.Text = ds.Rows[0]["Weight"].ToString();
                textboxTotalBoxUnits.Text = ds.Rows[0]["TotalBoxUnits"].ToString();
                textboxNoofPlateinBox.Text = ds.Rows[0]["NoOfPlateInBoxUnit"].ToString();
                textboxRemarks.Text = ds.Rows[0]["Remarks"].ToString();
                SQLString = "select * from dbo.Product where productID=" + ds.Rows[0]["ProductID"];
                Utils.PopulateDropDownList(DropDownListProductName, SQLString, CnnString, "-- Select Product --");
                DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
            }
        } 
    }
}