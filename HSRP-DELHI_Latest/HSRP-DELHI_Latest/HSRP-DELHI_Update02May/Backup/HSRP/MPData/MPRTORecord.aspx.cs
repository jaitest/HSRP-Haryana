
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using System.Text;

namespace HSRP.Master
{
    public partial class MPRTORecord : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
         int intHSRPStateID;
         int intRTOLocationID;
         string OrderStatus = string.Empty;
         DateTime AuthorizationDate;
         DateTime OrderDate1;
         DataProvider.BAL bl = new DataProvider.BAL();
         string StickerManditory = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["MPConnectionString"].ToString();
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"]);
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString()); 
                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress; 
                if (!IsPostBack)
                {
                      InitialSetting(); 
                    try
                    { 
                        if (UserType.Equals(0))
                        { 
                        }
                        else
                        { 
                            hiddenUserType.Value = "1"; 
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }
         
        private void ShowGrid()
        { 
            buildGrid();
        }


        #region Grid
        public void buildGrid()
        {
            try
            { 
                if (UserType.Equals(0))
                {
                      SQLString = "SELECT CONVERT (varchar(20),HSRPRecord_AuthorizationDate+4,103) as Duedate,  CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecord_AuthorizationNo, HSRPRecords.OwnerName, HSRPRecords.VehicleRegNo, HSRPRecords.OrderStatus, HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.HSRPRecordID, RTOInventory.LaserPlateBoxNo FROM HSRPRecords INNER JOIN RTOInventory ON HSRPRecords.HSRPRecordID = RTOInventory.HSRPRecordID where HSRPRecords.RTOLocationID=" + intRTOLocationID;

                }
                else
                {
                    intHSRPStateID=HSRPStateID;
                    SQLString = "SELECT CONVERT (varchar(20),HSRPRecord_AuthorizationDate+4,103) as Duedate,  CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecord_AuthorizationNo, HSRPRecords.OwnerName, HSRPRecords.VehicleRegNo, HSRPRecords.OrderStatus, HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.HSRPRecordID, RTOInventory.LaserPlateBoxNo FROM HSRPRecords INNER JOIN RTOInventory ON HSRPRecords.HSRPRecordID = RTOInventory.HSRPRecordID where HSRPRecords.RTOLocationID=" + intRTOLocationID+" and OrderStatus='New Order'";
                } 
                DataTable dt = Utils.GetDataTable(SQLString, CnnString); 
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                Grid1.RecordCount.ToString();
                
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }
        public void OnNeedRebind(object sender, EventArgs oArgs)
        {
            System.Threading.Thread.Sleep(200);
            Grid1.DataBind();
        }
        public void OnNeedDataSource(object sender, EventArgs oArgs)
        {
            buildGrid();
        }
        public void OnPageChanged(object sender, ComponentArt.Web.UI.GridPageIndexChangedEventArgs oArgs)
        {
            Grid1.CurrentPageIndex = oArgs.NewIndex;
        }
        public void OnFilter(object sender, ComponentArt.Web.UI.GridFilterCommandEventArgs oArgs)
        {
            Grid1.Filter = oArgs.FilterExpression;
        }
        public void OnSort(object sender, ComponentArt.Web.UI.GridSortCommandEventArgs oArgs)
        {
            Grid1.Sort = oArgs.SortExpression;
        }
        private void ddGridRunningMode_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            buildGrid();
            Grid1.DataBind();
            adjustToRunningMode();
        }
        public void OnGroup(object sender, ComponentArt.Web.UI.GridGroupCommandEventArgs oArgs)
        {
            Grid1.GroupBy = oArgs.GroupExpression;
        }
        private void adjustToRunningMode()
        {

            Grid1.SliderPopupClientTemplateId = "SliderTemplate";
            Grid1.SliderPopupOffsetX = 20;

        }
        #endregion

        protected void Grid1_ItemCommand(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {
            String UserID = e.Item["UserID"].ToString();
            string ActiveStatus = e.Item["ActiveStatus"].ToString();
            if (ActiveStatus == "Active")
            {
                ActiveStatus = "N";
            }
            else {
                ActiveStatus = "Y";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Users Set ActiveStatus='" + ActiveStatus + "' Where UserID=" + UserID);
            Utils.ExecNonQuery(sb.ToString(), CnnString);
            buildGrid();
        } 
       
       protected void ButtonGo_Click(object sender, EventArgs e)
        {
           String[] StringAuthDate =  OrderDate.SelectedDate.ToString().Split('/'); 
            AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1])); 
            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/'); 
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
              
           //SQLString = "select ROW_NUMBER() Over (Order by RTO_NAME) As SNo, RTO_NAME,COUNT(RTO_NAME) as records from HSRP_StagingData where  convert(varchar,  WS_UPLOAD_AT, 105) between '" + FromDateTo + "' and '" + FromDate + "' group by RTO_NAME";
            SQLString = "select ROW_NUMBER() Over (Order by RTO_NAME) As SNo, RTO_NAME,COUNT(RTO_NAME) as records from HSRP_StagingData where WS_UPLOAD_AT between '" + AuthorizationDate + "' and '" + OrderDate1 + "' group by RTO_NAME";

            DataSet dt = MPUtils.getDataSet(SQLString, CnnString);
            if (dt.Tables[0].Rows.Count > 0)
            {
                 
                Grid1.DataSource = dt;
                Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                Grid1.SearchOnKeyPress = true;
                Grid1.DataBind();
                Grid1.RecordCount.ToString();  
            }
            else
            {
                Grid1.Items.Clear();
               
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
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-7.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {
           
        }

        protected void Grid1_ItemCommand1(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {
             
        } 
        
        
    }
}