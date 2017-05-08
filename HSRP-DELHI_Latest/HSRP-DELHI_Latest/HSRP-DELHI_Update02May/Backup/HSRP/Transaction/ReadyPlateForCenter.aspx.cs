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
    public partial class ReadyPlateForCenter : System.Web.UI.Page
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
            //AutoCompleteExtender1.ContextKey = Session["UserHSRPStateID"].ToString();
            //AutoCompleteExtender2.ContextKey = Session["UserHSRPStateID"].ToString();

           

            SQLString = "select count(*) from laserAssignquick where userID='" + Session["UID"].ToString() + "' and recordDate between (convert(varchar,GetDate(),111)+' 00:00:00') and (convert(varchar,GetDate(),111)+' 23:59:59')";
            
            int totalrecord = Utils.getScalarCount(SQLString, CnnString);
            if (totalrecord > 0)
            {
                lbltotalSavedRecord.Text = Convert.ToString(totalrecord);
            }

            if (!Page.IsPostBack)
            {
                RTOLocationID =  Session["UserRTOLocationID"].ToString();
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
       int Records;

       lblErrMess.Text = "";
       lblSucMess.Text = "";
       // btnSave.Enabled = false;

       string vehicleRegNo = txtVehicleRegNo.Text.Trim();
       string FrontLaserCode = textBoxFrontPlate.Text.Trim().ToUpper();
       string RearLaserCode = textBoxRearPlate.Text.Trim().ToUpper();
       string Remarks = txtRemarks.Text.Trim().ToUpper();
       if (vehicleRegNo == "")
       {
           lblErrMess.Text = "Please Provide Vehicle No.!!";
       }
       else if (FrontLaserCode == "" || RearLaserCode == "")
       {
           lblErrMess.Text = "Please Provide & Plate No.!!";
       }
       //else
       //{
       //    SQLString = "select count(*) from hsrprecords where hsrp_stateid='" + HSRPStateID + "' and vehicleRegNo ='" + vehicleRegNo + "'";
       //    int Records = Utils.getScalarCount(SQLString, CnnString);
       //    if (Records > 0)
       //    {

       //    }
       else
       {
           SQLString = "select count(*) from EmbossedStockInHand where HSRP_StateID='" + HSRPStateID + "' and vehicleRegNo ='" + vehicleRegNo + "' and HSRP_Front_LaserNo ='" + FrontLaserCode + "'and HSRP_Rear_LaserNo ='" + RearLaserCode + "'";
           Records = Utils.getScalarCount(SQLString, CnnString);
           if (Records == 0)
           {
               SQLString = "insert into EmbossedStockInHand (vehicleRegNo,HSRP_Front_LaserNo,HSRP_Rear_LaserNo,hsrp_StateID,RtolocationID,UserID,Remarks) values ('" + vehicleRegNo + "','" + FrontLaserCode + "','" + RearLaserCode + "','" + HSRPStateID + "','" + RTOLocationID + "','" + UserID + "','" + Remarks + "')";
               int i = Utils.ExecNonQuery(SQLString, CnnString);
               if (i > 0)
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
               lblErrMess.Text = "Embossing Entry Already Exisit.";
           }
       }
   }
   

   

   protected void LinkButton1_Click(object sender, EventArgs e)
   {
       txtVehicleRegNo.Text = "";
       textBoxFrontPlate.Text = "";
       textBoxRearPlate.Text = "";
   }


    }
}
