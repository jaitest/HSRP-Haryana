using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using AjaxControlToolkit;
using System.Data.SqlClient;
using System.Configuration;

namespace HSRP.Transaction
{


    public partial class ViewBankTransactionApprovalDealer : System.Web.UI.Page
    {

        Utils bl = new Utils();
        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string UserType1 = string.Empty;
        string dealerid = string.Empty;

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
                CalendarExtender1.EndDate = System.DateTime.Now;
                // dealerid = Session["DealerID"].ToString();

                if (!IsPostBack)
                {
                    try
                    {
                        //buildGrid();
                        Filldropdowndealer("Dealername");

                        // Utils.user_log(strUserID, "View Organization", ComputerIP, "Page load", CnnString);
                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }

        }

        private void Filldropdowndealer(string preference)
        {
            if (preference == "Dealerid")
            {
                SQLString = "select distinct dm.DealerID,dm.dealername from dealermaster dm join BankTransaction bt on bt.dealerid=dm.dealerid where bt.stateid='" + HSRPStateID + "' order by dm.DealerID";
                DataTable dt = new DataTable();
                dt = Utils.GetDataTable(SQLString.ToString(), CnnString);
                ddlBothDealerHHT.DataSource = dt;
                ddlBothDealerHHT.DataTextField = "DealerID";
                ddlBothDealerHHT.DataValueField = "DealerID";
                ddlBothDealerHHT.DataBind();
                ddlBothDealerHHT.Items.Insert(0, new ListItem("--Select--", "--Select--"));
            }
            else
            {
                SQLString = "select distinct bt.DealerID,dm.dealername from dealermaster dm join BankTransaction bt on bt.dealerid=dm.dealerid where bt.stateid='" + HSRPStateID + "' order by dm.dealername";
                DataTable dt = new DataTable();
                dt = Utils.GetDataTable(SQLString.ToString(), CnnString);
                ddlBothDealerHHT.DataSource = dt;
                ddlBothDealerHHT.DataTextField = "dealername";
                ddlBothDealerHHT.DataValueField = "DealerID";
                ddlBothDealerHHT.DataBind();
                ddlBothDealerHHT.Items.Insert(0, new ListItem("--Select--", "--Select--"));
            }

        }

