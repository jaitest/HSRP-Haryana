using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

namespace HSRP.Transaction
{


    public partial class ViewBankTransactionUpdate : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        Utils bl = new Utils();

        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType1 = string.Empty;
        string ProductivityID = string.Empty;
        string Transactionid = string.Empty;
        string AccountNo = string.Empty;
        string Bankname = string.Empty; 
        

        int UserType;
       
        //string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
       
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        string query1 = string.Empty;
        
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

                        lblErrMsg.Text = string.Empty;
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
                //string SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.rtolocation where StateID='" + HSRPStateID + "' and isnull(dealerid,'')=''  order by DepositDate desc";

                string SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid  where StateID='" + HSRPStateID + "'   order by DepositDate desc";
              
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                Grid1.DataSource = dt;
                Grid1.PageIndex = Convert.ToInt32(Request.QueryString["PageIndex"]);
                Grid1.DataBind();
                lblErrMsg.Text = string.Empty;
               
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void Grid1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblvoid = (Label)e.Row.FindControl("lblvoid");
                
                if (lblvoid != null)
                {
                    if (lblvoid.Text.ToLower() == "void")
                    {
                        Label lbledit = (Label)e.Row.FindControl("lbledit");

                        foreach (TableCell cell in e.Row.Cells)
                        {

                            cell.BackColor = Color.AliceBlue;

                        }
                        e.Row.Enabled = false;
                        lblvoid.Visible = false;
                        lbledit.Visible = false;

                    }
                    else
                    {

                        lblvoid.Text = "Void";
                    }
                   
                }
            }
        }

        protected void Grid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            Response.Redirect("viewBankTransactionupdate.aspx?PageIndex=" + e.NewPageIndex.ToString());
            buildGrid();
          
        }

        protected void Grid1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void BtnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                Transactionid = txtransactionid.Text.Trim().ToString();

                AccountNo = txtacno.Text.Trim().ToString();

                Bankname = txtbankname.Text.Trim().ToString();
                string SQLString = string.Empty;

                if (Transactionid != "" && string.IsNullOrEmpty(AccountNo) && string.IsNullOrEmpty(Bankname))
                {
                    SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid where StateID='" + HSRPStateID + "'  and TransactionID like '%" + Transactionid + "%' order by DepositDate desc";

                }

                else if (string.IsNullOrEmpty(Transactionid) && AccountNo !="" && string.IsNullOrEmpty(txtbankname.Text))
                {
                    SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid where StateID='" + HSRPStateID + "'   and  bm.AccountNo  like '%" + AccountNo + "%'    order by DepositDate desc";

                }
                else if (string.IsNullOrEmpty(Transactionid) &&  string.IsNullOrEmpty( AccountNo) && Bankname != "" )
                {
                    SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid where StateID='" + HSRPStateID + "'  and  bm.BankName  like '%" + Bankname + "%'   order by DepositDate desc";

                }

                else if (Transactionid != ""  && AccountNo != "" && string.IsNullOrEmpty(txtbankname.Text))
                {
                    SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid where StateID='" + HSRPStateID + "'   and TransactionID like '%" + Transactionid + "%'  and  bm.AccountNo  like '%" + AccountNo + "%'  order by DepositDate desc";

                }
                else if (Transactionid != "" && AccountNo != "" && Bankname != "")
                {
                    SQLString = "SELECT [voidstatus], [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],[DepositBy],r.RtoLocationName as DepositLocation,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate, convert(varchar(10), EntryDate, 103) as EntryDate, [BankSlipNo],[Remarks],bm.[AccountNo] FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.depositelocationid where StateID='" + HSRPStateID + "'  and  TransactionID like '%" + Transactionid + "%'  and  bm.AccountNo  like '%" + AccountNo + "%'   and  bm.BankName  like '%" + Bankname + "%'  order by DepositDate desc";

                }
                else
                {
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Not Valid Search";
                    buildGrid(); 
                    return;
                }

                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                if (dt.Rows.Count > 0)
                {

                    lblErrMsg.Text = string.Empty;
                    Grid1.DataSource = dt;
                    // Grid1.PageIndex = Convert.ToInt32(Request.QueryString["PageIndex"]);
                    Grid1.DataBind();
                   
                }
                else
                {
                   
                    buildGrid();
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Not Valid Search";
                     return;
                } 

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }


        }


     
       
    }
}