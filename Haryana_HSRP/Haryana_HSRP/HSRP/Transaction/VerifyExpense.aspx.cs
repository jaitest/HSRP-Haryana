using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class VerifyExpense : System.Web.UI.Page
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
        

        string AllLocation = string.Empty;
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
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                        }
                        else
                        {
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                            //buildGrid();
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

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(-3.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");

                
            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownListClient.Visible = true;

                dropDownListClient.DataSource = dss;
                dropDownListClient.DataBind();
                

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();

                    }
                  

                }
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

                ClearGrid();
                Grid1.Items.Clear();

                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string AuthorizationDate = From + " 00:00:00"; // Convert.ToDateTime();

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                string FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];

                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string OrderDate1 = From1 + " 23:59:59";

                DateTime StartDate = Convert.ToDateTime(OrderDate.SelectedDate);
                DateTime EndDate = Convert.ToDateTime(HSRPAuthDate.SelectedDate);

                HSRPStateID = DropDownListStateName.SelectedValue;
                RTOLocationID = dropDownListClient.SelectedValue;
                string UID = Session["UID"].ToString();
                string OrderStatus = dropDownListorderStatus.SelectedItem.Text;


                //SQLString = "SELECT CONVERT (varchar(20),BillDate,103) as BillDate,ExpenseSaveID,(BillAmount-VerifiedAmount) as Balance,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,BillNo,BillAmount,Remarks,ExpenseStatus,VerifiedAmount,VerifiedRemarks,CONVERT (varchar(20),VerifiedDate,103) as VerifiedDate,(select (userfirstname +' '+ userlastname) as name  from users where userid='" + UID + "') as VerifiedBy FROM ExpenseSave INNER JOIN  RTOLocation ON ExpenseSave.LocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "')  and ExpenseSave.BillDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and ExpenseSave.ExpenseStatus='" + OrderStatus + "'";
                //SQLString = "SELECT CONVERT (varchar(20),BillDate,103) as BillDate,ExpenseSaveID,(BillAmount-VerifiedAmount) as Balance,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,BillNo,BillAmount,Remarks,ExpenseStatus,VerifiedAmount,VerifiedRemarks,CONVERT (varchar(20),VerifiedDate,103) as VerifiedDate,(select (userfirstname +' '+ userlastname) as name  from users where userid='" + UID + "') as VerifiedBy FROM ExpenseSave INNER JOIN  RTOLocation ON ExpenseSave.LocationID = RTOLocation.RTOLocationID where  RTOLocation.RTOLocationID='" + RTOLocationID + "' and ExpenseSave.BillDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and ExpenseSave.ExpenseStatus='" + OrderStatus + "'";
                SQLString = "SELECT CONVERT (varchar(20),BillDate,103) as BillDate,ExpenseSaveID,((BillAmount + isnull(VatAmount,0) + isnull(ServiceAmount,0) + isnull(ExciseAmount,0) + isnull(OtherAmount,0))) as Balance,(select ExpenseName from ExpenseMaster where ExpenceId=ExpenseId) as ExpenseName,BillNo,BillAmount,Remarks,ExpenseStatus,VerifiedAmount,VerifiedRemarks,CONVERT (varchar(20),VerifiedDate,103) as VerifiedDate,(select (userfirstname +' '+ userlastname) as name  from users where userid='" + UID + "') as VerifiedBy FROM ExpenseSave INNER JOIN  RTOLocation ON ExpenseSave.LocationID = RTOLocation.RTOLocationID where  RTOLocation.RTOLocationID='" + RTOLocationID + "' and ExpenseSave.BillDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' ";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                if (dt.Rows.Count > 0)
                {
                    DataView viewStartLocation = new DataView();
                    viewStartLocation = dt.DefaultView;
                    //viewStartLocation.RowFilter = "Status='Pending'";
                    if (OrderStatus == "Hold")
                    {
                        viewStartLocation.RowFilter = "Balance <> '0'";
                    }
                    else if (OrderStatus == "Pending")
                    {
                        viewStartLocation.RowFilter = "ExpenseStatus='Pending'";
                    }
                    else if (OrderStatus == "Approve")
                    {
                        viewStartLocation.RowFilter = "ExpenseStatus='Approve'";
                    }
                    Grid1.DataSource = viewStartLocation;
                    Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                    Grid1.SearchOnKeyPress = true;
                    Grid1.DataBind();
                    Grid1.RecordCount.ToString();
                }
                else
                {
                    Grid1.Items.Clear();
                    ClearGrid();
                }

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

       
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            dropDownListClient.Visible = true;
            labelClient.Visible = true;

        }
        private void ClearGrid()
        {
            Grid1.Items.Clear();
            lblErrMsg.Text = String.Empty;
            return;
        }

        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            buildGrid();

        }
    }
}