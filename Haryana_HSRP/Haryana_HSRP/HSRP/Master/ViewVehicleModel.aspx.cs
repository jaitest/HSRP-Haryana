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
    public partial class ViewVhicleModel : System.Web.UI.Page
    {
        Utils bl = new Utils();

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        //int RTOLocationID;
        //int intHSRPStateID;
        //int HSRPStateID;
        //int VehicleModelID;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType =Session["UserType"].ToString();
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
                         lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }



        public void buildGrid()
        {
            try
            {
                string SQLString = "select VehicleModelMaster.* ,VehicleMakerMaster.VehicleMakerDescription from VehicleModelMaster inner join VehicleMakerMaster on VehicleModelMaster.VehicleMakerID=VehicleMakerMaster.VehicleMakerID";
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();

            }
            catch (Exception ex)
            {
                // lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
    }
}