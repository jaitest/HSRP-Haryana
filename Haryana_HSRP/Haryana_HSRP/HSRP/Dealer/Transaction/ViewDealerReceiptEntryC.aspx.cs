using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using DataProvider;

namespace HSRP.Dealer.Transaction
{
    public partial class ViewDealerReceiptEntryC : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string StringMode = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {

            lblErrMess.Text = "";
            lblSucMess.Text = "";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringDealerHSRP"].ToString();
            InitialSetting();

            buttonUpdate.Visible = false;
            btnSave.Visible = true;
            if (!IsPostBack)
            {
                BL.blEntryDetail bl = new BL.blEntryDetail();
                bl.DailyEnityDeatail(Session["UID"].ToString(), "DealerReceiptEntryC.aspx");

                DealerName();
                //StringMode = Request.QueryString["Mode"].ToString();

                if (StringMode.Equals("Edit"))
                {
                    edit();
                    buttonUpdate.Visible = true;
                    btnSave.Visible = false;
                }
                txtChequeNo.Attributes.Add("onkeypress", "return isNumberKey(event);");
                txtChequeAmount.Attributes.Add("onkeypress", "return isNumberKey(event);");
                // ddlDealerName.Attributes.Add("onChange", "return OnSelectedIndexChangeDealer();");
            }
        }
        DataTable dt = new DataTable();
        public void DealerName()
        {
            SQLString = "select DealerId ,dealername from dbo.dealerMaster order by DealerId";
            dt = Utils.GetDataTable(SQLString, CnnString);
            ddlDealerName.DataSource = dt;
            ddlDealerName.DataTextField = "dealername";
            ddlDealerName.DataValueField = "DealerId";
            ddlDealerName.DataBind();
            ddlDealerName.Items.Insert(0, "--Select Dealer Name--");
            ddlDealerName.Items[0].Value = "--Select Dealer Name--";
        }

        public void edit()
        {
            SQLString = "select * from dbo.DealerReceiptEntry where Id ='" + Request.QueryString["ID"].ToString() + "'";
            dt = Utils.GetDataTable(SQLString, CnnString);
            OrderDate.SelectedDate = DateTime.Parse(dt.Rows[0]["ChequeDate"].ToString());
            //HSRPAuthDate.SelectedDate = DateTime.Parse(dt.Rows[0]["ReceivedDate"].ToString());
            ddlDealerName.SelectedValue = dt.Rows[0]["DealerID"].ToString();
            txtChequeAmount.Text = dt.Rows[0]["ChequeAmount"].ToString();
            txtChequeNo.Text = dt.Rows[0]["ChequeNo"].ToString();
            txtDrawnBankName.Text = dt.Rows[0]["DrawnBankForm"].ToString();
            txtDeliveredBy.Text = dt.Rows[0]["ChequeDeliveredBy"].ToString();
            // dealerID.Value = dt.Rows[0]["DealerID"].ToString();
        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            //HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate).AddDays(30);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            // CheckDeliveredDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00); 
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (ddlDealerName.SelectedValue == "--Select Dealer Name--")
            //{
            //    lblErrMess.Text = "Please select correct Dealer Name.";
            //    return;
            //}

            SQLString = "select count(*)as co from dbo.DealerReceiptEntry where chequeNo='" + txtChequeNo.Text + "' and DrawnBankForm='" + txtDrawnBankName.Text + "'";
            int count = Utils.getScalarCount(SQLString, CnnString);
            if (count > 0)
            {
                lblErrMess.Text = "Cheque No and Bank Name Already Exists.";
                // lblErrMess.Text = "duplicate record ";

                //string script = "<script type=\"text/javascript\">  alert('Cheque No and Bank Name Already Exists.');</script>";
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                //txtChequeNo.Focus();
            }
            else
            {
                ///>>> OrderDate means Cheque Date
                ///>>> OrderDate means HSRPAuthDate Received Date
                SQLString = "insert into  dbo.DealerReceiptEntry (OrgID ,ChequeDate,DealerID,ChequeAmount,ChequeNo,DrawnBankForm," +
                "ChequeDeliveredBy) Values(2,'" + OrderDate.SelectedDate + "'," +
                "" + ddlDealerName.SelectedValue.ToString() + ",'" + txtChequeAmount.Text + "','" + txtChequeNo.Text + "','" + txtDrawnBankName.Text + "'," +
                "'" + txtDeliveredBy.Text + "')";
                int i = Utils.ExecNonQuery(SQLString, CnnString);
                if (i > 0)
                {
                    lblSucMess.Text = "Saved Successfully";
                    Refresh();
                }
                else
                {
                    lblErrMess.Text = "Not Added.";
                    Refresh();
                }
            }
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (ddlDealerName.SelectedValue == "--Select Dealer Name--")
            {
                lblErrMess.Text = "Please select correct Dealer Name.";
                return;
            }
            SQLString = "update dbo.DealerReceiptEntry set  ChequeDate='" + OrderDate.SelectedDate + "'" +
            ",DealerID='" + ddlDealerName.SelectedValue.ToString() + "',ChequeAmount='" + txtChequeAmount.Text + "'," +
            "ChequeNo='" + txtChequeNo.Text + "',DrawnBankForm='" + txtDrawnBankName.Text + "',ChequeDeliveredBy='" + txtDeliveredBy.Text + "'" +
            " where Id ='" + Request.QueryString["ID"].ToString() + "'";
            int i = Utils.ExecNonQuery(SQLString, CnnString);
            if (i > 0)
            {
                lblSucMess.Text = "Update Successfully.";
                Refresh();
            }
            else
            {
                lblErrMess.Text = "Not Updated.";
                Refresh();
            }
        }

        public void Refresh()
        {
            txtChequeAmount.Text = "";
            txtChequeNo.Text = "";
            txtDeliveredBy.Text = "";
            txtDrawnBankName.Text = "";
            DealerName();
            InitialSetting();

        }
    }
}