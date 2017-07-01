using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Transaction
{
    public partial class dateTest : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            String strquery1 = "select [dbo].GetAffxDate_Insert(9) as Date";
            string strDate = Utils.getScalarValue(strquery1, ConnectionString);
            lblDate.Text = strDate;
        }

       // public string CnnString { get; set; }
    }
}