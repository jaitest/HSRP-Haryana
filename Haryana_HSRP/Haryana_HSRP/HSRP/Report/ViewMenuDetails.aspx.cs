using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ComponentArt.Web.UI;

namespace HSRP.Report
{
    public partial class ViewMenuDetails : System.Web.UI.Page
    {
     
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        int UserType;
        string HSRP_StateID = string.Empty;
        StringBuilder sbQuery = new StringBuilder();
        DataTable dt= new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                HSRP_StateID = Session["UserHSRPStateID"].ToString();
                UserType = Convert.ToInt32(Session["UserType"]);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();


                if (!IsPostBack)
                {

                    try
                    {
                        if (UserType.Equals(0))
                        {

                            DropDownListStateName.Visible = true;
                            DropDownListMenuName.Visible = true;
                            FillDropDownListMenuName();
                            FilldropDownListOrganization();
                        }
                        else
                        {
                            DropDownListStateName.Visible = true;
                            DropDownListMenuName.Visible = true;
                            FillDropDownListMenuName();
                            FilldropDownListOrganization();


                        }


                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }


            }
        }

        private void FilldropDownListOrganization()
        {
            if (UserType.Equals(0))
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where ActiveStatus='Y' Order by HSRPStateName";
                Utils.PopulateDropDownList(DropDownListStateName, SQLString.ToString(), CnnString, "--Select State--");
            }
            else
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState  where HSRP_StateID=" + HSRP_StateID + " and ActiveStatus='Y' Order by HSRPStateName";
                DataSet dts = Utils.getDataSet(SQLString, CnnString);
                DropDownListStateName.DataSource = dts;
                DropDownListStateName.DataBind();
            }
        }

        private void FillDropDownListMenuName()
        {
               SQLString = "select menuid,menuname from menu where menunavigateurl is null order by menuname";
                Utils.PopulateDropDownList(DropDownListMenuName, SQLString.ToString(), CnnString, "--Select Menu Name--");
            
        }
        
        
        
        protected void btngo_Click(object sender, EventArgs e)
        {
            if (DropDownListStateName.SelectedItem.Text != "--Select State--" && DropDownListMenuName.SelectedItem.Text != "--Select Menu Name--")
            {
                BuildGrid();

            }

            else
            {
               
                Grid1.Items.Clear();

                return;
            }
        }


        public void BuildGrid()
        {
            try
            {




                SQLString = " select distinct users.userid,users.Userloginname,rtolocation.rtolocationname,users.activestatus from Hsrpstate inner join rtolocation"
                          +"  on hsrpstate.hsrp_stateid=rtolocation.hsrp_stateid"
                           +" inner join users on users.rtolocationid=rtolocation.rtolocationid"
                           +" inner join usermenurelation on users.userid=usermenurelation.userid"
                            +" inner join menurelation on usermenurelation.menurelationid=menurelation.menurelationid "
                              + "  inner join menu on menurelation.menuid=menu.menuid where hsrpstate.hsrp_stateid='" + DropDownListStateName.SelectedValue.ToString() + "' and menurelation.menuid='" + DropDownListMenuName.SelectedValue.ToString() + "' and users.activestatus='y' order by rtolocation.rtolocationname";

 
                dt = Utils.GetDataTable(SQLString, CnnString);

                Grid1.DataSource = dt;

                Grid1.DataBind();




            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void DropDownListStateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDropDownListMenuName();
        }

        protected void DropDownListMenuName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      
        
    }
}