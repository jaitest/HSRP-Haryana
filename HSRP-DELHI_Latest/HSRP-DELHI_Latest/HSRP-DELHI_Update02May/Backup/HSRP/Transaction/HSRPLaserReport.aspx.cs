using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DataProvider;
using System.Drawing;
using System.Windows.Forms;

namespace HSRP.Transaction
{
    public partial class HSRPLaserReport : System.Web.UI.Page
    {

        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string SqlString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        int UserType;
        string HSRP_StateID=string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID;
        int cunt;
     
      

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                UserType = Convert.ToInt32(Session["UserType"]);

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                strUserID = Session["UID"].ToString();
                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                ComputerIP = Request.UserHostAddress;
                if (!IsPostBack)
                {
                    try
                    {
                        if (UserType.Equals(0))
                        {
                            FilldropDownListPrefix();
                        }
                        else
                        {
                            FilldropDownListPrefix();
                        }
                       
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
             }           
        }

        private void FilldropDownListPrefix()
        {
            SQLString = "select distinct Prefix from PrefixLaserNo";
            Utils.PopulateDropDownList(DropDownListPrefix, SQLString.ToString(), CnnString, "--Select Prefix--");          
        }

        private void FilldropDownListOrganization()
        {
           
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
          
        }

        private void FilldropDownListClient()
        {
           
                int.TryParse(DropDownListStateName.SelectedValue, out intHSRPStateID);
                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID + " and ActiveStatus!='N' Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Location--");
                                   
                //SQLString = "SELECT  distinct (a.RTOLocationName) as RTOLocationName, a.RTOLocationID FROM UserRTOLocationMapping INNER JOIN RTOLocation a ON UserRTOLocationMapping.RTOLocationID = a.RTOLocationID where  a.LocationType!='District' and UserRTOLocationMapping.UserID='" + strUserID + "' ";
                //DataSet dss = Utils.getDataSet(SQLString, CnnString);
                //dropDownListClient.DataSource = dss;
                //dropDownListClient.DataBind();
                      
        }

