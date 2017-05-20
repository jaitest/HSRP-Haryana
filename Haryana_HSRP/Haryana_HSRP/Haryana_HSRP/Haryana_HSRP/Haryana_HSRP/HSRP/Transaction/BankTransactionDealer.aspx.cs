using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{


    public partial class BankTransactionDealer : System.Web.UI.Page
    {

        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty, DealerID = string.Empty;
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
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserName = Session["UID"].ToString();
                DealerID = Session["dealerid"].ToString();
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
                //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }

            //if (string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()))
            //{
            //    Response.Write("<script language='javascript'> {window.close();} </script>");
            //}
            //else
            //{
            //    StringMode = Request.QueryString["Mode"].ToString();
            //}

            if (StringMode.Equals("Edit"))
            {
                ProductivityID = Request.QueryString["TransactionID"].ToString();
                buttonUpdate.Visible = true;
                buttonSave.Visible = false;


            }
            else
            {
                buttonSave.Visible = true;
                //buttonUpdate.Visible = false;
            }
            if (!Page.IsPostBack)
            {
                FillddlLocation();
                BankName();
                if (StringMode.Equals("Edit"))
                {
                    EditBankTransaction(ProductivityID);


                }

                InitialSetting();
            }
        }



        public void FillddlLocation()
        {
            query1 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserName + "' and ActiveStatus='Y' order by RTOLocationName ";
            DataTable dt = Utils.GetDataTable(query1, ConnectionString);
            /* ddlLocation.DataSource = dt;
             ddlLocation.DataTextField = "RTOLocationName";
             ddlLocation.DataValueField = "RTOLocationID";
             ddlLocation.DataBind();
             ddlLocation.Items.Insert(0, "--Select Location Name--");
             ddlLocation.Items[0].Value = "0";*/

        }

        public void BankName()
        {
            if (UserType == "0")
            {


                SQLString = "select * from dbo.BankMaster where hsrp_stateid='" + HSRPStateID + "' and id not in (9,18,7,16,20)";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                DropDownListBankName.DataSource = dt;
                DropDownListBankName.DataTextField = "Bankname";
                DropDownListBankName.DataValueField = "id";
                DropDownListBankName.DataBind();
                DropDownListBankName.Items.Insert(0, new ListItem("Select Bank Name"));
                txtacc.Text = accno;
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

                //DropDownListBankName.Items.Insert(0, "--Select Bank Name--");
                //DropDownListBankName.Items[0].Value = "0";

            }
            else
            {

                SQLString = "select * from dbo.BankMaster where hsrp_stateid='" + HSRPStateID + "' and id not in (9,18,7,16,20)";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                DropDownListBankName.DataSource = dt;
                DropDownListBankName.DataTextField = "Bankname";
                DropDownListBankName.DataValueField = "id";
                DropDownListBankName.DataBind();
                DropDownListBankName.Items.Insert(0, new ListItem("Select Bank Name"));
                txtacc.Text = accno;

                //DropDownListBankName.Items.Insert(0, "--Select Bank Name--");
                //DropDownListBankName.Items[0].Value = "0";
            }




        }

        private void EditBankTransaction(string TransactionID)
        {
            SQLString = "SELECT [TransactionID],[DepositDate],[BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],[UserID],[CurrentDate],[BankSlipNo],[Remarks],[AccountNo],[depositelocationid] FROM [BankTransaction] where TransactionID=" + TransactionID;
            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            DepositDate.SelectedDate = DateTime.Parse(ds.Rows[0]["DepositDate"].ToString());
            // txtBankName.Text = ds.Rows[0]["BankName"].ToString();
            DropDownListBankName.SelectedValue = ds.Rows[0]["BankName"].ToString();
            txtBranchName.Text = ds.Rows[0]["BranchName"].ToString();
            TextBoxDepositAmount.Text = ds.Rows[0]["DepositAmount"].ToString();
            HSRPStateID = ds.Rows[0]["StateID"].ToString();
            RTOLocationID = ds.Rows[0]["RTOLocation"].ToString();
            UserName = ds.Rows[0]["UserID"].ToString();
            CurrentDate = ds.Rows[0]["CurrentDate"].ToString();
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (TextBoxDepositAmount.Text.ToString() == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Enter Some Amount";
                return;

            }
            if (DropDownListBankName.SelectedItem.ToString() == "Select Bank Name")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Please Select Bank Name";
                return;

            }
            if (DdltransactionMode.SelectedItem.ToString() == "--Select--")
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
            string sqlstring = "Select * from BankTransaction where  StateID='" + HSRPStateID + "' and UserID='"+UserName+"' and BankSlipNo='" + txtChecqNo.Text + "'";
            DataTable dt = Utils.GetDataTable(sqlstring, ConnectionString);
            if (dt.Rows.Count > 0)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "";
                lblErrMess.Text = "Duplicate Entry!.";
                return;
            }
            //DateTime OrderDate1;
       
            //String[] StringOrderDate = DepositDate.SelectedDate.ToString().Split('/');
            //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            try
            {
                if (int.Parse(TextBoxDepositAmount.Text) >= 1)
                {
                    List<string> lst = new List<string>();
                   // lst.Add(OrderDate1.ToString());
                    lst.Add(DepositDate.SelectedDate.ToString());              
                    lst.Add(DropDownListBankName.SelectedValue.ToString());  
                    lst.Add(txtBranchName.Text.ToString().ToUpper());
                    lst.Add(TextBoxDepositAmount.Text.ToString());
                    lst.Add(UserName);
                    lst.Add(HSRPStateID);
                    lst.Add(RTOLocationID);
                    lst.Add(UserName);
                    lst.Add(txtChecqNo.Text.ToString());
                    lst.Add("Remarks");
                    lst.Add(accno.ToString());
                    lst.Add(RTOLocationID);
                    lst.Add(DealerID);
                    lst.Add(DdltransactionMode.SelectedValue.ToString());
                    string strResult = string.Empty;
                    string TId = string.Empty;
                    bl.SaveBankTranctionDealer(lst, out strResult);
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


            txtBranchName.Text = "";
            TextBoxDepositAmount.Text = "";
            txtChecqNo.Text = "";

            FillddlLocation();
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

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string TransactionID1 = Request.QueryString["TransactionID"].ToString();
                DateTime OrderDate1;
                String[] StringOrderDate = DepositDate.SelectedDate.ToString().Split('/');
                String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
                OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));

                if (int.Parse(TextBoxDepositAmount.Text) >= 1)
                {
                    List<string> lst = new List<string>();
                    lst.Add(OrderDate1.ToString());
                    //lst.Add(txtBankName.Text.ToString().ToUpper());
                    lst.Add(DropDownListBankName.SelectedValue.ToString());
                    // lst.Add(ddlLocation.SelectedValue);
                    lst.Add(RTOLocationID);
                    lst.Add(txtBranchName.Text.ToString().ToUpper());
                    lst.Add(TextBoxDepositAmount.Text.ToString());
                    lst.Add("0");
                    lst.Add(HSRPStateID);
                    lst.Add(RTOLocationID);
                    lst.Add(UserName);

                    lst.Add("0");
                    lst.Add("0");
                    lst.Add("0");
                    lst.Add(TransactionID1);


                    int i = bl.UpdateBankTranction(lst);
                    if (i >= 1)
                    {
                        //    string script = "<script type=\"text/javascript\">  alert('Save Successfully');</script>";
                        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

                        //    referess();
                        string script = "<script type=\"text/javascript\">  alert('Updated Successfully');</script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                        referess();
                    }
                    else
                    {
                        string script = "<script type=\"text/javascript\">  alert('Updated not Successfully');</script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                        // referess();
                    }
                }
                else
                {
                    string script = "<script type=\"text/javascript\">  alert('Please Provide Some Amount');</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                    // referess();
                    TextBoxDepositAmount.Focus();
                }

            }
            catch (Exception ex)
            {
                string script = "<script type=\"text/javascript\">  alert('error message ');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                //referess();
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblTransactionID.Text = "";
            referess();
        }
        public static string accno;
        protected void DropDownListBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            accno = DropDownListBankName.SelectedValue.ToString();
            SQLString = "select accountno from dbo.BankMaster where id='" + DropDownListBankName.SelectedValue + "'";
            accno = Utils.GetDataTable(SQLString, ConnectionString).Rows[0]["accountno"].ToString();

            //if (DropDownListBankName.SelectedValue == "0")
            //{

            //}
            //else
            //{
            //    //SQLString = "select * from dbo.BankMaster where id='" + DropDownListBankName.SelectedValue.ToString() + "'";
            //    //DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
            //    //lblAccountNo.Text = dt.Rows[0]["accountno"].ToString();
            //}

        }
    }
}