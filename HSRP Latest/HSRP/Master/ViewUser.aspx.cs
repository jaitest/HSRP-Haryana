﻿using System;
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
    public partial class ViewUser : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRP_StateID;
        int RTOLocationID;
        int intHSRP_StateID;
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
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRP_StateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
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
                            Grid2.Visible = false;
                        }
                        else if (UserType.Equals(1))
                        {
                            labelOrganization.Visible = false;
                            dropDownListOrg.Visible = false;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListClient();
                        }
                      else if (UserType.Equals(8))
                        {
                            labelOrganization.Visible = true;
                            dropDownListOrg.Visible = true;
                            FilldropDownListOrganization();
                            Grid1.Visible = false;
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
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + "Order by HSRPStateName";
            }
            Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
            dropDownListOrg.SelectedIndex = 0;
        }

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' Order by RTOLocationName";
            }
            else
            {
                intHSRP_StateID = HSRP_StateID;
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRP_StateID + " and ActiveStatus!='N' Order by RTOLocationName";
            }
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Location--");
            ///dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
        }

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            {
                dropDownListClient.Visible = true;
                labelClient.Visible = true;
                FilldropDownListClient();
                UpdateClient.Update();
            }
            else
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Text = "Please Select State.";
                dropDownListClient.Visible = false;
                labelClient.Visible = false;
                UpdateClient.Update();
                Grid1.Items.Clear();
            }
        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select RTO Location--")
            {
                if(UserType.Equals(0))
                { 
                 ShowGrid();
                }
                else if(UserType.Equals(8))
                {
                   ShowGrid2();
                }
            }
            else
            {
                if (UserType.Equals(0))
                {
                    Grid1.Items.Clear();
                    lblErrMsg.Text = String.Empty;
                    lblErrMsg.Text = "Please Select RTO Location.";
                    return;
                }
                else if(UserType.Equals(8))
                {
                    Grid2.Items.Clear();
                    lblErrMsg.Text = String.Empty;
                    lblErrMsg.Text = "Please Select RTO Location.";
                    return;

                }
            }

        }
        #endregion

        private void ShowGrid()
        {

            if (String.IsNullOrEmpty(dropDownListClient.SelectedValue) || dropDownListClient.SelectedValue.Equals("--Select RTO Location--"))
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select RTO Location.";
                return;
            }
            buildGrid();
        }
        private void ShowGrid2()
        {

            if (String.IsNullOrEmpty(dropDownListClient.SelectedValue) || dropDownListClient.SelectedValue.Equals("--Select RTO Location--"))
            {
                Grid2.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select RTO Location.";
                return;
            }
            buildGrid2();
        }



        #region Grid
        public void buildGrid()
        {
            try
            {
                if (UserType.Equals(0))
                {
                    int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                }
                else if (UserType.Equals(1))
                {
                    intHSRP_StateID = HSRP_StateID;
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                }
                else
                {
                    intHSRP_StateID = HSRP_StateID;
                    intRTOLocationID = RTOLocationID;
                }
                SQLString = "SELECT [UserID],[UserFirstName] + space(2) + [UserLastName] as UserName,UserLoginName,[Address1] + space(2) + [Address2]+ space(2)+[City]+space(2)+ [State]+space(2)+[Zip] as Address,[EmailID],[MobileNo],[ContactNo],case when ActiveStatus IN ('Y') then 'Active' else 'Inactive' end as ActiveStatus FROM [Users] Where HSRP_StateID=" + intHSRP_StateID + " and RTOLocationID=" + intRTOLocationID + " order by UserloginName";
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

        public void buildGrid2()
        {
            try
            {
                if (UserType.Equals(0))
                {
                    int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                }
                else if (UserType.Equals(8))
                {
                    int.TryParse(dropDownListOrg.SelectedValue, out intHSRP_StateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                }
                else if (UserType.Equals(1))
                {
                    intHSRP_StateID = HSRP_StateID;
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                }
                else
                {
                    intHSRP_StateID = HSRP_StateID;
                    intRTOLocationID = RTOLocationID;
                }
                SQLString = "SELECT [UserID],[UserFirstName] + space(2) + [UserLastName] as UserName,UserLoginName,[Address1] + space(2) + [Address2]+ space(2)+[City]+space(2)+ [State]+space(2)+[Zip] as Address,[EmailID],[MobileNo],[ContactNo],case when ActiveStatus IN ('Y') then 'Active' else 'Inactive' end as ActiveStatus FROM [Users] Where HSRP_StateID=" + intHSRP_StateID + " and RTOLocationID=" + intRTOLocationID + " order by UserloginName";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                Grid2.DataSource = dt;
                Grid2.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid2.SearchOnKeyPress = true;
                Grid2.DataBind();
                Grid2.RecordCount.ToString();
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
        public void OnNeedRebind2(object sender, EventArgs oArgs)
        {
            System.Threading.Thread.Sleep(200);
            Grid2.DataBind();
        }
        public void OnNeedDataSource(object sender, EventArgs oArgs)
        {
            buildGrid();
        }
        public void OnNeedDataSource2(object sender, EventArgs oArgs)
        {
            buildGrid2();
        }
        public void OnPageChanged(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        {
            Grid1.CurrentPageIndex = oArgs.NewIndex;
        }
        public void OnPageChanged2(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        {
            Grid2.CurrentPageIndex = oArgs.NewIndex;
        }
        public void OnFilter(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        {
            Grid1.Filter = oArgs.FilterExpression;
        }
        public void OnFilter2(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        {
            Grid2.Filter = oArgs.FilterExpression;
        }
        public void OnSort(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        {
            Grid1.Sort = oArgs.SortExpression;
        }
        public void OnSort2(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        {
            Grid2.Sort = oArgs.SortExpression;
        }
        private void ddGridRunningMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            buildGrid();
            Grid1.DataBind();
            adjustToRunningMode();
        }
        private void ddGridRunningMode_SelectedIndexChanged2(object sender, System.EventArgs e)
        {
            Grid2.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            buildGrid2();
            Grid2.DataBind();
            adjustToRunningMode2();
        }
        public void OnGroup(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        {
            Grid1.GroupBy = oArgs.GroupExpression;
        }
        public void OnGroup2(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        {
            Grid2.GroupBy = oArgs.GroupExpression;
        }
        private void adjustToRunningMode()
        {

            Grid1.SliderPopupClientTemplateId = "SliderTemplate";
            Grid1.SliderPopupOffsetX = 20;

        }
        private void adjustToRunningMode2()
        {

            Grid2.SliderPopupClientTemplateId = "SliderTemplate";
            Grid2.SliderPopupOffsetX = 20;

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
            else
            {
                ActiveStatus = "Y";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Users Set ActiveStatus='" + ActiveStatus + "' Where UserID=" + UserID);
            Utils.ExecNonQuery(sb.ToString(), CnnString);
            buildGrid();
        }

        protected void Grid2_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {
            String UserID = e.Item["UserID"].ToString();
            string ActiveStatus = e.Item["ActiveStatus"].ToString();
            if (ActiveStatus == "Active")
            {
                ActiveStatus = "N";
            }
            else
            {
                ActiveStatus = "Y";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Users Set ActiveStatus='" + ActiveStatus + "' Where UserID=" + UserID);
            Utils.ExecNonQuery(sb.ToString(), CnnString);
            buildGrid2();

        }

        



    }
}