        private void FilldropDownListPlateSize()
        {
            CnnString=System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
          
                int.TryParse(DropDownListPlateSize.SelectedValue, out intHSRPStateID);
                SQLString = "select ProductID,ProductCode from Product where HSRP_StateID=" + HSRP_StateID  +" and ActiveStatus!='N' Order by ProductCode ";
               
                Utils.PopulateDropDownList(DropDownListPlateSize, SQLString.ToString(), CnnString, "--Select Plate Size--");
           
                //SQLString = "select ProductID,ProductCode from Product where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus!='N' Order by ProductCode ";
                //DataSet dss = Utils.getDataSet(SQLString, CnnString);
                //DropDownListPlateSize.DataSource = dss;
                //DropDownListPlateSize.DataBind();
           
 
        }
    

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListClient();
           
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilldropDownListPlateSize();
        }
        DataTable dt = new DataTable();

        private void Display()
        {
            if (DropDownListPrefix.SelectedValue.ToString() == "--Select Prefix--")
            {
                lblerror.Visible = true;
                lblerror.Text = "Select Prefix";
                return;
            }

            else
            {
                SQLString = "select count(*) from rtoinventory where lasernowithoutprefix >=" + txtfrom.Text.ToString() + " and lasernowithoutprefix <=" + txtto.Text.ToString() + " and prefix ='" + DropDownListPrefix.SelectedValue.ToString() + "'";
                int count = Utils.getScalarCount(SQLString, CnnString);
                cunt = count;
                SqlString = "select count(*) from rtoinventory where lasernowithoutprefix >=" + txtfrom.Text.ToString() + "and lasernowithoutprefix <=" + txtto.Text.ToString() + " and prefix ='" + DropDownListPrefix.SelectedValue.ToString() + "'and inventorystatus not in ('New Order')";
                int count1 = Utils.getScalarCount(SqlString, CnnString);


                if (Int64.Parse(txtfrom.Text) > Int64.Parse(txtto.Text))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "The value in FROM  can not be greater than To ";
                    tblecount.Visible = false;
                    tbleupdate.Visible = false;
                    tblegrid.Visible = false;
                    tbledropstate.Visible = false;
                    tblesubmit.Visible = false;
                    return;
                }

                if (cunt < 50)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Laser Count must be greater than 50";
                    tblecount.Visible = false;
                    tbleupdate.Visible = false;
                    tblegrid.Visible = false;
                    tbledropstate.Visible = false;
                    tblesubmit.Visible = false;
                    return;
                }


                lblusdplate.Text = count1.ToString();
                if (int.Parse(lblusdplate.Text) > 0)
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Used Plate Can't Be Greater Than 0";
                    tblecount.Visible = false;
                    tbleupdate.Visible = false;
                    tblegrid.Visible = false;
                    tbledropstate.Visible = false;
                    tblesubmit.Visible = false;
                    return;
                }

                lblerror.Visible = false;
                tblecount.Visible = true;
                tbleupdate.Visible = true;
                lblpcount.Text = count.ToString();

                SQLString = "select top 1 ProductCode,ProductColor from Product inner join rtoinventory ON Product.ProductID=rtoinventory.ProductID where lasernowithoutprefix >=" + txtfrom.Text.ToString() + " and lasernowithoutprefix <=" + txtto.Text.ToString() + " and prefix ='" + DropDownListPrefix.SelectedValue.ToString() + "' Order by ProductCode";
                dt = Utils.GetDataTable(SQLString, CnnString);
                lblps.Text = dt.Rows[0]["ProductCode"].ToString();
                lblc.Text = dt.Rows[0]["ProductColor"].ToString();
                tblegrid.Visible = true;
                buildgrid();

            }           
        }
        protected void btngo_Click(object sender, EventArgs e)
        {

            Display();
            
        }           
                    
        public void buildgrid()
        {
            SQLString = "SELECT hsrpstate.HSRPStateName,RTOLocation.RtolocationName, count (rtoinventory.HSRP_StateID)as 'Laser No Count'FROM  hsrpstate INNER JOIN rtoinventory ON hsrpstate.HSRP_StateID=rtoinventory.HSRP_StateID INNER JOIN RTOLocation  ON RTOLocation.RTOLocationID=rtoinventory.RTOLocationID and RTOLocation.HSRP_StateID=hsrpstate.HSRP_StateID where lasernowithoutprefix >=" + txtfrom.Text.ToString() + " and lasernowithoutprefix <=" + txtto.Text.ToString() + " and prefix ='" + DropDownListPrefix.SelectedValue.ToString() + "'  group by hsrpstate.HSRPStateName,RTOLocation.RtolocationName";
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {

            //tbleupdate.Visible = false;

            btnupdate.Visible = true;
            tbledropstate.Visible = true;
            tblesubmit.Visible = true;
            FilldropDownListOrganization();
        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (DropDownListStateName.SelectedValue.ToString() == "--Select State--" || dropDownListClient.SelectedValue.ToString() == "--Select Location--" || DropDownListPlateSize.SelectedValue.ToString() == "--Select Plate Size--")
            {
                lblerror1.Visible = true;
                lblerror1.Text = "Please Select State Name OR Location OR Plate SIze";
            }
                
            else
            {

            SQLString = ("update rtoinventory set productid='" + DropDownListPlateSize.SelectedValue.ToString() + "',hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "',rtolocationid ='" + dropDownListClient.SelectedValue.ToString() + "' where lasernowithoutprefix >=" + txtfrom.Text.ToString() + " and lasernowithoutprefix <=" + txtto.Text.ToString() + " and prefix ='" + DropDownListPrefix.SelectedValue.ToString() + "'");
            if (SQLString != "")
            {
                using (SqlConnection connection = new SqlConnection(CnnString))
                {
                    SqlCommand command = new SqlCommand(SQLString, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                    lblsuccess.Visible = true;
                    lblsuccess.Text = "Record Update successfully";
                    Display();
         
                }
            }
            
            }
            
           
        }

        protected void DropDownListPlateSize_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtstate_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtlocation_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtplatecount_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtusedplate_TextChanged(object sender, EventArgs e)
        {

        }

        //protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if(DropDownListPrefix.SelectedValue.ToString()== "--Select Prefix--")
        //    MessageBox.Show("Please Select Prefix.");
        //    DropDownListPrefix.Focus();
        //    return;
        //}

        protected void txtfrom_TextChanged(object sender, EventArgs e)
        {
            //if ((e. >= 48 && e.KeyChar <= 57))
            //{
            //    e.Handled = false;
            //}
            //else
            //{
            //    lblerror.Text = "You Can Only Enter A Number!";
            //    e.Handled = true;
            //}
        }

        protected void txtto_TextChanged(object sender, EventArgs e)
        {
            // if ((e. >= 48 && e.KeyChar <= 57))
            //{
            //    e.Handled = false;
            //}
            //else
            //{
            //    lblerror.Text = "You Can Only Enter A Number!";
            //    e.Handled = true;
            //}

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       

        //protected void txtfrom_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if ((e.KeyChar >= 48 && e.KeyChar <= 57) )
        //    {
        //        e.Handled = false;
        //    }
        //    else
        //    {
        //        lblerror.Text = "You Can Only Enter A Number!";
        //        e.Handled = true;
        //    }

        //    //if (Utils.CheckInt(e.KeyChar, 2) == true)
        //    //{
        //    //    e.Handled = false;

        //    //}
        //    //else
        //    //{
        //    //    e.Handled = true;
        //    //}
        //}

        //protected void txtto_KeyPress(object sender, KeyPressEventArgs e)
        //{


        //    if ((e.KeyChar >= 48 && e.KeyChar <= 57))
        //    {
        //        e.Handled = false;
        //    }
        //    else
        //    {
        //        lblerror.Text = "You Can Only Enter A Number!";
        //        e.Handled = true;
        //    }
        //}

       

      

       
    }
}