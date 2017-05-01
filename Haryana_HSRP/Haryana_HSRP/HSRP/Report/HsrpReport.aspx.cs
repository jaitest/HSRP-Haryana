using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;


namespace HSRP.Report
{
    public partial class HsrpReport : System.Web.UI.Page
    {
        int UserType;
        string CnnString = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string strUserID = string.Empty;
       // int intHSRPStateID;
       // int intRTOLocationID;
        string SQLString = string.Empty;
        //DateTime OrderDate1;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();

                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                if (!IsPostBack)
                {
                   
                    try
                    {
                        //Bindata();
                        if (UserType.Equals(0))
                        {
                           
                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            OrderDateto.SelectedDate = System.DateTime.Now;
                           

                        }
                        else
                        {

                        

                            OrderDatefrom.SelectedDate = System.DateTime.Now;
                            OrderDateto.SelectedDate = System.DateTime.Now;

                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }


      // SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
       // DataTable dt = new DataTable();
       private void Bindata()
       {
           using (SqlConnection con = new SqlConnection(CnnString))
           {
               DateTime fromdate = OrderDatefrom.SelectedDate;
               DateTime Todate=OrderDateto.SelectedDate;
               string strFromdate = fromdate.ToString("dd-MMM-yyyy");
               string strTodate = Todate.ToString("dd-MMM-yyyy");
               string FromDateNew = strFromdate + " 00:00:00";
               string TODateNew = strTodate + " 23:59:59";
              // cmd.Parameters.AddWithValue("@fromdate", "17-Aug-2014 00:00:00");
               //cmd.Parameters.AddWithValue("@todate", "22-Aug-2014 23:59:59");

               
               SQLString = "exec HRweekwisereport '" + FromDateNew + "', '" + TODateNew + "'";
               DataTable dt = Utils.GetDataTable(SQLString, CnnString);


               GridView1.DataSource = dt;
               GridView1.DataBind();
               ViewState["GridData"] = dt;
           }
       }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //
        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
           // SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            Bindata();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "HRweekwisereport.xls"));
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            GridView1.AllowPaging = false;

            DataTable dtExport = new DataTable();
            dtExport = (DataTable)ViewState["GridData"];

            GridView1.DataSource = dtExport;
            GridView1.DataBind();
            if (dtExport.Rows.Count > 0)
            {
                //Change the Header Row back to white color
                GridView1.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
                {
                    GridView1.HeaderRow.Cells[i].Style.Add("background-color", "#507CD1");
                }
                int j = 1;
                //This loop is used to apply stlye to cells based on particular row
                foreach (GridViewRow gvrow in GridView1.Rows)
                {
                    gvrow.BackColor = Color.White;
                    if (j <= GridView1.Rows.Count)
                    {
                        if (j % 2 != 0)
                        {
                            for (int k = 0; k < gvrow.Cells.Count; k++)
                            {
                                gvrow.Cells[k].Style.Add("background-color", "#EFF3FB");
                            }
                        }
                    }
                    j++;
                }
                GridView1.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
            else
            {
                //
            }
        }
    }
}