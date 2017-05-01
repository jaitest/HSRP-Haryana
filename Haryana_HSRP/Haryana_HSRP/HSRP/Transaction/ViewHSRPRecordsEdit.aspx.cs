using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class ViewHSRPRecordsEdit : System.Web.UI.Page
    {
        int UID;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;

        protected void Page_Load(object sender, EventArgs e)
        {

           // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "AddNewPop();", true);
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {

                        FilldropDownListHSRPState();
                        FilldropDownListRTOLocation();

                        //if (UserType.Equals(0))
                        //{
                        //    labelHSRPState.Visible = true;
                        //    dropDownListHSRPState.Visible = true;
                        //    FilldropDownListHSRPState();
                        //}
                        //else if (UserType.Equals(1))
                        //{
                        //    labelHSRPState.Enabled = false;
                        //    dropDownListHSRPState.Enabled = false;
                        //    labelRTOLocation.Enabled = true;
                        //    dropDownListRTOLocation.Enabled = true;
                        //    FilldropDownListRTOLocation();
                        //}
                        //else
                        //{
                        //    labelHSRPState.Enabled = false;
                        //    dropDownListHSRPState.Enabled = false;
                        //    labelRTOLocation.Enabled = false;
                        //    dropDownListRTOLocation.Enabled = false;
                        //    buildGrid();
                        //}
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }


        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            if (dropDownListRTOLocation.SelectedItem.Text != "--Select RTO Location--" || dropDownListHSRPState.SelectedItem.Text != "--Select State--")
            {
                ShowGrid();
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select RTO Location and State Also";
                return;
            }
        }

        #region DropDown

        private void FilldropDownListHSRPState()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListHSRPState, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                dropDownListHSRPState.DataSource = dts;
                dropDownListHSRPState.DataBind();
            }
        }

        private void FilldropDownListRTOLocation()
        {


            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListHSRPState.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus='Y' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListRTOLocation, SQLString.ToString(), CnnString, "--Select RTO--");
            }
            else
            {
                int.TryParse(Session["UID"].ToString(), out UID);
                SQLString = "Select R.RTOLocationName,R.RTOLocationID From RTOLocation R inner join UserRTOLocationMapping U on R.RTOLocationID=U.RTOLocationID Where U.UserID=" + UID + "  order by R.RTOLocationName";
               // SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + RTOLocationID + " and ActiveStatus='Y' Order by RTOLocationName";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListRTOLocation.DataSource = dss;
                dropDownListRTOLocation.DataBind();
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
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Text = "Please Select State";
                dropDownListHSRPState.Visible = false;
                labelRTOLocation.Visible = false;
                UpdateRTOLocation.Update();
                Grid1.Items.Clear();
            }
        }


        #endregion

        private void ShowGrid()
        {

            if (String.IsNullOrEmpty(dropDownListRTOLocation.SelectedValue) || dropDownListRTOLocation.SelectedValue.Equals("--Select RTO Location--"))
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select RTO Location.";
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
                    int.TryParse(dropDownListHSRPState.SelectedValue, out intHSRPStateID);
                    int.TryParse(dropDownListRTOLocation.SelectedValue, out intRTOLocationID);
                }
                else if (UserType.Equals(1))
                {
                    intHSRPStateID = HSRPStateID;
                    int.TryParse(dropDownListRTOLocation.SelectedValue, out intRTOLocationID);
                }
                else
                {
                    intHSRPStateID = HSRPStateID;
                    intRTOLocationID = RTOLocationID;
                }
                SQLString = "SELECT [HSRPRecordID],[HSRP_StateID],ChassisNo,InvoiceDateTime,HSRPRecord_AuthorizationNo,VehicleRegNo,NetAmount,[OwnerName] ,EngineNo,VehicleType,[MobileNo],[EmailID],OrderStatus FROM [HSRPRecords] Where HSRP_StateID=" + intHSRPStateID + " and RTOLocationID=" + intRTOLocationID + " and OrderStatus!='Closed' order by HSRPRecord_AuthorizationNo";
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

        
    }
}