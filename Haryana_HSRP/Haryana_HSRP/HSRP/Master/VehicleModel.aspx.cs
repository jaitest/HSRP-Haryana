using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;
using HSRP;


namespace HSRP.Master
{
    public partial class VhicleModel : System.Web.UI.Page
    {
        string Mode = string.Empty;
        string HSRP_StateID = string.Empty;
        string SQLString = string.Empty;
        string UserType = string.Empty;
        string EditStateID = string.Empty;
        string ActiveStatus = string.Empty;
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        DataProvider.BAL bl = new DataProvider.BAL();


        protected void Page_Load(object sender, EventArgs e)
        {

            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }

            HSRP_StateID = Session["UserHSRPStateID"].ToString();
            UserType = Session["UserType"].ToString();
            if (!Page.IsPostBack)
            {

                showModelName();
                buttonUpdate.Visible = false;
                Mode = Request.QueryString["Mode"];
                if (Mode == "Edit")
                {
                    H_VehicleModelID.Value= Request.QueryString["VehicleModelID"].ToString();
                    buttonUpdate.Visible = true;
                    ButtonSave.Visible = false;
                    Edit(H_VehicleModelID.Value.ToString());
                }

            }
        }

        public void showModelName()
        {
            string SQLString = "SELECT  [VehicleMakerID],[VehicleMakerDescription],[ActiveStatus] FROM [VehicleMakerMaster] order by VehicleMakerDescription ";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), ConnectionString.ToString());

            DropDownListVehicleMaker.DataSource = dt;
            DropDownListVehicleMaker.DataTextField = "VehicleMakerDescription";
            DropDownListVehicleMaker.DataValueField = "VehicleMakerID";
            DropDownListVehicleMaker.DataBind();

            DropDownListVehicleMaker.Items.Insert(0, ".....Select Model Name.....");
            DropDownListVehicleMaker.Items[0].Value = "0";
        }

        public void Edit(string VehicleModelID)
        {
            string SQLString = "select VehicleModelMaster.* ,VehicleMakerMaster.VehicleMakerDescription from VehicleModelMaster inner join VehicleMakerMaster on VehicleModelMaster.VehicleMakerID=VehicleMakerMaster.VehicleMakerID  where VehicleModelID='" + VehicleModelID + "' ";
            DataTable dt = Utils.GetDataTable(SQLString.ToString(), ConnectionString.ToString());
            DropDownListVehicleMaker.SelectedValue = dt.Rows[0]["VehicleMakerID"].ToString();
            DropDownListVehicleMaker.SelectedItem.Text = dt.Rows[0]["VehicleMakerDescription"].ToString();
            textboxVehicleModel.Text = dt.Rows[0]["VehicleModelDescription"].ToString();
           string ActiveStatus1 = dt.Rows[0]["ActiveStatus"].ToString();

           if (ActiveStatus1 == "Y")
           {
               checkBoxActiveStatus.Checked= true;
           }
           else
           {
               checkBoxActiveStatus.Checked = false;
           }
           
        }


        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            lst.Add(DropDownListVehicleMaker.SelectedValue.ToString());
            lst.Add(textboxVehicleModel.Text);

            if (checkBoxActiveStatus.Checked == true)
            {
                ActiveStatus = "Y";
                lst.Add(ActiveStatus);
            }
            else
            {
                ActiveStatus = "N";
                lst.Add(ActiveStatus);
            }

            lst.Add(Session["UID"].ToString());
            lst.Add(H_VehicleModelID.Value.ToString());
            int i = bl.UpdateVehicleModel(lst);
            if (i > 0)
            {
                string script = "<script type=\"text/javascript\">  alert('Update Successfully');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                Refresh();
            }
            else
            {
                string script = "<script type=\"text/javascript\">  alert('this value already exists');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                Refresh();
            }
           

        }

        

        protected void ButtonSave_Click1(object sender, EventArgs e)
        {

            //lblErrMess.Text = "";
            //lblSucMess.Text = "";

            List<string> lst = new List<string>();
            lst.Add(DropDownListVehicleMaker.SelectedValue.ToString());
            lst.Add(textboxVehicleModel.Text);
                        
            if (checkBoxActiveStatus.Checked == true)
            {
                ActiveStatus = "Y";
                lst.Add(ActiveStatus);
            }
            else
            {
                ActiveStatus = "N";
                lst.Add(ActiveStatus);
            }

            lst.Add(Session["UID"].ToString());
            int i= bl.SaveVehicleModel(lst);

            if (i > 0)
            {
                string script = "<script type=\"text/javascript\">  alert('Save Successfully');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                Refresh();
            }
            else
            {
                string script = "<script type=\"text/javascript\">  alert('this value already exists');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script);
                Refresh();
            }

            
        }

        public void Refresh()
        {
           // showModelName();
            textboxVehicleModel.Text = "";
            checkBoxActiveStatus.Checked = false;
        }
    }
}