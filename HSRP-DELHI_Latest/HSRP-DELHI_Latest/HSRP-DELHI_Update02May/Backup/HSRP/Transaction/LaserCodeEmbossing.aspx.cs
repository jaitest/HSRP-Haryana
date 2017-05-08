using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataProvider;


namespace HSRP.Master
{
    public partial class LaserCodeEmbossing : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        int UserType;
        string HSRPRecordID = string.Empty;
        string Mode;
        string orderstatus;
        string frontLaser;
        string RearLaser;
        string UserID;

        protected void Page_Load(object sender, EventArgs e)
        {
             
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            if (!Page.IsPostBack)
            {
                Mode = Request.QueryString["Mode"];
                UserID = Request.QueryString["UserID"];
                CheckBoxSticker.Enabled = false;
                UserType = Convert.ToInt32(Session["UserType"]);
                if (Mode == "Embossing")
                {
                    HSRPRecordID = Convert.ToString(Request.QueryString["HSRPRecordID"]);
                    LaserCodeEdit(HSRPRecordID);

                }

            }
        } 
         
        protected void LinkButtonFrontLaserNoFree_Click(object sender, EventArgs e)
        {
            if (LabelFrontPlateLaserNo.Text != "")
            {
                if (UserType.Equals(0))
                { 
                    string FrontLaserCode = LabelFrontPlateLaserNo.Text;
                    HSRPRecordID = Convert.ToString(Request.QueryString["HSRPRecordID"]);
                    int IsExists = -1;
                    BAL obj = new BAL();
                    obj.Transaction_UpdateAssignedLaser_MakeFree(FrontLaserCode, HSRPRecordID);
                    if (IsExists.Equals(1))
                    {
                        lblErrMess.Text = "Record Already Exist!!";
                    }
                    else
                    {
                        lblSucMess.Text = "Laser Code Successfully Free.";
                        LabelFrontPlateLaserNo.Text = "";
                        LinkButtonFrontLaserNoFree.Visible = false;
                        LinkButtonFrontLaserNoReject.Visible = false;
                       //btnSave.Enabled = false;
                        LabelFrontPlate.Visible = false; 
                    }
                }
                else
                {
                   // btnSave.Enabled = false;
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Please Admin Login');</script>");
                }
            }
        }

        protected void LinkButtonFrontLaserNoReject_Click(object sender, EventArgs e)
        {
            if (LabelFrontPlateLaserNo.Text != "")
            {
                string OrderStatus = "Reject";
                string FrondLaserCode = LabelFrontPlateLaserNo.Text;
                HSRPRecordID = Convert.ToString(Request.QueryString["HSRPRecordID"]);
                string Type = "FrontPlate";
                int IsExists = -1;
                BAL obj = new BAL();
                obj.PlateFrontRejectEmbossing(OrderStatus, FrondLaserCode, HSRPRecordID);
                if (IsExists.Equals(1))
                {
                    lblErrMess.Text = "Record Already Rejected!!";
                }
                else
                {
                    lblSucMess.Text = "Laser Code Successfully Rejected.";
                    LabelFrontPlateLaserNo.Text = "";
                    LinkButtonFrontLaserNoReject.Visible = false;
                    LinkButtonFrontLaserNoFree.Visible = false;
                   // btnSave.Enabled = false;
                    LabelFrontPlate.Visible = false;
                }
            }
            else
            {
                lblErrMess.Text = "Check Recject!!";
            }
        }

        protected void LinkButtonRearLaserNoReject_Click(object sender, EventArgs e)
        {
            if (LabelRearPlateLaserNo.Text != "")
            {
                HSRPRecordID = Convert.ToString(Request.QueryString["HSRPRecordID"]);
                string OrderStatus = "Reject";  
                int IsExists = -1;
                BAL obj = new BAL();
                obj.PlateRearRejectEmbossing(OrderStatus, LabelRearPlateLaserNo.Text, HSRPRecordID);

                if (IsExists.Equals(1))
                {
                    lblErrMess.Text = "Record Already Rejected!!";
                }
                else
                {
                    lblSucMess.Text = "Laser Code Successfully Rejected.";
                    LabelRearPlateLaserNo.Text = "";
                    LinkButtonRearLaserNoReject.Visible = false;
                    LinkButtonRearLaserNoFree.Visible = false;
                    LabelRearPlate.Visible = false;
                    //btnSave.Enabled = false;
                }
            }
            else
            {
                lblErrMess.Text = "Check Recject!!";
            }
           
        }

        protected void LinkButtonRearLaserNoFree_Click(object sender, EventArgs e)
        {
            if (LabelRearPlateLaserNo.Text != "" || orderstatus=="Closed")
            { 
                if (UserType.Equals(0))
                {
                    HSRPRecordID = Convert.ToString(Request.QueryString["HSRPRecordID"]);
                    int IsExists = -1;
                    BAL obj = new BAL();
                    obj.Transaction_UpdateAssignedLaserRear_MakeFree(LabelRearPlateLaserNo.Text, HSRPRecordID);
                    if (IsExists.Equals(1))
                    {
                        lblErrMess.Text = "Record Already Exist!!";
                    }
                    else
                    {
                        lblSucMess.Text = "Laser Code Successfully Free.";
                        LabelRearPlateLaserNo.Text = "";
                        LinkButtonRearLaserNoFree.Visible = false;
                        LinkButtonRearLaserNoReject.Visible = false;
                        LabelRearPlate.Visible = false;
                     //   btnSave.Enabled = false;
                    }
                }
                else
                {
                  //  btnSave.Enabled = false;
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Please Admin Login');</script>");
                }
            }
        }

