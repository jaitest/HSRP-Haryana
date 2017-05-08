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
    public partial class ViewRTOInventory : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
         int intHSRPStateID;
         int intRTOLocationID;
         string OrderStatus = string.Empty;
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
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"]);
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    //InitialSetting(); 
                    try
                    { 
                         if (UserType.Equals(1))
                        {
                            hiddenUserType.Value = "1";  
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            dropDownListClient.Enabled = false;
                           // labelDate.Visible = false;
                           // labelTO.Visible = false; 
                            labelOrderStatus.Visible = false;
                            dropDownListorderStatus.Visible = false;
                            ButtonGo.Visible = false; 

                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            buildGrid();
                        }
                        else
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;  
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState Order by HSRPStateName"; 
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
               // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + "Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
                Session["StateName"] = dts;
            }
        }

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");
               // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                Session["RTOLocation"] = dss;

            }
        } 
        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListStateName.SelectedItem.Text != "--Select Client--")
            {
                ShowGrid();
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Client.";
                return;
            } 
        }
        #endregion

        private void ShowGrid()
        {
          
            if (String.IsNullOrEmpty(dropDownListClient.SelectedValue) || dropDownListClient.SelectedValue.Equals("--Select Client--"))
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Client.";
                return;
            }
            buildGrid();
        }


        #region Grid
        public void buildGrid()
        {
            try
            { 
                if (UserType.Equals(0))
                {
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                   // SQLString = "SELECT [UserID],[UserFirstName] + space(2) + [UserLastName] as UserName,UserLoginName,[Address1] + space(2) + [Address2]+ space(2)+[City]+space(2)+ [State]+space(2)+[Zip] as Address,[EmailID],[MobileNo],[ContactNo],case when ActiveStatus IN ('Y') then 'Active' else 'Inactive' end as ActiveStatus FROM [Users] Where HSRPStateID=" + intHSRPStateID + " and RTOLocationID=" + intRTOLocationID + " order by UserloginName";
                    SQLString = "SELECT  HSRPAuthorizationSlipDate + 4 as Duedate, HSRPRecordID, HSRPRecord_AuthorizationNo, OwnerName, VehicleRegNo, HSRPAuthorizationSlipDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode FROM HSRPRecords where RTOLocationID=" + intRTOLocationID;

                }
                else if (UserType.Equals(1))
                {
                    intHSRPStateID=HSRPStateID;
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                    SQLString = "SELECT  HSRPAuthorizationSlipDate + 4 as Duedate, HSRPRecordID, HSRPRecord_AuthorizationNo, OwnerName, VehicleRegNo, HSRPAuthorizationSlipDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode FROM HSRPRecords where RTOLocationID=" + RTOLocationID;
                }
                else
                {
                    intHSRPStateID = HSRPStateID;
                    intRTOLocationID=RTOLocationID;
                    SQLString = "SELECT  HSRPAuthorizationSlipDate + 4 as Duedate, HSRPRecordID, HSRPRecord_AuthorizationNo, OwnerName, VehicleRegNo, HSRPAuthorizationSlipDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode FROM HSRPRecords where RTOLocationID=" + RTOLocationID;
                } 
                DataTable dt = Utils.GetDataTable(SQLString, CnnString); 
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                Grid1.RecordCount.ToString();
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        public void OnNeedRebind(object sender, EventArgs oArgs)
        {
            System.Threading.Thread.Sleep(200);
            Grid1.DataBind();
        }
        public void OnNeedDataSource(object sender, EventArgs oArgs)
        {
            buildGrid();
        }
        public void OnPageChanged(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        {
            Grid1.CurrentPageIndex = oArgs.NewIndex;
        }
        public void OnFilter(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        {
            Grid1.Filter = oArgs.FilterExpression;
        }
        public void OnSort(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        {
            Grid1.Sort = oArgs.SortExpression;
        }
        private void ddGridRunningMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            
            Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
           
            buildGrid();
            Grid1.DataBind();
            adjustToRunningMode();
        }
        public void OnGroup(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        {
            Grid1.GroupBy = oArgs.GroupExpression;
        }
        private void adjustToRunningMode()
        {

            Grid1.SliderPopupClientTemplateId = "SliderTemplate";
            Grid1.SliderPopupOffsetX = 20;

        }
        #endregion

        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {
            String UserID = e.Item["UserID"].ToString();
            string ActiveStatus = e.Item["ActiveStatus"].ToString();
            if (ActiveStatus == "Active")
            {
                ActiveStatus = "N";
            }
            else {
                ActiveStatus = "Y";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Users Set ActiveStatus='" + ActiveStatus + "' Where UserID=" + UserID);
            Utils.ExecNonQuery(sb.ToString(), CnnString);
            buildGrid();
        } 
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true; 

        }
         

       protected void ButtonGo_Click(object sender, EventArgs e)
        { 
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
            string OrderStatus = dropDownListorderStatus.SelectedItem.Text;

            //SQLString = "SELECT  HSRPAuthorizationSlipDate + 4 as Duedate, HSRPRecordID, HSRPRecord_AuthorizationNo, OwnerName, VehicleRegNo, HSRPAuthorizationSlipDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode FROM HSRPRecords where HSRP_StateID='" + intHSRPStateID + "'and RTOLocationID='" + intRTOLocationID + "'and OrderDate between'" + OrderDate1 + "'and'" + AuthorizationDate + "'and OrderStatus='" + OrderStatus + "'";
            SQLString = "SELECT  RTOInventory.Prefix, RTOInventory.LaserNowithoutPrefix, RTOInventory.LaserNo, RTOInventory.InventoryStatus, RTOInventory.InventoryID, RTOInventory.ProductID,  Product.ProductCode, RTOInventory.HSRP_StateID, HSRPState.HSRPStateName, RTOInventory.RTOLocationID, RTOLocation.RTOLocationName FROM RTOInventory INNER JOIN Product ON RTOInventory.ProductID = Product.ProductID INNER JOIN HSRPState ON RTOInventory.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON RTOInventory.RTOLocationID = RTOLocation.RTOLocationID where RTOInventory.HSRP_StateID= '" + intHSRPStateID + "' and RTOInventory.RTOLocationID ='" + intRTOLocationID + "'and RTOInventory.InventoryStatus='" + OrderStatus + "'  order by InventoryID desc";
            DataSet dt = Utils.getDataSet(SQLString, CnnString);
            Grid1.DataSource = dt;
            Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            Grid1.SearchOnKeyPress = true;
            Grid1.DataBind();
            Grid1.RecordCount.ToString();
        }
        //private void InitialSetting()
        //{

        //    string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

        //    HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        //    OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    OrderDate.MaxDate = DateTime.Parse(MaxDate);
        //    CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //}
         
    }
}