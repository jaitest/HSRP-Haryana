using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using DataProvider;

namespace HSRP.Master
{
    public partial class RTOHRAssignment : System.Web.UI.Page
    {
        //int UserID;
        //int intStateID;
        //int HSRPStateID;
        //int intUserID;
        //int UserType;
        string CnnString = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        String SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Utils.GZipEncodePage();
                lblErrMess.Text = string.Empty;
                lblSucMess.Text = string.Empty;

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
                    lblErrMess.Text = string.Empty;
                    strUserID = Session["UID"].ToString();
                    ComputerIP = Request.UserHostAddress;
                  //  int.TryParse(Session["UserType"].ToString(), out UserType);
                  //  int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                  //  int.TryParse(Session["UID"].ToString(), out UserID);
                   ///int.TryParse(Session["UID"].ToString(), out RTOLocationID);
                    lblErrMess.Text = string.Empty;
                    lblSucMess.Text = string.Empty;
                   // CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                }


                if (!Page.IsPostBack)
                {

                    if (UserType=="0")
                    {
                        dropDownListState.Enabled = true;
                        FilldropDownListState();
                        //  FilldropDownListClient();
                    }
                    else if (UserType.Equals(1))
                    {
                        FilldropDownListState();
                        dropDownListState.SelectedValue = HSRPStateID.ToString();
                        dropDownListState.Enabled = false;
                        FilldropDownListUser();
                        dropDownListUser.Enabled = true;
                        dropDownListUser.SelectedIndex = 0;
                    }
                    else
                    {
                        FilldropDownListState();
                        dropDownListState.SelectedValue = HSRPStateID.ToString();
                        dropDownListState.Enabled = false;
                        FilldropDownListUser();
                        dropDownListUser.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ee)
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Contact to admin.";
                lblErrMess.Text = "Error on Page Load" + ee.Message.ToString();
                //lblErrMess.Text = lblErrMess.Text + " " + ee.Message;
            }
        }

        #region DropDown

        private void FilldropDownListState()
        {
            try
            {
                SQLString = "select HSRPStateName,HSRP_StateID from HSRPState where HSRP_StateID=4 order by HSRPStateName";
                Utils.PopulateDropDownList(dropDownListState, SQLString.ToString(), CnnString, "--Select State--");
                dropDownListState.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        private void FilldropDownListUser()
        {
            try
            {
                //SQLString = "select UserFirstName+Space(2)+isnull(UserLastName,'') as UserName,UserID from Users  where HSRP_StateID=" + dropDownListState.SelectedValue + " order by UserName";
                SQLString = "Select RTOLocationName,RTOLocationID from RTOLocation where HSRP_StateID='" + dropDownListState.SelectedValue + "' order by RTOLocationName asc";
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                Utils.PopulateDropDownList(dropDownListUser, SQLString.ToString(), CnnString, "--Select User--");
                //dropDownListUser.SelectedIndex = dropDownListUser.Items.Count - 1;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListState.SelectedItem.Text != "--Select State--")
            {
                dropDownListUser.Enabled = true;
                FilldropDownListUser();
                UpdatePanelUser.Update();
            }
            else
            {
                lblErrMess.Text = string.Empty;
                lblErrMess.Text = "Please select Organization.";
            }
        }

        protected void dropDownListUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrMess.Text = String.Empty;
            lblSucMess.Text = String.Empty;
          //  checkBoxListGroup.Items.Clear();
            if (!dropDownListUser.SelectedValue.Equals("--Select User--"))
            {
               // checkBoxListGroup.Visible = true;
               // FillCheckBoxListRTOLocation();
            }
            else
            {
               // checkBoxListGroup.Visible = false;
            }
        }

        #endregion

        

       
        protected void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strdealer = "select RTOLocationCode from rtolocation where hsrp_stateID=4 and RtolocationID='" + dropDownListUser.SelectedValue + "'";
                DataTable dealername = Utils.GetDataTable(strdealer, CnnString);

               
                //if ((!Regex.Match(txtassignday.Text.ToString().Trim(), @"^[0-9]{1,5}$", RegexOptions.None).Success))
                //{
                //    ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "alert('Please Enter valid AssigndayValue!');", true);
                //    txtassignday.Focus();
                //    return;
                //}
          
                StringBuilder Query = new StringBuilder();
                string sql = "insert into hr_assignment_days_rto(rtolocationid,days_Assignment,stateID,rtocode) values('" + dropDownListUser.SelectedValue.ToString().Trim() + "','" + txtassignday.Text.ToString().Trim() + "','" + dropDownListState.SelectedValue.ToString().Trim() + "','" + dealername.Rows[0]["RTOLocationCode"].ToString().Trim() + "')"; 
                Utils.ExecNonQuery(sql, CnnString);
                lblSucMess.Text = "Record Save Successfuly";               
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
        }    

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
                       
        }
    }
}