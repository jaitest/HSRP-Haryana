using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;
using System.Configuration;

namespace HSRPDataEntryNew
{
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
            //  Refresh();
            //string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
            string CnnString = String.Empty;
            string SqlString = String.Empty;
                SqlString="select Prefix,PrefixID from PrefixLaserNo where hsrp_stateid ='"+ utils.getStateId() +"' order by prefix";
                DataTable dt = utils.GetDataTable(SqlString, utils.getCnnHSRPApp);
                CMBPrefix.DisplayMember  = "Prefix";
                CMBPrefix.ValueMember    = "PrefixID";
                CMBPrefix.DataSource=dt;
                SqlString = "select ProductCode,ProductID from Product where hsrp_stateid ='" + utils.getStateId() + "' order by ProductCode";
                DataTable dt1 = utils.GetDataTable(SqlString, utils.getCnnHSRPApp);
                CMBproduct.DisplayMember = "ProductCode";
                CMBproduct.ValueMember = "ProductID";
                CMBproduct.DataSource = dt1;
            }
    

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();      
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SqlString = "exec Transaction_InsertInventory '" + CMBproduct.SelectedValue.ToString() + "','" + CMBPrefix.Text  + "','" + txtboxqty.Text + "','" + utils.getStateId() + "','" + txtlaserfrom.Text + "'";
            utils.ExecNonQuery(SqlString, utils.getCnnHSRPApp); 
            MessageBox.Show("Done");
           
        }

        private void txtlaserfrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }


        private void txtboxqty_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtboxqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != Delete;
        }

        private void txtlaserfrom_TextChanged(object sender, EventArgs e)
        {

        }
    }
       
}
