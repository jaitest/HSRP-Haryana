using System;
using System.Data;
using System.Windows.Forms;
using HSRPTransferData;
using System.ComponentModel;

using System.IO;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;
using System.Data.SqlClient;

namespace HSRPDataEntry
{
    public partial class OrderClosed : Form
    {
        public OrderClosed()
        {
            InitializeComponent();
          //  Refresh();
        }
      

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();      
        }

        string Query   = string.Empty;
        string Sticker = string.Empty ;
        string Vip     = string.Empty;
        string recno = string.Empty;
        string strorderstatus = string.Empty;
        DataTable dt1  = new DataTable();
         HSRPDataEntry.HsrpService.HSRPServiceSoapClient objHSRP = new HSRPDataEntry.HsrpService.HSRPServiceSoapClient();
        private void btnSave_Click(object sender, EventArgs e)
        {
              bool i = isConnected();
              if (i == true)
              {

                  if (string.IsNullOrEmpty(txtVehRegNo.Text))
                  {
                      MessageBox.Show("Please Insert Vehicle Registration No.");

                      txtVehRegNo.Text = "";
                      txtVehRegNo.Focus();
                      return;
                  }

                  if (strorderstatus == "New Order")
                  {
                      if (string.IsNullOrEmpty(txtFlaser.Text.Trim()))
                      {
                          MessageBox.Show("Please Insert Front Laser No.");
                          txtFlaser.Text = "";
                          txtFlaser.Focus();
                          return;
                      }
                      if (string.IsNullOrEmpty(txtRlaser.Text.Trim()))
                      {
                          MessageBox.Show("Please Insert Rear Laser No.");
                          txtRlaser.Text = "";
                          txtRlaser.Focus();
                          return;
                      }
                      // string qry = "select laserno from RTOInventory where laserno='" + txtFlaser.Text + "' and inventorystatus not in ('New Order')";
                      string laservalue = objHSRP.InventoryStatusF_NotNewOrder(txtFlaser.Text);
                      if (laservalue != "0")
                      {
                          MessageBox.Show("Front Laser No Already Used.");
                          txtFlaser.Focus();
                          return;
                      }

                      // qry = "select laserno from RTOInventory where  laserno='" + txtRlaser.Text + "' and inventorystatus not in ('New Order')";
                      laservalue = objHSRP.InventoryStatusR_NotNewOrder(txtRlaser.Text);
                      if (laservalue != "0")
                      {
                          MessageBox.Show("Rear Laser No Already Used.");
                          txtFlaser.Focus();
                          return;
                      }


                      //qry = "select laserno from RTOInventory where laserno='" + txtFlaser.Text + "' and inventorystatus  ='New Order'";
                      laservalue = objHSRP.InventoryStatusF_NewOrder(txtFlaser.Text);
                      if (laservalue == "0")
                      {
                          MessageBox.Show("Front Laser No Not Found.");
                          txtFlaser.Focus();
                          return;
                      }

                      // qry = "select laserno from RTOInventory where  laserno='" + txtRlaser.Text + "' and inventorystatus = 'New Order'";
                      laservalue = objHSRP.InventoryStatusR_NewOrder(txtRlaser.Text);
                      if (laservalue == "0")
                      {
                          MessageBox.Show("Rear Laser No Not Found.");
                          txtFlaser.Focus();
                          return;
                      }

                      if (txtFlaser.Text.Length > 10 && txtRlaser.Text.Length > 10)
                      {
                          if (txtFlaser.Text.Trim() == txtRlaser.Text.Trim())
                          {
                              MessageBox.Show("Both Laser No. can not be same..");
                              txtRlaser.Focus();
                              return;
                          }
                          else
                          {
                              updateRecord();

                          }
                      }
                      else
                      {
                          MessageBox.Show("Length Laser No. should be more than 10");
                          txtFlaser.Focus();
                          return;
                      }
                  }
                  else
                  {
                      /// For Clossing
                      updateRecord();

                  }
              }
              else
              {

                  MessageBox.Show("Internet Is Not Connected");

              }
            
        }
 
      
        public void Refresh()
        {
            txtVehRegNo.Text = "";
            lblEngineno.Text = "";
            lblchassisno.Text = "";
            txtFlaser.Text = "";
            txtRlaser.Text = "";
            //checkBox1.
        }

      
               

