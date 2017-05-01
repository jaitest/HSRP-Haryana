using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataProvider;

namespace HSRP.Master
{
    public partial class AddEmpDesignation : System.Web.UI.Page
    {
        string Mode;
        int HSRP_StateID;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string UserID = string.Empty;
        int EditStateID;
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            HSRP_StateID = Convert.ToInt16(Session["UserHSRPStateID"]);
            UserID = Session["UID"].ToString();
            UserType = Session["UserType"].ToString();
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }

        protected void ButtonSave_Click1(object sender, EventArgs e)
        {
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            //string ActiveStatus;
            //if (checkBoxActiveStatus.Checked == true)
            //{
            //    ActiveStatus = "Y";
            //}
            //else
            //{
            //    ActiveStatus = "N";
            //}
            string ExpenseName = textboxBoxHSRPState.Text;
            int Count = Utils.getScalarCount("select COUNT(*) from EmpDesignation where DesignationName='" + ExpenseName + "' and IsDelete=1", ConnectionString);
            if (Count > 0)
            {
                lblErrMess.Text = "Record Already Exist!!";
                return;
            }
            else
            {
                //string InsertExpence = "INSERT INTO ExpenseMaster (ExpenseName,ActiveStatus) VALUES ('" + ExpenseName + "','" + ActiveStatus + "')";
                int i = Utils.ExecNonQuery("InsertUpdateSelectAndDeleteWithEmpDesignation 0,'" + ExpenseName + "'," + UserID + ",'INSERT'", ConnectionString);
                if (i > 0)
                {
                    lblSucMess.Text = "Record Save Successfully.";
                    BindGrid();
                    textboxBoxHSRPState.Text = "";
                }
                else
                {
                    lblErrMess.Text = "Record not saved";
                }
            }
        }

        public void BindGrid()
        {
            DataSet ds = Utils.getDataSet("InsertUpdateSelectAndDeleteWithEmpDesignation '0','','0','SELECT'", ConnectionString);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvEmpDesignation.DataSource = ds;
                gvEmpDesignation.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                gvEmpDesignation.DataSource = ds;
                gvEmpDesignation.DataBind();
                int columncount = gvEmpDesignation.Rows[0].Cells.Count;
                gvEmpDesignation.Rows[0].Cells.Clear();
                gvEmpDesignation.Rows[0].Cells.Add(new TableCell());
                gvEmpDesignation.Rows[0].Cells[0].ColumnSpan = columncount;
                gvEmpDesignation.Rows[0].Cells[0].Text = "No Records Found";
            }
        }

        protected void gvEmpDesignation_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvEmpDesignation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEmpDesignation.EditIndex = e.NewEditIndex;
            this.BindGrid();
        }

        protected void gvEmpDesignation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEmpDesignation.EditIndex = -1;
            this.BindGrid();
        }

        protected void gvEmpDesignation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvEmpDesignation.Rows[e.RowIndex];
            int Id = Convert.ToInt32(gvEmpDesignation.DataKeys[e.RowIndex].Values[0]);
            string ExpenseName = (row.Cells[2].Controls[0] as TextBox).Text;
            int i = Utils.ExecNonQuery("InsertUpdateSelectAndDeleteWithEmpDesignation " + Id + ",'" + ExpenseName + "'," + UserID + ",'UPDATE'", ConnectionString);
            if (i > 0)
            {
                lblSucMess.Text = "Record Update Successfully.";
                BindGrid();
                textboxBoxHSRPState.Text = "";
            }
            else
            {
                lblErrMess.Text = "Record not Update";
            }
            gvEmpDesignation.EditIndex = -1;
            this.BindGrid();
        }

        protected void gvEmpDesignation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int Id = Convert.ToInt32(gvEmpDesignation.DataKeys[e.RowIndex].Values[0]);
            int i = Utils.ExecNonQuery("InsertUpdateSelectAndDeleteWithEmpDesignation " + Id + ",''," + UserID + ",'DELETE'", ConnectionString);
            if (i > 0)
            {
                lblSucMess.Text = "Record Delete Successfully.";
                BindGrid();
                textboxBoxHSRPState.Text = "";
            }
            else
            {
                lblErrMess.Text = "Record not Delete";
            }
            this.BindGrid();
        }

    }
}