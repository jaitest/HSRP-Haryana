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
    public partial class ViewGoodsDispatchRegister : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int UID;
        int intHSRPStateID;
        int intRTOLocationID;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrMsg.Text = "";
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                int.TryParse(Session["UserType"].ToString(), out UserType);
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UID);
              //  lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();

                
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType.Equals(0))
                        {
                            buildGrid();
                            //labelOrganization.Visible = true;
                            //dropDownListOrg.Visible = true;
                            FilldropDownState();
                        }
                        else if (UserType.Equals(1))
                        {
                            buildGrid();
                            //labelOrganization.Visible = false;
                            //dropDownListOrg.Visible = false;
                            //labelClient.Visible = true;
                            //dropDownListClient.Visible = true;
                            FilldropDownRTOLocation();
                           
                        }
                        else
                        {
                            //labelOrganization.Visible = false;
                            //dropDownListOrg.Visible = false;
                            //labelClient.Visible = false;
                            //dropDownListClient.Visible = false;
                            
                            buildGrid();
                        }
                        //string SQLdispachCode = "select LastNo from Prefix where HSRP_StateID='" + HSRPStateID + "' AND RTOLocationID='" + RTOLocationID + "' AND PrefixFor='Dispatch' ";
                        //int LastNo = Convert.ToInt32(Utils.getScalarCount(SQLdispachCode, CnnString));
                        //Session["LastNo"] = LastNo + 1;
                    }
                    catch (Exception err)
                    {
                       // lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        private void FilldropDownState()
        {

            //SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
            //Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
        }

        private void FilldropDownRTOLocation()
        {
            //int.TryParse(dropDownListOrg.SelectedValue, out intHSRPStateID);
            //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus='Y' Order by RTOLocationName";
            //Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
       }


        #endregion

        private void ShowGrid()
        {
          
            //if (String.IsNullOrEmpty(dropDownListClient.SelectedValue) || dropDownListClient.SelectedValue.Equals("--Select Client--"))
            //{
            //    Grid1.Items.Clear();
            //    lblErrMsg.Text = String.Empty;
            //    lblErrMsg.Text = "Please Select Client.";
            //    return;
            //}
            buildGrid();
        }


        #region Grid
        public void buildGrid()
        {
            try
            {
               
                intHSRPStateID = HSRPStateID;
                intRTOLocationID=RTOLocationID;
              //  SQLString = "SELECT DispatchID,DispatchCode,[GoodsDispatchType],(select RTOLocationName from RTOLocation Where RTOLocationID=DispatchToRTOLocationID) as DispatchToRTOLocationID,[RTOAddress],[DispatchDate],(select UserLoginName from users where UserID=DispatchBy) as DispatchBy,dispatchDeliveryStatus FROM [GoodsDispatchRegister] Where HSRP_StateID=" + dropDownListOrg.SelectedValue + " and RTOLocationID=" + dropDownListClient.SelectedValue + " order by DispatchID";
               SQLString =" select goodsDispatchInvoiceData.*,GoodsDispatchRegister.*,(a.[RTOLocationName]) AS RtoTo, (b.[RTOLocationName]) as RtoFrom,[PlateBarcodeDetail].* from goodsDispatchInvoiceData inner join GoodsDispatchRegister on "+
   " goodsDispatchInvoiceData.ReceiveID=GoodsDispatchRegister.DispatchID inner join [dbo].[RTOLocation] a  on a.RTOLocationID=GoodsDispatchRegister.rtolocationidto inner join [RTOLocation] b "+
    " on b.RTOLocationID=GoodsDispatchRegister.rtolocationidfrom 	inner join PlateBarcodeDetail on PlateBarcodeDetail.Autoid =goodsDispatchInvoiceData.PlantReceiveID";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {
                    Grid1.DataSource = dt;
                    Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                    Grid1.SearchOnKeyPress = true;
                    Grid1.DataBind();
                    Grid1.RecordCount.ToString();
                }
                else
                {
                    lblErrMsg.Text = "Record Not Found";
                }
            }
            catch (Exception ex)
            {
                //lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
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

        protected void dropDownListOrg_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            //{
            //    labelClient.Visible = true;
            //    dropDownListClient.Visible = true;
            //    FilldropDownRTOLocation();

            //}
            //else
            //{
            //    labelClient.Visible = false;
            //    dropDownListClient.Visible = false;
            //}
        }

       

        

        protected void btnGo_Click(object sender, EventArgs e)
        {
            buildGrid();
        }

        


        

    }
}