        public void buildGrid()
        {
            string dealerid = string.Empty;
            try
            {
                if (txtSearchByID.Text != "")
                {
                    string s = txtSearchByID.Text;
                    string[] words = s.Split(',');
                    dealerid = words[0].ToString();

                }
                else if (ddlBothDealerHHT.SelectedValue.ToString() != "--Select--")
                {
                    dealerid = ddlBothDealerHHT.SelectedValue.ToString();
                }
                else if (txtSearchByname.ToString() != "")
                {
                    string s = txtSearchByname.Text;
                    string[] words = s.Split(',');
                    dealerid = words[1].ToString();
                }
                string SQLString = "SELECT [TransactionID] ,convert(varchar(10), DepositDate, 103) AS 'Deposit Date',bm.[BankName],[BranchName],[DepositAmount],(select Userfirstname from users where dealerid='" + dealerid + "') as [DepositBy],r.RtoLocationName as DepositLocation,(Select dealername+','+' '+ convert(varchar,dealerid) from dealermaster where dealerid='" + dealerid + "') as Dealer,[StateID],[RTOLocation],[UserID],convert(varchar(10), CurrentDate, 103) as CurrentDate,[BankSlipNo],[Remarks],bm.[AccountNo],bt.ApprovedStatus,bt.ApprovedBy,convert(varchar(10), bt.ApprovedDate, 103) as 'ApprovedDate', bt.ChqrecStatus,bt.ChqrecBy,convert(varchar(10), bt.ChqrecDateTime, 103) as 'ChqrecDateTime',convert(varchar(10), bt.AmtClearDate, 103) as 'AmtClearDate',RejectedBy,convert(varchar(10), bt.RejectDate, 103) as 'RejectDate' FROM [dbo].[BankTransaction] bt inner join [dbo].[BankMaster] bm on bt.bankname=convert(varchar,bm.id) inner join Rtolocation r on r.rtolocationId=bt.rtolocation where StateID='" + HSRPStateID + "' and dealerid='" + dealerid + "' and userid in(select userid from users where isnull(dealerid,'')!='')  order by [TransactionID] desc";
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString());
                if (dt.Rows.Count > 0)
                {
                    grdid.DataSource = dt;
                    grdid.DataBind();
                    grdid.Visible = true;
                    lblErrMsg.Text = "";

                }
                else
                {
                    txtSearchByID.Text = "";
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Record not found";
                    grdid.DataSource = "";
                    grdid.DataBind();
                    grdid.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }

        protected void grdid_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            if (e.CommandName == "Approval")
            {
                LinkButton lnkView = (LinkButton)e.CommandSource;
                lblID.Text = lnkView.CommandArgument;
                this.ModalPopupExtender1.Show();
            }
            if (e.CommandName == "CheqeApproval")
            {
                LinkButton lnkView = (LinkButton)e.CommandSource;
                string TransactionID = lnkView.CommandArgument;

                if (TransactionID != "")
                {
                    string SQLString = "select UserFirstName+' '+UserLastName as UserName from [Users] where UserID='" + Session["UID"].ToString() + "'";
                    string username = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString()).Rows[0]["UserName"].ToString();

                    string sqlquery1 = "update banktransaction set ChqrecStatus='Y' , ChqrecBy='" + username + "' , ChqrecDateTime='" + System.DateTime.Now.ToShortDateString() + "' where TransactionID='" + TransactionID + "'";
                    int i = Utils.ExecNonQuery(sqlquery1, CnnString);
                    if (i > 0)
                    {
                        string SQLStrin = "Select * from users u,BankTransaction b where u.UserID=b.UserID and b.TransactionID='" + TransactionID + "'";
                        DataTable dt = Utils.GetDataTable(SQLStrin.ToString(), CnnString.ToString());//.Rows[0]["MobileNo"].ToString();
                        if (dt.Rows.Count > 0)
                        {
                           // string MobileNo = dt.Rows[0]["MobileNo"].ToString();
                            //if (HSRPStateID.ToString() == "11")
                            //{
                            //    if (MobileNo.Length > 0)
                            //    {
                            //        string SMSText = "Dear Customer, A cheque of Rs. " + dt.Rows[0]["DepositAmount"].ToString() + " has been received against your account ID " + dt.Rows[0]["UserLoginName"].ToString() + ". Cheque subject to realization. Thank You - LAPL";
                            //        string sendURL = "http://quick.smseasy.in:8080/bulksms/bulksms?username=sse-tlhsrp1&password=tlhsrp1&type=0&dlr=1&destination=" + MobileNo.ToString() + "&source=TSHSRP&message=" + SMSText;
                            //        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                            //        myRequest.Method = "GET";
                            //        WebResponse myResponse = myRequest.GetResponse();
                            //        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                            //        string result = sr.ReadToEnd();
                            //        sr.Close();
                            //        myResponse.Close();
                            //        System.Threading.Thread.Sleep(350);
                            //        Utils.ExecNonQuery("insert into TSSMSDetail(RtoLocationID,MobileNo,SentResponseCode,smstext) values('" + dt.Rows[0]["rtolocationid"].ToString() + "'," + MobileNo.ToString() + ",'" + result + "','" + SMSText + "')", CnnString);
                            //    }
                            //}
                            //else if (HSRPStateID.ToString() == "9")
                            //{
                            //    if (MobileNo.Length > 0)
                            //    {
                            //        string SMSText = "Dear Customer, A cheque of Rs. " + dt.Rows[0]["DepositAmount"].ToString() + " has been received against your account ID " + dt.Rows[0]["UserLoginName"].ToString() + ". Cheque subject to realization. Thank You - LAPL";
                            //        string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-aphsrp&password=aphsrp&type=0&dlr=1&destination=" + MobileNo.ToString() + "&source=APHSRP&message=" + SMSText;
                            //        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                            //        myRequest.Method = "GET";
                            //        WebResponse myResponse = myRequest.GetResponse();
                            //        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                            //        string result = sr.ReadToEnd();
                            //        sr.Close();
                            //        myResponse.Close();
                            //        System.Threading.Thread.Sleep(350);

                            //        Utils.ExecNonQuery("insert into APSMSDetail(RtoLocationID,MobileNo,SentResponseCode,smstext) values('" + dt.Rows[0]["rtolocationid"].ToString() + "'," + MobileNo.ToString() + ",'" + result + "','" + SMSText + "')", CnnString);
                            //    }
                            //}

                            lblSucMess.Text = "Cheqe Approved Successfully...";
                            buildGrid();
                        }

                    }
                    else
                    {
                        lblSucMess.Text = "Cheqe not Approved..";
                    }

                }
            }
            if (e.CommandName == "Rejected")
            {
                LinkButton lnkView = (LinkButton)e.CommandSource;
                lblTransactionId.Text = lnkView.CommandArgument;
                this.ModalPopupExtender2.Show();
            }

        }

