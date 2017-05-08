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

namespace HSRP.Master
{
    public partial class AssignMenuToUser : System.Web.UI.Page
    {
        int HSRPStateID;
        int RTOLocationID;
        int UserID;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        int UserType;
        StringBuilder sbQuery = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {
                lblErrMess.Text = String.Empty;
                UserType = Convert.ToInt32(Session["UserType"].ToString());
                int.TryParse(Session["UserHSRPStateID"].ToString(), out HSRPStateID);
                int.TryParse(Session["UserRTOLocationID"].ToString(), out RTOLocationID);
                int.TryParse(Session["UID"].ToString(), out UserID);
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }

            if (!Page.IsPostBack)
            {

                if (string.IsNullOrEmpty(UserType.ToString()))
                {
                    Response.Write("<script language='javascript'> {window.close();} </script>");
                }
                else
                {

                    FilldropDownListRTOLocation();
                    FilldropDownListUser();
                    if (UserType.Equals(0))
                    {
                        FilldropDownListHSRPState();
                        dropDownListHSRPState.Enabled = true;
                    }
                    else if (UserType.Equals(1))
                    {
                        FilldropDownListHSRPState();
                        dropDownListHSRPState.SelectedValue = HSRPStateID.ToString();
                        dropDownListHSRPState.Enabled = false;
                        FilldropDownListRTOLocation();
                        dropDownListRTOLocation.Enabled = true;
                    }
                    else if (UserType.Equals(2))
                    {
                        FilldropDownListUser();
                        FilldropDownListHSRPState();
                        FilldropDownListRTOLocation();
                        FilldropDownListUser();
                        dropDownListHSRPState.SelectedValue = HSRPStateID.ToString();
                        dropDownListHSRPState.Enabled = false;
                        FilldropDownListRTOLocation();
                        dropDownListRTOLocation.SelectedValue = RTOLocationID.ToString();
                        dropDownListRTOLocation.Enabled = false;
                        dropDownListUser.Enabled = true;
                    }
                }
            }
        }

