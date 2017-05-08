using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HSRP;

namespace HSRP.Master
{
    public partial class ViewRTOLocation : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int HSRP_StateID;
        int UserType;
        int OrgID;
        int intOrgID; 

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRP_StateID = Convert.ToInt16(Session["UserHSRPStateID"]);
                UserType = Convert.ToInt32(Session["UserType"].ToString());
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

                            
                            labelOrganization.Enabled = false;
                            dropDownListOrg.Enabled = false;
                            statenamebyuser();
                            buildGrid(); 
                        }
                        else
                        {
                            labelOrganization.Enabled = false;
                            dropDownListOrg.Enabled = false; 
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }
        private void FilldropDownListOrganization()
        {

            if (UserType.Equals(0))
            {

                SQLString = "select * from dbo.HSRPState where ActiveStatus='Y'";
            }
            else
            {
                SQLString = "select * from dbo.RTOLocation where HSRP_StateID==" + HSRP_StateID;
            }
            Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
           // dropDownListOrg.SelectedIndex = dropDownListOrg.Items.Count - 1;
        }
        public void buildGrid()
        {
            try
            {
                if (UserType.Equals(1))
                {

                    intOrgID = Convert.ToInt16(Session["UserHSRPStateID"]);
                   
                }
                else
                {
                int.TryParse(dropDownListOrg.SelectedValue, out intOrgID);
                }

                 
                string SQLString = "select RTOLocationID,RTOLocationName,ContactPersonName,MobileNo,LandlineNo,EmailID,ActiveStatus from dbo.RTOLocation where HSRP_StateID= " + intOrgID + "order by RTOLocationName";
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
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
            Grid2.DataBind();
        }
        public void OnNeedDataSource(object sender, EventArgs oArgs)
        {
            buildGrid();
        }
        public void OnPageChanged(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        {
            Grid2.CurrentPageIndex = oArgs.NewIndex;
        }
        public void OnFilter(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        {
            Grid2.Filter = oArgs.FilterExpression;
        }
        public void OnSort(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        {
            Grid2.Sort = oArgs.SortExpression;
        }
        private void ddGridRunningMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Grid2.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            buildGrid();
            Grid2.DataBind();
            adjustToRunningMode();
        }
        public void OnGroup(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        {
            Grid2.GroupBy = oArgs.GroupExpression;
        }
        private void adjustToRunningMode()
        {
            Grid2.SliderPopupClientTemplateId = "SliderTemplate";
            Grid2.SliderPopupOffsetX = 20;
        }

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            buildGrid();
           
        } 
        public void statenamebyuser()
        {
            intOrgID = Convert.ToInt16(Session["UserHSRPStateID"]);
            string SQLString = "select HSRP_StateID, HSRPStateName from HSRPState where ActiveStatus='Y' and HSRP_StateID=" + intOrgID;
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
            if (UserType.Equals(1))
            {
                dropDownListOrg.DataSource = dt;
                dropDownListOrg.DataBind();
            }
            Session["Name"] = dt.Rows[0]["HSRPStateName"]; ;
            Session["StateID"] = dt.Rows[0]["HSRP_StateID"];
            Session["StateName"] = dt;


        }
    }
}