using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace HSRP.Master
{
    public partial class AddEmployee : System.Web.UI.Page
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
            }
            if (!Page.IsPostBack)
            {
                FillUsername();
                FillHeads();
                FillRoles();
            }
        }
        

        //private void InitialSetting()
        //{

        //    string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
        //     string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();

        //    OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        //    OrderDate.MaxDate = DateTime.Parse(MaxDate);          

        //    //OrderDate.MinDate = (DateTime.Parse(TodayDate)).AddDays(-2.00);

        //}


        public void FillUsername()
        {
            SQLString = "Select UserID,UserFirstName+' '+UserLastName as username from [Users] where hsrp_stateid='" + HSRPStateID + "' and isnull(dealerid,'')='' and activestatus='Y' order by userfirstname";
            DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
            ddlUserAccount.DataSource = dt;
            ddlUserAccount.DataTextField = "username";
            ddlUserAccount.DataValueField = "UserID";
            ddlUserAccount.DataBind();
            ddlUserAccount.Items.Insert(0, new ListItem("--Select username--"));
        }

        public void FillHeads()
        {
            SQLString = "Select UserID,UserFirstName+' '+UserLastName as username from [Users] where hsrp_stateid='" + HSRPStateID + "' and isnull(dealerid,'')='' and activestatus='Y' order by userfirstname";
            DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
            ddlhead.DataSource = dt;
            ddlhead.DataTextField = "username";
            ddlhead.DataValueField = "UserID";
            ddlhead.DataBind();
            ddlhead.Items.Insert(0, new ListItem("--Select State Head--"));
        }
        public void FillRoles()
        {
            DataSet ds = Utils.getDataSet("InsertUpdateSelectAndDeleteWithEmpDesignation '0','','0','SELECT'", ConnectionString);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlResponsibility.DataSource = ds.Tables[0];
                ddlResponsibility.DataTextField = "DesignationName";
                ddlResponsibility.DataValueField = "ID";
                ddlResponsibility.DataBind();
                ddlResponsibility.Items.Insert(0, new ListItem("--Select Role--"));
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlUserAccount.SelectedItem.ToString() == "--Select username--")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Select Bank Name";
                    return;

                }
                if (txtempname.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Employee Name";
                    return;

                }
                if (txtEmail.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Email-ID";
                    return;

                }
                if (txtmobileno.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Mobile No";
                    return;

                }
                if (txtempcode.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Employee Code";
                    return;

                }
                if (txtDesignation.Text.ToString() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Please Enter Designation";
                    return;
                }
                string sqlstring = "Select * from employeemaster where UserId='" + ddlUserAccount.SelectedValue.ToString() + "' and activestatus='Y'";
                DataTable dt = Utils.GetDataTable(sqlstring, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "";
                    lblErrMess.Text = "Duplicate Entry!.";
                    return;
                }
                sqlstring = "Insert into employeemaster(Emp_Name,Email,MobileNo,Emp_id,Designation,EntryDate,ActiveStatus,UserId,ApprovalHead,[Role]) values('" + txtempname.Text.ToString() + "','" + txtEmail.Text.ToString() + "','" + txtmobileno.Text.ToString() + "','" + txtempcode.Text.ToString() + "','" + txtDesignation.Text.ToString() + "',getdate(),'Y','" + ddlUserAccount.SelectedValue.ToString() + "','" + ddlhead.SelectedValue + "','" + ddlResponsibility.SelectedValue.ToString() + "')";
                int i = Utils.ExecNonQuery(sqlstring.ToString(), ConnectionString);
                if (i > 0)
                {
                    lblSucMess.Text = "Record inserted successfully";
                    clear();
                }
                else
                {
                    lblSucMess.Text = "Record not insert";
                }
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
            }
        }
        public void clear()
        {
            ddlUserAccount.ClearSelection();
            txtempname.Text = "";
            txtmobileno.Text = "";
            txtempcode.Text = "";
            txtEmail.Text = "";
            txtDesignation.Text = "";
            ddlhead.ClearSelection();
            ddlResponsibility.ClearSelection();
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            clear();
        }

    }
}