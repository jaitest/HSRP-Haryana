using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DataProvider;


namespace HSRP.Transaction
{
    public partial class viewDetailstagingarea : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string HSRPRecordID = string.Empty;
        string Mode;
        string ISFrontPlateSize;
        string ISRearPlateSize;
        string StickerMandatory;
        string UserID;
        int HSRPStateID;
        int RTOLocationID;

        string RearPlateSize = string.Empty;
        string FrontPlateSize = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            //SQLString = "SELECT  FrontPlateSize, RearPlateSize FROM HSRPRecords where HSRPRecordID='" + Request.QueryString["HSRPRecordID"].ToString() + "'";
            //DataTable dtPlateSize = Utils.GetDataTable(SQLString, CnnString);
            //if (dtPlateSize.Rows.Count > 0)
            //{
            //    RearPlateSize = dtPlateSize.Rows[0]["RearPlateSize"].ToString();
            //}

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
            }

            if (!Page.IsPostBack)
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"].ToString());
                Mode = Request.QueryString["Mode"];
                UserID = Request.QueryString["UserID"];


                if (Mode == "Edit")
                {
                    HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                    AssignLaserEdit(HSRPRecordID);


                }
            }
        }

        private void AssignLaserEdit(string HSRPRecordID)
        {
            SQLString = "select HSRPRecordStaggingID as HSRPRecordID,	HSRP_StateID as HSRP_StateID,	CONVERT(varchar, HSRPRecord_CreationDate,105)  as OrderDate,HSRPRecord_AuthorizationNo, convert(varchar,HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, RegistrationDate,	OwnerName,	VehicleClass,	NICVehicleType as VehicleType,	ChassisNo,	EngineNo,	VehicleRegNo,	NICVehicleRegNo,	HSRP_Front_LaserCode as HSRP_Front_LaserCode,	HSRP_Rear_LaserCode as HSRP_Rear_LaserCode ,mobileno  FROM HSRPRecordsStaggingArea where HSRPRecordStaggingID=" + HSRPRecordID;
         
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            if (ds.Rows.Count > 0)
            {
                LabelDataEntryDate.Text = ds.Rows[0]["OrderDate"].ToString();
              
                LabelAuthorizationNo.Text = ds.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();

                LabelVehicleOwnerName.Text = ds.Rows[0]["OwnerName"].ToString();
              
                LabelVehicleClass.Text = ds.Rows[0]["VehicleClass"].ToString();
                LabelVehicleType.Text = ds.Rows[0]["VehicleType"].ToString();
                LabelVehicleRegNo.Text = ds.Rows[0]["VehicleRegNo"].ToString();
                     LabelEngineNumber.Text=ds.Rows[0]["EngineNo"].ToString();
                     LabelChasisNo.Text = ds.Rows[0]["ChassisNo"].ToString();
                     LabelMobileNo.Text = ds.Rows[0]["mobileno"].ToString();
                	
                LabelFrontPlate.Text = ds.Rows[0]["HSRP_Front_LaserCode"].ToString();
                LabelRearPlateNo.Text = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString();
              

                SQLString = "select HSRPStateName from HSRPState where HSRP_StateID=" + ds.Rows[0]["HSRP_StateID"];
                DataTable dtState = Utils.GetDataTable(SQLString, CnnString);
                if (dtState.Rows.Count > 0)
                {
                    LabelStateID.Text = dtState.Rows[0]["HSRPStateName"].ToString();
                }

                            

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('No Record Found');</script>");
            }
        }
    }
}