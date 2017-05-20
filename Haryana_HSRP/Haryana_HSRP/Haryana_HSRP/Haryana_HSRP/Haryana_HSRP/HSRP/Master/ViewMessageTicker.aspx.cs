using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace HSRP.Master
{
    public partial class ViewMessageTicker : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType;
        int HSRPStateID;
        int RTOLocationID;
         int intHSRPStateID; 
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
                UserType =  Session["UserType"].ToString();
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress; 
                if (!IsPostBack)
                { 
                    labelClient.Visible = false;
                    dropDownListClient.Visible = false;
                    try
                    {  if (UserType == "1")
                        { 
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            labelOrganization.Visible = true;
                            DropDownListStateName.Enabled = false;
                            labelClient.Visible = true;
                            dropDownListClient.Enabled = false;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            buildGrid();
                        }
                        else
                        { 
                            DropDownListStateName.Visible = true; 
                            labelOrganization.Visible = true; 
                            labelClient.Visible = false;
                            dropDownListClient.Visible = false;
                            FilldropDownListOrganization(); 
                            //buildGrid();
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
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState Order by HSRPStateName"; 
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--"); 
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + "Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1; 
            }
        }

        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName"; 
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Location--"); 
            }
            else
            { 
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Location--");
                 dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1; 
            }
        }
 

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        { 
            if (DropDownListStateName.SelectedItem.Text != "--Select RTO Location--")
            {
                ShowGrid();
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select State";
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
                if (UserType=="0")
                {
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID); 
                  
                    SQLString = "SELECT HSRPState.HSRPStateName, RTOLocation.RTOLocationName, MessageTicker.* FROM MessageTicker INNER JOIN HSRPState ON MessageTicker.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON MessageTicker.RTOLocationID = RTOLocation.RTOLocationID where MessageTicker.HSRP_StateID=" + intHSRPStateID;
                }
                else if (UserType == "1")
                {
                 
                    SQLString = "SELECT HSRPState.HSRPStateName, RTOLocation.RTOLocationName, MessageTicker.* FROM MessageTicker INNER JOIN HSRPState ON MessageTicker.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON MessageTicker.RTOLocationID = RTOLocation.RTOLocationID where MessageTicker.HSRP_StateID=" + HSRPStateID + " and MessageTicker.RTOLocationID=" + RTOLocationID + " order by RTOLocationName";
                }
                else
                {
                    SQLString = "SELECT HSRPState.HSRPStateName, RTOLocation.RTOLocationName, MessageTicker.* FROM MessageTicker INNER JOIN HSRPState ON MessageTicker.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON MessageTicker.RTOLocationID = RTOLocation.RTOLocationID where MessageTicker.HSRP_StateID=" + HSRPStateID + " and MessageTicker.RTOLocationID=" + RTOLocationID + " order by RTOLocationName";
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
            labelClient.Visible = true;
            dropDownListClient.Visible = true; 
            FilldropDownListClient(); 
            buildGrid();
        }

        

    }
}