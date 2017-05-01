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
    public partial class menutreeview : System.Web.UI.Page
    {
        StringBuilder sbQuery = new StringBuilder();
        string CnnString = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            buildTree(int.Parse(Request.QueryString["UserID"].ToString()));
        }
       
        private void buildTree(int userid)
        {
            DataSet ds = new DataSet();
            sbQuery.Remove(0, sbQuery.Length);
            sbQuery.Append("SELECT a.MenuName, b.ParentMenuID,b.MenuID, b.MenuRelationID,isnull(cast((select case ISNUMERIC(v.MenuRelationID) when 1 ");
            sbQuery.Append("then 'Y'else 'N' end from UserMenuRelation v where b.MenuRelationID=v.MenuRelationID and v.userid='" + userid + "') as varchar(44)),'N') as UnCheckItem");
            sbQuery.Append(" from Menu a,MenuRelation  b where a.MenuID = b.MenuID " );



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
    
    }
}