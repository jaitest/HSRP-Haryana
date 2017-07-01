using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DataProvider;

namespace HSRP.Master
{
    public partial class CaptureStock : System.Web.UI.Page
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
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        int intRTOLocationID;
        string FrontReject;
        string RearReject;
        string FrontRejectLaserNo;
        string RearRejectLaserNo;
        int RTOLocationIDAssign;
        int StateIDAssign;
        string RearPlateSize = string.Empty;
        string FrontPlateSize = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

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
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                Mode = Request.QueryString["Mode"];
                // UserID = Request.QueryString["UserID"];
                textBoxFrontPlate.Text = "";


            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {


        }


        public void blanckfield()
        {
            textBoxFrontPlate.Text = "";
            textBoxRearPlate.Text = "";
            lblErrMess.Text = "";
            txtVehicleRegNo.Text = "";
        }

        protected void btnSaveStockinHand_Click(object sender, EventArgs e)
        {
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserID = Session["UID"].ToString();

            lblErrMess.Text = "";
            lblSucMess.Text = "";
            // btnSave.Enabled = false;

            string vehicleRegNo = txtVehicleRegNo.Text.Trim();
            string FrontLaserCode = textBoxFrontPlate.Text.Trim().ToUpper();
            string RearLaserCode = textBoxRearPlate.Text.Trim().ToUpper();

            if (FrontLaserCode == "" || RearLaserCode == "")
            {
                lblErrMess.Text = "Please Provide Plate No.!!";
            }
            else
            {
                SQLString = "select top 1 orderstatus from hsrprecords where hsrp_stateid='" +
                        HSRPStateID + "' and rtolocationid='" + RTOLocationID + "' and vehicleRegNo ='" 
                        + vehicleRegNo + "' and hsrp_front_lasercode='" + FrontLaserCode + "' and hsrp_rear_lasercode='" 
                        + RearLaserCode + "' and orderstatus='New '";
                string Records = Utils.getScalarValue(SQLString, CnnString);
                if (!string.IsNullOrEmpty(Records))
                {
                    if (Records.Equals("New Order"))
                    {
                        SQLString = "update HSRPRecords set orderstatus='Embossing Done',EmbossingUserID='" + UserID + "' where hsrp_stateid='" +
                            HSRPStateID + "' and rtolocationid='" + RTOLocationID + "' and vehicleregno='" + vehicleRegNo + "' and hsrp_front_lasercode='" + FrontLaserCode + "' and hsrp_rear_lasercode='" + RearLaserCode + "'";
                        int i = Utils.ExecNonQuery(SQLString, CnnString);

                        if (i.Equals(0))
                        {
                            lblErrMess.Text = "Record Not Saved!!";
                            return;
                        }
                    }
                    string strQuery = "INSERT INTO [dbo].[CaptureStock] ([HSRP_StateId],[RTOLocationId],[VehicleRegNo],[FLaser],[RLaser],[CreationDate],[Remarks],createdby) " +
                                        " VALUES('" + HSRPStateID + "','" + RTOLocationID + "','" + vehicleRegNo + "','" + FrontLaserCode + "','" + RearLaserCode + "',getdate(),'"+txtRemarks.Text+"','"+UserID+"')";
                    int j = Utils.ExecNonQuery(strQuery, CnnString);
                    if (!j.Equals(0))
                    {
                        lblSucMess.Text = "Record Save Successfully.";
                    }
                    else
                    {
                        lblErrMess.Text = "Record Not Saved!!";
                    }

                }
                else
                {
                    lblErrMess.Text = "Record Not Found!!";
                }
               
            }

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            txtVehicleRegNo.Text = "";
            textBoxFrontPlate.Text = "";
            textBoxRearPlate.Text = "";
            txtRemarks.Text = string.Empty;
        }

    }
}