        private void txtVehRegNo_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            VehicleRegNo();
        }

        public void VehicleRegNo()
        {


            string ConString = "Database=hsrpdemo;Server=115.112.157.60;UID=sa;PWD=*S7p@E6#; pooling=true; Max Pool Size=200;Connect Timeout=0";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand("select top 10 vehicleregno from HSRPRecords_AP where vehicleregno like '" + txtVehRegNo.Text + "%'", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                while (reader.Read())
                {
                    MyCollection.Add(reader.GetString(0));
                }

                txtVehRegNo.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtVehRegNo.AutoCompleteCustomSource = MyCollection;
                txtVehRegNo.AutoCompleteMode = AutoCompleteMode.Suggest;
                con.Close();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {
              bool i = isConnected();
              if (i == true)
              {
                  if (string.IsNullOrEmpty(txtVehRegNo.Text))
                  {
                      MessageBox.Show("Please Insert Vehicle Registration No.");

                      txtVehRegNo.Text = "";
                      txtVehRegNo.Focus();
                      return;
                  }

                  checkBox1.Checked = false;
                  checkBox1.Enabled = false;
                  search_veh();
              }
              else
              {

                  MessageBox.Show("Internet Is Not Connected");

              }
        }

        
        private void search_veh()
        {
           // Query = "select * from OrderBookingOffLine where vehicleregno='" + txtVehRegNo.Text + "' and orderstatus in ('Closed')";
            
             dt1 = objHSRP.RecordClosed(txtVehRegNo.Text);
            if (dt1.Rows.Count > 0)
            {
                MessageBox.Show("Record Already Closed...");
                txtVehRegNo.Focus();
                return;
            }

         //   Query = "select * from OrderBookingOffLine where vehicleregno='" + txtVehRegNo.Text + "' and orderstatus not in ('Closed')";
            dt1 = objHSRP.RecordNotClosed(txtVehRegNo.Text);
            if (dt1.Rows.Count > 0)
            {

                txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();
                lblchassisno.Text = dt1.Rows[0]["chassisno"].ToString();
                lblEngineno.Text = dt1.Rows[0]["Engineno"].ToString();
                strorderstatus = dt1.Rows[0]["OrderStatus"].ToString();
                if (dt1.Rows[0]["OrderStatus"].ToString() == "Embossing Done")
                {
                    txtFlaser.Text = dt1.Rows[0]["hsrp_front_lasercode"].ToString();
                    txtRlaser.Text = dt1.Rows[0]["hsrp_rear_lasercode"].ToString();
                    txtFlaser.Enabled = false;
                    txtRlaser.Enabled = false;
                }
                else
                {
                    txtFlaser.Enabled = true;
                    txtRlaser.Enabled = true;
                    txtFlaser.Text = "";
                    txtRlaser.Text = "";
                }

                if ((dt1.Rows[0]["Vehicletype"].ToString() == "LMV") || (dt1.Rows[0]["Vehicletype"].ToString() == "LMV(Class)") || (dt1.Rows[0]["Vehicletype"].ToString() == "Three Wheeler") || (dt1.Rows[0]["Vehicletype"].ToString() == "MCV/HCV/TRAILERS"))
                {
                    checkBox1.Checked = true;
                    checkBox1.Enabled = false;
                }
                else
                {
                    checkBox1.Checked = false;
                    checkBox1.Enabled = false;
                }

            }
            else
            {
                MessageBox.Show("Record Not Found");
                txtVehRegNo.Focus();
                return;
            }

        }
        public static bool isConnected()
        {
            try
            {
                string myAddress = "www.yahoo.com";
                IPAddress[] addresslist = Dns.GetHostAddresses(myAddress);

                if (addresslist[0].ToString().Length > 6)
                {
                    return true;
                }
                else
                    return false;

            }
            catch
            {
                return false;
            }

        }
        private void updateRecord()
        {
            string qry = string.Empty;
            if (strorderstatus == "New Order")
            {

                // qry = "select vehicleregno from OrderBookingOffLine where hsrp_front_lasercode='" + txtFlaser.Text + "' or hsrp_rear_lasercode='" + txtFlaser.Text + "'";
                string laservalue = objHSRP.FrontValidation(txtFlaser.Text);
                if (laservalue != "0")
                {
                    MessageBox.Show("Front Laser No Already Used.");
                    txtFlaser.Focus();
                    return;
                }

                // qry = "select vehicleregno from OrderBookingOffLine where hsrp_front_lasercode='" + txtRlaser.Text + "' or hsrp_rear_lasercode='" + txtRlaser.Text + "'";
                laservalue = objHSRP.RearValidation(txtFlaser.Text);
                if (laservalue != "0")
                {
                    MessageBox.Show("Rear Laser No Already Used.");
                    txtRlaser.Focus();
                    return;
                }

            }

            //Query = "select * from OrderBookingOffLine where vehicleregno = '" + txtVehRegNo.Text + "' and orderstatus not in ('Closed')";
            dt1 = objHSRP.OrderStatusNotClosed(txtVehRegNo.Text);
            if (dt1.Rows.Count > 0)
            {
                txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();
                if (dt1.Rows[0]["orderstatus"].ToString() == "Embossing Done")
                {
                    // qry = "update OrderBookingOffLine set  ordercloseddate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + txtFlaser.Text + "', hsrp_rear_lasercode ='" + txtRlaser.Text + "' where vehicleregno ='" + txtVehRegNo.Text + "' and orderstatus='Embossing Done'";
                    int j = objHSRP.OrderStatusEmbossingDone(txtFlaser.Text, txtRlaser.Text, txtVehRegNo.Text);
                    //  qry = "update HSRP_DTLS set HSRP_NO_FRONT='" + txtFlaser.Text + "',HSRP_NO_back= '" + txtRlaser.Text + "',HSRP_FIX_DT =getdate() where replace(REGN_NO,' ','') ='" + txtVehRegNo.Text + "'";
                    //j = objHSRP.OrderStatusEmbossingDoneUpdate(txtFlaser.Text, txtRlaser.Text, txtVehRegNo.Text);
                    MessageBox.Show("Record Save Successfully....");
                    txtVehRegNo.Focus();
                    Refresh();
                }
                else
                {
                    // qry = "update OrderBookingOffLine set  ordercloseddate = getdate(),orderembossingdate = getdate(),orderstatus='Closed', hsrp_front_lasercode='" + txtFlaser.Text + "', hsrp_rear_lasercode ='" + txtRlaser.Text + "' where vehicleregno ='" + txtVehRegNo.Text + "' and orderstatus='New Order'";
                    int j = objHSRP.OrderStatusNewOrder(txtFlaser.Text, txtRlaser.Text, txtVehRegNo.Text);
                    //qry = "update HSRP_DTLS set HSRP_NO_FRONT='" + txtFlaser.Text + "',HSRP_NO_back= '" + txtRlaser.Text + "',HSRP_ISSUE_DT =getdate(),HSRP_FIX_DT =getdate() where replace(REGN_NO,' ','') ='" + txtVehRegNo.Text + "'";
                    //j = objHSRP.OrderStatusNewOrderUpdate(txtFlaser.Text, txtRlaser.Text, txtVehRegNo.Text);
                    MessageBox.Show("Record Save Successfully....");
                    txtVehRegNo.Focus();
                    Refresh();

                }

                //    if (isConnected())
                //    {

                //      //  DateTime affdate = Convert.ToDateTime(dt1.Rows[0]["Record_CreationDate"].ToString()).AddDays(4);
                //        //string MobileNo = "919958299100,919810509118,919958894692"; //+ dt1.Rows[0]["MobileNo"].ToString();
                //        string MobileNo = "91" + dt1.Rows[0]["MobileNo"].ToString() ;
                //        string smsmessage = "Your HSRP " + txtVehRegNo.Text.Trim() + " has been affixed.  Transport Dept HP.";
                //        string url = "http://alerts.smseasy.in/api/web2sms.php?workingkey=627691h463b66gov5j83&sender=HPHSRP&to=" + MobileNo + "&message=" + smsmessage + "";
                //        HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                //        MyRequest.Method = "GET";
                //        WebResponse myRespose = MyRequest.GetResponse();
                //        StreamReader sr = new StreamReader(myRespose.GetResponseStream(), System.Text.Encoding.UTF8);
                //        string result = sr.ReadToEnd();
                //        sr.Close();
                //        myRespose.Close();
                //        //txtVehRegNo.Text = dt1.Rows[0]["vehicleregno"].ToString();
                //        int j1;
                //        qry = "update OrderBookingOffLine set  SecondSMSText ='" + smsmessage + "',SecondSMSDateTime=getdate(),SecondSMSServerReponseID='" + result.ToString() + "'  where vehicleregno = '" + txtVehRegNo.Text + "' and orderstatus='Closed'";
                //        j1 = utils.ExecNonQuery(qry, utils.getCnnHSRPApp);
                //        //qry = "insert into HSRP_DTLS_SMS([MobileNo],[REGN_NO],[AUTH_NO],[HSRP_FLAG],[SecondSMSText],[SecondSMSSentDateTime],[SecondSMSServerResponseID]) values ('" + dt1.Rows[0]["MobileNo"].ToString() + "','" + txtVehRegNo.Text + "','" + dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "','" + dt1.Rows[0]["ordertype"].ToString() + "','" + smsmessage + "',getdate(),'" + result.ToString() + "') ";
                //        qry = "update HSRP_DTLS_SMS set [SecondSMSText]='" + smsmessage + "',[SecondSMSSentDateTime]=getdate(),[SecondSMSServerResponseID]='" + result.ToString() + "' where [MobileNo] ='" + MobileNo + "' and [AUTH_NO]= '" + dt1.Rows[0]["HSRPRecord_AuthorizationNo"].ToString() + "'"; 
                //        j1 = utils.ExecNonQuery(qry, utils.getCnnHSRPVahan);

                //        MessageBox.Show("Record Saved and SMS Sent To Client On : " + MobileNo);
                //        //Update offline order booking
                //        //Update HSRP_HP DB for SMS

                //    }
                //    else
                //    {
                //        MessageBox.Show("Record Saved and SMS Not Sent as No Internet.");

                //    }


                //    return;
                //}
                //else
                //{
                //    MessageBox.Show("Record Not Found");
                //    txtVehRegNo.Focus();
                //    return;
                //}



            }
        }

        private void txtFlaser_TextChanged(object sender, EventArgs e)
        {
            Flaser();
        }

        public void Flaser()
        {

            string ConString = "Database=hsrpdemo;Server=115.112.157.60;UID=sa;PWD=*S7p@E6#; pooling=true; Max Pool Size=200;Connect Timeout=0";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand("select top 10 [LaserNo] from [RTOInventory] where LaserNo like '%" + txtFlaser.Text + "' and [InventoryStatus]='New Order'", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                while (reader.Read())
                {
                    MyCollection.Add(reader.GetString(0));
                }

                txtFlaser.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtFlaser.AutoCompleteCustomSource = MyCollection;
                txtFlaser.AutoCompleteMode = AutoCompleteMode.Suggest;
                con.Close();
            }

        }



     
        private void txtRlaser_TextChanged(object sender, EventArgs e)
        {
            Rlaser();
        }

        public void Rlaser()
        {

            string ConString = "Database=hsrpdemo;Server=115.112.157.60;UID=sa;PWD=*S7p@E6#; pooling=true; Max Pool Size=200;Connect Timeout=0";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                SqlCommand cmd = new SqlCommand("select top 10 [LaserNo] from [RTOInventory] where LaserNo like '%" + txtRlaser.Text + "' and [InventoryStatus]='New Order'", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                while (reader.Read())
                {
                    MyCollection.Add(reader.GetString(0));
                }

                txtRlaser.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtRlaser.AutoCompleteCustomSource = MyCollection;
                txtRlaser.AutoCompleteMode = AutoCompleteMode.Suggest;
                con.Close();
            }

        }  
        
    }
       
}
