using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{


    public partial class ViewBankTransactionApproval : System.Web.UI.Page
    {

        Utils bl = new Utils();
        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
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
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;

                if (!IsPostBack)
                {
                    try
                    {
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


        
        public void buildGrid()
        {
            try
            {
                string SQLString = "SELECT [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.rtolocation where StateID='" + HSRPStateID + "' and DepositDate>='1-april-2015' and ApprovedStatus='N' order by DepositDate desc";


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

       

        protected void grdid_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approval")
            {
                LinkButton lnkView = (LinkButton)e.CommandSource;
                string TransactionID = lnkView.CommandArgument;

                if (TransactionID!="")
                {
                    string sqlquery1 = "update banktransaction set ApprovedStatus='Y' where TransactionID='" + TransactionID + "'";
                    int i = Utils.ExecNonQuery(sqlquery1, CnnString);
                    if (i > 0)
                    {
                        lblSucMess.Text = "Record Approved Successfully...";
                    }
                    else
                    {
                        lblSucMess.Text = "Record not Approved..";
                    }

                }
            }

        }
    }
}