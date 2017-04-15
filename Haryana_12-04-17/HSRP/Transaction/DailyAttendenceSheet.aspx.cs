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
    public partial class DailyAttendenceSheet : System.Web.UI.Page
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
      
        string StringEmbcode = string.Empty;
        string StringDesignation = string.Empty;
        string StringTimeIN = string.Empty;
        string StringTimeout = string.Empty;
        string SQLString = string.Empty;
        string sqlQuery12 = string.Empty;
        string HSRP_StateID = string.Empty;
        string RTOLocationID = string.Empty;
        string USERID = string.Empty;
        string strtimeinto = string.Empty;
        string strtimeoutto = string.Empty; 
        string strempid =string.Empty;
        string Empidd = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                USERID = Session["UID"].ToString();
            }

            if (!Page.IsPostBack)
            {
                // textBoxUserName.Text = Session["UserName"].ToString();   
                FilldropDownListClient();
            }
            else
            {
                //FilldropDownListClient();
            }


          
        }


        public void FilldropDownListClient()
        {

            string SQLStringemp = "select emp_name,emp_id from employeemaster a,AttZoneMaster b where a.attzoneid=b.attzoneid and userid='" + USERID + "' order by emp_name";
            //Utils.PopulateDropDownList(ddlZone, SQLStringemp.ToString(), CnnString, "--Select Employee Name--");
            DataTable dtemp = Utils.GetDataTable(SQLStringemp, CnnString);
            ddlZone.DataSource = dtemp;
            ddlZone.DataTextField = "emp_name";
            ddlZone.DataValueField = "emp_id";
            ddlZone.DataBind();

            ddlZone.Items.Insert(0, "--Select Employee Name--");
            ddlZone.Items[0].Value = "0";

            string strempcode=dtemp.Rows[0]["emp_id"].ToString();

             string SQLStringID = "select ID,Emp_ID from employeemaster where emp_id='"+strempcode+"'";
             DataTable dtemp12 = Utils.GetDataTable(SQLStringID, CnnString);
             strempid =dtemp12.Rows[0]["ID"].ToString();
             ViewState["IDD"] = strempid;

            //string SQLString21 = "select attZoneID,AttZoneName from AttzoneMaster where HSRP_StateID='" + HSRP_StateID + "'";
            //DataTable dt = Utils.GetDataTable(SQLString21, CnnString);
            //ddlZone.DataSource = dt;
            //ddlZone.DataTextField = "AttZoneName";
            //ddlZone.DataValueField = "attZoneID";
            //ddlZone.DataBind();

            //ddlZone.Items.Insert(0, "--Select Attendence Zone--");
            //ddlZone.Items[0].Value = "0";

        }

      


        protected void buttonSave_Click(object sender, EventArgs e)
        {
            lblErrMess.Text = string.Empty;
            lblSucMess.Text = string.Empty;
            //StringEmbcode = txtEmbcode.Text.Trim();
            //StringDesignation = txtDesignation.Text.Trim();
            StringTimeIN = txtTimein.Text.Trim();
            StringTimeout = txttimeout.Text.Trim();
            //if (string.IsNullOrEmpty(StringEmbcode))
            //{
            //    lblErrMess.Text = "Please Provide Emp Code";
            //    txtEmbcode.Focus();
            //    return;
            //}
            //if (string.IsNullOrEmpty(StringDesignation))
            //{
            //    lblErrMess.Text = "Please Provide Designation";
            //    txtDesignation.Focus();
            //    return;
            //}
            if (string.IsNullOrEmpty(StringTimeIN))
            {
                lblErrMess.Text = "Please Provide Time IN";
                txtTimein.Focus();
                return;
            }

            //if (string.IsNullOrEmpty(StringTimeout))
            //{
            //    lblErrMess.Text = "Please Provide Time OUT";
            //    txttimeout.Focus();
            //    return;
            //}

            //string strtimeout = txttimeout.Text.Trim();
            //string hour1 = strtimeout.Substring(0, 1);
            //string time1 = strtimeout.Substring(1, 2);
            //if (hour1.Substring(0, 1) == "0")
            //    hour1 = hour1.Substring(1, 1);
            //StringTimeout = (hour1 + ":" + time1);
            //strtimeoutto = StringTimeout + " PM";
            //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

           // DateTime attendence = DateTime.ParseExact(DateTime.Now.ToString(), "yyyy-MM-dd HH:mm tt",null);

            //Regex regexp = new Regex("^(1[0-2]|0[1-9]):[0-5][0-9]\040(AM|am|PM|pm)$");
            //if (!regexp.IsMatch(txtTimein.Text))
            //{
            //    Response.Write("<script>window.alert('Please enter time in the following format HH:MMAM/PM')</script>");
            //    return;
            //}

            //Regex regexp = new Regex(@"^(([0]?[1-9])|([1][0-2])):(([0-5][0-9])|([1-9])):([0-5][0-9]) [AP][M]$");
            //if (!regexp.IsMatch(txtTimein.Text))
            //{
            //    Response.Write("<script>window.alert('Please enter time in the following format HH:MMAM/PM')</script>");
            //    return;
            //}

            int strattendence;
            string todaydate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            string sqlQuerydep = "select count(entrydate) from DailyAttandenceEmp where convert(date,entrydate,103)='" + todaydate + "' ";
            strattendence = Utils.getScalarCount(sqlQuerydep, CnnString);
            if (strattendence > 0)
            {

                if (txttimeout.Text.Trim() == "")
                {
                    string strtimeout1 = (txttimeout.Text != null) ? txttimeout.Text : string.Empty;
                    //sqlQuery12 = "insert into DailyAttandenceEmp(UserID,HSRP_ID,RToLocationid,EmbCode,Designation,TimeIN,TimeOUT) values('" + USERID + "','" + HSRP_StateID + "','" + RTOLocationID + "', '" + StringEmbcode + "', '" + StringDesignation + "','" + strtimeinto + "','" + strtimeoutto + "')";
                    sqlQuery12 = "Update DailyAttandenceEmp set TimeOUT='" + strtimeout1 + "' where userid='" + USERID + "'";
                    int count1 = Utils.ExecNonQuery(sqlQuery12, CnnString);
                    if (count1 > 0)
                    {
                        lblSucMess.Text = "Your Attandence has been Already Marks";
                        return;
                    }
                    else
                    {
                        lblErrMess.Text = "Record Not Saved";
                        return;
                    }

                }
                else
                {
                    //Regex regexpout = new Regex(@"^(([0]?[1-9])|([1][0-2])):(([0-5][0-9])|([1-9])):([0-5][0-9]) [AP][M]$");
                    //if (!regexpout.IsMatch(txttimeout.Text))
                    //{
                    //    Response.Write("<script>window.alert('Please enter time in the following format HH:MMAM/PM')</script>");
                    //    return;
                    //}
                    strtimeoutto = StringTimeout;
                   
                    //sqlQuery12 = "insert into DailyAttandenceEmp(UserID,HSRP_ID,RToLocationid,EmbCode,Designation,TimeIN,TimeOUT) values('" + USERID + "','" + HSRP_StateID + "','" + RTOLocationID + "', '" + StringEmbcode + "', '" + StringDesignation + "','" + strtimeinto + "','" + strtimeoutto + "')";
                    sqlQuery12 = "update DailyAttandenceEmp set TimeOUT='" + strtimeoutto + "' where UserID='" + USERID + "'";
                    int count = Utils.ExecNonQuery(sqlQuery12, CnnString);
                    if (count > 0)
                    {
                        lblSucMess.Text = "Your Attandence has been Marks";
                    }
                    else
                    {
                        lblErrMess.Text = "Record Not Saved";
                    }

                }
            }
            

            //string strtime1 =txtTimein.Text.Trim();
            //string hour = strtime1.Substring(0, 1);
            //string time = strtime1.Substring(1, 2);
            //if (hour.Substring(0, 1) == "0")
            //hour = hour.Substring(1, 1);
            //StringTimeIN = (hour + ":" + time);

            strtimeinto = StringTimeIN;

            Empidd=ViewState["IDD"].ToString();

            if (txttimeout.Text.Trim()== "")
            {
                string strtimeout1 = (txttimeout.Text != null) ? txttimeout.Text : string.Empty;
                sqlQuery12 = "insert into DailyAttandenceEmp(UserID,HSRP_ID,RToLocationid,EmbCode,Designation,TimeIN,TimeOUT) values('" + USERID + "','" + HSRP_StateID + "','" + RTOLocationID + "', '" + Empidd + "', '" + StringDesignation + "','" + strtimeinto + "','" + strtimeout1 + "')";
                int count1 = Utils.ExecNonQuery(sqlQuery12, CnnString);
                if (count1 > 0)
                {
                    lblSucMess.Text = "Your Attandence has been Marks";
                    return;
                }
                else
                {
                    lblErrMess.Text = "Record Not Saved";
                    return;
                }
            }
            


        }

        protected void txtTimein_TextChanged(object sender, EventArgs e)
        {
            Regex reg = new Regex(@"^((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))$|^([01]\d|2[0-3])(:[0-5]\d){0,2}$");
            Regex regNum = new Regex(@"^((0?[1-9]|1[012])([0-5]\d){0,2})$|^([01]\d|2[0-3])([0-5]\d){0,2}$");
            if (!reg.IsMatch(txtTimein.Text))
            {
                if (!regNum.IsMatch(txtTimein.Text))
                {
                    txtTimein.Text = strtimeinto.ToString();
                }
                else
                {
                    txtTimein.Text = txtTimein.Text.Insert(txtTimein.Text.Length - 2, ":");
                }
            }
        }

        protected void txttimeout_TextChanged(object sender, EventArgs e)
        {
            Regex reg = new Regex(@"^((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))$|^([01]\d|2[0-3])(:[0-5]\d){0,2}$");
            Regex regNum = new Regex(@"^((0?[1-9]|1[012])([0-5]\d){0,2})$|^([01]\d|2[0-3])([0-5]\d){0,2}$");
            if (!reg.IsMatch(txtTimein.Text))
            {
                if (!regNum.IsMatch(txttimeout.Text))
                {
                    txttimeout.Text = strtimeinto.ToString();
                }
                else
                {
                    txttimeout.Text = txttimeout.Text.Insert(txttimeout.Text.Length - 2, ":");
                }
            }

        }

        //protected void ddlZone_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlZone.SelectedValue == "0")
        //    {
        //        txtEmbcode.Text = "";
        //    }
        //    else
        //    {
                
        //        string SQLString12 = "select * from Embployeemaster where Attzoneid='" +ddlZone.SelectedValue.ToString()+ "'";
        //        DataTable dt = Utils.GetDataTable(SQLString12, CnnString);
        //        txtEmbcode.Text = dt.Rows[0]["Emp_id"].ToString();
        //    }
        
    }
}
