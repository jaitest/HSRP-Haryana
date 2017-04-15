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

namespace HSRP.Report
{
    public partial class ViewComplainReport : System.Web.UI.Page
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


        string strcomplaindatetime = string.Empty;
        string strname = string.Empty;
        string strmobileNo = string.Empty;
        string stremailid = string.Empty;
        string strRegNo = string.Empty;
        string strEngineNo = string.Empty;
        string strChessisNo = string.Empty;
        string strRemarks = string.Empty;
        string strStatus = string.Empty;
        string strSolution = string.Empty;
       
        
        

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
                


                string usrname1 = Utils.getDataSingleValue("Select UserFirstName + space(2)+UserLastName as UserName From Users where UserID=" + UID.ToString(), CnnString, "UserName");
                LabelCreatedID.Text = usrname1;


                LabelCreatedDateTime.Text = DateTime.Now.ToString("dd /MM /yyyy");
                string  strcomplaindID = Request.QueryString["strComplaintID"];

                string strQuery = "SELECT [id],[ComplaintNo],[Region],[StateId],[OwnerName],[MobileNo],[Email],[Regno],[EngineNo],[ChasisNo],[Remarks],[IPAddress],[ComplaintDate],[Status],[Solution],[SolutionDate] FROM [Complaint] where [id]='" + strcomplaindID + "'";
                DataTable dtresult = Utils.GetDataTable(strQuery, CnnString);
                strcomplaindatetime = dtresult.Rows[0]["ComplaintDate"].ToString();
                strname = dtresult.Rows[0]["OwnerName"].ToString();
                strmobileNo = dtresult.Rows[0]["MobileNo"].ToString();
                stremailid = dtresult.Rows[0]["Email"].ToString();
                strRegNo = dtresult.Rows[0]["Regno"].ToString();
                strEngineNo = dtresult.Rows[0]["EngineNo"].ToString();
                strChessisNo = dtresult.Rows[0]["ChasisNo"].ToString();
                strRemarks = dtresult.Rows[0]["Remarks"].ToString();
                strStatus = dtresult.Rows[0]["Status"].ToString();
                strSolution = dtresult.Rows[0]["Solution"].ToString();

                lblCompalaintDate.Text = strcomplaindatetime;
                lblName.Text = strname;
                lblMobileNo.Text = strmobileNo;
                lblEmail.Text = stremailid;
                lblRegNo.Text = strRegNo;
                lblEngineNo.Text = strEngineNo;
                lblChaissNo.Text = strChessisNo;
                lblRemarks.Text = strRemarks;
                lblStatus.Text = strStatus;
                lblresolution.Text = strSolution;

               
                
            }
           
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            SQLString = "update Complaint set ";
            int result=Utils.ExecNonQuery(SQLString, CnnString);
            if (result> 0)
            {
                llbMSGSuccess.Visible = true;
                llbMSGSuccess.Text = "";
                llbMSGSuccess.Text = "Record Save Successfully...";
            }

        }

        protected void btnprint_Click(object sender, EventArgs e)
        {
            //btnprint.Attributes.Add("onclick", "return printing()");
            ClientScript.RegisterStartupScript(this.GetType(), "PrintOperation", "printing()", true);
        }


    }

      
         
    }
