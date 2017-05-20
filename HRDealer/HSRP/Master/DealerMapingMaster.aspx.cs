using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

namespace HSRP.Master
{
    public partial class DealerMapingMaster : System.Web.UI.Page
    {
        int UserID;
        int intStateID;
        int HSRPStateID;
        int intUserID;
        int UserType;
        string CnnString = string.Empty;
        String SQLString = string.Empty;

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
                    int.TryParse(Session["UserType"].ToString(), out UserType);
                    int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                    int.TryParse(Session["UID"].ToString(), out UserID);
                    lblErrMess.Text = string.Empty;
                    lblSucMess.Text = string.Empty;
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                }


                if (!Page.IsPostBack)
                {

                    if (UserType.Equals(0))
                    {
                        dropDownLisuusername.Enabled = true;
                        FilldropDownLisuusername();
                        //  FilldropDownListClient();
                    }
                    else if (UserType.Equals(1))
                    {
                        FilldropDownLisuusername();
                        dropDownLisuusername.SelectedValue = HSRPStateID.ToString();
                        dropDownLisuusername.Enabled = false;
                      //  FilldropDownListUser();
                        dropDownListUser.Enabled = true;
                        dropDownListUser.SelectedIndex = 0;
                    }
                    else
                    {
                        FilldropDownLisuusername();
                        dropDownLisuusername.SelectedValue = HSRPStateID.ToString();
                        dropDownLisuusername.Enabled = false;
                       // FilldropDownListUser();
                        dropDownListUser.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ee)
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Contact to admin.";
                lblErrMess.Text = lblErrMess.Text + " " + ee.Message;
            }
        }

        #region DropDown

        private void FilldropDownLisuusername()
        {
            SQLString = "select  userid ,  UserfirstName +' '+UserLastName  as UserName  from users  where hsrp_stateid = 4 and  activestatus = 'Y'  order by UserfirstName";
            Utils.PopulateDropDownList(dropDownLisuusername, SQLString.ToString(), CnnString, "--Select User Name--");
            dropDownLisuusername.SelectedIndex = 0;
        }

        private void FilldropDownListDealer()
        {
            SQLString = "select UserFirstName+Space(2)+isnull(UserLastName,'') as UserName,UserID from Users  where HSRP_StateID=" + dropDownLisuusername.SelectedValue + " order by UserName";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListUser, SQLString.ToString(), CnnString, "--Select Dealer Name--");
            dropDownListUser.SelectedIndex = dropDownListUser.Items.Count - 1;
        }

       

        #endregion

       
        protected void buttonSave_Click(object sender, EventArgs e)
        {
            if (dropDownListUser.SelectedValue.Equals("--Select User Name--"))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select User.";
                return;
            }
            
        }

        protected void dropDownLisuusername_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      

       
    }
}