using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;



namespace HSRP.Master
{
    public partial class LaserTransferInterState : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string Mode;
        string UserID;
        string BatchID;
        int StateID;
        int RTOLocationID;
        string UserType;
        int HSRPStateID; 
        int intHSRPStateID;
        int ProductID;
        int intRTOLocationID;
        string RTONameForDropdown; 
        DateTime AuthorizationDate;
        DateTime OrderDate1;

         
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();  
            Mode = Request.QueryString["Mode"].ToString();
            lblExist.Visible = false;
            TextBoxLaserNoError.Visible = false;
            UserType = Session["UserType"].ToString();
            HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
             UserID =  Session["UID"].ToString();
            

            if (!Page.IsPostBack)
            {
                
                 FilldropDownListOrganization();
                 FilldropDownListClient();
                 dropdownProduct();
                
                if (Mode == "Edit")
                {
                    LabelFormName.Text = "Interstate Laser Transfer";
                    BatchID =  Request.QueryString["BatchID"].ToString();
                     
                    btnSave.Visible = false;
                   
                }
                else
                {
                    LabelFormName.Text = "Interstate Plate Transfer";
                    
                    btnSave.Visible = true;
                    InitialSetting();  
                }
            }
            if (UserType == "0")
            {
                dataLabellbl.Visible = false;
                TRRTOHide.Visible = false;
            } 
           

            //SQLString = "select top 50000 hsrpRecordID, OwnerName from hsrprecords";
            //DataTable ds = Utils.GetDataTable(SQLString, CnnString);
            //IDLIstView.DataSource = ds;
            //IDLIstView.DataBind();


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

            if (DropDownListStateName.SelectedValue != "--Select State--")
            {
                if (UserType == "0")
                { 
                    int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);

                    //SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and ActiveStatus!='N'   Order by RTOLocationName";
                    SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + DropDownListStateName.SelectedValue + " and ActiveStatus!='N'   Order by RTOLocationName";
                    Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Name--");
                    Utils.PopulateDropDownList(dropDownListTransferLocationName, SQLString.ToString(), CnnString, "--Select Transfer RTO Location--");

                   
                }
                else
                {
                    string UserID = Convert.ToString(Session["UID"]);
                    SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                    DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                    labelOrganization.Visible = true;
                    DropDownListStateName.Visible = true;
                    //labelClient.Visible = true;
                    dropDownListClient.Visible = true;

                    dropDownListClient.DataSource = dss;
                    dropDownListClient.DataBind();
                    dropDownListTransferLocationName.DataSource = dss;
                    dropDownListTransferLocationName.DataBind();

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
        }
        public void dropdownPrefix()
        {
            SQLString = "select distinct Prefix from prefixLaserNo where hsrp_StateID='"+DropDownListStateName.SelectedValue+"' order by Prefix";
            Utils.PopulateDropDownList(DropDownListPrefix, SQLString, CnnString, "-- Select Prefix --");
        }

        public void dropdownProduct()
        {
            if (DropDownListStateName.SelectedValue != "--Select State--")
            {

                SQLString = "select ProductCode, ProductID from product where hsrp_stateid='" + DropDownListStateName.SelectedValue + "'";
                Utils.PopulateDropDownList(DropDownListProduct, SQLString, CnnString, "-- Select Product --");
            }
        }
         
        #endregion

     
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblSucMess.Text = "";
            lblErrMess.Text = "";

            Int64 LaserNoFrom = Convert.ToInt64(textboxLaserCodeFrom.Text.Trim().ToUpper());
            Int64 LaserNoTo = Convert.ToInt64(textboxLaserCodeTo.Text.Trim().ToUpper());
            string StartLaserNo = DropDownListPrefix.Text + LaserNoFrom;
            string EndLaserNo = DropDownListPrefix.Text + LaserNoTo;

            string prefix = DropDownListPrefix.Text;
            string LaserNo = string.Empty;
            Int64 startNo = LaserNoFrom-1;


            string LaserNoStart = prefix + LaserNoFrom;
            string laserNoEnd = prefix + LaserNoTo;


            Int64 difference = (LaserNoTo-startNo);
            SQLString = "select count(*)as totalLaserNo from rtoinventory where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and inventorystatus='New Order' and rtolocationID='" + dropDownListClient.SelectedValue + "' and ProductID ='" + DropDownListProduct.SelectedValue + "' and laserNo between '" + StartLaserNo + "' and '" + laserNoEnd + "'";
            DataTable dtlaserCount = Utils.GetDataTable(SQLString, CnnString);
            int totalTransfer = 0;
            int prefiLaserNoCount=0;
            if (dtlaserCount.Rows.Count > 0)
            {
                prefiLaserNoCount = Convert.ToInt16(dtlaserCount.Rows[0]["totalLaserNo"]);
            }


            if (prefiLaserNoCount == difference)
            {
                //Int64 count = 0;

                SQLString = "update rtoinventory set  rtolocationID='" + dropDownListTransferLocationName.SelectedValue + "', inventorystatus='In Transit' where hsrp_StateID='" + DropDownListStateName.SelectedValue + "' and inventorystatus='New Order' and rtolocationID='" + dropDownListClient.SelectedValue + "' and ProductID ='" + DropDownListProduct.SelectedValue + "' and laserNo between '" + LaserNoStart + "' and '" + laserNoEnd + "'";
                  int z = Utils.ExecNonQuery(SQLString, CnnString);
                  if (z > 0)
                  {
                      lblSucMess.Text = "Total Transfer Record : " + difference;
                  }
                  else
                  {
                      lblErrMess.Text = "Inventory Not Transfer Successfully.!!";
                  }
            }
            else
            {
                lblErrMess.Text = "Selected Inventory Already Used!!";
            }
        }



        private void InitialSetting()
        { 
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

           // HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
           // HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            

            //OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListStateName.SelectedValue != "--Select State--")
            {
                FilldropDownListClient();
                dropdownPrefix();
                dropdownProduct();
            }
        }

        

       
    }
}