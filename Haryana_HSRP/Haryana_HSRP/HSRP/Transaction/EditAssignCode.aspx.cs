using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HSRP;

namespace HSRP.Transaction
{
    public partial class EditAssignCode : System.Web.UI.Page
    {
        string i;
        string HSRP_AuthID;
        string SQLString = string.Empty;
        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                
                i = Request.QueryString["Mode"];
                HSRP_AuthID = Request.QueryString["HSRPID"];


                    SQLString = "select * from HSRPRecords where HSRPRecordID=" + HSRP_AuthID;
                    DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
                    if (i == "Edit")
                    {
                        txtHSRPAuthNo.Text = ds.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                        txtVehRegNo.Text = ds.Rows[0]["VehicleRegNo"].ToString();
                        txtOwnerName.Text = ds.Rows[0]["OwnerName"].ToString();
                        txtMobNo.Text = ds.Rows[0]["MobileNo"].ToString();
                        txtChassisNo.Text = ds.Rows[0]["ChassisNo"].ToString();
                        txtEngineNo.Text = ds.Rows[0]["EngineNo"].ToString();
                        txtVehClass.Text = ds.Rows[0]["VehicleClass"].ToString();
                        txtVehType.Text = ds.Rows[0]["VehicleType"].ToString();
                        txtRegDate.Text = ds.Rows[0]["HSRPRecord_CreationDate"].ToString();
                        txtStickerCode.Text = ds.Rows[0]["HSRP_Sticker_LaserCode"].ToString();
                        txtFrontCode.Text = ds.Rows[0]["HSRP_Front_LaserCode"].ToString();
                        txtRearCode.Text = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString();
                    }
                    else
                    {
                        txtHSRPAuthNo.Text = ds.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                        txtVehRegNo.Text = ds.Rows[0]["VehicleRegNo"].ToString();
                        txtOwnerName.Text = ds.Rows[0]["OwnerName"].ToString();
                        txtMobNo.Text = ds.Rows[0]["MobileNo"].ToString();
                        txtChassisNo.Text = ds.Rows[0]["ChassisNo"].ToString();
                        txtEngineNo.Text = ds.Rows[0]["EngineNo"].ToString();
                        txtVehClass.Text = ds.Rows[0]["VehicleClass"].ToString();
                        txtVehType.Text = ds.Rows[0]["VehicleType"].ToString();
                        txtRegDate.Text = ds.Rows[0]["HSRPRecord_CreationDate"].ToString();
                    }

            }
            int HSRPStateID;
            string UserType = string.Empty;


            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            else
            {
                HSRPStateID = Convert.ToInt32(Session["HSRPStateID"].ToString());
                UserType = Session["UserType"].ToString();
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            HSRP_AuthID = Request.QueryString["HSRPID"];
            string update_hsrpLaser = "UPDATE HSRPRecords SET HSRP_Sticker_LaserCode='" + txtStickerCode.Text + "', HSRP_Front_LaserCode='" + txtFrontCode.Text + "',HSRP_Rear_LaserCode='" + txtRearCode.Text + "' where HSRPRecordID='"+ HSRP_AuthID+"'";
            Utils.ExecNonQuery(update_hsrpLaser,ConnectionString);
            lblSucMess.Text = "Successfully Updated Record!";
        }
    }
}