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
    public partial class ViewPrefix : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType;
        int RTOLocationID;
        int HSRPStateID;
        int intHSRPStateID;
        int intRTOLocationID;


        protected void Page_Load(object sender, EventArgs e)
        {  
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                lblErrMsg.Text = string.Empty;

                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType =  Session["UserType"].ToString();
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    labelClient.Visible = false;
                    dropDownListClient.Visible = false;

                    try
                    {
                        if (UserType == "0")
                        {
                            
                            FilldropDownListOrganization();
                        }
                        else if (UserType=="1")
                        {
                            labelOrganization.Visible = true;
                            DropDownListState.Enabled = false;
                            dropDownListClient.Enabled = false;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            buildGrid();

                        }
                        else
                        {
                            DropDownListState.Enabled = false; 
                            dropDownListClient.Visible = false;
                            buildGrid();
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
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";

                Utils.PopulateDropDownList(DropDownListState, SQLString.ToString(), CnnString, "--Select State--"); 
            } 
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID= " + HSRPStateID + "and ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListState, SQLString.ToString(), CnnString, "--Select State--");
                DropDownListState.SelectedIndex = DropDownListState.Items.Count - 1;
            }

        }

        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {
                int.TryParse(DropDownListState.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Location--");
            }
            else
            {
                SQLString = "select RTOLocationName, RTOLocationID from RTOLocation where RTOLocationID = " + RTOLocationID + "and ActiveStatus='Y'";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Location--");
               dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }

        }

        
        #endregion
        #region Grid
        public void buildGrid()
        {
            try
            {
                    
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                if (UserType == "0")
                {
                    SQLString = "SELECT Prefix.SerialPrefixID,Prefix.HSRP_StateID, HSRPState.HSRPStateName,  Prefix.PrefixFor, Prefix.PrefixText,Prefix.RTOLocationID, RTOLocation.RTOLocationName FROM Prefix INNER JOIN HSRPState ON Prefix.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON Prefix.RTOLocationID = RTOLocation.RTOLocationID where Prefix.RTOLocationID=" + intRTOLocationID;

                }
                else
                {
                    SQLString = "SELECT Prefix.SerialPrefixID, HSRPState.HSRPStateName, Prefix.PrefixFor, Prefix.PrefixText, RTOLocation.RTOLocationName, Prefix.RTOLocationID, Prefix.HSRP_StateID FROM Prefix INNER JOIN HSRPState ON Prefix.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN RTOLocation ON Prefix.RTOLocationID = RTOLocation.RTOLocationID where Prefix.RTOLocationID=" + RTOLocationID;
                }

                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                Grid1.DataSource = dt;
                    Grid1.Visible = true;
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

        protected void dropDownListOrg_SelectedIndexChanged3(object sender, EventArgs e)
        {
            FilldropDownListClient();
            buildGrid();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();

            labelClient.Visible = true;
            dropDownListClient.Visible = true;
            Grid1.Items.Clear();
            lblErrMsg.Text = String.Empty;
            lblErrMsg.Text = "Please Select RTO Location.";
            return;
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (dropDownListClient.SelectedItem.Text != "--Select State--")
            {
                buildGrid();
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select RTO Location.";
                return;
            } 
        }
            
       
       
    }
}