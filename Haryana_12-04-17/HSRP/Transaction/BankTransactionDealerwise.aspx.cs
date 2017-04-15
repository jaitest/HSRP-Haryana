using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class BankTransactionDealerwise : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserId =  string.Empty, UserName = string.Empty, DealerID = string.Empty;
        string SQLString = string.Empty;
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        string query1 = string.Empty;

        DataProvider.BAL bl = new DataProvider.BAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
                HSRPStateID ="4";
                //RTOLocationID = Session["UserRTOLocationID"].ToString();
                 UserName = Session["UID"].ToString();

                 string sqlusername = "select UserFirstName +' '+userlastname as username from users where hsrp_stateid = 4  and userid = '" + UserName.ToString() + "'";
                 DataTable dtusername = Utils.GetDataTable(sqlusername, ConnectionString);
                 if (dtusername.Rows.Count > 0)
                 {
                     Session["UserName"] = dtusername.Rows[0]["username"].ToString();
                 }
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
                DepositDate.Enabled = false;
                
            }

         
                buttonSave.Visible = true;
               
            if (!Page.IsPostBack)
            {               
                BankName();
                ReportType();
                DealerName();             

                InitialSetting();
            }
        }



        public void BankName()
        { 
                SQLString = "select * from dbo.BankMaster where hsrp_stateid='4' and banktype = 'D'";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                DropDownListBankName.DataSource = dt;
                DropDownListBankName.DataTextField = "Bankname";
                DropDownListBankName.DataValueField = "id";
                DropDownListBankName.DataBind();
                DropDownListBankName.Items.Insert(0, new ListItem("--Select Bank Name---"));
                txtacc.Text = accno;


        }

        public void ReportType()
        {
            SQLString = "select bankcode from dbo.BankMaster where hsrp_stateid='4' and banktype = 'M'";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                DdltransactionMode.DataSource = dt;
                DdltransactionMode.DataTextField = "bankcode";
                DdltransactionMode.DataValueField = "bankcode";
                DdltransactionMode.DataBind();
                DdltransactionMode.Items.Insert(0, new ListItem("--Select Payment Mode------"));               

        }

        public void DealerName()
        {
            SQLString = " select Dealerid,    CONCAT(Dealerid, ',', DealerName) as DealerName  from dealermaster where HSRP_StateID='4' and IsActive = 1 ";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                DropDownListDealer.DataSource = dt;         
                DropDownListDealer.DataTextField = "DealerName"; 
                DropDownListDealer.DataValueField = "Dealerid";
                DropDownListDealer.DataBind();
                DropDownListDealer.Items.Insert(0, new ListItem("--Select Dealer Name---"));

        }

       

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {

            if (DropDownListDealer.SelectedItem.Text.ToString() == "--Select Dealer Name---")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Select Dealer Name";
                return;

            }
            else {
                SQLString = "select   rtolocationid from dealermaster where HSRP_StateID='4' and IsActive = 1 and Dealerid= '" + DropDownListDealer.SelectedValue.ToString()+ "'";
                DataTable dt1 = Utils.GetDataTable(SQLString, ConnectionString);

               string   SQLString2 = "select   userid  from users where HSRP_StateID='4' and  activestatus = 'Y' and Dealerid= '" + DropDownListDealer.SelectedValue.ToString() + "'";
                DataTable dt22 = Utils.GetDataTable(SQLString2, ConnectionString);

                if (dt22.Rows.Count > 0)
                {
                    UserId = dt22.Rows[0]["userid"].ToString();

                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = " Invalid Dealer Not Map With Process.";
                    return;
 
                }
              


                if (dt1.Rows.Count > 0)
                {
                    RTOLocationID = dt1.Rows[0]["rtolocationid"].ToString(); 
                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Select Dealer Name";
                    return;

                }
             
            }

            if (TextBoxDepositAmount.Text.ToString() == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Enter Some Amount";
                return;

            }
            if (DropDownListBankName.SelectedItem.ToString() == "--Select Bank Name---")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Select Bank Name";
                return;

            }
            if (DdltransactionMode.SelectedItem.ToString() == "--Select Payment Mode------")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Select Payment Mode";
                return;

            }
            if (txtChecqNo.Text.ToString() == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Enter Payment Mode";
                return;

            }
            if (txtBranchName.Text.ToString() == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Enter Bank Address";
                return;

            }
            string sqlstring = "Select * from BankTransaction where  StateID='" + HSRPStateID + "' and UserID='" + UserId + "' and BankSlipNo='" + txtChecqNo.Text + "'";
            DataTable dt = Utils.GetDataTable(sqlstring, ConnectionString);
            if (dt.Rows.Count > 0)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Duplicate Entry!.";
                return;
            }

             

            try
            {
                if (int.Parse(TextBoxDepositAmount.Text) >= 1)
                {
                    List<string> lst = new List<string>();
                    lst.Add(DepositDate.SelectedDate.ToString());
                    lst.Add(DropDownListBankName.SelectedValue.ToString());
                    lst.Add(txtBranchName.Text.ToString().ToUpper());
                    lst.Add(TextBoxDepositAmount.Text.ToString());
                    lst.Add(UserId);
                    lst.Add(HSRPStateID);
                    lst.Add(RTOLocationID);
                    lst.Add(UserId);
                    lst.Add(txtChecqNo.Text.ToString());

                    lst.Add("Credit Given By " + Session["UserName"].ToString());
                    lst.Add(accno.ToString());
                    lst.Add(RTOLocationID);
                    lst.Add(DropDownListDealer.SelectedValue.ToString());
                    lst.Add(DdltransactionMode.SelectedValue.ToString());
                    string strResult = string.Empty;
                    string TId = string.Empty;
                     bl.SaveBankTranctionDealerwise(lst, out strResult);
                    lblTransactionID.Text = "";
                    lblTransactionID.Text = strResult.ToString();
                    referess();
                }
                else
                {

                    string script = "<script type=\"text/javascript\">  alert('Please Provide Some Amount');</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                    TextBoxDepositAmount.Focus();
                }

            }
            catch (Exception ex)
            {
                string script = "<script type=\"text/javascript\">  alert('error message ');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                TextBoxDepositAmount.Focus();
            }

        }

        public void referess()
        {


            BankName();
            ReportType();


            txtBranchName.Text = "";
            TextBoxDepositAmount.Text = "";
            txtChecqNo.Text = "";

           // FillddlLocation();
            DealerName();
        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            CalendarDepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDepositDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            DepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            DepositDate.MaxDate = DateTime.Parse(MaxDate);
        }

       

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblTransactionID.Text = "";
            referess();
        }
        public static string accno;
        protected void DropDownListBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListBankName.SelectedItem.Text.ToString() != "--Select Bank Name---")
            {
                accno = DropDownListBankName.SelectedValue.ToString();
                SQLString = "select accountno from dbo.BankMaster where id='" + DropDownListBankName.SelectedValue + "'";
                accno = Utils.GetDataTable(SQLString, ConnectionString).Rows[0]["accountno"].ToString();

            }

           

        }
    }
}