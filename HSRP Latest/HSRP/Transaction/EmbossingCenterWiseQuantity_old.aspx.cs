using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Transaction
{
    public partial class EmbossingCenterWiseQuantity : System.Web.UI.Page
    {
        string Stringboxno = string.Empty;
        string Stringlaserfrom = string.Empty;
        string Stringlaserto = string.Empty;
        string Stringquantity = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string sqlQuery12 = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string USERID = string.Empty;
        string strtimeinto = string.Empty;
        string strtimeoutto = string.Empty;
        int UserType1;
         

        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType1 = Convert.ToInt32(Session["UserType"]);
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                USERID = Session["UID"].ToString();

            }
            if (!Page.IsPostBack)
            {
                //textBoxUserName.Text = Session["UserName"].ToString();
                FilldropDownListClient();
            
            }

 
        }

        private void FilldropDownListClient()
        {

            SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HSRP_StateID + " and navembid is not null  Order by EmbCenterName";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Embossing Center--");

        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            Stringboxno = txtboxno.Text.Trim();
            Stringlaserfrom = txtlaserfrom.Text.Trim();
            Stringlaserto = txtlaserto.Text.Trim();
            Stringquantity = txtQuantity.Text.Trim();
            //if (string.IsNullOrEmpty(StringEmbcode))
            //{
            //    lblErrMess.Text = "Please Provide Emp Code";
            //    txtEmbcode.Focus();
            //    return;
            //}

            if (dropDownListClient.SelectedItem.Text == "--Select Embossing Center--")
            {
                lblErrMess.Text = "Please Select Embossing Center";
                return;
            }
            if (string.IsNullOrEmpty(Stringboxno))
            {
                lblErrMess.Text = "Please Provide Box No.";
                txtboxno.Focus();
                return;
            }
            if (string.IsNullOrEmpty(Stringlaserfrom))
            {
                lblErrMess.Text = "Please Provide Laser from";
                txtlaserfrom.Focus();
                return;
            }

            if (string.IsNullOrEmpty(Stringlaserto))
            {
                lblErrMess.Text = "Please Provide Laser To";
                txtlaserto.Focus();
                return;
            }

            if (string.IsNullOrEmpty(Stringquantity))
            {
                lblErrMess.Text = "Please Provide Quantity";
                txtQuantity.Focus();
                return;
            }


           // CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //SQLString = "Select [Password] From Users where UserID='" + Convert.ToInt32(Session["UID"].ToString()) + "'";
            
            //if (!Utils.getDataSingleValue(SQLString, CnnString, "Password").Equals(StringOldPassword))
            //{
            //    lblErrMess.Text = "Old Password didn’t match.";
            //    textOldPassword.Text = string.Empty;
            //    textNewPassword.Text = string.Empty;
            //    textConfirmPassword.Text = string.Empty;
            //    textOldPassword.Focus();
            //    return;
            //}
            //if (!string.IsNullOrEmpty(StringNewPassword) && !string.IsNullOrEmpty(StringConfirmPassword))
            //{
            //    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            //    SQLString = "Update Users Set Password='" + StringNewPassword + "',PasswordChangedDate='" + DateTime.Now.ToString() + "' where UserID='" + Convert.ToInt32(Session["UID"].ToString()) + "'";
            //    if (Utils.ExecNonQuery(SQLString, CnnString) <= 0)
            //    {
            //        lblErrMess.Text = "Password Not Changed.";
            //    }
            //    else
            //    {
            //        lblSucMess.Text = "Password Changed sucessfully.";
            //    }

            //}
           //string todaydate = DateTime.Now.ToString("dd/MM/yyyy");
           //string sqlQuerydep = "select TimeIN from DailyAttandenceEmp where entrydate='" + todaydate + "' ";
           // DataTable dt = new DataTable();
           // dt = Utils.GetDataTable(sqlQuerydep, CnnString);
           // string timeingin = dt.Rows[0]["TimeIN"].ToString();
           // if (timeingin == DateTime.Now.ToString("dd/MM/yyyy"))
           // {
           //     lblSucMess.Text = "You have Already Entered Time.";
           //     return;
           // }


           // string strtime =txtTimein.Text.Trim();
           // string hour = strtime.Substring(0, 1);
           // string time = strtime.Substring(1, 2);
           // if (hour.Substring(0, 1) == "0")
           // hour = hour.Substring(1, 1);
           // StringTimeIN = (hour + ":" + time);

           // strtimeinto = StringTimeIN + " AM";
           // strtimeoutto = StringTimeout + " PM";

            //string sql12 = "select right(laserfrom,8) from EmbossingCenterLaserQuantity where laserfrom='" + Stringlaserfrom + "'";
            //DataTable dt1 = new DataTable();
            //dt1 = Utils.GetDataTable(sql12, CnnString);
            //string laserfrom = dt1.Rows[0]["laserfrom"].ToString();


            //string sql123 = "select right(LaserTo,8) from EmbossingCenterLaserQuantity where laserfrom='" + Stringlaserto + "'";
            //DataTable dtto = new DataTable();
            //dt1 = Utils.GetDataTable(sql123, CnnString);
            //string laserto = dt1.Rows[0]["LaserTo"].ToString();

            string SQLStringbox = "select count(BoxNO) from EmbossingCenterLaserQuantity where  BoxNO='" + Stringboxno + "'";
            int boxcount;
            boxcount = Utils.getScalarCount(SQLStringbox, CnnString);
            if (boxcount > 0)
            {
                lblErrMess.Text = "Box Number Already Exists";
                return;
            }

            string SQLStringfrom = "select count(laserfrom) from EmbossingCenterLaserQuantity where  laserfrom='" + Stringlaserfrom + "'";
            int fromlaser;
            fromlaser = Utils.getScalarCount(SQLStringfrom, CnnString);
            if (fromlaser> 0)
            {
                lblErrMess.Text = "From laser code Already Exists";
                return;
            }
            string SQLStringTo = "select count(LaserTo) from EmbossingCenterLaserQuantity where  LaserTo='" + Stringlaserto + "'";
            int fromlaserto;
            fromlaserto = Utils.getScalarCount(SQLStringTo, CnnString);
            if (fromlaserto> 0)
            {
                lblErrMess.Text = "To Laser code Already Exists";
                return;
            }

            sqlQuery12 = "insert into EmbossingCenterLaserQuantity(HSRP_ID,RToLocationid,EmbID,BoxNO,LaserFrom,LaserTo,Quantity) values('" + HSRP_StateID + "','" + RTOLocationID + "', '"+dropDownListClient.SelectedValue+"','" + Stringboxno + "', '" + Stringlaserfrom + "','" + Stringlaserto + "','" + Stringquantity + "')";
            int count = Utils.ExecNonQuery(sqlQuery12, CnnString);
            if (count > 0)
            {
                lblSucMess.Text = "Records has been Saved Successfully";
            }
            else
            {
                lblErrMess.Text = "Record Not Saved";
            }

        }

       
    }
}