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
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using System.IO;
using HSRP.TGWebrefrence;
using System.Net;

namespace HSRP.Master
{
    public partial class LaserQuickClosed_TG : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string HSRPRecordID = string.Empty;
        string Mode;
        string UserID;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string RearPlateSize = string.Empty;
        string FrontPlateSize = string.Empty;
        string mobileno = string.Empty;
        int Isexists = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //  Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                AutoCompleteExtender2.ContextKey = Session["UserHSRPStateID"].ToString();// +"/" + dtPlateSize.Rows[0]["FrontPlateSize"].ToString() + "/" + dtPlateSize.Rows[0]["RearPlateSize"].ToString();
                AutoCompleteExtender1.ContextKey = Session["UserHSRPStateID"].ToString();
                AutoCompleteExtender3.ContextKey = Session["UserHSRPStateID"].ToString();
                UserType = Session["UserType"].ToString();
            }
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            //UserID = Request.QueryString["UserID"];
            UserID = Session["UID"].ToString();
            if (!Page.IsPostBack)
            {

                Mode = Request.QueryString["Mode"];
                UserID = Session["UID"].ToString();
                //textBoxFrontPlate.Text = "";  
                textBoxFrontPlate.Visible = false;
                textBoxRear.Visible = false;

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Exists = string.Empty;
            if (lblFrontLaser.Text == "" || lblRearLaser.Text == "")
            {
                lblErrMess.Text = "Please get laser codes checked with administrator.!!";
            }
            else
            {
                lblErrMess.Text = "";
                int p;
                SQLString = "select hsrprecordid from hsrprecords where HSRP_StateID='" + HSRPStateID + "' and HSRPRecord_AuthorizationNo='" + lblauthno.Text.Trim().ToUpper() + "' and orderstatus ='Embossing Done' and RecievedAffixationStatus='Y'";
                //and orderdate>'1-jan-2015 00:00:00'
                string hsrpid = Utils.getDataSingleValue(SQLString, CnnString, "hsrprecordid");      
                string strCloseFlg = string.Empty;
                if (int.Parse(hsrpid) > 0)
                {
                    UserID = Session["UID"].ToString();
                    string serverdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                    HSRP.TGWebrefrence.HSRPAuthorizationService objTGAssign = new HSRP.TGWebrefrence.HSRPAuthorizationService();
                    strCloseFlg = objTGAssign.UpdateHSRPAffixation(lblauthno.Text, serverdate);                    
                    SQLString = "update hsrprecords set APwebservicerespAffdate=getdate(),APwebservicerespAff='" + strCloseFlg + "',closeduserid = '" + UserID + "',orderstatus='Closed',orderclosedDate=GETDATE() where hsrprecordid ='" + hsrpid + "' ";
                    p = Utils.ExecNonQuery(SQLString, CnnString);
                    lblSucMess.Text = "Record Closed Successfully";
                   // btnDownloadInvoice.Visible = true;
                    blanckfield();
                }
     
            }

        }


        public void blanckfield()
        {

            lblFrontLaser.Text = "";
            lblRearLaser.Text = "";
            lblErrMess.Text = "";
            //txtVehicleRegNo.Text = "";
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {

           DataTable dtRecords1 = new DataTable();
            Label2.Visible = true;
            blanckfield();
           // lblErrMessvehicle.Text = "";
            lblSucMess.Text = "";

            SQLString = "select r.RTOLocationName, h.HSRPRecord_AuthorizationNo,h.HSRP_Front_LaserCode,h.HSRP_Rear_LaserCode from hsrprecords as h inner join rtolocation as r on h.RTOLocationID=r.RTOLocationID where  h.HSRP_STATEID=11 AND  h.VehicleRegno ='" + textBoxVehicleRegNo.Text.Trim().ToUpper() + "'";

            dtRecords1 = Utils.GetDataTable(SQLString.ToString(), CnnString);
            if (dtRecords1.Rows.Count <= 0)
            {
                lblErrMess.Text = "Record Not found...!!";
           
                
                DivSecond.Visible = false;
                DivFirst.Visible = false;
                lblauthno.Text = "";
                return;
            }
            else
            {
                lblrto.Visible = true;
                Label2.Visible = true;

                lblauthno.Text = dtRecords1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                lblrto.Text = dtRecords1.Rows[0]["RTOLocationName"].ToString();

                lblFrontLaser.Text = dtRecords1.Rows[0]["HSRP_Front_LaserCode"].ToString();
                lblRearLaser.Text = dtRecords1.Rows[0]["HSRP_Rear_LaserCode"].ToString();
            }

            SQLString = "select count(VehicleRegNo) as Record from hsrprecords where  HSRP_STATEID='" + HSRPStateID + "' AND  HSRPRecord_AuthorizationNo ='" + lblauthno.Text.Trim().ToUpper() + "' and orderStatus='Closed'";

            DataTable dtRecords = Utils.GetDataTable(SQLString.ToString(), CnnString);
            if (dtRecords.Rows[0]["Record"].ToString() != "0")
            {
                lblSucMess.Text = "Record Already Closed!!";
            
                btnSave.Visible = false;
                DivSecond.Visible = false;
                DivFirst.Visible = false;
                // btnDownloadInvoice.Visible = true;
                return;
            }

            SQLString = "select count(VehicleRegNo) as Record from hsrprecords where  HSRP_STATEID='" + HSRPStateID + "' AND  HSRPRecord_AuthorizationNo ='" + lblauthno.Text.Trim().ToUpper() + "' and orderStatus='New Order'";

            DataTable dtRecords5 = Utils.GetDataTable(SQLString.ToString(), CnnString);
            if (dtRecords5.Rows[0]["Record"].ToString() != "0")
            {
                lblErrMess.Text = "Record not Embossed!!";
               
                btnSave.Visible = false;
                DivSecond.Visible = false;
                DivFirst.Visible = false;
                // btnDownloadInvoice.Visible = true;
                return;
            }
            SQLString = "select count(VehicleRegNo) as Record from hsrprecords where  HSRP_STATEID='" + HSRPStateID + "' AND  HSRPRecord_AuthorizationNo ='" + lblauthno.Text.Trim().ToUpper() + "' and orderStatus='Embossing Done'";

            DataTable dtRecordsemb = Utils.GetDataTable(SQLString.ToString(), CnnString);
            if (dtRecordsemb.Rows[0]["Record"].ToString() != "0")
            {
                //lblSucMess.Text = "Record Embossing Done!!";

                btnSave.Visible = true;
                DivSecond.Visible = false;
                DivFirst.Visible = false;
                // btnDownloadInvoice.Visible = true;
                return;
            }
                
            }

        

    }
}
