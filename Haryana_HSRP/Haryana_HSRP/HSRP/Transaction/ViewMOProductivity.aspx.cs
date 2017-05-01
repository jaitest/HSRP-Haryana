using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace HSRP.Transaction
{
    public partial class ViewMOProductivity : System.Web.UI.Page
    {

        Utils bl = new Utils();

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string  UserType;
        //int RTOLocationID;
        //int intHSRPStateID;
        //int HSRPStateID;
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
                string SQLString = "SELECT MOProductivityID,ProductivityDate,MachineOperatorProductivity.MachineID,OperatorName,MachineOperatorProductivity.ProductID,Quantity,ScrapQuantity,ScrapWeight,Remarks,StateID,PlantAddress,UserID  , Product.ProductID ,Product.ProductCode,MachineMaster.MachineID ,(MachineType+'-'+MachineName) as mtype ,Plant.PlantID,Plant.PlantAddress from MachineOperatorProductivity inner join Product on MachineOperatorProductivity.ProductID= Product.ProductID inner join MachineMaster on MachineOperatorProductivity.MachineID = MachineMaster.MachineID inner join Plant on MachineOperatorProductivity.PlantID=Plant.PlantID  order by ProductivityDate";


                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
    }
}