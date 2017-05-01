using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Transaction
{
    public partial class DayWiseProductionAnalysis : System.Web.UI.Page
    {

        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string Stringboxno = string.Empty;
        string Stringlaserfrom = string.Empty;
        string Stringlaserto = string.Empty;
        string Stringquantity = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string sqlQuery12 = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string USERID = string.Empty;
        string strOperatorname = string.Empty;
        string strMachineType = string.Empty;
        string strStartTime = string.Empty;
        string strEndTime = string.Empty;
        string strduration = string.Empty;
        string strTwohundred = string.Empty;
        string strtwofift = string.Empty;
        string strthreeHundred = string.Empty;
        string strfivehundred = string.Empty;
        string strtotal = string.Empty;
        string strReject = string.Empty;
        string strperHours = string.Empty;
        string strdownTime = string.Empty;
        string strReason = string.Empty;
        string strDate = string.Empty;
        string strSheetno = string.Empty;
        string strCentername = string.Empty;
        int UserType1;
    

        protected void Page_Load(object sender, EventArgs e)
        {
           
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType1 = Convert.ToInt32(Session["UserType"]);
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                USERID = Session["UID"].ToString();

            }
            if (!Page.IsPostBack)
            {
                //textBoxUserName.Text = Session["UserName"].ToString();
                FilldropDownListClient();
            
            }

 
        }

        private void FilldropDownListClient()
        {

            SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HSRP_StateID + " and navembid is not null  Order by EmbCenterName";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), ConnectionString, "--Select Embossing Center--");

        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            strOperatorname = Txtoperatorname.Text.Trim();
            strMachineType = txtmachinename.Text.Trim();
            strStartTime = txtstarytime.Text.Trim();
            strEndTime = txtendTime.Text.Trim();
            strduration = txtDuration.Text.Trim();
            strTwohundred = Txthundred.Text.Trim();
            strtwofift = Txttwo.Text.Trim();
            strthreeHundred = txtthree.Text.Trim();
            strfivehundred = Txtfive.Text.Trim();
            strtotal=Txttotal.Text.Trim();
            strReject = Txtreject.Text.Trim();
            strperHours=Txtperhour.Text.Trim();
            strdownTime = TxtDownTime.Text.Trim();
            strReason = TxtReason.Text.Trim();
            strSheetno = Txtsheetno.Text.Trim();
            strCentername = txtCenter.Text.Trim();

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);



            if (dropDownListClient.SelectedItem.Text == "--Select Embossing Center--")
            {
                lblErrMess.Text = "Please Select Embossing Center";
                return;
            }
            if (string.IsNullOrEmpty(strOperatorname))
            {
                lblErrMess.Text = "Please Provide Operator Name.";
                Txtoperatorname.Focus();
                return;
            }
            if (string.IsNullOrEmpty(strMachineType))
            {
                lblErrMess.Text = "Please Provide  Machine Type.";
                txtmachinename.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strStartTime))
            {
                lblErrMess.Text = "Please Provide Start Time";
                txtstarytime.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strEndTime))
            {
                lblErrMess.Text = "Please Provide End Time";
                txtendTime.Focus();
                return;
            }


            if (string.IsNullOrEmpty(strduration))
            {
                lblErrMess.Text = "Please Provide Time Duration";
                txtDuration.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strtotal))
            {
                lblErrMess.Text = "Please Provide Total";
                Txttotal.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strReject))
            {
                lblErrMess.Text = "Please Provide Rejection Plates";
                Txtreject.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strperHours))
            {
                lblErrMess.Text = "Please Provide Per Hours Plates";
                Txtperhour.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strdownTime))
            {
                lblErrMess.Text = "Please Provide Down Time.";
                TxtDownTime.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strReason))
            {
                lblErrMess.Text = "Please Provide Reason.";
                TxtReason.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strSheetno))
            {
                lblErrMess.Text = "Please Provide Sheet No.";
                Txtsheetno.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strCentername))
            {
                lblErrMess.Text = "Please Provide Center Name.";
                txtCenter.Focus();
                return;
            }

            //string SQLStringbox = "select count(BoxNO) from EmbossingCenterLaserQuantity where  BoxNO='" + Stringboxno + "'";
            //int boxcount;
            //boxcount = Utils.getScalarCount(SQLStringbox, CnnString);
            //if (boxcount > 0)
            //{
            //    lblErrMess.Text = "Box Number Already Exists";
            //    return;
            //}

            sqlQuery12 = "insert into DayWiseProductionAnalysis(HSRP_Stateid, RToLocationID, EmbID, OperatorName, MachineType, StartTime, EndTime, Duration, ProductDimesion200, ProductDimesion285, ProductDimesion300, ProductDimesion500, Total, Reject, PerHours, DownTime, Reason, Date, SheetNo, CenterName) values('" + HSRP_StateID + "','" + RTOLocationID + "', '" + dropDownListClient.SelectedValue + "','" + strOperatorname.ToString() + "','" + strMachineType + "','" + strStartTime + "','" + strEndTime + "','" + strduration + "','" + strTwohundred + "','" + strtwofift + "','" + strthreeHundred + "','" + strfivehundred + "','" + strtotal + "','" + strReject + "','" + strperHours + "','" + strdownTime + "','" + strReason + "','" + OrderDatefrom.SelectedDate + "','" + strSheetno + "','" + strCentername + "')";
            int count = Utils.ExecNonQuery(sqlQuery12, ConnectionString);
            if (count > 0)
            {
                lblSucMess.Text = "Records has been Saved Successfully";
            }
            else
            {
                lblErrMess.Text = "Record Not Saved";
            }

        }

       
    }
}