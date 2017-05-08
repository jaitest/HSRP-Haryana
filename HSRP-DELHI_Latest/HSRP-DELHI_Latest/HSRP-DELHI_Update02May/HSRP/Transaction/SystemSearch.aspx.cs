using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using DataProvider;

namespace HSRP.Transaction
{
    public partial class SystemSearch : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "AddNewPop();", true);
        //    return;
        //}
        int UID;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        int HSRPStateID;
        int RTOLocationID;
        int intHSRPStateID;
        int intRTOLocationID;


        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ScriptRegistration", "AddNewPop();", true);
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                RTOLocationID = Convert.ToInt32(Session["UserRTOLocationID"].ToString());
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                HSRPStateID = Convert.ToInt32(Session["UserHSRPStateID"].ToString());

                if (Session["UserHSRPStateID"].ToString() == "6")
                {
                    // ButImpData.Visible = true;
                }
                else
                {
                    // ButImpData.Visible = false;
                }

                lblErrMsg.Text = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        FilldropDownListHSRPState();
                        dropDownListHSRPState.Visible = false;
                        labelHSRPState.Visible = false;
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }


        //protected void ButtonGo_Click(object sender, EventArgs e)
        //{

        //}

        #region DropDown

        private void FilldropDownListHSRPState()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListHSRPState, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                dropDownListHSRPState.DataSource = dts;
                dropDownListHSRPState.DataBind();
            }
        }


        #endregion

        protected void button_Click(object sender, EventArgs e)
        {
            string orderstatus = string.Empty;

            try
            {
                if (txtSearchAll.Text == "")
                {
                    lblsuccess.Text = "Please Enter Reg No.";
                }

                else
                {

                    SQLString = "select Orderdate as OrderDate,Hsrprecord_authorizationno,Vehicleregno,orderEmbossingdate as OrderEmbossingDate,orderstatus,challanno,Hsrp_front_lasercode,hsrp_rear_lasercode from HSRPRecords Where VehicleRegNo like '%" + txtSearchAll.Text + "%'  order by OrderDate";
                    DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                    Grdsearch.DataSource = dt;
                    Grdsearch.DataBind();
                    orderstatus = dt.Rows[0]["orderstatus"].ToString();
                    if (orderstatus == "New Order")
                    {
                        lblsuccess.Text = "Order is New";

                    }
                    else if (orderstatus == "Embossing Done")
                    {
                        lblsuccess.Text = "Order has been Embossing Done ";
                    }
                    else if (orderstatus == "Closed")
                    {
                        lblsuccess.Text = "Order has been closed";
                    }
                    else
                    {
                        lblsuccess.Text = "VehicleRegno not found";
                    }
                }
            }
        
            catch (Exception ex)
            {
                lblsuccess.Text = "VehicleRegno not found";
               // lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

       



    }
}



       


        
    