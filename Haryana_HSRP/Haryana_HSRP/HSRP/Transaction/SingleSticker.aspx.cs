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


namespace HSRP.Master
{
    public partial class SingleSticker : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        string trnasportname,pp;
        string transtr, statename1;
        BaseFont basefont;
        string fontpath;

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
                if (Session["UserHSRPStateID"].ToString() == "6")
                {
                    ButImpData.Visible = true;
                }
                else
                {
                    ButImpData.Visible = false;
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
                            hiddenUserType.Value = "1";
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
                  
                 dataLabellbl.Visible = false;
                 TRRTOHide.Visible = false; 
            }
            else
            {
                string UserID = Convert.ToString( Session["UID"]);
                 SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                 DataTable dss = Utils.GetDataTable(SQLString, CnnString); 
                 labelOrganization.Visible = true;
                 DropDownListStateName.Visible = true;
                 labelClient.Visible = true;
                 dropDownListClient.Visible = true; 

                 dropDownListClient.DataSource = dss;
                 dropDownListClient.DataBind();
                 dataLabellbl.Visible = true;

                string RTOCode = string.Empty;
                if (dss.Rows.Count > 0)
                {
                    for (int i = 0; i <= dss.Rows.Count - 1; i++)
                    {
                        RTOCode += dss.Rows[i]["RTOCode"].ToString();
                        
                    }
                    lblRTOCode.Text = RTOCode.Remove(RTOCode.LastIndexOf(","));
                    
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
                if (UserType == "0")
                {
                    
                     SQLString = "SELECT CONVERT (varchar(20),HSRPRecord_AuthorizationDate+4,103) as Duedate,  CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecord_AuthorizationNo, HSRPRecords.OwnerName, HSRPRecords.VehicleRegNo, HSRPRecords.OrderStatus, HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.HSRPRecordID, RTOInventory.LaserPlateBoxNo FROM HSRPRecords INNER JOIN RTOInventory ON HSRPRecords.HSRPRecordID = RTOInventory.HSRPRecordID where HSRPRecords.RTOLocationID=" + RTOLocationID;

                }
                else
                {
                    
                     SQLString = "SELECT CONVERT (varchar(20),HSRPRecord_AuthorizationDate+4,103) as Duedate,  CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecord_AuthorizationNo, HSRPRecords.OwnerName, HSRPRecords.VehicleRegNo, HSRPRecords.OrderStatus, HSRPRecords.HSRP_Front_LaserCode, HSRPRecords.HSRP_Rear_LaserCode, HSRPRecords.HSRPRecordID, RTOInventory.LaserPlateBoxNo FROM HSRPRecords INNER JOIN RTOInventory ON HSRPRecords.HSRPRecordID = RTOInventory.HSRPRecordID where HSRPRecords.RTOLocationID=" + RTOLocationID+" and OrderStatus='New Order'";
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
            ClearGrid();
            Grid1.Items.Clear();

           String[] StringAuthDate =  OrderDate.SelectedDate.ToString().Split('/');
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
            string OrderDate1= From1 + " 23:59:59";

            DateTime StartDate = Convert.ToDateTime( OrderDate.SelectedDate);
            DateTime EndDate = Convert.ToDateTime( HSRPAuthDate.SelectedDate);

            HSRPStateID = DropDownListStateName.SelectedValue;
            RTOLocationID = dropDownListClient.SelectedValue;
            string UID = Session["UID"].ToString();
            string OrderStatus = dropDownListorderStatus.SelectedItem.Text;

            if (RadioButtonCreateion.Checked == true)
            {
               // if (OrderStatus == "New Order")
              //  {
                    SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_CreationDate + 4, 103) AS Duedate,HSRPRecords.MobileNo,convert(varchar, HSRPRecords.HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, HSRPRecords.OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, HSRPRecords.OrderEmbossingDate, 105) as OrderEmbossingDate, CONVERT(varchar(20),  HSRPRecord_CreationDate,103) AS HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "') and HSRPRecords.HSRPRecord_CreationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and HSRPRecords.OrderStatus='" + OrderStatus + "'";
              //  }
                //else if (OrderStatus == "Closed")
                //{
                //    SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_CreationDate + 4, 103) AS Duedate,HSRPRecords.MobileNo,convert(varchar, HSRPRecords.HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, HSRPRecords.OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, HSRPRecords.OrderEmbossingDate, 105) as OrderEmbossingDate, CONVERT(varchar(20),  HSRPRecord_CreationDate,103) AS HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "')  and HSRPRecords.ordercloseddate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' ";
                //}
                //else
                //{
                //    SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_CreationDate + 4, 103) AS Duedate,HSRPRecords.MobileNo,convert(varchar, HSRPRecords.HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, HSRPRecords.OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, HSRPRecords.OrderEmbossingDate, 105) as OrderEmbossingDate, CONVERT(varchar(20),  HSRPRecord_CreationDate,103) AS HSRPRecord_AuthorizationDate, HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "')  and HSRPRecords.orderembossingdate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and HSRPRecords.OrderStatus='" + OrderStatus + "'";
                //}
            }
            if (radiobuttonAuthorization.Checked == true)
            {
                //if (OrderStatus == "New Order")
               // {
                    SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_AuthorizationDate + 4, 103) AS Duedate, CONVERT(varchar(20), HSRPRecords.HSRPRecord_AuthorizationDate,103) AS HSRPRecord_AuthorizationDate,HSRPRecords.MobileNo,  convert(varchar, HSRPRecords.OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "') and HSRPRecords.HSRPRecord_AuthorizationDate between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and HSRPRecords.OrderStatus='" + OrderStatus + "'";
              //  }
                //else
               // {
                //    SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_AuthorizationDate + 4, 103) AS Duedate, CONVERT(varchar(20), HSRPRecords.HSRPRecord_AuthorizationDate,103) AS HSRPRecord_AuthorizationDate,HSRPRecords.MobileNo,  convert(varchar, HSRPRecords.OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "') and HSRPRecords.HSRPRecord_AuthorizationDate between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and HSRPRecords.OrderStatus='" + OrderStatus + "'";
                //}
            }
            if (radiobuttonOrderClose.Checked == true)
            {
               // if (OrderStatus == "New Order")
              //  {
                    SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_AuthorizationDate + 4, 103) AS Duedate, CONVERT(varchar(20), HSRPRecords.HSRPRecord_AuthorizationDate,103) AS HSRPRecord_AuthorizationDate,HSRPRecords.MobileNo, HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "') and HSRPRecords.ordercloseddate between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and HSRPRecords.OrderStatus='" + OrderStatus + "'";
              //  }
               // else
              //  {
                 //   SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_AuthorizationDate + 4, 103) AS Duedate, CONVERT(varchar(20), HSRPRecords.HSRPRecord_AuthorizationDate,103) AS HSRPRecord_AuthorizationDate,HSRPRecords.MobileNo, HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "') and HSRPRecords.ordercloseddate between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and HSRPRecords.OrderStatus='" + OrderStatus + "'";
               // }
            }
            if (RadioButtonEmbossingDate.Checked == true)
            {
                // if (OrderStatus == "New Order")
                //  {
                SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_AuthorizationDate + 4, 103) AS Duedate, CONVERT(varchar(20),"+
                    " HSRPRecords.HSRPRecord_AuthorizationDate,103) AS HSRPRecord_AuthorizationDate,HSRPRecords.MobileNo, HSRPRecords.HSRPRecordID,"+
                    "HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo,"+
                    " CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime,"+
                    " convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,"+
                    "HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" +
                    RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "') and HSRPRecords.orderembossingdate between '" + AuthorizationDate + "' and '" +
                    OrderDate1 + "' and HSRPRecords.OrderStatus='" + OrderStatus + "'";
                //  }
                // else
                //  {
                //   SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_AuthorizationDate + 4, 103) AS Duedate, CONVERT(varchar(20), HSRPRecords.HSRPRecord_AuthorizationDate,103) AS HSRPRecord_AuthorizationDate,HSRPRecords.MobileNo, HSRPRecords.HSRPRecordID,HSRPRecords.HSRPRecord_AuthorizationNo,HSRPRecords.OwnerName,HSRPRecords.VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate,HSRPRecords.OrderStatus,HSRPRecords.HSRP_Front_LaserCode,HSRPRecords.HSRP_Rear_LaserCode,HSRPRecords.LaserPlateBoxNo  FROM HSRPRecords INNER JOIN  RTOLocation ON HSRPRecords.RTOLocationID = RTOLocation.RTOLocationID where (RTOLocation.distRelation='" + RTOLocationID + "' or RTOLocation.RTOLocationID='" + RTOLocationID + "') and HSRPRecords.ordercloseddate between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and HSRPRecords.OrderStatus='" + OrderStatus + "'";
                // }
            }

            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
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
                ClearGrid();
            }
           
        }

       public void SAVEStickerLog( string vehicleRegNo, string HSRPRecordID)
       {
           SQLString = "insert into stickerLog (vehicleRegNo, hsrprecordID, userID) values ('" + vehicleRegNo + "','" + HSRPRecordID + "','" + Session["UID"] + "')";
           Utils.ExecNonQuery(SQLString, CnnString);
           
       }

       protected void ButImpData_Click(object sender, EventArgs e)
       {
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
           

           string getDataQry = string.Empty;
           if (RadioButtonCreateion.Checked == true)
           {
               getDataQry = "select b.NICVehicleRegNo,b.ChassisNo,b.EngineNo,b.OrderType,b.NICVehicleType,b.OwnerName,b.Manufacturer,b.HSRPRecord_AuthorizationNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,CONVERT(VARCHAR(10), a.OrderClosedDate, 103) AS AffixationDate,a.TotalAmount,CONVERT(VARCHAR(10), a.OrderClosedDate, 103) AS CashReceiptDateTime from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and a.HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and a.RTOLocationID='" + dropDownListClient.SelectedValue + "' and a.HSRPRecord_CreationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N'";
           }
           if (radiobuttonAuthorization.Checked == true)
           {
               getDataQry = "select b.NICVehicleRegNo,b.ChassisNo,b.EngineNo,b.OrderType,b.NICVehicleType,b.OwnerName,b.Manufacturer,b.HSRPRecord_AuthorizationNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,CONVERT(VARCHAR(10), a.OrderClosedDate, 103) AS AffixationDate,a.TotalAmount,CONVERT(VARCHAR(10), a.OrderClosedDate, 103) AS CashReceiptDateTime from HSRPRecords a,HSRPRecordsStaggingArea b where a.VehicleRegNo=b.VehicleRegNo and a.HSRP_StateID='" + DropDownListStateName.SelectedValue + "' and a.RTOLocationID='" + dropDownListClient.SelectedValue + "' and a.HSRPRecord_AuthorizationDate between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='Closed' and a.SendRecordToNic='N'";
           }

           
           
           DataTable dt = Utils.GetDataTable(getDataQry, CnnString);
            
           if (dt.Rows.Count > 0)
           {     
               string fileName = ConfigurationManager.AppSettings["DataFolder"].ToString() + "HSRPClosedRecord-" + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".dat";
               CreateCSVFile(dt, fileName);

           }
           else
           {
                
           }  
       }

       #region Export Grid to CSV
       public void CreateCSVFile(DataTable dt, string strFilePath)
       { 
           StreamWriter sw = new StreamWriter(strFilePath, false); 

           int iColCount = dt.Columns.Count; 

           foreach (DataRow dr in dt.Rows)
           {

               for (int i = 0; i < iColCount; i++)
               {
                   if (!Convert.IsDBNull(dr[i]))
                   {
                       sw.Write(dr[i].ToString());
                   }

                   if (i < iColCount - 1)
                   {
                       sw.Write(";");
                   }
               }

               sw.Write(sw.NewLine);

           }

           sw.Close();
            

           System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
           response.ClearContent();
           response.Clear();
           response.ContentType = "text/plain";
           response.AddHeader("Content-Disposition", "attachment; filename=" + strFilePath + ";");
           response.TransmitFile(strFilePath);
           response.Flush();
           response.End(); 
       }
       #endregion


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

        protected void dropDownListClient_SelectedIndexChanged1(object sender, EventArgs e)
        {
           
        }

        protected void Grid1_ItemCommand1(object sender, ComponentArt.Web.UI.GridItemCommandEventArgs e)
        {
            if (e.Control.ID == "LinkButtonSticker")
            {

                String UserID = e.Item["HSRPRecordID"].ToString();
                string OrderStatus = e.Item["OrderStatus"].ToString();
                if (OrderStatus == "New Order")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Laser code is not assign');</script>");

                }
                else
                {


                    LinkButton lnkInvc = (LinkButton)Grid1.FindControl("LinkButtonSticker");
                    string str = lnkInvc.CommandName.ToString();
                    SQLString = " select a.HSRPRecordID,a.vehicleRegNo, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID where  HSRPRecordID='" + UserID + "' and  a.HSRP_Front_LaserCode is not null and a.HSRP_Front_LaserCode!=''";
                    DataTable ds = Utils.GetDataTable(SQLString, CnnString);
                    if (ds.Rows.Count > 0)
                    {
                        string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                        if (Stricker == "Y")
                        {
                            string filename = "Sticker" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";


                            String StringField = String.Empty;
                            String StringAlert = String.Empty;

                            StringBuilder bb = new StringBuilder();

                            Document document = new Document(PageSize.A4, 0, 0, 212, 0);
                            document.SetPageSize(new iTextSharp.text.Rectangle(500, 400));
                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));



                            //Opens the document:
                            document.Open();

                            PdfPTable table = new PdfPTable(1);

                            table.TotalWidth = 300f;

                            StringBuilder sbtrnasportname = new StringBuilder();
                            trnasportname = "TRANSPORT DEPARTMENT";


                            PdfPCell cell6 = new PdfPCell(new Phrase(trnasportname, new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell6.PaddingTop = 8f;
                            cell6.BorderColor = BaseColor.WHITE;
                            cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell6);
                            StringBuilder sb = new StringBuilder();
                            StringBuilder sb1 = new StringBuilder();


                            string statename = ds.Rows[0]["statetext"].ToString().ToUpper();
                            string HSRPStateName = ds.Rows[0]["HSRPStateName"].ToString().ToUpper();
                            if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }

                            else if (HSRPStateName == "HARYANA")
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }
                            else
                            {

                                PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 20f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }


                            StringBuilder sbVehicleRegNo = new StringBuilder();

                            string VehicleRegNo = ds.Rows[0]["VehicleRegNo"].ToString().ToUpper();



                            PdfPCell cell2 = new PdfPCell(new Phrase(VehicleRegNo, new iTextSharp.text.Font(basefont, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell2.PaddingTop = 8f;
                            cell2.BorderColor = BaseColor.WHITE;
                            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell2);
                            StringBuilder sbHSRP_Front_LaserCode = new StringBuilder();
                            StringBuilder sbHSRP_Rear_LaserCode = new StringBuilder();



                            string HSRP_Front_LaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString().ToUpper();
                            string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();
                            PdfPCell cell3 = new PdfPCell(new Phrase("" + HSRP_Front_LaserCode + " - " + HSRP_Rear_LaserCode, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell3.PaddingTop = 8f;
                            cell3.PaddingLeft = 193f;
                            cell3.BorderColor = BaseColor.WHITE;
                            cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell3);

                            StringBuilder sbEngineNo = new StringBuilder();
                            string EngineNo = "ENGINE NO - " + ds.Rows[0]["EngineNo"].ToString().ToUpper();


                            PdfPCell cell4 = new PdfPCell(new Phrase(EngineNo, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell4.PaddingTop = 8f;
                            cell4.PaddingLeft = 193f;
                            cell4.BorderColor = BaseColor.WHITE;
                            cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell4);

                            StringBuilder sbChassisNo = new StringBuilder();
                            string ChassisNo = "CHASIS NO - " + ds.Rows[0]["ChassisNo"].ToString().ToUpper();
                            PdfPCell cell5 = new PdfPCell(new Phrase(ChassisNo, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell5.PaddingTop = 8f;
                            cell5.PaddingLeft = 193f;
                            cell5.BorderColor = BaseColor.WHITE;
                            cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell5);

                            document.Add(table);

                            document.Close();

                            SAVEStickerLog(ds.Rows[0]["vehicleRegNo"].ToString().ToUpper().Trim(), ds.Rows[0]["HSRPRecordID"].ToString());

                            HttpContext context = HttpContext.Current;

                            context.Response.ContentType = "Application/pdf";
                            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                            context.Response.WriteFile(PdfFolder);
                            context.Response.End();
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                        }
                    }
                }
            }


            //string OrderStatus = e.Item["OrderStatus"].ToString();
            if (e.Control.ID == "LinkButtonStickerTVS")
            {
                String UserID = e.Item["HSRPRecordID"].ToString();
                string OrderStatus = e.Item["OrderStatus"].ToString();
                if (OrderStatus == "New Order")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Laser code is not assign');</script>");

                }
                else
                {
                    LinkButton lnkInvc = (LinkButton)Grid1.FindControl("LinkButtonSticker");
                    string str = lnkInvc.CommandName.ToString();
                    SQLString = " select a.HSRPRecordID,a.vehicleRegNo,  a.HSRP_Sticker_LaserCode,a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID where  HSRPRecordID='" + UserID + "' and  a.HSRP_Front_LaserCode is not null and a.HSRP_Front_LaserCode!=''";
                    DataTable ds = Utils.GetDataTable(SQLString, CnnString);
                    if (ds.Rows.Count > 0)
                    {
                        string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                        if (Stricker == "Y")
                        {
                            string filename = "Sticker" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                             
                            String StringField = String.Empty;
                            String StringAlert = String.Empty;

                            StringBuilder bb = new StringBuilder();

                            Document document = new Document(PageSize.A4, 0, 0, 220, 0);

                            document.SetPageSize(new iTextSharp.text.Rectangle(400, 300));
                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                            string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                            PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));



                            //Opens the document:
                            document.Open();

                            //Adds content to the document:
                            // document.Add(new Paragraph("Ignition Log Report"));
                            PdfPTable table = new PdfPTable(1);
                            //actual width of table in points
                            table.TotalWidth = 300f;

                            //iTextSharp.text.Font ft = new iTextSharp.text.Font();
                            //FontFactory.Register(@"../bin/PRSANSR.TTF", "PRMirror");
                            //ft = FontFactory.GetFont("PRMirror");


                            // fontpath = Environment.GetEnvironmentVariable("http://localhost:51047") + "C:\\Users\\Ambrish Singh\\Desktop\\ambrish\\HSRPViewOrder new printer according sticker\\HSRP\\bin\\PRSANSR.TTF";

                            //fontpath = ConfigurationManager.AppSettings["DataFolder"].ToString() + "PRSANSR.TTF";
                            fontpath = ConfigurationManager.AppSettings["DataFolder"].ToString();
                            basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);

                            StringBuilder sbtrnasportname = new StringBuilder();
                            trnasportname = "TRANSPORT DEPARTMENT";

                            for (int i = trnasportname.Length - 1; i >= 0; i--)
                            {
                                sbtrnasportname.Append(trnasportname[i].ToString());
                            }

                            //fix the absolute width of the table
                            PdfPCell cell6 = new PdfPCell(new Phrase(sbtrnasportname.ToString(), new iTextSharp.text.Font(basefont, 17f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell6.PaddingRight = 240f;
                            cell6.BorderColor = BaseColor.WHITE;
                            cell6.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell6);
                            StringBuilder sb = new StringBuilder();
                            StringBuilder sb1 = new StringBuilder();
                            string statename = ds.Rows[0]["statetext"].ToString().ToUpper();

                            for (int i = statename.Length - 1; i >= 0; i--)
                            {
                                sb.Append(statename[i].ToString());
                            }

                            string HSRPStateName = ds.Rows[0]["HSRPStateName"].ToString().ToUpper();

                            for (int i = HSRPStateName.Length - 1; i >= 0; i--)
                            {
                                sb1.Append(HSRPStateName[i].ToString());
                            }
                            if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                cell.PaddingRight = 240f;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }
                            else if (HSRPStateName == "HARYANA")
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                cell.PaddingRight = 243f;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 18f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                // cell.Colspan = 4;
                                //cell.PaddingRight = 240f;
                                cell.PaddingTop = 8f;
                                cell.BorderColor = BaseColor.WHITE;
                                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            } 
                            StringBuilder sbVehicleRegNo = new StringBuilder();

                            string VehicleRegNo = ds.Rows[0]["VehicleRegNo"].ToString().ToUpper();

                            for (int i = VehicleRegNo.Length - 1; i >= 0; i--)
                            {
                                sbVehicleRegNo.Append(VehicleRegNo[i].ToString());
                            }

                            PdfPCell cell2 = new PdfPCell(new Phrase(sbVehicleRegNo.ToString(), new iTextSharp.text.Font(basefont, 27f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell2.PaddingTop = 8f;
                            cell2.BorderColor = BaseColor.WHITE;
                            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell2);
                            StringBuilder sbHSRP_Front_LaserCode = new StringBuilder();
                            StringBuilder sbHSRP_Rear_LaserCode = new StringBuilder();
                            string HSRP_Front_LaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString().ToUpper();

                            for (int i = HSRP_Front_LaserCode.Length - 1; i >= 0; i--)
                            {
                                sbHSRP_Front_LaserCode.Append(HSRP_Front_LaserCode[i].ToString());
                            }

                            string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();

                            for (int i = HSRP_Rear_LaserCode.Length - 1; i >= 0; i--)
                            {
                                sbHSRP_Rear_LaserCode.Append(HSRP_Rear_LaserCode[i].ToString());
                            }

                            PdfPCell cell3 = new PdfPCell(new Phrase("" + sbHSRP_Rear_LaserCode.ToString() + " - " + sbHSRP_Front_LaserCode.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell3.PaddingTop = 8f;
                            cell3.BorderColor = BaseColor.WHITE;
                            cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell3);

                            StringBuilder sbEngineNo = new StringBuilder();
                            string EngineNo = "ENGINE NO - " + ds.Rows[0]["EngineNo"].ToString().ToUpper();

                            for (int i = EngineNo.Length - 1; i >= 0; i--)
                            {
                                sbEngineNo.Append(EngineNo[i].ToString());
                            }


                            PdfPCell cell4 = new PdfPCell(new Phrase(sbEngineNo.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell4.PaddingTop = 8f;
                            cell4.PaddingRight = 245f;
                            cell4.BorderColor = BaseColor.WHITE;
                            cell4.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell4);

                            StringBuilder sbChassisNo = new StringBuilder();
                            string ChassisNo = "CHASIS NO - " + ds.Rows[0]["ChassisNo"].ToString().ToUpper();

                            for (int i = ChassisNo.Length - 1; i >= 0; i--)
                            {
                                sbChassisNo.Append(ChassisNo[i].ToString());
                            }

                            PdfPCell cell5 = new PdfPCell(new Phrase(sbChassisNo.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                            // cell.Colspan = 4;
                            cell5.PaddingTop = 8f;
                            cell5.PaddingRight = 245f;
                            cell5.BorderColor = BaseColor.WHITE;
                            cell5.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell5);

                            document.Add(table);

                            document.Close();

                            SAVEStickerLog(ds.Rows[0]["vehicleRegNo"].ToString().ToUpper().Trim(), ds.Rows[0]["HSRPRecordID"].ToString());

                            HttpContext context = HttpContext.Current;

                            context.Response.ContentType = "Application/pdf";
                            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                            context.Response.WriteFile(PdfFolder);
                            context.Response.End();
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                        }
                    }
                }
            }

        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //LabelError.Visible = false;
                String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');  
                string MonTo = ("0" + StringAuthDate[0]);
                string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
                String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0]; 
                String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
                //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
                string AuthorizationDate = From + " 00:00:00"; 

                String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
                string Mon = ("0" + StringOrderDate[0]);
                string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
                String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
                String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
                string OrderDate1 = From1 + " 23:59:59";
                string UID = Session["UID"].ToString();
                HSRPStateID = DropDownListStateName.SelectedValue;
                RTOLocationID = dropDownListClient.SelectedValue;
                 string OrderStatus = dropDownListorderStatus.SelectedItem.Text;
                 

                if (RadioButtonCreateion.Checked == true)
                 {
                     //if (OrderStatus=="New Order")
                     //{
                         SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, a.OrderStatus, CONVERT(varchar(20),HSRPRecord_CreationDate ,103) AS OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize,a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "') and a.HSRPRecord_CreationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                     //}
                     //else
                     //{
                     //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, a.OrderStatus, CONVERT(varchar(20),HSRPRecord_CreationDate ,103) AS OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize,a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "')   and a.OrderEmbossingDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                     //}
                 }

                if (RadioButtonEmbossingDate.Checked == true)
                {
                    //if (OrderStatus=="New Order")
                    //{
                    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, a.OrderStatus, CONVERT(varchar(20),HSRPRecord_CreationDate ,103) AS OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize,a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "') and a.OrderEmbossingDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                    //}
                    //else
                    //{
                    //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, a.OrderStatus, CONVERT(varchar(20),HSRPRecord_CreationDate ,103) AS OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize,a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "')   and a.OrderEmbossingDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                    //}
                }
                 if (radiobuttonAuthorization.Checked == true)
                 {
                     //if (OrderStatus == "New Order")
                     //{
                         SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "')  and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                     //}
                     //else
                     //{
                     //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "')  and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                     //}
                 } 
                 if (radiobuttonOrderClose.Checked == true)
                 {
                     //if (OrderStatus == "New Order")
                     //{
                         SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "' and a.ordercloseddate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                     //}
                     //else
                     //{
                     //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "'  and a.ordercloseddate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                     //}
                 }
                 if (checkboxAll.Checked == true)
                 {
                     if (DropDownListStateName.SelectedValue != "--Select State--")
                     {
                         if (UserType == "0")
                         {
                             SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                         }
                         else
                         {
                            SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+',') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";
                         }
                          
                         DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                         if (dss.Rows.Count > 0)
                         {
                             for (int i = 0; i <= dss.Rows.Count - 1; i++)
                             {
                                 AllLocation += dss.Rows[i]["RTOLocationID"].ToString() + ",";
                             }
                             AllLocation = AllLocation.Remove(AllLocation.LastIndexOf(",")); 
                         }
                         if (RadioButtonCreateion.Checked == true)
                         {
                             //if (OrderStatus == "New Order")
                             //{
                                 SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID in (" + AllLocation + ") and  a.HSRPRecord_CreationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'  order by b.RTOLocationID";
                             //}
                             //else
                             //{
                             //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID in (" + AllLocation + ")  and  a.HSRPRecord_CreationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'  order by b.RTOLocationID";
                             //}
                         }
                         if (radiobuttonAuthorization.Checked == true)
                         {
                             //if (OrderStatus == "New Order")
                             //{
                                 //SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ") and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                                 SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ") and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                             //}
                             //else
                             //{
                             //    SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ")  and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                             //}
                         }
                         if (RadioButtonEmbossingDate.Checked == true)
                         {
                             //if (OrderStatus == "New Order")
                             //{
                             //SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ") and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                             SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ") and a.OrderEmbossingDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                             //}
                             //else
                             //{
                             //    SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ")  and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                             //}
                         }
                     }

                 }
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                if (dt.Rows.Count > 0)
                { 
                    string filename = "DailyAssignEmbossing-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".xls";
                    Workbook book = new Workbook(); 
                    // Specify which Sheet should be opened and the size of window by default
                    book.ExcelWorkbook.ActiveSheetIndex = 1;
                    book.ExcelWorkbook.WindowTopX = 100;
                    book.ExcelWorkbook.WindowTopY = 200;
                    book.ExcelWorkbook.WindowHeight = 7000;
                    book.ExcelWorkbook.WindowWidth = 8000;

                    // Some optional properties of the Document
                    book.Properties.Author = "HSRP";
                    book.Properties.Title = "Daily Assign Embossing Report";
                    book.Properties.Created = DateTime.Now;


                    // Add some styles to the Workbook
                    WorksheetStyle style = book.Styles.Add("HeaderStyle");
                    style.Font.FontName = "Tahoma";
                    style.Font.Size = 10;
                    style.Font.Bold = true;
                    style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style6 = book.Styles.Add("HeaderStyle6");
                    style6.Font.FontName = "Tahoma";
                    style6.Font.Size = 10;
                    style6.Font.Bold = false;
                    style6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                    style6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

                    WorksheetStyle style7 = book.Styles.Add("HeaderStyle7");
                    style7.Font.FontName = "Tahoma";
                    style7.Font.Size = 10;
                    style7.Font.Bold = false;
                    style7.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                    style7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                    style7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

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

                    Worksheet sheet = book.Worksheets.Add("HSRP Daily Assign Embossing Report");

                    sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(110));
                    sheet.Table.Columns.Add(new WorksheetColumn(90));
                    sheet.Table.Columns.Add(new WorksheetColumn(115));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(130));
                    sheet.Table.Columns.Add(new WorksheetColumn(120)); 
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(160));
                    sheet.Table.Columns.Add(new WorksheetColumn(120)); 
                    WorksheetRow row = sheet.Table.Rows.Add();
                    // row.
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle3"));
                    row.Cells.Add(new WorksheetCell("Report :", "HeaderStyle3"));
                    WorksheetCell cell = row.Cells.Add("HSRP Daily Assign Embossing Report");
                    cell.MergeAcross = 3; // Merge two cells together
                    cell.StyleID = "HeaderStyle3";

                    //row.Cells.Add(<br>);
                    row = sheet.Table.Rows.Add();
                    row.Index = 3;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("State:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(DropDownListStateName.SelectedItem.ToString(), "HeaderStyle2"));

                    row = sheet.Table.Rows.Add();

                    row.Index = 4;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Location:", "HeaderStyle2"));
                    if (checkboxAll.Checked == true)
                    {
                        row.Cells.Add(new WorksheetCell("All Location:", "HeaderStyle2"));
                    }
                    else
                    {
                        row.Cells.Add(new WorksheetCell(dropDownListClient.SelectedItem.ToString(), "HeaderStyle2"));
                    } 
                    //row.Cells.Add(<br>); 
                    row = sheet.Table.Rows.Add();

                    // Skip one row, and add some text
                    row.Index = 5;
                    DateTime date = System.DateTime.Now;
                    string formatted = date.ToString("dd/MM/yyyy");

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Date Generated :", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(formatted, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 6;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date From:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(ReportDateFrom, "HeaderStyle2")); 
                    row = sheet.Table.Rows.Add();
                    row.Index = 7;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Report Date TO:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(ReportDateTo, "HeaderStyle2"));
                    row = sheet.Table.Rows.Add();
                    row.Index = 8;
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell("Order Status:", "HeaderStyle2"));
                    row.Cells.Add(new WorksheetCell(OrderStatus, "HeaderStyle2"));
                    //row.Cells.Add(<br>); 
                    row = sheet.Table.Rows.Add();


                    row.Index = 10;
                    //row.Cells.Add("Order Date");
                    row.Cells.Add(new WorksheetCell("S.No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Vehicle No", "HeaderStyle"));
                    if (RadioButtonCreateion.Checked == true)
                    {
                        row.Cells.Add(new WorksheetCell(" Creation Date", "HeaderStyle"));
                    }
                    if (radiobuttonAuthorization.Checked==true)
                    {
                    row.Cells.Add(new WorksheetCell("Authorization Date", "HeaderStyle"));
                    }
                    row.Cells.Add(new WorksheetCell("Vehicle Class", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Vehicle Type", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Front Plate Size", "HeaderStyle")); 
                    row.Cells.Add(new WorksheetCell("Front Laser Plate No.", "HeaderStyle")); 
                    row.Cells.Add(new WorksheetCell("Rear Plate Size", "HeaderStyle")); 
                    row.Cells.Add(new WorksheetCell("Rear Laser Plate No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Chassis No.", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Engine No.", "HeaderStyle")); 
                    row.Cells.Add(new WorksheetCell("Owner Name", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Mobile No.", "HeaderStyle"));

                    if (OrderStatus == "New Order")
                    {
                        row.Cells.Add(new WorksheetCell("Order Date", "HeaderStyle"));
                    }
                    if (OrderStatus == "Embossing Done")
                    {
                        row.Cells.Add(new WorksheetCell("Embossing Date", "HeaderStyle"));
                    }
                    if (OrderStatus == "Closed")
                    {
                        row.Cells.Add(new WorksheetCell("Closed Date", "HeaderStyle"));
                    }

                    row = sheet.Table.Rows.Add();

                    String StringField = String.Empty;
                    String StringAlert = String.Empty;


                    if (dt.Rows.Count <= 0)
                    { 
                        string closescript1 = "<script>alert('No records found for selected Sate.')</script>";
                        Page.RegisterStartupScript("abc", closescript1);
                        return;
                    }
                    row.Index = 11; 
                    int sno = 0;

                    foreach (DataRow dtrows in dt.Rows) // Loop over the rows.
                    {
                        sno = sno + 1;
                        row = sheet.Table.Rows.Add();
                        row.Cells.Add(new WorksheetCell(Convert.ToInt16(sno).ToString(), DataType.String, "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleRegNo"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["OrderDate"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleClass"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["VehicleType"].ToString(), DataType.String, "HeaderStyle6"));

                        row.Cells.Add(new WorksheetCell(dtrows["FrontProductCode"].ToString(), DataType.String, "HeaderStyle6"));

                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Front_LaserCode"].ToString(), DataType.String, "HeaderStyle6"));

                        row.Cells.Add(new WorksheetCell(dtrows["RearProductCode"].ToString(), DataType.String, "HeaderStyle6"));
                        row.Cells.Add(new WorksheetCell(dtrows["HSRP_Rear_LaserCode"].ToString(), DataType.String, "HeaderStyle6"));

                        row.Cells.Add(new WorksheetCell(dtrows["ChassisNo"].ToString(), DataType.String, "HeaderStyle7"));
                        row.Cells.Add(new WorksheetCell(dtrows["EngineNo"].ToString(), DataType.String, "HeaderStyle7"));

                        row.Cells.Add(new WorksheetCell(dtrows["OwnerName"].ToString(), DataType.String, "HeaderStyle7"));
                        row.Cells.Add(new WorksheetCell(dtrows["MobileNo"].ToString(), DataType.String, "HeaderStyle6"));

                        if (OrderStatus == "New Order")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["OrderBookDate"].ToString(), DataType.String, "HeaderStyle6"));
                        }
                        if (OrderStatus == "Embossing Done")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["OrderEmbossingDate"].ToString(), DataType.String, "HeaderStyle6"));
                        }
                        if (OrderStatus == "Closed")
                        {
                            row.Cells.Add(new WorksheetCell(dtrows["OrderClosedDate"].ToString(), DataType.String, "HeaderStyle6"));
                        } 

                    }
                    row = sheet.Table.Rows.Add();

                    row.Cells.Add(new WorksheetCell("", "HeaderStyle2"));

                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();


                    // Save the file and open it
                    book.Save(Response.OutputStream);

                    //context.Response.ContentType = "text/csv";
                    context.Response.ContentType = "application/vnd.ms-excel";

                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.End();
                }
                else
                {
                    string closescript1 = "<script>alert('No records found for selected date.')</script>";
                    Page.RegisterStartupScript("abc", closescript1);
                    return;
                }
            }

            catch (Exception ex)
            {
                //LabelError.Visible = true;
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void btnExportToPDF_Click(object sender, EventArgs e)
        {
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00"; 

            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
           // OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            string OrderDate1 = From1 + " 23:59:59";

            string UID = Session["UID"].ToString();

            HSRPStateID = DropDownListStateName.SelectedValue;
            RTOLocationID = dropDownListClient.SelectedValue;
            string OrderStatus = dropDownListorderStatus.SelectedItem.Text;

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            int i = 0;

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

           
            //OrderStatus = dropDownListorderStatus.SelectedItem.Text;
            if (RadioButtonCreateion.Checked == true)
            {
                //if (OrderStatus == "New Order")
                //{
                    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, a.OrderStatus, CONVERT(varchar(20),HSRPRecord_CreationDate ,103) AS OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize,a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "') and a.HSRPRecord_CreationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //}
                //else
                //{
                //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, a.OrderStatus, CONVERT(varchar(20),HSRPRecord_CreationDate ,103) AS OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize,a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "')  and a.orderembossingdate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //}
            }
            if (radiobuttonAuthorization.Checked == true)
            {
                //if (OrderStatus == "New Order")
                //{
                    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "')  and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //}
                //else
                //{
                //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  (b.distRelation='" + RTOLocationID + "' or b.RTOLocationID='" + RTOLocationID + "')  and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //}
            }
            if (radiobuttonOrderClose.Checked == true)
            {
                //if (OrderStatus == "New Order")
                //{
                    //SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "' and a.InvoiceDateTime  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "' and a.ordercloseddate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //}
                //else
                //{
                //    //SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "'  and a.InvoiceDateTime  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "'  and a.ordercloseddate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //}
            }
            if (RadioButtonEmbossingDate.Checked == true)
            {
                //if (OrderStatus == "New Order")
                //{
                //SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "' and a.InvoiceDateTime  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "' and a.OrderEmbossingDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //}
                //else
                //{
                //    //SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "'  and a.InvoiceDateTime  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //    SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.FrontPlateSize, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID='" + RTOLocationID + "'  and a.ordercloseddate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                //}
            }
            if (checkboxAll.Checked == true)
            {
                if (DropDownListStateName.SelectedValue != "--Select State--")
                {
                    if (UserType == "0")
                    {
                        SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                    }
                    else
                    {
                        SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+',') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + strUserID + "' ";
                    }

                    DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                    if (dss.Rows.Count > 0)
                    {
                        for (int j = 0; j <= dss.Rows.Count - 1; j++)
                        {
                            AllLocation += dss.Rows[j]["RTOLocationID"].ToString() + ",";
                        }
                        AllLocation = AllLocation.Remove(AllLocation.LastIndexOf(","));
                    }
                    if (RadioButtonCreateion.Checked == true)
                    {
                        if (OrderStatus == "New Order")
                        {
                            SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID in (" + AllLocation + ") and  a.HSRPRecord_CreationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'  order by b.RTOLocationID";
                        }
                        else
                        {
                            SQLString = "Select a.HSRPRecord_AuthorizationNo,convert(varchar, OrderClosedDate, 105) as OrderClosedDate, CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID in (" + AllLocation + ")  and  a.HSRPRecord_CreationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'  order by b.RTOLocationID";
                        }
                    }
                    if (radiobuttonAuthorization.Checked == true)
                    {
                        if (OrderStatus == "New Order")
                        {
                            SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ") and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                        }
                        else
                        {
                            SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ")    and a.HSRPRecord_AuthorizationDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                        }
                    }
                    if (RadioButtonEmbossingDate.Checked == true)
                    {
                        if (OrderStatus == "New Order")
                        {
                            SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ") and a.OrderEmbossingDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                        }
                        else
                        {
                            SQLString = "Select a.HSRPRecord_AuthorizationNo, convert(varchar, OrderClosedDate, 105) as OrderClosedDate,CONVERT(varchar(20),orderdate ,103) AS OrderBookDate,CONVERT(varchar(20),OrderEmbossingDate ,103) AS OrderEmbossingDate, a.OwnerName, a.MobileNo, a.HSRPRecordID, CONVERT(varchar(20), HSRPRecord_AuthorizationDate,103) AS OrderDate, a.OrderDate, a.EngineNo, a.ChassisNo, b.RTOLocationName,  a.VehicleRegNo, a.VehicleType, a.VehicleClass, a.OwnerName AS Expr1, a.HSRP_StateID, s.HSRPStateName, Product_1.ProductCode AS FrontProductCode, Product.ProductCode AS RearProductCode, a.HSRP_Front_LaserCode, a.HSRP_Rear_LaserCode, a.FrontPlateSize, a.RearPlateSize FROM HSRPRecords AS a INNER JOIN HSRPState AS s ON a.HSRP_StateID = s.HSRP_StateID INNER JOIN Product ON a.FrontPlateSize = Product.ProductID INNER JOIN Product AS Product_1 ON a.RearPlateSize = Product_1.ProductID INNER JOIN RTOLocation AS b ON a.RTOLocationID = b.RTOLocationID  where a.HSRP_StateID='" + HSRPStateID + "' and  a.RTOLocationID  in (" + AllLocation + ")    and a.OrderEmbossingDate  between '" + AuthorizationDate + "' and '" + OrderDate1 + "' and a.OrderStatus='" + OrderStatus + "'";
                        }
                    }
                   
                }

            }
            dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                i = dt.Rows.Count;
                string filename = "HSRP Daily Assign Embossing" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";
                String StringField = String.Empty;
                String StringAlert = String.Empty;
                StringBuilder bb = new StringBuilder();
                Document document = new Document();
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                // iTextSharp.text.Font times = new Font(bfTimes, 12, Font.ITALIC, Color.Black);
                //Creates a Writer that listens to this document and writes the document to the Stream of your choice:
                string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                //Opens the document:
                document.Open();

                //Adds content to the document:
                // document.Add(new Paragraph("Ignition Log Report"));
                PdfPTable table = new PdfPTable(10);
                //actual width of table in points
                table.TotalWidth = 1500f;

                PdfPCell cell120911 = new PdfPCell(new Phrase("HSRP Daily Assign Embossing Report", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120911.Colspan = 10;
                cell120911.BorderWidthLeft = 0f;
                cell120911.BorderWidthRight = 0f;
                cell120911.BorderWidthTop = 0f;
                cell120911.BorderWidthBottom = 0f;

                cell120911.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120911);

                PdfPCell cell12091 = new PdfPCell(new Phrase("State Name : " + dt.Rows[0]["HSRPStateName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12091.Colspan = 6;
                cell12091.BorderWidthLeft = 1f;
                cell12091.BorderWidthRight = 0f;
                cell12091.BorderWidthTop = 1f;
                cell12091.BorderWidthBottom = 0f;

                cell12091.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12091);

                // PdfPCell cell12093 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy"), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                PdfPCell cell12093 = new PdfPCell(new Phrase("Report Date From : " + ReportDateFrom, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12093.Colspan = 4;
                cell12093.BorderWidthLeft = 0f;
                cell12093.BorderWidthRight = 1f;
                cell12093.BorderWidthTop = 1f;
                cell12093.BorderWidthBottom = 0f;

                cell12093.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12093);

                PdfPCell cell12092 = new PdfPCell(new Phrase("RTOLocation Name : " + dt.Rows[0]["RTOLocationName"].ToString(), new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12092.Colspan = 3;
                cell12092.BorderWidthLeft = 1f;
                cell12092.BorderWidthRight = 0f;
                cell12092.BorderWidthTop = 0f;
                cell12092.BorderWidthBottom = 0f;

                cell12092.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12092);
                PdfPCell cell12094 = new PdfPCell(new Phrase("Order Status : " + OrderStatus, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12094.Colspan = 4;
                cell12094.BorderWidthLeft = 0f;
                cell12094.BorderWidthRight = 0f;
                cell12094.BorderWidthTop = 0f;
                cell12094.BorderWidthBottom = 0f;

                cell12094.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12094);

                PdfPCell cell12095 = new PdfPCell(new Phrase("Report Date To : " + ReportDateTo, new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12095.Colspan = 3;
                cell12095.BorderWidthLeft = 0f;
                cell12095.BorderWidthRight = 1f;
                cell12095.BorderWidthTop = 0f;
                cell12095.BorderWidthBottom = 0f;

                cell12095.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12095);


                PdfPCell cell1209 = new PdfPCell(new Phrase("Vehicle No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1209.Colspan = 1;
                cell1209.BorderWidthLeft = 1f;
                cell1209.BorderWidthRight = .8f;
                cell1209.BorderWidthTop = 1f;
                cell1209.BorderWidthBottom = 1f;

                cell1209.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1209);

                if (RadioButtonCreateion.Checked == true)
                {
                    PdfPCell cell1210 = new PdfPCell(new Phrase("Creation Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1210.Colspan = 1;
                    cell1210.BorderWidthLeft = 0f;
                    cell1210.BorderWidthRight = .8f;
                    cell1210.BorderWidthTop = 1f;
                    cell1210.BorderWidthBottom = 1f;

                    cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1210);

                }
                if (radiobuttonAuthorization.Checked == true)
                {
                    PdfPCell cell1210 = new PdfPCell(new Phrase("Authorization Date", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell1210.Colspan = 1;
                    cell1210.BorderWidthLeft = 0f;
                    cell1210.BorderWidthRight = .8f;
                    cell1210.BorderWidthTop = 1f;
                    cell1210.BorderWidthBottom = 1f;

                    cell1210.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1210);

                }

                PdfPCell cell1213 = new PdfPCell(new Phrase("Vehicle Class", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1213.Colspan = 1;
                cell1213.BorderWidthLeft = 0f;
                cell1213.BorderWidthRight = .8f;
                cell1213.BorderWidthTop = 1f;
                cell1213.BorderWidthBottom = 1f;

                cell1213.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1213);



                PdfPCell cell12233 = new PdfPCell(new Phrase("Vehicle Type", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell12233.Colspan = 1;
                cell12233.BorderWidthLeft = 0f;
                cell12233.BorderWidthRight = .8f;
                cell12233.BorderWidthTop = 1f;
                cell12233.BorderWidthBottom = 1f;

                cell12233.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12233);

                PdfPCell cell122331 = new PdfPCell(new Phrase("Front Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122331.Colspan = 1;
                cell122331.BorderWidthLeft = 0f;
                cell122331.BorderWidthRight = .8f;
                cell122331.BorderWidthTop = 1f;
                cell122331.BorderWidthBottom = 1f;

                cell122331.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122331);


                PdfPCell cell122332 = new PdfPCell(new Phrase("Rear Plate Size", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell122332.Colspan = 1;
                cell122332.BorderWidthLeft = 0f;
                cell122332.BorderWidthRight = .8f;
                cell122332.BorderWidthTop = 1f;
                cell122332.BorderWidthBottom = 1f;

                cell122332.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell122332);

                PdfPCell cell1206 = new PdfPCell(new Phrase("Chassis No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1206.Colspan = 1;
                cell1206.BorderWidthLeft = 0f;
                cell1206.BorderWidthRight = .8f;
                cell1206.BorderWidthTop = 1f;
                cell1206.BorderWidthBottom = 1f;
                cell1206.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1206);

                PdfPCell cell1221 = new PdfPCell(new Phrase("EngineNo", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell1221.Colspan = 1;
                cell1221.BorderWidthLeft = 0f;
                cell1221.BorderWidthRight = 1f;
                cell1221.BorderWidthTop = 1f;
                cell1221.BorderWidthBottom = 1f;

                cell1221.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell1221);


                PdfPCell cell120933 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120933.Colspan = 1;
                cell120933.BorderWidthLeft = 1f;
                cell120933.BorderWidthRight = .8f;
                cell120933.BorderWidthTop = 1f;
                cell120933.BorderWidthBottom = 1f;

                cell120933.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120933);

                PdfPCell cell120934 = new PdfPCell(new Phrase("Mobile No", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                cell120934.Colspan = 1;
                cell120934.BorderWidthLeft = 1f;
                cell120934.BorderWidthRight = .8f;
                cell120934.BorderWidthTop = 1f;
                cell120934.BorderWidthBottom = 1f;

                cell120934.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell120934);
                //PdfPCell cell1223 = new PdfPCell(new Phrase("Owner Name", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                //cell1223.Colspan = 2;
                //cell1223.BorderWidthLeft = 0f;
                //cell1223.BorderWidthRight = 1f;
                //cell1223.BorderWidthTop = 1f;
                //cell1223.BorderWidthBottom = 1f;
                //cell1223.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //table.AddCell(cell1223);
                i = i - 1;
                while (i >= 0)
                {  
                    PdfPCell cell1211 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleRegNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1211.Colspan = 1;
                    cell1211.BorderWidthLeft = 1f;
                    cell1211.BorderWidthRight = .8f;
                    cell1211.BorderWidthTop = .5f;
                    cell1211.BorderWidthBottom = .5f;

                    cell1211.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1211);

                    PdfPCell cell1212 = new PdfPCell(new Phrase(dt.Rows[i]["OrderDate"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1212.Colspan = 1;
                    cell1212.BorderWidthLeft = 1f;
                    cell1212.BorderWidthRight = .8f;
                    cell1212.BorderWidthTop = .5f;
                    cell1211.BorderWidthBottom = .5f;

                    cell1212.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1212);




                    PdfPCell cell1214 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleClass"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1214.Colspan = 1;
                    cell1214.BorderWidthLeft = 0f;
                    cell1214.BorderWidthRight = .8f;
                    cell1214.BorderWidthTop = .5f;
                    cell1214.BorderWidthBottom = .5f;

                    cell1214.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1214);

                    PdfPCell cell1219 = new PdfPCell(new Phrase(dt.Rows[i]["VehicleType"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1219.Colspan = 1;
                    cell1219.BorderWidthLeft = 0f;
                    cell1219.BorderWidthRight = .8f;
                    cell1219.BorderWidthTop = .5f;
                    cell1219.BorderWidthBottom = .5f;

                    cell1219.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell1219);



                    string FPS = dt.Rows[i]["FrontPlateSize"].ToString();
                    string SQLString1 = "select * from Product where ProductID='" + FPS + "'";

                    dt1 = Utils.GetDataTable(SQLString1, CnnString);




                    PdfPCell cell12193 = new PdfPCell(new Phrase(dt1.Rows[0]["ProductCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12193.Colspan = 1;
                    cell12193.BorderWidthLeft = 0f;
                    cell12193.BorderWidthRight = .8f;
                    cell12193.BorderWidthTop = .5f;
                    cell12193.BorderWidthBottom = .5f;

                    cell12193.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12193);


                    string FPS1 = dt.Rows[i]["RearPlateSize"].ToString();
                    string SQLString2 = "select * from Product where ProductID='" + FPS1 + "'";

                    dt2 = Utils.GetDataTable(SQLString2, CnnString);

                    PdfPCell cell12194 = new PdfPCell(new Phrase(dt2.Rows[0]["ProductCode"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell12194.Colspan = 1;
                    cell12194.BorderWidthLeft = 0f;
                    cell12194.BorderWidthRight = .8f;
                    cell12194.BorderWidthTop = .5f;
                    cell12194.BorderWidthBottom = .5f;

                    cell12194.HorizontalAlignment = 0; //0=Left, 1=     //PdfPCell cell1218 = new PdfPCell(new Phrase("", new iTextSharp.text.Font(bfTimes, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));

                    table.AddCell(cell12194);


                    PdfPCell cell1216 = new PdfPCell(new Phrase(dt.Rows[i]["ChassisNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1216.Colspan = 1;
                    cell1216.BorderWidthLeft = 0f;
                    cell1216.BorderWidthRight = .8f;
                    cell1216.BorderWidthTop = .5f;
                    cell1216.BorderWidthBottom = .5f;

                    cell1216.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1216);


                    PdfPCell cell1222 = new PdfPCell(new Phrase(dt.Rows[i]["EngineNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell1222.Colspan = 1;
                    cell1222.BorderWidthLeft = 0f;
                    cell1222.BorderWidthRight = .8f;
                    cell1222.BorderWidthTop = .5f;
                    cell1222.BorderWidthBottom = .5f;

                    cell1222.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell1222);




                    PdfPCell cell120935 = new PdfPCell(new Phrase(dt.Rows[i]["OwnerName"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                    cell120935.Colspan = 1;
                    cell120935.BorderWidthLeft = 0f;
                    cell120935.BorderWidthRight = .8f;
                    cell120935.BorderWidthTop = .5f;
                    cell120935.BorderWidthBottom = .5f;

                    cell120935.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120935);

                    PdfPCell cell120936 = new PdfPCell(new Phrase(dt.Rows[i]["MobileNo"].ToString(), new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    cell120936.Colspan = 1;
                    cell120936.BorderWidthLeft = 0f;
                    cell120936.BorderWidthRight = .8f;
                    cell120936.BorderWidthTop = .5f;
                    cell120936.BorderWidthBottom = .5f;

                    cell120936.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell120936);

                     

                    i--;



                }


                // document.Add(table);
                PdfPCell cell12241 = new PdfPCell(new Phrase("(Sign)", new iTextSharp.text.Font(bfTimes, 6f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK)));
                cell12241.Colspan = 7;
                cell12241.BorderWidthLeft = 0f;
                cell12241.BorderWidthRight = 0f;
                cell12241.BorderWidthTop = 0f;
                cell12241.BorderWidthBottom = 0f;

                cell12241.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell12241);

                document.Add(table);
                // document.Add(table1);

                document.Close();
                HttpContext context = HttpContext.Current;

                context.Response.ContentType = "Application/pdf";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                context.Response.WriteFile(PdfFolder);
                context.Response.End();
            }
            else
            {
                string closescript1 = "<script>alert('No records found for selected date.')</script>";
                Page.RegisterStartupScript("abc", closescript1);
                return;
            }
        }


        protected void Linkbutton1_Click(object sender, EventArgs e)
        {
            ClearGrid();
             
            SQLString = "SELECT CONVERT(varchar(20), HSRPRecord_CreationDate + 4, 103) AS Duedate,HSRPRecords.MobileNo,convert(varchar, HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate, CONVERT(varchar(20), HSRPRecord_CreationDate,103) AS HSRPRecord_AuthorizationDate, HSRPRecordID,HSRPRecord_AuthorizationNo,OwnerName,VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode,LaserPlateBoxNo  FROM HSRPRecords  Where hsrp_stateID='" + HSRPStateID + "' and VehicleRegNo like '%" + textboxSearch.Text + "%'";

                        //SELECT CONVERT(varchar(20), HSRPRecord_CreationDate + 4, 103) AS Duedate,HSRPRecords.MobileNo,convert(varchar, HSRPRecord_AuthorizationDate, 105) as HSRPRecord_AuthorizationDate, convert(varchar, OrderClosedDate, 105) as InvoiceDateTime, convert(varchar, OrderEmbossingDate, 105) as OrderEmbossingDate, CONVERT(varchar(20), HSRPRecord_CreationDate,103) AS HSRPRecord_AuthorizationDate, HSRPRecordID,HSRPRecord_AuthorizationNo,OwnerName,VehicleRegNo, CONVERT (varchar(20),HSRPRecord_AuthorizationDate,103) as HSRPRecord_AuthorizationDate,OrderStatus, HSRP_Front_LaserCode,HSRP_Rear_LaserCode,LaserPlateBoxNo  FROM HSRPRecords  Where hsrp_stateID='3' and VehicleRegNo like '%9211%'  order by vehicleRegNo
             
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
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
                ClearGrid();
            }
        }
    }
}