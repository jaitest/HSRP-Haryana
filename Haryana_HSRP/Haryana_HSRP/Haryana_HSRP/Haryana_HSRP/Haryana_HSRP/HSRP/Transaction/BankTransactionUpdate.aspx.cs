using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;
using System.Globalization;

namespace HSRP.Master
{
    public partial class BankTransactionUpdate : System.Web.UI.Page
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
            try
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
                InitialSetting();
                FillddlLocation();
                FillDropDownApprovedby();
                BankName();
                if (StringMode.Equals("Edit"))
                {
                    EditBankTransaction(ProductivityID);
                  
                    
                }

               
            }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        
        public void FillddlLocation()
        {
           // query1 = "select RTOLocationName,RTOLocationID from RTOLocation where hsrp_stateid='" + HSRPStateID + "' and RTOLocationID='" + RTOLocationID + "' and ActiveStatus='Y' order by RTOLocationName ";
            query1 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserName + "' and ActiveStatus='Y' order by RTOLocationName ";
           // query1 = "SELECT rtolocationid, rtolocationname  FROM  RTOLocation a where  RTOLocationID  = '"+RTOLocationID+"' and hsrp_stateid ='"+HSRPStateID+"'";
            DataTable dt = Utils.GetDataTable(query1, ConnectionString);
            ddlLocation.DataSource = dt;
            ddlLocation.DataTextField = "rtolocationname";
            ddlLocation.DataValueField = "rtolocationid";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, "--Select Location Name--");
            ddlLocation.Items[0].Value = "0";


            ddldisipositeLocation.DataSource = dt;
            ddldisipositeLocation.DataTextField = "rtolocationname";
            ddldisipositeLocation.DataValueField = "rtolocationid";
            ddldisipositeLocation.DataBind();
            ddldisipositeLocation.Items.Insert(0, "--Select Location Name--");
            ddldisipositeLocation.Items[0].Value = "0";

            
          
        }

        public void FillDropDownApprovedby()
        {
           
            query1 = "SELECT  distinct  Emp_Id ,Emp_Name  from  employeemaster  where activestatus ='Y'";
            DataTable dt = Utils.GetDataTable(query1, ConnectionString);
            DropDownApprovedby.DataSource = dt;
            DropDownApprovedby.DataTextField = "Emp_Name";
            DropDownApprovedby.DataValueField = "Emp_Id";
            DropDownApprovedby.DataBind();
            DropDownApprovedby.Items.Insert(0, "--Select Approved By--");
            DropDownApprovedby.Items[0].Value = "0";
            
        }


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
            try
            {
                            
                    SQLString = "SELECT [TransactionID],[DepositDate],[BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],[UserID],[CurrentDate],[BankSlipNo],[Remarks],[AccountNo],[ETAProcess],[depositelocationid],[EntryType],[oldTransactionID],[EntryDate],[DealerID],[chq_no],[chq_date],[ApprovedStatus],[VoidStatus] ,[ApprovedBy] FROM [BankTransaction] where TransactionID="+TransactionID;
                    DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
                                                            
                    // string s =  ds.Rows[0]["DepositDate"].ToString(); 
                    DepositDate.SelectedDate = (DateTime.Parse(ds.Rows[0]["DepositDate"].ToString()));                     
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
                    ddldisipositeLocation.SelectedValue = ds.Rows[0]["depositelocationid"].ToString(); 
                    ddlLocation.SelectedValue = ds.Rows[0]["rtolocation"].ToString();                   
                    txtetaprocess.Text = ds.Rows[0]["ETAProcess"].ToString();
                   // string ss1 = ds.Rows[0]["EntryDate"].ToString();
                    DepositDate1.SelectedDate = DateTime.Parse(ds.Rows[0]["EntryDate"].ToString());                   
                    txtoldtrnid.Text = ds.Rows[0]["oldTransactionID"].ToString();
                    txtdealer.Text = ds.Rows[0]["DealerID"].ToString();
                    txtchqno.Text = ds.Rows[0]["chq_no"].ToString();

                    if (string.IsNullOrEmpty(ds.Rows[0]["ApprovedBy"].ToString()))
                    {
                    }
                    else
                    {
                      DropDownApprovedby.SelectedValue = ds.Rows[0]["ApprovedBy"].ToString();
                    }

            }
            catch (Exception ex )
            {

                throw ex ;
            }                   
       

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

            try
            {
                if (int.Parse(TextBoxDepositAmount.Text) >= 1)
                {
                    List<string> lst = new List<string>();
                    lst.Add(OrderDate1.ToString());
                    lst.Add(DropDownListBankName.SelectedValue.ToString());
                    ////ddlLocation.SelectedValue = ds.Rows[0]["depositelocationid"].ToString(); 
                    lst.Add(ddldisipositeLocation.SelectedValue);
                    lst.Add(txtBranchName.Text.ToString().ToUpper());
                    lst.Add(TextBoxDepositAmount.Text.ToString());
                    lst.Add(TextBoxDepositby.Text);
                    lst.Add(HSRPStateID);
                    lst.Add(RTOLocationID);
                    lst.Add(UserName);
                   
                    lst.Add(TextBoxBankSlipNo.Text);
                    lst.Add(TextBoxRemarks.Text);
                    lst.Add(lblAccountNo.Text);

                    string strResult = string.Empty;
                    string TId = string.Empty;
                    bl.SaveBankTranction(lst,out strResult);

                 

                    string script = "<script type=\"text/javascript\">  alert('"+strResult+"');</script>";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

                    if (strResult == "Please Check Duplicate Record")
                    {
 
                    }
                    else
                    {
                        referess();
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
           
            txtchqno.Text = "";
            txtdealer.Text = "";
            txtentrytype.Text = "";
            txtetaprocess.Text = "";
            txtoldtrnid.Text = "";
          //  txtvoid.Text = "";
           
            FillddlLocation();
            FillDropDownApprovedby();
        }

        private void InitialSetting()
        {

            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

            CalendarDepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDepositDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);          
            DepositDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            DepositDate.MaxDate = DateTime.Parse(MaxDate);

            CalendarDepositDate1.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarDepositDate1.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            DepositDate1.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            DepositDate1.MaxDate = DateTime.Parse(MaxDate);

           
        }
             


        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string TransactionID1= Request.QueryString["TransactionID"].ToString();             
              

                string OrderDate1 = DepositDate.SelectedDate.ToString();
                string OrderDate2 = DepositDate1.SelectedDate.ToString();

                if (Convert.ToDateTime(OrderDate1)<= Convert.ToDateTime(OrderDate2))
                {


                    if (int.Parse(TextBoxDepositAmount.Text) >= 1)
                    {
                        List<string> lst = new List<string>();

                        lst.Add(OrderDate1.ToString());
                        lst.Add(OrderDate2.ToString());
                        lst.Add(DropDownListBankName.SelectedValue.ToString());
                        lst.Add(txtBranchName.Text.ToString().ToUpper());
                        lst.Add(TextBoxDepositAmount.Text.ToString());
                        lst.Add(TextBoxDepositby.Text);
                        //lst.Add(HSRPStateID);
                        //lst.Add(RTOLocationID);
                        //lst.Add(UserName);
                        lst.Add(TextBoxBankSlipNo.Text);
                        lst.Add(TextBoxRemarks.Text);
                        lst.Add(lblAccountNo.Text);
                        lst.Add(TransactionID1);
                        lst.Add(ddldisipositeLocation.SelectedValue);
                        lst.Add(txtoldtrnid.Text.ToString());
                        lst.Add(DropDownApprovedby.SelectedValue.ToString());
                        lst.Add(ddlLocation.SelectedValue.ToString());



                        SQLString = "SELECT  [Depositdate],[Entrydate], [BankName],[BranchName],[DepositAmount],[DepositBy],[StateID],[RTOLocation],[UserID],[BankSlipNo],[AccountNo],[depositelocationid],[ApprovedBy]FROM [BankTransaction] where TransactionID=" + TransactionID1;
                        DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
                        if (ds.Rows.Count > 0)
                        {
                            List<string> list = new List<string>();
	
                           list.Add(TransactionID1);                             
                            
                           list.Add(ds.Rows[0]["BankName"].ToString());
                           list.Add(DropDownListBankName.SelectedValue.ToString());
                           list.Add(ds.Rows[0]["BranchName"].ToString());
                           list.Add(txtBranchName.Text.ToString().ToUpper());
                           list.Add(ds.Rows[0]["DepositAmount"].ToString());
                           list.Add(TextBoxDepositAmount.Text.ToString());
                           list.Add(ds.Rows[0]["DepositBy"].ToString());
                           list.Add(TextBoxDepositby.Text);
                           list.Add(ds.Rows[0]["StateID"].ToString());
                           list.Add(ds.Rows[0]["RTOLocation"].ToString());
                           list.Add(UserName);
                           list.Add(ds.Rows[0]["BankSlipNo"].ToString());
                           list.Add(TextBoxBankSlipNo.Text);
                           list.Add(ds.Rows[0]["AccountNo"].ToString());
                           list.Add(lblAccountNo.Text);
                           list.Add(ds.Rows[0]["depositelocationid"].ToString());
                           list.Add(ddldisipositeLocation.SelectedValue.ToString());
                           list.Add(ds.Rows[0]["ApprovedBy"].ToString()); 
                           list.Add(DropDownApprovedby.SelectedValue.ToString());
                           list.Add(ds.Rows[0]["Depositdate"].ToString());
                           list.Add(OrderDate1.ToString().ToString());
                           list.Add(ds.Rows[0]["Entrydate"].ToString());
                           list.Add(OrderDate2.ToString().ToString());
                           list.Add(ddlLocation.SelectedValue.ToString());

 
                           int j = bl.BankTransactionlogcrete(list);
                     
                            
                        }

                      

                      

                        int i = bl.UpdateBankTranctionDetails(lst);
                        if (i >= 1)
                        {
                           
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

                else
                {
                    string script = "<script type=\"text/javascript\">  alert('Deposite Date should Be Greater then Or Equal Entry Date');</script>";
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
            referess();
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

       
       
    }
}