using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class PlateRates : System.Web.UI.Page
{
    public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();    
    String SQLString = String.Empty;
    String Status = String.Empty;
    // string SQLString = String.Empty;
    string CnnString = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            FillDetailsOfPlates();
        }
    }
   
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["REG"] = null;
        Response.Redirect("OrderStatus.aspx");
    }
    private void FillDetailsOfPlates()
    {
    //        SQLString="PlateRatesStaeWise '4'";
    //        DataTable dtResult = utils.GetDataTable(SQLString, ConnectionString);
    //        GridView1.DataSource = dtResult;
    //        GridView1.DataBind();
    }

}