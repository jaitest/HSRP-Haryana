using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
namespace HSRP.Transaction
{
    public partial class AddSubmitRequest : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intOrgID;
        int intClientID;
        int UID;
        string SaveLocation = string.Empty;
        string strQuery = string.Empty;
        DataTable dtresult = new DataTable();
        StringBuilder SBSQL = new StringBuilder();
        string strRequestID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                int.TryParse(Session["UserType"].ToString(), out UserType);
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UID);

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                //strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                string usrname1 = Utils.getDataSingleValue("Select UserFirstName + space(2)+UserLastName as UserName From Users where UserID=" + UID.ToString(), CnnString, "UserName");
                LabelCreatedID.Text = usrname1;
                LabelCreatedDateTime.Text = DateTime.Now.ToString("dd MMM yyyy");

                string strMenuID = Session["M"].ToString();

                switch (strMenuID)
                {
                        case "283" :
                        lblRequest.Text = "Hardware Request";
                        lblRequest.Enabled = false;
                        strRequestID = "1";
                        break;

                        case "284" :
                        lblRequest.Text = "Software Request";
                        lblRequest.Enabled = false;
                        strRequestID = "2";
                        break;

                        case "285" :
                        lblRequest.Text = "Message to management";
                        lblRequest.Enabled = false;
                        strRequestID = "3";
                        break;

                        case "286" :
                        lblRequest.Text = "Complaint";
                        lblRequest.Enabled = false;
                        strRequestID = "4";
                        break;
                        
                }
                if (!IsPostBack)
                {
                    FilldropDownState();
                    FilldropDownRTOLocation();
                }
            }
        }

        #region DropDown

        private void FilldropDownState()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListOrg, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                dropDownListOrg.DataSource = dts;
                dropDownListOrg.DataBind();
            } 
        } 
        private void FilldropDownRTOLocation()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(dropDownListOrg.SelectedValue, out intOrgID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intOrgID + " and ActiveStatus='Y' and LocationType='Sub-Urban' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO--");
            }
            else
            {
                 
                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + RTOLocationID + " and ActiveStatus='Y' and LocationType='Sub-Urban' Order by RTOLocationName";
                SQLString = "SELECT distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + UID + "' ";
                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();

                //string RTOCode = string.Empty;
                //SQLString = "SELECT distinct (a.RTOLocationCode+' , ') as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + UID + "' ";
                //DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                //if (dts.Rows.Count > 0)
                //{
                //    for (int i = 0; i <= dts.Rows.Count - 1; i++)
                //    {
                //        RTOCode += dts.Rows[i]["RTOLocationName"].ToString();
                //    }
                    
                //    lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));
                //}
            }
        }


        #endregion

        
        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListOrg.SelectedItem.Text != "--Select State--")
            {
                dropDownListClient.Visible = true;
                FilldropDownRTOLocation();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string filename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
             Boolean fileOK = false;
            if (FileUpload1.HasFile)
            {
                  String fileExtension =    System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
                    String[] allowedExtensions =    {".csv",".xls", ".xlsx", ".doc", ".pdf",".gif", ".png", ".jpeg", ".jpg"};
              for (int i = 0; i < allowedExtensions.Length; i++)
              {
                   if (fileExtension == allowedExtensions[i])
                   {
                        fileOK = true;
                   }
              }

              if (fileOK)
              {
                  try
                  {
                      string SaveLocation = ConfigurationManager.AppSettings["RequestFolder"].ToString() + filename;
                      FileUpload1.PostedFile.SaveAs(SaveLocation);
                  }
                  catch (Exception ex)
                  {
                      llbMSGError.Text = "File could not be uploaded.";
                  }
              }

              }



            string sqlSaveResult = "INSERT INTO SubmitRequest (UserID,RequestedBy,RequestType,UploadedFileName,RequestPriority,Remarks,UserHSRPStateID,UserRTOLocationID) VALUES ('" + UID + "','" + txtRequestName.Text + "','" + strRequestID + "','" + filename.ToString() + "','" + DDLPriority.SelectedValue + "','" + textboxRemarks.Text + "','" + HSRPStateID + "','" + RTOLocationID + "')";
            Utils.ExecNonQuery(sqlSaveResult, CnnString);
            llbMSGSuccess.Text = "Record Saved Successfully";
            ClearTextBox();
        }

        private void ClearTextBox()
        {
            txtRequestName.Text = "";
            
            DDLPriority.SelectedValue = "--Select Priority--";
            textboxRemarks.Text = "";
        }

        


    }
}