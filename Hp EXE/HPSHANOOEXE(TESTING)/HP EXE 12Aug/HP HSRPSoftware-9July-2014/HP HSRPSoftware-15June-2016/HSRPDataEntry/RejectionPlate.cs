using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HSRPTransferData;

namespace HSRPDataEntryNew
{
    public partial class RejectionPlate : Form
    {
        public RejectionPlate()
        {
            InitializeComponent();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            string strVehicleRegNo  = string.Empty;
            string strLaserNo       = string.Empty;
            string strRemarks       = string.Empty;
            string UserID           = string.Empty;
            
            strLaserNo          = txtVehicleLaserNo.Text.Trim();
            strVehicleRegNo     = txtVehicleRegNo.Text.Trim();
            strRemarks          = txtRemark.Text.Trim();

            if (string.IsNullOrEmpty(strLaserNo))
            {
                MessageBox.Show("Please Insert Laser No.");
                txtVehicleLaserNo.Text = "";
                txtVehicleLaserNo.Focus();
                return;
            }

            if (string.IsNullOrEmpty(strVehicleRegNo.Trim()))
            {
                MessageBox.Show("Please Insert Vehicle Registration No.");
                txtVehicleRegNo.Text = "";
                txtVehicleRegNo.Focus();
                return;
            }
            if (strLaserNo.Length <= 10)
            {
                MessageBox.Show("Length Laser No. should be more than 10");
                txtVehicleLaserNo.Focus();
                return;
            }


            string qry = "select vehicleregno from OrderBookingOffLine where vehicleregno='" + txtVehicleRegNo.Text + "' ";
            string checkvalue = utils.getDataSingleValue(qry, utils.getCnnHSRPApp, "vehicleregno");
            if (checkvalue == "0")
            {
                MessageBox.Show("Vehicle Regno Not Booked Yet.");
                txtVehicleRegNo.Focus();
                return;
            }
              qry = "select laserno from RejectedPlates where laserno='" + txtVehicleLaserNo.Text + "' ";
              checkvalue = utils.getDataSingleValue(qry, utils.getCnnHSRPApp, "laserno");
            if (checkvalue == "0")
            {
                MessageBox.Show("Laser No Already Rejected.");
                txtVehicleLaserNo.Focus();
                return;
            }

           string strSQLText = "insert into RejectedPlates([vehicleRegNo] ,[LaserNo],[Remarks],[UserID]) values('" + strVehicleRegNo + "','" + strLaserNo + "','" + strRemarks + "','" + UserID + "')";
            int i = utils.ExecNonQuery(strSQLText, utils.getCnnHSRPApp);
            if (i > 0)
            {
                lblMessage.Text = "Save Records";
                Refresh();
            }
        }

        public void Refresh()
        {
            txtVehicleLaserNo.Text = string.Empty;
            txtVehicleRegNo.Text = string.Empty;
            txtRemark.Text = string.Empty;

        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }

    }
}
