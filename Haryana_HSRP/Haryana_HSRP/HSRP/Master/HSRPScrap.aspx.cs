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
    public partial class HSRPScrap : System.Web.UI.Page
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
                        btnSave.Text = "Save";
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
            txtScrapqty.Text = "";
            chkActive.Checked = false;

        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select RTO Location--")
            {
                ShowGrid();
                txtScrapqty.Text = "";
                chkActive.Checked = false;
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select RTO Location.";
                return;
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
                SQLString = "SELECT ID,HSRPSTATEID,HSRPRTOLOCATIONID,SCRAPQTY,USERID,CREATIONDATE,case when ActiveStatus IN ('Y') then 'Active' else 'Inactive' end as ActiveStatus FROM HSRPScrap Where HSRPStateID=" + intHSRP_StateID + " and hsrpRTOLocationID=" + intRTOLocationID + " order by creationdate desc";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                Grid1.RecordCount.ToString();
                txtScrapqty.Text = "";
                chkActive.Checked = false;
                btnSave.Text = "Save";
                HiddenID.Value = "";
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
            String ID = e.Item["ID"].ToString();
                       
            DataTable dtscrap = Utils.GetDataTable("select top 1 scrapqty,activestatus from hsrpscrap where id=" + ID + "", CnnString);
            foreach (DataRow dr in dtscrap.Rows)
            {
                txtScrapqty.Text = dr["scrapqty"].ToString().Trim();
                if (dr["activestatus"].ToString().Trim() == "Y")
                {
                    chkActive.Checked = true;
                }
                else
                {
                    chkActive.Checked = false;
                }
                HiddenID.Value = ID;
                btnSave.Text = "Update";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string activestatus = string.Empty;
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
            if (chkActive.Checked == true)
            {
                activestatus = "Y";
            }
            else
            {
                activestatus = "N";
            }
            if (txtScrapqty.Text.Trim() != "")
            {
                if (btnSave.Text == "Update")
                {
                    sql = "update hsrpscrap set scrapqty=" + txtScrapqty.Text.Trim() + ",activestatus='" + activestatus + "',userid=" + strUserID + " where id=" + HiddenID.Value.Trim() + "";
                }
                else
                {
                    sql = "insert into hsrpscrap(HSRPStateID,hsrpRTOLocationID,scrapqty,activestatus,creationdate,userid) values(" + intHSRP_StateID + "," + intRTOLocationID + "," + txtScrapqty.Text.Trim() + ",'" + activestatus + "',getdate()," + strUserID + ")";
                    
                }
                Utils.ExecNonQuery(sql, CnnString);
                lblErrMsg.Text = "Record Saved..";
                buildGrid();
            }
            else
            {
                lblErrMsg.Text = "Scrap Qty should not be blank.";
            }
        }
    }
}