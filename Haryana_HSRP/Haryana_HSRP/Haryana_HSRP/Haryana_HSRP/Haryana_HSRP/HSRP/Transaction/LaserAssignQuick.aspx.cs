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
    public partial class LaserAssignQuick : System.Web.UI.Page
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
             
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
            }
            AutoCompleteExtender1.ContextKey = Session["UserHSRPStateID"].ToString();
            AutoCompleteExtender2.ContextKey = Session["UserHSRPStateID"].ToString();

           

            SQLString = "select count(*) from laserAssignquick where userID='" + Session["UID"].ToString() + "' and recordDate between (convert(varchar,GetDate(),111)+' 00:00:00') and (convert(varchar,GetDate(),111)+' 23:59:59')";
            
            int totalrecord = Utils.getScalarCount(SQLString, CnnString);
            if (totalrecord > 0)
            {
                lbltotalSavedRecord.Text = Convert.ToString(totalrecord);
            }

            if (!Page.IsPostBack)
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"].ToString());
                Mode = Request.QueryString["Mode"];
                // UserID = Request.QueryString["UserID"];
                textBoxFrontPlate.Text = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            // btnSave.Enabled = false;

            string FrontLaserPlate = textBoxFrontPlate.Text.ToUpper().Trim();
            string RearLaserPlate = textBoxRearPlate.Text.ToUpper().Trim();

            SQLString = "select RecordID from LaserAssignQuick where vehicleRegNo='" + txtVehicleRegNo.Text + "'";
            DataTable dtcount = Utils.GetDataTable(SQLString.ToString(), CnnString);
            if (dtcount.Rows.Count > 0)
            {
                lblErrMess.Text = "Record Already Exist!!";
                textBoxFrontPlate.Text = "";
                textBoxRearPlate.Text = "";
            }
            else
            {
                SQLString = "select hsrpRecordID from hsrprecords where hsrp_stateid ='"+ HSRPStateID +"' and ' vehicleRegNo='" + txtVehicleRegNo.Text + "'";
                dtcount = Utils.GetDataTable(SQLString.ToString(), CnnString);
                if (dtcount.Rows.Count > 0)
                {
                    lblErrMess.Text = "Record Already Exist!!";
                    textBoxFrontPlate.Text = "";
                    textBoxRearPlate.Text = "";
                }
                else
                {

                    if (FrontLaserPlate == RearLaserPlate)
                    {
                        //Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Same Laser No Could Not be Assigned!!');</script>");
                        lblErrMess.Text = "Same Laser No Could Not be Assigned!!";
                    }
                    else
                    {
                        //SQLString = "select HSRP_StateID,RearPlateSize,FrontPlateSize, isfrontPlateSize, IsRearPlateSize  from HSRPRecords where HSRPRecordID='" + hiddenfieldHSRPRecordID.Value + "'";
                        //DataTable badt = Utils.GetDataTable(SQLString.ToString(), CnnString);

                        // SQLString = "select InventoryStatus, LaserNo from RTOInventory where LaserNo='" + FrontLaserPlate + "'or LaserNo='" + RearLaserPlate + "'";
                        if (FrontLaserPlate != "")
                        {
                            SQLString = "select InventoryStatus, LaserNo from RTOInventory where hsrp_stateid ='" + HSRPStateID + "' and LaserNo='" + FrontLaserPlate + "' and inventoryStatus='New Order'";
                            DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString);
                            try
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
                                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Selected Inventory Not Uploaded!!');</script>");
                                    //return;
                                    lblErrMess.Text = "Selected Inventory Not Uploaded!!";
                                }


                            }
                            catch
                            {
                                FrontRejectLaserNo = "0";
                            }
                        }
                        if (RearLaserPlate != "")
                        {
                            SQLString = "select InventoryStatus, LaserNo from RTOInventory where hsrp_stateid ='" + HSRPStateID + "' and LaserNo='" + RearLaserPlate + "' and inventoryStatus='New Order'";
                            DataTable dtrear = Utils.GetDataTable(SQLString.ToString(), CnnString);
                            try
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
                                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Selected Inventory Not Uploaded!!');</script>");
                                    //return;
                                    lblErrMess.Text = "Selected Inventory Not Uploaded!!";

                                }
                            }
                            catch
                            {
                                RearRejectLaserNo = "0";
                            }
                        }


                        HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());
                        HSRPRecordID = hiddenfieldHSRPRecordID.Value;
                        string OperatorID = string.Empty;// = dropDownListOperator.SelectedValue;

                        if (FrontRejectLaserNo != "0" || RearRejectLaserNo != "0")
                        {
                            string UID = Session["UID"].ToString();
                            int k;
                            int z;
                            //SQLString = "update HSRPRecords set OrderStatus='Embossing Done',OrderEmbossingDate=Getdate(),hsrp_front_LaserCode='" + FrontLaserPlate + "', hsrp_rear_LaserCode='" + RearLaserPlate + "', LaserPlateBoxNo='" + textBoxLaserPlateBoxNo.Text + "', HSRP_Sticker_LaserCode='" + HSRP_Sticker_LaserCode + "',OperatorID='" + OperatorID + "',EmbossingUserID='" + UID + "'  where HSRPRecordID='" + HSRPRecordID + "'";
                            SQLString = "insert into LaserAssignQuick (VehicleRegNo,HSRP_Front_LaserNo,HSRP_Rear_LaserNo,HSRP_StateID,RtoLocationID,UserID,RecordDate) values ('" + txtVehicleRegNo.Text.Trim() + "','" + FrontLaserPlate + "','" + RearLaserPlate + "','" + HSRPStateID + "','" + RTOLocationID + "','" + UID + "',GetDate())";
                            int i = Utils.ExecNonQuery(SQLString, CnnString);

                           //SQLString = "select max(RecordID) as RecordID from [LaserAssignQuick]";
                            SQLString = "select  recordID from laserAssignquick where hsrp_Front_LaserNo ='" + FrontLaserPlate + "' and hsrp_Rear_LaserNo='" + RearLaserPlate + "'";
                            DataTable LaserAssignQuick = Utils.GetDataTable(SQLString.ToString(), CnnString);

                            if (i > 0)
                            {
                                if (FrontLaserPlate != "0")
                                {

                                    SQLString = "update RTOInventory set InventoryStatus='Embossing Done',HSRP_StateID='" + HSRPStateID + "',LaserAssignQuick='" + LaserAssignQuick.Rows[0]["RecordID"].ToString() + "', EmbossingDate=GETDATE() where hsrp_stateid ='" + HSRPStateID + "' and LaserNo = '" + FrontLaserPlate + "'";
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
                                        SQLString = "update RTOInventory set InventoryStatus='Embossing Done',HSRP_StateID='" + HSRPStateID + "',LaserAssignQuick='" + LaserAssignQuick.Rows[0]["RecordID"].ToString() + "', EmbossingDate=GETDATE() where hsrp_stateid ='" + HSRPStateID + "' and LaserNo= '" + RearLaserPlate + "'";
                                        k = Utils.ExecNonQuery(SQLString, CnnString);
                                    }
                                    else
                                    {
                                        k = 1;
                                    }

                                }

                                lblSucMess.Text = "Record Saved Successfully.";
                                blanckfield();
                            }
                            else
                            {
                                lblErrMess.Text = "Record Not Save!!";
                                blanckfield();
                            }
                        }
                    }
                }
            }
        }
         
     
   public void blanckfield()
  { 
                textBoxFrontPlate.Text = ""; 
                textBoxRearPlate.Text = "";
                lblErrMess.Text = "";
                txtVehicleRegNo.Text = "";
    }


    }
}
