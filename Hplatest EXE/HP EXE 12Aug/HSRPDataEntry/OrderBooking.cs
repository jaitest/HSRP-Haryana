using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;
using System.IO;
using System.Drawing.Printing;
using System.Drawing;

namespace HSRPDataEntryNew
{
    public partial class OrderBooking : Form
    {
        public OrderBooking()
        {
            InitializeComponent();
        }
        string ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionStringAPP"].ToString();
        DataTable dt1 = new DataTable();
        string Query = string.Empty, Sticker = string.Empty, Vip = string.Empty;

       
        private void OrderBooking_Load(object sender, EventArgs e)
        {
            //if(! this)
            VehicleMaker();
            
        }

        public void VehicleMaker()
        {
            string Query1 = "select VehicleMakerID,VehicleMakerDescription from dbo.VehicleMakerMaster";
            dt1 = utils.GetDataTable(Query1, ConnectionString);
            if (dt1.Rows.Count > 0)
            {

                ddlVehicleMaker.Items.Insert(0, "--Select Vehicle Maker--");
                ddlVehicleMaker.DisplayMember = "VehicleMakerDescription";
                ddlVehicleMaker.ValueMember = "VehicleMakerID";
                
                ddlVehicleMaker.DataSource = dt1;
                
                // ddlVehicleMaker.SelectedIndex = 0;
               
            }

        }

        DataTable dt2 = new DataTable();
        private void ddlVehicleMaker_TextChanged(object sender, EventArgs e)
        {

            
            string Query1 = "select VehicleMakerID,VehicleMakerDescription from dbo.VehicleMakerMaster where VehicleMakerDescription='" + ddlVehicleMaker.Text + "' ";
            dt2 = utils.GetDataTable(Query1, ConnectionString);


            string Query = "select * from dbo.VehicleModelMaster where VehicleMakerID='" +ddlVehicleMaker.SelectedValue.ToString()+ "'";
            dt1 = utils.GetDataTable(Query, ConnectionString);
            if (dt1.Rows.Count > 0)
            {
                ddlvehicleModel.DataSource = dt1;
                ddlvehicleModel.DisplayMember = "VehicleModelDescription";
                ddlvehicleModel.ValueMember = "VehicleModelID";
                //  this.ddlvehicleModel.Items.Insert(0, "--Select Vehicle Type--");
            }
            else
            {
                this.ddlvehicleModel.Items.Insert(0, "--Select Vehicle Type--");
            }
        }
        //public void VehicleModel()
        //{
        //    string Query = "select * from dbo.VehicleModelMaster";
        //    dt1 = utils.GetDataTable(Query, ConnectionString);

