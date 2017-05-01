using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;

namespace HSRP.Transaction
{
    public partial class PendingAssignment : System.Web.UI.Page
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

            //if (string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()))
            //{
            //    Response.Write("<script language='javascript'> {window.close();} </script>");
            //}
            //else
            //{
            //    StringMode = Request.QueryString["Mode"].ToString();
            //}

            //if (StringMode.Equals("Edit"))
            //{
            //    ProductivityID = Request.QueryString["TransactionID"].ToString();
               
              buttonSave.Visible =true;

                
            //}
            //else
            //{
            //    buttonSave.Visible = true;
            //    //buttonUpdate.Visible = false;
            //}
            if (!Page.IsPostBack)
            {
                FillddlLocation();
                FilldropDownListClient();
             

                InitialSetting();
            }

        }


        public void FillddlLocation()
        {
           //// query1 = "select RTOLocationName,RTOLocationID from RTOLocation where hsrp_stateid='" + HSRPStateID + "' and RTOLocationID='" + RTOLocationID + "' and ActiveStatus='Y' order by RTOLocationName ";
           // query1 = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, (a.RTOLocationCode+', ') as RTOCode, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where UserRTOLocationMapping.UserID='" + UserName + "' and ActiveStatus='Y' order by RTOLocationName ";
           // DataTable dt = Utils.GetDataTable(query1, ConnectionString);
           // ddlLocation.DataSource = dt;
           // ddlLocation.DataTextField = "RTOLocationName";
           // ddlLocation.DataValueField = "RTOLocationID";
           // ddlLocation.DataBind();
           // ddlLocation.Items.Insert(0, "--Select Location Name--");
           // ddlLocation.Items[0].Value = "0";
            
        }

        //public void BankName()
        //{
        //  if (UserType == "0")
        //    {


        //        SQLString = "select * from dbo.BankMaster where hsrp_stateid='" + HSRPStateID + "'";
        //        DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
        //        DropDownListBankName.DataSource = dt;
        //        DropDownListBankName.DataTextField = "Bankname";
        //        DropDownListBankName.DataValueField = "id";
        //        DropDownListBankName.DataBind();
        //       // DropDownListStateName.SelectedIndex = DropDownListStateName.Items.Count - 1;

        //        DropDownListBankName.Items.Insert(0, "--Select Bank Name--");
        //        DropDownListBankName.Items[0].Value = "0";
                 
        //    }
        //    else
        //    {
               
        //        SQLString = "select * from dbo.BankMaster where hsrp_stateid='" + HSRPStateID + "'";
        //        DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
        //        DropDownListBankName.DataSource = dt;
        //        DropDownListBankName.DataTextField = "Bankname";
        //        DropDownListBankName.DataValueField = "id";
        //        DropDownListBankName.DataBind();

        //        DropDownListBankName.Items.Insert(0, "--Select Bank Name--");
        //        DropDownListBankName.Items[0].Value = "0";
        //    }
               
              
           

        //}


        private void FilldropDownListClient()
        {

            SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and navembid is not null  Order by EmbCenterName";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), ConnectionString, "--Select Embossing Center--");

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
           
            if (dropDownListClient.SelectedItem.Text == "--Select Embossing Center--")
            {
                lblErrMess.Text = "Please Select Embossing Center";
                return;
            }

            try
            {

                SQLString = "insert into pendingassignmentreason(navembid,reasondate,reason) values('" + dropDownListClient.SelectedValue.ToString() + "','" + OrderDate1 + "', '"+TextBoxRemarks.Text + "')";
                int count = Utils.ExecNonQuery(SQLString, ConnectionString);
                if (count > 0)
                {
                    lblSucMess.Text = "Records has been Saved Successfully";
                }
                else
                {
                    lblErrMess.Text = "Record Not Saved";
                }           
              
            }

            catch
            {

            }
           
        }

        public void referess()
        {
            TextBoxRemarks.Text = "";
            //BankName();
            //lblAccountNo.Text = "";
         
            //txtBranchName.Text = "";
            //TextBoxDepositAmount.Text = "";
            //TextBoxDepositby.Text = "";
            //TextBoxBankSlipNo.Text = "";
            //TextBoxRemarks.Text = "";
            //FillddlLocation();
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


        

        protected void btnReset_Click(object sender, EventArgs e)
        {
            referess();
        }

       
       
    }
}