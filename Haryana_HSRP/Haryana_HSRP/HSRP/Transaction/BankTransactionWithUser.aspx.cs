﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;

namespace HSRP.Master
{
    public partial class BankTransactionWithUser : System.Web.UI.Page
    {



        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
        string SQLString = string.Empty;
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        string query1 = string.Empty;
        string UserFirstName = string.Empty;

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
                UserFirstName = ddl_user.SelectedValue;
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
                FillUsers();
                //FillddlLocation();
                BankName();

                Filldropdowndealer();
                if (StringMode.Equals("Edit"))
                {
                    EditBankTransaction(ProductivityID);


                }

                InitialSetting();
            }
            //  FillUsers();

        }

        public void FillUsers()
        {
            // query1 = "select RTOLocationName,RTOLocationID from RTOLocation where hsrp_stateid='" + HSRPStateID + "' and RTOLocationID='" + RTOLocationID + "' and ActiveStatus='Y' order by RTOLocationName ";
            query1 = "SELECT UserID,UserLoginname FROM USERS WHERE HSRP_STATEID='" + HSRPStateID + "' and UserID='" + UserName + "' and ActiveStatus='Y' order by UserLoginname asc";
            DataTable dt = Utils.GetDataTable(query1, ConnectionString);
            ddl_user.DataSource = dt;
            ddl_user.DataTextField = "UserLoginname";
            ddl_user.DataValueField = "UserID";
            ddl_user.DataBind();
            ddl_user.Items.Insert(0, "--Select User Name--");
            ddl_user.Items[0].Value = "0";

        }


        private void Filldropdowndealer()
        {
            if (UserType == "0")
            {
                //ddlBothDealerHHT.Visible = false;
                
               SQLString = "select DealerId,dealername from dealerMaster where hsrp_stateID='" + HSRPStateID + "' order by dealername asc";              
                //Utils.PopulateDropDownList(ddlBothDealerHHT1, SQLString.ToString(), ConnectionString, "--Select Dealer Name--");
                DataTable dt1 = Utils.GetDataTable(SQLString, ConnectionString);
                ddlBothDealerHHT1.DataSource = dt1;
                ddlBothDealerHHT1.DataTextField = "dealername";
                ddlBothDealerHHT1.DataValueField = "DealerId";
                ddlBothDealerHHT1.DataBind();
                ddlBothDealerHHT1.Items.Insert(0, "--Select Dealer Name--");
                ddlBothDealerHHT1.Items[0].Value = "0";



            }
            else
            {
                //ddlBothDealerHHT.Visible = false;
               SQLString = "select DealerId,dealername from dealerMaster where hsrp_stateID='" + HSRPStateID + "' order by dealername asc";
                
                //Utils.PopulateDropDownList(ddlBothDealerHHT1, SQLString.ToString(), ConnectionString, "--Select Dealer Name--");
                DataTable dt1 = Utils.GetDataTable(SQLString, ConnectionString);
                ddlBothDealerHHT1.DataSource = dt1;
                ddlBothDealerHHT1.DataTextField = "dealername";
                ddlBothDealerHHT1.DataValueField = "DealerId";
                ddlBothDealerHHT1.DataBind();
                ddlBothDealerHHT1.Items.Insert(0, "--Select Dealer Name--");
                ddlBothDealerHHT1.Items[0].Value = "0";
            }
        }


        //public void FillddlLocation()
        //{ 
            
        //    // query1 = "select RTOLocationName,RTOLocationID from RTOLocation where hsrp_stateid='" + HSRPStateID + "' and RTOLocationID='" + RTOLocationID + "' and ActiveStatus='Y' order by RTOLocationName ";
        //    query1 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserFirstName + "' and ActiveStatus='Y' order by RTOLocationName ";
        //    DataTable dt = Utils.GetDataTable(query1, ConnectionString);
        //    ddlLocation.DataSource = dt;
        //    ddlLocation.DataTextField = "RTOLocationName";
        //    ddlLocation.DataValueField = "RTOLocationID";
        //    ddlLocation.DataBind();
        //    ddlLocation.Items.Insert(0, "--Select Location Name--");
        //    ddlLocation.Items[0].Value = "0";

        //}

        public void BankName()
        {
            if (UserType == "0")
            {


                SQLString = "select * from dbo.BankMaster where hsrp_stateid='" + HSRPStateID + "'";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                DropDownListBankName.DataSource = dt;
                DropDownListBankName.DataTextField = "Bankname";
                DropDownListBankName.DataValueField = "id";
                DropDownListBankName.DataBind();
                // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

                DropDownListBankName.Items.Insert(0, "--Select Bank Name--");
                DropDownListBankName.Items[0].Value = "0";

            }
            else
            {

                SQLString = "select * from dbo.BankMaster where hsrp_stateid='" + HSRPStateID + "'";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                DropDownListBankName.DataSource = dt;
                DropDownListBankName.DataTextField = "Bankname";
                DropDownListBankName.DataValueField = "id";
                DropDownListBankName.DataBind();

                DropDownListBankName.Items.Insert(0, "--Select Bank Name--");
                DropDownListBankName.Items[0].Value = "0";
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
            TextBoxDepositby.Text = ds.Rows[0]["DepositBy"].ToString();
            HSRPStateID = ds.Rows[0]["StateID"].ToString();
            RTOLocationID = ds.Rows[0]["RTOLocation"].ToString();
            UserName = ds.Rows[0]["UserID"].ToString();
            CurrentDate = ds.Rows[0]["CurrentDate"].ToString();
            TextBoxBankSlipNo.Text = ds.Rows[0]["BankSlipNo"].ToString();
            TextBoxRemarks.Text = ds.Rows[0]["Remarks"].ToString();
            lblAccountNo.Text = ds.Rows[0]["AccountNo"].ToString();
            hdnrtolocation.Value = ds.Rows[0]["depositelocationid"].ToString();
            //ddlLocation.SelectedValue = ds.Rows[0]["depositelocationid"].ToString();
            // TransactionID1 = ds.Rows[0]["TransactionID"].ToString(); 

        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            DateTime OrderDate1;
            //String[] StringOrderDate = DepositDate.SelectedDate.ToString().Split('/');
            //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            String[] StringOrderDate = DepositDate.SelectedDate.ToString().Split('/');
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            // OrderDate1 = System.DateTime.Now;

            DateTime OrderDate2;
            //String[] StringOrderDate = DepositDate.SelectedDate.ToString().Split('/');
            //String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            //OrderDate1 = new DateTime(Convert.ToInt32(StringOrderDate[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate[0]), Convert.ToInt32(StringOrderDate[1]));
            String[] StringOrderDate2 = DepositDate1.SelectedDate.ToString().Split('/');
            String From2 = StringOrderDate2[0] + "/" + StringOrderDate2[1] + "/" + StringOrderDate2[2].Split(' ')[0];
            OrderDate2 = new DateTime(Convert.ToInt32(StringOrderDate2[2].Split(' ')[0]), Convert.ToInt32(StringOrderDate2[0]), Convert.ToInt32(StringOrderDate2[1]));
            // OrderDate1 = System.DateTime.Now;


            string checkno = txtcheckno.Text;
            //string DealeridDL = ddlBothDealerHHT.SelectedValue;
            string Dealerid = ddlBothDealerHHT1.SelectedValue;

            try
            {
                UserFirstName = ddl_user.SelectedValue;
                if (int.Parse(TextBoxDepositAmount.Text) >= 1)
                {
                    List<string> lst = new List<string>();
                    lst.Add(OrderDate1.ToString());
                    //lst.Add(txtBankName.Text.ToString().ToUpper());
                    lst.Add(DropDownListBankName.SelectedValue.ToString());
                    lst.Add(txtBranchName.Text.ToString().ToUpper());
                    lst.Add(TextBoxDepositAmount.Text.ToString());
                    lst.Add(TextBoxDepositby.Text.ToString());
                    lst.Add(HSRPStateID);
                    lst.Add(RTOLocationID);
                    lst.Add(UserFirstName);
                    lst.Add(TextBoxBankSlipNo.Text);
                    lst.Add(TextBoxRemarks.Text);
                    lst.Add(lblAccountNo.Text);
                    lst.Add(hdnrtolocation.Value.ToString());
                    //lst.Add(hdnrtolocation ddlLocation.SelectedValue.ToString());
                    lst.Add(Dealerid);                              
                    lst.Add(OrderDate2.ToString());
                    lst.Add(checkno);
                    lst.Add(txtChequeBankName.Text);

                    string strResult = string.Empty;
                    bl.SaveBankTranctionwithUser(lst, out strResult);



                    //string script = "<script type=\"text/javascript\">  alert('" + strResult + "');</script>";
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                    lblSucMess.Text = strResult;
                    referess();

                    // string sql = "select TransactionID from BankTransaction where ";



                }
                else
                {

                    //string script = "<script type=\"text/javascript\">  alert('Please Provide Some Amount');</script>";
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                    // referess();
                    lblErrMess.Text = "Please Provide Some Amount";
                    lblSucMess.Text = "";
                    TextBoxDepositAmount.Focus();
                }

            }
            catch (Exception ex)
            {
                //string script = "<script type=\"text/javascript\">  alert('error message ');</script>";
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                lblErrMess.Text = "error message ";
                lblSucMess.Text = "";
                // referess();
                TextBoxDepositAmount.Focus();
            }

        }

        public void referess()
        {

            BankName();
            lblAccountNo.Text = "";

            txtBranchName.Text = "";
            TextBoxDepositAmount.Text = "";
            TextBoxDepositby.Text = "";
            TextBoxBankSlipNo.Text = "";
            TextBoxRemarks.Text = "";
            //FillddlLocation();
            txtcheckno.Text = "";
            ddlBothDealerHHT1.ClearSelection();
            lblDepositedLocation.Text = "";
            hdnrtolocation.Value = "";
            txtChequeBankName.Text = "";
            //lblErrMess.Text = "";
            //lblSucMess.Text = "";
            
        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            CalendarDepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDepositDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            DepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            DepositDate.MaxDate = DateTime.Parse(MaxDate);

            Calendar2.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            Calendar2.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

            DepositDate1.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
           // DepositDate1.MaxDate = DateTime.Parse(MaxDate);
        }


        //private void TextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        //{
        //    if (Char.IsDigit(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Textbox must be numeric only!");
        //    }
        //}


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
                    lst.Add(hdnrtolocation.Value);
                    //lst.Add(ddlLocation.SelectedValue);
                    lst.Add(txtBranchName.Text.ToString().ToUpper());
                    lst.Add(TextBoxDepositAmount.Text.ToString());
                    lst.Add(TextBoxDepositby.Text);
                    lst.Add(HSRPStateID);
                    lst.Add(RTOLocationID);
                    lst.Add(UserName);

                    lst.Add(TextBoxBankSlipNo.Text);
                    lst.Add(TextBoxRemarks.Text);
                    lst.Add(lblAccountNo.Text);
                    lst.Add(TransactionID1);


                    int i = bl.UpdateBankTranction(lst);
                    if (i >= 1)
                    {
                        //    string script = "<script type=\"text/javascript\">  alert('Save Successfully');</script>";
                        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

                        //    referess();
                        //string script = "<script type=\"text/javascript\">  alert('Updated Successfully');</script>";
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                        lblSucMess.Text = "Updated Successfully";
                        referess();
                    }
                    else
                    {
                        //string script = "<script type=\"text/javascript\">  alert('Updated not Successfully');</script>";
                        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                        // referess();
                        lblErrMess.Text = "Updated not Successfully";
                        lblSucMess.Text = "";
                    }
                }
                else
                {
                    //string script = "<script type=\"text/javascript\">  alert('Please Provide Some Amount');</script>";
                    //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                    lblErrMess.Text = "Please Provide Some Amount";
                    lblSucMess.Text = "";
                    // referess();
                    TextBoxDepositAmount.Focus();
                }

            }
            catch (Exception ex)
            {
                //string script = "<script type=\"text/javascript\">  alert('error message ');</script>";
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                lblErrMess.Text = "error message";
                lblSucMess.Text = "";
                //referess();
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            referess();
            lblErrMess.Text = "";
            lblSucMess.Text = "";
        }

        protected void DropDownListBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListBankName.SelectedValue == "0")
            {
                lblAccountNo.Text = "";
            }
            else
            {
                SQLString = "select * from dbo.BankMaster where id='" + DropDownListBankName.SelectedValue.ToString() + "'";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                lblAccountNo.Text = dt.Rows[0]["accountno"].ToString();
            }

        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LiveReports/LiveTracking.aspx");
        }

        protected void ddl_user_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSucMess.Text = "";
            lblErrMess.Text = "";
            //FillddlLocation();
        }

        protected void ddlBothDealerHHT1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            string sqlquery = "Select RTOLocationID,RTOLocationName from RTOLocation where RTOLocationID=(select RTOLocationID from dealerMaster where hsrp_stateID='" + HSRPStateID + "' and DealerId='" + ddlBothDealerHHT1.SelectedValue + "')";
            DataTable dt= new DataTable();
            dt=Utils.GetDataTable(sqlquery, ConnectionString);

           
            if (ddlBothDealerHHT1.SelectedValue.ToString() != "0")
            {
                lblDepositedLocation.Text = dt.Rows[0]["RTOLocationName"].ToString();
                hdnrtolocation.Value = dt.Rows[0]["RTOLocationID"].ToString();
                LblDealerId.Text = ddlBothDealerHHT1.SelectedValue.ToString();

            }
            else
            {
                lblDepositedLocation.Text = "";// dt.Rows[0]["RTOLocationName"].ToString();
                hdnrtolocation.Value = "";// dt.Rows[0]["RTOLocationID"].ToString();
                LblDealerId.Text = "";
            }

        }




    }
}