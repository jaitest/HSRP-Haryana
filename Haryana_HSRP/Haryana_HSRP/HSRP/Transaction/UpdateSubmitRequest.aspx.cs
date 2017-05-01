using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using HSRP;
using DataProvider;

namespace HSRP.Master
{
    public partial class UpdateSubmitRequest : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        int RTOLocationID;
        string Mode;
        string UserID;
        string SubmitID = string.Empty;
        DataTable dt = new DataTable();
        int UID;
        string HSRP_StateID = string.Empty;

        string RtoID = string.Empty;
        string Stateid = string.Empty;
        string Rtoid = string.Empty;
        DataTable dtsname = new DataTable();
        DataTable dtState = new DataTable();
        DataTable dtrname = new DataTable();
        DataTable dtRto = new DataTable();
        
        

        protected void Page_Load(object sender, EventArgs e)
        {
            
            
           
          //  lblState.Text = SubmitID;
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
                int.TryParse(Session["UID"].ToString(), out UID);
               
                
            }

            if (!Page.IsPostBack)
            {
                SubmitID = Request.QueryString["QrySubmitID"].ToString();
                //HSRP_StateID = Request.QueryString["HSRPStateIDEdit"].ToString();
                //RtoID = Request.QueryString["RTOLocationIDEdit"].ToString();
                HSRP_StateID = "select UserHSRPStateID from SubmitRequest where SubmitID='" + SubmitID + "'";
                dtsname = Utils.GetDataTable(HSRP_StateID, CnnString);
                Stateid = dtsname.Rows[0]["UserHSRPStateID"].ToString();

                string StateName = "Select HSRPStateName from HSRPState where HSRP_StateID='" + Stateid + "' ";
                dtState = Utils.GetDataTable(StateName, CnnString);
                string S_name = dtState.Rows[0]["HSRPStateName"].ToString();





                RtoID = "select UserRTOLocationID from SubmitRequest where SubmitID='" + SubmitID + "'";
                dtrname = Utils.GetDataTable(RtoID, CnnString);
                Rtoid = dtrname.Rows[0]["UserRTOLocationID"].ToString();

                string RtoName = "Select RTOLocationName from RTOLocation where RTOLocationID='" + Rtoid + "' ";
                dtRto = Utils.GetDataTable(RtoName, CnnString);
                string R_name = dtRto.Rows[0]["RTOLocationName"].ToString();


                SQLString = "select * from SubmitRequest where SubmitID='" + SubmitID + "'";
                dt = Utils.GetDataTable(SQLString, CnnString);
                //string StateName1 = dtState.Rows[0]["HSRPStateName"].ToString();


                string usrname1 = Utils.getDataSingleValue("Select UserFirstName + space(2)+UserLastName as UserName From Users where UserID=" + UID.ToString(), CnnString, "UserName");
                LabelCreatedID.Text = usrname1;


                LabelCreatedDateTime.Text = DateTime.Now.ToString("dd /MM /yyyy");




                lblState.Text = S_name;

                labRequestName.Text = dt.Rows[0]["RequestedBy"].ToString();
                lblRequestType.Text = dt.Rows[0]["RequestType"].ToString();
                lblUploadFile.Text = dt.Rows[0]["UploadedFileName"].ToString();
                lblPriority.Text = dt.Rows[0]["RequestPriority"].ToString();
                lblRemark.Text = dt.Rows[0]["Remarks"].ToString();
               
                lblSelectLocation.Text = R_name;
                
            }
        }





        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            llbMSGSuccess.Text = "";
            llbMSGError.Text = "";
            RTOLocationID =Convert.ToInt16(Request.QueryString["RTOID"]);

            BAL obj = new BAL();

            //obj.Update_Master_HSRP_RTOLocation(RTOLocationID, StateID, LocationType, textboxRtoLocationName.Text, textboxShippingAddress.Text, textboxBillingAddress.Text, textboxRTOLocationCode.Text, textboxRTOLocationAddress.Text, textboxContactPersonName.Text, textboxMobileNo.Text, textboxLandlineNo.Text, textboxEmailId.Text, ActiveStatus, EmbossingStation, ref IsExists);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SubmitID = Request.QueryString["QrySubmitID"].ToString();

            HSRP_StateID = "select UserHSRPStateID from SubmitRequest where SubmitID='" + SubmitID + "'";
            dtsname = Utils.GetDataTable(HSRP_StateID, CnnString);
            Stateid = dtsname.Rows[0]["UserHSRPStateID"].ToString();



            RtoID = "select UserRTOLocationID from SubmitRequest where SubmitID='" + SubmitID + "'";
            dtrname = Utils.GetDataTable(RtoID, CnnString);
            Rtoid = dtrname.Rows[0]["UserRTOLocationID"].ToString();

           
            LabelCreatedDateTime.Text = DateTime.Now.ToString("dd MMM yyyy");
            string RecordDateTime = DateTime.Now.ToString("dd MMM yyyy");
            if (dropDownListorderStatus.Text == "-- Select Status --" || txtRemarks.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select DropDown And Remarks');", true);
            }
            else
            {
                SQLString = "insert into SubmitRequestDetail ( RequestID, UserID,UserHSRPStateID,UserRTOLocationID,Remarks,RecordDateTime,REQUESTSTATUS) values ('" + SubmitID + "', '" + Session["UID"] + "', '" + Stateid + "', '" + Rtoid + "', '" + txtRemarks.Text + "','" + RecordDateTime + "','" + dropDownListorderStatus.SelectedItem + "')";
                Utils.ExecNonQuery(SQLString, CnnString);
                llbMSGSuccess.Text = "Record Save Successfully";
            }
        }


    }

      
         
    }
