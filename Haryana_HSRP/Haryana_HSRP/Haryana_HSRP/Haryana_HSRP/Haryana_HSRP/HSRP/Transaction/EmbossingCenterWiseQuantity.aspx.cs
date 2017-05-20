using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        string UserType = string.Empty;
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
                UserType = Session["UserType"].ToString();
               

            }
            if (!Page.IsPostBack)
            {
                //textBoxUserName.Text = Session["UserName"].ToString();
                FilldropDownListClient();
                FillErpProductCode();
            
            }

 
        }

        private void FilldropDownListClient()
        {
            try
            {
                if (UserType == "0")
                {
                    SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where navembid is not null and  hsrp_stateid='" + HSRP_StateID + "'  Order by EmbCenterName ";
                    DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                    dropDownListClient.DataSource = dts;
                    dropDownListClient.DataTextField = "EmbCenterName";
                    dropDownListClient.DataValueField = "NAVEMBID";
                    dropDownListClient.DataBind();
                    dropDownListClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Emb Center--", "0"));

                }
                else
                {
                    SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HSRP_StateID + " and navembid is not null  Order by EmbCenterName ";
                    DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                    dropDownListClient.DataSource = dts;
                    dropDownListClient.DataTextField = "EmbCenterName";
                    dropDownListClient.DataValueField = "NAVEMBID";
                    dropDownListClient.DataBind();
                    dropDownListClient.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Emb Center--", "0"));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //private void FilldropDownListClient()
        //{

        //    SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where HSRP_StateID=" + HSRP_StateID + " and navembid is not null  Order by EmbCenterName";
        //    Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select Embossing Center--");

        //}

        private void FillErpProductCode()
        {

            try
            {
                SQLString = "select ProductERPID,ProductCode from ProductSizeERP order by ProductERPID";
                DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                ddlErpProductCode.DataSource = dts;
                ddlErpProductCode.DataTextField = "ProductCode";
                ddlErpProductCode.DataValueField = "ProductERPID";
                ddlErpProductCode.DataBind();
                ddlErpProductCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Product Code--", "0"));
            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void ddlErpProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dropDownListClient.SelectedValue.ToString() == "0")
                {
                    lblErrMess.Text = "Select Embossing Center";
                    lblErrMess.ForeColor = Color.Red;
                }
                else
                {
                    //string strrtolocationid = "select top 1 RtolocationID from rtolocation where Navembid='" + dropDownListClient.SelectedValue.ToString() + "' and ";

                    //DataTable dtlocationid = Utils.GetDataTable(strrtolocationid, CnnString);
                    lblSucMess.Text = "";
                    lblErrMess.Text = "";
                   // lblErrMess.Text = GetInventoryDataCount(dropDownListClient.SelectedValue, ddlErpProductCode.SelectedValue);
                   // hiddenUserType.Value = GetInventoryDataCount(dropDownListClient.SelectedValue, ddlErpProductCode.SelectedValue);
                   // ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
            }
            catch (Exception)
            {

                throw;
            }
            //string s1 = GetInventoryDataCount("EC825","RM0007");

        }

        //public string GetInventoryDataCount(string RtoLocation, string ProductionCode)
        //{
        //    try
        //    {
        //        if (HSRP_StateID == "9")
        //        {
        //            WebReference.HSRPWebService service = new WebReference.HSRPWebService();
        //            service.UseDefaultCredentials = false;
        //            service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
        //            // int a = Convert.ToInt32(service.UpdateWebInventory(RtoLocation, ProductionCode));
        //            int a = Convert.ToInt32(service.UpdateItemBYSerialInventory(RtoLocation, ProductionCode));
        //            //WebReference.HSRPWebService WebInventoryData Cust = new WebReference.WebInventoryData();
        //            return a.ToString();
        //        }
        //        else
        //        {
        //            WebReference_TG.HSRPWebService service = new WebReference_TG.HSRPWebService();
        //            service.UseDefaultCredentials = false;
        //            service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
        //            //int a = Convert.ToInt32(service.UpdateWebInventory(RtoLocation, ProductionCode));
        //            int a = Convert.ToInt32(service.UpdateItemBYSerialInventory(RtoLocation, ProductionCode));
        //            //WebReference.HSRPWebService WebInventoryData Cust = new WebReference.WebInventoryData();
        //            return a.ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


        string locationname = string.Empty;

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
            if (ddlErpProductCode.SelectedItem.Text == "--Select Product Code--")
            {
                lblErrMess.Text = "Please Select Prodect Code";
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

            if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                lblErrMess.Text = "Please provide Quantity.";
                txtQuantity.Focus();
                return;
            }
            //if (string.IsNullOrEmpty(hiddenUserType.Value))
            //{
            //    lblErrMess.Text = "ERP Quantity Not Received.";
            //    ddlErpProductCode.Focus();
            //    return;
            //}
            //if (hiddenUserType.Value == "0")
            //{
            //    lblErrMess.Text = "Please contact to administrator.";
            //    ddlErpProductCode.Focus();
            //    return;
            //}


            //string SQLStringbox = "select count(BoxNO) from EmbossingCenterLaserQuantity where  BoxNO='" + Stringboxno + "'";
            //int boxcount;
            //boxcount = Utils.getScalarCount(SQLStringbox, CnnString);
            //if (boxcount > 0)
            //{
            //    lblErrMess.Text = "Box Number Already Exists";
            //    return;
            //}

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

            sqlQuery12 = "insert into EmbossingCenterLaserQuantity(HSRP_ID,RToLocationid,EmbID,BoxNO,LaserFrom,LaserTo,Quantity,productcode,ERPQuantity,CreateBy) values('" + HSRP_StateID + "','" + RTOLocationID + "', '" + dropDownListClient.SelectedValue + "','" + Stringboxno + "', '" + Stringlaserfrom + "','" + Stringlaserto + "','" + Stringquantity + "','" + ddlErpProductCode.SelectedValue.ToString().Trim() + "','" + hiddenUserType.Value + "','"+USERID+"')";
            int count = Utils.ExecNonQuery(sqlQuery12, CnnString);
            if (count > 0)
            {
                lblSucMess.Text = "Records has been Saved Successfully";
                clear();
            }
            else
            {
                lblErrMess.Text = "Record Not Saved";
            }

        }


        public void clear()
        {
            txtboxno.Text = "";
            txtlaserfrom.Text = "";
            txtlaserto.Text = "";
            txtQuantity.Text = "";
           // dropDownListClient.ClearSelection();
           // ddlErpProductCode.ClearSelection();          
            
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlErpProductCode.ClearSelection();
                ddlErpProductCode.SelectedValue = "0";
                lblErrMess.Text = "";
            }
            catch (Exception)
            {

                throw;
            }   
        }

       
    }
}