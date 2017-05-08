using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;

namespace HSRP.DLReports
{
    public partial class AgingReport : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SQLString1 = string.Empty;
        string SQLString2 = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRPStateID = string.Empty;
        // int RTOLocationID;
        int intHSRPStateID;
        // int intRTOLocationID;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
        string FromDate, ToDate;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataProvider.BAL bl = new DataProvider.BAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                HSRPStateID = Session["UserHSRPStateID"].ToString();

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        string State_ID = DropDownListStateName.SelectedValue.ToString();
                        InitialSetting();
                        if (UserType.Equals(0))
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            FilldropDownZone();
                            labelClient.Visible = false;
                            dropDownListClient.Visible = false;


                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            FilldropDownZone();
                            labelClient.Visible = false;
                            dropDownListClient.Visible = false;
                        }
                    }
                    catch (Exception err)
                    {
                        err.ToString();
                    }
                }
            }
        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }

    
       

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='" + HSRPStateID + "' and ActiveStatus='Y' Order by HSRPStateName";
                //Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID='" + HSRPStateID + "' and ActiveStatus='Y' Order by HSRPStateName";
               // Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
            
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
          
        }

        private void FilldropDownListClient()
        {
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "All");
                       
        }


        private void FilldropDownZone()
        {
            SQLString = "select distinct zone from RTOLocation where hsrp_stateid='" + HSRPStateID + "' and zone is not null order by zone";
            Utils.PopulateDropDownList(ddl_zone, SQLString.ToString(), CnnString, "--Please Select Zone--");

        }

        protected void ddl_zone_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddl_zone.SelectedItem.Text=="All")
            {
                labelClient.Visible = false;
                dropDownListClient.Visible = false;
                FilldropDownListClient();
            }
            else
            { 
            //Getlocation();
            labelClient.Visible = true;
            dropDownListClient.Visible = true;
            FilldropDownListClient();
            }
        }

        //public void Getlocation()
        //{
        //    SQLString = "select rtolocationid,rtolocationname from rtolocation where HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and zone='" + ddl_zone.SelectedValue + "'";
        //    Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
        //}
       

        #endregion

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (DropDownListStateName.Text == ("--Select State--"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Please Select State..');", true);
                    return;

                }
                if (dropDownListClient.SelectedItem.Text == "All")
                {
                    string fromdate = OrderDate.SelectedDate.ToString("yyyy/MM/dd 00:00:00");
                    ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd 23:59:59");

                    string Zone = ddl_zone.SelectedItem.ToString();

                    string strSqlGo = "select b.zone,rtolocationname,year(hsrprecord_creationdate) AS year,datename(month,hsrprecord_creationdate) as month, count(*) as total from hsrprecords a,rtolocation b where a.rtolocationid=b.rtolocationid and orderembossingdate between '" + fromdate + "' and '" + ToDate + "' and a.hsrp_stateid='" + HSRPStateID + "' and zone='" + Zone + "' group by b.zone,rtolocationname,year(hsrprecord_creationdate),datename(month,hsrprecord_creationdate) order by 1";

                    DataTable dtcount = Utils.GetDataTable(strSqlGo, CnnString);
                    GridView1.DataSource = dtcount;
                    GridView1.DataBind();

                }
                else
                {

                  string  fromdate = OrderDate.SelectedDate.ToString("yyyy/MM/dd 00:00:00");
                    ToDate = HSRPAuthDate.SelectedDate.ToString("yyyy/MM/dd 23:59:59");

                    string Zone = ddl_zone.SelectedItem.ToString();
                    string rtolocation = dropDownListClient.SelectedItem.ToString();

                    string strSqlGo = "select b.zone,rtolocationname,year(hsrprecord_creationdate) AS year,datename(month,hsrprecord_creationdate) as month, count(*) as total from hsrprecords a,rtolocation b where a.rtolocationid=b.rtolocationid and orderembossingdate between '" + fromdate + "' and '" + ToDate + "' and a.hsrp_stateid='" + HSRPStateID + "' and zone='" + Zone + "' and RtoLocationName='" + rtolocation + "' group by b.zone,rtolocationname,year(hsrprecord_creationdate),datename(month,hsrprecord_creationdate) order by 1";

                   DataTable dtcount = Utils.GetDataTable(strSqlGo, CnnString);
                   GridView1.DataSource = dtcount;
                   GridView1.DataBind();

                }

            }
            catch (Exception ex)
            {

            }
        }

       



       
       

    }
}

           

        
   

 
