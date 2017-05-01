using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using DataProvider;

namespace HSRP.Transaction
{
    public partial class MOProductivity : System.Web.UI.Page
    {



        string MOProductivityID = string.Empty, HSRPStateID = string.Empty, RTOLocationID = string.Empty, ProductivityID = string.Empty, UserType = string.Empty, UserName = string.Empty;
        String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        String CnnString = string.Empty, StringMode = string.Empty, SQLString = string.Empty;

        DataProvider.BAL bl = new DataProvider.BAL(); 


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
              
                buttonUpdate.Visible = false;
                string Mode = Request.QueryString["Mode"];
                
                if (Mode == "Edit")
                {
                    UserType = Session["UserType"].ToString();
                    HSRPStateID = Session["UserHSRPStateID"].ToString();
                    RTOLocationID = Session["UserRTOLocationID"].ToString();
                    UserName = Session["UID"].ToString();
                    ProductivityID = Request.QueryString["ProductivityID"].ToString();
                    buttonUpdate.Visible = true;
                    buttonSave.Visible = false;

                    editMOproductivity(ProductivityID);

                   
                }

           
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
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                Machine();
                product();
                RTOLocation();
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
                ProductivityID = Request.QueryString["ProductivityID"].ToString();
                buttonUpdate.Visible = true;
                buttonSave.Visible = false;
            }
            else
            {
                buttonSave.Visible = true;
                buttonUpdate.Visible = false;
            }
         
