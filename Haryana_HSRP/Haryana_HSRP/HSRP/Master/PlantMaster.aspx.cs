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
    public partial class PlantMaster1 : System.Web.UI.Page
    {
        DataProvider.BAL bl = new DataProvider.BAL();
        public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        string HSRPStateID, RTOLocationID, PlantID, UserType, UserName;
        string SQLString = string.Empty;
       String CnnString, StringMode;
        string CurrentDate = DateTime.Now.ToString();
    




        protected void Page_Load(object sender, EventArgs e)
        {
            UserType = Session["UserType"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UserName"].ToString();
            Utils.GZipEncodePage();
            if (!IsPostBack)
            {
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    UserType = Session["UserType"].ToString();
                    HSRPStateID = Session["UserHSRPStateID"].ToString();
                    RTOLocationID = Session["UserRTOLocationID"].ToString();
                    UserName = Session["UserName"].ToString();
                    lblErrMess.Text = string.Empty;
                    lblSucMess.Text = string.Empty;
                    state();
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
                    PlantID = Request.QueryString["PlantID"].ToString();
                    buttonUpdate.Visible = true;
                    buttonSave.Visible = false;
                    state();

                }
                else
                {
                    buttonSave.Visible = true;
                    //buttonUpdate.Visible = false;
                }

                if (StringMode.Equals("Edit"))
                {
                    EditPlatMaster(PlantID);
                   // state();
                }


            }

        }

        string ActiveStatus1;
        //string PlantID1;


        public void state()
        {
            SQLString = "select * from dbo.HSRPState ORDER BY HSRPStateName";

            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            DropDownListPlantState.DataSource = ds;
            DropDownListPlantState.DataTextField = "HSRPStateName";
            DropDownListPlantState.DataValueField = "HSRP_StateID";
            DropDownListPlantState.DataBind();
            DropDownListPlantState.Items.Insert(0, "Select");
            DropDownListPlantState.Items[0].Value = "0";
        }

        private void EditPlatMaster(string PlantID)
        {

            SQLString = "SELECT [Plant].* , HSRPState.HSRPStateName, HSRPState.HSRP_StateID FROM [Plant] inner join dbo.HSRPState on HSRPState.HSRP_StateID=[Plant].PlantState where PlantID=" + PlantID;
            DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);

            TextBoxPlantAddress.Text = dt.Rows[0]["PlantAddress"].ToString();
            TextBoxPlantCity.Text = dt.Rows[0]["PlantCity"].ToString();


            //DropDownListPlantState.DataSource = ds;
            DropDownListPlantState.SelectedItem.Text = dt.Rows[0]["HSRPStateName"].ToString();
            DropDownListPlantState.SelectedValue = dt.Rows[0]["PlantState"].ToString(); 
            //DropDownListPlantState.DataBind();
            //DropDownListPlantState.DataTextField = "HSRPStateName";
            //DropDownListPlantState.DataValueField = "PlantState";

            //TextBoxPlantState.Text = ds.Rows[0]["PlantState"].ToString();
            TextBoxPlantZip.Text = dt.Rows[0]["PlantZip"].ToString();
            TextBoxContactPersonName.Text = dt.Rows[0]["ContactPersonName"].ToString();
            TextBoxMobileNo.Text = dt.Rows[0]["MobileNo"].ToString();
            TextBoxLandlineNo.Text = dt.Rows[0]["LandlineNo"].ToString();
            TextBoxEmailId.Text = dt.Rows[0]["EmailID"].ToString();
            ActiveStatus1 = dt.Rows[0]["ActiveStatus"].ToString();
            if (ActiveStatus1 == "Y")
            {
                checkBoxActiveStatus.Checked = true;
            }
            else
            {
                checkBoxActiveStatus.Checked = false;
            }

            H_PlantID1.Value = dt.Rows[0]["PlantID"].ToString();



        }


        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            string ActiveStatus;
           
            List<string> lst = new List<string>();
            lst.Add(TextBoxPlantAddress.Text);
            lst.Add(TextBoxPlantCity.Text);
            lst.Add(DropDownListPlantState.SelectedValue.ToString());
            lst.Add(TextBoxPlantZip.Text);
            lst.Add(TextBoxContactPersonName.Text);
            lst.Add(TextBoxMobileNo.Text);
            lst.Add(TextBoxLandlineNo.Text);
            lst.Add(TextBoxEmailId.Text);
            if (checkBoxActiveStatus.Checked == true)
            {
                ActiveStatus = "Y";
            }
            else
            {
                ActiveStatus = "N";
            }
            lst.Add(ActiveStatus);


            int i = bl.SavePlantDetail(lst);
            if (i >= 1)
            {
            
                    lblSucMess.Text = "Record Save sucessfully";
                    //string closescript1 = "<script>alert('Record save sucessfully.')</script>";
                    //Page.RegisterStartupScript("abc", closescript1);

                   refresh();
                }
                else
                {

                    lblErrMess.Text = "Record Already Exist!!";
                    //string closescript1 = "<script>alert('Record save not sucessfully.')</script>";
                    //Page.RegisterStartupScript("abc", closescript1);
                   refresh();
                }




        }

       
        protected void buttonUpdate_Click(object sender, EventArgs e)
        {

            string ActiveStatus;
           
            List<string> lst = new List<string>();
            lst.Add(TextBoxPlantAddress.Text);
            lst.Add(TextBoxPlantCity.Text);
            lst.Add(DropDownListPlantState.SelectedValue.ToString());
            lst.Add(TextBoxPlantZip.Text);
            lst.Add(TextBoxContactPersonName.Text);
            lst.Add(TextBoxMobileNo.Text);
            lst.Add(TextBoxLandlineNo.Text);
            lst.Add(TextBoxEmailId.Text);
            if (checkBoxActiveStatus.Checked == true)
            {
                ActiveStatus = "Y";
            }
            else
            {
                ActiveStatus = "N";
            }
            lst.Add(ActiveStatus);
            lst.Add(H_PlantID1.Value.ToString());


            int i = bl.UpdatePlantDetail(lst);
            if (i >= 1)
            {


                lblSucMess.Text = "Record Update sucessfully";
              

                refresh();
            }
            else
            {

                lblErrMess.Text = "Record Already Exist!!";
               
                refresh();
            }



        }

        public void refresh()
        {
           TextBoxPlantAddress.Text="";
           TextBoxPlantCity.Text="";
          
           TextBoxPlantZip.Text="";
           TextBoxContactPersonName.Text="";
           TextBoxMobileNo.Text="";
           TextBoxLandlineNo.Text="";
           TextBoxEmailId.Text = "";
           state();
        }

    }
}