        protected void btngo_Click(object sender, EventArgs e)
        {
            if (ddlSearch.SelectedValue.ToString() == "--Select--")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Select Search preference";
                return;
            }
            else
            {
                string Search = ddlSearch.SelectedValue.ToString();
                if (Search.ToString() == "Dealerid" || Search.ToString() == "Dealername")
                {
                    if (ddlBothDealerHHT.SelectedValue.ToString() == "--Select--")
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "";
                        lblErrMsg.Text = "Select Dealer";
                        return;
                    }
                    else
                    {
                        buildGrid();
                    }
                }
                else if (Search.ToString() == "ManualId")
                {
                    if (txtSearchByID.Text == "")
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "";
                        lblErrMsg.Text = "Enter Dealer Id";
                        return;
                    }
                    else
                    {
                        buildGrid();
                    }
                }
                else if (Search.ToString() == "ManualName")
                {
                    if (txtSearchByname.Text == "")
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "";
                        lblErrMsg.Text = "Enter Dealer Name";
                        return;
                    }
                    else
                    {
                        buildGrid();
                    }
                }
            }
        }

        protected void grdid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow gr in grdid.Rows)
            {
                LinkButton lnkView;
                LinkButton lnkChrec;
                LinkButton lnkRejected;
                string cell_1_Value = grdid.Rows[gr.RowIndex].Cells[9].Text;
                string cell_2_Value = grdid.Rows[gr.RowIndex].Cells[10].Text;
                string cell_3_Value = grdid.Rows[gr.RowIndex].Cells[11].Text;
                string cell_4_Value = grdid.Rows[gr.RowIndex].Cells[12].Text;
                string cell_5_Value = grdid.Rows[gr.RowIndex].Cells[13].Text;
                string cell_6_Value = grdid.Rows[gr.RowIndex].Cells[14].Text;

                if (cell_1_Value != "" && cell_1_Value != null && cell_2_Value != "" && cell_2_Value != null && cell_1_Value != "&nbsp;" && cell_2_Value != "&nbsp;")
                {
                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        lnkView = gr.FindControl("lnkView") as LinkButton;
                        lnkRejected = gr.FindControl("lnkRejected") as LinkButton;
                        lnkView.Visible = false;
                        lnkRejected.Visible = false;
                    }
                }
                if (cell_3_Value != "" && cell_3_Value != null && cell_4_Value != "" && cell_4_Value != null && cell_3_Value != "&nbsp;" && cell_4_Value != "&nbsp;")
                {
                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        lnkChrec = gr.FindControl("lnkChrec") as LinkButton;
                        lnkChrec.Visible = false;
                    }
                }
                if (cell_5_Value != "" && cell_5_Value != null && cell_6_Value != "" && cell_6_Value != null && cell_5_Value != "&nbsp;" && cell_6_Value != "&nbsp;")
                {
                    if (gr.RowType == DataControlRowType.DataRow)
                    {
                        lnkRejected = gr.FindControl("lnkRejected") as LinkButton;
                        lnkChrec = gr.FindControl("lnkChrec") as LinkButton;
                        lnkView = gr.FindControl("lnkView") as LinkButton;
                        lnkChrec.Visible = false;
                        lnkView.Visible = false;
                        lnkRejected.Visible = false;
                    }
                }

            }

        }
        protected void btnApproval(object sender, EventArgs e)
        {
            if (txtdate.Text.ToString() == "")
            {
                lblErrMsg.Visible = true;
                lblErrMsg.Text = "";
                lblErrMsg.Text = "Please Enter Amount Clearance Date";
                return;
            }
           

            string TransactionID = lblID.Text.ToString();
            if (TransactionID != "")
            {

                DateTime amtdate = DateTime.ParseExact(txtdate.Text, "dd/mm/yyyy", null);
                string SQLString = "select UserFirstName+' '+UserLastName as UserName from [Users] where UserID='" + Session["UID"].ToString() + "'";
                string username = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString()).Rows[0]["UserName"].ToString();

               // string sqlquery1 = "update banktransaction set ApprovedStatus='Y' , ApprovedBy='" + username + "' , ApprovedDate='" + System.DateTime.Now.ToShortDateString() + "',AmtClearDate='" + amtdate + "' where TransactionID='" + TransactionID + "' and isnull(ChqrecBy,'')!='' and isnull(ChqrecDateTime,'')!='' and isnull(chqrecstatus,'')!='' and chqrecstatus='Y'";
                string sqlquery1 = "update banktransaction set ApprovedStatus='Y' , ApprovedBy='" + username + "' , ApprovedDate='" + System.DateTime.Now.ToShortDateString() + "',AmtClearDate='" + System.DateTime.Now.ToShortDateString() + "' where TransactionID='" + TransactionID + "' and isnull(ChqrecBy,'')!='' and isnull(ChqrecDateTime,'')!='' and isnull(chqrecstatus,'')!='' and chqrecstatus='Y'";
                int i = Utils.ExecNonQuery(sqlquery1, CnnString);
                if (i > 0)
                {
                    string SQLStrin = "Select * from users u,BankTransaction b where u.UserID=b.UserID and b.TransactionID='" + TransactionID + "'";
                    DataTable dt = Utils.GetDataTable(SQLStrin.ToString(), CnnString.ToString());//.Rows[0]["MobileNo"].ToString();
                    if (dt.Rows.Count > 0)
                    {
                        string MobileNo = dt.Rows[0]["MobileNo"].ToString();
                        //if (HSRPStateID.ToString() == "11")
                        //{
                        //    if (MobileNo.Length > 0)
                        //    {
                        //        string SMSText = "Dear Customer, amount of Rs. " + dt.Rows[0]["DepositAmount"].ToString() + " has been credited to your account ID " + dt.Rows[0]["UserLoginName"].ToString() + " for a Cheque/RTGS payment. Transaction ID - " + dt.Rows[0]["TransactionID"].ToString() + " Thank You - LAPL";
                        //        string sendURL = "http://quick.smseasy.in:8080/bulksms/bulksms?username=sse-tlhsrp1&password=tlhsrp1&type=0&dlr=1&destination=" + MobileNo.ToString() + "&source=TSHSRP&message=" + SMSText;
                        //        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                        //        myRequest.Method = "GET";
                        //        WebResponse myResponse = myRequest.GetResponse();
                        //        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                        //        string result = sr.ReadToEnd();
                        //        sr.Close();
                        //        myResponse.Close();
                        //        System.Threading.Thread.Sleep(350);
                        //        Utils.ExecNonQuery("insert into TSSMSDetail(RtoLocationID,MobileNo,SentResponseCode,smstext) values('" + dt.Rows[0]["rtolocationid"].ToString() + "'," + MobileNo.ToString() + ",'" + result + "','" + SMSText + "')", CnnString);
                        //    }
                        //}
                        //else if (HSRPStateID.ToString() == "9")
                        //{
                        //    if (MobileNo.Length > 0)
                        //    {
                        //        string SMSText = "Dear Customer, amount of Rs. " + dt.Rows[0]["DepositAmount"].ToString() + " has been credited to your account ID " + dt.Rows[0]["UserLoginName"].ToString() + " for a Cheque/RTGS payment. Transaction ID - " + dt.Rows[0]["TransactionID"].ToString() + " Thank You - LAPL";
                        //        string sendURL = "http://103.16.101.52:8080/bulksms/bulksms?username=sse-aphsrp&password=aphsrp&type=0&dlr=1&destination=" + MobileNo.ToString() + "&source=APHSRP&message=" + SMSText;
                        //        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(sendURL);
                        //        myRequest.Method = "GET";
                        //        WebResponse myResponse = myRequest.GetResponse();
                        //        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                        //        string result = sr.ReadToEnd();
                        //        sr.Close();
                        //        myResponse.Close();
                        //        System.Threading.Thread.Sleep(350);

                        //        Utils.ExecNonQuery("insert into APSMSDetail(RtoLocationID,MobileNo,SentResponseCode,smstext) values('" + dt.Rows[0]["rtolocationid"].ToString() + "'," + MobileNo.ToString() + ",'" + result + "','" + SMSText + "')", CnnString);
                        //    }
                        //}
                        lblSucMess.Text = "Record Approved Successfully...";
                        buildGrid();
                    }
                }
                else
                {
                    lblSucMess.Text = "Record not Approved..";
                }

            }
        }

        protected void btnReject(object sender, EventArgs e)
        {
            string TransactionID = lblTransactionId.Text;
            if (TransactionID != "")
            {
                if (txtRemarks.Text.ToString() == "")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = "Please Enter Rejected Summary";
                    return;
                }
                string SQLString = "select UserFirstName+' '+UserLastName as UserName from [Users] where UserID='" + Session["UID"].ToString() + "'";
                string username = Utils.GetDataTable(SQLString.ToString(), CnnString.ToString()).Rows[0]["UserName"].ToString();

                string sqlquery1 = "update banktransaction set ApprovedStatus='R' , RejectedBy='" + username + "' , RejectDate='" + System.DateTime.Now.ToShortDateString() + "',RejectSummary='" + txtRemarks.Text + "' where TransactionID='" + TransactionID + "'";
                int i = Utils.ExecNonQuery(sqlquery1, CnnString);
                if (i > 0)
                {
                    lblSucMess.Text = "Record Rejected Successfully...";
                    buildGrid();
                }
                else
                {
                    lblSucMess.Text = "Record not Rejected..";
                }

            }
        }

        protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSearch.SelectedValue == "Dealerid")
            {
                Filldropdowndealer("Dealerid");
                txtSearchByID.Visible = false;
                txtSearchByID.Text = "";
                txtSearchByname.Text = "";
                ddlBothDealerHHT.Visible = true;
                txtSearchByname.Visible = false;
                grdid.DataSource = "";
                grdid.DataBind();
                grdid.Visible = true;
                lblErrMsg.Text = "";
            }
            else if (ddlSearch.SelectedValue == "--Select--")
            {
                Filldropdowndealer("Dealername");
                txtSearchByID.Visible = false;
                txtSearchByID.Text = "";
                txtSearchByname.Text = "";
                ddlBothDealerHHT.Visible = true;
                txtSearchByname.Visible = false;
                grdid.DataSource = "";
                grdid.DataBind();
                grdid.Visible = true;
                lblErrMsg.Text = "";
            }
            else if (ddlSearch.SelectedValue == "ManualId")
            {
                ddlBothDealerHHT.Visible = false;
                txtSearchByID.Visible = true;
                txtSearchByname.Visible = false;
                grdid.DataSource = "";
                txtSearchByname.Text = "";
                grdid.DataBind();
                grdid.Visible = true;
                txtSearchByID.Text = "";
                lblErrMsg.Text = "";
            }
            else if (ddlSearch.SelectedValue == "ManualName")
            {
                txtSearchByname.Text = "";
                ddlBothDealerHHT.Visible = false;
                txtSearchByID.Visible = false;
                txtSearchByname.Visible = true;
                grdid.DataSource = "";
                grdid.DataBind();
                grdid.Visible = true;
                txtSearchByID.Text = "";
                lblErrMsg.Text = "";
            }
            else
            {
                Filldropdowndealer("Dealername");
                txtSearchByID.Visible = false;
                ddlBothDealerHHT.Visible = true;
                txtSearchByname.Visible = false;
                grdid.DataSource = "";
                txtSearchByname.Text = "";
                grdid.DataBind();
                grdid.Visible = true;
                txtSearchByID.Text = "";
                lblErrMsg.Text = "";
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchCustomers(string prefixText, int count)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select distinct bt.DealerID+','+' '+dm.dealername as dealer from dealermaster dm join BankTransaction bt on bt.dealerid=dm.dealerid where  " +
                    " bt.DealerID like @SearchText + '%'";
                    cmd.Parameters.AddWithValue("@SearchText", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    List<string> customers = new List<string>();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(sdr["dealer"].ToString());
                        }
                    }
                    conn.Close();
                    return customers;
                }
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchByDealerName(string prefixText, int count)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select distinct (dm.dealername +' '+','+ CONVERT(varchar,dm.DealerID))  as dealer from dealermaster dm join BankTransaction bt on bt.dealerid=dm.dealerid where " +
                    " dm.dealername like @SearchText + '%'";
                    cmd.Parameters.AddWithValue("@SearchText", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    List<string> customers = new List<string>();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(sdr["dealer"].ToString());
                        }
                    }
                    conn.Close();
                    return customers;
                }
            }
        }
    
    
    
    }
}