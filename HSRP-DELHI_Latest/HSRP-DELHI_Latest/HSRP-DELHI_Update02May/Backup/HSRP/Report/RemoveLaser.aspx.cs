using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using System.Globalization;


namespace HSRP.Report
{
    public partial class RemoveLaser : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname,pp;
        string transtr, statename1;
        BaseFont basefont;
        string fontpath;
        string  strComplaintID =string.Empty;
        string strSql = string.Empty;

        string AllLocation = string.Empty;
         string OrderStatus = string.Empty;
         DateTime AuthorizationDate;
         DateTime OrderDate1;
         DataProvider.BAL bl = new DataProvider.BAL();
         string StickerManditory = string.Empty;

         string SubmitId = string.Empty;
         string QrySubmitID = string.Empty;

         string State_ID = string.Empty;
         string RTO_ID = string.Empty;
         string HSRPStateIDEdit = string.Empty;
         string RTOLocationIDEdit = string.Empty;
         string fromdate = string.Empty;
         string ToDate = string.Empty;
         string strSqlGo = string.Empty;
         string strsqlgonew = string.Empty;
         DataTable dtcount = new DataTable();
         DataTable dtshow = new DataTable();

         
         DataTable dt = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
           
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (Session["UserHSRPStateID"].ToString() == "6")
                {
                   // ButImpData.Visible = true;
                }
                else
                {
                   // ButImpData.Visible = false;
                }

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
               
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                 
                if (!IsPostBack)
                {
                      InitialSetting();

                     
                    try
                    { 
                        if (UserType == "0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            FilldropDownListOrganization();
                           
                        }
                        else
                        { 
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
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

     



        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
           
        }

       

        protected void Update_Click(object sender, EventArgs e)
        {
            lblSucessMsg.Text = "";
            lblErrMsg.Text = "";
            try
            {

                string strHsrp_front_lasercode = string.Empty;
                string strHsrp_rear_lasercode = string.Empty;
                string strRemovelDateTime = string.Empty;

                State_ID = DropDownListStateName.SelectedValue.ToString();
                if (DropDownListStateName.Text == ("--Select State--"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select State..');", true);

                }
                else
                {
                    if (txtHSRPRecordID.Text == "")
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Text Box can not be null');", true);
                    }
                    else
                    {
                        if (ddllasercode.SelectedItem.Text == "Both")
                        {
                            lblSucessMsg.Text = "";
                            lblErrMsg.Text = "";
                            string UID = Session["UID"].ToString();

                            string[] TextBoxValue = txtHSRPRecordID.Text.Split(new Char[] { '\n', '\r','|' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < TextBoxValue.Length; j++)
                            {
                                SQLString = "select hsrp_front_lasercode,hsrp_rear_lasercode from hsrprecords where HSRPRecordID='" + TextBoxValue[j] + "'";
                                dt = Utils.GetDataTable(SQLString,CnnString);

                                strHsrp_front_lasercode = dt.Rows[0]["hsrp_front_lasercode"].ToString();
                                strHsrp_rear_lasercode = dt.Rows[0]["hsrp_rear_lasercode"].ToString();
                                strRemovelDateTime = System.DateTime.Now.ToString();

                                SQLString = "insert into HSRP_Laser_Removal(HSRPRecordID,HSRP_Front_LaserCode,HSRP_Rear_LaserCode,UserID,RemovalDatetime,Remarks)values('" + TextBoxValue[j] + "','" + strHsrp_front_lasercode + "','" + strHsrp_rear_lasercode + "','" + UID + "','" + strRemovelDateTime + "','" + txtRemarks.Text + "')";
                                Utils.ExecNonQuery(SQLString,CnnString);


                                SQLString = "update hsrprecords set HSRP_Front_LaserCode=null,HSRP_Rear_LaserCode=null, internal_remarks='" + UID + "' where HSRP_StateID='" + State_ID + "' and HSRPRecordID='" + TextBoxValue[j] + "'";
                                Utils.ExecNonQuery(SQLString, CnnString);
                                lblSucessMsg.Text = "Record Update Sucessfully";
                            }
                        }
                        if (ddllasercode.SelectedItem.Text == "Front")
                        {
                            lblSucessMsg.Text = "";
                            lblErrMsg.Text = "";
                            string UID = Session["UID"].ToString();

                           // string[] TextBoxValue = txtHSRPRecordID.Text.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            string [] TextBoxValue = txtHSRPRecordID.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < TextBoxValue.Length; j++)
                            {
                                SQLString = "select hsrp_front_lasercode,hsrp_rear_lasercode from hsrprecords where HSRPRecordID='" + TextBoxValue[j] + "'";
                                dt = Utils.GetDataTable(SQLString, CnnString);

                                strHsrp_front_lasercode = dt.Rows[0]["hsrp_front_lasercode"].ToString();
                                strHsrp_rear_lasercode = dt.Rows[0]["hsrp_rear_lasercode"].ToString();
                                strRemovelDateTime = System.DateTime.Now.ToString();

                                SQLString = "insert into HSRP_Laser_Removal(HSRPRecordID,HSRP_Front_LaserCode,UserID,RemovalDatetime,Remarks)values('" + TextBoxValue[j] + "','" + strHsrp_front_lasercode + "','" + UID + "','" + strRemovelDateTime + "','" + txtRemarks.Text + "')";
                                Utils.ExecNonQuery(SQLString, CnnString);

                                SQLString = "update hsrprecords set HSRP_Front_LaserCode=null, internal_remarks='" + UID + "' where HSRP_StateID='" + State_ID + "' and HSRPRecordID='" + TextBoxValue[j] + "'";
                                Utils.ExecNonQuery(SQLString, CnnString);
                                lblSucessMsg.Text = "Record Update Sucessfully";
                            }
                        }

                        if(ddllasercode.SelectedItem.Text=="Rear")
                        {
                            lblSucessMsg.Text = "";
                            lblErrMsg.Text = "";
                            string UID = Session["UID"].ToString();

                            string[] TextBoxValue = txtHSRPRecordID.Text.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < TextBoxValue.Length; j++)
                            {
                                SQLString = "select hsrp_front_lasercode,hsrp_rear_lasercode from hsrprecords where HSRPRecordID='" + TextBoxValue[j] + "'";
                                dt = Utils.GetDataTable(SQLString, CnnString);

                                strHsrp_front_lasercode = dt.Rows[0]["hsrp_front_lasercode"].ToString();
                                strHsrp_rear_lasercode = dt.Rows[0]["hsrp_rear_lasercode"].ToString();
                                strRemovelDateTime = System.DateTime.Now.ToString();

                                SQLString = "insert into HSRP_Laser_Removal(HSRPRecordID,HSRP_Rear_LaserCode,UserID,RemovalDatetime,Remarks)values('" + TextBoxValue[j] + "','" + strHsrp_rear_lasercode + "','" + UID + "','" + strRemovelDateTime + "','" + txtRemarks.Text + "')";
                                Utils.ExecNonQuery(SQLString, CnnString);

                                SQLString = "update hsrprecords set HSRP_Rear_LaserCode=null, internal_remarks='" + UID + "' where HSRP_StateID='" + State_ID + "' and HSRPRecordID='" + TextBoxValue[j] + "'";
                                Utils.ExecNonQuery(SQLString, CnnString);
                                lblSucessMsg.Text = "Laser Code Removed Sucessfully";
                            }
                        }

                        else
                        {
                          //  lblErrMsg.Text = "laser Code Not Removed";
                        
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Record Not Updated');", true);
            }
        }

        protected void ddllasercode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }






       

       
    }
}