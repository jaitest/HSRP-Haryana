using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace HSRP.Report
{
    public partial class ZonewiseReport : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
       
        string SQLString1 = string.Empty;
       
        string recordtype = string.Empty;
        string day1date1 = string.Empty;
        string day1date = string.Empty;
        string day2date = string.Empty;
        string day3date = string.Empty;
        string day4date = string.Empty;
        string day5date = string.Empty;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtsession = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();

                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType1.Equals(0))
                        {                            
                           FilldropDownZone();
                           InitialSetting();
                           BindReportTypeddl();
                        }
                        else
                        {
                            FilldropDownZone();
                            InitialSetting();
                            BindReportTypeddl();
                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        private void FilldropDownZone()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select distinct zone from rtolocation where hsrp_stateid = 4  and isnull(zone,'')!='' and  ActiveStatus='Y' Order by zone";
                Utils.PopulateDropDownList(DropDownListzone, SQLString1.ToString(), CnnString1, "--Select Zoze--");
            }
            else
            {
                SQLString1 = "SELECT  zone FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" +strUserID1 + "' and ActiveStatus!='N' order by zone ";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListzone.DataSource = dts;
                DropDownListzone.DataBind();
            }
        }

        private void FilldropDownListRtolocation()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select RTOLocationID, RTOLocationName from rtolocation where hsrp_stateid = 4  and isnull(zone,'')!='' and  ActiveStatus='Y'  and zone='" + DropDownListzone.SelectedValue.ToString() + "' Order by  RTOLocationName";
                Utils.PopulateDropDownList(DropDownListRtolocation, SQLString1.ToString(), CnnString1, "--Select RtoLocation--");
            }
            else
            {
                SQLString1 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName,  a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.zone ='" + DropDownListzone.SelectedValue.ToString() + "'  and ActiveStatus!='N' order by RTOLocationName ";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListRtolocation.DataSource = dts;
                DropDownListRtolocation.DataBind();
            }
        }

        private void FilldropDownListDealer()
        {
            //if (UserType1.Equals(0))
            //{
                SQLString1 = "select Dealerid,DealerName  from dealermaster  where hsrp_stateid = 4 and RTOLocationID='" + DropDownListRtolocation.SelectedValue.ToString()+ "'  Order by DealerName";
                Utils.PopulateDropDownList(DropDownListDealer, SQLString1.ToString(), CnnString1, "--Select Dealer Name--");
            //}
            //else
            //{
            //    SQLString1 = "select Dealerid,DealerName  from dealermaster  where hsrp_stateid = 4 and RTOLocationID='" + DropDownListRtolocation.SelectedValue.ToString() + "'  Order by DealerName";
            //    DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
            //    DropDownListDealer.DataSource = dts;
            //    DropDownListDealer.DataBind();
            //}
        }

        private void BindReportTypeddl()
        {
            string sqlstring = "[Business_ReportTypewisesummary_onlinereport_master] '" + Dateto.SelectedDate + "','" + Dateto.SelectedDate + "','DB','4','0','0','0' ";
           DataTable  dt11 = new DataTable();
            dt11 = Utils.GetDataTable(sqlstring, CnnString1);
            DdlReportType.DataSource = dt11;
            DdlReportType.DataTextField = "ReportType";
            DdlReportType.DataValueField = "Code";
            DdlReportType.DataBind();
            DdlReportType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "--Select--"));
        }



        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            Datefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Datefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            Dateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Dateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        //private Boolean validate1()
        //{
        //    Boolean blnvalid = true;
        //    String getvalue = string.Empty;
        //    getvalue = DropDownListStateName.SelectedItem.Text;
        //    if (getvalue == "--Select State--")
        //    {
        //        blnvalid = false;

        //        Label1.Text = "Please select State Name";

        //    }
        //    return blnvalid;

        //}

        protected void btnexport_Click(object sender, EventArgs e)
        {
            Lblerror.Visible = false;
            if (DropDownListzone.SelectedItem.Text.ToString() == "--Select Zoze--")
            {
                Lblerror.Visible = true;
                Lblerror.Text = "Please Select Zone";
                return;
            }

          
        }

        protected void DropDownListzone_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            if (DropDownListzone.SelectedItem.Text.ToString() == "--Select Zoze--")
            {
                Lblerror.Visible = true;
                Lblerror.Text = "Please Select Zone";
                return;
            }

            else
            {
                Lblerror.Visible = false;
                FilldropDownListRtolocation();
            }
           

        }

        protected void DropDownListRtolocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListRtolocation.SelectedItem.Text.ToString() == "--Select RtoLocation--")
            {
                Lblerror.Visible = true;
                Lblerror.Text = "Please Select Zone";
                return;
            }

            else
            {
                Lblerror.Visible = false;
                FilldropDownListDealer();
            }
           
        }

        protected void DropDownListDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

    
    }
}