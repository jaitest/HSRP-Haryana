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
    public partial class collectionagging : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        int intHSRPStateID1;
        int intRTOLocationID1;
        string SQLString1 = string.Empty;
        string OrderType;
        string recordtype = string.Empty;
        string strStateId = string.Empty;
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

                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();

                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType1.Equals(0))
                        {
                            DropDownListStateName.Visible = true;
                            FilldropDownListOrganization();
                         //   OrderDatefrom.SelectedDate = System.DateTime.Now;
                        }
                        else
                        {

                            DropDownListStateName.Visible = true;
                            FilldropDownListOrganization();

                           // OrderDatefrom.SelectedDate = System.DateTime.Now;
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
            if (UserType1.Equals(0))
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString1.ToString(), CnnString1, "--Select State--");
            }
            else
            {
                SQLString1 = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID1 + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString1, CnnString1);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
           
           // OrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
          //  OrderDatefrom.MaxDate = DateTime.Parse(MaxDate);
           // CalendarOrderDatefrom.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
           // CalendarOrderDatefrom.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

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
        string FromDate = System.DateTime.Now.ToString("yyyy/MM/dd");
        string type = "B";
        protected void btnexport_Click(object sender, EventArgs e)
        {
            //SaveAndDownloadFile();


            try
            {
                // FromDate = OrderDate.SelectedDate.ToString("MM/dd/yy 00:00:00");
                // ToDate = HSRPAuthDate.SelectedDate.ToString("MM/dd/yy 23:59:59");
                strStateId = DropDownListStateName.SelectedValue;
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);

                SqlCommand cmd = new SqlCommand("[Report_CollectionInHandAgging]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StateId", strStateId));
                cmd.Parameters.Add(new SqlParameter("@reportDate", FromDate));
                cmd.Parameters.Add(new SqlParameter("@flag", type));
                // cmd.Parameters.Add(new SqlParameter("@rtolocationid", dropDownListClient.SelectedValue));



                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);



                da.Fill(dt1);



                // dt = Utils.GetDataTable(SQLString, CnnString);
                GridView1.DataSource = dt1;
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

       

    }
}