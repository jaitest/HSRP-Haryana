using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI; 
namespace HSRP
{
    public partial class Main1 : System.Web.UI.MasterPage
    {

        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string SQLString = String.Empty;
        string StateID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
           // Utils.GZipEncodePage();
           // LabelError.Visible = false;
            if (!IsPostBack)
            {
               // string sd = Session["UID"].ToString();
               if (Session["UID"].ToString() == null)
                {
                    //Response.Redirect("~/Login.aspx", true);
                    Response.Redirect("~/LogOut.aspx", true);
                }
                else
                {
                    StateID = Session["UserHSRPStateID"].ToString();
                    SQLString = "Select CompanyName from HSRPState where HSRP_StateID=" + StateID;
                    labelCompanyName.Text = Utils.getDataSingleValue(SQLString, CnnString, "CompanyName").ToString();
               
                    String UserID = string.Empty;
                    UserID = Session["UID"].ToString();

                    lblUser.Text = Session["UserName"].ToString() + " / Your  Location : " + Session["RTOLocationName"].ToString();
                    Utils.buildMenu(Menu12, UserID);

                    string mac = Session["MacAddress"].ToString();

                   
                    Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + Session["UID"].ToString() + "', LastLoggedInDatetime =GetDate() where MacAddress='" + Session["MacAddress"].ToString() + "'", CnnString);

                }
            }
            
        }

        protected void Menu12_ItemSelected(object sender, MenuItemEventArgs e)
        {
            Session["M"] = e.Item.ID.ToString();
        } 
        
        protected void buttonLogout_Click(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                string login = "~/LogOut.aspx";
                Session.Abandon();
                Session.Clear();
                Response.CacheControl = "no-cache";
                Response.Redirect(login);
            }
            else
            {
                string login = string.Empty;
                string wmac = Session["macbaseflag"].ToString();
                if (wmac == "N")
                {
                    login = "~/Login.aspx?X=" + Session["MacAddress"].ToString();
                }
                else
                {
                    Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where MacAddress='" + Session["MacAddress"].ToString() + "'", CnnString);
                    login = "~/Login.aspx";
                }
                Session.Abandon();
                Session.Clear();
                Response.CacheControl = "no-cache";
                Response.Redirect(login);


            }

        }

      

       

    }
}