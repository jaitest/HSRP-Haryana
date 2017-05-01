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

namespace HSRP.BusinessReports
{
    public partial class APTGCallCenterReportDealer : System.Web.UI.Page
    {
        int UserType;
        string CnnString = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string strUserID = string.Empty;
        int intHSRPStateID;
        int intRTOLocationID;
        string SQLString = string.Empty;
        //DateTime OrderDate1;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtshow = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();

                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType.Equals(0))
                        {
                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            //OrderDateto.SelectedDate = System.DateTime.Now;


                        }
                        else
                        {

                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            //OrderDateto.SelectedDate = System.DateTime.Now;

                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }


        private void FilldropDownListOrganization()
        {
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

        private void FilldropDownListClient()
        {
            if (UserType.Equals(0))
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "ALL");
            }
            else
            {
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "ALL");
                //DataSet dss = Utils.getDataSet(SQLString, CnnString);
                //dropDownListClient.DataSource = dss;
                //dropDownListClient.DataBind();
            }
        }


        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
           // OrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
           // OrderDateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }

        protected void btnexport_Click(object sender, EventArgs e)
        {


            try
            {
                //String[] StringAuthDate = OrderDateto.SelectedDate.ToString().Split('/');
                //String ReportDateEnd = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //string ReportDate2 = ReportDateEnd + " 23:59";
                //String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = OrderDatefrom.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                //String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00";


                DateTime StartDate = Convert.ToDateTime(OrderDatefrom.SelectedDate);
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //int.TryParse(dropDownListUser.SelectedValue, out intUserID);

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                //int.TryParse(dropDownListUser.SelectedValue, out UserID);


                string fromdate = OrderDatefrom.SelectedDate.ToString("yyyy/MM/dd");
                string ToDate = OrderDatefrom.SelectedDate.ToString("yyyy/MM/dd 23:59:59");

                SQLString = "select vehicleregno,VEHICLETYPE,convert(varchar(20),Orderdate,103) as OrderDate,OwnerName,Mobileno,convert(varchar(20),OrderEmbossingDate,103) as OrderEmbossingDate,convert(varchar(20),ChallanDate,103) as ChallanDate,remarks from hsrprecords where HSRP_StateID='" + DropDownListStateName.SelectedValue.ToString() + "' and RTOLocationID='" + dropDownListClient.SelectedValue.ToString() + "' and Orderdate between '" + fromdate + "' and '" + ToDate + "' and OrderStatus='Embossing Done' and isnull(challandate,'')!=''order by ChallanDate";
                dtshow = Utils.GetDataTable(SQLString, CnnString);
                grdpending.DataSource = dtshow;
                grdpending.DataBind();


            }

            catch(Exception ex)
            {
                ex.ToString();
            }

        }
    

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       

        protected void grdpending_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void grdpending_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Approval")
            //{
            //    string url = "UKNewProcessStatus.aspx";
            //    string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100');";
            //    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            //}

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    LinkButton link = e.Row.FindControl("LinkButton1") as LinkButton;
            //    link.Attributes["onclick"] = "return popwin(" + ((vehicleregno)e.Row.DataItem).Id + ")";
            //}
        }

        
    }
}