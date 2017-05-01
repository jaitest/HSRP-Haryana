using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;

namespace HSRP.Master
{
    public partial class HSRPRTOInventory : System.Web.UI.Page
    {
        
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
        string SQLString = string.Empty;
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        string query1 = string.Empty;
       
        DataProvider.BAL bl = new DataProvider.BAL();
        



        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                UserType = Session["UserType"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                UserName = Session["UID"].ToString();
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
                //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }

            if (string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()))
            {
                Response.Write("<script language='javascript'> {window.close();} </script>");
            }
            else
            {
                StringMode = Request.QueryString["Mode"].ToString();
            }

            if (StringMode.Equals("Edit"))
            {
                ProductivityID = Request.QueryString["TransactionID"].ToString();
              
                buttonSave.Visible = false;

                
            }
            else
            {
                buttonSave.Visible = true;
                //buttonUpdate.Visible = false;
            }
            string SQLQuery = string.Empty;
            string rtolocationname = string.Empty;
            string Embname = string.Empty;
            DataTable dtrto = new DataTable();
           // LblRTO.Text = RTOLocationID.ToString();
            SQLQuery = "select * from rtolocation where rtolocationid='" + RTOLocationID + "'";
            dtrto = Utils.GetDataTable(SQLQuery, ConnectionString);
            rtolocationname = dtrto.Rows[0]["rtolocationname"].ToString();
            Embname = dtrto.Rows[0]["EmbCenterName"].ToString();
            LblRTO.Text = rtolocationname;
            LblEmb.Text = Embname;

        }

            
       

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
           
            try
            {
                string rtoName = string.Empty;
                string productName = string.Empty;
                string make = string.Empty;
                string model = string.Empty;
                string serial = string.Empty;
                string status = string.Empty;
                string UserName = string.Empty;
                string mobile = string.Empty;
                string remarks = string.Empty;

               

                if (DropDownListProduct.SelectedValue == "")
                {
                    lblErrMess.Text = ("Please Select Product ");
                }

                if ( txtBoxMake.Text == "")
                {
                    lblErrMess.Text = ("Please Provide Make");
                }
                if (TextBoxModel.Text == "")
                {
                    lblErrMess.Text = ("Please Provide Model");
                }
                if (txtBoxSerial.Text == "")
                {
                    lblErrMess.Text = ("Please Provide Serial No.");
                }
                if (DropDownListStation.Text == "")
                {
                    lblErrMess.Text = ("Please Select Status ");
                }
                if (TextBoxUser.Text == "")
                {
                    lblErrMess.Text = ("Please Provide User Name");
                }
                if (TextBoxMobile.Text == "")
                {
                    lblErrMess.Text = ("Please Provide Mobile No.");
                }
                rtoName = LblRTO.Text;
                productName=DropDownListProduct.SelectedItem.ToString();
                make = txtBoxMake.Text;
                model= TextBoxModel.Text;
                serial = txtBoxSerial.Text;
                status  = DropDownListStation.SelectedValue;
                UserName = TextBoxUser.Text;
                mobile = TextBoxMobile.Text;
                remarks = TextBoxRemarks.Text;
                query1 = "insert into tbl values(" + HSRPStateID + "," + rtoName + "," + productName + "," + make + "," + model + "," + serial + "," + status + "," + UserName + "," + mobile + "," + remarks + ")";
               
               int count= Utils.ExecNonQuery(query1, ConnectionString);
               if (count > 0)
               {
                   lblSucMess.Text = "Records has been SuccessFully Saved";
               }
               else
               {
                   lblErrMess.Text = "";
               }
             }
            catch (Exception ex)
            {
                string script = "<script type=\"text/javascript\">  alert('error message ');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
              
            }
           
        }

      
       
    }
}