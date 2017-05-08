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
    public partial class Batch : System.Web.UI.Page
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
        int intHSRPStateID;
        int ProductID;
        int intRTOLocationID;
        string RTONameForDropdown; 
        DateTime AuthorizationDate;
        DateTime OrderDate1;

         
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();  
            Mode = Request.QueryString["Mode"].ToString();
            lblExist.Visible = false;
            TextBoxLaserNoError.Visible = false;
            UserType = Session["UserType"].ToString();
            HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
             UserID =  Session["UID"].ToString();
            

            if (!Page.IsPostBack)
            {
                
                dropdownPrefix();
                
                if (Mode == "Edit")
                {
                    LabelFormName.Text = "Batch Laser Assigen";
                    BatchID =  Request.QueryString["BatchID"].ToString();
                    buttonUpdate.Visible = true;
                    btnSave.Visible = false;
                    BatchEdit(BatchID); 
                }
                else
                {
                    LabelFormName.Text = "Add New Batch";
                    buttonUpdate.Visible = false;
                    btnSave.Visible = true;
                    InitialSetting(); 
                    dropdown();
                }
            }
        }
        #region DropDown
 
        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            LabelUpdated.Text = "";
            lblTotalRecord.Text = "";

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

            int BatchCheckedBy = Convert.ToInt16(Session["UID"]);
            string orderStatus = "New";
            int.TryParse(DropDownListProductName.SelectedValue, out ProductID);
            string BatchCode = textboxBatchCode.Text;
            string Prefix = DropDownListPrefix.SelectedItem.Text;

            Int64 LaserCodeFrom = Convert.ToInt64(textboxLaserCodeFrom.Text);
            Int64 LaserCodeTo = Convert.ToInt64(textboxLaserCodeTo.Text);

            DateTime DateOfManufacturing = Convert.ToDateTime(OrderDate.SelectedDate);


            decimal Weight = Convert.ToDecimal(textboxWeight.Text);
            string Remarks = textboxRemarks.Text;
            DateTime CreateDateTime = System.DateTime.Now;
            int totalPlateInbox = Convert.ToInt32(textboxLaserCodeTo.Text) - Convert.ToInt32(textboxLaserCodeFrom.Text);
            BAL obj = new BAL();
            int IsExists = -1;
            // int BatchID;
            obj.InsertHSRPPBatch(ProductID, BatchCode, Prefix, LaserCodeFrom, LaserCodeTo, OrderDate1, Weight, BatchCheckedBy, CreateDateTime, Remarks, ref IsExists);
            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Save Successfully";
            }

            //SQLString = "select MAX(BatchID) as BatchID from Batch";
            //DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            //BatchID = Convert.ToInt16(dt.Rows[0]["BatchID"]);
            //int totalRecord = 0;
            //int TotalExistRecord = 0;
            //int Laser = Convert.ToInt32(textboxLaserCodeFrom.Text) - 1;
            //for (int i = 0; i <= totalPlateInbox; i++)
            //{
            //    Laser = +Laser + 1;
            //    string LaserNo = textboxPrefixFrom.Text + Laser.ToString();
            //    obj.InsertRTOInventory(ProductID, UserID, BatchID, Prefix, Laser, LaserNo, HSRPStateID, RTOLocationID, orderStatus, Remarks, ref IsExists);
            //    if (IsExists.Equals(1))
            //    {
            //        LabelUpdated.Text = "Total Exist Record :";
            //        lblExist.Visible = true;
            //        TextBoxLaserNoError.Visible = true;
            //        TextBoxLaserNoError.Text += (LaserNo).ToString();
            //        TextBoxLaserNoError.Text += System.Environment.NewLine;

            //        lblTotalRecord.Text = totalRecord.ToString();
            //    }
            //    else
            //    {
            //        totalRecord = +totalRecord + 1;
            //        LabelUpdated.Text = "Total Updated Record :";
            //        lblTotalRecord.Text = totalRecord.ToString();
            //    }
            //}
        } 
        public void dropdown()
        {

            SQLString = "select ProductCode, ProductID from Product where ActiveStatus='Y' and HSRP_StateID='" + HSRPStateID + "' order by ProductCode ";
            Utils.PopulateDropDownList(DropDownListProductName, SQLString, CnnString, "-- Select Product --");
            //DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
        }
        public void dropdownPrefix()
        {

            SQLString = "select distinct Prefix from prefixLaserNo order by Prefix";
            Utils.PopulateDropDownList(DropDownListPrefix, SQLString, CnnString, "-- Select Prefix --");
            //DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {

            lblErrMess.Text = "";
            lblSucMess.Text = "";
            LabelUpdated.Text = "";
            lblTotalRecord.Text = "";

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

            int BatchCheckedBy = Convert.ToInt16(Session["UID"]);
            string orderStatus = "New";
            int.TryParse(DropDownListProductName.SelectedValue, out ProductID);
            string BatchCode = textboxBatchCode.Text;
            string Prefix = DropDownListPrefix.SelectedItem.Text;

            Int64 LaserCodeFrom = Convert.ToInt64(textboxLaserCodeFrom.Text);
            Int64 LaserCodeTo = Convert.ToInt64(textboxLaserCodeTo.Text);

            DateTime DateOfManufacturing = Convert.ToDateTime(OrderDate.SelectedDate);


            decimal Weight = Convert.ToDecimal(textboxWeight.Text);
            string Remarks = textboxRemarks.Text;
            DateTime CreateDateTime = System.DateTime.Now;
            
            BAL obj = new BAL();
            int IsExists = -1;
            BatchID = Request.QueryString["BatchID"].ToString();
            obj.UpdateHSRPPBatch(BatchID,ProductID, BatchCode, Prefix, LaserCodeFrom, LaserCodeTo, OrderDate1, Weight, BatchCheckedBy, CreateDateTime, Remarks, ref IsExists);
            
            //obj.UpdateHSRPPBatch(Prefix, intHSRPStateID, intRTOLocationID);

            if (IsExists.Equals(1))
            {
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Update Successfully";

            }

        } 
        private void InitialSetting()
        { 
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

           // HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
           // HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        private void BatchEdit(string BatchID)
        {
            
            
            //DropDownListProductName.Enabled=false;
            //textboxBatchCode.Enabled=false;
            
            //textboxLaserCodeFrom.Enabled=false;
            //textboxWeight.Enabled=false;
            //textboxLaserCodeTo.Enabled=false;
            //textboxCurrentCost.Enabled=false;
            //textboxTotalBoxUnits.Enabled=false;
            //textboxNoofPlateinBox.Enabled = false;
            //textboxRemarks.Enabled = false;
            
            SQLString = "SELECT Batch.BatchCode, Product.ProductCode, Batch.ProductID,Batch.Prefix, Batch.LaserCodeFrom, Batch.LaserCodeTo, Batch.DateOfManufacturing, Batch.CurrentCost, Batch.Weight, Batch.TotalBoxUnits, Batch.NoOfPlateInBoxUnit,  Batch.Remarks, Batch.CreateDateTime FROM Batch INNER JOIN Product ON Batch.ProductID = Product.ProductID where BatchID =" + BatchID;
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);

            textboxBatchCode.Text = ds.Rows[0]["BatchCode"].ToString();
            textboxLaserCodeFrom.Text = ds.Rows[0]["LaserCodeFrom"].ToString();
            textboxLaserCodeTo.Text = ds.Rows[0]["LaserCodeTo"].ToString();
            OrderDate.SelectedDate = Convert.ToDateTime(ds.Rows[0]["DateOfManufacturing"]);
            //textboxCurrentCost.Text = ds.Rows[0]["CurrentCost"].ToString();
            textboxWeight.Text = ds.Rows[0]["Weight"].ToString();

           // string Prefix = ds.Rows[0]["Prefix"].ToString();
            //DropDownListPrefix
            DropDownListPrefix.SelectedItem.Text = ds.Rows[0]["Prefix"].ToString();

            textboxRemarks.Text = ds.Rows[0]["Remarks"].ToString();
            SQLString = "select * from dbo.Product where productID=" + ds.Rows[0]["ProductID"];
            Utils.PopulateDropDownList(DropDownListProductName, SQLString, CnnString, "-- Select Product --");
            DropDownListProductName.SelectedIndex = DropDownListProductName.Items.Count - 1;
        }

       
    }
}