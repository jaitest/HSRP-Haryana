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


namespace HSRP.Report
{
    public partial class AvgVehicleCount : System.Web.UI.Page
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
                            OrderDateto.SelectedDate = System.DateTime.Now;


                        }
                        else
                        {

                            // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            OrderDateto.SelectedDate = System.DateTime.Now;

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
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.ActiveStatus ='Y' and UserRTOLocationMapping.UserID='" + strUserID + "' order by a.rtolocationname ";
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
            OrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDateto.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDateto.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDateto.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void BuildGrid1()
        {
            try
            {


                String[] StringAuthDate = OrderDateto.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "-" + StringAuthDate[1] + "-" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = OrderDatefrom.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";

                if (ddlOrderType.SelectedItem.ToString() == "Booking")
                {
                    SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, ORDERDATE)) as [YEAR],DATENAME(MONTH, ORDERDATE) AS [MONTHNAME],"
                                     + "  DATEPART(MONTH, ORDERDATE) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                     + "  FROM hsrprecords "
                                     + "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + ""
                                     + "  GROUP BY DATEPART(YEAR, ORDERDATE),DATENAME(MONTH, ORDERDATE),DATEPART(MONTH, ORDERDATE)";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "Production")
                {

                    SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, orderembossingdate)) as [YEAR],DATENAME(MONTH, orderembossingdate) AS [MONTHNAME],"
                          + "  DATEPART(MONTH, orderembossingdate) as ORDERMONTH,"
                          + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                          + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                          + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                          + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                          + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                          + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB' ) THEN 1 ELSE 0 END ) AS [4W],"
                          + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                          + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                          + "  FROM hsrprecords "
                          + "  WHERE orderembossingdate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and Orderstatus in('Embossing Done','closed') and hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + ""
                          + "  GROUP BY DATEPART(YEAR, orderembossingdate),DATENAME(MONTH, orderembossingdate),DATEPART(MONTH, orderembossingdate)";
                }
                else
                {

                    SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, OrderClosedDate)) as [YEAR],DATENAME(MONTH, OrderClosedDate) AS [MONTHNAME],"
                                     + "  DATEPART(MONTH, OrderClosedDate) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                     + "  FROM hsrprecords "
                                     + "  WHERE OrderClosedDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and Orderstatus in('Closed') and hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + ""
                                     + "  GROUP BY DATEPART(YEAR, OrderClosedDate),DATENAME(MONTH, OrderClosedDate),DATEPART(MONTH, OrderClosedDate)";
                }
                dt = Utils.GetDataTable(SQLString, CnnString);

                DataGrid1.DataSource = dt;

                DataGrid1.DataBind();




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BuildGrid()
        {
            try
            {

               
                String[] StringAuthDate = OrderDateto.SelectedDate.ToString().Split('/');
                String ReportDateEnd = StringAuthDate[0] + "-" + StringAuthDate[1] + "-" + StringAuthDate[2].Split(' ')[0];
                string ReportDate2 = ReportDateEnd + " 23:59:59";
                String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                String[] StringOrderDate = OrderDatefrom.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                string ReportDate1 = ReportDate + " 00:00:00";


                if (ddlOrderType.SelectedItem.ToString()=="Booking")
                {
                    SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, ORDERDATE)) as [YEAR],DATENAME(MONTH, ORDERDATE) AS [MONTHNAME],"
                                     + "  DATEPART(MONTH, ORDERDATE) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                     + "  FROM hsrprecords "
                                     + "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and   hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + "AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                     + "  GROUP BY DATEPART(YEAR, ORDERDATE),DATENAME(MONTH, ORDERDATE),DATEPART(MONTH, ORDERDATE)";
                }
                else if (ddlOrderType.SelectedItem.ToString()=="Production")
                {
                    SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, orderembossingdate)) as [YEAR],DATENAME(MONTH, orderembossingdate) AS [MONTHNAME],"
                                     + "  DATEPART(MONTH, orderembossingdate) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                     + "  FROM hsrprecords "
                                     + "  WHERE orderembossingdate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus  in('Embossing Done','Closed') and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + "AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                     + "  GROUP BY DATEPART(YEAR, orderembossingdate),DATENAME(MONTH, orderembossingdate),DATEPART(MONTH, orderembossingdate)";
                                     //+ "  UNION "
                                     //+ "  SELECT 'Total' as [YEAR],'' AS [MONTHNAME],13 AS ORDERMONTH, "
                                     //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate], "
                                     //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB')  THEN 1 ELSE 0 END ) AS [2W], "
                                     //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [3W],"
                                     //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [4W], "
                                     //+ "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     //+ "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                     //+ "  FROM hsrprecords"
                                     //+ "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus  in('Embossing Done','Closed') and hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + "AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + ""
                                     //+ "  ORDER BY [YEAR], ORDERMONTH ";
                }

                else
                {

                    SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, ordercloseddate)) as [YEAR],DATENAME(MONTH, ordercloseddate) AS [MONTHNAME],"
                                     + "  DATEPART(MONTH, ordercloseddate) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                     + "  FROM hsrprecords "
                                     + "  WHERE ordercloseddate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus  in('Closed') and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + "AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                     + "  GROUP BY DATEPART(YEAR, ordercloseddate),DATENAME(MONTH, ordercloseddate),DATEPART(MONTH, ordercloseddate)";
                                     //+ "  UNION "
                                     //+ "  SELECT 'Total' as [YEAR],'' AS [MONTHNAME],13 AS ORDERMONTH, "
                                     //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate], "
                                     //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB')  THEN 1 ELSE 0 END ) AS [2W], "
                                     //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [3W],"
                                     //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [4W], "
                                     //+ "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     //+ "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                     //+ "  FROM hsrprecords"
                                     //+ "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus  in('Closed') and hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + "AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + ""
                                     //+ "  ORDER BY [YEAR], ORDERMONTH ";
                }

                dt1 = Utils.GetDataTable(SQLString, CnnString);

                DataGrid1.DataSource = dt1;

                DataGrid1.DataBind();




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DropDownListyearName_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btngo_Click(object sender, EventArgs e)
        {

            if (DropDownListStateName.SelectedItem.Text != "--Select State--" && dropDownListClient.SelectedItem.Text != "ALL")
            {
                BuildGrid();

            }

            else
            {
                if (DropDownListStateName.SelectedItem.Text != "--Select State--" && dropDownListClient.SelectedItem.Text == "ALL")
                {
                    BuildGrid1();
                }

                else
                {
                    DataGrid1.Items.Clear();

                    return;
                }
            }
        }

        private Boolean validate1()
        {
            Boolean blnvalid = true;
            String getvalue = string.Empty;
            getvalue = DropDownListStateName.SelectedItem.Text;
            if (getvalue == "--Select State--")
            {
                blnvalid = false;

                Label1.Text = "Please select State Name";

            }
            return blnvalid;

        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            validate1();
            if (validate1() == false)
            {
                return;
            }

            else
            {
                Label1.Visible = false;

                try
                {
                    String[] StringAuthDate = OrderDateto.SelectedDate.ToString().Split('/');
                    String ReportDateEnd = StringAuthDate[0] + "-" + StringAuthDate[1] + "-" + StringAuthDate[2].Split(' ')[0];
                    string ReportDate2 = ReportDateEnd + " 23:59:59";
                    String DatePrint2 = StringAuthDate[1] + "/" + StringAuthDate[0] + "/" + StringAuthDate[2].Split(' ')[0];


                    String[] StringOrderDate = OrderDatefrom.SelectedDate.ToString().Split('/');
                    string Mon = ("0" + StringOrderDate[0]);
                    string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                    String From2 = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                    String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                    String DatePrint1 = StringOrderDate[1] + "/" + StringOrderDate[0] + "/" + StringOrderDate[2].Split(' ')[0];

                    String DatePrint = DatePrint1 + "   To   " + DatePrint2;

                    String ReportDate = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                    string ReportDate1 = ReportDate + " 00:00:00";

                    DateTime StartDate = Convert.ToDateTime(OrderDatefrom.SelectedDate);
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                    //int.TryParse(dropDownListUser.SelectedValue, out intUserID);

                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                    //int.TryParse(dropDownListUser.SelectedValue, out UserID);

                    string filename = "Total_Vehicle_Count" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook();

                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "HSRP Total Vehicle Count";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
                    WorksheetStyle style = book.Styles.Add("HeaderStyle");
                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 9;
                    style.Font.Bold = false;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style8 = book.Styles.Add("HeaderStyle8");
                    style8.Font.FontName = "Tahoma";
                    style8.Font.Size = 10;
                    style8.Font.Bold = true;
                    style8.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                    style8.Interior.Color = "#D4CDCD";
                    style8.Interior.Pattern = StyleInteriorPattern.Solid;

                    WorksheetStyle style5 = book.Styles.Add("HeaderStyle5");
                    style5.Font.FontName = "Tahoma";
                    style5.Font.Size = 10;
                    style5.Font.Bold = false;
                    style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                    style5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = true;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                    WorksheetStyle style2 = book.Styles.Add("HeaderStyle2");
                    style2.Font.FontName = "Tahoma";
                    style2.Font.Size = 10;
                    style2.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                    WorksheetStyle style3 = book.Styles.Add("HeaderStyle3");
                    style3.Font.FontName = "Tahoma";
                    style3.Font.Size = 12;
                    style3.Font.Bold = true;
                    style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                    Worksheet sheet = book.Worksheets.Add("HSRP Total Vehicle Count");
                    sheet.Table.Columns.Add(new WorksheetColumn(60));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));

                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(92));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));


                    WorksheetStyle style9 = book.Styles.Add("HeaderStyle9");
                    style9.Font.FontName = "Tahoma";
                    style9.Font.Size = 10;
                    style9.Font.Bold = true;
                    style9.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                    style9.Interior.Color = "#FCF6AE";
                    style9.Interior.Pattern = StyleInteriorPattern.Solid;


                    WorksheetRow row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("HSRP Total Vehicle Count");
                    cell.MergeAcross = 3; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                    //row = sheet.Table.Rows.Add();
                    //row.Index = 4;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();
                    row.Index = 4;

                    DateTime dates = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Date Generated from:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DatePrint1, "HeaderStyle2"));
                    //row = sheet.Table.Rows.Add();
                    //row.Index = 6;
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    //row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    row.Cells.Add(new WorksheetCell("To:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DatePrint2, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();

                    row.Index = 5;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    //if (UserType.Equals(0))
                    //{
                    //    row.Cells.Add(new WorksheetCell(dropDownListUser.SelectedItem.Text, "HeaderStyle2"));
                    //}
                    //else
                    //{
                    //    row.Cells.Add(new WorksheetCell(Session["UserName"].ToString(), "HeaderStyle2"));
                    //}
                    row = sheet.Table.Rows.Add();

                    row.Index = 7;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("YEAR", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Month Name", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("Order Month", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("2W Single Plate", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("3W Single Plate", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("4W Single Plate", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("2W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("3W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("4W", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("MCV/HCV/TRAILERS", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("Close Date", "HeaderStyle6"));
                    row.Cells.Add(new WorksheetCell("Tractor", "HeaderStyle6"));
                    //row.Cells.Add(new WorksheetCell("Delay in No. of Days", "HeaderStyle"));

                    row = sheet.Table.Rows.Add();
                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    row.Index = 8;

                    if (dropDownListClient.SelectedItem.Text == "ALL")
                    {
                        if (ddlOrderType.SelectedItem.ToString() == "Booking")
                        {
                            SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, ORDERDATE)) as [YEAR],DATENAME(MONTH, ORDERDATE) AS [MONTHNAME],"
                                             + "  DATEPART(MONTH, ORDERDATE) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]" 
                                     + "  FROM hsrprecords "
                                             + "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and   hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + ""
                                             + "  GROUP BY DATEPART(YEAR, ORDERDATE),DATENAME(MONTH, ORDERDATE),DATEPART(MONTH, ORDERDATE)";
                        }
                        else if (ddlOrderType.SelectedItem.ToString() == "Production")
                        {
                            SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, orderembossingdate)) as [YEAR],DATENAME(MONTH, orderembossingdate) AS [MONTHNAME],"
                                             + "  DATEPART(MONTH, orderembossingdate) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]" 
                                     + "  FROM hsrprecords "
                                             + "  WHERE orderembossingdate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus  in('Embossing Done','Closed') and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + " "
                                             + "  GROUP BY DATEPART(YEAR, orderembossingdate),DATENAME(MONTH, orderembossingdate),DATEPART(MONTH, orderembossingdate)";
                        }

                        else
                        {

                            SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, ordercloseddate)) as [YEAR],DATENAME(MONTH, ordercloseddate) AS [MONTHNAME],"
                                             + "  DATEPART(MONTH, ordercloseddate) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]" 
                                     + "  FROM hsrprecords "
                                             + "  WHERE ordercloseddate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus  in('Closed') and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + ""
                                             + "  GROUP BY DATEPART(YEAR, ordercloseddate),DATENAME(MONTH, ordercloseddate),DATEPART(MONTH, ordercloseddate)";
                        }


                        dt = Utils.GetDataTable(SQLString, CnnString);

                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["YEAR"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["MONTHNAME"].ToString(), DataType.String, "HeaderStyle"));
                                //row.Cells.Add(new WorksheetCell(dtrows["ORDERMONTH"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["2W Single Plate"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["3W Single Plate"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["4W Single Plate"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["2W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["3W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["4W"].ToString(), DataType.String, "HeaderStyle"));
                                //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                                row.Cells.Add(new WorksheetCell(dtrows["MCV/HCV/TRAILERS"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["TRACTOR"].ToString(), DataType.String, "HeaderStyle"));

                                row = sheet.Table.Rows.Add();
                            }

                            row = sheet.Table.Rows.Add();
                            HttpContext context = HttpContext.Current;
                            context.Response.Clear();
                            // Save the file and open it
                            book.Save(Response.OutputStream);

                            //context.Response.ContentType = "text/csv";
                            context.Response.ContentType = "application/vnd.ms-excel";

                            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                            context.Response.End();
                        }
                    }
                    else
                    {
                        if (ddlOrderType.SelectedItem.ToString() == "Booking")
                        {
                            SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, ORDERDATE)) as [YEAR],DATENAME(MONTH, ORDERDATE) AS [MONTHNAME],"
                                         + "  DATEPART(MONTH, ORDERDATE) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]" + "  FROM hsrprecords "
                                         + "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + " AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                         + "  GROUP BY DATEPART(YEAR, ORDERDATE),DATENAME(MONTH, ORDERDATE),DATEPART(MONTH, ORDERDATE)";
                                         //+ "  UNION "
                                         //+ "  SELECT 'Total' as [YEAR],'' AS [MONTHNAME],13 AS ORDERMONTH, "
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                         //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate], "
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB')  THEN 1 ELSE 0 END ) AS [2W], "
                                         //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [3W],"
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [4W], "
                                         //+ "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                         //+ "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                         //+ "  FROM hsrprecords"
                                         //+ "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + " AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                         //+ "  ORDER BY [YEAR], ORDERMONTH ";
                        }

                        else if (ddlOrderType.SelectedItem.ToString()=="Production")
                        {
                            SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, orderembossingdate)) as [YEAR],DATENAME(MONTH, orderembossingdate) AS [MONTHNAME],"
                                         + "  DATEPART(MONTH, orderembossingdate) as ORDERMONTH,"
                                         + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                         + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                         + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                         + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                         + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                         + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                         + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                         + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                         + "  FROM hsrprecords "
                                         + "  WHERE orderembossingdate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus in('Embossing Done','Closed') and hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + " AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                         + "  GROUP BY DATEPART(YEAR, orderembossingdate),DATENAME(MONTH, orderembossingdate),DATEPART(MONTH, orderembossingdate)";
                                         //+ "  UNION "
                                         //+ "  SELECT 'Total' as [YEAR],'' AS [MONTHNAME],13 AS ORDERMONTH, "
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                         //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate], "
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB')  THEN 1 ELSE 0 END ) AS [2W], "
                                         //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [3W],"
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [4W], "
                                         //+ "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                         //+ "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                         //+ "  FROM hsrprecords"
                                         //+ "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus in('Embossing Done','Closed') and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + " AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                         //+ "  ORDER BY [YEAR], ORDERMONTH ";
                        }


                        else
                        {
                            SQLString = " SELECT convert(varchar(5),DATEPART(YEAR, ordercloseddate)) as [YEAR],DATENAME(MONTH, ordercloseddate) AS [MONTHNAME],"
                                         + "  DATEPART(MONTH, ordercloseddate) as ORDERMONTH,"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate],"
                                     + "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB' or orderType='DB')  THEN 1 ELSE 0 END ) AS [2W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [3W],"
                                     + " SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB' or orderType='DB') THEN 1 ELSE 0 END ) AS [4W],"
                                     + "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                     + "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                         + "  FROM hsrprecords "
                                         + "  WHERE ordercloseddate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus in('Closed') and hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + " AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                         + "  GROUP BY DATEPART(YEAR, ordercloseddate),DATENAME(MONTH, ordercloseddate),DATEPART(MONTH, ordercloseddate)";
                                         //+ "  UNION "
                                         //+ "  SELECT 'Total' as [YEAR],'' AS [MONTHNAME],13 AS ORDERMONTH, "
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [2W Single Plate],"
                                         //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER' and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [3W Single Plate],"
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)') and (orderType='DR' or orderType ='DF') THEN 1 ELSE 0 END ) AS [4W Single Plate], "
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'MOTOR CYCLE' OR VehicleType ='SCOOTER') and (orderType='NB' or orderType ='OB')  THEN 1 ELSE 0 END ) AS [2W], "
                                         //+ "  SUM(CASE WHEN  VehicleType = 'THREE WHEELER'  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [3W],"
                                         //+ "  SUM(CASE WHEN  (VehicleType = 'LMV' OR VehicleType = 'LMV(CLASS)')  and (orderType='NB' or orderType ='OB') THEN 1 ELSE 0 END ) AS [4W], "
                                         //+ "  SUM(CASE WHEN  VehicleType = 'MCV/HCV/TRAILERS' THEN 1 ELSE 0 END ) AS [MCV/HCV/TRAILERS],"
                                         //+ "  SUM(CASE WHEN  VehicleType = 'TRACTOR' THEN 1 ELSE 0 END ) AS [TRACTOR]"
                                         //+ "  FROM hsrprecords"
                                         //+ "  WHERE OrderDate between '" + ReportDate1 + "' and '" + ReportDate2 + "' and orderstatus in('Closed') and  hsrp_stateid=" + DropDownListStateName.SelectedValue.ToString() + " AND RTOLocationID=" + dropDownListClient.SelectedValue.ToString() + " "
                                         //+ "  ORDER BY [YEAR], ORDERMONTH ";
                        }



                        dt1 = Utils.GetDataTable(SQLString, CnnString);
                        if (dt1.Rows.Count > 0)
                        {

                            foreach (DataRow dtrows in dt1.Rows) // Loop over the rows.
                            {
                                row.Cells.Add(new WorksheetCell(dtrows["YEAR"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["MONTHNAME"].ToString(), DataType.String, "HeaderStyle"));
                                //row.Cells.Add(new WorksheetCell(dtrows["ORDERMONTH"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["2W Single Plate"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["3W Single Plate"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["4W Single Plate"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["2W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["3W"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["4W"].ToString(), DataType.String, "HeaderStyle"));
                                //row.Cells.Add(new WorksheetCell(dtrows["InvoiceDateTime"].ToString(), DataType.String, "HeaderStyle"));

                                row.Cells.Add(new WorksheetCell(dtrows["MCV/HCV/TRAILERS"].ToString(), DataType.String, "HeaderStyle"));
                                row.Cells.Add(new WorksheetCell(dtrows["TRACTOR"].ToString(), DataType.String, "HeaderStyle"));

                                row = sheet.Table.Rows.Add();
                            }

                            row = sheet.Table.Rows.Add();
                            HttpContext context = HttpContext.Current;
                            context.Response.Clear();
                            // Save the file and open it
                            book.Save(Response.OutputStream);

                            //context.Response.ContentType = "text/csv";
                            context.Response.ContentType = "application/vnd.ms-excel";

                            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                            context.Response.End();
                        }
                        return;
                    }


                }

                catch (Exception ex)
                {
                    throw ex;
                }


            }


        }
    }
}