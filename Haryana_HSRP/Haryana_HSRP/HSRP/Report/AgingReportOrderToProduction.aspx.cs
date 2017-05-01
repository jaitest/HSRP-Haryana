using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace HSRP.Report
{
    public partial class AgingReportOrderToProduction : System.Web.UI.Page
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

        string fontpath;

        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        DateTime AuthorizationDate;
        DateTime OrderDate1;
       
        string StickerManditory = string.Empty;
        DataTable dataSetFillHSRPRecord;
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
                         
                            FilldropDownListOrganization();
               
                        }
                        else
                        {
                            hiddenUserType.Value = "1";
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            
                            FilldropDownListOrganization();
                            //buildGrid();
                        }

                        ShowGrid();
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

        #endregion

        private void ShowGrid()
        {
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[2].Split(' ')[0] + "-" + StringAuthDate[0] + "-" + StringAuthDate[1];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00";
            
            if (DropDownListStateName.SelectedItem.Text != "--Select State--")
            {
                SQLString = "select row_number() over(order by r.RtoLocationName) as Sno,r.RtoLocationName,isnull(sum(case when h.HSRPRecord_CreationDate > (Convert(datetime, '" + AuthorizationDate + "')-7) then 1 end),0) as '7 days'," +
                            "isnull(sum(case when h.HSRPRecord_CreationDate > (Convert(datetime, '"+AuthorizationDate+"') -22) "+
                            "and h.HSRPRecord_CreationDate < (Convert(datetime, '"+AuthorizationDate+"') -7) then 1 end),0) as '15 days',"+
                            "isnull(sum(case when h.HSRPRecord_CreationDate > (Convert(datetime, '"+AuthorizationDate+"') -52) and h.HSRPRecord_CreationDate < (Convert(datetime, '"+AuthorizationDate+"') -22) then 1 end),0) as '30 days',"+
                            "isnull(sum(case when h.HSRPRecord_CreationDate > (Convert(datetime, '"+AuthorizationDate+"') -112) and h.HSRPRecord_CreationDate < (Convert(datetime, '"+AuthorizationDate+"') -52) then 1 end),0) as '60 days',"+
                            "isnull(sum(case when h.HSRPRecord_CreationDate < (Convert(datetime, '"+AuthorizationDate+"') -112) then 1 end),0) as 'Morethan 60 days' from "+
                            "hsrprecords h inner join rtolocation r on h.rtolocationid=r.rtolocationid where h.hsrp_stateid='" + DropDownListStateName.SelectedValue + "' and h.orderstatus='New Order' group by r.RtoLocationname order by r.RtoLocationname ";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                // .DataSource = dts;
                GridView1.DataSource = dts;
                GridView1.DataBind();
               

                Label7.Visible = true;
                lblReportDate.Text = "Report As On Date : " + StringAuthDate[1] + "-" + StringAuthDate[0] + "-" + StringAuthDate[2].Split(' ')[0];
                lblGeneratedDate.Text = "Generated On : " + System.DateTime.Now.ToString("dd-MM-yyyy");
            }
            else
            {
                
                lblErrMsg.Text = String.Empty;
                lblErrMsg.Text = "Please Select Location.";
                return;
            }
        }



     
        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
          
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate));
  
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void LinkbuttonSearch_Click(object sender, EventArgs e)
        {
            ShowGrid();
        }

        protected void printbtn_click(object sender, EventArgs e)
        {
            
        }

        Label lbl;
        string sqlquery = string.Empty;
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String[] StringAuthDate = OrderDate.SelectedDate.ToString().Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[2].Split(' ')[0] + "-" + StringAuthDate[0] + "-" + StringAuthDate[1];
            //AuthorizationDate = new DateTime(Convert.ToInt32(StringAuthDate[2].Split(' ')[0]), Convert.ToInt32(StringAuthDate[0]), Convert.ToInt32(StringAuthDate[1]));
            string AuthorizationDate = From + " 00:00:00";
            string i = e.CommandArgument.ToString();
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);


            lbl = gvr.FindControl("Label5") as Label;
            ClientScript.RegisterStartupScript(GetType(), "showModalScript", "editpage('" + e.CommandName + "','"+DropDownListStateName.SelectedValue+"','"+lbl.Text+"','"+AuthorizationDate+"');", true);


        }
    

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
        }
    
        
    }
}