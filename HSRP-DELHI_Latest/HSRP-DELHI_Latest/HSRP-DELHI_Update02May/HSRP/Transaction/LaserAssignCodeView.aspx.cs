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
    public partial class LaserAssignCodeView : System.Web.UI.Page
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

            SQLString = "SELECT  FrontPlateSize, RearPlateSize FROM HSRPRecords where HSRPRecordID='" + Request.QueryString["HSRPRecordID"].ToString() + "'";
            DataTable dtPlateSize = Utils.GetDataTable(SQLString, CnnString);
            if (dtPlateSize.Rows.Count > 0)
            {
                RearPlateSize = dtPlateSize.Rows[0]["RearPlateSize"].ToString();
            } 
            
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
                //textBoxFrontPlate.Text = ""; 
                //CheckBoxSticker.Enabled = false;
                //LabelSticker.Enabled = false;
                //LabelFrontPlateLeserNo.Enabled = false;
                //textBoxFrontPlate.Enabled = false;
                //LabelRearPlateLaserNo.Enabled = false;
                //textBoxRearPlate.Enabled = false;
                
                if (Mode == "Edit")
                {
                   HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                   AssignLaserEdit(HSRPRecordID);

                   //textBoxFrontPlate.Enabled = false;
                   //textBoxRearPlate.Enabled = false;
                   //textBoxLaserPlateBoxNo.Enabled = false;
                   //LabelOperatorName.Enabled = false;
                } 
            } 
        }
          
        private void AssignLaserEdit(string HSRPRecordID)
        {
            SQLString = "SELECT  HSRP_StateID,Remarks,CONVERT(varchar(20), HSRPRecords.OrderDate,103)  as OrderDate,CONVERT(varchar(20), HSRPRecords.OrderEmbossingDate,103)  as OrderEmbossingDate,CONVERT(varchar(20), HSRPRecords.OrderClosedDate,103)  as OrderClosedDate,LaserPlateBoxNo,  OrderStatus, HSRP_Sticker_LaserCode, ISFrontPlateSize, ISRearPlateSize, StickerMandatory, RTOLocationID, HSRPRecord_AuthorizationNo, OwnerName, MobileNo, VehicleClass, VehicleType, OrderType, VehicleRegNo, EngineNo, ChassisNo, CashReceiptNo, TotalAmount, FrontPlateSize, RearPlateSize, HSRP_Front_LaserCode, HSRP_Rear_LaserCode,CreatedBy,OperatorID, ManufacturerName,  ManufacturerModel FROM HSRPRecords where HSRPRecordID=" + HSRPRecordID;
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            if (ds.Rows.Count > 0)
            {

                LabelDataEntryDate.Text = ds.Rows[0]["OrderDate"].ToString();
                LabelRemarks.Text = ds.Rows[0]["Remarks"].ToString(); 
                LabelAuthorizationNo.Text = ds.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(); 

                LabelVehicleOwnerName.Text = ds.Rows[0]["OwnerName"].ToString();
                LabelMobileNo.Text = ds.Rows[0]["MobileNo"].ToString();
                LabelVehicleClass.Text = ds.Rows[0]["VehicleClass"].ToString();
                LabelVehicleType.Text = ds.Rows[0]["VehicleType"].ToString();
                LabelVehicleRegNo.Text = ds.Rows[0]["VehicleRegNo"].ToString();
                LabelPlateBoxNo.Text = ds.Rows[0]["LaserPlateBoxNo"].ToString();
                Labelorderstatus.Text = ds.Rows[0]["OrderStatus"].ToString();
                LabelEmbdate.Text = ds.Rows[0]["OrderEmbossingDate"].ToString();
                Labelcloesddate.Text = ds.Rows[0]["OrderClosedDate"].ToString();
                if (LabelPlateBoxNo.Text == "")
                {
                    LabelPlateBoxNo.Text = "0";
                } 
                LabelEngineNumber.Text = ds.Rows[0]["EngineNo"].ToString();
                LabelChasisNo.Text = ds.Rows[0]["ChassisNo"].ToString();
                LabelCashReceiptNo.Text = ds.Rows[0]["CashReceiptNo"].ToString();
                LabelTotalAmount.Text = ds.Rows[0]["TotalAmount"].ToString();

                LabelFrontPlate.Text = ds.Rows[0]["HSRP_Front_LaserCode"].ToString();
                LabelRearPlateNo.Text = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString();
                LabelOrderType.Text = ds.Rows[0]["OrderType"].ToString();
                ISFrontPlateSize = ds.Rows[0]["ISFrontPlateSize"].ToString();
                ISRearPlateSize = ds.Rows[0]["ISRearPlateSize"].ToString();
                StickerMandatory = ds.Rows[0]["StickerMandatory"].ToString();
                 

                SQLString = "select HSRPStateName from HSRPState where HSRP_StateID=" + ds.Rows[0]["HSRP_StateID"];
                DataTable dtState = Utils.GetDataTable(SQLString, CnnString);
                if (dtState.Rows.Count > 0)
                {
                    LabelStateID.Text = dtState.Rows[0]["HSRPStateName"].ToString();
                }

                SQLString = "Select RTOLocationName from RTOLocation where RTOLocationID=" + ds.Rows[0]["RTOLocationID"];
                DataTable dtLocation = Utils.GetDataTable(SQLString, CnnString);
                if (dtLocation.Rows.Count > 0)
                {
                    LabelRTOLocationID.Text = dtLocation.Rows[0]["RTOLocationName"].ToString();
                }

                RearPlateSize = ds.Rows[0]["RearPlateSize"].ToString();
                SQLString = "select ProductCode from Product where ProductID='"+ RearPlateSize+"'" ;
                DataTable dtz = Utils.GetDataTable(SQLString, CnnString);
                if (dtz.Rows.Count > 0)
                {
                    LabelRealPlateSizeAndColor.Text = dtz.Rows[0]["ProductCode"].ToString();
                }

                 FrontPlateSize = ds.Rows[0]["FrontPlateSize"].ToString();
                SQLString = "select ProductCode from Product where ProductID='"+ FrontPlateSize+"'";
                DataTable dtFront = Utils.GetDataTable(SQLString, CnnString);
                if (dtFront.Rows.Count > 0)
                {
                    LabelFrontPlateSizeAndColor.Text = dtFront.Rows[0]["ProductCode"].ToString();
                }

                //--------------------------------- For Creade By User Name -----------------------------------
                
                string CreatedBy = ds.Rows[0]["CreatedBy"].ToString();
                SQLString = "select (UserFirstName +' '+UserLastName) as FullName from users where UserID='" + CreatedBy + "'";
                DataTable dtCreatedBy = Utils.GetDataTable(SQLString, CnnString);
                if (dtCreatedBy.Rows.Count > 0)
                {
                    LabelOrderBookBy.Text = dtCreatedBy.Rows[0]["FullName"].ToString();
                }

                //--------------------------------- For Operator ID By Operator Name -----------------------------------

                string OperatorID = ds.Rows[0]["CreatedBy"].ToString();
                //SQLString = "select (UserFirstName +' '+UserLastName) as FullName from users where UserID='" + LabelOperatorID + "'";
                SQLString = "select OperatorID,OperatorName from Embossingoperators Where OperatorID='" + OperatorID + "'";
                DataTable dtLabelOperator = Utils.GetDataTable(SQLString, CnnString);
                if (dtLabelOperator.Rows.Count > 0)
                {
                    LabelOperatorName.Text = dtLabelOperator.Rows[0]["OperatorName"].ToString();
                }


                //
                //----------------------- Vehicle Maker & Model ------------------------------------
                try
                {
                    string ManufacturerName = Convert.ToString(ds.Rows[0]["ManufacturerName"]);
                    if (ManufacturerName == "" || ManufacturerName == "NULL" || ManufacturerName == "--Select Vehicle Maker--")
                    {
                        labelvehicleMaker.Text = "NA";
                    }
                    else
                    {
                        SQLString = "select VehicleMakerDescription from VehicleMakerMaster where VehicleMakerID='" + ManufacturerName + "'";
                        DataTable dtVehicleMaker = Utils.GetDataTable(SQLString, CnnString);
                        if (dtVehicleMaker.Rows.Count > 0)
                        {
                            labelvehicleMaker.Text = dtVehicleMaker.Rows[0]["VehicleMakerDescription"].ToString();
                        }
                    }

                    string ManufacturerModel = Convert.ToString(ds.Rows[0]["ManufacturerModel"]);
                    if (ManufacturerModel == "" || ManufacturerModel == "NULL" || ManufacturerModel == "--Select Vehicle Maker--")
                    {
                        labelvehicleModel.Text = "NA";
                    }
                    else
                    {
                        SQLString = "select VehicleModelDescription from VehicleModelMaster where VehicleModelID='" + ManufacturerModel + "'";
                        DataTable dtVehicleModel = Utils.GetDataTable(SQLString, CnnString);
                        if (dtVehicleModel.Rows.Count > 0)
                        {
                            labelvehicleModel.Text = dtVehicleModel.Rows[0]["VehicleModelDescription"].ToString();
                        }
                    }
                }
                catch
                {
                }
                //-------------------------------------------------------------------------
               

                //SQLString = "select OperatorID,OperatorName from Embossingoperators Where HSRP_StateID=" + intHSRPStateID + " Order by OperatorName";
                //Utils.PopulateDropDownList(dropDownListOperator, SQLString.ToString(), CnnString, "--Select Operator--");
                 
                

                //string orderStatus = ds.Rows[0]["OrderStatus"].ToString();
                //if (orderStatus == "Embossing Done" || orderStatus == "Closed")
                //{
                //    ////btnSave.Enabled = false;
                //}
                //if (StickerMandatory == "Y")
                //{
                //    CheckBoxSticker.Enabled = false;
                //    LabelSticker.Enabled = true;
                //    CheckBoxSticker.Checked = true;
                //}
                //else
                //{
                //    CheckBoxSticker.Enabled = false;
                //    LabelSticker.Enabled = false;
                //}

                //if (ISFrontPlateSize == "Y")
                //{
                //    LabelFrontPlateLeserNo.Enabled = true;
                //    textBoxFrontPlate.Enabled = true;
                //}
                //else
                //{
                //    LabelFrontPlateLeserNo.Enabled = false;
                //    textBoxFrontPlate.Enabled = false;
                //}
                //if (ISRearPlateSize == "Y")
                //{
                //    LabelRearPlateLaserNo.Enabled = true;
                //    textBoxRearPlate.Enabled = true;
                //}
                //else
                //{LabelRearPlateNo
                //    LabelRearPlateLaserNo.Enabled = false;
                //    textBoxRearPlate.Enabled = false;
                //}


                //if (textBoxFrontPlate.Text != "")
                //{
                //    textBoxFrontPlate.Enabled = false;
                //}
                //if (textBoxRearPlate.Text != "")
                //{
                //    textBoxRearPlate.Enabled = false;
                //}
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('No Record Found');</script>");
            }
        }
         
    }
}