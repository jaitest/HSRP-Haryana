using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DataProvider;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;

namespace HSRP.Master
{
    public partial class PendingOrder : System.Web.UI.Page
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
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"]);
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    InitialSetting();
                    try
                    {
                        if (UserType.Equals(0))
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            labelClient.Visible = true;

                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();

                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            labelDate.Visible = false;
                            labelTO.Visible = false;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            buildGrid();
                        }
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
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

                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                // dropDownListClient.SelectedIndex = dropDownListClient.Items.Count - 1;
            }
            else
            {
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where (RTOLocationID=" + RTOLocationID + " or distRelation=" + RTOLocationID + " )and ActiveStatus!='N'";
                //SQLString = "SELECT HSRPRecords.RTOLocationID, RTOLocation.LocationType, RTOLocation.RTOLocationID FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "'";

                DataSet dss = Utils.getDataSet(SQLString, CnnString);
                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RTOLocationID=" + RTOLocationID + " and ActiveStatus!='N' Order by RTOLocationName";
                //DataTable  dts = Utils.GetDataTable(SQLString, CnnString);
                //dropDownListClient.SelectedItem.Text = dts.Rows[0]["RTOLocationName"].ToString();
                //dropDownListClient.DataValueField = dts.Rows[0]["RTOLocationID"].ToString(); 


            }
        }
        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DropDownListStateName.SelectedItem.Text != "--Select Client--")
            {
                ShowGrid();
            }
            else
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Client.";
                return;
            }
        }
        #endregion

        private void ShowGrid()
        {

            if (String.IsNullOrEmpty(dropDownListClient.SelectedValue) || dropDownListClient.SelectedValue.Equals("--Select Client--"))
            {
                Grid1.Items.Clear();
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Client.";
                return;
            }
            buildGrid();
        }


        #region Grid
        public void buildGrid()
        {
            try
            {
                if (UserType.Equals(0))
                {
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                    // SQLString = "SELECT [UserID],[UserFirstName] + space(2) + [UserLastName] as UserName,UserLoginName,[Address1] + space(2) + [Address2]+ space(2)+[City]+space(2)+ [State]+space(2)+[Zip] as Address,[EmailID],[MobileNo],[ContactNo],case when ActiveStatus IN ('Y') then 'Active' else 'Inactive' end as ActiveStatus FROM [Users] Where HSRPStateID=" + intHSRPStateID + " and RTOLocationID=" + intRTOLocationID + " order by UserloginName";
                    SQLString = "SELECT  HSRPAuthorizationSlipDate + 4 as Duedate, HSRPRecordID, HSRPRecord_AuthorizationNo, OwnerName, VehicleRegNo, HSRPAuthorizationSlipDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode FROM HSRPRecords where RTOLocationID=" + intRTOLocationID;

                }
                else if (UserType.Equals(1))
                {
                    intHSRPStateID = HSRPStateID;
                    int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);
                    SQLString = "SELECT  HSRPAuthorizationSlipDate + 4 as Duedate, HSRPRecordID, HSRPRecord_AuthorizationNo, OwnerName, VehicleRegNo, HSRPAuthorizationSlipDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode FROM HSRPRecords where RTOLocationID='" + RTOLocationID + "' and OrderStatus='New Order'";
                }
                else
                {
                    intHSRPStateID = HSRPStateID;
                    intRTOLocationID = RTOLocationID;
                    // SQLString = "SELECT  HSRPAuthorizationSlipDate + 4 as Duedate, HSRPRecordID, HSRPRecord_AuthorizationNo, OwnerName, VehicleRegNo, HSRPAuthorizationSlipDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode FROM HSRPRecords where RTOLocationID='" + RTOLocationID + "' and OrderStatus='New Order'";
                    SQLString = "SELECT HSRPAuthorizationSlipDate + 4 as Duedate,HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo,HSRPRecords.HSRPAuthorizationSlipDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,  HSRPRecords.RTOLocationID, RTOLocation.LocationType, RTOLocation.RTOLocationID FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "') and HSRPRecords.OrderStatus='New Order'";
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
            else
            {
                ActiveStatus = "Y";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Users Set ActiveStatus='" + ActiveStatus + "' Where UserID=" + UserID);
            Utils.ExecNonQuery(sb.ToString(), CnnString);
            buildGrid();
        }
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

        }


        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            String[] StringAuthDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));

            String[] StringOrderDate = OrderDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));


            DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
            DateTime EndDate = Convert.ToDateTime(HSRPAuthDate.SelectedDate);
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

            //  SQLString = "SELECT HSRPAuthorizationSlipDate + 4 as Duedate,HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo,HSRPRecords.HSRPAuthorizationSlipDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,  HSRPRecords.RTOLocationID, RTOLocation.LocationType, RTOLocation.RTOLocationID FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + intRTOLocationID + "' or RTOLocation.RTOLocationID='" + intRTOLocationID + "') and HSRPRecords.OrderDate between'" + OrderDate1 + "'and'" + AuthorizationDate + "'";
            SQLString = "SELECT HSRPRecordID,CONVERT (varchar(20),OrderDate,103) as OrderDate, CONVERT (varchar(20),OrderDate+4,103) as DueDate,  convert(varchar, HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime,HSRP_Front_LaserCode, HSRP_Rear_LaserCode, OwnerName,VehicleType,VehicleClass,MobileNo,  HSRPRecord_AuthorizationNo, VehicleRegNo, ChassisNo, orderStatus, EngineNo FROM HSRPRecords   where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID='" + intRTOLocationID + "' and OrderDate between'" + OrderDate1 + "'and'" + AuthorizationDate + "'";
            
            DataSet dt = Utils.getDataSet(SQLString, CnnString);
            Grid1.DataSource = dt;
            Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
            Grid1.SearchOnKeyPress = true;
            Grid1.DataBind();
            Grid1.RecordCount.ToString();
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

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        { 
        }
        protected void btnExportToExcel_Click(object sender, EventArgs e)
        { 
             try
             {

                 DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                 DateTime EndDate = Convert.ToDateTime(HSRPAuthDate.SelectedDate);
                 int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                 int.TryParse(dropDownListClient.SelectedValue, out intRTOLocationID);

                 string filename = "HSRPPandingOrder-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                 Workbook book = new Workbook();

                 // Specify which Sheet should be opened and the size of window by default
                 book.ExcelWorkbook.ActiveSheetIndex = 1;
                 book.ExcelWorkbook.WindowTopX = 100;
                 book.ExcelWorkbook.WindowTopY = 200;
                 book.ExcelWorkbook.WindowHeight = 7000;
                 book.ExcelWorkbook.WindowWidth = 8000;

                 // Some optional properties of the Document
                 book.Properties.Author = "HSRP";
                 book.Properties.Title = "HSRP Panding Order";
                 book.Properties.Created = DateTime.Now;

                 // Add some styles to the Workbook
                 WorksheetStyle style = book.Styles.Add("HeaderStyle");
                 style.Font.FontName = "Tahoma";
                 style.Font.Size = 13;
                 style.Font.Bold = true;


                 Worksheet sheet = book.Worksheets.Add("HSRP Panding Order");
 
                 sheet.Table.Columns.Add(new WorksheetColumn(150));
                 sheet.Table.Columns.Add(new WorksheetColumn(100));

                 WorksheetRow row = sheet.Table.Rows.Add();
                 // row.
                 row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("HSRP Panding Order", "HeaderStyle"));
                 //row.Cells.Add(<br>);
                 row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell(System.DateTime.Now.ToString(), "HeaderStyle"));  

                 row = sheet.Table.Rows.Add();
                 // Skip one row, and add some text

                 row.Index = 3;

                 //row.Cells.Add("Order Date");
                 row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("Due Date", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("Aurhorization No", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("Vehical Type", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("Vehical Class", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("Mobile No", "HeaderStyle"));

                 row.Cells.Add(new WorksheetCell("Vechal Registration No", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("Chassis No", "HeaderStyle"));
                 row.Cells.Add(new WorksheetCell("Engine No", "HeaderStyle")); 
                 
                 //string SQLString = String.Empty;
                 String StringField = String.Empty;
                 String StringAlert = String.Empty;

                 SQLString = "SELECT HSRPRecordID,CONVERT (varchar(20),OrderDate,103) as OrderDate,CONVERT (varchar(20),OrderDate+4,103) as DueDate ,   OwnerName,VehicleType, VehicleClass,MobileNo, HSRPRecord_AuthorizationNo, VehicleRegNo, ChassisNo, EngineNo FROM HSRPRecords  where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID='" + intRTOLocationID + "' and OrderDate between'" + StartDate + "'and'" + EndDate + "'";

                 DataSet dt = Utils.getDataSet(SQLString, CnnString);
                 
                 if (dt.Tables[0].Rows.Count <= 0)
                 {
                     LabelError.Text = string.Empty;
                     LabelError.Text = "Their is no selected data for the selected  date range.";
                     return;
                 }
                 foreach (DataRow dtrows in dt.Tables[0].Rows) // Loop over the rows.
                 {
                     row = sheet.Table.Rows.Add();

                   
                     row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String));
                     row.Cells.Add(new WorksheetCell(dtrows["DueDate"].ToString(), DataType.String));
                     row.Cells.Add(new WorksheetCell(dtrows["HSRPRecord_AuthorizationNo"].ToString(), DataType.String));
                     row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String));
                     row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String)); 
                     row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String));
                     row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String)); 
                     row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String));
                     row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String)); 
                     row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String)); 

                 }

                 HttpContext context = HttpContext.Current;
                 context.Response.Clear();
                 // Save the file and open it
                 book.Save(Response.OutputStream);
                 //context.Response.ContentType = "text/csv";
                 context.Response.ContentType = "application/vnd.ms-excel";

                 context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                 context.Response.End();
             }

             catch (Exception ex)
             {
                 LabelError.Text = "Error in Populating Grid :" + ex.Message.ToString();
             }




        }

    }
}