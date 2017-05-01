using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;

//using HSRP.BRHSRPService;
////using System.Data.OracleClient;

namespace HSRP.Master
{
    public partial class StockDetail : System.Web.UI.Page
    {
        
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;       
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();       
        string strCompanyName = string.Empty;
        string strSqlQuery = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.GZipEncodePage();
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    RTOLocationID = Session["UserRTOLocationID"].ToString();
                    UserType = Session["UserType"].ToString();
                    HSRPStateID = Session["UserHSRPStateID"].ToString();

                    lblErrMsg.Text = string.Empty;
                    strUserID = Session["UID"].ToString();
                    ComputerIP = Request.UserHostAddress;

                    if (!IsPostBack)
                    {
                        try
                        {
                            FillDdlEmbCenter();
                            FillErpProductCode();
                        }
                        catch (Exception err)
                        {
                            lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #region DropDown


        private void FillDdlEmbCenter()
        {
            try
            {
                //if (UserType == "0")
                //{
                //   // SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where navembid is not null   Order by EmbCenterName ";
                //    SQLString = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where navembid is not null   Order by EmbCenterName ";
                //    DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                //    ddlEmbossingCenters.DataSource = dts;
                //    ddlEmbossingCenters.DataTextField = "EmbCenterName";
                //    ddlEmbossingCenters.DataValueField = "NAVEMBID";
                //    ddlEmbossingCenters.DataBind();
                //    ddlEmbossingCenters.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Emb Center--", "0"));

                //}
                //else
                //{ HSRPStateID
                    SQLString = " select  EmbCenterName ,   Emb_Center_Id  as  NAVEMBID  from EmbossingCenters  Where State_Id=" + HSRPStateID + " and ActiveStatus!='N' Order by EmbCenterName";
                     //SQLString  = "select distinct EmbCenterName,NAVEMBID from RTOLocation Where navembid is not null  and hsrp_stateid ='"+ HSRPStateID +"' union  select distinct  'Central Warehouse',  navcentralwarehousecode   from RTOLocation Where navembid is not null   and hsrp_stateid = '"+HSRPStateID+"' Order by EmbCenterName  ";
                    DataTable dts = Utils.GetDataTable(SQLString, CnnString);
                    ddlEmbossingCenters.DataSource = dts;
                    ddlEmbossingCenters.DataTextField = "EmbCenterName";
                    ddlEmbossingCenters.DataValueField = "NAVEMBID";
                    ddlEmbossingCenters.DataBind();
                    ddlEmbossingCenters.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Emb Center--", "0"));

                //}
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void FillErpProductCode()
        {

            try
            {
                SQLString = "select ProductERPID,ProductCode from ProductSizeERP";
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
       
        #endregion

        protected void ddlErpProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEmbossingCenters.SelectedValue.ToString() == "0")
                {
                    lblErrMsg.Text = "Select Embossing Center";
                    lblErrMsg.ForeColor = Color.Red;
                }
                else
                {
                    lblErrMsg.Text = "";
                    lblErrMsg.Text = GetInventoryDataCount(ddlEmbossingCenters.SelectedValue, ddlErpProductCode.SelectedValue);
                    hiddenUserType.Value = GetInventoryDataCount(ddlEmbossingCenters.SelectedValue, ddlErpProductCode.SelectedValue);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
             //string s1 = GetInventoryDataCount("EC825","RM0007");

        }

        public string GetInventoryDataCount(string RtoLocation,  string ProductionCode)
        {
            try
            {
                if (HSRPStateID == "1")
                {

                    //BRWebReference.HSRPWebService service = new BRWebReference.HSRPWebService();
                    //service.UseDefaultCredentials = false;
                    //service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                    ////int a = Convert.ToInt32(service.UpdateWebInventory(RtoLocation, ProductionCode));
                    //int a = Convert.ToInt32(service.UpdateItemBYSerialInventory(RtoLocation, ProductionCode));
                    
                    BRWebReference.HSRPWebService service = new BRWebReference.HSRPWebService();
                    service.UseDefaultCredentials = false;
                    service.Credentials = new System.Net.NetworkCredential("Administrator@erp.com", "ewq@)!^98");
                    //int a = Convert.ToInt32(service.UpdateWebInventory(RtoLocation, ProductionCode));
                    int a = Convert.ToInt32(service.UpdateItemBYSerialInventory(RtoLocation, ProductionCode));            



                    return a.ToString();
                
                }
                if (HSRPStateID == "4")
                {
                    string dealercode = string.Empty;
                    int Dealerstock = 0;
                    HRWebReference.HSRPWebService service = new HRWebReference.HSRPWebService();
                    service.UseDefaultCredentials = false;
                    service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");                   
                    if (RtoLocation.Trim() == "EC466")
                    {
                        dealercode = "EC-DLR-AMB";
                        Dealerstock = Convert.ToInt32(service.UpdateItemBYSerialInventory(dealercode, ProductionCode));

                    }                    	
                    if (RtoLocation.Trim() == "EC470")
                    {
                        dealercode = "EC-DLR-BWN";
                        Dealerstock = Convert.ToInt32(service.UpdateItemBYSerialInventory(dealercode, ProductionCode));

                    }
                    if (RtoLocation.Trim() == "EC487")
                    {
                        dealercode = "EC-DLR-HSR";
                        
                        Dealerstock = Convert.ToInt32(service.UpdateItemBYSerialInventory(dealercode, ProductionCode));

                    }                
                    if (RtoLocation.Trim() == "EC499")
                    {
                       
                        dealercode = "EC-DLR-KAR";
                        Dealerstock = Convert.ToInt32(service.UpdateItemBYSerialInventory(dealercode, ProductionCode));
                    }
                    
                    if (RtoLocation.Trim() == "EC486")
                    {
                        dealercode = "EC-DLR-GGN";
                        Dealerstock = Convert.ToInt32(service.UpdateItemBYSerialInventory(dealercode, ProductionCode));
                    }

                    if (RtoLocation.Trim() == "EC537")
                    {
                        dealercode = "EC-DLR-SNP";
                        Dealerstock = Convert.ToInt32(service.UpdateItemBYSerialInventory(dealercode, ProductionCode));
                    }


                    int a = Convert.ToInt32(service.UpdateItemBYSerialInventory(RtoLocation, ProductionCode));
                    a = a + Dealerstock;

                   
                    return a.ToString();
                }
                if (HSRPStateID == "3")
                {
                    HpWebRefrence.HSRPWebService service = new HpWebRefrence.HSRPWebService();
                    service.UseDefaultCredentials = false;
                    service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                    int a = Convert.ToInt32(service.UpdateItemBYSerialInventory(RtoLocation, ProductionCode));
                    return a.ToString();
                }
                else
                {
                    UKWebReference.HSRPWebService service = new UKWebReference.HSRPWebService();
                    service.UseDefaultCredentials = false;
                    service.Credentials = new System.Net.NetworkCredential("erpwebservice@erp.com", "E@rpweb");
                    int a = Convert.ToInt32(service.UpdateItemBYSerialInventory(RtoLocation, ProductionCode));
                    return a.ToString();
                }


             

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlEmbossingCenters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlErpProductCode.ClearSelection();
                ddlErpProductCode.SelectedValue = "0";
                lblErrMsg.Text = "";
            }
            catch (Exception)
            {
                
                throw;
            }   

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtQuantity.Text))
                {
                    lblErrMsg.Text = "Please provide Quantity.";
                    txtQuantity.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(hiddenUserType.Value))
                {
                    lblErrMsg.Text = "ERP Quantity Not Received.";
                    ddlErpProductCode.Focus();
                    return;
                }
                if (hiddenUserType.Value == "0")
                {
                    lblErrMsg.Text = "Please contact to administrator.";
                    ddlErpProductCode.Focus();
                    return;
                }
                SQLString = "Exec [InsertPhysicalStock] '" + txtQuantity.Text + "' , '" + hiddenUserType.Value + "','" + ddlEmbossingCenters.SelectedValue.ToString() + "','" + ddlErpProductCode.SelectedValue.ToString() + "','" + strUserID + "'";
                int i = Utils.ExecNonQuery(SQLString, CnnString);
                if (i > 0)
                {
                    lblErrMsg.Text = "Stock Save Successfully";
                    txtQuantity.Text = "";
                    ddlEmbossingCenters.ClearSelection();
                    ddlErpProductCode.ClearSelection();
                }
                else
                {
                    lblErrMsg.Text = "Stock save unsuccessful";
                }
            }
            catch (Exception)
            {
                
                throw;
            }

        }
       
    }
}
 
        
