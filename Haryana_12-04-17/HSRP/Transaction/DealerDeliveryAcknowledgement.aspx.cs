using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace HSRP.Transaction
{


    public partial class DealerDeliveryAcknowledgement : System.Web.UI.Page
    {

        Utils bl = new Utils();
        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string userdealerid = string.Empty;
        int UserType;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                userdealerid = Session["dealerid"].ToString();
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
               
                if (!IsPostBack)
                {
                    try
                    {

                        buildGrid();

                    }
                    catch (Exception err)
                    {
                        lblMsgRed.Text = "Error on Page Load" + err.Message.ToString();
                        lblMsgRed.Visible = true;
                    }
                }
            }
        }



        public void buildGrid()
        {
            try
            {
                string SQLString = "select  HSRPRecordID, VehicleRegNo,HSRPRecord_AuthorizationNo,OwnerName,VehicleClass,VehicleType,HSRP_Front_LaserCode,HSRP_Rear_LaserCode from hsrprecords  where  addrecordby='dealer' and dealerid='" + userdealerid + "' and hsrprecord_creationdate>'2015-12-01 00:00:00' and orderstatus='Embossing Done' order by VehicleRegNo";
                
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                if (dt.Rows.Count > 0)
                {
                    grdid.DataSource = dt;
                    grdid.DataBind();
                    grdid.Visible = true;
                }
                else
                {
                    lblMsgRed.Text = "";
                    lblMsgRed.Text = "There are No Records to Confirm.";
                    lblMsgRed.Visible = true;
                    btnGO.Enabled = false;
                }

               

            }
            catch (Exception ex)
            {
                lblMsgRed.Text = "";
                lblMsgRed.Text = "Error in Populating Grid :" + ex.Message.ToString();
                lblMsgRed.Visible = true;
            }
        }


        StringBuilder sb = new StringBuilder();
        CheckBox chk;



        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsgRed.Text = "";
                int cheked = 0;
                StringBuilder sbx = new StringBuilder();
                for (int i = 0; i < grdid.Rows.Count; i++)
                {
                    //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                    chk = grdid.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    // chk.Checked = true;
                    if (chk.Checked == true)
                    {
                        cheked = cheked + 1;
                        Label hsrprecordid = grdid.Rows[i].Cells[1].FindControl("id") as Label;
                        sbx.Append("update hsrprecords set orderstatus='Closed',ordercloseddate=getdate() where hsrprecordid='" + hsrprecordid.Text.ToString() + "';");

                    }
                }

                if (cheked == 0)
                {
                    lblMsgRed.Text = "";
                    lblMsgRed.Text = "Please select the rows by selecting checkbox for the records to be confirmed.";
                    lblMsgRed.Visible = true;

                }
                else
                {
                    int ii = Utils.ExecNonQuery(sbx.ToString(), CnnString);
                    if (ii > 0)
                    {
                        lblMsgBlue.Text = "";
                        lblMsgBlue.Text = "Selected Records Successfully Updated.";
                        lblMsgBlue.Visible = true;
                    }
                    else
                    {
                        lblMsgRed.Text = "";
                        lblMsgRed.Text = "Records Not Updated Contact Administrator";
                        lblMsgRed.Visible = true;
                    }
                }
            }
            catch (Exception err)
            {
                lblMsgRed.Text = "Error While Upload Contact Administrator :" + err.Message.ToString();
                lblMsgRed.Visible = true;
            }
        }
    }
}