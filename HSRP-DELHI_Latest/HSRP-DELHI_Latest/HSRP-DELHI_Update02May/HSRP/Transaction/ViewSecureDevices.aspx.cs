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
    public partial class viewSecureDevices : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int OrgID;
        int ClientID;
         int intOrgID;
         int intClientID;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                ClientID = Convert.ToInt32(Session["ClientID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                OrgID = Convert.ToInt32(Session["OrgID"].ToString());
                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType.Equals(0))
                        {
                            labelOrganization.Visible = true;
                            dropDownListOrg.Visible = true;
                            FilldropDownListOrganization();
                        }
                        else if (UserType.Equals(1))
                        {
                            labelOrganization.Visible = false;
                            dropDownListOrg.Visible = false;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListClient();
                        }
                        else
                        {
                            labelOrganization.Visible = false;
                            dropDownListOrg.Visible = false;
                            labelClient.Visible = false;
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
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState Order by HSRPStateName";
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + OrgID + "Order by HSRPStateName";
            }
            Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
            dropDownListOrg.SelectedIndex = dropDownListOrg.Items.Count - 1;
        }

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListOrg.SelectedValue, out intOrgID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intOrgID + " and ActiveStatus!='N' Order by RTOLocationName";
            }
            else
            {
                intOrgID = OrgID;
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intOrgID + " and ActiveStatus!='N' Order by RTOLocationName";
            }
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Client--");
            dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        }

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select Organization--")
            {
                dropDownListClient.Visible = true;
                labelClient.Visible = true;
                FilldropDownListClient();
                UpdateClient.Update();
            }
            else
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Text = "Please Select Organization";
                dropDownListClient.Visible = false;
                labelClient.Visible = false;
                UpdateClient.Update();
                Grid1.Items.Clear();
            }
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (dropDownListOrg.SelectedItem.Text != "--Select Client--")
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
                    int.TryParse(dropDownListOrg.SelectedValue, out intOrgID);
                    int.TryParse(dropDownListClient.SelectedValue, out intClientID);
                    SQLString = "SELECT [UserID],[UserFirstName] + space(2) + [UserLastName] as UserName,UserLoginName,[Address1] + space(2) + [Address2]+ space(2)+[City]+space(2)+ [State]+space(2)+[Zip] as Address,[EmailID],[MobileNo],[ContactNo],case when ActiveStatus IN ('Y') then 'Active' else 'Inactive' end as ActiveStatus FROM [Users] Where OrgID=" + intOrgID + " and ClientID=" + intClientID + " order by UserloginName";
                }
                else if (UserType.Equals(1))
                {
                    intOrgID=OrgID;
                    int.TryParse(dropDownListClient.SelectedValue, out intClientID);
                    SQLString = "SELECT [UserID],[UserFirstName] + space(2) + [UserLastName] as UserName,UserLoginName,[Address1] + space(2) + [Address2]+ space(2)+[City]+space(2)+ [State]+space(2)+[Zip] as Address,[EmailID],[MobileNo],[ContactNo],case when ActiveStatus IN ('Y') then 'Active' else 'Inactive' end as ActiveStatus FROM [Users] Where OrgID=" + intOrgID + " and ClientID=" + intClientID + " order by UserloginName";
                }
                else
                {
                    intOrgID = OrgID;
                    intClientID=ClientID;
                    SQLString = "SELECT [UserID],[UserFirstName] + space(2) + [UserLastName] as UserName,UserLoginName,[Address1] + space(2) + [Address2]+ space(2)+[City]+space(2)+ [State]+space(2)+[Zip] as Address,[EmailID],[MobileNo],[ContactNo],case when ActiveStatus IN ('Y') then 'Active' else 'Inactive' end as ActiveStatus FROM [Users] Where OrgID=" + intOrgID + " and ClientID=" + intClientID + " order by UserloginName";
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

    }
}