using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace HSRPDataEntryNew
{
    public partial class OfflineDataEntry : Form
    {
        public OfflineDataEntry()
        {
            InitializeComponent();
        }
       
        string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        string CnnString = String.Empty;
        string SqlString = String.Empty;
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {

            string strAuthNo = txtAuthNo.Text.Trim();
            string AuthDate = dtmAuthDate.Value.ToString();
            string CashReceiptDate = dtmCashReceiptDate.Value.ToString();
            string strCustName = txtCustName.Text.Trim();
            string strMobileNo = txtMobNo.Text.Trim();
            string strAddress = txtAdd.Text.Trim();
            string strVehicleRegNo = txtVehRegNo.Text.Trim();
            string strRTOLocation = cmbRTOLoc.SelectedItem.ToString();
            string strVehicleClass = cmbVehClass.SelectedItem.ToString();
            string strVehicleType = cmbVehType.SelectedItem.ToString();
            //string strVehicleMaker = cmbVehMaker.SelectedItem.ToString();
            //string strVehicleModel = cmbVehModel.SelectedItem.ToString();
            string strOrderStatus = cmbOrderStatus.SelectedItem.ToString();
            string strEngine = txtEng.Text.Trim();
            string strChassis = txtChass.Text.Trim();
            string strCashReceipt = txtCashRecipt.Text.Trim();
            string strAmount = txtAmount.Text.Trim();
            string strFrontLaserCode = txtFLaser.Text.Trim();
            string strRearLaserCode = txtRLaser.Text.Trim();
            string strFrontPlateSize = txtFSize.Text.Trim();
            string strRearPlateSize = txtRSize.Text.Trim();
            string StrISThirdStickerMandatory = "N";

            if (radioButton1.Checked == true)
            {
                StrISThirdStickerMandatory = "Y";
            }
            if (radioButton2.Checked == true)
            {
                StrISThirdStickerMandatory = "N";
            }

            if (strAuthNo == "")
            {
                MessageBox.Show("Authurization Number Cannot Be Left Blank.");
                txtAuthNo.Text = "";
                txtAuthNo.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(strMobileNo))
            {

                if (strMobileNo.Length != 10)
                {
                    MessageBox.Show("Mobile Cannot Be less then or more than 10 Digits.");
                    txtMobNo.Text = "";
                    txtMobNo.Focus();
                    return;
                }
            }


            if (strCustName == "")
            {
                MessageBox.Show("Customer Name Cannot Be Left Blank.");
                txtCustName.Text = "";
                txtCustName.Focus();
                return;
            }

            if (strVehicleRegNo == "")
            {
                MessageBox.Show("Vehicle Registration Number Be Left Blank.");
                txtVehicleRegNo.Text = "";
                txtVehicleRegNo.Focus();
                return;
            }
            if (strVehicleRegNo.Length != 10)
            {
                MessageBox.Show("Vehicle Registration Length should be 10 character.");
                txtVehicleRegNo.Text = "";
                txtVehicleRegNo.Focus();
                return;
            }


            if (strRTOLocation == "--Select RTO Location--")
            {
                MessageBox.Show("Please Select RTO Location.");
                cmbVehicleClass.Focus();
                return;
            }

            if (strVehicleClass == "--Select Vehicle Class--" || String.IsNullOrEmpty(strVehicleClass))
            {
                MessageBox.Show("Please Select Correct Vehicle Class.");
                cmbVehicleClass.Focus();
                return;
            }

            if (strVehicleType == "--Select Vehicle Type--" || String.IsNullOrEmpty(strVehicleType))
            {
                MessageBox.Show("Please Select Correct Vehicle Class.");
                cmbVehicleClass.Focus();
                return;
            }

            if (ddlOrderType.SelectedItem.ToString() == "--Select Order Type--" || String.IsNullOrEmpty(strVehicleType))
            {
                MessageBox.Show("Please Select Correct Order Type.");
                ddlOrderType.Focus();
                return;
            }
            //if (strVehicleMaker == "--Select Vehicle Maker--" || String.IsNullOrEmpty(strVehicleMaker))
            //{
            //    MessageBox.Show("Please Select Correct Vehicle Maker.");
            //    cmbVehicleClass.Focus();
            //    return;
            //}
            //if (strVehicleModel == "--Select Vehicle Type--" || String.IsNullOrEmpty(strVehicleModel))
            //{
            //    MessageBox.Show("Please Select Correct Vehicle Model.");
            //    cmbVehicleClass.Focus();
            //    return;
            //}

            if (strEngine.Length > 16 || String.IsNullOrEmpty(strEngine))
            {
                MessageBox.Show("Engine No is not correct.");
                txtEngineNo.Text = "";
                txtEngineNo.Focus();
                return;
            }
            if (strChassis.Length > 16 || String.IsNullOrEmpty(strChassis))
            {
                MessageBox.Show("Chassis No is not correct.");
                txtEngineNo.Text = "";
                txtEngineNo.Focus();
                return;
            }
            if (String.IsNullOrEmpty(strAmount))
            {
                MessageBox.Show("Chassis No is not correct.");
                txtEngineNo.Text = "";
                txtEngineNo.Focus();
                return;
            }

            if (strOrderStatus == "--Select Order Status--" || String.IsNullOrEmpty(strOrderStatus))
            {
                MessageBox.Show("Please Select Correct Order Status.");
                cmbVehicleClass.Focus();
                return;
            }

            string sql = string.Empty;
            sql = "INSERT INTO [HSRPRecords]([HSRP_StateID],[RTOLocationName],[HSRPRecord_AuthorizationNo],[HSRPRecord_AuthorizationDate]" +
           ",[OwnerName],[Address1],[MobileNo],[OrderType],[OrderStatus],[VehicleClass]" +
           ",[VehicleType],[ManufacturerName],[ManufacturerModel],[ChassisNo],[EngineNo]" +
           ",[VehicleRegNo],[HSRP_Front_LaserCode],[HSRP_Rear_LaserCode],[FrontPlateSize],[RearPlateSize]" +
           ",[StickerMandatory],CashReceipt,[CashReceiptDateTime],[TotalAmount],VehicleColor,Remarks)" +
           "Values(2,'" + strRTOLocation + "','" + strAuthNo + "','" + AuthDate + "'," +
           "'" + strCustName + "','" + strAddress + "','" + strMobileNo + "','" + orderType + "','" + strOrderStatus + "','" + strVehicleClass + "','" + strVehicleType + "'," +
           "'','','" + strChassis + "','" + strEngine + "','" + strVehicleRegNo + "','" + strFrontLaserCode + "','" + strRearLaserCode + "'," +
           "'" + strFrontPlateSize + "','" + strRearPlateSize + "','" + StrISThirdStickerMandatory + "','" + strCashReceipt + "','" + CashReceiptDate + "'," + strAmount + ",'" + txtVehicleColor.Text + "','" + txtRemarks.Text + "')";


           ExecuteNonQuery(sql, ConnectionString);

            DialogResult myDialogResult;
            myDialogResult = MessageBox.Show("Record Saved.", "Offline Entry", MessageBoxButtons.YesNo);

            if (myDialogResult == DialogResult.Yes)
            {

                OfflineDataEntry_Load(sender, e);
                ClearData();
            }

            if (myDialogResult == DialogResult.No)
            {

                ClearData();
                this.Close();
            }
        }
        public void ClearData()
        {
            txtAuthNo.Text = "";
            txtAuthNo.BackColor = Color.White;
            txtCustName.Text = "";
            txtCustName.BackColor = Color.White;
            txtEng.Text = "";
            txtEng.BackColor = Color.White;
            txtChass.Text = "";
            txtChass.BackColor = Color.White;
            txtCashRecipt.Text = "";
            txtCashRecipt.BackColor = Color.White;
            txtAmount.Text = "";
            txtAmount.BackColor = Color.White;
            txtRemarks.Text = "";
            txtRemarks.BackColor = Color.White;
            txtVehRegNo.Text = "";
            txtVehRegNo.BackColor = Color.White;
            txtCustName.Text = "";
            txtCustName.BackColor = Color.White;
            txtMobNo.Text = "";
            txtMobNo.BackColor = Color.White;
            txtAdd.Text = "";
            txtAdd.BackColor = Color.White;

            cmbVehClass.SelectedIndex = 0;
            cmbVehType.SelectedIndex = 0;
            cmbRTOLoc.SelectedIndex = 0;
            cmbVehMaker.SelectedIndex = 0;
            cmbVehModel.SelectedIndex = 0;
            cmbOrderStatus.SelectedIndex = 0;
            ddlOrderType.SelectedIndex = 0;
            txtFLaser.Text = "";
            txtFLaser.BackColor = Color.White;
            txtRLaser.Text = "";
            txtRLaser.BackColor = Color.White;
            txtFSize.Text = "";
            txtFSize.BackColor = Color.White;
            txtRSize.Text = "";
            txtRSize.BackColor = Color.White;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            
            txtVehicleColor.Text = "";
            txtVehicleColor.BackColor = Color.White;
        }
        private void ExecuteNonQuery(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                int mm= command.ExecuteNonQuery();
               
                command.Connection.Close();
            }

        }
        private void OfflineDataEntry_Load(object sender, EventArgs e)
        {
            //  FillVehicleMaker();
        }

        //public void FillVehicleMaker()
        //{
        //    CnnString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        //    SqlString = "Select VehicleMakerID,VehicleMakerDescription From VehicleMakerMaster order by VehicleMakerDescription";
        //    SqlConnection con = new SqlConnection(CnnString);
        //    SqlCommand cmd = new SqlCommand(SqlString, con);
        //    SqlDataAdapter da= new SqlDataAdapter ();
        //    try 
        //  {
        //     con.Open();
        //    da.SelectCommand=cmd;
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    cmbVehicleMaker.DataSource = dt;
        //    cmbVehicleMaker.ValueMember="VehicleMakerID";
        //    cmbVehicleMaker.DisplayMember="VehicleMakerDescription";
        //    cmbVehicleMaker.Items.Insert(0, "--Select Vehicle Maker--");
        //   // cmbVehicleMaker.SelectedIndex = 0;
        //    da.Dispose();
        //    cmd.Dispose();
        //    con.Close();

        //  }
        //catch (Exception)
        //{
        //    MessageBox.Show("Can not open connection ! ");
        //}


        //}

        //public void FillVehicleModel()
        //{
        //    CnnString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
        //    SqlString = "Select VehicleModelID,VehicleModelDescription From VehicleModelMaster where VehicleMakerID=" + cmbVehicleMaker.SelectedValue + " order by VehicleModelDescription";
        //    SqlConnection con = new SqlConnection(CnnString);
        //    SqlCommand cmd = new SqlCommand(SqlString, con);
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    try
        //    {
        //        con.Open();
        //        da.SelectCommand = cmd;
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        cmbVehicleMaker.DataSource = dt;
        //        cmbVehicleMaker.ValueMember = "VehicleMakerID";
        //        cmbVehicleMaker.DisplayMember = "VehicleMakerDescription";
        //        cmbVehicleMaker.Items.Insert(0, "--Select Vehicle Maker--");
        //        //cmbVehicleMaker.SelectedIndex = 0;
        //        da.Dispose();
        //        cmd.Dispose();
        //        con.Close();
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Can not open connection ! ");
        //    }
        //}

        private void buttonGo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty( txtVehRegNo.Text))
            {
                return;
            }
            string sqlString = "Select FrontLaserCode,RearLaserCode from Data Where VehicleRegNo='" + txtVehRegNo.Text + "'";
            SqlConnection cnn = new SqlConnection(ConnectionString);

            SqlCommand cmd = new SqlCommand(sqlString);
            DataTable dt = new DataTable();
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlString,cnn);
            da.Fill(dt);
            txtFLaser.Text = dt.Rows[0][0].ToString();
            txtRLaser.Text = dt.Rows[0][1].ToString();
            cnn.Close();
        }


        private void validateTextDouble(object sender, EventArgs e)
        {
            Exception X = new Exception();

            TextBox T = (TextBox)sender;

            try
            {
                if (T.Text != "-")
                {
                    double x = double.Parse(T.Text);

                    if (T.Text.Contains(','))
                        throw X;
                }
            }
            catch (Exception)
            {
                try
                {
                    int CursorIndex = T.SelectionStart - 1;
                    T.Text = T.Text.Remove(CursorIndex, 1);

                    //Align Cursor to same index
                    T.SelectionStart = CursorIndex;
                    T.SelectionLength = 0;
                }
                catch (Exception) { }
            }
        } 


        public void RetrieveMultipleResults(SqlConnection connection)
        {
            using (connection)
            {
                SqlCommand command = new SqlCommand(
                "SELECT CategoryID, CategoryName FROM dbo.Categories;" +
                "SELECT EmployeeID, LastName FROM dbo.Employees",
                connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        label60.Text = reader.GetValue(1).ToString();
                    }
                    reader.NextResult();
                }
            }
        }
        string orderType = string.Empty;
        private void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
           if( ddlOrderType.SelectedItem.ToString()=="NEW BOTH PLATES")
           {
               orderType = "NB";
           }
            else if(ddlOrderType.SelectedItem.ToString()=="OLD BOTH PLATES")
           {
               orderType = "OB";
           }
              else if(ddlOrderType.SelectedItem.ToString()=="DAMAGED BOTH PLATES")
           {
               orderType = "DB";
           }
              else if(ddlOrderType.SelectedItem.ToString()=="DAMAGED FRONT PLATE")
           {
               orderType = "DF";
           }
              else if(ddlOrderType.SelectedItem.ToString()=="DAMAGED REAR PLATE")
           {
               orderType = "DR";
           }
           else if (ddlOrderType.SelectedItem.ToString() == "ONLY STICKER")
           {
               orderType = "OS";
           }
           else
           {
               MessageBox.Show("Please Select Correct Order Type.");
               ddlOrderType.Focus();
               return;
           }
            
        }

        //private void txtAmount_TextChanged(object sender, EventArgs e)
        //{
        //    if (System.Text.RegularExpressions.Regex.IsMatch("[^0-9]", txtAmount.Text))
        //    {
        //        MessageBox.Show("Please enter only numbers.");
        //        txtAmount.Text.Remove(txtAmount.Text.Length - 1);
        //    }
        //}



        //private void cmbVehicleMaker_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmbVehicleMaker.SelectedIndex==0)
        //    {
        //        MessageBox.Show("Please Select Correct Vehicle Maker");
        //        return;
        //    }
        //    FillVehicleModel();
        //}
    }
}
