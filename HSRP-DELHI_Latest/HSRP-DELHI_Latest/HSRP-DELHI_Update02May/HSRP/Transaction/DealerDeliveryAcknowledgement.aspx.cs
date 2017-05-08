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
                userdealerid = Session["userdealerid"].ToString();
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
                        InitialSetting();
                        buildGrid();

                       // Utils.user_log(strUserID, "View Organization", ComputerIP, "Page load", CnnString);
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
            //HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            //CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }
        
        public void buildGrid()
        {
            try
            {
                string SQLString = "select  HSRPRecordID, VehicleRegNo,HSRPRecord_AuthorizationNo,OwnerName,VehicleClass,VehicleType,HSRP_Front_LaserCode,HSRP_Rear_LaserCode from hsrprecords  where  addrecordby='dealer' and dealerid='" + userdealerid + "' and hsrprecord_creationdate>'2015-11-22'";// and orderstatus='Embossing Done'";


                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                grdid.DataSource = dt;
                grdid.DataBind();
                grdid.Visible = true;


                //Grid1.DataSource = dt;
                //Grid1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                //Grid1.SearchOnKeyPress = true;
                //Grid1.DataBind();
                //Grid1.RecordCount.ToString();
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }


        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        //protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chk1 = grdid.HeaderRow.FindControl("CHKSelect1") as CheckBox;
        //    if (chk1.Checked == true)
        //    {
        //        for (int i = 0; i < grdid.Rows.Count; i++)
        //        {
        //            //  string lblVehicleRegNo,FLasercode,RearLaserCode;
        //            chk = grdid.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
        //            chk.Checked = true;
        //        }
        //    }
        //    else if (chk1.Checked == false)
        //    {
        //        for (int i = 0; i < grdid.Rows.Count; i++)
        //        {
        //            //  string lblVehicleRegNo,FLasercode,RearLaserCode;
        //            chk = grdid.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
        //            chk.Checked = false;
        //        }
        //    }

        //}
        protected void grdid_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Approval")
            //{
            //    LinkButton lnkView = (LinkButton)e.CommandSource;
            //    string TransactionID = lnkView.CommandArgument;

            //    if (TransactionID!="")
            //    {
            //        string sqlquery1 = "update banktransaction set ApprovedStatus='Y' where TransactionID='" + TransactionID + "'";
            //        int i = Utils.ExecNonQuery(sqlquery1, CnnString);
            //        if (i > 0)
            //        {
            //            lblSucMess.Text = "Record Approved Successfully...";
            //        }
            //        else
            //        {
            //            lblSucMess.Text = "Record not Approved..";
            //        }

            //    }
            //}

        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            lblMsgRed.Text = "";
            int cheked=0;
           
            for (int i = 0; i < grdid.Rows.Count; i++)
            {
                //  string lblVehicleRegNo,FLasercode,RearLaserCode;
                chk = grdid.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
               // chk.Checked = true;
                if (chk.Checked == true)
                {
                    cheked = cheked+1;
                    Label hsrprecordid = grdid.Rows[i].Cells[1].FindControl("id") as Label;
                    string sql = "update hsrprecords set orderstatus='Closed',ordercloseddate=getdate() where hsrprecordid='"+hsrprecordid+"';";
                  
                }
            }
            // int i = Utils.ExecNonQuery(sql, CnnString);
                 
            if(cheked==0)
            {
                lblMsgRed.Text="";
                lblMsgRed.Text = "Please select the rows by checkbox.";
                lblMsgRed.Visible = true;

            }
            else 
            {
                
                lblMsgBlue.Text = "";
                lblMsgBlue.Text = "Record Updated.";
                lblMsgBlue.Visible = true;

            }
        }
    }
}