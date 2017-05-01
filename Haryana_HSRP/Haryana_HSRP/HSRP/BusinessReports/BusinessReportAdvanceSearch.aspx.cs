using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace HSRP.BusinessReports
{
    public partial class BusinessReportAdvanceSearch : System.Web.UI.Page
    {
        int UserType1;
        string CnnString1 = string.Empty;
        string HSRP_StateID1 = string.Empty;
        string RTOLocationID1 = string.Empty;
        string strUserID1 = string.Empty;
        string SQLString1 = string.Empty;

       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType1 = Convert.ToInt32(Session["UserType"]);
                CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID1 = Session["UID"].ToString();


                if (!IsPostBack)
                {
                    try
                    {
                        
                     FillDropDown();

                        Panel1.Visible = false;
                        grd.Visible = false;
                        grd.Visible = false;

                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        private void FillDropDown()
        {
            SqlConnection con = new SqlConnection(CnnString1);

            DataSet ds = new DataSet();

            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            //change sp Name
            cmd = new SqlCommand("business_report_advanceSearch", con);//Business_ReportTypewisesummary_report

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@reporttype", "db"));
            cmd.Parameters.Add(new SqlParameter("@rep_filter", "A"));


            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
         
            Ddl1.DataSource = ds.Tables[0];
            Ddl1.DataTextField = ds.Tables[0].Columns[0].ToString();
            Ddl1.DataValueField = ds.Tables[0].Columns[1].ToString();
            Ddl1.DataBind();
            Ddl1.Items.Insert(0, new ListItem("--Select--", "--Select--"));


        }
        DataTable ds = new DataTable();
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(CnnString1);

          

            SqlCommand cmd = new SqlCommand();
            string strParameter = string.Empty;
            //change sp Name
            cmd = new SqlCommand("business_report_advanceSearch", con);//Business_ReportTypewisesummary_report

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@reporttype", Convert.ToString(Ddl1.SelectedValue)));
            cmd.Parameters.Add(new SqlParameter("@rep_filter", Convert.ToString(TxtSearchtype.Text.Trim())));
            con.Open();
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            ds.Load(dr);
            con.Close();

            //cmd.CommandTimeout = 0;
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);
            if (ds.Rows.Count> 0)
            {
                grd.DataSource = ds;
                grd.DataBind();

                Panel1.Visible = true;
                grd.Visible = true;
                grd.Visible = true;
            }
            else
            {
                Lblerror.Visible = true;
                Lblerror.Text = "Records Not Available .";
                Panel1.Visible = false;
                grd.Visible = false;
                grd.Visible = false;

            }
            
        }

        protected void Ddl1_SelectedIndexChanged(object sender, EventArgs e)
        {
              if (Ddl1.Items.ToString() == "--Select--")                {

                    lblsearch.Visible = false;
                    lblsearch.Visible = false;
                    TxtSearchtype.Visible = false;
                    BtnSearch.Visible = false;

                }

                else
                {

                    lblsearch.Text = "Enter "+Ddl1.SelectedItem.ToString();
                    TxtSearchtype.Text = "";
                    lblsearch.Visible = true;
                    TxtSearchtype.Visible = true;
                    BtnSearch.Visible = true;

                }      
            
           
            


        }
    }
}