        # region DropDown
        private void FilldropDownListHSRPState()
        {
            if (UserType == 0)
            {
                SQLString = "select HSRP_StateID,HSRPStateName from HSRPState order by HSRPStateName";
            }
            else
            {
                SQLString = "select HSRP_StateID,HSRPStateName from HSRPState where HSRP_StateID=" + HSRPStateID + "order by HSRPStateName";
            }
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListHSRPState, SQLString, CnnString, "--Select State--");
            dropDownListHSRPState.SelectedIndex = 0;
        }

        private void FilldropDownListRTOLocation()
        {
            int intHSRPStateID1;
            if (UserType == 0)
            {
                int.TryParse(dropDownListHSRPState.SelectedValue, out intHSRPStateID1);
            }
            else
            {
                intHSRPStateID1 = HSRPStateID;
            }
            SQLString = "select RTOLocationName,RTOLocationID from RTOLocation Where HSRP_StateID=" + intHSRPStateID1 + " order by RTOLocationName";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListRTOLocation, SQLString, CnnString, "--Select RTO Location--");
            dropDownListRTOLocation.SelectedIndex = 0;
        }

        private void FilldropDownListUser()
        {
            int intHSRPStateID;
            int intRTOLocationID;
            if (UserType == 0)
            {
                int.TryParse(dropDownListHSRPState.SelectedValue, out intHSRPStateID);
                int.TryParse(dropDownListRTOLocation.SelectedValue, out  intRTOLocationID);
            }
            else if (UserType == 1)
            {
                intHSRPStateID = HSRPStateID;
                int.TryParse(dropDownListRTOLocation.SelectedValue, out  intRTOLocationID);
            }
            else
            {
                intHSRPStateID = HSRPStateID;
                intRTOLocationID = RTOLocationID;
            }

            SQLString = "select UserID,ISNULL(UserFirstName,'')+Space(2)+ISNULL(UserLastName,'') as Names from Users where HSRP_StateID='" + intHSRPStateID + "' and RTOLocationID=" + intRTOLocationID + "  order by UserFirstName";
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Utils.PopulateDropDownList(dropDownListUser, SQLString, CnnString, "--Select User--");
            dropDownListUser.SelectedIndex = 0;
        }

        protected void dropDownListOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListHSRPState.SelectedItem.Text != "--Select State--")
            {
                dropDownListRTOLocation.Enabled = true;
                FilldropDownListRTOLocation();
                UpdatePanelClient.Update();
            }
            else
            {
                lblErrMess.Text = string.Empty;
                lblErrMess.Text = "Please Select State";
                dropDownListRTOLocation.Enabled = false;
                dropDownListUser.Enabled = false;
            }
        }

        protected void dropDownListClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListRTOLocation.SelectedItem.Text != "--Select RTO Location--")
            {
                dropDownListUser.Enabled = true;
                FilldropDownListUser();
                UpdatePanelUser.Update();
            }
            else
            {
                dropDownListUser.Enabled = false;
                dropDownListUser.Enabled = false;
                lblErrMess.Text = string.Empty;
                lblErrMess.Text = "Please Select User";
            }
        }

        protected void dropDownListUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropDownListUser.SelectedItem.Text != "--Select User--")
            {
                //if (UserType == 3)
                //{
                //    buildTree(UserID);
                //}
                //else
                //{
                //    buildTree(Convert.ToInt32(dropDownListUser.SelectedValue));
                //}


                TreeView1.Nodes.Clear();
                buildTree(Convert.ToInt32(dropDownListUser.SelectedValue));
                UpdatePanelTree.Update();
            }
            else
            {
                lblErrMess.Text = string.Empty;
                lblErrMess.Text = "Please Select User";
            }
        }

        #endregion

        #region Tree events and functions
        private void buildTree(int userid)
        {
            DataSet ds = new DataSet();
            sbQuery.Remove(0, sbQuery.Length);
            sbQuery.Append("SELECT a.MenuName, b.ParentMenuID,b.MenuID, b.MenuRelationID,isnull(cast((select case ISNUMERIC(v.MenuRelationID) when 1 ");
            sbQuery.Append("then 'Y'else 'N' end from UserMenuRelation v where b.MenuRelationID=v.MenuRelationID and v.userid='" + userid + "') as varchar(44)),'N') as UnCheckItem");
            sbQuery.Append(" from Menu a,MenuRelation  b where a.MenuID = b.MenuID ");

            

            SqlDataAdapter adapter = new SqlDataAdapter(sbQuery.ToString(), CnnString);
            adapter.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
            ds.Relations.Add("NodeRelation", ds.Tables[0].Columns["MenuId"], ds.Tables[0].Columns["ParentMenuId"]);
            this.TreeView1.AutoScroll = false;
            
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentMenuId"))
                    {
                        ComponentArt.Web.UI.TreeViewNode newNode;
                        newNode = CreateNode(dbRow, true);
                        TreeView1.Nodes.Add(newNode);
                        PopulateSubTree(dbRow, newNode);

                    }
                }
            }
        }

        private void PopulateSubTree(DataRow dbRow, ComponentArt.Web.UI.TreeViewNode node)
        {
            ComponentArt.Web.UI.TreeViewNode childNode;
            foreach (DataRow childRow in dbRow.GetChildRows("NodeRelation"))
            {
                childNode = new ComponentArt.Web.UI.TreeViewNode();
                childNode = this.CreateNode(childRow, true);
                node.Nodes.Add(childNode);
                this.PopulateSubTree(childRow, childNode);
            }
        }

        private TreeViewNode CreateNode(DataRow childRow, bool expanded)
        {
            TreeViewNode node = new TreeViewNode();
            node.Text = childRow["menuname"].ToString();

            node.Value = childRow["menurelationid"].ToString();

            node.ShowCheckBox = true;
            string chk = childRow["UnCheckItem"].ToString();
            if (chk == "Y")
            {
                node.Checked = true;
            }
            node.Expanded = expanded;
            node.ID = childRow["menuid"].ToString() + "~" + childRow["ParentMenuId"].ToString();
            return node;
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode[] tv;
                tv = TreeView1.CheckedNodes;
                string StringUserID = String.Empty;
                StringUserID = dropDownListUser.SelectedValue;
                string strnodeid;
                string strmenurelationid;
                sbQuery.Remove(0, sbQuery.Length);
                SQLString = "delete from usermenurelation where UserID='" + StringUserID + "'";
                Utils.ExecNonQuery(SQLString, CnnString);
                int chkcnt = 0;
                foreach (TreeViewNode node in tv)
                {
                    chkcnt = 1;
                    strnodeid = node.ID;
                    strmenurelationid = node.Value;
                    sbQuery.Append(" insert into usermenurelation (UserID, MenuRelationID) values ('" + StringUserID + "','" + strmenurelationid + "');");

                }
                if (chkcnt == 1)
                {
                    Utils.ExecNonQuery(sbQuery.ToString(), CnnString);
                    lblSucMess.Text = " Menu assigned Successfully";
                    dropDownListUser.Enabled = true;
                    TreeView1.Visible = true;
                }
                else
                {
                    lblErrMess.Text = "Select a Vehicle to Assign to the User.";
                }
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
            }
        }
    }
}