                if (StringMode.Equals("Edit"))
                {
                   // UpdateUserDetail(UserID);
                }

            }

        }

        public void RTOLocation()
        {
            SQLString = "select * from Plant  where  PlantState='" + HSRPStateID + "' order by PlantAddress";
            

            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            DropDownListPlantName.DataSource = ds;
            DropDownListPlantName.DataTextField = "PlantAddress";
            DropDownListPlantName.DataValueField = "PlantID";
            DropDownListPlantName.DataBind();
            DropDownListPlantName.Items.Insert(0, "Select");
            DropDownListPlantName.Items[0].Value = "0";
        }

        private void editMOproductivity(string ProductivityID)
        {
            //SQLString = "SELECT [MOProductivityID],[ProductivityDate],[MachineID],[OperatorName],[ProductID],[Quantity],[ScrapQuantity],[ScrapWeight],[Remarks],[StateID],[RTOLocation],[UserID] FROM [MachineOperatorProductivity]=" + MOProductivityID;
            SQLString = "SELECT RTOLocation,MOProductivityID,ProductivityDate,MachineOperatorProductivity.MachineID,OperatorName,MachineOperatorProductivity.ProductID,Quantity,ScrapQuantity,ScrapWeight,Remarks,StateID,PlantAddress,UserID  , Product.ProductID ,Product.ProductCode,MachineMaster.MachineID ,(MachineType+'-'+MachineName) as mtype ,Plant.PlantID,Plant.PlantAddress from MachineOperatorProductivity inner join Product on MachineOperatorProductivity.ProductID= Product.ProductID inner join MachineMaster on MachineOperatorProductivity.MachineID = MachineMaster.MachineID inner join Plant on MachineOperatorProductivity.PlantID=Plant.PlantID where MOProductivityID ='" + ProductivityID + "'";

            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            //DropDownListMachine.SelectedItem.Text = ds.Rows[0]["mtype"].ToString();
            DropDownListMachine.SelectedValue = ds.Rows[0]["MachineID"].ToString();
           

            DropDownListPlantName.SelectedValue = ds.Rows[0]["PlantID"].ToString();
           // DropDownListPlantName.SelectedItem.Text = ds.Rows[0]["PlantAddress"].ToString();

            TextBoxOperator.Text = ds.Rows[0]["OperatorName"].ToString();
            DropDownListProduct.SelectedValue = ds.Rows[0]["ProductID"].ToString();
           // DropDownListProduct.SelectedItem.Text = ds.Rows[0]["ProductCode"].ToString();
            TextBoxQuantity.Text = ds.Rows[0]["Quantity"].ToString();
            TextBoxScrapQty.Text = ds.Rows[0]["ScrapQuantity"].ToString();
            TextBoxScrapWeight.Text = ds.Rows[0]["ScrapWeight"].ToString();
            TextBoxRemarks.Text = ds.Rows[0]["Remarks"].ToString();
            MOProductivityID = ds.Rows[0]["Remarks"].ToString();
            HSRPStateID = ds.Rows[0]["StateID"].ToString();
            RTOLocationID = ds.Rows[0]["RTOLocation"].ToString();
            UserName = ds.Rows[0]["UserID"].ToString();
           // MOProductivityID1 = ds.Rows[0]["MOProductivityID"].ToString(); 
        }

        public void Machine()
        {
            SQLString = "select MachineID,(MachineType+'-'+MachineName) as mtype from MachineMaster order by mtype ";

            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            DropDownListMachine.DataSource = ds;
            DropDownListMachine.DataTextField = "mtype";
            DropDownListMachine.DataValueField = "MachineID";
            DropDownListMachine.DataBind();
            DropDownListMachine.Items.Insert(0, "Select");
            DropDownListMachine.Items[0].Value = "0";

           
        }

        public void product()
        {
            SQLString = "select ProductID ,ProductCode from Product where ActiveStatus='Y' and HSRP_StateID='" + HSRPStateID + "' order by ProductCode ";

            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            DropDownListProduct.DataSource = ds;
            DropDownListProduct.DataTextField = "ProductCode";
            DropDownListProduct.DataValueField = "ProductID";
            DropDownListProduct.DataBind();
            DropDownListProduct.Items.Insert(0, "Select");
            DropDownListProduct.Items[0].Value = "0";

        }


        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {


                    HSRPStateID = Session["UserHSRPStateID"].ToString();
                    RTOLocationID = Session["UserRTOLocationID"].ToString();
                    UserName = Session["UID"].ToString();
                     List<string> lst = new List<string>();
                   
                    lst.Add(DropDownListMachine.SelectedValue.ToString());
                    lst.Add(TextBoxOperator.Text);
                    lst.Add(DropDownListProduct.SelectedValue.ToString());
                    lst.Add(TextBoxQuantity.Text);
                    lst.Add(TextBoxScrapQty.Text);
                    lst.Add(TextBoxScrapWeight.Text);
                    lst.Add(TextBoxRemarks.Text);
                    lst.Add(HSRPStateID);
                    lst.Add(RTOLocationID);
                    lst.Add(UserName);
                    //lst.Add(ProductivityDate.ToString());
                    lst.Add(DropDownListPlantName.SelectedValue.ToString());
                    int i = bl.InsertMachineOperatorProductivity(lst);

                    if (i > 0)
                    {

                        lblSucMess.Text = "Record save sucessfully";
                        //string closescript1 = "<script>alert('Record save sucessfully.')</script>";
                        //Page.RegisterStartupScript("abc", closescript1);
                        
                        refres();
                    }
                    else
                    {

                        lblErrMess.Text = "Record Already Exist!!";
                        //string closescript1 = "<script>alert('Record save not sucessfully.')</script>";
                        //Page.RegisterStartupScript("abc", closescript1);
              
                      
                        refres();
                    }
                            
            }
           
           

        protected void buttonUpdate_Click(object sender, EventArgs e)
        {
            UserType = Session["UserType"].ToString();
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            ProductivityID = Request.QueryString["ProductivityID"].ToString();
           
                List<string> lst = new List<string>();
                lst.Add(DropDownListMachine.SelectedValue.ToString());
                lst.Add(TextBoxOperator.Text);
                lst.Add(DropDownListProduct.SelectedValue.ToString());
                lst.Add(TextBoxQuantity.Text);
                lst.Add(TextBoxScrapQty.Text);
                lst.Add(TextBoxScrapWeight.Text);
                lst.Add(TextBoxRemarks.Text);
                lst.Add(HSRPStateID);
                lst.Add(RTOLocationID);
                lst.Add(UserName);

                lst.Add(ProductivityID);
                lst.Add(DropDownListPlantName.SelectedValue.ToString());
                int i = bl.UpdateMachineOperatorProductivity(lst);

                if (i > 0)
                {
                    lblSucMess.Text = "Record Update sucessfully";
                    //string closescript1 = "<script>alert('Record save sucessfully.')</script>";
                    //Page.RegisterStartupScript("abc", closescript1);

                    refres();
                }
                else
                {

                    lblErrMess.Text = "Record Already Exist!!";
                    //string closescript1 = "<script>alert('Record save not sucessfully.')</script>";
                    //Page.RegisterStartupScript("abc", closescript1);
                     refres();
                }
            
        }


        public void refres()
        {

               
                TextBoxOperator.Text="";
                
                TextBoxQuantity.Text="";
                TextBoxScrapQty.Text="";
                TextBoxScrapWeight.Text="";
                TextBoxRemarks.Text = "";
                Machine();
                product();
                RTOLocation();
        }
       

       
    }
}