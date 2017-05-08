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
    public partial class Holiday : System.Web.UI.Page
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
      
        string StringEmbcode = string.Empty;
        string StringDesignation = string.Empty;
        string Stringdesc = string.Empty;
        string StringTimeout = string.Empty;
        string SQLString = string.Empty;
        string sqlQuery12 = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string USERID = string.Empty;
        string strtimeinto = string.Empty;
        string strtimeoutto = string.Empty; 
        string strempid =string.Empty;
        string Empidd = string.Empty;
        int UserType;
        string status = string.Empty;
        string stateid = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                USERID = Session["UID"].ToString();
            }

            if (!Page.IsPostBack)
            {
                // textBoxUserName.Text = Session["UserName"].ToString();   
                FilldropDownListOrganization();
                
            }
            else
            {
                //FilldropDownListClient();
            }


          
        }


        private void FilldropDownListOrganization()
        {

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();


            }



        }



        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
           // HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
           // HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
           // CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
           // CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }


        protected void buttonSave_Click(object sender, EventArgs e)
        {

            string type = "1";
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00";
           // String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            //string Mon = ("0" + StringOrderDate[0]);
            //string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            //String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            //String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
           // string ToDate = From1 + " 23:59:59";
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            Stringdesc = txtdesc.Text.ToString();
            status = ddlEmbossingCenter.SelectedItem.Text.ToString();
            stateid = DropDownListStateName.SelectedValue;
            try
            {
                sqlQuery12 = "insert into [HolidayDateTime](blockDate,hsrp_stateid,[status],userid,[Desc] ) values ('" + AuthorizationDate + "','" + stateid + "','" + status + "','" + USERID + "','" + Stringdesc + "')";
                int count = Utils.ExecNonQuery(sqlQuery12, CnnString);
                if (count > 0)
                {
                    lblSucMess.Text = "Record Saved.";
                }
                else
                {
                    lblErrMess.Text = "Record Not Saved";
                }
            }
            catch(Exception ex)
            { 
            
            }

       

        }

       

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
        
    }
}
