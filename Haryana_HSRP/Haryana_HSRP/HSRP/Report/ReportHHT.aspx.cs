using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Drawing;

namespace HSRP.Report
{
    public partial class ReportHHT : System.Web.UI.Page
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataAdapter da;
        String ConnectionString;
        DataTable dt;
       // StringBuilder sb;
        protected void Page_Load(object sender, EventArgs e)
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                if (!IsPostBack)
                {
                    Fill_TotalState_HHT();
                }
            }
        }

        private void Fill_TotalState_HHT()
        {
            using (con = new SqlConnection(ConnectionString))
            {
                using (cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[Report_HHT_TotalMachine]";
                    con.Open();
                    using (da = new SqlDataAdapter(cmd.CommandText, con))
                    {
                        dt = new DataTable();
                        da.Fill(dt);
                        con.Close();
                        Grid.DataSource = dt;
                        Grid.DataBind();
                    }
                }
            }
        }

        private void Fill_TotalState_HHT1(string from, string To, string State)
        {
            using (con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Report_HHTDateData]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@dateFrom", from));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@dateTo", To));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StateName", State));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                dt = new DataTable();
                Adap.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();

            }
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                //  Grid.SelectedRowStyle.BackColor = Color.Bisque;
                kk.Value = Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString();
                vehshow.Style.Add("display", "block");
                labelGridView1.Text = string.Empty;
                labelGridView2.Text = string.Empty;
                GridView1.DataSource = null;
                GridView1.DataSource = null;
                GridView1.DataBind();
                GridView2.DataBind();

            }
        }

        protected void linkButtonGenerateReport_Click(object sender, EventArgs e)
        {
            string from = PickerFrom.SelectedDate.ToShortDateString() + " 00:00:01";
            string To = PickerFrom.SelectedDate.ToShortDateString() + " 23:59:59";
            string state = kk.Value;
            labelGridView1.Text = "All Machines For State : " + kk.Value;
            Hiddenfrom.Value= from;
            HiddenTo.Value = To;
            veh.Style.Add("display", "block");
            Fill_TotalState_HHT1(from, To, state);
        }



        protected void Grid_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Fill_TotalState_HHT2(GridView1.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString(), Hiddenfrom.Value,HiddenTo.Value);
                Div1.Style.Add("display", "block");
                labelGridView2.Text = " Detailed Record for Machine : " + GridView1.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString() + " for Date : " + PickerFrom.SelectedDate.ToShortDateString();
            }
        }

        private void Fill_TotalState_HHT2(string MachineID, string from, string To)
        {
            using (con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Report_HHTRecordsDate]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MachineID", MachineID));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@dateFrom", from));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@dateTo", To));
                SqlDataAdapter Adap = new SqlDataAdapter(cmd);
                dt = new DataTable();
                Adap.Fill(dt);
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Session["MachineID"] = Grid.DataKeys[0].Value.ToString();

        }
    }
}