        //    ddlvehicleModel.DataSource = dt1;
        //    ddlvehicleModel.SelectedText = "VehicleModelDescription";
        //    ddlvehicleModel.SelectedValue = "VehicleModelID";
        //}
        string Prefix;
        private void button2_Click(object sender, EventArgs e)
        {
            string Query = "select * from dbo.OrderBookingOffLine where VehicleRegNo='" + txtVehRegNo.Text + "'";
            dt1 = utils.GetDataTable(Query, ConnectionString);

            if (dt1.Rows.Count > 0)
            {

                txtVehRegNo.Text = dt1.Rows[0]["VehicleRegNo"].ToString();
                txtAuthorizationNo.Text = dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                txtMobileNo.Text = dt1.Rows[0]["MobileNo"].ToString();
                txtOwnerName.Text = dt1.Rows[0]["OwnerName"].ToString();
                txtAmount.Text = dt1.Rows[0]["Amount"].ToString();
                txtTax.Text = dt1.Rows[0]["Tax"].ToString();
                Prefix = dt1.Rows[0]["CashReceiptNo"].ToString();
                txtCashReceiptNo.Text = dt1.Rows[0]["CashReceiptNo"].ToString();
                // ddlOrderType.SelectedItem = dt1.Rows[0]["OrderType"].ToString();

                ddlVehicleClass.SelectedItem = dt1.Rows[0]["VehicleClass"].ToString();
                ddlVehType.SelectedItem = dt1.Rows[0]["VehicleType"].ToString();
                if (dt1.Rows[0]["OrderType"].ToString() == "NB")
                {
                    ddlOrderType.SelectedItem = "NEW BOTH PLATES";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "OB")
                {
                    ddlOrderType.SelectedItem = "OLD BOTH PLATES";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "DB")
                {
                    ddlOrderType.SelectedItem = "DAMAGED BOTH PLATES";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "DF")
                {
                    ddlOrderType.SelectedItem = "DAMAGED FRONT PLATE";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "DR")
                {
                    ddlOrderType.SelectedItem = "DAMAGED REAR PLATE";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "OS")
                {
                    ddlOrderType.SelectedItem = "ONLY STICKER";

                }
                else
                {
                    MessageBox.Show("Please Select Correct Order Type.");
                    ddlOrderType.Focus();
                    return;
                }

                AuthDate.Value = DateTime.Parse(dt1.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                if (dt1.Rows[0]["VIP"].ToString() == "Y")
                {
                    chkVIP.Checked = true;
                }
                else
                {
                    chkVIP.Checked = false;
                }

                if (dt1.Rows[0]["StickerMandatory"].ToString() == "Y")
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
            }
            else
            {
                MessageBox.Show("Record Not Found");
            }
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            string Query = "select * from dbo.OrderBookingOffLine where CashReceiptNo='" + txtCashReceiptNo.Text + "'";
            dt1 = utils.GetDataTable(Query, ConnectionString);

            if (dt1.Rows.Count > 0)
            {

                txtVehRegNo.Text = dt1.Rows[0]["VehicleRegNo"].ToString();
                txtAuthorizationNo.Text = dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString();
                txtMobileNo.Text = dt1.Rows[0]["MobileNo"].ToString();
                txtOwnerName.Text = dt1.Rows[0]["OwnerName"].ToString();
                txtAmount.Text = dt1.Rows[0]["Amount"].ToString();
                txtTax.Text = dt1.Rows[0]["Tax"].ToString();
                Prefix = dt1.Rows[0]["CashReceiptNo"].ToString();
               // txtCashReceiptNo.Text = dt1.Rows[0]["CashReceiptNo"].ToString();
               
                // ddlOrderType.SelectedItem = dt1.Rows[0]["OrderType"].ToString();

                ddlVehicleClass.SelectedItem = dt1.Rows[0]["VehicleClass"].ToString();
                ddlVehType.SelectedItem = dt1.Rows[0]["VehicleType"].ToString();
                if (dt1.Rows[0]["OrderType"].ToString() == "NB")
                {
                    ddlOrderType.SelectedItem = "NEW BOTH PLATES";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "OB")
                {
                    ddlOrderType.SelectedItem = "OLD BOTH PLATES";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "DB")
                {
                    ddlOrderType.SelectedItem = "DAMAGED BOTH PLATES";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "DF")
                {
                    ddlOrderType.SelectedItem = "DAMAGED FRONT PLATE";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "DR")
                {
                    ddlOrderType.SelectedItem = "DAMAGED REAR PLATE";

                }
                else if (dt1.Rows[0]["OrderType"].ToString() == "OS")
                {
                    ddlOrderType.SelectedItem = "ONLY STICKER";

                }
                else
                {
                    MessageBox.Show("Please Select Correct Order Type.");
                    ddlOrderType.Focus();
                    return;
                }

                AuthDate.Value = DateTime.Parse(dt1.Rows[0]["HSRPRecord_AuthorizationDate"].ToString());
                if (dt1.Rows[0]["VIP"].ToString() == "Y")
                {
                    chkVIP.Checked = true;
                }
                else
                {
                    chkVIP.Checked = false;
                }

                if (dt1.Rows[0]["StickerMandatory "].ToString() == "Y")
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
            }
            else
            {
                MessageBox.Show("Record Not Found");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //select @cshno=(prefixtext+right('00000'+ convert(varchar,lastno+1),5)) from prefix where hsrp_stateid=@hsrp_stateid and rtolocationid =@rtolocationid and prefixfor='Cash Receipt No'
            //update prefix set lastno=lastno+1 where hsrp_stateid=@hsrp_stateid and rtolocationid =@rtolocationid and prefixfor='Cash Receipt No'
            //string Query = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as PrifixNo from prefix where hsrp_stateid='" + stateID + "' and rtolocationid ='" + LocationID + "' and prefixfor='Cash Receipt No'";
            //dt1 = utils.GetDataTable(Query, ConnectionString);

            //lblCashReceiptNo.Text = dt1.Rows[0]["PrifixNo"].ToString();
            if (string.IsNullOrEmpty(txtVehRegNo.Text))
            {
                MessageBox.Show("Please Insert Vehicle Registration No.");

                txtVehRegNo.Text = "";
                txtVehRegNo.Focus();
                return;
            }

            //if (txtVehRegNo.Text.Length != 10)
            //{
            //    MessageBox.Show("Vehicle Registration Length should be 10 character.");
            //    txtVehRegNo.Text = "";
            //    txtVehRegNo.Focus();
            //    return;
            //}

            if (string.IsNullOrEmpty(txtAuthorizationNo.Text))
            {
                MessageBox.Show("Please Insert Authorization No.");

                txtAuthorizationNo.Text = "";
                txtAuthorizationNo.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtOwnerName.Text))
            {
                MessageBox.Show("Please Insert Owner Name");

                txtOwnerName.Text = "";
                txtOwnerName.Focus();
                return;
            }


            if (string.IsNullOrEmpty(txtMobileNo.Text))
            {
                MessageBox.Show("Please Insert Mobile No.");

                txtMobileNo.Text = "";
                txtMobileNo.Focus();
                return;
            }

            if (ddlVehicleClass.Text == "--Select Vehicle Class--" || String.IsNullOrEmpty(ddlVehicleClass.SelectedItem.ToString()))
            {
                MessageBox.Show("Please Select Vehicle Class.");
                ddlVehicleClass.Focus();
                return;
            }
            if (ddlVehType.Text == "--Select Vehicle Type--" || String.IsNullOrEmpty(ddlVehType.SelectedItem.ToString()))
            {
                MessageBox.Show("Please Select Vehicle Type.");
                ddlVehType.Focus();
                return;
            }
            if (ddlOrderType.Text == "--Select Order Type--" || String.IsNullOrEmpty(ddlOrderType.SelectedItem.ToString()))
            {
                MessageBox.Show("Please Select Order Type.");
                ddlOrderType.Focus();
                return;
            }
            if (txtAmount.Text == "")
            {
                MessageBox.Show("Please Insert Amount.");
                txtAmount.Text = "";
                txtAmount.Focus();
                return;
            }

            if (txtChassisNo.Text == "")
            {
                MessageBox.Show("Please Insert Chassis No.");
                txtChassisNo.Text = "";
                txtChassisNo.Focus();
                return;
            }
            if (txtEngineNo.Text== "")
            {
                MessageBox.Show("Please Insert Engine No.");
                txtEngineNo.Text = "";
                txtEngineNo.Focus();
                return;
            }
            if (txtEmailID.Text == "")
            {
                MessageBox.Show("Please Insert EmailID.");
                txtEmailID.Text = "";
                txtEmailID.Focus();
                return;
            }
            if (txtAddress.Text == "")
            {
                MessageBox.Show("Please Insert Address.");
                txtAddress.Text = "";
                txtAddress.Focus();
                return;
            }

            if (ddlVehicleMaker.Text == "--Select Vehicle Maker--" || String.IsNullOrEmpty(ddlVehicleMaker.SelectedItem.ToString()))
            {
                MessageBox.Show("Please Select Vehicle Maker.");
                ddlVehicleMaker.Focus();
                return;
            }

            if (ddlvehicleModel.Text == "--Select Vehicle Type--" || String.IsNullOrEmpty(ddlvehicleModel.SelectedItem.ToString()))
            {
                MessageBox.Show("Please Select Vehicle Type.");
                ddlvehicleModel.Focus();
                return;
            }
            if (radioButton1.Checked == true)
            {
                Sticker = "Y";
            }
            else
            {
                Sticker = "N";
            }

            if (chkVIP.Checked == true)
            {
                Vip = "Y";
            }
            else
            {
                Vip = "N";
            }

            String SqlCheck = "select COUNT(*) from dbo.OrderBooking where VehicleRegNo ='" + txtVehRegNo.Text.Trim() + "'";

            float z = utils.getScalarCount(SqlCheck, ConnectionString);
            if (z <= 0)
            {

                Query = "INSERT INTO OrderBooking (Record_CreationDate,HSRP_StateID,RTOLocationID,HSRPRecord_AuthorizationNo,HSRPRecord_AuthorizationDate,VehicleRegNo,OwnerName,MobileNo,VehicleClass,OrderType,StickerMandatory,VIP,Amount,VehicleType,CashReceiptNo,Tax,Address,EmailID,VehicleMakerID,vehicleModelID,EngineNo,ChassisNo,Remark)" + "values (GetDate(), '" + stateID + "','" + LocationID + "','" + txtAuthorizationNo.Text + "','" + AuthDate.Value.ToString() + "','" + txtVehRegNo.Text + "','" + txtOwnerName.Text + "','" + txtMobileNo.Text + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + orderType + "','" + Sticker + "','" + Vip + "','" + txtAmount.Text + "','" + ddlVehType.SelectedItem.ToString() + "','" +txtCashReceiptNo.Text + "','" + txtTax.Text + "','" + txtAddress.Text + "','" + txtEmailID.Text + "','" + ddlVehicleMaker.SelectedValue.ToString() + "','" + ddlvehicleModel.SelectedValue.ToString() + "','" + txtEngineNo.Text + "','" + txtChassisNo.Text + "','" + txtRemark.Text + "')";

                int i = utils.ExecNonQuery(Query, ConnectionString);
                if (i > 0)
                {
                    lblMessage.Text = "Save Records";
                   
                    Refresh();
                }
                else
                {
                    lblMessage.Text = "Dublicate Reg No";
                }
            }

        }

        static string stateID, LocationID, state, RTOLocationAddress, CompanyName, CurrentDate, userid;

        internal void sendStateLocation(string p1, string p2, string p3)
        {
            stateID = p1;
            LocationID = p2;
            userid = p3;

            //String SqlCheck = "select RTOLocation.*,Users.* from dbo.RTOLocation inner join Users  on RTOLocation.RTOLocationID=Users.RTOLocationID where RTOLocation.HSRP_StateID='"+p1+"' and RTOLocation.RTOLocationID='"+p2+"' and Users.UserID='"+p3+"'";
            String SqlCheck = "SELECT   CONVERT(varchar, GETDATE(),110) as CurrentDate,  c.CompanyName, b.RTOLocationAddress FROM  Users a INNER JOIN RTOLocation b ON a.RTOLocationID = b.RTOLocationID INNER JOIN HSRPState c ON a.HSRP_StateID = c.HSRP_StateID  where a.UserID='" + p3 + "'";
            DataTable dt = utils.GetDataTable(SqlCheck, ConnectionString);
            if (dt.Rows.Count > 0)
            {
                RTOLocationAddress = dt.Rows[0]["RTOLocationAddress"].ToString();
                CompanyName = dt.Rows[0]["CompanyName"].ToString();
                CurrentDate = dt.Rows[0]["CurrentDate"].ToString();
               // VehicleMaker();
            }

            //string Query = "select (prefixtext+right('00000'+ convert(varchar,lastno+1),5)) as PrifixNo from prefix where hsrp_stateid='" + stateID + "' and rtolocationid ='" + LocationID + "' and prefixfor='Cash Receipt No'";
            //dt1 = utils.GetDataTable(Query, ConnectionString);

            //lblCashReceiptNo.Text = dt1.Rows[0]["PrifixNo"].ToString();
        }
        string orderType = string.Empty;
        private void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOrderType.SelectedText != "--Select Order Type--")
            {
                if (ddlOrderType.SelectedItem.ToString() == "NEW BOTH PLATES")
                {
                    orderType = "NB";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "OLD BOTH PLATES")
                {
                    orderType = "OB";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "DAMAGED BOTH PLATES")
                {
                    orderType = "DB";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "DAMAGED FRONT PLATE")
                {
                    orderType = "DF";
                }
                else if (ddlOrderType.SelectedItem.ToString() == "DAMAGED REAR PLATE")
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

                //String Amount = "DECLARE @ret int ; " + Environment.NewLine + " " + "EXEC @ret = dbo.hsrpplateamt 4,'LMV','Non-Transport','NB' " + "print @ret";
                //string amo = "DECLARE @ret int ; " + "EXEC @ret = dbo.hsrpplateamt 4,'LMV','Non-Transport','NB' " + "print @ret";
                //int amount = utils.getScalarCount(amo, ConnectionString);
                // DataTable dt = utils.GetDataTable(Amount, ConnectionString);
                //if (dt.Rows.Count > 0)
                //{

                //}
                DataTable dt = new DataTable();
                // amount = utils.getScalarCount("select dbo.hsrpplateamt ('" + stateID + "','" + ddlVehType.SelectedItem.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + orderType + "') as Amount", ConnectionString);
                dt = utils.GetDataTable("select dbo.hsrpplateamt ('" + stateID + "','" + ddlVehType.SelectedItem.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + orderType + "') as Amount", ConnectionString);
                // txtAmount.Text = amount.ToString();
                txtAmount.Text = dt.Rows[0]["Amount"].ToString();
                txtAmount.Enabled = false;

                dt = utils.GetDataTable("select dbo.hsrpplatetax ('" + stateID + "','" + ddlVehType.SelectedItem.ToString() + "','" + ddlVehicleClass.SelectedItem.ToString() + "','" + orderType + "') as tax", ConnectionString);
                // txtTax.Text = tax.ToString();
                txtTax.Text = dt.Rows[0]["tax"].ToString();
                txtTax.Enabled = false;

            }
        }


        public void Refresh()
        {
            txtVehRegNo.Text = "";
            txtOwnerName.Text = "";
            txtMobileNo.Text = "";
            txtAuthorizationNo.Text = "";
            txtAmount.Text = "";
            ddlVehicleClass.Text = "--Select Vehicle Class--";
            ddlVehType.Text = "--Select Vehicle Type--";
            ddlOrderType.Text = "--Select Order Type--";
            chkVIP.Checked = false;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            txtTax.Text = "";
            txtAddress.Text = "";
            txtEmailID.Text = "";
            txtRemark.Text = "";
            ddlVehicleMaker.Text = "--Select Vehicle Maker";
            ddlvehicleModel.Text = "--Select Vehicle Type";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        //public bool IsPostBack { get; set; }
    }
}
