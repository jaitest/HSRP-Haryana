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
    public partial class LaserCodeMakeFree : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        int UserType;
        string HSRPRecordID;
        string Mode;
        string UserID;
        string orderStatus;
         
        protected void Page_Load(object sender, EventArgs e)
        {  
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else if (!Page.IsPostBack)
            {
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                Mode = Request.QueryString["Mode"];
                UserID = Request.QueryString["UserID"];
                UserType = Convert.ToInt32(Session["UserType"]); 
                if (Mode == "LasderFree")
                {
                    HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                    CheckBoxSticker.Enabled = false;
                    LaserMakeFreeEdit(HSRPRecordID);
                } 

                if (LabelRealPlateLaserNo.Text == "")
                {
                   LinkButtonRearMakeitFree.Visible = false;
                }

                if (LabelFrontPlateLaserNo.Text == "")
                {
                    LinkButtonfrontMakeFree.Visible = false;
                }
            } 
        }
        protected void LinkButtonRearMakeitFree_Click(object sender, EventArgs e)
        {
            if (LabelRealPlateLaserNo.Text != "")
            { 
                if (UserType.Equals(0))
                {
                    HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                    int IsExists = -1;
                    BAL obj = new BAL();
                    obj.Transaction_UpdateAssignedLaserRear_MakeFree(LabelRealPlateLaserNo.Text, HSRPRecordID);
                    if (IsExists.Equals(1))
                    {
                        lblErrMess.Text = "Record Already Exist!!";
                    }
                    else
                    {
                        lblSucMess.Text = "Laser Code Successfully Free.";
                        LabelRealPlateLaserNo.Text = "";
                        LinkButtonRearMakeitFree.Visible = false;
                        LabelRearPlate.Visible = false;
                    }
                }
                else
                { 
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Please Admin Login');</script>");
                }
            }
        }

        protected void LinkButtonfrontMakeFree_Click(object sender, EventArgs e)
        {
            if (LabelFrontPlateLaserNo.Text != "")
            {
                if (UserType.Equals(0))
                {
                    HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                    int IsExists = -1;
                    BAL obj = new BAL();
                    obj.Transaction_UpdateAssignedLaser_MakeFree(LabelFrontPlateLaserNo.Text, HSRPRecordID);
                    if (IsExists.Equals(1))
                    {
                        lblErrMess.Text = "Record Already Exist!!";
                    }
                    else
                    {
                        lblSucMess.Text = "Laser Code Successfully Free.";
                        LabelFrontPlateLaserNo.Text = "";
                        LinkButtonfrontMakeFree.Visible = false;
                        LabelFrontPlateLaster.Visible = false;
                    }
                }
                else
                { 
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Please Admin Login');</script>");
                }
            }
        }


        private void LaserMakeFreeEdit(string HSRPRecordID)
        {
            SQLString = "SELECT HSRPRecords.HSRP_StateID, HSRPState.HSRPStateName,HSRPRecords.OrderStatus,HSRPRecords.ISFrontPlateSize,HSRPRecords.ISRearPlateSize,HSRPRecords.HSRP_Sticker_LaserCode, HSRPRecords.RTOLocationID, RTOLocation.RTOLocationName, HSRPRecords.HSRPRecord_AuthorizationNo, HSRPRecords.OwnerName, HSRPRecords.MobileNo, HSRPRecords.VehicleClass, HSRPRecords.VehicleType,HSRPRecords.OrderType, HSRPRecords.VehicleRegNo, HSRPRecords.EngineNo, HSRPRecords.ChassisNo, HSRPRecords.CashReceiptNo, HSRPRecords.TotalAmount, HSRPRecords.FrontPlateSize, HSRPRecords.RearPlateSize, HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode FROM   HSRPRecords INNER JOIN  HSRPState ON HSRPRecords.HSRP_StateID = HSRPState.HSRP_StateID INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where HSRPRecordID=" + HSRPRecordID;
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);

            LabelAuthorizationNo.Text = ds.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
            LabelStateID.Text = ds.Rows[0]["HSRPStateName"].ToString();
            LabelRTOLocationID.Text = ds.Rows[0]["RTOLocationName"].ToString();
            LabelVehicleOwnerName.Text = ds.Rows[0]["OwnerName"].ToString();
            LabelMobileNo.Text = ds.Rows[0]["MobileNo"].ToString();
            LabelVehicleClass.Text = ds.Rows[0]["VehicleClass"].ToString();
            LabelVehicleType.Text = ds.Rows[0]["VehicleType"].ToString();
            LabelVehicleRegNo.Text = ds.Rows[0]["VehicleRegNo"].ToString();

            LabelEngineNumber.Text = ds.Rows[0]["EngineNo"].ToString();
            LabelChasisNo.Text = ds.Rows[0]["ChassisNo"].ToString();
            LabelCashReceiptNo.Text = ds.Rows[0]["CashReceiptNo"].ToString();
            LabelTotalAmount.Text = ds.Rows[0]["TotalAmount"].ToString();
            LabelFrontPlateLaserNo.Text = ds.Rows[0]["HSRP_Front_LaserCode"].ToString();
            LabelRealPlateLaserNo.Text = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString();
            SQLString = "select ProductID, ProductCode from Product where ProductID=" + ds.Rows[0]["FrontPlateSize"];
            DataTable dtz = Utils.GetDataTable(SQLString, CnnString);
            LabelFrontPlateSizeAndColor.Text = dtz.Rows[0]["ProductCode"].ToString();
            SQLString = "select ProductID, ProductCode from Product where ProductID=" + ds.Rows[0]["RearPlateSize"];
            DataTable dz = Utils.GetDataTable(SQLString, CnnString);
            LabelRealPlateSizeColor.Text = dz.Rows[0]["ProductCode"].ToString();
            string sticker = ds.Rows[0]["HSRP_Sticker_LaserCode"].ToString();
            if (sticker == "Y")
            {
                CheckBoxSticker.Checked = true;
            }
            else
            {
                CheckBoxSticker.Checked = false;
            }
            orderStatus = ds.Rows[0]["OrderStatus"].ToString();
            if (orderStatus == "Embossing Done" || orderStatus == "Closed")
            {
                LinkButtonfrontMakeFree.Visible = false;
                LinkButtonRearMakeitFree.Visible = false;
                LabelFrontPlateLaster.Visible = false;
                LabelRearPlate.Visible = false;
            }
        }  
    }
}