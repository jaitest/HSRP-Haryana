using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace HSRP.Transaction
{
    public partial class HSRPRecordsVehicleClassEdit : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string HSRPStateID = string.Empty;
        DataTable dtgetDetail = new DataTable(); 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString(); 
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;
                lblmsg.Text = string.Empty;
                lblmsg.Visible = false;
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                if (HSRPStateID.ToString().Trim() == "4")
                { Label1.Visible=true;
                  txtChassisNo.Visible=true;
                  lnkModifyVehicle.Visible=true;                                                             
                                        
                }
                else
                {
                  Label6.Visible=true;
                  txtRegNo.Visible=true;
                  lnkModifyVehicle1.Visible=true;
                }

                if (!IsPostBack)
                {                    
                    try
                    {
                        td_Vehicle.Visible = false;
                        lnkModifyVehicle.Visible = false;
                        lnkModifyVehicle1.Visible = false;

                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Visible = true; 
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;

                if (HSRPStateID.ToString().Trim() == "4")
                {

                if (String.IsNullOrEmpty(txtChassisNo.Text))
                {

                    lblErrMsg.Visible = true; 
                    lblErrMsg.Text = "Please Enter Chassis No.";
                    return;
                }
                td_Vehicle.Visible = false;
                lnkModifyVehicle.Visible = false;


                txtOrderType.Text = "";
                ddlVehicleClass.ClearSelection();
                ddlVehicleClass.SelectedIndex = 0;

                dtgetDetail = Utils.GetDataTable("GetVehicleDetails '" + HSRPStateID + "','" + txtChassisNo.Text + "'", CnnString);
                if (dtgetDetail.Rows.Count <= 0)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Chassis No not found";
                    return;
                }
                else if (dtgetDetail.Columns.Count == 1)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = dtgetDetail.Rows[0]["msg"].ToString();
                    return;
                }
                else if (dtgetDetail.Columns.Count > 1)
                {
                    td_Vehicle.Visible = true;
                    lnkModifyVehicle.Visible = true;
                    btnGo.Visible = false;
                    HiddenRecordid.Value = dtgetDetail.Rows[0]["HSRPRecordID"].ToString();
                    txtOrderType.Text = dtgetDetail.Rows[0]["OrderType"].ToString();
                    VehicleClass = dtgetDetail.Rows[0]["VehicleClass"].ToString();
                    VehicleType = dtgetDetail.Rows[0]["VehicleType"].ToString();
                    RTOLocationID = dtgetDetail.Rows[0]["RTOLocationID"].ToString();
                    if (String.IsNullOrEmpty(dtgetDetail.Rows[0]["VehicleClass"].ToString()))
                    {
                        ddlVehicleClass.SelectedIndex = 0;
                    }
                    else
                        ddlVehicleClass.SelectedValue = dtgetDetail.Rows[0]["VehicleClass"].ToString().ToUpper();
                    txtChassisNo.Enabled = false;
                }


                }
                else
                {

                    if (String.IsNullOrEmpty(txtRegNo.Text.Trim()))
                    {

                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "Please Enter Vehicle Reg No.";
                        return;
                    }
                    td_Vehicle.Visible = false;
                    lnkModifyVehicle1.Visible = false;


                    txtOrderType.Text = "";
                    ddlVehicleClass.ClearSelection();
                    ddlVehicleClass.SelectedIndex = 0;

                    dtgetDetail = Utils.GetDataTable(" GetVehicleDetailsVehicleNo '" + HSRPStateID + "','" + txtRegNo.Text + "'", CnnString);
                    if (dtgetDetail.Rows.Count <= 0)
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = " Vehicle Reg No not found";
                        return;
                    }
                    else if (dtgetDetail.Columns.Count == 1)
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = dtgetDetail.Rows[0]["msg"].ToString();
                        return;
                    }
                    else if (dtgetDetail.Columns.Count > 1)
                    {
                        td_Vehicle.Visible = true;
                        lnkModifyVehicle1.Visible = true;
                        btnGo.Visible = false;
                        HiddenRecordid.Value = dtgetDetail.Rows[0]["HSRPRecordID"].ToString();
                        txtOrderType.Text = dtgetDetail.Rows[0]["OrderType"].ToString();
                        VehicleClass = dtgetDetail.Rows[0]["VehicleClass"].ToString();
                        VehicleType = dtgetDetail.Rows[0]["VehicleType"].ToString();
                        RTOLocationID = dtgetDetail.Rows[0]["RTOLocationID"].ToString();
                        if (String.IsNullOrEmpty(dtgetDetail.Rows[0]["VehicleClass"].ToString()))
                        {
                            ddlVehicleClass.SelectedIndex = 0;
                        }
                        else
                            ddlVehicleClass.SelectedValue = dtgetDetail.Rows[0]["VehicleClass"].ToString().ToUpper();
                        txtChassisNo.Enabled = false;
                    }
                }

                

                 
            
            }
            catch(Exception ex)
            {
                throw ex;            
            }
        }

        public static string VehicleClass;
        public static string VehicleType;
        public static string RTOLocationID;

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;
                lblmsg.Text = string.Empty;
                lblmsg.Visible = false;
            
                if (string.IsNullOrEmpty(txtapprovedBy.Text))
                {
                    lblErrMsg.Visible = true; 
                    lblErrMsg.Text = "Please Enter Approved By";
                    return;
                }
               if ((!Regex.Match(txtapprovedBy.Text.ToString().Trim(), @"^[a-zA-Z\s\.]{1,100}$", RegexOptions.None).Success))
               {
                   lblErrMsg.Visible = true; 
                   lblErrMsg.Text = "Please enter vaild Name of Approved by";
                   return;               
               }

                if (string.IsNullOrEmpty(txtRemarks.Text))
                {
                    lblErrMsg.Visible = true; 
                    lblErrMsg.Text = "Please Enter Remarks";
                    return;
                }
                if ((!Regex.Match(txtRemarks.Text.ToString().Trim(), @"^[a-zA-Z0-9\@\!\$\&amp;\^\?\*\/\+\,\-\.\:\;\{\}\|\~\(\)\=\ \`\[\]\\\%]{1,250}$", RegexOptions.None).Success))
                {
                    lblErrMsg.Visible = true; 
                    lblErrMsg.Text = "Please enter vaild Remarks!"; 
                    return;
                }
                

                    if (ddlVehicleClass.SelectedItem.Value.Equals("--Select Vehicle Class--"))
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "please select Correct vehicle Class.";
                        return;
                    }

                    string strUserID = string.Empty;
                    strUserID = Session["UID"].ToString();
                    string RTOLocationID = string.Empty;
                    RTOLocationID = Session["UserRTOLocationID"].ToString();
                    HSRPStateID = Session["UserHSRPStateID"].ToString();
                    DataTable dt4 = new DataTable();
                    dt4 = GetTable(HSRPStateID, VehicleType, ddlVehicleClass.SelectedValue, txtOrderType.Text);
                    int a=0;

                    if (HSRPStateID.ToString().Trim() == "4")
                    {
                        a = Utils.ExecNonQuery("UpdateVehicleClass1 '" + HSRPStateID + "','" + RTOLocationID + "','" + HiddenRecordid.Value.ToString() + "','" + txtChassisNo.Text + "','" + ddlVehicleClass.SelectedValue + "','" + txtapprovedBy.Text + "','" + txtRemarks.Text + "','" + strUserID + "','" + VehicleType + "','" + dt4.Rows[0]["FrontPlateID"].ToString() + "','" + dt4.Rows[0]["RearPlateID"].ToString() + "'", CnnString);
                    }
                    else
                    {
                     a = Utils.ExecNonQuery("UpdateVehicleClass1onvehicleNo '" + HSRPStateID + "','" + RTOLocationID + "','" + HiddenRecordid.Value.ToString() + "','" + txtRegNo.Text + "','" + ddlVehicleClass.SelectedValue + "','" + txtapprovedBy.Text + "','" + txtRemarks.Text + "','" + strUserID + "','" + VehicleType + "','" + dt4.Rows[0]["FrontPlateID"].ToString() + "','" + dt4.Rows[0]["RearPlateID"].ToString() + "'", CnnString);
                    }
                  
                    
                     if (a > 0)
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "Update successfully";
                        return;
                    }
                    else
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "Record not Updated";
                        return;
                    }

              
              
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lnkModifyVehicle_Click(object sender, EventArgs e)
        {
            lnkModifyVehicle.Visible = false;
            btnGo.Visible = true;
            txtChassisNo.Enabled = true;
            td_Vehicle.Visible = false;
            ddlVehicleClass.SelectedIndex = 0;    
        }
        protected void lnkModifyVehicle1_Click(object sender, EventArgs e)
        {
            lnkModifyVehicle1.Visible = false;
            btnGo.Visible = true;
            txtRegNo.Enabled = true;
            td_Vehicle.Visible = false;
            ddlVehicleClass.SelectedIndex = 0;
        }

        public DataTable GetTable(string HSRPStateId, string Vehicletype, string VehicleClass, string TransactionType)
        {
            string sql = "exec [getPlatesData] '" + HSRPStateId + "','" + Vehicletype.ToString() + "','" + VehicleClass.ToString() + "', '" + TransactionType.ToString() + "'";
            DataTable dt = Utils.GetDataTable(sql, CnnString);
            return dt;
        }
    
    }
}