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
    public partial class LaserAssignCode : System.Web.UI.Page
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
            AutoCompleteExtender1.ContextKey = Session["UserHSRPStateID"].ToString() + "/" + dtPlateSize.Rows[0]["FrontPlateSize"].ToString() + "/" + dtPlateSize.Rows[0]["RearPlateSize"].ToString();
            AutoCompleteExtender2.ContextKey = Session["UserHSRPStateID"].ToString() + "/" + dtPlateSize.Rows[0]["FrontPlateSize"].ToString() + "/" + dtPlateSize.Rows[0]["RearPlateSize"].ToString();

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
               // UserID = Request.QueryString["UserID"];
                textBoxFrontPlate.Text = ""; 
                CheckBoxSticker.Enabled = false;
                LabelSticker.Enabled = false;
                LabelFrontPlateLeserNo.Enabled = false;
                textBoxFrontPlate.Enabled = false;
                LabelRearPlateLaserNo.Enabled = false;
                textBoxRearPlate.Enabled = false;
                SQLString = "select OperatorID,OperatorName from Embossingoperators Where HSRP_StateID=" + intHSRPStateID + " Order by OperatorName";
                Utils.PopulateDropDownList(dropDownListOperator, SQLString.ToString(), CnnString, "--Select Operator--");
                if (Mode == "Edit")
                {
                   HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                   AssignLaserEdit(HSRPRecordID);

                } 
            } 
        }
         
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = ""; 
            btnSave.Enabled = false;

            string FrontLaserPlate =textBoxFrontPlate.Text.ToUpper().Trim();
            string RearLaserPlate = textBoxRearPlate.Text.ToUpper().Trim();

            if (FrontLaserPlate == RearLaserPlate)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Same Laser No Could Not be Assigned!!');</script>");
                }
                else
                {
                    SQLString = "select HSRP_StateID,RearPlateSize,FrontPlateSize, isfrontPlateSize, IsRearPlateSize  from HSRPRecords where HSRPRecordID='" + Request.QueryString["HSRPRecordID"].ToString() + "'";
                    DataTable badt = Utils.GetDataTable(SQLString.ToString(), CnnString);

                   // SQLString = "select InventoryStatus, LaserNo from RTOInventory where LaserNo='" + FrontLaserPlate + "'or LaserNo='" + RearLaserPlate + "'";
                    SQLString = "select InventoryStatus, LaserNo from RTOInventory where LaserNo='" + FrontLaserPlate + "' and Hsrp_StateID='" + badt.Rows[0]["Hsrp_StateID"].ToString() + "' and ProductID='" + badt.Rows[0]["FrontPlateSize"].ToString() + "'";
                    DataTable dt= Utils.GetDataTable(SQLString.ToString(), CnnString);
                    try
                    {
                        if (badt.Rows[0]["isfrontPlateSize"].ToString() == "Y")
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["InventoryStatus"] != null)
                                {
                                    FrontReject = dt.Rows[0]["InventoryStatus"].ToString();
                                    FrontRejectLaserNo = dt.Rows[0]["LaserNo"].ToString();
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Selected  Inventory Not Uploaded!!');</script>");
                                return;
                            }

                        }
                        else
                        {
                            FrontReject = "0";
                            FrontRejectLaserNo ="0";
                        }

                        SQLString = "select InventoryStatus, LaserNo from RTOInventory where LaserNo='" + RearLaserPlate + "' and Hsrp_StateID='" + badt.Rows[0]["Hsrp_StateID"].ToString() + "' and ProductID='" + badt.Rows[0]["RearPlateSize"].ToString() + "'";
                        DataTable dtrear = Utils.GetDataTable(SQLString.ToString(), CnnString);

                        if (badt.Rows[0]["IsRearPlateSize"].ToString() == "Y")
                        {
                            if (dtrear.Rows.Count > 0)
                            {

                                if (dtrear.Rows[0]["InventoryStatus"] != null)
                                {
                                    RearReject = dtrear.Rows[0]["InventoryStatus"].ToString();
                                    RearRejectLaserNo = dtrear.Rows[0]["LaserNo"].ToString();
                                }
                            } 
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Selected Inventory Not Uploaded!!');</script>");
                                return;
                            }
                            
                        }
                        else
                        {
                            RearReject = "0";
                            RearRejectLaserNo = "0";
                        }
                    }
                    catch
                    {
                    }
                    if (FrontReject != "Reject" && RearReject != "Reject" && FrontRejectLaserNo != null && RearRejectLaserNo!=null)
                    { 
                        RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                        HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                        HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                        string OperatorID = string.Empty;// = dropDownListOperator.SelectedValue;
                        if (dropDownListOperator.SelectedValue == "--Select Operator--")
                        {
                            OperatorID = "0";
                        }
                        else
                        {
                            OperatorID = dropDownListOperator.SelectedValue;
                        }
                         
                        string HSRP_Sticker_LaserCode;
                        if (CheckBoxSticker.Checked == true)
                        {
                            HSRP_Sticker_LaserCode = "Y";
                        }
                        else
                        {
                            HSRP_Sticker_LaserCode = "N";
                        }
                        HSRPRecordID = Request.QueryString["HSRPRecordID"].ToString();
                        SQLString = "select hsrp_front_LaserCode, hsrp_rear_LaserCode,isfrontPlateSize, IsRearPlateSize  from HSRPRecords where HSRPRecordID='" + HSRPRecordID + "'";
                        DataTable dtLaserCheckBoth = Utils.GetDataTable(SQLString.ToString(), CnnString);
                        if (dtLaserCheckBoth.Rows.Count > 0)
                        {
                            if (dtLaserCheckBoth.Rows[0]["isfrontPlateSize"].ToString() == "Y")
                            {
                                if (dtLaserCheckBoth.Rows[0]["hsrp_front_LaserCode"].ToString() == "")
                                {
                                    SQLString = "select hsrp_front_LaserCode  from HSRPRecords where hsrp_front_LaserCode='" + FrontLaserPlate + "'";
                                    DataTable dtLaserCheckFront = Utils.GetDataTable(SQLString.ToString(), CnnString);
                                    if (dtLaserCheckFront.Rows.Count > 0)
                                    {
                                        lblErrMess.Text = "Front Plate Record Already Exist!!";
                                        return;
                                    }
                                }
                            }
                            if (dtLaserCheckBoth.Rows[0]["IsRearPlateSize"].ToString() == "Y")
                            {
                                if (dtLaserCheckBoth.Rows[0]["hsrp_rear_LaserCode"].ToString() == "")
                                {
                                    SQLString = "select hsrp_rear_LaserCode  from HSRPRecords where hsrp_rear_LaserCode='" + RearLaserPlate + "'";
                                    DataTable dtLaserCheckRear = Utils.GetDataTable(SQLString.ToString(), CnnString);
                                    if (dtLaserCheckRear.Rows.Count > 0)
                                    {
                                        lblErrMess.Text = "Rear Plate Record Already Exist!!";
                                        return;
                                    }
                                }
                            }
                            string UID = Session["UID"].ToString();
                            int k;
                            int z;
                            SQLString = "update HSRPRecords set OrderStatus='Embossing Done',OrderEmbossingDate=Getdate(),hsrp_front_LaserCode='" + FrontLaserPlate + "', hsrp_rear_LaserCode='" + RearLaserPlate + "', LaserPlateBoxNo='" + textBoxLaserPlateBoxNo.Text + "', HSRP_Sticker_LaserCode='" + HSRP_Sticker_LaserCode + "',OperatorID='" + OperatorID + "',EmbossingUserID='" + UID + "'  where HSRPRecordID='" + HSRPRecordID + "'";
                          int i = Utils.ExecNonQuery(SQLString, CnnString);

                          if (i > 0)
                          {
                              //For Embossing 
                              string Query = "select max(Embossing)+1 as Embossing from rtolocation where rtolocationid='" + RTOLocationID + "'";
                              DataTable dt1 = new DataTable();
                              dt1 = Utils.GetDataTable(Query, CnnString);
                              Query = "update rtolocation set Embossing='" + dt1.Rows[0]["Embossing"].ToString() + "',lastEmbDate=getdate() where rtolocationid='" + RTOLocationID + "'";
                              Utils.ExecNonQuery(Query, CnnString);


                              if (FrontLaserPlate != "0")
                              {
                                  SQLString = "update RTOInventory set InventoryStatus='Embossing Done',HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "',HSRPRecordID='" + HSRPRecordID + "', EmbossingDate=GETDATE() where  LaserNo = '" + FrontLaserPlate + "'";
                                   z = Utils.ExecNonQuery(SQLString, CnnString);
                              }
                              else
                              {
                                  z = 1;
                              }
                              if (z > 0)
                              {
                                  if (RearLaserPlate != "0")
                                  {
                                      SQLString = "update RTOInventory set InventoryStatus='Embossing Done',HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "',HSRPRecordID='" + HSRPRecordID + "', EmbossingDate=GETDATE() where LaserNo= '" + RearLaserPlate + "'";
                                       k = Utils.ExecNonQuery(SQLString, CnnString);
                                  }
                                  else
                                  {
                                      k = 1;
                                  }
                                 if (k == 0)
                                 {
                                     SQLString = "update HSRPRecords set OrderStatus='New Order',OrderEmbossingDate=Getdate(),hsrp_front_LaserCode='', hsrp_rear_LaserCode='', LaserPlateBoxNo='', HSRP_Sticker_LaserCode='',OperatorID=''  where HSRPRecordID='" + HSRPRecordID + "'";
                                     Utils.ExecNonQuery(SQLString, CnnString);
                                     SQLString = "update RTOInventory set InventoryStatus='New Order',HSRP_StateID='',RTOLocationID='',HSRPRecordID='', EmbossingDate='' where  LaserNo = '" + FrontLaserPlate + "'";
                                     Utils.ExecNonQuery(SQLString, CnnString);
                                     SQLString = "update RTOInventory set InventoryStatus='New Order',HSRP_StateID='',RTOLocationID='',HSRPRecordID='', EmbossingDate='' where LaserNo= '" + RearLaserPlate + "'";
                                     Utils.ExecNonQuery(SQLString, CnnString);

                                     lblErrMess.Text = "Record Not Updated";
                                 } 
                              }

                              lblSucMess.Text = "Record Update Successfully.";
                          }
                          else
                          {
                              lblErrMess.Text = "Record Not Updated";
                          } 
                        } 
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Selected Inventory Not Uploaded!!');</script>");
                    }
                } 
        } 
        private void AssignLaserEdit(string HSRPRecordID)
        {
            SQLString = "SELECT     HSRP_StateID,CONVERT(varchar(20), HsrpRecord_AuthorizationDate, 103) AS HsrpRecord_AuthorizationDate,  CONVERT(varchar(20), OrderDate, 103) AS OrderDate,CONVERT(varchar(20), OrderEmbossingDate, 103) AS OrderEmbossingDate,CONVERT(varchar(20), InvoiceDateTime, 103) AS InvoiceDateTime,Remarks,LaserPlateBoxNo, OrderStatus,isfrontPlateSize,IsRearPlateSize, HSRP_Sticker_LaserCode, ISFrontPlateSize, ISRearPlateSize, StickerMandatory, RTOLocationID, HSRPRecord_AuthorizationNo, OwnerName, MobileNo, VehicleClass, VehicleType, OrderType, VehicleRegNo, EngineNo, ChassisNo, CashReceiptNo, NetAmount as TotalAmount, FrontPlateSize, RearPlateSize, HSRP_Front_LaserCode, HSRP_Rear_LaserCode,CreatedBy, ManufacturerName,  ManufacturerModel FROM HSRPRecords where HSRPRecordID=" + HSRPRecordID;
            DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            if (ds.Rows.Count > 0)
            {

                LabelOrderBookDate.Text = ds.Rows[0]["OrderDate"].ToString();
                LabelInvoiceDate.Text = ds.Rows[0]["InvoiceDateTime"].ToString();
                LabelEmbossingDate.Text = ds.Rows[0]["OrderEmbossingDate"].ToString();
                LabelAuthorizationDate.Text = ds.Rows[0]["HsrpRecord_AuthorizationDate"].ToString();
               // lblRemarks.Text = ds.Rows[0]["Remarks"].ToString();
                
                

                LabelAuthorizationNo.Text = ds.Rows[0]["HSRPRecord_AuthorizationNo"].ToString(); 

                LabelVehicleOwnerName.Text = ds.Rows[0]["OwnerName"].ToString();
                LabelMobileNo.Text = ds.Rows[0]["MobileNo"].ToString();
                LabelVehicleClass.Text = ds.Rows[0]["VehicleClass"].ToString();
                LabelVehicleType.Text = ds.Rows[0]["VehicleType"].ToString();
                LabelVehicleRegNo.Text = ds.Rows[0]["VehicleRegNo"].ToString();
                textBoxLaserPlateBoxNo.Text= ds.Rows[0]["LaserPlateBoxNo"].ToString();
                if (textBoxLaserPlateBoxNo.Text == "")
                {
                    textBoxLaserPlateBoxNo.Text = "0";
                }

                LabelEngineNumber.Text = ds.Rows[0]["EngineNo"].ToString();
                LabelChasisNo.Text = ds.Rows[0]["ChassisNo"].ToString();
                LabelCashReceiptNo.Text = ds.Rows[0]["CashReceiptNo"].ToString();
                LabelTotalAmount.Text = ds.Rows[0]["TotalAmount"].ToString();

                string FrontLaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString();
                if (FrontLaserCode == "null")
                {
                    textBoxFrontPlate.Text = "";
                }
                string FearLaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString();
                if (FearLaserCode == "null")
                {
                    textBoxRearPlate.Text = "";
                }

                string FrontLaserCodeother = ds.Rows[0]["HSRP_Front_LaserCode"].ToString();
                if (FrontLaserCodeother == "NULL")
                {
                    textBoxFrontPlate.Text = "";
                }
                string FearLaserCodeother = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString();
                if (FearLaserCodeother == "NULL")
                {
                    textBoxRearPlate.Text = "";
                } 

                LabelOrderType.Text = ds.Rows[0]["OrderType"].ToString();
                ISFrontPlateSize = ds.Rows[0]["ISFrontPlateSize"].ToString();
                ISRearPlateSize = ds.Rows[0]["ISRearPlateSize"].ToString();

                StickerMandatory = ds.Rows[0]["StickerMandatory"].ToString();

                ViewState["RTOLocationIDAssign"] = Convert.ToInt16(ds.Rows[0]["RTOLocationID"]);
                ViewState["StateIDAssign"] = Convert.ToInt16(ds.Rows[0]["HSRP_StateID"]);

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
                if (ds.Rows[0]["IsRearPlateSize"].ToString() == "Y")
                {
                    RearPlateSize = ds.Rows[0]["RearPlateSize"].ToString();
                    if (RearPlateSize != "--Select Rear Plate--")
                    {
                        SQLString = "select ProductCode from Product where ProductID='" + RearPlateSize + "'";
                        DataTable dtz = Utils.GetDataTable(SQLString, CnnString);
                        if (dtz.Rows.Count > 0)
                        {
                            LabelRealPlateSizeAndColor.Text = dtz.Rows[0]["ProductCode"].ToString();
                        }
                    }
                    else
                    {
                        btnSave.Visible = false;
                        lblErrMessvehicle.Text = "Rear Plate Size Not Found";
                    }
                }
                else
                {
                    LabelRealPlateSizeAndColor.Text = ds.Rows[0]["RearPlateSize"].ToString();
                    LabelRealPlateSizeAndColor.Enabled = false;
                    textBoxRearPlate.Text = "0";
                    textBoxRearPlate.Enabled = false;
                }

                if (ds.Rows[0]["isfrontPlateSize"].ToString() == "Y")
                {
                    FrontPlateSize = ds.Rows[0]["FrontPlateSize"].ToString();
                    if (FrontPlateSize != "--Select Front Plate--")
                    {
                        SQLString = "select ProductCode from Product where ProductID='" + FrontPlateSize + "'";
                        DataTable dtFront = Utils.GetDataTable(SQLString, CnnString);
                        if (dtFront.Rows.Count > 0)
                        {
                            LabelFrontPlateSizeAndColor.Text = dtFront.Rows[0]["ProductCode"].ToString();
                        }
                    }
                    else
                    {
                        btnSave.Visible = false;
                        lblErrMessvehicleFront.Text = "Front Plate Size Not Found";
                    }
                }

                else
                {
                    LabelFrontPlateSizeAndColor.Text = ds.Rows[0]["FrontPlateSize"].ToString();
                    textBoxFrontPlate.Text = "0";
                    textBoxFrontPlate.Enabled = false;
                    LabelFrontPlateSizeAndColor.Enabled = false;
                }

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
               

                SQLString = "select OperatorID,OperatorName from Embossingoperators Where HSRP_StateID=" + intHSRPStateID + " Order by OperatorName";
                Utils.PopulateDropDownList(dropDownListOperator, SQLString.ToString(), CnnString, "--Select Operator--");
                 


                string orderStatus = ds.Rows[0]["OrderStatus"].ToString();
                if (orderStatus == "Embossing Done" || orderStatus == "Closed")
                {
                    btnSave.Enabled = false;
                }
                if (StickerMandatory == "Y")
                {
                    CheckBoxSticker.Enabled = false;
                    LabelSticker.Enabled = true;
                    CheckBoxSticker.Checked = true;
                }
                else
                {
                    CheckBoxSticker.Enabled = false;
                    LabelSticker.Enabled = false;
                }

                if (ISFrontPlateSize == "Y")
                {
                    LabelFrontPlateLeserNo.Enabled = true;
                    textBoxFrontPlate.Enabled = true;
                }
                else
                {
                    LabelFrontPlateLeserNo.Enabled = false;
                    textBoxFrontPlate.Enabled = false;
                }
                if (ISRearPlateSize == "Y")
                {
                    LabelRearPlateLaserNo.Enabled = true;
                    textBoxRearPlate.Enabled = true;
                }
                else
                {
                    LabelRearPlateLaserNo.Enabled = false;
                    textBoxRearPlate.Enabled = false;
                }


                if (textBoxFrontPlate.Text != "")
                {
                    textBoxFrontPlate.Enabled = false;
                }
                if (textBoxRearPlate.Text != "")
                {
                    textBoxRearPlate.Enabled = false;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('No Record Found');</script>");
            }
        }
         
    }
}