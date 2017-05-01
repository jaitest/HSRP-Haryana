using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class Packaging : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductID = string.Empty, UserType = string.Empty, UserName = string.Empty;
        string SQLString = string.Empty;
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();

        DataProvider.BAL bl = new DataProvider.BAL();




        protected void Page_Load(object sender, EventArgs e)
        {

           
                message.InnerText = "";
                lblid.Text = "";
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
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
                }

                StringMode = Request.QueryString["Mode"].ToString();

                if (StringMode.Equals("Edit"))
                {
                    ProductID = Request.QueryString["PackagingID"].ToString();
                    buttonUpdate.Visible = true;
                    buttonSave.Visible = false;


                }
                else
                {
                    buttonSave.Visible = true;
                    buttonUpdate.Visible = false;
                }
                if (!Page.IsPostBack)
                {
                    prefixbind();
                    productbind();
                    InitialSetting();
                    if (StringMode.Equals("Edit"))
                    {
                        EditPackaging(ProductID);


                    }


                }
               

        }

        private void prefixbind()
        {

            string SQLString = "select Prefixid,prefix from dbo.PrefixLaserNo where hsrp_StateID='" + HSRPStateID + "'";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), ConnectionString.ToString());
            ddlPrefixName.DataSource = dt;
            ddlPrefixName.DataTextField = "prefix";
            ddlPrefixName.DataValueField = "Prefixid";
            ddlPrefixName.DataBind();
            ddlPrefixName.Items.Insert(0, "--Select Prefix Name--");
            ddlPrefixName.Items[0].Value = "0";
        }

        private void productbind()
        {
            string SQLString = "select productid,productcode from dbo.Product where Hsrp_stateid='"+HSRPStateID+"' order by productcode";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), ConnectionString.ToString());
            ddlProductName.DataSource = dt;
            ddlProductName.DataTextField = "productcode";
            ddlProductName.DataValueField = "productid";
            ddlProductName.DataBind();
            ddlProductName.Items.Insert(0, "--Select Product Name--");
            ddlProductName.Items[0].Value = "0";
        }

        private void EditPackaging(string ProductID)
        {
            string SQLString = "select Packaging.*,Product.productcode from Packaging inner join Product on Product.productid=Packaging.productid where Packaging.Packagingid='" + ProductID + "' ";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), ConnectionString.ToString());

            InitialSetting();
         
            ddlProductName.SelectedValue = dt.Rows[0]["productid"].ToString();
            if (dt.Rows[0]["Lasered"].ToString() == "Y")
            {
                rdoLaseredYes.Checked = true;
            }
            else
            {
                rdoLaseredNo.Checked = true;
            }
            txtLaserCodeFrom.Text = dt.Rows[0]["LaseredCodeForm"].ToString();
            txtLaserCodeTo.Text = dt.Rows[0]["LaseredCodeTo"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
            HiddenPackagingID.Value = dt.Rows[0]["PackagingID"].ToString();
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
        string rdbchecked = "N";
        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            if (rdoLaseredYes.Checked == true)
            {
                rdbchecked = "Y";
            }
            else
            {
                rdbchecked = "N";
            }
            string SQLString = "insert into Packaging(Plantid,DateofPackaging,Productid,Lasered,LaseredCodeForm,LaseredCodeTo,Remarks,GenratedBy,prifix) values('1'," + DepositDate.SelectedDate.ToString("dd/MM/yyyy") + ",'" + ddlProductName.SelectedValue.ToString() + "','" + rdbchecked + "','" + txtLaserCodeFrom.Text + "','" + txtLaserCodeTo.Text + "','" + txtRemarks.Text + "','" + Session["UID"].ToString() + "','"+ddlPrefixName.SelectedValue.ToString()+"')";
            
            int i=Utils.ExecNonQuery(SQLString.ToString(), ConnectionString.ToString());
            if (i > 0)
            {
                string SQLString1 = "select max(PackagingID) as id from Packaging";
                DataTable dt = Utils.GetDataTable(SQLString1.ToString(), ConnectionString.ToString());
                lblid.Text = dt.Rows[0]["id"].ToString();
                message.InnerText = "Your laser Code Package ID is " ;
                lblSucMess.Text = "Saved Successfully";
                makelasercode();
            }
            else
            {
                lblErrMess.Text = "error message";
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            InitialSetting();
            //productbind();
            rdoLaseredYes.Checked = false;
            rdoLaseredNo.Checked = false;
            txtLaserCodeFrom.Text = "";
            txtLaserCodeTo.Text = "";
            txtRemarks.Text = "";
            makelasercode();
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (rdoLaseredYes.Checked == true)
            {
                rdbchecked = "Y";
            }
            else
            {
                rdbchecked = "N";
            }
            string SQLString = "update Packaging set Plantid='1',DateofPackaging='" + DepositDate.SelectedDate + "',Productid='" + ddlProductName.SelectedValue.ToString() + "',Lasered='" + rdbchecked + "',LaseredCodeForm='" + txtLaserCodeFrom.Text + "',LaseredCodeTo='" + txtLaserCodeTo.Text + "',Remarks='" + txtRemarks.Text + "',GenratedBy='" + Session["UID"].ToString() + "' where PackagingID='" + HiddenPackagingID.Value + "'";

            int i = Utils.ExecNonQuery(SQLString.ToString(), ConnectionString.ToString());
            if (i > 0)
            {
                lblSucMess.Text = "Update Successfully";
            }
            else
            {
                lblErrMess.Text = "error message";
            }
        }

        //protected void rdoLaseredYes_CheckedChanged(object sender, EventArgs e)
        //{
        //    txtLaserCodeFrom.Enabled = false;
        //    txtLaserCodeTo.Enabled = false;
        //}

        //protected void rdoLaseredNo_CheckedChanged(object sender, EventArgs e)
        //{
        //    txtLaserCodeFrom.Enabled = false;
        //    txtLaserCodeTo.Enabled = false;
        //}

        protected void ddlPrefixName_SelectedIndexChanged(object sender, EventArgs e)
        {
            makelasercode();
        }

        public void makelasercode()
        {
            string SQLString = "SELECT top 1  packagingid,  convert(int,(SUBSTRING(LaseredCodeto, 5, 50)))+1  as fromlast, convert(int,(SUBSTRING(LaseredCodeto, 5, 50)))+50  as tolast   from dbo.Packaging where productid='" + ddlProductName.SelectedValue.ToString() + "' and  prifix= '" + ddlPrefixName.SelectedValue.ToString() + "' order by packagingid desc";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), ConnectionString.ToString());
            if ((dt.Rows.Count > 0) && (ddlPrefixName.SelectedValue!="0"))
            {
                txtLaserCodeFrom.Text = ddlPrefixName.SelectedItem.ToString() + int.Parse(dt.Rows[0]["fromlast"].ToString());
                txtLaserCodeTo.Text = ddlPrefixName.SelectedItem.ToString() + int.Parse(dt.Rows[0]["tolast"].ToString());
                txtLaserCodeFrom.Enabled = false;
            }
            else
            {
                txtLaserCodeFrom.Text = "";
                txtLaserCodeTo.Text = "";
                txtLaserCodeFrom.Enabled = true;
            }
        }

        protected void ddlProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            makelasercode();
        }

        protected void txtLaserCodeFrom_TextChanged(object sender, EventArgs e)
        {
            txtLaserCodeTo.Text = (ddlPrefixName.SelectedItem.ToString() + (int.Parse(txtLaserCodeFrom.Text) + 50).ToString()).ToString();
            txtLaserCodeFrom.Text = ddlPrefixName.SelectedItem.ToString() + txtLaserCodeFrom.Text;
        }

        

    }
}