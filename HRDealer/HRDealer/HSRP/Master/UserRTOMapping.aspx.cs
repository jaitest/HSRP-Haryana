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
    public partial class UserRTOMapping : System.Web.UI.Page
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
                lblErrMess.Text = lblErrMess.Text + " " + ee.Message;
            }
        }

        #region DropDown

        private void FilldropDownListState()
        {
            SQLString = "select HSRPStateName,HSRP_StateID from HSRPState order by HSRPStateName";
            Utils.PopulateDropDownList(dropDownListState, SQLString.ToString(), CnnString, "--Select State--");
            dropDownListState.SelectedIndex = 0;
        }

        private void FilldropDownListUser()
        {
            SQLString = "select UserFirstName+Space(2)+isnull(UserLastName,'') as UserName,UserID from Users  where HSRP_StateID=" + dropDownListState.SelectedValue + " order by UserName";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListUser, SQLString.ToString(), CnnString, "--Select User--");
            dropDownListUser.SelectedIndex = dropDownListUser.Items.Count - 1;
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
            checkBoxListGroup.Items.Clear();
            if (!dropDownListUser.SelectedValue.Equals("--Select User--"))
            {
                checkBoxListGroup.Visible = true;
                FillCheckBoxListRTOLocation();
            }
            else
            {
                checkBoxListGroup.Visible = false;
            }
        }

        #endregion

        private void FillCheckBoxListRTOLocation()
        {
            if (UserType.Equals(0))
            {
                if (String.IsNullOrEmpty(dropDownListState.SelectedValue) || dropDownListState.SelectedValue.Equals("--Select Organization--"))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Select organization.";
                    return;
                }
                int.TryParse(dropDownListState.SelectedValue, out intStateID);
                int.TryParse(dropDownListUser.SelectedValue, out intUserID);
            }
            else
            {
                if (String.IsNullOrEmpty(dropDownListUser.SelectedValue) || dropDownListUser.SelectedValue.Equals("--Select User--"))
                {
                    lblErrMess.Text = String.Empty;
                    lblErrMess.Text = "Please Select User.";
                    return;
                }
                //intStateID = HSRPStateID;
                //intUserID = UserID;
                int.TryParse(dropDownListState.SelectedValue, out intStateID);
                int.TryParse(dropDownListUser.SelectedValue, out intUserID);
            }

            SQLString = "Select R.RTOLocationName,R.RTOLocationID,URM.UserID as tt From RTOLocation R left join UserRTOLocationMapping URM on  R.RTOLocationID=URM.RTOLocationID where R.HSRP_StateID=" + intStateID + " and UserID=" + intUserID + "";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            DataTable dtt = Utils.GetDataTable(SQLString, CnnString);

            SQLString = "Select RTOLocationName,RTOLocationID From RTOLocation where HSRP_StateID=" + intStateID + " ";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            string var = String.Empty;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var = dr["RTOLocationID"].ToString();
                    bool flag = false;
                    foreach (DataRow dr1 in dtt.Rows)
                    {
                        if (dr1["RTOLocationID"].ToString() == var)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        checkBoxListGroup.Items.Add(new ListItem(dr["RTOLocationName"].ToString(), dr["RTOLocationID"].ToString(), true));
                    }
                    else
                    {
                        checkBoxListGroup.Items.Add(new ListItem(dr["RTOLocationName"].ToString(), dr["RTOLocationID"].ToString(), false));
                    }
                }
            }
            foreach (ListItem item in checkBoxListGroup.Items)
            {
                if (item.Enabled == true)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Enabled = true;
                }
            }
        }

        protected void buttonSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(dropDownListUser.SelectedValue) || dropDownListUser.SelectedValue.Equals("--Select User--"))
            {
                lblErrMess.Text = String.Empty;
                lblErrMess.Text = "Please Select User.";
                return;
            }
            CheckboxListSelections(checkBoxListGroup);
        }

        private void CheckboxListSelections(System.Web.UI.WebControls.CheckBoxList Checklist)
        {

            StringBuilder Query = new StringBuilder();
            int.TryParse(dropDownListUser.SelectedValue, out UserID);
            foreach (ListItem item in Checklist.Items)
            {
                if (item.Selected.Equals(true))
                {
                    Query.Append("insert into UserRTOLocationMapping(UserID,RTOLocationID,CreatedBy) values(" + UserID + "," + item.Value + "," + HSRPStateID + ");");
                }
            }
            SQLString = "Delete from UserRTOLocationMapping Where UserID=" + UserID;
            Utils.ExecNonQuery(SQLString, CnnString);
            if (Utils.ExecNonQuery(Query.ToString(), CnnString) > 0)
            {
                lblSucMess.Text = "RTO Locations Sucessfully assigned to User:  " + dropDownListUser.SelectedItem.Text;
            }
            else
            {
                lblErrMess.Text = "RTO Locations not Sucessfully assigned to User:  " + dropDownListUser.SelectedItem.Text;
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkBoxListGroup.Items.Count; i++)
            {
                if (CheckBox1.Checked == true)
                {
                    checkBoxListGroup.Items[i].Selected = true;
                }
                else
                {
                    checkBoxListGroup.Items.Clear();
                    FillCheckBoxListRTOLocation();
                    break;
                }
            }
            
        }
    }
}