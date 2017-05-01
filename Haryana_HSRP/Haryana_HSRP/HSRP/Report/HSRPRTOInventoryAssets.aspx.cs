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
    public partial class HSRPRTOInventoryAssets : System.Web.UI.Page
    {
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        string HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
        string SQLString = string.Empty;
        String StringMode = string.Empty;
        string CurrentDate = DateTime.Now.ToString();
        string query1 = string.Empty;
        string UserFirstName = string.Empty;

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
                //UserName = Session["UID"].ToString();
              //  UserFirstName = ddl_user.SelectedValue;
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;
                //CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }

            //if (string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()))
            //{
            //    Response.Write("<script language='javascript'> {window.close();} </script>");
            //}
            //else
            //{
            //    StringMode = Request.QueryString["Mode"].ToString();
            //}

            if (StringMode.Equals("Edit"))
            {
                ProductivityID = Request.QueryString["TransactionID"].ToString();
               // buttonUpdate.Visible = true;
                buttonSave.Visible = false;


            }
            else
            {
                buttonSave.Visible = true;
                //buttonUpdate.Visible = false;
            }
            if (!Page.IsPostBack)
            {
             
                if (StringMode.Equals("Edit"))
                {
                    //EditBankTransaction(ProductivityID);


                }

               // InitialSetting();

                string SQLQuery = string.Empty;
                string rtolocationname = string.Empty;
                string rtolocationid = string.Empty;
                string Embname = string.Empty;
                DataTable dtrto = new DataTable();
                // LblRTO.Text = RTOLocationID.ToString();
                SQLQuery = "select * from rtolocation where rtolocationid='" + RTOLocationID + "'";
                dtrto = Utils.GetDataTable(SQLQuery, ConnectionString);
                rtolocationname = dtrto.Rows[0]["rtolocationname"].ToString();
               // rtolocationid = dtrto.Rows[0]["Rtolocationid"].ToString();
                Embname = dtrto.Rows[0]["EmbCenterName"].ToString();
                LblRTO.Text = rtolocationname;
                lblembname.Text = Embname;

            }
            //  FillUsers();

        }

      
   
        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string rtoName = string.Empty;
                string rtoid = string.Empty;
                string productName = string.Empty;
                string make = string.Empty;
                string model = string.Empty;
                string serial = string.Empty;
                string status = string.Empty;
                string UserName = string.Empty;
                string mobile = string.Empty;
                string remarks = string.Empty;

                rtoName = LblRTO.Text;
               // rtoid = LblRTO.Text;
                productName = DropDownListProduct.SelectedItem.ToString();
                make = txtBoxMake.Text;
                model = TextBoxModel.Text;
                serial = txtBoxSerial.Text;
                status = DropDownListStation.SelectedItem.ToString();
                UserName = TextBoxUser.Text;
                mobile = TextBoxMobile.Text;
                remarks = TextBoxRemarks.Text;


                if (DropDownListProduct.SelectedItem.Text.Trim() == "--Select Product--")
                {
                    lblErrMess.Text = ("Please select Product.");
                    return;
                }

                if (txtBoxMake.Text.Trim() == "")
                {
                    lblErrMess.Text = ("Please Provide Make");
                    return;
                }
                if (TextBoxModel.Text.Trim() == "")
                {
                    lblErrMess.Text=("Please Provide Model");
                    return;
                }
                if (txtBoxSerial.Text.Trim() == "")
                {
                    lblErrMess.Text=("Please Provide Serial No.");
                    return;
                }
                if (DropDownListStation.SelectedItem.Text.Trim() == "--Select--")
                {
                    lblErrMess.Text = ("Please Select Status ");
                    return;;
                }
                if (TextBoxUser.Text.Trim() == "")
                {
                    lblErrMess.Text= ("Please Provide User Name");
                    return;
                }
                if (TextBoxMobile.Text.Trim() == "")
                {
                    lblErrMess.Text = ("Please Provide Mobile No.");
                    return;
                }

                query1 = "insert into hsrp_Assets(HSRP_StateID,RTOLocationID,ProductName,Make,Model,SerialNo,WorkingStatus,UserName,UserMobileNo,Remarks) values(" + HSRPStateID + ",'" + RTOLocationID + "','" + productName + "','" + make + "','" + model + "','" + serial + "','" + status + "','" + UserName + "','" + mobile + "','" + remarks + "')";

                int count = Utils.ExecNonQuery(query1, ConnectionString);
                if (count > 0)
                {
                    lblSucMess.Text = "Records has been Saved Successfully";
                }
                else
                {
                    lblErrMess.Text = "Record Not Saved";
                }
            }
            catch (Exception ex)
            {
                string script = "<script type=\"text/javascript\">  alert('error message ');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);

            }

        }

        public void referess()
        {

            DropDownListProduct.SelectedIndex = -1;
            
            
            txtBoxMake.Text = "";
            TextBoxModel.Text = "";
            txtBoxSerial.Text = "";
            DropDownListStation.SelectedIndex = -1;
            TextBoxUser.Text = "";
            TextBoxMobile.Text = "";
            TextBoxRemarks.Text = "";
        }

        protected void btnclose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LiveReports/LiveTracking.aspx");
        }



        protected void btnReset_Click(object sender, EventArgs e)
        {
            referess();
        }

        protected void DropDownListProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       






    }
}