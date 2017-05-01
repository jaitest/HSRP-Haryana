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
    public partial class HSRPRecordsVehicleLocEdit : System.Web.UI.Page
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

                if (!IsPostBack)
                {
                    try
                    {
                        td_Vehicle.Visible = false;
                        lnkModifyVehicle.Visible = false;                       
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        private void FilldropDownListRTOLocation()
        {
            int UID;
            int.TryParse(Session["UID"].ToString(), out UID);
            DataSet dss = Utils.getDataSet(" [getRTOLocation] '" + UID + "'", CnnString);
            ddlVehicleLoc.DataSource = dss;  
            ddlVehicleLoc.DataTextField="RTOLocationName";
            ddlVehicleLoc.DataValueField ="RTOLocationID";
            ddlVehicleLoc.DataBind();
            ddlVehicleLoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select RTO Location--", "--Select RTO Location--"));
            ddlVehicleLoc.SelectedIndex=0;
        }
       
        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;

                if (String.IsNullOrEmpty(txtVehicleNo.Text))
                {

                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please Enter Vehicle Registration No.";
                    return;
                }

                HSRPStateID = Session["UserHSRPStateID"].ToString();
                td_Vehicle.Visible = false;
                lnkModifyVehicle.Visible = false;

                txtOrderType.Text = "";                

                dtgetDetail = Utils.GetDataTable(" GetVehicleDetails '" + HSRPStateID + "','" + txtVehicleNo.Text + "'", CnnString);
                if (dtgetDetail.Rows.Count <= 0)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Vehicle not found";
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
                    FilldropDownListRTOLocation();
                    HiddenRecordid.Value = dtgetDetail.Rows[0]["HSRPRecordID"].ToString();
                    txtOrderType.Text = dtgetDetail.Rows[0]["OrderType"].ToString();
                    if (String.IsNullOrEmpty(dtgetDetail.Rows[0]["RTOLocationID"].ToString()))
                    {
                        ddlVehicleLoc.SelectedIndex = 0;
                    }
                    else
                        ddlVehicleLoc.SelectedValue = dtgetDetail.Rows[0]["RTOLocationID"].ToString();

                    txtVehicleNo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = string.Empty;
                lblErrMsg.Visible = false;
                lblmsg.Text = string.Empty;
                lblmsg.Visible = false;
               
                if (string.IsNullOrEmpty(txtOrderType.Text))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "No Order Type found";
                    return;
                }
                if (string.IsNullOrEmpty(txtapprovedBy.Text))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please Enter Approved By";
                    return;
                }
                //if ((!Regex.Match(txtapprovedBy.Text.ToString().Trim(), @"^[a-zA-Z\s\.]{1,100}$", RegexOptions.None).Success))
                //{
                //    lblErrMsg.Visible = true;
                //    lblErrMsg.Text = "Please enter vaild Name of Approved by";
                //    return;
                //}

                if (string.IsNullOrEmpty(txtRemarks.Text))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please Enter Remarks";
                    return;
                }
                //if ((!Regex.Match(txtRemarks.Text.ToString().Trim(), @"^[a-zA-Z0-9\@\!\$\&amp;\^\?\*\/\+\,\-\.\:\;\{\}\|\~\(\)\=\ \`\[\]\\\%]{1,250}$", RegexOptions.None).Success))
                //{
                //    lblErrMsg.Visible = true;
                //    lblErrMsg.Text = "Please enter vaild Remarks!";
                //    return;
                //}

                if (ddlVehicleLoc.SelectedItem.Value.Equals("--Select Vehicle Location--"))
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "please select Correct vehicle Class.";
                    return;
                }

                string strUserID = string.Empty;
                strUserID = Session["UID"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                dtgetDetail = Utils.GetDataTable(" UpdateVehicleLocation '" + HSRPStateID + "','" + ddlVehicleLoc.SelectedValue + "','" + HiddenRecordid.Value.ToString() + "','" + txtVehicleNo.Text + "','" + txtapprovedBy.Text + "','" + txtRemarks.Text + "','" + strUserID + "'", CnnString);
                if (dtgetDetail.Rows.Count <= 0)
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Vehicle not found";
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
                    lblmsg.Visible = true;
                    lblmsg.Text = dtgetDetail.Rows[0]["msg"].ToString();
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
            txtVehicleNo.Enabled = true;
            td_Vehicle.Visible = false;
            ddlVehicleLoc.SelectedIndex = 0;
        }
    }
}