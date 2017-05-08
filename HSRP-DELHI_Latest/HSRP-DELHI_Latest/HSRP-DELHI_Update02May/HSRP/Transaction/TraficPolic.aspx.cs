using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using DataProvider;
using System.Data.SqlClient;

namespace HSRP.Transaction
{
    public partial class TraficPolic : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "AddNewPop();", true);
        //    return;
        //}

        int UID;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;

        protected void Page_Load(object sender, EventArgs e)
        {

            // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "AddNewPop();", true);
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        FilldropDownListHSRPState();
                        dropDownListHSRPState.Visible = false;
                        labelHSRPState.Visible = false; 
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        private void Fill()
        {
            string txtVehicleRegNo = txtSearchAll.Text.Trim();
            DataTable datatraficrecords = new DataTable();
            BAL obj = new BAL();
            if (obj.TrVehicleRecord(txtVehicleRegNo, HSRPStateID, ref datatraficrecords))
            {
                if (datatraficrecords.Rows.Count > 0)
                {
                    string orderStatus = datatraficrecords.Rows[0]["orderStatus"].ToString();
                    string sno = datatraficrecords.Rows[0]["sno"].ToString();

                    String OrderStatus = String.Empty;
                    String Rtocode = String.Empty;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='99%' border='0' align='center'   cellpadding='0' cellspacing='0'>");
                    sb.Append("<tr style='background-color:#acab3d'>");
                    sb.Append("<td valign='top' class='midtablebg'>");
                    sb.Append("<table width='100%' border='0' align='center' cellpadding='7' cellspacing='0'>");
                    sb.Append("<tr align='center'>");
                    sb.Append("<td   height='19' valign='top' nowrap='nowrap' class='midboxtop'>SNo.</td>");
                    sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>Vehicle Registration No.</td>");
                    sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>Status</font></td>");
                    sb.Append("</tr>");


                    sb.Append("<tr style='background-color:#ffffff'>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + sno + "</td>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + txtVehicleRegNo + "</td>");
                    sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + orderStatus + "</td>");
                    sb.Append("</tr>");

                    sb.Append("</table></td></tr></table>");
                    vehshow.InnerHtml = sb.ToString();
                }
                else
                {
                    vehshow.InnerHtml = "";
                    lblErrMsg.Text = "Record Searching....";
                }
            }




            ////Utils dbLink = new Utils();
            ////dbLink.strProvider = CnnString;
            ////dbLink.CommandTimeOut = 0;
            ////dbLink.sqlText = SQLString.ToString();
            ////SqlDataReader PReader = dbLink.GetReader();


            //String SrNo = String.Empty;
            //String RTOLocationName = String.Empty;
            //String RTOLocationAddress = String.Empty;
            //String ContactPersonName = String.Empty;
            //String MobileNo = String.Empty;
            //String LandlineNo = String.Empty;
            //String EmailID = String.Empty;
            //String Rtocode = String.Empty;
            //StringBuilder sb = new StringBuilder();
            //sb.Append("<table width='99%' border='0' align='center' cellpadding='0' cellspacing='0'>");
            //sb.Append("<tr style='background-color:#acab3d'>");
            //sb.Append("<td valign='top' class='midtablebg'>");
            //sb.Append("<table width='100%' border='0' align='center' cellpadding='7' cellspacing='0'>");
            //sb.Append("<tr align='center'>");
            //sb.Append("<td   height='19' valign='top' nowrap='nowrap' class='midboxtop'>RTO Code</td>");
            //sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>Contact Person Name</td>");
            //sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>RTO Location Name</font></td>");
            //sb.Append("<td  valign='top' align='center' width='400px' valign='top' class='midboxtop'>RTO Location Address</td>");
            //sb.Append("<td  valign='top' nowrap='nowrap'  class='midboxtop'>Mobile No</td>");
            //sb.Append("<td  valign='top' nowrap='nowrap' class='midboxtop'>Email ID</td>");
            //sb.Append("</tr>");


            //string bgcol = string.Empty;


            //if (PReader.HasRows)
            //{
            //    while (PReader.Read())
            //    {
            //        SrNo = String.Empty;
            //        RTOLocationName = String.Empty;
            //        RTOLocationAddress = String.Empty;
            //        ContactPersonName = String.Empty;
            //        MobileNo = String.Empty;
            //        EmailID = String.Empty;
            //        Rtocode = String.Empty;
            //        SrNo = PReader["Rtocode"].ToString();
            //        RTOLocationName = PReader["RTOLocationName"].ToString();
            //        RTOLocationAddress = PReader["RTOLocationAddress"].ToString();
            //        ContactPersonName = PReader["ContactPersonName"].ToString();
            //        MobileNo = PReader["MobileNo"].ToString();
            //        LandlineNo = PReader["LandlineNo"].ToString();
            //        EmailID = PReader["EmailID"].ToString();
            //        Rtocode = "HR" + PReader["Rtocode"].ToString();
            //        if (int.Parse(SrNo) % 2 == 0)
            //        {
            //            bgcol = "#ffffff";
            //        }
            //        else
            //        {
            //            bgcol = "#ffffff";
            //        }

            //        sb.Append("<tr style='background-color:#ffffff'>");
            //        sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + Rtocode + "</td>");
            //        sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + ContactPersonName + "</td>");
            //        sb.Append("<td width='150' align='center' class='heading1'>" + RTOLocationName + "</td>");
            //        sb.Append("<td align='center' class='heading1'>" + RTOLocationAddress + "</td>");
            //        sb.Append("<td width='150' align='center' class='heading1'>" + MobileNo + "</td>");
            //        sb.Append("<td nowrap='nowrap' align='center' class='heading1'>" + EmailID + "</td>");
            //        sb.Append("</tr>");
            //    }

            //    sb.Append("</table></td></tr></table>");
            //    vehshow.InnerHtml = sb.ToString();
            //}
            //else
            //{
            //    //LabelError.Text = string.Empty;
            //    //LabelError.Text = "Their is no selected data for the selected  date range.";
            //    //vehshow.InnerHtml = string.Empty;
            //    //UpdatePanelError.Update();
            //    //UpdatePanelDiv.Update();
            //    return;
            //}
        }

        //protected void ButtonGo_Click(object sender, EventArgs e)
        //{

        //}

        #region DropDown

        private void FilldropDownListHSRPState()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListHSRPState, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                dropDownListHSRPState.DataSource = dts;
                dropDownListHSRPState.DataBind();
            }
        } 
        #endregion

        protected void button_Click(object sender, EventArgs e)
        { 
            lblErrMsg.Text = "";
            Fill();
        }
         
    }
}