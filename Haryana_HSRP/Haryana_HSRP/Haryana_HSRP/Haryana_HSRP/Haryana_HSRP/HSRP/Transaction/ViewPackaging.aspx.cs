using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace HSRP.Transaction
{
    public partial class ViewPackaging : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string SQLString = "select Packaging.*,Product.productcode from Packaging inner join Product on Product.productid=Packaging.productid order by PackagingID desc";
                DataTable dt = Utils.GetDataTable(SQLString.ToString(), ConnectionString.ToString());
                Grid1.DataSource = dt;
                Grid1.DataBind();
            }
        }
    }
}