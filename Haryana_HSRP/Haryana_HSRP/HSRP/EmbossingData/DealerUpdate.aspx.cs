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


namespace HSRP.EmbossingData
{
    public partial class DealerUpdate : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int intHSRPStateID; 
        DataProvider.BAL bl = new DataProvider.BAL();
        string State_ID = string.Empty;
         string RTO_ID = string.Empty;
         string HSRPStateIDEdit = string.Empty;
         string RTOLocationIDEdit = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
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
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;
                btnupdates.Visible = false;
                if (!IsPostBack)
                {
                     
                    try
                    { 
                        if (UserType == "0")
                        {
                            labelOrganization.Visible = true;
                            DropDownListStateName.Visible = true;
                            DropDownListStateName.Enabled = true;
                            labelClient.Visible = true;
                            dropDownListClient.Visible = true;
                            btnupdates.Visible = false;
                            FilldropDownListOrganization();
                            FilldropDownListClient();
                          
                        }
                        else
                        { 
                          
                            labelOrganization.Enabled = false;
                            DropDownListStateName.Enabled = false;
                            labelClient.Enabled = false;
                            btnupdates.Visible = false;
                            FilldropDownListClient();
                            FilldropDownListOrganization();
                          
                        }
                    }
                    catch (Exception err)
                    {
                      // lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        #region DropDown

        private void FilldropDownListOrganization()
        {
            if (UserType == "0")
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName"; 
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");            
                 
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRPStateID + " ";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");            
            }
        } 
        private void FilldropDownListClient()
        {
            if (UserType == "0")
            {

                SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + HSRPStateID + " and   ActiveStatus!='N'   Order by RTOLocationName";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Location Name--");                 
              
            }
            else
            {
                          
                SQLString = " SELECT  distinct (RTOLocationName) as RTOLocationName,  RTOLocationID, * FROM  RTOLocation where  hsrp_stateid =" + HSRPStateID + " and  RTOLocationID=" + RTOLocationID + " and ActiveStatus!='N'   Order by RTOLocationName ";
                Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Location Name--");                 
  
             
            }
        }

      
        #endregion

      
        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" +DropDownListStateName.SelectedValue.ToString() + " and ActiveStatus!='N'   Order by RTOLocationName";
            Utils.PopulateDropDownList(dropDownListClient, SQLString.ToString(), CnnString, "--Select RTO Location Name--");  
    
            dropDownListClient.Visible = true;
            labelClient.Visible = true;
             }    
     


     

        StringBuilder sb = new StringBuilder();
        CheckBox chk;

        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                }
                btnupdates.Visible = true;
            }
            else if (chk1.Checked == false)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                }
                btnupdates.Visible = true;
            }

        }

        private void ShowGrid()
        {


            SQLString = "select DealerId,dealername ,City, State  from dealermaster where hsrp_stateid= " + DropDownListStateName.SelectedValue.ToString() + " and rtolocationid = " + dropDownListClient.SelectedValue.ToString() + "   and  isnull(isactive,'')=''";
             DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count > 0)
                {                                      
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    lblSubmit.Text = "";
                    GridView1.Visible = true;
                    btnupdates.Visible = true;

                }
                else
                {
                    lblSubmit.Text = "";
                    lblSubmit.Text = "Dealer Not Availabe.";
                    GridView1.Visible = false;
                    btnupdates.Visible = false;
                    lblactive.Text = "";
                    return;
                   
                }

            }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            if (DropDownListStateName.SelectedItem.ToString() == "--Select State--")
            {
                lblSubmit.Text = "";
                lblactive.Text = "";
                lblSubmit.Text = "Please Select State Name.";
                GridView1.Visible = false;
                btnupdates.Visible = false;
                return;
            }
            if (dropDownListClient.SelectedItem.ToString() == "--Select RTO Location Name--")
            {
                lblSubmit.Text = "";
                lblactive.Text = "";
                lblSubmit.Text = " Please Select RTO Location Name.";
                GridView1.Visible = false;
                btnupdates.Visible = false;
                return;
            }
            ShowGrid();

        }

        protected void btnupdates_Click(object sender, EventArgs e)
        {
            try
            {
                string strdealerId = string.Empty;

                int iChkCount = 0;
                StringBuilder sbx = new StringBuilder();
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;

                    if (chk.Checked == true)
                    {
                        iChkCount = iChkCount + 1;

                        string strRecordId = GridView1.DataKeys[i]["dealerid"].ToString();
                        if (strdealerId == "")
                        {
                            strdealerId = strRecordId;
                        }
                        else
                        {
                            strdealerId = strdealerId + "," + strRecordId;
                        }

                        sbx.Append("update dealermaster set IsActive=1 where  dealerid ='" + strRecordId + "';");
                       
                    }

                }
                if (iChkCount > 0)
                {
                    int i = Utils.ExecNonQuery(sbx.ToString(), CnnString);
                    if (i > 0)
                    {
                        lblactive.Text = "Dealer Status Active ";
                        lblSubmit.Text = "";
                        SQLString = "select DealerId,dealername ,City, State  from dealermaster where hsrp_stateid= " + DropDownListStateName.SelectedValue.ToString() + " and rtolocationid = " + dropDownListClient.SelectedValue.ToString() + "   and  isnull(isactive,'')=''";
                        DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                        if (dt.Rows.Count > 0)
                        {
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                            GridView1.Visible = true;
                            btnupdates.Visible = true;

                        }
                        else
                        {
                            GridView1.Visible = false;
                            btnupdates.Visible = false;
                            lblactive.Text = "";
                            return;

                        }
                       
                       
                    }
                    else
                    {
                        lblactive.Text = "";
                        ShowGrid();
                    }


                }
                else
                {
                    lblactive.Text = "";
                    ShowGrid();
                }

            }
            catch (Exception ex)
            {
                lblSubmit.Text = ex.Message;
            }


        }




     
      

       

    }
}