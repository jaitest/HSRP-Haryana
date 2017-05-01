using System;
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
    public partial class BankTransactionVoid : System.Web.UI.Page
    {
        
        

        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
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
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
                //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }

            if (string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()))
            {
                Response.Write("<script language='javascript'> {window.close();} </script>");
            }
            else
            {
                StringMode = Request.QueryString["Mode"].ToString();
            }

            if (StringMode.Equals("Voids"))
            {
                ProductivityID = Request.QueryString["TransactionID"].ToString();
                buttonUpdate.Visible = true;
                //btnReset.Visible = true;
              //  buttonSave.Visible = false;

                
            }
            else
            {
               // buttonSave.Visible = true;
                //buttonUpdate.Visible = false;
            }
            if (!Page.IsPostBack)
            {
               // FillddlLocation();
              //  BankName();
                if (StringMode.Equals("Voids"))
                {
                  //  EditBankTransaction(ProductivityID);
                  
                    
                }

               // InitialSetting();
            }

        }


       
         
        private void EditBankTransaction(string TransactionID)
        {
            //SQLString = "SELECT [TransactionID],[DepositDate],[BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],[UserID],[CurrentDate],[BankSlipNo],[Remarks],[AccountNo],[depositelocationid] FROM [BankTransaction] where TransactionID=" + TransactionID;
            //        DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            //        DepositDate.SelectedDate = DateTime.Parse(ds.Rows[0]["DepositDate"].ToString());
            //       // txtBankName.Text = ds.Rows[0]["BankName"].ToString();
            //        DropDownListBankName.SelectedValue = ds.Rows[0]["BankName"].ToString();
            //        txtBranchName.Text = ds.Rows[0]["BranchName"].ToString();
            //        TextBoxDepositAmount.Text = ds.Rows[0]["DepositAmount"].ToString();
            //        TextBoxDepositby.Text = ds.Rows[0]["DepositBy"].ToString();
            //        HSRPStateID = ds.Rows[0]["StateID"].ToString();
            //        RTOLocationID = ds.Rows[0]["RTOLocation"].ToString();
            //        UserName = ds.Rows[0]["UserID"].ToString();
            //        CurrentDate = ds.Rows[0]["CurrentDate"].ToString();
            //        TextBoxBankSlipNo.Text = ds.Rows[0]["BankSlipNo"].ToString();
            //        TextBoxRemarks.Text = ds.Rows[0]["Remarks"].ToString();
            //        lblAccountNo.Text = ds.Rows[0]["AccountNo"].ToString();
            //        ddlLocation.SelectedValue = ds.Rows[0]["depositelocationid"].ToString();
                   // TransactionID1 = ds.Rows[0]["TransactionID"].ToString(); 
             
        }

     

       

        //private void InitialSetting()
        //{

        //    string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //    string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

        //    CalendarDepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    CalendarDepositDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);

        //    DepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    DepositDate.MaxDate = DateTime.Parse(MaxDate);
        //}


        //private void UpdateVoid(string TransactionID)
        //{
        //    SQLString = "Update BankTransaction set=voidstatus='Void' WHERE TransactionID='"+TransactionID+"'";

        //    int i = Utils.ExecNonQuery(SQLString, ConnectionString); //GetDataTable(SQLString, ConnectionString);          


        //}


        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string TransactionID= Request.QueryString["TransactionID"].ToString();

                SQLString = "Update BankTransaction set voidstatus='Void' WHERE TransactionID='" + TransactionID + "'";

                int i = Utils.ExecNonQuery(SQLString, ConnectionString);               
                    if (i >= 1)
                    {
                       
                       // lblSucMess.Text = "Updated Successfully";
                        string script = "<script type=\"text/javascript\">  alert('Updated Successfully');</script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                        //referess();
                    }
                    else
                    {
                       // lblErrMess.Text = "Updated not Successfully";
                        string script = "<script type=\"text/javascript\">  alert('Updated not Successfully');</script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                       // referess();
                    }
               // }
                //else
                //{
                //    string script = "<script type=\"text/javascript\">  alert('Please Provide Some Amount');</script>";
                //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                //   // referess();
                //    TextBoxDepositAmount.Focus();
                //}

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
          
        }

        //protected void DropDownListBankName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DropDownListBankName.SelectedValue == "0")
        //    {
        //        lblAccountNo.Text = "";
        //    }
        //    else
        //    {
        //        SQLString = "select * from dbo.BankMaster where id='" + DropDownListBankName.SelectedValue.ToString() + "'";
        //        DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
        //        lblAccountNo.Text = dt.Rows[0]["accountno"].ToString();
        //    }
            
        //}

        //protected void btnApproval_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string TransactionID1 = Request.QueryString["TransactionID"].ToString();
        //        SQLString = "SELECT [TransactionID],[DepositDate],[BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],[UserID],[CurrentDate],[BankSlipNo],[Remarks],[AccountNo],[depositelocationid] FROM [BankTransaction] where TransactionID=" + TransactionID1;
        //        DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
        //        if (ds.Rows.Count > 0)
        //        {
        //            string sqlquery1 = "update banktransaction set ApprovedStatus='Y' where TransactionID='" + TransactionID1 + "'";
        //            int i = Utils.ExecNonQuery(sqlquery1, ConnectionString);
        //            if (i > 0)
        //            {
        //                lblSucMess.Text = "Record Approved Successfully...";
        //            }
        //            else
        //            {
        //                lblSucMess.Text = "Record not Approved..";
        //            }

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        lblErrMess.Text = ex.ToString();
        //    }
        //}

       
    }
}