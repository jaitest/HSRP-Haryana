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
    public partial class LaserMonitor : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname, pp;
        string transtr, statename1;
        BaseFont basefont;
        string fontpath;
        string strComplaintID = string.Empty;
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
        DataTable Totaltg = new DataTable();
        DataTable Totalap = new DataTable();


        DataTable dt = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            //btnUpdate.Visible = false;
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
                            //LaserMonitorBeforeErp();
                            //LaserMonitorAfterErp();
                          

                        }
                        else
                        {
                            // LaserMonitorBeforeErp();
                            //LaserMonitorAfterErp();
                          

                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

       





        private void InitialSetting()
        {

            //string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-1);
            //HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-1);
            //OrderDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }


        public void LaserMonitorBeforeErp()
        {
            try
            {
                //string date = System.DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                string date1 = System.DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
                string show = "select hsrpstatename,sum(totorders) as LaserMonitor from vw_allstatelaser_beforeERP group by hsrpstatename";
                DataTable dtsop = Utils.GetDataTable(show, CnnString);
                GrdAP.DataSource = dtsop;
                GrdAP.DataBind();
            }

            catch (Exception err)
            {
                lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
            }
        }


        public void LaserMonitorAfterErp()
        {
            try
            {
                //string date = System.DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                string date1 = System.DateTime.Now.ToString("yyyy/MM/dd 23:59:59");
                string show = "select hsrpstatename,sum(totorders) as LaserMonitor from vw_allstatelaser_afterERP group by hsrpstatename";
                DataTable dtsop = Utils.GetDataTable(show, CnnString);
                GridView1.DataSource = dtsop;
                GridView1.DataBind();
            }

            catch (Exception err)
            {
                lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
            }
        }

        protected void btnBeforeErp_Click(object sender, EventArgs e)
        {
            LaserMonitorBeforeErp();
        }

        protected void btnAfterErp_Click(object sender, EventArgs e)
        {
            LaserMonitorAfterErp();
        }



        













        }
    }


