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
    public partial class ViewMACbaseInActive : System.Web.UI.Page
    {
        Utils bl = new Utils();

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int RTOLocationID;
        int intHSRPStateID;
        int HSRPStateID;
        int VehicleModelID;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"].ToString());
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        buildGrid();
                        // VehicleModelID = int.Parse(Request.QueryString["VehicleModelID"]);
                        // Utils.user_log(strUserID, "View Organization", ComputerIP, "Page load", CnnString);
                    }
                    catch (Exception err)
                    {
                        // lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }



        public void buildGrid()
        {
            try
            {
               
                string status1 = "N";
                string SQLString = "select [MACBase].* ,HSRPState.HSRPStateName,RTOLocation.RTOLocationName from dbo.MACBase inner join HSRPState on HSRPState.[HSRP_StateID]=MACBase.[HSRP_StateID] inner join RTOLocation on RTOLocation.RTOLocationID=MACBase.RTOLocationID where [MACBase].[ActiveStatus] ='" + status1 + "'";
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                Grid2.DataSource = dt;
                //Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                //Grid1.SearchOnKeyPress = true;
                Grid2.DataBind();
                
            }
            catch (Exception ex)
            {
                   //lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
    }
}