using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
namespace HSRP.Master
{
    public partial class ViewMACbase : System.Web.UI.Page
    {
        Utils bl = new Utils();

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string SQLString1 = string.Empty;
        int UserType;        
        string HSRP_StateID1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                Grid1.Visible = false;
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;


                if (!IsPostBack)
                {
                    try
                    {
                        FilldropDownListOrganization();
                       
                        
                    }
                    catch (Exception err)
                    {
                        Label2.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString.ToString(), "--Select State--");
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString.ToString());
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }





        public void buildGrid()
        {
            Label2.Text = "";
            try
            {
                if (ddlstatus.SelectedValue.ToString() == "0")
                {
                    Label2.Visible = true;
                    Label2.Text = "Please Select Status..";
                    return;
                }
                if (DropDownListStateName.SelectedItem.Text.ToString() == "--Select State--")
                {
                    Label2.Visible = true;
                    Label2.Text = "Please Select State..";
                    return;
                }

                string status1 = ddlstatus.SelectedValue.ToString();
                string SQLString = "select [MACBase].* ,HSRPState.HSRPStateName,RTOLocation.RTOLocationName from dbo.MACBase inner join HSRPState on HSRPState.[HSRP_StateID]=MACBase.[HSRP_StateID] inner join RTOLocation on RTOLocation.RTOLocationID=MACBase.RTOLocationID where [MACBase].[ActiveStatus] ='" + status1 + "' and MACBase.[HSRP_StateID]= '" + DropDownListStateName.SelectedValue.ToString()+ "'";
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());

                if (dt.Rows.Count > 0)
                {
                    
                    Grid1.DataSource = dt;
                    Grid1.DataBind();
                    Grid1.Visible = true;
                }
                else
                {
                    Label2.Visible = true;
                    Label2.Text = "Record Not Found.";
                    return;
                }
               
              

            }
            catch (Exception ex)
            {
                Label2.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void btnDetail_Click(object sender, EventArgs e)
        {
            buildGrid();
        }
    }
}