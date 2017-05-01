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
using iTextSharp.text.pdf;


namespace HSRP.Transaction
{
    public partial class ProductionSheetReleased : System.Web.UI.Page
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
        string strHsrpStateId = string.Empty;
        string strRtoId = string.Empty;
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

                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();

                HSRP_StateID1 = Session["UserHSRPStateID"].ToString();
                RTOLocationID1 = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {
                    try
                    {
                        InitialSetting();
                        if (UserType1.Equals(0))
                        {
                            FilldropDownListOrganization();
                        }
                        else
                        {
                            FilldropDownListOrganization();
                            FilldropDownListRTO();
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

        private void FilldropDownListRTO()
        {
            if (UserType1.Equals(0))
            {
                SQLString1 = "select RTOLocationid,Rtolocationname from Rtolocation  where hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and ActiveStatus='Y' Order by RtoLocationname";
                Utils.PopulateDropDownList(ddlRto, SQLString1.ToString(), CnnString1, "--Select RTO--");
            }
            else
            {
                SQLString1 = "select RTOLocationid,Rtolocationname from Rtolocation  where hsrp_stateid='" + HSRP_StateID1 + "' and ActiveStatus='Y' Order by RtoLocationname";
                Utils.PopulateDropDownList(ddlRto, SQLString1.ToString(), CnnString1, "--Select RTO--");
            }
        }

        
        private void InitialSetting()
        {

            OrderDate.MinDate = new DateTime(2014, 09, 09);
            
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
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

        

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListRTO();
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string dateFrom = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
            string strdateFrom = dateFrom + " 00:00:00";
            string strDateTO = dateFrom + " 23:59:59";
            strHsrpStateId=DropDownListStateName.SelectedValue.ToString();
            strRtoId=ddlRto.SelectedValue.ToString();
            SQLString1 = "select count(*) from hsrprecords where hsrp_stateid='" + strHsrpStateId + "' and RTOLocationId='" + strRtoId + "' and convert(date,hsrprecord_creationdate,103) between convert(date,'"+strdateFrom+"',103) and convert(date,'"+strDateTO+"',103)";
            string strResult = Utils.getScalarValue(SQLString1, CnnString1);
           
            SQLString1 = "Update hsrprecords set productionsheetReleasedstatus='Y' where hsrp_stateid='" + strHsrpStateId + "' and RTOLocationId='" + strRtoId + "' and convert(date,hsrprecord_creationdate,103) between convert(date,'" + strdateFrom + "',103) and convert(date,'" + strDateTO + "',103)";
            Utils.ExecNonQuery(SQLString1,CnnString1);

            Label1.Visible = true;
            Label1.Text = "";
            Label1.Text = " Production sheet Released For  " + strResult + " Orders";

        }

      
    }
}