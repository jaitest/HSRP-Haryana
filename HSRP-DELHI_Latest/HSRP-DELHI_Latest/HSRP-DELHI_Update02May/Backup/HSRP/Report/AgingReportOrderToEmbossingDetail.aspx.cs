using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Report
{
    public partial class AgingReportOrderToEmbossingDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sqlquery = string.Empty;
            string lbl = string.Empty;
            string AuthorizationDate = string.Empty;
            string StateName = string.Empty;
            string CnnString = string.Empty;
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!IsPostBack)
            {
                string CommandName = Request.QueryString["CommandName"].ToString();
                StateName = Request.QueryString["StateName"].ToString();
                lbl = Request.QueryString["Location"].ToString();
                AuthorizationDate = Request.QueryString["Date"].ToString();



                if (CommandName == "7 days")
                {
                    sqlquery = "select row_number() over(order by HSRPRecords.vehicleregno) as Sno,HSRPRecords.vehicleregno,HSRPRecords.Chassisno as 'Chasis No' ,HSRPRecords.EngineNo as 'Engine No',upper(HSRPRecords.VehicleClass) as 'Vehicle Class',HSRPRecords.VehicleType as 'Vehicle Type',HSRPRecords.RoundOff_NetAmount as Fees,convert(varchar(10),HSRPRecords.orderdate,103)  as 'Fees Collection Date' from HSRPRecords  inner join rtolocation on HSRPRecords.rtolocationid=rtolocation.rtolocationid and rtolocation.rtolocationname='" + lbl + "' where HSRPRecords.HSRP_StateID='" + StateName + "' and HSRPRecords.HSRPRecord_CreationDate > (Convert(datetime, '" + AuthorizationDate + "')-7) and HSRPRecords.orderstatus='Embossing Done'";
                }
                if (CommandName == "15 days")
                {
                    sqlquery = "select row_number() over(order by HSRPRecords.vehicleregno) as Sno,HSRPRecords.vehicleregno,HSRPRecords.Chassisno as 'Chasis No' ,HSRPRecords.EngineNo as 'Engine No',upper(HSRPRecords.VehicleClass) as 'Vehicle Class',HSRPRecords.VehicleType as 'Vehicle Type',HSRPRecords.RoundOff_NetAmount as Fees,convert(varchar(10),HSRPRecords.orderdate,103)  as 'Fees Collection Date' from HSRPRecords  inner join rtolocation on HSRPRecords.rtolocationid=rtolocation.rtolocationid and rtolocation.rtolocationname='" + lbl + "' where HSRPRecords.HSRP_StateID='" + StateName + "' and HSRPRecords.HSRPRecord_CreationDate > (Convert(datetime, '" + AuthorizationDate + "')-22) and HSRPRecords.HSRPRecord_CreationDate < (Convert(datetime, '" + AuthorizationDate + "') -7) and HSRPRecords.orderstatus='Embossing Done'";
                }
                if (CommandName == "30 days")
                {
                    sqlquery = "select row_number() over(order by HSRPRecords.vehicleregno) as Sno,HSRPRecords.vehicleregno,HSRPRecords.Chassisno as 'Chasis No' ,HSRPRecords.EngineNo as 'Engine No',upper(HSRPRecords.VehicleClass) as 'Vehicle Class',HSRPRecords.VehicleType as 'Vehicle Type',HSRPRecords.RoundOff_NetAmount as Fees,convert(varchar(10),HSRPRecords.orderdate,103)  as 'Fees Collection Date' from HSRPRecords  inner join rtolocation on HSRPRecords.rtolocationid=rtolocation.rtolocationid and rtolocation.rtolocationname='" + lbl + "' where HSRPRecords.HSRP_StateID='" + StateName + "' and HSRPRecords.HSRPRecord_CreationDate > (Convert(datetime, '" + AuthorizationDate + "')-52) and HSRPRecords.HSRPRecord_CreationDate < (Convert(datetime, '" + AuthorizationDate + "') -22) and HSRPRecords.orderstatus='Embossing Done'";
                }
                if (CommandName == "60 days")
                {
                    sqlquery = "select row_number() over(order by HSRPRecords.vehicleregno) as Sno,HSRPRecords.vehicleregno,HSRPRecords.Chassisno as 'Chasis No' ,HSRPRecords.EngineNo as 'Engine No',upper(HSRPRecords.VehicleClass) as 'Vehicle Class',HSRPRecords.VehicleType as 'Vehicle Type',HSRPRecords.RoundOff_NetAmount as Fees,convert(varchar(10),HSRPRecords.orderdate,103)  as 'Fees Collection Date' from HSRPRecords  inner join rtolocation on HSRPRecords.rtolocationid=rtolocation.rtolocationid and rtolocation.rtolocationname='" + lbl + "' where HSRPRecords.HSRP_StateID='" + StateName + "' and HSRPRecords.HSRPRecord_CreationDate > (Convert(datetime, '" + AuthorizationDate + "')-112) and HSRPRecords.HSRPRecord_CreationDate < (Convert(datetime, '" + AuthorizationDate + "') -52) and HSRPRecords.orderstatus='Embossing Done'";
                }
                if (CommandName == " More Than 60 days")
                {
                    sqlquery = "select row_number() over(order by HSRPRecords.vehicleregno) as Sno,HSRPRecords.vehicleregno,HSRPRecords.Chassisno as 'Chasis No' ,HSRPRecords.EngineNo as 'Engine No',upper(HSRPRecords.VehicleClass) as 'Vehicle Class',HSRPRecords.VehicleType as 'Vehicle Type',HSRPRecords.RoundOff_NetAmount as Fees,convert(varchar(10),HSRPRecords.orderdate,103)  as 'Fees Collection Date' from HSRPRecords  inner join rtolocation on HSRPRecords.rtolocationid=rtolocation.rtolocationid and rtolocation.rtolocationname='" + lbl + "' where HSRPRecords.HSRP_StateID='" + StateName + "' and HSRPRecords.HSRPRecord_CreationDate < (Convert(datetime, '" + AuthorizationDate + "')-112) and HSRPRecords.orderstatus='Embossing Done'";
                }
                DataTable dt = Utils.GetDataTable(sqlquery, CnnString);
                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add(dt.NewRow());
                    DetailsView1.DataSource = dt;
                    DetailsView1.DataBind();
                    int totalcolums = DetailsView1.Rows[0].Cells.Count;
                    DetailsView1.Rows[0].Cells.Clear();
                    DetailsView1.Rows[0].Cells.Add(new TableCell());
                    DetailsView1.Rows[0].Cells[0].ColumnSpan = totalcolums;
                    DetailsView1.Rows[0].Cells[0].Text = "No Data Found";
                }
                else
                {
                    DetailsView1.DataSource = dt;
                    DetailsView1.DataBind();
                }
            }
        }
    }
}