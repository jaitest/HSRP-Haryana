using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using DataProvider;

namespace HSRP.Master
{
    public partial class StockEntryNew : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string Mode;
        string UserID;
        string StockEntryID;
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
             
            UserType = Session["UserType"].ToString();
            HSRPStateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            RTOLocationID = Convert.ToInt16(Session["UserRTOLocationID"]);
             UserID =  Session["UID"].ToString();
            

            if (!Page.IsPostBack)
            {
                string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
                if ((DateTime.Parse(TodayDate).DayOfWeek == DayOfWeek.Monday) || (DateTime.Parse(TodayDate).DayOfWeek == DayOfWeek.Tuesday) || (DateTime.Parse(TodayDate).DayOfWeek == DayOfWeek.Wednesday) || (DateTime.Parse(TodayDate).DayOfWeek == DayOfWeek.Saturday))
                {
                    btnSave.Visible = true;
                }

                FilldropDownListClient();
                FilldropDownListOrganization();
                InitialSetting();
                if (Mode == "Edit")
                {

                    LabelFormName.Text = "Update Stock Entry";

                    EditStockEntry();
                    btnSave.Visible = false;
                    lblUserName.Visible = true; 
                    lblUserNamelbl.Visible = true;
                    btnReset.Visible = false;
                    DiableAllField();
                    
                }
                else
                {
                    LabelFormName.Text = "Add New Stock Entry"; 
                    btnSave.Visible = true;
                    lblUserName.Visible = false;
                    lblUserNamelbl.Visible = false;
                }
            }
        }
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
                Utils.PopulateDropDownList(dropDownRTOLocation, SQLString.ToString(), CnnString, "--Select RTO Name--");


            }
            else
            {
                string UserID = Convert.ToString(Session["UID"]);
                SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserID + "' ";
                DataTable dss = Utils.GetDataTable(SQLString, CnnString);
                labelOrganization.Visible = true;
                DropDownListStateName.Visible = true;
                labelClient.Visible = true;
                dropDownRTOLocation.Visible = true;
                dropDownRTOLocation.DataSource = dss;
                dropDownRTOLocation.DataBind();
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
        #region DropDown
 
        #endregion
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
           // string minusdate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + "7";
           
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            if (DateTime.Parse(TodayDate).DayOfWeek == DayOfWeek.Saturday)
            {
                OrderDate.SelectedDate = (DateTime.Parse(TodayDate));
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    if(DateTime.Parse(TodayDate).AddDays(i).DayOfWeek==DayOfWeek.Saturday)
                    {
                        string upcomingsaturday = DateTime.Parse(TodayDate).AddDays(i).ToString();
                        OrderDate.SelectedDate = (DateTime.Parse(upcomingsaturday).AddDays(-7));
                        break;
                    }
                }
            }
            
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            lblTotalRecord.Text = "";
            int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
            int.TryParse(dropDownRTOLocation.SelectedValue, out intRTOLocationID);
            string UID = Session["UID"].ToString();
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String FromDateTo = StringAuthDate[1] + "-" + MonthdateTO + "-" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
               
            
            SQLString = "insert into PlateStockEntry (HSRP_StateID,RtolocationID,UserID,BlanckPlate_200_100White,EmbosedPlate200_100White,ClosedPlate_200_100White,BlanckPlate_200_100Yellow,EmbosedPlate200_100Yellow,ClosedPlate_200_100Yellow,BlanckPlate_285_45White,EmbosedPlate285_45White,ClosedPlate_285_45White, BlanckPlate_285_45Yellow,EmbosedPlate285_45Yellow,ClosedPlate_285_45Yellow, BlanckPlate_340_200Yellow,EmbosedPlate_340_200Yellow,ClosedPlate_340_200Yellow,BlanckPlate_340_200White,EmbosedPlate_340_200White,ClosedPlate_340_200White, BlanckPlate_500_120White,EmbosedPlate_500_120White,ClosedPlate_500_120White,BlanckPlate_500_120Yellow,EmbosedPlate_500_120Yellow,ClosedPlate_500_120Yellow,Stock_Date) values ('" + intHSRPStateID + "','" + intRTOLocationID + "','" + UID + "','" + txt200_100BlnkPlate.Text.Trim() + "','" + txt200_100EmbosedPlate.Text.Trim() + "','" + txt200_100ClosedPlate.Text.Trim() + "','" + txt200_100BlnkPlateYellow.Text.Trim() + "','" + txt200_100EmbosedPlateYellow.Text.Trim() + "','" + txt200_100ClosedPlateYellow.Text.Trim() + "','" + txt285_45BlnkPlateWhite.Text.Trim() + "','" + txt285_45EmbosedPlateWhite.Text.Trim() + "','" + txt285_45ClosedPlateWhite.Text.Trim() + "','" + txt285_45BlnkPlateYellow.Text.Trim() + "','" + txt285_45EmbosedPlateYellow.Text.Trim() + "','" + txt285_45ClosedPlateYellow.Text.Trim() + "','" + txt300_200BlnkPlateWhite.Text.Trim() + "','" + txt300_200EmbosedPlateWhite.Text.Trim() + "','" + txt300_200ClosedPlateWhite.Text.Trim() + "','" + txt300_200BlnkPlateYellow.Text.Trim() + "','" + txt300_200EmbosedPlateYellow.Text.Trim() + "','" + txt300_200ClosedPlateYellow.Text.Trim() + "','" + txt500_120BlnkPlateWhite.Text.Trim() + "','" + txt500_120EmbosedPlateWhite.Text.Trim() + "','" + txt500_120ClosedPlateWhite.Text.Trim() + "','" + txt500_120BlnkPlateYellow.Text.Trim() + "','" + txt500_120EmbosedPlateYellow.Text.Trim() + "','" + txt500_120ClosedPlateYellow.Text.Trim() + "','"+From+"')";
            int i = Utils.ExecNonQuery(SQLString.ToString(), CnnString);
            if (i > 0)
            {
                lblSuccess.Visible = true;
                lblSuccess.Text = "Record Save Successfully";
                txt200_100BlnkPlate.Text = "";
                txt200_100EmbosedPlate.Text = "";
                txt200_100ClosedPlate.Text = "";

                txt200_100BlnkPlateYellow.Text = "";
                txt200_100EmbosedPlateYellow.Text = "";
                txt200_100ClosedPlateYellow.Text = "";

                txt285_45BlnkPlateWhite.Text = "";
                txt285_45EmbosedPlateWhite.Text = "";
                txt285_45ClosedPlateWhite.Text = "";

                txt285_45BlnkPlateYellow.Text = "";
                txt285_45EmbosedPlateYellow.Text = "";
                txt285_45ClosedPlateYellow.Text = "";

                txt300_200BlnkPlateWhite.Text = "";
                txt300_200EmbosedPlateWhite.Text = "";
                txt300_200ClosedPlateWhite.Text = "";

                txt300_200BlnkPlateYellow.Text = "";
                txt300_200EmbosedPlateYellow.Text = "";
                txt300_200ClosedPlateYellow.Text = "";

                txt500_120BlnkPlateWhite.Text = "";
                txt500_120EmbosedPlateWhite.Text = "";
                txt500_120ClosedPlateWhite.Text = "";

                txt500_120BlnkPlateYellow.Text = "";
                txt500_120EmbosedPlateYellow.Text = "";
                txt500_120ClosedPlateYellow.Text = "";

                FilldropDownListClient();
                FilldropDownListOrganization();
            }
            else
            {
                lblSuccess.Text = "";
                lblSuccess.Visible = false;
                lblExist.Text = "Record Not Save Successfully";
            }
             
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
            lblSuccess.Text ="";
            lblExist.Text = "";
            lblSuccess.Text = "";
            lblSuccess.Visible = false;
        }
        public void EditStockEntry()
        {
            StockEntryID = Request.QueryString["ID"].ToString();
            SQLString = "select *, (Select (UserFirstName +' '+ userLastName) as name from users where userID=plateStockentry.UserID) as UserName  from plateStockentry where stockPlateID='" + StockEntryID + "'";
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                lblSuccess.Visible = false;
                lblUserName.Text = dt.Rows[0]["UserName"].ToString();
                txt200_100BlnkPlate.Text = dt.Rows[0]["BlanckPlate_200_100White"].ToString();
                txt200_100BlnkPlate.Text = dt.Rows[0]["BlanckPlate_200_100White"].ToString();
                txt200_100EmbosedPlate.Text = dt.Rows[0]["EmbosedPlate200_100White"].ToString();
                txt200_100ClosedPlate.Text = dt.Rows[0]["ClosedPlate_200_100White"].ToString();
                txt200_100BlnkPlateYellow.Text = dt.Rows[0]["BlanckPlate_200_100Yellow"].ToString();
                txt200_100EmbosedPlateYellow.Text = dt.Rows[0]["EmbosedPlate200_100Yellow"].ToString();
                txt200_100ClosedPlateYellow.Text = dt.Rows[0]["ClosedPlate_200_100Yellow"].ToString();
                txt285_45BlnkPlateWhite.Text = dt.Rows[0]["BlanckPlate_285_45White"].ToString();
                txt285_45EmbosedPlateWhite.Text = dt.Rows[0]["EmbosedPlate285_45White"].ToString();
                txt285_45ClosedPlateWhite.Text = dt.Rows[0]["ClosedPlate_285_45White"].ToString();
                txt285_45BlnkPlateYellow.Text = dt.Rows[0]["BlanckPlate_285_45Yellow"].ToString();
                txt285_45EmbosedPlateYellow.Text = dt.Rows[0]["EmbosedPlate285_45Yellow"].ToString();
                txt285_45ClosedPlateYellow.Text = dt.Rows[0]["ClosedPlate_285_45Yellow"].ToString();
                txt300_200BlnkPlateWhite.Text = dt.Rows[0]["BlanckPlate_340_200Yellow"].ToString();
                txt300_200EmbosedPlateWhite.Text = dt.Rows[0]["EmbosedPlate_340_200Yellow"].ToString();
                txt300_200ClosedPlateWhite.Text = dt.Rows[0]["ClosedPlate_340_200Yellow"].ToString();
                txt300_200BlnkPlateYellow.Text = dt.Rows[0]["BlanckPlate_340_200White"].ToString();
                txt300_200EmbosedPlateYellow.Text = dt.Rows[0]["EmbosedPlate_340_200White"].ToString();
                txt300_200ClosedPlateYellow.Text = dt.Rows[0]["ClosedPlate_340_200White"].ToString();
                txt500_120BlnkPlateWhite.Text = dt.Rows[0]["BlanckPlate_500_120White"].ToString();
                txt500_120EmbosedPlateWhite.Text = dt.Rows[0]["EmbosedPlate_500_120White"].ToString();
                txt500_120ClosedPlateWhite.Text = dt.Rows[0]["ClosedPlate_500_120White"].ToString();
                txt500_120BlnkPlateYellow.Text = dt.Rows[0]["BlanckPlate_500_120Yellow"].ToString();
                txt500_120EmbosedPlateYellow.Text = dt.Rows[0]["EmbosedPlate_500_120Yellow"].ToString();
                txt500_120ClosedPlateYellow.Text = dt.Rows[0]["ClosedPlate_500_120Yellow"].ToString();

                string StateID = dt.Rows[0]["HSRP_StateID"].ToString();
                string LocationID = dt.Rows[0]["RtolocationID"].ToString();

                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where hsrp_stateID='" + StateID + "'";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
                 DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

                 SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where RtoLocationID= '"+LocationID +"'";
                 Utils.PopulateDropDownList(dropDownRTOLocation, SQLString.ToString(), CnnString, "--Select RTO Name--");
                 dropDownRTOLocation.SelectedIndex = dropDownRTOLocation.Items.Count - 1;
            }

        }
        public void DiableAllField()
        {
            txt200_100BlnkPlate.Enabled = false;
            txt200_100BlnkPlate.Enabled = false;
            txt200_100EmbosedPlate.Enabled = false;
            txt200_100ClosedPlate.Enabled = false;
            txt200_100BlnkPlateYellow.Enabled = false;
            txt200_100EmbosedPlateYellow.Enabled = false;
            txt200_100ClosedPlateYellow.Enabled = false;
            txt285_45BlnkPlateWhite.Enabled = false;
            txt285_45EmbosedPlateWhite.Enabled = false;
            txt285_45ClosedPlateWhite.Enabled = false;
            txt285_45BlnkPlateYellow.Enabled = false;
            txt285_45EmbosedPlateYellow.Enabled = false;
            txt285_45ClosedPlateYellow.Enabled = false;
            txt300_200BlnkPlateWhite.Enabled = false;
            txt300_200EmbosedPlateWhite.Enabled = false;
            txt300_200ClosedPlateWhite.Enabled = false;
            txt300_200BlnkPlateYellow.Enabled = false;
            txt300_200EmbosedPlateYellow.Enabled = false;
            txt300_200ClosedPlateYellow.Enabled = false;
            txt500_120BlnkPlateWhite.Enabled = false;
            txt500_120EmbosedPlateWhite.Enabled = false;
            txt500_120ClosedPlateWhite.Enabled = false;
            txt500_120BlnkPlateYellow.Enabled = false;
            txt500_120EmbosedPlateYellow.Enabled = false;
            txt500_120ClosedPlateYellow.Enabled = false;
            DropDownListStateName.Enabled = false;
            dropDownRTOLocation.Enabled = false;
        }
        protected void dropDownRTOLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSuccess.Text = "";
        }
       
    }
}