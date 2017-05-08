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

namespace HSRP.Report
{
    public partial class HSRPCountRecords : System.Web.UI.Page
    {
        int UserType;
        string CnnString = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string strUserID = string.Empty;
        int intHSRPStateID;
        string SQLString = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();

                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType.Equals(0))
                        {
                           // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;

                            FilldropDownListOrganization();


                        }
                        else
                        {

                           // labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;

                            FilldropDownListOrganization();



                        }


                    }
                    catch (Exception err)
                    {

                    }
                }
            }
        }


        private void FilldropDownListOrganization()
        {


            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
            Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");




        }
        protected void DropDownListStateName_SelectedIndexChanged1(object sender, EventArgs e)
        {


            if (DropDownListStateName.SelectedItem.Text != "--Select Client--")
            {
                BuildGrid();
            }
            else
            {
                Grid10.Items.Clear();

                return;
            }



        }

        public void BuildGrid()
        {
            try
            {
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                 SQLString = "SELECT RTOLocation.RTOLocationName AS RTOLOCATION,"
                            + " COUNT(CASE WHEN OrderStatus='New Order' THEN 1 END ) AS ORDER_BOOKED,"
                            + " COUNT(CASE WHEN OrderStatus='Embossing Done' THEN 1 END) AS EMBOSSED,"
                            + " COUNT(CASE WHEN OrderStatus='Closed' OR OrderStatus='Close' THEN 1 END) AS CLOSED,"
                            + " COUNT(ORDERSTATUS) AS TOTAL "
                            + " FROM HSRPRECORDS"
                            + " INNER JOIN RTOLocation ON RTOLocation.RTOLocationID=HSRPRECORDS.RTOLOCATIONID"
                            + " WHERE HSRPRECORDS.HSRP_StateID="  + intHSRPStateID +""
                            + " GROUP BY RTOLocation.RTOLocationName"
                            + " ORDER BY RTOLocation.RTOLocationName";

                DataTable dt = Utils.GetDataTable(SQLString, CnnString);

                Grid10.DataSource = dt;
                Grid10.SearchOnKeyPress = true;

                Grid10.DataBind();

                //Grid10.RecordCount.ToString();
            }
            catch (Exception ex)
            {

            }
        }
    }
}