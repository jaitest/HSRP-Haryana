using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

namespace HSRP.Transaction
{
    public partial class ViewLocationResources : System.Web.UI.Page
    {
        int HSRPStateID;
        int RTOLocationID;
        int UserID;
        string CnnString = string.Empty;
        string UserName = string.Empty;
        //string SQLString = string.Empty;
        int UserType;
       // StringBuilder sbQuery = new StringBuilder();
        string SQLString = string.Empty;
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        DataProvider.BAL BL = new DataProvider.BAL();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                //lblErrMess.Text = String.Empty;
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                UserName = Session["UserName"].ToString();
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UserID);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }

            if (!Page.IsPostBack)
            {
                
                if (string.IsNullOrEmpty(UserType.ToString()))
                {
                    Response.Write("<script language='javascript'> {window.close();} </script>");
                }
                else
                {

                    
                    if (UserType.Equals(0))
                    {
                        buildGrid();
                    }
                    else if (UserType.Equals(1))
                    {
                        buildGrid();
                    }
                    else if (UserType.Equals(2))
                    {
                        buildGrid();
                    }
                }
            }


            
        }


        public void buildGrid()
        {
            try
            {
                string SQLString = "select LocationResources.*,Users.UserID,Users.UserFirstName from LocationResources inner join Users on LocationResources.CreatedBy=Users.UserID";


                DataTable dt = Utils.GetDataTable(SQLString.ToString(), ConnectionString.ToString());
                GridView1.DataSource = dt;
                //GridView1.RunningMode = (ComponentArt.Web.UI.GridRunningMode)Enum.Parse(typeof(ComponentArt.Web.UI.GridRunningMode), "Client");
                //GridView1.SearchOnKeyPress = true;
                GridView1.DataBind();
                // GridView1.RecordCount.ToString();
            }
            catch (Exception ex)
            {
                // lblErrMsg.Text = "Error in Populating Grid :" + ex.Message.ToString();
            }
        }


        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            buildGrid();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }




        protected void EditResources(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            buildGrid();
        }
        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            buildGrid();
        }

        protected void UpdateResources(object sender, GridViewUpdateEventArgs e)
        {
            string ResID = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblResID")).Text;
            string ResourcesType = ((DropDownList)GridView1.Rows[e.RowIndex].FindControl("DropDownList1")).SelectedItem.Text;
            string quantity = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtquantity")).Text;
            string ResourcesDetail = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtResourcesDetail")).Text;
            List<string> lst = new List<string>();

            lst.Add(ResourcesType);
            lst.Add(quantity);
            lst.Add(ResourcesDetail);
            lst.Add(UserID.ToString());
            lst.Add(DateTime.Now.ToString());
            lst.Add(DateTime.Now.ToString());
            lst.Add(ResID);
            int i = BL.UpdateResources(lst);
            if (1 > 0)
            {
                buildGrid();
                string script = "<script type=\"text/javascript\">  alert('Update Successfully');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

            }
            else
            {
                string script = "<script type=\"text/javascript\">  alert('Update not Successfully');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
            }
            // buildGrid();
            Response.Redirect("../Transaction/ViewLocationResources.aspx");
        }


        protected void DeleteResources(object sender, EventArgs e)
        {
            LinkButton lnkRemove = (LinkButton)sender;
            //SqlConnection con = new SqlConnection(strConnString);
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "delete from  customers where " +
            //"CustomerID=@CustomerID;" +
            // "select CustomerID,ContactName,CompanyName from customers";
            //cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar).Value = lnkRemove.CommandArgument;
            //GridView1.DataSource = GetData(cmd);
            //GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //string ResID = ((TextBox)GridView1.FooterRow.FindControl("txtResID")).Text;
            string ResourcesType = ((DropDownList)GridView1.FooterRow.FindControl("DropDownList1")).SelectedItem.Text;
            string quantity = ((TextBox)GridView1.FooterRow.FindControl("txtquantity")).Text;
            string ResourcesDetail = ((TextBox)GridView1.FooterRow.FindControl("txtResourcesDetail")).Text;

            List<string> lst = new List<string>();
            //  lst.Add(ResID);
            lst.Add(ResourcesType);
            lst.Add(quantity);
            lst.Add(ResourcesDetail);
            lst.Add(UserID.ToString());
            lst.Add(DateTime.Now.ToString());
            lst.Add(DateTime.Now.ToString());
            int i = BL.InsertResources(lst);
            if (1 > 0)
            {
                string script = "<script type=\"text/javascript\">  alert('Save Successfully');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                buildGrid();
            }
            else
            {
                string script = "<script type=\"text/javascript\">  alert('Save not Successfully');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
            }
        }
    }
}