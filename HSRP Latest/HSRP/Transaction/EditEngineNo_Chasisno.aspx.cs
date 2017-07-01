using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System;
using System.Web;

using System.Collections.Generic;
using System.Linq;

using System.Net;
namespace HSRP.Transaction
{
    public partial class EditEngineNo_Chasisno : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
             
        string sendURL = string.Empty;
        string SMSText = string.Empty;
        string SqlQuery = string.Empty;
       


        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;


      
        string vehicle = string.Empty;
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {


                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {

                    try
                    {

                        if (UserType == "0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                          
                            FilldropDownListOrganization();
                           
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListOrganization();
                           


                           
                        }

                       
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown



        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }
       

        #endregion




        private void ShowGrid()
        {
            try
            {              
             string SQLString = ""; 
              
            if (RadioButtonEngineNoSearch.Checked == true)
            {
               SQLString = "select top 20  hsrprecordid ,CONVERT(varchar, HSRPRecords.hsrprecord_creationdate,105)  as OrderDate,chassisNo,EngineNo, VehicleRegNo, [OwnerName] , NetAmount,VehicleType,vehicleclass ,OrderStatus from hsrprecords where  OrderStatus= 'New Order' and hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and EngineNo = '" + txtserchtext.Text.ToString().Trim() + "' order by HSRPRecord_CreationDate desc";
                
            }

            if (RadioBtnchassisnosearch.Checked == true)
            {

              SQLString = "select top 20  hsrprecordid ,CONVERT(varchar, HSRPRecords.hsrprecord_creationdate,105)  as OrderDate,chassisNo,EngineNo, VehicleRegNo, [OwnerName] , NetAmount,VehicleType,vehicleclass ,OrderStatus from hsrprecords where OrderStatus= 'New Order' and  hsrp_StateID='" + DropDownListStateName.SelectedValue + "'  and chassisno = '" + txtserchtext.Text.ToString().Trim()+ "' order by HSRPRecord_CreationDate desc";
                               
            }           

            dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                gvhsrpedit.DataSource = dt;
                gvhsrpedit.DataBind();
            }
            else
            {

                lblErrMsg.Text = "Record Not Found";
                gvhsrpedit.DataSource = null;
                gvhsrpedit.DataBind();

            }
           }

                catch (Exception ex)
                {
                    lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
                }


        }
        StringBuilder sb = new StringBuilder();     

        protected void btnGO_Click(object sender, EventArgs e)
        {
           
            ShowGrid();

        }
        DataTable dt = new DataTable();
        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
            // dt = Utils.GetDataTable(SQLString, CnnString);
            ShowGrid();
        }

        protected void gvhsrpedit_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvhsrpedit_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvhsrpedit.EditIndex = e.NewEditIndex;
            this.ShowGrid();
        }

        protected void gvhsrpedit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvhsrpedit.EditIndex = -1;
            this.ShowGrid();
        }

        protected void gvhsrpedit_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            Label lblhsrprecordid = (Label)gvhsrpedit.Rows[e.RowIndex].FindControl("lblEdithsrprecordid");

            string query = "  select chassisno,engineno, OwnerName , vehicleregno  from hsrprecords where hsrprecordid ='" + lblhsrprecordid.Text + "'";
           
            DataTable dt = Utils.GetDataTable(query,CnnString);            
            string Preengineno = dt.Rows[0]["engineno"].ToString();
            string prechassisno = dt.Rows[0]["chassisno"].ToString();
            string txt_OwnerName = dt.Rows[0]["OwnerName"].ToString();
            string txt_vehicleregno = dt.Rows[0]["vehicleregno"].ToString(); 

            StringBuilder sb = new StringBuilder();
            sb.Append("insert into HSRPRecordChangeHistory(HSRPRecordId,chassisno,Engineno, UpdateField,UserId ,RTOLocationId, OwnerName, vehicleregno )  values('" + lblhsrprecordid.Text + "', '" + prechassisno + "', '" + Preengineno + "' ,'chassisno/Engineno' ,'" + strUserID.ToString() + "','" + RTOLocationID + "', '" + txt_OwnerName + "' , '" + txt_vehicleregno+ "') ");

            TextBox txtChassisNo = (TextBox)gvhsrpedit.Rows[e.RowIndex].FindControl("txtChassisNo");
            TextBox txtEngineNo = (TextBox)gvhsrpedit.Rows[e.RowIndex].FindControl("txtEngineNo");
            TextBox txtOwnerName = (TextBox)gvhsrpedit.Rows[e.RowIndex].FindControl("txtOwnerName");
            TextBox txtVehicleRegNo = (TextBox)gvhsrpedit.Rows[e.RowIndex].FindControl("txtVehicleRegNo");

            SqlQuery = "  update  hsrprecords  set vehicleregno = '" + txtVehicleRegNo.Text.ToString()+ "' , OwnerName = '" + txtOwnerName.Text.Trim().ToString() + "' , chassisNo ='" + txtChassisNo.Text.Trim() + "',EngineNo ='" + txtEngineNo.Text.Trim() + "' where hsrprecordid='" + lblhsrprecordid.Text + "'";
            
             int i = Utils.ExecNonQuery(SqlQuery, CnnString);

             if (i > 0)
             {
                Utils.ExecNonQuery(sb.ToString(), CnnString);
                lblErrMsg.Text = "";
                 LblMessage.Text = "Record Update Successfully.";
                 gvhsrpedit.EditIndex = -1;
                 this.ShowGrid();
              
                return; 
               
             }
            else
            {
                LblMessage.Text = "";
                lblErrMsg.Text = "Record not Update";
            }
             gvhsrpedit.EditIndex = -1;
             this.ShowGrid();
        }

      
    }
}