        private void LaserCodeEdit(string HSRPRecordID)
        {
            SQLString = "SELECT HSRPRecords.HSRP_StateID, HSRPState.HSRPStateName,HSRPRecords.OrderStatus,isfrontPlateSize,IsRearPlateSize,HSRPRecords.ISFrontPlateSize,HSRPRecords.ISRearPlateSize,HSRPRecords.HSRP_Sticker_LaserCode, HSRPRecords.RTOLocationID, RTOLocation.RTOLocationName, HSRPRecords.HSRPRecord_AuthorizationNo, HSRPRecords.OwnerName, HSRPRecords.MobileNo, HSRPRecords.VehicleClass, HSRPRecords.VehicleType,HSRPRecords.OrderType, HSRPRecords.VehicleRegNo, HSRPRecords.EngineNo, HSRPRecords.ChassisNo, HSRPRecords.CashReceiptNo, HSRPRecords.TotalAmount, HSRPRecords.FrontPlateSize, HSRPRecords.RearPlateSize, HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode FROM   HSRPRecords INNER JOIN  HSRPState ON HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where HSRPRecordID=" + HSRPRecordID;
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);

            LabelAuthorizationNo.Text = dt.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
            LabelStateID.Text = dt.Rows[0]["HSRPStateName"].ToString();

            LabelRTOLocationID.Text = dt.Rows[0]["RTOLocationName"].ToString();
            LabelVehicleOwnerName.Text = dt.Rows[0]["OwnerName"].ToString();
            LabelMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
            LabelVehicleClass.Text = dt.Rows[0]["VehicleClass"].ToString();
            LabelVehicleType.Text = dt.Rows[0]["VehicleType"].ToString();
            LabelVehicleRegNo.Text = dt.Rows[0]["VehicleRegNo"].ToString();
            LabelEngineNumber.Text = dt.Rows[0]["EngineNo"].ToString();
            LabelChasisNo.Text = dt.Rows[0]["ChassisNo"].ToString();
            LabelCashReceiptNo.Text = dt.Rows[0]["CashReceiptNo"].ToString();
            LabelTotalAmount.Text = dt.Rows[0]["TotalAmount"].ToString();
            LabelFrontPlateLaserNo.Text = dt.Rows[0]["HSRP_Front_LaserCode"].ToString();
            LabelRearPlateLaserNo.Text = dt.Rows[0]["HSRP_Rear_LaserCode"].ToString();
            orderstatus = dt.Rows[0]["OrderStatus"].ToString();
            RearLaser = dt.Rows[0]["HSRP_Rear_LaserCode"].ToString();
            frontLaser = dt.Rows[0]["HSRP_Front_LaserCode"].ToString();
            if (dt.Rows[0]["isfrontPlateSize"].ToString() == "Y")
            {
                SQLString = "select ProductID, ProductCode from Product where ProductID=" + dt.Rows[0]["FrontPlateSize"];
                DataTable dtz = Utils.GetDataTable(SQLString, CnnString);
                LabelFrontPlateSizeAndColor.Text = dtz.Rows[0]["ProductCode"].ToString();
            }
            else
            {
                LinkButtonFrontLaserNoFree.Visible = false;
                LinkButtonFrontLaserNoReject.Visible = false;
            }
            if (dt.Rows[0]["IsRearPlateSize"].ToString() == "Y")
            {
                SQLString = "select ProductID, ProductCode from Product where ProductID=" + dt.Rows[0]["RearPlateSize"];
                DataTable dz = Utils.GetDataTable(SQLString, CnnString);
                LabelRealPlateSizeColor.Text = dz.Rows[0]["ProductCode"].ToString();
            }
            else
            {
                LinkButtonRearLaserNoFree.Visible = false;
                LinkButtonRearLaserNoReject.Visible = false;
            }
            string sticker = dt.Rows[0]["HSRP_Sticker_LaserCode"].ToString();
            if (sticker == "Y")
            {
                CheckBoxSticker.Checked = true;
            }
            else
            {
                CheckBoxSticker.Checked = false;
            }

            if (orderstatus == "Closed")
            {
                LinkButtonFrontLaserNoFree.Enabled = false;
                LinkButtonFrontLaserNoReject.Enabled = false;
                LinkButtonRearLaserNoFree.Enabled = false;
                LinkButtonRearLaserNoReject.Enabled = false;
               // btnSave.Enabled = false;
            }

            if (LabelFrontPlateLaserNo.Text == "")
            {
                LinkButtonFrontLaserNoFree.Visible = false;
                LinkButtonFrontLaserNoReject.Visible = false;
                LabelFrontPlate.Visible = false;
            }
            if (LabelRearPlateLaserNo.Text == "")
            {
                LinkButtonRearLaserNoFree.Visible = false;
                LinkButtonRearLaserNoReject.Visible = false;
                LabelRearPlate.Visible = false;
            }
        }
          
    }
}