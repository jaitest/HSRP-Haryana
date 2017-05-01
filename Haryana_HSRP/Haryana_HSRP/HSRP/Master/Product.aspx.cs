using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataProvider;
using System.Data; 
using HSRP; 
using System.Data.SqlClient;



namespace HSRP.Master
{
    public partial class Product : System.Web.UI.Page
    {
        string  ModeType; 
        string SQLString = string.Empty;
        int ProductID;
        string ConnectionString;
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                lblErrMess.Text = "";
                lblSucMess.Text = ""; 
                buttonUpdate.Visible = false; 
                ModeType = Request.QueryString["Mode"];

                if (ModeType == "Edit")
                { 
                    ProductID = Convert.ToInt16( Request.QueryString["ProductID"]);
                    ProductEdit(ProductID); 
                }
            }
        }


        protected void ButtonProductSave_Click(object sender, EventArgs e)
        { 
             lblErrMess.Text="";
             lblSucMess.Text = "";
            string ActiveStatus;
            if (checkBoxActiveStatus.Checked == true)
            {
                ActiveStatus = "Y";
            }
            else
            {
                ActiveStatus = "N";
            }
            int IsExists = -1;
             
            BAL obj = new BAL();
            obj.InsertHSRPPruduct(textboxProductCode.Text, textboxProductColor.Text, ActiveStatus, textboxProductDimension.Text, ref IsExists);
            if (IsExists.Equals(1))
            { 
                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Update Successfully."; 
            }
        }

        protected void buttonUpdate_Click(object sender, EventArgs e)
        { 
            lblErrMess.Text = "";
            lblSucMess.Text = "";
            ProductID = Convert.ToInt16(Request.QueryString["ProductID"]); 
            string ActiveStatus;
            if (checkBoxActiveStatus.Checked == true)
            {
                ActiveStatus = "Y";
            }
            else
            {
                ActiveStatus = "N";
            }
            int IsExists = -1;

            BAL obj = new BAL();
            obj.UpdateHSRPPruduct( ProductID, textboxProductCode.Text, textboxProductColor.Text, ActiveStatus, textboxProductDimension.Text, ref IsExists);
            if (IsExists.Equals(1))
            {

                lblErrMess.Text = "Record Already Exist!!";
            }
            else
            {
                lblSucMess.Text = "Record Update Successfully."; 
            }
        }

        private void ProductEdit(int ProductID)
        {
            buttonUpdate.Visible = true;
            ButtonProductSave.Visible = false; 
            buttonUpdate.Visible = true;
            ButtonProductSave.Visible = false;
            SQLString = "select * from Product where ProductID=" + ProductID;
            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            textboxProductCode.Text = ds.Rows[0]["ProductCode"].ToString();
            textboxProductDimension.Text = ds.Rows[0]["ProductDimension"].ToString();
            textboxProductColor.Text = ds.Rows[0]["ProductColor"].ToString();
            string check = ds.Rows[0]["ActiveStatus"].ToString();
            if (check == "Y")
            {
                checkBoxActiveStatus.Checked = true;
            }
            else
            {
                checkBoxActiveStatus.Checked = false;
            }
        }
          
    }
}